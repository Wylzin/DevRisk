//Willian Farias Schultz
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DevRisk
{
    class Program
    {
        public static void loadCategory(ref List<ModelCategory> lstCategory)
        {
            ModelCategory[] cat = new ModelCategory[4];
            cat[0] = new ModelCategory { id = 0, name = "NOT CATEGORIZED" };
            cat[1] = new ModelCategory { id = 1, name = "DEFAULTED" };
            cat[2] = new ModelCategory { id = 2, name = "HIGHRISK" };
            cat[3] = new ModelCategory { id = 3, name = "MEDIUMRISK" };
            lstCategory = cat.ToList();
        }
        static void Main(string[] args)
        {
            List<ModelCategory> lstCategory = new List<ModelCategory>();
            loadCategory(ref lstCategory);
            bool sucess = false;
            DateTime datereference = new DateTime();
            int inttradesn = 0;

            while (!sucess)
            {
                Console.WriteLine("Please, insert bellow the reference date (mm/dd/yyyy). (to quit: insert 0000)");
                string strreference = Console.ReadLine();
                DateTime dtresult = new DateTime();
                if (strreference == "0000")
                    return;
                if (DateTime.TryParseExact(strreference, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtresult))
                {
                    datereference = dtresult;
                    sucess = true;
                }
                else
                {
                    Console.WriteLine("Invalid date.");
                }
            }
            sucess = false;
            while (!sucess)
            {
                Console.WriteLine("Please, insert the value of this portfolio's number of trades. (to quit: insert 0000)");
                string strtraden = Console.ReadLine();
                if (strtraden == "0000")
                    return;
                if (!string.IsNullOrEmpty(strtraden) && !Regex.Match(strtraden, "[a-zA-Z]").Success)
                {
                    string stringnumbers = Regex.Replace(strtraden, @"[^\d]", ""); //Regex was used in case of user inserted ',' or "." to separate groups of thousands
                    inttradesn = Convert.ToInt32(stringnumbers);
                    sucess = true;
                }
                else
                {
                    Console.WriteLine("The value must be only Numbers");
                }
            }
            List<ModelNormalTrade> lsttrade = new List<ModelNormalTrade>();
            for (int i = 1; i <= inttradesn; i++)
            {
                sucess = false;
                while (!sucess)
                {
                    Console.WriteLine("TRADE {0} Please, insert separated by spaces a number value that represents trade amount, a word that represents the client's sector and for last, a date that represents the next pending payment (mm/dd/yyyy)", i);
                    string strline = Console.ReadLine();
                    strline = Regex.Replace(strline, "/s+", " ").Trim();
                    string[] arrayline = strline.Split(' ');
                    if (arrayline.Count() == 3)
                    {
                        //Input values
                        ModelNormalTrade normaltrade = new ModelNormalTrade();
                        double dbresultado = 0;
                        if (double.TryParse(arrayline[0], out dbresultado))
                        {
                            normaltrade.Value = dbresultado;
                            sucess = true;
                        }
                        else
                        {
                            Console.WriteLine("Trade Amont Value is invalid");
                            sucess = false;
                        }
                        normaltrade.ClientSector = arrayline[1];
                        DateTime dtresult2 = new DateTime();
                        if (DateTime.TryParseExact(arrayline[2], "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtresult2))
                        {
                            normaltrade.NextPaymentDate = dtresult2;
                            sucess = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid next pending payment date.");
                            sucess = false;
                        }
                        //DEFAULTED
                        Categorize(ref normaltrade, datereference);
                        lsttrade.Add(normaltrade);

                    }
                }
            }
            Console.WriteLine("OUTPUT CATEGORY:");
            foreach(var normaltrade in lsttrade)
            {
                string categoryname = lstCategory.Single(x => x.id == normaltrade.idCategory).name;
                Console.WriteLine(categoryname);
            }
            Console.WriteLine("END (press ENTER to quit)");
            Console.ReadKey();
            return;
        }

        public static void Categorize(ref ModelNormalTrade ntrade, DateTime refdate)
        {
            //Categorizing
            TimeSpan difference = refdate.Date - ntrade.NextPaymentDate;
            double days = difference.TotalDays;
            ntrade.idCategory = 0;
            if (days > 30)
                ntrade.idCategory = 1;  //DEFAULTED
            if (ntrade.Value > 1000000 && ntrade.ClientSector.ToLower().Contains("private"))
                ntrade.idCategory = 2; //HIGHRISK
            if (ntrade.Value > 1000000 && ntrade.ClientSector.ToLower().Contains("public"))
                ntrade.idCategory = 3; //MEDIUMRISK
        }
    }
}
//QUESTION 2: 
//ANSWER:
//First of all, add the new Category inside the method "loadCategory" (this method was created to easily be replace by a future
//implementation of loading from a database), add the bool "IsPoliticallyExposed" also inside the model "ModelNormalTrade" and
//add the validation of isPoliticallyExposed inside the method "Categorize" relating with the new ModelCategorize's id.