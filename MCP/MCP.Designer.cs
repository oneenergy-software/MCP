namespace MCP
{
    partial class MCP_tool
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MCP_tool));
            this.label1 = new System.Windows.Forms.Label();
            this.btnImportRef = new System.Windows.Forms.Button();
            this.btnImportTarget = new System.Windows.Forms.Button();
            this.btnRunMCP = new System.Windows.Forms.Button();
            this.btnExportTS = new System.Windows.Forms.Button();
            this.btnExportTAB = new System.Windows.Forms.Button();
            this.cboMCP_Type = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboNumWD = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtRefAvgWS = new System.Windows.Forms.TextBox();
            this.txtTargAvgWS = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtWS_bin_width = new System.Windows.Forms.TextBox();
            this.cboWD_sector = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.chtScatter = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRef_LT_WS = new System.Windows.Forms.TextBox();
            this.txtTarg_LT_WS = new System.Windows.Forms.TextBox();
            this.ofdRefSite = new System.Windows.Forms.OpenFileDialog();
            this.btnClearRef = new System.Windows.Forms.Button();
            this.date_Corr_Start = new System.Windows.Forms.DateTimePicker();
            this.label10 = new System.Windows.Forms.Label();
            this.date_Corr_End = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.btnClearTarget = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.date_Export_End = new System.Windows.Forms.DateTimePicker();
            this.label13 = new System.Windows.Forms.Label();
            this.date_Export_Start = new System.Windows.Forms.DateTimePicker();
            this.txtDataCount = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtAvgRatio = new System.Windows.Forms.TextBox();
            this.txtLTratio = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.txtNumYrsRef = new System.Windows.Forms.TextBox();
            this.txtNumYrsTarg = new System.Windows.Forms.TextBox();
            this.txtNumYrsConc = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.lstBins = new System.Windows.Forms.ListView();
            this.WS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Mean = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SD = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Count = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.txtOSlope = new System.Windows.Forms.TextBox();
            this.txtOIntercept = new System.Windows.Forms.TextBox();
            this.txtORsq = new System.Windows.Forms.TextBox();
            this.sfdSaveTimeSeries = new System.Windows.Forms.SaveFileDialog();
            this.sfdSaveTAB = new System.Windows.Forms.SaveFileDialog();
            this.btnExportBinRatios = new System.Windows.Forms.Button();
            this.btnConvertToHourly = new System.Windows.Forms.Button();
            this.btnConvertMonthly = new System.Windows.Forms.Button();
            this.txtLoadedReference = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.txtLoadedTarget = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.btnUpdate_Conc_Plot = new System.Windows.Forms.Button();
            this.txtVSlope = new System.Windows.Forms.TextBox();
            this.txtVIntercept = new System.Windows.Forms.TextBox();
            this.txtVRsq = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.btnExportMultitest = new System.Windows.Forms.Button();
            this.btnMCP_Uncert = new System.Windows.Forms.Button();
            this.chtUncert = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label34 = new System.Windows.Forms.Label();
            this.lstUncert = new System.Windows.Forms.ListView();
            this.Window = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AVG = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SDU = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnResetDates = new System.Windows.Forms.Button();
            this.btnResetConcDates = new System.Windows.Forms.Button();
            this.sfdSaveMCP = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMCPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ofdOpenMCP = new System.Windows.Forms.OpenFileDialog();
            this.label35 = new System.Windows.Forms.Label();
            this.cboNumHours = new System.Windows.Forms.ComboBox();
            this.label36 = new System.Windows.Forms.Label();
            this.cboHourInt = new System.Windows.Forms.ComboBox();
            this.label39 = new System.Windows.Forms.Label();
            this.cboNumTemps = new System.Windows.Forms.ComboBox();
            this.label37 = new System.Windows.Forms.Label();
            this.cboTemp_Int = new System.Windows.Forms.ComboBox();
            this.txtWS_PDF_Wgt = new System.Windows.Forms.TextBox();
            this.label38 = new System.Windows.Forms.Label();
            this.txtLast_WS_Wgt = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.txtTargetName = new System.Windows.Forms.TextBox();
            this.txtUTMX = new System.Windows.Forms.TextBox();
            this.label41 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.txtUTMY = new System.Windows.Forms.TextBox();
            this.label43 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.btnExportAnnualTABs = new System.Windows.Forms.Button();
            this.cboTAB_bins = new System.Windows.Forms.ComboBox();
            this.label45 = new System.Windows.Forms.Label();
            this.TABfolder = new System.Windows.Forms.FolderBrowserDialog();
            this.label46 = new System.Windows.Forms.Label();
            this.cboUncertStep = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.chtScatter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chtUncert)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(469, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Measure-Correlate-Predict (MCP) analyzer";
            // 
            // btnImportRef
            // 
            this.btnImportRef.Location = new System.Drawing.Point(9, 110);
            this.btnImportRef.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnImportRef.Name = "btnImportRef";
            this.btnImportRef.Size = new System.Drawing.Size(178, 60);
            this.btnImportRef.TabIndex = 1;
            this.btnImportRef.Text = "Import Hourly Ref. site data\r\n(Date, WS, WD, Temp)";
            this.btnImportRef.UseVisualStyleBackColor = true;
            this.btnImportRef.Click += new System.EventHandler(this.btnImportRef_Click);
            // 
            // btnImportTarget
            // 
            this.btnImportTarget.Location = new System.Drawing.Point(194, 110);
            this.btnImportTarget.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnImportTarget.Name = "btnImportTarget";
            this.btnImportTarget.Size = new System.Drawing.Size(151, 60);
            this.btnImportTarget.TabIndex = 2;
            this.btnImportTarget.Text = "Import Hourly Target site data\r\n(Date, WS, WD)";
            this.btnImportTarget.UseVisualStyleBackColor = true;
            this.btnImportTarget.Click += new System.EventHandler(this.btnImportTarget_Click);
            // 
            // btnRunMCP
            // 
            this.btnRunMCP.Location = new System.Drawing.Point(248, 286);
            this.btnRunMCP.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRunMCP.Name = "btnRunMCP";
            this.btnRunMCP.Size = new System.Drawing.Size(97, 44);
            this.btnRunMCP.TabIndex = 3;
            this.btnRunMCP.Text = "Run MCP";
            this.btnRunMCP.UseVisualStyleBackColor = true;
            this.btnRunMCP.Click += new System.EventHandler(this.btnRunMCP_Click);
            // 
            // btnExportTS
            // 
            this.btnExportTS.Location = new System.Drawing.Point(11, 614);
            this.btnExportTS.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExportTS.Name = "btnExportTS";
            this.btnExportTS.Size = new System.Drawing.Size(129, 58);
            this.btnExportTS.TabIndex = 4;
            this.btnExportTS.Text = "Export Estimated data as Time Series";
            this.btnExportTS.UseVisualStyleBackColor = true;
            this.btnExportTS.Click += new System.EventHandler(this.btnExportTS_Click);
            // 
            // btnExportTAB
            // 
            this.btnExportTAB.Location = new System.Drawing.Point(11, 678);
            this.btnExportTAB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExportTAB.Name = "btnExportTAB";
            this.btnExportTAB.Size = new System.Drawing.Size(129, 48);
            this.btnExportTAB.TabIndex = 5;
            this.btnExportTAB.Text = "Export Estimated data as TAB file\r\n";
            this.btnExportTAB.UseVisualStyleBackColor = true;
            this.btnExportTAB.Click += new System.EventHandler(this.btnExportTAB_Click);
            // 
            // cboMCP_Type
            // 
            this.cboMCP_Type.FormattingEnabled = true;
            this.cboMCP_Type.Items.AddRange(new object[] {
            "Orth. Regression",
            "Method of Bins",
            "Variance Ratio",
            "Matrix"});
            this.cboMCP_Type.Location = new System.Drawing.Point(20, 262);
            this.cboMCP_Type.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboMCP_Type.Name = "cboMCP_Type";
            this.cboMCP_Type.Size = new System.Drawing.Size(205, 24);
            this.cboMCP_Type.TabIndex = 6;
            this.cboMCP_Type.SelectedIndexChanged += new System.EventHandler(this.cboMCP_Type_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(21, 232);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(256, 24);
            this.label2.TabIndex = 7;
            this.label2.Text = "2) Select MCP Method && Run";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(18, 300);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Num. WD bins :";
            // 
            // cboNumWD
            // 
            this.cboNumWD.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboNumWD.FormattingEnabled = true;
            this.cboNumWD.Items.AddRange(new object[] {
            "1",
            "4",
            "8",
            "12",
            "16"});
            this.cboNumWD.Location = new System.Drawing.Point(171, 294);
            this.cboNumWD.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboNumWD.Name = "cboNumWD";
            this.cboNumWD.Size = new System.Drawing.Size(55, 28);
            this.cboNumWD.TabIndex = 9;
            this.cboNumWD.Text = "1";
            this.cboNumWD.SelectedIndexChanged += new System.EventHandler(this.cboNumWD_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(864, 43);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "Ref. Site";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(949, 43);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 20);
            this.label6.TabIndex = 12;
            this.label6.Text = "Target Site";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(672, 66);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(166, 20);
            this.label7.TabIndex = 13;
            this.label7.Text = "Avg. Conc. WS (m/s)";
            // 
            // txtRefAvgWS
            // 
            this.txtRefAvgWS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRefAvgWS.Location = new System.Drawing.Point(864, 66);
            this.txtRefAvgWS.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRefAvgWS.Name = "txtRefAvgWS";
            this.txtRefAvgWS.ReadOnly = true;
            this.txtRefAvgWS.Size = new System.Drawing.Size(80, 24);
            this.txtRefAvgWS.TabIndex = 14;
            // 
            // txtTargAvgWS
            // 
            this.txtTargAvgWS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTargAvgWS.Location = new System.Drawing.Point(960, 66);
            this.txtTargAvgWS.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTargAvgWS.Name = "txtTargAvgWS";
            this.txtTargAvgWS.ReadOnly = true;
            this.txtTargAvgWS.Size = new System.Drawing.Size(80, 24);
            this.txtTargAvgWS.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(18, 394);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(162, 20);
            this.label8.TabIndex = 16;
            this.label8.Text = "WS bin width (m/s) :";
            // 
            // txtWS_bin_width
            // 
            this.txtWS_bin_width.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWS_bin_width.Location = new System.Drawing.Point(185, 390);
            this.txtWS_bin_width.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtWS_bin_width.Name = "txtWS_bin_width";
            this.txtWS_bin_width.Size = new System.Drawing.Size(40, 26);
            this.txtWS_bin_width.TabIndex = 17;
            this.txtWS_bin_width.Text = "1";
            this.txtWS_bin_width.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtWS_bin_width.TextChanged += new System.EventHandler(this.txtWS_bin_width_TextChanged);
            // 
            // cboWD_sector
            // 
            this.cboWD_sector.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboWD_sector.FormattingEnabled = true;
            this.cboWD_sector.Items.AddRange(new object[] {
            "All WD"});
            this.cboWD_sector.Location = new System.Drawing.Point(1106, 128);
            this.cboWD_sector.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboWD_sector.Name = "cboWD_sector";
            this.cboWD_sector.Size = new System.Drawing.Size(91, 26);
            this.cboWD_sector.TabIndex = 18;
            this.cboWD_sector.Text = "All WD";
            this.cboWD_sector.SelectedIndexChanged += new System.EventHandler(this.cboWD_sector_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(993, 130);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(102, 20);
            this.label9.TabIndex = 19;
            this.label9.Text = "WD Sector :";
            // 
            // chtScatter
            // 
            chartArea1.AxisX.Title = "Reference WS, m/s";
            chartArea1.AxisY.Title = "Target WS, m/s";
            chartArea1.Name = "ChartArea1";
            this.chtScatter.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.chtScatter.Legends.Add(legend1);
            this.chtScatter.Location = new System.Drawing.Point(596, 160);
            this.chtScatter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chtScatter.Name = "chtScatter";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chtScatter.Series.Add(series1);
            this.chtScatter.Size = new System.Drawing.Size(860, 302);
            this.chtScatter.TabIndex = 20;
            this.chtScatter.Text = "chtWS_scatter";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(697, 98);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 20);
            this.label4.TabIndex = 21;
            this.label4.Text = "Avg. LT WS (m/s)";
            // 
            // txtRef_LT_WS
            // 
            this.txtRef_LT_WS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRef_LT_WS.Location = new System.Drawing.Point(864, 96);
            this.txtRef_LT_WS.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRef_LT_WS.Name = "txtRef_LT_WS";
            this.txtRef_LT_WS.ReadOnly = true;
            this.txtRef_LT_WS.Size = new System.Drawing.Size(80, 24);
            this.txtRef_LT_WS.TabIndex = 22;
            // 
            // txtTarg_LT_WS
            // 
            this.txtTarg_LT_WS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTarg_LT_WS.Location = new System.Drawing.Point(959, 96);
            this.txtTarg_LT_WS.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTarg_LT_WS.Name = "txtTarg_LT_WS";
            this.txtTarg_LT_WS.ReadOnly = true;
            this.txtTarg_LT_WS.Size = new System.Drawing.Size(80, 24);
            this.txtTarg_LT_WS.TabIndex = 23;
            // 
            // ofdRefSite
            // 
            this.ofdRefSite.Filter = "CSV|*.csv";
            // 
            // btnClearRef
            // 
            this.btnClearRef.Location = new System.Drawing.Point(8, 178);
            this.btnClearRef.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClearRef.Name = "btnClearRef";
            this.btnClearRef.Size = new System.Drawing.Size(179, 47);
            this.btnClearRef.TabIndex = 24;
            this.btnClearRef.Text = "Clear Reference data";
            this.btnClearRef.UseVisualStyleBackColor = true;
            this.btnClearRef.Click += new System.EventHandler(this.btnClearRef_Click);
            // 
            // date_Corr_Start
            // 
            this.date_Corr_Start.CustomFormat = "MM/dd/yyyy hh:mm";
            this.date_Corr_Start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.date_Corr_Start.Location = new System.Drawing.Point(148, 508);
            this.date_Corr_Start.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.date_Corr_Start.Name = "date_Corr_Start";
            this.date_Corr_Start.Size = new System.Drawing.Size(174, 22);
            this.date_Corr_Start.TabIndex = 25;
            this.date_Corr_Start.ValueChanged += new System.EventHandler(this.date_Corr_Start_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(167, 488);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(104, 18);
            this.label10.TabIndex = 26;
            this.label10.Text = "Correlate Start";
            // 
            // date_Corr_End
            // 
            this.date_Corr_End.CustomFormat = "MM/dd/yyyy hh:mm";
            this.date_Corr_End.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.date_Corr_End.Location = new System.Drawing.Point(148, 554);
            this.date_Corr_End.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.date_Corr_End.Name = "date_Corr_End";
            this.date_Corr_End.Size = new System.Drawing.Size(174, 22);
            this.date_Corr_End.TabIndex = 27;
            this.date_Corr_End.ValueChanged += new System.EventHandler(this.date_Corr_End_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(167, 535);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(99, 18);
            this.label11.TabIndex = 28;
            this.label11.Text = "Correlate End";
            // 
            // btnClearTarget
            // 
            this.btnClearTarget.Location = new System.Drawing.Point(194, 178);
            this.btnClearTarget.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClearTarget.Name = "btnClearTarget";
            this.btnClearTarget.Size = new System.Drawing.Size(151, 47);
            this.btnClearTarget.TabIndex = 29;
            this.btnClearTarget.Text = "Clear Target data";
            this.btnClearTarget.UseVisualStyleBackColor = true;
            this.btnClearTarget.Click += new System.EventHandler(this.btnClearTarget_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(182, 806);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(81, 18);
            this.label12.TabIndex = 33;
            this.label12.Text = "Export End";
            // 
            // date_Export_End
            // 
            this.date_Export_End.CustomFormat = "MM/dd/yyyy hh:mm  ";
            this.date_Export_End.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.date_Export_End.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.date_Export_End.Location = new System.Drawing.Point(159, 827);
            this.date_Export_End.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.date_Export_End.Name = "date_Export_End";
            this.date_Export_End.Size = new System.Drawing.Size(176, 23);
            this.date_Export_End.TabIndex = 32;
            this.date_Export_End.ValueChanged += new System.EventHandler(this.date_Export_End_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(182, 754);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(86, 18);
            this.label13.TabIndex = 31;
            this.label13.Text = "Export Start";
            // 
            // date_Export_Start
            // 
            this.date_Export_Start.CustomFormat = "MM/dd/yyyy hh:mm";
            this.date_Export_Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.date_Export_Start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.date_Export_Start.Location = new System.Drawing.Point(159, 776);
            this.date_Export_Start.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.date_Export_Start.Name = "date_Export_Start";
            this.date_Export_Start.Size = new System.Drawing.Size(176, 23);
            this.date_Export_Start.TabIndex = 30;
            this.date_Export_Start.ValueChanged += new System.EventHandler(this.date_Export_Start_ValueChanged);
            // 
            // txtDataCount
            // 
            this.txtDataCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDataCount.Location = new System.Drawing.Point(1153, 65);
            this.txtDataCount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDataCount.Name = "txtDataCount";
            this.txtDataCount.ReadOnly = true;
            this.txtDataCount.Size = new System.Drawing.Size(76, 24);
            this.txtDataCount.TabIndex = 34;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(1161, 41);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 20);
            this.label14.TabIndex = 35;
            this.label14.Text = "Count";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(1051, 43);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(85, 20);
            this.label15.TabIndex = 36;
            this.label15.Text = "Ratio: T/R\r\n";
            // 
            // txtAvgRatio
            // 
            this.txtAvgRatio.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAvgRatio.Location = new System.Drawing.Point(1056, 65);
            this.txtAvgRatio.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtAvgRatio.Name = "txtAvgRatio";
            this.txtAvgRatio.ReadOnly = true;
            this.txtAvgRatio.Size = new System.Drawing.Size(80, 24);
            this.txtAvgRatio.TabIndex = 37;
            // 
            // txtLTratio
            // 
            this.txtLTratio.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLTratio.Location = new System.Drawing.Point(1057, 95);
            this.txtLTratio.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLTratio.Name = "txtLTratio";
            this.txtLTratio.ReadOnly = true;
            this.txtLTratio.Size = new System.Drawing.Size(80, 24);
            this.txtLTratio.TabIndex = 38;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(10, 72);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(331, 24);
            this.label16.TabIndex = 39;
            this.label16.Text = "1) Import Reference && Target Site Data";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(14, 579);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(290, 24);
            this.label17.TabIndex = 40;
            this.label17.Text = "3) Export Estimates at Target Site ";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(370, 323);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(70, 18);
            this.label18.TabIndex = 41;
            this.label18.Text = "Num. Yrs";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(371, 345);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(39, 18);
            this.label19.TabIndex = 42;
            this.label19.Text = "Ref. ";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(427, 345);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(42, 18);
            this.label20.TabIndex = 43;
            this.label20.Text = "Targ.";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(477, 345);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(48, 18);
            this.label21.TabIndex = 44;
            this.label21.Text = "Conc.";
            // 
            // txtNumYrsRef
            // 
            this.txtNumYrsRef.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumYrsRef.Location = new System.Drawing.Point(373, 363);
            this.txtNumYrsRef.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNumYrsRef.Name = "txtNumYrsRef";
            this.txtNumYrsRef.ReadOnly = true;
            this.txtNumYrsRef.Size = new System.Drawing.Size(43, 23);
            this.txtNumYrsRef.TabIndex = 45;
            // 
            // txtNumYrsTarg
            // 
            this.txtNumYrsTarg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumYrsTarg.Location = new System.Drawing.Point(426, 363);
            this.txtNumYrsTarg.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNumYrsTarg.Name = "txtNumYrsTarg";
            this.txtNumYrsTarg.ReadOnly = true;
            this.txtNumYrsTarg.Size = new System.Drawing.Size(43, 23);
            this.txtNumYrsTarg.TabIndex = 46;
            // 
            // txtNumYrsConc
            // 
            this.txtNumYrsConc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumYrsConc.Location = new System.Drawing.Point(477, 363);
            this.txtNumYrsConc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNumYrsConc.Name = "txtNumYrsConc";
            this.txtNumYrsConc.ReadOnly = true;
            this.txtNumYrsConc.Size = new System.Drawing.Size(43, 23);
            this.txtNumYrsConc.TabIndex = 47;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(719, 11);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(404, 24);
            this.label22.TabIndex = 48;
            this.label22.Text = "Mean Concurrent and Long-term Wind Speeds";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(366, 159);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(191, 24);
            this.label23.TabIndex = 49;
            this.label23.Text = "MCP Orth. Reg. Stats.";
            // 
            // lstBins
            // 
            this.lstBins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.WS,
            this.Mean,
            this.SD,
            this.Count});
            this.lstBins.Location = new System.Drawing.Point(362, 423);
            this.lstBins.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lstBins.Name = "lstBins";
            this.lstBins.Size = new System.Drawing.Size(224, 354);
            this.lstBins.TabIndex = 50;
            this.lstBins.UseCompatibleStateImageBehavior = false;
            this.lstBins.View = System.Windows.Forms.View.Details;
            // 
            // WS
            // 
            this.WS.Text = "WS";
            this.WS.Width = 44;
            // 
            // Mean
            // 
            this.Mean.Text = "Mean";
            this.Mean.Width = 50;
            // 
            // SD
            // 
            this.SD.Text = "SD";
            this.SD.Width = 44;
            // 
            // Count
            // 
            this.Count.Text = "Count";
            this.Count.Width = 66;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(363, 398);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(211, 20);
            this.label24.TabIndex = 51;
            this.label24.Text = "Avg && SD WS Ratios (T/R)";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(366, 186);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(50, 18);
            this.label25.TabIndex = 52;
            this.label25.Text = "Slope:";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(439, 188);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(68, 18);
            this.label26.TabIndex = 53;
            this.label26.Text = "Intercept:";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.Location = new System.Drawing.Point(521, 190);
            this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(32, 18);
            this.label27.TabIndex = 54;
            this.label27.Text = "R² :";
            // 
            // txtOSlope
            // 
            this.txtOSlope.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOSlope.Location = new System.Drawing.Point(370, 210);
            this.txtOSlope.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtOSlope.Name = "txtOSlope";
            this.txtOSlope.ReadOnly = true;
            this.txtOSlope.Size = new System.Drawing.Size(61, 23);
            this.txtOSlope.TabIndex = 55;
            // 
            // txtOIntercept
            // 
            this.txtOIntercept.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOIntercept.Location = new System.Drawing.Point(441, 210);
            this.txtOIntercept.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtOIntercept.Name = "txtOIntercept";
            this.txtOIntercept.ReadOnly = true;
            this.txtOIntercept.Size = new System.Drawing.Size(61, 23);
            this.txtOIntercept.TabIndex = 56;
            // 
            // txtORsq
            // 
            this.txtORsq.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtORsq.Location = new System.Drawing.Point(512, 208);
            this.txtORsq.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtORsq.Name = "txtORsq";
            this.txtORsq.ReadOnly = true;
            this.txtORsq.Size = new System.Drawing.Size(61, 23);
            this.txtORsq.TabIndex = 57;
            // 
            // sfdSaveTimeSeries
            // 
            this.sfdSaveTimeSeries.Filter = "CSV files|*.csv";
            // 
            // sfdSaveTAB
            // 
            this.sfdSaveTAB.Filter = "TAB files|*.TAB";
            // 
            // btnExportBinRatios
            // 
            this.btnExportBinRatios.Location = new System.Drawing.Point(413, 786);
            this.btnExportBinRatios.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExportBinRatios.Name = "btnExportBinRatios";
            this.btnExportBinRatios.Size = new System.Drawing.Size(130, 58);
            this.btnExportBinRatios.TabIndex = 58;
            this.btnExportBinRatios.Text = "Export WS Bin Ratios";
            this.btnExportBinRatios.UseVisualStyleBackColor = true;
            this.btnExportBinRatios.Click += new System.EventHandler(this.btnExportBinRatios_Click);
            // 
            // btnConvertToHourly
            // 
            this.btnConvertToHourly.Location = new System.Drawing.Point(380, 74);
            this.btnConvertToHourly.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnConvertToHourly.Name = "btnConvertToHourly";
            this.btnConvertToHourly.Size = new System.Drawing.Size(184, 31);
            this.btnConvertToHourly.TabIndex = 59;
            this.btnConvertToHourly.Text = "Convert to Hourly";
            this.btnConvertToHourly.UseVisualStyleBackColor = true;
            this.btnConvertToHourly.Click += new System.EventHandler(this.btnConvertToHourly_Click);
            // 
            // btnConvertMonthly
            // 
            this.btnConvertMonthly.Location = new System.Drawing.Point(380, 117);
            this.btnConvertMonthly.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnConvertMonthly.Name = "btnConvertMonthly";
            this.btnConvertMonthly.Size = new System.Drawing.Size(184, 31);
            this.btnConvertMonthly.TabIndex = 60;
            this.btnConvertMonthly.Text = "Convert to Monthly";
            this.btnConvertMonthly.UseVisualStyleBackColor = true;
            this.btnConvertMonthly.Click += new System.EventHandler(this.btnConvertMonthly_Click);
            // 
            // txtLoadedReference
            // 
            this.txtLoadedReference.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoadedReference.Location = new System.Drawing.Point(768, 774);
            this.txtLoadedReference.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLoadedReference.Name = "txtLoadedReference";
            this.txtLoadedReference.ReadOnly = true;
            this.txtLoadedReference.Size = new System.Drawing.Size(690, 24);
            this.txtLoadedReference.TabIndex = 61;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.Location = new System.Drawing.Point(595, 777);
            this.label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(148, 18);
            this.label28.TabIndex = 62;
            this.label28.Text = "Reference Filename :";
            // 
            // txtLoadedTarget
            // 
            this.txtLoadedTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoadedTarget.Location = new System.Drawing.Point(768, 802);
            this.txtLoadedTarget.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLoadedTarget.Name = "txtLoadedTarget";
            this.txtLoadedTarget.ReadOnly = true;
            this.txtLoadedTarget.Size = new System.Drawing.Size(690, 24);
            this.txtLoadedTarget.TabIndex = 63;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(592, 806);
            this.label29.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(151, 18);
            this.label29.TabIndex = 64;
            this.label29.Text = "Target Site Filename :";
            // 
            // btnUpdate_Conc_Plot
            // 
            this.btnUpdate_Conc_Plot.Location = new System.Drawing.Point(596, 127);
            this.btnUpdate_Conc_Plot.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUpdate_Conc_Plot.Name = "btnUpdate_Conc_Plot";
            this.btnUpdate_Conc_Plot.Size = new System.Drawing.Size(109, 30);
            this.btnUpdate_Conc_Plot.TabIndex = 65;
            this.btnUpdate_Conc_Plot.Text = "Update Plot";
            this.btnUpdate_Conc_Plot.UseVisualStyleBackColor = true;
            this.btnUpdate_Conc_Plot.Click += new System.EventHandler(this.btnUpdate_Conc_Plot_Click);
            // 
            // txtVSlope
            // 
            this.txtVSlope.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVSlope.Location = new System.Drawing.Point(370, 292);
            this.txtVSlope.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtVSlope.Name = "txtVSlope";
            this.txtVSlope.ReadOnly = true;
            this.txtVSlope.Size = new System.Drawing.Size(61, 23);
            this.txtVSlope.TabIndex = 55;
            // 
            // txtVIntercept
            // 
            this.txtVIntercept.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVIntercept.Location = new System.Drawing.Point(441, 292);
            this.txtVIntercept.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtVIntercept.Name = "txtVIntercept";
            this.txtVIntercept.ReadOnly = true;
            this.txtVIntercept.Size = new System.Drawing.Size(61, 23);
            this.txtVIntercept.TabIndex = 56;
            // 
            // txtVRsq
            // 
            this.txtVRsq.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVRsq.Location = new System.Drawing.Point(512, 292);
            this.txtVRsq.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtVRsq.Name = "txtVRsq";
            this.txtVRsq.ReadOnly = true;
            this.txtVRsq.Size = new System.Drawing.Size(61, 23);
            this.txtVRsq.TabIndex = 57;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(523, 270);
            this.label30.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(32, 18);
            this.label30.TabIndex = 69;
            this.label30.Text = "R² :";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.Location = new System.Drawing.Point(441, 273);
            this.label31.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(68, 18);
            this.label31.TabIndex = 68;
            this.label31.Text = "Intercept:";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.Location = new System.Drawing.Point(367, 271);
            this.label32.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(50, 18);
            this.label32.TabIndex = 67;
            this.label32.Text = "Slope:";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(367, 242);
            this.label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(176, 24);
            this.label33.TabIndex = 66;
            this.label33.Text = "MCP Var. Rat. Stats.";
            // 
            // btnExportMultitest
            // 
            this.btnExportMultitest.Location = new System.Drawing.Point(8, 793);
            this.btnExportMultitest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExportMultitest.Name = "btnExportMultitest";
            this.btnExportMultitest.Size = new System.Drawing.Size(129, 58);
            this.btnExportMultitest.TabIndex = 73;
            this.btnExportMultitest.Text = "Export Uncertainty Analysis\r\n\r\n";
            this.btnExportMultitest.UseVisualStyleBackColor = true;
            this.btnExportMultitest.Click += new System.EventHandler(this.btnExportMultitest_Click);
            // 
            // btnMCP_Uncert
            // 
            this.btnMCP_Uncert.Location = new System.Drawing.Point(248, 345);
            this.btnMCP_Uncert.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnMCP_Uncert.Name = "btnMCP_Uncert";
            this.btnMCP_Uncert.Size = new System.Drawing.Size(97, 47);
            this.btnMCP_Uncert.TabIndex = 74;
            this.btnMCP_Uncert.Text = "Run Uncert";
            this.btnMCP_Uncert.UseVisualStyleBackColor = true;
            this.btnMCP_Uncert.Click += new System.EventHandler(this.btnMCP_Uncert_Click);
            // 
            // chtUncert
            // 
            chartArea2.AxisX.Title = "Window Size (months)";
            chartArea2.AxisY.Title = "LT Est. Target WS, m/s";
            chartArea2.Name = "ChartArea1";
            this.chtUncert.ChartAreas.Add(chartArea2);
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend2.Name = "Legend1";
            this.chtUncert.Legends.Add(legend2);
            this.chtUncert.Location = new System.Drawing.Point(598, 478);
            this.chtUncert.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chtUncert.Name = "chtUncert";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series2.Legend = "Legend1";
            series2.Name = "LT Est. WS";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series3.Legend = "Legend1";
            series3.Name = "LT Est, Avg";
            this.chtUncert.Series.Add(series2);
            this.chtUncert.Series.Add(series3);
            this.chtUncert.Size = new System.Drawing.Size(633, 288);
            this.chtUncert.TabIndex = 75;
            this.chtUncert.Text = "chtWS_Uncert";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.Location = new System.Drawing.Point(1199, -38);
            this.label34.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(229, 24);
            this.label34.TabIndex = 77;
            this.label34.Text = "Avg && SD WS Ratios (T/R)";
            // 
            // lstUncert
            // 
            this.lstUncert.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Window,
            this.AVG,
            this.SDU});
            this.lstUncert.Location = new System.Drawing.Point(1243, 482);
            this.lstUncert.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lstUncert.Name = "lstUncert";
            this.lstUncert.Size = new System.Drawing.Size(216, 289);
            this.lstUncert.TabIndex = 76;
            this.lstUncert.UseCompatibleStateImageBehavior = false;
            this.lstUncert.View = System.Windows.Forms.View.Details;
            this.lstUncert.SelectedIndexChanged += new System.EventHandler(this.lstUncert_SelectedIndexChanged);
            // 
            // Window
            // 
            this.Window.Text = "Window";
            this.Window.Width = 51;
            // 
            // AVG
            // 
            this.AVG.Text = "Mean";
            this.AVG.Width = 44;
            // 
            // SDU
            // 
            this.SDU.Text = "SD";
            this.SDU.Width = 40;
            // 
            // btnResetDates
            // 
            this.btnResetDates.Location = new System.Drawing.Point(183, 856);
            this.btnResetDates.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnResetDates.Name = "btnResetDates";
            this.btnResetDates.Size = new System.Drawing.Size(128, 43);
            this.btnResetDates.TabIndex = 78;
            this.btnResetDates.Text = "Reset Default Dates";
            this.btnResetDates.UseVisualStyleBackColor = true;
            this.btnResetDates.Click += new System.EventHandler(this.btnResetDates_Click);
            // 
            // btnResetConcDates
            // 
            this.btnResetConcDates.Location = new System.Drawing.Point(24, 495);
            this.btnResetConcDates.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnResetConcDates.Name = "btnResetConcDates";
            this.btnResetConcDates.Size = new System.Drawing.Size(104, 47);
            this.btnResetConcDates.TabIndex = 79;
            this.btnResetConcDates.Text = "Reset Conc Period";
            this.btnResetConcDates.UseVisualStyleBackColor = true;
            this.btnResetConcDates.Click += new System.EventHandler(this.btnResetConcDates_Click);
            // 
            // sfdSaveMCP
            // 
            this.sfdSaveMCP.Filter = "MCP file|*.mcp";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutMCPToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1512, 28);
            this.menuStrip1.TabIndex = 80;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(187, 26);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(187, 26);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(187, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(187, 26);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(187, 26);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // aboutMCPToolStripMenuItem
            // 
            this.aboutMCPToolStripMenuItem.Name = "aboutMCPToolStripMenuItem";
            this.aboutMCPToolStripMenuItem.Size = new System.Drawing.Size(96, 24);
            this.aboutMCPToolStripMenuItem.Text = "About MCP";
            this.aboutMCPToolStripMenuItem.Click += new System.EventHandler(this.aboutMCPToolStripMenuItem_Click);
            // 
            // ofdOpenMCP
            // 
            this.ofdOpenMCP.Filter = "MCP files|*.mcp";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(18, 330);
            this.label35.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(136, 20);
            this.label35.TabIndex = 81;
            this.label35.Text = "Num. Hour bins :";
            // 
            // cboNumHours
            // 
            this.cboNumHours.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboNumHours.FormattingEnabled = true;
            this.cboNumHours.Items.AddRange(new object[] {
            "1",
            "2",
            "4",
            "6",
            "8"});
            this.cboNumHours.Location = new System.Drawing.Point(171, 325);
            this.cboNumHours.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboNumHours.Name = "cboNumHours";
            this.cboNumHours.Size = new System.Drawing.Size(55, 28);
            this.cboNumHours.TabIndex = 82;
            this.cboNumHours.Text = "1";
            this.cboNumHours.SelectedIndexChanged += new System.EventHandler(this.cboNumHours_SelectedIndexChanged);
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.Location = new System.Drawing.Point(719, 129);
            this.label36.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(115, 20);
            this.label36.TabIndex = 84;
            this.label36.Text = "Hour Interval :";
            // 
            // cboHourInt
            // 
            this.cboHourInt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboHourInt.FormattingEnabled = true;
            this.cboHourInt.Items.AddRange(new object[] {
            "All Hours"});
            this.cboHourInt.Location = new System.Drawing.Point(844, 127);
            this.cboHourInt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboHourInt.Name = "cboHourInt";
            this.cboHourInt.Size = new System.Drawing.Size(131, 26);
            this.cboHourInt.TabIndex = 83;
            this.cboHourInt.Text = "All Hours";
            this.cboHourInt.SelectedIndexChanged += new System.EventHandler(this.cboHourInt_SelectedIndexChanged);
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label39.Location = new System.Drawing.Point(18, 362);
            this.label39.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(145, 20);
            this.label39.TabIndex = 87;
            this.label39.Text = "Num. Temp. bins :";
            // 
            // cboNumTemps
            // 
            this.cboNumTemps.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboNumTemps.FormattingEnabled = true;
            this.cboNumTemps.Items.AddRange(new object[] {
            "1",
            "2",
            "4"});
            this.cboNumTemps.Location = new System.Drawing.Point(171, 356);
            this.cboNumTemps.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboNumTemps.Name = "cboNumTemps";
            this.cboNumTemps.Size = new System.Drawing.Size(55, 28);
            this.cboNumTemps.TabIndex = 88;
            this.cboNumTemps.Text = "1";
            this.cboNumTemps.SelectedIndexChanged += new System.EventHandler(this.cboNumTemps_SelectedIndexChanged);
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.Location = new System.Drawing.Point(1215, 130);
            this.label37.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(95, 20);
            this.label37.TabIndex = 90;
            this.label37.Text = "Temp. Bin :";
            // 
            // cboTemp_Int
            // 
            this.cboTemp_Int.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTemp_Int.FormattingEnabled = true;
            this.cboTemp_Int.Items.AddRange(new object[] {
            "All Temps"});
            this.cboTemp_Int.Location = new System.Drawing.Point(1330, 127);
            this.cboTemp_Int.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboTemp_Int.Name = "cboTemp_Int";
            this.cboTemp_Int.Size = new System.Drawing.Size(126, 26);
            this.cboTemp_Int.TabIndex = 89;
            this.cboTemp_Int.Text = "All Temps";
            this.cboTemp_Int.SelectedIndexChanged += new System.EventHandler(this.cboTemp_Int_SelectedIndexChanged);
            // 
            // txtWS_PDF_Wgt
            // 
            this.txtWS_PDF_Wgt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWS_PDF_Wgt.Location = new System.Drawing.Point(183, 427);
            this.txtWS_PDF_Wgt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtWS_PDF_Wgt.Name = "txtWS_PDF_Wgt";
            this.txtWS_PDF_Wgt.Size = new System.Drawing.Size(40, 26);
            this.txtWS_PDF_Wgt.TabIndex = 92;
            this.txtWS_PDF_Wgt.Text = "1";
            this.txtWS_PDF_Wgt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtWS_PDF_Wgt.TextChanged += new System.EventHandler(this.txtWS_PDF_Wgt_TextChanged);
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.Location = new System.Drawing.Point(18, 427);
            this.label38.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(142, 20);
            this.label38.TabIndex = 91;
            this.label38.Text = "WS PDF Weight :";
            // 
            // txtLast_WS_Wgt
            // 
            this.txtLast_WS_Wgt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLast_WS_Wgt.Location = new System.Drawing.Point(183, 455);
            this.txtLast_WS_Wgt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLast_WS_Wgt.Name = "txtLast_WS_Wgt";
            this.txtLast_WS_Wgt.Size = new System.Drawing.Size(40, 26);
            this.txtLast_WS_Wgt.TabIndex = 94;
            this.txtLast_WS_Wgt.Text = "0.5";
            this.txtLast_WS_Wgt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtLast_WS_Wgt.TextChanged += new System.EventHandler(this.txtLast_WS_Wgt_TextChanged);
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label40.Location = new System.Drawing.Point(16, 458);
            this.label40.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(141, 20);
            this.label40.TabIndex = 93;
            this.label40.Text = "Last WS Weight :";
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(245, 614);
            this.txtTargetName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(96, 22);
            this.txtTargetName.TabIndex = 95;
            // 
            // txtUTMX
            // 
            this.txtUTMX.Location = new System.Drawing.Point(245, 639);
            this.txtUTMX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtUTMX.Name = "txtUTMX";
            this.txtUTMX.Size = new System.Drawing.Size(96, 22);
            this.txtUTMX.TabIndex = 96;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.Location = new System.Drawing.Point(147, 616);
            this.label41.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(81, 18);
            this.label41.TabIndex = 97;
            this.label41.Text = "Met Name:";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label42.Location = new System.Drawing.Point(156, 642);
            this.label42.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(59, 18);
            this.label42.TabIndex = 98;
            this.label42.Text = "UTM X:";
            // 
            // txtUTMY
            // 
            this.txtUTMY.Location = new System.Drawing.Point(245, 665);
            this.txtUTMY.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtUTMY.Name = "txtUTMY";
            this.txtUTMY.Size = new System.Drawing.Size(96, 22);
            this.txtUTMY.TabIndex = 99;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label43.Location = new System.Drawing.Point(156, 670);
            this.label43.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(58, 18);
            this.label43.TabIndex = 100;
            this.label43.Text = "UTM Y:";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label44.Location = new System.Drawing.Point(156, 694);
            this.label44.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(54, 18);
            this.label44.TabIndex = 101;
            this.label44.Text = "Height:";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(245, 692);
            this.txtHeight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(96, 22);
            this.txtHeight.TabIndex = 102;
            // 
            // btnExportAnnualTABs
            // 
            this.btnExportAnnualTABs.Location = new System.Drawing.Point(8, 734);
            this.btnExportAnnualTABs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExportAnnualTABs.Name = "btnExportAnnualTABs";
            this.btnExportAnnualTABs.Size = new System.Drawing.Size(129, 48);
            this.btnExportAnnualTABs.TabIndex = 103;
            this.btnExportAnnualTABs.Text = "Export Annual TAB files\r\n";
            this.btnExportAnnualTABs.UseVisualStyleBackColor = true;
            this.btnExportAnnualTABs.Click += new System.EventHandler(this.btnExportAnnualTABs_Click);
            // 
            // cboTAB_bins
            // 
            this.cboTAB_bins.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTAB_bins.FormattingEnabled = true;
            this.cboTAB_bins.Items.AddRange(new object[] {
            "16",
            "24"});
            this.cboTAB_bins.Location = new System.Drawing.Point(275, 719);
            this.cboTAB_bins.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboTAB_bins.Name = "cboTAB_bins";
            this.cboTAB_bins.Size = new System.Drawing.Size(55, 26);
            this.cboTAB_bins.TabIndex = 104;
            this.cboTAB_bins.Text = "16";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label45.Location = new System.Drawing.Point(156, 725);
            this.label45.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(105, 18);
            this.label45.TabIndex = 105;
            this.label45.Text = "Num WD bins:";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label46.Location = new System.Drawing.Point(244, 401);
            this.label46.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(94, 30);
            this.label46.TabIndex = 106;
            this.label46.Text = "Uncert. window \r\nstep (mnths):";
            // 
            // cboUncertStep
            // 
            this.cboUncertStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboUncertStep.FormattingEnabled = true;
            this.cboUncertStep.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cboUncertStep.Location = new System.Drawing.Point(266, 435);
            this.cboUncertStep.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboUncertStep.Name = "cboUncertStep";
            this.cboUncertStep.Size = new System.Drawing.Size(55, 28);
            this.cboUncertStep.TabIndex = 107;
            this.cboUncertStep.Text = "1";
            this.cboUncertStep.SelectedIndexChanged += new System.EventHandler(this.cboUncertStep_SelectedIndexChanged);
            // 
            // MCP_tool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1512, 922);
            this.Controls.Add(this.cboUncertStep);
            this.Controls.Add(this.label46);
            this.Controls.Add(this.label45);
            this.Controls.Add(this.cboTAB_bins);
            this.Controls.Add(this.btnExportAnnualTABs);
            this.Controls.Add(this.txtHeight);
            this.Controls.Add(this.label44);
            this.Controls.Add(this.label43);
            this.Controls.Add(this.txtUTMY);
            this.Controls.Add(this.label42);
            this.Controls.Add(this.label41);
            this.Controls.Add(this.txtUTMX);
            this.Controls.Add(this.txtTargetName);
            this.Controls.Add(this.txtLast_WS_Wgt);
            this.Controls.Add(this.label40);
            this.Controls.Add(this.txtWS_PDF_Wgt);
            this.Controls.Add(this.label38);
            this.Controls.Add(this.label37);
            this.Controls.Add(this.cboTemp_Int);
            this.Controls.Add(this.cboNumTemps);
            this.Controls.Add(this.label39);
            this.Controls.Add(this.label36);
            this.Controls.Add(this.cboHourInt);
            this.Controls.Add(this.cboNumHours);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.btnResetConcDates);
            this.Controls.Add(this.btnResetDates);
            this.Controls.Add(this.label34);
            this.Controls.Add(this.lstUncert);
            this.Controls.Add(this.chtUncert);
            this.Controls.Add(this.btnMCP_Uncert);
            this.Controls.Add(this.btnExportMultitest);
            this.Controls.Add(this.txtVRsq);
            this.Controls.Add(this.txtVIntercept);
            this.Controls.Add(this.txtVSlope);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.label32);
            this.Controls.Add(this.label33);
            this.Controls.Add(this.btnUpdate_Conc_Plot);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.txtLoadedTarget);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.txtLoadedReference);
            this.Controls.Add(this.btnConvertMonthly);
            this.Controls.Add(this.btnConvertToHourly);
            this.Controls.Add(this.btnExportBinRatios);
            this.Controls.Add(this.txtORsq);
            this.Controls.Add(this.txtOIntercept);
            this.Controls.Add(this.txtOSlope);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.lstBins);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.txtNumYrsConc);
            this.Controls.Add(this.txtNumYrsTarg);
            this.Controls.Add(this.txtNumYrsRef);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txtLTratio);
            this.Controls.Add(this.txtAvgRatio);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtDataCount);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.date_Export_End);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.date_Export_Start);
            this.Controls.Add(this.btnClearTarget);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.date_Corr_End);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.date_Corr_Start);
            this.Controls.Add(this.btnClearRef);
            this.Controls.Add(this.txtTarg_LT_WS);
            this.Controls.Add(this.txtRef_LT_WS);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chtScatter);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cboWD_sector);
            this.Controls.Add(this.txtWS_bin_width);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtTargAvgWS);
            this.Controls.Add(this.txtRefAvgWS);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboNumWD);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboMCP_Type);
            this.Controls.Add(this.btnExportTAB);
            this.Controls.Add(this.btnExportTS);
            this.Controls.Add(this.btnRunMCP);
            this.Controls.Add(this.btnImportTarget);
            this.Controls.Add(this.btnImportRef);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MCP_tool";
            this.Load += new System.EventHandler(this.MCP_tool_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chtScatter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chtUncert)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnImportRef;
        private System.Windows.Forms.Button btnImportTarget;
        private System.Windows.Forms.Button btnRunMCP;
        private System.Windows.Forms.Button btnExportTS;
        private System.Windows.Forms.Button btnExportTAB;
        private System.Windows.Forms.ComboBox cboMCP_Type;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboNumWD;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtRefAvgWS;
        private System.Windows.Forms.TextBox txtTargAvgWS;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtWS_bin_width;
        private System.Windows.Forms.ComboBox cboWD_sector;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtScatter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRef_LT_WS;
        private System.Windows.Forms.TextBox txtTarg_LT_WS;
        private System.Windows.Forms.OpenFileDialog ofdRefSite;
        private System.Windows.Forms.Button btnClearRef;
        private System.Windows.Forms.DateTimePicker date_Corr_Start;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker date_Corr_End;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnClearTarget;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DateTimePicker date_Export_End;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DateTimePicker date_Export_Start;
        private System.Windows.Forms.TextBox txtDataCount;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtAvgRatio;
        private System.Windows.Forms.TextBox txtLTratio;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtNumYrsRef;
        private System.Windows.Forms.TextBox txtNumYrsTarg;
        private System.Windows.Forms.TextBox txtNumYrsConc;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ListView lstBins;
        private System.Windows.Forms.ColumnHeader WS;
        private System.Windows.Forms.ColumnHeader Mean;
        private System.Windows.Forms.ColumnHeader SD;
        private System.Windows.Forms.ColumnHeader Count;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox txtOSlope;
        private System.Windows.Forms.TextBox txtOIntercept;
        private System.Windows.Forms.TextBox txtORsq;
        private System.Windows.Forms.TextBox txtVSlope;
        private System.Windows.Forms.TextBox txtVIntercept;
        private System.Windows.Forms.TextBox txtVRsq;
        private System.Windows.Forms.SaveFileDialog sfdSaveTimeSeries;
        private System.Windows.Forms.SaveFileDialog sfdSaveTAB;
        private System.Windows.Forms.Button btnExportBinRatios;
        private System.Windows.Forms.Button btnConvertToHourly;
        private System.Windows.Forms.Button btnConvertMonthly;
        private System.Windows.Forms.TextBox txtLoadedReference;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox txtLoadedTarget;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Button btnUpdate_Conc_Plot;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Button btnExportMultitest;
        private System.Windows.Forms.Button btnMCP_Uncert;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtUncert;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.ListView lstUncert;
        private System.Windows.Forms.ColumnHeader Window;
        private System.Windows.Forms.ColumnHeader AVG;
        private System.Windows.Forms.ColumnHeader SDU;
        private System.Windows.Forms.Button btnResetDates;
        private System.Windows.Forms.Button btnResetConcDates;
        private System.Windows.Forms.SaveFileDialog sfdSaveMCP;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog ofdOpenMCP;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.ComboBox cboNumHours;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.ComboBox cboHourInt;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.ComboBox cboNumTemps;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.ComboBox cboTemp_Int;
        private System.Windows.Forms.TextBox txtWS_PDF_Wgt;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.TextBox txtLast_WS_Wgt;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.TextBox txtTargetName;
        private System.Windows.Forms.TextBox txtUTMX;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.TextBox txtUTMY;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.Button btnExportAnnualTABs;
        private System.Windows.Forms.ComboBox cboTAB_bins;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.FolderBrowserDialog TABfolder;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.ComboBox cboUncertStep;
        private System.Windows.Forms.ToolStripMenuItem aboutMCPToolStripMenuItem;
    }
}

