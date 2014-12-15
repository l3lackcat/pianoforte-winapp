﻿namespace PianoForte.View
{
    partial class BookDetailForm
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
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.Button_Submit = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.Button_Cancel = new System.Windows.Forms.Button();
            this.TextBox_BookId = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TextBox_BookAmount = new System.Windows.Forms.TextBox();
            this.TextBox_BookPrice = new System.Windows.Forms.TextBox();
            this.TextBox_Barcode = new System.Windows.Forms.TextBox();
            this.TextBox_BookName = new System.Windows.Forms.TextBox();
            this.Button_Edit = new System.Windows.Forms.Button();
            this.CheckBox_Cancel_Book = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label15.Location = new System.Drawing.Point(170, 131);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(28, 16);
            this.label15.TabIndex = 59;
            this.label15.Text = "เล่ม";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label14.Location = new System.Drawing.Point(170, 102);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(31, 16);
            this.label14.TabIndex = 58;
            this.label14.Text = "บาท";
            // 
            // Button_Submit
            // 
            this.Button_Submit.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Button_Submit.Location = new System.Drawing.Point(104, 157);
            this.Button_Submit.Name = "Button_Submit";
            this.Button_Submit.Size = new System.Drawing.Size(60, 23);
            this.Button_Submit.TabIndex = 44;
            this.Button_Submit.Text = "ยืนยัน";
            this.Button_Submit.UseVisualStyleBackColor = true;
            this.Button_Submit.Click += new System.EventHandler(this.Button_Submit_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label11.Location = new System.Drawing.Point(30, 15);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 16);
            this.label11.TabIndex = 56;
            this.label11.Text = "รหัสหนังสือ";
            // 
            // Button_Cancel
            // 
            this.Button_Cancel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Button_Cancel.Location = new System.Drawing.Point(170, 157);
            this.Button_Cancel.Name = "Button_Cancel";
            this.Button_Cancel.Size = new System.Drawing.Size(60, 23);
            this.Button_Cancel.TabIndex = 55;
            this.Button_Cancel.Text = "ยกเลิก";
            this.Button_Cancel.UseVisualStyleBackColor = true;
            this.Button_Cancel.Click += new System.EventHandler(this.Button_Cancel_Click);
            // 
            // TextBox_BookId
            // 
            this.TextBox_BookId.Enabled = false;
            this.TextBox_BookId.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.TextBox_BookId.Location = new System.Drawing.Point(104, 12);
            this.TextBox_BookId.Name = "TextBox_BookId";
            this.TextBox_BookId.Size = new System.Drawing.Size(100, 23);
            this.TextBox_BookId.TabIndex = 39;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(194, 131);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(16, 16);
            this.label10.TabIndex = 53;
            this.label10.Text = "*";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(197, 102);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(16, 16);
            this.label9.TabIndex = 52;
            this.label9.Text = "*";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(206, 44);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(16, 16);
            this.label8.TabIndex = 51;
            this.label8.Text = "*";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(406, 73);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(16, 16);
            this.label13.TabIndex = 50;
            this.label13.Text = "*";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label7.Location = new System.Drawing.Point(55, 131);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 16);
            this.label7.TabIndex = 49;
            this.label7.Text = "จำนวน";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label6.Location = new System.Drawing.Point(64, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 16);
            this.label6.TabIndex = 48;
            this.label6.Text = "ราคา";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label5.Location = new System.Drawing.Point(36, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 16);
            this.label5.TabIndex = 47;
            this.label5.Text = "ชื่อหนังสือ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.Location = new System.Drawing.Point(48, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 16);
            this.label3.TabIndex = 43;
            this.label3.Text = "บาร์โค้ด";
            // 
            // TextBox_BookAmount
            // 
            this.TextBox_BookAmount.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.TextBox_BookAmount.Location = new System.Drawing.Point(104, 128);
            this.TextBox_BookAmount.Name = "TextBox_BookAmount";
            this.TextBox_BookAmount.Size = new System.Drawing.Size(60, 23);
            this.TextBox_BookAmount.TabIndex = 46;
            this.TextBox_BookAmount.TextChanged += new System.EventHandler(this.TextBox_BookAmount_TextChanged);
            this.TextBox_BookAmount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_BookAmount_KeyDown);
            // 
            // TextBox_BookPrice
            // 
            this.TextBox_BookPrice.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.TextBox_BookPrice.Location = new System.Drawing.Point(104, 99);
            this.TextBox_BookPrice.Name = "TextBox_BookPrice";
            this.TextBox_BookPrice.Size = new System.Drawing.Size(60, 23);
            this.TextBox_BookPrice.TabIndex = 45;
            this.TextBox_BookPrice.TextChanged += new System.EventHandler(this.TextBox_BookPrice_TextChanged);
            this.TextBox_BookPrice.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_BookPrice_KeyDown);
            // 
            // TextBox_Barcode
            // 
            this.TextBox_Barcode.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.TextBox_Barcode.Location = new System.Drawing.Point(104, 41);
            this.TextBox_Barcode.MaxLength = 15;
            this.TextBox_Barcode.Name = "TextBox_Barcode";
            this.TextBox_Barcode.Size = new System.Drawing.Size(100, 23);
            this.TextBox_Barcode.TabIndex = 40;
            this.TextBox_Barcode.TextChanged += new System.EventHandler(this.TextBox_Barcode_TextChanged);
            this.TextBox_Barcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_Barcode_KeyDown);
            // 
            // TextBox_BookName
            // 
            this.TextBox_BookName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.TextBox_BookName.Location = new System.Drawing.Point(104, 70);
            this.TextBox_BookName.Name = "TextBox_BookName";
            this.TextBox_BookName.Size = new System.Drawing.Size(300, 23);
            this.TextBox_BookName.TabIndex = 60;
            this.TextBox_BookName.TextChanged += new System.EventHandler(this.TextBox_BookName_TextChanged);
            // 
            // Button_Edit
            // 
            this.Button_Edit.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Button_Edit.Location = new System.Drawing.Point(104, 157);
            this.Button_Edit.Name = "Button_Edit";
            this.Button_Edit.Size = new System.Drawing.Size(60, 23);
            this.Button_Edit.TabIndex = 61;
            this.Button_Edit.Text = "แก้ไข";
            this.Button_Edit.UseVisualStyleBackColor = true;
            this.Button_Edit.Click += new System.EventHandler(this.Button_Edit_Click);
            // 
            // CheckBox_Cancel_Book
            // 
            this.CheckBox_Cancel_Book.AutoSize = true;
            this.CheckBox_Cancel_Book.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.CheckBox_Cancel_Book.Location = new System.Drawing.Point(210, 14);
            this.CheckBox_Cancel_Book.Name = "CheckBox_Cancel_Book";
            this.CheckBox_Cancel_Book.Size = new System.Drawing.Size(105, 20);
            this.CheckBox_Cancel_Book.TabIndex = 62;
            this.CheckBox_Cancel_Book.Text = "ยกเลิกการขาย";
            this.CheckBox_Cancel_Book.UseVisualStyleBackColor = true;
            // 
            // BookDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 192);
            this.Controls.Add(this.CheckBox_Cancel_Book);
            this.Controls.Add(this.Button_Edit);
            this.Controls.Add(this.TextBox_BookName);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.Button_Submit);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.Button_Cancel);
            this.Controls.Add(this.TextBox_BookId);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TextBox_BookAmount);
            this.Controls.Add(this.TextBox_BookPrice);
            this.Controls.Add(this.TextBox_Barcode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BookDetailForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button Button_Submit;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button Button_Cancel;
        private System.Windows.Forms.TextBox TextBox_BookId;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextBox_BookAmount;
        private System.Windows.Forms.TextBox TextBox_BookPrice;
        private System.Windows.Forms.TextBox TextBox_Barcode;
        private System.Windows.Forms.TextBox TextBox_BookName;
        private System.Windows.Forms.Button Button_Edit;
        private System.Windows.Forms.CheckBox CheckBox_Cancel_Book;
    }
}