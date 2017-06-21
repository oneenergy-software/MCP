////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Stats.cs
//
// summary:	Implements the statistics class which calculates average, variance, covariance, etc.
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCP;
using System.Windows.Forms;

namespace MCP
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary> Statistics class used in MCP tool. </summary>
    ///
    /// <remarks>   Liz, 5/26/2017. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class Stats
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>  Calculates and returns the average wind speed at Site for specified WS bounds, 
        ///             start/end time, hourly bin and WD bounds</summary>
        ///
        /// <remarks>   Liz, 5/26/2017. </remarks>
        ///
        /// <param name="Site">         WS, WD and temperature time series data. </param>
        /// <param name="Min_WS">       Wind speed filter minimum. </param>
        /// <param name="Max_WS">       Wind speed filter maximum. </param>
        /// <param name="Start_time">   Start time. </param>
        /// <param name="End_time">     End time. </param>
        /// <param name="Min_WD">       Wind direction filter minimum. </param>
        /// <param name="Max_WD">       Wind direction filter maximum. </param>
        /// <param name="All_hours">    True if all hours are to be used in average WS calculation. </param>
        /// <param name="Hourly_index"> Zero-based index of the hour of day. </param>
        /// <param name="All_Temps">    True if all temperature bins are to be used in average WS calculation. </param>
        /// <param name="Temp_index">   Zero-based index of  hour of day. </param> 
        /// <param name="This_MCP">     MCP object </param>
        ///
        /// <returns>   The calculated average wind speed. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public float Calc_Avg_WS(MCP_tool.Site_data[] Site, float Min_WS, float Max_WS, DateTime Start_time, DateTime End_time, float Min_WD, float Max_WD, 
            bool All_hours, int Hourly_index, bool All_Temps, int Temp_index, MCP_tool This_MCP)
        {
             
            float Avg_WS = 0;
            int Avg_count = 0;
            int This_WD_ind = 0;
            int This_Hour_ind = 0;
            int This_Temp_ind = 0;

            foreach (MCP_tool.Site_data This_Site in Site)
            {
                This_WD_ind = This_MCP.Get_WD_ind(This_Site.This_WD, This_MCP.Get_Num_WD());
                This_Hour_ind = This_MCP.Get_Hourly_Index(This_Site.This_Date.Hour);
                This_Temp_ind = This_MCP.Get_Temp_ind(This_WD_ind, This_Hour_ind, This_Site.This_Temp);

                if (This_Site.This_Date >= Start_time && This_Site.This_Date <= End_time && This_Site.This_WS > Min_WS && This_Site.This_WS < Max_WS
                    && (All_hours == true || This_Hour_ind == Hourly_index) && (All_Temps == true || This_Temp_ind == Temp_index))
                {
                    if (Max_WD > Min_WD)
                    {
                        if (This_Site.This_WD >= Min_WD && This_Site.This_WD <= Max_WD)
                        {
                            Avg_WS = Avg_WS + This_Site.This_WS;
                            Avg_count = Avg_count + 1;
                        }
                    }
                    else if (This_Site.This_WD >= Min_WD || This_Site.This_WD <= Max_WD)
                    {
                        Avg_WS = Avg_WS + This_Site.This_WS;
                        Avg_count = Avg_count + 1;
                    }
                }
            }      

            if (Avg_count > 0)
                Avg_WS = Avg_WS / Avg_count;
                        
            return Avg_WS;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>  Returns the count of Site_data for specified start/end time, WD bounds, Hourly bin
        ///             and Temp bin. </summary>
        ///
        /// <remarks>  Liz, 5/26/2017. </remarks>
        ///
        /// <param name="Site">         Target or Reference Site_data. </param>
        /// <param name="Start_time">   Start time. </param>
        /// <param name="End_time">     End time. </param>
        /// <param name="WD_index">     Wind direction index. </param>
        /// <param name="Hourly_index"> Hourly bin. </param>
        /// <param name="Temp_index">   Temperature bin. </param>
        /// <param name="This_MCP">     MCP object. </param>
        /// <param name="Get_All">      True to get count of all data. </param>
        ///
        /// <returns>   The data count. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int Get_Data_Count(MCP_tool.Site_data[] Site, DateTime Start_time, DateTime End_time, int WD_index, int Hourly_index, int Temp_index, 
            MCP_tool This_MCP, bool Get_All)
        {            
           
            int Avg_count = 0;
            int This_WD_ind = 0;
            int This_Hour_ind = 0;
            int This_Temp_ind = 0;
                        
            foreach (MCP_tool.Site_data This_Site in Site)
                if (This_Site.This_Date >= Start_time && This_Site.This_Date <= End_time )
                {
                    if (Get_All == true)
                    {
                        Avg_count++;
                    }
                    else
                    {
                        This_WD_ind = This_MCP.Get_WD_ind(This_Site.This_WD, This_MCP.Get_Num_WD());
                        This_Hour_ind = This_MCP.Get_Hourly_Index(This_Site.This_Date.Hour);
                        This_Temp_ind = This_MCP.Get_Temp_ind(This_WD_ind, This_Hour_ind, This_Site.This_Temp);

                        if ((This_WD_ind == WD_index) && (This_Hour_ind == Hourly_index) && (This_Temp_ind == Temp_index))
                            Avg_count++;
                    }                                    
                }            

            return Avg_count;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Calculates and returns the variance of vals[]. Used to calculate R^2 and in 
        /// orthogonal regression and variance ratio </summary>
        ///
        /// <remarks>   Liz, 5/26/2017. </remarks>
        ///
        /// <param name="vals"> Array of float values. </param>
        ///
        /// <returns>   The calculated variance. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public double Calc_Variance(float[] vals)
        {           
            double variance = 0;
            double sum_x = 0;
            double mean = 0;
            int val_length = vals.Length;

            if (vals != null)
            {
                foreach (double value in vals)
                    sum_x = sum_x + value;
                

                mean = sum_x / val_length;

                foreach (double value in vals)
                    variance = variance + (Math.Pow(value - mean, 2) / (val_length));
                
            }

            return variance;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Calculates and returns the covariance between x_vals and y_vals. Used in orthogonal 
        ///             regression. </summary>
        ///
        /// <remarks>   Liz, 5/26/2017. </remarks>
        ///
        /// <param name="x_vals">   Array of float X values. </param>
        /// <param name="y_vals">   Array of float Y values. </param>
        ///
        /// <returns>   The calculated covariance. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public double Calc_Covariance(float[] x_vals, float[] y_vals)
        {            

            double covar = 0;
            double sum_XY = 0;
            double sum_x = 0;
            double sum_y = 0;
            double mean_x = 0;
            double mean_y = 0;

            if ((x_vals != null) && (y_vals != null))
                if (x_vals.Length == y_vals.Length)
                {
                    for (int i = 0; i < x_vals.Length; i++)                    
                        sum_x = sum_x + x_vals[i];                    

                    mean_x = sum_x / x_vals.Length;

                    for (int i = 0; i < x_vals.Length; i++)                    
                        sum_y = sum_y + y_vals[i];                    

                    mean_y = sum_y / y_vals.Length;

                    for (int i = 0; i < x_vals.Length; i++)                    
                        sum_XY = sum_XY + (x_vals[i] - mean_x) * (y_vals[i] - mean_y);                    

                    covar = sum_XY / x_vals.Length;
                }

            return covar;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Calculates the r sqr. </summary>
        ///
        /// <remarks>   OEE, 5/18/2017. </remarks>
        ///
        /// <param name="covar_xy"> The covar xy. </param>
        /// <param name="var_x">    The variable x coordinate. </param>
        /// <param name="var_y">    The variable y coordinate. </param>
        ///
        /// <returns>   The calculated r sqr. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public float Calc_R_sqr(float covar_xy, float var_x, float var_y)
        {
            float R_sqr = (float)Math.Pow(covar_xy / (float)Math.Pow(var_x, 0.5) / (float)Math.Pow(var_y, 0.5), 2);
            return R_sqr;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Calculates the intercept. </summary>
        ///
        /// <remarks>   OEE, 5/18/2017. </remarks>
        ///
        /// <param name="Avg_Y">    The average y coordinate. </param>
        /// <param name="slope">    The slope. </param>
        /// <param name="Avg_X">    The average x coordinate. </param>
        ///
        /// <returns>   The calculated intercept. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public float Calc_Intercept(float Avg_Y, float slope, float Avg_X)
        {
            float Intercept = Avg_Y - slope * Avg_X;
            return Intercept;
        }
}
}
