using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
namespace DataAccessLayer
{
    public class DataAccessLayer
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static MySqlConnection GetConnection()
        {
            Logger.Debug("Method Start");
            try
            {
                string ConnString = "server=localhost;user id=root;Password=leadsquared;database=mvc_database;persist security info=False";
                Logger.Debug("Method End");
                return (new MySqlConnection(ConnString));
            }
            catch (Exception exception)
            {

                Logger.Debug(exception);
                throw exception;
            }
           
        }


    }
}
