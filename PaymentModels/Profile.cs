//=====================================================================================================
//   Class          : Customer            
//   Developed By   : 
//   Purpose        : Customer details
//   Date           : 04/07/2010            
//=====================================================================================================

using System.Runtime.Serialization;

namespace Microsoft.SupplyChain.Care.PaymentModels
{
    [DataContract(Namespace = "http://EDD.Highlander.Commerce.ExtendedService.ContractLibrary.DataContract.Profile/2012/06/Profile")]
    public class Profile : IExtensibleDataObject
    {
        #region Profile data members

        [DataMember]
        public long? CustomerId { get; set; }
        [DataMember]
        public long? Puid { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Flag { get; set; }
        [DataMember]
        public int PreferredLanguageCodeIso3 { get; set; }
        [DataMember]
        public string AddressLine1 { get; set; }
        [DataMember]
        public string AddressLine2 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string StateCodeIso2 { get; set; }
        [DataMember]
        public string CountryCodeIso3 { get; set; }
        [DataMember]
        public string Zip { get; set; }

        public virtual ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }
}
