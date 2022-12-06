using is5.cs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace is5.cs.Controllers
{
    public class HomeController : Controller
    {
        // создаем контекст данных
        InvestmentsContext db = new InvestmentsContext();

        const int USD = 73;
        const int EUR = 93;

        public void OllSum()
        {
            double ollSum = 0;

            foreach (var b in db.Shares)
            {
                RegisterShares newRegisterShares = db.RegisterShares.FirstOrDefault(r => r.Id == b.Flag);

                switch (b.TypeVal)
                {
                    case "RUB":
                        {
                            ollSum += b.Cost * newRegisterShares.QuantityLot;
                            break;                            
                        }//case
                    case "USD":
                        {
                            ollSum += b.Cost * newRegisterShares.QuantityLot * USD;
                            break;
                        }//case
                    case "EUR":
                        {
                            ollSum += b.Cost * newRegisterShares.QuantityLot * EUR;
                            break;
                        }//case
                }//switch
            }

            Score score = db.Score.Find(1);

            ollSum += score.RUB + score.USD * USD + score.EUR * EUR;

            score.OllRUB = ollSum;

            db.SaveChanges();

        }//OllSum

        public ActionResult History()
        {
            // получаем из бд все объекты вкладка история
            var transferAndReplenishment = db.TransferAndReplenishment;
            ViewBag.TransferAndReplenishment = transferAndReplenishment;

            var historyShares = db.HistoryShares;
            ViewBag.HistoryShares = historyShares;

            return View();

        }//History

        public ActionResult PersonalArea()
        {
            // получаем из бд все объекты вкладка личный кабинет
            var score = db.Score;
            ViewBag.Score = score;
            
            var registerShares = db.RegisterShares;
            ViewBag.RegisterShares = registerShares;


            return View();

        }//PersonalArea

        public ActionResult Actions()
        {
            // получаем из бд все объекты вкладка действия
            
            var score = db.Score;            
            ViewBag.Score = score;

            var shares = db.Shares;
            ViewBag.Shares = shares;

            return View();

        }//Actions

        public ActionResult Index()
        {
            // получаем из бд все объекты вкладка главная страница
            
            var score = db.Score;
            ViewBag.Score = score;

            OllSum();
            ViewBag.OllRUB = score.Find(1).OllRUB;

            var registerShares = db.RegisterShares;
            ViewBag.RegisterShares = registerShares;

            var shares = db.Shares;
            ViewBag.Shares = shares;

            var transferAndReplenishment = db.TransferAndReplenishment;
            ViewBag.TransferAndReplenishment = transferAndReplenishment;

            var historyShares = db.HistoryShares;
            ViewBag.HistoryShares = historyShares;

            return View();

        }//Index

        //---------------------------------------------------------Issue shares buy

        [HttpGet]
        public ActionResult IssueBuy(int id)
        {
            var shares = db.Shares.Find(id);
            
            ViewBag.Id = id;

            ViewBag.Name = shares.Name;
            ViewBag.Cost = shares.Cost;
            ViewBag.TypeVal = shares.TypeVal;
            ViewBag.Lot = shares.Lot;
            ViewBag.Description = shares.Description;

            return View();

        }//IssueBuy[HttpGet]

        [HttpPost]
        public ActionResult IssueBuy(RegisterShares registerShares, HistoryShares historyShares, int id)
        {
            var shares = db.Shares.Find(id);

            //добавляем в историю покупку акции
            historyShares.Name = shares.Name;
            historyShares.Cost = shares.Cost;
            historyShares.TypeVal = shares.TypeVal;
            historyShares.QuantityLot *= shares.Lot;
            historyShares.BuySell = "Покупка";
            historyShares.Date = DateTime.Now;

            //вычитаем со счетов стоимость открытого вклада       
            switch (shares.TypeVal)
            {
                case "RUB":
                    {
                        if (db.Score.Find(1).RUB >= shares.Cost * registerShares.QuantityLot * shares.Lot)
                        {
                            db.Score.Find(1).RUB -= shares.Cost * registerShares.QuantityLot * shares.Lot;

                            db.SaveChanges();
                            break;
                        }//if
                        else
                            return RedirectToAction("Insufficient");
                    }//case
                case "USD":
                    {
                        if (db.Score.Find(1).USD >= shares.Cost * registerShares.QuantityLot * shares.Lot)
                        {
                            db.Score.Find(1).USD -= shares.Cost * registerShares.QuantityLot * shares.Lot;

                            db.SaveChanges();
                            break;
                        }//if
                        else
                            return RedirectToAction("Insufficient");
                    }//case
                case "EUR":
                    {
                        if (db.Score.Find(1).EUR >= shares.Cost * registerShares.QuantityLot * shares.Lot)
                        {
                            db.Score.Find(1).EUR -= shares.Cost * registerShares.QuantityLot * shares.Lot;

                            db.SaveChanges();
                            break;
                        }//if
                        else
                            return RedirectToAction("Insufficient");
                    }//case
            }//switch

            if (null == shares.Flag)
            {
                registerShares.Date = DateTime.Now;
                registerShares.Name = shares.Name;
                registerShares.Cost = shares.Cost;
                registerShares.TypeVal = shares.TypeVal;
                registerShares.QuantityLot *= shares.Lot;

                db.RegisterShares.Add(registerShares);

                db.HistoryShares.Add(historyShares);

                db.SaveChanges();

                RegisterShares newRegisterShares = db.RegisterShares.FirstOrDefault(r => r.Name == shares.Name);

                shares.Flag = newRegisterShares.Id;

                db.SaveChanges();
            }
            else 
            {
                RegisterShares newRegisterShares = db.RegisterShares.FirstOrDefault(r => r.Id == shares.Flag);

                double l = newRegisterShares.Cost * newRegisterShares.QuantityLot;

                newRegisterShares.QuantityLot += registerShares.QuantityLot * shares.Lot;

                newRegisterShares.Cost = (l + shares.Cost * registerShares.QuantityLot * shares.Lot) / (newRegisterShares.QuantityLot);

                db.HistoryShares.Add(historyShares);

                db.SaveChanges();
            }

            //OllSum();

            return RedirectToAction("Actions");

        }//IssueBuy[HttpPost]

        //---------------------------------------------------------Issue shares sell

        [HttpGet]
        public ActionResult IssueSell(int id)
        {
            var registerShares = db.RegisterShares.Find(id);

            Shares shares = db.Shares.FirstOrDefault(r => r.Flag == id);

            ViewBag.Id = id;

            ViewBag.Name = registerShares.Name;
            ViewBag.Cost = shares.Cost;
            ViewBag.TypeVal = registerShares.TypeVal;
            ViewBag.Lot = shares.Lot;
            ViewBag.QuantityLot = registerShares.QuantityLot;

            return View();

        }//IssueSell[HttpGet]

        [HttpPost]
        public ActionResult IssueSell(RegisterShares registerShares, HistoryShares historyShares, int id)
        {
            Shares shares = db.Shares.FirstOrDefault(r => r.Flag == id);

            //добавляем в историю продажу акции
            historyShares.Name = shares.Name;
            historyShares.Cost = shares.Cost;
            historyShares.TypeVal = shares.TypeVal;
            historyShares.QuantityLot *= shares.Lot;
            historyShares.BuySell = "Продажа";
            historyShares.Date = DateTime.Now;

            RegisterShares newRegisterShares = db.RegisterShares.Find(id);

            if (newRegisterShares.QuantityLot >= registerShares.QuantityLot * shares.Lot)
            {
                //вычитаем со счетов стоимость открытого вклада
                switch (newRegisterShares.TypeVal)
                {
                    case "RUB":
                        {
                            db.Score.Find(1).RUB += registerShares.QuantityLot * shares.Lot * shares.Cost;
                            break;
                        }//case
                    case "USD":
                        {
                            db.Score.Find(1).USD += registerShares.QuantityLot * shares.Lot * shares.Cost;
                            break;
                        }//case
                    case "EUR":
                        {
                            db.Score.Find(1).EUR += registerShares.QuantityLot * shares.Lot * shares.Cost;
                            break;
                        }//case
                }//switch

                newRegisterShares.QuantityLot -= registerShares.QuantityLot * shares.Lot;

                db.HistoryShares.Add(historyShares);

                db.SaveChanges();
            }
            else
                return RedirectToAction("Insufficient");

            //OllSum();

            return RedirectToAction("PersonalArea");

        }//IssueSell[HttpPost]

        //---------------------------------------------------------Transfer
        [HttpGet]
        public ActionResult Transfer(int id)
        {
            var score = db.Score.Find(id);
        
            ViewBag.Id = id;

            ViewBag.RUB = score.RUB;
            ViewBag.USD = score.USD;
            ViewBag.EUR = score.EUR;

            return View();

        }//Transfer[HttpGet]

        [HttpPost]
        public ActionResult Transfer(Score registrationScore, TransferAndReplenishment transferAndReplenishment, int id)
        {
            var score = db.Score.Find(id);

            if (db.Score.Find(1).RUB >= registrationScore.RUB
                && db.Score.Find(1).USD >= registrationScore.USD
                && db.Score.Find(1).EUR >= registrationScore.EUR)
            {
                //уменьшение нынешнего счета на сумму перевода
                score.RUB -= registrationScore.RUB;
                score.USD -= registrationScore.USD;
                score.EUR -= registrationScore.EUR;

                //добавление в историю события перевод
                transferAndReplenishment.RUB = registrationScore.RUB;
                transferAndReplenishment.USD = registrationScore.USD;
                transferAndReplenishment.EUR = registrationScore.EUR;
                transferAndReplenishment.NumberPhoneCard = registrationScore.Number;
                transferAndReplenishment.FlagTR = "Перевод";
                transferAndReplenishment.Date = DateTime.Now; 

                db.TransferAndReplenishment.Add(transferAndReplenishment);

                db.SaveChanges();

                //OllSum();

                return RedirectToAction("Index");
            }//if

            return RedirectToAction("Insufficient");

        }//Transfer[HttpPost]

        //---------------------------------------------------------Replenishment
        [HttpGet]
        public ActionResult Replenishment(int id)
        {
            var score = db.Score.Find(id);

            ViewBag.Id = id;

            ViewBag.RUB = score.RUB;
            ViewBag.USD = score.USD;
            ViewBag.EUR = score.EUR;

            return View();
        }//Replenishment[HttpGet]

        [HttpPost]
        public ActionResult Replenishment(Score registrationScore, TransferAndReplenishment transferAndReplenishment, int id)
        {
            var score = db.Score.Find(id);

            //увеличение нынешнего счета на сумму пополнения
            score.RUB += registrationScore.RUB;
            score.USD += registrationScore.USD;
            score.EUR += registrationScore.EUR;

            //добавление в историю события пополнение
            transferAndReplenishment.RUB = registrationScore.RUB;
            transferAndReplenishment.USD = registrationScore.USD;
            transferAndReplenishment.EUR = registrationScore.EUR;
            transferAndReplenishment.NumberPhoneCard = registrationScore.Number;
            transferAndReplenishment.FlagTR = "Пополнение";
            transferAndReplenishment.Date = DateTime.Now;

            db.TransferAndReplenishment.Add(transferAndReplenishment);

            db.SaveChanges();

            //OllSum();

            return RedirectToAction("Index");

        }//Replenishment[HttpPost]

        
        //------------------------------------------------------------Admin_Score

        [HttpGet]
        public ActionResult ChengeScore(int? id)
        {
            ViewBag.Id = id;

            return View();
        }//ChengeScore

        [HttpPost]
        public ActionResult ChengeScore(Score score, int? id)
        {
            var changeScore = db.Score.Find(id);

            changeScore.RUB = score.RUB;
            changeScore.USD = score.USD;
            changeScore.EUR = score.EUR;

            db.SaveChanges();

            return RedirectToAction("Score");
        }//ChengeScore

        //------------------------------------------------------------Admin_Shares

        [HttpGet]
        public ActionResult AddShares(int? id)
        {
            ViewBag.Id = id;

            return View();
        }//AddDeposit

        [HttpPost]
        public ActionResult AddShares(Shares shares, int? id)
        {
            db.Shares.Add(shares);

            db.SaveChanges();

            return RedirectToAction("Shares");
        }//AddDeposit

        [HttpGet]
        public ActionResult DeleteShares(int? id)
        {
            ViewBag.Id = id;

            var shares = db.Shares.Find(id);

            ViewBag.Name = shares.Name;
            ViewBag.Description = shares.Description;
            ViewBag.Cost = shares.Cost;
            ViewBag.Lot = shares.Lot;
            ViewBag.TypeVal = shares.TypeVal;

            return View();
        }//DeleteDeposit

        [HttpPost]
        public ActionResult DeleteShares(Shares shares, int? id)
        {
            shares = db.Shares.Find(id);

            //удаление вклада из бд
            db.Shares.Remove(shares);

            db.SaveChanges();

            return RedirectToAction("Shares");
        }//DeleteDeposit

        [HttpGet]
        public ActionResult ChengeShares(int? id)
        {
            ViewBag.Id = id;

            return View();
        }//ChengeScore

        [HttpPost]
        public ActionResult ChengeShares(Shares shares, int? id)
        {
            var changeShares = db.Shares.Find(id);

            changeShares.Name = shares.Name;
            changeShares.Description = shares.Description;
            changeShares.Cost = shares.Cost;
            changeShares.Lot = shares.Lot;
            changeShares.TypeVal = shares.TypeVal;

            db.SaveChanges();

            return RedirectToAction("Shares");
        }//ChengeScore

        public ActionResult About()
        {
            ViewBag.Message = "Разработал ЛитвиновДВ";

            return View();
        }//About

        public ActionResult Contact()
        {
            ViewBag.Message = "Здесь вы можите связаться с нами";

            return View();
        }//Contact

        public ActionResult Insufficient()
        {
            return View();
        }//Insufficient

        [HttpGet]
        public ActionResult Admin(int? id)
        {
            //ViewBag.Message = "Админ";

            var score = db.Score;
            ViewBag.Score = score;

            var shares = db.Shares;
            ViewBag.Shares = shares;

            return View();
        }//Admin

        
        public ActionResult Score(int? id)
        {

            var score = db.Score;
            ViewBag.Score = score;

            return View();
        }//Score

        public ActionResult Shares(int? id)
        {

            var shares = db.Shares;
            ViewBag.Shares = shares;

            return View();
        }//Score

    }
}