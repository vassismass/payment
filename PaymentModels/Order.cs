using System.Runtime.Serialization;

namespace Microsoft.SupplyChain.Care.PaymentModels
{
    [DataContract(Namespace = "http://EDD.Highlander.Commerce.ExtendedService.ContractLibrary.DataContract.Payment/2012/06/Order")]

    #region Exsiting
    //public class Order : IExtensibleDataObject
    //{
    //    [DataMember]
    //    public Device.Device Device { get; set; }

    //    [DataMember]
    //    public LineItem LineItem { get; set; }

    //    [DataMember]
    //    public decimal TotalWithoutTax { get; set; }

    //    [DataMember]
    //    public Profile.Profile UserInfo { get; set; }

    //    [DataMember]
    //    public decimal Tax { get; set; }

    //    [DataMember]
    //    public string IncidentNumber { get; set; }

    //    [DataMember]
    //    public string InstrumentId { get; set; }

    //    [DataMember]
    //    public string Locale { get; set; }

    //    [DataMember]
    //    public string ProblemCode { get; set; }

    //    public virtual ExtensionDataObject ExtensionData { get; set; }
    //}
    #endregion
    public class Order : IExtensibleDataObject
    {
       
        [DataMember]
        public LineItem LineItem { get; set; }

        [DataMember]
        public decimal TotalWithoutTax { get; set; }

        [DataMember]
        public decimal Tax { get; set; }

        //added by v-ashutr
        [DataMember]
        public string InstrumentId { get; set; }

        public virtual ExtensionDataObject ExtensionData { get; set; }
    }

}
