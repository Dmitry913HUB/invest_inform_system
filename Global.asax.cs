using is5.cs.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace is5.cs
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            using (InvestmentsContext db = new InvestmentsContext())
            {
                //акции
                //db.Shares.Add(new Shares
                //{
                //    Id = 1,
                //    Name = "Сбербанк",
                //    Description = "Сбербанк старейший российский банк. Занимает первое место в росии по обему активов",
                //    Cost = 280,
                //    Lot = 10,
                //    TypeVal = "RUB",
                //});

                //db.Shares.Add(new Shares
                //{
                //    Id = 2,
                //    Name = "Apple",
                //    Description = "Apple Inc - американская корпорация, производитель персональных планшетов и копьютеров, телефонов",
                //    Cost = 131,
                //    Lot = 1,
                //    TypeVal = "USD",
                //});

                //db.Shares.Add(new Shares
                //{
                //    Id = 3,
                //    Name = "Intel",
                //    Description = "Intel Corporation - американская корпорация, производящаяя широкий электронных устройств и копьютерных компонентов",
                //    Cost = 47,
                //    Lot = 1,
                //    TypeVal = "USD",
                //});

                //db.Shares.Add(new Shares
                //{
                //    Id = 4,
                //    Name = "Газпром",
                //    Description = "Крупнейшая газовая компания в мире, занимающая лидирующие позиции на рынке добычи сырья",
                //    Cost = 210,
                //    Lot = 10,
                //    TypeVal = "RUB",
                //});

                //-------------------------------------------------------------------------Credit

                //db.Credit.Add(new Credit
                //{
                //    Id = 1,
                //    CreditName = "На недвижимость 'Рублевый'",
                //    Percent = 20,
                //    Quantity = 100000,
                //    Period = 360,
                //    CreditType = "RUB"
                //});

                //db.Credit.Add(new Credit
                //{
                //    Id = 2,
                //    CreditName = "На Автиомобиль 'Рублевый'",
                //    Percent = 15,
                //    Quantity = 150000,
                //    Period = 720,
                //    CreditType = "RUB"
                //});

                //db.Credit.Add(new Credit
                //{
                //    Id = 3,
                //    CreditName = "Для бизнеса 'Рублевый'",
                //    Percent = 13,
                //    Quantity = 15000000,
                //    Period = 1800,
                //    CreditType = "RUB"
                //});

                //--------------------------------------------------------------------------Score

                //db.Score.Add(new Score
                //{
                //    Id = 1,
                //    RUB = 100000,
                //    USD = 1500,
                //    EUR = 1000,
                //    Number = 0
                //});

                db.SaveChanges();
            }

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Database.SetInitializer(new DbInitializer());//-----
        }
    }
}
