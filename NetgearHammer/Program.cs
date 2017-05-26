using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.PhantomJS;
using System.IO;
using System.Reflection;

namespace NetgearHammer
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootfilename = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string filename = rootfilename + "\\output.csv";
            int invalidCounter = 0;
            Console.WriteLine("Enter desired number of attempts (ex. 200):" + Environment.NewLine);
            var attemptsCount = Console.ReadLine();
            Console.WriteLine("Running through " + attemptsCount.ToString() + " iterations, now select your desired prodcut...." + Environment.NewLine + " (1) ReadyNAS RNDP6000" + " (2) ReadyNAS 516" + " (3) ReadyNAS 716X" + " (4) R8000 Router" + " (5) R8500 Router" + " (6) A6210 WiFi USB Adapter" + " (7) ProSAFE M7300-24XF Switch" + " (8) ReadyNAS 526X" + " (9) ReadyNAS 528X" + " (10) R9000 Router" + " (11) ProSAFE XS728T Switch" + " (12) ProSAFE XS748T Switch MSRP 3,000$" + Environment.NewLine);
            var productChoice = Console.ReadLine();

            //Using PhamtomJS for autoamtion 
            IWebDriver driver = new PhantomJSDriver(@"C:\PhantomJs\bin\phantomjs\bin\");

            StringBuilder validserials = new StringBuilder();
            for(int i = 0; i < Convert.ToInt32(attemptsCount); i++)
            {
                driver.Navigate().GoToUrl("https://my.netgear.com/register/register.aspx");
                string tempSerial = GenerateSerial(productChoice);
                driver.FindElement(By.Id("ContentPlaceHolder1_txtSerial")).SendKeys(tempSerial);
                driver.FindElement(By.Id("ContentPlaceHolder1_ddlMonth")).Click();

                //Wait a bit just in case the js refresh takes awhile
                System.Threading.Thread.Sleep(5000);
                try
                {
                    string sertype = driver.FindElement(By.Id("ContentPlaceHolder1_lblProductName")).Text;
                    validserials.AppendLine(sertype + "," + tempSerial);
                }
                catch (NoSuchElementException)
                {
                    invalidCounter++;
                }


            }
            Console.WriteLine("We found " + invalidCounter.ToString() + " invalid serials out of the total attempt count of " + attemptsCount.ToString());
        }
       


        static string GenerateSerial(string productID)
        {
            switch(productID)
            {
                //Netgear utilizes a pre-fixed serial number system wherein the first three characters identify the product class when combined with another sequence of two characters later in the serial number.
                case "1":
                    return "20S" + RandomNum(3) + "RV" + RandomNum(5);
                case "2":
                    return "3C8" + RandomNum(3) + "06" + RandomNum(5);
                case "3":
                    return "3PA" + RandomNum(3) + "CA" + RandomNum(5);
                case "4":
                    return "3W7" + RandomNum(3) + "7E" + RandomNum(5);
                case "5":
                    return "4DG" + RandomNum(3) + "7W" + RandomNum(5);
                case "6":
                    return "484" + RandomNum(3) + "5R" + RandomNum(5);
                case "7":
                    return "2ER" + RandomNum(3) + "5A" + RandomNum(5);
                case "8":
                    return "4MC" + RandomNum(3) + "EV" + RandomNum(5);
                case "9":
                    return "4VB" + RandomNum(3) + "E8" + RandomNum(5);
                case "10":
                    return "4MY" + RandomNum(3) + "5Y" + RandomNum(5);
                case "11":
                    return "49E" + RandomNum(3) + "5T" + RandomNum(5);
                case "12":
                    return "4M0" + RandomNum(10);

                default:
                    return "Invalid type specified.";


            }

        }
        static string RandomNum(int length)
        {
            StringBuilder sb = new StringBuilder();
            Random r = new Random();
            for(int i = 0; i < length; i++)
            {
                sb.Append(r.Next(1, 10));
            }
            return sb.ToString();
        }
    }
}
