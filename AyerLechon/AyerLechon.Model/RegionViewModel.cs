namespace AyerLechon.Model
{
    public class RegionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal DeliveryFee { get; set; }
        public bool IsPickupAtStore { get; set; }
        public bool IsAirFreight { get; set; }
    }
}
