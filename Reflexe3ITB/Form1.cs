using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reflexe3ITB
{
    public partial class Form1 : Form
    {
        Tvar aktualni;

        public Form1() {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e) {
            Ctverec c = new Ctverec(10,10,50);
            aktualni = c;

            label1.Text = "Ctverec";
        }

        private void Button2_Click(object sender, EventArgs e) {
            Tycinka t = new Tycinka(new Point(10,10), new Point(180,30), 7);

            aktualni = t;
            label1.Text = "Tyčinka";
        }

        private void Button3_Click(object sender, EventArgs e) {
            Type t = aktualni.GetType();
            var fieldInfos = t.GetFields();
            var methodInfos = t.GetMethods();

            flowLayoutPanel1.Controls.Clear();

            foreach(var field in fieldInfos) {
                Vlastnost v = new Vlastnost();
                v.SetName(field.Name);
                v.SetValue(field.GetValue(aktualni));
                flowLayoutPanel1.Controls.Add(v);
            }

            foreach(var method in methodInfos) {
                Button button = new Button();
                button.Text = method.Name;
                button.Click += (send, evt) => { method.Invoke(aktualni, new object[] { }); };
                flowLayoutPanel1.Controls.Add(button);
            }

        }
    }

    public class Tvar
    {

    }

    public class Clovek
    {
        Label label;

        public Clovek(Label label) {
            this.label = label;
        }

        public void NastavJmeno(string jm) {
            label.Text = jm;
        }
    }

    public class Ctverec : Tvar
    {
        public int x, y, sirka;
        public Ctverec(int x, int y, int sirka) {
            this.x = x;
            this.y = y;
            this.sirka = sirka;
        }

        public void PosunDoprava() {
            x += 10;
        }
    }

    public class Tycinka : Tvar
    {
        public Point a;
        public Point b;
        public int sul; // 0-10

        public Tycinka(Point a, Point b, int sul) {
            this.a = a;
            this.b = b;
            this.sul = sul;
        }

        public void PridejSul() {
            sul += 1;
        }

        public void OdeberSul() {
            sul -= 1;
        }
    }
}
