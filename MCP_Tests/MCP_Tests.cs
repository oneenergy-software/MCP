﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MCP;
using System.IO;
using System.Windows.Forms;

namespace MCP_Tests
{
    [TestClass]
    public class MCP_Tests
    {
        public string Get_PC_Name()
        {
            string This_Dir = Directory.GetCurrentDirectory();
            string PC_Name = "";

            for (int i = 1; i <= This_Dir.Length; i++)
            {
                if (This_Dir.Substring(i, 3) == "OEE")
                    for (int j = i; j <= This_Dir.Length; j++)
                        if (This_Dir.Substring(j, 1) == "\\")
                        {
                            PC_Name = This_Dir.Substring(i, j - i);
                            break;
                        }

                if (PC_Name != "")
                    break;
            }

            return PC_Name;
        }


        [TestMethod]
        public void Calc_Ortho_Slope_Test()
        {
            float varx = 1.45F;
            float vary = 2.20F;
            float covar = 1.8F;

            float slope_expected = 1.229804F;

            MCP_tool ThisMCP = new MCP_tool();
            float slope_calc = ThisMCP.Calc_Ortho_Slope(varx, vary, covar);

            Assert.AreEqual(slope_expected, slope_calc, 0.00001, "Orthogonal Slope not calculated correctly");

        }

        [TestMethod]
        public void Get_WD_ind_test()
        {
            MCP_tool ThisMCP = new MCP_tool();

            // Test 1
            float This_WD = 0f;
            int Num_WD = 16;

            int WD_index_expected = 0;
            int WD_index_calc = ThisMCP.Get_WD_ind(This_WD, Num_WD);

            Assert.AreEqual(WD_index_expected, WD_index_calc, 0.1, "Wind Direction index not calculated correctly");

            // Test 2
            This_WD = 105f;
            Num_WD = 16;

            WD_index_expected = 5;
            WD_index_calc = ThisMCP.Get_WD_ind(This_WD, Num_WD);

            Assert.AreEqual(WD_index_expected, WD_index_calc, 0.1, "Wind Direction index not calculated correctly");

            // Test 3
            This_WD = 355f;
            Num_WD = 16;

            WD_index_expected = 0;
            WD_index_calc = ThisMCP.Get_WD_ind(This_WD, Num_WD);

            Assert.AreEqual(WD_index_expected, WD_index_calc, 0.1, "Wind Direction index not calculated correctly");

            // Test 4
            This_WD = 45f;
            Num_WD = 8;

            WD_index_expected = 1;
            WD_index_calc = ThisMCP.Get_WD_ind(This_WD, Num_WD);

            Assert.AreEqual(WD_index_expected, WD_index_calc, 0.1, "Wind Direction index not calculated correctly");

            // Test 5
            This_WD = 315f;
            Num_WD = 8;

            WD_index_expected = 7;
            WD_index_calc = ThisMCP.Get_WD_ind(This_WD, Num_WD);

            Assert.AreEqual(WD_index_expected, WD_index_calc, 0.1, "Wind Direction index not calculated correctly");

            // Test 6
            This_WD = 31f;
            Num_WD = 24;

            WD_index_expected = 2;
            WD_index_calc = ThisMCP.Get_WD_ind(This_WD, Num_WD);

            Assert.AreEqual(WD_index_expected, WD_index_calc, 0.1, "Wind Direction index not calculated correctly");


        }

        [TestMethod]
        public void Get_WS_ind_test()
        {
            MCP_tool ThisMCP = new MCP_tool();

            // Test 1
            float This_WS = 0f;
            float Bin_Width = 1;

            int WS_index_expected = 0;
            int WS_index_calc = ThisMCP.Get_WS_ind(This_WS, Bin_Width);

            Assert.AreEqual(WS_index_expected, WS_index_calc, 0.1, "Wind Speed index not calculated correctly");

            // Test 2
            This_WS = 2.2f;
            Bin_Width = 1;

            WS_index_expected = 2;
            WS_index_calc = ThisMCP.Get_WS_ind(This_WS, Bin_Width);

            Assert.AreEqual(WS_index_expected, WS_index_calc, 0.1, "Wind Speed index not calculated correctly");

            // Test 3
            This_WS = 6.8f;
            Bin_Width = 1;

            WS_index_expected = 7;
            WS_index_calc = ThisMCP.Get_WS_ind(This_WS, Bin_Width);

            Assert.AreEqual(WS_index_expected, WS_index_calc, 0.1, "Wind Speed index not calculated correctly");

            // Test 4
            This_WS = 3.1f;
            Bin_Width = 0.5f;

            WS_index_expected = 6;
            WS_index_calc = ThisMCP.Get_WS_ind(This_WS, Bin_Width);

            Assert.AreEqual(WS_index_expected, WS_index_calc, 0.1, "Wind Speed index not calculated correctly");

            // Test 5
            This_WS = 1.5f;
            Bin_Width = 0.5f;

            WS_index_expected = 3;
            WS_index_calc = ThisMCP.Get_WS_ind(This_WS, Bin_Width);

            Assert.AreEqual(WS_index_expected, WS_index_calc, 0.1, "Wind Speed index not calculated correctly");

            // Test 6
            This_WS = 15f;
            Bin_Width = 0.5f;

            WS_index_expected = 30;
            WS_index_calc = ThisMCP.Get_WS_ind(This_WS, Bin_Width);

            Assert.AreEqual(WS_index_expected, WS_index_calc, 0.1, "Wind Speed index not calculated correctly");
        }

        [TestMethod]
        public void Find_Min_Max_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            
            string PC_Name = Get_PC_Name();                   
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";

            ThisMCP.Import_Reference_data(filename);

            // Test 1 with 8 WD sectors and 1 hourly interval
            ThisMCP.Num_WD_Sectors = 8;
            ThisMCP.Num_Hourly_Ints = 1;

            ThisMCP.Find_Min_Max_temp();

            Assert.AreEqual(ThisMCP.Min_Temp[0, 0], -24.1, 0.1, "Incorrect Min Temperature Test1A");
            Assert.AreEqual(ThisMCP.Max_Temp[0, 0], 33.35, 0.1, "Incorrect Max Temperature Test1A");

            Assert.AreEqual(ThisMCP.Min_Temp[1, 0], -18.8, 0.1, "Incorrect Min Temperature Test1B");
            Assert.AreEqual(ThisMCP.Max_Temp[1, 0], 34, 0.1, "Incorrect Max Temperature Test1B");

            Assert.AreEqual(ThisMCP.Min_Temp[7, 0], -21.47, 0.1, "Incorrect Min Temperature Test1C");
            Assert.AreEqual(ThisMCP.Max_Temp[7, 0], 38.79, 0.1, "Incorrect Max Temperature Test1C");

            // Test 2 with 16 WD sectors and 2 hourly intervals
            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 2;

            ThisMCP.Find_Min_Max_temp();

            Assert.AreEqual(ThisMCP.Min_Temp[0, 0], -24.1, 0.1, "Incorrect Min Temperature Test2A");
            Assert.AreEqual(ThisMCP.Max_Temp[0, 0], 32.15, 0.1, "Incorrect Max Temperature Test2A");

            Assert.AreEqual(ThisMCP.Min_Temp[0, 1], -22.64, 0.1, "Incorrect Min Temperature Test2B");
            Assert.AreEqual(ThisMCP.Max_Temp[0, 1], 29.82, 0.1, "Incorrect Max Temperature Test2B");

            Assert.AreEqual(ThisMCP.Min_Temp[4, 0], -19.66, 0.1, "Incorrect Min Temperature Test2C");
            Assert.AreEqual(ThisMCP.Max_Temp[4, 0], 33.69, 0.1, "Incorrect Max Temperature Test2C");

            Assert.AreEqual(ThisMCP.Min_Temp[4, 1], -18.11, 0.1, "Incorrect Min Temperature Test2D");
            Assert.AreEqual(ThisMCP.Max_Temp[4, 1], 31.56, 0.1, "Incorrect Max Temperature Test2D");

            Assert.AreEqual(ThisMCP.Min_Temp[15, 0], -21.47, 0.1, "Incorrect Min Temperature Test2E");
            Assert.AreEqual(ThisMCP.Max_Temp[15, 0], 34.96, 0.1, "Incorrect Max Temperature Test2E");

            Assert.AreEqual(ThisMCP.Min_Temp[15, 1], -21.27, 0.1, "Incorrect Min Temperature Test2F");
            Assert.AreEqual(ThisMCP.Max_Temp[15, 1], 31.44, 0.1, "Incorrect Max Temperature Test2F");

            // Test 3 with 16 WD sectors and 4 hourly intervals
            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 4;

            ThisMCP.Find_Min_Max_temp();

            Assert.AreEqual(ThisMCP.Min_Temp[1, 0], -19.34, 0.1, "Incorrect Min Temperature Test3A");
            Assert.AreEqual(ThisMCP.Max_Temp[1, 0], 32.60, 0.1, "Incorrect Max Temperature Test3A");

            Assert.AreEqual(ThisMCP.Min_Temp[1, 1], -14.46, 0.1, "Incorrect Min Temperature Test3B");
            Assert.AreEqual(ThisMCP.Max_Temp[1, 1], 33.43, 0.1, "Incorrect Max Temperature Test3B");

            Assert.AreEqual(ThisMCP.Min_Temp[1, 2], -17.51, 0.1, "Incorrect Min Temperature Test3C");
            Assert.AreEqual(ThisMCP.Max_Temp[1, 2], 30.82, 0.1, "Incorrect Max Temperature Test3C");

            Assert.AreEqual(ThisMCP.Min_Temp[1, 3], -18.5, 0.1, "Incorrect Min Temperature Test3D");
            Assert.AreEqual(ThisMCP.Max_Temp[1, 3], 24.69, 0.1, "Incorrect Max Temperature Test3D");

            Assert.AreEqual(ThisMCP.Min_Temp[15, 0], -21.47, 0.1, "Incorrect Min Temperature Test3E");
            Assert.AreEqual(ThisMCP.Max_Temp[15, 0], 32.12, 0.1, "Incorrect Max Temperature Test3E");

            Assert.AreEqual(ThisMCP.Min_Temp[15, 1], -16.15, 0.1, "Incorrect Min Temperature Test3F");
            Assert.AreEqual(ThisMCP.Max_Temp[15, 1], 34.96, 0.1, "Incorrect Max Temperature Test3F");

            Assert.AreEqual(ThisMCP.Min_Temp[15, 2], -18.27, 0.1, "Incorrect Min Temperature Test3G");
            Assert.AreEqual(ThisMCP.Max_Temp[15, 2], 31.44, 0.1, "Incorrect Max Temperature Test3G");

            Assert.AreEqual(ThisMCP.Min_Temp[15, 3], -21.27, 0.1, "Incorrect Min Temperature Test3H");
            Assert.AreEqual(ThisMCP.Max_Temp[15, 3], 26.61, 0.1, "Incorrect Max Temperature Test3H");

            // Test 4 with 16 WD sectors and 6 hourly intervals
            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 6;

            ThisMCP.Find_Min_Max_temp();

            Assert.AreEqual(ThisMCP.Min_Temp[1, 0], -18.77, 0.1, "Incorrect Min Temperature Test4A");
            Assert.AreEqual(ThisMCP.Max_Temp[1, 0], 29.68, 0.1, "Incorrect Max Temperature Test4A");

            Assert.AreEqual(ThisMCP.Min_Temp[1, 1], -19.34, 0.1, "Incorrect Min Temperature Test4B");
            Assert.AreEqual(ThisMCP.Max_Temp[1, 1], 32.74, 0.1, "Incorrect Max Temperature Test4B");

            Assert.AreEqual(ThisMCP.Min_Temp[1, 2], -12.25, 0.1, "Incorrect Min Temperature Test4C");
            Assert.AreEqual(ThisMCP.Max_Temp[1, 2], 33.43, 0.1, "Incorrect Max Temperature Test4C");

            Assert.AreEqual(ThisMCP.Min_Temp[1, 3], -15.98, 0.1, "Incorrect Min Temperature Test4D");
            Assert.AreEqual(ThisMCP.Max_Temp[1, 3], 30.82, 0.1, "Incorrect Max Temperature Test4D");

            Assert.AreEqual(ThisMCP.Min_Temp[1, 4], -17.94, 0.1, "Incorrect Min Temperature Test4E");
            Assert.AreEqual(ThisMCP.Max_Temp[1, 4], 27.36, 0.1, "Incorrect Max Temperature Test4E");

            Assert.AreEqual(ThisMCP.Min_Temp[1, 5], -18.50, 0.1, "Incorrect Min Temperature Test4F");
            Assert.AreEqual(ThisMCP.Max_Temp[1, 5], 24.69, 0.1, "Incorrect Max Temperature Test4F");

            Assert.AreEqual(ThisMCP.Min_Temp[0, 0], -24.10, 0.1, "Incorrect Min Temperature Test4G");
            Assert.AreEqual(ThisMCP.Max_Temp[0, 0], 31.58, 0.1, "Incorrect Max Temperature Test4G");

            Assert.AreEqual(ThisMCP.Min_Temp[0, 1], -21.62, 0.1, "Incorrect Min Temperature Test4H");
            Assert.AreEqual(ThisMCP.Max_Temp[0, 1], 31.96, 0.1, "Incorrect Max Temperature Test4H");

            Assert.AreEqual(ThisMCP.Min_Temp[0, 2], -14.32, 0.1, "Incorrect Min Temperature Test4I");
            Assert.AreEqual(ThisMCP.Max_Temp[0, 2], 32.15, 0.1, "Incorrect Max Temperature Test4I");

            Assert.AreEqual(ThisMCP.Min_Temp[0, 3], -16.72, 0.1, "Incorrect Min Temperature Test4J");
            Assert.AreEqual(ThisMCP.Max_Temp[0, 3], 31.94, 0.1, "Incorrect Max Temperature Test4J");

            Assert.AreEqual(ThisMCP.Min_Temp[0, 4], -18.95, 0.1, "Incorrect Min Temperature Test4K");
            Assert.AreEqual(ThisMCP.Max_Temp[0, 4], 25.88, 0.1, "Incorrect Max Temperature Test4K");

            Assert.AreEqual(ThisMCP.Min_Temp[0, 5], -21.22, 0.1, "Incorrect Min Temperature Test4L");
            Assert.AreEqual(ThisMCP.Max_Temp[0, 5], 25.49, 0.1, "Incorrect Max Temperature Test4L");

            // Test 5 with 16 WD sectors and 8 hourly intervals
            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 8;

            ThisMCP.Find_Min_Max_temp();

            Assert.AreEqual(ThisMCP.Min_Temp[14, 0], -21.36, 0.1, "Incorrect Min Temperature Test5A");
            Assert.AreEqual(ThisMCP.Max_Temp[14, 0], 28.12, 0.1, "Incorrect Max Temperature Test5A");

            Assert.AreEqual(ThisMCP.Min_Temp[14, 1], -20.64, 0.1, "Incorrect Min Temperature Test5B");
            Assert.AreEqual(ThisMCP.Max_Temp[14, 1], 32.38, 0.1, "Incorrect Max Temperature Test5B");

            Assert.AreEqual(ThisMCP.Min_Temp[14, 2], -16.49, 0.1, "Incorrect Min Temperature Test5C");
            Assert.AreEqual(ThisMCP.Max_Temp[14, 2], 38.68, 0.1, "Incorrect Max Temperature Test5C");

            Assert.AreEqual(ThisMCP.Min_Temp[14, 3], -16.48, 0.1, "Incorrect Min Temperature Test5D");
            Assert.AreEqual(ThisMCP.Max_Temp[14, 3], 38.79, 0.1, "Incorrect Max Temperature Test5D");

            Assert.AreEqual(ThisMCP.Min_Temp[14, 4], -17.02, 0.1, "Incorrect Min Temperature Test5E");
            Assert.AreEqual(ThisMCP.Max_Temp[14, 4], 36.43, 0.1, "Incorrect Max Temperature Test5E");

            Assert.AreEqual(ThisMCP.Min_Temp[14, 5], -17.24, 0.1, "Incorrect Min Temperature Test5F");
            Assert.AreEqual(ThisMCP.Max_Temp[14, 5], 29.96, 0.1, "Incorrect Max Temperature Test5F");

            Assert.AreEqual(ThisMCP.Min_Temp[14, 6], -18.22, 0.1, "Incorrect Min Temperature Test5G");
            Assert.AreEqual(ThisMCP.Max_Temp[14, 6], 27.40, 0.1, "Incorrect Max Temperature Test5G");

            Assert.AreEqual(ThisMCP.Min_Temp[14, 7], -19.43, 0.1, "Incorrect Min Temperature Test5H");
            Assert.AreEqual(ThisMCP.Max_Temp[14, 7], 26.41, 0.1, "Incorrect Max Temperature Test5H");                       

        }

        [TestMethod]
        public void Generate_Matrix_CDFs_Test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string PC_Name = Get_PC_Name();
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\Paulding 10-min data_hourly.csv";
            ThisMCP.Import_Target_Data(filename);

            // Test 1
            ThisMCP.Num_WD_Sectors = 1;
            ThisMCP.Num_Hourly_Ints = 1;
            ThisMCP.Num_Temp_bins = 1;
            ThisMCP.WS_bin_width = 1;

            MCP_tool.CDF_Obj[] ThisCDF_Obj = ThisMCP.Generate_Matrix_CDFs();

            // check counts of each CDF
            Assert.AreEqual(ThisCDF_Obj[0].Count, 9);
            Assert.AreEqual(ThisCDF_Obj[1].Count, 214);
            Assert.AreEqual(ThisCDF_Obj[2].Count, 464);
            Assert.AreEqual(ThisCDF_Obj[3].Count, 652);
            Assert.AreEqual(ThisCDF_Obj[4].Count, 786);
            Assert.AreEqual(ThisCDF_Obj[5].Count, 1012);

            // check CDF in three locations
            Assert.AreEqual(ThisCDF_Obj[0].CDF[25], 0.333, 0.01, "Wrong CDF[25] for WS_ind = 0");
            Assert.AreEqual(ThisCDF_Obj[0].CDF[50], 0.667, 0.01, "Wrong CDF[50] for WS_ind = 0");
            Assert.AreEqual(ThisCDF_Obj[0].CDF[75], 0.778, 0.01, "Wrong CDF[75] for WS_ind = 0");

            Assert.AreEqual(ThisCDF_Obj[1].CDF[25], 0.6028, 0.01, "Wrong CDF[25] for WS_ind = 1");
            Assert.AreEqual(ThisCDF_Obj[1].CDF[50], 0.9065, 0.01, "Wrong CDF[50] for WS_ind = 1");
            Assert.AreEqual(ThisCDF_Obj[1].CDF[75], 0.9813, 0.01, "Wrong CDF[75] for WS_ind = 1");

            Assert.AreEqual(ThisCDF_Obj[2].CDF[25], 0.3987, 0.01, "Wrong CDF[25] for WS_ind = 2");
            Assert.AreEqual(ThisCDF_Obj[2].CDF[50], 0.8836, 0.01, "Wrong CDF[50] for WS_ind = 2");
            Assert.AreEqual(ThisCDF_Obj[2].CDF[75], 0.9935, 0.01, "Wrong CDF[75] for WS_ind = 2");

            // Test 2
            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 4;
            ThisMCP.Num_Temp_bins = 2;
            ThisMCP.WS_bin_width = 1;
            ThisMCP.Find_Min_Max_temp();
            ThisCDF_Obj = ThisMCP.Generate_Matrix_CDFs();

            // find CDF with WS ind = 7, Hour ind = 2, Temp ind = 1, WD ind = 1

            foreach (MCP_tool.CDF_Obj CDF in ThisCDF_Obj)
            {
                if (CDF.WS_ind == 7 && CDF.Hour_ind == 2 && CDF.Temp_ind == 1 && CDF.WD_ind == 1)
                {                 
                    Assert.AreEqual(CDF.Count, 7, 0, "Wrong count for Test2");
                    Assert.AreEqual(CDF.CDF[25], 0.2857, 0.01, "Wrong CDF[25] for Test2");
                    Assert.AreEqual(CDF.CDF[50], 0.428571, 0.01, "Wrong CDF[50] for Test2");
                    Assert.AreEqual(CDF.CDF[75], 0.857143, 0.01, "Wrong CDF[75] for Test2");
                }

            }

        }

        [TestMethod]
        public void Get_Conc_Avgs_Count_Test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string PC_Name = Get_PC_Name();
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\Paulding 10-min data_hourly.csv";
            ThisMCP.Import_Target_Data(filename);

            float[] Avg_WS_WD; // 0: Target WS; 1: Reference WS; 2: Data Count'

            // Test 1
            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 6;
            ThisMCP.Num_Temp_bins = 2;
            ThisMCP.Find_Min_Max_temp();

            Avg_WS_WD = ThisMCP.Get_Conc_Avgs_Count(0, 0, 1, false);

            Assert.AreEqual(Avg_WS_WD[0], 3.543, 0.01, "Wrong average target wind speed in Test 1");
            Assert.AreEqual(Avg_WS_WD[1], 4.1508, 0.01, "Wrong average reference wind speed in Test 1");
            Assert.AreEqual(Avg_WS_WD[2], 39, 0, "Wrong data count in Test 1");

            // Test 2
            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 1;
            ThisMCP.Num_Temp_bins = 1;
            ThisMCP.Find_Min_Max_temp();

            Avg_WS_WD = ThisMCP.Get_Conc_Avgs_Count(15, 0, 0, false);

            Assert.AreEqual(Avg_WS_WD[2], 554, 0, "Wrong data count in Test 2");
            Assert.AreEqual(Avg_WS_WD[0], 5.43388, 0.01, "Wrong average target wind speed in Test 2");
            Assert.AreEqual(Avg_WS_WD[1], 6.2731, 0.01, "Wrong average reference wind speed in Test 2");
            
        }

        [TestMethod]
        public void Get_Conc_WS_Array_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string PC_Name = Get_PC_Name();
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\Paulding 10-min data_hourly.csv";
            ThisMCP.Import_Target_Data(filename);

            ThisMCP.Num_WD_Sectors = 8;
            ThisMCP.Num_Hourly_Ints = 2;
            ThisMCP.Num_Temp_bins = 1;

            float[] These_WS = ThisMCP.Get_Conc_WS_Array("Target", 2, 1, 0, 4.5f, 5.5f, false);
            Assert.AreEqual(These_WS.Length, 77, 0, "Wrong data count in Test 1");

            These_WS = ThisMCP.Get_Conc_WS_Array("Reference", 2, 1, 0, 4.5f, 5.5f, false);
            Assert.AreEqual(These_WS.Length, 77, 0, "Wrong data count in Test 2");
        }

        [TestMethod]
        public void Get_Hourly_Index_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            ThisMCP.Num_Hourly_Ints = 2;

            int This_Hour_ind;

            This_Hour_ind = ThisMCP.Get_Hourly_Index(3);
            Assert.AreEqual(This_Hour_ind, 1, 0, "Wrong hourly index in Test 1");

            This_Hour_ind = ThisMCP.Get_Hourly_Index(14);
            Assert.AreEqual(This_Hour_ind, 0, 0, "Wrong hourly index in Test 2");

            ThisMCP.Num_Hourly_Ints = 4;
            This_Hour_ind = ThisMCP.Get_Hourly_Index(5);
            Assert.AreEqual(This_Hour_ind, 3, 0, "Wrong hourly index in Test 3");

            This_Hour_ind = ThisMCP.Get_Hourly_Index(20);
            Assert.AreEqual(This_Hour_ind, 2, 0, "Wrong hourly index in Test 4");

            ThisMCP.Num_Hourly_Ints = 6;
            This_Hour_ind = ThisMCP.Get_Hourly_Index(0);
            Assert.AreEqual(This_Hour_ind, 4, 0, "Wrong hourly index in Test 5");

            This_Hour_ind = ThisMCP.Get_Hourly_Index(12);
            Assert.AreEqual(This_Hour_ind, 1, 0, "Wrong hourly index in Test 6");

            ThisMCP.Num_Hourly_Ints = 8;
            This_Hour_ind = ThisMCP.Get_Hourly_Index(0);
            Assert.AreEqual(This_Hour_ind,6, 0, "Wrong hourly index in Test 7");

            This_Hour_ind = ThisMCP.Get_Hourly_Index(5);
            Assert.AreEqual(This_Hour_ind, 7, 0, "Wrong hourly index in Test 8");

            This_Hour_ind = ThisMCP.Get_Hourly_Index(12);
            Assert.AreEqual(This_Hour_ind, 2, 0, "Wrong hourly index in Test 9");

            This_Hour_ind = ThisMCP.Get_Hourly_Index(21);
            Assert.AreEqual(This_Hour_ind, 5, 0, "Wrong hourly index in Test 10");
        }

        [TestMethod]
        public void Get_Temp_ind_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string PC_Name = Get_PC_Name();
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);
                        
            // Test 1
            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 6;
            ThisMCP.Num_Temp_bins = 2;
            ThisMCP.Find_Min_Max_temp();

            float This_Temp = 0;
            int Temp_ind = ThisMCP.Get_Temp_ind(0, 0, This_Temp);
            Assert.AreEqual(Temp_ind, 0, 0, "Wrong temperature index in Test 1");

            This_Temp = 5;
            Temp_ind = ThisMCP.Get_Temp_ind(0, 0, This_Temp);
            Assert.AreEqual(Temp_ind, 1, 0, "Wrong temperature index in Test 2");

            ThisMCP.Num_Temp_bins = 4;
            This_Temp = -15;
            Temp_ind = ThisMCP.Get_Temp_ind(0, 0, This_Temp);
            Assert.AreEqual(Temp_ind, 0, 0, "Wrong temperature index in Test 3");

            This_Temp = -2;
            Temp_ind = ThisMCP.Get_Temp_ind(0, 0, This_Temp);
            Assert.AreEqual(Temp_ind, 1, 0, "Wrong temperature index in Test 4");

            This_Temp = 7;
            Temp_ind = ThisMCP.Get_Temp_ind(0, 0, This_Temp);
            Assert.AreEqual(Temp_ind, 2, 0, "Wrong temperature index in Test 5");

            This_Temp = 20;
            Temp_ind = ThisMCP.Get_Temp_ind(0, 0, This_Temp);
            Assert.AreEqual(Temp_ind, 3, 0, "Wrong temperature index in Test 6");
        }

        [TestMethod]
        public void Get_Min_Max_Temp_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string PC_Name = Get_PC_Name();
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            // Test 1
            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 6;
            ThisMCP.Num_Temp_bins = 2;
            ThisMCP.Find_Min_Max_temp();

            float[] Min_Max_temp = ThisMCP.Get_Min_Max_Temp(0, 0, 0);
            Assert.AreEqual(Min_Max_temp[0], -24.1, 0.01, "Wrong minimum temperature in Test 1");
            Assert.AreEqual(Min_Max_temp[1], 3.74, 0.01, "Wrong minimum temperature in Test 2");

            Min_Max_temp = ThisMCP.Get_Min_Max_Temp(0,0,1);
            Assert.AreEqual(Min_Max_temp[0], 3.74, 0.01, "Wrong minimum temperature in Test 3");
            Assert.AreEqual(Min_Max_temp[1], 31.58, 0.01, "Wrong minimum temperature in Test 4");

            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 2;
            ThisMCP.Num_Temp_bins = 4;
            ThisMCP.Find_Min_Max_temp();

            Min_Max_temp = ThisMCP.Get_Min_Max_Temp(5, 1, 0);
            Assert.AreEqual(Min_Max_temp[0], -17.24, 0.01, "Wrong minimum temperature in Test 5");
            Assert.AreEqual(Min_Max_temp[1], -5.385, 0.01, "Wrong minimum temperature in Test 6");

            Min_Max_temp = ThisMCP.Get_Min_Max_Temp(5, 1, 1);
            Assert.AreEqual(Min_Max_temp[0], -5.385, 0.01, "Wrong minimum temperature in Test 7");
            Assert.AreEqual(Min_Max_temp[1], 6.47, 0.01, "Wrong minimum temperature in Test 8");

            Min_Max_temp = ThisMCP.Get_Min_Max_Temp(5, 1, 2);
            Assert.AreEqual(Min_Max_temp[0], 6.47, 0.01, "Wrong minimum temperature in Test 9");
            Assert.AreEqual(Min_Max_temp[1], 18.325, 0.01, "Wrong minimum temperature in Test 10");

            Min_Max_temp = ThisMCP.Get_Min_Max_Temp(5, 1, 3);
            Assert.AreEqual(Min_Max_temp[0], 18.325, 0.01, "Wrong minimum temperature in Test 11");
            Assert.AreEqual(Min_Max_temp[1], 30.18, 0.01, "Wrong minimum temperature in Test 12");
        }

        [TestMethod]
        public void Get_Min_Max_WD_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            ThisMCP.Num_WD_Sectors = 4;

            float[] Min_Max_WD = ThisMCP.Get_Min_Max_WD(0);
            Assert.AreEqual(Min_Max_WD[0], 315, 0.01, "Wrong minimum WD in Test 1");
            Assert.AreEqual(Min_Max_WD[1], 45, 0.01, "Wrong minimum WD in Test 2");

            Min_Max_WD = ThisMCP.Get_Min_Max_WD(2);
            Assert.AreEqual(Min_Max_WD[0], 135, 0.01, "Wrong minimum WD in Test 3");
            Assert.AreEqual(Min_Max_WD[1], 225, 0.01, "Wrong minimum WD in Test 4");

            ThisMCP.Num_WD_Sectors = 8;
            Min_Max_WD = ThisMCP.Get_Min_Max_WD(2);
            Assert.AreEqual(Min_Max_WD[0], 67.5, 0.01, "Wrong minimum WD in Test 5");
            Assert.AreEqual(Min_Max_WD[1], 112.5, 0.01, "Wrong minimum WD in Test 6");

            Min_Max_WD = ThisMCP.Get_Min_Max_WD(7);
            Assert.AreEqual(Min_Max_WD[0], 292.5, 0.01, "Wrong minimum WD in Test 7");
            Assert.AreEqual(Min_Max_WD[1], 337.5, 0.01, "Wrong minimum WD in Test 8");

            ThisMCP.Num_WD_Sectors = 16;
            Min_Max_WD = ThisMCP.Get_Min_Max_WD(0);
            Assert.AreEqual(Min_Max_WD[0], 348.75, 0.01, "Wrong minimum WD in Test 9");
            Assert.AreEqual(Min_Max_WD[1], 11.25, 0.01, "Wrong minimum WD in Test 10");

            Min_Max_WD = ThisMCP.Get_Min_Max_WD(1);
            Assert.AreEqual(Min_Max_WD[0], 11.25, 0.01, "Wrong minimum WD in Test 11");
            Assert.AreEqual(Min_Max_WD[1], 33.75, 0.01, "Wrong minimum WD in Test 12");

            Min_Max_WD = ThisMCP.Get_Min_Max_WD(8);
            Assert.AreEqual(Min_Max_WD[0], 168.75, 0.01, "Wrong minimum WD in Test 13");
            Assert.AreEqual(Min_Max_WD[1], 191.25, 0.01, "Wrong minimum WD in Test 14");

            Min_Max_WD = ThisMCP.Get_Min_Max_WD(15);
            Assert.AreEqual(Min_Max_WD[0], 326.25, 0.01, "Wrong minimum WD in Test 15");
            Assert.AreEqual(Min_Max_WD[1], 348.75, 0.01, "Wrong minimum WD in Test 16");
        }

        [TestMethod]
        public void Get_Subset_Conc_Data_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string PC_Name = Get_PC_Name();
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\Paulding 10-min data_hourly.csv";
            ThisMCP.Import_Target_Data(filename);

            ThisMCP.Num_WD_Sectors = 8;
            ThisMCP.Num_Hourly_Ints = 2;
            ThisMCP.Num_Temp_bins = 1;

            DateTime This_Start = new DateTime(2008, 12, 1);
            DateTime This_End = new DateTime(2009, 7, 1);
            
            ThisMCP.Get_Subset_Conc_Data(This_Start, This_End);
            Assert.AreEqual(ThisMCP.Conc_Data[0].This_Date, This_Start,"Wrong start date");
            int Last_ind = ThisMCP.Conc_Data.Length - 1;
            Assert.AreEqual(ThisMCP.Conc_Data[Last_ind].This_Date, This_End, "Wrong end date");

            This_Start = new DateTime(2009, 8, 31);
            This_End = new DateTime(2009, 9, 2);

            ThisMCP.Get_Subset_Conc_Data(This_Start, This_End);
            Assert.AreEqual(ThisMCP.Conc_Data[0].This_Date, This_Start, "Wrong start date");
            Last_ind = ThisMCP.Conc_Data.Length - 1;
            Assert.AreEqual(ThisMCP.Conc_Data[Last_ind].This_Date, This_End, "Wrong end date");
            
        }

        [TestMethod]
        public void Find_CDF_Index_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string PC_Name = Get_PC_Name();
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\Paulding 10-min data_hourly.csv";
            ThisMCP.Import_Target_Data(filename);

            ThisMCP.Num_WD_Sectors = 1;
            ThisMCP.Num_Hourly_Ints = 1;
            ThisMCP.Num_Temp_bins = 1;
            ThisMCP.WS_bin_width = 1;
            
            MCP_tool.CDF_Obj[] ThisCDF_Obj = ThisMCP.Generate_Matrix_CDFs();
            
            float Rando = 0.2f;            
            int CDF_ind = ThisMCP.Find_CDF_Index(ThisCDF_Obj[3], Rando);
            Assert.AreEqual(CDF_ind, 21, 0, "Wrong CDF index");

            Rando = 0.4f;
            CDF_ind = ThisMCP.Find_CDF_Index(ThisCDF_Obj[3], Rando);
            Assert.AreEqual(CDF_ind, 28, 0, "Wrong CDF index");

            Rando = 0.6f;
            CDF_ind = ThisMCP.Find_CDF_Index(ThisCDF_Obj[3], Rando);
            Assert.AreEqual(CDF_ind, 35, 0, "Wrong CDF index");

            Rando = 0.8f;
            CDF_ind = ThisMCP.Find_CDF_Index(ThisCDF_Obj[3], Rando);
            Assert.AreEqual(CDF_ind, 44, 0, "Wrong CDF index");
            
        }

        [TestMethod]
        public void Find_SD_Change_in_WS_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string PC_Name = Get_PC_Name();
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\Paulding 10-min data_hourly.csv";
            ThisMCP.Import_Target_Data(filename);

            ThisMCP.WS_bin_width = 1;
            ThisMCP.Find_SD_Change_in_WS();

            Assert.AreEqual(ThisMCP.SD_WS_Lag[0], 0.656435, 0.001, "Wrong Last WS Standard deviation");
            Assert.AreEqual(ThisMCP.SD_WS_Lag[1], 1.1078, 0.001, "Wrong Last WS Standard deviation");
            Assert.AreEqual(ThisMCP.SD_WS_Lag[2], 0.980807, 0.001, "Wrong Last WS Standard deviation");
        }

        [TestMethod]
        public void Get_Lag_WS_CDF_test()
        {
            float This_Min_WS = 0.454f;
            float This_Last_WS = 6.105f;
            float This_WS_Int = 0.1128f;

            MCP_tool ThisMCP = new MCP_tool();
            ThisMCP.WS_bin_width = 1;

            ThisMCP.Find_SD_Change_in_WS();
            float[] This_CDF = ThisMCP.Get_Lag_WS_CDF(This_Last_WS, This_Min_WS, This_WS_Int);

            Assert.AreEqual(This_CDF[50], 0.49457, 0.01, "Wrong Last WS CDF");
            Assert.AreEqual(This_CDF[20],0.0003113, 0.01, "Wrong Last WS CDF");
            Assert.AreEqual(This_CDF[90], 0.99999, 0.01, "Wrong Last WS CDF");
        }

        [TestMethod]
        public void Calc_Varrat_Slope_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            float This_Var_X = 9.750792f;
            float This_Var_Y = 8.325867f;
            float This_Slope = ThisMCP.Calc_Varrat_Slope(This_Var_X, This_Var_Y);
            
            Assert.AreEqual(This_Slope, 0.924049, 0.001, "Wrong slope for variance ratio MCP method");
        }

        [TestMethod]
        public void Calc_Avg_SD_Uncert_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string PC_Name = Get_PC_Name();
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\Paulding 10-min data_hourly.csv";
            ThisMCP.Import_Target_Data(filename);
            ThisMCP.cboMCP_Type.SelectedIndex = 0; // orthogonal
            ThisMCP.Num_WD_Sectors = 1;

            ThisMCP.Do_MCP_Uncertainty();
                       
            Assert.AreEqual(ThisMCP.Uncert_Ortho[0].avg, 6.30065, 0.001, "Wrong calculated average of LT estimates in uncertainty analysis");
            Assert.AreEqual(ThisMCP.Uncert_Ortho[0].std_dev, 0.211046, 0.001, "Wrong calculated standard deviation of LT estimates in uncertainty analysis");
        }

        [TestMethod]
        public void Do_MCP_Uncertainty_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string PC_Name = Get_PC_Name();
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\Paulding 10-min data_hourly.csv";
            ThisMCP.Import_Target_Data(filename);
            ThisMCP.cboMCP_Type.SelectedIndex = 0; // orthogonal
            ThisMCP.Num_WD_Sectors = 1;

            ThisMCP.Do_MCP_Uncertainty();
            
            Assert.AreEqual(ThisMCP.Uncert_Ortho.Length, 12, 0, "Wrong number of uncertainty objects");
            Assert.AreEqual(ThisMCP.Uncert_Ortho[0].NWindows, 12, 0, "Wrong number of monthly intervals");
            Assert.AreEqual(ThisMCP.Uncert_Ortho[7].NWindows, 5, 0, "Wrong number ofintervals");
            Assert.AreEqual(ThisMCP.Uncert_Ortho[5].WSize, 6, 0, "Wrong wrong window size");
            Assert.AreEqual(ThisMCP.Uncert_Ortho[2].avg, 6.3326, 0.001, "Wrong average LT Estimate in uncertainty calculation");

        }

        [TestMethod]
        public void Import_Reference_data_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string PC_Name = Get_PC_Name();
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            Assert.AreEqual(ThisMCP.Ref_Data.Length / 365.0 / 24.0, 31.01, 0.01, "Wrong reference data length");
            Stats Stat = new Stats();
            float Ref_Avg =  Stat.Calc_Avg_WS(ThisMCP.Ref_Data, 0, 10000, ThisMCP.Ref_Start, ThisMCP.Ref_End, 0, 360, true , 0, true, 0, ThisMCP);

            Assert.AreEqual(Ref_Avg, 6.73913, 0.01, "Wrong average reference wind speed");

            ThisMCP.Num_Temp_bins = 1;
            ThisMCP.Num_WD_Sectors = 1;
            ThisMCP.Num_Hourly_Ints = 1;

            ThisMCP.Find_Min_Max_temp();

            Assert.AreEqual(ThisMCP.Min_Temp[0,0], -27.33, 0.01, "Wrong minimum temperature");
            Assert.AreEqual(ThisMCP.Max_Temp[0, 0], 38.79, 0.01, "Wrong maximum temperature");
        }

        [TestMethod]
        public void Get_WS_width_for_MCP_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            ThisMCP.WS_bin_width = 1;
            Assert.AreEqual(ThisMCP.Get_WS_width_for_MCP(), 1, "Wrong wind speed bin width");

            ThisMCP.WS_bin_width = 0.5f;
            Assert.AreEqual(ThisMCP.Get_WS_width_for_MCP(), 0.5f, "Wrong wind speed bin width");
        }

        [TestMethod]
        public void Get_TAB_export_WS_width_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            ThisMCP.txtTAB_WS_bin.Text = "1";
            Assert.AreEqual(ThisMCP.Get_TAB_export_WS_width(), 1, 0, "Wrong WS width for TAB export");

            ThisMCP.txtTAB_WS_bin.Text = "0.5";
            Assert.AreEqual(ThisMCP.Get_TAB_export_WS_width(), 0.5, 0, "Wrong WS width for TAB export");
        }

        [TestMethod]
        public void Get_WD_ind_to_plot_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 1;
            ThisMCP.Num_Temp_bins = 1;

            ThisMCP.Update_WD_DropDown();
            ThisMCP.Update_Hourly_DropDown();
            ThisMCP.Update_Temp_Dropdown();

            ThisMCP.cboWD_sector.SelectedIndex = 0;
            Assert.AreEqual(ThisMCP.Get_WD_ind_to_plot(), 0, 0, "Wrong Wind Direction index to plot");

            ThisMCP.cboWD_sector.SelectedIndex = 16;
            Assert.AreEqual(ThisMCP.Get_WD_ind_to_plot(), 16, 0, "Wrong Wind Direction index to plot");

            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 4;
            ThisMCP.Num_Temp_bins = 1;

            ThisMCP.Update_WD_DropDown();
            ThisMCP.Update_Hourly_DropDown();
            ThisMCP.Update_Temp_Dropdown();

            ThisMCP.cboWD_sector.SelectedIndex = 1; // Choosing WD sect = 1
            ThisMCP.cboHourInt.SelectedIndex = 4; // But now selecting Hour interval "All Hours", this should make WD sector choose "All WD"

            Assert.AreEqual(ThisMCP.Get_WD_ind_to_plot(), 16, 0, "Wrong Wind Direction index to plot");


        }

        [TestMethod]
        public void Get_Hourly_ind_to_plot_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 2;
            ThisMCP.Num_Temp_bins = 1;

            ThisMCP.Update_WD_DropDown();
            ThisMCP.Update_Hourly_DropDown();
            ThisMCP.Update_Temp_Dropdown();

            ThisMCP.cboHourInt.SelectedIndex = 1;
            Assert.AreEqual(ThisMCP.Get_Hourly_ind_to_plot(), 1, 0, "Wrong hourly index to plot");

            ThisMCP.cboHourInt.SelectedIndex = 1; // Choosing Hourly int = 1 
            ThisMCP.cboWD_sector.SelectedIndex = 16; // But now selecting WD interval "All WD", this should make hour dropdown choose "All Hours"

            Assert.AreEqual(ThisMCP.Get_Hourly_ind_to_plot(), 2, 0, "Wrong hourly indexto plot");
        }

        [TestMethod]
        public void Get_Temp_Ind_to_plot_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 6;
            ThisMCP.Num_Temp_bins = 4;

            ThisMCP.Find_Min_Max_temp();
            ThisMCP.Update_WD_DropDown();
            ThisMCP.Update_Hourly_DropDown();
            ThisMCP.Update_Temp_Dropdown();

            ThisMCP.cboTemp_Int.SelectedIndex = 1;
            Assert.AreEqual(ThisMCP.Get_Temp_Ind_to_plot(), 1, 0, "Wrong temperature index to plot");

            ThisMCP.cboHourInt.SelectedIndex = 4; // Choosing Hourly int = 1 
            ThisMCP.cboWD_sector.SelectedIndex = 16; // But now selecting WD interval "All WD", this should make Temp Int choose "All Temps"

            Assert.AreEqual(ThisMCP.Get_Temp_Ind_to_plot(), 4, 0, "Wrong hourly indexto plot");
        }

        [TestMethod]
        public void Get_Num_WD_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            ThisMCP.Num_WD_Sectors = 16;
            Assert.AreEqual(ThisMCP.Get_Num_WD(), 16, 0, "Wrong number of WD sectors");

            ThisMCP.cboNumWD.SelectedIndex = 2;
            Assert.AreEqual(ThisMCP.Get_Num_WD(), 8, 0, "Wrong number of WD sectors");

            ThisMCP.cboNumWD.SelectedIndex = 3;
            Assert.AreEqual(ThisMCP.Get_Num_WD(), 12, 0, "Wrong number of WD sectors");
        }

        [TestMethod]
        public void Get_Num_Hourly_Ints_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            ThisMCP.Num_Hourly_Ints = 4;
            Assert.AreEqual(ThisMCP.Get_Num_Hourly_Ints(), 4, 0, "Wrong number of hourly intervals");

            ThisMCP.cboNumHours.SelectedIndex = 4;
            Assert.AreEqual(ThisMCP.Get_Num_Hourly_Ints(), 8, 0, "Wrong number of hourly intervals");

            ThisMCP.cboNumHours.SelectedIndex = 2;
            Assert.AreEqual(ThisMCP.Get_Num_Hourly_Ints(), 4, 0, "Wrong number of hourly intervals");
        }

        [TestMethod]
        public void Get_Num_Temp_Ints_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            ThisMCP.Num_Temp_bins = 1;
            Assert.AreEqual(ThisMCP.Get_Num_Temp_Ints(), 1, 0, "Wrong number of temperature intervals test 1");

            ThisMCP.cboNumTemps.SelectedIndex = 1;
            Assert.AreEqual(ThisMCP.Get_Num_Temp_Ints(), 2, 0, "Wrong number of temperature intervals test 2");

            ThisMCP.cboNumTemps.SelectedIndex = 2;
            Assert.AreEqual(ThisMCP.Get_Num_Temp_Ints(), 4, 0, "Wrong number of temperature intervals test 3");
        }

        [TestMethod]
        public void Get_MCP_Method_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            ThisMCP.cboMCP_Type.SelectedIndex = 0;
            Assert.AreEqual(ThisMCP.Get_MCP_Method(), "Orth. Regression", true, "Wrong MCP Method test 1");

            ThisMCP.cboMCP_Type.SelectedIndex = 1;
            Assert.AreEqual(ThisMCP.Get_MCP_Method(), "Method of Bins", true, "Wrong MCP Method test 2");

            ThisMCP.cboMCP_Type.SelectedIndex = 2;
            Assert.AreEqual(ThisMCP.Get_MCP_Method(), "Variance Ratio", true, "Wrong MCP Method test 3");

            ThisMCP.cboMCP_Type.SelectedIndex = 3;
            Assert.AreEqual(ThisMCP.Get_MCP_Method(), "Matrix", true, "Wrong MCP Method test 4");

        }

        [TestMethod]
        public void Get_WS_PDF_Weight_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            ThisMCP.txtWS_PDF_Wgt.Text = "1";
            Assert.AreEqual(ThisMCP.Get_WS_PDF_Weight(), 1, 0, "Wrong WS PDF Weight test 1");

            ThisMCP.txtWS_PDF_Wgt.Text = "2";
            Assert.AreEqual(ThisMCP.Get_WS_PDF_Weight(), 2, 0, "Wrong WS PDF Weight test 2");

            ThisMCP.txtWS_PDF_Wgt.Text = "0.5";
            Assert.AreEqual(ThisMCP.Get_WS_PDF_Weight(), 0.5, 0, "Wrong WS PDF Weight test 3");
        }

        [TestMethod]
        public void Get_Last_WS_Weight_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            ThisMCP.txtLast_WS_Wgt.Text = "0.1";
            Assert.AreEqual(ThisMCP.Get_Last_WS_Weight(), 0.1, 0.0001, "Wrong Last WS Weight test 1");

            ThisMCP.txtLast_WS_Wgt.Text = "1";
            Assert.AreEqual(ThisMCP.Get_Last_WS_Weight(), 1, 0.0001, "Wrong Last WS Weight test 21");

            ThisMCP.txtLast_WS_Wgt.Text = "2";
            Assert.AreEqual(ThisMCP.Get_Last_WS_Weight(), 2, 0.0001, "Wrong Last WS Weight test 3");

        }

        [TestMethod]
        public void Get_Uncert_Step_Size_test()
        {
            MCP_tool ThisMCP = new MCP_tool();

            ThisMCP.cboUncertStep.SelectedIndex = 1;
            Assert.AreEqual(ThisMCP.Get_Uncert_Step_Size(), 2, 0, "Wrong Uncertainty window step size test 1");

            ThisMCP.cboUncertStep.SelectedIndex = 3;
            Assert.AreEqual(ThisMCP.Get_Uncert_Step_Size(), 4, 0, "Wrong Uncertainty window step size test 2");

            ThisMCP.cboUncertStep.SelectedIndex = 0;
            Assert.AreEqual(ThisMCP.Get_Uncert_Step_Size(), 1, 0, "Wrong Uncertainty window step size test 3");
        }

        [TestMethod]
        public void Import_Target_Data_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string PC_Name = Get_PC_Name();
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\Paulding 10-min data_hourly.csv";
            ThisMCP.Import_Target_Data(filename);

            Stats Stat = new Stats();
            float Target_Avg = Stat.Calc_Avg_WS(ThisMCP.Target_Data, 0, 10000, ThisMCP.Target_Start, ThisMCP.Target_End, 0, 360, true, 0, true, 0, ThisMCP);
            Assert.AreEqual(Target_Avg, 6.266685, 0.001, "Wrong average target wind speed");

            Assert.AreEqual(ThisMCP.Target_Data.Length / 365.0 / 24.0, 0.985, 0.01, "Wrong reference data length");

        }

        [TestMethod]
        public void Find_Concurrent_Data_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string PC_Name = Get_PC_Name();
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\Paulding 10-min data_hourly.csv";
            ThisMCP.Import_Target_Data(filename);

            ThisMCP.Find_Concurrent_Data(true, ThisMCP.Conc_Start, ThisMCP.Conc_End);
            Assert.AreEqual(ThisMCP.Conc_Data.Length, 8627, 0, "Wrong concurrent data length");

            float[] Conc_Avgs = ThisMCP.Get_Conc_Avgs_Count(0, 0, 0, true);
            Assert.AreEqual(Conc_Avgs[0], 6.266685, 0.001, "Wrong average target wind speed");
            Assert.AreEqual(Conc_Avgs[1], 6.68748, 0.001, "Wrong average reference wind speed");
            Assert.AreEqual(Conc_Avgs[2], 8627, 0, "Wrong concurrent data count");
        }

        [TestMethod]
        public void Get_Sector_Count_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string PC_Name = Get_PC_Name();
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);
                       
            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 1;
            ThisMCP.Num_Temp_bins = 1;
                        
            Assert.AreEqual(ThisMCP.Get_Sector_Count(0, 0, 0), 12529, 0, "Wrong sector count in WD = 0");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(1, 0, 0), 11360, 0, "Wrong sector count in WD = 1");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(2, 0, 0), 12149, 0, "Wrong sector count in WD = 2");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(3, 0, 0), 13874, 0, "Wrong sector count in WD = 3");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(4, 0, 0), 13380, 0, "Wrong sector count in WD = 4");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(5, 0, 0), 10538, 0, "Wrong sector count in WD = 5");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(6, 0, 0), 9180, 0, "Wrong sector count in WD = 6");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(7, 0, 0), 10799, 0, "Wrong sector count in WD = 7");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(8, 0, 0), 15793, 0, "Wrong sector count in WD = 8");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(9, 0, 0), 24231, 0, "Wrong sector count in WD = 9");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(10, 0, 0), 29027, 0, "Wrong sector count in WD = 10");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(11, 0, 0), 26659, 0, "Wrong sector count in WD = 11");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(12, 0, 0), 24669, 0, "Wrong sector count in WD = 12");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(13, 0, 0), 22260, 0, "Wrong sector count in WD = 13");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(14, 0, 0), 19661, 0, "Wrong sector count in WD = 14");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(15, 0, 0), 15538, 0, "Wrong sector count in WD = 15");
            
        }

        [TestMethod]
        public void Find_Sector_Counts_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string PC_Name = Get_PC_Name();
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 1;
            ThisMCP.Num_Temp_bins = 1;
            ThisMCP.Find_Sector_Counts();

            Assert.AreEqual(ThisMCP.Get_Sector_Count(0, 0, 0), 12529, 0, "Wrong sector count in WD = 0");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(1, 0, 0), 11360, 0, "Wrong sector count in WD = 1");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(2, 0, 0), 12149, 0, "Wrong sector count in WD = 2");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(3, 0, 0), 13874, 0, "Wrong sector count in WD = 3");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(4, 0, 0), 13380, 0, "Wrong sector count in WD = 4");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(5, 0, 0), 10538, 0, "Wrong sector count in WD = 5");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(6, 0, 0), 9180, 0, "Wrong sector count in WD = 6");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(7, 0, 0), 10799, 0, "Wrong sector count in WD = 7");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(8, 0, 0), 15793, 0, "Wrong sector count in WD = 8");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(9, 0, 0), 24231, 0, "Wrong sector count in WD = 9");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(10, 0, 0), 29027, 0, "Wrong sector count in WD = 10");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(11, 0, 0), 26659, 0, "Wrong sector count in WD = 11");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(12, 0, 0), 24669, 0, "Wrong sector count in WD = 12");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(13, 0, 0), 22260, 0, "Wrong sector count in WD = 13");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(14, 0, 0), 19661, 0, "Wrong sector count in WD = 14");
            Assert.AreEqual(ThisMCP.Get_Sector_Count(15, 0, 0), 15538, 0, "Wrong sector count in WD = 15");

        }


        [TestMethod]
        public void New_MCP_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string PC_Name = Get_PC_Name();
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\Paulding 10-min data_hourly.csv";
            ThisMCP.Import_Target_Data(filename);

            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 1;
            ThisMCP.Num_Temp_bins = 1;
            ThisMCP.Find_Min_Max_temp();
            ThisMCP.cboMCP_Type.SelectedText = "Orth. Regression";
            ThisMCP.Do_MCP(ThisMCP.Conc_Start, ThisMCP.Conc_End, true, "Orth. Regression");

            ThisMCP.New_MCP(true, true);
            Assert.AreEqual(ThisMCP.Get_Num_WD(), 16, 0, "Didn't reset the number of WD sectors");
            Assert.AreEqual(ThisMCP.Got_Conc, false, "Didn't reset the concurrent data");
            Assert.AreEqual(ThisMCP.Got_Ref, false, "Didn't reset the reference data");
            Assert.AreEqual(ThisMCP.Got_Targ, false, "Didn't reset the target data");

            filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\Paulding 10-min data_hourly.csv";
            ThisMCP.Import_Target_Data(filename);

            ThisMCP.Do_MCP(ThisMCP.Conc_Start, ThisMCP.Conc_End, true, "Orth. Regression");
            ThisMCP.New_MCP(false, true); // clears reference data only
            Assert.AreEqual(ThisMCP.Got_Conc, false, "Didn't reset the concurrent data");
            Assert.AreEqual(ThisMCP.Got_Ref, true, "Reset the reference data when it should not have");
            Assert.AreEqual(ThisMCP.Got_Targ, false, "Didn't reset the target data");

        }

        [TestMethod]
        public void txtWS_bin_width_TextChanged_Test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string PC_Name = Get_PC_Name();
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\Paulding 10-min data_hourly.csv";
            ThisMCP.Import_Target_Data(filename);

            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 1;
            ThisMCP.Num_Temp_bins = 1;
            ThisMCP.Find_Min_Max_temp();
            ThisMCP.cboMCP_Type.SelectedText = "Matrix";
            Assert.AreEqual(ThisMCP.WS_bin_width, 1, 0, "Wrong WS bin width. Should be 1");

            ThisMCP.txtWS_bin_width.Text = "0.5";
            Assert.AreEqual(ThisMCP.WS_bin_width, 0.5, 0, "Wrong WS bin width. Should be 0.5");

            ThisMCP.txtWS_bin_width.Text = "2";
            Assert.AreEqual(ThisMCP.WS_bin_width, 2, 0, "Wrong WS bin width. Should be 2");

        }

        [TestMethod]
        public void txtWS_PDF_Wgt_TextChanged_Test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string PC_Name = Get_PC_Name();
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\Paulding 10-min data_hourly.csv";
            ThisMCP.Import_Target_Data(filename);

            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 1;
            ThisMCP.Num_Temp_bins = 1;            
            ThisMCP.Find_Min_Max_temp();
            ThisMCP.cboMCP_Type.SelectedText = "Matrix";

            ThisMCP.txtWS_PDF_Wgt.Text = "10";
            Assert.AreEqual(ThisMCP.Matrix_Wgt, 10, 0, "Wrong Matrix PDF weight");

            ThisMCP.txtWS_PDF_Wgt.Text = "4";
            Assert.AreEqual(ThisMCP.Matrix_Wgt, 4, 0, "Wrong Matrix PDF weight");

        }

        [TestMethod]
        public void txtLastWS_PDF_Wgt_TextChanged_Test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string PC_Name = Get_PC_Name();
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\Paulding 10-min data_hourly.csv";
            ThisMCP.Import_Target_Data(filename);

            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 1;
            ThisMCP.Num_Temp_bins = 1;
            ThisMCP.Find_Min_Max_temp();
            ThisMCP.cboMCP_Type.SelectedText = "Matrix";

            ThisMCP.txtLast_WS_Wgt.Text = "10";
            Assert.AreEqual(ThisMCP.LastWS_Wgt, 10, 0, "Wrong Last WS PDF weight");

            ThisMCP.txtLast_WS_Wgt.Text = "4";
            Assert.AreEqual(ThisMCP.LastWS_Wgt, 4, 0, "Wrong Last WS PDF weight");
        }

        [TestMethod]
        public void Set_Conc_Dates_On_Form_Test()
        {
            MCP_tool ThisMCP = new MCP_tool();

            string PC_Name = Get_PC_Name();
            string filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\" + PC_Name + "\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\Paulding 10-min data_hourly.csv";
            ThisMCP.Import_Target_Data(filename);

            ThisMCP.date_Corr_Start.Value = Convert.ToDateTime("12/1/2008 19:00:00");
            ThisMCP.date_Corr_End.Value = Convert.ToDateTime("7/24/2009 02:00:00");

            ThisMCP.Set_Conc_Dates_On_Form();

            Assert.AreEqual(ThisMCP.date_Corr_Start.Value, Convert.ToDateTime("9/28/2008 16:00:00"), "Wrong start date");
            Assert.AreEqual(ThisMCP.date_Corr_End.Value, Convert.ToDateTime("9/28/2009 14:00:00"), "Wrong end date");

        }

        [TestMethod]
        public void Set_Default_Folder_locations_Test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            ThisMCP.Set_Default_Folder_locations("C:\\Users");

            Assert.AreEqual(ThisMCP.ofdOpenMCP.InitialDirectory, "C:\\", "Wrong initial directory");

        }

    }

}
    
