
namespace Microsoft.SupplyChain.Care.PaymentModels
{
    /// <summary>
    /// Service order 
    /// </summary>
    public class ServiceOrder
    {
        /// <summary>
        /// Service Request Number - Unique Identifier
        /// </summary>
        public string ServiceRequestNumber { get; set; }
        /// <summary>
        /// Ship to Address
        /// </summary>
        public ContactInformation ShipTo { get; set; }
        /// <summary>
        /// Bill to Address
        /// </summary>
        public ContactInformation BillTo { get; set; }
        /// <summary>
        /// Carrier information for Inbound or outbound shipment
        /// </summary>
        public Carrier Carrier { get; set; }
        /// <summary>
        /// Customer Id - Unique identifier for Customer
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// Payment information
        /// </summary>
        public OrderPayment Payment { get; set; }
        /// <summary>
        /// Comments for product problem
        /// </summary>
        public string ProblemComments { get; set; }
        /// <summary>
        /// Problem Id - Unique identifier
        /// </summary>
        public string ProblemId { get; set; }
        /// <summary>
        /// Proof of purchase Id - Unique Identifier
        /// </summary>
        public string ProofOfPurchaseId { get; set; }
        /// <summary>
        /// Repair offer SKU
        /// </summary>
        public string OfferSku { get; set; }
        /// <summary>
        /// Sales order document details containing details of customised accessory or device sale
        /// </summary>
        public SalesOrderDoc SalesOrder { get; set; }
        /// <summary>
        /// device which has been shipped back as part of repair or exchange
        /// </summary>
        public DeviceKey ShippedDevice { get; set; }
        /// <summary>
        /// Shipping comments
        /// </summary>
        public string ShippingComments { get; set; }
        /// <summary>
        /// Shipping offer SKU
        /// </summary>
        public string ShippingOfferSku { get; set; }
        /// <summary>
        /// Service request status Open or Closed or Cancelled
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Current state of order e.g New, InProcess, InitiateRequestOrder, ShipmentReceived,ShipmentSent
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// device on which order has been created
        /// </summary>
        public DeviceKey TicketedDevice { get; set; }

        /// <summary>
        /// OrderType or service type availed by customer e.g RepairOrder, AdvanceExchange,ISE, Upgrade etc.
        /// </summary>
        public string OrderType { get; set; }
        //public string SymptomCode { get; set; }
        /// <summary>
        /// In case of fullfilment order , accessory type is mandatory. e.g Pen,Power Cable
        /// </summary>
        public string AccessoryType { get; set; }
    }
}