using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MCP;
using System.IO;
using System.Windows.Forms;

namespace MCP_Tests
{
    [TestClass]
    public class MCP_Tests
    {
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
            string filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";

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
            string filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\QA Backup files\\Paulding 10-min data_hourly.csv";
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
            string filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\QA Backup files\\Paulding 10-min data_hourly.csv";
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
            string filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\QA Backup files\\Paulding 10-min data_hourly.csv";
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
            string filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
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
            string filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
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
            string filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\QA Backup files\\Paulding 10-min data_hourly.csv";
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
            string filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\QA Backup files\\Paulding 10-min data_hourly.csv";
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
            string filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\QA Backup files\\Paulding 10-min data_hourly.csv";
            ThisMCP.Import_Target_Data(filename);

            ThisMCP.WS_bin_width = 1;
            ThisMCP.Find_SD_Change_in_WS();

            Assert.AreEqual(0, 44, 0, "TEST INCOMPLETE");
        }

        [TestMethod]
        public void Get_Lag_WS_CDF_test()
        {
            Assert.AreEqual(0, 44, 0, "TEST INCOMPLETE");
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
            string filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\QA Backup files\\Paulding 10-min data_hourly.csv";
            ThisMCP.Import_Target_Data(filename);
            ThisMCP.cboMCP_Type.SelectedIndex = 0; // orthogonal

            ThisMCP.Do_MCP_Uncertainty();

            Assert.AreEqual(ThisMCP.Uncert_Ortho[0].avg, 6.30065, 0.001, "Wrong calculated average of LT estimates in uncertainty analysis");
            Assert.AreEqual(ThisMCP.Uncert_Ortho[0].std_dev, 0.211046, 0.001, "Wrong calculated standard deviation of LT estimates in uncertainty analysis");
        }

        [TestMethod]
        public void Do_MCP_Uncertainty_test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\QA Backup files\\Paulding 10-min data_hourly.csv";
            ThisMCP.Import_Target_Data(filename);
            ThisMCP.cboMCP_Type.SelectedIndex = 0; // orthogonal

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
            string filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);

            Assert.AreEqual(ThisMCP.Ref_Data.Length / 365.0 / 24.0, 31.01, 0.01, "Wrong reference data length");
            Stats Stat = new Stats();
            float Ref_Avg =  Stat.Calc_Avg_WS(ThisMCP.Ref_Data, 0, 10000, ThisMCP.Ref_Start, ThisMCP.Ref_End, 0, 360, true , 0, ThisMCP);

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

        }
   }

}
    
