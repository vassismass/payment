using Microsoft.SupplyChain.Care.PaymentModels;
using CPProxy = Microsoft.SupplyChain.Care.Payment.ServiceLibrary.CP;
using Microsoft.SupplyChain.Care.Payment.ServiceLibrary.CP;
using Microsoft.SupplyChain.Care.Payment.ServiceLibrary.ExtDependencies;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Configuration;

namespace Microsoft.SupplyChain.Care.Payment.ServiceLibrary
{
    public static class CorePaymentServiceExtension
    {
        // readonly static PaymentProvider paymentProvider = new CPPaymentProvider();

        internal static PaymentModels.Address ConvertCPAddressToAddress(this CPProxy.Address addressIn, string email)
        {
            if (addressIn == null) return null;
            return new PaymentModels.Address
            {
                AddressId = addressIn.AddressId,
                FirstName = addressIn.FirstName,
                LastName = addressIn.LastName,
                FriendlyName = addressIn.FriendlyName,
                Email = email,
                City = addressIn.City,
                Country = GetCountryFromCountryCode(addressIn.CountryCode),
                Zip = addressIn.PostalCode,
                State = addressIn.State,
                Address1 = addressIn.Street1,
                Address2 = addressIn.Street2,
                Address3 = addressIn.Street3,
                UnitNumber = addressIn.UnitNumber,
            };
        }

        //newly added

        public static PurchaseContentRequest ConvertToPurchaseContentRequest(this LineItem lineItem, string dataCenter, Guid partnerGuid)
        {
            var lineitemdescription = string.IsNullOrEmpty(lineItem.Description) ? lineItem.Description : ((lineItem.Description.Length > 128) ? lineItem.Description.Trim().Substring(0, 128) : lineItem.Description);
            var title = string.IsNullOrEmpty(lineitemdescription) ? lineitemdescription : ((lineitemdescription.Length > 63) ? lineitemdescription.Substring(0, 63) : lineitemdescription);


            var lstProp = new List<Property>();

            var prop = new Property
            {
                Name = "Datacentre",
                Namespace = "SOMExtendedService",
                Value = dataCenter
            };
            lstProp.Add(prop);

            var purchaseRequest = new PurchaseContentRequest
            {
                AccountContext = new AccountContext()
                {
                    CountryCode = CorePaymentServiceExtension.GetTwoLetterCountryCode(lineItem.AddressInfo.Country),
                    CustomerType = CustomerType.Personal
                },

            };

            var digitalItems = new List<PurchaseContentInput>();
            var item = new PurchaseContentInput
            {

                ContentCatalogInfo = new ContentCatalogInfo()
                {
                    ContentCategory = "Service",
                    EntitlementType = "Durable",
                    ExternalParentProductItemId = lineItem.Sku,
                    LicenseType = "Full",
                    Quantity = 1
                },

                SalesModel = SalesModelType.Reseller,
                Description = lineitemdescription,
                ExternalProductItemId = lineItem.Sku,
                Title = title,
                PriceInfo = new PriceInfo
                {
                    Amount = lineItem.PriceInfo.Amount,
                    Currency = lineItem.PriceInfo.Currency,
                    //TaxCode = "online",
                    TaxCode = lineItem.PriceInfo.TaxCode,
                    IsTaxExemptionAllowed = false,
                    IsTaxIncluded = false
                },
                RevenueInfo = new RevenueInfo
                {
                    RevenueSku = lineItem.PaymentSku
                },

                CustomProperties = lstProp.ToArray()


            };

            digitalItems.Add(item);

            purchaseRequest.PurchaseContentInputSet = digitalItems.ToArray();

            purchaseRequest.OnBehalfOfPartnerGuid = partnerGuid;
            return purchaseRequest;
        }

        
        /// <summary>
        /// Get the two letter country code
        /// </summary>
        /// <param name="countryCode">Customer Country Code</param>
        /// <returns></returns>
        internal static string GetTwoLetterCountryCode(string countryCode)
        {
            //TBD 
            countryCode = "US";

            return countryCode;
        }
        /// <summary>
        /// Get the country from country code
        /// </summary>
        /// <param name="country">Customer Country Code</param>
        /// <returns></returns>
        internal static string GetCountryFromCountryCode(string countryCode)
        {
            //TBD 
            countryCode = "US";

            return countryCode;

        }

       

        public static PurchasePhysicalGoodsRequest ConvertToPurchaseRequest(this LineItem lineItem, string dataCenter, Guid partnerGuid, string shipFromPath)
        {
            string lineitemdescription = string.Empty;
            string shippingitemdescription = string.Empty;
            var shipmentinfo = lineItem.ShippingInfo;
            lineitemdescription = string.IsNullOrEmpty(lineItem.Description) ? lineItem.Description : ((lineItem.Description.Length > 128) ? lineItem.Description.Trim().Substring(0, 128) : lineItem.Description);
            shippingitemdescription = string.IsNullOrEmpty(shipmentinfo.Description) ? shipmentinfo.Description : ((shipmentinfo.Description.Length > 128) ? shipmentinfo.Description.Trim().Substring(0, 128) : shipmentinfo.Description);

            CPProxy.Address addr = new CPProxy.Address();
            var lstProp = new List<Property>();

            var prop = new Property
            {
                Name = "Datacentre",
                Namespace = "SOMExtendedService",
                Value = dataCenter
            };
            lstProp.Add(prop);

            addr = GetShipFromAddress(lineItem, shipFromPath);
            var purchaseRequest = new PurchasePhysicalGoodsRequest
            {
                AccountContext = new AccountContext()
                {
                    CountryCode = lineItem.ShippingInfo.AddressInfo.Country,
                    CustomerType = CustomerType.Personal
                }

            };

            var physicalGoodItems = new List<PurchasePhysicalGoodsInput>();

            var item = new PurchasePhysicalGoodsInput
            {
                SalesModel = SalesModelType.Reseller,
                Description = lineitemdescription,
                ExternalProductItemId = lineItem.Sku,
                PriceInfo = new PriceInfo
                {
                    Amount = lineItem.PriceInfo.Amount,
                    Currency = lineItem.PriceInfo.Currency,
                    TaxCode = lineItem.PriceInfo.TaxCode,
                    // TaxCode = "ComputerPeripherals",
                    IsTaxExemptionAllowed = false,
                    IsTaxIncluded = false

                },
                RevenueInfo = new RevenueInfo
                {
                    RevenueAllocationPriceType = RevenueAllocationPriceType.Amount,
                    RevenueSku = lineItem.PaymentSku
                    //RevenueSku = "F7U-00005"
                },
                ShipFromAddress = new CPProxy.Address
                {
                    City = addr.City,
                    CountryCode = addr.CountryCode,
                    FriendlyName = lineItem.AddressInfo.FriendlyName,
                    UnitNumber = lineItem.ShippingInfo.AddressInfo.UnitNumber,
                    PostalCode = addr.PostalCode,
                    Street1 = addr.Street1,
                    State = addr.State

                },


                ProductShipmentInfo = new ProductShipmentInfo
                {
                    Dimension = "12*12*12",
                    ShipmentMethod = "UPS",
                    Weight = "120kg"
                },
                CustomProperties = lstProp.ToArray()
            };

            physicalGoodItems.Add(item);

            purchaseRequest.PurchasePhysicalItemInputSet = physicalGoodItems.ToArray();


            var feesAndServiceChargesItems = new List<FeesAndServiceChargesInput>();
            var shippment = lineItem.ShippingInfo;
            var shippingitem = new FeesAndServiceChargesInput
            {
                Description = shippingitemdescription,
                SalesModel = SalesModelType.Reseller,
                //TODO
                //Title = shippingitemdescription,
                ExternalProductItemId = shippment.Sku,
                PriceInfo = new PriceInfo
                {
                    Amount = shippment.PriceInfo.Amount,
                    Currency = shippment.PriceInfo.Currency,
                    TaxCode = shippment.PriceInfo.TaxCode,
                    IsTaxExemptionAllowed = false,
                    IsTaxIncluded = false
                },
                RevenueInfo = new RevenueInfo
                {
                    RevenueAllocationPriceType = RevenueAllocationPriceType.Amount,
                    RevenueSku = shippment.PaymentSku
                },
                ShipFromAddress = new CPProxy.Address
                {
                    City = addr.City,
                    CountryCode = addr.CountryCode,
                    FriendlyName = lineItem.AddressInfo.FriendlyName,
                    UnitNumber = lineItem.ShippingInfo.AddressInfo.UnitNumber,
                    PostalCode = addr.PostalCode,
                    Street1 = addr.Street1,
                    State = addr.State
                },
                CustomProperties = lstProp.ToArray()
            };

            feesAndServiceChargesItems.Add(shippingitem);
            purchaseRequest.FeesAndServiceChargesInputSet = feesAndServiceChargesItems.ToArray();
            purchaseRequest.TrackingGuid = Guid.NewGuid();
            purchaseRequest.OnBehalfOfPartnerGuid = partnerGuid;
            return purchaseRequest;
        }


        internal static CPProxy.Address GetShipFromAddress(LineItem lineitem, string shipFromPath)
        {
            CPProxy.Address addr = new CPProxy.Address();
            ShipFromManager.ShipFromPath = shipFromPath;
            var collection = ShipFromManager.GetShipFromAddressList();
            string countrycode = lineitem.AddressInfo.Country;
            string brand = string.Empty;
            var requestContext = new RequestContext();
            if (requestContext != null && String.IsNullOrEmpty(requestContext.Brand))
            {
                brand = requestContext.Brand;
            }


            if (collection != null)
            {
                var collectionforBrand = collection.Find(t => t.name.Trim().ToLower() == requestContext.Brand.Trim().ToLower()); ;
                if (collectionforBrand == null)
                {
                    var conEx = new ConfigurationErrorsException("Ship From address Brand could not be found.");
                    throw conEx;
                }
                var shipfromaddr = collectionforBrand.ShipToCountryCollection.Find(x => x.name.Contains(countrycode) && x.Default == true);
                if (shipfromaddr != null)
                {
                    addr.Street1 = shipfromaddr.Address1;
                    addr.City = shipfromaddr.City;
                    addr.CountryCode = shipfromaddr.CountryCode;
                    addr.State = shipfromaddr.State;
                    addr.PostalCode = shipfromaddr.PostalCode;
                    addr.Street2 = shipfromaddr.Address2;
                    return addr;
                }
                else
                {
                    var conEx = new ConfigurationErrorsException(
                            "Ship From address could not be found for the provided address.");
                    throw conEx;
                }
            }
            return addr;
        }


        internal static Dictionary<string, string> GetCurrentMethodContext<T>(this T obj, params object[] paramValues) where T : class
        {
            var context = new Dictionary<string, string>();
            try
            {
                var methodInfo = new StackTrace().GetFrame(1).GetMethod();

                context["MethodName"] = methodInfo.Name;

                var paramInfo = methodInfo.GetParameters();

                if (paramInfo == null || paramInfo.Length == 0 || paramValues == null || paramValues.Length == 0 || paramInfo.Length > paramValues.Length)
                    return context;

                for (int index = 0; index < paramInfo.Length; index++)
                {
                    var param = paramInfo[index];
                    var value = paramValues[index];
                    context[param.Name] = value.SerializeObject();
                }

                for (int index = paramInfo.Length; index < paramValues.Length; index++)
                {
                    var value = paramValues[index];
                    var param = "AdditionalContext" + (paramInfo.Length - index).ToString(System.Globalization.CultureInfo.InvariantCulture);
                    context[param] = value.SerializeObject();
                }
            }
            catch { }
            return context;
        }

        internal static string SerializeObject<T>(this T obj) where T : class
        {
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                using (System.IO.StringWriter writer = new System.IO.StringWriter())
                {
                    serializer.Serialize(writer, obj);
                    return writer.ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
