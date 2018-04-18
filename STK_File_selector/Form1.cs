using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STK_File_selector
{
    public partial class NPAS_to_STK : Form
    {

        NPAS_To_STK catl_data_transfer;

        public NPAS_to_STK()
        {
            
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {//station button
            if (catl_data_transfer == null) catl_data_transfer = new NPAS_To_STK();
            catl_data_transfer.set_mask_file(this.textBox1.Text);
            catl_data_transfer.Station_generation();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void Get_File_Btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Mask File";
            //theDialog.Filter = "TXT files|*.txt";
            //theDialog.InitialDirectory = @"C:\";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                //MessageBox.Show(theDialog.FileName.ToString());
                this.textBox1.Text = theDialog.FileName.ToString();
                this.Refresh();
                if (catl_data_transfer == null) catl_data_transfer = new NPAS_To_STK();
                catl_data_transfer.set_mask_file(this.textBox1.Text);
                catl_data_transfer.load_Station_File();
            }
        }


        private void Exit_Btn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Orbit File";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                //MessageBox.Show(theDialog.FileName.ToString());
                this.textBox2.Text = theDialog.FileName.ToString();
                this.Refresh();
                if (catl_data_transfer == null) catl_data_transfer = new NPAS_To_STK();
                this.catl_data_transfer.set_orbit_file(this.textBox2.Text);
                catl_data_transfer.load_orbit_file();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {//orbit button
            if (catl_data_transfer == null) catl_data_transfer = new NPAS_To_STK();
            catl_data_transfer.set_orbit_file(this.textBox2.Text);
            catl_data_transfer.Orbit_generation();
            catl_data_transfer.Set_intervalFiles();
        }

        private void TLFile_PB_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open TL File";
            //theDialog.Filter = "TXT files|*.txt";
            //theDialog.InitialDirectory = @"C:\";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                //MessageBox.Show(theDialog.FileName.ToString());
                if (catl_data_transfer == null) catl_data_transfer = new NPAS_To_STK();
                this.TLFile_TB.Text = theDialog.FileName.ToString();
                this.Refresh();
                catl_data_transfer.set_timeline_file(this.TLFile_TB.Text);
                catl_data_transfer.load_timeline_file();
            }
        }

        private void Run_PB_Click(object sender, EventArgs e)
        {
            //catl_data_transfer.set_tdrs_enabled(tdrscheckBox.Checked);
            
            catl_data_transfer.set_startenddates(this.StartdateTimePicker.Text, this.EnddateTimePicker.Text);
            catl_data_transfer.process_mission_data(this.checkedListBox1);
            catl_data_transfer.process_station_data(this.checkedListBox2);
        }

        private void Submit_pb_Click(object sender, EventArgs e)
        {
            //submit button
            //catl_data_transfer.process_mission_checklist(this.checkedListBox1);
            catl_data_transfer.set_groundtrack(this.groundtrack_CB.Checked);
            catl_data_transfer.process_checklist(this.checkedListBox1, catl_data_transfer.Rtn_orbitdata());
            catl_data_transfer.process_checklist(this.checkedListBox2, catl_data_transfer.Rtn_stationdata());
            catl_data_transfer.print_selected();
            //station data
            catl_data_transfer.set_mask_file(this.textBox1.Text);
            catl_data_transfer.Station_generation();
            //orbit data
            catl_data_transfer.set_orbit_file(this.textBox2.Text);
            catl_data_transfer.Orbit_generation();
            catl_data_transfer.Set_intervalFiles();

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            FolderBrowserDialog theDialog = new FolderBrowserDialog();
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                //MessageBox.Show(theDialog.FileName.ToString());
                if (catl_data_transfer == null) catl_data_transfer = new NPAS_To_STK();

                this.tmp_dir_tb.Text = (theDialog.SelectedPath.ToString() +"\\");
                catl_data_transfer.set_temp_directory(this.tmp_dir_tb.Text);
            }
        }

        private void EfileBTN_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog theDialog = new FolderBrowserDialog();
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                //MessageBox.Show(theDialog.FileName.ToString());
                if (catl_data_transfer == null) catl_data_transfer = new NPAS_To_STK();

                this.EFileTB.Text = (theDialog.SelectedPath.ToString() + "\\");
                catl_data_transfer.set_efile_directory(this.EFileTB.Text);
            }
        }
    }
}
