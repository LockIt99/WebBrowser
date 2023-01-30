using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WebBrowser
{
    public partial class WinForm : Form
    {
        public WinForm()
        {
            InitializeComponent();
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            browser.GoBack();
        }

        private void cbURL_KeyDown(object sender, KeyEventArgs e)
        {
            if(cbURL.Text.Length > 0 && e.KeyCode == Keys.Enter) 
            {
                browser.Navigate(cbURL.Text);
            }
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            browser.GoForward();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            browser.Refresh();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            browser.Navigate(cbURL.Text);
        }

        private void WinForm_Load(object sender, EventArgs e)
        {
            browser.ScriptErrorsSuppressed = true;
            string appName =Process.GetCurrentProcess().ProcessName + ".exe";
            //MessageBox.Show("Current process name is " + appName);
           SetIEkeyBrowserContrl(appName);
        }

      static void SetIEkeyBrowserContrl(string appName)
        {
            RegistryKey regKey = null;
            try
            {
                if (Environment.Is64BitOperatingSystem)
                    regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Wow6432Node\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
                else
                    regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
                if(regKey == null)
                {
                    MessageBox.Show("Failed to save settings. Address not found");
                    return;
                }
                string key = regKey.GetValue(appName).ToString();
               // MessageBox.Show($"Available Key for {appName}.{key}");
                if (key == "8000")
                {
                    MessageBox.Show("It's already there.");
                    regKey.Close();
                    return;
                }
                else if (string.IsNullOrEmpty(key))
                {
                    regKey.SetValue(appName, unchecked((int)0x1F40),RegistryValueKind.DWord);
                }
                //key = regKey.GetValue(appName).ToString();
                //if (key == "8000")
                //    MessageBox.Show("Application Settings Applied Successfully");
                //else
                //    MessageBox.Show("Application Settings Failed, Ref: " + key);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured while doing reg setup -  "+ ex.Message);
                
            }
            finally
            {
                if(regKey != null) regKey.Close();
            }
        }

        private void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Text = "Brave - " + browser.Document.Title;
        }
    }
}