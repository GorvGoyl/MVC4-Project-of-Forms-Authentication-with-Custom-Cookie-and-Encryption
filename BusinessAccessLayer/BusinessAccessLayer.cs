using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using DataAccessLayer;
using System.IO;
using Newtonsoft.Json;
using System.Data;
using System.Web;
using System.Web.Security;
namespace BusinessAccessLayer
{
    public class BusinessAccessLayer
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void AddUser(string user)
        {
            Logger.Debug("Method Start");

            List<string> UserList = JsonConvert.DeserializeObject<List<string>>(user);
            using (MySqlConnection Con = DataAccessLayer.DataAccessLayer.GetConnection())
            {
                try
                {
                    Con.Open();
                    MySqlCommand Cmd = new MySqlCommand("Insert_user_data", Con);
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("var_UserNameparam", UserList[0]);
                    Cmd.Parameters.AddWithValue("var_Passwordsparam", UserList[1]);
                    Cmd.Parameters.AddWithValue("var_Phoneparam", UserList[2]);
                    Cmd.Parameters.AddWithValue("var_Cityparam", UserList[3]);
                    Cmd.ExecuteNonQuery();
                }
                catch (Exception exception)
                {


                    Logger.Debug(exception);
                }
            }

            Logger.Debug("Method End");

        }
        public static string ValidateUser(string username, string password)
        {
            Logger.Debug("Method Start");
            //Will write code for validate user from our own database 
            using (MySqlConnection Con = DataAccessLayer.DataAccessLayer.GetConnection())
            {
                try
                {
                    Con.Open();
                    MySqlCommand Comm = Con.CreateCommand();
                    Comm.CommandText = "Select * from user_data  WHERE UserName = @UserName and Passwords = @Password";
                    Comm.Parameters.AddWithValue("@UserName", username);
                    Comm.Parameters.AddWithValue("@Password", password);
                    string JsonUser = "";
                    MySqlDataReader Reader = Comm.ExecuteReader();

                    while (Reader.Read())
                    {
                        List<string> UserData = new List<string>();
                        UserData.Add(Reader["UserName"].ToString());
                        UserData.Add(Reader["Passwords"].ToString());
                        UserData.Add(Reader["Phone"].ToString());
                        UserData.Add(Reader["City"].ToString());
                        JsonUser = JsonConvert.SerializeObject(UserData, Formatting.Indented);
                    }
                    Logger.Debug("Method End");
                    return JsonUser;
                }
                catch (Exception exception)
                {

                    Logger.Debug(exception);
                    throw exception;
                }


            }

        }

       
    }
}
