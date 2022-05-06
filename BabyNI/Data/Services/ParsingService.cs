using BabyNI.Controllers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BabyNI.Data.Services
{
    public class ParsingService
    {

        private readonly IConfiguration configuration;

        public ParsingService(IConfiguration Configuration)
        {
            configuration = Configuration;
        }




        public void ParseFile()
        {
            //string readfilePath = "C:\\Users\\User\\Desktop\\MiniProject\\SOEM1_TN_RADIO_LINK_POWER_20200312_001500.txt";

            string readfilePath = "C:\\Users\\User\\Desktop\\ReceivedFiles\\";
            // string name = Path.GetFileNameWithoutExtension(readfilePath);
            string[] filename = Directory.GetFiles("C:\\Users\\User\\Desktop\\ReceivedFiles");

            DateTime time = DateTime.Now;
            string format = "MMddyyyy-HHmm"; // creating time format to output
            string writefilePath = "C:\\Users\\User\\Desktop\\ParsedFiles\\Parsedfile" + time.ToString(format) + ".txt"; // naming the file with date of creation
            List<string> toBeInserted = new List<string>();
            using (StreamReader sr = new StreamReader(filename[0]))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    toBeInserted.Add(line);
                }
            }//inserting each row to the list

            string[] rows = toBeInserted.ToArray(); //transforming the list to an array of each row
            int rowCount = rows.Length;
            string connectionString = configuration.GetConnectionString("VerticaConnectionString");


            using (StreamWriter sw = new StreamWriter(writefilePath, true)) //// true to append data to the file        
            {

                string[] columns;
                for (int currentRowIndex = 1; currentRowIndex < rowCount; currentRowIndex++)
                {
                    columns = rows[currentRowIndex].Split(","); //transforming the entire row to a temp array 
                    string slot, port; // slot and port will be used in below sections to fill slot & port cols

                    if (columns[2] == "Unreachable Bulk FC" || columns[17] != "-")
                        continue; //skip entire row if second value is unreachable bulk fc or Description col is != "-"

                    //--------------------------reserved for NETWORK_SID generated column------------------------------------------------------col 0 in table
                    string Object = columns[2]; //fetching object val
                    string NeAlias = columns[6]; //fetching NeAlias val
                    string Obj_NeAl = Object + "/" + NeAlias; //Creating the combination to be hashed
                    MD5 hasher = MD5.Create(); //creating the hasher
                    var hashed = hasher.ComputeHash(Encoding.UTF8.GetBytes(Obj_NeAl)); //hashing the combination
                    var ivalue = BitConverter.ToInt32(hashed, 0); //Converting the hashcode to int 
                    sw.Write(ivalue);
                    //--------------------------reserved for NETWORK_SID generated column------------------------------------------------------col 0 in table



                    //--------------------------reserved for DATETIMEKEY generated column------------------------------------------------------col 0 in table




                    string fileName = Path.GetFileNameWithoutExtension(filename[0]); //fetching the file name without its extension
                                                                                     //fileName.Length-15 -> 15 is the count of ddMMyyyy_HHmmss from file name






                    string dateOfFile = fileName.Substring(fileName.Length - 15).Replace("_", " "); //result is the string containing only date-time from file name
                    dateOfFile = dateOfFile.Insert(4, "-");
                    dateOfFile = dateOfFile.Insert(7, "-");
                    dateOfFile = dateOfFile.Insert(13, ":");
                    dateOfFile = dateOfFile.Insert(16, ":");
                    DateTime timeofFile = DateTime.Parse(dateOfFile);
                    sw.Write("," + timeofFile); //DateTime date = Convert.ToDateTime(dateOfFile);



                    //--------------------------reserved for DATETIMEKEY generated column------------------------------------------------------col 0 in table



                    sw.Write("," + float.Parse(columns[1]));//converted from string to float 
                    sw.Write("," + columns[2]);
                    DateTime oDate = Convert.ToDateTime(columns[3]);
                    sw.Write("," + oDate); //converted to datetime 
                    sw.Write("," + int.Parse(columns[4])); //converted from string to int 
                    sw.Write("," + columns[5]);
                    sw.Write("," + columns[6]);
                    sw.Write("," + columns[7]);
                    sw.Write("," + columns[9]);
                    sw.Write("," + columns[10]);
                    sw.Write("," + float.Parse(columns[11], CultureInfo.InvariantCulture.NumberFormat));//converted from string to float 
                    sw.Write("," + float.Parse(columns[12], CultureInfo.InvariantCulture.NumberFormat));//converted from string to float 
                    sw.Write("," + columns[13]);
                    sw.Write("," + float.Parse(columns[14], CultureInfo.InvariantCulture.NumberFormat)); //converted from string to float 
                    sw.Write("," + float.Parse(columns[15], CultureInfo.InvariantCulture.NumberFormat));//converted from string to float 
                    sw.Write("," + columns[17]);


                    //--------------------------reserved for LINK generated column------------------------------------------------------col 18 in table




                    int Indexof_ = columns[2].IndexOf("_");    //searching for _ in the object        
                    string Substring = columns[2].Remove(Indexof_); //Removing the rest of the string starting from _


                    if (Substring.Contains("."))
                    {
                        int c = Substring.IndexOf("/");
                        string b = Substring.Remove(0, c + 1);
                        c = b.IndexOf("/");
                        b = b.Remove(c);
                        c = b.IndexOf(".");
                        b = b.Replace(".", "/");
                        sw.Write("," + b);


                    }
                    else if (Substring.Contains("+"))
                    {
                        string slot_2;
                        int c = Substring.IndexOf("/");
                        Substring = Substring.Remove(0, c + 1);
                        c = Substring.IndexOf("/");
                        Substring = Substring.Remove(c);
                        sw.Write("," + Substring);


                    }
                    else if (!Substring.Contains("+") && !Substring.Contains("."))
                    {
                        int c = Substring.IndexOf("/");
                        Substring = Substring.Remove(0, c + 1);
                        sw.Write("," + Substring);
                    }

                    //--------------------------reserved for LINK generated column------------------------------------------------------col 18 in table



                    //--------------------------reserved for TID generated column------------------------------------------------------col 19 in table

                    string temp;
                    int IndexOf_ = columns[2].IndexOf("_");
                    temp = columns[2].Remove(0, IndexOf_ + 2);
                    string farendtid = temp;     //string used in generating FARENDTID column below in the form xxx__xxx
                    IndexOf_ = temp.IndexOf("_");
                    temp = temp.Remove(IndexOf_);
                    sw.Write("," + temp);

                    //--------------------------reserved for TID generated column------------------------------------------------------col 19 in table


                    //--------------------------reserved for FARENDTID generated column------------------------------------------------------col 20 in table
                    IndexOf_ = farendtid.IndexOf("_");
                    farendtid = farendtid.Substring(IndexOf_ + 2);
                    sw.Write("," + farendtid);
                    //--------------------------reserved for FARENDTID generated column------------------------------------------------------col 20 in table



                    //--------------------------reserved for SLOT generated column------------------------------------------------------col 21 in table
                    if (Substring.Contains("."))
                    {
                        int c = Substring.IndexOf("/");
                        string b = Substring.Remove(0, c + 1);
                        c = b.IndexOf("/");
                        b = b.Remove(c);
                        c = b.IndexOf(".");
                        slot = b.Remove(c);
                        port = b.Substring(c + 1);
                        sw.Write("," + slot);
                        sw.WriteLine("," + port);
                    }


                    else if (Substring.Contains("+"))
                    {
                        string allxpression;
                        string slot_2;
                        int c = Substring.IndexOf("+");
                        slot = Substring.Remove(c);
                        slot_2 = Substring.Substring(c + 1);
                        port = "1";
                        sw.Write("," + slot);
                        sw.WriteLine("," + port);
                        allxpression = ivalue + "," +
                            timeofFile + "," +
                            float.Parse(columns[1]) + "," +
                            columns[2] + "," + oDate + "," +
                            int.Parse(columns[4]) + "," +
                            columns[5] + "," +
                            columns[6] + "," +
                             columns[7] + "," +
                             columns[9] + "," +
                             columns[10] + "," +
                             float.Parse(columns[11], CultureInfo.InvariantCulture.NumberFormat) + "," +
                             float.Parse(columns[12], CultureInfo.InvariantCulture.NumberFormat) + "," +
                             columns[13] + "," +
                             float.Parse(columns[14], CultureInfo.InvariantCulture.NumberFormat) + "," +
                             float.Parse(columns[15], CultureInfo.InvariantCulture.NumberFormat) + "," +
                             columns[17] + "," +
                             Substring + "," +
                             temp + "," +
                             farendtid + "," +
                             slot_2 + "," +
                             port;



                        sw.WriteLine(allxpression);
                    }
                    else if (!Substring.Contains("+") && !Substring.Contains("."))
                    {
                        int c = Substring.IndexOf("/");
                        slot = Substring.Remove(c);
                        port = Substring.Substring(c + 1);
                        sw.Write("," + slot);
                        sw.WriteLine("," + port);



                    }




                }


            }
        }

        Boolean parsed = false;
        public void ParseFile(string path)
        {
            Boolean containsplus = false;
            string name = Path.GetFileNameWithoutExtension(path);
            string connectionString = configuration.GetConnectionString("VerticaConnectionString");
            DateTime time = DateTime.Now;
            string format = "MMddyyyy-HHmmss"; // creating time format to output
            string writefilePath = "C:\\Users\\User\\Desktop\\ParsedFiles\\Parsed_" + name + ".csv"; // naming the file with date of creation
            string writefilePath_ = "C:\\Users\\User\\Desktop\\ParsedFilesTxt\\Parsedfile" + time.ToString(format) + ".txt"; // naming the file with date of creation
            string movepath = "C:\\Users\\User\\Desktop\\ParsedFilesTxt\\Parsedfile" + time.ToString(format) + ".txt";

            

            List<string> toBeInserted = new List<string>();
            using (StreamReader sr = new StreamReader(path))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    toBeInserted.Add(line);
                }
            }//inserting each row to the list

            string[] rows = toBeInserted.ToArray(); //transforming the list to an array of each row
            int rowCount = rows.Length;

            //// true to append data to the file  
            using (StreamWriter sw = new StreamWriter(writefilePath, true))
            {


                string[] columns;
                for (int currentRowIndex = 1; currentRowIndex < rowCount; currentRowIndex++)
                {
                    columns = rows[currentRowIndex].Split(","); //transforming the entire row to a temp array 
                    string slot, port; // slot and port will be used in below sections to fill slot & port cols

                    if (columns[2] == "Unreachable Bulk FC" || columns[17] != "-")
                    {

                        continue; //skip entire row if second value is unreachable bulk fc or Description col is != "-"

                    }
                    parsed = true;





                    //--------------------------reserved for NETWORK_SID generated column------------------------------------------------------col 0 in table
                    string Object = columns[2]; //fetching object val
                    string NeAlias = columns[6]; //fetching NeAlias val
                    string Obj_NeAl = Object + "/" + NeAlias; //Creating the combination to be hashed
                    MD5 hasher = MD5.Create(); //creating the hasher
                    var hashed = hasher.ComputeHash(Encoding.UTF8.GetBytes(Obj_NeAl)); //hashing the combination
                    var ivalue = BitConverter.ToInt32(hashed, 0); //Converting the hashcode to int 
                    sw.Write(ivalue);
                    //--------------------------reserved for NETWORK_SID generated column------------------------------------------------------col 0 in table










                    //--------------------------reserved for DATETIMEKEY generated column------------------------------------------------------col 0 in table




                    string fileName = Path.GetFileNameWithoutExtension(path); //fetching the file name without its extension
                                                                              //fileName.Length-15 -> 15 is the count of ddMMyyyy_HHmmss from file name
                    string dateOfFile = fileName.Substring(fileName.Length - 15).Replace("_", " "); //result is the string containing only date-time from file name
                    dateOfFile = dateOfFile.Insert(4, "-");
                    dateOfFile = dateOfFile.Insert(7, "-");
                    dateOfFile = dateOfFile.Insert(13, ":");
                    dateOfFile = dateOfFile.Insert(16, ":");
                    DateTime timeofFile = DateTime.Parse(dateOfFile);
                    sw.Write("," + timeofFile); //DateTime date = Convert.ToDateTime(dateOfFile);



                    //--------------------------reserved for DATETIMEKEY generated column------------------------------------------------------col 0 in table







                    sw.Write("," + float.Parse(columns[1]));//converted from string to float 
                    sw.Write("," + columns[2]);
                    DateTime oDate = Convert.ToDateTime(columns[3]);
                    sw.Write("," + oDate); //converted to datetime 
                    sw.Write("," + int.Parse(columns[4])); //converted from string to int 
                    sw.Write("," + columns[5]);
                    sw.Write("," + columns[6]);
                    sw.Write("," + columns[7]);
                    sw.Write("," + columns[9]);
                    sw.Write("," + columns[10]);
                    sw.Write("," + float.Parse(columns[11], CultureInfo.InvariantCulture.NumberFormat));//converted from string to float 
                    sw.Write("," + float.Parse(columns[12], CultureInfo.InvariantCulture.NumberFormat));//converted from string to float 
                    sw.Write("," + columns[13]);
                    sw.Write("," + float.Parse(columns[14], CultureInfo.InvariantCulture.NumberFormat)); //converted from string to float 
                    sw.Write("," + float.Parse(columns[15], CultureInfo.InvariantCulture.NumberFormat));//converted from string to float 
                    sw.Write("," + columns[17]);









                    //--------------------------reserved for LINK generated column------------------------------------------------------col 18 in table

                    int Indexof_ = columns[2].IndexOf("_");    //searching for _ in the object        
                    string Substring = columns[2].Remove(Indexof_); //Removing the rest of the string starting from _


                    if (Substring.Contains("."))
                    {
                        int c = Substring.IndexOf("/");
                        string b = Substring.Remove(0, c + 1);
                        c = b.IndexOf("/");
                        b = b.Remove(c);
                        c = b.IndexOf(".");
                        b = b.Replace(".", "/");
                        sw.Write("," + b);


                    }
                    else if (Substring.Contains("+"))
                    {
                        string slot_2;
                        int c = Substring.IndexOf("/");
                        Substring = Substring.Remove(0, c + 1);
                        sw.Write("," + Substring);
                        //c = Substring.IndexOf("/");
                        //Substring = Substring.Remove(c);



                    }
                    else if (!Substring.Contains("+") && !Substring.Contains("."))
                    {
                        int c = Substring.IndexOf("/");
                        Substring = Substring.Remove(0, c + 1);
                        sw.Write("," + Substring);
                    }

                    //--------------------------reserved for LINK generated column------------------------------------------------------col 18 in table









                    //--------------------------reserved for TID generated column------------------------------------------------------col 19 in table

                    string temp;
                    int IndexOf_ = columns[2].IndexOf("_");
                    temp = columns[2].Remove(0, IndexOf_ + 2);
                    string farendtid = temp;     //string used in generating FARENDTID column below in the form xxx__xxx
                    IndexOf_ = temp.IndexOf("_");
                    temp = temp.Remove(IndexOf_);
                    sw.Write("," + temp);

                    //--------------------------reserved for TID generated column------------------------------------------------------col 19 in table








                    //--------------------------reserved for FARENDTID generated column------------------------------------------------------col 20 in table
                    IndexOf_ = farendtid.IndexOf("_");
                    farendtid = farendtid.Substring(IndexOf_ + 2);
                    sw.Write("," + farendtid);
                    //--------------------------reserved for FARENDTID generated column------------------------------------------------------col 20 in table









                    //--------------------------reserved for SLOT generated column------------------------------------------------------col 21 in table
                    if (Substring.Contains("."))
                    {
                        int c = Substring.IndexOf("/");
                        string b = Substring.Remove(0, c + 1);
                        c = b.IndexOf("/");
                        b = b.Remove(c);
                        c = b.IndexOf(".");
                        slot = b.Remove(c);
                        port = b.Substring(c + 1);
                        sw.Write("," + slot);
                        sw.WriteLine("," + port);
                    }


                    else if (Substring.Contains("+"))
                    {
                        string allxpression;
                        string slot_2;
                        int c = Substring.IndexOf("+");
                        slot = Substring.Remove(c);
                        slot_2 = Substring.Substring(c + 1);
                        c = slot_2.IndexOf("/");
                        slot_2 = slot_2.Remove(c);
                        port = "1";
                        sw.Write("," + slot);
                        sw.WriteLine("," + port);
                        allxpression = ivalue + "," +
                            timeofFile + "," +
                            float.Parse(columns[1]) + "," +
                            columns[2] + "," + oDate + "," +
                            int.Parse(columns[4]) + "," +
                            columns[5] + "," +
                            columns[6] + "," +
                             columns[7] + "," +
                             columns[9] + "," +
                             columns[10] + "," +
                             float.Parse(columns[11], CultureInfo.InvariantCulture.NumberFormat) + "," +
                             float.Parse(columns[12], CultureInfo.InvariantCulture.NumberFormat) + "," +
                             columns[13] + "," +
                             float.Parse(columns[14], CultureInfo.InvariantCulture.NumberFormat) + "," +
                             float.Parse(columns[15], CultureInfo.InvariantCulture.NumberFormat) + "," +
                             columns[17] + "," +
                             Substring + "," +
                             temp + "," +
                             farendtid + "," +
                             slot_2 + "," +
                             port;



                        sw.WriteLine(allxpression);
                    }

                    else if (!Substring.Contains("+") && !Substring.Contains("."))
                    {
                        int c = Substring.IndexOf("/");
                        slot = Substring.Remove(c);
                        port = Substring.Substring(c + 1);
                        sw.Write("," + slot);
                        sw.WriteLine("," + port);



                    }



                }
                ////////////////////////////////////////////////////////LOGGING////////////////////////////////////////////////////////

                string filenamewithoutext = Path.GetFileNameWithoutExtension(path);
                string datepart_name = filenamewithoutext.Substring(filenamewithoutext.Length - 15).Replace("_", " "); //result is the string containing only date-time from file name
                datepart_name = datepart_name.Insert(4, "-");
                datepart_name = datepart_name.Insert(7, "-");
                datepart_name = datepart_name.Insert(13, ":");
                datepart_name = datepart_name.Insert(16, ":");
                DateTime TimeofFile = DateTime.Parse(datepart_name);
                DateTime activitytime = DateTime.Now;
                string successactivity = "Done";
                string failedactivity = "Failed";
                string activity = "Parsing";
                string successquery = $"INSERT INTO Logs (Filename,TimeofFile,Activity,Time_of_activity,Comment) VALUES ('{filenamewithoutext}','{TimeofFile}','{activity}','{activitytime}','{successactivity}')";
                string failedquery = $"INSERT INTO Logs (Filename,TimeofFile,Activity,Time_of_activity,Comment) VALUES ('{filenamewithoutext}','{TimeofFile}','{activity}','{activitytime}','{failedactivity}')";


                OdbcCommand successcommand = new OdbcCommand(successquery);
                OdbcCommand failedcommand = new OdbcCommand(failedquery);
                using (OdbcConnection conn = new OdbcConnection(connectionString))
                {
                    if (parsed)
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


            File.Copy(path, movepath);
            File.Copy(writefilePath, writefilePath_, true);


        }
    }
}






////////LOADED FILE AND WRITTEN TO A FILE USING ONLY ONE USING STREAMWRITER WITHOUT KEEPING FILE BUSY///////////////////////////
/*
            List<string> toBeInserted = new List<string>();
            using (StreamReader sr = new StreamReader(path))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    toBeInserted.Add(line);
                }
            }//inserting each row to the list

            string[] rows = toBeInserted.ToArray(); //transforming the list to an array of each row
            int rowCount = rows.Length;

            //// true to append data to the file  
            List<string> rowlist = new List<string>();
            List<string> templist = new List<string>();

            string[] columns;
            for (int currentRowIndex = 1; currentRowIndex < rowCount; currentRowIndex++)
            {
                rowlist.Clear();
                templist.Clear();
                columns = rows[currentRowIndex].Split(","); //transforming the entire row to a temp array 
                string slot, port; // slot and port will be used in below sections to fill slot & port cols

                if (columns[2] == "Unreachable Bulk FC" || columns[17] != "-")
                {
                    parsed = false;
                    continue; //skip entire row if second value is unreachable bulk fc or Description col is != "-"
                }
                parsed = true;





                //--------------------------reserved for NETWORK_SID generated column------------------------------------------------------col 0 in table
                string Object = columns[2]; //fetching object val
                string NeAlias = columns[6]; //fetching NeAlias val
                string Obj_NeAl = Object + "/" + NeAlias; //Creating the combination to be hashed
                MD5 hasher = MD5.Create(); //creating the hasher
                var hashed = hasher.ComputeHash(Encoding.UTF8.GetBytes(Obj_NeAl)); //hashing the combination
                var ivalue = BitConverter.ToInt32(hashed, 0); //Converting the hashcode to int 
                                                              // sw.Write(ivalue);
                rowlist.Add(ivalue + ",");
                //--------------------------reserved for NETWORK_SID generated column------------------------------------------------------col 0 in table










                //--------------------------reserved for DATETIMEKEY generated column------------------------------------------------------col 0 in table




                string fileName = Path.GetFileNameWithoutExtension(path); //fetching the file name without its extension
                                                                          //fileName.Length-15 -> 15 is the count of ddMMyyyy_HHmmss from file name
                string dateOfFile = fileName.Substring(fileName.Length - 15).Replace("_", " "); //result is the string containing only date-time from file name
                dateOfFile = dateOfFile.Insert(4, "-");
                dateOfFile = dateOfFile.Insert(7, "-");
                dateOfFile = dateOfFile.Insert(13, ":");
                dateOfFile = dateOfFile.Insert(16, ":");
                DateTime timeofFile = DateTime.Parse(dateOfFile);
                //tobewritten += timeofFile + ",";
                rowlist.Add(timeofFile + ",");
                //sw.Write("," + timeofFile); //DateTime date = Convert.ToDateTime(dateOfFile);



                //--------------------------reserved for DATETIMEKEY generated column------------------------------------------------------col 0 in table

                for (int i = 1; i <= 17; i++)
                {
                    if (i == 8 || i == 16)
                        continue;
                    rowlist.Add(columns[i] + ",");

                }

                DateTime oDate = Convert.ToDateTime(columns[3]);
                //--------------------------reserved for LINK generated column------------------------------------------------------col 18 in table

                int Indexof_ = columns[2].IndexOf("_");    //searching for _ in the object        
                string Substring = columns[2].Remove(Indexof_); //Removing the rest of the string starting from _


                if (Substring.Contains("."))
                {
                    int c = Substring.IndexOf("/");
                    string b = Substring.Remove(0, c + 1);
                    c = b.IndexOf("/");
                    b = b.Remove(c);
                    c = b.IndexOf(".");
                    b = b.Replace(".", "/");
                    rowlist.Add(b + ",");



                }
                else if (Substring.Contains("+"))
                {
                    string slot_2;
                    int c = Substring.IndexOf("/");
                    Substring = Substring.Remove(0, c + 1);
                    rowlist.Add(Substring + ",");


                }
                else if (!Substring.Contains("+") && !Substring.Contains("."))
                {
                    int c = Substring.IndexOf("/");
                    Substring = Substring.Remove(0, c + 1);
                    rowlist.Add(Substring + ",");
                }

                //--------------------------reserved for LINK generated column------------------------------------------------------col 18 in table









                //--------------------------reserved for TID generated column------------------------------------------------------col 19 in table

                string temp;
                int IndexOf_ = columns[2].IndexOf("_");
                temp = columns[2].Remove(0, IndexOf_ + 2);
                string farendtid = temp;     //string used in generating FARENDTID column below in the form xxx__xxx
                IndexOf_ = temp.IndexOf("_");
                temp = temp.Remove(IndexOf_);
                rowlist.Add(temp + ",");
                //sb.Append(temp + ",");
                //tobewritten += temp + ",";
                //sw.Write("," + temp);

                //--------------------------reserved for TID generated column------------------------------------------------------col 19 in table








                //--------------------------reserved for FARENDTID generated column------------------------------------------------------col 20 in table
                IndexOf_ = farendtid.IndexOf("_");
                farendtid = farendtid.Substring(IndexOf_ + 2);
                //sb.Append(farendtid + ",");
                rowlist.Add(farendtid + ",");
                //tobewritten += farendtid + ",";
                //sw.Write("," + farendtid);
                //--------------------------reserved for FARENDTID generated column------------------------------------------------------col 20 in table









                //--------------------------reserved for SLOT generated column------------------------------------------------------col 21 in table
                if (Substring.Contains("."))
                {
                    int c = Substring.IndexOf("/");
                    string b = Substring.Remove(0, c + 1);
                    c = b.IndexOf("/");
                    b = b.Remove(c);
                    c = b.IndexOf(".");
                    slot = b.Remove(c);
                    port = b.Substring(c + 1);
                    rowlist.Add(slot + "," + port + "\n");
                    // sb.Append(slot + ","+ port+"\n");
                    // tobewritten += slot + "," + port;
                    //sw.Write("," + slot);
                    //sw.WriteLine("," + port);
                }

                else if (!Substring.Contains("+") && !Substring.Contains("."))
                {
                    int c = Substring.IndexOf("/");
                    slot = Substring.Remove(c);
                    port = Substring.Substring(c + 1);
                    rowlist.Add(slot + "," + port + "\n");
                    //sb.Append(slot+","+port+"\n");
                    //tobewritten += slot + "," + port;
                    //sw.Write("," + slot);
                    //sw.WriteLine("," + port);
                }
                else if (Substring.Contains("+"))
                {

                    string slot_2;
                    int c = Substring.IndexOf("+");
                    slot = Substring.Remove(c);//     18+19/1

                    slot_2 = Substring.Remove(0, c + 1);
                    int ndexofdash = slot_2.IndexOf("/");
                    slot_2 = slot_2.Remove(ndexofdash);
                    port = "1";
                    rowlist.Add(slot + "," + port + "\n");

                    templist.Add(ivalue + ",");
                    templist.Add(timeofFile + ",");
                    for (int i = 1; i <= 17; i++)
                    {
                        if (i == 8 || i == 16)
                            continue;
                        templist.Add(columns[i] + ",");

                    }
                    templist.Add(Substring + ",");
                    templist.Add(temp + ",");
                    templist.Add(farendtid + ",");
                    templist.Add(slot_2 + ",");
                    templist.Add(port + "\n");
                    using (StreamWriter sr = new StreamWriter(writefilePath, true))
                    {
                        for (int i = 0; i <= 21; i++)
                        {
                            sr.Write(templist[i]);
                        }

                    }
                    for (int lc = 0; lc < 21; lc++)
                    {
                        Console.Write(rowlist[lc]);
                    }
                    for (int lc = 0; lc <= 21; lc++)
                    {
                        Console.Write(templist[lc]);
                    }


                    //     port + "\n");
                    containsplus = true;



                }

                using (StreamWriter sr = new StreamWriter(writefilePath, true))
                {

                    for (int i = 0; i < 21; i++)
                    {
                        sr.Write(rowlist[i]);
                    }

                }

            }
            File.Copy(path, movepath);
            File.Copy(writefilePath, writefilePath_, true);
        }*/


