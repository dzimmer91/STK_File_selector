//Data_Processing.cs

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

        public void process_mission_data(CheckedListBox passed)
        {

            for (int i = 0; i < orbitmissioncount; i++)
            {

                bool a = Array.Exists<timeline_str>(tldata, x => x.mis == orbitdata[i].name);
                if (a == true)
                {
                    //orbitdata[i].used = 1;
                    if (passed.Items.Cast<string>().Any(r => r == orbitdata[i].name))
                    {
                        Console.Write("Mission name already exists\n");
                    }
                    else
                    {
                        Console.Write("Mission name doesn't exists\n");
                        passed.Items.Add(orbitdata[i].name, false);
                    }
                }
            }
        }

        public void process_station_data(CheckedListBox passed)
        {
            for (int i = 0; i < stationcount; i++)
            {
                bool a = Array.Exists<timeline_str>(tldata, x => x.sta == stationdata[i].name);
                if (a == true)
                {
                    // orbitdata[i].used = 1;
                    if (passed.Items.Cast<string>().Any(r => r == stationdata[i].name))
                    {
                        Console.Write("Mission name already exists\n");
                    }
                    else
                    {
                        Console.Write("Mission name doesn't exists\n");
                        passed.Items.Add(stationdata[i].name, false);
                    }
                }
            }
        }



    }
}
