using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace E_liquid_recipe_calculator
{
    public partial class Form1 : Form
    {
        public void reset()
        {
            Form1 NewForm = new Form1();
            NewForm.Show();
            this.Dispose(false);
        }
        public void refresh()
        {
            // Diluent refresh
            diluent = (numericUpDown2.Value / 100) * amountToMake;
            dataGridView1.Rows[3].Cells[0].Value = diluent;

            diluentDiv = diluent / 2;

            // Nico refresh
            if (nicoBase == 0)
            {
                totNic = 0;
                dataGridView1.Rows[2].Cells[0].Value = totNic;
            }
            else
            {
                totNic = ((nicoAmount / nicoBase) * amountToMake);
                totNic = Math.Round(totNic, 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[2].Cells[0].Value = totNic;
            }

            // Refresh PG/VG
            pgPercent = (pg / 100) * amountToMake;
            vgPercent = (vg / 100) * amountToMake;

            if (checkBox9.Checked == true) pgPercent = pgPercent - flavTot;
            else vgPercent = vgPercent - flavTot;

            if (pgNic == true)
            {
                pgPercent = (pgPercent - totNic) - diluentDiv;
                vgPercent = vgPercent - diluentDiv;

                vgPercent = Math.Round(vgPercent, 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[1].Cells[0].Value = vgPercent;
                pgPercent = Math.Round(pgPercent, 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[0].Value = pgPercent;
            }
            if (vgNic == true)
            {
                vgPercent = (vgPercent - totNic) - (diluentDiv);
                pgPercent = pgPercent - diluentDiv;

                vgPercent = Math.Round(vgPercent, 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[1].Cells[0].Value = vgPercent;
                pgPercent = Math.Round(pgPercent, 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[0].Cells[0].Value = pgPercent;
            }
            if(mix == true)
            {
                decimal pgMix = totNic * 0.30M;
                decimal vgMix = totNic * 0.70M;
                pgPercent = pgPercent - pgMix; pgPercent = Math.Round(pgPercent, 2, MidpointRounding.AwayFromZero);
                vgPercent = vgPercent - vgMix; vgPercent = Math.Round(vgPercent, 2, MidpointRounding.AwayFromZero);

                dataGridView1.Rows[1].Cells[0].Value = vgPercent;
                dataGridView1.Rows[0].Cells[0].Value = pgPercent;
            }

            if (vgPercent < 1)
            {
                vgPercent = 0;
                dataGridView1.Rows[1].Cells[0].Value = vgPercent;
                label14.Text = "Incorrect mix please adjust PG/VG ratio";
            }

            if (pgPercent < 1)
            {
                pgPercent = 0;
                dataGridView1.Rows[0].Cells[0].Value = pgPercent;
                label14.Text = "Incorrect mix please adjust PG/VG ratio";
            }

            if (pgPercent > 0 && vgPercent > 0) label14.Text = "";

            // Convert output
            dataGridView1.Rows[0].Cells[1].Value = pgPercent;
            dataGridView1.Rows[0].Cells[2].Value = (pgPercent / amountToMake) * 100 + "%";

            decimal vgGram = vgPercent * 1.26M;
            vgGram = Math.Round(vgGram, 2, MidpointRounding.AwayFromZero);
            dataGridView1.Rows[1].Cells[1].Value = vgGram;
            dataGridView1.Rows[1].Cells[2].Value = (vgPercent / amountToMake) * 100 + "%";

            if (pgNic == true)
            {
                dataGridView1.Rows[2].Cells[1].Value = totNic;
                nicGram = totNic;
            }
            else
            {
                nicGram = totNic * 1.26M;
                nicGram = Math.Round(nicGram, 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[2].Cells[1].Value = nicGram;
            }
            if(mix == true)
            {
                nicGram = totNic * 1.182M;
                nicGram = Math.Round(nicGram, 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[2].Cells[1].Value = nicGram;
            }

            dataGridView1.Rows[2].Cells[2].Value = (totNic / amountToMake) * 100 + "%";

            dataGridView1.Rows[3].Cells[1].Value = diluent;
            dataGridView1.Rows[3].Cells[2].Value = numericUpDown2.Value + "%";

            dataGridView1.Rows[totalPosition].Cells[2].Value = "100%";
            dataGridView1.Rows[totalPosition].Cells[1].Value =  vgGram + nicGram + pgPercent + flavGramTot + diluent;
        }

        // Initialize variables
        decimal amountToMake, diluent, diluentDiv, flavPerc, nicGram;
        decimal nicoAmount = 6, nicoBase, totNic, flav, flavTot, flavGramTot;
        decimal vgPercent = 10;
        decimal pgPercent = 10;
        decimal pg = 50;
        decimal vg = 50;
        int flavNameNum = 4;
        int totalPosition = 4;
        string flavName, flavBase, flavGram, name;
        bool pgNic, vgNic, mix;

        public Form1()
        {
            InitializeComponent();

            // Set  columns
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].HeaderText= "ml";
            Column1.Width = 60;
            dataGridView1.Columns[1].HeaderText= "Grams";
            dataGridView1.Columns[2].HeaderText = "Percent";

            // Set rows
            dataGridView1.RowCount = 5;
            dataGridView1.Rows[0].HeaderCell.Value = "PG";
            dataGridView1.Rows[1].HeaderCell.Value = "VG";
            dataGridView1.Rows[2].HeaderCell.Value = "Nicotine";
            dataGridView1.Rows[3].HeaderCell.Value = "Diluent";
            dataGridView1.Rows[4].HeaderCell.Value = "Total";

            // Start up defaults
            amountToMake = numericUpDown1.Value;
            dataGridView1.Rows[4].Cells[0].Value = amountToMake;
            dataGridView1.Rows[0].Cells[0].Value = pgPercent;
            dataGridView1.Rows[1].Cells[0].Value = vgPercent;
            label5.Text = pg.ToString() + " PG%";
            label6.Text = vg.ToString() + " VG%";
            flavBase = " (PG)";
            checkBox1.Checked = true;

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        
        // Recipe name
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           name = textBox1.Text;
        }

        // PG/VG slider
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            pg = trackBar1.Value * 5;
            vg = 100 - pg;
            label5.Text = pg.ToString() + " PG%";
            label6.Text = vg.ToString() + " VG%";

            refresh();            
        }

        // Amount to make switch
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            amountToMake = numericUpDown1.Value;
            dataGridView1.Rows[totalPosition].Cells[0].Value = amountToMake;
            
            if (numericUpDown1.Value > 1000)
            {
                numericUpDown1.Value = 1000;
            }

            refresh();
        }

        // Diluent switch
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            diluent = (numericUpDown2.Value / 100) * amountToMake;
            dataGridView1.Rows[3].Cells[0].Value = diluent;
            refresh();
        }

        // Reset
        private void button3_Click(object sender, EventArgs e)
        {
            reset();
        }

        // Save button
        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            name = textBox1.Text;
            dialog.Filter = "Text File|*.txt";
            dialog.FileName = name;
            var result = dialog.ShowDialog();
            if (result != DialogResult.OK)
                return;

            // setup for export
            dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridView1.SelectAll();
            // hiding row headers to avoid extra \t in exported text
            var rowHeaders = dataGridView1.RowHeadersVisible;
            dataGridView1.RowHeadersVisible = true;

            // ! creating text from grid values
            string content = dataGridView1.GetClipboardContent().GetText();

            // restoring grid state
            dataGridView1.ClearSelection();
            dataGridView1.RowHeadersVisible = rowHeaders;

            System.IO.File.WriteAllText(dialog.FileName, content);
            MessageBox.Show(@"Recipe was created.");
        }

        // PG nico switch
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                checkBox2.Checked = false; checkBox3.Checked = false;
                pgNic = true; mix = false; vgNic = false;
                refresh();
            }
        }

        // 30/70 nico switch
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked == true)
            {
                checkBox1.Checked = false; checkBox3.Checked = false;
                mix = true; pgNic = false; vgNic = false;
                refresh();
            }
        }

        // VG nico switch
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox3.Checked == true)
            {
                checkBox1.Checked = false; checkBox2.Checked = false;
                vgNic = true; pgNic = false; mix = false;
                refresh();
            }
        }

        // Flavour percentage
        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            flavPerc = (numericUpDown4.Value / 100);
            flav = flavPerc * amountToMake;
        }

        // Flavour vg base
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox8.Checked == true)
            {
                flavBase = "(VG)";
                checkBox9.Checked = false;
            }
        }

        //Flavour name
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            flavName = textBox2.Text;
        }

        // Flavour pg base
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox9.Checked == true)
            {
                flavBase = "(PG)";
                checkBox8.Checked = false;
            }
        }

        // Flavour add button
        private void button1_Click(object sender, EventArgs e)
        {
            flavTot = flavTot + flav;

            string flavAPerc = numericUpDown4.Value.ToString();

            if(flavBase == "(VG)")
            {
                decimal temp1 = flav * 1.26M;
                temp1 = Math.Round(temp1, 2, MidpointRounding.AwayFromZero);
                flavGram = temp1.ToString();
            }
            else flavGram = flav.ToString();

            string flavMl = flav.ToString();
            string[] row = new string[] { flavMl, flavGram, flavAPerc + "%", "" };
            dataGridView1.Rows.Add(row);
            dataGridView1.Rows[flavNameNum].HeaderCell.Value = flavBase + flavName;

            decimal temp = decimal.Parse(flavGram);
            flavGramTot = flavGramTot + temp;
            textBox2.Text = "";
            flavNameNum++;
            totalPosition++;

            refresh();
        }

        // 72mg nico base switch
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox4.Checked == true)
            {
                nicoBase = 72;
                checkBox5.Checked = false; checkBox6.Checked = false;
                checkBox7.Checked = false;
                refresh();
            }
        }

        // 50mg nico base switch
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                nicoBase = 50;
                checkBox4.Checked = false; checkBox6.Checked = false;
                checkBox7.Checked = false;
                refresh();
            }
        }

        // 18mg nico base switch
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked == true)
            {
                nicoBase = 18;
                checkBox4.Checked = false; checkBox5.Checked = false;
                checkBox7.Checked = false;
                refresh();
            }
        }

        // 0mg nico base switch
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked == true)
            {
                nicoBase = 0;
                checkBox4.Checked = false; checkBox5.Checked = false;
                checkBox6.Checked = false;
                refresh();
            }
        }

        // Desired nicotine strength
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            nicoAmount = numericUpDown3.Value;
            refresh();
        }
    }
}
