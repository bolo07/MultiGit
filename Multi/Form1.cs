using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Diagnostics;

namespace Multi
{
    
    public partial class Form1 : Form
    {

        ////////////////////////////////zmienne//////////////////////////

        public int[] odbiorcy; //lista węzłów odbiorczych
        public int nadawca; // węzeł nadawczy
        public siec[] graf; //tablica sąsiadów przechowująca model sieci
        public bool check = false;
        int ile_node; //ilość węzłów w grafie
        Int16 m_generowania = 1, m_selekcji =1, m_krzyzowania =1;

        public static int seed = Environment.TickCount;
        public static System.Random x = new Random(seed);
        /////////////////////////////////////////////////////////////////
        public Form1() 
        {
            InitializeComponent();
            nadawca =Convert.ToInt16(textBox4.Text);
            odbiorcy = new int[Convert.ToInt16(numericUpDown1.Value) + 1];
                    
           
        }

        //inicjalizacja odbiorców
        
       
        public void button1_Click(object sender, EventArgs e)
        {
            //wczytanie pliku 
            OpenFileDialog fdb = new OpenFileDialog();
			 fdb.Filter = "BRITE files (*.brite)|*.brite|txt files (*.txt)|*.txt|All files (*.*)|*.*";
			 System.Windows.Forms.DialogResult result = fdb.ShowDialog();
			 if (result == System.Windows.Forms.DialogResult.OK)
			 {
				 textBox1.Text = fdb.FileName;
             }
            //odczytanie ilości węzłów z pliku
             System.IO.StreamReader file = new System.IO.StreamReader(textBox1.Text);
             ile_node = Convert.ToInt32(file.ReadLine());
            //utworzenie grafu z modelem sieci i inicjalizacja obiektu
              graf = new siec[ile_node];
             for (int i = 0; i < ile_node; i++)
                 graf[i] = null;


             siec temp3 = new siec();

            //wczytanie grafu z pliku
             graf = temp3.wczytaj(textBox1.Text);
             file.Close();
            
            ////test wczytania/////
             
            for (int i = 0; i < ile_node; i++) //wyswietla Lsonsiadow
             {
                 Debug.Write("graf[" + i + "] =");
                 temp3 = graf[i];
                 while (temp3 != null)
                 {
                     Debug.Write(temp3.to + " ");
                     temp3 = temp3.next;
                 }
                 Debug.WriteLine("");
             }//////////////////////////////////koniec wyswietla 

            odbiorcy[0] = Convert.ToInt16(textBox4.Text); ;
            for (int i = 1; i <= Convert.ToInt16(numericUpDown1.Value); i++)
            {
                
                var txtBox = this.Controls.Find("textBox" + (i + 4), true);

                odbiorcy[i] = Convert.ToInt16(txtBox[0].Text);
            }
            

          }//wczytywanie modelu sieci z pliku *.brite

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar); 
        }
        public void textBox4_TextChanged(object sender, EventArgs e) //wprowadzanie węzła nadawczego
        {
            if (textBox4.Text != "")
            {
                odbiorcy[0] =Convert.ToInt16(textBox4.Text);
              
            }
            
        }

       
        public void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            
            odbiorcy = new int[Convert.ToInt16(numericUpDown1.Value)+1];
         
                   
                if (numericUpDown1.Value > 0 && check ==false) { textBox5.Visible = true; label4.Visible = true; label7.Visible = true;} else { textBox5.Visible = false; label4.Visible = false; label7.Visible = false;}
                if (numericUpDown1.Value > 1 && check == false) { textBox6.Visible = true; } else { textBox6.Visible = false; }
                if (numericUpDown1.Value > 2 && check == false) { textBox7.Visible = true; } else { textBox7.Visible = false; }
                if (numericUpDown1.Value > 3 && check == false) { textBox8.Visible = true; } else { textBox8.Visible = false; }
                if (numericUpDown1.Value > 4 && check == false) { textBox9.Visible = true; } else { textBox9.Visible = false; }
                if (numericUpDown1.Value > 5 && check == false) { textBox10.Visible = true; } else { textBox10.Visible = false; }
                if (numericUpDown1.Value > 6 && check == false) { textBox11.Visible = true; } else { textBox11.Visible = false; }
                if (numericUpDown1.Value > 7 && check == false) { textBox12.Visible = true; } else { textBox12.Visible = false; }
                if (numericUpDown1.Value > 8 && check == false) { textBox13.Visible = true; label6.Visible = true; } else { textBox13.Visible = false; label6.Visible = false; }
                if (numericUpDown1.Value > 9 && check == false) { textBox14.Visible = true; label5.Visible = true; } else { textBox14.Visible = false; label5.Visible = false; }
                if (numericUpDown1.Value > 10 && check == false) { textBox15.Visible = true; } else { textBox15.Visible = false; }
                if (numericUpDown1.Value > 11 && check == false) { textBox16.Visible = true; } else { textBox16.Visible = false; }
                if (numericUpDown1.Value > 12 && check == false) { textBox17.Visible = true; } else { textBox17.Visible = false; }
                if (numericUpDown1.Value > 13 && check == false) { textBox18.Visible = true; } else { textBox18.Visible = false; }
                if (numericUpDown1.Value > 14 && check == false) { textBox19.Visible = true; } else { textBox19.Visible = false; }
                if (numericUpDown1.Value > 15 && check == false) { textBox20.Visible = true; } else { textBox20.Visible = false; }
                if (numericUpDown1.Value > 16 && check == false) { textBox21.Visible = true; label8.Visible = true; } else { textBox21.Visible = false; label8.Visible = false; }
                if (numericUpDown1.Value > 17 && check == false) { textBox22.Visible = true; label9.Visible = true; } else { textBox22.Visible = false; label9.Visible = false; }
                if (numericUpDown1.Value > 18 && check == false) { textBox23.Visible = true; } else { textBox23.Visible = false; }
                if (numericUpDown1.Value > 19 && check == false) { textBox24.Visible = true; } else { textBox24.Visible = false; }
                if (numericUpDown1.Value > 20 && check == false) { textBox25.Visible = true; } else { textBox25.Visible = false; }
                if (numericUpDown1.Value > 21 && check == false) { textBox26.Visible = true; } else { textBox26.Visible = false; }
                if (numericUpDown1.Value > 22 && check == false) { textBox27.Visible = true; } else { textBox27.Visible = false; }
                if (numericUpDown1.Value > 23 && check == false) { textBox28.Visible = true; } else { textBox28.Visible = false; }
                if (numericUpDown1.Value > 24 && check == false) { textBox29.Visible = true; label10.Visible = true; } else { textBox29.Visible = false; label10.Visible = false; }
                if (numericUpDown1.Value > 25 && check == false) { textBox30.Visible = true; label11.Visible = true; } else { textBox30.Visible = false; label11.Visible = false; }
                if (numericUpDown1.Value > 26 && check == false) { textBox31.Visible = true; } else { textBox31.Visible = false; }
                if (numericUpDown1.Value > 27 && check == false) { textBox32.Visible = true; } else { textBox32.Visible = false; }
                if (numericUpDown1.Value > 28 && check == false) { textBox33.Visible = true; } else { textBox33.Visible = false; }
                if (numericUpDown1.Value > 29 && check == false) { textBox34.Visible = true; } else { textBox34.Visible = false; }
                if (numericUpDown1.Value > 30 && check == false) { textBox35.Visible = true; } else { textBox35.Visible = false; }
                if (numericUpDown1.Value > 31 && check == false) { textBox36.Visible = true; } else { textBox36.Visible = false; }

            }  //wprowadzanie odbiorców
        
        public void button2_Click(object sender, EventArgs e) //KPP
        {
            for (int i = 1; i <= Convert.ToInt16(numericUpDown1.Value); i++)
            {

                var txtBox = this.Controls.Find("textBox" + (i + 4), true);
                odbiorcy[i] = Convert.ToInt16(txtBox[0].Text);
            }
            odbiorcy[0] = Convert.ToInt16(textBox4.Text);

            heurystyki kpp = new heurystyki();
            kpp.KPP(Convert.ToInt16(textBox4.Text), odbiorcy, odbiorcy.Length, graf, ile_node, Convert.ToInt16(textBox2.Text));
       
        }

        public void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            check = checkBox1.Checked;
            if (checkBox1.Checked == false) { numericUpDown1.Value = 0; }
           
                label4.Visible = false;
                label7.Visible = false;
                textBox5.Visible = false;
                textBox6.Visible = false;
                textBox7.Visible = false;
                textBox8.Visible = false;
                textBox9.Visible = false;
                textBox10.Visible = false;
                textBox11.Visible = false;
                textBox12.Visible = false;
                textBox13.Visible = false; label6.Visible = false;
                textBox14.Visible = false; label5.Visible = false;
                textBox15.Visible = false;
                textBox16.Visible = false;
                textBox17.Visible = false;
                textBox18.Visible = false;
                textBox19.Visible = false;
                textBox20.Visible = false;
                textBox21.Visible = false; label8.Visible = false;
                textBox22.Visible = false; label9.Visible = false;
                textBox23.Visible = false;
                textBox24.Visible = false;
                textBox25.Visible = false;
                textBox26.Visible = false;
                textBox27.Visible = false;
                textBox28.Visible = false;
                textBox29.Visible = false; label10.Visible = false;
                textBox30.Visible = false; label11.Visible = false;
                textBox31.Visible = false;
                textBox32.Visible = false;
                textBox33.Visible = false;
                textBox34.Visible = false;
                textBox35.Visible = false;
                textBox36.Visible = false;

                for (int i = 0; i < Convert.ToInt16(numericUpDown1.Value); i++)
                {

                    var txtBox = this.Controls.Find("textBox" + (i + 5), true);

                    odbiorcy[i] = Convert.ToInt16(txtBox[0].Text);
                }
        }//losowanie odbiorców do zrobienia w kolejnej kontrolce

        //obsługa kontrolek wprowadzania odbiorców
        #region 

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
       {
           e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);      
        }

       
        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

       
        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox20_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox19_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox18_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox17_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox16_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox15_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox21_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox22_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox23_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox24_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox25_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox26_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox27_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox28_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox29_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

     
        private void textBox30_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox31_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox32_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox33_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox34_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox35_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox36_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

            if (textBox5.Text != "")
            {
                odbiorcy[1] = Convert.ToInt16(textBox5.Text);
            }


        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text != "")
            {
                odbiorcy[2] = Convert.ToInt16(textBox6.Text);
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text != "")
            {
                odbiorcy[3] = Convert.ToInt16(textBox7.Text);
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text != "")
            {
                odbiorcy[4] = Convert.ToInt16(textBox8.Text);
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (textBox9.Text != "")
            {
                odbiorcy[5] = Convert.ToInt16(textBox9.Text);
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (textBox10.Text != "")
            {
                odbiorcy[6] = Convert.ToInt16(textBox10.Text);
            }
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (textBox11.Text != "")
            {
                odbiorcy[7] = Convert.ToInt16(textBox11.Text);
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            if (textBox12.Text != "")
            {
                odbiorcy[8] = Convert.ToInt16(textBox12.Text);
            }
        }

        private void textBox20_TextChanged(object sender, EventArgs e)
        {
            if (textBox20.Text != "")
            {
                odbiorcy[16] = Convert.ToInt16(textBox20.Text);
            }
        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {
            if (textBox19.Text != "")
            {
                odbiorcy[15] = Convert.ToInt16(textBox19.Text);
            }
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {
            if (textBox18.Text != "")
            {
                odbiorcy[14] = Convert.ToInt16(textBox18.Text);
            }
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            if (textBox17.Text != "")
            {
                odbiorcy[13] = Convert.ToInt16(textBox17.Text);
            }
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            if (textBox16.Text != "")
            {
                odbiorcy[12] = Convert.ToInt16(textBox16.Text);
            }
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            if (textBox15.Text != "")
            {
                odbiorcy[11] = Convert.ToInt16(textBox15.Text);
            }
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            if (textBox14.Text != "")
            {
                odbiorcy[10] = Convert.ToInt16(textBox14.Text);
            }
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            if (textBox13.Text != "")
            {
                odbiorcy[9] = Convert.ToInt16(textBox13.Text);
            }
        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {
            if (textBox21.Text != "")
            {
                odbiorcy[17] = Convert.ToInt16(textBox21.Text);
            }
        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {
            if (textBox22.Text != "")
            {
                odbiorcy[18] = Convert.ToInt16(textBox22.Text);
            }
        }

        private void textBox23_TextChanged(object sender, EventArgs e)
        {
            if (textBox23.Text != "")
            {
                odbiorcy[19] = Convert.ToInt16(textBox23.Text);
            }
        }

        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            if (textBox24.Text != "")
            {
                odbiorcy[20] = Convert.ToInt16(textBox24.Text);
            }
        }

        private void textBox25_TextChanged(object sender, EventArgs e)
        {
            if (textBox25.Text != "")
            {
                odbiorcy[21] = Convert.ToInt16(textBox25.Text);
            }
        }

        private void textBox26_TextChanged(object sender, EventArgs e)
        {
            if (textBox26.Text != "")
            {
                odbiorcy[22] = Convert.ToInt16(textBox26.Text);
            }
        }

        private void textBox27_TextChanged(object sender, EventArgs e)
        {
            if (textBox27.Text != "")
            {
                odbiorcy[23] = Convert.ToInt16(textBox27.Text);
            }
        }

        private void textBox28_TextChanged(object sender, EventArgs e)
        {
            if (textBox28.Text != "")
            {
                odbiorcy[24] = Convert.ToInt16(textBox28.Text);
            }
        }

        private void textBox29_TextChanged(object sender, EventArgs e)
        {
            if (textBox29.Text != "")
            {
                odbiorcy[25] = Convert.ToInt16(textBox29.Text);
            }
        }

        private void textBox30_TextChanged(object sender, EventArgs e)
        {
            if (textBox30.Text != "")
            {
                odbiorcy[26] = Convert.ToInt16(textBox30.Text);
            }
        }

        private void textBox31_TextChanged(object sender, EventArgs e)
        {
            if (textBox31.Text != "")
            {
                odbiorcy[27] = Convert.ToInt16(textBox31.Text);
            }
        }

        private void textBox32_TextChanged(object sender, EventArgs e)
        {
            if (textBox32.Text != "")
            {
                odbiorcy[28] = Convert.ToInt16(textBox32.Text);
            }
        }

        private void textBox33_TextChanged(object sender, EventArgs e)
        {
            if (textBox32.Text != "")
            {
                odbiorcy[29] = Convert.ToInt16(textBox32.Text);
            }
        }

        private void textBox34_TextChanged(object sender, EventArgs e)
        {
            if (textBox34.Text != "")
            {
                odbiorcy[30] = Convert.ToInt16(textBox34.Text);
            }
        }

        private void textBox35_TextChanged(object sender, EventArgs e)
        {
            if (textBox35.Text != "")
            {
                odbiorcy[31] = Convert.ToInt16(textBox35.Text);
            }
        }

        private void textBox36_TextChanged(object sender, EventArgs e)
        {
            if (textBox36.Text != "")
            {
                odbiorcy[32] = Convert.ToInt16(textBox36.Text);
            }
        }
        #endregion 

      
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar); 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= Convert.ToInt16(numericUpDown1.Value); i++)
            {

                var txtBox = this.Controls.Find("textBox" + (i + 4), true);
                odbiorcy[i] = Convert.ToInt16(txtBox[0].Text);
            }
            odbiorcy[0] = nadawca;

            heurystyki cspt = new heurystyki();
            cspt.CSPT(Convert.ToInt16(textBox4.Text), odbiorcy, odbiorcy.Length, graf, ile_node, Convert.ToInt16(textBox3.Text));
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton3.Checked == false) { this.numericUpDown3.Enabled = false; } else { this.numericUpDown3.Enabled = true; }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
           
            m_generowania = 1;
        }

       public void button4_Click(object sender, EventArgs e) ////Algorytm genetyczny
        {
           
            for (int i = 1; i <= Convert.ToInt16(numericUpDown1.Value); i++)
            {

                var txtBox = this.Controls.Find("textBox" + (i + 4), true);
                odbiorcy[i] = Convert.ToInt16(txtBox[0].Text);
            }
            odbiorcy[0] = Convert.ToInt16(textBox4.Text);
            
            AG chromosom = new AG();
           
            chromosom.algorytm_genetyczny(Convert.ToInt16(numericUpDown2.Value), m_generowania, m_selekcji, m_krzyzowania, Convert.ToDouble(numericUpDown4.Value), graf, odbiorcy, Convert.ToInt16( numericUpDown5.Value));

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            m_generowania = 2;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            m_selekcji = 1;
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            m_selekcji = 2;
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            m_selekcji = 3;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            m_krzyzowania = 1;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            m_krzyzowania = 2;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            m_krzyzowania = 3;
        }

      

       
    

    }
    }
