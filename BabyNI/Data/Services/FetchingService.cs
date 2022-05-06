using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;

namespace BabyNI.Data.Services
{
    public class FetchingService
    {
        private readonly IConfiguration configuration;
        public FetchingService(IConfiguration Configuration)
        {
            configuration = Configuration;
        }

        public List<Dictionary<string,string>> GetDailyData()
        {
            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
            Dictionary<string, string> column;
            //string sqlQuery = "SELECT TIME,MAXRXLEVEL FROM TRANS_MW_AGG_SLOT_HOURLY";
           string sqlQuery = "SELECT TIME,NETYPE,NEALIAS,LINK,MAXRXLEVEL,MAXTXLEVEL,RSL_DEVIATION_db FROM TRANS_MW_AGG_SLOT_DAILY GROUP BY TIME,NETYPE,NEALIAS,LINK,MAXRXLEVEL,MAXTXLEVEL,RSL_DEVIATION_db";
            string connectionString = "Driver={Vertica};database=mario_chayeb;port=5433;server=10.10.4.171;uid=mario_chayeb;pwd=Mario978ChayebYuvo";
            OdbcCommand command = new OdbcCommand(sqlQuery);
            OdbcConnection con = new OdbcConnection(connectionString);
            command.Connection = con;

            try
            {
                con.Open();

                OdbcDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {    //Every new row will create a new dictionary that holds the columns
                    column = new Dictionary<string, string>();

                    column["TIME"] = reader["TIME"].ToString();
                    column["NETYPE"] = reader["NETYPE"].ToString();
                    column["NEALIAS"] = reader["NEALIAS"].ToString();
                    column["LINK"] = reader["LINK"].ToString();
                    column["MAXRXLEVEL"] = reader["MAXRXLEVEL"].ToString();
                    column["MAXTXLEVEL"] = reader["MAXTXLEVEL"].ToString();
                    column["RSL_DEVIATION_db"] = reader["RSL_DEVIATION_db"].ToString();
                    rows.Add(column); //Place the dictionary into the list
                }
                reader.Close();
            }
            //catch (Exception ex)
            //{
            //    //If an exception occurs, write it to the console
            //    Console.WriteLine(ex.ToString());
            //}
            finally
            {
                con.Close();
            }

            return rows;
        }


        public List<Dictionary<string, string>> GetHourlyData()
        {
            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
            Dictionary<string, string> column;
            //string sqlQuery = "SELECT TIME,MAXRXLEVEL FROM TRANS_MW_AGG_SLOT_HOURLY";
            string sqlQuery = "SELECT TIME,NETYPE,NEALIAS,LINK,MAXRXLEVEL,MAXTXLEVEL,RSL_DEVIATION_db FROM TRANS_MW_AGG_SLOT_HOURLY GROUP BY TIME,NETYPE,NEALIAS,LINK,MAXRXLEVEL,MAXTXLEVEL,RSL_DEVIATION_db";
            string connectionString = configuration.GetConnectionString("VerticaConnectionString");
            OdbcCommand command = new OdbcCommand(sqlQuery);
            OdbcConnection con = new OdbcConnection(connectionString);
            command.Connection = con;

            try
            {
                con.Open();

                OdbcDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {    //Every new row will create a new dictionary that holds the columns
                    column = new Dictionary<string, string>();

                    column["TIME"] = reader["TIME"].ToString();
                    column["NETYPE"] = reader["NETYPE"].ToString();
                    column["NEALIAS"] = reader["NEALIAS"].ToString();
                    column["LINK"] = reader["LINK"].ToString();
                    column["MAXRXLEVEL"] = reader["MAXRXLEVEL"].ToString();
                    column["MAXTXLEVEL"] = reader["MAXTXLEVEL"].ToString();
                    column["RSL_DEVIATION_db"] = reader["RSL_DEVIATION_db"].ToString();
                    rows.Add(column); //Place the dictionary into the list
                }
                reader.Close();
            }
            finally
            {
                con.Close();
            }

            return rows;
        }

             public List<Dictionary<string, string>> GetHourlyData_link_slot(string NE_ALIAS)
        {
            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
            Dictionary<string, string> column;
           
            string sqlQuery = $"SELECT TIME,NETYPE,NEALIAS,LINK,MAXRXLEVEL,MAXTXLEVEL,RSL_DEVIATION_db FROM TRANS_MW_AGG_SLOT_HOURLY WHERE NEALIAS='{NE_ALIAS}' GROUP BY TIME,NETYPE,NEALIAS,LINK,MAXRXLEVEL,MAXTXLEVEL,RSL_DEVIATION_db";
            //string sqlQuery = $"SELECT TIME,NETYPE,NEALIAS,LINK,MAXRXLEVEL,MAXTXLEVEL,RSL_DEVIATION_db FROM TRANS_MW_AGG_SLOT_HOURLY WHERE NEALIAS='TN-ALT13.1' AND LINK='16/1' GROUP BY TIME,NETYPE,NEALIAS,LINK,MAXRXLEVEL,MAXTXLEVEL,RSL_DEVIATION_db";
            string connectionString = "Driver={Vertica};database=mario_chayeb;port=5433;server=10.10.4.171;uid=mario_chayeb;pwd=Mario978ChayebYuvo";
            OdbcCommand command = new OdbcCommand(sqlQuery);
            OdbcConnection con = new OdbcConnection(connectionString);
            command.Connection = con;

            try
            {
                con.Open();

                OdbcDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {    //Every new row will create a new dictionary that holds the columns
                    column = new Dictionary<string, string>();

                    column["TIME"] = reader["TIME"].ToString();
                    column["NETYPE"] = reader["NETYPE"].ToString();
                    column["NEALIAS"] = reader["NEALIAS"].ToString();
                    column["LINK"] = reader["LINK"].ToString();
                    column["MAXRXLEVEL"] = reader["MAXRXLEVEL"].ToString();
                    column["MAXTXLEVEL"] = reader["MAXTXLEVEL"].ToString();
                    column["RSL_DEVIATION_db"] = reader["RSL_DEVIATION_db"].ToString();
                    rows.Add(column); //Place the dictionary into the list
                }
                reader.Close();
            }
            finally
            {
                con.Close();
            }

            return rows;
        }


        public List<Dictionary<string, string>> Getdistinctlinks(string NE_ALIAS)
        {
            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
            Dictionary<string, string> column;
            string sqlquery = $"SELECT DISTINCT LINK FROM TRANS_MW_AGG_SLOT_HOURLY WHERE NEALIAS='{NE_ALIAS}'";
            string connectionString = configuration.GetConnectionString("VerticaConnectionString");
            OdbcCommand command = new OdbcCommand(sqlquery);
            OdbcConnection con = new OdbcConnection(connectionString);
            command.Connection = con;

            try
            {
                con.Open();

                OdbcDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {    //Every new row will create a new dictionary that holds the columns
                    column = new Dictionary<string, string>();

                   
                    column["LINK"] = reader["LINK"].ToString();
                  
                    rows.Add(column); //Place the dictionary into the list
                }
                reader.Close();
            }
            finally
            {
                con.Close();
            }

            return rows;
        }
    }
}
