using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelManager
{
    public class Settings
    {
        public static void Main(string[] args)
        {
            var appSettings = ReadAllSettings();
            foreach (var key in appSettings.AllKeys)
            {
                Console.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
            }

            if (ReadSetting("Setting1", out string Setting1))
            {
                Console.WriteLine(Setting1);
            }

            if (ReadSetting("NotValid", out string NotValid))
            {
                Console.WriteLine(NotValid);
            }
            else
            {
                Console.WriteLine(NotValid);
            }

            //Console.WriteLine(ReadSetting("NotValid"));
            AddUpdateAppSettings("NewSetting", "May 7, 2014");
            AddUpdateAppSettings("Setting1", "May 8, 2014");
            Console.ReadLine();
        }

        public static System.Collections.Specialized.NameValueCollection ReadAllSettings()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                return appSettings;

                //if (appSettings.Count == 0)
                //{
                //    Console.WriteLine("AppSettings is empty.");
                //}
                //else
                //{
                //    foreach (var key in appSettings.AllKeys)
                //    {
                //        Console.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
                //    }
                //}
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }

            return null;
        }

        public static bool ReadSetting(string key, out string result)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                result = appSettings[key] ?? "Not Found";
                return appSettings[key] != null;
            }
            catch (ConfigurationErrorsException)
            {
                result = "Error reading app settings";
            }
            return false;
        }

        public static void AddUpdateAppSettings(string key, in string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
    }
}
