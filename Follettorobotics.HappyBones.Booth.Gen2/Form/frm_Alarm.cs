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
    public partial class frm_Alarm : Form
    {
        #region Timer

        private Timer _TimerAlarm = null;
        private void Tick_Alarm(object sender, EventArgs e)
        {
            _TimerAlarm?.Stop();
            try
            {
                UpdateAlarm();
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            finally
            {
                _TimerAlarm?.Start();
            }
        }

        private void UpdateAlarm()
        {
            try
            {
                DataGridView dgv = dgv_Alarm;

                dgv.Rows.Clear();

                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                UpdateAlarm(dgv, GlobalFunction.DB.MySQL.GetDataTable($"SELECT * FROM {e_DB._Alarm} WHERE {e_DB_Alarm.Column00}>='{GlobalFunction.GetDateTimeString(GlobalVariable.ProgramStarted)}' AND {e_DB_Alarm.Column01} IS NULL ORDER BY {e_DB_Alarm.Column00} DESC"));

                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }
        private void UpdateAlarm(DataGridView dgv, DataTable data)
        {
            try
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    dgv.Rows.Add();

                    for (int j = 0; j < Enum.GetNames(typeof(e_DB_Alarm)).Length; j++)
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

        public frm_Alarm()
        {
            InitializeComponent();
        }

        private void frm_Alarm_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalVariable.Form.Alarm = null;
        }

        private void frm_Alarm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_TimerAlarm != null)
            {
                if (_TimerAlarm.Enabled)
                {
                    _TimerAlarm.Stop();
                }

                _TimerAlarm.Dispose();
                _TimerAlarm = null;
            }
        }

        private void frm_Alarm_Load(object sender, EventArgs e)
        {
            InitializeForm();

            _TimerAlarm = new Timer();
            _TimerAlarm.Tick += new EventHandler(Tick_Alarm);
            _TimerAlarm.Interval = 1000;
            _TimerAlarm.Start();
        }

        public void InitializeForm()
        {
            SetDataGridView();
        }

        private void SetDataGridView()
        {
            GlobalFunction.DataGridView.SetProperties(dgv_Alarm);

            GlobalFunction.DataGridView.AddColumns(dgv_Alarm, Enum.GetNames(typeof(e_DB_Alarm)));
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
    }
}
