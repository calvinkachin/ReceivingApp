using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Novacode;
using System.IO;
using System.Drawing.Printing;
using System.Drawing.Text;

namespace ReceivingApp
{
    public partial class Form1 : Form
    {
        List<CheckBox> Accessories = new List<CheckBox>();
        List<List<string>> ContactsList = new List<List<string>>();

        List<string> listNumber = new List<string>();
        List<string> listAccount = new List<string>();
        List<string> listCustName = new List<string>();

        public Form1()
        {
            InitializeComponent();

            
            Accessories.Add(checkBox1);
            Accessories.Add(checkBox2);
            Accessories.Add(checkBox3);
            Accessories.Add(checkBox4);
            Accessories.Add(checkBox5);
            Accessories.Add(checkBox6);
            Accessories.Add(checkBox7);
            Accessories.Add(checkBox8);
            Accessories.Add(checkBox9);
            Accessories.Add(checkBox10);
            Accessories.Add(checkBox11);
            Accessories.Add(checkBox12);
            Accessories.Add(checkBox13);
            Accessories.Add(checkBox14);
            Accessories.Add(checkBox15);
            Accessories.Add(checkBox16);
            Accessories.Add(checkBox17);
            Accessories.Add(checkBox18);
            Accessories.Add(checkBox19);
            Accessories.Add(checkBox20);
            Accessories.Add(checkBox21);
            Accessories.Add(checkBox22);
            Accessories.Add(checkBox23);
            Accessories.Add(checkBox24);
            Accessories.Add(checkBox25);
            Accessories.Add(checkBox26);
            Accessories.Add(checkBox27);
            Accessories.Add(checkBox28);
            Accessories.Add(checkBox29);
            Accessories.Add(checkBox30);
            Accessories.Add(checkBox31);
            Accessories.Add(checkBox32);
            Accessories.Add(checkBox33);
            Accessories.Add(checkBox34);
            Accessories.Add(checkBox35);
            Accessories.Add(checkBox36);
            Accessories.Add(checkBox37);
            Accessories.Add(checkBox38);

            ContactsList.Add(listNumber);
            ContactsList.Add(listAccount);
            ContactsList.Add(listCustName);

            UpdateCustomerList();
        }

        private void UpdateCustomerList()
        {
            try {
                var reader = new StreamReader(File.OpenRead(@"T:\\Databases\\c_list.dbs"));

                foreach (List<string> i in ContactsList)
                {
                    i.Clear();
                }

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    listNumber.Add(values[0]);
                    listAccount.Add(values[1]);
                    listCustName.Add(values[2]);
                }
                reader.Close();
            }
            catch
            {
                //Do something
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (txtSR.Text == "")
            {
                MessageBox.Show("Please type in an SR Number.");
                return;
            }

            if (cmbUser.SelectedIndex<0)
            {
                MessageBox.Show("Please select a User.");
                return;
            }

            Double myNum;
            if (Double.TryParse(txtSR.Text, out myNum))
            {
                //do nothing
            }
            else {
                MessageBox.Show("Invalid SR Number. Can not contain any non-numeric characters!");
                return;
            }

            if(radOutgoing.Checked== true)
            {
                bool missing=false;
                //Do outgoing stuff
                string data = "";
                foreach(CheckBox i in Accessories)
                {
                    if (i.Visible == true)
                    {
                        data = data + "1";
                        if(i.Checked== false)
                        {
                            missing = true;
                            break;
                        }
                    }
                    else
                    {
                        data = data + "0";
                    }
                }

                if(missing== true)
                {
                    MessageBox.Show("An accessory was not checked off. If it is missing, please inform Tech Depot.");
                    return;
                }
                else
                {
                    MessageBox.Show("Outgoing unit verified.");
                    try {
                        var writer = new StreamWriter(@"T:\\Databases\shipping_records_" + DateTime.Today.ToString("yyyy") + ".txt", true);
                        writer.WriteLine(DateTime.Now.ToString("yyyy/MM/dd - HH:mm") + ",OUTGOING," + cmbUser.Text + "," + txtSR.Text + "," + txtSerial.Text + "," + txtProduct.Text + "," + txtCustNum.Text + "," + txtCustName.Text + "," + data);
                        writer.Close();
                    }
                    catch
                    {
                        //do nothing

                    }

                    ClearForm();
                }

            }



            if (radIncoming.Checked == true) {
                string outputfile = "";

                if (chkLoaner.Checked == false) {
                    outputfile = "T:\\! SR FOLDERS\\" + txtSR.Text + "\\Inventory.docx";
                }
                else {
                    outputfile = "T:\\! LOANER SR FOLDERS\\" + txtSR.Text + "\\Inventory.docx";
                }


                DocX letter = DocX.Load("Inv.docx");
                
                // Perform the replace:
                letter.ReplaceText("#sr#", txtSR.Text);
                letter.ReplaceText("#sn#", txtSerial.Text);
                letter.ReplaceText("#product#", txtProduct.Text);
                letter.ReplaceText("#custname#", txtCustName.Text);
                letter.ReplaceText("#custnumber#", txtCustNum.Text);

                if (chkLoaner.Checked == true)
                {
                    letter.ReplaceText("#loaner#", "(Service Loaner)");
                }
                else
                {
                    letter.ReplaceText("#loaner#", "");
                }

                letter.ReplaceText("#date#", DateTime.Now.ToString("MMMM dd yyyy, HH:mm"));

                if (radDamaged.Checked == true)
                {
                    letter.ReplaceText("#damage#", "Damaged");
                }
                else if (radUndamaged.Checked == true)
                {
                    letter.ReplaceText("#damage#", "Undamaged");
                }

                if (txtDamageReason.Text != "How?")
                {
                    letter.ReplaceText("#damagereason#", txtDamageReason.Text);
                }
                else
                {
                    letter.ReplaceText("#damagereason#", "");
                }

                string accessorystring = "• ";
                string datastring = "";

                foreach (CheckBox i in Accessories)
                {
                    if (i.Checked == true)
                    {
                        datastring = datastring + "1";

                        if (i.Name == "checkBox18")
                        {
                            if (txtSpO2Cable.Text != "")
                            {
                                accessorystring = accessorystring + i.Text + " -- Lot # " + txtSpO2Cable.Text + "\n• ";
                            }
                            else
                            {
                                accessorystring = accessorystring + i.Text + "\n• ";
                            }
                        }

                        if (i.Name == "checkBox37")
                        {
                            if (txtLegs.Text == "")
                            {
                                txtLegs.Text = "0";
                            }

                            if (txtScrews.Text == "")
                            {
                                txtScrews.Text = "0";
                            }

                            if (txtRubberFeet.Text == "")
                            {
                                txtRubberFeet.Text = "0";
                            }

                            accessorystring = accessorystring + i.Text + " -- " + txtLegs.Text + " Legs, " + txtScrews.Text + " Screws, and " + txtRubberFeet.Text + " Rubber Feet" + "\n• ";
                        }

                        else if (i.Name == "checkBox19")
                        {
                            if (txtSpO2Sensor.Text != "")
                            {
                                accessorystring = accessorystring + i.Text + " -- Lot # " + txtSpO2Sensor.Text + "\n• ";
                            }
                            else
                            {
                                accessorystring = accessorystring + i.Text + "\n• ";
                            }
                        }

                        else if (i.Name == "checkBox20")
                        {
                            if (txtEtCO2Sensor.Text != "")
                            {
                                accessorystring = accessorystring + i.Text + " -- Lot # " + txtEtCO2Sensor.Text + "\n• ";
                            }
                            else
                            {
                                accessorystring = accessorystring + i.Text + "\n• ";
                            }
                        }

                        else if (i.Name == "checkBox24")
                        {
                            if (txtBatterySN1.Text != "")
                            {
                                accessorystring = accessorystring + i.Text + " -- Serial # " + txtBatterySN1.Text + "\n• ";
                            }
                            else
                            {
                                accessorystring = accessorystring + i.Text + "\n• ";
                            }
                        }

                        else if (i.Name == "checkBox25")
                        {
                            if (txtBatterySN2.Text != "")
                            {
                                accessorystring = accessorystring + i.Text + " -- Serial # " + txtBatterySN2.Text + "\n• ";
                            }
                            else
                            {
                                accessorystring = accessorystring + i.Text + "\n• ";
                            }
                        }

                        else if (i.Name == "checkBox30")
                        {
                            if (txtPads1.Text != "")
                            {
                                accessorystring = accessorystring + i.Text + " -- Serial # " + txtPads1.Text + "\n• ";
                            }
                            else
                            {
                                accessorystring = accessorystring + i.Text + "\n• ";
                            }
                        }

                        else if (i.Name == "checkBox31")
                        {
                            if (txtPads2.Text != "")
                            {
                                accessorystring = accessorystring + i.Text + " -- Serial # " + txtPads2.Text + "\n• ";
                            }
                            else
                            {
                                accessorystring = accessorystring + i.Text + "\n• ";
                            }
                        }

                        else if (i.Name == "checkBox32")
                        {
                            if (txtPaddles.Text != "")
                            {
                                accessorystring = accessorystring + i.Text + " -- Serial # " + txtPaddles.Text + "\n• ";
                            }
                            else
                            {
                                accessorystring = accessorystring + i.Text + "\n• ";
                            }
                        }

                        else if (i.Name == "checkBox29")
                        {
                            if (cmbROC.SelectedIndex >= 0)
                            {
                                accessorystring = accessorystring + i.Text + " --  " + cmbROC.Text + "\n• ";
                            }
                            else
                            {
                                accessorystring = accessorystring + i.Text + "\n• ";
                            }
                        }

                        else if (i.Name == "checkBox33")
                        {
                            if (cmbDataCard.SelectedIndex >= 0)
                            {
                                accessorystring = accessorystring + i.Text + " --  " + cmbDataCard.Text + "\n• ";
                            }
                            else
                            {
                                accessorystring = accessorystring + i.Text + "\n• ";
                            }
                        }


                        else
                        {
                            accessorystring = accessorystring + i.Text + "\n• ";
                        }

                    } else
                    {
                        datastring = datastring + "0";
                    }
                }

                accessorystring = accessorystring.Remove(accessorystring.Length - 1);

                letter.ReplaceText("#acc#", accessorystring);

                if (txtComments.Text != "")
                {
                    letter.ReplaceText("#comments#", txtComments.Text);
                }
                else
                {
                    letter.ReplaceText("#comments#", "N/A");
                }

                
                if (chkLoaner.Checked == false) {
                    if (Directory.Exists("T:\\! SR FOLDERS\\" + txtSR.Text + "\\") == false)
                    {
                        Directory.CreateDirectory("T:\\! SR FOLDERS\\" + txtSR.Text + "\\");
                    }
                }
                else
                {
                    if (Directory.Exists("T:\\! LOANER SR FOLDERS\\" + txtSR.Text + "\\") == false)
                    {
                        Directory.CreateDirectory("T:\\! LOANER SR FOLDERS\\" + txtSR.Text + "\\");
                    }
                }
                
              
                string incomingfile = txtSR.Text+"_IncomingInventory.txt";
                
                if (chkLoaner.Checked == false)
                {
                    incomingfile = "T:\\! SR FOLDERS\\" + txtSR.Text + "\\" + txtSR.Text + "_IncomingInventory.txt";
                }
                else
                {
                    incomingfile = "T:\\! LOANER SR FOLDERS\\" + txtSR.Text + "\\" + txtSR.Text + "_IncomingInventory.txt";
                }

                try {
                    var writer = new StreamWriter(incomingfile);
                    writer.WriteLine(txtSerial.Text + "<<" + txtCustNum.Text + "<" + txtCustName.Text);
                    writer.WriteLine(datastring + "<" + txtSpO2Cable.Text + "<" + txtSpO2Sensor.Text + "<" + txtEtCO2Sensor.Text + "<" + txtBatterySN1.Text + "<" + txtBatterySN2.Text + "<" + cmbROC.Text + "<" + txtPads1.Text + "<" + txtPads2.Text + "<" + txtPaddles.Text + "<" + cmbDataCard.Text + "<" + txtLegs.Text + "<" + txtScrews.Text + "<" + txtRubberFeet.Text + "<" + txtComments.Text);
                    writer.Close();
                }
                catch
                {
                    //do nothing
                }

                letter.SaveAs(outputfile);
                //letter.SaveAs(txtSR.Text+".docx");
                letter.Dispose();

                string data = "";

                foreach (CheckBox i in Accessories)
                {
                    if (i.Checked == true)
                    {
                        data = data + "1";
                    }
                    else
                    {
                        data = data + "0";
                    }
                }

                try
                {
                    var writer = new StreamWriter(@"T:\\Databases\shipping_records_" + DateTime.Today.ToString("yyyy") + ".txt", true);
                    writer.WriteLine(DateTime.Now.ToString("yyyy/MM/dd - HH:mm") + ",INCOMING," + cmbUser.Text + "," + txtSR.Text + "," + txtSerial.Text + "," + txtProduct.Text + "," + txtCustNum.Text + "," + txtCustName.Text + "," + data);
                    writer.Close();
                }
                catch
                {
                    //do nothing

                }

                PrintTextBoxContent();
            }
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox18.Checked== true)
            {
                lblSpO2Cable.Visible = true;
                txtSpO2Cable.Visible = true;
            }
            else
            {
                lblSpO2Cable.Visible =false;
                txtSpO2Cable.Visible = false;
                //txtSpO2Cable.Clear();
            }
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox19.Checked == true)
            {
                lblSpO2Sensor.Visible = true;
                txtSpO2Sensor.Visible = true;
            }
            else
            {
                lblSpO2Sensor.Visible = false;
                txtSpO2Sensor.Visible = false;
               // txtSpO2Sensor.Clear();
            }
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox20.Checked == true)
            {
                lblEtCO2Sensor.Visible = true;
                txtEtCO2Sensor.Visible = true;
            }
            else
            {
                lblEtCO2Sensor.Visible = false;
                txtEtCO2Sensor.Visible = false;
                ///txtEtCO2Sensor.Clear();
            }
        }

        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox24.Checked == true)
            {
                lblBatterySN1.Visible = true;
                txtBatterySN1.Visible = true;
            }
            else
            {
                lblBatterySN1.Visible = false;
                txtBatterySN1.Visible = false;
                //txtBatterySN1.Clear();
            }
        }

        private void checkBox25_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox25.Checked == true)
            {
                lblBatterySN2.Visible = true;
                txtBatterySN2.Visible = true;
            }
            else
            {
                lblBatterySN2.Visible = false;
                txtBatterySN2.Visible = false;
               // txtBatterySN2.Clear();
            }
        }

        private void checkBox30_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox30.Checked == true)
            {
                lblPads1.Visible = true;
                txtPads1.Visible = true;
            }
            else
            {
                lblPads1.Visible = false;
                txtPads1.Visible = false;
                //txtPads1.Clear();
            }
        }

        private void checkBox31_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox31.Checked == true)
            {
                lblPads2.Visible = true;
                txtPads2.Visible = true;
            }
            else
            {
                lblPads2.Visible = false;
                txtPads2.Visible = false;
                //txtPads2.Clear();
            }
        }

        private void checkBox32_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox32.Checked == true)
            {
                lblPaddles.Visible = true;
                txtPaddles.Visible = true;
            }
            else
            {
                lblPaddles.Visible = false;
                txtPaddles.Visible = false;
                //txtPaddles.Clear();
            }
        }

        private void checkBox29_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox29.Checked == true)
            {
                cmbROC.Visible = true;
                cmbROC.SelectedIndex = 0;
            }
            else
            {
                cmbROC.Visible = false;
                cmbROC.SelectedIndex = -1;
            }
        }

        private void checkBox33_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox33.Checked == true)
            {
                cmbDataCard.Visible = true;
                cmbDataCard.SelectedIndex = 0;
            }
            else
            {
                cmbDataCard.Visible = false;
                cmbDataCard.SelectedIndex = -1;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void txtSerial_TextChanged(object sender, EventArgs e)
        {
                if (txtSerial.Text.Length > 2)
                {
                    if (txtSerial.Text[0].ToString() + txtSerial.Text[1].ToString() == "AB")
                    {
                        txtProduct.Text = "E-SERIES";
                        return;
                    }

                    if (txtSerial.Text[0].ToString() + txtSerial.Text[1].ToString() == "AR")
                    {
                        txtProduct.Text = "X-SERIES";
                        return;
                    }

                    if (txtSerial.Text[0].ToString() == "T")
                    {
                        txtProduct.Text = "M-SERIES";
                        return;
                    }

                    if (txtSerial.Text[0].ToString() + txtSerial.Text[1].ToString() == "AF")
                    {
                        txtProduct.Text = "R-SERIES";
                        return;
                    }

                    if (txtSerial.Text[0].ToString() + txtSerial.Text[1].ToString() == "AA")
                    {
                        txtProduct.Text = "AED-PRO";
                        return;
                    }

                    if (txtSerial.Text[0].ToString() == "X")
                    {
                        txtProduct.Text = "AED-PLUS";
                        return;
                    }

                    if (txtSerial.Text[0].ToString() + txtSerial.Text[1].ToString() == "AI")
                    {
                        txtProduct.Text = "PROPAQ";
                        return;
                    }

                    int myNum;
                    if (Int32.TryParse(txtSerial.Text[0].ToString(), out myNum) && txtSerial.Text.Length == 5)
                    {
                        txtProduct.Text = "AUTOPULSE";
                        return;
                    }
                    else {
                        // it is not a number
                    }
                }
                else
                {
                    txtProduct.Text = "";
                }
            
        }

        private void txtCustNum_TextChanged(object sender, EventArgs e)
        {
            if (radIncoming.Checked == true)
            {
                for (int i = 0; i < ContactsList[0].Count; i++)
                {
                    if (ContactsList[0][i] == txtCustNum.Text)
                    {
                        txtCustName.Text = ContactsList[2][i];
                        return;
                    }
                }
                txtCustName.Text = "";
            }
        }

        private void PrintTextBoxContent()
        {


            #region Printer Selection
            PrintDialog printDlg = new PrintDialog();
            #endregion

            #region Create Document
            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = "Print Document";
            printDoc.PrintPage += printDoc_PrintPage;
            printDlg.Document = printDoc;
            #endregion

            if (printDlg.ShowDialog() == DialogResult.OK)
                printDoc.Print();
        }

        void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            FontFamily[] fontFamilies;

            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile("Code39.ttf");
            fontFamilies = pfc.Families;
            Font code39 = new Font(fontFamilies[0], 20, FontStyle.Regular);


            e.Graphics.DrawString("*" + txtSR.Text + "*", code39, Brushes.Black, 150, 15);
            e.Graphics.DrawString(txtSR.Text, txtSR.Font, Brushes.Black, 180, 50);
            e.Graphics.DrawString(txtSerial.Text, txtSerial.Font, Brushes.Black, 159, 80);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //PrintTextBoxContent();
        }

        private void btnPrint_Click_1(object sender, EventArgs e)
        {
            //PrintTextBoxContent();

        }

        private void ClearForm()
        {
            foreach(CheckBox i in Accessories)
            {
                i.Checked = false;
            }
            txtSR.Clear();
            txtSerial.Clear();
            txtProduct.Clear();
            txtCustName.Clear();
            txtCustNum.Clear();
            txtComments.Clear();
            txtDamageReason.Clear();
            radUndamaged.Checked = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void checkBox37_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox37.Checked == true)
            {
                lblLegs.Visible = true;
                lblScrews.Visible = true;
                lblRubberFeet.Visible = true;

                txtLegs.Visible = true;
                txtScrews.Visible = true;
                txtRubberFeet.Visible = true;
                
            }
            else
            {
                lblLegs.Visible = false;
                lblScrews.Visible = false;
                lblRubberFeet.Visible = false;

                txtLegs.Visible = false;
                txtScrews.Visible = false;
                txtRubberFeet.Visible = false;

                txtLegs.Clear();
                txtScrews.Clear();
                txtRubberFeet.Clear();
            }
        }

        private void radDamaged_CheckedChanged(object sender, EventArgs e)
        {
            if (radDamaged.Checked == true)
            {
                txtDamageReason.Visible = true;
            }
            else
            {
                txtDamageReason.Clear();
                txtDamageReason.Visible = false;
            }
        }

        private void txtSR_TextChanged(object sender, EventArgs e)
        {

            if (radIncoming.Checked == true)
            {
                //do incoming stuff

                try
                {
                    var reader = new StreamReader(@"T:\Databases\current.dbs");

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        if (txtSR.Text == values[4])
                        {
                            txtSerial.Text = values[0];
                            txtProduct.Text = values[9];
                            txtCustNum.Text = values[14];
                            txtCustName.Text = values[3];
                            chkLoaner.Checked = true;

                            try
                            {
                                var reader2 = new StreamReader("T:\\! LOANER SR FOLDERS\\" + txtSR.Text + "\\" + txtSR.Text + "_OutgoingInventory.txt");

                                var accline = reader2.ReadLine();
                                /*
                                var accvalues = accline.Split('<');

                                txtSerial.Text = accvalues[0];
                                txtProduct.Text = accvalues[1];
                                txtCustNum.Text = accvalues[2];
                                txtCustName.Text = accvalues[3];
                                */
                                accline = reader2.ReadLine();
                                var accvalues2 = accline.Split('<');

                                string databinary = accvalues2[0];

                                /*
                                foreach (CheckBox i in Accessories)
                                {
                                    i.Visible = false;
                                }
                                */

                                for (int i = 0; i < databinary.Length; i++)
                                {
                                    if (databinary[i] == '1')
                                    {
                                        Accessories[i].Checked = true;
                                    }
                                }

                                txtSpO2Cable.Text = accvalues2[1];
                                txtSpO2Sensor.Text = accvalues2[2];
                                txtEtCO2Sensor.Text = accvalues2[3];
                                txtBatterySN1.Text = accvalues2[4];
                                txtBatterySN2.Text = accvalues2[5];
                                cmbROC.Text = accvalues2[6];
                                txtPads1.Text = accvalues2[7];
                                txtPads2.Text = accvalues2[8];
                                txtPaddles.Text = accvalues2[9];
                                cmbDataCard.Text = accvalues2[10];

                                txtLegs.Text = accvalues2[11];
                                txtScrews.Text = accvalues2[12];
                                txtRubberFeet.Text = accvalues2[13];
                                txtComments.Text = accvalues2[14];
                            }
                            catch
                            {

                                
                            }
                            break;
                        }
                    }
                    reader.Close();

                }

                catch
                {
                    txtSerial.Clear();
                    txtProduct.Clear();
                    txtCustNum.Clear();
                    txtCustName.Clear();

                    txtSerial.Enabled = false;
                    txtProduct.Enabled = false;
                    txtCustNum.Enabled = false;
                    txtCustName.Enabled = false;

                    txtSpO2Cable.Clear();
                    txtSpO2Sensor.Clear();
                    txtEtCO2Sensor.Clear();
                    txtBatterySN1.Clear();
                    txtBatterySN2.Clear();
                    txtPads1.Clear();
                    txtPads2.Clear();
                    txtPaddles.Clear();

                    txtSpO2Cable.Visible = false;
                    txtSpO2Sensor.Visible = false;
                    txtEtCO2Sensor.Visible = false;
                    txtBatterySN1.Visible = false;
                    txtBatterySN2.Visible = false;
                    cmbROC.Visible = false;
                    txtPads1.Visible = false;
                    txtPads2.Visible = false;
                    txtPaddles.Visible = false;
                    cmbDataCard.Visible = false;

                    lblSpO2Cable.Visible = false;
                    lblSpO2Sensor.Visible = false;
                    lblEtCO2Sensor.Visible = false;
                    lblBatterySN1.Visible = false;
                    lblBatterySN2.Visible = false;
                    lblPads1.Visible = false;
                    lblPads2.Visible = false;
                    lblPaddles.Visible = false;

                    cmbDataCard.SelectedIndex = -1;
                    cmbROC.SelectedIndex = -1;

                    foreach (CheckBox i in Accessories)
                    {
                        i.Visible = false;
                    }
                }

            }

            if (radOutgoing.Checked == true)
            {
                //Try to check whether or not this is a loaner

                try
                {
                    /*
                    var reader = new StreamReader(@"T:\Databases\current.dbs");

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        if (txtSR.Text == values[4])
                        {
                            txtSerial.Text = values[0];
                            txtProduct.Text = values[9];
                            txtCustNum.Text = values[14];
                            txtCustName.Text = values[3];
                            chkLoaner.Checked = true;
                            */
                    try
                    {
                        var reader2 = new StreamReader("T:\\! LOANER SR FOLDERS\\" + txtSR.Text + "\\" + txtSR.Text + "_OutgoingInventory.txt");

                        var accline = reader2.ReadLine();
                        
                        var accvalues = accline.Split('<');
                        
                        txtSerial.Text = accvalues[0];

                        //txtProduct.Text = accvalues[1];
                        txtCustNum.Text = accvalues[2];
                        txtCustName.Text = accvalues[3];
                        
                        accline = reader2.ReadLine();
                        var accvalues2 = accline.Split('<');
                        chkLoaner.Checked = true;

                        string databinary = accvalues2[0];

                        foreach (CheckBox i in Accessories)
                        {
                            i.Visible = false;
                        }

                        for (int i = 0; i < databinary.Length; i++)
                        {
                            if (databinary[i] == '1')
                            {
                                Accessories[i].Visible = true;
                            }
                        }

                        txtSpO2Cable.Text = accvalues2[1];
                        if (txtSpO2Cable.Text != "")
                        {
                            txtSpO2Cable.Visible = true;
                            lblSpO2Cable.Visible = true;
                        }

                        txtSpO2Sensor.Text = accvalues2[2];
                        if (txtSpO2Sensor.Text != "")
                        {
                            txtSpO2Sensor.Visible = true;
                            lblSpO2Sensor.Visible = true;
                        }

                        txtEtCO2Sensor.Text = accvalues2[3];
                        if (txtEtCO2Sensor.Text != "")
                        {
                            txtEtCO2Sensor.Visible = true;
                            lblEtCO2Sensor.Visible = true;
                        }

                        txtBatterySN1.Text = accvalues2[4];
                        if (txtBatterySN1.Text != "")
                        {
                            txtBatterySN1.Visible = true;
                            lblBatterySN1.Visible = true;
                        }

                        txtBatterySN2.Text = accvalues2[5];
                        if (txtBatterySN2.Text != "")
                        {
                            txtBatterySN2.Visible = true;
                            lblBatterySN2.Visible = true;
                        }

                        cmbROC.Text = accvalues2[6];
                        if (cmbROC.Text != "")
                        {
                            cmbROC.Visible = true;
                        }

                        txtPads1.Text = accvalues2[7];
                        if (txtPads1.Text != "")
                        {
                            txtPads1.Visible = true;
                            lblPads1.Visible = true;
                        }

                        txtPads2.Text = accvalues2[8];
                        if (txtPads2.Text != "")
                        {
                            txtPads2.Visible = true;
                            lblPads2.Visible = true;
                        }

                        txtPaddles.Text = accvalues2[9];
                        if (txtPaddles.Text != "")
                        {
                            txtPaddles.Visible = true;
                            lblPaddles.Visible = true;
                        }

                        cmbDataCard.Text = accvalues2[10];
                        if (cmbDataCard.Text != "")
                        {
                            cmbDataCard.Visible = true;
                        }

                        txtLegs.Text = accvalues2[11];
                        if (txtLegs.Text != "")
                        {
                            txtLegs.Visible = true;
                            lblLegs.Visible = true;
                        }

                        txtScrews.Text = accvalues2[12];
                        if (txtScrews.Text != "")
                        {
                            txtScrews.Visible = true;
                            lblScrews.Visible = true;
                        }

                        txtRubberFeet.Text = accvalues2[13];
                        if (txtRubberFeet.Text != "")
                        {
                            txtRubberFeet.Visible = true;
                            lblRubberFeet.Visible = true;
                        }

                        txtComments.Text = accvalues2[14];
                        if (txtComments.Text != "")
                        {
                            txtComments.Visible = true;
                            lblComments.Visible = true;
                        }
                    }
                    catch
                    {
                        txtSerial.Clear();
                        txtProduct.Clear();
                        txtCustNum.Clear();
                        txtCustName.Clear();

                        txtSerial.Enabled = false;
                        txtProduct.Enabled = false;
                        txtCustNum.Enabled = false;
                        txtCustName.Enabled = false;

                        txtSpO2Cable.Clear();
                        txtSpO2Sensor.Clear();
                        txtEtCO2Sensor.Clear();
                        txtBatterySN1.Clear();
                        txtBatterySN2.Clear();
                        txtPads1.Clear();
                        txtPads2.Clear();
                        txtPaddles.Clear();

                        txtSpO2Cable.Visible = false;
                        txtSpO2Sensor.Visible = false;
                        txtEtCO2Sensor.Visible = false;
                        txtBatterySN1.Visible = false;
                        txtBatterySN2.Visible = false;
                        cmbROC.Visible = false;
                        txtPads1.Visible = false;
                        txtPads2.Visible = false;
                        txtPaddles.Visible = false;
                        cmbDataCard.Visible = false;

                        lblSpO2Cable.Visible = false;
                        lblSpO2Sensor.Visible = false;
                        lblEtCO2Sensor.Visible = false;
                        lblBatterySN1.Visible = false;
                        lblBatterySN2.Visible = false;
                        lblPads1.Visible = false;
                        lblPads2.Visible = false;
                        lblPaddles.Visible = false;

                        cmbDataCard.SelectedIndex = -1;
                        cmbROC.SelectedIndex = -1;

                        foreach (CheckBox i in Accessories)
                        {
                            i.Visible = false;
                        }
                    }

                    //break;
                    //}
                    //}
                    //reader.Close();

                    //Loaner with that SR was not found, check SR folders
                    try
                    {

                        var reader = new StreamReader("T:\\! SR FOLDERS\\" + txtSR.Text + "\\" + txtSR.Text + "_IncomingInventory.txt");

                        var line = reader.ReadLine();
                        var values = line.Split('<');

                        txtSerial.Text = values[0];
                        txtProduct.Text = values[1];
                        txtCustNum.Text = values[2];
                        txtCustName.Text = values[3];

                        line = reader.ReadLine();
                        var values2 = line.Split('<');

                        string databinary = values2[0];

                        foreach (CheckBox i in Accessories)
                        {
                            i.Visible = false;
                        }

                        for (int i = 0; i < databinary.Length; i++)
                        {
                            if (databinary[i] == '1')
                            {
                                Accessories[i].Visible = true;
                            }
                        }

                        txtSpO2Cable.Text = values2[1];
                        txtSpO2Sensor.Text = values2[2];
                        txtEtCO2Sensor.Text = values2[3];
                        txtBatterySN1.Text = values2[4];
                        txtBatterySN2.Text = values2[5];
                        cmbROC.Text = values2[6];
                        txtPads1.Text = values2[7];
                        txtPads2.Text = values2[8];
                        txtPaddles.Text = values2[9];
                        cmbDataCard.Text = values2[10];

                        txtLegs.Text = values2[11];
                        txtScrews.Text = values2[12];
                        txtRubberFeet.Text = values2[13];
                        txtComments.Text = values2[14];

                    }
                    catch
                    {
                        // do nothing
                    }

                }
                catch
                {
                    // do nothing
                }
            }

        }

        private void txtSR_Leave(object sender, EventArgs e)
        {
            
            
        }

        private void radIncoming_CheckedChanged(object sender, EventArgs e)
        {
            if (radIncoming.Checked)
            {
                ClearForm();
                grpDamage.Visible = true;
                grpAcc.Top = 216;
                grpAcc.Text = "4. Select Incoming Accessories";
                btnGenerate.Text = "Generate";

                txtSerial.Enabled = true;
                txtProduct.Enabled = true;
                txtCustNum.Enabled = true;
                txtCustName.Enabled = true;

                txtSpO2Cable.Clear();
                txtSpO2Sensor.Clear();
                txtEtCO2Sensor.Clear();
                txtBatterySN1.Clear();
                txtBatterySN2.Clear();
                txtPads1.Clear();
                txtPads2.Clear();
                txtPaddles.Clear();

                txtSpO2Cable.Visible = false;
                txtSpO2Sensor.Visible = false;
                txtEtCO2Sensor.Visible = false;
                txtBatterySN1.Visible = false;
                txtBatterySN2.Visible = false;
                cmbROC.Visible = false;
                txtPads1.Visible = false;
                txtPads2.Visible = false;
                txtPaddles.Visible = false;
                cmbDataCard.Visible = false;

                lblSpO2Cable.Visible = false;
                lblSpO2Sensor.Visible = false;
                lblEtCO2Sensor.Visible = false;
                lblBatterySN1.Visible = false;
                lblBatterySN2.Visible = false;
                lblPads1.Visible = false;
                lblPads2.Visible = false;
                lblPaddles.Visible = false;

                cmbDataCard.SelectedIndex = -1;
                cmbROC.SelectedIndex = -1;

                foreach (CheckBox i in Accessories)
                {
                    i.Visible = true;
                }
            }
        }

        private void radOutgoing_CheckedChanged(object sender, EventArgs e)
        {
            if (radOutgoing.Checked)
            {
                ClearForm();
                grpDamage.Visible = false;
                grpAcc.Top = 157;
                grpAcc.Text = "3. Check Accessories";
                btnGenerate.Text = "Verify";

                txtSerial.Enabled = false;
                txtProduct.Enabled = false;
                txtCustNum.Enabled = false;
                txtCustName.Enabled = false;

                foreach (CheckBox i in Accessories)
                {
                    i.Visible = false;
                }

                

                   
            }
        }

        private void txtSR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {/*
                if (radOutgoing.Checked == true)
                {
                    try
                    {
                        
                        var reader = new StreamReader("T:\\! SR FOLDERS\\" + txtSR.Text + "\\" + txtSR.Text + "_IncomingInventory.txt");

                        var line=reader.ReadLine();
                        var values = line.Split('<');

                        txtSerial.Text = values[0];
                        txtProduct.Text = values[1];
                        txtCustNum.Text = values[2];
                        txtCustName.Text = values[3];

                        line = reader.ReadLine();
                        var values2 = line.Split('<');

                        string databinary = values2[0];

                        for(int i = 0; i < databinary.Length; i++)
                        {
                            if (databinary[i] == '1')
                            {
                                Accessories[i].Visible = true;
                            }
                        }

                        txtSpO2Cable.Text = values2[1];
                        txtSpO2Sensor.Text = values2[2];
                        txtEtCO2Sensor.Text = values2[3];
                        txtBatterySN1.Text = values2[4];
                        txtBatterySN2.Text = values2[5];
                        cmbROC.Text = values2[6];
                        txtPads1.Text = values2[7];
                        txtPads2.Text = values2[8];
                        txtPaddles.Text = values2[9];
                        cmbDataCard.Text = values2[10];

                        txtLegs.Text = values2[11];
                        txtScrews.Text = values2[12];
                        txtRubberFeet.Text = values2[13];
                        txtComments.Text = values2[14];

                    }
                    catch
                    {
                        //do nothing
                    }
                    
                }
                */
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void txtDamageReason_Enter(object sender, EventArgs e)
        {
            if (txtDamageReason.Text == "How?")
            {
                txtDamageReason.Text = "";
                txtDamageReason.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void txtDamageReason_Leave(object sender, EventArgs e)
        {
            if (txtDamageReason.Text == ""||txtDamageReason.Text=="How?")
            {
                txtDamageReason.Text = "How?";
                txtDamageReason.ForeColor = System.Drawing.Color.Gray;
            }
        }
    }
}
