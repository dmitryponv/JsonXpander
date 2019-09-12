using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Reflection;
using System.Collections;

namespace JsonXpander
{
    public partial class Graphs : Form
    {
        dynamic jsonData;
        public Graphs(dynamic json_data)
        {
            InitializeComponent();

            jsonData = json_data;

            CheckBox cbox;
            PropertyInfo[] properties = typeof(JsonFile.RootObject).GetProperties();
            int j = 0;
            foreach (PropertyInfo property in properties)
            {
                cbox = new CheckBox();
                cbox.Text = property.Name;
                cbox.Name = "RootObject";
                cbox.AutoSize = true;
                cbox.Location = new Point(0, j++ * 20); //vertical
                cbox.CheckedChanged += new EventHandler(ck_CheckedChanged);
                this.Controls.Add(cbox);
            }
        }

        List<int> vertical_position = new List<int>();
        void ck_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            string curr_property = chk.Text;
            string prev_properties = chk.Name;
            List<string> prop_list = prev_properties.Split('!').ToList();
            prop_list.Add(curr_property);
            int horizontal_position = prop_list.Count() - 1;
            while (vertical_position.Count <= horizontal_position)
                vertical_position.Add(0);

            bool is_checked = chk.Checked;

            //First sort through the properties
            PropertyInfo[] properties = new PropertyInfo[0];
            Type currentType = typeof(JsonFile.RootObject);
            object currentObject = jsonData;
            for (int i = 1; i < prop_list.Count(); i++)
            {
                if (prop_list[i].Contains('[') && prop_list[i].Contains(']'))
                {
                    int index = 0;
                    Int32.TryParse(prop_list[i].Substring(1, prop_list[i].Length - 2), out index);
                    PropertyInfo indexer = currentObject.GetType().GetProperty("Item");
                    currentObject = indexer.GetValue(currentObject, new object[] { index });
                    currentType = currentObject.GetType();
                    properties = currentType.GetProperties();
                }
                else
                {
                    currentObject = currentType.GetProperty(prop_list[i]).GetValue(currentObject);
                    currentType = currentObject.GetType();
                    properties = currentType.GetProperties();
                }
            }

            CheckBox cbox;
            if (currentType.Name == "List`1")
            {
                string list_type = currentType.AssemblyQualifiedName.Split('[')[2].Split(',')[0];
                Type type = Type.GetType(list_type);
                var countProperty = currentObject.GetType().GetProperty("Count");
                var count = (int)countProperty.GetValue(currentObject);
                for (int i = 0; i < count; i++)
                {
                    if (is_checked)
                    {
                        cbox = new CheckBox();
                        cbox.Text = "[" + i.ToString() + "]";
                        cbox.Name = prev_properties + "!" + curr_property;
                        cbox.AutoSize = true;
                        cbox.Location = new Point(horizontal_position * 150, vertical_position[horizontal_position]++ * 20); //vertical
                        cbox.CheckedChanged += new EventHandler(ck_CheckedChanged);
                        this.Controls.Add(cbox);
                    }
                    else
                    {
                        string cbox_name = prev_properties + "!" + curr_property;
                        this.Controls.Remove(this.Controls[cbox_name]);
                        vertical_position[horizontal_position]--;
                    }
                }
            }
            else if (currentType.Name == "String")
                //Add some string processing
                return;
            else if (currentType.Name == "Double")
                //Add Some number processing 
                return;
            else
            {
                int i = 0;
                foreach (PropertyInfo property in properties)
                {
                    if (is_checked)
                    {
                        cbox = new CheckBox();
                        cbox.Text = property.Name;
                        cbox.Name = prev_properties + "!" + curr_property;
                        cbox.AutoSize = true;
                        cbox.Location = new Point(horizontal_position * 150, vertical_position[horizontal_position]++ * 20); //vertical
                        cbox.CheckedChanged += new EventHandler(ck_CheckedChanged);
                        this.Controls.Add(cbox);
                    }
                    else
                    {
                        string cbox_name = prev_properties + "!" + curr_property;
                        this.Controls.Remove(this.Controls[cbox_name]);
                        vertical_position[horizontal_position]--;
                    }
                }
            }

        }
    }
}
