using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App2
{
    public partial class ChooseNetwork : Form
    {
        public ChooseNetwork()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void addItem(string item) {
            comboBox1.Items.Add(item);
        }

        public string selected() {
            return comboBox1.SelectedItem.ToString();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
