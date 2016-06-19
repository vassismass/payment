//=====================================================================================================
//   Class          : ProfileBase            
//   Developed By   : 
//   Purpose        : ProfileBase 
//   Date           : 04/07/2010            
//=====================================================================================================


using System.Runtime.Serialization;

namespace Microsoft.SupplyChain.Care.PaymentModels
{
    [DataContract(Namespace = "http://EDD.Highlander.Commerce.ExtendedService/Profile/2010/04/ProfileBase")]
    public abstract class ProfileBase : IExtensibleDataObject
    {
        //private PropertyCollection _properties = new PropertyCollection();

        //[DataMember]
        //public PropertyCollection Properties
        //{
        //    get { return _properties; }
        //    set { _properties = value; }
        //}

        public virtual ExtensionDataObject ExtensionData { get; set; }

    }
}
