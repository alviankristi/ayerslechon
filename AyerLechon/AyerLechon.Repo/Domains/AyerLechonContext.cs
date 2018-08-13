namespace AyerLechon.Repo.Domains
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AyerLechonContext : DbContext
    {
        public AyerLechonContext()
            : base("name=AyerLechonContext")
        {
        }

        public virtual DbSet<BankDetail> BankDetails { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<FileStorage> FileStorages { get; set; }
        public virtual DbSet<ItemCategory> ItemCategories { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<LoginDevice> LoginDevices { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderStatu> OrderStatus { get; set; }
        public virtual DbSet<OrderSummary> OrderSummaries { get; set; }
        public virtual DbSet<PaymentOption> PaymentOptions { get; set; }
        public virtual DbSet<PaymentStatu> PaymentStatus { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Region> Regions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankDetail>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.CreditLimit)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.LoginDevices)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.OrderSummaries)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Discount>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Discount>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Discount>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<FileStorage>()
                .Property(e => e.FileName)
                .IsUnicode(false);

            modelBuilder.Entity<FileStorage>()
                .Property(e => e.MIMEType)
                .IsUnicode(false);

            modelBuilder.Entity<FileStorage>()
                .HasMany(e => e.Discounts)
                .WithRequired(e => e.FileStorage)
                .HasForeignKey(e => e.Image)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FileStorage>()
                .HasMany(e => e.Items)
                .WithOptional(e => e.FileStorage)
                .HasForeignKey(e => e.Image);

            modelBuilder.Entity<ItemCategory>()
                .Property(e => e.Category)
                .IsUnicode(false);

            modelBuilder.Entity<Item>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Item>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Item>()
                .Property(e => e.WebPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Item>()
                .Property(e => e.AirFreight)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LoginDevice>()
                .Property(e => e.FbAccountId)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.SubTotal)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OrderStatu>()
                .Property(e => e.OrderStatus)
                .IsUnicode(false);

            modelBuilder.Entity<OrderSummary>()
                .Property(e => e.AmountPaid)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OrderSummary>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<OrderSummary>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.OrderSummary)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PaymentOption>()
                .Property(e => e.BankDetailLine1)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentOption>()
                .Property(e => e.BankDetailLine2)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentOption>()
                .Property(e => e.BankDetailLine3)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentOption>()
                .HasMany(e => e.OrderSummaries)
                .WithRequired(e => e.PaymentOption)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PaymentStatu>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Region>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Region>()
                .Property(e => e.DeliveryFee)
                .HasPrecision(19, 4);
        }
    }
}
