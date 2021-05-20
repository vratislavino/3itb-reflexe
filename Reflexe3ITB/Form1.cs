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
        List<Tvar> tvary = new List<Tvar>();

        public Form1() {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e) {
            Ctverec c = new Ctverec(10,10,50);
            AddTvar(c);
        }

        private void Button2_Click(object sender, EventArgs e) {
            Tycinka t = new Tycinka(new Point(10,10), new Point(180,30), 7);
            AddTvar(t);
        }

        private void AddTvar(Tvar tvar) {
            if(aktualni != null) {
                tvary.Add(aktualni);
                UpdateHistory();
            }

            aktualni = tvar;
            aktualni.NecoSeZmenilo += () => {
                UpdateProperties();
            };
            label1.Text = tvar.GetType().Name;
            UpdateProperties();
        }

        private void UpdateHistory() {

            flowLayoutPanel2.Controls.Clear();
            foreach(var tvar in tvary) {
                Button btn = new Button();
                btn.Text = tvar.GetType().Name;
                btn.Tag = tvar;
                btn.Click += (sender, e) => {

                    var t = (Tvar)(((Button) sender).Tag);
                    aktualni = t;

                    label1.Text = t.GetType().Name;
                    UpdateProperties();
                };
                flowLayoutPanel2.Controls.Add(btn);
            }

        }

        private void UpdateProperties() {
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
                button.Click += (send, evt) => {
                    method.Invoke(aktualni, new object[] { });
                };
                flowLayoutPanel1.Controls.Add(button);
            }
        }

        private void Button3_Click(object sender, EventArgs e) {
            UpdateProperties();
        }

        private void OnValueChanged(PropertyInfo info, object val) {
            Console.WriteLine(info.Name + " : " + val);
            info.SetValue(aktualni, val);
        }
    }

    public abstract class Tvar
    {
        public abstract event Action NecoSeZmenilo;
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
        public override event Action NecoSeZmenilo;

        private int x, y, sirka;

        public int X { get => x; set {
                x = value;
                NecoSeZmenilo?.Invoke();
            }
        }
        public int Y { get => y; set {
                y = value;
                NecoSeZmenilo?.Invoke();
            }
        }
        public int Sirka { get => sirka; set {
                sirka = value;
                NecoSeZmenilo?.Invoke();
            }
        }

        public Ctverec(int x, int y, int sirka) {
            this.x = x;
            this.y = y;
            this.sirka = sirka;
        }

        public void PosunDoprava() {
            Sirka += 10;
        }
    }

    public class Tycinka : Tvar
    {
        public override event Action NecoSeZmenilo;

        private Point a;
        public Point A {
            get { return a; }
            set {
                a = value;
                NecoSeZmenilo?.Invoke();
            }
        }

        private Point b;
        public Point B {
            get { return b; }
            set {
                b = value;
                NecoSeZmenilo?.Invoke();
            }
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
                NecoSeZmenilo?.Invoke();
            }
        }

        public Tycinka(Point a, Point b, int sul) {
            this.a = a;
            this.b = b;
            this.sul = sul;
        }

        public void PridejSul() {
            Sul += 1;
            if (Sul > 10)
                Sul = 10;
        }

        public void OdeberSul() {
            Sul -= 1;
            if (Sul < 0)
                Sul = 0;
        }
    }
}
