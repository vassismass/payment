using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.SupplyChain.Care.Payment.ServiceLibrary.Exceptions;
using Microsoft.SupplyChain.Care.Payment.ServiceLibrary.Fakes;
using Microsoft.SupplyChain.Care.Payment.TestTax;
using Microsoft.SupplyChain.Care.PaymentModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SettlePayment;
using SettlePayment.Exceptions;
using SettlePayment.Fakes;
using System;
using System.Collections.Generic;

namespace PaymentUnitTest
{
    [TestClass]
    public class CPPaymentProviderUnitTest
    {


        [TestMethod]
        [ExpectedException(typeof(PaymentProviderException))]
        public void PurchaseGoods_Test_PurchaseGoodsRequestValidation()
        {
            var getConfigurationStub = new StubIGetConfiguration();
            var serviceAgentStub = new StubIServiceAgent();
            var billingServiceStub = new StubIBillingServices();

            PurchaseGoodsRequest request = null;
            Dictionary<string, string> Configuration = new Dictionary<string, string>();
            Configuration["partnerGuid"] = "11112222-3333-4444-5555-666677778888";
            Configuration["productGuid"] = "11112222-3333-4444-5555-666677778888";
            Configuration["isZipCodeIncludedInCpComparision"] = "false";
            Configuration["shipFromPath"] = "yhtyrhty";
            Configuration["datacenterCurrent"] = "qwedge";
            Configuration["geographyPath"] = "C:\\Users\\t-pugoe\\Documents\\My_Project\\Payment_Final\\PaymentServiceLibrary\\Geography\\Geography.xml";
            MockedGetConfiguration config = new MockedGetConfiguration()
            {
                GetConfigFunc = () =>
                {
                    PaymentConfiguration mockedConfig = new PaymentConfiguration { Config = Configuration };
                    return mockedConfig;
                }
            };
            var cpPaymentProvider = new CPPaymentProvider(config, serviceAgentStub, billingServiceStub);

            using (ShimsContext.Create()) {
                ShimCPServiceValidator.ValidatePurchaseGoodsPurchaseGoodsRequestValidationExceptionRef = (PurchaseGoodsRequest requestParam, ref ValidationException fault) => { return false; };
                cpPaymentProvider.PurchaseGoods(null);
            }
        }

        [TestMethod]
        public void PurchaseGoods_Test_PurchaseResponse()
        {
            var getConfigurationStub = new StubIGetConfiguration();
            var serviceAgentStub = new StubIServiceAgent();
            var billingServiceStub = new StubIBillingServices();

            Dictionary<string, string> Configuration = new Dictionary<string, string>();
            Configuration["partnerGuid"] = "11112222-3333-4444-5555-666677778888";
            Configuration["productGuid"] = "11112222-3333-4444-5555-666677778888";
            Configuration["isZipCodeIncludedInCpComparision"] = "false";
            Configuration["shipFromPath"] = "yhtyrhty";
            Configuration["datacenterCurrent"] = "qwedge";
            Configuration["geographyPath"] = "C:\\Users\\t-pugoe\\Documents\\My_Project\\Payment_Final\\PaymentServiceLibrary\\Geography\\Geography.xml";
            MockedGetConfiguration config = new MockedGetConfiguration()
            {
                GetConfigFunc = () =>
                {
                    PaymentConfiguration mockedConfig = new PaymentConfiguration { Config = Configuration };
                    return mockedConfig;
                }
            };

            Dictionary<string, string> fraudDetectionProperty  = new Dictionary<string, string>();
            fraudDetectionProperty["vass"] = "mass";
            Address addr = new Address { Country = "AdUT", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer" };
            List<Address> addresslst = new List<Address>();
            addresslst.Add(addr);
            AccountInfo accInf = new AccountInfo { Addresses = addresslst };
            PaymentIdentity paymentidentity = new PaymentIdentity { PaymentInstrumentId = "egherhe", AccountId = "THrt", GuestId = "RFGEr", Puid = "fgr" };
            LineItem skuInfo = new LineItem { Sku = "ER", PaymentSku = "ERFER", Description = "FG", PriceInfo = new Price { TaxCode = "ef", Amount = 213, Currency = "dg" }, AddressInfo = new Address { Country = "AdUT", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer" } };
            var cpPaymentProvider = new CPPaymentProvider(config, serviceAgentStub, billingServiceStub);
            PurchaseGoodsRequest request = new PurchaseGoodsRequest { PurchaseType = "vass", TrackingGuid = new Guid(),
                paymentIdentityInfo = paymentidentity, Country = "AUT", Account = accInf, SerialNumber="1456666", Locale = "joke", Currency = "rupee",
                        SKUInfo = skuInfo, FraudDetectionProperty =fraudDetectionProperty };

          //  var response = new PurchaseGoodsResponse { OrderStatus = default(string), Order = new Order { Tax=default(decimal) }, LineItemTax = default(decimal), ShippmentTax = default(decimal), };

            billingServiceStub.GetBillingAccountStringPaymentIdentity = (a, b) =>
            {
                return accInf;
            };
            billingServiceStub.UpdateBillingAccountAccountInfoPaymentIdentityStringString = (a, b , c , d) =>
            {
                return accInf;
            };
            var response =  new PurchaseGoodsResponse();
            using (ShimsContext.Create())
            {
                ShimCPServiceValidator.ValidatePurchaseGoodsPurchaseGoodsRequestValidationExceptionRef = (PurchaseGoodsRequest requestParam, ref ValidationException fault) => { return true; };
 
                response = cpPaymentProvider.PurchaseGoods(request);
            }
            Assert.AreEqual(response.PurchaseId, default(string));
            Assert.AreEqual(response.LineItemTax, default(decimal));
            Assert.AreEqual(response.ShippmentTax, default(decimal));
            Assert.AreEqual(response.OrderStatus, default(string));
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void PurchaseGoods_Test_ValidateGetAddressId()
        {
            var getConfigurationStub = new StubIGetConfiguration();
            var serviceAgentStub = new StubIServiceAgent();
            var billingServiceStub = new StubIBillingServices();

            Dictionary<string, string> Configuration = new Dictionary<string, string>();
            Configuration["partnerGuid"] = "11112222-3333-4444-5555-666677778888";
            Configuration["productGuid"] = "11112222-3333-4444-5555-666677778888";
            Configuration["isZipCodeIncludedInCpComparision"] = "hodor";
            Configuration["shipFromPath"] = "yhtyrhty";
            Configuration["datacenterCurrent"] = "qwedge";
            Configuration["geographyPath"] = "C:\\Users\\t-pugoe\\Documents\\My_Project\\Payment_Final\\PaymentServiceLibrary\\Geography\\Geography.xml";
            MockedGetConfiguration config = new MockedGetConfiguration()
            {
                GetConfigFunc = () =>
                {
                    PaymentConfiguration mockedConfig = new PaymentConfiguration { Config = Configuration };
                    return mockedConfig;
                }
            };

            Dictionary<string, string> fraudDetectionProperty = new Dictionary<string, string>();
            fraudDetectionProperty["vass"] = "mass";
            Address addr = new Address { Country = "AdUT", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer" };
            List<Address> addresslst = new List<Address>();
            addresslst.Add(addr);
            AccountInfo accInf = new AccountInfo { Addresses = addresslst };
            PaymentIdentity paymentidentity = new PaymentIdentity { PaymentInstrumentId = "egherhe", AccountId = "THrt", GuestId = "RFGEr", Puid = "fgr" };
            LineItem skuInfo = new LineItem { Sku = "ER", PaymentSku = "ERFER", Description = "FG", PriceInfo = new Price { TaxCode = "ef", Amount = 213, Currency = "dg" }, AddressInfo = new Address { Country = "AdUT22", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer" } };
            var cpPaymentProvider = new CPPaymentProvider(config, serviceAgentStub, billingServiceStub);
            PurchaseGoodsRequest request = new PurchaseGoodsRequest
            {
                PurchaseType = "vass",
                TrackingGuid = new Guid(),
                paymentIdentityInfo = paymentidentity,
                Country = "AUT",
                Account = accInf,
                SerialNumber = "1456666",
                Locale = "joke",
                Currency = "rupee",
                SKUInfo = skuInfo,
                FraudDetectionProperty = fraudDetectionProperty
            };

            billingServiceStub.GetBillingAccountStringPaymentIdentity = (a, b) =>
            {
                return accInf;
            };
            billingServiceStub.UpdateBillingAccountAccountInfoPaymentIdentityStringString = (a, b, c, d) =>
            {
                return accInf;
            };
            var response = new PurchaseGoodsResponse();
            using (ShimsContext.Create())
            {
                ShimCPServiceValidator.ValidatePurchaseGoodsPurchaseGoodsRequestValidationExceptionRef = (PurchaseGoodsRequest requestParam, ref ValidationException fault) => { return true; };

                response=cpPaymentProvider.PurchaseGoods(request);
            }
        }

        [TestMethod]
        public void PurchaseGoods_Test_Val()
        {
            var getConfigurationStub = new StubIGetConfiguration();
            var serviceAgentStub = new StubIServiceAgent();
            var billingServiceStub = new StubIBillingServices();

            Dictionary<string, string> Configuration = new Dictionary<string, string>();
            Configuration["partnerGuid"] = "11112222-3333-4444-5555-666677778888";
            Configuration["productGuid"] = "11112222-3333-4444-5555-666677778888";
            Configuration["isZipCodeIncludedInCpComparision"] = "false";
            Configuration["shipFromPath"] = "yhtyrhty";
            Configuration["datacenterCurrent"] = "qwedge";
            Configuration["geographyPath"] = "C:\\Users\\t-pugoe\\Documents\\My_Project\\Payment_Final\\PaymentServiceLibrary\\Geography\\Geography.xml";
            MockedGetConfiguration config = new MockedGetConfiguration()
            {
                GetConfigFunc = () =>
                {
                    PaymentConfiguration mockedConfig = new PaymentConfiguration { Config = Configuration };
                    return mockedConfig;
                }
            };

            Dictionary<string, string> fraudDetectionProperty = new Dictionary<string, string>();
            fraudDetectionProperty["vass"] = "mass";
            Address addr = new Address { Country = "AdUT", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer" };
            List<Address> addresslst = new List<Address>();
            addresslst.Add(addr);
            AccountInfo accInf = new AccountInfo { Address= addr , Addresses = addresslst };
            PaymentIdentity paymentidentity = new PaymentIdentity { PaymentInstrumentId = "egherhe", AccountId = "THrt", GuestId = "RFGEr", Puid = "fgr" };
            LineItem skuInfo = new LineItem { Sku = "ER", PaymentSku = "ERFER", Description = "FG", PriceInfo = new Price { TaxCode = "ef", Amount = 213, Currency = "dg" }, AddressInfo = new Address { Country = "AdUT22", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer" } };
            var cpPaymentProvider = new CPPaymentProvider(config, serviceAgentStub, billingServiceStub);
            PurchaseGoodsRequest request = new PurchaseGoodsRequest
            {
                PurchaseType = "vass",
                TrackingGuid = new Guid(),
                paymentIdentityInfo = paymentidentity,
                Country = "AUT",
                Account = accInf,
                SerialNumber = "1456666",
                Locale = "joke",
                Currency = "rupee",
                SKUInfo = skuInfo,
                FraudDetectionProperty = fraudDetectionProperty
            };

            billingServiceStub.GetBillingAccountStringPaymentIdentity = (a, b) =>
            {
                return accInf;
            };
            billingServiceStub.UpdateBillingAccountAccountInfoPaymentIdentityStringString = (a, b, c, d) =>
            {
                return accInf;
            };
            var response = new PurchaseGoodsResponse();
            using (ShimsContext.Create())
            {
                ShimCPServiceValidator.ValidatePurchaseGoodsPurchaseGoodsRequestValidationExceptionRef = (PurchaseGoodsRequest requestParam, ref ValidationException fault) => { return true; };

                response = cpPaymentProvider.PurchaseGoods(request);
            }
            Assert.AreEqual(response.PurchaseId, default(string));
            Assert.AreEqual(response.LineItemTax, default(decimal));
            Assert.AreEqual(response.ShippmentTax, default(decimal));
            Assert.AreEqual(response.OrderStatus, default(string));
        }
        [TestMethod]
        [ExpectedException(typeof(AddressException))]
        public void PurchaseGoods_Test_ValidateGetAddresswithAddress()
        {
            var getConfigurationStub = new StubIGetConfiguration();
            var serviceAgentStub = new StubIServiceAgent();
            var billingServiceStub = new StubIBillingServices();

            Dictionary<string, string> Configuration = new Dictionary<string, string>();
            Configuration["partnerGuid"] = "11112222-3333-4444-5555-666677778888";
            Configuration["productGuid"] = "11112222-3333-4444-5555-666677778888";
            Configuration["isZipCodeIncludedInCpComparision"] = "true";
            Configuration["shipFromPath"] = "yhtyrhty";
            Configuration["datacenterCurrent"] = "qwedge";
            Configuration["geographyPath"] = "C:\\Users\\t-pugoe\\Documents\\My_Project\\Payment_Final\\PaymentServiceLibrary\\Geography\\Geography.xml";
            MockedGetConfiguration config = new MockedGetConfiguration()
            {
                GetConfigFunc = () =>
                {
                    PaymentConfiguration mockedConfig = new PaymentConfiguration { Config = Configuration };
                    return mockedConfig;
                }
            };

            Dictionary<string, string> fraudDetectionProperty = new Dictionary<string, string>();
            fraudDetectionProperty["vass"] = "mass";
            Address addr = new Address { Country = "AdUT", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer" };
            List<Address> addresslst = new List<Address>();
            addresslst.Add(addr);
            AccountInfo accInf = new AccountInfo { Address = addr, Addresses = addresslst };
            PaymentIdentity paymentidentity = new PaymentIdentity { PaymentInstrumentId = "egherhe", AccountId = "THrt", GuestId = "RFGEr", Puid = "fgr" };
            LineItem skuInfo = new LineItem { Sku = "ER", PaymentSku = "ERFER", Description = "FG", PriceInfo = new Price { TaxCode = "ef", Amount = 213, Currency = "dg" }, AddressInfo = new Address { Country = "AdUT22", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer" } };
            var cpPaymentProvider = new CPPaymentProvider(config, serviceAgentStub, billingServiceStub);
            PurchaseGoodsRequest request = new PurchaseGoodsRequest
            {
                PurchaseType = "vass",
                TrackingGuid = new Guid(),
                paymentIdentityInfo = paymentidentity,
                Country = "AUT",
                Account = accInf,
                SerialNumber = "1456666",
                Locale = "joke",
                Currency = "rupee",
                SKUInfo = skuInfo,
                FraudDetectionProperty = fraudDetectionProperty
            };

            billingServiceStub.GetBillingAccountStringPaymentIdentity = (a, b) =>
            {
                return accInf;
            };
            billingServiceStub.UpdateBillingAccountAccountInfoPaymentIdentityStringString = (a, b, c, d) =>
            {
                return accInf;
            };
            var response = new PurchaseGoodsResponse();
            using (ShimsContext.Create())
            {
                ShimCPServiceValidator.ValidatePurchaseGoodsPurchaseGoodsRequestValidationExceptionRef = (PurchaseGoodsRequest requestParam, ref ValidationException fault) => { return true; };

                response = cpPaymentProvider.PurchaseGoods(request);
            }
        }


        [TestMethod]
        [ExpectedException(typeof(AddressException))]
        public void PurchaseGoods_Test_ValidateGetAddresswithAddresseithZipCode()
        {
            var getConfigurationStub = new StubIGetConfiguration();
            var serviceAgentStub = new StubIServiceAgent();
            var billingServiceStub = new StubIBillingServices();

            Dictionary<string, string> Configuration = new Dictionary<string, string>();
            Configuration["partnerGuid"] = "11112222-3333-4444-5555-666677778888";
            Configuration["productGuid"] = "11112222-3333-4444-5555-666677778888";
            Configuration["isZipCodeIncludedInCpComparision"] ="true";
            Configuration["shipFromPath"] = "yhtyrhty";
            Configuration["datacenterCurrent"] = "qwedge";
            Configuration["geographyPath"] = "C:\\Users\\t-pugoe\\Documents\\My_Project\\Payment_Final\\PaymentServiceLibrary\\Geography\\Geography.xml";
            MockedGetConfiguration config = new MockedGetConfiguration()
            {
                GetConfigFunc = () =>
                {
                    PaymentConfiguration mockedConfig = new PaymentConfiguration { Config = Configuration };
                    return mockedConfig;
                }
            };

            Dictionary<string, string> fraudDetectionProperty = new Dictionary<string, string>();
            fraudDetectionProperty["vass"] = "mass";
            Address addr = new Address { Country = "AdUT", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer" };
            List<Address> addresslst = new List<Address>();
            addresslst.Add(addr);
            AccountInfo accInf = new AccountInfo { Address = addr, Addresses = addresslst };
            PaymentIdentity paymentidentity = new PaymentIdentity { PaymentInstrumentId = "egherhe", AccountId = "THrt", GuestId = "RFGEr", Puid = "fgr" };
            LineItem skuInfo = new LineItem { Sku = "ER", PaymentSku = "ERFER", Description = "FG", PriceInfo = new Price { TaxCode = "ef", Amount = 213, Currency = "dg" }, AddressInfo = new Address { Country = "AdUT22", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer" } };
            var cpPaymentProvider = new CPPaymentProvider(config, serviceAgentStub, billingServiceStub);
            PurchaseGoodsRequest request = new PurchaseGoodsRequest
            {
                PurchaseType = "vass",
                TrackingGuid = new Guid(),
                paymentIdentityInfo = paymentidentity,
                Country = "AUT",
                Account = accInf,
                SerialNumber = "1456666",
                Locale = "joke",
                Currency = "rupee",
                SKUInfo = skuInfo,
                FraudDetectionProperty = fraudDetectionProperty
            };

            billingServiceStub.GetBillingAccountStringPaymentIdentity = (a, b) =>
            {
                return accInf;
            };
            billingServiceStub.UpdateBillingAccountAccountInfoPaymentIdentityStringString = (a, b, c, d) =>
            {
                return accInf;
            };
            var response = new PurchaseGoodsResponse();
            using (ShimsContext.Create())
            {
                ShimCPServiceValidator.ValidatePurchaseGoodsPurchaseGoodsRequestValidationExceptionRef = (PurchaseGoodsRequest requestParam, ref ValidationException fault) => { return true; };

                response = cpPaymentProvider.PurchaseGoods(request);
            }

        }

        [TestMethod]
        public void PurchaseGoods_Test_ValidateGetAddresswithAddresseithZipCodeandAlternateAddress()
        {
            var getConfigurationStub = new StubIGetConfiguration();
            var serviceAgentStub = new StubIServiceAgent();
            var billingServiceStub = new StubIBillingServices();

            Dictionary<string, string> Configuration = new Dictionary<string, string>();
            Configuration["partnerGuid"] = "11112222-3333-4444-5555-666677778888";
            Configuration["productGuid"] = "11112222-3333-4444-5555-666677778888";
            Configuration["isZipCodeIncludedInCpComparision"] = "true";
            Configuration["shipFromPath"] = "yhtyrhty";
            Configuration["datacenterCurrent"] = "qwedge";
            Configuration["geographyPath"] = "C:\\Users\\t-pugoe\\Documents\\My_Project\\Payment_Final\\PaymentServiceLibrary\\Geography\\Geography.xml";
            MockedGetConfiguration config = new MockedGetConfiguration()
            {
                GetConfigFunc = () =>
                {
                    PaymentConfiguration mockedConfig = new PaymentConfiguration { Config = Configuration };
                    return mockedConfig;
                }
            };

            Dictionary<string, string> fraudDetectionProperty = new Dictionary<string, string>();
            fraudDetectionProperty["vass"] = "mass";
            Address addr = new Address { Country = "AdUT", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer" , FriendlyName = "RahulKumar"};
            List <Address> addresslst = new List<Address>();
            addresslst.Add(addr);
            AccountInfo accInf = new AccountInfo { Address = addr, Addresses = addresslst };
            PaymentIdentity paymentidentity = new PaymentIdentity { PaymentInstrumentId = "egherhe", AccountId = "THrt", GuestId = "RFGEr", Puid = "fgr" };
            LineItem skuInfo = new LineItem { Sku = "ER", PaymentSku = "ERFER", Description = "FG", PriceInfo = new Price { TaxCode = "ef", Amount = 213, Currency = "dg" }, AddressInfo = new Address { Country = "AdUT22", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer" } };
            var cpPaymentProvider = new CPPaymentProvider(config, serviceAgentStub, billingServiceStub);
            Address addr2 = new Address { Country = "AdUT22", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer" , FriendlyName = "RahulKumar" };
            addresslst = new List<Address>();
            addresslst.Add(addr2);
            AccountInfo accInf2 = new AccountInfo { Address = addr2 , Addresses = addresslst};
            PurchaseGoodsRequest request = new PurchaseGoodsRequest
            {
                PurchaseType = "vass",
                TrackingGuid = new Guid(),
                paymentIdentityInfo = paymentidentity,
                Country = "AUT",
                Account = accInf,
                SerialNumber = "1456666",
                Locale = "joke",
                Currency = "rupee",
                SKUInfo = skuInfo,
                FraudDetectionProperty = fraudDetectionProperty
            };

            billingServiceStub.GetBillingAccountStringPaymentIdentity = (a, b) =>
            {
                return accInf;
            };
            billingServiceStub.UpdateBillingAccountAccountInfoPaymentIdentityStringString = (a, b, c, d) =>
            {
                return accInf2;
            };
            var response = new PurchaseGoodsResponse();
            using (ShimsContext.Create())
            {
                ShimCPServiceValidator.ValidatePurchaseGoodsPurchaseGoodsRequestValidationExceptionRef = (PurchaseGoodsRequest requestParam, ref ValidationException fault) => { return true; };

                response = cpPaymentProvider.PurchaseGoods(request);
            }
            Assert.AreEqual(response.PurchaseId, default(string));
            Assert.AreEqual(response.LineItemTax, default(decimal));
            Assert.AreEqual(response.ShippmentTax, default(decimal));
            Assert.AreEqual(response.OrderStatus, default(string));

        }

        [TestMethod]
        public void PurchaseGoods_Test_ValidateGetAddresswithAddresseithZipCodeStatusFalseandAlternateAddress()
        {
            var getConfigurationStub = new StubIGetConfiguration();
            var serviceAgentStub = new StubIServiceAgent();
            var billingServiceStub = new StubIBillingServices();

            Dictionary<string, string> Configuration = new Dictionary<string, string>();
            Configuration["partnerGuid"] = "11112222-3333-4444-5555-666677778888";
            Configuration["productGuid"] = "11112222-3333-4444-5555-666677778888";
            Configuration["isZipCodeIncludedInCpComparision"] = "true";
            Configuration["shipFromPath"] = "yhtyrhty";
            Configuration["datacenterCurrent"] = "qwedge";
            Configuration["geographyPath"] = "C:\\Users\\t-pugoe\\Documents\\My_Project\\Payment_Final\\PaymentServiceLibrary\\Geography\\Geography.xml";
            MockedGetConfiguration config = new MockedGetConfiguration()
            {
                GetConfigFunc = () =>
                {
                    PaymentConfiguration mockedConfig = new PaymentConfiguration { Config = Configuration };
                    return mockedConfig;
                }
            };

            Dictionary<string, string> fraudDetectionProperty = new Dictionary<string, string>();
            fraudDetectionProperty["vass"] = "mass";
            Address addr = new Address { Country = "AdUT", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer", FriendlyName = "RahulKumar" };
            List<Address> addresslst = new List<Address>();
            addresslst.Add(addr);
            AccountInfo accInf = new AccountInfo { Address = addr, Addresses = addresslst };
            PaymentIdentity paymentidentity = new PaymentIdentity { PaymentInstrumentId = "egherhe", AccountId = "THrt", GuestId = "RFGEr", Puid = "fgr" };
            LineItem skuInfo = new LineItem { Sku = "ER", PaymentSku = "ERFER", Description = "FG", PriceInfo = new Price { TaxCode = "ef", Amount = 213, Currency = "dg" }, AddressInfo = new Address { Country = "AdUT22", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer" } };
            var cpPaymentProvider = new CPPaymentProvider(config, serviceAgentStub, billingServiceStub);
            Address addr2 = new Address { Country = "AdUT22", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer", FriendlyName = "RahulKumar" };
            addresslst = new List<Address>();
            addresslst.Add(addr2);
            AccountInfo accInf2 = new AccountInfo { Address = addr2, Addresses = addresslst };
            PurchaseGoodsRequest request = new PurchaseGoodsRequest
            {
                PurchaseType = "vass",
                TrackingGuid = new Guid(),
                paymentIdentityInfo = paymentidentity,
                Country = "AUT",
                Account = accInf,
                SerialNumber = "1456666",
                Locale = "joke",
                Currency = "rupee",
                SKUInfo = skuInfo,
                FraudDetectionProperty = fraudDetectionProperty
            };

            billingServiceStub.GetBillingAccountStringPaymentIdentity = (a, b) =>
            {
                return accInf;
            };
            billingServiceStub.UpdateBillingAccountAccountInfoPaymentIdentityStringString = (a, b, c, d) =>
            {
                return accInf2;
            };
            var response = new PurchaseGoodsResponse();
            using (ShimsContext.Create())
            {
                ShimCPServiceValidator.ValidatePurchaseGoodsPurchaseGoodsRequestValidationExceptionRef = (PurchaseGoodsRequest requestParam, ref ValidationException fault) => { return true; };

                response = cpPaymentProvider.PurchaseGoods(request);
            }
            Assert.AreEqual(response.PurchaseId, default(string));
            Assert.AreEqual(response.LineItemTax, default(decimal));
            Assert.AreEqual(response.ShippmentTax, default(decimal));
            Assert.AreEqual(response.OrderStatus, default(string));

        }

        [TestMethod]
        [ExpectedException(typeof(PaymentException))]
        public void PurchaseGoods_Test_ValidateAuthorizeDigitalPurchase()
        {
            var getConfigurationStub = new StubIGetConfiguration();
            var serviceAgentStub = new StubIServiceAgent();
            var billingServiceStub = new StubIBillingServices();

            Dictionary<string, string> Configuration = new Dictionary<string, string>();
            Configuration["partnerGuid"] = "11112222-3333-4444-5555-666677778888";
            Configuration["productGuid"] = "11112222-3333-4444-5555-666677778888";
            Configuration["isZipCodeIncludedInCpComparision"] = "false";
            Configuration["shipFromPath"] = "yhtyrhty";
            Configuration["datacenterCurrent"] = "qwedge";
            Configuration["geographyPath"] = "C:\\Users\\t-pugoe\\Documents\\My_Project\\Payment_Final\\PaymentServiceLibrary\\Geography\\Geography.xml";
            MockedGetConfiguration config = new MockedGetConfiguration()
            {
                GetConfigFunc = () =>
                {
                    PaymentConfiguration mockedConfig = new PaymentConfiguration { Config = Configuration };
                    return mockedConfig;
                }
            };

            Dictionary<string, string> fraudDetectionProperty = new Dictionary<string, string>();
            fraudDetectionProperty["vass"] = "mass";
            Address addr = new Address { Country = "AdUT", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer", FriendlyName = "RahulKumar" };
            List<Address> addresslst = new List<Address>();
            addresslst.Add(addr);
            AccountInfo accInf = new AccountInfo { Address = addr, Addresses = addresslst };
            PaymentIdentity paymentidentity = new PaymentIdentity { PaymentInstrumentId = "egherhe", AccountId = "THrt", GuestId = "RFGEr", Puid = "fgr" };
            LineItem skuInfo = new LineItem { Sku = "ER", PaymentSku = "ERFER", Description = "FG", PriceInfo = new Price { TaxCode = "ef", Amount = 213, Currency = "dg" }, AddressInfo = new Address { Country = "AdUT", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer" } };
            var cpPaymentProvider = new CPPaymentProvider(config, serviceAgentStub, billingServiceStub);
            Address addr2 = new Address { Country = "AdUT22", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer", FriendlyName = "RahulKumar" };
            addresslst = new List<Address>();
            addresslst.Add(addr2);
            AccountInfo accInf2 = new AccountInfo { Address = addr2, Addresses = addresslst };
            PurchaseGoodsRequest request = new PurchaseGoodsRequest
            {
                PurchaseType = "digital",
                TrackingGuid = new Guid(),
                paymentIdentityInfo = paymentidentity,
                Country = "AUT",
                Account = accInf,
                SerialNumber = "1456666",
                Locale = "joke",
                Currency = "rupee",
                SKUInfo = skuInfo,
                FraudDetectionProperty = fraudDetectionProperty
            };

            billingServiceStub.GetBillingAccountStringPaymentIdentity = (a, b) =>
            {
                return accInf;
            };
            billingServiceStub.UpdateBillingAccountAccountInfoPaymentIdentityStringString = (a, b, c, d) =>
            {
                return accInf2;
            };
            var response = new PurchaseGoodsResponse();
            using (ShimsContext.Create())
            {
                ShimCPServiceValidator.ValidatePurchaseGoodsPurchaseGoodsRequestValidationExceptionRef = (PurchaseGoodsRequest requestParam, ref ValidationException fault) => { return true; };

                response = cpPaymentProvider.PurchaseGoods(request);
            }
            Assert.AreEqual(response.PurchaseId, default(string));
            Assert.AreEqual(response.LineItemTax, default(decimal));
            Assert.AreEqual(response.ShippmentTax, default(decimal));
            Assert.AreEqual(response.OrderStatus, default(string));

        }

        [TestMethod]
        [ExpectedException(typeof(PaymentProviderException))]
        public void PurchaseGoods_Test_ValidateAuthorizeDigitalPurchaseReturnstatus()
        {
            var getConfigurationStub = new StubIGetConfiguration();
            var serviceAgentStub = new StubIServiceAgent();
            var billingServiceStub = new StubIBillingServices();

            Dictionary<string, string> Configuration = new Dictionary<string, string>();
            Configuration["partnerGuid"] = "11112222-3333-4444-5555-666677778888";
            Configuration["productGuid"] = "11112222-3333-4444-5555-666677778888";
            Configuration["isZipCodeIncludedInCpComparision"] = "false";
            Configuration["shipFromPath"] = "yhtyrhty";
            Configuration["datacenterCurrent"] = "qwedge";
            Configuration["geographyPath"] = "C:\\Users\\t-pugoe\\Documents\\My_Project\\Payment_Final\\PaymentServiceLibrary\\Geography\\Geography.xml";
            MockedGetConfiguration config = new MockedGetConfiguration()
            {
                GetConfigFunc = () =>
                {
                    PaymentConfiguration mockedConfig = new PaymentConfiguration { Config = Configuration };
                    return mockedConfig;
                }
            };

            Dictionary<string, string> fraudDetectionProperty = new Dictionary<string, string>();
            fraudDetectionProperty["vass"] = "mass";
            Address addr = new Address { Country = "AdUT", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer", FriendlyName = "RahulKumar" };
            List<Address> addresslst = new List<Address>();
            addresslst.Add(addr);
            AccountInfo accInf = new AccountInfo { Address = addr, Addresses = addresslst };
            PaymentIdentity paymentidentity = new PaymentIdentity { PaymentInstrumentId = "egherhe", AccountId = "THrt", GuestId = "RFGEr", Puid = "fgr" };
            LineItem skuInfo = new LineItem { Sku = "ER", PaymentSku = "ERFER", Description = "FG", PriceInfo = new Price { TaxCode = "ef", Amount = 213, Currency = "dg" }, AddressInfo = new Address { Country = "AdUT", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer" } };
            var cpPaymentProvider = new CPPaymentProvider(config, serviceAgentStub, billingServiceStub);
            Address addr2 = new Address { Country = "AdUT22", FirstName = "Rahul", LastName = "Kumar", AddressId = "EGFer", FriendlyName = "RahulKumar" };
            addresslst = new List<Address>();
            addresslst.Add(addr2);
            AccountInfo accInf2 = new AccountInfo { Address = addr2, Addresses = addresslst };
            PurchaseGoodsRequest request = new PurchaseGoodsRequest
            {
                PurchaseType = "digital",
                TrackingGuid = new Guid(),
                paymentIdentityInfo = paymentidentity,
                Country = "AUT",
                Account = accInf,
                SerialNumber = "1456666",
                Locale = "joke",
                Currency = "rupee",
                SKUInfo = skuInfo,
                FraudDetectionProperty = fraudDetectionProperty
            };

            billingServiceStub.GetBillingAccountStringPaymentIdentity = (a, b) =>
            {
                return accInf;
            };
            billingServiceStub.UpdateBillingAccountAccountInfoPaymentIdentityStringString = (a, b, c, d) =>
            {
                return accInf2;
            };
            var response = new PurchaseGoodsResponse();
            using (ShimsContext.Create())
            {
                ShimCPServiceValidator.ValidatePurchaseGoodsPurchaseGoodsRequestValidationExceptionRef = (PurchaseGoodsRequest requestParam, ref ValidationException fault) => { return true; };

                response = cpPaymentProvider.PurchaseGoods(request);
            }

        }
        //Dummy test for reference, please delete it.
        /* [TestMethod]
         public void PurchaseGoodsRequest_Validate_PurchaseGoods2()
         {
             var serviceAgentStub = new StubCPServiceAgent(new Guid(), new Guid(), true, true, string.Empty, string.Empty,string.Empty);
             var billingServiceStub = new StubIBillingService();

             var cpPaymentProvider = new CPPaymentProvider(serviceAgentStub, billingServiceStub);
             PurchaseGoodsRequest request = null;

             try
             {
                 using (ShimsContext.Create())
                 {
                     ShimCPServiceValidator.ValidatePurchaseGoodsPurchaseGoodsRequestValidationExceptionRef = (PurchaseGoodsRequest requestParam, ref ValidationException fault) => { return false; };
                     cpPaymentProvider.PurchaseGoods(null);
                 }
             }
             catch(Exception ex)
             {
                 Assert.IsTrue(ex is PaymentProviderException);
                 Assert.AreEqual(ex.Message, "ankna");
             }
         }

         /*[TestMethod]
         public void PurchaseGoodsRequest_Validate_PurchaseGoods()
         {
             var serviceAgentStub = new StubCPServiceAgent(new Guid(), new Guid(), true, true, string.Empty, string.Empty, string.Empty)
             {

             }
             var billingServiceStub = new StubIBillingService
             {
                 GetBillingAccountStringPaymentIdentity = (a,b) => { return null; }
             };
             var cpPaymentProvider = new CPPaymentProvider(serviceAgentStub,billingServiceStub);
             PurchaseGoodsRequest request = null;

         }
         */
    }
}
