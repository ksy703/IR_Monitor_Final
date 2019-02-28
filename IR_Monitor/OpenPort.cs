using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace IR_Monitor
{
    public class OpenPort
    {

        public Parity GetParity(string p)
        {
            switch (p)
            {
                default: return Parity.None;
                case "Odd": return Parity.Odd;
                case "Even": return Parity.Even;
                case "Mark": return Parity.Mark;
                case "Space": return Parity.Space;

            }
        }
        public StopBits GetStopBits(string s)
        {
            switch (s)
            {
                default: return StopBits.None;
                case "1": return StopBits.One;
                case "2": return StopBits.Two;
            }
        }
        public Handshake GetHandshake(string f)
        {
            switch (f)
            {
                default: return Handshake.None;
                case "XON/XOFF": return Handshake.XOnXOff;
                case "RTS/CTS": return Handshake.RequestToSend;
            }
        }
    }
}
