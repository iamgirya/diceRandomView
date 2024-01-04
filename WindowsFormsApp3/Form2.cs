using System;
using System.Collections.Generic;
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
            }
            catch { }
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
            chooseHandMode(1, checkBox3.Checked);
            if (checkBox3.Checked)
            {
                try
                {
                    diceHand.needCount = Convert.ToInt32(textBox3.Text);
                }
                catch { }
            }
            else
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
            if (diceHand.needRezultForSucess != Int32.MaxValue)
            {
                textBox6.Enabled = true;
                textBox6.Text = diceHand.needRezultForSucess.ToString();
                checkBox4.Checked = true;
            }
            if (diceHand.needCount != 0)
            {
                textBox3.Enabled = true;
                textBox3.Text = diceHand.needRezultForSucess.ToString();
                checkBox3.Checked = true;
            }
            textBox4.Text = diceHand.bonus.ToString();
            textBox5.Text = diceHand.mult.ToString();

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            chooseHandMode(2, checkBox4.Checked);
            if (checkBox4.Checked)
            {
                try
                {
                    diceHand.needRezultForSucess = Convert.ToInt32(textBox6.Text);
                }
                catch { }
            }
            else
            {
                diceHand.needRezultForSucess = Int32.MaxValue;
            }
        }

        private void chooseHandMode(int mode, bool checkState)
        {
            List<CheckBox> modeChecks = new List<CheckBox>() { checkBox3, checkBox4, count_of_coincidences_box };
            List<TextBox> modeInputs = new List<TextBox>() { textBox3, textBox6, null };
            for (int i = 0; i < modeChecks.Count; i++)
            {
                if (i != mode - 1)
                {
                    modeChecks[i].Checked = false;
                    if (modeInputs[i] != null)
                    {
                        modeInputs[i].Enabled = false;
                    }
                }
                else
                {
                    modeChecks[i].Checked = checkState;
                    if (modeInputs[i] != null)
                    {
                        modeInputs[i].Enabled = checkState;
                    }

                }
            }
        }

        private void count_of_coincidences_box_CheckedChanged(object sender, EventArgs e)
        {
            chooseHandMode(3, count_of_coincidences_box.Checked);
            diceHand.calculateCoincidences = count_of_coincidences_box.Checked;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                diceHand.needRezultForSucess = Convert.ToInt32(textBox6.Text);
            }
            catch
            { }
        }


    }
}
