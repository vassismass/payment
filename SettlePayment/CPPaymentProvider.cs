using Microsoft.SupplyChain.Care.PaymentModels;
using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using Microsoft.SupplyChain.Care.Payment.ServiceLibrary;
using Microsoft.SupplyChain.Care.Payment.ServiceLibrary.Exceptions;
using System.Linq;
using SettlePayment.Exceptions;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

namespace SettlePayment
{
    public class CPPaymentProvider : PaymentProvider
    {
        private const string ConfigErrorMsg = "Empty or missing '{0}'";
        private const string PartnerGuidKey = "partnerGuid";
        private const string ProductGuidKey = "productGuid";
        private const string IsZipCodeIncludedInCpComparisionKey = "isZipCodeIncludedInCpComparision";
        private const string EnableFraudDetectionKey = "enableFraudDetection";
        private const string ShipFromPathKey = "shipFromPath";
        private const string AlreadySettledErrMsgKey = "alreadySettledErrMsg";
        private const string DatacenterCurrentKey = "datacenterCurrent";
        private const string ShippingAddressNotFound = "Shipping address is not found in user profile.";
        public bool IsZipCodeIncluded;
        private CPAgentInfo info;
        private CPServiceAgent _serviceAgent;
        private IBillingServices _billingService;
        private Dictionary<string, string> Configuration = new Dictionary<string, string>();

        public CPPaymentProvider(IGetConfiguration Configuration, IServiceAgent serviceAgent, IBillingServices billingService)
        {
            PaymentConfiguration config = Configuration.GetConfig();
            foreach (var c in config.Config)
            {
                this.Configuration.Add(c.Key, c.Value);
            }
            _serviceAgent = GetServiceAgent();
            _billingService = billingService;
        }
        #region Properties
        private Guid PartnerGuid
        {
            get
            {
                string partnerGuid = this.Configuration[PartnerGuidKey];
                if (String.IsNullOrEmpty(partnerGuid))
                {
                    throw new ProviderException(string.Format(ConfigErrorMsg, PartnerGuidKey));
                }
                return new Guid(partnerGuid);
            }
        }

        private Guid ProductGuid
        {
            get
            {
                var productGuid = this.Configuration[ProductGuidKey];
                if (String.IsNullOrEmpty(productGuid))
                {
                    throw new ProviderException(string.Format(ConfigErrorMsg, ProductGuidKey));
                }
                return new Guid(productGuid);
            }
        }

        private string ShipFromPath
        {
            get
            {
                var shipFromPath = this.Configuration[ShipFromPathKey];
                if (String.IsNullOrEmpty(shipFromPath))
                {
                    throw new ProviderException(string.Format(ConfigErrorMsg, ShipFromPathKey));
                }
                return shipFromPath;
            }
        }

        private string AlreadySettledErrMsg
        {
            get
            {
                var alreadySettledErrMsg = this.Configuration[AlreadySettledErrMsgKey];
                if (String.IsNullOrEmpty(alreadySettledErrMsg))
                {
                    throw new ProviderException(string.Format(ConfigErrorMsg, AlreadySettledErrMsgKey));
                }
                return alreadySettledErrMsg;
            }
        }

        private string DatacenterCurrent
        {
            get
            {
                var datacenterCurrent = this.Configuration[DatacenterCurrentKey];
                if (String.IsNullOrEmpty(datacenterCurrent))
                {
                    throw new ProviderException(string.Format(ConfigErrorMsg, DatacenterCurrentKey));
                }
                return datacenterCurrent;
            }
        }

        private bool IsZipCodeIncludedInCpComparision
        {
            get
            {
                var isZipCodeIncludedInCpComparision = this.Configuration[IsZipCodeIncludedInCpComparisionKey];
                if (String.IsNullOrEmpty(isZipCodeIncludedInCpComparision))
                {
                    throw new ProviderException(string.Format(ConfigErrorMsg, IsZipCodeIncludedInCpComparisionKey));
                }
                return bool.Parse(isZipCodeIncludedInCpComparision);
            }
        }

        private bool EnableFraudDetection
        {
            get
            {
                var enableFraudDetection = this.Configuration[EnableFraudDetectionKey];
                if (String.IsNullOrEmpty(enableFraudDetection))
                {
                    throw new ProviderException(string.Format(ConfigErrorMsg, EnableFraudDetectionKey));
                }
                return bool.Parse(enableFraudDetection);
            }
        }
        #endregion

        #region Public Methods
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override PurchaseGoodsResponse PurchaseGoods(PurchaseGoodsRequest request)
        {
            var response = new PurchaseGoodsResponse();

            try
            {
                ValidationException fault = null;
                if (!CPServiceValidator.ValidatePurchaseGoods(request, ref fault))
                    throw fault;
                //todo 2 : return true from Validate
                var purchaseID = default(string);
                var lineItemTax = default(decimal);
                var shippmentTax = default(decimal);
                var orderStatus = default(string);

                var order = PurchaseGoods(
                    request.PurchaseType, request.TrackingGuid,
                    request.paymentIdentityInfo, request.Country,
                    request.Account, request.SerialNumber, request.Locale, request.Currency,
                    request.SKUInfo, request.FraudDetectionProperty, out purchaseID, out lineItemTax, out shippmentTax, out orderStatus);

                //todo 3: validate response
                response.PurchaseId = purchaseID;
                response.LineItemTax = lineItemTax;
                response.ShippmentTax = shippmentTax;
                response.Order = order;
                response.OrderStatus = orderStatus;
            }
            catch (Exception ex)
            {
                //todo 4
                if (ex is PaymentIntegrationException)
                    throw;

                var context = new Dictionary<string, string>();
                context["MethodName"] = "PurchaseGoods";
                context["PurchaseGoodsRequest"] = XmlUtility.SerializeObject(request);

                this.HandleException(ex, PaymentExceptioncode.PaymentProviderPurchaseGoodsException, context);
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>*/
        public override SettlePurchaseGoodsResponse SettlePurchaseGoods(SettlePurchaseGoodsRequest request)
        {
            var response = new SettlePurchaseGoodsResponse();
            try
            {
                ValidationException fault = null;
                if (!CPServiceValidator.ValidateSettlePurchaseGoods(request, ref fault))
                    throw fault;

                response.Status = SettlePurchaseGoods(request.paymentIdentityInfo, request.Country, request.SkuInfo, request.PurchaseID, request.TrackingGuid);
            }
            catch (Exception ex)
            {
                if (ex is PaymentIntegrationException)
                    throw ex;

                var context = new Dictionary<string, string>();
                context["MethodName"] = "SettlePurchaseGoods";
                context["SettlePurchaseGoodsRequest"] = XmlUtility.SerializeObject(request);

                this.HandleException(ex, PaymentExceptioncode.PaymentProviderSettlePurchaseGoodsException, context);
            }
            return response;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        internal
            string GetAddressId(AccountInfo accountInfo, PaymentIdentity paymentIdentity, LineItem skuInfo, string locale, string currency)
        {
            var addressId = default(string);
            var matchedAddress = Microsoft.SupplyChain.Care.PaymentModels.Address.FindAddress(accountInfo.Addresses, skuInfo.AddressInfo);
            if (matchedAddress == null)
            {
                //Update Billing Account
                AccountInfo updateAccountInfo = new AccountInfo();

                updateAccountInfo.AccountId = accountInfo.AccountId;
                updateAccountInfo.Address = skuInfo.AddressInfo;
                if (accountInfo.Address != null)
                {
                    updateAccountInfo.Address.FriendlyName = accountInfo.Address.FriendlyName;
                }
                else
                {
                    updateAccountInfo.Address.FriendlyName = updateAccountInfo.Address.FirstName + updateAccountInfo.Address.LastName;
                }

                accountInfo = _billingService.UpdateBillingAccount(updateAccountInfo, paymentIdentity, locale, currency);
                try
                {
                    IsZipCodeIncluded = this.IsZipCodeIncludedInCpComparision;
                }
                catch(Exception e)
                { throw e;
                }

                if (IsZipCodeIncluded)
                {
                    var address = Microsoft.SupplyChain.Care.PaymentModels.Address.FindAddress(accountInfo.Addresses, updateAccountInfo.Address);
                    if (address != null)
                    {
                        addressId = address.AddressId;
                    }
                    else
                    {
                        throw new AddressException(ShippingAddressNotFound + "\n" + Serialize(updateAccountInfo.Address) + "\n" + Serialize(accountInfo.Addresses));
                    }
                }
                else
                {
                    //addressId = accountInfo.Addresses.Where(ad => ad.Equals(updateAccountInfo.Address)).First().AddressId;
                    var matchedResults = accountInfo.Addresses.Where(ad => ad.Equals(updateAccountInfo.Address));
                    if (matchedResults.Any())
                    {
                        addressId = matchedResults.First().AddressId;
                    }
                }
            }
            else
            {
                addressId = matchedAddress.AddressId;
            }

            return addressId;
        }
        #region Private Methods
        /// <summary>
        /// 
        /// </summary>


        private Order PurchaseGoods(string purchaseType, Guid trackingGuid, PaymentIdentity paymentIdentity, string country, AccountInfo accountInfo, string serialNumber, string locale, string currency,
             LineItem skuInfo, Dictionary<string, string> fraudDetectionProperty, out string purchaseID, out decimal lineItemTax, out decimal shippmentTax, out string status)
        {
            var response = new Order();
            var accountInfoGet = _billingService.GetBillingAccount(country, paymentIdentity);
            var addressId = GetAddressId(accountInfoGet, paymentIdentity, skuInfo, locale, currency);

            purchaseID = default(string);
            lineItemTax = default(decimal);
            shippmentTax = default(decimal);
            status = default(string);

            if (purchaseType.Equals("digital", StringComparison.OrdinalIgnoreCase))//Digital Order
            {
                var AuthorizeDigitalPurchaseStatus =_serviceAgent.AuthorizeDigitalPurchase(addressId, paymentIdentity, skuInfo, trackingGuid, fraudDetectionProperty);
                status = AuthorizeDigitalPurchaseStatus.Status.ToString();
                if (!string.IsNullOrEmpty(AuthorizeDigitalPurchaseStatus.PurchaseId.ToString()))
                    purchaseID = AuthorizeDigitalPurchaseStatus.PurchaseId.ToString();

            }
            else if (purchaseType.Equals("physical", StringComparison.OrdinalIgnoreCase))//Physical Order
                status = _serviceAgent.AuthorizeOrder(addressId, paymentIdentity, skuInfo, response, out purchaseID, out lineItemTax, out shippmentTax, fraudDetectionProperty);

            return response;
        }
        //  private string SettlePurchaseGoods(string accountId, string identity, string country, LineItem skuInfo, string purchaseID, Guid trackingGuid)
        private string SettlePurchaseGoods(PaymentIdentity paymentIdentity, string country, LineItem skuInfo, string purchaseID, Guid trackingGuid)
        {
            //var puid = long.Parse(identity);
            var billService = new BillingService();
            var accountInfoGet = billService.GetBillingAccount(country, paymentIdentity);
            return _serviceAgent.SettlePurchaseGoods(paymentIdentity, skuInfo, purchaseID, trackingGuid);
        }


        private static string Serialize(object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                var xs = new XmlSerializer(obj.GetType());
                var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

                xs.Serialize(xmlTextWriter, obj);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }
        private CPAgentInfo GetInfo()
        {
            CPAgentInfo inf = new CPAgentInfo();
            inf.productGuid = this.ProductGuid;
            inf.partnerGuid = this.PartnerGuid;
            inf.isZipCodeIncludedInCpComparision = this.IsZipCodeIncludedInCpComparision;
            inf.dataCenter = this.DatacenterCurrent;
            inf.shipFromPath = this.ShipFromPath;
            return inf;
        }
        private CPServiceAgent GetServiceAgent()
        {
            info = GetInfo();
            return new CPServiceAgent(info);
        }
        #endregion
    }
}
