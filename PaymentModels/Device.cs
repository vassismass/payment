//=====================================================================================================
//   Class          : Device            
//   Developed By   : 
//   Purpose        : Device Info
//   Date           : 04/07/2010            
//=====================================================================================================

using System;
using System.Runtime.Serialization;

namespace Microsoft.SupplyChain.Care.PaymentModels
{
    [DataContract(Namespace = "http://EDD.Highlander.Commerce.ExtendedService/Profile/2010/04/Device")]
    public class Device : ProfileBase
    {
        #region Device Data Members

        [DataMember]
        public string ProductType { get; set; }
      
        [DataMember]
        public string ProductSeries {get;set;}

        [DataMember]
        public string ProductFamilyName { get; set; }
   
        [DataMember]
        public string ProductMarketPlace {get;set;}
     
        [DataMember]
        public string PlatformType {get;set;}
     
        [DataMember]
        public string WarrantyStatus {get;set;}

        [DataMember]
        public string WarrantyType { get; set; }

        [DataMember]
        public DateTime? WarrantyStartDate {get;set;}

        [DataMember]
        public DateTime? WarrantyExpirationDate { get; set; }
      
        [DataMember]
        public int RepairHistoryCount{get;set;} 
      
        [DataMember]
        public string IndustryType {get;set;}
      
        [DataMember]
        public string ProductFlag {get;set;}
      
        [DataMember]
        public string SerialNumber {get;set;}
       
        [DataMember]
        public string Description{get;set;} 
      
        [DataMember]
        public string Image {get;set;}

        [DataMember]
        public bool IsRegistered { get; set; }
      
        [DataMember]
        public WarrantyCollection SupplimentalWarranties { get; set; }

        [DataMember]
        public WarrantyCollection AdditionalWarranties { get; set; }

        [DataMember]
        public Warranty AccessoryWarranty { get; set; }

        [DataMember]
        public ComponentDetailsCollection ComponentDetails { get; set; }

        [DataMember]
        public bool HasOpenRepairOrder { get; set; }

        [DataMember]
        public string RepairServiceOrderRequestId { get; set; }

        [DataMember]
        public FulfilmentDetailsCollection FulfilmentHistory { get; set; }

        [DataMember]
        public Warranty StandardWarranty { get; set; }

        #endregion
    }
}
