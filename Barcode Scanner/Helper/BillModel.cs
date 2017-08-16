using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Barcode_Scanner.Helper
{
    public class BillModel
    {
        public string method { get; set; }
        public string date { get; set; }
        public string customerName { get; set; }
        public string customerCode { get; set; }
        public string remark { get; set; }
        public string issueBy { get; set; }
        public string issueDate { get; set; }
        public string billNo { get; set; }
    }
    public class SettingFooter {
        public bool hasIssue { get; set; }
        public bool hasReciver { get; set; }
        public bool hasRecieDate { get; set; }
        public bool hasPaymentDue { get; set; }
        public bool hasPaymentCheckbox { get; set; }
        public bool hasChequeNo { get; set; }
        public bool hasChuqueDate { get; set; }
        public bool hasChuqueAmount { get; set; }
    }

}