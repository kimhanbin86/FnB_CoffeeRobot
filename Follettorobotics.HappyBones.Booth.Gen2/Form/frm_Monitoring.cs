using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Reflection;

using Library.Log;

namespace Follettorobotics.HappyBones.Booth.Gen2
{
    public partial class frm_Monitoring : Form
    {
        #region Timer

        private Timer _TimerMonitoring = null;
        private void Tick_Monitoring(object sender, EventArgs e)
        {
            _TimerMonitoring?.Stop();
            try
            {
                #region Door

                UpdateDoor();

                #endregion

                #region Robot

                lbl_Robot_D1B0.BackColor = GlobalDevice.Robot.Feedback[0 * 8 + 0] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D1B1.BackColor = GlobalDevice.Robot.Feedback[0 * 8 + 1] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D1B2.BackColor = GlobalDevice.Robot.Feedback[0 * 8 + 2] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D1B3.BackColor = GlobalDevice.Robot.Feedback[0 * 8 + 3] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D1B4.BackColor = GlobalDevice.Robot.Feedback[0 * 8 + 4] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D1B5.BackColor = GlobalDevice.Robot.Feedback[0 * 8 + 5] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D1B6.BackColor = GlobalDevice.Robot.Feedback[0 * 8 + 6] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D1B7.BackColor = GlobalDevice.Robot.Feedback[0 * 8 + 7] ? Color.Lime : SystemColors.Control;

                lbl_Robot_D2B0.BackColor = GlobalDevice.Robot.Feedback[1 * 8 + 0] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D2B1.BackColor = GlobalDevice.Robot.Feedback[1 * 8 + 1] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D2B2.BackColor = GlobalDevice.Robot.Feedback[1 * 8 + 2] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D2B3.BackColor = GlobalDevice.Robot.Feedback[1 * 8 + 3] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D2B4.BackColor = GlobalDevice.Robot.Feedback[1 * 8 + 4] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D2B5.BackColor = GlobalDevice.Robot.Feedback[1 * 8 + 5] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D2B6.BackColor = GlobalDevice.Robot.Feedback[1 * 8 + 6] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D2B7.BackColor = GlobalDevice.Robot.Feedback[1 * 8 + 7] ? Color.Lime : SystemColors.Control;

                lbl_Robot_D3B0.BackColor = GlobalDevice.Robot.Feedback[2 * 8 + 0] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D3B1.BackColor = GlobalDevice.Robot.Feedback[2 * 8 + 1] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D3B2.BackColor = GlobalDevice.Robot.Feedback[2 * 8 + 2] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D3B3.BackColor = GlobalDevice.Robot.Feedback[2 * 8 + 3] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D3B4.BackColor = GlobalDevice.Robot.Feedback[2 * 8 + 4] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D3B5.BackColor = GlobalDevice.Robot.Feedback[2 * 8 + 5] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D3B6.BackColor = GlobalDevice.Robot.Feedback[2 * 8 + 6] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D3B7.BackColor = GlobalDevice.Robot.Feedback[2 * 8 + 7] ? Color.Lime : SystemColors.Control;

                lbl_Robot_D4B0.BackColor = GlobalDevice.Robot.Feedback[3 * 8 + 0] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D4B1.BackColor = GlobalDevice.Robot.Feedback[3 * 8 + 1] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D4B2.BackColor = GlobalDevice.Robot.Feedback[3 * 8 + 2] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D4B3.BackColor = GlobalDevice.Robot.Feedback[3 * 8 + 3] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D4B4.BackColor = GlobalDevice.Robot.Feedback[3 * 8 + 4] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D4B5.BackColor = GlobalDevice.Robot.Feedback[3 * 8 + 5] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D4B6.BackColor = GlobalDevice.Robot.Feedback[3 * 8 + 6] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D4B7.BackColor = GlobalDevice.Robot.Feedback[3 * 8 + 7] ? Color.Lime : SystemColors.Control;

                lbl_Robot_D5B0.BackColor = GlobalDevice.Robot.Feedback[4 * 8 + 0] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D5B1.BackColor = GlobalDevice.Robot.Feedback[4 * 8 + 1] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D5B2.BackColor = GlobalDevice.Robot.Feedback[4 * 8 + 2] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D5B3.BackColor = GlobalDevice.Robot.Feedback[4 * 8 + 3] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D5B4.BackColor = GlobalDevice.Robot.Feedback[4 * 8 + 4] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D5B5.BackColor = GlobalDevice.Robot.Feedback[4 * 8 + 5] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D5B6.BackColor = GlobalDevice.Robot.Feedback[4 * 8 + 6] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D5B7.BackColor = GlobalDevice.Robot.Feedback[4 * 8 + 7] ? Color.Lime : SystemColors.Control;

                lbl_Robot_D6B0.BackColor = GlobalDevice.Robot.Feedback[5 * 8 + 0] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D6B1.BackColor = GlobalDevice.Robot.Feedback[5 * 8 + 1] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D6B2.BackColor = GlobalDevice.Robot.Feedback[5 * 8 + 2] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D6B3.BackColor = GlobalDevice.Robot.Feedback[5 * 8 + 3] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D6B4.BackColor = GlobalDevice.Robot.Feedback[5 * 8 + 4] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D6B5.BackColor = GlobalDevice.Robot.Feedback[5 * 8 + 5] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D6B6.BackColor = GlobalDevice.Robot.Feedback[5 * 8 + 6] ? Color.Lime : SystemColors.Control;
                lbl_Robot_D6B7.BackColor = GlobalDevice.Robot.Feedback[5 * 8 + 7] ? Color.Lime : SystemColors.Control;

                #endregion
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            finally
            {
                _TimerMonitoring?.Start();
            }
        }

        private void UpdateDoor()
        {
            try
            {
                DataGridView dgv = dgv_Door;

                dgv.Rows.Clear();

                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                UpdateDoor(dgv, GlobalFunction.DB.MySQL.GetDataTable($"SELECT * FROM {e_DB._Door} ORDER BY {e_DB_Door.Column00} ASC"));

                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }
        private void UpdateDoor(DataGridView dgv, DataTable data)
        {
            try
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    dgv.Rows.Add();

                    for (int j = 0; j < Enum.GetNames(typeof(e_DB_Door)).Length; j++)
                    {
                        dgv.Rows[dgv.Rows.Count - 1].Cells[j].Value = data.Rows[i][j].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        #endregion

        public frm_Monitoring()
        {
            InitializeComponent();
        }

        private void frm_Monitoring_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalVariable.Form.Monitoring = null;
        }

        private void frm_Monitoring_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_TimerMonitoring != null)
            {
                if (_TimerMonitoring.Enabled)
                {
                    _TimerMonitoring.Stop();
                }

                _TimerMonitoring.Dispose();
                _TimerMonitoring = null;
            }
        }

        private void frm_Monitoring_Load(object sender, EventArgs e)
        {
            InitializeForm();

            _TimerMonitoring = new Timer();
            _TimerMonitoring.Tick += new EventHandler(Tick_Monitoring);
            _TimerMonitoring.Interval = 100;
            _TimerMonitoring.Start();
        }

        public void InitializeForm()
        {
            SetDataGridView();

            SetControlsText_Robot();
        }

        private void SetDataGridView()
        {
            GlobalFunction.DataGridView.SetProperties(dgv_Door);

            GlobalFunction.DataGridView.AddColumns(dgv_Door, Enum.GetNames(typeof(e_DB_Door)));
        }

        private void DataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                DataGridView dgv = sender as DataGridView;

                Rectangle rectangle = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, dgv.RowHeadersWidth, e.RowBounds.Height);

                StringFormat stringFormat = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                e.Graphics.DrawString((e.RowIndex + 1).ToString(), new Font(e_Font.Tahoma.ToString(), 11f), SystemBrushes.ControlText, rectangle, stringFormat);
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        public void SetControlsText_Robot()
        {
            string[] strings = Enum.GetNames(typeof(e_Device_Robot_Feedback));

            foreach (Label label in GlobalFunction.GetControls(grp_Robot))
            {
                string labelName = label.Name.Substring(label.Name.LastIndexOf("_") + 1);

                foreach (string str in strings)
                {
                    if (str.Contains(labelName))
                    {
                        label.Text = str.Replace("_", " ");

                        break;
                    }
                }
            }
        }
    }
}
