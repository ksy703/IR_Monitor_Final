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



//'Monitoring' tab의 'View log' button 클릭 시 popup 창
namespace IR_Monitor
{
    public partial class Form3 : Form
    {
        string log;
        public Form3()
        {
            InitializeComponent();
            
        }
        
        public string Passvalue
        {
            set {  this.log = value; tB_Mtr_Log.Text = log; tB_Mtr_Log.Select(log.Length, 0); tB_Mtr_Log.ScrollToCaret();  }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "모든파일|*.*";
            saveFileDialog1.Title = "Save Log";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fs= (FileStream)saveFileDialog1.OpenFile();
                StreamWriter s = new StreamWriter(fs);
                s.WriteLine(tB_Mtr_Log.Text);
                s.Close();
                fs.Close();

            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Visible=false;
        }
        
    }
}
