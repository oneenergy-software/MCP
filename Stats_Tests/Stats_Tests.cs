using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MCP;
using System.Windows.Forms;

namespace Stats_Tests
{
    [TestClass]
    public class Stats_Tests
    {
        [TestMethod]
        public void Calc_Avg_WS_Test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            string filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\Paulding 10-min data_hourly.csv";
            ThisMCP.Import_Target_Data(filename);

            Stats ThisStats = new Stats();
            DateTime Start = Convert.ToDateTime("10/1/2008 12:00:00 AM");
            DateTime End = Convert.ToDateTime("10/31/2008 11:00:00 PM");

            // Test 1
            float This_Avg = ThisStats.Calc_Avg_WS(ThisMCP.Target_Data, 6, 7, Start, End, 90, 270, true, 0, true, 0, ThisMCP);
            Assert.AreEqual(This_Avg, 6.49889, 0.001, "Wrong Avg WS");

            // Test 2
            Start = Convert.ToDateTime("2/1/2009 12:00:00 AM");
            End = Convert.ToDateTime("2/13/2009 1:00:00 PM");
            ThisMCP.Num_Hourly_Ints = 2;
            
            This_Avg = ThisStats.Calc_Avg_WS(ThisMCP.Target_Data, 5, 10, Start, End, 210, 300, false, 0, true, 0, ThisMCP);
            Assert.AreEqual(This_Avg, 6.783322, 0.001, "Wrong Avg WS");
        }

        [TestMethod]
        public void Get_Data_Count_Test()
        {
            MCP_tool ThisMCP = new MCP_tool();
            Stats These_Stats = new Stats();
            string filename = "C:\\Users\\OEE2015_12\\Dropbox (OEE)\\Software - Development\\MCP\\v1.0\\QA Backup files\\MERRA2_Lat_41.5_Long_-84.375_19850101_to_20151231_50mWS_50mWD_10mTemp.csv";
            ThisMCP.Import_Reference_data(filename);
            ThisMCP.Num_WD_Sectors = 16;
            ThisMCP.Num_Hourly_Ints = 1;
            ThisMCP.Num_Temp_bins = 2;
            ThisMCP.Find_Min_Max_temp();

            DateTime Start = Convert.ToDateTime("3/4/2009 4:00:00 AM");
            DateTime End = Convert.ToDateTime("5/16/2009 4:00:00 PM");

            // Test 1
            int This_Count = These_Stats.Get_Data_Count(ThisMCP.Ref_Data, Start, End, 7, 0, 1, ThisMCP, false);
           Assert.AreEqual(This_Count, 45, 0, "Wrong data count");

            ThisMCP.Num_WD_Sectors = 4;
            ThisMCP.Num_Hourly_Ints = 2;
            ThisMCP.Num_Temp_bins = 2;
            ThisMCP.Find_Min_Max_temp();

            Start = Convert.ToDateTime("1/1/1985 12:00:00 AM");
            End = Convert.ToDateTime("12/31/2015 12:00:00 AM");

            // Test 2
            This_Count = These_Stats.Get_Data_Count(ThisMCP.Ref_Data, Start, End, 0, 1, 0, ThisMCP, false);
            Assert.AreEqual(This_Count, 14114, 0, "Wrong data count");
        }

        [TestMethod]
        public void Calc_Variance_Test()
        {
            float[] Values = new float[12];
            Values[0] = 0.54f;
            Values[1] = 0.108f;
            Values[2] = 0.789f;
            Values[3] = 0.55f;
            Values[4] = 0.83f;
            Values[5] = 3.64f;
            Values[6] = 87.6f;
            Values[7] = 5.3f;
            Values[8] = 0.95f;
            Values[9] = 3.5f;
            Values[10] = 0.605f;
            Values[11] = 2.36f;

            Stats These_Stats = new Stats();

            double This_Var = These_Stats.Calc_Variance(Values);
            Assert.AreEqual(This_Var, 565.507, 0.001, "Wrong Variance");
            
        }

        [TestMethod]
        public void Calc_Covariance_Test()
        {
            float[] X_Values = new float[12];
            X_Values[0] = 0.54f;
            X_Values[1] = 0.108f;
            X_Values[2] = 0.789f;
            X_Values[3] = 0.55f;
            X_Values[4] = 0.83f;
            X_Values[5] = 3.64f;
            X_Values[6] = 87.6f;
            X_Values[7] = 5.3f;
            X_Values[8] = 0.95f;
            X_Values[9] = 3.5f;
            X_Values[10] = 0.605f;
            X_Values[11] = 2.36f;

            float[] Y_Values = new float[12];
            Y_Values[0] = 4.56f;
            Y_Values[1] = 6.07f;
            Y_Values[2] = 0.95f;
            Y_Values[3] = 31f;
            Y_Values[4] = 1.67f;
            Y_Values[5] = 0.21f;
            Y_Values[6] = 8.34f;
            Y_Values[7] = 11.54f;
            Y_Values[8] = 3.66f;
            Y_Values[9] = 4.54f;
            Y_Values[10] = 0.85f;
            Y_Values[11] = 5.89f;

            Stats These_Stats = new Stats();
            double This_Cov = These_Stats.Calc_Covariance(X_Values, Y_Values);
            Assert.AreEqual(This_Cov, 11.93237, 0.001, "Wrong co-variance");

        }

        [TestMethod]
        public void Calc_R_sqr()
        {
            float[] X_Values = new float[12];
            X_Values[0] = 0.54f;
            X_Values[1] = 0.108f;
            X_Values[2] = 0.789f;
            X_Values[3] = 0.55f;
            X_Values[4] = 0.83f;
            X_Values[5] = 3.64f;
            X_Values[6] = 87.6f;
            X_Values[7] = 5.3f;
            X_Values[8] = 0.95f;
            X_Values[9] = 3.5f;
            X_Values[10] = 0.605f;
            X_Values[11] = 2.36f;

            float[] Y_Values = new float[12];
            Y_Values[0] = 4.56f;
            Y_Values[1] = 6.07f;
            Y_Values[2] = 0.95f;
            Y_Values[3] = 31f;
            Y_Values[4] = 1.67f;
            Y_Values[5] = 0.21f;
            Y_Values[6] = 8.34f;
            Y_Values[7] = 11.54f;
            Y_Values[8] = 3.66f;
            Y_Values[9] = 4.54f;
            Y_Values[10] = 0.85f;
            Y_Values[11] = 5.89f;

            Stats These_Stats = new Stats();
            double Var_X = These_Stats.Calc_Variance(X_Values);
            double Var_Y = These_Stats.Calc_Variance(Y_Values);
            double This_Cov = These_Stats.Calc_Covariance(X_Values, Y_Values);
            double This_Rsq = These_Stats.Calc_R_sqr((float)This_Cov, (float)Var_X, (float)Var_Y);

            Assert.AreEqual(This_Rsq, 0.00392, 0.0001, "Wrong R sq");
        }

        [TestMethod]
        public void Calc_Intercept_Test()
        {
            float X = 6.456f;
            float Y = 5.289f;
            float Slope = 1.0943f;

            Stats These_Stats = new Stats();
            float This_Int = These_Stats.Calc_Intercept(Y, Slope, X);

            Assert.AreEqual(This_Int, -1.7758, 0.001, "Wrong Intercept");

        }

    }
}
