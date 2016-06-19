using System;
using Microsoft.SupplyChain.Care.PaymentModels;
using System.Configuration;
using System.Linq;
using CPProxy = Microsoft.SupplyChain.Care.Payment.ServiceLibrary.CP;
using Microsoft.SupplyChain.Care.Payment.ServiceLibrary.Extension;
using System.Collections.Generic;
using Microsoft.SupplyChain.Care.Payment.ServiceLibrary.Exceptions;
using Microsoft.SupplyChain.Care.Payment.ServiceLibrary.CommonMethods;

namespace Microsoft.SupplyChain.Care.Payment.ServiceLibrary
{
    public class BillingService : IBillingServices
    {
        readonly static List<int> LstAddressErrorCodes = new List<int> { 60001, 60011, 60012, 60013, 60014, 60015, 60016, 60017, 60018, 60019, 60029, 60030, 60041, 60042, 10021 };
        public BillingService()
        {
        }


        private CPProxy.ICommerceService GetCpClient()
        {
            return new CP.CommerceServiceClient();
        }


        /// <summary>
        /// Get the Account Information based on Country & PUID/GuestId combination
        /// </summary>
        /// <param name="country"></param>
        /// <param name="paymentIdentity"></param>
        /// <returns></returns>
        public AccountInfo GetBillingAccount(string country, PaymentIdentity paymentIdentity)
        {
            CPProxy.ICommerceService cpclient = null;
            string identityValue = string.Empty;
            return GetBillingAccount(country, paymentIdentity, cpclient, identityValue); 
        }
        //96 lines
        private AccountInfo GetBillingAccount(string country, PaymentIdentity paymentIdentity, CPProxy.ICommerceService cpclient, string identityValue)
        {
            try
            {

                PaymentException payex = null;

                var countryIso2 = CorePaymentServiceExtension.GetTwoLetterCountryCode(country);

                cpclient = GetCpClient();

                var getIdentity = PaymentCommon.SelectIdentity(paymentIdentity);
                identityValue = getIdentity.IdentityValue;

                var searchaccountRequest = new CPProxy.SearchAccountsRequest
                {
                    Filters = new CPProxy.SearchAccountFilters { CustomerType = CPProxy.CustomerType.Personal, CountryCode = countryIso2 },

                    Requester = new CPProxy.Identity { IdentityValue = getIdentity.IdentityValue, IdentityType = getIdentity.IdentityType }

                };

                CPProxy.SearchAccountsResponse searchresult = cpclient.SearchAccounts(searchaccountRequest);

                if (searchresult == null || searchresult.AccountInfos == null || searchresult.AccountInfos.Count() == 0)
                {
                    //No AccountId passed in input
                    if (paymentIdentity.AccountId == null && searchresult.Ack == CPProxy.AckCode.Success)
                    {
                        return null;
                    }
                    //AccountID was passed in input request to be matched
                    else
                    {
                        throw new PaymentException(String.Format("The AccountID: {0} does not exist for Country: {1} and PUID/GuestID: {2} ", paymentIdentity.AccountId, countryIso2, identityValue))
                        { ExceptionCode = PaymentExceptioncode.PaymentGetBillingAccountInvalidInput }; ;
                    }
                }

                CPProxy.AccountInfoOutput accountInfoOutput = null;
                if (string.IsNullOrEmpty(paymentIdentity.AccountId))
                {
                    accountInfoOutput = searchresult.AccountInfos.FirstOrDefault(d => d.AccountLevel == "Primary" && d.Status == "Active");

                    if (accountInfoOutput == null)
                        return null;
                }
                else
                {
                    accountInfoOutput = searchresult.AccountInfos.FirstOrDefault(p => p.AccountId.Equals(paymentIdentity.AccountId));

                    if (accountInfoOutput == null)
                    {
                        throw new PaymentException(String.Format("The AccountID: {0} does not exist for Country: {1} and PUID/GuestID: {2} ", paymentIdentity.AccountId, countryIso2, identityValue))
                        { ExceptionCode = PaymentExceptioncode.PaymentGetBillingAccountInvalidInput };
                    }
                }
                
                if (searchresult.Ack == CPProxy.AckCode.Success)
                {
                    return AccountAcknowleged(accountInfoOutput);
                }
        
                payex = IdentifyAccountException(searchresult);
                if (payex != null)
                {
                    var context = this.GetCurrentMethodContext(getIdentity.IdentityValue, country);
                    payex.PaymentServiceLibraryContext = context;
                    throw payex;
                }
            }
            catch (Exception ex)
            {
                PaymentException payex = new PaymentException();
                var context = this.GetCurrentMethodContext(identityValue, country);
                payex.HandleException(ex, context);
            }
            finally
            {
                PaymentCommon.CloseOrAbortProxy(cpclient);
            }
            return null;
        }
        //20lines
        private PaymentException IdentifyAccountException(CPProxy.SearchAccountsResponse searchresult)
        {
            PaymentException payex = null;
            switch (searchresult.Ack)
            {
                case CPProxy.AckCode.InvalidInput:
                    payex = new PaymentException(PaymentCommon.GetCPExceptionMessage(searchresult)) { ExceptionCode = PaymentExceptioncode.PaymentGetBillingAccountInvalidInput };
                    break;
                case CPProxy.AckCode.NonRetryableFailure:
                    payex = new PaymentException(PaymentCommon.GetCPExceptionMessage(searchresult)) { ExceptionCode = PaymentExceptioncode.PaymentGetBillingAccountNonRetryableFailure };
                    break;
                case CPProxy.AckCode.RetryableFailure:
                    payex = new PaymentException(PaymentCommon.GetCPExceptionMessage(searchresult)) { ExceptionCode = PaymentExceptioncode.PaymentGetBillingAccountRetryableFailure };
                    break;
                default:
                    payex = null;
                    break;
            }
            return payex;
        }
        //36 lines
        private AccountInfo AccountAcknowleged(CPProxy.AccountInfoOutput accountInfoOutput)
        {
            var accountInfo = new AccountInfo { AccountId = accountInfoOutput.AccountId };
            CPProxy.Address defaultAddress = null;

            try
            {
                defaultAddress = accountInfoOutput.AddressSet.FirstOrDefault(a => a.AddressId == accountInfoOutput.DefaultAddressId);
            }
            catch
            {
            }
            if (defaultAddress != null)
            {
                accountInfo.Address =
                    new Address
                    {
                        Address1 = defaultAddress.Street1,
                        City = defaultAddress.City,
                        Country = defaultAddress.CountryCode,
                        Zip = defaultAddress.PostalCode,
                        FriendlyName = defaultAddress.FriendlyName,
                        FirstName = defaultAddress.FirstName,
                        LastName = defaultAddress.LastName,
                        Email = accountInfoOutput.Email
                    };
            }
            accountInfo.Addresses = new List<Address>();
            if (accountInfoOutput.AddressSet != null)
            {
                foreach (var item in accountInfoOutput.AddressSet)
                {
                    accountInfo.Addresses.Add(item.ConvertCPAddressToAddress(accountInfoOutput.Email));
                }
            }
            return accountInfo;
        }
        /// <summary>
        /// Update Billing Account
        /// </summary>
        /// <param name="accountInfo">Account Details</param>
        /// <param name="serialNumber">Device Key</param>
        /// <param name="locale">Locale</param>
        /// <param name="currency">Currency</param>
        /// <returns>Updated AccountInfo</returns>






    

        
        public AccountInfo UpdateBillingAccount(AccountInfo accountInfo, PaymentIdentity paymentIdentity, string locale, string currency)
        {
            PaymentException payex = null;
            PaymentAddressException payAdEx = null;
            CPProxy.ICommerceService cpclient = null;
            string identityValue = string.Empty;
            return UpdateBillingAccount(accountInfo, paymentIdentity, locale, currency, payex, payAdEx, cpclient, identityValue);
        }

        private CPProxy.Identity GenerateIdentity(CPProxy.Identity getIdentity, AccountInfo accountInfo)
        {
            var identity = new CPProxy.Identity
            {
                IdentityType = getIdentity.IdentityType,
                IdentityValue = getIdentity.IdentityValue,
                IdentityEmail = accountInfo.Address.Email
            };
            return identity;
        }

        private CPProxy.UpdateAccountRequest GenerateRequest(CPProxy.Identity identity, AccountInfo accountInfo, string locale)
        {
            var request = new CPProxy.UpdateAccountRequest
            {
                //DeviceInfo = new DeviceInfo { DeviceId = serialNumber },
                Requester = identity,
                TrackingGuid = Guid.NewGuid(),
                AccountInfoInput = new CPProxy.AccountInfoInput
                {
                    FirstName = accountInfo.Address.FirstName,
                    LastName = accountInfo.Address.LastName,
                    FriendlyName = accountInfo.Address.FriendlyName,
                    Email = accountInfo.Address.Email,
                    CustomerType = CPProxy.CustomerType.Personal,
                    CountryCode = CorePaymentServiceExtension.GetTwoLetterCountryCode(accountInfo.Address.Country),
                    Locale = locale

                },
                AccountId = accountInfo.AccountId

            };
            return request;
        }

        private CPProxy.Address GenerateAddress(AccountInfo accountInfo)
        {
            var address = new CPProxy.Address
            {
                FirstName = accountInfo.Address.FirstName,
                LastName = accountInfo.Address.LastName,
                FriendlyName = accountInfo.Address.FriendlyName,
                ExternalAddressId = Guid.NewGuid().ToString(),
                City = accountInfo.Address.City,
                CountryCode = CorePaymentServiceExtension.GetTwoLetterCountryCode(accountInfo.Address.Country),
                PostalCode = accountInfo.Address.Zip,
                State = accountInfo.Address.State,
                Street1 = accountInfo.Address.Address1,
                Street2 = accountInfo.Address.Address2,
                Street3 = accountInfo.Address.Address3,
                UnitNumber = accountInfo.Address.UnitNumber,

            };
            return address;
        }
        private CPProxy.Phone GeneratePhone(AccountInfo accountInfo)
        {
            var phone = new CPProxy.Phone
            {
                CountryCode = CorePaymentServiceExtension.GetTwoLetterCountryCode(accountInfo.Phone.CountryCode),
                PhoneExtension = accountInfo.Phone.Extension,
                PhonePrefix = accountInfo.Phone.AreaCode,
                PhoneNumber = accountInfo.Phone.Number
            };
            return phone;

        }
        private AccountInfo UpdateAccountSuccessfully(AccountInfo accountInfo, CPProxy.UpdateAccountResponse updateAccountResponse)
        {
            if (updateAccountResponse.AccountInfoOutput.AddressSet.Any())
            {
                accountInfo.Addresses = new List<Address>();
                foreach (var item in updateAccountResponse.AccountInfoOutput.AddressSet)
                {
                    accountInfo.Addresses.Add(item.ConvertCPAddressToAddress(updateAccountResponse.AccountInfoOutput.Email));
                }
            }
            return accountInfo;
        }
        //originally around 200 reduced to 100, if we use refrences and replace switch block with function 30 more lines be reduced
        private AccountInfo UpdateBillingAccount(AccountInfo accountInfo, PaymentIdentity paymentIdentity, string locale, string currency, PaymentException payex, PaymentAddressException payAdEx, CPProxy.ICommerceService cpclient, string identityValue)
        {
            try
            {
                AccountInfo account = null;
                var getIdentity = PaymentCommon.SelectIdentity(paymentIdentity);
                identityValue = getIdentity.IdentityValue;
                cpclient = GetCpClient();
                if (accountInfo.Address != null)
                {
                    var identity = GenerateIdentity(getIdentity, accountInfo);
                    var request = GenerateRequest(identity, accountInfo, locale);
                    if (accountInfo.Address != null)
                    {
                        var address = GenerateAddress(accountInfo);
                        request.AccountInfoInput.AddressSet = new[] { address };
                    }
                    if (accountInfo.Phone != null)
                    {
                        var phone = GeneratePhone(accountInfo);
                        request.AccountInfoInput.PhoneSet = new[] { phone };
                    }
                    var updateAccountResponse = cpclient.UpdateAccount(request);
                    //TODO
                    //if (_cpServiceSection.EnableUpdateAccountTrace.Value.Equals("true", StringComparison.OrdinalIgnoreCase))
                    //    LogRequestResponse("UpdateBillingAccount", updateAccountResponse, request);
                    if (updateAccountResponse.Ack == CPProxy.AckCode.Success && updateAccountResponse.AccountInfoOutput != null)
                    {
                        return UpdateAccountSuccessfully(accountInfo, updateAccountResponse);
                    }
                    if (updateAccountResponse.Ack == CPProxy.AckCode.Success && updateAccountResponse.AccountInfoOutput == null)
                    {
                        payex = new PaymentException("Could not update the Billing account");
                        payex.ExceptionCode = PaymentExceptioncode.PaymentUpdateBillingAccountResponseNull;
                    }
                    else
                        switch (updateAccountResponse.Ack)
                        {
                            case CPProxy.AckCode.InvalidInput:
                                payex = new PaymentException(PaymentCommon.GetCPExceptionMessage(updateAccountResponse));
                                payex.ExceptionCode = PaymentExceptioncode.PaymentUpdateBillingAccountInvalidInput;
                                break;

                            case CPProxy.AckCode.NonRetryableFailure:
                                if (LstAddressErrorCodes.Contains(updateAccountResponse.Error.ErrorCode))
                                {
                                    if (updateAccountResponse.Error.ErrorCode == 10021)
                                    {
                                        payAdEx = new PaymentAddressException(updateAccountResponse.Error.Detail);
                                        payAdEx.ExceptionCode = PaymentExceptioncode.InvalidZipCode;
                                    }
                                    else
                                    {
                                        payAdEx = new PaymentAddressException("Invalid Address");
                                        payAdEx.ExceptionCode = PaymentExceptioncode.InvalidAddress;
                                    }
                                }
                                else
                                {
                                    payex = new PaymentException(PaymentCommon.GetCPExceptionMessage(updateAccountResponse));
                                    payex.ExceptionCode = PaymentExceptioncode.PaymentUpdateBillingAccountNonRetryableFailure;
                                }
                                break;

                            case CPProxy.AckCode.RetryableFailure:
                                payex = new PaymentException(PaymentCommon.GetCPExceptionMessage(updateAccountResponse));
                                payex.ExceptionCode = PaymentExceptioncode.PaymentUpdateBillingAccountRetryableFailure;
                                break;
                        }
                }
                if (payex != null)
                {
                    var context = this.GetCurrentMethodContext(accountInfo, identityValue, locale, currency);
                    payex.PaymentServiceLibraryContext = context;
                    throw payex;
                }
            }
            catch (Exception exception)
            {
                //TODO
                var context = this.GetCurrentMethodContext(accountInfo, identityValue, locale, currency);
                payex = new PaymentException(exception.Message, PaymentExceptioncode.GenericPaymentException, exception);
                payex.HandleException(exception, context);
            }
            finally
            {
                PaymentCommon.CloseOrAbortProxy(cpclient);
            }
            return null;
        }

    }

}




