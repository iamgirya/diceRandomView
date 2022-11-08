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

namespace DiceRandomView
{
    public partial class Form1 : Form
    {
        bool isBuilding = false;
        long iterationCount;
        DiceHandBuilder dicesBuilder;

        private long getIterationCount()
        {
            try
            {
                string tmpText = textBox2.Text.Replace('.', ',');
                double tmp = Convert.ToDouble(tmpText);
                if (tmp < 0 || tmp >3)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (tmp >= 1)
                {
                    return Convert.ToInt64(10000 * Math.Pow(10, tmp) + 20);
                } else
                {
                    return Convert.ToInt64(100000 * tmp + 20);
                } 
            } catch
            {
                return 0;
            }
        }
        private void buildRandom()
        {
            iterationCount = getIterationCount();
            if (iterationCount != 0)
            {
                isBuilding = true;
                build(zedGraphControl1);
            }
        }

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
            PointPairList points = dicesBuilder.getRandomScheme();
            PointPairList pointsMoreThen = dicesBuilder.getMoreThenRandomScheme();

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

            iterationCount = getIterationCount();
            if (iterationCount != 0)
            {
                build(zedGraphControl1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buildRandom();
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

        private void Form1_Load_1(object sender, EventArgs e)
        {
            dicesBuilder = new DiceHandBuilder(getIterationCount(), new DiceHand());
            zedGraphControl1.GraphPane.Title.Text = "Вероятности";
            zedGraphControl1.GraphPane.XAxis.Title.Text = "Значение суммы";
            zedGraphControl1.GraphPane.YAxis.Title.Text = "Шанс";
            buildRandom();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 embeddedForm = new Form2(dicesBuilder.diceHand);
            
            embeddedForm.ShowDialog();
            
        }
    }
}
