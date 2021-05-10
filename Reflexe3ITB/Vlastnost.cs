using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace Reflexe3ITB
{
    public partial class Vlastnost : UserControl
    {
        public event Action<PropertyInfo, object> ValueChanged;
        private PropertyInfo propertyInfo;

        public Vlastnost(PropertyInfo prop) {
            InitializeComponent();
            this.propertyInfo = prop;
        }

        public void SetName(string nazevVlastnosti) {
            label1.Text = nazevVlastnosti;
        }

        public void SetValue(object hodnota) {

            if(hodnota is Point) {
                var p = hodnota as Point?;
                if(p.HasValue) {
                    textBox1.Text = p.Value.X + "," + p.Value.Y;
                }
            } else {
                textBox1.Text = hodnota.ToString();
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e) {
            object data = null;
            string error = "";
            if(propertyInfo.PropertyType == typeof(int)) {
                data = int.Parse(textBox1.Text);
            }
            if(propertyInfo.PropertyType == typeof(Point)) {
                var cisla = textBox1.Text.Split(',');
                if(cisla.Length != 2) {
                    error = "Je špatně zadaný formát bodu!";
                } else {
                    int x, y;
                    bool okx = int.TryParse(cisla[0], out x);
                    bool oky = int.TryParse(cisla[1], out y);
                    if(okx && oky) {
                        data = new Point(x,y);
                    } else {
                        error = "Je špatně zadaný formát bodu!";
                    }
                }
            }
            if(error != "") {
                MessageBox.Show(error);
            } else {
                ValueChanged?.Invoke(propertyInfo, data);
            }
        }
    }
}
