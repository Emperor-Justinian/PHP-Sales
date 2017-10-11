using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PHP.Sales.Core.Models.System;
using PHP.Sales.DataAccess;
using PHP.Sales.Core.Extensions;
using PHP.Sales.Web.ViewModels;
using static PHP.Sales.Web.ViewModels.ChartViewModel;
using Jitbit.Utils;
using PHP.Sales.Logic;

namespace PHP.Sales.Web.Controllers
{
    public class ReportController : Controller
    {
        /// <summary>
        /// Retrieve all the Products from the database
        /// </summary>
        /// <param name="selected">Selected product</param>
        /// <returns>A list of products in a SelectList format</returns>
        public IEnumerable<SelectListItem> GetProducts(Guid? selected)
        {
            SalesDbContext ctx = new SalesDbContext();

            var Products = ctx.Products.Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.Name
            });

            return new SelectList(Products, "Value", "Text", selected);
        }

        /// <summary>
        /// List of the products for a selection
        /// </summary>
        /// <param name="selected">Selected product</param>
        /// <returns>A selection list</returns>
        public ViewResult AddProduct(Guid? selected)
        {
            var model = new ProductListViewModel()
            {
                Products = GetProducts(selected)
            };

            if (selected != null)
            {
                model.ProductId = (Guid)selected;
            }

            return View("_ReportProductListSelector", model);
        }

        // GET: Reports
        /// <summary>
        /// Display the Reports landing page
        /// </summary>
        /// <returns>List of all repors</returns>
        public ActionResult Index()
        {
            using (var ctx = new SalesDbContext())
            {
                return View(ctx.Reports.ToList());
            }
        }

        // GET: Reports/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            using (var ctx = new SalesDbContext())
            {
                ChartViewModel chart = new ChartViewModel()
                {
                    Report = ctx.Reports.Find(id)
                };
                if (chart.Report == null)
                {
                    return HttpNotFound();
                }
                chart.Report.Product = ctx.Products.FirstOrDefault(x => x.ID == chart.Report.ProductID);

                //GET DATA
                var data1 = ctx.StockSnapshot
                                .Where(x => x.ProductID == chart.Report.ProductID)
                                .Where(x => x.TimeStamp >= chart.Report.Start.Date)
                                .Where(x => x.TimeStamp <= chart.Report.End.Date)
                                .ToList();
                var data2 = ctx.Sales
                                .Where(x => x.ProductID == chart.Report.ProductID)
                                .Where(x => x.Transaction.SaleTime >= chart.Report.Start.Date)
                                .Where(x => x.Transaction.SaleTime <= chart.Report.End.Date)
                                .ToList();
                DateTime check = chart.Report.Start.Date;
                do
                {
                    //var dayResult1 = data1.Where(X => X.TimeStamp.Date == check).ToList();
                    //var dayResult2 = data2.Where(X => X.Transaction.SaleTime.Date == check).ToList();
                    decimal stock = data1.Where(X => X.TimeStamp.Date == check).Sum(l => l.QTY);
                    decimal sale = data2.Where(X => X.Transaction.SaleTime.Date == check).Sum(s => s.QTY);

                    StockSaleSet daySet = new StockSaleSet()
                    {
                        Sale = sale,
                        Stock = stock
                    };

                    chart.Data.Add(check, daySet);
                    check = check.AddDays(1);
                } while (check <= chart.Report.End.Date);

                return View(chart);
            }
        }

        // GET: Reports/Create
        /// <summary>
        /// Create an empty report
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Create and process a report
        /// </summary>
        /// <param name="report">Report to be created</param>
        /// <returns>Report display</returns>
        [HttpPost]
        public ActionResult Create(Report report)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new SalesDbContext())
                {
                    report.Update();
                    ctx.Reports.Add(report);
                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(report);
        }

        // GET: Reports/Edit/5
        /// <summary>
        /// Edit Report Contents
        /// </summary>
        /// <param name="id">Report ID</param>
        /// <returns>Report display</returns>
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var ctx = new SalesDbContext())
            {
                Report report = ctx.Reports.Find(id);
                if (report == null)
                {
                    return HttpNotFound();
                }
                return View(report);
            }
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edit and process Report details
        /// </summary>
        /// <param name="report">Report</param>
        /// <returns>Report view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Report report)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new SalesDbContext())
                {
                    ctx.Entry(report).State = EntityState.Modified;
                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(report);
        }

        // GET: Reports/Delete/5
        /// <summary>
        /// Ask to Remove a report
        /// </summary>
        /// <param name="id">Report ID</param>
        /// <returns></returns>
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var ctx = new SalesDbContext())
            {
                Report report = ctx.Reports.Find(id);
                if (report == null)
                {
                    return HttpNotFound();
                }
                return View(report);
            }
        }

        // POST: Reports/Delete/5
        /// <summary>
        /// Remove a report from the Database
        /// </summary>
        /// <param name="id">Report ID</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            using (var ctx = new SalesDbContext())
            {
                Report report = ctx.Reports.Find(id);
                ctx.Reports.Remove(report);
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            using (var ctx = new SalesDbContext())
            {
                if (disposing)
                {
                    ctx.Dispose();
                }
                base.Dispose(disposing);
            }
        }

        private string fileNameBuilder(Guid? id)
        {
            string reportName = null;

            if (id == null)
            {
                return null;
            }
            if (ModelState.IsValid)
            {
                using (var ctx = new SalesDbContext())
                {
                    Report report = ctx.Reports.Find(id);

                    string dateYear = report.Start.Year.ToString();
                    string dateMonth = report.Start.Month.ToString();
                    string dateDay = report.Start.Day.ToString();

                    string startDate = dateYear + "." + dateMonth + "." + dateDay;

                    dateYear = report.End.Year.ToString();
                    dateMonth = report.End.Month.ToString();
                    dateDay = report.End.Day.ToString();

                    string endDate = dateYear + "." + dateMonth + "." + dateDay;
                    reportName = "report-" + startDate + "-" + endDate + ".csv";
                }
            }

            return reportName;
        }

        //// GET: Reports/ExportCSV
        //public ActionResult ExportCSV(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    using (var ctx = new SalesDbContext())
        //    {
        //        Report report = ctx.Reports.Find(id);
        //        if (report == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        return View(report);
        //    }
        //}

        // POST: Reports/ExportCSV
        [HttpPost]
        public ActionResult ExportCSV(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //[HttpPost]
            //[ValidateAntiForgeryToken]
            //public ActionResult ExportCSV(Report report)
            //{
            if (ModelState.IsValid)
            {
                using (var ctx = new SalesDbContext())
                {
                    CsvExport myExport = new CsvExport();
                    Report report = ctx.Reports.Find(id);

                    myExport.AddRow();
                    myExport["Item"] = report.Name.ToString();
                    myExport["Sales"] = report.Product.QTY.ToString();

                    string dateYear = report.Start.Year.ToString();
                    string dateMonth = report.Start.Month.ToString();
                    string dateDay = report.Start.Day.ToString();

                    string startDate = dateYear + "." + dateMonth + "." + dateDay;

                    myExport["Date Start"] = startDate;

                    dateYear = report.End.Year.ToString();
                    dateMonth = report.End.Month.ToString();
                    dateDay = report.End.Day.ToString();

                    string endDate = dateYear + "." + dateMonth + "." + dateDay;

                    myExport["Date End"] = endDate;

                    // Then you can do any of the following three output options:
                    //string myCsv = myExport.Export();
                    string csvName = "report-" + startDate + "-" + endDate + ".csv";
                    string csvpath = "C:\\reports\\";

                    //File(myExport.ExportToBytes(), "text/csv", csvName);
                    myExport.ExportToFile(csvpath+csvName);

                    //byte[] myCsvData = myExport.ExportToBytes();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        public void DownloadFile(Guid? id)
        {
            string fileName = fileNameBuilder(id);
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "text/csv";
            response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ";");
            response.TransmitFile("C:\\reports\\"+fileName);
            response.Flush();
            response.End();
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            Response.Redirect("PathToHttpHandler/DownloadFile.ashx");
        }

        // GET: Report/Prediction?q=m|w
        public ActionResult Prediction(Guid? ProductID, int? Type)
        {
            PredictionChartViewModel chart = new PredictionChartViewModel();
            DateTime beginRange;

            if (ProductID == null || Type == null) {
                return View(chart);
            }
            if (Type == (int)PredictType.MONTHLY && ProductID != null)
            {
                using (var ctx = new SalesDbContext())
                {
                    chart.Name = "Monthly Sales Report";
                    chart.ProductID = (Guid)ProductID;
                    chart.Product = ctx.Products.Where(x => x.ID == ProductID).FirstOrDefault();

                    chart.Type = PredictType.MONTHLY;

                    chart.Start = DateTime.Today.AddDays(-DateTime.Now.DayOfYear+1);
                    chart.End = chart.Start.AddYears(+2).AddDays(-1);

                    beginRange = DateTime.Today.AddDays(-DateTime.Now.DayOfYear + 1);

                    var sales = ctx.Sales.Where(x => x.Product.ID == ProductID)
                                .Where(x => x.Transaction.SaleTime >= beginRange)
                                .ToList();

                    List<double> list = new List<double>();
                    DateTime check = beginRange;
                    do
                    {
                        list.Add((double)sales
                            .Where(x => x.Transaction.SaleTime.Date >= check)
                            .Where(x => x.Transaction.SaleTime.Date < check.AddMonths(1))
                            .Sum(i => i.QTY));
                        check = check.AddMonths(1);
                    } while (check <= chart.End);

                    Algorithum algorithum = new Algorithum(list.ToArray(), 3);

                    check = chart.Start;
                    int PredictCount = 0;
                    do
                    {
                        if (check <= DateTime.Today)
                        {
                            chart.CurrentCycle.Add(check.Date, new PredictModel()
                            {
                                Value = (double)sales.Where(x => x.Transaction.SaleTime.Date >= check)
                                                    .Where(x => x.Transaction.SaleTime.Date < check.AddMonths(1))
                                                    .Sum(i => i.QTY),
                                IsPredict = false
                            });
                        }
                        else if (check < chart.Start.AddYears(1))
                        {
                            chart.CurrentCycle.Add(check, new PredictModel()
                            {
                                Value = algorithum.Prediction(++PredictCount),
                                IsPredict = true
                            });
                        }
                        else
                        {
                            chart.NextCycle.Add(check, new PredictModel()
                            {
                                Value = algorithum.Prediction(++PredictCount),
                                IsPredict = true
                            });
                        }
                        check = check.AddMonths(1);
                    } while (check < chart.Start.AddYears(2));
                }
            } else
            {
                using (var ctx = new SalesDbContext())
                {
                    chart.Name = "Weekly Sales Report";
                    chart.ProductID = (Guid)ProductID;
                    chart.Product = ctx.Products.Where(x => x.ID == ProductID).FirstOrDefault();

                    chart.Type = PredictType.WEEKLY;

                    chart.Start = DateTime.Today.AddDays(-(int)DateTime.Now.DayOfWeek);
                    chart.End = chart.Start.AddDays(14);

                    beginRange = DateTime.Today.AddDays((double)(DateTime.Now.DayOfWeek - 35));

                    var sales = ctx.Sales.Where(x => x.Product.ID == ProductID)
                               .Where(x => x.Transaction.SaleTime >= beginRange)
                               .ToList();

                    List<double> list = new List<double>();
                    DateTime check = beginRange;
                    do
                    {
                        list.Add((double)sales.Where(x => x.Transaction.SaleTime.Date == check).Sum(i => i.QTY));
                        check = check.AddDays(1);
                    } while (check <= chart.End);

                    Algorithum algorithum = new Algorithum(list.ToArray(), 7);

                    check = chart.Start;
                    int PredictCount = 0;
                    do
                    {
                        if (check <= DateTime.Today)
                        {
                            chart.CurrentCycle.Add(check.Date, new PredictModel() {
                                Value = (double)sales.Where(x => x.Transaction.SaleTime.Date == check).Sum(i => i.QTY),
                                IsPredict = false
                            });
                        }
                        else if(check < chart.Start.Date.AddDays(7))
                        {
                            chart.CurrentCycle.Add(check, new PredictModel()
                            {
                                Value = algorithum.Prediction(++PredictCount),
                                IsPredict = true
                            });
                        }
                        else
                        {
                            chart.NextCycle.Add(check, new PredictModel()
                            {
                                Value = algorithum.Prediction(++PredictCount),
                                IsPredict = true
                            });
                        }
                        check = check.AddDays(1);
                    } while (check < chart.Start.AddDays(14));
                }
            }

            return View(chart);
        }
    }
}
