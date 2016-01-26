namespace EntryInformation
{
    partial class First
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxStreetName = new System.Windows.Forms.TextBox();
            this.radioButtonPm = new System.Windows.Forms.RadioButton();
            this.radioButtonAm = new System.Windows.Forms.RadioButton();
            this.buttonSubmit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.textBoxTimeTo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxTimeFrom = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Street Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Time:";
            // 
            // textBoxStreetName
            // 
            this.textBoxStreetName.Location = new System.Drawing.Point(144, 38);
            this.textBoxStreetName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxStreetName.Name = "textBoxStreetName";
            this.textBoxStreetName.Size = new System.Drawing.Size(184, 22);
            this.textBoxStreetName.TabIndex = 2;
            this.textBoxStreetName.Text = "Test";
            // 
            // radioButtonPm
            // 
            this.radioButtonPm.AutoSize = true;
            this.radioButtonPm.BackColor = System.Drawing.Color.Transparent;
            this.radioButtonPm.Location = new System.Drawing.Point(267, 157);
            this.radioButtonPm.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButtonPm.Name = "radioButtonPm";
            this.radioButtonPm.Size = new System.Drawing.Size(49, 21);
            this.radioButtonPm.TabIndex = 7;
            this.radioButtonPm.Text = "PM";
            this.radioButtonPm.UseVisualStyleBackColor = false;
            // 
            // radioButtonAm
            // 
            this.radioButtonAm.AutoSize = true;
            this.radioButtonAm.BackColor = System.Drawing.Color.Transparent;
            this.radioButtonAm.Checked = true;
            this.radioButtonAm.Location = new System.Drawing.Point(212, 157);
            this.radioButtonAm.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButtonAm.Name = "radioButtonAm";
            this.radioButtonAm.Size = new System.Drawing.Size(49, 21);
            this.radioButtonAm.TabIndex = 8;
            this.radioButtonAm.TabStop = true;
            this.radioButtonAm.Text = "AM";
            this.radioButtonAm.UseVisualStyleBackColor = false;
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.Location = new System.Drawing.Point(224, 217);
            this.buttonSubmit.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(99, 37);
            this.buttonSubmit.TabIndex = 9;
            this.buttonSubmit.Text = "Enter";
            this.buttonSubmit.UseVisualStyleBackColor = true;
            this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.dateTimePicker);
            this.groupBox1.Controls.Add(this.radioButtonAm);
            this.groupBox1.Controls.Add(this.buttonSubmit);
            this.groupBox1.Controls.Add(this.radioButtonPm);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxTimeTo);
            this.groupBox1.Controls.Add(this.textBoxStreetName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxTimeFrom);
            this.groupBox1.Location = new System.Drawing.Point(472, 103);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(343, 262);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Startup";
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Location = new System.Drawing.Point(98, 98);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(245, 22);
            this.dateTimePicker.TabIndex = 11;
            // 
            // textBoxTimeTo
            // 
            this.textBoxTimeTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTimeTo.Location = new System.Drawing.Point(212, 125);
            this.textBoxTimeTo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxTimeTo.Name = "textBoxTimeTo";
            this.textBoxTimeTo.Size = new System.Drawing.Size(68, 28);
            this.textBoxTimeTo.TabIndex = 6;
            this.textBoxTimeTo.Text = "09:00";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(173, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "To";
            // 
            // textBoxTimeFrom
            // 
            this.textBoxTimeFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTimeFrom.Location = new System.Drawing.Point(98, 125);
            this.textBoxTimeFrom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxTimeFrom.Name = "textBoxTimeFrom";
            this.textBoxTimeFrom.Size = new System.Drawing.Size(69, 28);
            this.textBoxTimeFrom.TabIndex = 3;
            this.textBoxTimeFrom.Text = "08:00";
            // 
            // First
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::EntryInformation.Properties.Resources.First;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(825, 517);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "First";
            this.Text = "Information";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxStreetName;
        private System.Windows.Forms.RadioButton radioButtonPm;
        private System.Windows.Forms.RadioButton radioButtonAm;
        private System.Windows.Forms.Button buttonSubmit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.TextBox textBoxTimeTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxTimeFrom;
    }
}

