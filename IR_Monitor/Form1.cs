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
using System.IO;
using System.Threading;

namespace IR_Monitor
{
    public partial class form1 : Form
    {
        string btn_flag = string.Empty;

        SerialPort sport;
        public OpenPort op = new OpenPort();
        public string[] Serial_settings = new string[6]; //[0]:Port, [1]:baud rate, [2]:data bits, [3]:parity, [4]:stop bits, [5]:flow control

        public Command cm = new Command();

        bool monitoring; int line_cnt = 0; int file_cnt = 1;
        FileStream Mtr_fs;
        StreamWriter Mtr_sw;
        string Mtr_Filepath = string.Empty;
        DateTime ReadlogTime;
        DateTime WritelogTime;
        string Mtr_Log = string.Empty;
        public DataTable Graph_table;

        Form3 log_popup = new Form3();

        Form4 graph_popup = new Form4();

        string checked_LED;


        public form1()
        {
            InitializeComponent();
        }


        private void Open_port_Click(object sender, EventArgs e)
        {
            //Open Port popup창 위치 설정
            Point parentLocation = this.Location;
            Form2 OpenPortForm = new Form2();
            OpenPortForm.StartPosition = FormStartPosition.Manual;
            OpenPortForm.Location = new Point(parentLocation.X + 200, parentLocation.Y + 50);

            OpenPortForm.Passvalue = Serial_settings;
            OpenPortForm.ShowDialog();
            Serial_settings = OpenPortForm.Passvalue;
            textBox_port.Text = OpenPortForm.Serial_settings[0];

            tB_ReceivedString.Clear();
            tB_Enter_cmd.Clear();
            tb_Parsed_command.Clear();

            Start_Open_port();
        }


        public void Start_Open_port()
        {
            try
            {
                //Serial Port null 상태에서 open일 경우
                if (null == sport)
                {
                    sport = new SerialPort();

                    //Port가 read 할 때 동작 과정
                    sport.DataReceived += sp_DataReceived;

                    setSerialPort(sport, Serial_settings);
                    sport.Open();

                }

                //Port 변경할 경우
                else if (sport.PortName != Serial_settings[0] && sport.IsOpen)
                {
                    sport.Close();
                    setSerialPort(sport, Serial_settings);
                    sport.Open();
                }
                if (sport.IsOpen)
                {
                    textBox_port.BackColor = Color.LawnGreen;
                }

            }
            catch (System.Exception ex)
            {
                sport = null;
                textBox_port.Clear();
                textBox_port.BackColor = Color.White;
                MessageBox.Show(ex.Message);
            }
        }

        //serial port data receive
        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                IAsyncResult ar = this.BeginInvoke(new MethodInvoker(delegate () { sp_DataReceived(sender, e); }));
                this.EndInvoke(ar);
            }
            else
            {
                //CheckForIllegalCrossThreadCalls = false;
                timer2.Enabled = false;
                tB_ReceivedString.Clear();
                string log = "";
                ReadlogTime = DateTime.Now;


                log += ReadlogTime.ToString("yyyy/MM/dd HH:mm:ss.fff") + " << ";
                SerialPort sp = (SerialPort)sender;

                byte[] read_bytes = new byte[600];
                int LEN = 0;
                int read_len = sp.BytesToRead;
                //Console.WriteLine(read_len);
                sp.Read(read_bytes, 0, read_len);
                int cnt = 0; string newlog = "";
                foreach (byte b in read_bytes)
                {
                    cnt++;
                    char sb = Convert.ToChar(b);

                    newlog += String.Format("{0:X}", b) + " ";
                    tB_ReceivedString.Text += sb;
                    if (tB_ReceivedString.Text.Length == 12)
                    {
                        LEN = Convert.ToInt32(tB_ReceivedString.Text.Substring(8, 4), 16);
                    }
                    else if (cnt == LEN + 17)
                    {

                        break;
                    }
                }
                tB_Log.Text += log + newlog + "\r\n";
                tB_Log.Select(tB_Log.Text.Length, 0); tB_Log.ScrollToCaret();

                if (!(btn_flag == "Monitoring") && !check_crc(tB_ReceivedString.Text, LEN + 12, read_bytes) && !tb_Parsed_command.Text.Contains("24E3") && !tb_Parsed_command.Text.Contains("24E4"))
                {
                    MessageBox.Show("wrong data");
                }
                tB_ReceivedString_con.Text = tB_ReceivedString.Text;
                tB_Received_Sensor.Text = tB_ReceivedString.Text;
                tB_ReceivedUserConfig.Text = tB_ReceivedString.Text;
                if (tabControl1.SelectedIndex == 0)
                {
                    if (tB_ReceivedString.Text.Contains("5039") || tB_ReceivedString.Text.Contains("5040"))
                    {
                        ReadLife.Text = Convert.ToInt32((tB_ReceivedString.Text.Split('=')[1].Substring(0, 8)), 16).ToString();
                    }
                }
                if (btn_flag == "Monitoring")
                {
                    Fill_Data(tB_ReceivedString.Text);
                    timer3.Enabled = false;
                    Mtr_Log += tB_Log.Text;
                    if (log_popup.Visible == true)
                    {
                        log_popup.Passvalue = Mtr_Log;
                    }
                    tB_Log.Clear();

                }
                else if (btn_flag.Equals("User Option"))
                {
                    usrOptions_Indicator(tB_ReceivedString_con.Text);
                }
                else if (btn_flag.Equals("H2S"))
                {
                    Load_H2S(tB_ReceivedString_con.Text);
                }
                else if (btn_flag.Equals("CO"))
                {
                    Load_CO(tB_ReceivedString_con.Text);
                }
                else if (btn_flag.Equals("O2"))
                {
                    Load_O2(tB_ReceivedString_con.Text);
                }
                else if (btn_flag.Equals("LEL"))
                {
                    Load_LEL(tB_ReceivedString_con.Text);
                }
                else if (btn_flag.Equals("sensor"))
                {
                    load_sensor(tB_Received_Sensor.Text);
                }

                if (Repeat.Checked)
                {
                    Thread.Sleep(500);
                    write_log(tb_Parsed_command.Text);
                    sport.Write(tb_Parsed_command.Text);
                }
                btn_flag = "";
                //this.Invoke(new EventHandler(displayText));
            }
        }

        //port read 할 때 crc check
        public Boolean check_crc(string s, int len, byte[] data)
        {

            UInt32 us_UpdateCRC16 = 0xFFFF;
            byte[] b = new byte[len + 6];
            System.Buffer.BlockCopy(data, 0, b, 0, len);
            us_UpdateCRC16 = cm.calculateCRC2(len, b, us_UpdateCRC16);
            if (us_UpdateCRC16.ToString("X4").Equals(s.Substring(s.Length - 5, 4)))
            {
                return true;
            }
            return false;
        }

        //port write 할 때 log
        public void write_log(string s)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    IAsyncResult ar2 = this.BeginInvoke(new MethodInvoker(delegate () { write_log(s); }));
                    this.EndInvoke(ar2);
                }
                else
                {
                    char[] values = s.ToCharArray();
                    WritelogTime = DateTime.Now;

                    tB_Log.Text += WritelogTime.ToString("yyyy/MM/dd HH:mm:ss.fff") + " >> ";
                    foreach (char letter in values)
                    {
                        int v = Convert.ToInt32(letter);
                        tB_Log.Text += String.Format("{0:X}", v) + " ";
                    }
                    tB_Log.Text += "\r\n";
                }
            }
            catch (FormatException ex)
            {
                throw (ex);
            }
        }

        //serial port setting
        public void setSerialPort(SerialPort sport, string[] settings)
        {
            sport.PortName = Serial_settings[0];
            sport.BaudRate = Convert.ToInt32(Serial_settings[1]);
            sport.DataBits = Convert.ToInt32(Serial_settings[2]);
            sport.Parity = op.GetParity(Serial_settings[3]);
            sport.StopBits = op.GetStopBits(Serial_settings[4]);
            sport.ReadTimeout = (int)500;
            sport.Handshake = op.GetHandshake(Serial_settings[5]);

        }

        private void Button_Close_Click(object sender, EventArgs e)
        {
            if (null != sport)
            {
                if (monitoring)
                {
                    MessageBox.Show("Stop Monitoring first.");
                }
                else if (sport.IsOpen)
                {
                    var result = MessageBox.Show("Close port?", "Port", MessageBoxButtons.OKCancel);
                    if (result == DialogResult.OK)
                    {
                        textBox_port.Clear();
                        sport.Close();
                        sport.Dispose();
                        textBox_port.BackColor = Color.White;
                        sport = null;
                    }
                }
            }

        }


        //*********************** Manual Test Tab ************************//

        //Connection Command
        private void cB_Connection_cmd_SelectedIndexChanged(object sender, EventArgs e)
        {
            tB_Enter_cmd.Text = cm.Manual_Test_command(cB_Connection_cmd.SelectedItem.ToString());
        }

        //Test Command
        private void cB_Test_cmd_SelectedIndexChanged(object sender, EventArgs e)
        {
            tB_Enter_cmd.Text = cm.Manual_Test_command(cB_Test_cmd.SelectedItem.ToString());
            if (cB_Test_cmd.SelectedItem.ToString().Contains("LED"))
            {
                tB_Enter_cmd.Text += String.Format("{0:X1}", Convert.ToInt32(checked_LED, 2)) + ";";
            }
            else if (cB_Test_cmd.SelectedItem.ToString().Contains("Frequency"))
            {
                tB_Enter_cmd.Text += String.Format("{0:X4}", Convert.ToInt32(Frequency.Value)) + ";";
            }
            else if (cB_Test_cmd.SelectedItem.ToString().Contains("Duty"))
            {
                tB_Enter_cmd.Text += String.Format("{0:X}", Convert.ToInt32(Duty.Text)) + ";";
            }
            else if (cB_Test_cmd.SelectedItem.ToString().Contains("Life Time"))
            {
                if (cB_Test_cmd.SelectedItem.ToString().Contains("Set"))
                {
                    tB_Enter_cmd.Text += String.Format("{0:X8}", Convert.ToInt32(LifeTime.Value)) + ";";
                }
            }
            else if (cB_Test_cmd.SelectedItem.ToString().Contains("Time"))
            {
                if (cB_Test_cmd.SelectedItem.ToString().Contains("Set"))
                {
                    tB_Enter_cmd.Text += String.Format("{0:X}", Convert.ToInt32(Time.Text)) + ";";
                }
            }
        }

        //LED Number CheckBox
        public void check_LED(object sender, EventArgs e)
        {
            tB_Enter_cmd.Clear();
            cB_Test_cmd.SelectedIndex = 0;
            checked_LED = Convert.ToInt32(checkBox_LED4.Checked).ToString()
            + Convert.ToInt32(checkBox_LED3.Checked).ToString()
            + Convert.ToInt32(checkBox_LED2.Checked).ToString()
            + Convert.ToInt32(checkBox_LED1.Checked).ToString();
        }

        //Clear Button Click
        private void Clear_Click(object sender, EventArgs e)
        {
            tB_Log.Clear();
        }

        //SEND Button Click
        private void SEND_Click(object sender, EventArgs e)
        {
            if (sport == null)
            {
                MessageBox.Show("open the port");
            }
            else
            {
                timer2.Enabled = true;
                int length = tB_Enter_cmd.TextLength;
                tb_Parsed_command.Text = cm.make_cmd(tB_Enter_cmd.Text, length);
                write_log(tb_Parsed_command.Text);
                sport.Write(tb_Parsed_command.Text);

            }
        }

        //************************ Config Tab **************************//

        //Updatable CheckBox (User Option / H2S / CO / O2 / LEL)
        private void cB_Updatable_CheckedChanged(object sender, EventArgs e)
        {
            //User Option
            tB_Config_data.Clear();
            tB_Config_data.Text += cm.BoolConfig(cB_UserOption_Stealth.Checked, Stealth_Enabled.Checked, "Stealth", "")
            + cm.BoolConfig(cB_UserOption_AlarmLatch.Checked, AlarmLatch_Enabled.Checked, "Alarm Latching", "")
            + cm.NumConfig(cB_UserOption_LatchTime.Checked, nUpDown_UserOption_LatchTime.Value, "Latching Activation Time", "")
            + cm.BoolConfig(cB_UserOption_Blank.Checked, blank_Enabled.Checked, "Blank Zone", "")
            + cm.BoolConfig(cB_UserOption_FactoryCal.Checked, FactoryCal_Enabled.Checked, "Factory Calibration Mode", "")
            + cm.BoolConfig(cB_UserOption_TravelDisabled.Checked, TravelDisabled_Enabled.Checked, "Travel Mode Disabled", "")
            + cm.BoolConfig(cB_UserOption_Service.Checked, Service_Enabled.Checked, "Service Mode", "");

            //H2S
            tB_Config_data.Text += cm.BoolConfig(cBUpdate_H2S_SensorDisabled.Checked, cB_H2S_SensorDisabled.Checked, "Sensor Disabled", "A1")
                + cm.NumConfig(cBUpdate_H2S_Low.Checked, nUpDown_H2S_Low.Value, "Low", "A1")
                + cm.NumConfig(cBUpdate_H2S_High.Checked, nUpDown_H2S_High.Value, "High", "A1")
                + cm.NumConfig(cBUpdate_H2S_TWA.Checked, nUpDown_H2S_TWA.Value, "TWA", "A1")
                + cm.NumConfig(cBUpdate_H2S_STEL.Checked, nUpDown_H2S_STEL.Value, "STEL", "A1")
                + cm.NumConfig(cBUpdate_H2S_CalInterval.Checked, nUpDown_H2S_CalInterval.Value, "Calibration Interval", "A1")
                + cm.NumConfig(cBUpdate_H2S_BumpInterval.Checked, nUpDown_H2S_BumpInterval.Value, "Bump Interval", "A1")
                + cm.NumConfig(cBUpdate_H2S_STELInterval.Checked, nUpDown_H2S_STELInterval.Value, "STEL Interval", "A1")
                + cm.NumConfig(cBUpdate_H2S_LastCal.Checked, nUpDown_H2S_LastCal.Value, "Last Calibration", "A1")
                + cm.NumConfig(cBUpdate_H2S_lastBump.Checked, nUpDown_H2S_LastBump.Value, "Last Bump", "A1")
                + cm.BoolConfig(cBUpdate_H2S_DisplayDecimal.Checked, cB_H2S_DisplayDecimal.Checked, "Display Decimal", "A1")
                + cm.NumConfig(cBUpdate_H2S_calGasconc.Checked, nUpDown_H2S_CalGasConc.Value, "Calibration Gas Conc", "A1")
                + cm.NumConfig(cBUpdate_H2S_calrange.Checked, nUpDown_H2S_CalRange.Value, "Calibration Range", "A1")
                + cm.NumConfig(cBUpdate_H2S_BumpThreshold.Checked, nUpDown_H2S_BumpThreshold.Value, "Bump Threshold", "A1")
                + cm.NumConfig(cBUpdate_H2S_Interval.Checked, nUpDown_H2S_Interval.Value, "Interval", "A1")
                + cm.NumConfig(cBUpdate_H2S_PulseWidth.Checked, nUpDown_H2S_PulseWidth.Value, "Pulse Width", "A1")
                + cm.NumConfig(cBUpdate_H2S_DecayTime.Checked, nUpDown_H2S_DecayTime.Value, "Decay Time", "A1")
                + cm.NumConfig(cBUpdate_H2S_DecayThreshold.Checked, nUpDown_H2S_DecayThreshold.Value, "Decay Threshold", "A1")
                + cm.NumConfig(cBUpdate_H2S_RecoveryTime.Checked, nUpDown_H2S_RecoveryTime.Value, "Recovery Time", "A1");


            //CO
            tB_Config_data.Text += cm.BoolConfig(Update_CO_SensorDisabled.Checked, cB_CO_SensorDisabled.Checked, "Sensor Disabled", "A2")
                + cm.NumConfig(Update_CO_Low.Checked, n_CO_Low.Value, "Low", "A2")
                + cm.NumConfig(Update_CO_High.Checked, n_CO_High.Value, "High", "A2")
                + cm.NumConfig(Update_CO_TWA.Checked, n_CO_TWA.Value, "TWA", "A2")
                + cm.NumConfig(Update_CO_STEL.Checked, n_CO_STEL.Value, "STEL", "A2")
                + cm.NumConfig(Update_CO_CalInterval.Checked, n_CO_CalInterval.Value, "Calibration Interval", "A2")
                + cm.NumConfig(Update_CO_BumpInterval.Checked, n_CO_BumpInterval.Value, "Bump Interval", "A2")
                + cm.NumConfig(Update_CO_STELInterval.Checked, n_CO_STELInterval.Value, "STEL Interval", "A2")
                + cm.NumConfig(Update_CO_LastCal.Checked, n_CO_LastCal.Value, "Last Calibration", "A2")
                + cm.NumConfig(Update_CO_LastBump.Checked, n_CO_LastBump.Value, "Last Bump", "A2")
                + cm.NumConfig(Update_CO_CalGasConc.Checked, n_CO_CalGasConc.Value, "Calibration Gas Conc", "A2")
                + cm.NumConfig(Update_CO_CalRange.Checked, n_CO_CalRange.Value, "Calibration Range", "A2")
                + cm.NumConfig(Update_CO_BumpThreshold.Checked, n_CO_BumpThreshold.Value, "Bump Threshold", "A2")
                + cm.NumConfig(Update_CO_Interval.Checked, n_CO_Interval.Value, "Interval", "A2")
                + cm.NumConfig(Update_CO_PulseWidth.Checked, n_CO_PulseWidth.Value, "Pulse Width", "A2")
                + cm.NumConfig(Update_CO_DecayTime.Checked, n_CO_DecayTime.Value, "Decay Time", "A2")
                + cm.NumConfig(Update_CO_DecayThreshold.Checked, n_CO_DecayThreshold.Value, "Decay Threshold", "A2")
                + cm.NumConfig(Update_CO_RecoveryTime.Checked, n_CO_RecoveryTime.Value, "Recovery Time", "A2");

            //O2
            tB_Config_data.Text += cm.BoolConfig(Update_O2_SensorDisabled.Checked, cB_O2_SensorDisabled.Checked, "Sensor Disabled", "A3")
                + cm.NumConfig(Update_O2_Low.Checked, O2_Low.Value, "Low", "A3")
                + cm.NumConfig(Update_O2_High.Checked, O2_High.Value, "High", "A3")
                + cm.NumConfig(Update_O2_TriggerLow.Checked, O2_Trigger_Low.SelectedIndex, "Low Trigger", "A3")
                + cm.NumConfig(Update_O2_TriggerHigh.Checked, O2_Trigger_High.SelectedIndex, "High Trigger", "A3")
                + cm.NumConfig(Update_O2_CalInterval.Checked, O2_CalInterval.Value, "Calibration Interval", "A3")
                + cm.NumConfig(Update_O2_BumpInterval.Checked, O2_BumpInterval.Value, "Bump Interval", "A3")
                + cm.NumConfig(Update_O2_LastCal.Checked, O2_LastCal.Value, "Last Calibration", "A3")
                + cm.NumConfig(Update_O2_lastBump.Checked, O2_LastBump.Value, "Last Bump", "A3")
                + cm.NumConfig(Update_O2_CalGasConc.Checked, O2_CalGasConc.Value, "Calibration Gas Conc", "A3")
                + cm.NumConfig(Update_O2_CalRange.Checked, O2_CalRange.Value, "Calibration Range", "A3")
                + cm.NumConfig(Update_O2_BumpThreshold.Checked, O2_BumpThreshold.Value, "Bump Threshold", "A3");

            //LEL
            tB_Config_data.Text += cm.BoolConfig(Update_LEL_SensorDisabled.Checked, cB_LEL_SensorDisabled.Checked, "Sensor Disabled", "A4")
                + cm.NumConfig(Update_LEL_Low.Checked, LEL_Low.Value, "Low(LEL)", "A4")
                + cm.NumConfig(Update_LEL_High.Checked, LEL_High.Value, "High(LEL)", "A4")
                + cm.NumConfig(Update_LEL_CalInterval.Checked, LEL_CalInterval.Value, "Calibration Interval", "A4")
                + cm.NumConfig(Update_LEL_BumpInterval.Checked, LEL_BumpInterval.Value, "Bump Interval", "A4")
                + cm.NumConfig(Update_LEL_LastCal.Checked, LEL_LastCal.Value, "Last Calibration", "A4")
                + cm.NumConfig(Update_LEL_LastBump.Checked, LEL_LastBump.Value, "Last Bump", "A4")
                + cm.NumConfig(Update_LEL_CalGasConc.Checked, LEL_CalGasConc.Value, "Calibration Gas Conc(LEL)", "A4")
                + cm.NumConfig(Update_LEL_CalRange.Checked, LEL_CalRange.Value, "Calibration Range", "A4")
                + cm.NumConfig(Update_LEL_BumpThreshold.Checked, LEL_BumpThreshold.Value, "Bump Threshold", "A4")
                + cm.BoolConfig(Update_LEL_Volume.Checked, cB_LEL_Volume.Checked, "Volume", "A4")
                + cm.BoolConfig(Update_LEL_CSA.Checked, cB_LEL_CSA.Checked, "CSA", "A4")
                + cm.NumConfig(Update_LEL_VolumeFactor.Checked, LEL_VolumeFactor.SelectedIndex, "Volume Factor", "A4");

        }

        //Send Config Data Button Click
        private void btn_sendConfigData_Click(object sender, EventArgs e)
        {
            if (sport == null)
            {
                MessageBox.Show("open the port");
            }
            else
            {
                tB_ParsedCommand_con.Text = cm.make_cmd(tB_Config_data.Text, tB_Config_data.TextLength);
                write_log(tB_ParsedCommand_con.Text);
                timer2.Enabled = true;
                sport.Write(tB_ParsedCommand_con.Text);
            }
        }

        //port read fail
        private void timer2_tick(object sender, EventArgs e)
        {
            if (monitoring == true)
            {
                timer1.Enabled = false;
                monitoring = false;
                Mtr_sw.Close();
                Mtr_fs.Close();
                timer2.Interval = 1500;
                btn_MonitoringStop.Enabled = false;
                btn_MonitoringStart.Enabled = true;
                tabControl1.TabPages[0].Enabled = true;
                tabControl1.TabPages[1].Enabled = true;
                tabControl1.TabPages[2].Enabled = true;
                tabControl1.TabPages[3].Enabled = true;
            }

            timer2.Enabled = false;
            MessageBox.Show("Failed. Please retry.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            btn_flag = "";
        }

        //Load H2S / Load CO / Load O2 / Load LEL
        private void btn_LoadH2S_Click(object sender, EventArgs e)
        {
            if (sport == null)
            {
                MessageBox.Show("open the port");
            }
            else if (btn_flag == "")
            {
                btn_flag = "H2S";
                tabControl3.SelectedIndex = 1;
                tB_Log.Text = "";
                tB_ParsedCommand_con.Text = "<a0180010072A100?;A124?;A125?;A134?;A135?;A107?;A110?;A136?;A152?;A153?;A139?;A106?;A157?;A128?;A149?;A14A?;A14B?;A14C?;A14D?;02E9>";
                sport.Write(tB_ParsedCommand_con.Text);
                write_log(tB_ParsedCommand_con.Text);
                timer2.Enabled = true;

            }
        }

        private void btn_LoadCO_Click(object sender, EventArgs e)
        {


            if (sport == null)
            {
                MessageBox.Show("open the port");
            }
            else if (btn_flag == "")
            {
                tB_Log.Text = "";
                btn_flag = "CO";
                tabControl3.SelectedIndex = 3;
                tB_ParsedCommand_con.Text = "<a018001006CA200?;A224?;A225?;A234?;A235?;A207?;A210?;A236?;A252?;A253?;A206?;A257?;A228?;A249?;A24A?;A24B?;A24C?;A24D?;13BE>";
                sport.Write(tB_ParsedCommand_con.Text);
                write_log(tB_ParsedCommand_con.Text);
                timer2.Enabled = true;
            }
        }

        private void btn_LoadO2_Click(object sender, EventArgs e)
        {
            if (sport == null)
            {
                MessageBox.Show("open the port");
            }
            else if (btn_flag == "")
            {
                btn_flag = "O2";
                tB_Log.Text = "";
                tabControl3.SelectedIndex = 5;
                tB_ParsedCommand_con.Text = "<a0180010048A300?;A324?;A325?;A326?;A327?;A307?;A310?;A352?;A353?;A306?;A357?;A328?;F5CF>";
                sport.Write(tB_ParsedCommand_con.Text);
                write_log(tB_ParsedCommand_con.Text);
                timer2.Enabled = true;
            }
        }

        private void btn_LoadLEL_Click(object sender, EventArgs e)
        {
            if (sport == null)
            {
                MessageBox.Show("open the port");
            }
            else if (btn_flag == "")
            {
                btn_flag = "LEL";
                tB_Log.Text = "";
                tabControl3.SelectedIndex = 7;
                tB_ParsedCommand_con.Text = "<a018001004EA400?;A424?;A425?;A407?;A410?;A452?;A453?;A406?;A457?;A428?;A445?;A447?;A444?;E56E>";
                sport.Write(tB_ParsedCommand_con.Text);
                write_log(tB_ParsedCommand_con.Text);
                timer2.Enabled = true;
            }
        }

        public void Load_LEL(string s)
        {
            LELLoad_SensorDisabled.Checked = false;
            LELLoad_Volume.Checked = false;
            LELLoad_CSA.Checked = false;

            string[] result = new string[13];
            string[] parse = s.Split(';');
            for (int i = 0; i < 13; i++)
            {
                result[i] = parse[i].Split('=')[1];
            }
            if (result[0].Equals("1"))
            {
                LELLoad_SensorDisabled.Checked = true;
            }
            LELLoad_Low.Text = (Convert.ToUInt32(result[1], 16) / 5).ToString();
            LELLoad_High.Text = (Convert.ToUInt32(result[2], 16) / 5).ToString();
            LELLoad_CalInterval.Text = Convert.ToUInt16(result[3], 16).ToString();
            LELLoad_BumpInterval.Text = Convert.ToUInt16(result[4], 16).ToString();
            LELLoad_LastCal.Text = Convert.ToUInt32(result[5], 16).ToString();
            LELLoad_LastBump.Text = Convert.ToUInt32(result[6], 16).ToString();
            LELLoad_CalGasConc.Text = (Convert.ToInt32(result[7], 16) / 5).ToString();
            LELLoad_CalRange.Text = Convert.ToUInt16(result[8], 16).ToString();
            LELLoad_BumpThreshold.Text = Convert.ToUInt16(result[9], 16).ToString();
            if (result[10].Equals("1"))
            {
                LELLoad_Volume.Checked = true;
            }
            if (result[11].Equals("1"))
            {
                LELLoad_CSA.Checked = true;
            }
            LELLoad_VolumeFactor.Text = ((float)Convert.ToInt32(result[12].Substring(0, result[12].Length - 5), 16) / 100).ToString();


        }

        public void Load_O2(string s)
        {
            cB_O2Load_SensorDisabled.Checked = false;

            string[] result = new string[12];
            string[] parse = s.Split(';');
            for (int i = 0; i < 12; i++)
            {
                result[i] = parse[i].Split('=')[1];
            }
            if (result[0].Equals("1"))
            {
                cB_O2Load_SensorDisabled.Checked = true;
            }
            O2Load_Low.Text = ((float)Convert.ToUInt32(result[1], 16) / 100).ToString();
            O2Load_High.Text = ((float)Convert.ToUInt32(result[2], 16) / 100).ToString();
            O2Load_Trigger_Low.SelectedIndex = Convert.ToUInt16(result[3]);
            O2Load_Trigger_High.SelectedIndex = Convert.ToUInt16(result[4]);
            O2Load_CalInterval.Text = Convert.ToUInt16(result[5], 16).ToString();
            O2Load_BumpInterval.Text = Convert.ToUInt16(result[6], 16).ToString();
            O2Load_LastCal.Text = Convert.ToUInt32(result[7], 16).ToString();
            O2Load_LastBump.Text = Convert.ToUInt32(result[8], 16).ToString();
            O2Load_CalGasConc.Text = ((float)Convert.ToInt32(result[9], 16) / 100).ToString();
            O2Load_CalRange.Text = Convert.ToUInt16(result[10], 16).ToString();
            O2Load_BumpThreshold.Text = Convert.ToUInt16(result[11].Substring(0, result[11].Length - 5), 16).ToString();
        }

        public void Load_CO(string s)
        {
            cB_COLoad_SensorDisabled.Checked = false;

            string[] result = new string[18];
            string[] parse = s.Split(';');
            for (int i = 0; i < 18; i++)
            {
                result[i] = parse[i].Split('=')[1];
            }
            if (result[0].Equals("1"))
            {
                cB_COLoad_SensorDisabled.Checked = true;
            }
            COLoad_Low.Text = (Convert.ToUInt32(result[1], 16) / 100).ToString();
            COLoad_High.Text = (Convert.ToUInt32(result[2], 16) / 100).ToString();
            COLoad_TWA.Text = (Convert.ToUInt32(result[3], 16) / 100).ToString();
            COLoad_STEL.Text = (Convert.ToUInt32(result[4], 16) / 100).ToString();
            COLoad_CalInterval.Text = Convert.ToUInt16(result[5], 16).ToString();
            COLoad_BumpInterval.Text = Convert.ToUInt16(result[6], 16).ToString();
            COLoad_STELInterval.Text = Convert.ToUInt16(result[7], 16).ToString();
            COLoad_LastCal.Text = Convert.ToUInt32(result[8], 16).ToString();
            COLoad_LastBump.Text = Convert.ToUInt32(result[9], 16).ToString();
            COLoad_CalGasConc.Text = (Convert.ToInt32(result[10], 16) / 100).ToString();
            COLoad_CalRange.Text = Convert.ToUInt16(result[11], 16).ToString();
            COLoad_BumpThreshold.Text = Convert.ToUInt16(result[12], 16).ToString();
            COLoad_Interval.Text = Convert.ToUInt16(result[13], 16).ToString();
            COLoad_PulseWidth.Text = Convert.ToUInt16(result[14], 16).ToString();
            COLoad_DecayTime.Text = Convert.ToUInt16(result[15], 16).ToString();
            COLoad_DecayThreshold.Text = Convert.ToInt16(result[16], 16).ToString();
            COLoad_RecoveryTime.Text = Convert.ToUInt16(result[17].Substring(0, result[17].Length - 5), 16).ToString();
        }

        public void Load_H2S(string s)
        {

            cB_H2SLoad_SensorDisabled.Checked = false;
            cB_H2SLoad_DisplayDecimal.Checked = false;

            string[] result = new string[19];
            string[] parse = s.Split(';');
            for (int i = 0; i < 19; i++)
            {
                result[i] = parse[i].Split('=')[1];
            }
            if (result[0].Equals("1"))
            {
                cB_H2SLoad_SensorDisabled.Checked = true;
            }
            H2SLoad_Low.Text = (Convert.ToUInt32(result[1], 16) / 100).ToString();
            H2SLoad_High.Text = (Convert.ToUInt32(result[2], 16) / 100).ToString();
            H2SLoad_TWA.Text = (Convert.ToUInt32(result[3], 16) / 100).ToString();
            H2SLoad_STEL.Text = (Convert.ToUInt32(result[4], 16) / 100).ToString();
            H2SLoad_CalInterval.Text = Convert.ToUInt16(result[5], 16).ToString();
            H2SLoad_BumpInterval.Text = Convert.ToUInt16(result[6], 16).ToString();
            H2SLoad_STELInterval.Text = Convert.ToUInt16(result[7], 16).ToString();
            H2SLoad_LastCal.Text = Convert.ToUInt32(result[8], 16).ToString();
            H2SLoad_LastBump.Text = Convert.ToUInt32(result[9], 16).ToString();
            if (result[10].Equals("1"))
            {
                cB_H2SLoad_DisplayDecimal.Checked = true;
            }
            H2SLoad_CalGasConc.Text = (Convert.ToInt32(result[11], 16) / 100).ToString();
            H2SLoad_CalRange.Text = Convert.ToUInt16(result[12], 16).ToString();
            H2SLoad_BumpThreshold.Text = Convert.ToUInt16(result[13], 16).ToString();
            H2SLoad_Interval.Text = Convert.ToUInt16(result[14], 16).ToString();
            H2SLoad_PulseWidth.Text = Convert.ToUInt16(result[15], 16).ToString();
            H2SLoad_DecayTime.Text = Convert.ToUInt16(result[16], 16).ToString();
            H2SLoad_DecayThreshold.Text = Convert.ToInt16(result[17], 16).ToString();
            H2SLoad_RecoveryTime.Text = Convert.ToUInt16(result[18].Substring(0, result[18].Length - 5), 16).ToString();
        }

        //Load User Option
        public void usrOptions_Indicator(string s)
        {
            
            rB_Stealth.Checked = false;
            rB_AlarmLatch.Checked = false;
            rB_Blankzone.Checked = false;
            rB_TravelModeDisabled.Checked = false;
            rB_serviceMode.Checked = false;

            string[] result = new string[7];
            string[] parse = s.Split(';');
            for (int i = 0; i < 7; i++)
            {
                
                if (i != 4&&parse[i].Contains('=')) 
                {
                    result[i] = parse[i].Split('=')[1];

                }
                else
                {
                    result[i] = "";
                }
            }
            if (result[0] != "" && result[0].Contains("1"))
            {
                rB_Stealth.Checked = true;
            }
            if (result[1] != "" && result[1].Contains("1"))
            {
                rB_AlarmLatch.Checked = true;
            }
            if (result[3] != "" && result[3].Contains("1"))
            {
                rB_Blankzone.Checked = true;
            }
            if (result[5] != "" && result[5].Contains("1"))
            {
                rB_TravelModeDisabled.Checked = true;
            }
            if (result[6]!=""&&result[6].Substring(0, 1).Contains("1"))
            {
                rB_serviceMode.Checked = true;
            }
            domainUpDown_LatchTime.Text = Convert.ToUInt16(result[2], 16).ToString();
        }

        private void btn_load_user_option_Click(object sender, EventArgs e)
        {

            if (sport == null)
            {
                MessageBox.Show("open the port");
            }
            else if (btn_flag == "")
            {
                btn_flag = "User Option";
                tabControl2.SelectedIndex = 1;
                tB_ParsedCommand_con.Text = "<a018001002A506A?;5069?;5064?;5068?;5075?;507A?;507B?;B87C>";
                sport.Write(tB_ParsedCommand_con.Text);
                timer2.Enabled = true;
                write_log(tB_ParsedCommand_con.Text);
            }

        }

        //Save Config Data Button Click
        private void btn_SaveConfigData_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "모든파일|*.*";
            saveFileDialog1.Title = "Save Config Data";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = (FileStream)saveFileDialog1.OpenFile();
                StreamWriter s = new StreamWriter(fs);
                s.WriteLine(tB_ParsedCommand_con.Text);
                s.Close();
                fs.Close();
            }
        }

        //********************** User Config Tab ***********************//

        //Updatable CheckBox (Baseline Temp Comp./Span Temp Comp./Linearity Scale Factor)
        public void User_Config_Updatable_CheckedChaged(object sender, EventArgs e)
        {
            tB_UserConfigdata.Clear();
            decimal[] H2S_B = { H2S_B1.Value, H2S_B2.Value, H2S_B3.Value, H2S_B4.Value, H2S_B5.Value, H2S_B6.Value, H2S_B7.Value, H2S_B8.Value, H2S_B9.Value, H2S_B10.Value, H2S_B11.Value, H2S_B12.Value };
            decimal[] H2S_S = { H2S_S1.Value, H2S_S2.Value, H2S_S3.Value, H2S_S4.Value, H2S_S5.Value, H2S_S6.Value, H2S_S7.Value, H2S_S8.Value, H2S_S9.Value, H2S_S10.Value, H2S_S11.Value, H2S_S12.Value };
            decimal[] H2S_L = { H2S_L1.Value, H2S_L2.Value, H2S_L3.Value, H2S_L4.Value, H2S_L5.Value, H2S_L6.Value, H2S_L7.Value, H2S_L8.Value, H2S_L9.Value, H2S_L10.Value, H2S_L11.Value, H2S_L12.Value };
            decimal[] CO_B = { CO_B1.Value, CO_B2.Value, CO_B3.Value, CO_B4.Value, CO_B5.Value, CO_B6.Value, CO_B7.Value, CO_B8.Value, CO_B9.Value, CO_B10.Value, CO_B11.Value, CO_B12.Value };
            decimal[] CO_S = { CO_S1.Value, CO_S2.Value, CO_S3.Value, CO_S4.Value, CO_S5.Value, CO_S6.Value, CO_S7.Value, CO_S8.Value, CO_S9.Value, CO_S10.Value, CO_S11.Value, CO_S12.Value };
            decimal[] CO_L = { CO_L1.Value, CO_L2.Value, CO_L3.Value, CO_L4.Value, CO_L5.Value, CO_L6.Value, CO_L7.Value, CO_L8.Value, CO_L9.Value, CO_L10.Value, CO_L11.Value, CO_L12.Value };
            decimal[] O2_B = { O2_B1.Value, O2_B2.Value, O2_B3.Value, O2_B4.Value, O2_B5.Value, O2_B6.Value, O2_B7.Value, O2_B8.Value, O2_B9.Value, O2_B10.Value, O2_B11.Value, O2_B12.Value };
            decimal[] O2_S = { O2_S1.Value, O2_S2.Value, O2_S3.Value, O2_S4.Value, O2_S5.Value, O2_S6.Value, O2_S7.Value, O2_S8.Value, O2_S9.Value, O2_S10.Value, O2_S11.Value, O2_S12.Value };
            decimal[] O2_L = { O2_L1.Value, O2_L2.Value, O2_L3.Value, O2_L4.Value, O2_L5.Value, O2_L6.Value, O2_L7.Value, O2_L8.Value, O2_L9.Value, O2_L10.Value, O2_L11.Value, O2_L12.Value };
            decimal[] LEL_B = { LEL_B1.Value, LEL_B2.Value, LEL_B3.Value, LEL_B4.Value, LEL_B5.Value, LEL_B6.Value, LEL_B7.Value, LEL_B8.Value, LEL_B9.Value, LEL_B10.Value, LEL_B11.Value, LEL_B12.Value };
            decimal[] LEL_S = { LEL_S1.Value, LEL_S2.Value, LEL_S3.Value, LEL_S4.Value, LEL_S5.Value, LEL_S6.Value, LEL_S7.Value, LEL_S8.Value, LEL_S9.Value, LEL_S10.Value, LEL_S11.Value, LEL_S12.Value };
            decimal[] LEL_L = { LEL_L1.Value, LEL_L2.Value, LEL_L3.Value, LEL_L4.Value, LEL_L5.Value, LEL_L6.Value, LEL_L7.Value, LEL_L8.Value, LEL_L9.Value, LEL_L10.Value, LEL_L11.Value, LEL_L12.Value };

            //User Config Option
            tB_UserConfigdata.Text += cm.BoolConfig(Update_Acceleration.Checked, Acceleration.Checked, "Acceleration", "")
                + cm.BoolConfig(Update_BlankZone.Checked, BlankZone.Checked, "Blank Zone", "")
                + cm.NumConfig(Update_TempCalTarget.Checked, TempCalTarget.Value, "Temp Calibration Target", "");
            //H2S
            tB_UserConfigdata.Text += cm.NumConfig(Update_H2S_SpanTime.Checked, H2S_SpanTime.Value, "H2S Span Time", "A1")
                + cm.GasUserConfig(Update_H2S_B.Checked, H2S_B, "Baseline Temp Comp.", "A1")
                + cm.GasUserConfig(Update_H2S_S.Checked, H2S_S, "Span Temp Comp.", "A1")
                + cm.GasUserConfig(Update_H2S_L.Checked, H2S_L, "Linearity Scale Factor", "A1");
            //CO
            tB_UserConfigdata.Text += cm.NumConfig(Update_CO_SpanTime.Checked, CO_SpanTime.Value, "CO Span Time", "A2")
                + cm.GasUserConfig(Update_CO_B.Checked, CO_B, "Baseline Temp Comp.", "A2")
                + cm.GasUserConfig(Update_CO_S.Checked, CO_S, "Span Temp Comp.", "A2")
                + cm.GasUserConfig(Update_CO_L.Checked, CO_L, "Linearity Scale Factor", "A2");

            //O2
            tB_UserConfigdata.Text += cm.NumConfig(Update_O2_SpanTime.Checked, O2_SpanTime.Value, "O2 Span Time", "A3")
               + cm.GasUserConfig(Update_O2_B.Checked, O2_B, "Baseline Temp Comp.", "A3")
               + cm.GasUserConfig(Update_O2_S.Checked, O2_S, "Span Temp Comp.", "A3")
               + cm.GasUserConfig(Update_O2_L.Checked, O2_L, "Linearity Scale Factor", "A3");

            //LEL
            tB_UserConfigdata.Text += cm.NumConfig(Update_LEL_SpanTime.Checked, LEL_SpanTime.Value, "LEL Span Time", "A4")
               + cm.GasUserConfig(Update_LEL_B.Checked, LEL_B, "Baseline Temp Comp.", "A4")
               + cm.GasUserConfig(Update_LEL_S.Checked, LEL_S, "Span Temp Comp.", "A4")
               + cm.GasUserConfig(Update_LEL_L.Checked, LEL_L, "Linearity Scale Factor", "A4");

        }

        //Send Config Data Button Click
        private void btn_SendUserConfig_Click(object sender, EventArgs e)
        {
            if (sport == null)
            {
                MessageBox.Show("open the port");
            }
            else
            {
                tB_ParsedUserConfig.Text = cm.make_cmd(tB_UserConfigdata.Text, tB_UserConfigdata.TextLength);
                write_log(tB_ParsedUserConfig.Text);
                sport.Write(tB_ParsedUserConfig.Text);
            }
        }

        //(Logging Interval) Send Button Click
        private void btn_send_LoggingInterval_Click(object sender, EventArgs e)
        {
            if (sport == null)
            {
                MessageBox.Show("open the port");
            }
            else
            {
                string cmd = cm.NumConfig(true, LoggingInterval.Value, "Logging Interval", "");
                tB_ParsedUserConfig.Text = cm.make_cmd(cmd, cmd.Length);
                sport.Write(tB_ParsedUserConfig.Text);
                write_log(tB_ParsedUserConfig.Text);
            }
        }

        //(File Type) Send Button Click
        private void btn_send_FileType_Click(object sender, EventArgs e)
        {
            if (sport == null)
            {
                MessageBox.Show("open the port");
            }
            else
            {
                string cmd = cm.NumConfig(true, FileType.SelectedIndex, "File Type", "");
                tB_ParsedUserConfig.Text = cm.make_cmd(cmd, cmd.Length);
                sport.Write(tB_ParsedUserConfig.Text);
                write_log(tB_ParsedUserConfig.Text);
            }
        }

        //************************ Sensor Tab *************************//

        //Clear Button Click
        private void btn_clear_sensor_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView2.Items.Clear();
            listView3.Items.Clear();
            listView4.Items.Clear();
            listView5.Items.Clear();
            listView6.Items.Clear();
            listView7.Items.Clear();
            listView8.Items.Clear();
            listView9.Items.Clear();
            listView10.Items.Clear();
            listView11.Items.Clear();
        }

        //Sensor Configuration
        public void load_sensor(string data)
        {
            string[] result = new string[4];
            string[] parse = data.Split(';');
            ListViewItem lvi = new ListViewItem();
            for (int i = 0; i < 4; i++)
            {
                result[i] = parse[i].Split('=')[1];
            }
            switch (Sensor_Config.SelectedIndex)
            {
                case 1:
                    listView1.Items.Clear();
                    listView1.Items.Add(Convert.ToInt16(result[0], 16).ToString());
                    listView1.Items.Add(Convert.ToInt16(result[1], 16).ToString());
                    listView1.Items.Add(Convert.ToInt16(result[2], 16).ToString());
                    listView1.Items.Add(Convert.ToInt16(result[3].Substring(0, result[3].Length - 5), 16).ToString());
                    break;
                case 2:
                    listView2.Items.Clear();
                    listView2.Items.Add(Convert.ToInt32(result[0], 16).ToString());
                    listView2.Items.Add(Convert.ToInt32(result[1], 16).ToString());
                    listView2.Items.Add(Convert.ToInt32(result[2], 16).ToString());
                    listView2.Items.Add(Convert.ToInt32(result[3].Substring(0, result[3].Length - 5), 16).ToString());
                    break;
                case 3:
                    listView3.Items.Clear();
                    listView3.Items.Add(Convert.ToInt32(result[0], 16).ToString());
                    listView3.Items.Add(Convert.ToInt32(result[1], 16).ToString());
                    listView3.Items.Add(Convert.ToInt32(result[2], 16).ToString());
                    listView3.Items.Add(Convert.ToInt32(result[3].Substring(0, result[3].Length - 5), 16).ToString());
                    break;
                case 4:
                    listView4.Items.Clear();
                    listView4.Items.Add(Convert.ToInt32(result[0], 16).ToString());
                    listView4.Items.Add(Convert.ToInt32(result[1], 16).ToString());
                    listView4.Items.Add(Convert.ToInt32(result[2], 16).ToString());
                    listView4.Items.Add(Convert.ToInt32(result[3].Substring(0, result[3].Length - 5), 16).ToString());
                    break;
                case 5:
                    listView5.Items.Clear();
                    listView5.Items.Add(Convert.ToUInt16(result[0], 16).ToString());
                    listView5.Items.Add(Convert.ToUInt16(result[1], 16).ToString());
                    listView5.Items.Add(Convert.ToUInt16(result[2], 16).ToString());
                    listView5.Items.Add(Convert.ToUInt16(result[3].Substring(0, result[3].Length - 5), 16).ToString());
                    break;
                case 6:
                    listView6.Items.Clear();
                    listView6.Items.Add(Convert.ToUInt16(result[0], 16).ToString());
                    listView6.Items.Add(Convert.ToUInt16(result[1], 16).ToString());
                    listView6.Items.Add(Convert.ToUInt16(result[2], 16).ToString());
                    listView6.Items.Add(Convert.ToUInt16(result[3].Substring(0, result[3].Length - 5), 16).ToString());
                    break;
                case 7:
                    listView7.Items.Clear();
                    listView7.Items.Add(Convert.ToUInt32(result[0], 16).ToString());
                    listView7.Items.Add(Convert.ToUInt32(result[1], 16).ToString());
                    listView7.Items.Add(Convert.ToUInt32(result[2], 16).ToString());
                    listView7.Items.Add(Convert.ToUInt32(result[3].Substring(0, result[3].Length - 5), 16).ToString());
                    break;
                case 8:
                    listView8.Items.Clear();
                    listView8.Items.Add(Convert.ToUInt32(result[0], 16).ToString());
                    listView8.Items.Add(Convert.ToUInt32(result[1], 16).ToString());
                    listView8.Items.Add(Convert.ToUInt32(result[2], 16).ToString());
                    listView8.Items.Add(Convert.ToUInt32(result[3].Substring(0, result[3].Length - 5), 16).ToString());
                    break;
                case 9:
                    listView9.Items.Clear();
                    listView9.Items.Add(Convert.ToInt16(result[0], 16).ToString());
                    listView9.Items.Add(Convert.ToInt16(result[1], 16).ToString());
                    listView9.Items.Add(Convert.ToInt16(result[2], 16).ToString());
                    listView9.Items.Add(Convert.ToInt16(result[3].Substring(0, result[3].Length - 5), 16).ToString());
                    break;
                case 10:
                    listView10.Items.Clear();
                    listView10.Items.Add(Convert.ToInt32(result[0], 16).ToString());
                    listView10.Items.Add(Convert.ToInt32(result[1], 16).ToString());
                    listView10.Items.Add(Convert.ToInt32(result[2], 16).ToString());
                    listView10.Items.Add(Convert.ToInt32(result[3].Substring(0, result[3].Length - 5), 16).ToString());
                    break;
                case 11:
                    listView11.Items.Clear();
                    listView11.Items.Add(Convert.ToUInt16(result[0], 16).ToString());
                    listView11.Items.Add(Convert.ToUInt16(result[1], 16).ToString());
                    listView11.Items.Add(Convert.ToUInt16(result[2], 16).ToString());
                    listView11.Items.Add(Convert.ToUInt16(result[3].Substring(0, result[3].Length - 5), 16).ToString());
                    break;
                default: break;
            }
        }

        private void Sensor_Config_SelectedIndexChanged(object sender, EventArgs e)
        {
            tB_sensor_cmd.Text = cm.Sensor_command(Sensor_Config.Text);
            tB_ParsedCmd_Sensor.Text = cm.make_cmd(tB_sensor_cmd.Text, tB_sensor_cmd.TextLength);
        }

        //Send Button Click
        private void btn_Send_sensor_Click(object sender, EventArgs e)
        {
            if (sport == null)
            {
                MessageBox.Show("open the port");
            }
            else
            {
                btn_flag = "sensor";
                int length = tB_ParsedCmd_Sensor.TextLength;
                write_log(tB_ParsedCmd_Sensor.Text);
                timer2.Enabled = true;
                sport.Write(tB_ParsedCmd_Sensor.Text);
            }
        }

        //Calibration Button Click
        private void btn_Calibration_Click(object sender, EventArgs e)
        {
            if (sport == null)
            {
                MessageBox.Show("open the port");
            }
            else
            {
                tB_ParsedUserConfig.Text = "<a0180010005506D;0594>";
                write_log(tB_ParsedUserConfig.Text);
                sport.Write(tB_ParsedUserConfig.Text);
            }
        }

        //Set Temp Button click
        private void btn_SetTemp_Click(object sender, EventArgs e)
        {
            if (sport == null)
            {
                MessageBox.Show("open the port");
            }
            else
            {
                string cmd = cm.NumConfig(true, TempCalTarget.Value, "Temp Calibration Target", "");
                cmd = cmd.Substring(0, cmd.Length - 6);
                tB_ParsedUserConfig.Text = cm.make_cmd(cmd, cmd.Length);
                sport.Write(tB_ParsedUserConfig.Text);
                write_log(tB_ParsedUserConfig.Text);
            }
        }

        //************************ Monitoring Tab *************************//

        //fill data, write file
        public void Fill_Data(string s)
        {
            try
            {
                string[] result = new string[14];
                string[] parse = s.Split(';');
                for (int i = 0; i < 14; i++)
                {
                    if (parse[i].Contains('=')) { result[i] = parse[i].Split('=')[1]; }
                    else { result[i] = ""; }
                }
                listView22.Items.Clear();
                listView22.Items.Add("Ch #1_H2S");
                listView22.Items.Add("Ch #2_CO");
                listView22.Items.Add("Ch #3_O2");
                listView22.Items.Add("Ch #4_FL");
                if (!result[12].Equals("")) { result[12] = Convert.ToUInt32(result[12], 16).ToString(); }
                if (!result[13].Equals("")) { result[13] = Convert.ToUInt16(result[13].Substring(0, result[13].Length - 5), 16).ToString(); }
                for (int i = 0; i < 4; i++)
                {

                    if (!result[i].Equals("")) { result[i] = Convert.ToInt16(result[i], 16).ToString(); }
                    if (!result[i + 4].Equals(""))
                    {
                        int resolution = 0;
                        float tmp = Convert.ToInt32(result[i + 4], 16);
                        switch (i)
                        {
                            case 0: resolution = H2S_Resolution.SelectedIndex; tmp /= 100; break;
                            case 1: resolution = CO_Resolution.SelectedIndex; tmp /= 100; break;
                            case 2: resolution = O2_Resolution.SelectedIndex; tmp /= 100; break;
                            case 3: resolution = FL_Resolution.SelectedIndex; tmp /= 5; break;
                        }
                        switch (resolution)
                        {
                            case 0: result[i + 4] = String.Format("{0:0}", tmp); break;
                            case 1: result[i + 4] = String.Format("{0:0.0}", tmp); break;
                            case 2: result[i + 4] = String.Format("{0:0.00}", tmp); break;
                            case 3: result[i + 4] = String.Format("{0:0.000}", tmp); break;
                        }
                    }

                    if (!result[i + 8].Equals(""))
                    {
                        float tmp = Convert.ToUInt32(result[i + 8], 16);
                        result[i + 8] = String.Format("{0:0.0}", (tmp / 100));
                    }


                    listView22.Items[i].SubItems.Add(result[i]);
                    listView22.Items[i].SubItems.Add(result[i + 4]);
                    listView22.Items[i].SubItems.Add(result[i + 8]);
                    listView22.Items[i].SubItems.Add("");
                    listView22.Items[i].SubItems.Add("");
                    listView22.Items[i].SubItems.Add(result[12]);
                    listView22.Items[i].SubItems.Add(result[13]);

                }

                Graph_table.Rows.Add(WritelogTime.ToString("yyyy/MM/dd HH:mm:ss.fff"), result[0], result[4], result[8],
                    result[1], result[5], result[9],
                    result[2], result[6], result[10],
                    result[3], result[7], result[11],
                    result[12], result[13]);

                //graph 180개 data만 표시
                if (Graph_table.Rows.Count > 180)
                {
                    Graph_table.Rows.RemoveAt(0);
                }
                if (graph_popup.Visible == true)
                {
                    graph_popup.Passvalue = Graph_table;
                }


                listView21.Items.Add(WritelogTime.ToString("yyyy/MM/dd HH:mm:ss.fff"));
                int n = listView21.Items.Count - 1;
                for (int i = 0; i < 4; i++)
                {
                    listView21.Items[n].SubItems.Add(result[i]);
                    listView21.Items[n].SubItems.Add(result[i + 4]);
                    listView21.Items[n].SubItems.Add(result[i + 8]);
                }
                listView21.Items[n].SubItems.Add(result[12]);
                listView21.Items[n].SubItems.Add(result[13]);

                if (cB_AutoScroll.Checked)
                {
                    listView21.Items[n].EnsureVisible();
                }
                //File write
                if (Mtr_fs.CanWrite)
                {
                    Mtr_sw.WriteLine(WritelogTime.ToString("yyyy/MM/dd HH:mm:ss.fff") + "," + result[0] + "," + result[4] + "," + result[8] + "," + result[1] + "," +
                                                        result[5] + "," + result[9] + "," + result[2] + "," + result[6] + "," + result[10] + "," + result[3] + "," + result[7] + "," + result[11] + "," + result[12] + "," + result[13]);
                    line_cnt++;
                    if (line_cnt == 20000)
                    {
                        file_cnt++;
                        Mtr_sw.Close();
                        Mtr_fs.Close();

                        Mtr_fs = new FileStream((Mtr_Filepath + "(" + file_cnt + ")"), FileMode.Create);
                        Mtr_sw = new StreamWriter(Mtr_fs);
                        Mtr_sw.WriteLine("Time,Ch#1_H2S_ADC,Ch#1_H2S_Conc,Ch#1_H2S_Status,Ch#2_CO_ADC,Ch#2_CO_Conc,Ch#2_CO_Status,Ch#3_O2_ADC,Ch#3_O2_Conc,Ch#3_O2_Status,Ch#4__FL_ADC,Ch#4_FL_Conc,Ch#4_FL_Status,Volt(mV),Temp(oC)");
                        line_cnt = 0;
                    }
                }

                //Recorded data, Log 초기화
                if (listView21.Items.Count == 180)
                {
                    listView21.Items.Clear();
                    Mtr_Log = String.Empty;
                }

                //Console.WriteLine(listView21.Items.Count);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //Check port connection during Monitoring
        private void timer3_Tick(object sender, EventArgs e)
        {
            btn_MonitoringStart.Enabled = true;
            btn_MonitoringStop.Enabled = false;
            tabControl1.TabPages[0].Enabled = true;
            tabControl1.TabPages[1].Enabled = true;
            tabControl1.TabPages[2].Enabled = true;
            tabControl1.TabPages[3].Enabled = true;
            monitoring = false;
            timer1.Enabled = false;
            Mtr_sw.Close();
            Mtr_fs.Close();
            timer3.Enabled = false;
            MessageBox.Show("Check connection");
        }

        //Monitoring 시작 후 sec마다 command write
        private void timer1_Tick(object sender, EventArgs e)
        {
            btn_flag = "Monitoring";
            string cmd = "<a0180010053A114?;A214?;A314?;A414?;A122?;A222?;A322?;A422?;A13C?;A23C?;A340?;A440?;502F?;5028?36FF>";
            int sec = Convert.ToInt32(nUpDown_Interval.Value) * 1000;

            timer1.Interval = sec;
            write_log(cmd);
            sport.Write(cmd);

            timer3.Interval = 120000;
            timer3.Enabled = true;

        }

        private void btn_MonitoringStart_Click(object sender, EventArgs e)
        {
            if (sport == null)
            {
                MessageBox.Show("open the port");
            }
            else if (monitoring != true)
            {
                btn_flag = "Monitoring";
                listView21.Items.Clear();
                Mtr_Log = string.Empty;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "모든파일|*.*";
                saveFileDialog1.Title = "Save Data";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Mtr_Filepath = saveFileDialog1.FileName;
                    Mtr_fs = (FileStream)saveFileDialog1.OpenFile();
                    Mtr_sw = new StreamWriter(Mtr_fs);
                    Mtr_sw.WriteLine("Time,Ch#1_H2S_ADC,Ch#1_H2S_Conc,Ch#1_H2S_Status,Ch#2_CO_ADC,Ch#2_CO_Conc,Ch#2_CO_Status,Ch#3_O2_ADC,Ch#3_O2_Conc,Ch#3_O2_Status,Ch#4__FL_ADC,Ch#4_FL_Conc,Ch#4_FL_Status,Volt(mV),Temp(oC)");

                    Graph_table = new DataTable();
                    Graph_table.Columns.Add("DateTime");
                    Graph_table.Columns.Add("H2S_ADC");
                    Graph_table.Columns.Add("H2S_Conc");
                    Graph_table.Columns.Add("H2S_State");
                    Graph_table.Columns.Add("CO_ADC");
                    Graph_table.Columns.Add("CO_Conc");
                    Graph_table.Columns.Add("CO_State");
                    Graph_table.Columns.Add("O2_ADC");
                    Graph_table.Columns.Add("O2_Conc");
                    Graph_table.Columns.Add("O2_State");
                    Graph_table.Columns.Add("FL_ADC");
                    Graph_table.Columns.Add("FL_Conc");
                    Graph_table.Columns.Add("FL_State");
                    Graph_table.Columns.Add("Volt");
                    Graph_table.Columns.Add("Temp");
                    monitoring = true;

                    int sec = Convert.ToInt32(nUpDown_Interval.Value) * 1000;
                    btn_flag = "Monitoring";
                    string cmd = "<a0180010053A114?;A214?;A314?;A414?;A122?;A222?;A322?;A422?;A13C?;A23C?;A340?;A440?;502F?;5028?36FF>";

                    write_log(cmd);
                    sport.Write(cmd);

                    timer2.Interval = sec;
                    timer2.Enabled = true;

                    timer1.Interval = sec;
                    timer1.Enabled = true;
                    btn_MonitoringStart.Enabled = false;
                    btn_MonitoringStop.Enabled = true;
                    tabControl1.TabPages[0].Enabled = false;
                    tabControl1.TabPages[1].Enabled = false;
                    tabControl1.TabPages[2].Enabled = false;
                    tabControl1.TabPages[3].Enabled = false;
                }
            }
        }

        private void btn_MonitoringStop_Click(object sender, EventArgs e)
        {
            if (monitoring == true)
            {
                btn_MonitoringStart.Enabled = true;
                btn_MonitoringStop.Enabled = false;
                tabControl1.TabPages[0].Enabled = true;
                tabControl1.TabPages[1].Enabled = true;
                tabControl1.TabPages[2].Enabled = true;
                tabControl1.TabPages[3].Enabled = true;
                monitoring = false;
                timer1.Enabled = false;
                Mtr_sw.Close();
                Mtr_fs.Close();
            }
        }

        private void btn_MonitoringViewLog_Click(object sender, EventArgs e)
        {
            Point parentLocation = this.Location;
            log_popup.StartPosition = FormStartPosition.Manual;
            log_popup.Location = new Point(parentLocation.X + 100, parentLocation.Y + 50);
            log_popup.Visible = true;

        }

        private void btn_MonitoringClear_Click(object sender, EventArgs e)
        {
            listView21.Items.Clear();
            Mtr_Log = String.Empty;
        }

        private void btn_MonitoringGraph_Click(object sender, EventArgs e)
        {
            if (graph_popup.IsDisposed)
            {
                //graph_popup = new Form4();
                graph_popup.Passvalue = Graph_table;
            }
            Point parentLocation = this.Location;
            graph_popup.StartPosition = FormStartPosition.Manual;
            graph_popup.Location = new Point(parentLocation.X, parentLocation.Y + 50);
            graph_popup.Visible = true;
        }


    }
}
