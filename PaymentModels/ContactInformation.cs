

namespace Microsoft.SupplyChain.Care.PaymentModels
{
    public class ContactInformation
    {
        /// <summary>
        /// FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// LastName
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// EmailAddress
        /// </summary>  
        public string EmailAddress { get; set; }

        /// <summary>
        /// ContactNumber
        /// </summary>
        public PhoneNumber ContactNumber { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public Address Address { get; set; }
    }
}