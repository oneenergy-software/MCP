using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCP
{
    
    public class Stats
    {
        
        public float Calc_Avg_WS(MCP_tool.Site_data[] Site, float Min_WS, float Max_WS, DateTime Start_time, DateTime End_time, float Min_WD, float Max_WD, 
            bool All_hours, int Hourly_index, MCP_tool This_MCP)
        {
            // Calculates and returns the average wind speed at Site for specified WS bounds, specified start/end time and WD bounds
            float Avg_WS = 0;
            int Avg_count = 0;

            foreach (MCP_tool.Site_data This_Site in Site)
                if (This_Site.This_Date >= Start_time && This_Site.This_Date <= End_time && This_Site.This_WS > Min_WS && This_Site.This_WS < Max_WS
                    && (All_hours = true || This_MCP.Get_Hourly_Index(This_Site.This_Date.Hour) == Hourly_index))
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
                        

            if (Avg_count > 0)
                Avg_WS = Avg_WS / Avg_count;

            return Avg_WS;
        }

        public int Get_Data_Count(MCP_tool.Site_data[] Site, DateTime Start_time, DateTime End_time, int WD_index, int Hourly_index, int Temp_index, MCP_tool This_MCP, bool Get_All)
        {
            // Returns the count of Site_data for specified start/end time, WD bounds, Hourly bin and Temp bin
           
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

        public double Calc_Variance(float[] vals)
        {
            // Calculates and returns the variance of vals[].
            // Used to calculate R^2 and in orthogonal regression and variance ratio

            double variance = 0;
            double sum_x = 0;
            double mean = 0;
            int val_length = vals.Length;

            if (vals != null)
            {
                foreach (double value in vals)
                {
                    sum_x = sum_x + value;
                }

                mean = sum_x / val_length;

                foreach (double value in vals)
                {
                    variance = variance + (Math.Pow(value - mean, 2) / (val_length));
                }

            }

            return variance;
        }

        public double Calc_Covariance(float[] x_vals, float[] y_vals)
        {
            // Calculates and returns the covariance between x_vals and y_vals.
            // Used in orthogonal regression

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
                    {
                        sum_x = sum_x + x_vals[i];
                    }

                    mean_x = sum_x / x_vals.Length;

                    for (int i = 0; i < x_vals.Length; i++)
                    {
                        sum_y = sum_y + y_vals[i];
                    }

                    mean_y = sum_y / y_vals.Length;

                    for (int i = 0; i < x_vals.Length; i++)
                    {
                        sum_XY = sum_XY + (x_vals[i] - mean_x) * (y_vals[i] - mean_y);
                    }

                    covar = sum_XY / x_vals.Length;
                }


            return covar;
        }

    }
}
