using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Runtime.Serialization.Formatters.Binary;


namespace MCP
{
    [Serializable()]
    public partial class MCP_tool : Form
    {
        DateTime Ref_Start;
        DateTime Ref_End;
        DateTime Target_Start;
        DateTime Target_End;
        DateTime Conc_Start;
        DateTime Conc_End;
        DateTime Export_Start;
        DateTime Export_End;

        public Site_data[] Ref_Data = new Site_data[0];
        bool Got_Ref = false;
        string Ref_filename = "";
        Site_data[] Target_Data = new Site_data[0];
        bool Got_Targ = false;
        string Target_filename = "";

        Concurrent_data[] Conc_Data = new Concurrent_data[0];
        bool Got_Conc = false;

        int Num_WD_Sectors = 1;
        float WS_bin_width = 1;

        Lin_MCP MCP_Ortho;
        Method_of_Bins MCP_Bins;
        Lin_MCP MCP_Varrat;

        MCP_Uncert[] Uncert_Ortho = new MCP_Uncert[0];
        MCP_Uncert[] Uncert_Bins = new MCP_Uncert[0];
        MCP_Uncert[] Uncert_Varrat = new MCP_Uncert[0];

        public Stats Stat = new Stats();
        string Saved_Filename = "";

        bool Is_Newly_Opened_File = false;
        
        // structure for MCP_Uncertainty
        [Serializable()]
        public struct MCP_Uncert
        {
            public int WSize; //Window size
            public int NWindows; //Number of windows per given array size
            public double[] LT_Ests;
            public float avg;
            public float std_dev;
            public DateTime[] Start;
            public DateTime[] End;

            public void Clear()
            {
                WSize = 0;
                NWindows = 0;
                LT_Ests = null;
                avg = 0;
                std_dev = 0;
                Start = null;
                End = null;

            }

        }

        [Serializable()]
        public struct Site_data
        {
            public DateTime This_Date;
            public float This_WS;
            public float This_WD;
        }

        [Serializable()]
        public struct Concurrent_data
        {
            public DateTime This_Date;
            public float Ref_WS;
            public float Ref_WD;
            public float Target_WS;
            public float Target_WD;
        }

        [Serializable()]
        public struct Lin_MCP
        {
            public float[] Slope; // Slope of linear MCP methods for each WD sector & overall
            public float[] Intercept; // Intercept of linear MCP methods for each WD sector & overall
            public float[] R_sq; // R^2 of linear MCP methods for WD & overall
            public Site_data[] LT_WS_Est; // Estimate of wind speed at target site based on linear MCP methods (WD is same as Ref site WD)

            public void Clear()
            {
                Slope = null;
                Intercept = null;
                R_sq = null;
                LT_WS_Est = null;
            }
        }

        [Serializable()]
        public struct Method_of_Bins
        {
            public Bin_Object[,] Bin_Avg_SD_Cnt; // i = WS bin, j = WD bin
            public Site_data[] LT_WS_Est; // Estimate of wind speed at target site based on method of bins (WD is same as Ref site WD)

            public void Clear()
            {
                Bin_Avg_SD_Cnt = null;
                LT_WS_Est = null;
            }
        }

        [Serializable()]
        public struct Bin_Object
        {
            public float Avg_WS_Ratio;
            public float SD_WS_Ratio;
            public float Count;
        }

        public MCP_tool()
        {
            InitializeComponent();
            cboMCP_Type.SelectedIndex = 0;

        }

        private void btnImportRef_Click(object sender, EventArgs e)
        {

            // Read in time series wind speed and WD data at reference site
            // Prompt user to find reference data file
            string filename = "";
            string line;
            DateTime This_Date;
            float This_WS;
            float This_WD;
            int Ref_count = 0;
            // Add time series data every 1000 data points (to speed up computation time by not resizing array every time)
            int New_data_count = 0;
            Site_data[] TS_data = null;
            Array.Resize(ref TS_data, 1000);

            if (ofdRefSite.ShowDialog() == DialogResult.OK)
                filename = ofdRefSite.FileName;

            if (filename != "")
            {
                StreamReader file;
                try
                {
                    file = new StreamReader(filename);
                }
                catch
                {
                    MessageBox.Show("Error opening the reference data file. Check that it's not open in another program.", "", MessageBoxButtons.OK);
                    return;
                }

                Ref_filename = filename;
                txtLoadedReference.Text = filename;
                while ((line = file.ReadLine()) != null)
                {
                    try
                    {
                        Char[] delims = { ',' };
                        String[] substrings = line.Split(delims);
                        // Only read in data intervals with valid WS & WD
                        if (substrings[1] != "NaN" && substrings[2] != "NaN" && Convert.ToSingle(substrings[1]) > 0)
                        {
                            This_Date = Convert.ToDateTime(substrings[0]);
                            This_WS = Convert.ToSingle(substrings[1]);
                            This_WD = Convert.ToSingle(substrings[2]);

                            if (New_data_count < 1000)
                            {
                                TS_data[New_data_count].This_Date = This_Date;
                                TS_data[New_data_count].This_WS = This_WS;
                                TS_data[New_data_count].This_WD = This_WD;
                                New_data_count = New_data_count + 1;
                            }
                            else
                            {
                                Array.Resize(ref Ref_Data, Ref_count + New_data_count);
                                for (int i = Ref_count; i < Ref_count + New_data_count; i++)
                                {
                                    Ref_Data[i].This_Date = TS_data[i - Ref_count].This_Date;
                                    Ref_Data[i].This_WS = TS_data[i - Ref_count].This_WS;
                                    Ref_Data[i].This_WD = TS_data[i - Ref_count].This_WD;
                                }

                                Ref_count = Ref_count + New_data_count;

                                New_data_count = 0;
                                Array.Resize(ref TS_data, 1000);
                            }
                        }

                    }
                    catch
                    { }
                }


                // add last of time series (< 1000)
                Array.Resize(ref Ref_Data, Ref_count + New_data_count);
                for (int i = Ref_count; i < Ref_count + New_data_count; i++)
                {
                    Ref_Data[i].This_Date = TS_data[i - Ref_count].This_Date;
                    Ref_Data[i].This_WS = TS_data[i - Ref_count].This_WS;
                    Ref_Data[i].This_WD = TS_data[i - Ref_count].This_WD;
                }
                Ref_count = Ref_count + New_data_count;

                file.Close();

                // Find start and end dates (in case the data file wasn't chronologically sorted)
                Ref_Start = Ref_Data[0].This_Date;
                Ref_End = Ref_Data[Ref_count - 1].This_Date;

                for (int i = 0; i < Ref_count; i++)
                {
                    if (Ref_Data[i].This_Date < Ref_Start)
                        Ref_Start = Ref_Data[i].This_Date;

                    if (Ref_Data[i].This_Date > Ref_End)
                        Ref_End = Ref_Data[i].This_Date;
                }

                Export_Start = Ref_Start;
                date_Export_Start.Value = Export_Start;
                Export_End = Ref_End;
                date_Export_End.Value = Export_End;

                Got_Ref = true;
                Set_Conc_Dates_On_Form();

                if (Target_Data.Length > 0)
                    Find_Concurrent_Data(true, Conc_Start, Conc_End);
                                
                Update_plot();
                Update_Text_boxes();
                Update_Dates();
                Changes_Made();
            }
        }

        public float Get_WS_width()
        {
            // Read WS interval width to be used in Method of Bins
            return WS_bin_width;
        }

        public int Get_WD_ind()
        {
            // Read selected WD sector to show in plot
            int WD_ind = 0;
            try
            {
                WD_ind = cboWD_sector.SelectedIndex;
                if (WD_ind == -1)
                {
                    cboWD_sector.SelectedIndex = 0;
                    WD_ind = 0;
                }

            }

            catch
            {
                cboWD_sector.SelectedIndex = 0;
                WD_ind = 0;
            }

            return WD_ind;
        }

        public int Get_Num_WD()
        {
                        
            return Num_WD_Sectors;
        }

        public string Get_MCP_Method()
        {
            string MCP_Method = "";

            try
            {
                MCP_Method = cboMCP_Type.SelectedText.ToString();
                MCP_Method = cboMCP_Type.Text;
            }
            catch
            { }

            return MCP_Method;
        }

        

        public float[] Get_Conc_Avgs_Count(float Min_WD, float Max_WD)
        {
            // Calculates and returns the average wind speed at the target and reference sites during the concurrent period for 
            // specified WD bounds as well as the data count

            float[] Avg_WS_WD = { 0, 0, 0 }; // 0: Target WS; 1: Reference WS; 2: Data Count'

            foreach (Concurrent_data Conc in Conc_Data)
                if (Conc.This_Date >= Conc_Start && Conc.This_Date <= Conc_End)
                {
                    if (Max_WD > Min_WD)
                    {
                        if (Conc.Ref_WD >= Min_WD && Conc.Ref_WD <= Max_WD)
                        {
                            Avg_WS_WD[0] = Avg_WS_WD[0] + Conc.Target_WS;
                            Avg_WS_WD[1] = Avg_WS_WD[1] + Conc.Ref_WS;
                            Avg_WS_WD[2] = Avg_WS_WD[2] + 1;
                        }
                    }
                    else if (Conc.Ref_WD >= Min_WD || Conc.Ref_WD <= Max_WD)
                    {
                        Avg_WS_WD[0] = Avg_WS_WD[0] + Conc.Target_WS;
                        Avg_WS_WD[1] = Avg_WS_WD[1] + Conc.Ref_WS;
                        Avg_WS_WD[2] = Avg_WS_WD[2] + 1;
                    }
                }

            if (Avg_WS_WD[2] > 0)
            {
                Avg_WS_WD[0] = Avg_WS_WD[0] / Avg_WS_WD[2];
                Avg_WS_WD[1] = Avg_WS_WD[1] / Avg_WS_WD[2];
            }

            return Avg_WS_WD;
        }

        public float[] Get_Conc_WS_Array(string Target_or_Ref, float Min_WD, float Max_WD)
        {
            // Returns array of WS for either the target or reference site for specified WD bounds
            // Used to form the scatterplot

            float[] These_WS = null;

            if (Got_Conc)
            {
                if (Min_WD == 0 && Max_WD == 360)
                {
                    Array.Resize(ref These_WS, Conc_Data.Length);

                    for (int i = 0; i < Conc_Data.Length; i++)
                        if (Target_or_Ref == "Target")
                            These_WS[i] = Conc_Data[i].Target_WS;
                        else
                            These_WS[i] = Conc_Data[i].Ref_WS;
                }
                else
                {
                    int WD_count = 0;
                    foreach (Concurrent_data These_Conc in Conc_Data)
                        if (Max_WD > Min_WD)
                        {
                            if (These_Conc.Ref_WD >= Min_WD && These_Conc.Ref_WD <= Max_WD)
                                WD_count++;
                        }
                        else if (These_Conc.Ref_WD >= Min_WD || These_Conc.Ref_WD <= Max_WD)
                            WD_count++;

                    Array.Resize(ref These_WS, WD_count);
                    WD_count = 0;

                    foreach (Concurrent_data These_Conc in Conc_Data)
                        if (Max_WD > Min_WD)
                        {
                            if (These_Conc.Ref_WD >= Min_WD && These_Conc.Ref_WD <= Max_WD)
                            {
                                if (Target_or_Ref == "Target")
                                    These_WS[WD_count] = These_Conc.Target_WS;
                                else
                                    These_WS[WD_count] = These_Conc.Ref_WS;
                                WD_count++;
                            }
                        }
                        else if (These_Conc.Ref_WD >= Min_WD || These_Conc.Ref_WD <= Max_WD)
                        {
                            if (Target_or_Ref == "Target")
                                These_WS[WD_count] = These_Conc.Target_WS;
                            else
                                These_WS[WD_count] = These_Conc.Ref_WS;
                            WD_count++;
                        }
                }
            }

            return These_WS;
        }

        
        private void btnImportTarget_Click(object sender, EventArgs e)
        {
            // Read in time series wind speed and WD data at reference site
            // Prompt user to find reference data file
            string filename = "";
            string line;
            DateTime This_Date;
            float This_WS;
            float This_WD;
            int Target_count = 0;
            int New_data_count = 0;
            Site_data[] TS_data = null;
            Array.Resize(ref TS_data, 1000);

            if (ofdRefSite.ShowDialog() == DialogResult.OK)
                filename = ofdRefSite.FileName;

            if (filename != "")
            {
                StreamReader file;

                try
                {
                    file = new StreamReader(filename);
                }
                catch
                {
                    MessageBox.Show("Error opening the target data file. Check that it's not open in another program.", "", MessageBoxButtons.OK);
                    return;
                }

                Target_filename = filename;
                txtLoadedTarget.Text = filename;

                while ((line = file.ReadLine()) != null)
                {
                    try
                    {
                        Char[] delims = { ',' };
                        String[] substrings = line.Split(delims);
                        if (substrings[1] != "NaN" && substrings[2] != "NaN" && Convert.ToSingle(substrings[1]) > 0)
                        {
                            This_Date = Convert.ToDateTime(substrings[0]);
                            This_WS = Convert.ToSingle(substrings[1]);
                            This_WD = Convert.ToSingle(substrings[2]);

                            if (New_data_count < 1000)
                            {
                                TS_data[New_data_count].This_Date = This_Date;
                                TS_data[New_data_count].This_WS = This_WS;
                                TS_data[New_data_count].This_WD = This_WD;
                                New_data_count = New_data_count + 1;
                            }
                            else
                            {
                                Array.Resize(ref Target_Data, Target_count + New_data_count);
                                for (int i = Target_count; i < Target_count + New_data_count; i++)
                                {
                                    Target_Data[i].This_Date = TS_data[i - Target_count].This_Date;
                                    Target_Data[i].This_WS = TS_data[i - Target_count].This_WS;
                                    Target_Data[i].This_WD = TS_data[i - Target_count].This_WD;
                                }

                                Target_count = Target_count + New_data_count;

                                New_data_count = 0;
                                Array.Resize(ref TS_data, 1000);

                            }
                        }


                    }
                    catch 
                    {

                    }
                }

                file.Close();

                Got_Targ = true;

                Set_Conc_Dates_On_Form();

                // Find concurrent data, if have target data
                if (Ref_Data.Length > 0)
                {
                    Find_Concurrent_Data(true, Conc_Start, Conc_End);

                }

                Update_plot();
                Update_Text_boxes();
                Changes_Made();

            }
        }

        public void Find_Concurrent_Data(bool Conc_form, DateTime Start, DateTime End)
        {
            // Creates array of Concurrent_data containing WS & WD at reference and target sites
            int Conc_count = 0;
            int Ref_Start_ind = 0;
            int Targ_Start_ind = 0;

            // Read the start and end dates for concurrent period
            if (Conc_form == true)
            {
                Conc_Start = date_Corr_Start.Value;
                Conc_End = date_Corr_End.Value;
            }
            else
            {
                Conc_Start = Start;
                Conc_End = End;
            }

            if (Ref_Data.Length == 0 || Target_Data.Length == 0) return;

            foreach (Site_data RefSite in Ref_Data)
            {
                if (RefSite.This_Date < Conc_Start)
                    Ref_Start_ind++;
                else
                    break;
            }

            foreach (Site_data TargSite in Target_Data)
            {
                if (TargSite.This_Date < Conc_Start)
                    Targ_Start_ind++;
                else
                    break;
            }

            for (int i = Targ_Start_ind; i < Target_Data.Length; i++)
            {
                for (int j = Ref_Start_ind; j < Ref_Data.Length; j++)
                {
                    if (Target_Data[i].This_Date == Ref_Data[j].This_Date && Target_Data[i].This_Date <= Conc_End)
                    {
                        Conc_count = Conc_count + 1;
                        Array.Resize(ref Conc_Data, Conc_count);
                        Conc_Data[Conc_count - 1].This_Date = Target_Data[i].This_Date;
                        Conc_Data[Conc_count - 1].Ref_WS = Ref_Data[j].This_WS;
                        Conc_Data[Conc_count - 1].Ref_WD = Ref_Data[j].This_WD;
                        Conc_Data[Conc_count - 1].Target_WS = Target_Data[i].This_WS;
                        Conc_Data[Conc_count - 1].Target_WD = Target_Data[i].This_WD;
                        break;
                    }

                }
                if (Target_Data[i].This_Date >= Conc_End)
                {
                    break;
                }
            }

            if (Conc_count == 0)
                MessageBox.Show("There is no concurrent data between the reference and target site for the selected start and end dates.");
            else
                Got_Conc = true;
            
        }
         
      
        public float Do_MCP(DateTime This_Conc_Start, DateTime This_Conc_End, bool Use_All_Data, string MCP_Method)
        {
            // Performs MCP using a linear model (i.e orthogonal regression or variance ratio) 
            // Orth. Reg. minimizes the distance between both the reference and target site wind speeds from the regression line
            
            Find_Concurrent_Data(Use_All_Data, This_Conc_Start, This_Conc_End);

            // Calculate the regression for each WD and overall
            // To calculate the slope and intercept, need the variance of Y and X and the co-variance of X and Y

            // First calculate for all WD sectors
            float Min_WD = 0;
            float Max_WD = 360;
            float[] Ref_WS = Get_Conc_WS_Array("Reference", Min_WD, Max_WD);
            float[] Target_WS = Get_Conc_WS_Array("Target", Min_WD, Max_WD);

            Stats Stat = new Stats();
            float var_x = Convert.ToSingle(Stat.Calc_Variance(Ref_WS));
            float var_y = Convert.ToSingle(Stat.Calc_Variance(Target_WS));
            float covar_xy = Convert.ToSingle(Stat.Calc_Covariance(Ref_WS, Target_WS));

            int Num_WD = Get_Num_WD();
            int WD_ind = Get_WD_ind();
            double WS_bin = Get_WS_width();
            int Num_WS = (int)(30 / WS_bin);

            Method_of_Bins Uncert_MCP = new Method_of_Bins();
            Method_of_Bins These_Bins = new Method_of_Bins();

            float[] This_Conc = Get_Conc_Avgs_Count(Min_WD, Max_WD);
            float Avg_Targ = This_Conc[0];
            float Avg_Ref = This_Conc[1];

            float LT_WS_Est = 0; // if Use_All_Data is false then it is an uncertainty analysis and this value is returned
            float This_Slope = 0;
            float This_Int = 0;
            int Total_Count = 0;
            int Sector_Count = 0;

            try
            {
                Total_Count = Ref_WS.Length;
            }
            catch
            {
                return LT_WS_Est;
            }

           // if this is not an uncertainty analysis, then calculate the slope, intercept and R^2 for all WD (this is not used in LT WS Estimation, just GUI)
            if (Use_All_Data == true && MCP_Method == "Orth. Regression")
            {
                MCP_Ortho.Clear();
                Array.Resize(ref MCP_Ortho.Slope, Num_WD + 1); // Slope for each WD plus overall
                Array.Resize(ref MCP_Ortho.Intercept, Num_WD + 1); // Intercept for each WD plus overall
                Array.Resize(ref MCP_Ortho.R_sq, Num_WD + 1); // R_sq for each WD plus overall

                MCP_Ortho.Slope[Num_WD] = Calc_Ortho_Slope(var_x, var_y, covar_xy);
                MCP_Ortho.Intercept[Num_WD] = Avg_Targ - MCP_Ortho.Slope[Num_WD] * Avg_Ref;
                MCP_Ortho.R_sq[Num_WD] = (float)Math.Pow(covar_xy / (float)Math.Pow(var_x, 0.5) / (float)Math.Pow(var_y, 0.5), 2);
            }
            else if (Use_All_Data == true && MCP_Method == "Variance Ratio") 
            {
                MCP_Varrat.Clear();
                Array.Resize(ref MCP_Varrat.Slope, Num_WD + 1); // Slope for each WD plus overall
                Array.Resize(ref MCP_Varrat.Intercept, Num_WD + 1); // Intercept for each WD plus overall
                Array.Resize(ref MCP_Varrat.R_sq, Num_WD + 1); // R_sq for each WD plus overall

                MCP_Varrat.Slope[Num_WD] = Calc_Varrat_Slope(var_x, var_y);
                MCP_Varrat.Intercept[Num_WD] = Avg_Targ - MCP_Varrat.Slope[Num_WD] * Avg_Ref;
                MCP_Varrat.R_sq[Num_WD] = (float)Math.Pow(covar_xy / (float)Math.Pow(var_x, 0.5) / (float)Math.Pow(var_y, 0.5), 2);
            }
            else if (Use_All_Data == true && MCP_Method == "Method of Bins")
            {
                MCP_Bins.Clear();
                MCP_Bins.Bin_Avg_SD_Cnt = new Bin_Object[Num_WS, Num_WD + 1]; // WD_ind = Num_WD is overall ratio
            }
            
            // Now calculate for all WD
            if (MCP_Method == "Orth. Regression" || MCP_Method == "Variance Ratio")
            {
                for (int i = 0; i < Num_WD; i++)
                {

                    if (Num_WD == 1)
                    {
                        Min_WD = 0;
                        Max_WD = 360;
                    }
                    else
                    {
                        Min_WD = i * 360 / Num_WD - 360 / Num_WD / 2;
                        if (Min_WD < 0) Min_WD = Min_WD + 360;

                        Max_WD = i * 360 / Num_WD + 360 / Num_WD / 2;
                        if (Max_WD > 360) Max_WD = Max_WD - 360;
                    }

                    

                    Ref_WS = Get_Conc_WS_Array("Reference", Min_WD, Max_WD);
                    Target_WS = Get_Conc_WS_Array("Target", Min_WD, Max_WD);

                    try
                    {
                        Sector_Count = Ref_WS.Length;
                    }
                    catch
                    {
                        Sector_Count = 0;
                    }

                    var_x = Convert.ToSingle(Stat.Calc_Variance(Ref_WS));
                    var_y = Convert.ToSingle(Stat.Calc_Variance(Target_WS));
                    covar_xy = Convert.ToSingle(Stat.Calc_Covariance(Ref_WS, Target_WS));

                    This_Conc = Get_Conc_Avgs_Count(Min_WD, Max_WD);
                    Avg_Targ = This_Conc[0];
                    Avg_Ref = This_Conc[1];

                    if (MCP_Method == "Orth. Regression")
                        This_Slope = Calc_Ortho_Slope(var_x, var_y, covar_xy);
                    else
                        This_Slope = Calc_Varrat_Slope(var_x, var_y);

                    This_Int = Avg_Targ - This_Slope * Avg_Ref;

                    if (Use_All_Data == true)
                    {
                        if (MCP_Method == "Orth. Regression")
                        {
                            MCP_Ortho.Slope[i] = This_Slope;
                            MCP_Ortho.Intercept[i] = This_Int;
                            MCP_Ortho.R_sq[i] = (float)Math.Pow(covar_xy / (float)Math.Pow(var_x, 0.5) / (float)Math.Pow(var_y, 0.5), 2);
                        }
                        else // if more linear models are added, will need to add another else if
                        {
                            MCP_Varrat.Slope[i] = This_Slope;
                            MCP_Varrat.Intercept[i] = This_Int;
                            MCP_Varrat.R_sq[i] = (float)Math.Pow(covar_xy / (float)Math.Pow(var_x, 0.5) / (float)Math.Pow(var_y, 0.5), 2);
                        }
                    }

                    Avg_Ref = Stat.Calc_Avg_WS(Ref_Data, 0, 30, Ref_Start, Ref_End, Min_WD, Max_WD);
                    
                    if (Double.IsNaN(This_Slope) == false) LT_WS_Est = LT_WS_Est + (Avg_Ref * This_Slope + This_Int) * ((float)Sector_Count / (float)Total_Count);
                }
            }
            else // Method of Bins
            {

                if (Use_All_Data)
                    These_Bins = MCP_Bins;
                else
                {
                    Uncert_MCP.Bin_Avg_SD_Cnt = new Bin_Object[Num_WS, Num_WD + 1]; // WD_ind = Num_WD is overall ratio
                    These_Bins = Uncert_MCP;
                }

                foreach (Concurrent_data These_Conc in Conc_Data)
                {
                    int WS_ind = Convert.ToInt32(Math.Round(These_Conc.Ref_WS / WS_bin_width, 0));
                    if (WS_ind > Num_WS) WS_ind = Num_WS - 1;

                    WD_ind = Convert.ToInt32(These_Conc.Ref_WD / (360 / Num_WD));
                    if (WD_ind == Num_WD) WD_ind = 0;

                    // Directional ratios
                    These_Bins.Bin_Avg_SD_Cnt[WS_ind, WD_ind].Avg_WS_Ratio = These_Bins.Bin_Avg_SD_Cnt[WS_ind, WD_ind].Avg_WS_Ratio + These_Conc.Target_WS / These_Conc.Ref_WS;
                    These_Bins.Bin_Avg_SD_Cnt[WS_ind, WD_ind].SD_WS_Ratio = These_Bins.Bin_Avg_SD_Cnt[WS_ind, WD_ind].SD_WS_Ratio + (float)Math.Pow(These_Conc.Target_WS / These_Conc.Ref_WS, 2);
                    These_Bins.Bin_Avg_SD_Cnt[WS_ind, WD_ind].Count++;

                    // Overall ratios (all WD)
                    These_Bins.Bin_Avg_SD_Cnt[WS_ind, Num_WD].Avg_WS_Ratio = These_Bins.Bin_Avg_SD_Cnt[WS_ind, Num_WD].Avg_WS_Ratio + These_Conc.Target_WS / These_Conc.Ref_WS;
                    These_Bins.Bin_Avg_SD_Cnt[WS_ind, Num_WD].SD_WS_Ratio = These_Bins.Bin_Avg_SD_Cnt[WS_ind, Num_WD].SD_WS_Ratio + (float)Math.Pow(These_Conc.Target_WS / These_Conc.Ref_WS, 2);
                    These_Bins.Bin_Avg_SD_Cnt[WS_ind, Num_WD].Count++;
                    
                }

                for (int i = 0; i < Num_WS; i++)
                    for (int j = 0; j <= Num_WD; j++)
                    {
                        if (These_Bins.Bin_Avg_SD_Cnt[i, j].Count > 0)
                        {
                            These_Bins.Bin_Avg_SD_Cnt[i, j].Avg_WS_Ratio = These_Bins.Bin_Avg_SD_Cnt[i, j].Avg_WS_Ratio / These_Bins.Bin_Avg_SD_Cnt[i, j].Count;
                            These_Bins.Bin_Avg_SD_Cnt[i, j].SD_WS_Ratio = These_Bins.Bin_Avg_SD_Cnt[i, j].SD_WS_Ratio / These_Bins.Bin_Avg_SD_Cnt[i, j].Count -
                                    (float)Math.Pow(These_Bins.Bin_Avg_SD_Cnt[i, j].Avg_WS_Ratio, 2);
                            }
                    }
                                   

            }
            
            if (Use_All_Data == false && MCP_Method != "Method of Bins") // if conducting uncertainty analysis (with a linear model) then return the LT value
                return LT_WS_Est;

            Update_plot();
            Update_Bin_List();

            // Estimate time series at target site
            if (MCP_Method == "Orth. Regression") Array.Resize(ref MCP_Ortho.LT_WS_Est, Ref_Data.Length);
            if (MCP_Method == "Variance Ratio") Array.Resize(ref MCP_Varrat.LT_WS_Est, Ref_Data.Length);
            if (MCP_Method == "Method of Bins") Array.Resize(ref These_Bins.LT_WS_Est, Ref_Data.Length);

            for (int i = 0; i < Ref_Data.Length; i++)
            {
                int This_WD_ind = Convert.ToInt32(Ref_Data[i].This_WD * Num_WD / 360);
                if (This_WD_ind == Num_WD) This_WD_ind = 0;
                int WS_ind = Convert.ToInt32(Math.Round(Ref_Data[i].This_WS /  WS_bin_width, 0));

                if (MCP_Method == "Orth. Regression")
                {
                    MCP_Ortho.LT_WS_Est[i].This_Date = Ref_Data[i].This_Date;
                    MCP_Ortho.LT_WS_Est[i].This_WD = Ref_Data[i].This_WD;
                    MCP_Ortho.LT_WS_Est[i].This_WS = Ref_Data[i].This_WS * MCP_Ortho.Slope[This_WD_ind] + MCP_Ortho.Intercept[This_WD_ind];
                }
                else if (MCP_Method == "Variance Ratio")
                {
                    MCP_Varrat.LT_WS_Est[i].This_Date = Ref_Data[i].This_Date;
                    MCP_Varrat.LT_WS_Est[i].This_WD = Ref_Data[i].This_WD;
                    MCP_Varrat.LT_WS_Est[i].This_WS = Ref_Data[i].This_WS * MCP_Varrat.Slope[This_WD_ind] + MCP_Varrat.Intercept[This_WD_ind];

                }
                else if (MCP_Method == "Method of Bins")
                {
                    These_Bins.LT_WS_Est[i].This_Date = Ref_Data[i].This_Date;
                    if (These_Bins.Bin_Avg_SD_Cnt[WS_ind, This_WD_ind].Avg_WS_Ratio > 0)
                        These_Bins.LT_WS_Est[i].This_WS = Ref_Data[i].This_WS * These_Bins.Bin_Avg_SD_Cnt[WS_ind, This_WD_ind].Avg_WS_Ratio;
                    else
                    {
                        // there was no data for this bin so find the two closest ratios and use average of two
                        float Avg_Ratio = 0;
                        int Avg_Ratio_count = 0;
                        int Minus_Ind = WS_ind;
                        int Plus_Ind = WS_ind;
                        int count_while = 0;

                        while (Avg_Ratio_count < 2 && (Minus_Ind != 0 || Plus_Ind != Num_WS))
                        {
                            if (Minus_Ind > 0) Minus_Ind--;
                            if (Plus_Ind < (Num_WS - 1)) Plus_Ind++;

                            if (These_Bins.Bin_Avg_SD_Cnt[Minus_Ind, This_WD_ind].Avg_WS_Ratio > 0)
                            {
                                Avg_Ratio = Avg_Ratio + These_Bins.Bin_Avg_SD_Cnt[Minus_Ind, This_WD_ind].Avg_WS_Ratio;
                                Avg_Ratio_count++;
                            }

                            if (These_Bins.Bin_Avg_SD_Cnt[Plus_Ind, This_WD_ind].Avg_WS_Ratio > 0)
                            {
                                Avg_Ratio = Avg_Ratio + These_Bins.Bin_Avg_SD_Cnt[Plus_Ind, This_WD_ind].Avg_WS_Ratio;
                                Avg_Ratio_count++;
                            }
                            count_while++;
                            if (count_while > 30)
                            {
                                break;
                            }
                        }

                        if (Avg_Ratio_count > 0) Avg_Ratio = Avg_Ratio / Avg_Ratio_count;
                        These_Bins.LT_WS_Est[i].This_WS = Ref_Data[i].This_WS * Avg_Ratio;
                    }
                    These_Bins.LT_WS_Est[i].This_WD = Ref_Data[i].This_WD;

                }
                
            }

            if (MCP_Method == "Method of Bins" && Use_All_Data == true)
                MCP_Bins = These_Bins;
            else if (MCP_Method == "Method of Bins")
                LT_WS_Est = Stat.Calc_Avg_WS(These_Bins.LT_WS_Est, 0, 30, Ref_Start, Ref_End, Min_WD, Max_WD);
            
            
            Update_Text_boxes();
            Update_Export_buttons();

            return LT_WS_Est;
        }

        public void Do_Method_of_Bins()
        {
            // Using specified WS interval width and number of WD sectors, "Method of Bins" calculates the average ratio between
            // the target and reference data during the concurrent period. These ratios are then used with the long-term reference
            // data to estimate the long-term wind speed at the target site

            int Num_WD = Get_Num_WD();
            double WS_bin = Get_WS_width();
            int Num_WS = (int)(30 / WS_bin);

            MCP_Bins.Bin_Avg_SD_Cnt = new Bin_Object[Num_WS, Num_WD + 1]; // WD_ind = Num_WD is overall ratio

            // Go through all concurrent data and calculate average, SD and count of WS ratio for each WD and WS bin
            if (Conc_Data.Length == 0) Find_Concurrent_Data(true, Conc_Start, Conc_End);

            foreach (Concurrent_data These_Conc in Conc_Data)
            {
                int WS_ind = Convert.ToInt32(Math.Round(These_Conc.Ref_WS, 0));
                if (WS_ind > Num_WS) WS_ind = Num_WS - 1;

                int WD_ind = Convert.ToInt32(These_Conc.Ref_WD / (360 / Num_WD));
                if (WD_ind == Num_WD) WD_ind = 0;

                // Directional ratios
                MCP_Bins.Bin_Avg_SD_Cnt[WS_ind, WD_ind].Avg_WS_Ratio = MCP_Bins.Bin_Avg_SD_Cnt[WS_ind, WD_ind].Avg_WS_Ratio + These_Conc.Target_WS / These_Conc.Ref_WS;
                MCP_Bins.Bin_Avg_SD_Cnt[WS_ind, WD_ind].SD_WS_Ratio = MCP_Bins.Bin_Avg_SD_Cnt[WS_ind, WD_ind].SD_WS_Ratio + (float)Math.Pow(These_Conc.Target_WS / These_Conc.Ref_WS, 2);
                MCP_Bins.Bin_Avg_SD_Cnt[WS_ind, WD_ind].Count++;

                // Overall ratios (all WD)
                MCP_Bins.Bin_Avg_SD_Cnt[WS_ind, Num_WD].Avg_WS_Ratio = MCP_Bins.Bin_Avg_SD_Cnt[WS_ind, Num_WD].Avg_WS_Ratio + These_Conc.Target_WS / These_Conc.Ref_WS;
                MCP_Bins.Bin_Avg_SD_Cnt[WS_ind, Num_WD].SD_WS_Ratio = MCP_Bins.Bin_Avg_SD_Cnt[WS_ind, Num_WD].SD_WS_Ratio + (float)Math.Pow(These_Conc.Target_WS / These_Conc.Ref_WS, 2);
                MCP_Bins.Bin_Avg_SD_Cnt[WS_ind, Num_WD].Count++;
            }

            for (int i = 0; i < Num_WS; i++)
                for (int j = 0; j <= Num_WD; j++)
                {
                    if (MCP_Bins.Bin_Avg_SD_Cnt[i, j].Count > 0)
                    {
                        MCP_Bins.Bin_Avg_SD_Cnt[i, j].Avg_WS_Ratio = MCP_Bins.Bin_Avg_SD_Cnt[i, j].Avg_WS_Ratio / MCP_Bins.Bin_Avg_SD_Cnt[i, j].Count;
                        MCP_Bins.Bin_Avg_SD_Cnt[i, j].SD_WS_Ratio = MCP_Bins.Bin_Avg_SD_Cnt[i, j].SD_WS_Ratio / MCP_Bins.Bin_Avg_SD_Cnt[i, j].Count -
                            (float)Math.Pow(MCP_Bins.Bin_Avg_SD_Cnt[i, j].Avg_WS_Ratio, 2);
                    }

                }

            // Estimate time series data at target site

            int Ref_count = Ref_Data.Length;
            Array.Resize(ref MCP_Bins.LT_WS_Est, Ref_count);

            for (int i = 0; i < Ref_count; i++)
            {
                int WS_ind = Convert.ToInt32(Math.Round(Ref_Data[i].This_WS, 0));
                int WD_ind = Convert.ToInt32(Ref_Data[i].This_WD / (360 / Num_WD));

                if (WD_ind == Num_WD) WD_ind = Num_WD - 1;

                MCP_Bins.LT_WS_Est[i].This_Date = Ref_Data[i].This_Date;
                if (MCP_Bins.Bin_Avg_SD_Cnt[WS_ind, WD_ind].Avg_WS_Ratio > 0)
                    MCP_Bins.LT_WS_Est[i].This_WS = Ref_Data[i].This_WS * MCP_Bins.Bin_Avg_SD_Cnt[WS_ind, WD_ind].Avg_WS_Ratio;
                else
                {
                    // there was no data for this bin so find the two closest ratios and use average of two
                    float Avg_Ratio = 0;
                    int Avg_Ratio_count = 0;
                    int Minus_Ind = WS_ind;
                    int Plus_Ind = WS_ind;
                    int count_while = 0;

                    while (Avg_Ratio_count < 2 && (Minus_Ind != 0 || Plus_Ind != Num_WS))
                    {
                        if (Minus_Ind > 0) Minus_Ind--;
                        if (Plus_Ind < (Num_WS - 1)) Plus_Ind++;

                        if (MCP_Bins.Bin_Avg_SD_Cnt[Minus_Ind, WD_ind].Avg_WS_Ratio > 0)
                        {
                            Avg_Ratio = Avg_Ratio + MCP_Bins.Bin_Avg_SD_Cnt[Minus_Ind, WD_ind].Avg_WS_Ratio;
                            Avg_Ratio_count++;
                        }

                        if (MCP_Bins.Bin_Avg_SD_Cnt[Plus_Ind, WD_ind].Avg_WS_Ratio > 0)
                        {
                            Avg_Ratio = Avg_Ratio + MCP_Bins.Bin_Avg_SD_Cnt[Plus_Ind, WD_ind].Avg_WS_Ratio;
                            Avg_Ratio_count++;
                        }
                        count_while++;
                        if (count_while > 30)
                        {
                            break;
                        }
                    }

                    if (Avg_Ratio_count > 0) Avg_Ratio = Avg_Ratio / Avg_Ratio_count;
                    MCP_Bins.LT_WS_Est[i].This_WS = Ref_Data[i].This_WS * Avg_Ratio;
                }
                MCP_Bins.LT_WS_Est[i].This_WD = Ref_Data[i].This_WD;
            }

            Update_plot();
            Update_Text_boxes();
            Update_Bin_List();

        }
          
        public float Calc_Ortho_Slope(float var_x, float var_y, float covar_xy)
        {
            // Calculates and returns slope of orthogonal regression
            double dbl_slope = 0;
            float slope = 0;

            dbl_slope = (var_y - var_x + Math.Pow((Math.Pow((var_y - var_x), 2) + 4 * Math.Pow(covar_xy, 2)), 0.5)) / (2 * covar_xy);
            slope = Convert.ToSingle(dbl_slope);

            return slope;
        }

        public float Calc_Varrat_Slope(float var_x, float var_y)
        {
            // Calculates and returns slope of Variance Ratio
            double dbl_slope = 0;
            float slope = 0;

            if (var_x > 0)
            {
                dbl_slope = Math.Pow(var_y, 0.5) / Math.Pow(var_x, 0.5);
                slope = Convert.ToSingle(dbl_slope);
            }
            else
            {
                slope = 0;
            }

            return slope;
        }

        public void Update_Text_boxes()
        {
            int WD_ind = Get_WD_ind();
            int Num_WD = Get_Num_WD();
            float Avg_Ref = 0;
            float Avg_Targ = 0;

            // Update Num. Yrs text boxes 
            if (Got_Ref)
            {
                double num_yrs = Ref_Data.Length / 365 / 24;
                txtNumYrsRef.Text = Convert.ToString(Math.Round(num_yrs, 2));
            }
            else
                txtNumYrsRef.Text = "";

            if (Got_Targ)
            {
                int This_length = Target_Data.Length;
                double num_yrs = This_length / 8760.0;
                txtNumYrsTarg.Text = Convert.ToString(Math.Round(num_yrs, 2));
            }
            else
                txtNumYrsTarg.Text = "";

            if (Got_Conc)
            {
                int This_length = Conc_Data.Length;
                double num_yrs = Conc_Data.Length / 8760.0;
                txtNumYrsConc.Text = Convert.ToString(Math.Round(num_yrs, 2));
            }
            else
                txtNumYrsConc.Text = "";

            // Update WS and WS Ratio text boxes
            float Min_WD;
            float Max_WD;

            if (WD_ind == Num_WD)
            {
                Min_WD = 0;
                Max_WD = 360;
            }
            else
            {
                Min_WD = WD_ind * 360 / Num_WD - 360 / Num_WD / 2;
                if (Min_WD < 0)
                    Min_WD = Min_WD + 360;
                Max_WD = WD_ind * 360 / Num_WD + 360 / Num_WD / 2;
                if (Max_WD > 360)
                    Max_WD = Max_WD - 360;
            }

            Stats Stat = new Stats();
            if (Got_Ref)
            {
                Avg_Ref = Stat.Calc_Avg_WS(Ref_Data, 0, 30, Ref_Start, Ref_End, Min_WD, Max_WD);
                txtRef_LT_WS.Text = Convert.ToString(Math.Round(Avg_Ref, 2));
            }
            else
                txtRef_LT_WS.Text = "";

            if (Got_Conc)
            {
                float[] This_Conc = Get_Conc_Avgs_Count(Min_WD, Max_WD);
                Avg_Targ = This_Conc[0];
                Avg_Ref = This_Conc[1];
                float Avg_Ratio = Avg_Targ / Avg_Ref;

                txtRefAvgWS.Text = Convert.ToString(Math.Round(Avg_Ref, 2));
                txtTargAvgWS.Text = Convert.ToString(Math.Round(Avg_Targ, 2));
                txtAvgRatio.Text = Convert.ToString(Math.Round(Avg_Ratio, 2));
                txtDataCount.Text = Convert.ToString(This_Conc[2]);
            }
            else
            {
                txtRefAvgWS.Text = "";
                txtTargAvgWS.Text = "";
                txtAvgRatio.Text = "";
                txtDataCount.Text = "";
            }

            if (MCP_Ortho.LT_WS_Est != null)
            {
                float Slope = MCP_Ortho.Slope[WD_ind];
                float Intercept = MCP_Ortho.Intercept[WD_ind];
                float Rsq = MCP_Ortho.R_sq[WD_ind];
                txtOSlope.Text = Convert.ToString(Math.Round(Slope, 3));
                txtOIntercept.Text = Convert.ToString(Math.Round(Intercept, 3));
                txtORsq.Text = Convert.ToString(Math.Round(Rsq, 3));
            }
            else
            {
                txtOSlope.Text = "";
                txtOIntercept.Text = "";
                txtORsq.Text = "";
            }

            if (MCP_Varrat.LT_WS_Est != null)
            {
                float Slope = MCP_Varrat.Slope[WD_ind];
                float Intercept = MCP_Varrat.Intercept[WD_ind];
                float Rsq = MCP_Varrat.R_sq[WD_ind];
                txtVSlope.Text = Convert.ToString(Math.Round(Slope, 3));
                txtVIntercept.Text = Convert.ToString(Math.Round(Intercept, 3));
                txtVRsq.Text = Convert.ToString(Math.Round(Rsq, 3));
            }
            else
            {
                txtVSlope.Text = "";
                txtVIntercept.Text = "";
                txtVRsq.Text = "";
            }

            if (MCP_Ortho.LT_WS_Est != null && (Get_MCP_Method() == "Orth. Regression"))
            {
                Avg_Ref = Stat.Calc_Avg_WS(Ref_Data, 0, 30, Ref_Start, Ref_End, Min_WD, Max_WD);
                float Avg_Target_LT = Stat.Calc_Avg_WS(MCP_Ortho.LT_WS_Est, 0, 30, Ref_Start, Ref_End, Min_WD, Max_WD);
                float Avg_Ratio = Avg_Target_LT / Avg_Ref;
                txtTarg_LT_WS.Text = Convert.ToString(Math.Round(Avg_Target_LT, 2));
                txtLTratio.Text = Convert.ToString(Math.Round(Avg_Ratio, 2));
            }
            else if (MCP_Varrat.LT_WS_Est != null && (Get_MCP_Method() == "Variance Ratio"))
            {
                Avg_Ref = Stat.Calc_Avg_WS(Ref_Data, 0, 30, Ref_Start, Ref_End, Min_WD, Max_WD);
                float Avg_Target_LT = Stat.Calc_Avg_WS(MCP_Varrat.LT_WS_Est, 0, 30, Ref_Start, Ref_End, Min_WD, Max_WD);
                float Avg_Ratio = Avg_Target_LT / Avg_Ref;
                txtTarg_LT_WS.Text = Convert.ToString(Math.Round(Avg_Target_LT, 2));
                txtLTratio.Text = Convert.ToString(Math.Round(Avg_Ratio, 2));
            }
            else if (MCP_Bins.LT_WS_Est != null && (Get_MCP_Method() == "Method of Bins"))
            {
                Avg_Ref = Stat.Calc_Avg_WS(Ref_Data, 0, 30, Ref_Start, Ref_End, Min_WD, Max_WD);
                float Avg_Target_LT = Stat.Calc_Avg_WS(MCP_Bins.LT_WS_Est, 0, 30, Ref_Start, Ref_End, Min_WD, Max_WD);
                float Avg_Ratio = Avg_Target_LT / Avg_Ref;
                txtTarg_LT_WS.Text = Convert.ToString(Math.Round(Avg_Target_LT, 2));
                txtLTratio.Text = Convert.ToString(Math.Round(Avg_Ratio, 2));
            }
            else
            {
                txtTarg_LT_WS.Text = "";
                txtLTratio.Text = "";
            }

        }

        public void Update_Dates()
        {
            // updates calendar dates for concurrent period used in MCP and dates used in export

            if (Got_Conc && (date_Corr_Start.Value != Conc_Start))
            {
                date_Corr_Start.Value = Conc_Start;
            }

            if (Got_Conc && (date_Corr_End.Value != Conc_End))
            {
                date_Corr_End.Value = Conc_End;
            }


            if (Got_Ref)
            {
                date_Export_Start.Value = Export_Start;
                date_Export_End.Value = Export_End;
            }
        }

        public void Reset_Export_Dates()
        {
            if (Ref_Data.Length > 0)
            {
                date_Export_Start.Value = Ref_Start;
                date_Export_End.Value = Ref_End;
            }
        }

        public void Set_Conc_Dates_On_Form()
        {
            // Find start and end dates of full concurrent period and updates the form dates

            if (Got_Targ != true || Got_Ref != true)
                return;
            
                
            Target_Start = Target_Data[0].This_Date;
            Target_End = Target_Data[Target_Data.Length - 1].This_Date;

            for (int i = 0; i < Target_Data.Length - 1; i++)
            {
                if (Target_Data[i].This_Date < Target_Start)
                    Target_Start = Target_Data[i].This_Date;

                if (Target_Data[i].This_Date > Target_End)
                    Target_End = Target_Data[i].This_Date;
            }

            if (Target_Start > Ref_Start)
                date_Corr_Start.Value = Target_Start;
            else
                date_Corr_Start.Value = Ref_Start;

            if (Target_End < Ref_End)
                date_Corr_End.Value = Target_End;
            else
                date_Corr_End.Value = Ref_End;
                       
        }

        public void Update_WD_DropDown()
        {
            cboWD_sector.Items.Clear();

            int Num_WD = Get_Num_WD();

            if (Num_WD > 1)
                for (int i = 0; i < Num_WD; i++)
                    cboWD_sector.Items.Add(i * 360 / Num_WD);

            cboWD_sector.Items.Add("All WD");
            cboWD_sector.SelectedIndex = cboWD_sector.Items.Count-1;

        }

        public void Update_Export_buttons()
        {

            // Enables or Disables export buttons based on what analysis has been done
            if ((Get_MCP_Method() == "Orth. Regression" && MCP_Ortho.LT_WS_Est != null) || (Get_MCP_Method() == "Method of Bins" && MCP_Bins.LT_WS_Est != null) || (Get_MCP_Method() == "Variance Ratio" && MCP_Varrat.LT_WS_Est != null))
            {
                btnExportTS.Enabled = true;
                btnExportTAB.Enabled = true;
            }
            else
            {
                btnExportTS.Enabled = false;
                btnExportTAB.Enabled = false;
            }

            if (Get_MCP_Method() == "Method of Bins" && MCP_Bins.LT_WS_Est != null)
                btnExportBinRatios.Enabled = true;
            else
                btnExportBinRatios.Enabled = false;

            if ((Get_MCP_Method() == "Orth. Regression" && Uncert_Ortho.Length > 0) || (Get_MCP_Method() == "Method of Bins" && Uncert_Bins.Length > 0) || (Get_MCP_Method() == "Variance Ratio" && Uncert_Varrat.Length > 0))
                btnExportMultitest.Enabled = true;
            else
                btnExportMultitest.Enabled = false;
                           


        }

        public void Update_plot()
        {
            int WD_ind = Get_WD_ind();
            int Num_WD = Get_Num_WD();

            chtScatter.Series.Clear();

            if (Got_Conc == false) Find_Concurrent_Data(true, Conc_Start, Conc_End);

            if (Got_Conc)
            {

                chtScatter.Series.Add("Concurrent data");
                chtScatter.Series["Concurrent data"].ChartType = SeriesChartType.Point;
                chtScatter.ChartAreas[0].AxisX.Interval = 2.0;
                chtScatter.ChartAreas[0].AxisX.Minimum = 0;
                chtScatter.ChartAreas[0].AxisY.Interval = 2.0;
                chtScatter.ChartAreas[0].AxisY.Minimum = 0;

                float Min_WD = 0;
                float Max_WD = 0;

                if (WD_ind == Num_WD)
                {
                    Min_WD = 0;
                    Max_WD = 360;
                }
                else
                {
                    Min_WD = WD_ind * 360 / Num_WD - 360 / Num_WD / 2;
                    if (Min_WD < 0) Min_WD = Min_WD + 360;
                    Max_WD = WD_ind * 360 / Num_WD + 360 / Num_WD / 2;
                    if (Max_WD > 360) Max_WD = Max_WD - 360;
                }

                float[] This_Ref_WS = Get_Conc_WS_Array("Ref", Min_WD, Max_WD);
                float[] This_Targ_WS = Get_Conc_WS_Array("Target", Min_WD, Max_WD);

                if (This_Ref_WS != null)
                {
                    for (int i = 0; i < This_Ref_WS.Length; i++)
                        chtScatter.Series["Concurrent data"].Points.AddXY(This_Ref_WS[i], This_Targ_WS[i]);
                }

                if ((MCP_Ortho.Slope != null) && (Get_MCP_Method() == "Orth. Regression"))
                {

                    chtScatter.Series.Add("Ortho. Reg.");
                    chtScatter.Series["Ortho. Reg."].ChartType = SeriesChartType.Line;

                    int Max_WS = 0;

                    for (int i = 0; i < This_Ref_WS.Length; i++)
                        if (This_Ref_WS[i] > Max_WS) Max_WS = Convert.ToInt32(This_Ref_WS[i]);

                    float[] Ortho_Y = null;
                    float[] Ortho_X = null;

                    Array.Resize(ref Ortho_Y, Max_WS + 5);
                    Array.Resize(ref Ortho_X, Max_WS + 5);

                    for (int i = 0; i < Max_WS + 5; i++)
                    {
                        Ortho_X[i] = i;
                        Ortho_Y[i] = MCP_Ortho.Slope[WD_ind] * i + MCP_Ortho.Intercept[WD_ind];
                        chtScatter.Series["Ortho. Reg."].Points.AddXY(Ortho_X[i], Ortho_Y[i]);
                    }
                }

                else if ((MCP_Varrat.Slope != null) && (Get_MCP_Method() == "Variance Ratio"))
                {

                    chtScatter.Series.Add("Variance Ratio");
                    chtScatter.Series["Variance Ratio"].ChartType = SeriesChartType.Line;

                    int Max_WS = 0;

                    for (int i = 0; i < This_Ref_WS.Length; i++)
                        if (This_Ref_WS[i] > Max_WS) Max_WS = Convert.ToInt32(This_Ref_WS[i]);

                    float[] Varrat_Y = null;
                    float[] Varrat_X = null;

                    Array.Resize(ref Varrat_Y, Max_WS + 5);
                    Array.Resize(ref Varrat_X, Max_WS + 5);

                    for (int i = 0; i < Max_WS + 5; i++)
                    {
                        Varrat_X[i] = i;
                        Varrat_Y[i] = MCP_Varrat.Slope[WD_ind] * i + MCP_Varrat.Intercept[WD_ind];
                        chtScatter.Series["Variance Ratio"].Points.AddXY(Varrat_X[i], Varrat_Y[i]);
                    }
                }
                else if ((MCP_Bins.Bin_Avg_SD_Cnt != null) && (Get_MCP_Method() == "Method of Bins"))
                {
                    chtScatter.Series.Add("Method of Bins");
                    chtScatter.Series["Method of Bins"].ChartType = SeriesChartType.Point;
                    chtScatter.Series["Method of Bins"].MarkerColor = Color.Red;
                    chtScatter.Series["Method of Bins"].YAxisType = AxisType.Secondary;

                    for (int i = 0; i < MCP_Bins.Bin_Avg_SD_Cnt.GetUpperBound(0); i++)
                    {
                        if (MCP_Bins.Bin_Avg_SD_Cnt[i, WD_ind].Avg_WS_Ratio > 0)
                            chtScatter.Series["Method of Bins"].Points.AddXY(i * Get_WS_width(), MCP_Bins.Bin_Avg_SD_Cnt[i, WD_ind].Avg_WS_Ratio);
                    }

                }

                Update_Text_boxes();
            }

        }

        public void Update_Bin_List()
        {
            // Update list with mean and SD WS ratios
            lstBins.Items.Clear();

            if (MCP_Bins.Bin_Avg_SD_Cnt != null)
            {
                ListViewItem objlist = new ListViewItem();
                int Num_WS = MCP_Bins.Bin_Avg_SD_Cnt.GetUpperBound(0);
                int Num_WD = Get_Num_WD();
                int WD_ind = Get_WD_ind();
                float WS_Width = Get_WS_width();

                for (int i = 0; i < Num_WS; i++)
                {
                    float This_WS = i * WS_Width;
                    if (MCP_Bins.Bin_Avg_SD_Cnt[i, WD_ind].Avg_WS_Ratio > 0)
                    {
                        objlist = lstBins.Items.Add(Convert.ToString(This_WS));
                        objlist.SubItems.Add(Convert.ToString(Math.Round(MCP_Bins.Bin_Avg_SD_Cnt[i, WD_ind].Avg_WS_Ratio, 2)));
                        objlist.SubItems.Add(Convert.ToString(Math.Round(MCP_Bins.Bin_Avg_SD_Cnt[i, WD_ind].SD_WS_Ratio, 2)));
                        objlist.SubItems.Add(Convert.ToString(Math.Round(MCP_Bins.Bin_Avg_SD_Cnt[i, WD_ind].Count, 2)));
                    }
                }
            }
        }

        public void Update_Uncert_List()
        {
            // Update list with mean and SD WS ratios
            lstUncert.Items.Clear();
            ListViewItem objlist = new ListViewItem();

            // Get Active MCP type
            string active_method = Get_MCP_Method();

            if ((active_method == "Orth. Regression") && (Uncert_Ortho.Length > 0))
            {
                for (int u = 0; u < Uncert_Ortho.Length; u++)
                {
                    // Assign LT Avg series = Avg of Uncert obj
                    if ((Uncert_Ortho[u].avg != 0) && (Uncert_Ortho[u].std_dev != 0))
                    {
                        objlist = lstUncert.Items.Add(Convert.ToString(Uncert_Ortho[u].WSize));
                        objlist.SubItems.Add(Convert.ToString(Math.Round(Uncert_Ortho[u].avg, 2)));
                        objlist.SubItems.Add(Convert.ToString(Math.Round(Uncert_Ortho[u].std_dev, 2)));
                    }
                }
            }
            if ((active_method == "Method of Bins") && (Uncert_Bins.Length > 0))
            {
                for (int u = 0; u < Uncert_Bins.Length; u++)
                {
                    // Assign LT Avg series = Avg of Uncert obj
                    if ((Uncert_Bins[u].avg != 0) && (Uncert_Bins[u].std_dev != 0))
                    {
                        objlist = lstUncert.Items.Add(Convert.ToString(Uncert_Bins[u].WSize));
                        objlist.SubItems.Add(Convert.ToString(Math.Round(Uncert_Bins[u].avg, 2)));
                        objlist.SubItems.Add(Convert.ToString(Math.Round(Uncert_Bins[u].std_dev, 2)));
                    }
                }
            }
            if ((active_method == "Variance Ratio") && (Uncert_Varrat.Length > 0))
            {
                for (int u = 0; u < Uncert_Varrat.Length; u++)
                {
                    // Assign LT Avg series = Avg of Uncert obj
                    if ((Uncert_Varrat[u].avg != 0) && (Uncert_Varrat[u].std_dev != 0))
                    {
                        objlist = lstUncert.Items.Add(Convert.ToString(Uncert_Varrat[u].WSize));
                        objlist.SubItems.Add(Convert.ToString(Math.Round(Uncert_Varrat[u].avg, 2)));
                        objlist.SubItems.Add(Convert.ToString(Math.Round(Uncert_Varrat[u].std_dev, 2)));
                    }
                }
            }
        }

        private void cboNumWD_SelectedIndexChanged(object sender, EventArgs e)
        {
            // update WD sector drop-down
            if ((MCP_Ortho.Slope == null) && (MCP_Varrat.Slope == null) && (MCP_Bins.Bin_Avg_SD_Cnt == null))
            {
                Num_WD_Sectors = Convert.ToInt16(cboNumWD.SelectedItem.ToString());
                Update_WD_DropDown();
            }
            else if ((Is_Newly_Opened_File == false) && ((MCP_Ortho.Slope != null) || (MCP_Varrat.Slope != null) || (MCP_Bins.Bin_Avg_SD_Cnt != null) || (Uncert_Ortho.Length > 0) || (Uncert_Varrat.Length > 0) || (Uncert_Bins.Length > 0)))
            {
                bool show_msg = true;
            
             /*   if (MCP_Ortho.Slope != null)
                    if (MCP_Ortho.Slope.Length - 1 != Get_Num_WD())
                        show_msg = true;

                if (MCP_Varrat.Slope != null)
                    if (MCP_Varrat.Slope.Length - 1 != Get_Num_WD())
                        show_msg = true;

                if (MCP_Bins.Bin_Avg_SD_Cnt != null)
                    if (MCP_Bins.Bin_Avg_SD_Cnt.GetUpperBound(1) - 1 != Get_Num_WD())
                        show_msg = true;
                        */

                if (show_msg == true)
                {
                    string message = "Changing the number of WD bins will reset the MCP. Do you want to continue?";
                    
                    DialogResult result = MessageBox.Show(message, "", MessageBoxButtons.YesNo);

                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        

                        Conc_Data = new Concurrent_data[0];
                        MCP_Ortho.Clear();
                        MCP_Bins.Clear();
                        MCP_Varrat.Clear();

                        Uncert_Ortho = new MCP_Uncert[0];
                        Uncert_Bins = new MCP_Uncert[0];
                        Uncert_Varrat = new MCP_Uncert[0];
                                                
                        Conc_Start = date_Corr_Start.Value;
                        Got_Conc = false;
                        btnRunMCP.Enabled = true;
                        btnMCP_Uncert.Enabled = true;

                        Num_WD_Sectors = Convert.ToInt16(cboNumWD.SelectedItem.ToString());
                        Update_WD_DropDown();
                        Update_Text_boxes();
                        Update_Bin_List();
                        Update_Uncert_List();
                        Update_plot();
                        Update_Uncert_plot();
                        Update_Export_buttons();
                    }
                    else
                    {
                        cboNumWD.Text = Num_WD_Sectors.ToString();
                        
                    }
                }
            }
        }

        private void btnClearRef_Click(object sender, EventArgs e)
        {
            string message = "Are you sure that you want to clear the reference data?";
            string caption = "Clear Reference Data";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            // Displays the MessageBox.

            result = MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                New_MCP(true, false);
            }

            Changes_Made();

        }

        private void btnClearTarget_Click(object sender, EventArgs e)
        {
            if (Target_Data.Length > 0)
            {
                string message = "Are you sure that you want to clear the target data?";
                string caption = "Clear Target Data";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    New_MCP(false, true);
                }
            }

            Changes_Made();

        }

        private void btnRunMCP_Click(object sender, EventArgs e)
        {
            // Read MCP method
            string MCP_method = Get_MCP_Method();

            Do_MCP(Conc_Start, Conc_End, true, MCP_method);
            btnRunMCP.Enabled = false;
            Changes_Made();
                        
        }

        private void cboWD_sector_SelectedIndexChanged(object sender, EventArgs e)
        {
            Update_plot();
            Update_Uncert_plot();
            Update_Text_boxes();
            Update_Bin_List();
            Update_Uncert_List();
        }

        private void date_Corr_Start_ValueChanged(object sender, EventArgs e)
        {
            DialogResult result = System.Windows.Forms.DialogResult.Yes;

            if (((MCP_Ortho.Slope != null) || (MCP_Bins.Bin_Avg_SD_Cnt != null) || (MCP_Varrat.Slope != null)) && Is_Newly_Opened_File == false)
            {
                string message = "Changing the start of the correlation will reset the MCP. Do you want to continue?";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                result = MessageBox.Show(message, "", buttons);
            }

            if (result == System.Windows.Forms.DialogResult.Yes && Is_Newly_Opened_File == false)
            {
                Conc_Data = new Concurrent_data[0];
                MCP_Ortho.Clear();
                MCP_Bins.Clear();
                MCP_Varrat.Clear();
                Conc_Start = date_Corr_Start.Value;
                Conc_End = date_Corr_End.Value;
                               

                if (Uncert_Ortho.Length > 0)
                {
                    for (int u = 0; u < Uncert_Ortho.Length; u++)
                    {
                        Uncert_Ortho[u].Clear();
                    }
                }
                if (Uncert_Bins.Length > 0)
                {
                    for (int u = 0; u < Uncert_Varrat.Length; u++)
                    {
                        Uncert_Bins[u].Clear();
                    }
                }
                if (Uncert_Varrat.Length > 0)
                {
                    for (int u = 0; u < Uncert_Varrat.Length; u++)
                    {
                        Uncert_Varrat[u].Clear();
                    }
                }
                Got_Conc = false;
                btnRunMCP.Enabled = true;
                btnMCP_Uncert.Enabled = true;
                Update_Text_boxes();
                Update_Bin_List();
                Update_Uncert_List();
                chtScatter.Series.Clear();
                chtUncert.Series.Clear();
                
            }
            
            
        }

        private void date_Corr_End_ValueChanged(object sender, EventArgs e)
        {
            DialogResult result = System.Windows.Forms.DialogResult.Yes;

            if ((Is_Newly_Opened_File == false) && ((MCP_Ortho.Slope != null) || (MCP_Bins.Bin_Avg_SD_Cnt != null) || (MCP_Varrat.Slope != null)))
            {
                string message = "Changing the start of the correlation will reset the MCP. Do you want to continue?";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                result = MessageBox.Show(message, "", buttons);
            }

            if (result == System.Windows.Forms.DialogResult.Yes && Is_Newly_Opened_File == false)
            {
                Conc_Data = new Concurrent_data[0];
                MCP_Ortho.Clear();
                MCP_Bins.Clear();
                MCP_Varrat.Clear();
                Conc_Start = date_Corr_Start.Value;
                Conc_End = date_Corr_End.Value;
                                
                if (Uncert_Ortho.Length > 0)
                {
                    for(int u = 0; u < Uncert_Ortho.Length; u++)
                    {
                        Uncert_Ortho[u].Clear();
                    }
                }
                if (Uncert_Bins.Length > 0)
                {
                    for (int u = 0; u < Uncert_Varrat.Length; u++)
                    {
                        Uncert_Bins[u].Clear();
                    }
                }
                if (Uncert_Varrat.Length > 0)
                {
                    for (int u = 0; u < Uncert_Varrat.Length; u++)
                    {
                        Uncert_Varrat[u].Clear();
                    }
                }
                Got_Conc = false;
                btnRunMCP.Enabled = true;
                btnMCP_Uncert.Enabled = true;
                Update_Text_boxes();
                Update_Bin_List();
                Update_Uncert_List();
                chtScatter.Series.Clear();
                chtUncert.Series.Clear();
            }
        }


        private void date_Export_Start_ValueChanged(object sender, EventArgs e)
        {
            if (date_Export_Start.Value > Ref_End && Is_Newly_Opened_File == false)
            {
                MessageBox.Show("Export date cannot be later than the end of the reference site data.");
                date_Export_Start.Value = Ref_Start;
            }
            else
                Export_Start = date_Export_Start.Value;
        }

        private void MCP_tool_Load(object sender, EventArgs e)
        {
            //
        }

        private void cboMCP_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            Update_Run_Buttons();          
            Update_Bin_List();
            Update_plot();
            Update_Text_boxes();
            Update_Uncert_List();
            Update_Uncert_plot();
            Update_Export_buttons();

        }

        public void Update_Run_Buttons()
        {
            string MCP_type = Get_MCP_Method();

            if (((MCP_type == "Orth. Regression") && (MCP_Ortho.Slope != null)) || ((MCP_type == "Method of Bins") && (MCP_Bins.Bin_Avg_SD_Cnt != null)) || ((MCP_type == "Variance Ratio") && (MCP_Varrat.Slope != null)))
                btnRunMCP.Enabled = false;
            else
                btnRunMCP.Enabled = true;

            if ((MCP_type == "Orth. Regression" && Uncert_Ortho.Length > 0) || (MCP_type == "Method of Bins" && Uncert_Bins.Length > 0) || (MCP_type == "Variance Ratio" && Uncert_Varrat.Length > 0))
                btnMCP_Uncert.Enabled = false;
            else
                btnMCP_Uncert.Enabled = true;
                
                     
             
        }

        private void btnExportTS_Click(object sender, EventArgs e)
        {
            string filename = "";
            // Export estimated time series WS and WD at target site

            // Check that the export start/end are within interval of estimated data
            if (Export_Start > Ref_End)
            {
                MessageBox.Show("The selected export start date is after the end of the reference data period.");
                return;
            }


            if (sfdSaveTimeSeries.ShowDialog() == DialogResult.OK)
            {
                filename = sfdSaveTimeSeries.FileName;

                StreamWriter file = new StreamWriter(filename);
                file.WriteLine("MCP WS & WD Estimates");
                file.WriteLine(DateTime.Today);
                file.WriteLine(Get_MCP_Method());
                file.WriteLine("Data binned into " + Get_Num_WD() + " wind direction sectors");
                file.WriteLine();

                file.WriteLine("Date, WS Est [m/s], WD Est [deg]");

                if (Get_MCP_Method() == "Method of Bins" && MCP_Bins.LT_WS_Est != null)
                {
                    foreach (Site_data LT_WS_WD in MCP_Bins.LT_WS_Est)
                    {
                        if (LT_WS_WD.This_Date >= Export_Start && LT_WS_WD.This_Date <= Export_End)
                        {
                            file.Write(LT_WS_WD.This_Date);
                            file.Write(",");
                            file.Write(Math.Round(LT_WS_WD.This_WS,3));
                            file.Write(",");
                            file.Write(Math.Round(LT_WS_WD.This_WD,2));
                            file.WriteLine();
                        }
                    }

                }
                else if (Get_MCP_Method() == "Orth. Regression" && MCP_Ortho.LT_WS_Est != null)
                {
                    foreach (Site_data LT_WS_WD in MCP_Ortho.LT_WS_Est)
                    {
                        if (LT_WS_WD.This_Date >= Export_Start && LT_WS_WD.This_Date <= Export_End)
                        {
                            file.Write(LT_WS_WD.This_Date);
                            file.Write(",");
                            file.Write(Math.Round(LT_WS_WD.This_WS, 3));
                            file.Write(",");
                            file.Write(Math.Round(LT_WS_WD.This_WD, 2));
                            file.WriteLine();
                        }
                    }
                }

                else if (Get_MCP_Method() == "Variance Ratio" && MCP_Varrat.LT_WS_Est != null)
                {
                    foreach (Site_data LT_WS_WD in MCP_Varrat.LT_WS_Est)
                    {
                        if (LT_WS_WD.This_Date >= Export_Start && LT_WS_WD.This_Date <= Export_End)
                        {
                            file.Write(LT_WS_WD.This_Date);
                            file.Write(",");
                            file.Write(Math.Round(LT_WS_WD.This_WS, 3));
                            file.Write(",");
                            file.Write(Math.Round(LT_WS_WD.This_WD, 2));
                            file.WriteLine();
                        }
                    }
                }

                file.Close();

            }
        }

        private void btnExportBinRatios_Click(object sender, EventArgs e)
        {
            // exports average, SD and count of WS ratios in each WS/WD bin

            string filename = "";
            if (sfdSaveTimeSeries.ShowDialog() == DialogResult.OK)
                filename = sfdSaveTimeSeries.FileName;
            else
                return;

            StreamWriter file = new StreamWriter(filename);
            file.WriteLine("Avg, SD & Count of WS Ratios (Target/Reference) from Method of Bins");
            file.WriteLine(DateTime.Today.ToShortDateString());
            file.WriteLine();

            file.WriteLine("Average WS Ratios by WS & WD");
            file.WriteLine();
            file.Write("WS [m/s],");
            for (int i = 0; i <= MCP_Bins.Bin_Avg_SD_Cnt.GetUpperBound(0); i++)
            {
                file.Write(i * Get_WS_width());
                file.Write(",");
            }

            for (int j = 0; j <= MCP_Bins.Bin_Avg_SD_Cnt.GetUpperBound(1); j++)
            {
                if (j != MCP_Bins.Bin_Avg_SD_Cnt.GetUpperBound(1))
                {
                    file.Write(j * 360 / Get_Num_WD());
                    file.Write(",");
                }
                else
                    file.Write("All WD,");

                for (int i = 0; i <= MCP_Bins.Bin_Avg_SD_Cnt.GetUpperBound(0); i++)
                    if (MCP_Bins.Bin_Avg_SD_Cnt[i, j].Avg_WS_Ratio > 0)
                    {
                        file.Write(Math.Round(MCP_Bins.Bin_Avg_SD_Cnt[i, j].Avg_WS_Ratio, 3));
                        file.Write(",");
                    }
                    else
                        file.Write(" ,");
                file.WriteLine();
            }

            file.WriteLine();
            file.WriteLine("Standard Deviation of WS Ratios by WS & WD");
            file.WriteLine();
            file.Write("WS [m/s],");
            for (int i = 0; i <= MCP_Bins.Bin_Avg_SD_Cnt.GetUpperBound(0); i++)
            {
                file.Write(i * Get_WS_width());
                file.Write(",");
            }

            for (int j = 0; j <= MCP_Bins.Bin_Avg_SD_Cnt.GetUpperBound(1); j++)
            {
                file.Write(j * 360 / Get_Num_WD());
                file.Write(",");
                for (int i = 0; i <= MCP_Bins.Bin_Avg_SD_Cnt.GetUpperBound(0); i++)
                    if (MCP_Bins.Bin_Avg_SD_Cnt[i, j].Avg_WS_Ratio > 0)
                    {
                        file.Write(Math.Round(MCP_Bins.Bin_Avg_SD_Cnt[i, j].SD_WS_Ratio, 3));
                        file.Write(",");
                    }
                    else
                        file.Write(" ,");
                file.WriteLine();
            }

            file.WriteLine();
            file.WriteLine("Count of WS Ratios by WS & WD");
            file.WriteLine();
            file.Write("WS [m/s],");
            for (int i = 0; i <= MCP_Bins.Bin_Avg_SD_Cnt.GetUpperBound(0); i++)
            {
                file.Write(i * Get_WS_width());
                file.Write(",");
            }

            for (int j = 0; j <= MCP_Bins.Bin_Avg_SD_Cnt.GetUpperBound(1); j++)
            {
                file.Write(j * 360 / Get_Num_WD());
                file.Write(",");
                for (int i = 0; i <= MCP_Bins.Bin_Avg_SD_Cnt.GetUpperBound(0); i++)
                    if (MCP_Bins.Bin_Avg_SD_Cnt[i, j].Avg_WS_Ratio > 0)
                    {
                        file.Write(MCP_Bins.Bin_Avg_SD_Cnt[i, j].Count);
                        file.Write(",");
                    }
                    else
                        file.Write(" ,");
                file.WriteLine();
            }

            file.Close();

        }

        private void btnExportTAB_Click(object sender, EventArgs e)
        {
            // Export estimated time series data as a TAB file (i.e. joint WS/WD distribution)

            string filename = "";
            if (sfdSaveTAB.ShowDialog() == DialogResult.OK)
                filename = sfdSaveTAB.FileName;

            if (filename != "")
            {
                StreamWriter file = new StreamWriter(filename);
                file.WriteLine("MCP Estimate at Target Site using " + Get_MCP_Method() + ", " + Export_Start + " to " + Export_End + " binned into " + Get_Num_WD() + " WD sectors");

                file.WriteLine("0 0 0");
                file.Write("16");
                file.Write(" ");
                file.Write(Get_WS_width());
                file.WriteLine(" 0");

                int Num_WD = 16; // Using 16 WD sectors for TAB file creation
                int Num_WS = 31;

                float[] Wind_Rose = new float[Num_WD];
                float[,] WSWD_Dist = new float[Num_WS, Num_WD];

                DateTime This_TS = DateTime.Today;
                float This_WS = 0;
                float This_WD = 0;

                string MCP_type = Get_MCP_Method();

                int Est_data_ind = 0;

                if (MCP_type == "Orth. Regression" && MCP_Ortho.LT_WS_Est != null)
                {
                    for (int i = 0; i < MCP_Ortho.LT_WS_Est.Length; i++)
                    {
                        if (MCP_Ortho.LT_WS_Est[i].This_Date < Export_Start)
                            Est_data_ind++;
                        else
                            break;
                    }

                    This_TS = MCP_Ortho.LT_WS_Est[Est_data_ind].This_Date;
                    This_WS = MCP_Ortho.LT_WS_Est[Est_data_ind].This_WS;
                    This_WD = MCP_Ortho.LT_WS_Est[Est_data_ind].This_WD;
                    Est_data_ind++;

                }
                else if (MCP_type == "Method of Bins" && MCP_Bins.LT_WS_Est != null)
                {
                    for (int i = 0; i < MCP_Bins.LT_WS_Est.Length; i++)
                    {
                        if (MCP_Bins.LT_WS_Est[i].This_Date < Export_Start)
                            Est_data_ind++;
                        else
                            break;
                    }
                    This_TS = MCP_Bins.LT_WS_Est[Est_data_ind].This_Date;
                    This_WS = MCP_Bins.LT_WS_Est[Est_data_ind].This_WS;
                    This_WD = MCP_Bins.LT_WS_Est[Est_data_ind].This_WD;
                    Est_data_ind++;
                }

                else if (MCP_type == "Variance Ratio" && MCP_Varrat.LT_WS_Est != null)
                {
                    for (int i = 0; i < MCP_Varrat.LT_WS_Est.Length; i++)
                    {
                        if (MCP_Varrat.LT_WS_Est[i].This_Date < Export_Start)
                            Est_data_ind++;
                        else
                            break;
                    }

                    This_TS = MCP_Varrat.LT_WS_Est[Est_data_ind].This_Date;
                    This_WS = MCP_Varrat.LT_WS_Est[Est_data_ind].This_WS;
                    This_WD = MCP_Varrat.LT_WS_Est[Est_data_ind].This_WD;
                    Est_data_ind++;

                }

                while (This_TS < Export_End)
                {
                    if (This_WS > 0 && This_WD > 0)
                    {
                        int WS_ind = (int)Math.Round(This_WS, 0);
                        if (WS_ind >= Num_WS) WS_ind = Num_WS - 1;

                        int WD_ind = (int)Math.Round(This_WD / (360 / Num_WD), 0);
                        if (WD_ind == Num_WD) WD_ind = 0;

                        Wind_Rose[WD_ind]++;
                        WSWD_Dist[WS_ind, WD_ind]++;
                    }

                    if (MCP_type == "Orth. Regression" && MCP_Ortho.LT_WS_Est != null)
                    {
                        This_TS = MCP_Ortho.LT_WS_Est[Est_data_ind].This_Date;
                        This_WS = MCP_Ortho.LT_WS_Est[Est_data_ind].This_WS;
                        This_WD = MCP_Ortho.LT_WS_Est[Est_data_ind].This_WD;
                        Est_data_ind++;

                    }
                    else if (MCP_type == "Method of Bins" && MCP_Bins.LT_WS_Est != null)
                    {
                        This_TS = MCP_Bins.LT_WS_Est[Est_data_ind].This_Date;
                        This_WS = MCP_Bins.LT_WS_Est[Est_data_ind].This_WS;
                        This_WD = MCP_Bins.LT_WS_Est[Est_data_ind].This_WD;
                        Est_data_ind++;
                    }
                    else if (MCP_type == "Variance Ratio" && MCP_Varrat.LT_WS_Est != null)
                    {
                        This_TS = MCP_Varrat.LT_WS_Est[Est_data_ind].This_Date;
                        This_WS = MCP_Varrat.LT_WS_Est[Est_data_ind].This_WS;
                        This_WD = MCP_Varrat.LT_WS_Est[Est_data_ind].This_WD;
                        Est_data_ind++;

                    }
                }


                float Sum_WD = 0;
                for (int i = 0; i < Num_WD; i++)
                    Sum_WD = Sum_WD + Wind_Rose[i];

                for (int i = 0; i < Num_WD; i++)
                {
                    Wind_Rose[i] = Wind_Rose[i] / Sum_WD * 100;
                    file.Write(Math.Round(Wind_Rose[i], 2) + "\t");
                }
                file.WriteLine();

                for (int WD_ind = 0; WD_ind < Num_WD; WD_ind++)
                {
                    float Sum_WS = 0;
                    for (int WS_ind = 0; WS_ind < Num_WS; WS_ind++)
                        Sum_WS = Sum_WS + WSWD_Dist[WS_ind, WD_ind];

                    for (int WS_ind = 0; WS_ind < Num_WS; WS_ind++)
                        WSWD_Dist[WS_ind, WD_ind] = WSWD_Dist[WS_ind, WD_ind] / Sum_WS * 1000;

                }

                for (int i = 0; i < Num_WS; i++)
                {
                    file.Write((float)(i + (float)Get_WS_width() / 2) + "\t");
                    for (int j = 0; j < Num_WD; j++)
                        file.Write(Math.Round(WSWD_Dist[i, j], 3) + "\t");
                    file.WriteLine();

                }

                file.Close();
            }
        }

        private void txtWS_bin_width_TextChanged(object sender, EventArgs e)
        {
            DialogResult result = DialogResult.Yes;

            if (Is_Newly_Opened_File == false)
            {
                string message = "Changing the WS bin width will reset the MCP. Do you want to continue?";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                result = MessageBox.Show(message, "", buttons);
            }

            if (Is_Newly_Opened_File == false && result == System.Windows.Forms.DialogResult.Yes)
            {
                Conc_Data = new Concurrent_data[0];
                MCP_Ortho.Clear();
                MCP_Bins.Clear();
                MCP_Varrat.Clear();

                Uncert_Ortho = new MCP_Uncert[0];
                Uncert_Bins = new MCP_Uncert[0];
                Uncert_Varrat = new MCP_Uncert[0];

                try
                {
                    WS_bin_width = Convert.ToSingle(txtWS_bin_width.Text.ToString());
                }
                catch
                {
                    return;
                }
                
                                        
                Conc_Start = date_Corr_Start.Value;
                Got_Conc = false;
                btnRunMCP.Enabled = true;
                Update_Text_boxes();
                Update_Bin_List();
                Update_Uncert_List();
                Update_plot();
                Update_Uncert_plot();
            }
            
        }

        private void btnConvertToHourly_Click(object sender, EventArgs e)
        {
            // Read in 10-minute time series wind speed and WD data and convert to hourly data
            // Prompt user to find reference data file
            string filename = "";
            string line;
            DateTime This_Date;
            DateTime Last_Date = DateTime.Today;
            float This_WS;
            float This_WD;

            float[] WS_Arr = null;
            float[] WD_Arr = null;
            float Avg_WS = 0;
            float Avg_WD = 0;
            int Avg_Count = 0;

            if (ofdRefSite.ShowDialog() == DialogResult.OK)
                filename = ofdRefSite.FileName;

            string[] split_filename = filename.Split('.');
            string hour_filename = split_filename[0] + "_hourly.csv";

            if (filename != "")
            {

                StreamReader file = new StreamReader(filename);
                StreamWriter hour_file = new StreamWriter(hour_filename);

                while ((line = file.ReadLine()) != null)
                {
                    try
                    {
                        Char[] delims = { ',' };
                        String[] substrings = line.Split(delims);
                        if (substrings[1] != "NaN" && substrings[2] != "NaN")
                        {
                            This_Date = Convert.ToDateTime(substrings[0]);
                            This_WS = Convert.ToSingle(substrings[1]);
                            This_WD = Convert.ToSingle(substrings[2]);

                            if (Last_Date == DateTime.Today)
                                Last_Date = This_Date;

                            if (This_Date.Hour == Last_Date.Hour)
                            {
                                Avg_Count++;
                                Array.Resize(ref WS_Arr, Avg_Count);
                                Array.Resize(ref WD_Arr, Avg_Count);

                                WS_Arr[Avg_Count - 1] = This_WS;
                                WD_Arr[Avg_Count - 1] = This_WD;
                                Last_Date = This_Date;
                            }
                            else if (Avg_Count >= 1) // need at least one record per hour
                            {
                                // calculate avg WS
                                for (int i = 0; i < Avg_Count; i++)
                                    Avg_WS = Avg_WS + WS_Arr[i];

                                // first figure out if there is cross-over
                                float max_diff = 0;
                                for (int i = 0; i < Avg_Count - 1; i++)
                                {
                                    float this_diff = Math.Abs(WD_Arr[i + 1] - WD_Arr[i]);
                                    if (this_diff > max_diff)
                                        max_diff = this_diff;
                                }

                                if (max_diff > 270)
                                {
                                    for (int i = 0; i < Avg_Count; i++)
                                        if (WD_Arr[i] > 270) WD_Arr[i] = WD_Arr[i] - 360;
                                }

                                // calculate avg WD
                                for (int i = 0; i < Avg_Count; i++)
                                    Avg_WD = Avg_WD + WD_Arr[i];

                                Avg_WS = Avg_WS / Avg_Count;
                                Avg_WD = Avg_WD / Avg_Count;

                                if (Avg_WD < 0) Avg_WD = Avg_WD + 360;

                                DateTime Hour_Date = Last_Date;
                                int This_Year = Hour_Date.Year;
                                int This_Month = Hour_Date.Month;
                                int This_Day = Hour_Date.Day;
                                int This_New_Hour = Hour_Date.Hour;

                                DateTime New_Hour_date = new DateTime(This_Year, This_Month, This_Day);
                                TimeSpan ts = new TimeSpan(This_New_Hour, 0, 0);
                                New_Hour_date = New_Hour_date.Date + ts;

                                hour_file.Write(New_Hour_date + ",");
                                hour_file.Write(Math.Round(Avg_WS, 3) + ",");
                                hour_file.WriteLine(Math.Round(Avg_WD, 2));

                                Avg_Count = 0;
                                Avg_WS = 0;
                                Avg_WD = 0;
                                WS_Arr = null;
                                WD_Arr = null;

                                Avg_Count++;
                                Array.Resize(ref WS_Arr, Avg_Count);
                                Array.Resize(ref WD_Arr, Avg_Count);

                                WS_Arr[Avg_Count - 1] = This_WS;
                                WD_Arr[Avg_Count - 1] = This_WD;
                                Last_Date = This_Date;
                            }
                            else
                            {
                                Avg_Count = 0;
                                Avg_WS = 0;
                                Avg_WD = 0;
                                WS_Arr = null;
                                WD_Arr = null;

                                Avg_Count++;
                                Array.Resize(ref WS_Arr, Avg_Count);
                                Array.Resize(ref WD_Arr, Avg_Count);

                                WS_Arr[Avg_Count - 1] = This_WS;
                                WD_Arr[Avg_Count - 1] = This_WD;
                                Last_Date = This_Date;
                            }


                        }
                    }
                    catch
                    {

                    }

                }
                file.Close();
                hour_file.Close();

            }


        }

        private void btnConvertMonthly_Click(object sender, EventArgs e)
        {
            // Read in 10-minute time series wind speed and WD data and convert to monthly data
            // Prompt user to find reference data file
            string filename = "";
            string line;
            DateTime This_Date;
            DateTime Last_Date = DateTime.Today;
            float This_WS;
            float This_WD;

            float[] WS_Arr = null;
            float[] WD_Arr = null;
            float Avg_WS = 0;
            float Avg_WD = 0;
            int Avg_Count = 0;

            if (ofdRefSite.ShowDialog() == DialogResult.OK)
                filename = ofdRefSite.FileName;

            string[] split_filename = filename.Split('.');
            string month_filename = split_filename[0] + "_monthly.csv";

            if (filename != "")
            {

                StreamReader file = new StreamReader(filename);
                StreamWriter month_file = new StreamWriter(month_filename);

                while ((line = file.ReadLine()) != null)
                {
                    try
                    {
                        Char[] delims = { ',' };
                        String[] substrings = line.Split(delims);
                        if (substrings[1] != "NaN" && substrings[2] != "NaN")
                        {
                            This_Date = Convert.ToDateTime(substrings[0]);
                            This_WS = Convert.ToSingle(substrings[1]);
                            This_WD = Convert.ToSingle(substrings[2]);

                            if (Last_Date == DateTime.Today)
                                Last_Date = This_Date;

                            if (This_Date.Month == Last_Date.Month)
                            {
                                Avg_Count++;
                                Array.Resize(ref WS_Arr, Avg_Count);
                                Array.Resize(ref WD_Arr, Avg_Count);

                                WS_Arr[Avg_Count - 1] = This_WS;
                                WD_Arr[Avg_Count - 1] = This_WD;
                                Last_Date = This_Date;
                            }
                            else if (Avg_Count >= 15)
                            {
                                // calculate avg WS
                                for (int i = 0; i < Avg_Count; i++)
                                    Avg_WS = Avg_WS + WS_Arr[i];

                                // first figure out if there is cross-over
                                float max_diff = 0;
                                for (int i = 0; i < Avg_Count - 1; i++)
                                {
                                    float this_diff = Math.Abs(WD_Arr[i + 1] - WD_Arr[i]);
                                    if (this_diff > max_diff)
                                        max_diff = this_diff;
                                }

                                if (max_diff > 270)
                                {
                                    for (int i = 0; i < Avg_Count; i++)
                                        if (WD_Arr[i] > 270) WD_Arr[i] = WD_Arr[i] - 360;
                                }

                                // calculate avg WD
                                for (int i = 0; i < Avg_Count; i++)
                                    Avg_WD = Avg_WD + WD_Arr[i];

                                Avg_WS = Avg_WS / Avg_Count;
                                Avg_WD = Avg_WD / Avg_Count;

                                if (Avg_WD < 0) Avg_WD = Avg_WD + 360;

                                DateTime Hour_Date = Last_Date;
                                int This_Year = Hour_Date.Year;
                                int This_Month = Hour_Date.Month;
                                int This_Day = 1;

                                DateTime New_Hour_date = new DateTime(This_Year, This_Month, This_Day);

                                month_file.Write(New_Hour_date + ",");
                                month_file.Write(Math.Round(Avg_WS, 3) + ",");
                                month_file.WriteLine(Math.Round(Avg_WD, 2));

                                Avg_Count = 0;
                                Avg_WS = 0;
                                Avg_WD = 0;
                                WS_Arr = null;
                                WD_Arr = null;

                                Avg_Count++;
                                Array.Resize(ref WS_Arr, Avg_Count);
                                Array.Resize(ref WD_Arr, Avg_Count);

                                WS_Arr[Avg_Count - 1] = This_WS;
                                WD_Arr[Avg_Count - 1] = This_WD;
                                Last_Date = This_Date;
                            }
                            else
                            {
                                Avg_Count = 0;
                                Avg_WS = 0;
                                Avg_WD = 0;
                                WS_Arr = null;
                                WD_Arr = null;

                                Avg_Count++;
                                Array.Resize(ref WS_Arr, Avg_Count);
                                Array.Resize(ref WD_Arr, Avg_Count);

                                WS_Arr[Avg_Count - 1] = This_WS;
                                WD_Arr[Avg_Count - 1] = This_WD;
                                Last_Date = This_Date;
                            }


                        }
                    }
                    catch
                    {

                    }

                }

                if (Avg_Count >= 15)
                {
                    // calculate avg WS
                    for (int i = 0; i < Avg_Count; i++)
                        Avg_WS = Avg_WS + WS_Arr[i];

                    float max_diff = 0;

                    for (int i = 0; i < Avg_Count - 1; i++)
                    {
                        float this_diff = Math.Abs(WD_Arr[i + 1] - WD_Arr[i]);

                        if (this_diff > max_diff)
                            max_diff = this_diff;
                    }

                    if (max_diff > 270)
                    {
                        for (int i = 0; i < Avg_Count; i++)
                            if (WD_Arr[i] > 270) WD_Arr[i] = WD_Arr[i] - 360;
                    }

                    // calculate avg WD

                    for (int i = 0; i < Avg_Count; i++)
                        Avg_WD = Avg_WD + WD_Arr[i];

                    Avg_WS = Avg_WS / Avg_Count;
                    Avg_WD = Avg_WD / Avg_Count;

                    if (Avg_WD < 0) Avg_WD = Avg_WD + 360;

                    DateTime Hour_Date = Last_Date;
                    int This_Year = Hour_Date.Year;
                    int This_Month = Hour_Date.Month;
                    int This_Day = 1;


                    DateTime New_Hour_date = new DateTime(This_Year, This_Month, This_Day);

                    month_file.Write(New_Hour_date + ",");
                    month_file.Write(Math.Round(Avg_WS, 3) + ",");
                    month_file.WriteLine(Math.Round(Avg_WD, 2));



                }

                file.Close();
                month_file.Close();
            }
        }

        private void date_Export_End_ValueChanged(object sender, EventArgs e)
        {
            if (date_Export_End.Value < Ref_Start && Is_Newly_Opened_File == false)
            {
                MessageBox.Show("Export end date cannot be before the start of the reference site data.");
                date_Export_End.Value = Ref_End;
            }
            else
                Export_End = date_Export_End.Value;
        }

        private void btnUpdate_Conc_Plot_Click(object sender, EventArgs e)
        {
            Update_plot();
            Update_Uncert_plot();
        }

        
        private void btnMCP_Uncert_Click(object sender, EventArgs e)
        {
            int WD_ind = Get_WD_ind();
            // how many months in Conc -> Number of MCP_Uncert objects to create
            int Num_Obj = ((Conc_End.Year - Conc_Start.Year) * 12) + Conc_End.Month - Conc_Start.Month;

            string current_method = Get_MCP_Method();

            DateTime Test_Start = Conc_Start;
            DateTime Test_End = Conc_End;
            DateTime Orig_Start = Conc_Start;


            // For every MCP_Uncert, for every possible conc window, construct Uncert structures
            if (current_method == "Orth. Regression")
            {
                Array.Resize(ref Uncert_Ortho, Num_Obj);

                for (int m = 0; m < Num_Obj; m++)
                {
                    Uncert_Ortho[m].WSize = m + 1;
                    Uncert_Ortho[m].NWindows = Num_Obj - m;

                    Array.Resize(ref Uncert_Ortho[m].LT_Ests, Uncert_Ortho[m].NWindows);
                    Array.Resize(ref Uncert_Ortho[m].Start, Uncert_Ortho[m].NWindows);
                    Array.Resize(ref Uncert_Ortho[m].End, Uncert_Ortho[m].NWindows);

                    Test_Start = Orig_Start;

                    for (int i = 0; i < Uncert_Ortho[m].NWindows; i++)
                    {
                        // Initialize First Test Start at Concurrent Start Date at beginning of each iteration
                        Test_End = Test_Start.AddMonths(m + 1);
                         
                        Uncert_Ortho[m].LT_Ests[i] = Do_MCP(Test_Start, Test_End, false, current_method);
                        Uncert_Ortho[m].Start[i] = Test_Start;
                        Uncert_Ortho[m].End[i] = Test_End;

                        Test_Start = Test_Start.AddMonths(1);
                    }
                    // Find Statistics for analysis
                    Calc_Avg_SD_Uncert(ref Uncert_Ortho[m]);
                }
                btnMCP_Uncert.Enabled = false;
            }
            else if (current_method == "Method of Bins")
            {
                Array.Resize(ref Uncert_Bins, Num_Obj);

                for (int m = 0; m < Num_Obj; m++)
                {
                    Uncert_Bins[m].WSize = m + 1;
                    Uncert_Bins[m].NWindows = Num_Obj - m;

                    Array.Resize(ref Uncert_Bins[m].LT_Ests, Uncert_Bins[m].NWindows);
                    Array.Resize(ref Uncert_Bins[m].Start, Uncert_Bins[m].NWindows);
                    Array.Resize(ref Uncert_Bins[m].End, Uncert_Bins[m].NWindows);

                    Test_Start = Orig_Start;

                    for (int i = 0; i < Uncert_Bins[m].NWindows; i++)
                    {
                        // Initialize First Test Start at Concurrent Start Date at beginning of each iteration
                        Test_End = Test_Start.AddMonths(m + 1);

                        Uncert_Bins[m].LT_Ests[i] = Do_MCP(Test_Start, Test_End, false, current_method);
                        Uncert_Bins[m].Start[i] = Test_Start;
                        Uncert_Bins[m].End[i] = Test_End;

                        Test_Start = Test_Start.AddMonths(1);
                    }
                    // Find Statistics for analysis
                    Calc_Avg_SD_Uncert(ref Uncert_Bins[m]);
                }
                btnMCP_Uncert.Enabled = false;
            }
            else if (current_method == "Variance Ratio")
            {
                Array.Resize(ref Uncert_Varrat, Num_Obj);

                for (int m = 0; m < Num_Obj; m++)
                {
                    Uncert_Varrat[m].WSize = m + 1;
                    Uncert_Varrat[m].NWindows = Num_Obj - m;

                    Array.Resize(ref Uncert_Varrat[m].LT_Ests, Uncert_Varrat[m].NWindows);
                    Array.Resize(ref Uncert_Varrat[m].Start, Uncert_Varrat[m].NWindows);
                    Array.Resize(ref Uncert_Varrat[m].End, Uncert_Varrat[m].NWindows);

                    Test_Start = Orig_Start;

                    for (int i = 0; i < Uncert_Varrat[m].NWindows; i++)
                    {
                        // Initialize First Test Start at Concurrent Start Date at beginning of each iteration
                        Test_End = Test_Start.AddMonths(m + 1);

                        Uncert_Varrat[m].LT_Ests[i] = Do_MCP(Test_Start, Test_End, false, current_method);
                        Uncert_Varrat[m].Start[i] = Test_Start;
                        Uncert_Varrat[m].End[i] = Test_End;

                        Test_Start = Test_Start.AddMonths(1);
                    }
                    // Find Statistics for analysis
                    Calc_Avg_SD_Uncert(ref Uncert_Varrat[m]);
                }
                btnMCP_Uncert.Enabled = false;
            }
            // Update Plot
            Update_Uncert_plot();
            //Update List
            Update_Uncert_List();
            Update_Export_buttons();

            Changes_Made();
        }

        public void Calc_Avg_SD_Uncert(ref MCP_Uncert This_Uncert)
        {
            double sum_x = 0;
            double var_x = 0;
            int val_length = This_Uncert.LT_Ests.Length;

            if (This_Uncert.LT_Ests != null)
            {
                foreach (double value in This_Uncert.LT_Ests)
                {
                    sum_x = sum_x + value;
                }

                This_Uncert.avg = Convert.ToSingle(sum_x / val_length);

                foreach (double value in This_Uncert.LT_Ests)
                {
                    var_x = var_x + (Math.Pow(value - This_Uncert.avg, 2) / (val_length));
                }
                This_Uncert.std_dev = Convert.ToSingle(Math.Pow(var_x, 0.5));

            }
        }

        public void Update_Uncert_plot()
        {

            chtUncert.Series.Clear();

            chtUncert.Series.Add("LT Est. Data");
            chtUncert.Series["LT Est. Data"].ChartType = SeriesChartType.Point;
            chtUncert.Series.Add("LT Est. Avg");
            chtUncert.Series["LT Est. Avg"].ChartType = SeriesChartType.Point;
            chtUncert.ChartAreas[0].AxisX.Interval = 1;
            chtUncert.ChartAreas[0].AxisX.Minimum = 0;
            chtUncert.ChartAreas[0].AxisY.Interval = 0.1;
            chtUncert.ChartAreas[0].AxisY.IsStartedFromZero = false;

            // Get Active MCP type
            string active_method = Get_MCP_Method();

            if (active_method == "Orth. Regression" && Uncert_Ortho.Length > 0)
            {
                for (int u = 0; u < Uncert_Ortho.Length; u++)
                {
                    if (Uncert_Ortho[u].LT_Ests != null)
                    {
                        for (int i = 0; i < Uncert_Ortho[u].LT_Ests.Length; i++)
                            chtUncert.Series["LT Est. Data"].Points.AddXY(Uncert_Ortho[u].WSize, Uncert_Ortho[u].LT_Ests[i]);
                    }
                    // Assign LT Avg series = Avg of Uncert obj
                    if (Uncert_Ortho[u].avg != 0)
                    {
                        chtUncert.Series["LT Est. Avg"].Points.AddXY(Uncert_Ortho[u].WSize, Uncert_Ortho[u].avg);
                    }
                }
            }
            if (active_method == "Method of Bins" && Uncert_Bins.Length > 0)
            {
                for (int u = 0; u < Uncert_Bins.Length; u++)
                {
                    if (Uncert_Bins[u].LT_Ests != null)
                    {
                        for (int i = 0; i < Uncert_Bins[u].LT_Ests.Length; i++)
                            chtUncert.Series["LT Est. Data"].Points.AddXY(Uncert_Bins[u].WSize, Uncert_Bins[u].LT_Ests[i]);
                    }
                    // Assign LT Avg series = Avg of Uncert obj
                    if (Uncert_Bins[u].avg != 0)
                    {
                        chtUncert.Series["LT Est. Avg"].Points.AddXY(Uncert_Bins[u].WSize, Uncert_Bins[u].avg);
                    }
                }
            }
            if (active_method == "Variance Ratio" && Uncert_Varrat.Length > 0)
            {
                for (int u = 0; u < Uncert_Varrat.Length; u++)
                {
                    if (Uncert_Varrat[u].LT_Ests != null)
                    {
                        for (int i = 0; i < Uncert_Varrat[u].LT_Ests.Length; i++)
                            chtUncert.Series["LT Est. Data"].Points.AddXY(Uncert_Varrat[u].WSize, Uncert_Varrat[u].LT_Ests[i]);
                    }
                    // Assign LT Avg series = Avg of Uncert obj
                    if (Uncert_Varrat[u].avg != 0)
                    {
                        chtUncert.Series["LT Est. Avg"].Points.AddXY(Uncert_Varrat[u].WSize, Uncert_Varrat[u].avg);
                    }
                }
            }
        }
                
        private void btnExportMultitest_Click(object sender, EventArgs e)
        {
            // Export estimated time series data as a TAB file (i.e. joint WS/WD distribution)

            string filename = "";
            if (sfdSaveTimeSeries.ShowDialog() == DialogResult.OK)
                filename = sfdSaveTimeSeries.FileName;

            string current_method = Get_MCP_Method();
            int ref_start = txtLoadedReference.Text.LastIndexOf('!')+1;
            int targ_start = txtLoadedTarget.Text.LastIndexOf('!')+1;

            if (filename != "")
            {
                StreamWriter file = new StreamWriter(filename);
                file.WriteLine("MCP Uncertainty at Target Site " + current_method + ",");
                file.WriteLine("Reference: " + txtLoadedReference.Text.Substring(ref_start) + ", Target: " + txtLoadedTarget.Text.Substring(targ_start) + ",");
                file.WriteLine("Data binned into " + Get_Num_WD() + " wind direction sectors");
                file.WriteLine("Start Time, End Time, Window Size, LT WS Est, LT Avg, Std Dev");

                if (current_method == "Orth. Regression" && Uncert_Ortho.Length > 0)
                { 
                    for (int u = 0; u < Uncert_Ortho.Length; u++)
                    {
                        // Assign LT Avg series = Avg of Uncert obj
                        if (Uncert_Ortho[u].avg != 0 && Uncert_Ortho[u].std_dev != 0)
                        {
                            for (int i = 0; i < Uncert_Ortho[u].LT_Ests.Length; i++)
                            {
                                file.Write(Uncert_Ortho[u].Start[i]);
                                file.Write(",");
                                file.Write(Uncert_Ortho[u].End[i]);
                                file.Write(",");
                                file.Write(Uncert_Ortho[u].WSize);
                                file.Write(",");
                                file.Write(Uncert_Ortho[u].LT_Ests[i]);
                                file.Write(",");
                                file.Write(Math.Round(Uncert_Ortho[u].avg, 3));
                                file.Write(",");
                                file.Write(Math.Round(Uncert_Ortho[u].std_dev, 4));
                                file.WriteLine();
                            }  
                        }
                    }
                }
                else if (current_method == "Method of Bins" && Uncert_Bins.Length > 0)
                {
                    for (int u = 0; u < Uncert_Bins.Length; u++)
                    {
                        // Assign LT Avg series = Avg of Uncert obj
                        if (Uncert_Bins[u].avg != 0 && Uncert_Bins[u].std_dev != 0)
                        {
                            for (int i = 0; i < Uncert_Bins[u].LT_Ests.Length; i++)
                            {
                                file.Write(Uncert_Bins[u].Start[i]);
                                file.Write(",");
                                file.Write(Uncert_Bins[u].End[i]);
                                file.Write(",");
                                file.Write(Uncert_Bins[u].WSize);
                                file.Write(",");
                                file.Write(Uncert_Bins[u].LT_Ests[i]);
                                file.Write(",");
                                file.Write(Math.Round(Uncert_Bins[u].avg, 3));
                                file.Write(",");
                                file.Write(Math.Round(Uncert_Bins[u].std_dev, 4));
                                file.WriteLine();
                            }
                        }
                    }
                }
                else if (current_method == "Variance Ratio" && Uncert_Varrat.Length > 0)
                {
                    for (int u = 0; u < Uncert_Varrat.Length; u++)
                    {
                        // Assign LT Avg series = Avg of Uncert obj
                        if (Uncert_Varrat[u].avg != 0 && Uncert_Varrat[u].std_dev != 0)
                        {
                            for (int i = 0; i < Uncert_Varrat[u].LT_Ests.Length; i++)
                            {
                                file.Write(Uncert_Varrat[u].Start[i]);
                                file.Write(",");
                                file.Write(Uncert_Varrat[u].End[i]);
                                file.Write(",");
                                file.Write(Uncert_Varrat[u].WSize);
                                file.Write(",");
                                file.Write(Uncert_Varrat[u].LT_Ests[i]);
                                file.Write(",");
                                file.Write(Math.Round(Uncert_Varrat[u].avg, 3));
                                file.Write(",");
                                file.Write(Math.Round(Uncert_Varrat[u].std_dev, 4));
                                file.WriteLine();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No Uncertainty Data for Selected MCP Method Exists, Please Try Again");
                }
                file.Close();
            }
            
        }

        private void btnResetDates_Click(object sender, EventArgs e)
        {
            Reset_Export_Dates();
        }

        private void btnResetConcDates_Click(object sender, EventArgs e)
        {
            Set_Conc_Dates_On_Form();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            New_MCP(true, true);
        }

        public void New_MCP(bool Clear_Ref, bool Clear_Target)
        {
            // Creates a MCP analysis

            if (Clear_Ref == true)
            {
                Ref_Data = new Site_data[0];
                txtLoadedReference.Clear();
                Got_Ref = false;
            }

            if (Clear_Target == true)
            {
                Target_Data = new Site_data[0];
                txtLoadedTarget.Clear();
                Got_Targ = false;
            }
            
            Conc_Data = new Concurrent_data[0];
            Got_Conc = false;

            Num_WD_Sectors = 1;
            WS_bin_width = 1;
            Is_Newly_Opened_File = true;

            for (int i = 0; i < cboNumWD.Items.Count - 1; i ++)
            {
                if (cboNumWD.Items[i].ToString() == Num_WD_Sectors.ToString())
                {
                    cboNumWD.SelectedIndex = i;
                    break;
                }
            }

            txtWS_bin_width.Text = WS_bin_width.ToString();

            MCP_Ortho.Clear();
            MCP_Bins.Clear();
            MCP_Varrat.Clear();

            Uncert_Ortho = new MCP_Uncert[0];
            Uncert_Bins = new MCP_Uncert[0];
            Uncert_Varrat = new MCP_Uncert[0];

            Update_Run_Buttons();
            Update_plot();
            Update_Uncert_plot();
            Update_Text_boxes();
            btnRunMCP.Enabled = true;
            btnMCP_Uncert.Enabled = true;
            Update_Bin_List();
            Update_Uncert_List();
            Update_Export_buttons();

            Saved_Filename = "";
            saveToolStripMenuItem.Enabled = false;
            Changes_Made();

            Is_Newly_Opened_File = false;
                  
            

        }

        private void Set_Default_Folder_locations(string Default_folder)
        {

            int Ind = Default_folder.LastIndexOf('\\');
            Default_folder = Default_folder.Substring(1, Ind);
            ofdOpenMCP.InitialDirectory = Default_folder;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sfdSaveMCP.ShowDialog() == DialogResult.OK)
            {
                string Whole_Path = sfdSaveMCP.FileName;
                Set_Default_Folder_locations(Whole_Path);

                Save_File(Whole_Path);
                
            }
        }

        public void Save_File(string Whole_Path)
        {
            FileStream fStream = File.Create(Whole_Path);
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(fStream, Ref_Start);
            formatter.Serialize(fStream, Ref_End);
            formatter.Serialize(fStream, Target_Start);
            formatter.Serialize(fStream, Target_End);
            formatter.Serialize(fStream, Conc_Start);
            formatter.Serialize(fStream, Conc_End);
            formatter.Serialize(fStream, Export_Start);
            formatter.Serialize(fStream, Export_End);

            formatter.Serialize(fStream, Ref_Data);
            formatter.Serialize(fStream, Got_Ref);
            formatter.Serialize(fStream, Ref_filename);
            formatter.Serialize(fStream, Target_Data);
            formatter.Serialize(fStream, Got_Targ);
            formatter.Serialize(fStream, Target_filename);

            formatter.Serialize(fStream, Conc_Data);
            formatter.Serialize(fStream, Got_Conc);
            formatter.Serialize(fStream, Num_WD_Sectors);
            formatter.Serialize(fStream, WS_bin_width);

            formatter.Serialize(fStream, MCP_Ortho);
            formatter.Serialize(fStream, MCP_Bins);
            formatter.Serialize(fStream, MCP_Varrat);

            formatter.Serialize(fStream, Uncert_Ortho);
            formatter.Serialize(fStream, Uncert_Bins);
            formatter.Serialize(fStream, Uncert_Varrat);

            fStream.Close();
            Saved_Filename = sfdSaveMCP.FileName;
            this.Text = Saved_Filename;
            this.saveToolStripMenuItem.Enabled = true;
        }

        public void Changes_Made()
        {
            this.Text = Saved_Filename + "*";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofdOpenMCP.ShowDialog() == DialogResult.OK)
            {
                string Whole_Path = "";
                Whole_Path = ofdOpenMCP.FileName;
                Set_Default_Folder_locations(Whole_Path);

                FileStream fstream = File.OpenRead(Whole_Path);
                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    Ref_Start = (DateTime)formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Ref_End = (DateTime)formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Target_Start = (DateTime)formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Target_End = (DateTime)formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Conc_Start = (DateTime)formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Conc_End = (DateTime)formatter.Deserialize(fstream);
                }
                catch
                { }
                
                try
                {
                    Export_Start = (DateTime)formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Export_End = (DateTime)formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Ref_Data = (Site_data[])formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Got_Ref = (bool)formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Ref_filename = (string)formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Target_Data = (Site_data[])formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Got_Targ = (bool)formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Target_filename = (string)formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Conc_Data = (Concurrent_data[])formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Got_Conc = (bool)formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Num_WD_Sectors = (int)formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    WS_bin_width = (float)formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    MCP_Ortho = (Lin_MCP)formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    MCP_Bins = (Method_of_Bins)formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    MCP_Varrat = (Lin_MCP)formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Uncert_Ortho = (MCP_Uncert[])formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Uncert_Bins = (MCP_Uncert[])formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Uncert_Varrat = (MCP_Uncert[])formatter.Deserialize(fstream);
                }
                catch
                { }

                Saved_Filename = ofdOpenMCP.FileName;
                this.Text = Saved_Filename;
                saveToolStripMenuItem.Enabled = true;
            }

            Is_Newly_Opened_File = true;

            for (int i = 0; i < cboNumWD.Items.Count; i++)
            {
                if (cboNumWD.Items[i].ToString() == Convert.ToString(Num_WD_Sectors))
                {
                    cboNumWD.SelectedIndex = i;
                    break;
                }
            }

            txtWS_bin_width.Text = Convert.ToString(WS_bin_width);

            Update_Run_Buttons();
            Update_WD_DropDown();
            Update_Bin_List();
            Update_Dates();
            Update_Export_buttons();
            Update_plot();
            Update_Text_boxes();
            Update_Uncert_List();
            Update_Uncert_plot();
            
            Is_Newly_Opened_File = false;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Saved_Filename != "")
                Save_File(Saved_Filename);
        }
    }
}



