namespace IR_Monitor
{
    partial class Form2
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbo_baudrate = new System.Windows.Forms.ComboBox();
            this.cboPort = new System.Windows.Forms.ComboBox();
            this.OKbutton = new System.Windows.Forms.Button();
            this.numericUpDown_stopbits = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_databits = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_stopbits)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_databits)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbo_baudrate);
            this.groupBox1.Controls.Add(this.cboPort);
            this.groupBox1.Controls.Add(this.OKbutton);
            this.groupBox1.Controls.Add(this.numericUpDown_stopbits);
            this.groupBox1.Controls.Add(this.numericUpDown_databits);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(2, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(179, 316);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Serial Settings";
            // 
            // cbo_baudrate
            // 
            this.cbo_baudrate.DisplayMember = "57600";
            this.cbo_baudrate.FormattingEnabled = true;
            this.cbo_baudrate.Items.AddRange(new object[] {
            "75",
            "110",
            "134",
            "150",
            "300",
            "600",
            "1200",
            "1800",
            "2400",
            "4800",
            "7200",
            "9600",
            "14400",
            "19200",
            "28800",
            "38400",
            "56000",
            "57600",
            "115200",
            "128000",
            "230400"});
            this.cbo_baudrate.Location = new System.Drawing.Point(40, 102);
            this.cbo_baudrate.Name = "cbo_baudrate";
            this.cbo_baudrate.Size = new System.Drawing.Size(103, 22);
            this.cbo_baudrate.TabIndex = 22;
            this.cbo_baudrate.Text = "57600";
            // 
            // cboPort
            // 
            this.cboPort.FormattingEnabled = true;
            this.cboPort.Location = new System.Drawing.Point(40, 43);
            this.cboPort.Name = "cboPort";
            this.cboPort.Size = new System.Drawing.Size(103, 22);
            this.cboPort.TabIndex = 21;
            // 
            // OKbutton
            // 
            this.OKbutton.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OKbutton.Location = new System.Drawing.Point(52, 272);
            this.OKbutton.Name = "OKbutton";
            this.OKbutton.Size = new System.Drawing.Size(75, 25);
            this.OKbutton.TabIndex = 17;
            this.OKbutton.Text = "OK";
            this.OKbutton.UseVisualStyleBackColor = true;
            this.OKbutton.Click += new System.EventHandler(this.OKbutton_Click);
            // 
            // numericUpDown_stopbits
            // 
            this.numericUpDown_stopbits.Location = new System.Drawing.Point(40, 222);
            this.numericUpDown_stopbits.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_stopbits.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_stopbits.Name = "numericUpDown_stopbits";
            this.numericUpDown_stopbits.Size = new System.Drawing.Size(103, 22);
            this.numericUpDown_stopbits.TabIndex = 15;
            this.numericUpDown_stopbits.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDown_databits
            // 
            this.numericUpDown_databits.Location = new System.Drawing.Point(40, 162);
            this.numericUpDown_databits.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numericUpDown_databits.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numericUpDown_databits.Name = "numericUpDown_databits";
            this.numericUpDown_databits.Size = new System.Drawing.Size(103, 22);
            this.numericUpDown_databits.TabIndex = 14;
            this.numericUpDown_databits.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "baud rate";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(23, 202);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "stop bits";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(23, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "data bits";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port";
            // 
            // Form2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(183, 334);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Form2";
            this.Text = "Open Port";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_stopbits)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_databits)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button OKbutton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboPort;
        private System.Windows.Forms.ComboBox cbo_baudrate;
        private System.Windows.Forms.NumericUpDown numericUpDown_stopbits;
        private System.Windows.Forms.NumericUpDown numericUpDown_databits;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
    }
}