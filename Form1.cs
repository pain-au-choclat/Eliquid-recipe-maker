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
        public void refresh()
        {
            // Nico refresh
            if (nicoBase == 0)
            {
                totNic = 0;
                dataGridView1.Rows[2].Cells[0].Value = totNic;
            }
            else
            {
                totNic = ((nicoAmount / nicoBase) * amountToMake) - (diluent / 3);
                totNic = Math.Round(totNic, 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[2].Cells[0].Value = totNic;
            }

            // Refresh PG/VG
            pg = pg - (numericUpDown2.Value / 3);
            pgPercent = ((pg / 100) * amountToMake);
            vgPercent = (vg / 100) * amountToMake;

            if(pgNic == true) pgPercent = pgPercent - totNic;
            if (vgNic == true) vgPercent = vgPercent - totNic;
            if(mix == true)
            {
                decimal pgMix = totNic * 0.30M;
                decimal vgMix = totNic * 0.70M;
                pgPercent = pgPercent - pgMix; pgPercent = Math.Round(pgPercent, 2, MidpointRounding.AwayFromZero);
                vgPercent = vgPercent - vgMix; vgPercent = Math.Round(vgPercent, 2, MidpointRounding.AwayFromZero);
            }

            if (vgPercent < 1)
            {
                vgPercent = 0;
                dataGridView1.Rows[1].Cells[0].Value = vgPercent;
                label14.Text = "Incorrect mix please adjust PG/VG ratio";
            }
            else
            {
                //vgPercent = vgPercent - 0.333333333333333M;
                vgPercent = Math.Round(vgPercent, 2, MidpointRounding.AwayFromZero);
                dataGridView1.Rows[1].Cells[0].Value = vgPercent;
            }
               
            if (pgPercent < 1)
            {
                pgPercent = 0;
                dataGridView1.Rows[0].Cells[0].Value = pgPercent;
                label14.Text = "Incorrect mix please adjust PG/VG ratio";
            }
            else dataGridView1.Rows[0].Cells[0].Value = pgPercent;

            if (pgPercent > 0 && vgPercent > 0) label14.Text = "";


            // Diluent refresh
            diluent = (numericUpDown2.Value / 100) * amountToMake;
            dataGridView1.Rows[3].Cells[0].Value = diluent;

            
        }

        // Initialize variables
        decimal amountToMake;
        decimal diluent;
        decimal vgPercent = 10;
        decimal pgPercent = 10;
        decimal pg = 50;
        decimal vg = 50;
        decimal nicoAmount = 6, nicoBase, totNic;
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

            // Use below to produce outputs
            string[] row = new string[] { "1", "Product 1", "1000", "" };
            dataGridView1.Rows.Add(row);
            row = new string[] { "", "Product 2", "2000", "" };
            dataGridView1.Rows.Add(row);
            row = new string[] { "3", "Product 3", "3000", "" };
            dataGridView1.Rows.Add(row);
            row = new string[] { "4", "Product 4", "4000", "" };
            dataGridView1.Rows.Add(row);



            // Start up defaults

            amountToMake = numericUpDown1.Value;
            dataGridView1.Rows[8].Cells[0].Value = amountToMake;
            dataGridView1.Rows[0].Cells[0].Value = pgPercent;
            dataGridView1.Rows[1].Cells[0].Value = vgPercent;
            label5.Text = pg.ToString() + " PG%";
            label6.Text = vg.ToString() + " VG%";
            checkBox1.Checked = true;


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string name = textBox1.Text;
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            pg = trackBar1.Value * 5;
            vg = 100 - pg;
            label5.Text = pg.ToString() + " PG%";
            label6.Text = vg.ToString() + " VG%";

            refresh();            
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            amountToMake = numericUpDown1.Value;
            dataGridView1.Rows[8].Cells[0].Value = amountToMake;
            
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
            decimal temp = diluent / 3;
            label15.Text = temp.ToString();
            refresh();
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

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            nicoAmount = numericUpDown3.Value;
            refresh();
        }
    }
}
