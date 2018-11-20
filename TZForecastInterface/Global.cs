using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TZForecastInterface
{
    class Global
    {
        private static string companyID;
        public static string CompanyID { get => companyID; set => companyID = value; }

        private static string mainPlant;
        public static string MainPlant { get => mainPlant; set => mainPlant = value; }

        private static Int16 icCustNum;
        public static Int16 ICCustNum { get => icCustNum; set => icCustNum = value; }

        private static Int16 custengCustNum;
        public static Int16 CustEngCustNum { get => custengCustNum; set => custengCustNum = value; }

        private static Int16 usageCustNum;
        public static Int16 UsageCustNum { get => usageCustNum; set => usageCustNum = value; }

        private static Boolean debug;
        public static Boolean Debug { get => debug; set => debug = value; }

    }
}
