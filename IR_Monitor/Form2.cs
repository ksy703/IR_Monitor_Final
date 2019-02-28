using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

//'open port'클릭 시 setting 팝업 창

namespace IR_Monitor
{
    public partial class Form2 : Form
    {

        public string[] Serial_settings = new string[6];

        public Form2()
        {
            InitializeComponent();

        }

        public string[] Passvalue
        {
            get { return this.Serial_settings; }
            set { this.Serial_settings = value; }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string[] ports=SerialPort.GetPortNames();
            
            cboPort.Items.AddRange(ports);
            CheckForIllegalCrossThreadCalls = false;
            
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {

            Serial_settings[0] = cboPort.Text;
            Serial_settings[1] = cbo_baudrate.Text;
            Serial_settings[2] = numericUpDown_databits.Text;
            Serial_settings[3] = "None";
            Serial_settings[4] = numericUpDown_stopbits.Text;
            Serial_settings[5] = "None";
            Passvalue = Serial_settings;
            this.Close();
            
        }
    }
}
