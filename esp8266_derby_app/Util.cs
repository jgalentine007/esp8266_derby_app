using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace esp8266_derby_app
{
    public class Util
    {
        public static Derby LoadDerby(string fileName)
        {
            StreamReader oSR = new StreamReader(fileName);            
            XmlSerializer x = new XmlSerializer(typeof(Derby));
            Derby derby = (Derby)x.Deserialize(oSR);            
            oSR.Close();
            return derby;
        }

        public static void SaveDerby(string fileName, Derby derby )
        {
            StreamWriter oSW = new StreamWriter(fileName);
            XmlSerializer x = new XmlSerializer(typeof(Derby));            
            x.Serialize(oSW, derby);
            oSW.Close();
        }

        public static void ClearControl(Control control)
        {
            if (control is TextBox)
            {
                TextBox txtbox = (TextBox)control;
                txtbox.Text = string.Empty;
            }
            else if (control is CheckBox)
            {
                CheckBox chkbox = (CheckBox)control;                
                chkbox.Checked = false;
            }
            else if (control is RadioButton)
            {
                RadioButton rdbtn = (RadioButton)control;
                rdbtn.Checked = false;                
            }
            else if (control is DateTimePicker)
            {
                DateTimePicker dtp = (DateTimePicker)control;
                dtp.Value = DateTime.Now;
            }
            else if (control is ComboBox)
            {
                ComboBox cmb = (ComboBox)control;
                if (cmb.DataSource != null)
                {
                    cmb.SelectedItem = string.Empty;
                    cmb.SelectedValue = 0;
                }
            }
            
            // repeat for combobox, listbox, checkbox and any other controls you want to clear
            if (control.HasChildren)
            {
                foreach (Control child in control.Controls)
                {
                    ClearControl(child);
                }
            }
        }
    }
}
