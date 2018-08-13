namespace AyerLechon.Model
{
    public static class PaymentOptionEnum
    {
        public const int BankDeposit = 1;
        public const int BankOnlineTransfer = 2;
        public const int MoneyTransfer = 3;
        public const int PayAtBranch = 4;
        public const int CreditLine = 5;
        public const int CreditCard = 6;
        public const int CashOnDelivery = 7;
    }

    public static class PaymentStatusEnum
    {
        public const int Unpaid = 1;
        public const int PartiallyPaid = 2;
        public const int FullyPaid = 3;
    }
}
