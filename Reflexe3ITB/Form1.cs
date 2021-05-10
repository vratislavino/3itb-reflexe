using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reflexe3ITB
{

    // TODO: změna vlastností se neprojevuje v textboxech (event)

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
            var propertyInfos = t.GetProperties();
            var methodInfos = t.GetMethods();
            Console.WriteLine("TYP: " + t.FullName);
            flowLayoutPanel1.Controls.Clear();

            /*
            foreach(var field in fieldInfos) {
                Vlastnost v = new Vlastnost();
                v.SetName(field.Name);
                v.SetValue(field.GetValue(aktualni));
                flowLayoutPanel1.Controls.Add(v);
            }
            */

            foreach (var property in propertyInfos) {
                Vlastnost v = new Vlastnost(property);
                v.ValueChanged += OnValueChanged;
                v.SetName(property.Name);
                v.SetValue(property.GetValue(aktualni));
                flowLayoutPanel1.Controls.Add(v);
            }

            foreach (var method in methodInfos) {
                if (method.DeclaringType != t)
                    continue;
                if (method.Attributes.HasFlag(MethodAttributes.SpecialName)) {
                    continue;
                }

                Button button = new Button();
                button.Text = method.Name;
                //Console.WriteLine(method.Name + " : " + method.DeclaringType);
                button.Click += (send, evt) => { method.Invoke(aktualni, new object[] { }); };
                flowLayoutPanel1.Controls.Add(button);
            }
        }

        private void OnValueChanged(PropertyInfo info, object val) {
            Console.WriteLine(info.Name + " : " + val);
            info.SetValue(aktualni, val);
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
        
        private Point a;
        public Point A {
            get { return a; }
            set { a = value; }
        }

        private Point b;
        public Point B {
            get { return b; }
            set { b = value; }
        }

        private int sul; // 0-10
        public int Sul {
            get { return sul; }
            set {
                sul = value;
                if (sul < 0)
                    sul = 0;
                if (sul > 10)
                    sul = 10;
            }
        }

        public Tycinka(Point a, Point b, int sul) {
            this.a = a;
            this.b = b;
            this.sul = sul;
        }

        public void PridejSul() {
            sul += 1;
            if (sul > 10)
                sul = 10;
        }

        public void OdeberSul() {
            sul -= 1;
            if (sul < 0)
                sul = 0;
        }
    }
}
