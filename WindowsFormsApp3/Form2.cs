using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiceRandomView
{
    public partial class Form2 : Form
    {
        DiceHand diceHand;
        public Form2(DiceHand diceHand)
        {
            this.diceHand = diceHand;
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void DiceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DiceList.SelectedIndex != -1)
            {
                diceHand.diceHand.RemoveAt(DiceList.SelectedIndex);
                DiceList.Items.RemoveAt(DiceList.SelectedIndex);
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                diceHand.bonus = Convert.ToInt64(textBox4.Text);
            } catch { }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string tmpText = textBox5.Text.Replace('.', ',');
                diceHand.mult = Convert.ToDouble(tmpText);
            }
            catch { }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.Enabled = checkBox3.Checked;
            if (textBox3.Enabled)
            {
                try
                {
                    diceHand.needCount = Convert.ToInt32(textBox3.Text);
                }
                catch { }
            } else
            {
                diceHand.needCount = 0;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                diceHand.needCount = Convert.ToInt32(textBox3.Text);
            }
            catch 
            { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(textBox1.Text) == 1 && checkBox1.Checked)
                {
                    label7.Text = "Проказник :3";
                    return;
                }

                Dice newDice = new Dice(Convert.ToInt32(textBox1.Text), checkBox1.Checked, Convert.ToInt32(textBox2.Text), checkBox2.Checked);
                
                diceHand.diceHand.Add(newDice);
                
                DiceList.Items.Add(newDice.ToString());
                label7.Text = "";
            }
            catch
            {
                label7.Text = "Ошибка параметров!";
            }

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < diceHand.diceHand.Count; i++)
            {
                DiceList.Items.Add(diceHand.diceHand[i].ToString());
            }
        }
    }
}
