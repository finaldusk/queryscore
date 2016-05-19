using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
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
             /*
             设定combobox1可选项为最近四学年
             若当前月为9月之后则判断当前学期为上学期            
            */
            int thisyear= int.Parse(DateTime.Now.Year.ToString());
            int thismonth=int.Parse(DateTime.Now.Month.ToString());            
            comboBox2.Text = "2";

            if (thismonth > 9)
            {
                thisyear++;
                comboBox2.Text = "1";
            }
                
            for (int i = 0; i < 4; i++)
            {
                int start = thisyear - i - 1;
                int end = thisyear - i;
                String nowyear = Convert.ToString(start)+"-"+Convert.ToString(end);
                comboBox1.Items.Add(nowyear);
            }
            
            comboBox1.SelectedIndex = 0;
            label3.Text = "";
            this.Height = 130;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            String sid = textBox1.Text;

            if (!(IsIDTrue(sid)))
            {
                MessageBox.Show("输入不正确！");
            }
            else
            {
                //冻结窗体防止程序死机
                label3.Text = "";
                button1.Text = "查询中...";
                this.Enabled = false;

                //在标题栏显示帮助提示信息               
                String tips=GetHelp();
                this.Text = tips;
                              
                //将学期信息处理为WebService需要的格式
                String yid = comboBox1.Text.Substring(2, 2) + (int.Parse(comboBox2.Text) - 1).ToString();
                

                chafen.ahxkp_cjcx_webserver cf = new chafen.ahxkp_cjcx_webserver();
                try
                {
                    //请求成绩信息
                    String s = cf.xscjcx(yid, sid);

                    //处理并输出
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
                    label3.Text = ("出错了请重试\n错误信息："+ioe.ToString());
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
            CopyToClip(lastgrade);
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Process.Start("http://www.wangxinlei.cn");
        }

        //将对象O放入剪贴板
        public static void CopyToClip(Object o)
        {
            Clipboard.SetDataObject(o);
            MessageBox.Show("成绩已复制！");
        }

        //判断学号是否为纯数字
        public static bool IsIDTrue(String id)
        {
            Regex reg = new Regex("\\d+");
            return reg.IsMatch(id);            
        }

        //随机产生帮助信息
        public static String GetHelp()
        {
            Random r = new Random();
            int index = r.Next(3);
            String tips = null;
            switch (index)
            {
                case 1: tips = "双击程序空白处有惊喜"; break;
                case 2: tips = "单击成绩可以复制到剪贴板"; break;
                default: tips = "你知道吗?不去考试会得0分"; break;
            }
            return tips;
        }
    }
}



