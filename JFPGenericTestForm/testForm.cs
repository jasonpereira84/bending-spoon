﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JFPGeneric;

namespace JFPGenericTestForm
{
    public partial class testForm : Form
    {
        public testForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < numericUpDown1.Value; i++)
                listBox1.Items.Add(Functions.GetUniqueCode());
        }
    }
}
