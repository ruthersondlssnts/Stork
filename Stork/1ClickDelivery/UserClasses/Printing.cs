using _1ClickDelivery.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace _1ClickDelivery.UserClasses
{
    public class Printing:Controller
    {
    

        public FileResult PrintScheduledPickupSummary(string fromDate, string toDate)
        {
            Warning[] warnings;
            string mimeType = null;
            string[] streamids;
            string encoding;
            string filenameExtension;

            var viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.SizeToReportContent = true;
            byte[] bytes = null;

            var from = Convert.ToDateTime(fromDate);
            var to = Convert.ToDateTime(toDate);
            using (var db = new _1ClickDBContext())
            {
                var wb = db.ScheduledPickups.Where(x => x.DateOfPickup >= from && x.DateOfPickup <= to).ToList();
                viewer.LocalReport.ReportPath = @"../1ClickDelivery/Reports/ScheduledPickup_Summary.rdlc";
                //viewer.LocalReport.ReportPath = @"h:\root\home\epalabay-001\www\site1\Reports\Waybill.rdlc";
                viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", wb));
                bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AddHeader(
                    "content-disposition",
                    "attachment; filename= filename" + "." + filenameExtension);
                Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
                Response.Flush(); // send it to the client to download  
                Response.End();
                return new FileContentResult(bytes, mimeType);
            }

        }
    }
}