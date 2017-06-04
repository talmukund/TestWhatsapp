using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace TestWhatsapp
{
    class Program
    {
        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        public static int GetRandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return getrandom.Next(min, max);
            }
        }
        [STAThread]
        static void Main(string[] args)
        {
            int number = GetRandomNumber(1, 12);
            var bmp = new Bitmap(string.Format(@"C:\MotivationQuote\{0}.jpg", number));
            Clipboard.SetImage(bmp);
            var option = new ChromeOptions();
            option.AddArgument("user-data-dir=c:/chromedriver");
            var driver = new ChromeDriver(option);
            driver.Navigate().GoToUrl("http://web.whatsapp.com");
            Thread.Sleep(20000);
            
            try
            {
                foreach (var name in args)
                {
                    var webObj = driver.FindElement(By.ClassName("icon-search"));
                    if (webObj != null)
                    {
                        webObj.Click();
                        Thread.Sleep(5000);
                    }


                    webObj = driver.FindElement(By.ClassName("input-search"));
                    if (webObj != null)
                    {
                        webObj.SendKeys(name);
                        Thread.Sleep(5000);
                    }


                    webObj = driver.FindElements(By.ClassName("infinite-list-item-transition"))[2];
                    if (webObj != null)
                    {
                        webObj.Click();
                        Thread.Sleep(5000);
                    }

                    webObj = driver.FindElement(By.XPath("//div[@contenteditable=\"true\"]"));
                    if (webObj != null)
                    {
                        webObj.SendKeys(OpenQA.Selenium.Keys.Control + 'v');
                        Thread.Sleep(5000);
                    }

                    webObj = driver.FindElement(By.XPath("//div[@contenteditable=\"true\"]"));
                    if (webObj != null)
                    {
                        webObj.SendKeys(string.Format("Good Morning {0}! Have a beautiful day!!!",name));
                        Thread.Sleep(3000);
                    }
                    webObj = driver.FindElement(By.ClassName("btn-l"));
                    webObj.Click();
                    Thread.Sleep(5000);
                }                
                driver.Close();
                driver.Dispose();
            }
            catch (Exception ex)
            {
                TextWriterTraceListener myListener = new TextWriterTraceListener("c:\\whatsapp.log", "myListener");
                myListener.WriteLine(DateTime.Now + "\t"+ ex.Message);
                // You must close or flush the trace listener to empty the output buffer.  
                myListener.Flush();

                driver.Close();
                driver.Dispose();
            }
            
            
        }
    }
}
