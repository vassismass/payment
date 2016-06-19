
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Microsoft.SupplyChain.Care.PaymentModels;
using Microsoft.SupplyChain.Care.Payment.ServiceLibrary.Exceptions;
using Microsoft.SupplyChain.Care.Payment.ServiceLibrary.CP;
using Microsoft.SupplyChain.Care.Payment.ServiceLibrary;

namespace SettlePayment
{
    
    public class CPServiceAgent : IServiceAgent
    {
        internal const int DecimalRoundDigit = 4;
        private const string CPPaymentIDNullException = "Payment Id is Null in CP Purchase Response";
        private const string ShippingAddressNotFound = "Shipping address is not found in user profile.";
        readonly static List<int> LstAddressErrorCodes = new List<int> { 60001, 60011, 60012, 60013, 60014, 60015, 60016, 60017, 60018, 60019, 60029, 60030, 60041, 60042, 10021 };
        private bool isZipCodeIncludedInCpComparision;
        private bool enableFraudDetection;
        private string alreadySettledErrMsg;
        private Guid productGuid;
        private Guid partnerGuid;
        private string dataCenter;
        private string shipFromPath;
        private CPAgentInfo Info;
        private ICommerceService cpclient = null;
        public CPServiceAgent()
        {

        }
        private ICommerceService GetCpClient()
        {
            return new Microsoft.SupplyChain.Care.Payment.ServiceLibrary.CP.CommerceServiceClient();
        }
        public CPServiceAgent(CPAgentInfo Inf)
        {
            this.productGuid = Inf.productGuid;
            this.isZipCodeIncludedInCpComparision = Inf.isZipCodeIncludedInCpComparision;
            this.dataCenter = Inf.dataCenter;
            this.shipFromPath = Inf.shipFromPath;
            this.partnerGuid = Inf.partnerGuid;
        }

        /// <summary>
        /// 
        /// To get PUID or Anonymous ID for GuestCheck Out 
        /// </summary>
        /// <param name="paymentIdentity"></param>
        /// <returns></returns>
        internal static Identity SelectIdentity(PaymentIdentity paymentIdentity)
        {
            var computeRequester = new Identity();

            if (!string.IsNullOrEmpty(paymentIdentity.GuestId))
                computeRequester = new Identity { IdentityValue = paymentIdentity.GuestId, IdentityType = CpConstants.AnonymousId };
            else
                computeRequester = new Identity { IdentityValue = paymentIdentity.Puid, IdentityType = CpConstants.Puid };

            return computeRequester;
        }

       
        internal AuthorizeDigitalPurchaseResponse AuthorizeDigitalPurchase(string addressid, PaymentIdentity paymentIdentity, LineItem skuInfo, Guid PurchasetrackingGuid, Dictionary<string, string> fraudDetectionProperty = null)
        {
            AuthorizeDigitalPurchaseResponse authresponse = new AuthorizeDigitalPurchaseResponse();
            var trackingGuid = PurchasetrackingGuid;
            PaymentException payex = null;
            string identityValue = string.Empty;

            try
            {
                cpclient = GetCpClient();

                var purchaseRequest = skuInfo.ConvertToPurchaseContentRequest(this.dataCenter, this.partnerGuid);

                purchaseRequest.BillingInfo = new BillingInfo
                {
                    BillingMode = BillingMode.ImmediateSettle,
                    PaymentMethod =
                        new RegisteredPaymentMethod { PaymentMethodId = paymentIdentity.PaymentInstrumentId }
                };
                purchaseRequest.AccountId = paymentIdentity.AccountId;

                var getIdentity = SelectIdentity(paymentIdentity);
                identityValue = getIdentity.IdentityValue;

                purchaseRequest.Requester = new Identity { IdentityValue = getIdentity.IdentityValue, IdentityType = getIdentity.IdentityType };
                purchaseRequest.PurchaseContext = new PurchaseContext
                {
                    Timestamp = DateTime.UtcNow,
                    ComputeOnly = false,
                    AddressId = addressid
                };

                purchaseRequest.PurchaseContext.ProductGuid = this.productGuid;
                purchaseRequest.TrackingGuid = trackingGuid;
                PopulateFraudDetectionProperties(purchaseRequest, fraudDetectionProperty);

                var purchaseResponse = cpclient.PurchaseContent(purchaseRequest);

                if (purchaseResponse.Ack == AckCode.Success && purchaseResponse != null)
                {
                    //In random cases, Purchase Id is coming null even when the response ACK is Success. 
                    //Throwing an exception in such case
                    if (purchaseResponse.PurchaseId == null)
                    {
                        payex = new PaymentException(CPPaymentIDNullException)
                        {
                            ExceptionCode = PaymentExceptioncode.PaymentAuthorizeDigitalOrderNonRetryableFailure
                        };
                        throw payex;
                    }
                    authresponse.Status = GetCPPaymentStatusCode(purchaseResponse.PurchaseStatus);
                    authresponse.TrackingGuid = trackingGuid;
                    authresponse.PurchaseId = purchaseResponse.PurchaseId;
                    return authresponse;
                }
                else if (purchaseResponse.Ack == AckCode.Success && purchaseResponse == null)
                {
                    payex = new PaymentException("Could not Authorize the order.");
                    payex.ExceptionCode = PaymentExceptioncode.PaymentAuthorizeDigitalOrderResponseNull;
                }
                else if (purchaseResponse.Ack == AckCode.InvalidInput)
                {

                    payex = new PaymentException(GetCPExceptionMessage(purchaseResponse));
                    payex.ExceptionCode = PaymentExceptioncode.PaymentAuthorizeDigitalOrderInvalidInput;
                }
                else if (purchaseResponse.Ack == AckCode.NonRetryableFailure)
                {
                    if (purchaseResponse.Error.ErrorCode == 30225 || purchaseResponse.Error.ErrorCode == 30336)
                    {
                        //ErrorCode  is valid for following cases:                        
                        //30336 - indicates Payment Declines
                        //30225 - CP risk declines
                        //var riskresult = CheckRiskCase(purchaseResponse.Error.Detail);
                        authresponse.Status = "Aborted";
                        authresponse.PurchaseId = string.Empty;
                        return authresponse;
                    }
                    else
                    {
                        payex = new PaymentException(GetCPExceptionMessage(purchaseResponse));
                        payex.ExceptionCode = PaymentExceptioncode.PaymentAuthorizeDigitalOrderNonRetryableFailure;
                    }
                }
                else if (purchaseResponse.Ack == AckCode.RetryableFailure)
                {
                    payex = new PaymentException(GetCPExceptionMessage(purchaseResponse));
                    payex.ExceptionCode = PaymentExceptioncode.PaymentAuthorizeDigitalOrderRetryableFailure;
                }

                //}
                var context = this.GetCurrentMethodContext(addressid, paymentIdentity.PaymentInstrumentId, paymentIdentity.AccountId, identityValue, skuInfo, PurchasetrackingGuid);
                payex.PaymentServiceLibraryContext = null;
                throw payex;
            }
            catch (PaymentException payEx)
            {
               
                    var context = this.GetCurrentMethodContext(addressid, paymentIdentity.PaymentInstrumentId, paymentIdentity.AccountId, identityValue, skuInfo, PurchasetrackingGuid);
                    HandleException(payEx, context);
                
            }
            catch (ConfigurationException conEx)
            {
               
                    var context = this.GetCurrentMethodContext(addressid, paymentIdentity.PaymentInstrumentId, paymentIdentity.AccountId, identityValue, skuInfo, PurchasetrackingGuid);
                    HandleException(conEx, context);
                
            }
            catch (Exception ex)
            {
                var context = this.GetCurrentMethodContext(addressid, paymentIdentity.PaymentInstrumentId, paymentIdentity.AccountId, identityValue, skuInfo, PurchasetrackingGuid);
                HandleException(ex, context);
            }
            finally
            {
                CloseOrAbortProxy(cpclient);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        internal string AuthorizeOrder(string addressId, PaymentIdentity paymentIdentity, LineItem skuInfo,
                                  Order response, out string purchaseID, out decimal lineItemTax, out decimal shippmentTax, Dictionary<string, string> fraudDetectionProperty = null)
        {
            PaymentException payex = null;
            var trackingGuid = Guid.NewGuid();
            purchaseID = "";
            lineItemTax = 0;
            shippmentTax = 0;
            string purchasestatus = string.Empty;
            string identityValue = string.Empty;
            try
            {
                cpclient = GetCpClient();

                var purchaseRequest = skuInfo.ConvertToPurchaseRequest(this.dataCenter, this.partnerGuid, this.shipFromPath);
                purchaseRequest.BillingInfo = new BillingInfo
                {
                    BillingMode = BillingMode.AuthorizedSettle,
                    PaymentMethod =
                        new RegisteredPaymentMethod { PaymentMethodId = paymentIdentity.PaymentInstrumentId }
                };
                purchaseRequest.AccountId = paymentIdentity.AccountId;

                var getIdentity = SelectIdentity(paymentIdentity);
                identityValue = getIdentity.IdentityValue;

                purchaseRequest.Requester = new Identity { IdentityValue = getIdentity.IdentityValue, IdentityType = getIdentity.IdentityType };
                purchaseRequest.PurchaseContext = new PurchaseContext
                {
                    Timestamp = DateTime.UtcNow,
                    ComputeOnly = false,
                    AddressId = addressId
                };

                purchaseRequest.PurchaseContext.ProductGuid = this.productGuid;
                purchaseRequest.TrackingGuid = trackingGuid;
                PopulateFraudDetectionProperties(purchaseRequest, fraudDetectionProperty);

                var purchaseResponse = cpclient.PurchasePhysicalGoods(purchaseRequest);

                purchaseID = purchaseResponse.PurchaseId;
                response.TotalWithoutTax = purchaseResponse.TotalAmountWithoutTax;
                response.Tax = purchaseResponse.TotalTax;
                response.LineItem = skuInfo;
                response.InstrumentId = purchaseResponse.PurchaseId;

                if (purchaseResponse.Ack == AckCode.Success)
                {
                    //In random cases, Purchase Id is coming null even when the response ACK is Success. 
                    //Throwing an exception in such case
                    if (purchaseResponse.PurchaseId == null)
                    {

                        payex = new PaymentException(CPPaymentIDNullException)
                        {
                            ExceptionCode = PaymentExceptioncode.PaymentAuthorizeOrderNonRetryableFailure
                        };
                        throw payex;
                    }
                    purchasestatus = GetCPPaymentStatusCode(purchaseResponse.PurchaseStatus);

                    //purchasestatus = purchaseResponse.PurchaseStatus;
                    if (purchaseResponse.OrderSet.FirstOrDefault() != null)
                    {
                        if (purchaseResponse.OrderSet.First().ItemEntries.First(x => x.TaxCode == skuInfo.PriceInfo.TaxCode).TaxEntries != null)
                        {
                            lineItemTax +=
                                purchaseResponse.OrderSet.First().ItemEntries.First(x => x.TaxCode == skuInfo.PriceInfo.TaxCode).TaxEntries.
                                    Sum(taxentry => taxentry.Amount);
                        }
                        if (purchaseResponse.OrderSet.First().ItemEntries.First(x => x.TaxCode == skuInfo.ShippingInfo.PriceInfo.TaxCode).TaxEntries != null)
                        {
                            shippmentTax +=
                                purchaseResponse.OrderSet.First().ItemEntries.First(x => x.TaxCode == skuInfo.ShippingInfo.PriceInfo.TaxCode).
                                    TaxEntries.Sum(taxentry => taxentry.Amount);
                        }
                    }
                    return purchasestatus;
                }
                else if (purchaseResponse.Ack == AckCode.Success && purchaseResponse.OrderSet.FirstOrDefault() == null)
                {
                    payex = new PaymentException("Could not Authorize the order.");
                    payex.ExceptionCode = PaymentExceptioncode.PaymentAuthorizeOrderResponseNull;
                }
                else if (purchaseResponse.Ack == AckCode.InvalidInput)
                {
                    payex = new PaymentException(GetCPExceptionMessage(purchaseResponse));
                    payex.ExceptionCode = PaymentExceptioncode.PaymentAuthorizeOrderInvalidInput;
                }
                else if (purchaseResponse.Ack == AckCode.NonRetryableFailure)
                {

                    if (purchaseResponse.Error.ErrorCode == 30225 || purchaseResponse.Error.ErrorCode == 30336)
                    {
                        //ErrorCode is valid for following cases:                        
                        //30336 - indicates Payment Declines
                        //30225 - CP risk declines
                        //var riskresult = CheckRiskCase(purchaseResponse.Error.Detail);
                        purchasestatus = "Aborted";

                        return purchasestatus;
                    }
                    else
                    {
                        payex = new PaymentException(GetCPExceptionMessage(purchaseResponse));
                        payex.ExceptionCode = PaymentExceptioncode.PaymentAuthorizeOrderNonRetryableFailure;
                    }
                }
                else if (purchaseResponse.Ack == AckCode.RetryableFailure)
                {
                    payex = new PaymentException(GetCPExceptionMessage(purchaseResponse));
                    payex.ExceptionCode = PaymentExceptioncode.PaymentAuthorizeOrderRetryableFailure;
                }

                var context = this.GetCurrentMethodContext(addressId, paymentIdentity.PaymentInstrumentId, paymentIdentity.AccountId, identityValue, skuInfo, response, purchaseID, lineItemTax, shippmentTax);
                payex.PaymentServiceLibraryContext = context;
                throw payex;
            }
            catch (PaymentException payEx)
            {
                
                    var context = this.GetCurrentMethodContext(addressId, paymentIdentity.PaymentInstrumentId, paymentIdentity.AccountId, identityValue, skuInfo, response, purchaseID, lineItemTax, shippmentTax);
                    HandleException(payEx, context);
                
            }
            catch (ConfigurationException conEx)
            {
                
                    var context = this.GetCurrentMethodContext(addressId, paymentIdentity.PaymentInstrumentId, paymentIdentity.AccountId, identityValue, skuInfo, response, purchaseID, lineItemTax, shippmentTax);
                    HandleException(conEx, context);
                
            }
            catch (Exception ex)
            {
                var context = this.GetCurrentMethodContext(addressId, paymentIdentity.PaymentInstrumentId, paymentIdentity.AccountId, identityValue, skuInfo, response, purchaseID, lineItemTax, shippmentTax);
                HandleException(ex, context);
            }
            finally
            {
                CloseOrAbortProxy(cpclient);
            }

            return purchasestatus;
        }

        /// <summary>
        /// 
        /// </summary>
        internal string SettlePurchaseGoods(PaymentIdentity paymentIdentity, LineItem skuInfo, string purchaseID, Guid trackingGuid)
        {
            string status = string.Empty;
            var getIdentity = SelectIdentity(paymentIdentity);
            try
            {
                PaymentException payex = null;
                cpclient = GetCpClient();

                var settleRequest = GetSettlePhysicalGoodsRequestObject(paymentIdentity.AccountId, getIdentity, skuInfo, purchaseID, trackingGuid);
                var settleResponse = cpclient.SettlePhysicalGoods(settleRequest);

                if (settleResponse.Ack == AckCode.Success)
                {
                    status = GetCPPaymentStatusCode(settleResponse.Ack.ToString());
                    return status;
                }

                switch (settleResponse.Ack)
                {
                    case AckCode.InvalidInput:
                        payex = new PaymentException(GetCPExceptionMessage(settleResponse))
                        {
                            ExceptionCode = PaymentExceptioncode.PaymentSettlePhysicalGoodsInvalidInput
                        };
                        break;
                    case AckCode.NonRetryableFailure:
                        if (settleResponse.Error.Detail.ToString().Equals(this.alreadySettledErrMsg, StringComparison.OrdinalIgnoreCase))
                        {
                            status = "Complete";
                            return status;
                        }
                        else
                        {
                            payex = new PaymentException(GetCPExceptionMessage(settleResponse))
                            {
                                ExceptionCode = PaymentExceptioncode.PaymentSettlePhysicalGoodsNonRetryableFailure
                            };
                        }
                        break;
                    case AckCode.RetryableFailure:
                        payex = new PaymentException(GetCPExceptionMessage(settleResponse))
                        {
                            ExceptionCode = PaymentExceptioncode.PaymentSettlePhysicalGoodsRetryableFailure
                        };
                        break;
                }
                var context = this.GetCurrentMethodContext(paymentIdentity.AccountId, getIdentity.IdentityValue, skuInfo, purchaseID, trackingGuid);
                payex.PaymentServiceLibraryContext = context;
                throw payex;
            }
            catch (PaymentException payEx)
            {
                
                var context = this.GetCurrentMethodContext(paymentIdentity.AccountId, getIdentity.IdentityValue, skuInfo, purchaseID, trackingGuid);
                HandleException(payEx, context);
                
            }
            catch (ConfigurationException conEx)
            {
                var context = this.GetCurrentMethodContext(paymentIdentity.AccountId, getIdentity.IdentityValue, skuInfo, purchaseID, trackingGuid);
                HandleException(conEx, context);
            }
            catch (Exception ex)
            {
                var context = this.GetCurrentMethodContext(paymentIdentity.AccountId, getIdentity.IdentityValue, skuInfo, purchaseID, trackingGuid);
                HandleException(ex, context);
            }
            finally
            {
                CloseOrAbortProxy(cpclient);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        private static SettlePhysicalGoodsRequest GetSettlePhysicalGoodsRequestObject(string accountId, Identity identity, LineItem skuInfo, string purchaseID, Guid trackingGuid)
        {
            return new SettlePhysicalGoodsRequest
            {
                AccountContext = new AccountContext
                {
                    CountryCode = skuInfo.AddressInfo.Country,
                    CustomerType = CustomerType.Personal
                },
                AccountId = accountId,
                Requester = new Identity { IdentityValue = identity.IdentityValue, IdentityType = identity.IdentityType },
                PurchaseId = purchaseID,
                PackageShipmentInfo = new PackageShipmentInfo
                {
                    CarrierName = "Air",
                    CarrierTrackingNumber = "123456",
                    ShipmentMethod = "UPS"
                },
                TrackingGuid = trackingGuid
            };
        }
        #region Private Methods
       
        /// 
        /// </summary>
        private string GetCPPaymentStatusCode(string purchasestatus)
        {
            string purchaseReturnStatus = default(string);

            switch (purchasestatus)
            {
                case "AuthFailed":
                case "AuthSettleFailed":
                case "AuthSettleConfirmationFailed":
                case "Abandon":
                case "Aborted":
                case "Rejected":
                    purchaseReturnStatus = "Aborted";
                    break;
                case "AuthSucceeded":
                case "AuthRequested":
                case "AuthSettleRequested":
                case "InProgress":
                case "AuthSettleConfirmationRequested":
                    purchaseReturnStatus = "InProgress";
                    break;
                case "AuthSettleSucceeded":
                case "AuthSettleConfirmationSucceeded":
                case "Complete":
                case "Success":
                case "ImmediateSettleSucceeded":
                    purchaseReturnStatus = "Complete";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(string.Format("{0} - {1}", purchasestatus, "Invalid Payment Status Returned From Commerce Platform"));

            }
            return purchaseReturnStatus;
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopulateFraudDetectionProperties(AbstractPurchaseRequest purchaseRequest, Dictionary<string, string> fraudDetectionProperty)
        {
            if (this.enableFraudDetection && fraudDetectionProperty != null && fraudDetectionProperty.Count > 0)
            {
                var properties = fraudDetectionProperty.ToList().ConvertAll<Property>(
                    prop => new Property { Name = prop.Key, Value = prop.Value, Namespace = "Platform" });

                purchaseRequest.FraudDetectionProperties = (from p in properties where !string.IsNullOrEmpty(p.Value) select p).ToArray();

                purchaseRequest.DeviceInfo = new DeviceInfo
                {
                    IPAddress = fraudDetectionProperty.FirstOrDefault(k => k.Key.Equals("IPAddress")).Value,
                    DeviceType = fraudDetectionProperty.FirstOrDefault(k => k.Key.Equals("DeviceType")).Value
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string GetCPExceptionMessage(AbstractResponse CPResponse)
        {
            string strCPResponse = "";
            if (CPResponse != null)
            {
                if (CPResponse.Error != null)
                {
                    strCPResponse = System.String.Format("{1} (CP ErrorCode:{0})",
                       CPResponse.Error.ErrorCode, CPResponse.Error.Detail);

                }
            }
            return strCPResponse;
        }

        /// <summary>
        /// Clean the WCF proxy
        /// </summary>
        /// <param name="proxy"></param>
        private void CloseOrAbortProxy(ICommerceService proxy)
        {
            if (proxy != null)
            {
                if (proxy.State != CommunicationState.Faulted && proxy.State != CommunicationState.Closed)
                {
                    try
                    {
                        proxy.Close();
                    }
                    catch
                    {
                        proxy.Abort();
                    }
                }
                else
                {
                    proxy.Abort();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void HandleException(Exception exception, Dictionary<string, string> context)
        {
            PaymentServiceLibraryException derived = null;

            if (exception is EndpointNotFoundException)
            {
                derived = new PaymentException(exception.Message, PaymentExceptioncode.PaymentEndpointNotFoundException,
                                                      exception);
            }
            else if (exception is TimeoutException)
            {
                derived = new PaymentException(exception.Message, PaymentExceptioncode.PaymentTimeoutException,
                                                      exception);
            }

            else if (exception is System.Web.Services.Protocols.SoapException)
            {
                derived = new PaymentException(exception.Message, PaymentExceptioncode.PaymentSoapException,
                                                      exception);
            }
            else if (exception is ConfigurationException)
            {
                derived = new PaymentException(exception.Message, PaymentExceptioncode.CPRequiredConfigValuesUnavailable,
                                                      exception);
            }
            else if (exception is PaymentException)
            {
                derived = new PaymentException(exception.Message, ((PaymentServiceLibraryException)(exception)).ExceptionCode,
                                                      exception);
            }
            else
            {
                derived = exception as PaymentIntegrationException ??
                          new PaymentException(exception.Message, PaymentExceptioncode.UnknownGeneralException, exception);
            }

            derived.AddToExistingContext(context);

            
            throw derived;
        }

        
        #endregion
    }
}
