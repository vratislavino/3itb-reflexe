using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reflexe3ITB
{
    public partial class Vlastnost : UserControl
    {
        public Vlastnost() {
            InitializeComponent();
        }

        public void SetName(string nazevVlastnosti) {
            label1.Text = nazevVlastnosti;
        }

        public void SetValue(object hodnota) {
            textBox1.Text = hodnota.ToString();
        }
    }
}
