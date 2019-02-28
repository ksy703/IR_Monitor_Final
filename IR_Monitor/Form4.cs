using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

//'Monitoring' tab의 'Graph' button 클릭 시 popup 창
namespace IR_Monitor
{
    public partial class Form4 : Form
    {
        DataTable dt=new DataTable();
        public Form4()
        {
            InitializeComponent();
        }

        public DataTable Passvalue
        {
            set {
                this.dt = value;
                draw();
            }
        }

        public void draw()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate ()
                {
                    draw();
                }
                    ));
            }
            else
            {
               
                    chart_H2S.ChartAreas[0].AxisY.Minimum = -5;
                    chart_H2S.ChartAreas[0].AxisY.Maximum = 50;
                    chart_H2S.Series[0].YValueMembers = "H2S_Conc";
                    chart_H2S.DataSource = dt;
                    chart_H2S.DataBind();

                    chart_CO.ChartAreas[0].AxisY.Minimum = -10;
                    chart_CO.ChartAreas[0].AxisY.Maximum = 100;
                    chart_CO.Series[0].YValueMembers = "Co_Conc";
                    chart_CO.DataSource = dt;
                    chart_CO.DataBind();

                    chart_O2.ChartAreas[0].AxisY.Maximum = 30;
                    chart_O2.Series[0].YValueMembers = "O2_Conc";
                    chart_O2.DataSource = dt;
                    chart_O2.DataBind();

                    chart_FL.ChartAreas[0].AxisY.Minimum = -10;
                    chart_FL.ChartAreas[0].AxisY.Maximum = 120;
                    chart_FL.Series[0].YValueMembers = "FL_Conc";
                    chart_FL.DataSource = dt;
                    chart_FL.DataBind();

                    chart_Temp.ChartAreas[0].AxisY.Minimum = -10;
                    chart_Temp.ChartAreas[0].AxisY.Maximum = 50;
                    chart_Temp.Series[0].YValueMembers = "Temp";
                    chart_Temp.DataSource = dt;
                    chart_Temp.DataBind();

                    chart_volt.ChartAreas[0].AxisY.Minimum = -10;
                    chart_volt.ChartAreas[0].AxisY.Maximum = 3500;
                    chart_volt.Series[0].YValueMembers = "Volt";
                    chart_volt.DataSource = dt;
                    chart_volt.DataBind();
                
            }
        }

        private void CLOSE_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}
