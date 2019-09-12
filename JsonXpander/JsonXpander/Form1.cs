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
using System.Web.Script.Serialization;
using System.Threading;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;

namespace JsonXpander
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        void UpdateItems()
        {
        }
        private void Form1_Activated(object sender, EventArgs e)
        {
            UpdateItems();
        }
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void jsonBtn_Click(object sender, EventArgs e)
        {
            string blob_path = "D:\\temp\\20190903-2353_20C\\process\\dpp\\CalibrationBlob.json";
            using (StreamReader r = new StreamReader(blob_path))
            {
                string json = r.ReadToEnd();
                dynamic json_data = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonFile.RootObject>(json);
                Graphs graphs = new Graphs(json_data);
                graphs.Show();
            }
        }
    }

    public class AppSettings
    {
    }
}
