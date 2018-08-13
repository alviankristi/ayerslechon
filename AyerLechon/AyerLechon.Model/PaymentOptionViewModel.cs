namespace AyerLechon.Model
{
    public class PaymentOptionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Charge { get; set; }
        public string BankDetailLine1 { get; set; }  
        public string BankDetailLine2 { get; set; }  
        public string BankDetailLine3 { get; set; }  

    }
}
