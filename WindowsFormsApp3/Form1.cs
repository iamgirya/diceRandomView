using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        bool isBuilding = false;
        long iterationCount;
        int randomSeed;

        private void Clear(ZedGraphControl Zed_GraphControl)
        {
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();

            zedGraphControl1.GraphPane.XAxis.Type = AxisType.Linear;
            zedGraphControl1.GraphPane.XAxis.Scale.TextLabels = null;
            zedGraphControl1.GraphPane.XAxis.MajorGrid.IsVisible = false;
            zedGraphControl1.GraphPane.YAxis.MajorGrid.IsVisible = false;
            zedGraphControl1.GraphPane.YAxis.MinorGrid.IsVisible = false;
            zedGraphControl1.GraphPane.XAxis.MinorGrid.IsVisible = false;
            zedGraphControl1.RestoreScale(zedGraphControl1.GraphPane);

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        public void build(ZedGraphControl Zed_GraphControl)
        {
            DiceRandom dices = new DiceRandom(randomSeed, iterationCount);
            PointPairList points = dices.getRandomScheme();
            PointPairList pointsMoreThen = dices.getMoreThenRandomScheme();


            Clear(zedGraphControl1);
            GraphPane my_Pane = Zed_GraphControl.GraphPane;
            LineItem myCircle1 = my_Pane.AddCurve("Значение", points, Color.Blue, SymbolType.Circle);
            LineItem myCircle2 = my_Pane.AddCurve("Больше чем", pointsMoreThen, Color.Red, SymbolType.Circle);
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!isBuilding) return;
            try { iterationCount = Convert.ToInt64(textBox2.Text);
                build(zedGraphControl1); return;
            }
            catch { return; }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (!isBuilding) return;
            try
            {
                randomSeed = Convert.ToInt32(textBox3.Text);
                build(zedGraphControl1); return;
            }
            catch { return; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                iterationCount = Convert.ToInt64(textBox2.Text);
                randomSeed = Convert.ToInt32(textBox3.Text);
                isBuilding = true;
                build(zedGraphControl1);
                return;
            }
            catch { return; }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void zedGraphControl1_MouseClick(object sender, MouseEventArgs e)
        {
            // Сюда будет сохранена кривая, рядом с которой был произведен клик
            CurveItem curve;

            // Сюда будет сохранен номер точки кривой, ближайшей к точке клика
            int index;

            GraphPane pane = zedGraphControl1.GraphPane;

            // Максимальное расстояние от точки клика до кривой в пикселях,
            // при котором еще считается, что клик попал в окрестность кривой.
            GraphPane.Default.NearestTol = 100;

            bool result = pane.FindNearestPoint(e.Location, out curve, out index);

            if (result)
            {
                textBox5.Text = "x: "+curve[index].X.ToString();
                textBox6.Text = "y: " + curve[index].Y.ToString();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            iterationCount = Convert.ToInt64(textBox2.Text);
            randomSeed = Convert.ToInt32(textBox3.Text);
            isBuilding = true;
            build(zedGraphControl1);
            return;
        }
    }
}
