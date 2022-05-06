
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BabyNI.Data.Services
{
    public class LoadService 
    {
        Boolean loaded;
        private readonly IConfiguration configuration;

        public AggregatingService _aggregatingService;//remove in case crash 

        public LoadService(IConfiguration Configuration)
        {
            configuration = Configuration;

            _aggregatingService = new AggregatingService(Configuration);
        }



        public LoadService(IConfiguration Configuration, AggregatingService aggregatingService)  //remove in case crash
        {
            configuration = Configuration;
            _aggregatingService = aggregatingService;
        }


        //-----------------------------Function to copy Data From file to database ---------------------------------------
        public void CopyData()
        {
            DateTime time = DateTime.Now;
            string format = "MMddyyyy-HHmm"; // creating time format to output
            string loadpath = "'C:\\Users\\User\\Desktop\\ParsedFiles\\Parsedfile" + time.ToString(format) + ".txt'";


            string connectionString = configuration.GetConnectionString("VerticaConnectionString");
            string query = "Copy TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER from local " + loadpath + " delimiter ',' REJECTED DATA 'C:\\Users\\User\\Desktop\\RejectedData.txt direct";
            OdbcCommand command = new OdbcCommand(query);
            using (OdbcConnection conn = new OdbcConnection(connectionString))
            {
                command.Connection = conn;
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

       
        public void CopyData(string path)
        {
            string filenamewithoutext = Path.GetFileNameWithoutExtension(path);
            string datepart_name = filenamewithoutext.Substring(filenamewithoutext.Length - 15).Replace("-", " "); //result is the string containing only date-time from file name
            //string datepart_name = filenamewithoutext.Substring(filenamewithoutext.Length - 15).Replace("-", " "); //result is the string containing only date-time from file name
            //datepart_name = datepart_name.Insert(2, "/");
            //datepart_name = datepart_name.Insert(5, "/");
            //datepart_name = datepart_name.Insert(13, ":");
            //datepart_name = datepart_name.Insert(16, ":");
            MD5 hasher = MD5.Create(); //creating the hasher
            var hashed = hasher.ComputeHash(Encoding.UTF8.GetBytes(filenamewithoutext));
            var ivalue = BitConverter.ToInt32(hashed, 0);
           // DateTime TimeofFile = DateTime.Parse(datepart_name);
            DateTime activitytime = DateTime.Now;
            string connectionString = configuration.GetConnectionString("VerticaConnectionString");



            DateTime time = DateTime.Now;
            string format = "MMddyyyy-HHmmss"; // creating time format to output
                                               // string loadpath = "' C:\\Users\\User\\Desktop\\ParsedFiles\\Parsedfile" + time.ToString(format) + ".txt'";
            string movepath = "C:\\Users\\User\\Desktop\\LoadedFiles\\LoadedFile" + time.ToString(format) + ".txt";




            // string query = "Copy TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER from local '" + path +"'" +" delimiter ',' REJECTED DATA ' C:\\Users\\User\\Desktop\\RejectedData.txt direct";

            string query = "Copy TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER from local '" + path + "'" + " delimiter ',' direct";
            string checkforrecord = " SELECT * FROM Logs WHERE Activity='Loading' AND Key="+ ivalue;




            OdbcCommand command = new OdbcCommand(query);
            OdbcCommand checker = new OdbcCommand(checkforrecord);


            using (OdbcConnection conn = new OdbcConnection(connectionString))
            {
                checker.Connection = conn;
                conn.Open();
                OdbcDataReader dr = checker.ExecuteReader();
                if (!dr.HasRows)
                {
                    command.Connection = conn;
                  //  conn.Open();
                    loaded = true;
                    command.ExecuteNonQuery();
                    conn.Close();
                }

            }

            File.Copy(path, movepath);

           


            LogInfo(path);





            _aggregatingService.AggregateData();

           

        }

        public void LogInfo(string path)
        {
            string namewithoutext = Path.GetFileNameWithoutExtension(path);

            DateTime activitytime = DateTime.Now;
            //string TimeofFile = namewithoutext.Substring(namewithoutext.Length - 15).Replace("-", " "); //result is the string containing only date-time from file name
            //TimeofFile = TimeofFile.Insert(2, "/");
            //TimeofFile = TimeofFile.Insert(5, "/");
            //TimeofFile = TimeofFile.Insert(13, ":");
            //TimeofFile = TimeofFile.Insert(16, ":");
            string successactivity = "File Loaded Successfully";
            string failedactivity = "Could not Load File";
            string activity = "Loading";
            MD5 hasher = MD5.Create(); //creating the hasher
            var hashed = hasher.ComputeHash(Encoding.UTF8.GetBytes(namewithoutext));
            var ivalue = BitConverter.ToInt32(hashed, 0);
            //string successquery = $"INSERT INTO Logs (Filename,TimeofFile,Activity,Time_of_activity,Comment,Key) VALUES ('{namewithoutext}','{TimeofFile}','{activity}','{activitytime}','{successactivity}',{ivalue})";
            //string failedquery = $"INSERT INTO Logs (Filename,TimeofFile,Activity,Time_of_activity,Comment,Key) VALUES ('{namewithoutext}','{TimeofFile}','{activity}','{activitytime}','{failedactivity}',{ivalue})";
            string successquery = $"INSERT INTO Logs (Filename,Activity,Time_of_activity,Comment,Key) VALUES ('{namewithoutext}','{activity}','{activitytime}','{successactivity}',{ivalue})";
            string failedquery = $"INSERT INTO Logs (Filename,Activity,Time_of_activity,Comment,Key) VALUES ('{namewithoutext}','{activity}','{activitytime}','{failedactivity}',{ivalue})";
            string connectionString = configuration.GetConnectionString("VerticaConnectionString");

            OdbcCommand successcommand = new OdbcCommand(successquery);
            OdbcCommand failedcommand = new OdbcCommand(failedquery);


            FileInfo fi = new FileInfo(path);
            long filesize = fi.Length;


            using (OdbcConnection conn = new OdbcConnection(connectionString))
            {
                if (filesize != 0)
                {
                    successcommand.Connection = conn;
                    conn.Open();
                    successcommand.ExecuteNonQuery();
                    conn.Close();

                }

                else
                {
                    failedcommand.Connection = conn;
                    conn.Open();
                    failedcommand.ExecuteNonQuery();
                    conn.Close();
                }
            }


        }



        public void ReExecuteLoader(string custompath)
        {
            CopyData(custompath);
        }
    }
}












