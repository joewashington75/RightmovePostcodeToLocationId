using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RightmovePostcodeToLocationId.PostcodeProcessor.Core.Enums;

namespace RightmovePostcodeToLocationId.PostcodeProcessor.Console.Helpers
{
    public static class RightmoveChromeDriverHelpers
    {
        public static ChromeDriver NavigateToRightMove(this ChromeDriver driver)
        {
            driver.Navigate().GoToUrl("https://www.rightmove.co.uk/");
            return driver;
        }

        public static ChromeDriver AddPostcodeToSearchLocation(this ChromeDriver driver, string postcode)
        {
            var search = driver.FindElement(By.Id("searchLocation"));
            search.SendKeys(postcode);
            AllowAllCookies(driver);
            return driver;
        }

        public static ChromeDriver ClickBuy(this ChromeDriver driver)
        {
            var searchButton = driver.FindElement(By.Id("buy"));
            searchButton.Click();
            return driver;
        }

        public static ChromeDriver ClickSubmit(this ChromeDriver driver)
        {
            var findPropertiesButton = driver.FindElement(By.Id("submit"));
            findPropertiesButton.Click();
            return driver;
        }

        public static (bool Found, ProcessingStatus Status) CheckPostcodeExists(this ChromeDriver driver)
        {
            try
            {
                var errorBox = driver.FindElementByClassName("errorbox");
                if (errorBox != null && errorBox.Text.Contains("We could not find a place name starting with"))
                {
                    return (false, ProcessingStatus.PostcodeNotFound);
                }
            }
            catch (NoSuchElementException)
            {
                return (true, ProcessingStatus.NoResultsForPostcode);
            }
            catch (Exception)
            {
                return (false, ProcessingStatus.Exception);
            }

            return (true, ProcessingStatus.PostcodeFound);
        }

        static void AllowAllCookies(ChromeDriver driver)
        {
            try
            {
                var manageSettings = driver.FindElementByCssSelector("#optanon-popup-bottom > div.optanon-button-wrapper.optanon-allow-all-button.optanon-allow-all > div.optanon-white-button-middle > button");
                manageSettings.Click();
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
