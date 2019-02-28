using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//command 작성
namespace IR_Monitor
{
    public class Command
    {
        public UInt16[] aus_CRC16Table = new UInt16[16] /* 16 bit CRC look-up table */
        {
               0x0000, 0xCC01, 0xD801, 0x1400, 0xF001, 0x3C00, 0x2800, 0xE401,
               0xA001, 0x6C00, 0x7800, 0xB401, 0x5000, 0x9C01, 0x8801, 0x4400
        };
        public UInt32 calculateCRC2(int len, byte[] data, UInt32 us_UpdateCRC16)
        {
            UInt32 us_Remainder = 0;
            int index = 1;
            /* Process only non-zero data array size */
            while (index < len)
            {
                /* Update lower nybble */
                us_Remainder = aus_CRC16Table[us_UpdateCRC16 & 0x0F];
                us_UpdateCRC16 = (us_UpdateCRC16 >> 4) & 0x0FFF;
                us_UpdateCRC16 = (us_UpdateCRC16 ^ us_Remainder) ^ aus_CRC16Table[data[index] & 0x0F];

                /* Update upper nybble */
                us_Remainder = aus_CRC16Table[us_UpdateCRC16 & 0x0F];
                us_UpdateCRC16 = (us_UpdateCRC16 >> 4) & 0x0FFF;
                us_UpdateCRC16 = (us_UpdateCRC16 ^ us_Remainder) ^ aus_CRC16Table[(data[index] >> 4) & 0x0F];

                index++;
            }


            return us_UpdateCRC16;
        }
        public UInt32 calculateCRC(string resultText, UInt32 us_UpdateCRC16)
        {
            char[] buf = new char[resultText.Length + 6];
            buf = resultText.ToCharArray();
            int len = resultText.Length - 1;
            UInt32 us_Remainder = 0;

            /* Process only non-zero data array size */
            while (len > 0)
            {
                /* Update lower nybble */
                us_Remainder = aus_CRC16Table[us_UpdateCRC16 & 0x0F];
                us_UpdateCRC16 = (us_UpdateCRC16 >> 4) & 0x0FFF;
                us_UpdateCRC16 = (us_UpdateCRC16 ^ us_Remainder) ^ aus_CRC16Table[buf[resultText.Length - len] & 0x0F];

                /* Update upper nybble */
                us_Remainder = aus_CRC16Table[us_UpdateCRC16 & 0x0F];
                us_UpdateCRC16 = (us_UpdateCRC16 >> 4) & 0x0FFF;
                us_UpdateCRC16 = (us_UpdateCRC16 ^ us_Remainder) ^ aus_CRC16Table[(buf[resultText.Length - len] >> 4) & 0x0F];
                len--;
            }
            return us_UpdateCRC16;
        }

        public string make_cmd(string enter_cmd, int length)
        {
            string parsedtext = "";
            UInt32 us_UpdateCRC16 = 0xFFFF;


            parsedtext = "<a01";
            parsedtext += "80";
            parsedtext += "01";

            parsedtext += length.ToString("X4") + enter_cmd;
            us_UpdateCRC16 = calculateCRC(parsedtext, us_UpdateCRC16);

            parsedtext += us_UpdateCRC16.ToString("X4");

            parsedtext += ">";


            return parsedtext;
        }

        public string Sensor_command(string input)
        {
            switch (input)
            {
                case "Get Internal Baseline":return "A116?;A216?;A316?;A416?;";
                case "Get Internal Sensitivity":return "A117?;A217?;A317?;A417?;";
                case "Get Internal Blank Zone High":return "A118?;A218?;A318?;A418?;";
                case "Get Internal Blank Zone Low":return "A119?;A219?;A319?;A419?;";
                case "Get Bump Test Threshold":return "A128?;A228?;A328?;A428?;";
                case "Get Sensor Type":return "A131?;A231?;A331?;A431?;";
                case "Get Internal TWA":return "A13C?;A23C?;A33C?;A43C?;";
                case "Get Internal STEL":return "A13D?;A23D?;A33D?;A43D?;";
                case "Get Factory Cal State":return "2A55?;2955?;2855?;2755?;";
                case "Get Factory Sensitivity":return "2A56?;2956?;2856?;2756?;";
                case "Get User Cal Window":return "A157?;A257?;A357?;A457?;";
                default:return "";
            }
        }
        public string Manual_Test_command(string input)
        {
            switch (input)
            {
                case "DOCK_MODE": return "24E3;";
                case "POWER_DOWN":return "24E4;";
                case "Serial number":return "2490?;";
                case "FW version":return "2492?;";
                case "Num sensors":return "249D?;";
                case "Slot assignment":return "2439?;";
                case "Sensor type": return "24A2?;";
                case "Battery type": return "241F?;";
                case "Cal due days": return "2AB0?;";
                case "Cal interval": return "2A21?;";
                case "Bump due days": return "2AB1?;";
                case "Bump interval": return "2A70?;";
                case "Log interval": return "2432?;";
                case "STEL interval": return "2A40?;";
                case "TWA interval": return "2A48?;";
                case "High alarm": return "2A11?;";
                case "Low alarm": return "2A10?;";
                case "TWA alarm": return "2A12?;";
                case "STEL alarm": return "2A13?;";
                case "MIPEX S/N": return "5076?;";
                case "MIPEX FW Ver.": return "5077?;";
                case "Get Reset Reason": return "5078?;";
                case "Turn on LED": return "502D=";
                case "Turn off LED": return "502E=";
                case "Turn on Backlight": return "502C=1;";
                case "Turn off Backlight": return "502C=0;";
                case "Set Beep Frequency": return "501A=";
                case "Turn on Beep": return "501B=01;";
                case "Turn off Beep": return "501B=00;";
                case "Set Beep Duty": return "502B=";
                case "Set Vibrator Frequency": return "5029=";
                case "Set Vibrator Duty": return "502A";
                case "Turn on Vib.": return "501C=1;";
                case "Turn off Vib.": return "501C=0;";
                case "Turn on Mipex Sensor": return "501D=1;";
                case "Turn off Mipex Sensor": return "501D=0;";
                case "Read Temperature": return "5028?;";
                case "Read Battery Voltage": return "502F?;";
                case "Read Push Button Count":return "501E?;";
                case "Read Magnetic Switch Count":return "501F?;";
                case "Read Switch Count":return "24F9?;24FA?;";
                case "Set Vibrator On Time":return "24FB=";
                case "Set Beeper On Time":return "24FC=";
                case "Read Sensor Raw Data":return "A114?;A214?;A314?;A414?;";
                case "Read Gas Conc":return "A122?;A222?;A322?;A422?;";
                case "Read Sensor Status":return "A140?;A240?;A340?;A440?;";
                case "Reflex Text":return "5025=1;";
                case "Set Reflex Pulse":return "503B=0;";
                case "Get Date and Time":return "503C?;";
                case "Get Latched Alarm activation Time":return "5064?;";
                case "Get Confidence Icon Interval":return "5066?;";
                case "Get Deadband Blank Zone Enable":return "5068?;";
                case "Read Bootloader Version":return "5059?;";
                case "Read Shelf Life Time":return "5039?;";
                case "Read Service Life Time":return "5040?;";
                case "Set Shelf Life Time":return "5039=";
                case "Set Service Life Time":return "5040=";
                case "Clear Event Log":return "5011=1;";
                case "Clear Device Log":return "5012=1;";

                default: return "";
            }
        }

        public string BoolConfig(bool updatable, bool enabled,string c,string gas)
        {
            if (updatable)
            {
                string b = "0" + Convert.ToInt32(enabled).ToString() + ";";
                switch (c)
                {
                    case "Stealth": return "506A=" + b;
                    case "Alarm Latching": return "5069=" + b;
                    case "Blank Zone": return "5068=" + b;
                    case "Factory Calibration Mode": return "5075=" + b;
                    case "Travel Mode Disabled": return "507A=" + b;
                    case "Service Mode": return "507B=" + b;
                    case "Sensor Disabled":return gas+"00=" + b;
                    case "Display Decimal":return gas + "39=" + b;
                    case "Volume":return gas + "45=" + b;
                    case "CSA":return gas + "47=" + b;
                    case "Acceleration":return "5056=" + b;
                    default: return "";
                }
            }
            else
            {
                return "";
            }

        }
        public string GasUserConfig(bool updatable,decimal[] num,string c,string gas)
        {
            string config=gas;
            if (updatable)
            {
                switch (c)
                {
                    default:return "";
                       
                    case "Baseline Temp Comp.":
                        config +="4F=";
                        foreach(decimal d in num)
                        {
                            string s = String.Format("{0:X4}", (Convert.ToUInt32(d)));
                            if (s.Length > 4)
                            {
                                s = "000" + s;
                            }
                            config += s+",";
                        }
                        return config.Substring(0, config.Length - 1) + ";";
                    case "Span Temp Comp.":
                        config+="50=";
                        foreach (decimal d in num)
                        {
                            string s = String.Format("{0:X4}", (Convert.ToUInt32(d*10)));
                            if (s.Length > 4)
                            {
                                s = "000" + s;
                            }
                            config += s + ",";
                        }
                        return config.Substring(0,config.Length-1) + ";";
                    case "Linearity Scale Factor":
                        config += "51=";
                        foreach (decimal d in num)
                        {
                            string s = String.Format("{0:X4}", (Convert.ToUInt32(d*10)));
                            if (s.Length > 4)
                            {
                                s = "000" + s;
                            }
                            config += s + ",";
                        }
                        return config.Substring(0, config.Length -1) + ";";

                }
            }
            else
            {
                return "";
            }
        }
        public string NumConfig(bool updatable,decimal num, string c,string gas)
        {
            if (updatable)
            {
                switch (c)
                {
                    case "Latching Activation Time": return "5064="+ String.Format("{0:X4}", Convert.ToUInt16(num))+";";
                    case "Low": return gas + "24=" + String.Format("{0:X8}", Convert.ToUInt32(num * 100))+";";
                    case "Low(LEL)":return gas+"24="+String.Format("{0:X8}", Convert.ToUInt32(num * 5) ) + ";";
                    case "High":return gas + "25=" + String.Format("{0:X8}", Convert.ToUInt32(num * 100)) + ";";
                    case "High(LEL)": return gas + "25=" + String.Format("{0:X8}", Convert.ToUInt32(num * 5)) + ";";
                    case "TWA": return gas + "34=" + String.Format("{0:X8}", Convert.ToUInt32(num * 100)) + ";";
                    case "STEL": return gas + "35=" + String.Format("{0:X8}", Convert.ToUInt32(num * 100) ) + ";";
                    case "Calibration Interval": return gas + "07=" + String.Format("{0:X4}", Convert.ToUInt16(num)) + ";";
                    case "Bump Interval":return gas + "10=" + String.Format("{0:X4}", Convert.ToUInt16(num)) + ";";
                    case "STEL Interval": return gas + "36=" + String.Format("{0:X4}", Convert.ToUInt16(num)) + ";";
                    case "Last Calibration": return gas + "52=" + String.Format("{0:X8}", Convert.ToUInt32(num)) + ";";
                    case "Last Bump": return gas + "53=" + String.Format("{0:X8}", Convert.ToUInt32(num)) + ";";
                    case "Calibration Gas Conc": return gas + "06=" + String.Format("{0:X8}", Convert.ToInt32(num * 100)) + ";";
                    case "Calibration Gas Conc(LEL)": return gas + "06=" + String.Format("{0:X8}", Convert.ToInt32(num * 5) ) + ";";
                    case "Calibration Range":return gas + "57=" + String.Format("{0:X2}", Convert.ToUInt16(num)) + ";";
                    case "Bump Threshold": return gas + "28=" + String.Format("{0:X2}", Convert.ToUInt16(num)) + ";";
                    case "Interval":return gas + "49=" + String.Format("{0:X4}", Convert.ToUInt16(num)) + ";";
                    case "Pulse Width": return gas + "4A=" + String.Format("{0:X4}", Convert.ToUInt16(num)) + ";";
                    case "Decay Time": return gas + "4B=" + String.Format("{0:X4}", Convert.ToUInt16(num)) + ";";
                    case "Decay Threshold": return gas + "4C=" + String.Format("{0:X4}", Convert.ToInt16(num)) + ";";
                    case "Recovery Time": return gas + "4D=" + String.Format("{0:X4}", Convert.ToUInt16(num)) + ";";
                    case "Low Trigger":return gas + "26=" + String.Format("{0:X2}", Convert.ToInt32(num)) + ";" ;
                    case "High Trigger": return gas + "27=" + String.Format("{0:X2}", Convert.ToInt32(num)) + ";";
                    case "Volume Factor": return gas + "44=" + String.Format("{0:X4}", Convert.ToInt32((2.2+(float)num*0.3)*100)) + ";";
                    case "Temp Calibration Target":return "506C=" + String.Format("{0:X4}", Convert.ToUInt16(num * 10)) + ";506D;";
                    case "H2S Span Time":return gas + "4E=" + String.Format("{0:X4}", Convert.ToUInt16(num)) + ";";
                    case "CO Span Time": return gas + "4E=" + String.Format("{0:X4}", Convert.ToUInt16(num)) + ";";
                    case "O2 Span Time": return gas + "4E=" + String.Format("{0:X4}", Convert.ToUInt16(num)) + ";";
                    case "LEL Span Time": return gas + "4E=" + String.Format("{0:X4}", Convert.ToUInt16(num)) + ";";
                    case "Logging Interval": return "506F=" + String.Format("{0:X4}", Convert.ToUInt16(num));
                    case "File Type":return "506E="+ String.Format("{0:X2}", Convert.ToUInt16(num)) ;
                    default: return "";
                }
            }
            else
            {
                return "";
            }
        }
    }
}
