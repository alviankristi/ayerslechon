using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AyerLechon.Repo.Domains;
using System.IO;
using System.Web;
using AyerLechon.Service.Implementations;
using AyerLechon.Model;
using System.Collections.Generic;

namespace AyerLechon.IntegrationsTest
{
    [TestClass]
    public class RepoTest
    {
        [TestMethod]
        public void InitClientTable()
        {
            var ctx = new AyerLechonContext();
            ctx.Clients.Add(new Client()
            {
                Active = true,
                AllowedOrigin = "*",
                ApplicationType = 0,
                Name = "IOS",
                RefreshTokenLifeTime = 1,
                Secret = "919d676f-fead-49eb-990c-b84848448df2",
                ClientID = Guid.Parse("EE8CF68C-BBA0-4615-A78D-683312CF03E3")
            });
            ctx.SaveChanges();
        }


        [TestMethod]
        public void DummyDiscount()
        {
            using (var ctx = new AyerLechonContext())
            {
                var filepath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Image\\promo.jpeg";


                string mimeType = MimeMapping.GetMimeMapping(filepath);


                FileStream stream = File.OpenRead(filepath);
                byte[] fileBytes = new byte[stream.Length];

                stream.Read(fileBytes, 0, fileBytes.Length);
                stream.Close();
                //Begins the process of writing the byte array back to a file

                using (Stream file = File.OpenWrite(filepath))
                {
                    file.Write(fileBytes, 0, fileBytes.Length);
                }

                var expiredDate = DateTime.Now.AddMonths(1).ToEpochTime();
                ctx.Discounts.Add(new Discount
                {
                    Code = "TESTPROMO3",
                    Description = "Example Promo 3",
                    ExpiredDate = expiredDate,
                    FileStorage = new FileStorage
                    {
                        MIMEType = mimeType,
                        FileName = Path.GetFileName(filepath),
                        UploadedFile = fileBytes
                    },

                });
                ctx.Discounts.Add(new Discount
                {
                    Code = "TESTPROMO4",
                    Description = "Example Promo 4",
                    ExpiredDate = expiredDate,
                    FileStorage = new FileStorage
                    {
                        MIMEType = mimeType,
                        FileName = Path.GetFileName(filepath),
                        UploadedFile = fileBytes
                    },

                });
                ctx.Discounts.Add(new Discount
                {
                    Code = "TESTPROMO5",
                    Description = "Example Promo 5",
                    ExpiredDate = expiredDate,
                    FileStorage = new FileStorage
                    {
                        MIMEType = mimeType,
                        FileName = Path.GetFileName(filepath),
                        UploadedFile = fileBytes
                    },

                });
                ctx.SaveChanges();
            }

        }


        [TestMethod]
        public void DummyItem()
        {
            using (var ctx = new AyerLechonContext())
            {
                //you can change to uploaded file source
                var filepath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Image\\image.jpg";


                string mimeType = MimeMapping.GetMimeMapping(filepath);


                FileStream stream = File.OpenRead(filepath);
                byte[] fileBytes = new byte[stream.Length];

                stream.Read(fileBytes, 0, fileBytes.Length);
                stream.Close();
                //Begins the process of writing the byte array back to a file

                using (Stream file = File.OpenWrite(filepath))
                {
                    file.Write(fileBytes, 0, fileBytes.Length);
                }
                ctx.Items.Add(new Item
                {
                    CategoryID = 1,
                    Description = "Lechon Belly (49-70 pax)",
                    FileStorage = new FileStorage
                    {
                        MIMEType = mimeType,
                        FileName = Path.GetFileName(filepath),
                        UploadedFile = fileBytes
                    },
                    Price = 2520,
                    ReadyStock = 10,

                });
                ctx.Items.Add(new Item
                {
                    CategoryID = 1,
                    Description = "Lechon Belly (100-170 pax)",
                    FileStorage = new FileStorage
                    {
                        MIMEType = mimeType,
                        FileName = Path.GetFileName(filepath),
                        UploadedFile = fileBytes
                    },
                    Price = 3520,
                    ReadyStock = 10,

                });
                ctx.SaveChanges();
            }

            //ctx.SaveChanges();
        }

        [TestMethod]
        public void TestCreditLimit()
        {
            using (var context = new AyerLechonContext())
            {
                var service = new PaymentService(context);

                var summary = new OrderSummaryViewModel()
                {
                    Amount = 500,
                    DeliveryAddress = "delivery address",
                    Notes = "notes",
                    PaymentOptionId = PaymentOptionEnum.CreditLine,
                    PhoneNumber = "Phone Number",
                    OrderDate = DateTime.Now.ToEpochTime(),
                    RegionId = null,
                    CustomerId = 1,
                    OrderDetails = new List<OrderDetailViewModel>()
                    {
                         new OrderDetailViewModel
                         {
                             Quantity = 1,
                             ItemId = 1,
                         }
                    }
                };
                service.Create(summary);
            }
        }
    }
}
