namespace Follettorobotics.HappyBones.Booth.Gen2
{
    partial class frm_Parameter
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_Parameter_Close = new System.Windows.Forms.Button();
            this.btn_Parameter_Save = new System.Windows.Forms.Button();
            this.btn_Parameter_Test = new System.Windows.Forms.Button();
            this.cb_Parameter_Order_Cup = new System.Windows.Forms.ComboBox();
            this.txt_Parameter_Description = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txt_Parameter_Description, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1264, 761);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1258, 526);
            this.tabControl1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.btn_Parameter_Close, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.btn_Parameter_Save, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btn_Parameter_Test, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.cb_Parameter_Order_Cup, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 687);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1258, 71);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // btn_Parameter_Close
            // 
            this.btn_Parameter_Close.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Parameter_Close.Location = new System.Drawing.Point(1007, 3);
            this.btn_Parameter_Close.Name = "btn_Parameter_Close";
            this.btn_Parameter_Close.Size = new System.Drawing.Size(248, 65);
            this.btn_Parameter_Close.TabIndex = 0;
            this.btn_Parameter_Close.Text = "Close";
            this.btn_Parameter_Close.UseVisualStyleBackColor = true;
            this.btn_Parameter_Close.Click += new System.EventHandler(this.Parameter_Button_Click);
            // 
            // btn_Parameter_Save
            // 
            this.btn_Parameter_Save.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Parameter_Save.Location = new System.Drawing.Point(3, 3);
            this.btn_Parameter_Save.Name = "btn_Parameter_Save";
            this.btn_Parameter_Save.Size = new System.Drawing.Size(245, 65);
            this.btn_Parameter_Save.TabIndex = 1;
            this.btn_Parameter_Save.Text = "Save";
            this.btn_Parameter_Save.UseVisualStyleBackColor = true;
            this.btn_Parameter_Save.Click += new System.EventHandler(this.Parameter_Button_Click);
            // 
            // btn_Parameter_Test
            // 
            this.btn_Parameter_Test.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Parameter_Test.Location = new System.Drawing.Point(254, 3);
            this.btn_Parameter_Test.Name = "btn_Parameter_Test";
            this.btn_Parameter_Test.Size = new System.Drawing.Size(245, 65);
            this.btn_Parameter_Test.TabIndex = 2;
            this.btn_Parameter_Test.Text = "Test";
            this.btn_Parameter_Test.UseVisualStyleBackColor = true;
            this.btn_Parameter_Test.Click += new System.EventHandler(this.Parameter_Button_Click);
            // 
            // cb_Parameter_Order_Cup
            // 
            this.cb_Parameter_Order_Cup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_Parameter_Order_Cup.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cb_Parameter_Order_Cup.FormattingEnabled = true;
            this.cb_Parameter_Order_Cup.Location = new System.Drawing.Point(505, 3);
            this.cb_Parameter_Order_Cup.Name = "cb_Parameter_Order_Cup";
            this.cb_Parameter_Order_Cup.Size = new System.Drawing.Size(245, 37);
            this.cb_Parameter_Order_Cup.TabIndex = 3;
            this.cb_Parameter_Order_Cup.TabStop = false;
            // 
            // txt_Parameter_Description
            // 
            this.txt_Parameter_Description.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Parameter_Description.Location = new System.Drawing.Point(3, 535);
            this.txt_Parameter_Description.Name = "txt_Parameter_Description";
            this.txt_Parameter_Description.ReadOnly = true;
            this.txt_Parameter_Description.Size = new System.Drawing.Size(1258, 146);
            this.txt_Parameter_Description.TabIndex = 2;
            this.txt_Parameter_Description.Text = "";
            // 
            // frm_Parameter
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1264, 761);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "frm_Parameter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parameter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_Parameter_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_Parameter_FormClosed);
            this.Load += new System.EventHandler(this.frm_Parameter_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btn_Parameter_Close;
        private System.Windows.Forms.Button btn_Parameter_Save;
        private System.Windows.Forms.Button btn_Parameter_Test;
        private System.Windows.Forms.ComboBox cb_Parameter_Order_Cup;
        private System.Windows.Forms.RichTextBox txt_Parameter_Description;
    }
}