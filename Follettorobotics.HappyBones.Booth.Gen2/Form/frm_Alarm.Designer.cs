namespace Follettorobotics.HappyBones.Booth.Gen2
{
    partial class frm_Alarm
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
            this.dgv_Alarm = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Alarm)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_Alarm
            // 
            this.dgv_Alarm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Alarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_Alarm.Location = new System.Drawing.Point(0, 0);
            this.dgv_Alarm.Name = "dgv_Alarm";
            this.dgv_Alarm.RowTemplate.Height = 23;
            this.dgv_Alarm.Size = new System.Drawing.Size(784, 561);
            this.dgv_Alarm.TabIndex = 0;
            this.dgv_Alarm.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.DataGridView_RowPostPaint);
            // 
            // frm_Alarm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.dgv_Alarm);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "frm_Alarm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alarm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_Alarm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_Alarm_FormClosed);
            this.Load += new System.EventHandler(this.frm_Alarm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Alarm)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_Alarm;
    }
}