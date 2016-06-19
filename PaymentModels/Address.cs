//=====================================================================================================
//   Class Name     : Address                 
//   Developed By   :
//   Purpose        : Defines MessageContract.
//   Date           :   
//=====================================================================================================
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SupplyChain.Care.PaymentModels
{
    [DataContract(Namespace = "http://EDD.Highlander.Commerce.ExtendedService/Common/2010/04/Address")]
    public class Address : IExtensibleDataObject
    {
        [DataMember()]
        public string AddressId { get; set; }
        [DataMember()]
        public string Address1 { get; set; }
        [DataMember()]
        public string Address2 { get; set; }
        [DataMember()]
        public string Address3 { get; set; }
        [DataMember()]
        public string City { get; set; }
        [DataMember()]
        public string Company { get; set; }
        [DataMember()]
        public string Country { get; set; }
        [DataMember()]
        public string Email { get; set; }
        [DataMember()]
        public string Fax { get; set; }
        [DataMember()]
        public string FirstName { get; set; }
        [DataMember()]
        public string LastName { get; set; }
        [DataMember()]
        public string Phone { get; set; }
        [DataMember()]
        public string PhoneAreaCode { get; set; }
        [DataMember()]
        public string PhoneExtension { get; set; }
        [DataMember()]
        public string PhoneCountryCode { get; set; }
        [DataMember()]
        public string State { get; set; }
        [DataMember()]
        public string Zip { get; set; }
        [DataMember()]
        public string UnitNumber { get; set; }
        [DataMember()]
        public string FriendlyName { get; set; }

        public virtual ExtensionDataObject ExtensionData { get; set; }

    

        public static Address FindAddress(List<Address> addresses, Address address)
        {
            var addressFound = addresses.FirstOrDefault(a => a.Equals(address));
            if (addressFound == null)
            {
                addressFound = addresses.FirstOrDefault(a => a.EqualsWithoutZip(address));
            }
            return addressFound;
        }

        public bool EqualsWithoutZip(object obj)
        {
            var input = obj as Address;
            return (input != null
                && string.Equals(Address1, input.Address1, StringComparison.OrdinalIgnoreCase)
                && string.Equals(City, input.City, StringComparison.OrdinalIgnoreCase)
                && string.Equals(Country, input.Country, StringComparison.OrdinalIgnoreCase)
                && string.Equals(Email, input.Email, StringComparison.OrdinalIgnoreCase)
                && string.Equals(FirstName, input.FirstName, StringComparison.OrdinalIgnoreCase)
                && string.Equals(LastName, input.LastName, StringComparison.OrdinalIgnoreCase));
        }

        //Match the Address,with all the param , if all param is not matching , try to match with removing "-" from ZipCode
        public bool AddrEquals(object shipaddress)
        {
            var input = shipaddress as Address;
            var flag = (input != null
                && string.Equals(Address1, input.Address1, StringComparison.OrdinalIgnoreCase)
                && string.Equals(City, input.City, StringComparison.OrdinalIgnoreCase)
                && string.Equals(Country, input.Country, StringComparison.OrdinalIgnoreCase)
                && string.Equals(Zip, input.Zip, StringComparison.OrdinalIgnoreCase)
                && string.Equals(FirstName, input.FirstName, StringComparison.OrdinalIgnoreCase)
                && string.Equals(LastName, input.LastName, StringComparison.OrdinalIgnoreCase));
            if (flag == true)
                return true;
            else
                return (input != null
                  && string.Equals(Address1, input.Address1, StringComparison.OrdinalIgnoreCase)
                  && string.Equals(City, input.City, StringComparison.OrdinalIgnoreCase)
                  && string.Equals(Country, input.Country, StringComparison.OrdinalIgnoreCase)
                  && string.Equals(Zip.Replace("-", ""), input.Zip, StringComparison.OrdinalIgnoreCase)
                  && string.Equals(FirstName, input.FirstName, StringComparison.OrdinalIgnoreCase)
                  && string.Equals(LastName, input.LastName, StringComparison.OrdinalIgnoreCase));
        }
       
      



        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
