using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;


namespace Dia.DAL
{
    static class DB
    {
        public readonly static string connectionString;
        static DB()
        {
            AppSettingsReader reader = new AppSettingsReader();
            connectionString = reader.GetValue("db", typeof(string)).ToString();
        }
    }
}