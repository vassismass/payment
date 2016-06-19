using Microsoft.SupplyChain.Care.PaymentModels;
using CPProxy = Microsoft.SupplyChain.Care.Payment.ServiceLibrary.CP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Care.Payment.ServiceLibrary.Extension
{
    public static class CorePaymentServiceExtension
    {
        // readonly static PaymentProvider paymentProvider = new CPPaymentProvider();

        internal static Address ConvertCPAddressToAddress(this CPProxy.Address addressIn, string email)
        {
            if (addressIn == null) return null;
            return new Address
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

        //internal static Address GetShipFromAddress(LineItem lineitem, string shipFromPath)
        //{
        //    Address addr = new Address();
        //    ShipFromManager.ShipFromPath = shipFromPath;
        //    var collection = ShipFromManager.GetShipFromAddressList();
        //    string countrycode = lineitem.AddressInfo.Country;
        //    string brand = string.Empty;
        //    var requestContext = ServiceLocator.Current.GetInstance<RequestContext>();
        //    if (requestContext != null && String.IsNullOrEmpty(requestContext.Brand))
        //    {
        //        brand = requestContext.Brand;
        //    }


        //    if (collection != null)
        //    {
        //        var collectionforBrand = collection.Find(t => t.name.Trim().ToLower() == requestContext.Brand.Trim().ToLower()); ;
        //        if (collectionforBrand == null)
        //        {
        //            var conEx = new Exception("Ship From address Brand could not be found.");
        //            throw conEx;
        //        }
        //        var shipfromaddr = collectionforBrand.ShipToCountryCollection.Find(x => x.name.Contains(countrycode) && x.Default == true);
        //        if (shipfromaddr != null)
        //        {
        //            addr.Street1 = shipfromaddr.Address1;
        //            addr.City = shipfromaddr.City;
        //            addr.CountryCode = shipfromaddr.CountryCode;
        //            addr.State = shipfromaddr.State;
        //            addr.PostalCode = shipfromaddr.PostalCode;
        //            addr.Street2 = shipfromaddr.Address2;
        //            return addr;
        //        }
        //        else
        //        {
        //            var conEx = new Exception(
        //                    "Ship From address could not be found for the provided address.");
        //            throw conEx;
        //        }
        //    }
        //    return addr;
        //}


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
