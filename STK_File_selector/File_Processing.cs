//File_Processing.cs

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Text;
using System.Data;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.InteropServices;
using AGI.STKUtil;
using AGI.STKObjects;
using AGI.Ui.Application;
using System.Drawing;
using AGI.STKVgt;


namespace STK_File_selector
{
    partial class NPAS_To_STK
    {



        private int numberoflines(string filename)
        {
            int counter = 0;

            StreamReader reader = File.OpenText(filename);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                ++counter;
            }
            reader.Close();
            return counter;
        }
        private int numberoflines(string filename, bool stationfile)
        {
            int counter = 0;

            StreamReader reader = File.OpenText(filename);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                line = Regex.Replace(line, @"\s+", " ");
                string[] items = line.Split();
                int errorCounter = Regex.Matches(items[0], @"[a-zA-z]").Count;
                if (errorCounter > 0)
                {
                    ++counter;
                }
            }
            reader.Close();
            return counter;
        }



        //loads stations and TDRS into the station file #TODO should upgrade to use a configuration file to load TDRSs;
        public void load_Station_File()
        {
            var format = new NumberFormatInfo();
            format.NegativeSign = "-";
            format.NumberDecimalSeparator = ".";

            stationcount = numberoflines(NPAS_Mask_File, true);
            stationdata = new station_str[stationcount + 12];

            StreamReader reader = File.OpenText(NPAS_Mask_File);

            string line;
            int pos = 0;
            while ((line = reader.ReadLine()) != null)
            {
                line = Regex.Replace(line, @"\s+", " ");
                string[] items = line.Split();

                int errorCounter = Regex.Matches(items[0], @"[a-zA-z]").Count;
                if (errorCounter > 0)
                {
                    try
                    {
                        stationdata[pos].name = items[0];
                        stationdata[pos].lat = double.Parse(items[1], format);
                        stationdata[pos].lon = double.Parse(items[2], format);
                        stationdata[pos].altitude = (double.Parse(items[3], format) / 1000);
                        stationdata[pos].used = false;
                        stationdata[pos].tdrs = false;
                        ++pos;
                    }
                    catch
                    {
                        MessageBox.Show("Error file format invalid");
                        return;
                    }
                }

            }
            reader.Close();

            stationdata[stationcount].name = "T012";
            stationdata[stationcount].lon = 12;
            stationdata[stationcount].used = false;
            stationdata[stationcount++].tdrs = true;
            stationdata[stationcount].name = "T018";
            stationdata[stationcount].lon = 18;
            stationdata[stationcount].used = false;
            stationdata[stationcount++].tdrs = true;
            stationdata[stationcount].name = "T041";
            stationdata[stationcount].lon = 40.9;
            stationdata[stationcount].used = false;
            stationdata[stationcount++].tdrs = true;
            stationdata[stationcount].name = "T046";
            stationdata[stationcount].lon = 46;
            stationdata[stationcount].used = false;
            stationdata[stationcount++].tdrs = true;
            stationdata[stationcount].name = "T049";
            stationdata[stationcount].lon = 49;
            stationdata[stationcount].used = false;
            stationdata[stationcount++].tdrs = true;
            stationdata[stationcount].name = "T062";
            stationdata[stationcount].lon = 62.4;
            stationdata[stationcount].used = false;
            stationdata[stationcount++].tdrs = true;
            stationdata[stationcount].name = "T150";
            stationdata[stationcount].lon = 150;
            stationdata[stationcount].used = false;
            stationdata[stationcount++].tdrs = true;
            stationdata[stationcount].name = "T168";
            stationdata[stationcount].lon = 167.5;
            stationdata[stationcount].used = false;
            stationdata[stationcount++].tdrs = true;
            stationdata[stationcount].name = "T171";
            stationdata[stationcount].lon = 171;
            stationdata[stationcount].used = false;
            stationdata[stationcount++].tdrs = true;
            stationdata[stationcount].name = "T174";
            stationdata[stationcount].lon = 174;
            stationdata[stationcount].used = false;
            stationdata[stationcount++].tdrs = true;
            stationdata[stationcount].name = "T271";
            stationdata[stationcount].lon = 271;
            stationdata[stationcount].used = false;
            stationdata[stationcount++].tdrs = true;
            stationdata[stationcount].name = "T275";
            stationdata[stationcount].lon = 275;
            stationdata[stationcount].used = false;
            stationdata[stationcount++].tdrs = true;
        }

        public void load_timeline_file()
        {
            tlcount = numberoflines(NPAS_Timeline_File);
            //StreamReader reader = File.OpenText(NPAS_Timeline_File);
            string line;


            tldata = new timeline_str[tlcount];
            StreamReader reader = File.OpenText(NPAS_Timeline_File);
            int tldatapos = 0;
            while ((line = reader.ReadLine()) != null)
            {
                Console.Write("line=" + line + "\n");
                line = Regex.Replace(line, @"\s+", " ");
                string[] items = line.Split(' ');

                try
                {
                    tldata[tldatapos].mis = items[0];
                    tldata[tldatapos].sta = items[1];
                    tldata[tldatapos].band = items[2];
                    tldata[tldatapos].link = items[3];
                    tldata[tldatapos].aosdate = items[4];
                    tldata[tldatapos].aostime = items[5];
                    tldata[tldatapos].losdate = items[6];
                    tldata[tldatapos].lostime = items[7];
                    tldata[tldatapos].duration = items[8];
                    ++tldatapos;
                }
                catch
                {
                    MessageBox.Show("Error file format invalid");
                    return;
                }
            }

            reader.Close();
            //tldata.GroupBy(timeline_str => timeline_str.mis);
            //sorts timeline file by mission
            Array.Sort<timeline_str>(tldata, (x, y) => x.mis.CompareTo(y.mis));
            //check if mission exists
            bool a = Array.Exists<timeline_str>(tldata, x => x.mis == tldata[0].mis);

            Console.Write("Dumping structure data \n\n");
            for (int i = 0; i < tlcount; i++)
            {
                Console.Write("TLdata=" + tldata[i].mis + " " + tldata[i].sta + " \n");

            }

        }

        public void load_Old_orbit_file()
        {

            orbitmissioncount = numberoflines(NPAS_Orbit_File);
            orbitdata = new orbit_str[orbitmissioncount];
            //string[] missionnumbers = new string[orbitmissioncount];

            var format = new NumberFormatInfo();
            format.NegativeSign = "-";
            format.NumberDecimalSeparator = ".";


            StreamReader reader = File.OpenText(NPAS_Orbit_File);
            string line;
            int pos = 0;
            bool orbitend = false;
            while ((line = reader.ReadLine()) != null)
            {
                Console.Write("line=" + line + "\n");
                if (line == "end_orbit_data")
                {
                    orbitend = true;
                }
                else if (orbitend == false)
                {
                    line = Regex.Replace(line, @"\s+", " ");
                    string[] items = line.Split(' ');

                    if (items.Length < 13)
                    {
                        MessageBox.Show("Error file format invalid");
                        return;
                    }

                    try
                    {
                        orbitdata[pos].misnum = int.Parse(items[0]); // missionnumbers[i]);
                        orbitdata[pos].name = items[1]; // missions[i];
                        orbitdata[pos].epoch = items[2]; // epochs[i];
                        orbitdata[pos].epoch_time = items[3]; // epoch_times[i];
                                                              // orbitdata[pos].centerbody = items[4]; //centerbodys[i];
                        orbitdata[pos].sma = double.Parse(items[7], format);
                        orbitdata[pos].ecc = double.Parse(items[8], format);
                        orbitdata[pos].inc = double.Parse(items[9], format);
                        orbitdata[pos].raan = double.Parse(items[10], format);
                        orbitdata[pos].aop = double.Parse(items[11], format); ;
                        orbitdata[pos].ma = double.Parse(items[12], format);
                        orbitdata[pos].cod_id = int.Parse(items[13]);
                        orbitdata[pos].efileused = false;
                        if (orbitdata[pos].misnum == orbitdata[pos].cod_id)
                        {
                            Console.Write("mission number == cod_id\n");
                            missionindex[orbitdata[pos].misnum] = orbitdata[pos].name;
                        }
                        ++pos;
                    }
                    catch
                    {
                        MessageBox.Show("Error file format invalid");
                        return;
                    }
                }
                else
                {
                    line = Regex.Replace(line, @"\s+", " ");
                    string[] items = line.Split(' ');
                    orbitdata[pos].misnum = int.Parse(items[0]); // missionnumbers[i]);
                    orbitdata[pos].name = items[1]; // missions[i];
                    orbitdata[pos].efilename = items[3];
                    orbitdata[pos].cod_id = int.Parse(items[4]);
                    orbitdata[pos].efileused = true;
                    if (orbitdata[pos].misnum == orbitdata[pos].cod_id)
                    {
                        Console.Write("mission number == cod_id\n");
                        missionindex[orbitdata[pos].misnum] = orbitdata[pos].name;
                    }
                    ++pos;

                }
            }
            reader.Close();

        }


        //this function coverts the bytes into the biniary structure
        public unsafe static CMWriteStr ByteToType<CMWriteStr>(BinaryReader reader, int* rtn, int size)
        {

            byte[] bytes = reader.ReadBytes(size);
            if (bytes.Length == 0)
            {
                *rtn = -1;
            }

            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);

            CMWriteStr theStructure = (CMWriteStr)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(CMWriteStr));
            handle.Free();

            return theStructure;
        }

        //TODO need to add eFile processing to both this function and the struct
        public unsafe void load_orbit_file()
        {


            var format = new NumberFormatInfo();
            format.NegativeSign = "-";
            format.NumberDecimalSeparator = ".";
            CMWriteStr data;

            //open the selected file
            FileStream reader = File.Open(NPAS_Orbit_File, FileMode.Open, FileAccess.Read);

            //compute the number of structs in the file
            orbitmissioncount = (int)(reader.Length / sizeof(CMWriteStr));
            Console.Write("orbit_count=" + orbitmissioncount + "\n");
            //allocate the number of missions that are in the file
            orbitdata = new orbit_str[orbitmissioncount];
            //setup the bin reader
            BinaryReader breader = new BinaryReader(reader);
            //rtnval is set to -1 if no data or invalid data is return???
            int rtnval = 0,
                miscount = 0;
            do
            {
                //convert the bytes read to the struct data
                data = ByteToType<CMWriteStr>(breader, &rtnval, sizeof(CMWriteStr));
                //TODO should check if data stuct is valid and the correct file is used

                //find the size of the mission name so misname isn't over alloced causeing ????? after text.....
                int misnamesize = 0;
                for (misnamesize = 0; misnamesize < 20 && data.misname[misnamesize] != 0; misnamesize++) ;

                //setup local vars to convert bytes to char array;
                char[] misname = new char[misnamesize];
                char[] epoch_ddmmyyyy = new char[10];
                char[] epoch_hhmmsss = new char[12];
                char[] start_ddmmyyyy = new char[10];
                char[] start_hhmmsss = new char[12];
                //loop though bytes converting to char[]
                for (int i = 0; i < 20; i++)
                {

                    if (i < misnamesize)
                        if (data.misname[i] != 0)
                        {
                            misname[i] = Convert.ToChar(data.misname[i]);
                            Console.Write(misname[i]);
                        }
                        else misname[i] = '\0';

                    if (i < 12)
                    {

                        if (data.epoch_hhmmsss[i] != 0)
                        {
                            epoch_hhmmsss[i] = Convert.ToChar(data.epoch_hhmmsss[i]);
                        }
                        else epoch_hhmmsss[i] = '\0';

                        if (data.start_hhmmsss[i] != 0)
                        {
                            start_hhmmsss[i] = Convert.ToChar(data.start_hhmmsss[i]);
                        }
                        else start_hhmmsss[i] = '\0';
                    }
                    if (i < 10)
                    {
                        if (data.epoch_ddmmyyyy[i] != 0)
                        {
                            epoch_ddmmyyyy[i] = Convert.ToChar(data.epoch_ddmmyyyy[i]);
                        }
                        else epoch_ddmmyyyy[i] = '\0';

                        if (data.start_ddmmyyyy[i] != 0)
                        {
                            start_ddmmyyyy[i] = Convert.ToChar(data.start_ddmmyyyy[i]);
                        }
                        else start_ddmmyyyy[i] = '\0';
                    }
                }
                string newmisname = new string(misname); // Convert.ToString()

                string newstart_ddmmyyyy = new string(start_ddmmyyyy); // Convert.ToString()
                string newstart_hhmmsss = new string(start_hhmmsss); // Convert.ToString()

                Console.Write(" miscount=" + miscount + "\n");

                orbitdata[miscount].misnum = data.misnum;
                orbitdata[miscount].name = newmisname;
                //set the start time from MIB1
                orbitdata[miscount].start_date = newstart_ddmmyyyy;
                orbitdata[miscount].start_time = newstart_hhmmsss;
                //set the end option
                orbitdata[miscount].endopt = data.endopt;
                if (data.endopt == 0)
                {
                    orbitdata[miscount].duration = data.duration;
                }
                else
                {
                    char[] end_ddmmyyyy = new char[10];
                    char[] end_hhmmsss = new char[12];
                    for (int i = 0; i < 12; i++)
                    {
                        if (i < 10)
                        {
                            if (data.end_ddmmyyyy[i] != 0)
                            {
                                end_ddmmyyyy[i] = Convert.ToChar(data.end_ddmmyyyy[i]);
                            }
                            else end_ddmmyyyy[i] = '\0';
                        }
                        end_hhmmsss[i] = Convert.ToChar(data.end_hhmmsss[i]);
                    }
                    string newend_ddmmyyyy = new string(end_ddmmyyyy); // Convert.ToString()
                    string newend_hhmmsss = new string(end_hhmmsss); // Convert.ToString()
                    orbitdata[miscount].end_date = newend_ddmmyyyy;
                    orbitdata[miscount].end_time = newend_hhmmsss;

                }


                if (data.efile_opt == 0)
                {//check if an efile was used 0 = not used & 1 = used

                    Console.Write("GTDS COD\n");

                    string newepoch_ddmmyyyy = new string(epoch_ddmmyyyy); // Convert.ToString()
                    string newepoch_hhmmsss = new string(epoch_hhmmsss); // Convert.ToString()

                    orbitdata[miscount].epoch = newepoch_ddmmyyyy;
                    orbitdata[miscount].epoch_time = newepoch_hhmmsss;


                    //TODO need to check on the end option and process the end date or the duration;

                    orbitdata[miscount].centerbody = data.epbod;
                    orbitdata[miscount].sma = data.epel_01;
                    orbitdata[miscount].ecc = data.epel_02;
                    orbitdata[miscount].inc = data.epel_03;
                    orbitdata[miscount].raan = data.epel_04;
                    orbitdata[miscount].aop = data.epel_05;
                    orbitdata[miscount].ma = data.epel_06;
                    orbitdata[miscount].cod_id = data.cod_id;
                    orbitdata[miscount].efileused = false;

                }
                else
                {
                    Console.Write("EFile COD\n");

                    char[] tmpefile_name = new char[200];
                    for (int i = 0; i < 200; i++) tmpefile_name[i] = Convert.ToChar(data.efile_name[i]);
                    string localefilename = new string(tmpefile_name); // Convert.ToString()

                    orbitdata[miscount].efilename = localefilename;
                    orbitdata[miscount].efileused = true;
                }

                if (orbitdata[miscount].misnum == orbitdata[miscount].cod_id)
                {
                    Console.Write("mission number == cod_id\n");
                    missionindex[orbitdata[miscount].misnum] = orbitdata[miscount].name;
                }
                Console.Write("\n misnum=" + data.misnum + " misname=" + newmisname + " epoch_ddmmyyyy=" + orbitdata[miscount].epoch + " newepoch_hhmmsss=" +
                    orbitdata[miscount].epoch_time + " cod_id =" + data.cod_id + "\n");
                ++miscount;


            } while (rtnval != -1 && miscount < orbitmissioncount);
            reader.Close();

        }
    }
}
