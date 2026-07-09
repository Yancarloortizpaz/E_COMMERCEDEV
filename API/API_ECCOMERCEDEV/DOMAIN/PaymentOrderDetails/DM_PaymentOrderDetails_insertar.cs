namespace DOMAIN.PaymentOrderDetails
{
    public class DM_PaymentOrderDetails_insertar
    {
        public int? orderDetailOrderId { get; set; }
        public int? orderDetailProductVariableId { get; set; }
        public decimal? orderDetailPrice { get; set; }
        public int? orderDetailQuantity { get; set; }
        public decimal? orderDetailDiscount { get; set; }
        public decimal? orderDetailSubTotal { get; set; }
        public decimal? orderDetailTAX { get; set; }
        public decimal? orderDetailTotal { get; set; }
        public int? orderDetailCurrencyId { get; set; }
        public int? orderDetailCreatorId { get; set; }
        public bool? orderDetailStatusId { get; set; }
    }
}
