using System;
using System.IO;
using OpenQA.Selenium.Chrome;

namespace RightmovePostcodeToLocationId.PostcodeProcessor.Console.Factories
{
    public class ChromeDriverFactory
    {
        public static ChromeDriver GetChromeDriver()
        {
            var options = new ChromeOptions();
            options.AddArguments("--window-size=1920,1080");
            options.AddArguments("--start-maximized");
            options.AddArguments("--headless");
            options.AddArguments("--whitelisted-ips");
            options.AddArguments("--no-sandbox");
            options.AddArguments("--disable-dev-shm-usage");
            options.AddArguments("--dns-prefetch-disable");
            options.AddArguments("--disable-features=VizDisplayCompositor");
            
            var driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Window.Maximize();
            return driver;
        }
    }
}
