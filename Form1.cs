using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NETCONLib;
using IcsManagerLibrary;
using NATUPNPLib;


namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public static iniFile cfg = new iniFile("./config.ini");
        public Form1()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                if (checkBox1.Checked)
                {
                    var sharedConnectionItem = cbSharedConnection.SelectedItem as ConnectionItem;
                    var homeConnectionItem = cbHomeConnection.SelectedItem as ConnectionItem;
                    if ((sharedConnectionItem == null) || (homeConnectionItem == null))
                    {
                        MessageBox.Show(@"Please select both connections.");
                        return;
                    }
                    if (sharedConnectionItem.Connection == homeConnectionItem.Connection)
                    {
                        MessageBox.Show(@"Please select different connections.");
                        return;
                    }
                    IcsManager.ShareConnection(sharedConnectionItem.Connection, homeConnectionItem.Connection);
                    RefreshConnections();
                    /*
                    startInfo.FileName = "cmd.exe";
                    //startInfo.Arguments = "NETSH WLAN SHOW DRIVERS";
                    startInfo.Arguments = "NETSH WLAN SET HOSTEDNETWORK MODE=ALLOW SSID=" + textBox1.Text + " KEY=" + textBox2.Text + "";
                    //startInfo.Arguments = "NETSH WLAN START HOSTEDNETWORK";
                    process.StartInfo = startInfo;
                    process.Start();
                    //StreamWriter inputStream = shellProcess.StandardInput;
                    //send command to cmd prompt and wait for command to execute with thread sleep
                    //inputStream.WriteLine("echo CMD just received input");
                    //inputStream.Flush();
                    */
                    var yourcommand = "NETSH WLAN SET HOSTEDNETWORK MODE=ALLOW SSID=" + textBox1.Text + " KEY=" + textBox2.Text + "";
                    var procStart = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + yourcommand);
                    procStart.CreateNoWindow = true;
                    procStart.RedirectStandardOutput = true;
                    procStart.UseShellExecute = false;

                    var proc = new System.Diagnostics.Process();
                    proc.StartInfo = procStart;
                    proc.Start();
                    yourcommand = "NETSH WLAN START HOSTEDNETWORK";
                    procStart = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + yourcommand);
                    procStart.CreateNoWindow = true;
                    procStart.RedirectStandardOutput = true;
                    procStart.UseShellExecute = false;

                    proc = new System.Diagnostics.Process();
                    proc.StartInfo = procStart;
                    proc.Start();
                    var result = proc.StandardOutput.ReadToEnd();
                    MessageBox.Show(result);
                    checkBox1.Text = "Stop";
                    textBox1.ReadOnly = true;
                    textBox2.ReadOnly = true;
                    button1.BackColor = Color.Lime;
                    button1.Text = "Working";
                }
                else
                {
                    var yourcommand = "NETSH WLAN STOP HOSTEDNETWORK";
                    var procStart = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + yourcommand);
                    procStart.CreateNoWindow = true;
                    procStart.RedirectStandardOutput = true;
                    procStart.UseShellExecute = false;
                    var proc = new System.Diagnostics.Process();
                    proc.StartInfo = procStart;
                    proc.Start();
                    checkBox1.Text = "Start";
                    IcsManager.ShareConnection(null, null);
                    RefreshConnections();
                    var result = proc.StandardOutput.ReadToEnd();
                    MessageBox.Show(result);
                    textBox1.ReadOnly = false;
                    textBox2.ReadOnly = false;
                    button1.BackColor = Color.Red;
                    button1.Text = "Stopped";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = cfg.IniReadValue("SETTING", "SSID");
            textBox2.Text = cfg.IniReadValue("SETTING", "PASS");
            var yourcommand = "NETSH WLAN SET HOSTEDNETWORK MODE=ALLOW SSID=" + textBox1.Text + " KEY=" + textBox2.Text + "";
            var procStart = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + yourcommand);
            procStart.CreateNoWindow = true;
            procStart.RedirectStandardOutput = true;
            procStart.UseShellExecute = false;
            var proc = new System.Diagnostics.Process();
            proc.StartInfo = procStart;
            proc.Start();
            proc = new System.Diagnostics.Process();
            proc.StartInfo = procStart;
            proc.Start();
            yourcommand = "NETSH WLAN START HOSTEDNETWORK";
            procStart = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + yourcommand);
            procStart.CreateNoWindow = true;
            procStart.RedirectStandardOutput = true;
            procStart.UseShellExecute = false;
            proc = new System.Diagnostics.Process();
            proc.StartInfo = procStart;
            proc.Start();
            try
            {
                RefreshConnections();

                var sharedConnectionItem = cbSharedConnection.SelectedItem as ConnectionItem;
                var homeConnectionItem = cbHomeConnection.SelectedItem as ConnectionItem;
                if ((sharedConnectionItem == null) || (homeConnectionItem == null))
                {
                    MessageBox.Show(@"Please select both connections.");
                    return;
                }
                if (sharedConnectionItem.Connection == homeConnectionItem.Connection)
                {
                    MessageBox.Show(@"Please select different connections.");
                    return;
                }
                IcsManager.ShareConnection(sharedConnectionItem.Connection, homeConnectionItem.Connection);
                RefreshConnections();
            }
            catch
            { }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.UserClosing:
                    e.Cancel = true;
                    break;
            }
            base.OnFormClosing(e);
            var yourcommand = "NETSH WLAN STOP HOSTEDNETWORK";
            var procStart = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + yourcommand);
            procStart.CreateNoWindow = true;
            procStart.RedirectStandardOutput = true;
            procStart.UseShellExecute = false;
            var proc = new System.Diagnostics.Process();
            proc.StartInfo = procStart;
            proc.Start();
            checkBox1.Text = "Start";
            IcsManager.ShareConnection(null, null);
            RefreshConnections();
            var result = proc.StandardOutput.ReadToEnd();
            MessageBox.Show(result);
            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
            button1.BackColor = Color.Red;
            button1.Text = "Stopped";
            Environment.Exit(0);
        }
        private void AddNic(NetworkInterface nic)
        {
            var connItem = new ConnectionItem(nic);
            cbSharedConnection.Items.Add(connItem);
            cbHomeConnection.Items.Add(connItem);
            var netShareConnection = connItem.Connection;
            if (netShareConnection != null)
            {
                var sc = IcsManager.GetConfiguration(netShareConnection);
                if (sc.SharingEnabled)
                {
                    switch (sc.SharingConnectionType)
                    {
                        case tagSHARINGCONNECTIONTYPE.ICSSHARINGTYPE_PUBLIC:
                            cbSharedConnection.SelectedIndex = cbSharedConnection.Items.Count - 1;
                            break;
                        case tagSHARINGCONNECTIONTYPE.ICSSHARINGTYPE_PRIVATE:
                            cbHomeConnection.SelectedIndex = cbSharedConnection.Items.Count - 1;
                            break;
                    }
                }
            }
        }

        private void RefreshConnections()
        {
            cbSharedConnection.Items.Clear();
            cbHomeConnection.Items.Clear();
            foreach (var nic in IcsManager.GetAllIPv4Interfaces())
            {
                AddNic(nic);
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            cfg.IniWriteValue("SETTING", "SSID",textBox1.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            cfg.IniWriteValue("SETTING", "PASS", textBox2.Text);
        }

        private void cbSharedConnection_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;
            Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon1.Visible = false;
            Show();
        }
    }
}
