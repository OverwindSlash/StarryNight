using System;
using System.Collections.Generic;
using System.Text;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using StarryNight.Configuration;

namespace StarryNight.Dapper
{
    public static class DbSettings
    {
        private static IConfigurationRoot _appConfiguration
            = AppConfigurations.Get(typeof(StarryNightDapperModule).GetAssembly().GetDirectoryPathOrNull());

        public static string GetConnectionString(string type)
        {
            string section = $"ConnectionStrings:{type}";

            try
            {
                string connectionStr = _appConfiguration.GetSection(section).Value;
                return connectionStr;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string DefaultConnectionString
        {
            get
            {
                return _appConfiguration.GetSection("ConnectionStrings:Default").Value;
            }
        }
    }
}
