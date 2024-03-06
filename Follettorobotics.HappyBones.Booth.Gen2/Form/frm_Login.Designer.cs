namespace Follettorobotics.HappyBones.Booth.Gen2
{
    partial class frm_Login
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Login_PW = new System.Windows.Forms.Label();
            this.btn_Login_OK = new System.Windows.Forms.Button();
            this.btn_Login_Cancel = new System.Windows.Forms.Button();
            this.txt_Login_PW = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(600, 1);
            this.label1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Location = new System.Drawing.Point(0, 298);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(600, 2);
            this.label2.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(0, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(1, 297);
            this.label3.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Location = new System.Drawing.Point(598, 1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(2, 297);
            this.label4.TabIndex = 3;
            // 
            // Login_PW
            // 
            this.Login_PW.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Login_PW.Location = new System.Drawing.Point(60, 160);
            this.Login_PW.Name = "Login_PW";
            this.Login_PW.Size = new System.Drawing.Size(150, 36);
            this.Login_PW.TabIndex = 4;
            this.Login_PW.Text = "PW";
            this.Login_PW.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Login_PW.DoubleClick += new System.EventHandler(this.Login_PW_DoubleClick);
            // 
            // btn_Login_OK
            // 
            this.btn_Login_OK.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Login_OK.Location = new System.Drawing.Point(215, 202);
            this.btn_Login_OK.Name = "btn_Login_OK";
            this.btn_Login_OK.Size = new System.Drawing.Size(150, 50);
            this.btn_Login_OK.TabIndex = 8;
            this.btn_Login_OK.Text = "Login";
            this.btn_Login_OK.UseVisualStyleBackColor = true;
            this.btn_Login_OK.Click += new System.EventHandler(this.btn_Login_OK_Click);
            // 
            // btn_Login_Cancel
            // 
            this.btn_Login_Cancel.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Login_Cancel.Location = new System.Drawing.Point(371, 202);
            this.btn_Login_Cancel.Name = "btn_Login_Cancel";
            this.btn_Login_Cancel.Size = new System.Drawing.Size(150, 50);
            this.btn_Login_Cancel.TabIndex = 9;
            this.btn_Login_Cancel.Text = "Cancel";
            this.btn_Login_Cancel.UseVisualStyleBackColor = true;
            this.btn_Login_Cancel.Click += new System.EventHandler(this.btn_Login_Cancel_Click);
            // 
            // txt_Login_PW
            // 
            this.txt_Login_PW.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_Login_PW.Location = new System.Drawing.Point(215, 160);
            this.txt_Login_PW.Name = "txt_Login_PW";
            this.txt_Login_PW.PasswordChar = '*';
            this.txt_Login_PW.Size = new System.Drawing.Size(306, 36);
            this.txt_Login_PW.TabIndex = 10;
            // 
            // frm_Login
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(600, 300);
            this.Controls.Add(this.txt_Login_PW);
            this.Controls.Add(this.btn_Login_Cancel);
            this.Controls.Add(this.btn_Login_OK);
            this.Controls.Add(this.Login_PW);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_Login_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_Login_FormClosed);
            this.Load += new System.EventHandler(this.frm_Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label Login_PW;
        private System.Windows.Forms.Button btn_Login_OK;
        private System.Windows.Forms.Button btn_Login_Cancel;
        private System.Windows.Forms.TextBox txt_Login_PW;
    }
}