////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	MCP.cs
//
// summary:	Imports reference and target WS and WD data and performs Measure-Correlate-Predict
// to estimate long-term wind speeds at target site. 
// Version Number: 1.2.4
// Release Date: 10/30/2019
////////////////////////////////////////////////////////////////////////////////////////////////////

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
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Main Form of MCP tool. </summary>
    ///
    /// <remarks>   OEE, 10/19/2017. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    [Serializable()]
    public partial class MCP_tool : Form
    {
        /// <summary>   Start Date/Time of Reference site data. </summary>
        public DateTime Ref_Start;
        /// <summary>   End Date/Time of Reference site data. </summary>
        public DateTime Ref_End;
        /// <summary>   Start Date/Time of Target site data. </summary>
        public DateTime Target_Start;
        /// <summary>   End Date/Time of Target site data. </summary>
        public DateTime Target_End;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Start Date/Time of the concurrent data (i.e. overlap between Reference and Target) .
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public DateTime Conc_Start;
        /// <summary>   End Date/Time of the concurrent data. </summary>
        public DateTime Conc_End;
        /// <summary>   Start Date/Time of export interval. </summary>
        DateTime Export_Start;
        /// <summary>   End Date/Time of the export interval. </summary>
        DateTime Export_End;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Array of type Site_data for Reference site. Each Site_data entry contains Date, WS, WD and
        /// Temp.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Site_data[] Ref_Data = new Site_data[0];
        /// <summary>   True if reference data has been imported. </summary>
        public bool Got_Ref = false;
        /// <summary>   Filename of the reference site data file. </summary>
        string Ref_filename = "";

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Array of type Site_data for Target site. Each Site_data entry contains Date/Time, WS, WD and
        /// Temp.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Site_data[] Target_Data = new Site_data[0];
        /// <summary>   True if target data has been imported. </summary>
        public bool Got_Targ = false;
        /// <summary>   Filename of the target site data file. </summary>
        string Target_filename = "";

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Array of type Concurrent_data where each entry contains Date/Time, Ref_WS, Ref_WD, Ref_Temp
        /// Target_WS, Target_WD. This array holds the concurrent data for a specified window (i.e. not
        /// necessarily all concurrent data)
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Concurrent_data[] Conc_Data = new Concurrent_data[0];
        /// <summary>   True if conccurent data is defined. </summary>
        public bool Got_Conc = false;

        /// <summary>   Array of type Concurrent_data which holds ALL concurrent data. </summary>
        Concurrent_data[] Conc_Data_All = new Concurrent_data[0];

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Number of Wind Direction sectors to conduct MCP. </summary>
        ///
        /// <value> The total number of wd sectors. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int Num_WD_Sectors { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Width of the Wind Speed bin used in Method of Bins and Matrix-LastWS methods.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public float WS_bin_width = 1;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Number of Hourly intervals to conduct MCP. </summary>
        ///
        /// <value> The total number of hourly ints. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int Num_Hourly_Ints { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Number of temperature bins to conduct MCP. </summary>
        ///
        /// <value> The total number of temporary bins. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int Num_Temp_bins { get; set; }

        // Matrix-LastWS weights

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Weight factor to apply to estimate from defined Reference-Target WS Matrix.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public float Matrix_Wgt = 1;
        /// <summary>   Weight factor to apply to estiamte from defined WS-LastWS Matrix. </summary>
        public float LastWS_Wgt = 1;

        /// <summary>   Minimum Temperature in each WD sector (i) and each hourly interval (j) </summary>
        public float[,] Min_Temp = new float[1,1];
        /// <summary>   Maximum temperature in each WD sector (i) and each hourly interval (j) </summary>
        public float[,] Max_Temp = new float[1, 1];

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Array containing standard deviation of wind speed change at target site for each WS interval.
        /// Used to create Last WS CDF to multiply with Matrix WS PDF in Matrix-LastWS method.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public float[] SD_WS_Lag = new float[0];

        /// <summary>   Object of type Lin_MCP conatining results of orthogonal regression MCP. </summary>
        public Lin_MCP MCP_Ortho = new Lin_MCP();
        /// <summary>   Object of type Method_of_Bins containing results of Method Of Bin MCP. </summary>
        public Method_of_Bins MCP_Bins = new Method_of_Bins();
        /// <summary>   Object of type Lin_MCP conatining results of variance MCP. </summary>
        public Lin_MCP MCP_Varrat = new Lin_MCP();
        /// <summary>   Object of type Matrix_Obj containing results of Matrix-LastWS MCP. </summary>
        public Matrix_Obj MCP_Matrix = new Matrix_Obj();

        /// <summary>   Size of the window step size (in months) used in uncertainty calculations. </summary>
        int Uncert_Step_size = 1;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Array of type MCP_Uncert containing results of uncertainty analysis using orthogonal
        /// regression.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public MCP_Uncert[] Uncert_Ortho = new MCP_Uncert[0];

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Array of type MCP_Uncert containing results of uncertainty analysis using Method of Bins.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public MCP_Uncert[] Uncert_Bins = new MCP_Uncert[0];

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Array of type MCP_Uncert containing results of uncertainty analysis using variance method.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public MCP_Uncert[] Uncert_Varrat = new MCP_Uncert[0];

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Array of type MCP_Uncert containing results of uncertainty analysis using Matrix-LastWS.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public MCP_Uncert[] Uncert_Matrix = new MCP_Uncert[0];

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Array of type Sector_count_bin containing the data count in each WD, hourly and temperature
        /// bin.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        Sector_count_bin[] Sectors = new Sector_count_bin[0];

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Object of type Stats used to perform statistics calcs such as variance and co-variance
        /// calculations.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Stats Stat = new Stats();
        /// <summary>   Filename of the saved MCP file. </summary>
        string Saved_Filename = "";

        /// <summary>   True if this analysis is a newly opened file. </summary>
        bool Is_Newly_Opened_File = false;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Structure defined for uncertainty calculation where multiple windows of varying sizes are
        /// used to generate LT Ests.
        /// </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [Serializable()]
        public struct MCP_Uncert
        {
            /// <summary>   Window size of concurrent WS data interval in months. </summary>
            public int WSize; 
            
            /// <summary>   Number of concurrent WS windows. </summary>
            public int NWindows;
                        
            /// <summary>   Array of long-term WS estimates generated from each concurrent WS window. </summary>
            public double[] LT_Ests;

            /// <summary>   Average LT WS Estimates. </summary>
            public float avg;
            
            /// <summary>   Standard deviation of LT WS Estimates. </summary>
            public float std_dev;

            public float[] Rsq;

            /// <summary>   Start of data period used in uncertainty analysis. </summary>
            public DateTime[] Start;

            /// <summary>   End of data period used in uncertainty analysis. </summary>
            public DateTime[] End;

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            /// <summary>   Clears this MCP_Uncert object to its blank/initial state. </summary>
            ///
            /// <remarks>   OEE, 10/19/2017. </remarks>
            ////////////////////////////////////////////////////////////////////////////////////////////////////

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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Structure defined to hold reference and target wind speed, wind direction and temperature
        /// time series data  (Serializable) a site data.
        /// </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [Serializable()]
        public struct Site_data
        {
            /// <summary>   Time stamp. </summary>
            public DateTime This_Date;
            /// <summary>   Wind Speed. </summary>
            public float This_WS;
            /// <summary>   Wind direction (degs). </summary>
            public float This_WD;
            /// <summary>   Temperature. </summary>
            public float This_Temp;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Structure defined to hold concurrent reference and target wind speed and wind direction time
        /// series plus temperature at reference site.
        /// </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [Serializable()]
        public struct Concurrent_data
        {
            /// <summary>   Time stamp. </summary>
            public DateTime This_Date;
            /// <summary>   Reference wind speed. </summary>
            public float Ref_WS;
            /// <summary>   Reference wind direction. </summary>
            public float Ref_WD;
            /// <summary>   Target wind speed. </summary>
            public float Target_WS;
            /// <summary>   Target wind direction. </summary>
            public float Target_WD;
            /// <summary>   Reference temperature. </summary>
            public float Ref_Temp;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Struture defined to hold linear MCP statistics (such as orthogonol or variance methods)
        /// </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [Serializable()]
        public struct Lin_MCP
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Slope of linear MCP methods for each WD sector and each hourly interval and each temp
            /// interval.
            /// </summary>
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            public float[,,] Slope;

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Intercept of linear MCP methods for each WD sector and each hourly interval and each temp
            /// interval.
            /// </summary>
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            public float[,,] Intercept;

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// R^2 of linear MCP methods for WD and each hourly interval and each temp interval.
            /// </summary>
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            public float[,,] R_sq;
            /// <summary>   Slope of linear MCP methods for all data (all WD, all hours) </summary>
            public float All_Slope;
            /// <summary>   Intercept of linear MCP methods for all data (all WD, all hours) </summary>
            public float All_Intercept;
            /// <summary>   R^2 of linear MCP methods for all data (all WD, all hours) </summary>
            public float All_R_sq;

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Time series of wind speed estimated at target site based on linear MCP methods and reference
            /// WS and WD (WD is same as Ref site WD)
            /// </summary>
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            public Site_data[] LT_WS_Est;

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            /// <summary>   Clears Lin_MCP object to its blank state. </summary>
            ///
            /// <remarks>   OEE, 10/19/2017. </remarks>
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            public void Clear()
            {
                Slope = null;
                Intercept = null;
                R_sq = null;
                LT_WS_Est = null;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Structure defined to hold Method of Bins MCP statistics. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [Serializable()]
        public struct Method_of_Bins
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Array of type Bin_Object containing average ratio, standard deviation of ratio and bin count
            /// for each WS and WD bin (i = WS bin, j = WD bin.)
            /// </summary>
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            public Bin_Object[,] Bin_Avg_SD_Cnt;

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Array of type Site_data containing time series of wind speed and wind direction estimated at
            /// target site based on method of bins (WD is same as Ref site WD)
            /// </summary>
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            public Site_data[] LT_WS_Est;

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            /// <summary>   Clears this Method_of_Bins object to its blank state. </summary>
            ///
            /// <remarks>   OEE, 10/19/2017. </remarks>
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            public void Clear()
            {
                Bin_Avg_SD_Cnt = null;
                LT_WS_Est = null;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Structure defined to contain average and standard deviation of wind speed ratio and data
        /// count used in Method of Bins MCP.
        /// </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [Serializable()]
        public struct Bin_Object
        {
            /// <summary>   Average wind speed ratio. </summary>
            public float Avg_WS_Ratio;
            /// <summary>   Standard deviation of wind speed ratio. </summary>
            public float SD_WS_Ratio;
            /// <summary>   Bin count. </summary>
            public float Count;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Structure defined to hold Matrix cumulative distribution functions and long-term estimated
        /// wind speeds.
        /// </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [Serializable()]
        public struct Matrix_Obj
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Array of type CDF_Obj which contain wind speed cumulative distribution functions which define
            /// probability of a WS at target occurring given a WS at the reference site.
            /// </summary>
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            public CDF_Obj[] WS_CDFs;

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Array of type Site_data containing time series of wind speed and wind direction estimated at
            /// target site based on Matrix-LastWS method (WD is same as Ref site WD)
            /// </summary>
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            public Site_data[] LT_WS_Est;

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            /// <summary>   Clears this object to its initial state. </summary>
            ///
            /// <remarks>   OEE, 10/19/2017. </remarks>
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            public void Clear()
            {
                WS_CDFs = null;
                LT_WS_Est = null;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Structure defined to hold statistics and variables associated with cumulative distribution
        /// functions.
        /// </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [Serializable()]
        public struct CDF_Obj
        {
            /// <summary>   Cumulative Distribution Function. </summary>
            public float[] CDF; 
            /// <summary>   Minimum wind speed. </summary>
            public float Min_WS;
            /// <summary>   Size of WS interval. </summary>
            public float WS_interval;

            /// <summary>   Weibull shape factor, k. </summary>
            public float Weibull_k;
            /// <summary>   Weibull scale factor, A. </summary>
            public float Weibull_A;

            /// <summary>   Wind Speed index. </summary>
            public float WS_ind;
            /// <summary>   Wind direction index. </summary>
            public int WD_ind;
            /// <summary>   Hourly interval index. </summary>
            public int Hour_ind;
            /// <summary>   Temperature interval index. </summary>
            public int Temp_ind;
            /// <summary>   Number of data points used to define CDF. </summary>
            public int Count;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Structure to store sector counts as a function of bins. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [Serializable()]
        public struct Sector_count_bin
        {
            /// <summary>   Wind direction index. </summary>
            public int WD;
            /// <summary>   Hourly index. </summary>
            public int Hour;
            /// <summary>   Temperature index. </summary>
            public int Temp;
            /// <summary>   Data count in bin. </summary>
            public int Count;
        }        

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public MCP_tool()
        {
            InitializeComponent();
            cboMCP_Type.SelectedIndex = 0;
            Num_WD_Sectors = 1;
            Num_Hourly_Ints = 1;
            Num_Temp_bins = 1;

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called when 'Import Reference data' button is clicked. </summary>
        ///
        /// <remarks>   Liz, 5/26/2017. Tested outside of Visual Studio. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void btnImportRef_Click(object sender, EventArgs e)
        {

            // Read in time series wind speed and WD data at reference site
            // Prompt user to find reference data file
            string filename = "";
            

            if (ofdRefSite.ShowDialog() == DialogResult.OK)
                filename = ofdRefSite.FileName;

            if (filename != "")
            {
                Import_Reference_data(filename);

                
                Set_Conc_Dates_On_Form();

                if (Target_Data.Length > 0)
                    Find_Concurrent_Data(true, Conc_Start, Conc_End);

                Find_Min_Max_temp();
                                
                Update_plot();
                Update_Text_boxes();
                Update_Dates();
                Changes_Made();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Imports reference wind speed, wind direction and temperature data. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <param name="filename"> Filename of the reference datafile. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Import_Reference_data(string filename)

        {
            string line;
            DateTime This_Date;
            float This_WS;
            float This_WD;
            float This_temp;

            int Ref_count = 0;
            // Add time series data every 1000 data points (to speed up computation time by not resizing array every time)
            int New_data_count = 0;
            int Catch_Counter = 0;
            Site_data[] TS_data = null;
            Array.Resize(ref TS_data, 1000);

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
                    if (substrings[1] != "NaN" && substrings[2] != "NaN" && substrings[3] != "NaN" && Convert.ToSingle(substrings[1]) > 0 && Convert.ToSingle(substrings[3]) > -270)
                    {
                        This_Date = Convert.ToDateTime(substrings[0]);
                        This_WS = Convert.ToSingle(substrings[1]);
                        This_WD = Convert.ToSingle(substrings[2]);
                        This_temp = Convert.ToSingle(substrings[3]);

                        if (New_data_count < 1000)
                        {
                            TS_data[New_data_count].This_Date = This_Date;
                            TS_data[New_data_count].This_WS = This_WS;
                            TS_data[New_data_count].This_WD = This_WD;
                            TS_data[New_data_count].This_Temp = This_temp;
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
                                Ref_Data[i].This_Temp = TS_data[i - Ref_count].This_Temp;
                            }

                            Ref_count = Ref_count + New_data_count;

                            New_data_count = 0;
                            Array.Resize(ref TS_data, 1000);

                            TS_data[New_data_count].This_Date = This_Date;
                            TS_data[New_data_count].This_WS = This_WS;
                            TS_data[New_data_count].This_WD = This_WD;
                            TS_data[New_data_count].This_Temp = This_temp;
                            New_data_count = New_data_count + 1;
                        }
                    }

                }
                catch
                {
                    if ((New_data_count > 10) || (Catch_Counter > 20)) // only break if an error occurs past the header
                    {
                        MessageBox.Show("Error reading in reference data. Make sure that the file contains four columns: Time Stamp, WS, WD, Temp");
                        txtLoadedReference.Text = "";
                        Ref_filename = "";
                        return;
                    }

                    Catch_Counter++;
                }
            }


            // add last of time series (< 1000)
            Array.Resize(ref Ref_Data, Ref_count + New_data_count);
            for (int i = Ref_count; i < Ref_count + New_data_count; i++)
            {
                Ref_Data[i].This_Date = TS_data[i - Ref_count].This_Date;
                Ref_Data[i].This_WS = TS_data[i - Ref_count].This_WS;
                Ref_Data[i].This_WD = TS_data[i - Ref_count].This_WD;
                Ref_Data[i].This_Temp = TS_data[i - Ref_count].This_Temp;
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
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the wind speed width entered on form for Method of Bins or Matrix-LastWS MCP.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/26/2017. </remarks>
        ///
        /// <returns>   The wind speed interval to use in Method of Bins or Matrix-LastWS MCP. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public float Get_WS_width_for_MCP()
        {            
            return WS_bin_width;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the wind speed interval to use in TAB file export. </summary>
        ///
        /// <remarks>   Liz, 5/26/2017, Not tested since it is a simple textbox to. </remarks>
        ///
        /// <returns>   The wind speed interval used in TAB file export. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public float Get_TAB_export_WS_width()
        {            
            float TAB_WS_bin = Convert.ToSingle(txtTAB_WS_bin.Text);
            return TAB_WS_bin;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the wind direction index to show on the main plot. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <returns>   The wind direction index to plot. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int Get_WD_ind_to_plot()
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets hourly index to show on main plot. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <returns>   The hourly index to plot. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int Get_Hourly_ind_to_plot()
        {
            // Read selected WD sector to show in plot
            int Hourly_ind = 0;
            try
            {
                Hourly_ind = cboHourInt.SelectedIndex;
                if (Hourly_ind == -1)
                {
                    cboHourInt.SelectedIndex = 0;
                    Hourly_ind = 0;
                }
            }
            catch
            {
                cboHourInt.SelectedIndex = 0;
                Hourly_ind = 0;
            }

            return Hourly_ind;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets temperature index to show on main plot. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <returns>   The temperature index to plot. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int Get_Temp_Ind_to_plot()
        {
            int Temp_ind = 0;

            try
            {
                Temp_ind = cboTemp_Int.SelectedIndex;
                if (Temp_ind == -1)
                {
                    cboTemp_Int.SelectedIndex = 0;
                    Temp_ind = 0;
                }
            }
            catch
            {
                cboTemp_Int.SelectedIndex = 0;
                Temp_ind = 0;

            }

            return Temp_ind;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the number of wind direction sectors used in MCP. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <returns>   The number of wind direction sectors. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int Get_Num_WD()
        {                        
            return Num_WD_Sectors;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets number hourly ints. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <returns>   The number hourly ints. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int Get_Num_Hourly_Ints()
        {
            return Num_Hourly_Ints;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets number temperature intervals used in MCP. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <returns>   The number of temperature intervals. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int Get_Num_Temp_Ints()
        {
            return Num_Temp_bins;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets MCP method selected on main form. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <returns>   The MCP method string. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public string Get_MCP_Method()
        {
            string MCP_Method = "";

            try
            {                
                MCP_Method = cboMCP_Type.Text;
            }
            catch
            { }
           
            return MCP_Method;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the WS Matrix PDF weight to use in Matrix-LastWS method. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <returns>   The Matrix WS PDF weight. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public float Get_WS_PDF_Weight()
        {
            // Returns WS PDF weight to be used in Matrix-Last_WS method            
            return Matrix_Wgt;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the LastWS PDF weight to use in Matrix-LastWS method. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <returns>   The LastWS PDF weight. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public float Get_Last_WS_Weight()
        {
            // Returns Last WS weight to be used in Matrix-Last_WS method            
            return LastWS_Wgt;            
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Calculates WD index of entered wind direction . </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <param name="This_WD">  Wind direction in degrees. </param>
        /// <param name="Num_WD">   Number of wind direction sectors. </param>
        ///
        /// <returns>   The wind direction index corresponding to entered wind direction. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int Get_WD_ind(float This_WD, int Num_WD)
        {            
            int WD_ind = (int)Math.Round(This_WD / (360 / (double)Num_WD),0, MidpointRounding.AwayFromZero);
                        
            if (WD_ind == Num_WD) WD_ind = 0;

            return WD_ind;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Calculates WS index of entered wind speed. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <param name="This_WS">      Wind Speed. </param>
        /// <param name="Bin_width">    Width of WS bin. </param>
        ///
        /// <returns>   The wind speed index corresponding to entered wind speed. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int Get_WS_ind(float This_WS, float Bin_width)
        {
            int WS_ind = 0;
            if (Bin_width != 0)
            WS_ind = (int)Math.Round(This_WS / Bin_width,0, MidpointRounding.AwayFromZero);
            
            return WS_ind;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets uncertainty analysis step size as entered on form. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <returns>   The step size (in months) of uncertainty analysis. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int Get_Uncert_Step_Size()
        {            
            return Uncert_Step_size;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Searches and finds the minimum and maximum temperature. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Find_Min_Max_temp()
        {
            Min_Temp = new float[Get_Num_WD(), Get_Num_Hourly_Ints()];
            Max_Temp = new float[Get_Num_WD(), Get_Num_Hourly_Ints()];                                         

            foreach(Site_data This_data in Ref_Data)
            {
                int WD_ind = Get_WD_ind(This_data.This_WD, Get_Num_WD());
                int Hour_ind = Get_Hourly_Index(This_data.This_Date.Hour);

                if ((Min_Temp[WD_ind, Hour_ind] == 0) || (This_data.This_Temp < Min_Temp[WD_ind, Hour_ind])) Min_Temp[WD_ind, Hour_ind] = This_data.This_Temp;
                if ((Max_Temp[WD_ind, Hour_ind] == 0) || (This_data.This_Temp > Max_Temp[WD_ind, Hour_ind])) Max_Temp[WD_ind, Hour_ind] = This_data.This_Temp;

            }
            
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Generates a cumulative distribution functions (CDF) of target wind speed distribution for
        /// each specified WD, hour and temperature bin.
        /// </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <returns>   An array of type CDF_Obj which contains the CDF, bin indices, etc. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public CDF_Obj[] Generate_Matrix_CDFs()
        {
            // calculates WS cumulative distribution functions for every WD, hourly, temperature and WS bin
            // returns array of CDFs
            
            float WS_width = Get_WS_width_for_MCP();
            int Num_WD = Get_Num_WD();
            int Num_Hours = Get_Num_Hourly_Ints();
            int Num_Temps = Get_Num_Temp_Ints();
            int Num_WS = (int)(30 / WS_width);

            if (Num_WS == 0) Num_WS = 1;

            int Num_Matrices = Num_WD * Num_Hours * Num_Temps * Num_WS;
            int CDF_count = 0;
            
            CDF_Obj[] These_CDFs = new CDF_Obj[Num_Matrices];

            for (int i = 0; i < Num_WD; i++)                      
                for (int j = 0; j < Num_Hours; j++)                   
                    for (int k = 0; k < Num_Temps; k++)
                        for (int l = 0; l < Num_WS; l++)
                        {
                            These_CDFs[CDF_count].Hour_ind = j;
                            These_CDFs[CDF_count].Temp_ind = k;
                            These_CDFs[CDF_count].WD_ind = i;
                            These_CDFs[CDF_count].WS_ind = l;                                                                                 

                            float[] Min_Max_WD = Get_Min_Max_WD(i);
                            float[] Min_Max_Temp = Get_Min_Max_Temp(i, j, k);
                            float Min_WS = l * WS_width - WS_width / 2;
                            float Max_WS = l * WS_width + WS_width / 2;

                            float[] Target_WS = Get_Conc_WS_Array("Target", i, j, k, Min_WS, Max_WS, false);

                            if (Target_WS.Length > 1)
                            {
                                Array.Sort(Target_WS);
                                float Targ_Min_WS = Target_WS[0];
                                float Targ_Max_WS = Target_WS[Target_WS.Length - 1];

                                if (Targ_Min_WS == Targ_Max_WS)
                                {
                                    Targ_Min_WS = (float)(Targ_Min_WS - Targ_Min_WS * 0.02);
                                    Targ_Max_WS = (float)(Targ_Max_WS + Targ_Max_WS * 0.02);
                                }

                                float WS_int = (Targ_Max_WS - Targ_Min_WS) / 99;

                                These_CDFs[CDF_count].Count = Target_WS.Length;
                                These_CDFs[CDF_count].Min_WS = Targ_Min_WS;
                                These_CDFs[CDF_count].WS_interval = WS_int;                                                        
                                     
                                                                    
                                // Count WS in each bin
                                int[] WS_count = new int[100];

                                for (int m = 0; m < Target_WS.Length; m++)
                                {
                                    int WS_ind = Convert.ToInt16((Target_WS[m] - Targ_Min_WS) / WS_int);
                                    int Round_ind = (int)Math.Round((Target_WS[m] - Targ_Min_WS) / WS_int, 0);

                                    WS_count[WS_ind]++;
                                }

                                These_CDFs[CDF_count].CDF = new float[100];
                                float[] This_PDF = new float[100];

                                for (int m = 0; m < 100; m++)
                                {
                                    This_PDF[m] = (float)WS_count[m] / Target_WS.Length / WS_int;
                                }

                                //  Calculate CDF                        

                                These_CDFs[CDF_count].CDF[0] = This_PDF[0] * These_CDFs[CDF_count].WS_interval;

                                for (int m = 1; m < 100; m++)
                                {
                                    These_CDFs[CDF_count].CDF[m] = These_CDFs[CDF_count].CDF[m - 1] + This_PDF[m] * These_CDFs[CDF_count].WS_interval;
                                }

                                // interpolate between plateaus in CDF
                                //CDF = Interpolate_CDF(CDF);

                                // normalize CDF to add to 1.0
                                //   for (int j = 0; j < 1000; j++)
                                //      CDF[j] = CDF[j] / CDF[999];
                            }
                            else if (Target_WS.Length > 0)
                            {
                                These_CDFs[CDF_count].Count = 1;
                                These_CDFs[CDF_count].Min_WS = Target_WS[0];
                                These_CDFs[CDF_count].WS_interval = 0;
                            }
                            else
                            {
                                These_CDFs[CDF_count].Count = 0;
                                These_CDFs[CDF_count].Min_WS = l * WS_width;
                                These_CDFs[CDF_count].WS_interval = 0;
                            }

                            CDF_count++;
                        
            }

            return These_CDFs;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Calculates and returns the average target WS [0], reference WS [1] and data count [2]  
        /// during the concurrent period for specified WD, Hourly and temperature index.
        /// </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <param name="WD_ind">   Wind direction index. </param>
        /// <param name="Hour_ind"> Hourly index. </param>
        /// <param name="Temp_ind"> Temperature index. </param>
        /// <param name="Get_All">  If true, then to get average of all concurrent data. </param>
        ///
        /// <returns>
        /// An array of type float containint: 0: Target WS; 1: Reference WS; 2: Data Count'.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public float[] Get_Conc_Avgs_Count(int WD_ind, int Hour_ind, int Temp_ind, bool Get_All)
        {
            float[] Avg_WS_WD = { 0, 0, 0 }; // 0: Target WS; 1: Reference WS; 2: Data Count'

            int This_WD_ind = 0;
            int This_Hour_ind = 0;
            int This_Temp_ind = 0;

            foreach (Concurrent_data Conc in Conc_Data)
                if (Conc.This_Date >= Conc_Start && Conc.This_Date <= Conc_End)
                {
                    This_WD_ind = Get_WD_ind(Conc.Ref_WD, Get_Num_WD());                    
                    This_Hour_ind = Get_Hourly_Index(Conc.This_Date.Hour);                    
                    This_Temp_ind = Get_Temp_ind(This_WD_ind, This_Hour_ind, Conc.Ref_Temp);
                    
                    if ((Get_All == true) || ((This_WD_ind == WD_ind) && (This_Hour_ind == Hour_ind) && (This_Temp_ind == Temp_ind)))
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns array of WS for either the target or reference site for specified WD, hourly and temp
        /// indices for concurrent data set (defined using function Find_Concurrent_Data)
        /// </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <param name="Target_or_Ref">    flag to specifiy target or reference WS. </param>
        /// <param name="WD_ind">           Wind direction index. </param>
        /// <param name="Hourly_ind">       Hourly index. </param>
        /// <param name="Temp_ind">         Temperature index. </param>
        /// <param name="Min_WS">           Minimum ws. </param>
        /// <param name="Max_WS">           Maximum ws. </param>
        /// <param name="Get_All">          If True then it exports all indices. </param>
        ///
        /// <returns>
        /// An array of type float containing either target or reference wind speeds during concurrent
        /// period.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public float[] Get_Conc_WS_Array(string Target_or_Ref, int WD_ind, int Hourly_ind, int Temp_ind, float Min_WS, float Max_WS, bool Get_All)
        {            
            float[] These_WS = null;

            int This_WD_ind = 0;
            int This_Hour_ind = 0;
            int This_Temp_ind = 0;

            if (Got_Conc)
            {
                if (Get_All == true) //|| ((Min_WS == 0) && (Max_WS == 30) && ((WD_ind == 0) && (Get_Num_WD() == 1)) && ((Hourly_ind == 0) && (Get_Num_Hourly_Ints() == 1)) && ((Temp_ind == 0) && (Get_Num_Temp_Ints() == 1))))            
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
                    {                                         
                        This_WD_ind = Get_WD_ind(These_Conc.Ref_WD, Get_Num_WD());
                        This_Hour_ind = Get_Hourly_Index(These_Conc.This_Date.Hour);
                        This_Temp_ind = Get_Temp_ind(This_WD_ind, This_Hour_ind, These_Conc.Ref_Temp);

                        if ((These_Conc.Ref_WS > Min_WS) && (These_Conc.Ref_WS <= Max_WS) && (This_WD_ind == WD_ind) && (This_Hour_ind == Hourly_ind) && (This_Temp_ind == Temp_ind))
                            WD_count++;                                               
                                                  
                    }

                    Array.Resize(ref These_WS, WD_count);
                    WD_count = 0;

                    foreach (Concurrent_data These_Conc in Conc_Data)
                    {
                        This_WD_ind = Get_WD_ind(These_Conc.Ref_WD, Get_Num_WD());
                        This_Hour_ind = Get_Hourly_Index(These_Conc.This_Date.Hour);
                        This_Temp_ind = Get_Temp_ind(This_WD_ind, This_Hour_ind, These_Conc.Ref_Temp);

                        if ((These_Conc.Ref_WS > Min_WS) && (These_Conc.Ref_WS <= Max_WS) && (This_WD_ind == WD_ind) && (This_Hour_ind == Hourly_ind) && (This_Temp_ind == Temp_ind))
                        {
                           /* if (This_WD_ind == 1 && This_Hour_ind == 2 && This_Temp_ind == 1 & Min_WS == 6.5)
                            {
                                MessageBox.Show("Ref WS = " + These_Conc.Ref_WS.ToString() + ", Target WS = " + These_Conc.Target_WS.ToString() + ", Ref WD = " + These_Conc.Ref_WD.ToString());
                            }

  */                          if (Target_or_Ref == "Target")
                                These_WS[WD_count] = These_Conc.Target_WS;
                            else
                                These_WS[WD_count] = These_Conc.Ref_WS;

                            WD_count++;
                       }                                       

                    }
                }
            }

            return These_WS;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Called when 'Import Target data' button is clicked. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void btnImportTarget_Click(object sender, EventArgs e)
        {
            // Read in time series wind speed and WD data at reference site
            // Prompt user to find reference data file
            string filename = "";

            if (ofdRefSite.ShowDialog() == DialogResult.OK)
                filename = ofdRefSite.FileName;

            if (filename != "")
            {
                Import_Target_Data(filename);

                Update_plot();
                Update_Text_boxes();
                Changes_Made();
            }
                           
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Imports Target site wind speed and wind direction data. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <param name="filename"> Filename of the reference datafile. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Import_Target_Data(string filename)
        {        
            string line;
            DateTime This_Date;
            float This_WS;
            float This_WD;
            int Target_count = 0;
            int Catch_Counter = 0;
            int New_data_count = 0;
            Site_data[] TS_data = null;
            Array.Resize(ref TS_data, 1000);

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
                    if (substrings[1] != "NaN" && substrings[2] != "NaN" && Convert.ToSingle(substrings[1]) > 0 && substrings[1] != "" && substrings[2] != "")
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

                            TS_data[New_data_count].This_Date = This_Date;
                            TS_data[New_data_count].This_WS = This_WS;
                            TS_data[New_data_count].This_WD = This_WD;
                            New_data_count = New_data_count + 1;

                        }
                    }

                }
                catch
                {
                    if ((New_data_count > 10) || (Catch_Counter > 20)) // only break if an error occurs past the header
                    {
                        MessageBox.Show("Error reading in target data. Make sure that the file contains three columns of hourly data: Time Stamp, WS, WD");
                        txtLoadedTarget.Text = "";
                        Target_filename = "";                        
                        return;
                    }

                    Catch_Counter++;
                }
            }                       
            
            // add last of time series (< 1000)
            Array.Resize(ref Target_Data, Target_count + New_data_count);
            for (int i = Target_count; i < Target_count + New_data_count; i++)
            {
                Target_Data[i].This_Date = TS_data[i - Target_count].This_Date;
                Target_Data[i].This_WS = TS_data[i - Target_count].This_WS;
                Target_Data[i].This_WD = TS_data[i - Target_count].This_WD;
            }
            Target_count = Target_count + New_data_count;

            if (Target_count == 0)
            {
                MessageBox.Show("No target data was imported. Check your input file!");
                return;
            }

            file.Close();

            Got_Targ = true;
            Target_Start = Target_Data[0].This_Date;
            Target_End = Target_Data[Target_Data.Length - 1].This_Date;

            Set_Conc_Dates_On_Form();

            // Find concurrent data, if have target data
            if (Ref_Data.Length > 0)            
                Find_Concurrent_Data( true, Conc_Start, Conc_End);
            
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates array of Concurrent_data containing WS &amp; WD at reference and target sites.
        /// </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <param name="Conc_form">    If True, uses start and end date on form otherwise uses dates in
        ///                             memory
        ///                              (i.e. this is done in uncertainty calculations). </param>
        /// <param name="Start">        Start Date/Time of concurrent data set. </param>
        /// <param name="End">          End Date/Time of concurrent data set. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Find_Concurrent_Data(bool Conc_form, DateTime Start, DateTime End)
        {            
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
                        Array.Resize(ref Conc_Data_All, Conc_count);
                        Conc_Data_All[Conc_count - 1].This_Date = Target_Data[i].This_Date;
                        Conc_Data_All[Conc_count - 1].Ref_WS = Ref_Data[j].This_WS;
                        Conc_Data_All[Conc_count - 1].Ref_WD = Ref_Data[j].This_WD;
                        Conc_Data_All[Conc_count - 1].Target_WS = Target_Data[i].This_WS;
                        Conc_Data_All[Conc_count - 1].Target_WD = Target_Data[i].This_WD;
                        Conc_Data_All[Conc_count - 1].Ref_Temp = Ref_Data[j].This_Temp;
                        break;
                    }

                }
                if (Target_Data[i].This_Date >= Conc_End)
                {
                    break;
                }
            }

            Conc_Data = Conc_Data_All;

            if (Conc_count == 0 && Conc_form == true)
                MessageBox.Show("There is no concurrent data between the reference and target site for the selected start and end dates.");
            else
                Got_Conc = true;
            
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets hourly index of specified hour and based on defined hourly intervals. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <param name="This_Hour">    Hour value. </param>
        ///
        /// <returns>   The Hourly index. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int Get_Hourly_Index(int This_Hour)
        {
            int Hour_Ind = 0;
            
            if (Num_Hourly_Ints == 1)
                Hour_Ind = 0;
            else if (Num_Hourly_Ints == 2)
                if (This_Hour >= 7 && This_Hour <= 18)
                    Hour_Ind = 0;
                else
                    Hour_Ind = 1;
            else if (Num_Hourly_Ints == 4)
                if (This_Hour >= 7 && This_Hour <= 12)
                    Hour_Ind = 0;
                else if (This_Hour >= 13 && This_Hour <= 18)
                    Hour_Ind = 1;
                else if (This_Hour >= 19 || This_Hour == 0)
                    Hour_Ind = 2;
                else
                    Hour_Ind = 3;
            else if (Num_Hourly_Ints == 6)
                if (This_Hour >= 6 && This_Hour <= 9)
                    Hour_Ind = 0;
                else if (This_Hour >= 10 && This_Hour <= 13)
                    Hour_Ind = 1;
                else if (This_Hour >= 14 && This_Hour <= 17)
                    Hour_Ind = 2;
                else if (This_Hour >= 18 && This_Hour <= 21)
                    Hour_Ind = 3;
                else if (This_Hour >= 22 || This_Hour <= 1)
                    Hour_Ind = 4;
                else
                    Hour_Ind = 5;
            else if (Num_Hourly_Ints == 8)
                if (This_Hour >= 6 && This_Hour <= 8)
                    Hour_Ind = 0;
                else if (This_Hour >= 9 && This_Hour <= 11)
                    Hour_Ind = 1;
                else if (This_Hour >= 12 && This_Hour <= 14)
                    Hour_Ind = 2;
                else if (This_Hour >= 15 && This_Hour <= 17)
                    Hour_Ind = 3;
                else if (This_Hour >= 18 && This_Hour <= 20)
                    Hour_Ind = 4;
                else if (This_Hour >= 21 && This_Hour <= 23)
                    Hour_Ind = 5;
                else if (This_Hour >= 0 && This_Hour <= 2)
                    Hour_Ind = 6;
                else
                    Hour_Ind = 7;
            
            return Hour_Ind;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets temperature index for specified temperature and wind direction and hourly indices.
        /// </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <param name="WD_ind">       Wind direction index. </param>
        /// <param name="Hour_ind">     Hourly index. </param>
        /// <param name="This_temp">    Temperature. </param>
        ///
        /// <returns>   The temperature index. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int Get_Temp_ind(int WD_ind, int Hour_ind, float This_temp)
        {
            int Temp_ind = 0;
            int Num_Temp = Get_Num_Temp_Ints();
            
            if (Num_Temp == 1)
                Temp_ind = 0;
            else if (Num_Temp == 2)
            {                
                float Mid_Temp = (Min_Temp[WD_ind, Hour_ind] + Max_Temp[WD_ind, Hour_ind]) / 2;
                
                if (This_temp <= Mid_Temp)
                    Temp_ind = 0;
                else
                    Temp_ind = 1;
            }
            else if (Num_Temp == 4)
            {
                float Temp_1 = Min_Temp[WD_ind, Hour_ind] + (Max_Temp[WD_ind, Hour_ind] - Min_Temp[WD_ind, Hour_ind]) / 4;                
                float Temp_2 = Min_Temp[WD_ind, Hour_ind] + (Max_Temp[WD_ind, Hour_ind] - Min_Temp[WD_ind, Hour_ind]) / 2;                
                float Temp_3 = Max_Temp[WD_ind, Hour_ind] - (Max_Temp[WD_ind, Hour_ind] - Min_Temp[WD_ind, Hour_ind]) / 4;
                
                if (This_temp <= Temp_1)
                    Temp_ind = 0;
                else if (This_temp <= Temp_2)
                    Temp_ind = 1;
                else if (This_temp <= Temp_3)
                    Temp_ind = 2;
                else
                    Temp_ind = 3;
            }

            return Temp_ind;

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets minimum and maximum temperature in bin with specified wind direction, hourly and
        /// temperature indices.
        /// </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <param name="WD_ind">   Wind direction index. </param>
        /// <param name="Hour_ind"> Hourly index. </param>
        /// <param name="Temp_ind"> Temperature index. </param>
        ///
        /// <returns>   An array of type float containing min and max temperature. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public float[] Get_Min_Max_Temp(int WD_ind, int Hour_ind, int Temp_ind)
        {
            float[] Min_Max_Temp = new float[2];
            int Num_Temp = Get_Num_Temp_Ints();
            int Num_WD = Get_Num_WD();

            float This_Min = 1000;
            float This_Max = -1000;

            if ((WD_ind == Num_WD) || (Num_WD == 1))
            {
                for (int i = 0; i < Get_Num_WD(); i++)
                    for (int j = 0; j < Get_Num_Hourly_Ints(); j++)
                    {
                        if ((This_Min == 1000) || (Min_Temp[i, j] < This_Min))
                            This_Min = Min_Temp[i, j];

                        if ((This_Max == -1000) || (Max_Temp[i, j] > This_Max))
                            This_Max = Max_Temp[i, j];
                    }                                           
            }
            else
            {
                This_Min = Min_Temp[WD_ind, Hour_ind];
                This_Max = Max_Temp[WD_ind, Hour_ind];
            }
            
            if ((Num_Temp == 1) || (Temp_ind == Num_Temp))
            {
                Min_Max_Temp[0] = This_Min;
                Min_Max_Temp[1] = This_Max;
            }
            else if (Num_Temp == 2)
            {
                if (Temp_ind == 0)
                {
                    Min_Max_Temp[0] = This_Min;
                    Min_Max_Temp[1] = (This_Max + This_Min) / 2;
                }
                else
                {
                    Min_Max_Temp[0] = (This_Max + This_Min) / 2;
                    Min_Max_Temp[1] = This_Max;
                }
            }
            else if (Num_Temp == 4)
            {
                if (Temp_ind == 0)
                {
                    Min_Max_Temp[0] = This_Min;
                    Min_Max_Temp[1] = This_Min + (This_Max - This_Min) / 4;
                }
                else if (Temp_ind == 1)
                {
                    Min_Max_Temp[0] = This_Min + (This_Max - This_Min) / 4;
                    Min_Max_Temp[1] = This_Min + (This_Max - This_Min) / 2;
                }
                else if (Temp_ind == 2)
                {
                    Min_Max_Temp[0] = This_Min + (This_Max - This_Min) / 2;
                    Min_Max_Temp[1] = This_Max - (This_Max - This_Min) / 4;
                }
                else if (Temp_ind == 3)
                {
                    Min_Max_Temp[0] = This_Max - (This_Max - This_Min) / 4;
                    Min_Max_Temp[1] = This_Max;
                }
            }

            return Min_Max_Temp;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets minimum and maximum wind direction in specified wind direction bin. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <param name="WD_ind">   Wind direction index. </param>
        ///
        /// <returns>   An array of type float containing min and max wind direction in degs. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public float[] Get_Min_Max_WD(int WD_ind)
        {
            float[] Min_Max_WD = new float[2];
            int Num_WD = Get_Num_WD();

            if ((Num_WD == 1) || (WD_ind == Num_WD))
            {
                Min_Max_WD[0] = 0;
                Min_Max_WD[1] = 360;
            }
            else
            {
                Min_Max_WD[0] = (float)WD_ind * 360 / Num_WD - (float)360 / Num_WD / 2;
                if (Min_Max_WD[0] < 0) Min_Max_WD[0] = Min_Max_WD[0] + 360;

                Min_Max_WD[1] = (float)WD_ind * 360 / Num_WD + (float)360 / Num_WD / 2;
                if (Min_Max_WD[1] > 360) Min_Max_WD[1] = Min_Max_WD[1] - 360;
            }

            return Min_Max_WD;

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Extracts array of concurrent data from Conc_Data_All based on specified start and end dates .
        /// </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <param name="This_Conc_Start">  Start Date/Time of concurrent data to use in MCP. </param>
        /// <param name="This_Conc_End">    End Date/Time of concurrent data to use in MCP. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Get_Subset_Conc_Data(DateTime This_Conc_Start, DateTime This_Conc_End)
        {
            int S = 0;
            int E = 0;
            int L = 0;

            for (int i = 0; i < Conc_Data_All.Length; i++)
            {
                if (Conc_Data_All[i].This_Date == This_Conc_Start)
                    S = i;
                else if (Conc_Data_All[i].This_Date == This_Conc_End)
                {
                    E = i;
                    L = E - S + 1;
                    break;
                }
                else if (i == Conc_Data_All.Length - 1 && E == 0)
                {
                    E = Conc_Data_All.Length;
                    L = E - S;
                    break;
                }
            }

            Array.Resize(ref Conc_Data, L);
            Array.ConstrainedCopy(Conc_Data_All, S, Conc_Data, 0, L);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets sector count. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <param name="WD_ind">   Wind direction index. </param>
        /// <param name="Hour_ind"> Hourly index. </param>
        /// <param name="Temp_ind"> Temperature index. </param>
        ///
        /// <returns>   The sector count. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int Get_Sector_Count(int WD_ind, int Hour_ind, int Temp_ind)
        {

            if (Sectors.Length == 0)
                Find_Sector_Counts();

            int Sector_Count = 0;
            for (int l = 0; l < Sectors.Length; l++)
            {
                if (Sectors[l].WD == WD_ind && Sectors[l].Hour == (Hour_ind) && Sectors[l].Temp == Temp_ind)
                {
                    Sector_Count = Sectors[l].Count;
                    break;
                }
            }

            return Sector_Count;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Executes the MCP calculation. </summary>
        /// <param name="This_Conc_Start">  this conc start Date/Time. </param>
        /// <param name="This_Conc_End">    this conc end Date/Time. </param>
        /// <param name="Use_All_Data">     True to use all data. </param>
        /// <param name="MCP_Method">   MCP_Method may be "Orth. Regression", "Variance Ratio", "Method of Bins", "Matrix" </param>
        
        /// <returns>   A float. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Executes the mcp operation. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <param name="This_Conc_Start">  Start Date/Time of concurrent data to use in MCP. </param>
        /// <param name="This_Conc_End">    End Date/Time of concurrent data to use in MCP. </param>
        /// <param name="Use_All_Data">     True to use all data. </param>
        /// <param name="MCP_Method">       The mcp method. </param>
        ///
        /// <returns>   A float. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public float Do_MCP(DateTime This_Conc_Start, DateTime This_Conc_End, bool Use_All_Data, string MCP_Method)
        {
            // Performs MCP using a linear model (i.e orthogonal regression or variance ratio) or a method of bins or a Matrix method
            // Orth. Reg. minimizes the distance between both the reference and target site wind speeds from the regression line
            
            // Build array of conccurent data for specified dates
            Get_Subset_Conc_Data(This_Conc_Start, This_Conc_End);

            // Calculate the regression for each WD and overall
            // To calculate the slope and intercept, need the variance of Y and X and the co-variance of X and Y

            // First calculate for all WD sectors and all hours and all temperatures
            float Min_WD = 0;
            float Max_WD = 360;
            float[] Ref_WS = Get_Conc_WS_Array("Reference", 0, 0, 0, 0, 30, true);
            float[] Target_WS = Get_Conc_WS_Array("Target", 0, 0, 0, 0, 30, true);
            
            Stats Stat = new Stats();
            float var_x = Convert.ToSingle(Stat.Calc_Variance(Ref_WS));
            float var_y = Convert.ToSingle(Stat.Calc_Variance(Target_WS));
            float covar_xy = Convert.ToSingle(Stat.Calc_Covariance(Ref_WS, Target_WS));

            int Num_WD = Get_Num_WD();
            int Num_Hour = Get_Num_Hourly_Ints();
            int Num_Temp = Get_Num_Temp_Ints();

            int WD_ind;
            float WS_bin = Get_WS_width_for_MCP();
            int Num_WS = (int)(30 / WS_bin);
        
            Method_of_Bins These_Bins = new Method_of_Bins();
            Matrix_Obj This_Matrix = new Matrix_Obj();

            float[] This_Conc = Get_Conc_Avgs_Count(0, 0, 0, true);
            float Avg_Targ = This_Conc[0];
            float Avg_Ref = This_Conc[1];

            float LT_WS_Est = 0; // if Use_All_Data is false then it is an uncertainty analysis and this value is returned
            float This_Slope = 0;
            float This_Int = 0;
            int Total_Count = 0;

            float LastWS_Wgt = Get_Last_WS_Weight();
            float Matrix_Wgt = Get_WS_PDF_Weight();

            // find total data count
            Total_Count = Total_Count + Stat.Get_Data_Count(Ref_Data, Export_Start, Export_End, 0, 0, 0, this, true);
                                    
           // if this is not an uncertainty analysis, then calculate the slope, intercept and R^2 for all WD (this is not used in LT WS Estimation, just GUI)
            if (Use_All_Data == true && MCP_Method == "Orth. Regression")
            {
                MCP_Ortho.Clear();
                MCP_Ortho.Slope = new float[Num_WD, Num_Hour, Num_Temp]; // Slope for each WD and each hour
                MCP_Ortho.Intercept = new float[Num_WD, Num_Hour, Num_Temp];
                MCP_Ortho.R_sq = new float[Num_WD, Num_Hour, Num_Temp];                              

                MCP_Ortho.All_Slope = Calc_Ortho_Slope(var_x, var_y, covar_xy);
                MCP_Ortho.All_Intercept = Stat.Calc_Intercept(Avg_Targ, MCP_Ortho.All_Slope, Avg_Ref);
                MCP_Ortho.All_R_sq = Stat.Calc_R_sqr(covar_xy, var_x, var_y);
            }
            else if (Use_All_Data == true && MCP_Method == "Variance Ratio") 
            {
                MCP_Varrat.Clear();
                MCP_Varrat.Slope = new float[Num_WD, Num_Hour, Num_Temp]; // Slope for each WD and each hour
                MCP_Varrat.Intercept = new float[Num_WD, Num_Hour, Num_Temp];
                MCP_Varrat.R_sq = new float[Num_WD, Num_Hour, Num_Temp];

                MCP_Varrat.All_Slope = Calc_Varrat_Slope(var_x, var_y);
                MCP_Varrat.All_Intercept = Stat.Calc_Intercept(Avg_Targ, MCP_Varrat.All_Slope, Avg_Ref);
                MCP_Varrat.All_R_sq = Stat.Calc_R_sqr(covar_xy, var_x, var_y);
            }
            else if (Use_All_Data == true && MCP_Method == "Method of Bins")
            {
                MCP_Bins.Clear();
                MCP_Bins.Bin_Avg_SD_Cnt = new Bin_Object[Num_WS, Num_WD + 1]; // WD_ind = Num_WD is overall ratio
            }
            else if (MCP_Method == "Matrix")
            {
                This_Matrix.WS_CDFs = Generate_Matrix_CDFs();
                Find_SD_Change_in_WS();
            }
            

            // Now calculate for all WD and all hourly intervals and all temp intervals
            if (MCP_Method == "Orth. Regression" || MCP_Method == "Variance Ratio")
            {                
                for (int i = 0; i < Num_WD; i++)
                    for (int j = 0; j < Num_Hourly_Ints; j++)
                        for (int k = 0; k < Num_Temp; k++)
                        {
                            float[] Min_Max_WD = Get_Min_Max_WD(i);
                            Min_WD = Min_Max_WD[0];
                            Max_WD = Min_Max_WD[1];

                            float[] Min_Max_Temp = Get_Min_Max_Temp(i, j, k);
                            Ref_WS = Get_Conc_WS_Array("Reference", i, j, k, 0, 30, false);
                            Target_WS = Get_Conc_WS_Array("Target", i, j, k, 0, 30, false);

                            // Find Sector count for specific WD, hour, and temp bin combination
                            int Sector_Count = Get_Sector_Count(i,j,k);                                                      

                            var_x = Convert.ToSingle(Stat.Calc_Variance(Ref_WS));
                            var_y = Convert.ToSingle(Stat.Calc_Variance(Target_WS));
                            covar_xy = Convert.ToSingle(Stat.Calc_Covariance(Ref_WS, Target_WS));

                            This_Conc = Get_Conc_Avgs_Count(i, j, k, false);
                            Avg_Targ = This_Conc[0];
                            Avg_Ref = This_Conc[1];

                            if (MCP_Method == "Orth. Regression")
                            {
                                if (This_Conc[2] > 3) // if there are three or fewer concurrent data points in bin then use a slope = Avg Target/Avg Ref WS
                                    This_Slope = Calc_Ortho_Slope(var_x, var_y, covar_xy);
                                else if (This_Conc[2] > 0)
                                    This_Slope = Avg_Targ / Avg_Ref;
                                else
                                    This_Slope = 1;
                            }
                            else
                            {
                                if (This_Conc[2] > 3)
                                    This_Slope = Calc_Varrat_Slope(var_x, var_y);
                                else if (This_Conc[2] > 0)
                                    This_Slope = Avg_Targ / Avg_Ref;
                                else
                                    This_Slope = 1;
                            }

                            // limit slope to +/- 5 (?)
                            if (Math.Abs(This_Slope) > 5)
                                This_Slope = Avg_Targ / Avg_Ref; 

                            if (This_Slope > 5) This_Slope = 5;
                            This_Int = Stat.Calc_Intercept(Avg_Targ, This_Slope, Avg_Ref);

                            if (Use_All_Data == true)
                            {
                                if (MCP_Method == "Orth. Regression")
                                {
                                    MCP_Ortho.Slope[i,j,k] = This_Slope;
                                    MCP_Ortho.Intercept[i,j, k] = This_Int;
                                    MCP_Ortho.R_sq[i,j, k] = Stat.Calc_R_sqr(covar_xy, var_x, var_y);
                                }
                                else // if more linear models are added, will need to add another else if
                                {
                                    MCP_Varrat.Slope[i,j, k] = This_Slope;
                                    MCP_Varrat.Intercept[i,j, k] = This_Int;
                                    MCP_Varrat.R_sq[i,j, k] = Stat.Calc_R_sqr(covar_xy, var_x, var_y);
                                }                                
                            }

                            Avg_Ref = Stat.Calc_Avg_WS(Ref_Data, 0, 10000, Ref_Start, Ref_End, Min_WD, Max_WD, false, j, false, k, this);

                            float This_WS = Avg_Ref * This_Slope + This_Int;
                            if (This_WS < 0)
                                This_WS = 0;
                    
                        if (Double.IsNaN(This_Slope) == false) LT_WS_Est = LT_WS_Est + This_WS * ((float)Sector_Count / (float)Total_Count);
                    }

            }
            else if (MCP_Method == "Method of Bins") // Method of Bins
            {     

                These_Bins.Bin_Avg_SD_Cnt = new Bin_Object[Num_WS, Num_WD + 1];

                foreach (Concurrent_data These_Conc in Conc_Data)
                {
                    int WS_ind = Get_WS_ind(These_Conc.Ref_WS, WS_bin);
                    WD_ind = Get_WD_ind(These_Conc.Ref_WD, Get_Num_WD());
                    
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
                        
            if (Use_All_Data == false && MCP_Method != "Method of Bins" && MCP_Method != "Matrix") // if conducting uncertainty analysis (with a linear model) then return the LT value
                return LT_WS_Est;                       
            
            // Estimate time series at target site
            if (MCP_Method == "Orth. Regression") Array.Resize(ref MCP_Ortho.LT_WS_Est, Ref_Data.Length);
            if (MCP_Method == "Variance Ratio") Array.Resize(ref MCP_Varrat.LT_WS_Est, Ref_Data.Length);
            if (MCP_Method == "Method of Bins") Array.Resize(ref These_Bins.LT_WS_Est, Ref_Data.Length);
            if (MCP_Method == "Matrix") Array.Resize(ref This_Matrix.LT_WS_Est, Ref_Data.Length);

            Random This_Rand = Get_Random_Number();
            float Last_WS = 0;
                        
            for (int i = 0; i < Ref_Data.Length; i++)
            {
                int This_WD_ind = Get_WD_ind(Ref_Data[i].This_WD, Get_Num_WD());
                int WS_ind = Get_WS_ind(Ref_Data[i].This_WS, WS_bin); 

                int This_Hour_ind = Get_Hourly_Index(Ref_Data[i].This_Date.Hour);
                int This_Temp_ind = Get_Temp_ind(This_WD_ind, This_Hour_ind, Ref_Data[i].This_Temp);

                if (MCP_Method == "Orth. Regression")
                {
                    MCP_Ortho.LT_WS_Est[i].This_Date = Ref_Data[i].This_Date;
                    MCP_Ortho.LT_WS_Est[i].This_WD = Ref_Data[i].This_WD;

                    float This_WS = Ref_Data[i].This_WS * MCP_Ortho.Slope[This_WD_ind, This_Hour_ind, This_Temp_ind] +
                        MCP_Ortho.Intercept[This_WD_ind, This_Hour_ind, This_Temp_ind];

                    if (This_WS < 0)
                        This_WS = 0;

                    MCP_Ortho.LT_WS_Est[i].This_WS = This_WS;
                }
                else if (MCP_Method == "Variance Ratio")
                {
                    MCP_Varrat.LT_WS_Est[i].This_Date = Ref_Data[i].This_Date;
                    MCP_Varrat.LT_WS_Est[i].This_WD = Ref_Data[i].This_WD;

                    float This_WS = Ref_Data[i].This_WS * MCP_Varrat.Slope[This_WD_ind, This_Hour_ind, This_Temp_ind] +
                        MCP_Varrat.Intercept[This_WD_ind, This_Hour_ind, This_Temp_ind];

                    if (This_WS < 0)
                        This_WS = 0;

                    MCP_Varrat.LT_WS_Est[i].This_WS = This_WS;

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
                else if (MCP_Method == "Matrix")
                {
                    This_Matrix.LT_WS_Est[i].This_Date = Ref_Data[i].This_Date;
                    CDF_Obj WS_CDF = new CDF_Obj();                    
                                        
                    // find PDF defined for this WD, hourly and temp bin
                    foreach (CDF_Obj This_CDF in This_Matrix.WS_CDFs)
                    {
                        if ((This_CDF.WD_ind == This_WD_ind) && (This_CDF.Hour_ind == This_Hour_ind) && (This_CDF.Temp_ind == This_Temp_ind) && (This_CDF.WS_ind == WS_ind))
                        {
                            WS_CDF = This_CDF;
                            break;
                        }
                    }                                      

                    CDF_Obj Combo_CDF = new CDF_Obj(); // combination of WS PDF and Last WS PDF

                    if ((Last_WS != 0) && (WS_CDF.Count > 1) && (LastWS_Wgt > 0))
                    {
                        
                        float[] Last_WS_CDF = Get_Lag_WS_CDF(Last_WS, WS_CDF.Min_WS, WS_CDF.WS_interval);

                        Combo_CDF.CDF = new float[100];
                        Combo_CDF.Count = 100;
                        Combo_CDF.Min_WS = WS_CDF.Min_WS;
                        Combo_CDF.WS_interval = WS_CDF.WS_interval;

                        // combine WS_CDF with Last_WS_CDF
                        for (int j = 0; j < 100; j++)
                            if (WS_CDF.CDF[j] != 0)
                                Combo_CDF.CDF[j] = (Matrix_Wgt * WS_CDF.CDF[j] + (Last_WS_CDF[j] * LastWS_Wgt)) / (LastWS_Wgt + Matrix_Wgt);                                                                          

                    }
                    else
                        Combo_CDF = WS_CDF;
                                                          
                    
                    if (Combo_CDF.Count > 1)
                    {                   
                        // Generate random number from 0 to 1 and find index in CDF                        
                        float Rand_Num = (float)This_Rand.NextDouble();
                        int Min_ind = Find_CDF_Index(Combo_CDF, Rand_Num);                        
                        This_Matrix.LT_WS_Est[i].This_WS = Combo_CDF.Min_WS + Combo_CDF.WS_interval * Min_ind;
                    }                    
                    else
                    {
                        This_Matrix.LT_WS_Est[i].This_WS = Combo_CDF.Min_WS; // no data for this WS bin so use same WS as reference
                        
                    }

                    This_Matrix.LT_WS_Est[i].This_WD = Ref_Data[i].This_WD;                                    
                    Last_WS = This_Matrix.LT_WS_Est[i].This_WS;                                        
                }
                
            }

            if (MCP_Method == "Method of Bins" && Use_All_Data == true)
                MCP_Bins = These_Bins;
            else if (MCP_Method == "Method of Bins")
                LT_WS_Est = Stat.Calc_Avg_WS(These_Bins.LT_WS_Est, 0, 10000, Ref_Start, Ref_End, 0, 360, true, 0, true, 0, this);
            
            if (MCP_Method == "Matrix" && Use_All_Data == true)
                MCP_Matrix = This_Matrix;
            else if (MCP_Method == "Matrix")
                LT_WS_Est = Stat.Calc_Avg_WS(This_Matrix.LT_WS_Est, 0, 10000, Ref_Start, Ref_End, 0, 360, true, 0, true, 0, this);                       
                   
            if (Use_All_Data == true)
            {
                Update_plot();
                Update_Bin_List();
                Update_Run_Buttons();
                Update_Text_boxes();
                Update_Export_buttons();
            }
            
            return LT_WS_Est;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Searches for the Cumulative Distribution Function index that corresponds to random number
        /// (between 0 and 1).
        /// </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <param name="This_CDF"> This Cumulative Distribution Function. </param>
        /// <param name="Rand_Num"> The random number from 0 to 1. </param>
        ///
        /// <returns>   The CDF index corresponding to random number. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int Find_CDF_Index(CDF_Obj This_CDF, float Rand_Num)
        {
            float Min_Diff = 100;
            int Min_ind = 10000;
            
            // find CDF index that most closely corresponds to random number
            for (int m = 0; m < 100; m++)
            {
                float This_Diff = Math.Abs(This_CDF.CDF[m] - Rand_Num);
                if (This_Diff < Min_Diff) 
                {
                    Min_ind = m;
                    Min_Diff = This_Diff;
                }                
                else if (This_Diff > Min_Diff)
                    break;
            }

            if (Min_ind == 10000)
                Min_ind = 99;

            return Min_ind;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// When creating a CDF from discrete data, plateaus are present in CDF, this function
        /// interpolates between points so the estimated WS ramps up with random number instead of being
        /// step-wise.
        /// </summary>
        ///
        /// <remarks>   NOT CURRENTLY USED. NOT TESTED. </remarks>
        ///
        /// <param name="This_CDF"> this cdf. </param>
        ///
        /// <returns>   Interpolated CDF as array of type float. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public float[] Interpolate_CDF(float[] This_CDF)
        {           
            float[] Interp_CDF = new float[1000];
            float Last_CDF = This_CDF[0];
            int Last_CDF_ind = 0;
            float Next_CDF = 0;
            int Next_CDF_ind = 0;
            int This_WS_ind = 1;

            while (This_WS_ind < 1000)
            {
                if (This_CDF[This_WS_ind] == Last_CDF)
                {
                    while ((This_CDF[This_WS_ind] == Last_CDF) && (This_WS_ind < 999))
                        This_WS_ind++;

                    Next_CDF_ind = This_WS_ind;
                    Next_CDF = This_CDF[This_WS_ind];

                    for (int j = Last_CDF_ind; j <= Next_CDF_ind; j++)
                        Interp_CDF[j] = (float)(j - Last_CDF_ind) / (Next_CDF_ind - Last_CDF_ind) * (This_CDF[Next_CDF_ind] - This_CDF[Last_CDF_ind]) + This_CDF[Last_CDF_ind];

                    Last_CDF = This_CDF[This_WS_ind];
                    Last_CDF_ind = This_WS_ind;
                    This_WS_ind++;

                }
                else
                {
                    Interp_CDF[This_WS_ind] = This_CDF[This_WS_ind];
                    Last_CDF = This_CDF[This_WS_ind];
                    Last_CDF_ind = This_WS_ind;
                    This_WS_ind++;
                }
            }
            
            return Interp_CDF;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Calculates and returns CDF corresponding to last WS estimate and calculated standard
        /// deviation of change in WS from one hour to next.
        /// </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <param name="Last_WS">      Wind speed estimated for previous timestamp. </param>
        /// <param name="CDF_Min_WS">   The minimum WS in CDF. </param>
        /// <param name="CDF_WS_int">   The WS interval in CDF. </param>
        ///
        /// <returns>   Calculated 'LastWS' CDF as array of type float. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public float[] Get_Lag_WS_CDF(float Last_WS, float CDF_Min_WS, float CDF_WS_int)
        {             
            float[] Lag_WS_CDF = new float[100];
            
            int WS_ind = Get_WS_ind(Last_WS, Get_WS_width_for_MCP());

            if (WS_ind >= SD_WS_Lag.Length)
                WS_ind = SD_WS_Lag.Length - 1;                     
            
            int Num_less_Min = (int)Math.Round(CDF_Min_WS / CDF_WS_int, 0);
            float This_X = CDF_Min_WS - Num_less_Min * CDF_WS_int;
            float SD_sqr = (float)Math.Pow(SD_WS_Lag[WS_ind], 2);
            float This_PDF;
            float Last_PDF = 0;
            float Mid_PDF = 0;                       
            
            while (This_X <= CDF_Min_WS)
            {
                This_PDF = 1 / (float)Math.Pow(2 * Math.Pow(SD_sqr, 2) * (float)Math.PI, 0.5) * (float)Math.Exp(-(float)Math.Pow((This_X - Last_WS), 2) / (2 * Math.Pow(SD_sqr, 2)));
                Mid_PDF = (This_PDF + Last_PDF) / 2;

                Lag_WS_CDF[0] = Lag_WS_CDF[0] + CDF_WS_int * Mid_PDF;
                Last_PDF = This_PDF;
                This_X = This_X + CDF_WS_int;
            }
                                               
            for (int i = 1; i < 100; i++)
            {
                This_X = CDF_Min_WS + i * CDF_WS_int;
                This_PDF = 1 / (float)Math.Pow(2 * Math.Pow(SD_sqr, 2) * (float)Math.PI, 0.5) * (float)Math.Exp(-(float)Math.Pow((This_X - Last_WS), 2) / (2 * Math.Pow(SD_sqr, 2)));
                Mid_PDF = (This_PDF + Last_PDF) / 2;

                Lag_WS_CDF[i] = Lag_WS_CDF[i-1] + CDF_WS_int * Mid_PDF;
                Last_PDF = This_PDF;
            }

            // normalize to add to 1.0
            for (int i = 0; i < 100; i++)
                Lag_WS_CDF[i] = Lag_WS_CDF[i] / Lag_WS_CDF[99];

            return Lag_WS_CDF;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Calculate standard deviation of change in wind speed at target site for specified concurrent
        /// period.
        /// </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Find_SD_Change_in_WS()
        {    
            DateTime Last_Record = DateTime.Today;
            DateTime Next_Record = Last_Record.AddHours(1);
                      
            int Num_WS = (int)(30 / Get_WS_width_for_MCP());
            
            SD_WS_Lag = new float[Num_WS];
            float[] This_Avg = new float[Num_WS];
            float Last_WS = 0;
            int[] This_count = new int[Num_WS];

            foreach (Concurrent_data This_Conc in Conc_Data)
            {           
                int WS_ind = Get_WS_ind(This_Conc.Target_WS, Get_WS_width_for_MCP());
                
                if ((Last_WS != 0) && (This_Conc.Target_WS > 0) && (Next_Record == This_Conc.This_Date) && (WS_ind < Num_WS))
                {
                    float This_Diff = This_Conc.Target_WS - Last_WS;
                    This_Avg[WS_ind] = This_Avg[WS_ind] + This_Diff;
                    SD_WS_Lag[WS_ind] = SD_WS_Lag[WS_ind] + (float)Math.Pow(This_Diff,2);
                    Last_WS = This_Conc.Target_WS;
                    This_count[WS_ind]++;
                    
                }
                
                Last_WS = This_Conc.Target_WS;
                Last_Record = This_Conc.This_Date;
                Next_Record = Last_Record.AddHours(1);
            }
                        
            for (int i = 0; i < Num_WS; i++)               
                {
                    if (This_count[i] > 1)
                    {
                        This_Avg[i] = This_Avg[i] / This_count[i];
                        SD_WS_Lag[i] = (float)Math.Pow(SD_WS_Lag[i] / This_count[i] - Math.Pow(This_Avg[i], 2), 0.5);
                    }
                    else
                        SD_WS_Lag[i] = 1; // If no data, assume a SD deviation of 1 m/s
            }
                      
           
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets a random number between 0 and 1. </summary>
        ///
        /// <remarks>   OEE, 10/19/2017. </remarks>
        ///
        /// <returns>   Random number. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Random Get_Random_Number()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);

            return rnd;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Calculates the slope of the orthogonal regression. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="var_x">    Variance of concurrent reference wind speed. </param>
        /// <param name="var_y">    Variance of concurrent target wind speed. </param>
        /// <param name="covar_xy"> Covariance of concurrent reference and target wind speed. </param>
        ///
        /// <returns>   The calculated slope of orthogonal regression. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public float Calc_Ortho_Slope(float var_x, float var_y, float covar_xy)
        {            
            double dbl_slope = 0;
            float slope = 0;

            dbl_slope = (var_y - var_x + Math.Pow((Math.Pow((var_y - var_x), 2) + 4 * Math.Pow(covar_xy, 2)), 0.5)) / (2 * covar_xy);
            slope = Convert.ToSingle(dbl_slope);

            return slope;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Calculates the slope of variance ratio method where slope is defined as ratio of standard
        /// deviation of reference and target concurrent wind speed.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="var_x">    Variance of concurrent reference wind speed. </param>
        /// <param name="var_y">    Variance of concurrent target wind speed. </param>
        ///
        /// <returns>   The calculated slope of variance ratio method. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the text boxes on MCP form. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Update_Text_boxes()
        {
            int WD_ind = Get_WD_ind_to_plot();
            int Num_WD = Get_Num_WD();
            int Hour_ind = Get_Hourly_ind_to_plot();
            int Temp_ind = Get_Temp_Ind_to_plot();

            float Avg_Ref = 0;
            float Avg_Targ = 0;

            // Update Num. Yrs text boxes 
            if (Got_Ref)
            {
                double num_yrs = Ref_Data.Length / 365.0 / 24.0;
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

            if (WD_ind == Num_WD || (WD_ind == 0 && Num_WD == 1))
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

            bool All_hours = false;
            if ((Hour_ind == Num_Hourly_Ints) || ((Hour_ind == 0) && (Num_Hourly_Ints == 1)))
                All_hours = true;

            bool All_Temps = false;
            if ((Temp_ind == Num_Temp_bins) || ((Temp_ind == 0) && (Num_Temp_bins == 1)))
                All_Temps = true;

            Stats Stat = new Stats();
            if (Got_Ref)
            {
                Avg_Ref = Stat.Calc_Avg_WS(Ref_Data, 0, 10000, Ref_Start, Ref_End, Min_WD, Max_WD, All_hours, Hour_ind, All_Temps, Temp_ind, this);
                txtRef_LT_WS.Text = Convert.ToString(Math.Round(Avg_Ref, 2));
            }
            else
                txtRef_LT_WS.Text = "";

            if (Got_Conc)
            {
                float[] This_Min_Max = Get_Min_Max_Temp(WD_ind, Hour_ind, Temp_ind);
                float[] This_Conc = new float[0];
                if ((WD_ind == Num_WD) || ((WD_ind == 0) && (Num_WD == 1) && (All_hours == true) && (All_Temps == true))) // all data
                    This_Conc = Get_Conc_Avgs_Count(WD_ind, Hour_ind, Temp_ind, true);
                else
                    This_Conc = Get_Conc_Avgs_Count(WD_ind, Hour_ind, Temp_ind, false);

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
                float Slope = 0;
                float Intercept = 0;
                float Rsq = 0;
                if (WD_ind == Num_WD || ((WD_ind == 0 && Num_WD == 1) && (Temp_ind == Num_Temp_bins)) || ((WD_ind == 0 && Num_WD == 1) && (Hour_ind == Num_Hourly_Ints)))
                {
                    Slope = MCP_Ortho.All_Slope;
                    Intercept = MCP_Ortho.All_Intercept;
                    Rsq = MCP_Ortho.All_R_sq;
                }
                else
                {
                    Slope = MCP_Ortho.Slope[WD_ind, Hour_ind, Temp_ind];
                    Intercept = MCP_Ortho.Intercept[WD_ind, Hour_ind, Temp_ind];
                    Rsq = MCP_Ortho.R_sq[WD_ind, Hour_ind, Temp_ind];
                }
                
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
                float Slope = 0;
                float Intercept = 0;
                float Rsq = 0;
                if (WD_ind == Num_WD || ((WD_ind == 0 && Num_WD == 1) && (Temp_ind == Num_Temp_bins)))
                {
                    Slope = MCP_Varrat.All_Slope;
                    Intercept = MCP_Varrat.All_Intercept;
                    Rsq = MCP_Varrat.All_R_sq;
                }
                else
                {
                    Slope = MCP_Varrat.Slope[WD_ind, Hour_ind, Temp_ind];
                    Intercept = MCP_Varrat.Intercept[WD_ind, Hour_ind, Temp_ind];
                    Rsq = MCP_Varrat.R_sq[WD_ind, Hour_ind, Temp_ind];
                }

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
                Avg_Ref = Stat.Calc_Avg_WS(Ref_Data, 0, 10000, Ref_Start, Ref_End, Min_WD, Max_WD, All_hours, Hour_ind, All_Temps, Temp_ind, this);
                float Avg_Target_LT = Stat.Calc_Avg_WS(MCP_Ortho.LT_WS_Est, 0, 10000, Ref_Start, Ref_End, Min_WD, Max_WD, All_hours, Hour_ind, All_Temps, Temp_ind, this);
                float Avg_Ratio = Avg_Target_LT / Avg_Ref;
                txtTarg_LT_WS.Text = Convert.ToString(Math.Round(Avg_Target_LT, 2));
                txtLTratio.Text = Convert.ToString(Math.Round(Avg_Ratio, 2));
            }
            else if (MCP_Varrat.LT_WS_Est != null && (Get_MCP_Method() == "Variance Ratio"))
            {
                Avg_Ref = Stat.Calc_Avg_WS(Ref_Data, 0, 10000, Ref_Start, Ref_End, Min_WD, Max_WD, All_hours, Hour_ind, All_Temps, Temp_ind, this);
                float Avg_Target_LT = Stat.Calc_Avg_WS(MCP_Varrat.LT_WS_Est, 0, 10000, Ref_Start, Ref_End, Min_WD, Max_WD, All_hours, Hour_ind, All_Temps, Temp_ind, this);
                float Avg_Ratio = Avg_Target_LT / Avg_Ref;
                txtTarg_LT_WS.Text = Convert.ToString(Math.Round(Avg_Target_LT, 2));
                txtLTratio.Text = Convert.ToString(Math.Round(Avg_Ratio, 2));
            }
            else if (MCP_Bins.LT_WS_Est != null && (Get_MCP_Method() == "Method of Bins"))
            {
                Avg_Ref = Stat.Calc_Avg_WS(Ref_Data, 0, 10000, Ref_Start, Ref_End, Min_WD, Max_WD, All_hours, Hour_ind, All_Temps, Temp_ind, this);
                float Avg_Target_LT = Stat.Calc_Avg_WS(MCP_Bins.LT_WS_Est, 0, 10000, Ref_Start, Ref_End, Min_WD, Max_WD, All_hours, Hour_ind, All_Temps, Temp_ind, this);
                float Avg_Ratio = Avg_Target_LT / Avg_Ref;
                txtTarg_LT_WS.Text = Convert.ToString(Math.Round(Avg_Target_LT, 2));
                txtLTratio.Text = Convert.ToString(Math.Round(Avg_Ratio, 2));
            }
            else if (MCP_Matrix.LT_WS_Est != null && (Get_MCP_Method() == "Matrix"))
            {
                Avg_Ref = Stat.Calc_Avg_WS(Ref_Data, 0, 10000, Ref_Start, Ref_End, Min_WD, Max_WD, All_hours, Hour_ind, All_Temps, Temp_ind, this);
                float Avg_Target_LT = Stat.Calc_Avg_WS(MCP_Matrix.LT_WS_Est, 0, 10000, Ref_Start, Ref_End, Min_WD, Max_WD, All_hours, Hour_ind, All_Temps, Temp_ind, this);
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Updates calendar dates for concurrent period used in MCP and dates used in export.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Update_Dates()
        {
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Resets the export dates with start/end date of reference dataset. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Reset_Export_Dates()
        {
            if (Ref_Data.Length > 0)
            {
                date_Export_Start.Value = Ref_Start;
                date_Export_End.Value = Ref_End;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Find start and end dates of full concurrent period and updates the form dates.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Set_Conc_Dates_On_Form()
        {            
            if (Got_Targ != true || Got_Ref != true)
                return;
                        
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Updates the dropdown menu used to select wind direction to display on plot.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Updates the dropdown meanu used to select temperature interval to display on plot.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Update_Temp_Dropdown()
        {
            cboTemp_Int.Items.Clear();

            int WD_ind = Get_WD_ind_to_plot();
            int Hour_ind = Get_Hourly_ind_to_plot();

            int Num_Temp = Get_Num_Temp_Ints();

            if (Num_Temp == 1)
            {
                cboTemp_Int.Items.Add("All Temps");                
            }
            else if (Num_Temp == 2)
            {
                float[] Min_Max_Temp = Get_Min_Max_Temp(WD_ind, Hour_ind, 0);
                cboTemp_Int.Items.Add(Min_Max_Temp[0] + " - " + Min_Max_Temp[1]);
                Min_Max_Temp = Get_Min_Max_Temp(WD_ind, Hour_ind, 1);
                cboTemp_Int.Items.Add(Min_Max_Temp[0] + " - " + Min_Max_Temp[1]);
                cboTemp_Int.Items.Add("All Temps");
            }
            else if (Num_Temp == 4)
            {
                float[] Min_Max_Temp = Get_Min_Max_Temp(WD_ind, Hour_ind, 0);
                cboTemp_Int.Items.Add(Min_Max_Temp[0] + " - " + Min_Max_Temp[1]);

                Min_Max_Temp = Get_Min_Max_Temp(WD_ind, Hour_ind, 1);
                cboTemp_Int.Items.Add(Min_Max_Temp[0] + " - " + Min_Max_Temp[1]);

                Min_Max_Temp = Get_Min_Max_Temp(WD_ind, Hour_ind, 2);
                cboTemp_Int.Items.Add(Min_Max_Temp[0] + " - " + Min_Max_Temp[1]);

                Min_Max_Temp = Get_Min_Max_Temp(WD_ind, Hour_ind, 3);
                cboTemp_Int.Items.Add(Min_Max_Temp[0] + " - " + Min_Max_Temp[1]);

                cboTemp_Int.Items.Add("All Temps");
            }

            cboTemp_Int.SelectedIndex = cboTemp_Int.Items.Count - 1;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Updates the dropdown meanu used to select hourly interval to display on plot.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Update_Hourly_DropDown()
        {
            cboHourInt.Items.Clear();                      
            
            if (Num_Hourly_Ints == 2)
            {
                cboHourInt.Items.Add("7am - 6pm");
                cboHourInt.Items.Add("7pm - 6am");
            }

            if (Num_Hourly_Ints == 4)
            {
                cboHourInt.Items.Add("7am - 12pm");
                cboHourInt.Items.Add("1pm - 6pm");
                cboHourInt.Items.Add("7pm - 12am");
                cboHourInt.Items.Add("1am - 6am");
            }

            if (Num_Hourly_Ints == 6)
            {
                cboHourInt.Items.Add("6am - 9am");
                cboHourInt.Items.Add("10am - 1pm");
                cboHourInt.Items.Add("2pm - 5pm");
                cboHourInt.Items.Add("6pm - 9pm");
                cboHourInt.Items.Add("10pm - 1am");
                cboHourInt.Items.Add("2am - 5am");
            }

            if (Num_Hourly_Ints == 8)
            {
                cboHourInt.Items.Add("6am - 8am");
                cboHourInt.Items.Add("9am - 11am");
                cboHourInt.Items.Add("12pm - 2pm");
                cboHourInt.Items.Add("3pm - 5pm");
                cboHourInt.Items.Add("6pm - 8pm");
                cboHourInt.Items.Add("9pm - 11pm");
                cboHourInt.Items.Add("12am - 2am");
                cboHourInt.Items.Add("3am - 5am");
            }
            
            cboHourInt.Items.Add("All Hours");
            cboHourInt.SelectedIndex = cboHourInt.Items.Count - 1;

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Enables or disables export buttons based on what analysis has been done. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Update_Export_buttons()
        {             
            if ((Get_MCP_Method() == "Orth. Regression" && MCP_Ortho.LT_WS_Est != null) || (Get_MCP_Method() == "Method of Bins" && MCP_Bins.LT_WS_Est != null) 
                || (Get_MCP_Method() == "Variance Ratio" && MCP_Varrat.LT_WS_Est != null) || (Get_MCP_Method() == "Matrix" && MCP_Matrix.LT_WS_Est != null))
            {
                btnExportTS.Enabled = true;
                btnExportTAB.Enabled = true;
                btnExportAnnualTABs.Enabled = true;
            }
            else
            {
                btnExportTS.Enabled = false;
                btnExportTAB.Enabled = false;
                btnExportAnnualTABs.Enabled = false;
            }

            if (Get_MCP_Method() == "Method of Bins" && MCP_Bins.LT_WS_Est != null)
                btnExportBinRatios.Enabled = true;
            else
                btnExportBinRatios.Enabled = false;

            if ((Get_MCP_Method() == "Orth. Regression" && Uncert_Ortho.Length > 0) || 
                (Get_MCP_Method() == "Method of Bins" && Uncert_Bins.Length > 0) || 
                (Get_MCP_Method() == "Variance Ratio" && Uncert_Varrat.Length > 0) ||
                (Get_MCP_Method() == "Matrix" && Uncert_Matrix.Length > 0))
                btnExportMultitest.Enabled = true;
            else
                btnExportMultitest.Enabled = false;
                                 
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the scatterplot showing target versus reference wind speed. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Update_plot()
        {            
            int WD_ind = Get_WD_ind_to_plot();
            int Num_WD = Get_Num_WD();
            int Hour_ind = Get_Hourly_ind_to_plot();
            int Temp_ind = Get_Temp_Ind_to_plot();

            bool All_hours = false;
            if ((Hour_ind == Num_Hourly_Ints) || (Num_Hourly_Ints == 1))
                All_hours = true;

            bool All_Temps = false;
            if ((Temp_ind == Num_Temp_bins) || (Num_Temp_bins == 1))
                All_Temps = true;

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
                                                               
                float[] Min_Max_WD = Get_Min_Max_WD(WD_ind);
                float Min_WD = Min_Max_WD[0];
                float Max_WD = Min_Max_WD[1];

                float[] Min_Max_Temp = Get_Min_Max_Temp(WD_ind, Hour_ind, Temp_ind);

                float[] This_Ref_WS = new float[0];
                float[] This_Targ_WS = new float[0];

                if ((WD_ind == Num_WD) || ((WD_ind == 0) && (Num_WD == 1) && (All_hours == true) && (All_Temps == true)))
                {
                    This_Ref_WS = Get_Conc_WS_Array("Ref", WD_ind, Hour_ind, Temp_ind, 0, 30, true);
                    This_Targ_WS = Get_Conc_WS_Array("Target", WD_ind, Hour_ind, Temp_ind, 0, 30, true);
                }
                else
                {
                    This_Ref_WS = Get_Conc_WS_Array("Ref", WD_ind, Hour_ind, Temp_ind, 0, 30, false);
                    This_Targ_WS = Get_Conc_WS_Array("Target", WD_ind, Hour_ind, Temp_ind, 0, 30, false);
                }
                              
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
                        float Slope;
                        float Intercept;

                        if (All_hours == true || WD_ind == Num_WD || (WD_ind == 0 && Num_WD == 1))
                        {
                            Slope = MCP_Ortho.All_Slope;
                            Intercept = MCP_Ortho.All_Intercept;
                        }
                        else
                        {
                            Slope = MCP_Ortho.Slope[WD_ind, Hour_ind, Temp_ind];
                            Intercept = MCP_Ortho.Intercept[WD_ind, Hour_ind, Temp_ind];
                        }

                        Ortho_Y[i] = Slope * i + Intercept;
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

                        float Slope;
                        float Intercept;

                        if (All_hours == true)
                        {
                            Slope = MCP_Varrat.All_Slope;
                            Intercept = MCP_Varrat.All_Intercept;
                        }
                        else
                        {
                            Slope = MCP_Varrat.Slope[WD_ind, Hour_ind, Temp_ind];
                            Intercept = MCP_Varrat.Intercept[WD_ind, Hour_ind, Temp_ind];
                        }
                                                
                        Varrat_Y[i] = Slope * i + Intercept;
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
                            chtScatter.Series["Method of Bins"].Points.AddXY(i * Get_WS_width_for_MCP(), MCP_Bins.Bin_Avg_SD_Cnt[i, WD_ind].Avg_WS_Ratio);
                    }
                }
                Update_Text_boxes();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Update list with mean and standard deviation of WS ratios. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Update_Bin_List()
        {             
            lstBins.Items.Clear();

            if (MCP_Bins.Bin_Avg_SD_Cnt != null)
            {
                ListViewItem objlist = new ListViewItem();
                int Num_WS = MCP_Bins.Bin_Avg_SD_Cnt.GetUpperBound(0);
                int Num_WD = Get_Num_WD();
                int WD_ind = Get_WD_ind_to_plot();
                float WS_Width = Get_WS_width_for_MCP();

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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Update table with results of uncertainty analysis showing window size, mean LT estimate and
        /// standard deviation of LT estimate.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Update_Uncert_List()
        {             
            lstUncert.Items.Clear();
            ListViewItem objlist = new ListViewItem();
                        
            string active_method = Get_MCP_Method();

            if ((active_method == "Orth. Regression") && (Uncert_Ortho.Length > 0))
            {
                for (int u = 0; u < Uncert_Ortho.Length; u++)
                {                    
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
                    if ((Uncert_Varrat[u].avg != 0) && (Uncert_Varrat[u].std_dev != 0))
                    {
                        objlist = lstUncert.Items.Add(Convert.ToString(Uncert_Varrat[u].WSize));
                        objlist.SubItems.Add(Convert.ToString(Math.Round(Uncert_Varrat[u].avg, 2)));
                        objlist.SubItems.Add(Convert.ToString(Math.Round(Uncert_Varrat[u].std_dev, 2)));
                    }
                }
            }
            if ((active_method == "Matrix") && (Uncert_Matrix.Length > 0))
            {
                for (int u = 0; u < Uncert_Matrix.Length; u++)
                {                    
                    if ((Uncert_Matrix[u].avg != 0) && (Uncert_Matrix[u].std_dev != 0))
                    {
                        objlist = lstUncert.Items.Add(Convert.ToString(Uncert_Matrix[u].WSize));
                        objlist.SubItems.Add(Convert.ToString(Math.Round(Uncert_Matrix[u].avg, 2)));
                        objlist.SubItems.Add(Convert.ToString(Math.Round(Uncert_Matrix[u].std_dev, 2)));
                    }
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Resets the MCP analysis and clears all calculated onjects. </summary>
        ///
        /// <remarks>
        /// Liz, 5/16/2017. This is different from New_MCP in that it clears the calculated values but it
        /// keeps the user-specified number of bins, weights.
        /// </remarks>
        ///
        /// <param name="All_or_Matrix_or_Bin"> If 'All', clears entire analysis, 'Matrix_and_Bins' clear
        ///                                     Matrix-LastWS and Method_of_Bins objects only, 'Matrix'
        ///                                     clears only Matrix-LasatWS object. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Reset_MCP(string All_or_Matrix_or_Bin)
        {
            if (All_or_Matrix_or_Bin == "All")
            {
                Conc_Data = new Concurrent_data[0];
                MCP_Ortho.Clear();
                MCP_Bins.Clear();
                MCP_Varrat.Clear();
                MCP_Matrix.Clear();

                Uncert_Ortho = new MCP_Uncert[0];
                Uncert_Bins = new MCP_Uncert[0];
                Uncert_Varrat = new MCP_Uncert[0];
                Uncert_Matrix = new MCP_Uncert[0];

                Conc_Start = date_Corr_Start.Value;
                Got_Conc = false;
                
                Num_WD_Sectors = Convert.ToInt16(cboNumWD.Text.ToString());
                Num_Hourly_Ints = Convert.ToInt16(cboNumHours.Text.ToString());
                Num_Temp_bins = Convert.ToInt16(cboNumTemps.Text.ToString());
                WS_bin_width = Convert.ToSingle(txtWS_bin_width.Text);

                Find_Min_Max_temp();
            }
            else if (All_or_Matrix_or_Bin == "Matrix_and_Bins") // this is called if the WS bin width is changed since it only affects Matrix and Method of Bins
            {
                MCP_Bins.Clear();
                MCP_Matrix.Clear();

                Uncert_Bins = new MCP_Uncert[0];
                Uncert_Matrix = new MCP_Uncert[0];
            }
            else if (All_or_Matrix_or_Bin == "Matrix") // this is called if the Matrix or LastWS weights are changed
            {
                MCP_Matrix.Clear();
                Uncert_Matrix = new MCP_Uncert[0];
            }

            Update_Run_Buttons();
            Update_WD_DropDown();
            Update_Hourly_DropDown();
            Update_Temp_Dropdown();
            Update_Text_boxes();
            Update_Bin_List();
            Update_Uncert_List();
            Update_plot();
            Update_Uncert_plot();
            Update_Export_buttons();
            Changes_Made();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called when number of wind direction sector dropdown is changed and resets MCP
        /// analysis if calculations have been conducted already.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void cboNumWD_SelectedIndexChanged(object sender, EventArgs e)
        {            
            if ((MCP_Ortho.Slope == null) && (MCP_Varrat.Slope == null) && (MCP_Bins.Bin_Avg_SD_Cnt == null) && (MCP_Matrix.LT_WS_Est == null) && (Uncert_Ortho.Length == 0)
                && (Uncert_Varrat.Length == 0) && (Uncert_Matrix.Length == 0) && (Uncert_Bins.Length == 0))
            {
                Num_WD_Sectors = Convert.ToInt16(cboNumWD.SelectedItem.ToString());
                Find_Min_Max_temp();
                Update_WD_DropDown();
            }
            else if ((Is_Newly_Opened_File == false) && ((MCP_Ortho.Slope != null) || (MCP_Varrat.Slope != null) || 
                (MCP_Bins.Bin_Avg_SD_Cnt != null) || (MCP_Matrix.LT_WS_Est != null) || (Uncert_Ortho.Length > 0) || 
                (Uncert_Varrat.Length > 0) || (Uncert_Bins.Length > 0) || (Uncert_Matrix.Length > 0)))
            {
                string message = "Changing the number of WD bins will reset the MCP. Do you want to continue?";
                DialogResult result = MessageBox.Show(message, "", MessageBoxButtons.YesNo);

                if (result == System.Windows.Forms.DialogResult.Yes)
                    Reset_MCP("All");
                else
                    cboNumWD.Text = Num_WD_Sectors.ToString();                       
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called when 'Clear Reference data' button is clicked. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void btnClearRef_Click(object sender, EventArgs e)
        {
            string message = "Are you sure that you want to clear the reference data?";
            string caption = "Clear Reference Data";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                New_MCP(true, false);
                Changes_Made();
            }                    
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called when 'Clear Target data' button is clicked. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void btnClearTarget_Click(object sender, EventArgs e)
        {
            if (Target_Data.Length > 0)
            {
                string message = "Are you sure that you want to clear the target data?";
                string caption = "Clear Target Data";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, caption, buttons); ;
                            
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    New_MCP(false, true);
                    Changes_Made();
                }
            }       

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called when 'Run MCP' button is clicked. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void btnRunMCP_Click(object sender, EventArgs e)
        {
            
            string MCP_method = Get_MCP_Method();
                        
            Find_Concurrent_Data(true, Conc_Start, Conc_End);
            Find_Sector_Counts();

            Do_MCP(Conc_Start, Conc_End, true, MCP_method);                              

            Changes_Made();                        
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called when WD Sector dropdown is changed which triggers the scatterplot to be
        /// updated. If selected WD interval is anything other than "All WD" and selected hourly interval
        /// is "All Hours" or selected Temperature bin is "All temps" then set to 'All WD' since we do
        /// MCP for All Hours and All WD and All temp and then for each WD, hourly interval and
        /// temperature bin.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void cboWD_sector_SelectedIndexChanged(object sender, EventArgs e)
        {            
            if (cboWD_sector.SelectedItem.ToString() != "All WD")
            {
                if (cboHourInt.SelectedItem.ToString() == "All Hours")
                    cboHourInt.SelectedIndex = 0;
                if (cboTemp_Int.SelectedItem.ToString() == "All Temps")
                    cboTemp_Int.SelectedIndex = 0;
            }

            if (cboWD_sector.SelectedItem.ToString() == "All WD")
            {
                if (cboHourInt.SelectedItem.ToString() != "All Hours")
                    cboHourInt.SelectedIndex = cboHourInt.Items.Count - 1;
                if (cboTemp_Int.SelectedItem.ToString() != "All Temps")
                    cboTemp_Int.SelectedIndex = cboTemp_Int.Items.Count - 1;

            }

            Update_plot();
            Update_Uncert_plot();
            Update_Text_boxes();
            Update_Bin_List();
            Update_Uncert_List();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called when Correlate Start data/time value is changed. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void date_Corr_Start_ValueChanged(object sender, EventArgs e)
        {
            DialogResult result = System.Windows.Forms.DialogResult.Yes;

            if (((MCP_Ortho.Slope != null) || (MCP_Bins.Bin_Avg_SD_Cnt != null) || (MCP_Varrat.Slope != null)) && Is_Newly_Opened_File == false)
            {
                string message = "Changing the start of the correlation will reset the MCP. Do you want to continue?";
                result = MessageBox.Show(message, "", MessageBoxButtons.YesNo);
            }

            if (result == System.Windows.Forms.DialogResult.Yes && Is_Newly_Opened_File == false)
            {
                Reset_MCP("All");
                Changes_Made();                
            }
                        
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called when Correlate End data/time value is changed. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void date_Corr_End_ValueChanged(object sender, EventArgs e)
        {
            DialogResult result = System.Windows.Forms.DialogResult.Yes;

            if ((Is_Newly_Opened_File == false) && ((MCP_Ortho.Slope != null) || (MCP_Bins.Bin_Avg_SD_Cnt != null) || (MCP_Varrat.Slope != null)))
            {
                string message = "Changing the end of the correlation will reset the MCP. Do you want to continue?";                
                result = MessageBox.Show(message, "", MessageBoxButtons.YesNo);
            }

            if (result == System.Windows.Forms.DialogResult.Yes && Is_Newly_Opened_File == false)
            {
                Reset_MCP("All");
                Changes_Made();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called when Export_Start date is changed. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called by MCP_tool for load events. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void MCP_tool_Load(object sender, EventArgs e)
        {
            //
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called when MCP method selected in dropdown is changed. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the 'Run MCP' and 'Run Uncertainty analysis' buttons. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Update_Run_Buttons()
        {
            string MCP_type = Get_MCP_Method();
                        
            if (((MCP_type == "Orth. Regression") && (MCP_Ortho.Slope != null)) || ((MCP_type == "Method of Bins") && (MCP_Bins.Bin_Avg_SD_Cnt != null)) 
                || ((MCP_type == "Variance Ratio") && (MCP_Varrat.Slope != null)) || ((MCP_type == "Matrix") && (MCP_Matrix.WS_CDFs != null)))
                btnRunMCP.Enabled = false;
            else
                btnRunMCP.Enabled = true;

            if ((MCP_type == "Orth. Regression" && Uncert_Ortho.Length > 0) || (MCP_type == "Method of Bins" && Uncert_Bins.Length > 0) 
                || (MCP_type == "Variance Ratio" && Uncert_Varrat.Length > 0) || (MCP_type == "Matrix" && Uncert_Matrix.Length > 0))
                btnMCP_Uncert.Enabled = false;
            else
                btnMCP_Uncert.Enabled = true;                          
             
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called when 'Export Estimated Data as Time Series' button is clicked. Exports
        /// estimated time series of WS and WD at target site to a .CSV.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. Tested outside of Visual Studio. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void btnExportTS_Click(object sender, EventArgs e)
        {
            string filename = "";
            
            // Check that the export start/end are within interval of estimated data
            if (Export_Start > Ref_End)
            {
                MessageBox.Show("The selected export start date is after the end of the reference data period.");
                return;
            }

            try
            {

                if (sfdSaveTimeSeries.ShowDialog() == DialogResult.OK)
                {
                    filename = sfdSaveTimeSeries.FileName;

                    StreamWriter file = new StreamWriter(filename);
                    file.WriteLine("MCP WS & WD Estimates");
                    file.WriteLine(DateTime.Today);
                    file.WriteLine(Get_MCP_Method());
                    file.WriteLine("Data binned into " + Get_Num_WD() + " WD bins; " + Get_Num_Hourly_Ints() + " Hourly bins; " + Get_Num_Temp_Ints() + " Temp bins" );
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
                                file.Write(Math.Round(LT_WS_WD.This_WS, 4));
                                file.Write(",");
                                file.Write(Math.Round(LT_WS_WD.This_WD, 3));
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
                    else if (Get_MCP_Method() == "Matrix" && MCP_Matrix.LT_WS_Est != null)
                    {
                        foreach (Site_data LT_WS_WD in MCP_Matrix.LT_WS_Est)
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
            catch
            {
                MessageBox.Show("Error saving to file. Check that it is not open in another program");
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called when 'Export WS Bin Ratios' button is clicked. Exports average, SD and
        /// count of WS ratios in each WS/WD bin.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void btnExportBinRatios_Click(object sender, EventArgs e)
        {
            string filename = "";
            if (sfdSaveTimeSeries.ShowDialog() == DialogResult.OK)
                filename = sfdSaveTimeSeries.FileName;
            else
                return;

            try
            {
                StreamWriter file = new StreamWriter(filename);
                file.WriteLine("Avg, SD & Count of WS Ratios (Target/Reference) from Method of Bins");
                file.WriteLine(DateTime.Today.ToShortDateString());
                file.WriteLine();

                file.WriteLine("Average WS Ratios by WS & WD");
                file.WriteLine();
                file.Write("WS [m/s],");
                for (int i = 0; i <= MCP_Bins.Bin_Avg_SD_Cnt.GetUpperBound(0); i++)
                {
                    file.Write(i * Get_WS_width_for_MCP());
                    file.Write(",");
                }
                file.WriteLine();

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
                    file.Write(i * Get_WS_width_for_MCP());
                    file.Write(",");
                }
                file.WriteLine();

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
                    file.Write(i * Get_WS_width_for_MCP());
                    file.Write(",");
                }
                file.WriteLine();

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
                            file.Write(MCP_Bins.Bin_Avg_SD_Cnt[i, j].Count);
                            file.Write(",");
                        }
                        else
                            file.Write(" ,");
                    file.WriteLine();
                }

                file.Close();
            }
            catch
            {
                MessageBox.Show("Error saving to file. Make sure that is not open in another program.");
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called when 'Export Estimated data as TAB file'. Export estimated time series
        /// data as a TAB file (i.e. joint WS/WD distribution)
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void btnExportTAB_Click(object sender, EventArgs e)
        {
            string filename = "";
            if (sfdSaveTAB.ShowDialog() == DialogResult.OK)
                filename = sfdSaveTAB.FileName;

            Create_TAB_file(filename, Export_Start, Export_End);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates TAB file which is joint wind speed and wind direction distribution.
        /// </summary>
        ///
        /// <remarks>
        /// Liz, 5/16/2017. Tested outside Visual Studio by comparing to Excel VBA: 'Create TAB
        /// file.xlsm'.
        /// </remarks>
        ///
        /// <param name="filename">     Filename of the output TAB file. </param>
        /// <param name="This_Start">   Start Date/Time to use when creating WS/WD distribution. </param>
        /// <param name="This_End">     End Date/Time to use when creating WS/WD distribution. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Create_TAB_file(string filename, DateTime This_Start, DateTime This_End)
        {
            if (filename != "")
            {
                try
                {
                    // open file to output TAB file
                    StreamWriter file = new StreamWriter(filename);
                    string MetName = txtTargetName.Text;
                    file.WriteLine(MetName);

                    // read in name, UTMX/Y and height
                    string UTMX = txtUTMX.Text;
                    string UTMY = txtUTMY.Text;
                    double Height = 0;

                    try
                    {
                        Height = Math.Round(Convert.ToDouble(txtHeight.Text), 1);
                    }
                    catch
                    {
                        MessageBox.Show("Error reading the hub height. Entering zero in TAB file");
                    }

                    string UTMs_Height = UTMX + " " + UTMY + " " + Height;

                    int Num_bins = Convert.ToInt16(cboTAB_bins.Text);
                    float WS_bin_width = Get_TAB_export_WS_width();

                    try
                    {
                        WS_bin_width = Convert.ToSingle(txtWS_bin_width.Text);
                    }
                    catch
                    {
                        MessageBox.Show("Invalid wind speed bin width");
                        file.Close();
                        return;
                    }

                    // write TAB header details
                    file.WriteLine(UTMs_Height);
                    file.Write(Num_bins);
                    file.Write(" ");
                    file.Write(WS_bin_width);
                    file.WriteLine(" 0");

                    int Num_WD = Num_bins;
                    int Num_WS = Convert.ToInt16(31 / WS_bin_width);

                    float[] Wind_Rose = new float[Num_WD];
                    float[,] WSWD_Dist = new float[Num_WS, Num_WD];

                    DateTime This_TS = DateTime.Today;
                    float This_WS = 0;
                    float This_WD = 0;

                    string MCP_type = Get_MCP_Method();

                    int Est_data_ind = 0;

                    // searches through MCP LT WS Est timeseries to find Est_data_ind corresponding
                    // to first data point to use in TAB file
                    if (MCP_type == "Orth. Regression" && MCP_Ortho.LT_WS_Est != null)
                    {
                        for (int i = 0; i < MCP_Ortho.LT_WS_Est.Length; i++)
                        {
                            if (MCP_Ortho.LT_WS_Est[i].This_Date < This_Start)
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
                            if (MCP_Bins.LT_WS_Est[i].This_Date < This_Start)
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
                            if (MCP_Varrat.LT_WS_Est[i].This_Date < This_Start)
                                Est_data_ind++;
                            else
                                break;
                        }

                        This_TS = MCP_Varrat.LT_WS_Est[Est_data_ind].This_Date;
                        This_WS = MCP_Varrat.LT_WS_Est[Est_data_ind].This_WS;
                        This_WD = MCP_Varrat.LT_WS_Est[Est_data_ind].This_WD;
                        Est_data_ind++;

                    }
                    else if (MCP_type == "Matrix" && MCP_Matrix.LT_WS_Est != null)
                    {
                        for (int i = 0; i < MCP_Matrix.LT_WS_Est.Length; i++)
                        {
                            if (MCP_Matrix.LT_WS_Est[i].This_Date < This_Start)
                                Est_data_ind++;
                            else
                                break;
                        }

                        This_TS = MCP_Matrix.LT_WS_Est[Est_data_ind].This_Date;
                        This_WS = MCP_Matrix.LT_WS_Est[Est_data_ind].This_WS;
                        This_WD = MCP_Matrix.LT_WS_Est[Est_data_ind].This_WD;
                        Est_data_ind++;

                    }
                                        
                    // starting at This_Start, goes through LT WS Est data, until it reaches This_End,
                    // and finds WD and WS/WD distributions
                    while (This_TS <= This_End)
                    {
                        if (This_WS >= 0 && This_WD >= 0)
                        {                
                            int WS_ind = Get_WS_ind(This_WS, WS_bin_width);
                            int WD_ind = Get_WD_ind(This_WD, Num_bins);

                            if (WS_ind > 30) WS_ind = 30;

                            Wind_Rose[WD_ind]++;
                            WSWD_Dist[WS_ind, WD_ind]++;                         
                                                       
                        }

                        if (This_TS == This_End)
                            break;

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
                        else if (MCP_type == "Matrix" && MCP_Matrix.LT_WS_Est != null)
                        {
                            This_TS = MCP_Matrix.LT_WS_Est[Est_data_ind].This_Date;
                            This_WS = MCP_Matrix.LT_WS_Est[Est_data_ind].This_WS;
                            This_WD = MCP_Matrix.LT_WS_Est[Est_data_ind].This_WD;
                            Est_data_ind++;
                        }
                    }


                    float Sum_WD = 0;
                    for (int i = 0; i < Num_WD; i++)
                        Sum_WD = Sum_WD + Wind_Rose[i];

                    for (int i = 0; i < Num_WD; i++)
                    {
                        Wind_Rose[i] = Wind_Rose[i] / Sum_WD * 100;
                        file.Write(Math.Round(Wind_Rose[i], 4) + "\t");
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
                        file.Write((float)(i + (float)WS_bin_width / 2) + "\t");
                        for (int j = 0; j < Num_WD; j++)
                            file.Write(Math.Round(WSWD_Dist[i, j], 3) + "\t");
                        file.WriteLine();

                    }

                    file.Close();
                }
                catch
                {
                    MessageBox.Show("Error writing to file. Make sure that it is not open in another program.");
                }
            }
            

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called when textbox "WS bin width" value changes. This value only affects
        /// Method of Bins and Matrix-LastWS MCP methods.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void txtWS_bin_width_TextChanged(object sender, EventArgs e)
        {
            DialogResult result = DialogResult.Yes;

            if (Is_Newly_Opened_File == false && (MCP_Matrix.LT_WS_Est != null || MCP_Bins.Bin_Avg_SD_Cnt != null || Uncert_Matrix.Length > 0 || Uncert_Bins.Length > 0))
            {
                string message = "Changing the WS bin width will reset the MCP. Do you want to continue?";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                result = MessageBox.Show(message, "", buttons);
            }

            if (Is_Newly_Opened_File == false && result == System.Windows.Forms.DialogResult.Yes)
            {
                Reset_MCP("Matrix_and_Bins");
                if (txtWS_bin_width.Text != "0") 
                    WS_bin_width = Convert.ToSingle(txtWS_bin_width.Text);
            }
            else
                txtWS_bin_width.Text = WS_bin_width.ToString();
            
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called when 'Convert to Hourly' button is clicked.  Reads in 10-minute time
        /// series WS and WD data, converts to hourly data and saves .CSV file.
        /// </summary>
        ///
        /// <remarks>
        /// Liz, 5/16/2017. Tested outside of Visual Studio by comparing to excel VBA tool output.
        /// </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void btnConvertToHourly_Click(object sender, EventArgs e)
        {            
            // Prompt user to find reference data file
            string filename = "";

            if (ofdRefSite.ShowDialog() == DialogResult.OK)
                filename = ofdRefSite.FileName;
            else
                return;

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

            string[] split_filename = filename.Split('.');

            string hour_filename = filename.Substring(0, filename.LastIndexOf('.')) + "_hourly.csv";

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
                        if (substrings[1] != "NaN" && substrings[2] != "NaN" && substrings[1] != "" && substrings[2] != "")
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called when 'Convert to Monthly' button is clicked. Reads in 10-mi time series
        /// WS and WD data, converts to monthly and saves .CSV file. It calculates the average WS and the
        /// mode of WD (to nearest 5 degrees)
        /// </summary>
        ///
        /// <remarks>
        /// Liz, 5/16/2017. Tested outside of Visual Studio by comparing to output from excel.
        /// </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void btnConvertMonthly_Click(object sender, EventArgs e)
        {
            
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
            float Mode_WD = 0;
            int Avg_Count = 0;

            if (ofdRefSite.ShowDialog() == DialogResult.OK)
                filename = ofdRefSite.FileName;
            else
                return;

            string[] split_filename = filename.Split('.');
            string month_filename = split_filename[0] + "_monthly.csv";

            try
            {

                if (filename != "")
                {

                    StreamReader file = new StreamReader(filename);
                    StreamWriter month_file = new StreamWriter(month_filename);

                    while ((line = file.ReadLine()) != null)
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

                                    Avg_WS = Avg_WS / Avg_Count;

                                    // find WD mode (most frequently occurring WD to nearest 5 degrees)
                                    int[] WD_Freq = new int[72]; // array of WD frequency (bin size = 5 degs) centered around 0

                                    for (int i = 0; i < Avg_Count; i++)
                                    {
                                        int WD_ind = (int)Math.Round(WD_Arr[i] / 5, 0);
                                        if (WD_ind >= 72)
                                            WD_ind = 0;
                                        WD_Freq[WD_ind]++;
                                    }

                                    // find sector with highest count
                                    int Freq_High = 0;
                                    for (int i = 0; i < 72; i++)
                                    {
                                        if (WD_Freq[i] > Freq_High)
                                        {
                                            Freq_High = WD_Freq[i];
                                            Mode_WD = i * 5;
                                        }
                                    }

                                    DateTime Hour_Date = Last_Date;
                                    int This_Year = Hour_Date.Year;
                                    int This_Month = Hour_Date.Month;
                                    int This_Day = 1;

                                    DateTime New_Hour_date = new DateTime(This_Year, This_Month, This_Day);

                                    month_file.Write(New_Hour_date + ",");
                                    month_file.Write(Math.Round(Avg_WS, 3) + ",");
                                    month_file.WriteLine(Math.Round(Mode_WD, 2));

                                    Avg_Count = 0;
                                    Avg_WS = 0;
                                    Mode_WD = 0;
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
                                    Mode_WD = 0;
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
                                       
                                       

                    if (Avg_Count >= 15)
                    {
                        // calculate avg WS
                        for (int i = 0; i < Avg_Count; i++)
                            Avg_WS = Avg_WS + WS_Arr[i];

                        Avg_WS = Avg_WS / Avg_Count;

                        // find WD mode (most frequently occurring WD to nearest 5 degrees)
                        int[] WD_Freq = new int[72]; // array of WD frequency (bin size = 5 degs) centered around 0

                        for (int i = 0; i < Avg_Count; i++)
                        {
                            int WD_ind = (int)Math.Round(WD_Arr[i] / 5, 0);
                            if (WD_ind >= 72)
                                WD_ind = 0;
                            WD_Freq[WD_ind]++;
                        }

                        // find sector with highest count
                        int Freq_High = 0;
                        for (int i = 0; i < 72; i++)
                        {
                            if (WD_Freq[i] > Freq_High)
                            {
                                Freq_High = WD_Freq[i];
                                Mode_WD = i * 5;
                            }
                        }

                        DateTime Hour_Date = Last_Date;
                        int This_Year = Hour_Date.Year;
                        int This_Month = Hour_Date.Month;
                        int This_Day = 1;

                        DateTime New_Hour_date = new DateTime(This_Year, This_Month, This_Day);

                        month_file.Write(New_Hour_date + ",");
                        month_file.Write(Math.Round(Avg_WS, 3) + ",");
                        month_file.WriteLine(Math.Round(Mode_WD, 2));

                    }

                    file.Close();
                    month_file.Close();
                }
            }
            catch
            {
                MessageBox.Show("Error writing to file. Make sure it is not open in another program.");                
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called when date_Export_End is changed. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called when 'Update Plot' button is clicked. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void btnUpdate_Conc_Plot_Click(object sender, EventArgs e)
        {
            Update_plot();
            Update_Uncert_plot();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called when 'Run Uncertainty Analysis' button is clicked. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. MCP runs. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void btnMCP_Uncert_Click(object sender, EventArgs e)
        {

            Do_MCP_Uncertainty();                       

            Changes_Made();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Executes the MCP uncertainty analysis. </summary>
        ///
        /// <remarks>   Liz, 5/24/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Do_MCP_Uncertainty()
        {
            int Uncert_Step_Size = Get_Uncert_Step_Size(); // Step size (in months) that defines the next start date. 
                                                           // Default is 1 month but for large datasets, increasing this 
                                                           // helps to reduce the number of calculations. Possible choices are 1, 2, 3 or 4 month step size.

            // how many months in Conc -> Number of MCP_Uncert objects to create
            int Num_Obj = ((Conc_End.Year - Conc_Start.Year) * 12) + Conc_End.Month - Conc_Start.Month;

            string current_method = Get_MCP_Method();

            DateTime Test_Start = Conc_Start;
            DateTime Test_End = Conc_End;
            DateTime Orig_Start = Conc_Start;

            // Get sector count to be used within loops
            Find_Sector_Counts();

            // Find concurrent data to be referenced in Do_MCP function
            Find_Concurrent_Data(true, Conc_Start, Conc_End);
            
            // For every MCP_Uncert, for every possible conc window, construct Uncert structures
            if (current_method == "Orth. Regression")
            {
                Array.Resize(ref Uncert_Ortho, Num_Obj);

                for (int m = 0; m < Num_Obj; m++)
                {
                    Uncert_Ortho[m].WSize = m + 1;
                    Uncert_Ortho[m].NWindows = (Num_Obj - m) / Uncert_Step_Size;

                    Array.Resize(ref Uncert_Ortho[m].LT_Ests, Uncert_Ortho[m].NWindows);
                    Array.Resize(ref Uncert_Ortho[m].Rsq, Uncert_Ortho[m].NWindows);
                    Array.Resize(ref Uncert_Ortho[m].Start, Uncert_Ortho[m].NWindows);
                    Array.Resize(ref Uncert_Ortho[m].End, Uncert_Ortho[m].NWindows);

                    Test_Start = Orig_Start;

                    for (int i = 0; i < Uncert_Ortho[m].NWindows; i++)
                    {
                        // Initialize First Test Start at Concurrent Start Date at beginning of each iteration                        
                        Test_End = Test_Start.AddMonths(m + 1);

                        Uncert_Ortho[m].LT_Ests[i] = Do_MCP(Test_Start, Test_End, false, current_method);

                        float[] Ref_WS = Get_Conc_WS_Array("Reference", 0, 0, 0, 0, 30, true);
                        float[] Target_WS = Get_Conc_WS_Array("Target", 0, 0, 0, 0, 30, true);

                        Stats Stat = new Stats();
                        float var_x = Convert.ToSingle(Stat.Calc_Variance(Ref_WS));
                        float var_y = Convert.ToSingle(Stat.Calc_Variance(Target_WS));
                        float covar_xy = Convert.ToSingle(Stat.Calc_Covariance(Ref_WS, Target_WS));
                        
                        Uncert_Ortho[m].Rsq[i] = Stat.Calc_R_sqr(covar_xy, var_x, var_y);
                        Uncert_Ortho[m].Start[i] = Test_Start;
                        Uncert_Ortho[m].End[i] = Test_End;

                        // Increment start date by Uncert_Step_Size (default = 1 month but may be up to 4 months)
                        Test_Start = Test_Start.AddMonths(Uncert_Step_Size);
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
                    Uncert_Bins[m].NWindows = (Num_Obj - m) / Uncert_Step_Size;

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

                        Test_Start = Test_Start.AddMonths(Uncert_Step_Size);
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
                    Uncert_Varrat[m].NWindows = (Num_Obj - m) / Uncert_Step_Size;

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

                        Test_Start = Test_Start.AddMonths(Uncert_Step_Size);
                    }
                    // Find Statistics for analysis
                    Calc_Avg_SD_Uncert(ref Uncert_Varrat[m]);
                }
                btnMCP_Uncert.Enabled = false;
            }
            else if (current_method == "Matrix")
            {
                Array.Resize(ref Uncert_Matrix, Num_Obj);

                for (int m = 0; m < Num_Obj; m++)
                {
                    Uncert_Matrix[m].WSize = m + 1;
                    Uncert_Matrix[m].NWindows = (Num_Obj - m) / Uncert_Step_Size;

                    Array.Resize(ref Uncert_Matrix[m].LT_Ests, Uncert_Matrix[m].NWindows);
                    Array.Resize(ref Uncert_Matrix[m].Start, Uncert_Matrix[m].NWindows);
                    Array.Resize(ref Uncert_Matrix[m].End, Uncert_Matrix[m].NWindows);

                    Test_Start = Orig_Start;

                    for (int i = 0; i < Uncert_Matrix[m].NWindows; i++)
                    {
                        // Initialize First Test Start at Concurrent Start Date at beginning of each iteration
                        Test_End = Test_Start.AddMonths(m + 1);

                        Uncert_Matrix[m].LT_Ests[i] = Do_MCP(Test_Start, Test_End, false, current_method);
                        Uncert_Matrix[m].Start[i] = Test_Start;
                        Uncert_Matrix[m].End[i] = Test_End;

                        Test_Start = Test_Start.AddMonths(Uncert_Step_Size);
                    }
                    // Find Statistics for analysis
                    Calc_Avg_SD_Uncert(ref Uncert_Matrix[m]);
                }
                
            }

            
            Update_Uncert_plot();            
            Update_Uncert_List();
            Update_Run_Buttons();
            Update_Export_buttons();
                        
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Calculates the average and standard deviation of long-term estimates generated for an
        /// uncertainty object (i.e. certain window size, start and end dates)
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="This_Uncert">  [in,out] MCP Uncertainty object, MCP_Uncert. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Updates the Uncertainty analysis plot which shows the average and standard deviation of the
        /// LT Estimates using different monthly window sizes.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

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
            if (active_method == "Matrix" && Uncert_Matrix.Length > 0)
            {
                for (int u = 0; u < Uncert_Matrix.Length; u++)
                {
                    if (Uncert_Matrix[u].LT_Ests != null)
                    {
                        for (int i = 0; i < Uncert_Matrix[u].LT_Ests.Length; i++)
                            chtUncert.Series["LT Est. Data"].Points.AddXY(Uncert_Matrix[u].WSize, Uncert_Matrix[u].LT_Ests[i]);
                    }
                    // Assign LT Avg series = Avg of Uncert obj
                    if (Uncert_Matrix[u].avg != 0)
                    {
                        chtUncert.Series["LT Est. Avg"].Points.AddXY(Uncert_Matrix[u].WSize, Uncert_Matrix[u].avg);
                    }
                }
            }

            chtUncert.Series["LT Est. Avg"].MarkerColor = Color.Red;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called when 'Export Uncertainty Analysis' button is clicked.
        /// </summary>
        ///
        /// <remarks>   OEE, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

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
                file.WriteLine("Data binned into " + Get_Num_WD() + " WD bins; " + Get_Num_Hourly_Ints() + " Hourly bins; " + Get_Num_Temp_Ints() + " Temp bins");
                file.WriteLine("Start Time, End Time, Window Size, LT WS Est, LT Avg, Std Dev, R Sq");

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
                                file.Write(",");
                                file.Write(Math.Round(Uncert_Ortho[u].Rsq[i], 4));
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
                else if (current_method == "Matrix" && Uncert_Matrix.Length > 0)
                {
                    for (int u = 0; u < Uncert_Matrix.Length; u++)
                    {
                        // Assign LT Avg series = Avg of Uncert obj
                        if (Uncert_Matrix[u].avg != 0 && Uncert_Matrix[u].std_dev != 0)
                        {
                            for (int i = 0; i < Uncert_Matrix[u].LT_Ests.Length; i++)
                            {
                                file.Write(Uncert_Matrix[u].Start[i]);
                                file.Write(",");
                                file.Write(Uncert_Matrix[u].End[i]);
                                file.Write(",");
                                file.Write(Uncert_Matrix[u].WSize);
                                file.Write(",");
                                file.Write(Uncert_Matrix[u].LT_Ests[i]);
                                file.Write(",");
                                file.Write(Math.Round(Uncert_Matrix[u].avg, 3));
                                file.Write(",");
                                file.Write(Math.Round(Uncert_Matrix[u].std_dev, 4));
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called when 'Reset Dates' button is clicked. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void btnResetDates_Click(object sender, EventArgs e)
        {
            Reset_Export_Dates();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called when 'Reset Conc Dates' button is clicked. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void btnResetConcDates_Click(object sender, EventArgs e)
        {
            Set_Conc_Dates_On_Form();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called when File->New is selected from top menu. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            New_MCP(true, true);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Creates a new MCP analysis and sets all fields to default values. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. Tested outside of Visual Studio. </remarks>
        ///
        /// <param name="Clear_Ref">    True to clear reference. </param>
        /// <param name="Clear_Target"> True to clear target. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

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

            Num_WD_Sectors = 16;
            Num_Hourly_Ints = 1;
            Num_Temp_bins = 1;
            WS_bin_width = 1;
            Matrix_Wgt = 1;
            LastWS_Wgt = (float)0.5;

            Is_Newly_Opened_File = true;

            cboNumWD.SelectedIndex = 4;
            cboNumHours.SelectedIndex = 0;
            cboNumTemps.SelectedIndex = 0;

            txtWS_bin_width.Text = Convert.ToString(WS_bin_width);
            txtWS_PDF_Wgt.Text = Convert.ToString(Matrix_Wgt);
            txtLast_WS_Wgt.Text = Convert.ToString(LastWS_Wgt);                     

            MCP_Ortho.Clear();
            MCP_Bins.Clear();
            MCP_Varrat.Clear();
            MCP_Matrix.Clear();

            Uncert_Step_size = 1;
            cboUncertStep.SelectedIndex = 0;
            Uncert_Ortho = new MCP_Uncert[0];
            Uncert_Bins = new MCP_Uncert[0];
            Uncert_Varrat = new MCP_Uncert[0];
            Uncert_Matrix = new MCP_Uncert[0];

            Update_Run_Buttons();
            Update_plot();
            Update_Uncert_plot();
            Update_Text_boxes();
            Update_Bin_List();
            Update_Uncert_List();
            Update_Export_buttons();

            Saved_Filename = "";
            saveToolStripMenuItem.Enabled = false;
            Changes_Made();

            Is_Newly_Opened_File = false;   // this is set to true and then set to false to avoid messages that appear to ask the user 
                                            // if they are sure that they want to clear the calculations
            

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets data counts for every WD, hourly and temperature bin in reference dataset.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Find_Sector_Counts()
        {
            int Total_comb = Num_WD_Sectors * Num_Hourly_Ints * Num_Temp_bins;
            int counter = 0;

            Array.Resize(ref Sectors,Total_comb);

            for (int i = 0; i < Num_WD_Sectors; i++)
                for (int j = 0; j < Num_Hourly_Ints; j++)
                    for (int k = 0; k < Num_Temp_bins; k++)
                    {
                        Sectors[counter].WD = i;
                        Sectors[counter].Hour = j;
                        Sectors[counter].Temp = k;
                        Sectors[counter].Count = Stat.Get_Data_Count(Ref_Data, Export_Start, Export_End, i, j, k, this, false);
                        counter = counter + 1;
                    }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Sets default folder locations. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="Default_folder">   The default folder location. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Set_Default_Folder_locations(string Default_folder)
        {
            int Ind = Default_folder.LastIndexOf('\\');
            Default_folder = Default_folder.Substring(0, Ind+1);
            ofdOpenMCP.InitialDirectory = Default_folder;
            
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called when 'File->SaveAs' is clicked from top menu bar. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sfdSaveMCP.ShowDialog() == DialogResult.OK)
            {
                string Whole_Path = sfdSaveMCP.FileName;
                Set_Default_Folder_locations(Whole_Path);

                Save_File(Whole_Path);                
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves an MCP analysis file with .MCP extension. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. Tested outside of Visual Studio. </remarks>
        ///
        /// <param name="Whole_Path">   Full pathname of the file to be saved. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

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
            formatter.Serialize(fStream, Num_Hourly_Ints);
            formatter.Serialize(fStream, Num_Temp_bins);
            formatter.Serialize(fStream, WS_bin_width);
            formatter.Serialize(fStream, Matrix_Wgt);
            formatter.Serialize(fStream, LastWS_Wgt);                       

            formatter.Serialize(fStream, Min_Temp);
            formatter.Serialize(fStream, Max_Temp);
            formatter.Serialize(fStream, SD_WS_Lag);

            formatter.Serialize(fStream, MCP_Ortho);
            formatter.Serialize(fStream, MCP_Bins);
            formatter.Serialize(fStream, MCP_Varrat);
            formatter.Serialize(fStream, MCP_Matrix);                     

            formatter.Serialize(fStream, Uncert_Step_size);
            formatter.Serialize(fStream, Uncert_Ortho);
            formatter.Serialize(fStream, Uncert_Bins);
            formatter.Serialize(fStream, Uncert_Varrat);
            formatter.Serialize(fStream, Uncert_Matrix);

            fStream.Close();
            Saved_Filename = sfdSaveMCP.FileName;
            this.Text = Saved_Filename;
            this.saveToolStripMenuItem.Enabled = true;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds an asterisk to filename on top bar of form when any changes are made to the MCP analysis.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Changes_Made()
        {
            this.Text = Saved_Filename + "*";
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called when File->Open is selected from the top menu. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. Tested outside of Visual Studio. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

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
                    Num_Hourly_Ints = (int)formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Num_Temp_bins = (int)formatter.Deserialize(fstream);
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
                    Matrix_Wgt = (float)formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    LastWS_Wgt = (float)formatter.Deserialize(fstream);
                }
                catch
                { }                                              

                try
                {
                    Min_Temp = (float[,])formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Max_Temp = (float[,])formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    SD_WS_Lag = (float[])formatter.Deserialize(fstream);
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
                    MCP_Matrix = (Matrix_Obj)formatter.Deserialize(fstream);
                }
                catch
                { }

                try
                {
                    Uncert_Step_size = (int)formatter.Deserialize(fstream);
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

                try
                {
                    Uncert_Matrix = (MCP_Uncert[])formatter.Deserialize(fstream);
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

            for (int i = 0; i < cboNumHours.Items.Count; i++)
            {
                if (cboNumHours.Items[i].ToString() == Convert.ToString(Num_Hourly_Ints))
                {
                    cboNumHours.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < cboNumTemps.Items.Count; i++)
            {
                if (cboNumTemps.Items[i].ToString() == Convert.ToString(Num_Temp_bins))
                {
                    cboNumTemps.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < cboUncertStep.Items.Count; i++)
            {
                if (cboUncertStep.Items[i].ToString() == Convert.ToString(Uncert_Step_size))
                {
                    cboUncertStep.SelectedIndex = i;
                    break;
                }
            }

            txtWS_bin_width.Text = Convert.ToString(WS_bin_width);
            txtWS_PDF_Wgt.Text = Convert.ToString(Matrix_Wgt);
            txtLast_WS_Wgt.Text = Convert.ToString(LastWS_Wgt);
            
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called when File->Save is clicked from top menu bar. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. Tested outside of Visual Studio. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Saved_Filename != "")
                Save_File(Saved_Filename);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called when 'Num Hours Bin' dropdown menu selection is changed.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void cboNumHours_SelectedIndexChanged(object sender, EventArgs e)
        {
            // update WD sector drop-down
            if ((MCP_Ortho.Slope == null) && (MCP_Varrat.Slope == null) && (MCP_Bins.Bin_Avg_SD_Cnt == null) && (MCP_Matrix.LT_WS_Est == null) && (Uncert_Ortho.Length == 0)
                && (Uncert_Varrat.Length == 0) && (Uncert_Matrix.Length == 0) && (Uncert_Bins.Length == 0))
            {
                Num_Hourly_Ints = Convert.ToInt16(cboNumHours.SelectedItem.ToString());
                Find_Min_Max_temp();
                Update_Hourly_DropDown();
            }
            else if ((Is_Newly_Opened_File == false) && ((MCP_Ortho.Slope != null) || (MCP_Varrat.Slope != null) ||
                (MCP_Bins.Bin_Avg_SD_Cnt != null) || (MCP_Matrix.LT_WS_Est != null) || (Uncert_Ortho.Length > 0) ||
                (Uncert_Varrat.Length > 0) || (Uncert_Bins.Length > 0) || (Uncert_Matrix.Length > 0)))
            {
                bool show_msg = true;


                if (show_msg == true)
                {
                    string message = "Changing the number of hourly intervals will reset the MCP. Do you want to continue?";

                    DialogResult result = MessageBox.Show(message, "", MessageBoxButtons.YesNo);

                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        Reset_MCP("All");
                    }
                    else
                    {
                        cboNumHours.Text = Num_Hourly_Ints.ToString();
                    }
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called when 'Hour Interval' dropdown menu selection for plot is changed. If
        /// selected hourly interval is anything other than "All Hours" and selected WD sector is "All
        /// WD" or selected temp bin is "All temps" then it is set to first index since we do MCP for All
        /// Hours &amp; All WD &amp; All temp and then for each WD and each hourly interval and each temp
        /// bin.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void cboHourInt_SelectedIndexChanged(object sender, EventArgs e)
        {            
            if (cboHourInt.SelectedItem.ToString() != "All Hours")
            {
                if (cboWD_sector.SelectedItem.ToString() == "All WD")
                    cboWD_sector.SelectedIndex = 0;
                if (cboTemp_Int.SelectedItem.ToString() == "All Temps")
                    cboTemp_Int.SelectedIndex = 0;
            }

            if (cboHourInt.SelectedItem.ToString() == "All Hours")
            {
                if (cboWD_sector.SelectedItem.ToString() != "All WD")
                    cboWD_sector.SelectedIndex = cboWD_sector.Items.Count - 1;
                if (cboTemp_Int.SelectedItem.ToString() != "All Temps")
                    cboTemp_Int.SelectedIndex = cboTemp_Int.Items.Count - 1;

            }

            Update_plot();
            Update_Uncert_plot();
            Update_Text_boxes();
            Update_Bin_List();
            Update_Uncert_List();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called when 'Temp. Bin' dropdown menu selection for plot is changed. If
        /// selected temperature interval is anything other than "All Temps" and selected WD sector is
        /// "All WD" or selected hour bin is "All Hours" then it is set to first index.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void cboTemp_Int_SelectedIndexChanged(object sender, EventArgs e)
        {         
            if (cboTemp_Int.SelectedItem.ToString() != "All Temps")
            {
                if (cboHourInt.SelectedItem.ToString() == "All Hours")
                    cboHourInt.SelectedIndex = 0;
                if (cboWD_sector.SelectedItem.ToString() == "All WD")
                    cboWD_sector.SelectedIndex = 0;
            }

            if (cboTemp_Int.SelectedItem.ToString() == "All Temps")
            {
                if (cboHourInt.SelectedItem.ToString() != "All Hours")
                    cboHourInt.SelectedIndex = cboHourInt.Items.Count - 1;
                if (cboWD_sector.SelectedItem.ToString() != "All WD")
                    cboWD_sector.SelectedIndex = cboWD_sector.Items.Count - 1;

            }

            Update_plot();
            Update_Uncert_plot();
            Update_Text_boxes();
            Update_Bin_List();
            Update_Uncert_List();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called when dropdown 'Num. Temp. Bins' is changed. </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void cboNumTemps_SelectedIndexChanged(object sender, EventArgs e)
        {
            // update temperature interval drop-down
            if ((MCP_Ortho.Slope == null) && (MCP_Varrat.Slope == null) && (MCP_Bins.Bin_Avg_SD_Cnt == null) && (MCP_Matrix.LT_WS_Est == null) && (Uncert_Ortho.Length == 0)
                && (Uncert_Varrat.Length == 0) && (Uncert_Matrix.Length == 0) && (Uncert_Bins.Length == 0))
            {
                Num_Temp_bins = Convert.ToInt16(cboNumTemps.SelectedItem.ToString());                
                Update_Temp_Dropdown();
            }
            else if ((Is_Newly_Opened_File == false) && ((MCP_Ortho.Slope != null) || (MCP_Varrat.Slope != null) ||
                (MCP_Bins.Bin_Avg_SD_Cnt != null) || (MCP_Matrix.LT_WS_Est != null) || (Uncert_Ortho.Length > 0) ||
                (Uncert_Varrat.Length > 0) || (Uncert_Bins.Length > 0) || (Uncert_Matrix.Length > 0)))
            {
                bool show_msg = true;
                
                if (show_msg == true)
                {
                    string message = "Changing the number of temperature bins will reset the MCP. Do you want to continue?";
                    DialogResult result = MessageBox.Show(message, "", MessageBoxButtons.YesNo);

                    if (result == System.Windows.Forms.DialogResult.Yes)                    
                        Reset_MCP("All");                    
                    else                    
                        cboNumTemps.Text = Num_Hourly_Ints.ToString();                    
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called when button 'Export Annual TAB files' is clicked. Exports annual TAB
        /// files, with first start year = first year starting in Jan. File Name convention:
        /// MetName_Year.TAB.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. Tested outside of Visual Studio. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void btnExportAnnualTABs_Click(object sender, EventArgs e)
        {
            
            // find first year
            DateTime TAB_Start = Export_Start;

            while ((TAB_Start.Month != 1) && (TAB_Start.Day != 1))
                TAB_Start = TAB_Start.AddDays(1);            

            DateTime TAB_Last = Export_End;
            while ((TAB_Last.Month != 1) && (TAB_Last.Day != 31))
                TAB_Last = TAB_Last.AddDays(-1);

            string MetName = txtTargetName.Text;
            string folder = "";

            if (TABfolder.ShowDialog() == DialogResult.OK)
                folder = TABfolder.SelectedPath;

            for (int This_Start = TAB_Start.Year; This_Start <= TAB_Last.Year; This_Start++)
            {
                string Start_str = "1/1/" + This_Start;
                DateTime This_TAB_start = Convert.ToDateTime(Start_str);

                string End_str = "12/31/" + This_Start + " 11:00:00 PM";
                DateTime This_TAB_end = Convert.ToDateTime(End_str);
                
                if (This_TAB_end >= Ref_End)
                    break; // This_TAB_end is past the end of the reference data so can't create any more annual TAB files

                string TAB_name = MetName + "_" + This_Start + ".TAB";
                TAB_name = folder + "\\" + TAB_name;

                Create_TAB_file(TAB_name, This_TAB_start, This_TAB_end);

            }

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called when 'About MCP' is clicked from top menu bar. </summary>
        ///
        /// <remarks>   OEE, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void aboutMCPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MCP_Info MCP_About = new MCP_Info();
            MCP_About.ShowDialog();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called when 'Uncert. Window size (months)' dropdown menu selection is changed.
        /// This determines the step size used in the uncertainty analysis by default it is 1 month.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void cboUncertStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((Is_Newly_Opened_File == false) && ((Uncert_Ortho.Length > 0) ||
                (Uncert_Varrat.Length > 0) || (Uncert_Bins.Length > 0) || (Uncert_Matrix.Length > 0)))
            {
                bool show_msg = true;

                if (show_msg == true)
                {
                    string message = "Changing the uncertainty step size will reset all uncertainty calculations. Do you want to continue?";

                    DialogResult result = MessageBox.Show(message, "", MessageBoxButtons.YesNo);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        Uncert_Step_size = Convert.ToInt16(cboUncertStep.SelectedItem.ToString());
                        Reset_MCP("All");
                        
                    }
                    else
                        cboUncertStep.Text = Uncert_Step_size.ToString();
                }
            }
            else
                Uncert_Step_size = Convert.ToInt16(cboUncertStep.SelectedItem.ToString());
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called when 'WS PDF Weight' textbox is changed. This determines the weighting
        /// factor used in the Matrix-LastWS method where the Ref-Target WS Matrix and the ThisWS-LastWS
        /// Matrix are combined by a weighting factors.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void txtWS_PDF_Wgt_TextChanged(object sender, EventArgs e)
        {
            if ((Is_Newly_Opened_File == false) && (MCP_Matrix.LT_WS_Est != null || MCP_Bins.Bin_Avg_SD_Cnt != null || Uncert_Matrix.Length > 0 || Uncert_Bins.Length > 0))
            {
                bool show_msg = true;

                if (show_msg == true)
                {
                    string message = "Changing the WS matrix weight will reset the Matrix MCP. Do you want to continue?";

                    DialogResult result = MessageBox.Show(message, "", MessageBoxButtons.YesNo);

                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        Reset_MCP("Matrix");
                    }
                    else
                    {
                        txtWS_PDF_Wgt.Text = Math.Round(Matrix_Wgt, 1).ToString();
                    }
                }
            }
            else
                Matrix_Wgt = Convert.ToSingle(txtWS_PDF_Wgt.Text);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called when 'Last WS Weight' textbox is changed. This determines the weighting
        /// factor used in the Matrix-LastWS method where the Ref-Target WS Matrix and the ThisWS-LastWS
        /// Matrix are combined by a weighting factors.
        /// </summary>
        ///
        /// <remarks>   Liz, 5/16/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void txtLast_WS_Wgt_TextChanged(object sender, EventArgs e)
        {
            if ((Is_Newly_Opened_File == false) && (MCP_Matrix.LT_WS_Est != null || MCP_Bins.Bin_Avg_SD_Cnt != null || Uncert_Matrix.Length > 0 || Uncert_Bins.Length > 0))
            {
                bool show_msg = true;

                if (show_msg == true)
                {
                    string message = "Changing the Last WS matrix weight will reset the Matrix MCP. Do you want to continue?";

                    DialogResult result = MessageBox.Show(message, "", MessageBoxButtons.YesNo);

                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        Reset_MCP("Matrix");
                    }
                    else
                    {
                        txtLast_WS_Wgt.Text = Math.Round(LastWS_Wgt, 1).ToString();
                    }
                }
            }
            else
                LastWS_Wgt = Convert.ToSingle(txtLast_WS_Wgt.Text);
            
        }

        private void lstUncert_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}



