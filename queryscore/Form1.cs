using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace queryscore
{
    public partial class Form1 : Form
    {
        String lastgrade = null;

        public Form1()
        {
            InitializeComponent();
        }

       

        private void Form1_Load(object sender, EventArgs e)
        {
            // WebReference.ahxkp_cjcx_webserver a = new WebReference.ahxkp_cjcx_webserver();
            
            comboBox2.Text = "1";
            label3.Text = "";
            this.Height = 130;
            
            int thisyear= int.Parse(DateTime.Now.Year.ToString());
            int thismonth=int.Parse(DateTime.Now.Month.ToString());
            if (thismonth > 9)
            {
                thisyear++;
            }
                
            for (int i = 0; i < 4; i++)
            {
                int start = thisyear - i - 1;
                int end = thisyear - i;
                String nowyear = Convert.ToString(start)+"-"+Convert.ToString(end);
                comboBox1.Items.Add(nowyear);
            }
            comboBox1.SelectedIndex = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || comboBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("输入不正确！");
            }
            else
            {
                label3.Text = "";
                button1.Text = "查询中...";
                this.Enabled = false;


                Random r = new Random();
                int index = r.Next(3);
                String tips = null;
                switch (index)
                {
                    case 1: tips = "双击程序空白处有惊喜"; break;
                    case 2: tips = "单击成绩可以复制到剪贴板"; break;
                    default: tips = "你知道吗?不去考试会得0分"; break;
                }
                this.Text = tips;

                
                
                String sid = textBox1.Text;
                String yid = comboBox1.Text.Substring(2, 2) + (int.Parse(comboBox2.Text) - 1).ToString();
                
                chafen.ahxkp_cjcx_webserver cf = new chafen.ahxkp_cjcx_webserver();
                try
                {
                    String s = cf.xscjcx(yid, sid);
                    lastgrade = s;
                    int a = s.IndexOf(" ");
                    String title = s.Substring(0, a + 1);
                    s = s.Substring(a + 1);
                    
                    String[] grade = s.Split(',');                   
                    groupBox1.Text = title;

                    int l = grade.Length;
                    groupBox1.Height =25*(l+1);
                    this.Height = 130 + groupBox1.Height;
                    
                    for(int i=0;i< l; i++)
                    {
                        label3.Text = label3.Text+ grade[i] + "\n" + "\n";
                    }
                    

                }
                catch(Exception ioe)
                {
                    label3.Text = ioe.ToString();
                }
                finally
                {
                    this.Enabled = true;
                    button1.Text = "查询";
                }
                
            }

        }

        private void label3_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(lastgrade);
            MessageBox.Show("成绩已复制！");

        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Process.Start("http://www.wangxinlei.cn");
        }
    }
}
