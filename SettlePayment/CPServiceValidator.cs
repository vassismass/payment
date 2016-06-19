using Microsoft.SupplyChain.Care.Payment.ServiceLibrary.Exceptions;
using Microsoft.SupplyChain.Care.Payment.ServiceLibrary.Geography;
using Microsoft.SupplyChain.Care.PaymentModels;
using System;

namespace SettlePayment
{
    public static class CPServiceValidator
        {
           
           
            /// <summary>
            /// Performs the PurchaseGoodsRequest request and updates the ValidationException for any deviances
            /// </summary>
            internal static bool ValidatePurchaseGoods(PurchaseGoodsRequest request, ref ValidationException fault)
            {
                //Validate PurchaseGoodsRequest object
                if (request == null)
                {
                    fault = new ValidationException("Input parameter: PurchaseGoodsRequest is NULL");
                    fault.ExceptionCode = PaymentExceptioncode.PurchaseGoodsRequestIsNull;
                    return false;
                }

                //validate Identity
                if (string.IsNullOrEmpty(request.paymentIdentityInfo.Puid) && string.IsNullOrEmpty(request.paymentIdentityInfo.GuestId))
                {
                    fault = new ValidationException("Input parameter: Identity is invalid");
                    fault.ExceptionCode = PaymentExceptioncode.PurchaseGoodsRequestIdentityIsNull;
                    return false;
                }

                //validate Country
                if (string.IsNullOrEmpty(request.Country))
                {
                    fault = new ValidationException("Input parameter: Country is invalid");
                    fault.ExceptionCode = PaymentExceptioncode.PurchaseGoodsRequestCountryIsNull;
                    return false;
                }

                //validate Currency
                if (string.IsNullOrEmpty(request.Currency))
                {
                    fault = new ValidationException("Input parameter: Currency is invalid");
                    fault.ExceptionCode = PaymentExceptioncode.PurchaseGoodsRequestCurrencyIsNull;
                    return false;
                }

                //validate Locale
                if (string.IsNullOrEmpty(request.Locale))
                {
                    fault = new ValidationException("Input parameter: Locale is invalid");
                    fault.ExceptionCode = PaymentExceptioncode.PurchaseGoodsRequestLocaleIsNull;
                    return false;
                }

                //validate Account
                if (request.Account == null)
                {
                    fault = new ValidationException("Input parameter: Account is invalid");
                    fault.ExceptionCode = PaymentExceptioncode.PurchaseGoodsRequestAccountIsNull;
                    return false;
                }

                //validate AccountID
                if (request.Account.AccountId == null)
                {
                    fault = new ValidationException("Input parameter: AccountId is invalid");
                    fault.ExceptionCode = PaymentExceptioncode.PurchaseGoodsRequestAccountIdIsNull;
                    return false;
                }

                //validate SerialNumber
                if (string.IsNullOrEmpty(request.SerialNumber))
                {
                    fault = new ValidationException("Input parameter: SerialNumber is invalid");
                    fault.ExceptionCode = PaymentExceptioncode.PurchaseGoodsRequestSerialNumberIsNull;
                    return false;
                }

                //validate PaymentInstrumentId
                if (string.IsNullOrEmpty(request.paymentIdentityInfo.PaymentInstrumentId))
                {
                    fault = new ValidationException("Input parameter: PaymentInstrumentId is invalid");
                    fault.ExceptionCode = PaymentExceptioncode.PurchaseGoodsRequestPaymentInstrumentIdIsNull;
                    return false;
                }

                //validate SKUInfo
                if (request.SKUInfo == null)
                {
                    fault = new ValidationException("Input parameter: SKUInfo is invalid");
                    fault.ExceptionCode = PaymentExceptioncode.PurchaseGoodsRequestSKUInfoIsNull;
                    return false;
                }

                return true;
            }

            /// <summary>
            /// Performs the SettlePurchaseGoodsRequest request and updates the ValidationException for any deviances
            /// </summary>
            internal static bool ValidateSettlePurchaseGoods(SettlePurchaseGoodsRequest request, ref ValidationException fault)
            {
                //Validate SettlePurchaseGoodsRequest object
                if (request == null)
                {
                    fault = new ValidationException("Input parameter: SettlePurchaseGoodsRequest is NULL");
                    fault.ExceptionCode = PaymentExceptioncode.SettlePurchaseGoodsRequestIsNull;
                    return false;
                }

                //validate Country
                if (string.IsNullOrEmpty(request.Country))
                {
                    fault = new ValidationException("Input parameter: Country is invalid");
                    fault.ExceptionCode = PaymentExceptioncode.SettlePurchaseGoodsRequestCountryIsNull;
                    return false;
                }

                //validate Identity
                if (string.IsNullOrEmpty(request.paymentIdentityInfo.GuestId) && string.IsNullOrEmpty(request.paymentIdentityInfo.Puid))
                {
                    fault = new ValidationException("Input parameter: Identity is invalid");
                    fault.ExceptionCode = PaymentExceptioncode.SettlePurchaseGoodsRequestIdentityIsNull;
                    return false;
                }

                //validate Account
                if (request.SkuInfo == null)
                {
                    fault = new ValidationException("Input parameter: SkuInfo is invalid");
                    fault.ExceptionCode = PaymentExceptioncode.SettlePurchaseGoodsRequestSkuInfoIsNull;
                    return false;
                }

                //validate PurchaseID
                if (string.IsNullOrEmpty(request.PurchaseID))
                {
                    fault = new ValidationException("Input parameter: PurchaseID is invalid");
                    fault.ExceptionCode = PaymentExceptioncode.SettlePurchaseGoodsRequestPurchaseIDIsNull;
                    return false;
                }

                //validate TrackingGuid
                if (request.TrackingGuid == Guid.Empty)
                {
                    fault = new ValidationException("Input parameter: TrackingGuid is invalid");
                    fault.ExceptionCode = PaymentExceptioncode.SettlePurchaseGoodsRequestTrackingGuidIsNull;
                    return false;
                }

                //validate AccountID
                if (string.IsNullOrEmpty(request.AccountID))
                {
                    fault = new ValidationException("Input parameter: AccountID is invalid");
                    fault.ExceptionCode = PaymentExceptioncode.SettlePurchaseGoodsRequestAccountIDIsNull;
                    return false;
                }

                return true;
            }
        }
    }
