﻿
namespace QLTVT
{
    partial class FormDangNhap
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
            this.cmbCHINHANH = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTAIKHOAN = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDANGNHAP = new System.Windows.Forms.Button();
            this.btnTHOAT = new System.Windows.Forms.Button();
            this.txtMATKHAU = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbCHINHANH
            // 
            this.cmbCHINHANH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCHINHANH.FormattingEnabled = true;
            this.cmbCHINHANH.Location = new System.Drawing.Point(192, 81);
            this.cmbCHINHANH.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbCHINHANH.Name = "cmbCHINHANH";
            this.cmbCHINHANH.Size = new System.Drawing.Size(328, 27);
            this.cmbCHINHANH.TabIndex = 0;
            this.cmbCHINHANH.SelectedIndexChanged += new System.EventHandler(this.cmbCHINHANH_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 7.8F);
            this.label1.Location = new System.Drawing.Point(50, 81);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "CHI NHÁNH";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 132);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "TÀI KHOẢN";
            // 
            // txtTAIKHOAN
            // 
            this.txtTAIKHOAN.Location = new System.Drawing.Point(192, 132);
            this.txtTAIKHOAN.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTAIKHOAN.Name = "txtTAIKHOAN";
            this.txtTAIKHOAN.Size = new System.Drawing.Size(328, 27);
            this.txtTAIKHOAN.TabIndex = 3;
            this.txtTAIKHOAN.TextChanged += new System.EventHandler(this.txtTAIKHOAN_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(49, 189);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 19);
            this.label3.TabIndex = 5;
            this.label3.Text = "MẬT KHẨU";
            // 
            // btnDANGNHAP
            // 
            this.btnDANGNHAP.BackColor = System.Drawing.Color.Blue;
            this.btnDANGNHAP.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnDANGNHAP.ForeColor = System.Drawing.Color.Transparent;
            this.btnDANGNHAP.Location = new System.Drawing.Point(15, 292);
            this.btnDANGNHAP.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDANGNHAP.Name = "btnDANGNHAP";
            this.btnDANGNHAP.Size = new System.Drawing.Size(251, 48);
            this.btnDANGNHAP.TabIndex = 6;
            this.btnDANGNHAP.Text = "ĐĂNG NHẬP";
            this.btnDANGNHAP.UseVisualStyleBackColor = false;
            this.btnDANGNHAP.Click += new System.EventHandler(this.btnDANGNHAP_Click);
            // 
            // btnTHOAT
            // 
            this.btnTHOAT.BackColor = System.Drawing.Color.Red;
            this.btnTHOAT.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold);
            this.btnTHOAT.ForeColor = System.Drawing.Color.White;
            this.btnTHOAT.Location = new System.Drawing.Point(294, 292);
            this.btnTHOAT.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTHOAT.Name = "btnTHOAT";
            this.btnTHOAT.Size = new System.Drawing.Size(251, 48);
            this.btnTHOAT.TabIndex = 7;
            this.btnTHOAT.Text = "THOÁT";
            this.btnTHOAT.UseVisualStyleBackColor = false;
            this.btnTHOAT.Click += new System.EventHandler(this.btnTHOAT_Click);
            // 
            // txtMATKHAU
            // 
            this.txtMATKHAU.Location = new System.Drawing.Point(192, 189);
            this.txtMATKHAU.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtMATKHAU.Name = "txtMATKHAU";
            this.txtMATKHAU.PasswordChar = '*';
            this.txtMATKHAU.Size = new System.Drawing.Size(328, 27);
            this.txtMATKHAU.TabIndex = 4;
            this.txtMATKHAU.TextChanged += new System.EventHandler(this.txtMATKHAU_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(134, 11);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(295, 38);
            this.label4.TabIndex = 8;
            this.label4.Text = "QUẢN LÝ VẬT TƯ";
            // 
            // FormDangNhap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 416);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnTHOAT);
            this.Controls.Add(this.btnDANGNHAP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMATKHAU);
            this.Controls.Add(this.txtTAIKHOAN);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbCHINHANH);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormDangNhap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng Nhập";
            this.Load += new System.EventHandler(this.FormDangNhap_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbCHINHANH;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTAIKHOAN;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnDANGNHAP;
        private System.Windows.Forms.Button btnTHOAT;
        private System.Windows.Forms.TextBox txtMATKHAU;
        private System.Windows.Forms.Label label4;
    }
}