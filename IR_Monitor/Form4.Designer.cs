namespace IR_Monitor
{
    partial class Form4
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart_H2S = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart_CO = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart_Temp = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart_volt = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart_FL = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart_O2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.CLOSE = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart_H2S)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_CO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Temp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_volt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_FL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_O2)).BeginInit();
            this.SuspendLayout();
            // 
            // chart_H2S
            // 
            chartArea1.AxisX.IsStartedFromZero = false;
            chartArea1.BackColor = System.Drawing.Color.White;
            chartArea1.CursorX.IsUserEnabled = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.CursorY.IsUserEnabled = true;
            chartArea1.CursorY.IsUserSelectionEnabled = true;
            chartArea1.Name = "ChartArea1";
            this.chart_H2S.ChartAreas.Add(chartArea1);
            legend1.BackColor = System.Drawing.Color.White;
            legend1.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend1.IsTextAutoFit = false;
            legend1.Name = "Legend1";
            this.chart_H2S.Legends.Add(legend1);
            this.chart_H2S.Location = new System.Drawing.Point(16, 38);
            this.chart_H2S.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart_H2S.Name = "chart_H2S";
            series1.BackSecondaryColor = System.Drawing.Color.White;
            series1.BorderColor = System.Drawing.Color.Black;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.Blue;
            series1.LabelBackColor = System.Drawing.Color.White;
            series1.Legend = "Legend1";
            series1.Name = "H2S";
            this.chart_H2S.Series.Add(series1);
            this.chart_H2S.Size = new System.Drawing.Size(365, 258);
            this.chart_H2S.TabIndex = 0;
            this.chart_H2S.Text = "chart1";
            // 
            // chart_CO
            // 
            chartArea2.CursorX.IsUserEnabled = true;
            chartArea2.CursorX.IsUserSelectionEnabled = true;
            chartArea2.CursorY.IsUserEnabled = true;
            chartArea2.CursorY.IsUserSelectionEnabled = true;
            chartArea2.Name = "ChartArea1";
            this.chart_CO.ChartAreas.Add(chartArea2);
            legend2.BackColor = System.Drawing.Color.White;
            legend2.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend2.IsTextAutoFit = false;
            legend2.Name = "Legend1";
            this.chart_CO.Legends.Add(legend2);
            this.chart_CO.Location = new System.Drawing.Point(397, 38);
            this.chart_CO.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart_CO.Name = "chart_CO";
            series2.BackSecondaryColor = System.Drawing.Color.White;
            series2.BorderColor = System.Drawing.Color.Black;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.Lime;
            series2.LabelBackColor = System.Drawing.Color.White;
            series2.Legend = "Legend1";
            series2.Name = "CO";
            this.chart_CO.Series.Add(series2);
            this.chart_CO.Size = new System.Drawing.Size(365, 258);
            this.chart_CO.TabIndex = 1;
            this.chart_CO.Text = "chart2";
            // 
            // chart_Temp
            // 
            chartArea3.CursorX.IsUserEnabled = true;
            chartArea3.CursorX.IsUserSelectionEnabled = true;
            chartArea3.CursorY.IsUserEnabled = true;
            chartArea3.CursorY.IsUserSelectionEnabled = true;
            chartArea3.Name = "ChartArea1";
            this.chart_Temp.ChartAreas.Add(chartArea3);
            legend3.BackColor = System.Drawing.Color.White;
            legend3.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend3.IsTextAutoFit = false;
            legend3.Name = "Legend1";
            this.chart_Temp.Legends.Add(legend3);
            this.chart_Temp.Location = new System.Drawing.Point(780, 38);
            this.chart_Temp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart_Temp.Name = "chart_Temp";
            series3.BackSecondaryColor = System.Drawing.Color.White;
            series3.BorderColor = System.Drawing.Color.Black;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Color = System.Drawing.Color.Red;
            series3.LabelBackColor = System.Drawing.Color.White;
            series3.Legend = "Legend1";
            series3.Name = "Temp";
            this.chart_Temp.Series.Add(series3);
            this.chart_Temp.Size = new System.Drawing.Size(365, 258);
            this.chart_Temp.TabIndex = 2;
            this.chart_Temp.Text = "chart3";
            // 
            // chart_volt
            // 
            chartArea4.BackColor = System.Drawing.Color.White;
            chartArea4.CursorX.IsUserEnabled = true;
            chartArea4.CursorX.IsUserSelectionEnabled = true;
            chartArea4.CursorY.IsUserEnabled = true;
            chartArea4.CursorY.IsUserSelectionEnabled = true;
            chartArea4.Name = "ChartArea1";
            this.chart_volt.ChartAreas.Add(chartArea4);
            legend4.BackColor = System.Drawing.Color.White;
            legend4.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend4.IsTextAutoFit = false;
            legend4.Name = "Legend1";
            this.chart_volt.Legends.Add(legend4);
            this.chart_volt.Location = new System.Drawing.Point(780, 318);
            this.chart_volt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart_volt.Name = "chart_volt";
            series4.BackSecondaryColor = System.Drawing.Color.White;
            series4.BorderColor = System.Drawing.Color.Black;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Color = System.Drawing.Color.DarkViolet;
            series4.LabelBackColor = System.Drawing.Color.White;
            series4.Legend = "Legend1";
            series4.Name = "Volt";
            this.chart_volt.Series.Add(series4);
            this.chart_volt.Size = new System.Drawing.Size(365, 258);
            this.chart_volt.TabIndex = 5;
            this.chart_volt.Text = "chart4";
            // 
            // chart_FL
            // 
            chartArea5.CursorX.IsUserEnabled = true;
            chartArea5.CursorX.IsUserSelectionEnabled = true;
            chartArea5.CursorY.IsUserEnabled = true;
            chartArea5.CursorY.IsUserSelectionEnabled = true;
            chartArea5.Name = "ChartArea1";
            this.chart_FL.ChartAreas.Add(chartArea5);
            legend5.BackColor = System.Drawing.Color.White;
            legend5.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend5.IsTextAutoFit = false;
            legend5.Name = "Legend1";
            this.chart_FL.Legends.Add(legend5);
            this.chart_FL.Location = new System.Drawing.Point(397, 318);
            this.chart_FL.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart_FL.Name = "chart_FL";
            series5.BackSecondaryColor = System.Drawing.Color.White;
            series5.BorderColor = System.Drawing.Color.Black;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            series5.LabelBackColor = System.Drawing.Color.White;
            series5.Legend = "Legend1";
            series5.Name = "%LEL";
            this.chart_FL.Series.Add(series5);
            this.chart_FL.Size = new System.Drawing.Size(365, 258);
            this.chart_FL.TabIndex = 4;
            this.chart_FL.Text = "chart5";
            // 
            // chart_O2
            // 
            chartArea6.CursorX.IsUserEnabled = true;
            chartArea6.CursorX.IsUserSelectionEnabled = true;
            chartArea6.CursorY.IsUserEnabled = true;
            chartArea6.CursorY.IsUserSelectionEnabled = true;
            chartArea6.Name = "ChartArea1";
            this.chart_O2.ChartAreas.Add(chartArea6);
            legend6.BackColor = System.Drawing.Color.White;
            legend6.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend6.IsTextAutoFit = false;
            legend6.Name = "Legend1";
            this.chart_O2.Legends.Add(legend6);
            this.chart_O2.Location = new System.Drawing.Point(16, 318);
            this.chart_O2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart_O2.Name = "chart_O2";
            series6.BackSecondaryColor = System.Drawing.Color.White;
            series6.BorderColor = System.Drawing.Color.Black;
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Color = System.Drawing.Color.Fuchsia;
            series6.LabelBackColor = System.Drawing.Color.White;
            series6.Legend = "Legend1";
            series6.Name = "O2";
            this.chart_O2.Series.Add(series6);
            this.chart_O2.Size = new System.Drawing.Size(365, 258);
            this.chart_O2.TabIndex = 3;
            this.chart_O2.Text = "chart6";
            // 
            // CLOSE
            // 
            this.CLOSE.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CLOSE.ForeColor = System.Drawing.Color.Red;
            this.CLOSE.Location = new System.Drawing.Point(1081, 3);
            this.CLOSE.Name = "CLOSE";
            this.CLOSE.Size = new System.Drawing.Size(63, 30);
            this.CLOSE.TabIndex = 6;
            this.CLOSE.Text = "CLOSE";
            this.CLOSE.UseVisualStyleBackColor = true;
            this.CLOSE.Click += new System.EventHandler(this.CLOSE_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 18);
            this.label1.TabIndex = 7;
            this.label1.Text = "H2S";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(394, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 18);
            this.label2.TabIndex = 8;
            this.label2.Text = "CO";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(777, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 18);
            this.label3.TabIndex = 9;
            this.label3.Text = "Temp";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 298);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 18);
            this.label4.TabIndex = 10;
            this.label4.Text = "O2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(394, 298);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 18);
            this.label5.TabIndex = 11;
            this.label5.Text = "FL";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(777, 298);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 18);
            this.label6.TabIndex = 12;
            this.label6.Text = "Volt";
            // 
            // Form4
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1162, 600);
            this.ControlBox = false;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CLOSE);
            this.Controls.Add(this.chart_volt);
            this.Controls.Add(this.chart_FL);
            this.Controls.Add(this.chart_O2);
            this.Controls.Add(this.chart_Temp);
            this.Controls.Add(this.chart_CO);
            this.Controls.Add(this.chart_H2S);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form4";
            this.Text = "Graph popup";
            ((System.ComponentModel.ISupportInitialize)(this.chart_H2S)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_CO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Temp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_volt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_FL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_O2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart_H2S;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_CO;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Temp;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_volt;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_FL;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_O2;
        private System.Windows.Forms.Button CLOSE;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}