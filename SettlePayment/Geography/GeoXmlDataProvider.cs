using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Microsoft.SupplyChain.Care.Payment.ServiceLibrary.Exceptions;

namespace Microsoft.SupplyChain.Care.Payment.ServiceLibrary.Geography
{
    public class GeoXmlDataProvider
    {
        private static GetCountriesResponse response = null;
        public GetCountriesResponse GetCountries()
        {
            if (response != null)
                return response;
            else
                response = GetCountriesFromXml();
            return response;
        }

        /// <summary>
        /// Pulling the Coutnry Data From XML
        /// </summary>
        /// <returns></returns>
        private GetCountriesResponse GetCountriesFromXml()
        {
            var response = new GetCountriesResponse();
            try
            {
                //XDocument xDoc = XDocument.Load(@"c:\users\v-chans\documents\visual studio 2010\Projects\Serviceorder.Service\ServiceOrder.ServiceImplementation\Geography.xml");
                string geographyPath = "C:\\Users\\t-pugoe\\Documents\\My_Project\\Payment_Final\\PaymentServiceLibrary\\Geography\\Geography.xml";
                XDocument xDoc = XDocument.Load(geographyPath);
                var Geography = from cDetail in xDoc.Descendants("Geography")
                                where cDetail.Element("ISOType").Value.ToString().Equals("country")
                                select new
                                {
                                    IsoName = (string)cDetail.Element("ISOName") != null ? (string)cDetail.Element("ISOName") : "",
                                    IsoCountryNumber = (string)cDetail.Element("ISOCountryNumber") != null ? (string)cDetail.Element("ISOCountryNumber") : "",
                                    IsoCode = (string)cDetail.Element("ISOCode") != null ? (string)cDetail.Element("ISOCode") : "",
                                    Id = (string)cDetail.Element("Id") != null ? (string)cDetail.Element("Id") : ""
                                };

                List<Country> lstCountry = new List<Country>();
                foreach (var c in Geography)
                {
                    Country objCountry = new Country();
                    objCountry.IsoCountryNumber = c.IsoCountryNumber;
                    objCountry.IsoCountryLongName = c.IsoName;
                    objCountry.IsoAlpha3Code = c.IsoCode;
                    lstCountry.Add(objCountry);
                }
                response.Countries = lstCountry;
            }
            catch (FormatException valEx)
            {
                ValidationException fault = new ValidationException("XML Format Exception", valEx);
                fault.ExceptionCode = PaymentExceptioncode.GeographyXMLDataError;
                throw fault;
            }
            catch (XmlException valEx)
            {
                ValidationException fault = new ValidationException("XML Format Exception", valEx);
                fault.ExceptionCode = PaymentExceptioncode.GeographyXMLDataError;
                throw fault;
            }
            catch (Exception ex)
            {
                throw new PaymentIntegrationException("An error occured while executing GetCountries method ", PaymentExceptioncode.GeographyXmlUnhandledError, ex);
            }

            return response;
        }
        
    }
}
