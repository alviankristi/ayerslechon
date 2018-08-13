namespace AyerLechon.Model
{
    public class DiscountViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        /// <summary>
        /// Discount has been used or not yet.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// If itemId is null, than the discount for amount payment.
        /// </summary>
        public int? ItemId { get; set; }
        public string Code { get; set; }
        public decimal? Amount { get; set; }
        public double? Percentage { get; set; }
    }
}
