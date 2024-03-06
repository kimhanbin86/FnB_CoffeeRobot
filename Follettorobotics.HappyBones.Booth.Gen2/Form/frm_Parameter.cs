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
    public partial class frm_Parameter : Form
    {
        private TabControl tabControl = null;
        private TabPage[] tabPages = null;
        private DataGridView[] dataGridViews = null;

        private bool _productOnly = false;

        #region Timer

        private Timer _TimerParameter = null;
        private void Tick_Parameter(object sender, EventArgs e)
        {
            _TimerParameter?.Stop();
            try
            {
                switch (GetParameter())
                {
                    case e_Parameter.Product:
                        bool update = GlobalFunction.UpdateProductStatus() && !GlobalFunction.CheckOrderProhibited();

                        DataGridView dgv = GetDataGridView();

                        var list = GlobalVariable.Parameter[(int)e_Parameter.Product].ToList();
                        for (int row = 0; row < list.Count; row++)
                        {
                            dgv.Rows[row].Cells[(int)e_Parameter_Product.Status - 1].Value = update ? GlobalVariable.Parameter[(int)e_Parameter.Product][list[row].Key][(int)e_Parameter_Product.Status] : CONST.S_NG;
                            dgv.Rows[row].Cells[(int)e_Parameter_Product.Status - 1].Style.BackColor = GlobalFunction.GetString(dgv.Rows[row].Cells[(int)e_Parameter_Product.Status - 1].Value) == CONST.S_OK ? Color.Lime : Color.Red;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            finally
            {
                _TimerParameter?.Start();
            }
        }

        #endregion

        public frm_Parameter(bool productOnly = false)
        {
            InitializeComponent();

            _productOnly = productOnly;
        }

        private void frm_Parameter_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalVariable.Form.Parameter = null;
        }

        private void frm_Parameter_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_TimerParameter != null)
            {
                if (_TimerParameter.Enabled)
                {
                    _TimerParameter.Stop();
                }

                _TimerParameter.Dispose();
                _TimerParameter = null;
            }

            ClearTabControl();
        }

        private void frm_Parameter_Load(object sender, EventArgs e)
        {
            tabControl = tabControl1;

            InitializeTabControl();

            InitializeForm();

            if (_productOnly)
            {
                for (int i = tabControl.TabPages.Count - 1; i > (int)e_Parameter.Product; i--)
                {
                    tabControl.TabPages.RemoveAt(i);
                }
            }

            TabControl_SelectedIndexChanged(null, null);

            _TimerParameter = new Timer();
            _TimerParameter.Tick += new EventHandler(Tick_Parameter);
            _TimerParameter.Interval = 100;
            _TimerParameter.Start();
        }

        public void InitializeForm()
        {
            SetControlsProperties();

            SetControlsText();

            #region ComboBox

            cb_Parameter_Order_Cup.Items.AddRange(Enum.GetNames(typeof(e_Order_Cup)));
            cb_Parameter_Order_Cup.SelectedIndex = 0;

            #endregion
        }

        public void SetControlsProperties()
        {
            GlobalFunction.SetControlsProperties(GlobalFunction.GetControls(this));
        }

        public void SetControlsText()
        {
            GlobalFunction.SetControlsText(GlobalFunction.GetControls(this));
        }

        private void ClearTabControl()
        {
            try
            {
                if (tabControl != null)
                {
                    tabControl.SelectedIndexChanged -= new EventHandler(TabControl_SelectedIndexChanged);
                }

                #region dataGridViews

                if (dataGridViews != null)
                {
                    for (int i = 0; i < dataGridViews.Length; i++)
                    {
                        if (dataGridViews[i] != null)
                        {
                            dataGridViews[i].CellContentClick -= new DataGridViewCellEventHandler(DataGridView_CellContentClick);
                            dataGridViews[i].DataError -= new DataGridViewDataErrorEventHandler(DataGridView_DataError);
                            dataGridViews[i].KeyUp -= new KeyEventHandler(DataGridView_KeyUp);
                            dataGridViews[i].MouseUp -= new MouseEventHandler(DataGridView_MouseUp);
                            dataGridViews[i].RowPostPaint -= new DataGridViewRowPostPaintEventHandler(DataGridView_RowPostPaint);

                            dataGridViews[i].CellValueChanged -= new DataGridViewCellEventHandler(DataGridView_CellValueChanged);

                            if (dataGridViews[i].ContextMenu != null)
                            {
                                dataGridViews[i].ContextMenu.Dispose();
                                dataGridViews[i].ContextMenu = null;
                            }

                            dataGridViews[i].Dispose();
                            dataGridViews[i] = null;
                        }
                    }

                    dataGridViews = null;
                }

                #endregion

                #region tabPages

                if (tabPages != null)
                {
                    for (int i = 0; i < tabPages.Length; i++)
                    {
                        if (tabPages[i] != null)
                        {
                            tabPages[i].Controls.Clear();

                            tabPages[i].Dispose();
                            tabPages[i] = null;
                        }
                    }

                    tabPages = null;
                }

                #endregion

                #region tabControl

                if (tabControl != null)
                {
                    tabControl.TabPages.Clear();

                    tabControl.Dispose();
                    tabControl = null;
                }

                #endregion
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        private DataGridView GetDataGridView()
        {
            return dataGridViews[tabControl.SelectedIndex];
        }

        private e_Parameter GetParameter()
        {
            return (e_Parameter)tabControl.SelectedIndex;
        }

        private void InitializeTabControl()
        {
            try
            {
                #region tabControl

                tabControl.SizeMode = TabSizeMode.Fixed;
                tabControl.ItemSize = new Size(100, 35);

                tabControl.SelectedIndexChanged += new EventHandler(TabControl_SelectedIndexChanged);

                #endregion

                string[] parameters = Enum.GetNames(typeof(e_Parameter));

                #region tabPages

                tabPages = new TabPage[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    tabPages[i] = new TabPage();

                    tabPages[i].Name = parameters[i];
                    tabPages[i].Text = parameters[i].Replace("_", " ");
                }

                tabControl.TabPages.AddRange(tabPages);

                #endregion

                #region dataGridViews

                dataGridViews = new DataGridView[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    dataGridViews[i] = new DataGridView();

                    dataGridViews[i].Dock = DockStyle.Fill;
                    dataGridViews[i].Name = parameters[i];

                    SetDataGridViewProperties(dataGridViews[i]);

                    dataGridViews[i].CellContentClick += new DataGridViewCellEventHandler(DataGridView_CellContentClick);
                    dataGridViews[i].DataError += new DataGridViewDataErrorEventHandler(DataGridView_DataError);
                    dataGridViews[i].KeyUp += new KeyEventHandler(DataGridView_KeyUp);
                    dataGridViews[i].MouseUp += new MouseEventHandler(DataGridView_MouseUp);
                    dataGridViews[i].RowPostPaint += new DataGridViewRowPostPaintEventHandler(DataGridView_RowPostPaint);

                    dataGridViews[i].CellValueChanged += new DataGridViewCellEventHandler(DataGridView_CellValueChanged);

                    tabPages[i].Controls.Add(dataGridViews[i]);
                }

                #endregion
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AddDataGridViewColumns(GetDataGridView(), GetParameter());

                AddDataGridViewRows(GetDataGridView(), GetParameter());

                btn_Parameter_Test.Visible = cb_Parameter_Order_Cup.Visible = GetParameter() == e_Parameter.Product;

                txt_Parameter_Description.Text = GetParameterDescription(GetParameter());
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        // TODO
        private string GetParameterDescription(e_Parameter parameter)
        {
            string result = string.Empty;
            try
            {
                string[] columns = GlobalFunction.GetNames(parameter);

                for (int i = 1; i < columns.Length; i++)
                {
                    result += $"- {columns[i].Replace("_", " ")} : ";

                    switch (parameter)
                    {
                        case e_Parameter.Product:
                            switch ((e_Parameter_Product)i)
                            {
                                case e_Parameter_Product.Status           : result += "제조 가능 여부"; break;
                                case e_Parameter_Product.Product_Code     : result += "음료 코드"; break;
                                case e_Parameter_Product.Product_Name     : result += "음료 이름"; break;
                                case e_Parameter_Product.Little_Delay     : result += "테이크아웃 컵을 사용하는 시럽 음료 제조 시, 컵 픽업부터 첫 번째 커피머신 도착까지 소요되는 시간 (초:0.0)"; break;
                                case e_Parameter_Product.Coffee_ID        : result += "음료별 커피머신 키값 / 커피 음료는 0초과 값을 입력해야 하며, 에이드 음료는 공백"; break;
                                case e_Parameter_Product.Hot_Water_Timeout: result += "HOT 아메리카노 제조 시, 뜨거운 물 받기 완료까지 타임아웃 / 경과 시에는 커피 위치로 이동 (초:0.0)"; break;
                                case e_Parameter_Product.Start_Delay      : result += "Non syrup - 컵 픽업부터 커피머신 도착까지 소요되는 시간 / Syrup - 첫 번째 커피머신 도착하여 소량 받기 완료부터 두 번째 커피머신 도착까지 소요되는 시간 (초:0.0)"; break;
                                case e_Parameter_Product.End_Delay        : result += "커피 추출 후 몇 방울 떨어지는데 대기후에 이동 (초:0.0)"; break;
                                case e_Parameter_Product.Cup              : result += "음료별 HOT 또는 ICE 컵 선택"; break;
                                case e_Parameter_Product.Milk             : result += "음료별 우유 사용 여부"; break;
                                case e_Parameter_Product.Ice_Time         : result += "음료별 얼음 추출 시간 (초:0.0)"; break;
                                case e_Parameter_Product.Water_Time       : result += "음료별 정수 추출 시간 / 에이드 음료는 탄산수 추출 시간 (초:0.0)"; break;
                                case e_Parameter_Product.Sauce_Type       : result += "음료별 소스 사용 여부"; break;
                                case e_Parameter_Product.Sauce_No         : result += "소스를 사용하는 음료별 소스 번호 선택"; break;
                                case e_Parameter_Product.Pumping_Count    : result += "소스를 사용하는 음료별 소스 펌핑 횟수 (회:0.0)"; break;
                                case e_Parameter_Product.Mixing_Count     : result += "소스를 사용하는 음료별 소스 믹싱 횟수 (회:0)"; break;
                                case e_Parameter_Product.Product_Time     : result += "음료별 제조 시간"; break;
                                case e_Parameter_Product.Remark           : result += ""; break;
                            }
                            break;
                        case e_Parameter.Booth:
                            switch ((e_Parameter_Booth)i)
                            {
                                case e_Parameter_Booth.Booth_No   : result += ""; break;
                                case e_Parameter_Booth.Language   : result += ""; break;
                                case e_Parameter_Booth.Log_Enabled: result += ""; break;
                                case e_Parameter_Booth.Simulation : result += ""; break;
                                case e_Parameter_Booth.Title      : result += ""; break;
                            }
                            break;
                        case e_Parameter.Door:
                            switch ((e_Parameter_Door)i)
                            {
                                case e_Parameter_Door.Enabled               : result += ""; break;
                                case e_Parameter_Door.DB_Initialize         : result += ""; break;
                                case e_Parameter_Door.Pickup_Delay_Sensor_OK: result += ""; break;
                                case e_Parameter_Door.Pickup_Delay_Sensor_NG: result += ""; break;
                            }
                            break;
                        case e_Parameter.CoffeeMaker:
                            switch ((e_Parameter_CoffeeMaker)i)
                            {
                                case e_Parameter_CoffeeMaker.Enabled           : result += ""; break;
                                case e_Parameter_CoffeeMaker.LogEnabled        : result += ""; break;
                                case e_Parameter_CoffeeMaker.Log_File          : result += ""; break;
                                case e_Parameter_CoffeeMaker.Device            : result += ""; break;
                                case e_Parameter_CoffeeMaker.PortName          : result += ""; break;
                                case e_Parameter_CoffeeMaker.Rinse_Interval_min: result += ""; break;
                                case e_Parameter_CoffeeMaker.Ignore_Warnings   : result += ""; break;
                            }
                            break;
                        case e_Parameter.Controller1:
                        case e_Parameter.Controller2:
                        case e_Parameter.IceMaker:
                            switch ((e_Parameter_Controller1)i)
                            {
                                case e_Parameter_Controller1.Enabled           : result += ""; break;
                                case e_Parameter_Controller1.LogEnabled        : result += ""; break;
                                case e_Parameter_Controller1.Log_File          : result += ""; break;
                                case e_Parameter_Controller1.LogEnabled_Process: result += ""; break;
                                case e_Parameter_Controller1.Device            : result += ""; break;
                                case e_Parameter_Controller1.PortName          : result += ""; break;
                                case e_Parameter_Controller1.BaudRate          : result += ""; break;
                                case e_Parameter_Controller1.Parity            : result += ""; break;
                                case e_Parameter_Controller1.DataBits          : result += ""; break;
                                case e_Parameter_Controller1.StopBits          : result += ""; break;
                                case e_Parameter_Controller1.Timeout           : result += ""; break;
                            }
                            break;
                        case e_Parameter.Robot:
                            switch ((e_Parameter_Robot)i)
                            {
                                case e_Parameter_Robot.Enabled           : result += ""; break;
                                case e_Parameter_Robot.LogEnabled        : result += ""; break;
                                case e_Parameter_Robot.Log_File          : result += ""; break;
                                case e_Parameter_Robot.LogEnabled_Process: result += ""; break;
                                case e_Parameter_Robot.Device            : result += ""; break;
                                case e_Parameter_Robot.IP                : result += ""; break;
                                case e_Parameter_Robot.Port              : result += ""; break;
                                case e_Parameter_Robot.ConnectTimeout    : result += ""; break;
                                case e_Parameter_Robot.ReceiveTimeout    : result += ""; break;
                            }
                            break;
                        case e_Parameter.Alarm:
                            switch ((e_Parameter_Alarm)i)
                            {
                                case e_Parameter_Alarm.Enabled  : result += ""; break;
                                case e_Parameter_Alarm.Alarm    : result += ""; break;
                                case e_Parameter_Alarm.Code     : result += ""; break;
                                case e_Parameter_Alarm.en_Text  : result += ""; break;
                                case e_Parameter_Alarm.en_Action: result += ""; break;
                                case e_Parameter_Alarm.ko_Text  : result += ""; break;
                                case e_Parameter_Alarm.ko_Action: result += ""; break;
                            }
                            break;
                        case e_Parameter.Control:
                            switch ((e_Parameter_Control)i)
                            {
                                case e_Parameter_Control.Control_Name: result += ""; break;
                                case e_Parameter_Control.en_Font     : result += ""; break;
                                case e_Parameter_Control.en_Text     : result += ""; break;
                                case e_Parameter_Control.ko_Font     : result += ""; break;
                                case e_Parameter_Control.ko_Text     : result += ""; break;
                                case e_Parameter_Control.Visible     : result += ""; break;
                            }
                            break;
                        case e_Parameter.DB:
                            switch ((e_Parameter_DB)i)
                            {
                                case e_Parameter_DB.Type                  : result += ""; break;
                                case e_Parameter_DB.Server                : result += ""; break;
                                case e_Parameter_DB.Port                  : result += ""; break;
                                case e_Parameter_DB.Database              : result += ""; break;
                                case e_Parameter_DB.Uid                   : result += ""; break;
                                case e_Parameter_DB.Pwd                   : result += ""; break;
                                case e_Parameter_DB.Connection_Timeout_sec: result += ""; break;
                            }
                            break;
                    }

                    if (i < columns.Length - 1)
                    {
                        result += "\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        #region DataGridView

        #region Event

        private void DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridView dgv = sender as DataGridView;

                    if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewButtonCell)
                    {
                        switch (GetParameter())
                        {
                            case e_Parameter.DID_Bottom:
                                switch ((e_Parameter_DID_Bottom)(e.ColumnIndex + 1))
                                {
                                    case e_Parameter_DID_Bottom.Font:
                                        using (FontDialog dlg = new FontDialog())
                                        {
                                            if (dlg.ShowDialog() == DialogResult.OK)
                                            {
                                                TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Font));
                                                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = typeConverter.ConvertToString(dlg.Font).Replace(",", "/");
                                            }
                                        }
                                        break;
                                }
                                break;
                            case e_Parameter.Door:
                                switch ((e_Parameter_Door)(e.ColumnIndex + 1))
                                {
                                    case e_Parameter_Door.DB_Initialize:
                                        if (GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetMessage(e_Message.Parameter_Door), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                        {
                                            GlobalFunction.Door.Initialize();
                                        }
                                        break;
                                }
                                break;
                            case e_Parameter.DID:
                                switch ((e_Parameter_DID)(e.ColumnIndex + 1))
                                {
                                    case e_Parameter_DID.Process_Path:
                                        using (OpenFileDialog dlg = new OpenFileDialog())
                                        {
                                            if (dlg.ShowDialog() == DialogResult.OK)
                                            {
                                                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dlg.FileName;
                                            }
                                        }
                                        break;
                                }
                                break;
                            case e_Parameter.Control:
                                switch ((e_Parameter_Control)(e.ColumnIndex + 1))
                                {
                                    case e_Parameter_Control.en_Font:
                                    case e_Parameter_Control.ko_Font:
                                        using (FontDialog dlg = new FontDialog())
                                        {
                                            if (dlg.ShowDialog() == DialogResult.OK)
                                            {
                                                TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Font));
                                                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = typeConverter.ConvertToString(dlg.Font).Replace(",", "/");
                                            }
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        private void DataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                e.Cancel = false;
                e.ThrowException = false;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        private void DataGridView_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                DataGridView dgv = sender as DataGridView;

                if (e.KeyCode == Keys.V && e.Control)
                {
                    char[] rowSplitter = { '\r', '\n' };
                    char[] columSplitter = { '\t' };
                    int[] columnsOrderArray = dgv.Columns.Cast<DataGridViewColumn>().Where(x => x.Visible).OrderBy(x => x.DisplayIndex).Select(x => x.Index).ToArray();

                    IDataObject dataInClipboard = Clipboard.GetDataObject();
                    string stringInClipboard = (string)dataInClipboard.GetData(DataFormats.Text);

                    string[] rowInClipboard = stringInClipboard.Split(rowSplitter, StringSplitOptions.RemoveEmptyEntries);
                    int r = dgv.SelectedCells[0].RowIndex;
                    int c = columnsOrderArray[dgv.SelectedCells[0].ColumnIndex];

                    if (dgv.Rows.Count < (r + rowInClipboard.Length))
                    {
                        dgv.Rows.Add(r + rowInClipboard.Length - dgv.Rows.Count + 1);
                    }

                    for (int iRow = 0; iRow < rowInClipboard.Length; iRow++)
                    {
                        string[] valuesInRow = rowInClipboard[iRow].Split(columSplitter);

                        for (int iCol = 0; iCol < valuesInRow.Length; iCol++)
                        {
                            if (columnsOrderArray.Count() - 1 >= c + iCol)
                            {
                                int idx = columnsOrderArray[c + iCol];
                                dgv.Rows[r + iRow].Cells[idx].Value = valuesInRow[iCol];
                            }
                        }
                    }
                }
                else if (e.KeyCode == Keys.Delete)
                {
                    foreach (DataGridViewCell cell in dgv.SelectedCells)
                    {
                        cell.Value = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        private void DataGridView_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    DataGridView dgv = sender as DataGridView;

                    DataGridView.HitTestInfo info = dgv.HitTest(e.X, e.Y);

                    if (info.Type == DataGridViewHitTestType.RowHeader)
                    {
                        if (dgv.ContextMenu == null)
                        {
                            ContextMenu contextMenu = new ContextMenu();
                            contextMenu.MenuItems.Add(new MenuItem("Remove", ContextMenu_Click_RemoveDataGridViewSelectedRows));

                            dgv.ContextMenu = contextMenu;
                        }

                        if (dgv.ContextMenu != null)
                        {
                            dgv.ContextMenu.Show(dgv, e.Location);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        #region ContextMenu

        private void ContextMenu_Click_RemoveDataGridViewSelectedRows(object sender, EventArgs e)
        {
            try
            {
                DataGridView dgv = GetDataGridView();

                DataGridViewSelectedRowCollection selectedRows = dgv.SelectedRows;
                for (int row = 0; row < selectedRows.Count; row++)
                {
                    dgv.Rows.Remove(selectedRows[row]);
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        #endregion

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

        private void DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        #endregion

        private void SetDataGridViewProperties(DataGridView dgv)
        {
            try
            {
                GlobalFunction.DoubleBuffered(dgv, true);

                //dgv.AllowUserToAddRows = false;
                //dgv.AllowUserToDeleteRows = false;
                //dgv.AllowUserToOrderColumns = false;
                //dgv.AllowUserToResizeColumns = false;
                dgv.AllowUserToResizeRows = false;
                //dgv.MultiSelect = false;

                //dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray; // 홀수 번호 행에 적용되는 기본 셀 스타일을 설정

                DataGridViewCellStyle defaultCellStyle = new DataGridViewCellStyle();
                defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                defaultCellStyle.Font = new Font(e_Font.Tahoma.ToString(), 11f);
                dgv.DefaultCellStyle = defaultCellStyle;

                //dgv.RowHeadersVisible = false;

                dgv.RowHeadersWidth = 60;
                dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

                using (DataGridViewRow rowTemplate = new DataGridViewRow())
                {
                    rowTemplate.Height = 30;
                    dgv.RowTemplate = rowTemplate;
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        private void AddDataGridViewColumns(DataGridView dgv, e_Parameter parameter)
        {
            try
            {
                string[] columns = GlobalFunction.GetNames(parameter);

                dgv.Columns.Clear();

                for (int i = 1; i < columns.Length; i++)
                {
                    dgv.Columns.Add(columns[i], columns[i].Replace("_", " "));

                    dgv.Columns[i - 1].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                switch (parameter)
                {
                    case e_Parameter.Product:
                        dgv.Columns[e_Parameter_Product.Product_Name.ToString()].Frozen = true;

                        dgv.Columns[e_Parameter_Product.Status.ToString()].ReadOnly = true;
                        dgv.Columns[e_Parameter_Product.Product_Code.ToString()].ReadOnly = true;
                        break;
                }

                dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                DataGridViewCellStyle defaultCellStyle = new DataGridViewCellStyle();
                defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //defaultCellStyle.BackColor = SystemColors.Highlight;
                //defaultCellStyle.ForeColor = SystemColors.HighlightText;
                defaultCellStyle.Font = new Font(e_Font.Tahoma.ToString(), 11f);
                dgv.ColumnHeadersDefaultCellStyle = defaultCellStyle;

                dgv.ColumnHeadersHeight = 30;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                dgv.EnableHeadersVisualStyles = false;

                //dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        private void AddDataGridViewRows(DataGridView dgv, e_Parameter parameter)
        {
            try
            {
                dgv.Rows.Clear();

                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                var list = GlobalVariable.Parameter[(int)parameter].ToList();
                for (int row = 0; row < list.Count; row++)
                {
                    dgv.Rows.Add();

                    for (int col = 1; col <= dgv.Columns.Count; col++)
                    {
                        #region Cell (Button, ComboBox)

                        DataGridViewButtonCell buttonCell = new DataGridViewButtonCell();

                        DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();

                        switch (parameter)
                        {
                            case e_Parameter.Product:
                                switch ((e_Parameter_Product)col)
                                {
                                    case e_Parameter_Product.Cup:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_ComboBox_Cup)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                    case e_Parameter_Product.Milk:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_ComboBox_Use)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                    case e_Parameter_Product.Sauce_Type:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_ComboBox_Sauce_Type)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                    case e_Parameter_Product.Sauce_No:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_Device_Controller2_Sauce)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                }
                                break;
                            case e_Parameter.Booth:
                                switch ((e_Parameter_Booth)col)
                                {
                                    case e_Parameter_Booth.Language:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_ComboBox_Language)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                    case e_Parameter_Booth.Log_Enabled:
                                    case e_Parameter_Booth.Simulation:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_ComboBox_Use)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                }
                                break;
                            case e_Parameter.DID_Bottom:
                                switch ((e_Parameter_DID_Bottom)col)
                                {
                                    case e_Parameter_DID_Bottom.Enabled:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_ComboBox_Use)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                    case e_Parameter_DID_Bottom.Font:
                                        buttonCell.Value = "value";
                                        dgv.Rows[row].Cells[col - 1] = buttonCell;
                                        break;
                                }
                                break;
                            case e_Parameter.Door:
                                switch ((e_Parameter_Door)col)
                                {
                                    case e_Parameter_Door.Enabled:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_ComboBox_Use)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                    case e_Parameter_Door.DB_Initialize:
                                        buttonCell.Value = "value";
                                        dgv.Rows[row].Cells[col - 1] = buttonCell;
                                        break;
                                }
                                break;

                            case e_Parameter.CoffeeMaker:
                                switch ((e_Parameter_CoffeeMaker)col)
                                {
                                    case e_Parameter_CoffeeMaker.Enabled:
                                    case e_Parameter_CoffeeMaker.LogEnabled:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_ComboBox_Use)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                    case e_Parameter_CoffeeMaker.Device:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_Device_CoffeeMaker)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                }
                                break;
                            case e_Parameter.Controller1:
                                switch ((e_Parameter_Controller1)col)
                                {
                                    case e_Parameter_Controller1.Enabled:
                                    case e_Parameter_Controller1.LogEnabled:
                                    case e_Parameter_Controller1.LogEnabled_Process:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_ComboBox_Use)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                    case e_Parameter_Controller1.Parity:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(System.IO.Ports.Parity)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                    case e_Parameter_Controller1.StopBits:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(System.IO.Ports.StopBits)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                }
                                break;
                            case e_Parameter.Controller2:
                                switch ((e_Parameter_Controller2)col)
                                {
                                    case e_Parameter_Controller2.Enabled:
                                    case e_Parameter_Controller2.LogEnabled:
                                    case e_Parameter_Controller2.LogEnabled_Process:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_ComboBox_Use)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                    case e_Parameter_Controller2.Parity:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(System.IO.Ports.Parity)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                    case e_Parameter_Controller2.StopBits:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(System.IO.Ports.StopBits)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                }
                                break;
                            case e_Parameter.IceMaker:
                                switch ((e_Parameter_IceMaker)col)
                                {
                                    case e_Parameter_IceMaker.Enabled:
                                    case e_Parameter_IceMaker.LogEnabled:
                                    case e_Parameter_IceMaker.LogEnabled_Process:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_ComboBox_Use)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                    case e_Parameter_IceMaker.Device:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_Device_IceMaker)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                    case e_Parameter_IceMaker.Parity:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(System.IO.Ports.Parity)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                    case e_Parameter_IceMaker.StopBits:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(System.IO.Ports.StopBits)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                }
                                break;
                            case e_Parameter.Robot:
                                switch ((e_Parameter_Robot)col)
                                {
                                    case e_Parameter_Robot.Enabled:
                                    case e_Parameter_Robot.LogEnabled:
                                    case e_Parameter_Robot.LogEnabled_Process:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_ComboBox_Use)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                    case e_Parameter_Robot.Device:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_Device_Robot)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                }
                                break;
                            case e_Parameter.Barcode:
                                switch ((e_Parameter_Barcode)col)
                                {
                                    case e_Parameter_Barcode.Enabled:
                                    case e_Parameter_Barcode.LogEnabled:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_ComboBox_Use)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                    case e_Parameter_Barcode.Parity:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(System.IO.Ports.Parity)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                    case e_Parameter_Barcode.StopBits:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(System.IO.Ports.StopBits)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                }
                                break;
                            case e_Parameter.DID:
                                switch ((e_Parameter_DID)col)
                                {
                                    case e_Parameter_DID.Enabled:
                                    case e_Parameter_DID.LogEnabled:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_ComboBox_Use)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                    case e_Parameter_DID.Thema:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_ComboBox_DID_Thema)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                    case e_Parameter_DID.Process_Path:
                                        buttonCell.Value = "value";
                                        dgv.Rows[row].Cells[col - 1] = buttonCell;
                                        break;
                                }
                                break;
                            case e_Parameter.Kiosk:
                                switch ((e_Parameter_Kiosk)col)
                                {
                                    case e_Parameter_Kiosk.Enabled:
                                    case e_Parameter_Kiosk.LogEnabled:
                                    case e_Parameter_Kiosk.Log_View:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_ComboBox_Use)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                }
                                break;
                            case e_Parameter.Remote:
                                switch ((e_Parameter_Remote)col)
                                {
                                    case e_Parameter_Remote.Enabled:
                                    case e_Parameter_Remote.LogEnabled:
                                    case e_Parameter_Remote.Log_View:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_ComboBox_Use)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                }
                                break;

                            case e_Parameter.Alarm:
                                switch ((e_Parameter_Alarm)col)
                                {
                                    case e_Parameter_Alarm.Alarm:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_Alarm)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                    case e_Parameter_Alarm.Enabled:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_ComboBox_Use)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                }
                                break;
                            case e_Parameter.Control:
                                switch ((e_Parameter_Control)col)
                                {
                                    case e_Parameter_Control.en_Font:
                                        buttonCell.Value = "value";
                                        dgv.Rows[row].Cells[col - 1] = buttonCell;
                                        break;
                                    case e_Parameter_Control.ko_Font:
                                        buttonCell.Value = "value";
                                        dgv.Rows[row].Cells[col - 1] = buttonCell;
                                        break;
                                    case e_Parameter_Control.Visible:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_ComboBox_Use)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                }
                                break;
                            case e_Parameter.DB:
                                switch ((e_Parameter_DB)col)
                                {
                                    case e_Parameter_DB.Type:
                                        comboBoxCell.Items.AddRange(Enum.GetNames(typeof(e_ComboBox_DB_Type)));
                                        dgv.Rows[row].Cells[col - 1] = comboBoxCell;
                                        break;
                                }
                                break;
                        }

                        #region Dispose

                        if (buttonCell.Value == null)
                        {
                            buttonCell.Dispose();
                            buttonCell = null;
                        }

                        if (comboBoxCell.Items.Count == 0)
                        {
                            comboBoxCell.Dispose();
                            comboBoxCell = null;
                        }

                        #endregion

                        #endregion

                        #region Value

                        if (col < GlobalVariable.Parameter[(int)parameter][list[row].Key].Length)
                        {
                            dgv.Rows[row].Cells[col - 1].Value = GlobalVariable.Parameter[(int)parameter][list[row].Key][col];
                        }

                        #endregion
                    }
                }

                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        #endregion

        private int Order_No = 1;

        private void Parameter_Button_Click(object sender, EventArgs e)
        {
            try
            {
                Button button = sender as Button;
                string buttonName = button.Name.Substring(button.Name.LastIndexOf("_") + 1);

                Log.Write(MethodBase.GetCurrentMethod().Name, buttonName);

                if (buttonName == "Close")
                {
                    Close();
                }
                else
                {
                    DataGridView dgv = GetDataGridView();
                    e_Parameter parameter = GetParameter();

                    if (buttonName == "Save")
                    {
                        if (GlobalFunction.SaveParameter(MethodBase.GetCurrentMethod().Name, dgv, parameter))
                        {
                            GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, $"{parameter} Parameter Save OK");

                            switch (parameter)
                            {
                                case e_Parameter.Booth:
                                    Log.Enabled = GlobalVariable.Parameter[(int)e_Parameter.Booth][CONST.S_KEY][(int)e_Parameter_Booth.Log_Enabled] == e_ComboBox_Use.Use.ToString();
                                    break;
                                case e_Parameter.DID_Bottom:
                                    GlobalVariable.Form.DID_Bottom?.Close();

                                    GlobalVariable.Form.ControlPanel?.dIDBottomToolStripMenuItem_Click(null, null);
                                    break;
                                case e_Parameter.CoffeeMaker:
                                    GlobalDevice.Stop(e_Device.CoffeeMaker);
                                    break;
                                case e_Parameter.Controller1:
                                    GlobalDevice.Stop(e_Device.Controller1);
                                    break;
                                case e_Parameter.Controller2:
                                    GlobalDevice.Stop(e_Device.Controller2);
                                    break;
                                case e_Parameter.IceMaker:
                                    GlobalDevice.Stop(e_Device.IceMaker);
                                    break;
                                case e_Parameter.Robot:
                                    GlobalDevice.Stop(e_Device.Robot);
                                    break;
                                case e_Parameter.Barcode:
                                    GlobalDevice.Stop(e_Device.Barcode);
                                    break;
                                case e_Parameter.DID:
                                    // TODO
                                    break;
                                case e_Parameter.Kiosk:
                                    GlobalDevice.Stop(e_Device.Kiosk);
                                    break;
                                case e_Parameter.Remote:
                                    GlobalDevice.Stop(e_Device.Remote);
                                    break;
                            }

                            #region InitializeForm, SetControlsProperties/Text, SetParameterText

                            switch (parameter)
                            {
                                case e_Parameter.Booth:
                                case e_Parameter.CoffeeMaker:
                                case e_Parameter.IceMaker:
                                case e_Parameter.Robot:
                                case e_Parameter.Control:
                                    GlobalVariable.Form.Alarm?.InitializeForm();

                                    GlobalVariable.Form.ControlPanel?.SetControlsProperties();
                                    GlobalVariable.Form.ControlPanel?.SetControlsText();
                                    GlobalVariable.Form.ControlPanel?.SetParameterText();

                                    SetControlsProperties();
                                    SetControlsText();
                                    break;
                            }

                            #endregion
                        }
                        else
                        {
                            GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, $"{parameter} Parameter Save NG", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (buttonName == "Test")
                    {
                        bool insert = false;

                        if (GlobalFunction.GetSimulation())
                        {
                            insert = true;
                        }
                        else
                        {
                            insert = GlobalFunction.GetString(dgv.Rows[dgv.CurrentRow.Index].Cells[(int)e_Parameter_Product.Status - 1].Value) == CONST.S_OK;
                        }

                        if (insert)
                        {
                            int key = dgv.CurrentRow.Index + 1;

                            for (int col = 0; col < dgv.Columns.Count; col++)
                            {
                                GlobalVariable.Parameter[(int)e_Parameter.Product][key.ToString()][(int)e_Parameter_Product.Status + col] = GlobalFunction.GetString(dgv.Rows[dgv.CurrentCell.RowIndex].Cells[col].Value);
                            }

                            string DateTime = GlobalFunction.GetDateTimeString(System.DateTime.Now);
                            string ID = $"{DateTime.Replace("-", "").Replace(" ", "").Replace(":", "").Replace(".", "").Replace(",", "")}_{Order_No++:000}";
                            string productCode = GlobalVariable.Parameter[(int)e_Parameter.Product][key.ToString()][(int)e_Parameter_Product.Product_Code];
                            string productName = GlobalVariable.Parameter[(int)e_Parameter.Product][key.ToString()][(int)e_Parameter_Product.Product_Name];

                            e_Order_Cup Order_Cup = e_Order_Cup.TAKEOUT;
                            try
                            {
                                Order_Cup = (e_Order_Cup)Enum.Parse(typeof(e_Order_Cup), cb_Parameter_Order_Cup.Text);
                            }
                            catch
                            {
                            }

                            string cmdText = $" INSERT INTO {e_DB._Order}({e_DB_Order.Column00},{e_DB_Order.Column01},{e_DB_Order.Column02},{e_DB_Order.Column04},{e_DB_Order.Column05},{e_DB_Order.Column07},{e_DB_Order.Column08},{e_DB_Order.Column12})" +
                                             $" VALUES('{DateTime}','{ID}','{e_Order_Status.주문}','{e_Order_Source.TEST}','{Order_No - 1:000}','{productCode}','{productName}','{Order_Cup}')";

                            GlobalFunction.DB.MySQL.Query(cmdText);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }
    }
}
