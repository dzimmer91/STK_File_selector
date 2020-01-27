//CATT_Display.Designer.cs
﻿namespace STK_File_selector
{
    partial class NPAS_to_STK
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.getMaskFile_PB = new System.Windows.Forms.Button();
            this.Exit_Btn = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.getOrbFile_PB = new System.Windows.Forms.Button();
            this.TLFile_PB = new System.Windows.Forms.Button();
            this.TLFile_TB = new System.Windows.Forms.TextBox();
            this.Run_PB = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox2 = new System.Windows.Forms.CheckedListBox();
            this.missionLB = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Submit_pb = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tmp_dir_tb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.StartdateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.EnddateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.EfileBTN = new System.Windows.Forms.Button();
            this.EFileTB = new System.Windows.Forms.TextBox();
            this.groundtrack_CB = new System.Windows.Forms.CheckBox();
            this.Options_LB = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // getMaskFile_PB
            //
            this.getMaskFile_PB.Location = new System.Drawing.Point(10, 87);
            this.getMaskFile_PB.Name = "getMaskFile_PB";
            this.getMaskFile_PB.Size = new System.Drawing.Size(99, 23);
            this.getMaskFile_PB.TabIndex = 1;
            this.getMaskFile_PB.Text = "Select Mask File";
            this.getMaskFile_PB.UseVisualStyleBackColor = true;
            this.getMaskFile_PB.Click += new System.EventHandler(this.Get_File_Btn_Click);
            //
            // Exit_Btn
            //
            this.Exit_Btn.Location = new System.Drawing.Point(466, 478);
            this.Exit_Btn.Name = "Exit_Btn";
            this.Exit_Btn.Size = new System.Drawing.Size(75, 23);
            this.Exit_Btn.TabIndex = 2;
            this.Exit_Btn.Text = "Exit";
            this.Exit_Btn.UseVisualStyleBackColor = true;
            this.Exit_Btn.Click += new System.EventHandler(this.Exit_Btn_Click);
            //
            // textBox1
            //
            this.textBox1.Location = new System.Drawing.Point(115, 86);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(436, 20);
            this.textBox1.TabIndex = 3;
            //
            // textBox2
            //
            this.textBox2.Location = new System.Drawing.Point(115, 112);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(436, 20);
            this.textBox2.TabIndex = 5;
            //
            // getOrbFile_PB
            //
            this.getOrbFile_PB.Location = new System.Drawing.Point(10, 113);
            this.getOrbFile_PB.Name = "getOrbFile_PB";
            this.getOrbFile_PB.Size = new System.Drawing.Size(99, 23);
            this.getOrbFile_PB.TabIndex = 4;
            this.getOrbFile_PB.Text = "Select Orbit File";
            this.getOrbFile_PB.UseVisualStyleBackColor = true;
            this.getOrbFile_PB.Click += new System.EventHandler(this.button2_Click);
            //
            // TLFile_PB
            //
            this.TLFile_PB.Location = new System.Drawing.Point(10, 58);
            this.TLFile_PB.Name = "TLFile_PB";
            this.TLFile_PB.Size = new System.Drawing.Size(100, 23);
            this.TLFile_PB.TabIndex = 7;
            this.TLFile_PB.Text = "Select TL File";
            this.TLFile_PB.UseVisualStyleBackColor = true;
            this.TLFile_PB.Click += new System.EventHandler(this.TLFile_PB_Click);
            //
            // TLFile_TB
            //
            this.TLFile_TB.Location = new System.Drawing.Point(116, 59);
            this.TLFile_TB.Name = "TLFile_TB";
            this.TLFile_TB.ReadOnly = true;
            this.TLFile_TB.Size = new System.Drawing.Size(435, 20);
            this.TLFile_TB.TabIndex = 8;
            //
            // Run_PB
            //
            this.Run_PB.Location = new System.Drawing.Point(11, 195);
            this.Run_PB.Name = "Run_PB";
            this.Run_PB.Size = new System.Drawing.Size(75, 23);
            this.Run_PB.TabIndex = 9;
            this.Run_PB.Text = "Build Lists";
            this.Run_PB.UseVisualStyleBackColor = true;
            this.Run_PB.Click += new System.EventHandler(this.Run_PB_Click);
            //
            // checkedListBox1
            //
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(11, 248);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(169, 169);
            this.checkedListBox1.TabIndex = 10;
            //
            // checkedListBox2
            //
            this.checkedListBox2.FormattingEnabled = true;
            this.checkedListBox2.Location = new System.Drawing.Point(227, 248);
            this.checkedListBox2.Name = "checkedListBox2";
            this.checkedListBox2.Size = new System.Drawing.Size(155, 169);
            this.checkedListBox2.TabIndex = 11;
            //
            // missionLB
            //
            this.missionLB.AutoSize = true;
            this.missionLB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.missionLB.Location = new System.Drawing.Point(49, 225);
            this.missionLB.Name = "missionLB";
            this.missionLB.Size = new System.Drawing.Size(62, 17);
            this.missionLB.TabIndex = 12;
            this.missionLB.Text = "Missions";
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(274, 225);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 13;
            this.label1.Text = "Stations";
            //
            // Submit_pb
            //
            this.Submit_pb.Location = new System.Drawing.Point(10, 479);
            this.Submit_pb.Name = "Submit_pb";
            this.Submit_pb.Size = new System.Drawing.Size(75, 23);
            this.Submit_pb.TabIndex = 14;
            this.Submit_pb.Text = "Submit";
            this.Submit_pb.UseVisualStyleBackColor = true;
            this.Submit_pb.Click += new System.EventHandler(this.Submit_pb_Click);
            //
            // button1
            //
            this.button1.Location = new System.Drawing.Point(11, 32);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "Select TMP Dir";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            //
            // tmp_dir_tb
            //
            this.tmp_dir_tb.AcceptsReturn = true;
            this.tmp_dir_tb.Location = new System.Drawing.Point(116, 34);
            this.tmp_dir_tb.Name = "tmp_dir_tb";
            this.tmp_dir_tb.ReadOnly = true;
            this.tmp_dir_tb.Size = new System.Drawing.Size(435, 20);
            this.tmp_dir_tb.TabIndex = 16;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 171);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Start Date";
            //
            // StartdateTimePicker
            //
            this.StartdateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.StartdateTimePicker.Location = new System.Drawing.Point(72, 169);
            this.StartdateTimePicker.Name = "StartdateTimePicker";
            this.StartdateTimePicker.Size = new System.Drawing.Size(96, 20);
            this.StartdateTimePicker.TabIndex = 18;
            this.StartdateTimePicker.Value = new System.DateTime(2017, 12, 12, 0, 0, 0, 0);
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(187, 171);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "End Date";
            //
            // EnddateTimePicker
            //
            this.EnddateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.EnddateTimePicker.Location = new System.Drawing.Point(245, 169);
            this.EnddateTimePicker.Name = "EnddateTimePicker";
            this.EnddateTimePicker.Size = new System.Drawing.Size(96, 20);
            this.EnddateTimePicker.TabIndex = 20;
            this.EnddateTimePicker.Value = new System.DateTime(2017, 12, 12, 0, 0, 0, 0);
            //
            // EfileBTN
            //
            this.EfileBTN.Location = new System.Drawing.Point(10, 140);
            this.EfileBTN.Name = "EfileBTN";
            this.EfileBTN.Size = new System.Drawing.Size(99, 23);
            this.EfileBTN.TabIndex = 21;
            this.EfileBTN.Text = "Select E File Dir";
            this.EfileBTN.UseVisualStyleBackColor = true;
            this.EfileBTN.Click += new System.EventHandler(this.EfileBTN_Click);
            //
            // EFileTB
            //
            this.EFileTB.Location = new System.Drawing.Point(116, 138);
            this.EFileTB.Name = "EFileTB";
            this.EFileTB.ReadOnly = true;
            this.EFileTB.Size = new System.Drawing.Size(436, 20);
            this.EFileTB.TabIndex = 22;
            //
            // groundtrack_CB
            //
            this.groundtrack_CB.AutoSize = true;
            this.groundtrack_CB.Location = new System.Drawing.Point(429, 248);
            this.groundtrack_CB.Name = "groundtrack_CB";
            this.groundtrack_CB.Size = new System.Drawing.Size(92, 17);
            this.groundtrack_CB.TabIndex = 23;
            this.groundtrack_CB.Text = "Ground Track";
            this.groundtrack_CB.UseVisualStyleBackColor = true;
            //
            // Options_LB
            //
            this.Options_LB.AutoSize = true;
            this.Options_LB.Location = new System.Drawing.Point(429, 225);
            this.Options_LB.Name = "Options_LB";
            this.Options_LB.Size = new System.Drawing.Size(43, 13);
            this.Options_LB.TabIndex = 24;
            this.Options_LB.Text = "Options";
            //
            // NPAS_to_STK
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 513);
            this.Controls.Add(this.Options_LB);
            this.Controls.Add(this.groundtrack_CB);
            this.Controls.Add(this.EFileTB);
            this.Controls.Add(this.EfileBTN);
            this.Controls.Add(this.EnddateTimePicker);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.StartdateTimePicker);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tmp_dir_tb);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Submit_pb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.missionLB);
            this.Controls.Add(this.checkedListBox2);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.Run_PB);
            this.Controls.Add(this.TLFile_TB);
            this.Controls.Add(this.TLFile_PB);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.getOrbFile_PB);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Exit_Btn);
            this.Controls.Add(this.getMaskFile_PB);
            this.Name = "NPAS_to_STK";
            this.Text = "NPAS Schedule to STK";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button getMaskFile_PB;
        private System.Windows.Forms.Button Exit_Btn;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button getOrbFile_PB;
        private System.Windows.Forms.Button TLFile_PB;
        private System.Windows.Forms.TextBox TLFile_TB;
        private System.Windows.Forms.Button Run_PB;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.CheckedListBox checkedListBox2;
        private System.Windows.Forms.Label missionLB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Submit_pb;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tmp_dir_tb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker StartdateTimePicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker EnddateTimePicker;
        private System.Windows.Forms.Button EfileBTN;
        private System.Windows.Forms.TextBox EFileTB;
        private System.Windows.Forms.CheckBox groundtrack_CB;
        private System.Windows.Forms.Label Options_LB;
    }






}
