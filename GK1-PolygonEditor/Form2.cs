using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GK1_PolygonEditor
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public void SetLabel(string s)
        {
            label1.Text = s;
        }

        public void SetNumericValue(float v)
        {
            numericUpDown1.Value = (Decimal)v;
        }

        public decimal GetNumericValue()
        {
            return numericUpDown1.Value;
        }
    }
}
