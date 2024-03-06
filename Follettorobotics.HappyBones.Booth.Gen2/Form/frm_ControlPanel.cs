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
    public partial class frm_ControlPanel : Form
    {
        #region

        private void Log_MsgEvent(Msg msg)
        {
            logListView1.AddListViewItem(msg);
        }

        private void splitter1_MouseUp(object sender, MouseEventArgs e)
        {
            panel1.Height -= e.Location.Y;
        }

        private void splitter1_DoubleClick(object sender, EventArgs e)
        {
            panel1.Height = 200;
        }

        #endregion

        #region Thread

        private void StartThread()
        {
            _ThreadCoffeeMaker = new System.Threading.Thread(Process_CoffeeMaker);
            _ThreadCoffeeMaker.IsBackground = true;
            _isThreadCoffeeMaker = true;
            _ThreadCoffeeMaker.Start();

            _ThreadController1 = new System.Threading.Thread(Process_Controller1);
            _ThreadController1.IsBackground = true;
            _isThreadController1 = true;
            _ThreadController1.Start();

            _ThreadController2 = new System.Threading.Thread(Process_Controller2);
            _ThreadController2.IsBackground = true;
            _isThreadController2 = true;
            _ThreadController2.Start();

            _ThreadIceMaker = new System.Threading.Thread(Process_IceMaker);
            _ThreadIceMaker.IsBackground = true;
            _isThreadIceMaker = true;
            _ThreadIceMaker.Start();

            _ThreadRobot = new System.Threading.Thread(Process_Robot);
            _ThreadRobot.IsBackground = true;
            _isThreadRobot = true;
            _ThreadRobot.Start();

            for (int i = (int)e_Door.Door1; i < Enum.GetNames(typeof(e_Door)).Length; i++)
            {
                _ThreadSequence_Door = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(Process_Sequence_Door));
                _ThreadSequence_Door.IsBackground = true;
                _isThreadSequence_Door = true;
                _ThreadSequence_Door.Start(i);
            }

            _ThreadSequence_Main = new System.Threading.Thread(Process_Sequence_Main);
            _ThreadSequence_Main.IsBackground = true;
            _isThreadSequence_Main = true;
            _ThreadSequence_Main.Start();

            _ThreadSequence_Coffee = new System.Threading.Thread(Process_Sequence_Coffee);
            _ThreadSequence_Coffee.IsBackground = true;
            _isThreadSequence_Coffee = true;
            _ThreadSequence_Coffee.Start();

            _ThreadSequence_Little = new System.Threading.Thread(Process_Sequence_Little);
            _ThreadSequence_Little.IsBackground = true;
            _isThreadSequence_Little = true;
            _ThreadSequence_Little.Start();

            _ThreadSequence_Tumbler = new System.Threading.Thread(Process_Sequence_Tumbler);
            _ThreadSequence_Tumbler.IsBackground = true;
            _isThreadSequence_Tumbler = true;
            _ThreadSequence_Tumbler.Start();
        }

        private void StopThread()
        {
            _isThreadAlarm = false;

            _isThreadCoffeeMaker = false;
            _isThreadController1 = false;
            _isThreadController2 = false;
            _isThreadIceMaker = false;
            _isThreadRobot = false;

            _isThreadSequence_Door = false;
            _isThreadSequence_Main = false;
            _isThreadSequence_Coffee = false;
            _isThreadSequence_Little = false;
            _isThreadSequence_Tumbler = false;
        }

        #endregion

        #region Timer

        private void StartTimer()
        {
            _TimerControlPanel = new Timer();
            _TimerControlPanel.Tick += new EventHandler(Tick_ControlPanel);
            _TimerControlPanel.Interval = 100;
            _TimerControlPanel.Start();

            _TimerDevice = new Timer();
            _TimerDevice.Tick += new EventHandler(Tick_Device);
            _TimerDevice.Interval = 1000;
            _TimerDevice.Start();

            _TimerOrder = new Timer();
            _TimerOrder.Tick += new EventHandler(Tick_Order);
            _TimerOrder.Interval = 1000;
            _TimerOrder.Start();
        }

        private void StopTimer()
        {
            if (_TimerControlPanel != null)
            {
                if (_TimerControlPanel.Enabled)
                {
                    _TimerControlPanel.Stop();
                }

                _TimerControlPanel.Dispose();
                _TimerControlPanel = null;
            }

            if (_TimerDevice != null)
            {
                if (_TimerDevice.Enabled)
                {
                    _TimerDevice.Stop();
                }

                _TimerDevice.Dispose();
                _TimerDevice = null;
            }

            if (_TimerOrder != null)
            {
                if (_TimerOrder.Enabled)
                {
                    _TimerOrder.Stop();
                }

                _TimerOrder.Dispose();
                _TimerOrder = null;
            }
        }

        #endregion

        public frm_ControlPanel()
        {
            InitializeComponent();
        }

        private void frm_ControlPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            Log.Dispose();

            GlobalVariable.Form.ControlPanel = null;
        }

        private void frm_ControlPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (GlobalVariable.Form.ControlPanel != null)
            {
                if (GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetMessage(e_Message.ControlPanel_UserClosing), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }

                if (e.Cancel == false)
                {
                    StopTimer();

                    StopThread();

                    GlobalDevice.Stop();

                    Log.Write(MethodBase.GetCurrentMethod().Name, "Program Stop");
                }
            }
        }

        private void frm_ControlPanel_Load(object sender, EventArgs e)
        {
            if (GlobalFunction.CheckProcess(System.Diagnostics.Process.GetCurrentProcess().ProcessName))
            {
                GlobalVariable.Form.ControlPanel = this;

                Log.MsgEvent += new MsgEventHandler(Log_MsgEvent);

                _ThreadAlarm = new System.Threading.Thread(Process_Alarm);
                _ThreadAlarm.IsBackground = true;
                _isThreadAlarm = true;
                _ThreadAlarm.Start();

                if (GlobalFunction.LoadParameter())
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, $"Program Start - Ver {Assembly.GetExecutingAssembly().GetName().Version}");

                    InitializeForm();

                    if (GlobalDevice.Start())
                    {
                        try
                        {
                            Log.Enabled = GlobalVariable.Parameter[(int)e_Parameter.Booth][CONST.S_KEY][(int)e_Parameter_Booth.Log_Enabled] == e_ComboBox_Use.Use.ToString();
                        }
                        catch
                        {
                        }

                        StartThread();

                        StartTimer();

                        dIDBottomToolStripMenuItem_Click(null, null);
                    }
                    else
                    {
                        GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_START] = true;
                    }
                }
                else
                {
                    GlobalVariable.Alarm[(int)e_Alarm.ERROR_PARAMETER_LOAD] = true;
                }
            }
            else
            {
                GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetMessage(e_Message.ControlPanel_CheckProcess), MessageBoxButtons.OK, MessageBoxIcon.Error);

                exitToolStripMenuItem_Click(null, null);
            }
        }

        public void InitializeForm()
        {
            SetControlsProperties();

            SetControlsText();

            SetParameterText();

            #region ComboBox

            SetComboBox(cb_ControlPanel_Controller2_Turn_Sauce, Enum.GetNames(typeof(e_Device_Controller2_Sauce)));

            SetComboBox(cb_ControlPanel_Robot_Control, Enum.GetNames(typeof(e_Device_Robot_Control)));

            SetComboBox(cb_ControlPanel_Robot_Command, Enum.GetNames(typeof(e_Device_Robot_Command)));

            SetComboBox(cb_ControlPanel_Robot_Cup, Enum.GetNames(typeof(e_Device_Robot_Cup)));

            #endregion

            SetDataGridView();
        }

        public void SetControlsProperties()
        {
            GlobalFunction.SetControlsProperties(GlobalFunction.GetControls(this));
        }

        public void SetControlsText()
        {
            GlobalFunction.SetControlsText(GlobalFunction.GetControls(this));
        }

        public void SetParameterText()
        {
            try
            {
                Text = GlobalVariable.Parameter[(int)e_Parameter.Booth][CONST.S_KEY][(int)e_Parameter_Booth.Title] + $" - Ver {Assembly.GetExecutingAssembly().GetName().Version} / {GlobalFunction.GetDateTimeString(GlobalVariable.ProgramStarted)}";

                grp_ControlPanel_CoffeeMaker.Text = grp_ControlPanel_CoffeeMaker.Text + " - " + GlobalVariable.Parameter[(int)e_Parameter.CoffeeMaker][CONST.S_KEY][(int)e_Parameter_CoffeeMaker.Device];

                grp_ControlPanel_IceMaker.Text = grp_ControlPanel_IceMaker.Text + " - " + GlobalVariable.Parameter[(int)e_Parameter.IceMaker][CONST.S_KEY][(int)e_Parameter_IceMaker.Device];

                grp_ControlPanel_Robot.Text = grp_ControlPanel_Robot.Text + " - " + GlobalVariable.Parameter[(int)e_Parameter.Robot][CONST.S_KEY][(int)e_Parameter_Robot.Device];

                #region Robot

                cb_ControlPanel_Robot_Control.Visible = btn_ControlPanel_Robot_Control.Visible = GlobalVariable.Parameter[(int)e_Parameter.Booth][CONST.S_KEY][(int)e_Parameter_Booth.Simulation] == e_ComboBox_Use.Use.ToString();
                chk_ControlPanel_Robot_Ade.Visible = cb_ControlPanel_Robot_Cup.Visible = cb_ControlPanel_Robot_Command.Visible = btn_ControlPanel_Robot_Command.Visible = GlobalVariable.Parameter[(int)e_Parameter.Booth][CONST.S_KEY][(int)e_Parameter_Booth.Simulation] == e_ComboBox_Use.Use.ToString();

                #endregion
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        private void SetComboBox(ComboBox comboBox, object[] items, int select = 0)
        {
            comboBox.Items.AddRange(items);

            comboBox.SelectedIndex = select;
        }

        private void SetDataGridView()
        {
            GlobalFunction.DataGridView.SetProperties(dgv_Order);

            GlobalFunction.DataGridView.AddColumns(dgv_Order, Enum.GetNames(typeof(e_DB_Order)));
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

        /// <summary>
        /// for Eversys
        /// </summary>
        private void GetAlarmCode()
        {
            string[] strings = Enum.GetNames(typeof(EversysApi.Defines.Error_t));

            for (int i = 0; i < strings.Length; i++)
            {
                EversysApi.Defines.Error_t _t = (EversysApi.Defines.Error_t)Enum.Parse(typeof(EversysApi.Defines.Error_t), strings[i]);

                Log.Write(string.Empty, $"E-{(int)_t:000}");
            }
        }

        #region MenuStrip

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void parameterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.Form.Parameter == null)
            {
                using (frm_Login frm = new frm_Login())
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVariable.Form.Parameter = new frm_Parameter();
                        GlobalVariable.Form.Parameter.Show();
                    }
                }
            }
            else
            {
                GlobalVariable.Form.Parameter.BringToFront();
            }
        }

        private void alarmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.Form.Alarm == null)
            {
                GlobalVariable.Form.Alarm = new frm_Alarm();
                GlobalVariable.Form.Alarm.Show();
            }
            else
            {
                GlobalVariable.Form.Alarm.BringToFront();
            }
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logToolStripMenuItem.Checked = !logToolStripMenuItem.Checked;

            panel1.Visible = logToolStripMenuItem.Checked;

            if (panel1.Visible)
            {
                splitter1_DoubleClick(null, null);
            }
        }

        private void monitoringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.Form.Monitoring == null)
            {
                GlobalVariable.Form.Monitoring = new frm_Monitoring();
                GlobalVariable.Form.Monitoring.Show();
            }
            else
            {
                GlobalVariable.Form.Monitoring.BringToFront();
            }
        }

        public void dIDBottomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalFunction.GetEnabled(e_Parameter.DID_Bottom))
            {
                if (GlobalVariable.Form.DID_Bottom == null)
                {
                    GlobalVariable.Form.DID_Bottom = new frm_DID_Bottom();
                    GlobalVariable.Form.DID_Bottom.Show();
                }
                else
                {
                    GlobalVariable.Form.DID_Bottom?.Close();
                }
            }
            else
            {
                GlobalVariable.Form.DID_Bottom?.Close();
            }
        }

        #endregion

        #region Button_Click

        private void ControlPanel_Button_Click(object sender, EventArgs e)
        {
            string call = "ControlPanel_Button_Click";
            try
            {
                Button button = sender as Button;
                string buttonName = button.Name.Substring(button.Name.LastIndexOf("_") + 1);

                Log.Write(call, buttonName);

                switch (buttonName)
                {
                    case "Product":
                        if (GlobalVariable.Form.Parameter == null)
                        {
                            GlobalVariable.Form.Parameter = new frm_Parameter(true);
                            GlobalVariable.Form.Parameter.Show();
                        }
                        else
                        {
                            GlobalVariable.Form.Parameter.BringToFront();
                        }
                        break;
                    case "Alarm":
                        alarmToolStripMenuItem_Click(null, null);
                        break;
                    case "Initialize": // 시스템 초기화
                        bool initialize = false;

                        initialize = GlobalFunction.MessageBox(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_Initialize), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

                        if (initialize)
                        {
                            ControlPanel_Button_Click_Robot(btn_ControlPanel_Robot_Initialize, null);

                            if (GlobalFunction.CheckProcess())
                            {
                                if (string.IsNullOrEmpty(_ID) == false)
                                {
                                    GlobalFunction.DB.MySQL.Query($"UPDATE {e_DB._Order} SET {e_DB_Order.Column02}='{e_Order_Status.오류}' WHERE {e_DB_Order.Column01}='{_ID}'");
                                }

                                _curr_Sequence_Main = e_Sequence_Main.리셋;
                            }
                        }
                        break;
                    case "Clear": // 주문 취소
                        bool clear = false;

                        clear = GlobalFunction.MessageBox(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_Clear), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

                        if (clear)
                        {
                            if (GlobalFunction.CheckOrder())
                            {
                                GlobalFunction.DB.MySQL.Query($"UPDATE {e_DB._Order} SET {e_DB_Order.Column02}='{e_Order_Status.주문취소}' WHERE {e_DB_Order.Column02}='{e_Order_Status.주문}'");
                            }
                        }
                        break;
                    case "Exit":
                        exitToolStripMenuItem_Click(null, null);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Write(call, GlobalFunction.GetString(ex));
            }
        }

        private async void ControlPanel_Button_Click_CoffeeMaker(object sender, EventArgs e)
        {
            string call = "ControlPanel_Button_Click_CoffeeMaker";
            try
            {
                #region Control

                foreach (Control control in GlobalFunction.GetControls(grp_ControlPanel_CoffeeMaker))
                {
                    if (control is Button)
                    {
                        control.BackColor = SystemColors.Control;
                    }

                    control.Enabled = false;
                }

                #endregion

                Button button = sender as Button;
                string buttonName = button.Name.Substring(button.Name.LastIndexOf("_") + 1);

                Log.Write(call, buttonName);

                #region CheckDevice

                if (GlobalDevice.CoffeeMaker.Instance == null)
                {
                    Log.Write(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_CheckDevice_CoffeeMaker_Instance));
                    return;
                }
                else if (GlobalDevice.CoffeeMaker.Status.StatusBase == false)
                {
                    Log.Write(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_CheckDevice_CoffeeMaker_StatusBase));
                    return;
                }

                #endregion

                button.BackColor = Color.Yellow;

                switch (buttonName)
                {
                    case "Start":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.CoffeeMaker.Instance.DoProduct(Convert.ToInt32(num_ControlPanel_CoffeeMaker_KeyId.Value)) ? Color.Lime : Color.Red; });
                        break;
                    case "Rinse":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.CoffeeMaker.Instance.DoRinse() ? Color.Lime : Color.Red; });
                        break;
                    case "Milk":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.CoffeeMaker.Instance.DoProduct(CONST.N_EVERSYS_PRODUCT_ID_HOT_MILK) ? Color.Lime : Color.Red; });
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Write(call, GlobalFunction.GetString(ex));
            }
            finally
            {
                #region Control

                foreach (Control control in GlobalFunction.GetControls(grp_ControlPanel_CoffeeMaker))
                {
                    control.Enabled = true;
                }

                #endregion
            }
        }

        private async void ControlPanel_Button_Click_Robot(object sender, EventArgs e)
        {
            string call = "ControlPanel_Button_Click_Robot";
            try
            {
                #region Control

                foreach (Control control in GlobalFunction.GetControls(grp_ControlPanel_Robot))
                {
                    if (control is Button)
                    {
                        control.BackColor = SystemColors.Control;
                    }

                    control.Enabled = false;
                }

                #endregion

                Button button = sender as Button;
                string buttonName = button.Name.Substring(button.Name.LastIndexOf("_") + 1);

                Log.Write(call, buttonName);

                #region CheckDevice

                if (GlobalDevice.Robot.Instance == null)
                {
                    Log.Write(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_CheckDevice_Robot_Instance));
                    return;
                }
                else if (GlobalDevice.Robot.Status.StatusBase == false)
                {
                    Log.Write(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_CheckDevice_Robot_StatusBase));
                    return;
                }

                #endregion

                #region ON

                switch (buttonName)
                {
                    case "Stop":
                        await Task.Run(delegate { GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Control.Stop); });
                        break;
                    case "Initialize":
                        bool initialize = false;

                        //await Task.Run(delegate { initialize = GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Control.Initialize); });

                        if (initialize)
                        {
                            button.BackColor = Color.Lime;

                            if (GlobalDevice.Robot.Clean)
                            {
                                GlobalDevice.Robot.Clean = false;
                            }

                            if (GlobalDevice.Robot.Maintenance)
                            {
                                GlobalDevice.Robot.Maintenance = false;
                            }
                        }
                        else
                        {
                            button.BackColor = Color.Red;
                        }
                        break;
                    case "Clean":
                        if (GlobalDevice.Robot.Clean)
                        {
                            await Task.Run(delegate { GlobalDevice.Robot.Clean = !GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Control.Clear); });

                            button.BackColor = !GlobalDevice.Robot.Clean ? SystemColors.Control : Color.Red;
                        }
                        else
                        {
                            await Task.Run(delegate { GlobalDevice.Robot.Clean = GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Control.Clean); });

                            button.BackColor = GlobalDevice.Robot.Clean ? Color.Lime : Color.Red;

                            if (GlobalDevice.Robot.Maintenance)
                            {
                                GlobalDevice.Robot.Maintenance = false;
                            }
                        }
                        break;
                    case "Maintenance":
                        if (GlobalDevice.Robot.Maintenance)
                        {
                            await Task.Run(delegate { GlobalDevice.Robot.Maintenance = !GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Control.Clear); });

                            button.BackColor = !GlobalDevice.Robot.Maintenance ? SystemColors.Control : Color.Red;
                        }
                        else
                        {
                            await Task.Run(delegate { GlobalDevice.Robot.Maintenance = GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Control.Maintenance); });

                            button.BackColor = GlobalDevice.Robot.Maintenance ? Color.Lime : Color.Red;

                            if (GlobalDevice.Robot.Clean)
                            {
                                GlobalDevice.Robot.Clean = false;
                            }
                        }
                        break;
                    case "Control":
                        e_Device_Robot_Control control = (e_Device_Robot_Control)Enum.Parse(typeof(e_Device_Robot_Control), cb_ControlPanel_Robot_Control.Text);

                        await Task.Run(delegate { button.BackColor = GlobalDevice.Robot.Instance.SetRobot(control) ? Color.Lime : Color.Red; });
                        break;
                    case "Command":
                        bool ade = chk_ControlPanel_Robot_Ade.Checked;
                        e_Device_Robot_Cup cup = (e_Device_Robot_Cup)Enum.Parse(typeof(e_Device_Robot_Cup), cb_ControlPanel_Robot_Cup.Text);
                        e_Device_Robot_Command command = (e_Device_Robot_Command)Enum.Parse(typeof(e_Device_Robot_Command), cb_ControlPanel_Robot_Command.Text);

                        await Task.Run(delegate { button.BackColor = GlobalDevice.Robot.Instance.SetRobot(command, cup, ade) ? Color.Lime : Color.Red; });
                        break;
                }

                #endregion

                #region Delay

                switch (buttonName)
                {
                    case "Initialize":
                        await Task.Delay(1000 * 5);
                        break;
                }

                #endregion

                #region OFF

                switch (buttonName)
                {
                    case "Initialize":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Control.Clear) ? SystemColors.Control : Color.Red; });
                        break;
                }

                #endregion
            }
            catch (Exception ex)
            {
                Log.Write(call, GlobalFunction.GetString(ex));
            }
            finally
            {
                #region Control

                foreach (Control control in GlobalFunction.GetControls(grp_ControlPanel_Robot))
                {
                    control.Enabled = true;
                }

                #endregion
            }
        }

        private async void ControlPanel_Button_Click_IceMaker(object sender, EventArgs e)
        {
            string call = "ControlPanel_Button_Click_IceMaker";
            try
            {
                #region Control

                foreach (Control control in GlobalFunction.GetControls(grp_ControlPanel_IceMaker))
                {
                    if (control is Button)
                    {
                        control.BackColor = SystemColors.Control;
                    }

                    control.Enabled = false;
                }

                #endregion

                Button button = sender as Button;
                string buttonName = button.Name.Substring(button.Name.LastIndexOf("_") + 1);

                Log.Write(call, buttonName);

                #region CheckDevice

                if (GlobalDevice.IceMaker.Instance == null)
                {
                    Log.Write(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_CheckDevice_IceMaker_Instance));
                    return;
                }
                else if (GlobalDevice.IceMaker.Status.StatusBase == false)
                {
                    Log.Write(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_CheckDevice_IceMaker_StatusBase));
                    return;
                }

                #endregion

                button.BackColor = Color.Yellow;

                switch (buttonName)
                {
                    case "Start":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.IceMaker.Instance.SetTime(num_ControlPanel_IceMaker_Time_Ice.Value.ToString(), num_ControlPanel_IceMaker_Time_Water.Value.ToString()) ? Color.Lime : Color.Red; });
                        break;
                }

                if (buttonName == "Start")
                {
                    if (button.BackColor == Color.Lime)
                    {
                        switch ((e_Device_IceMaker)Enum.Parse(typeof(e_Device_IceMaker), GlobalVariable.Parameter[(int)e_Parameter.IceMaker][CONST.S_KEY][(int)e_Parameter_IceMaker.Device]))
                        {
                            case e_Device_IceMaker.ICETRO:
                                await Task.Run(delegate { button.BackColor = GlobalDevice.Controller1.Instance.SetSol(e_Device_Controller1_Sol.Sol6, e_Device_Controller1_Sol_Trigger.ON) ? Color.Yellow : Color.Red; });

                                await Task.Delay(1000 * 1);

                                await Task.Run(delegate { button.BackColor = GlobalDevice.Controller1.Instance.SetSol(e_Device_Controller1_Sol.Sol6, e_Device_Controller1_Sol_Trigger.OFF) ? Color.Lime : Color.Red; });
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(call, GlobalFunction.GetString(ex));
            }
            finally
            {
                #region Control

                foreach (Control control in GlobalFunction.GetControls(grp_ControlPanel_IceMaker))
                {
                    control.Enabled = true;
                }

                #endregion
            }
        }

        private async void ControlPanel_Button_Click_Controller1_Door1(object sender, EventArgs e)
        {
            string call = "ControlPanel_Button_Click_Controller1_Door1";
            try
            {
                #region Control

                foreach (Control control in GlobalFunction.GetControls(grp_ControlPanel_Controller1_Door1))
                {
                    if (control is Button)
                    {
                        control.BackColor = SystemColors.Control;
                    }

                    control.Enabled = false;
                }

                #endregion

                e_Door door = e_Door.Door1;

                Button button = sender as Button;
                string buttonName = button.Name.Substring(button.Name.LastIndexOf("_") + 1);

                Log.Write(call, buttonName);

                #region CheckDevice

                if (buttonName != "Lock")
                {
                    if (GlobalDevice.Controller1.Instance == null)
                    {
                        Log.Write(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_CheckDevice_Controller1_Instance));
                        return;
                    }
                    else if (GlobalDevice.Controller1.Status.StatusBase == false)
                    {
                        Log.Write(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_CheckDevice_Controller1_StatusBase));
                        return;
                    }
                }

                #endregion

                #region Interlock

                bool process = false;

                switch (buttonName)
                {
                    case "Close":
                        if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B4_Door1_Close] == false)
                        {
                            process = true;
                        }
                        break;
                    case "Open":
                        if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B0_Door1_Open] == false)
                        {
                            process = true;
                        }
                        break;
                }

                if (buttonName != "Lock")
                {
                    if (process == false)
                    {
                        switch (buttonName)
                        {
                            case "Close":
                                Log.Write(call, $"{door} {GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_Interlock_Door_Close)}");
                                break;
                            case "Open":
                                Log.Write(call, $"{door} {GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_Interlock_Door_Open)}");
                                break;
                        }
                        return;
                    }
                }

                #endregion

                button.BackColor = Color.Yellow;

                switch (buttonName)
                {
                    case "Close":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.Controller1.Instance.SetDoor(door, e_Device_Controller1_Door_Command.Close) ? Color.Lime : Color.Red; });
                        break;
                    case "Open":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.Controller1.Instance.SetDoor(door, e_Device_Controller1_Door_Command.Open) ? Color.Lime : Color.Red; });
                        break;
                    case "Lock":
                        e_Door_Lock @lock = GlobalFunction.Door.GetLock(door);

                        if (@lock == e_Door_Lock.Lock)
                        {
                            string ID = GlobalFunction.Door.GetID(door);

                            if (string.IsNullOrEmpty(ID) == false)
                            {
                                GlobalFunction.DB.MySQL.Query($"UPDATE {e_DB._Order} SET {e_DB_Order.Column02}='{e_Order_Status.수동배출}' WHERE {e_DB_Order.Column01}='{ID}'");
                            }
                        }

                        GlobalFunction.Door.Clear(door);

                        if (@lock == e_Door_Lock.Unlock)
                        {
                            GlobalFunction.Door.SetLock(door, e_Door_Lock.Lock);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Write(call, GlobalFunction.GetString(ex));
            }
            finally
            {
                #region Control

                foreach (Control control in GlobalFunction.GetControls(grp_ControlPanel_Controller1_Door1))
                {
                    control.Enabled = true;
                }

                #endregion
            }
        }

        private async void ControlPanel_Button_Click_Controller1_Door2(object sender, EventArgs e)
        {
            string call = "ControlPanel_Button_Click_Controller1_Door2";
            try
            {
                #region Control

                foreach (Control control in GlobalFunction.GetControls(grp_ControlPanel_Controller1_Door2))
                {
                    if (control is Button)
                    {
                        control.BackColor = SystemColors.Control;
                    }

                    control.Enabled = false;
                }

                #endregion

                e_Door door = e_Door.Door2;

                Button button = sender as Button;
                string buttonName = button.Name.Substring(button.Name.LastIndexOf("_") + 1);

                Log.Write(call, buttonName);

                #region CheckDevice

                if (buttonName != "Lock")
                {
                    if (GlobalDevice.Controller1.Instance == null)
                    {
                        Log.Write(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_CheckDevice_Controller1_Instance));
                        return;
                    }
                    else if (GlobalDevice.Controller1.Status.StatusBase == false)
                    {
                        Log.Write(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_CheckDevice_Controller1_StatusBase));
                        return;
                    }
                }

                #endregion

                #region Interlock

                bool process = false;

                switch (buttonName)
                {
                    case "Close":
                        if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B5_Door2_Close] == false)
                        {
                            process = true;
                        }
                        break;
                    case "Open":
                        if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B1_Door2_Open] == false)
                        {
                            process = true;
                        }
                        break;
                }

                if (buttonName != "Lock")
                {
                    if (process == false)
                    {
                        switch (buttonName)
                        {
                            case "Close":
                                Log.Write(call, $"{door} {GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_Interlock_Door_Close)}");
                                break;
                            case "Open":
                                Log.Write(call, $"{door} {GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_Interlock_Door_Open)}");
                                break;
                        }
                        return;
                    }
                }

                #endregion

                button.BackColor = Color.Yellow;

                switch (buttonName)
                {
                    case "Close":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.Controller1.Instance.SetDoor(door, e_Device_Controller1_Door_Command.Close) ? Color.Lime : Color.Red; });
                        break;
                    case "Open":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.Controller1.Instance.SetDoor(door, e_Device_Controller1_Door_Command.Open) ? Color.Lime : Color.Red; });
                        break;
                    case "Lock":
                        e_Door_Lock @lock = GlobalFunction.Door.GetLock(door);

                        if (@lock == e_Door_Lock.Lock)
                        {
                            string ID = GlobalFunction.Door.GetID(door);

                            if (string.IsNullOrEmpty(ID) == false)
                            {
                                GlobalFunction.DB.MySQL.Query($"UPDATE {e_DB._Order} SET {e_DB_Order.Column02}='{e_Order_Status.수동배출}' WHERE {e_DB_Order.Column01}='{ID}'");
                            }
                        }

                        GlobalFunction.Door.Clear(door);

                        if (@lock == e_Door_Lock.Unlock)
                        {
                            GlobalFunction.Door.SetLock(door, e_Door_Lock.Lock);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Write(call, GlobalFunction.GetString(ex));
            }
            finally
            {
                #region Control

                foreach (Control control in GlobalFunction.GetControls(grp_ControlPanel_Controller1_Door2))
                {
                    control.Enabled = true;
                }

                #endregion
            }
        }

        private async void ControlPanel_Button_Click_Controller1_Door3(object sender, EventArgs e)
        {
            string call = "ControlPanel_Button_Click_Controller1_Door3";
            try
            {
                #region Control

                foreach (Control control in GlobalFunction.GetControls(grp_ControlPanel_Controller1_Door3))
                {
                    if (control is Button)
                    {
                        control.BackColor = SystemColors.Control;
                    }

                    control.Enabled = false;
                }

                #endregion

                e_Door door = e_Door.Door3;

                Button button = sender as Button;
                string buttonName = button.Name.Substring(button.Name.LastIndexOf("_") + 1);

                Log.Write(call, buttonName);

                #region CheckDevice

                if (buttonName != "Lock")
                {
                    if (GlobalDevice.Controller1.Instance == null)
                    {
                        Log.Write(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_CheckDevice_Controller1_Instance));
                        return;
                    }
                    else if (GlobalDevice.Controller1.Status.StatusBase == false)
                    {
                        Log.Write(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_CheckDevice_Controller1_StatusBase));
                        return;
                    }
                }

                #endregion

                #region Interlock

                bool process = false;

                switch (buttonName)
                {
                    case "Close":
                        if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B6_Door3_Close] == false)
                        {
                            process = true;
                        }
                        break;
                    case "Open":
                        if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B2_Door3_Open] == false)
                        {
                            process = true;
                        }
                        break;
                }

                if (buttonName != "Lock")
                {
                    if (process == false)
                    {
                        switch (buttonName)
                        {
                            case "Close":
                                Log.Write(call, $"{door} {GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_Interlock_Door_Close)}");
                                break;
                            case "Open":
                                Log.Write(call, $"{door} {GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_Interlock_Door_Open)}");
                                break;
                        }
                        return;
                    }
                }

                #endregion

                button.BackColor = Color.Yellow;

                switch (buttonName)
                {
                    case "Close":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.Controller1.Instance.SetDoor(door, e_Device_Controller1_Door_Command.Close) ? Color.Lime : Color.Red; });
                        break;
                    case "Open":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.Controller1.Instance.SetDoor(door, e_Device_Controller1_Door_Command.Open) ? Color.Lime : Color.Red; });
                        break;
                    case "Lock":
                        e_Door_Lock @lock = GlobalFunction.Door.GetLock(door);

                        if (@lock == e_Door_Lock.Lock)
                        {
                            string ID = GlobalFunction.Door.GetID(door);

                            if (string.IsNullOrEmpty(ID) == false)
                            {
                                GlobalFunction.DB.MySQL.Query($"UPDATE {e_DB._Order} SET {e_DB_Order.Column02}='{e_Order_Status.수동배출}' WHERE {e_DB_Order.Column01}='{ID}'");
                            }
                        }

                        GlobalFunction.Door.Clear(door);

                        if (@lock == e_Door_Lock.Unlock)
                        {
                            GlobalFunction.Door.SetLock(door, e_Door_Lock.Lock);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Write(call, GlobalFunction.GetString(ex));
            }
            finally
            {
                #region Control

                foreach (Control control in GlobalFunction.GetControls(grp_ControlPanel_Controller1_Door3))
                {
                    control.Enabled = true;
                }

                #endregion
            }
        }

        private async void ControlPanel_Button_Click_Controller1_Door4(object sender, EventArgs e)
        {
            string call = "ControlPanel_Button_Click_Controller1_Door4";
            try
            {
                #region Control

                foreach (Control control in GlobalFunction.GetControls(grp_ControlPanel_Controller1_Door4))
                {
                    if (control is Button)
                    {
                        control.BackColor = SystemColors.Control;
                    }

                    control.Enabled = false;
                }

                #endregion

                e_Door door = e_Door.Door4;

                Button button = sender as Button;
                string buttonName = button.Name.Substring(button.Name.LastIndexOf("_") + 1);

                Log.Write(call, buttonName);

                #region CheckDevice

                if (buttonName != "Lock")
                {
                    if (GlobalDevice.Controller1.Instance == null)
                    {
                        Log.Write(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_CheckDevice_Controller1_Instance));
                        return;
                    }
                    else if (GlobalDevice.Controller1.Status.StatusBase == false)
                    {
                        Log.Write(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_CheckDevice_Controller1_StatusBase));
                        return;
                    }
                }

                #endregion

                #region Interlock

                bool process = false;

                switch (buttonName)
                {
                    case "Close":
                        if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B7_Door4_Close] == false)
                        {
                            process = true;
                        }
                        break;
                    case "Open":
                        if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B3_Door4_Open] == false)
                        {
                            process = true;
                        }
                        break;
                    case "Home":
                        if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B2_LM_Up] == false)
                        {
                            process = true;
                        }
                        break;
                    case "Ready":
                        if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B2_LM_Up] == false)
                        {
                            process = true;
                        }
                        break;
                    case "Check":
                        if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B2_LM_Up])
                        {
                            process = true;
                        }
                        break;
                }

                if (buttonName != "Lock")
                {
                    if (process == false)
                    {
                        switch (buttonName)
                        {
                            case "Close":
                                Log.Write(call, $"{door} {GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_Interlock_Door_Close)}");
                                break;
                            case "Open":
                                Log.Write(call, $"{door} {GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_Interlock_Door_Open)}");
                                break;
                            case "Home":
                                Log.Write(call, $"{door} {GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_Interlock_Door_Tumbler_Home)}");
                                break;
                            case "Ready":
                                Log.Write(call, $"{door} {GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_Interlock_Door_Tumbler_Ready)}");
                                break;
                            case "Check":
                                Log.Write(call, $"{door} {GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_Interlock_Door_Tumbler_Check)}");
                                break;
                        }
                        return;
                    }
                }

                #endregion

                button.BackColor = Color.Yellow;

                switch (buttonName)
                {
                    case "Close":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.Controller1.Instance.SetDoor(door, e_Device_Controller1_Door_Command.Close) ? Color.Lime : Color.Red; });
                        break;
                    case "Open":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.Controller1.Instance.SetDoor(door, e_Device_Controller1_Door_Command.Open) ? Color.Lime : Color.Red; });
                        break;
                    case "Lock":
                        e_Door_Lock @lock = GlobalFunction.Door.GetLock(door);

                        if (@lock == e_Door_Lock.Lock)
                        {
                            string ID = GlobalFunction.Door.GetID(door);

                            if (string.IsNullOrEmpty(ID) == false)
                            {
                                GlobalFunction.DB.MySQL.Query($"UPDATE {e_DB._Order} SET {e_DB_Order.Column02}='{e_Order_Status.수동배출}' WHERE {e_DB_Order.Column01}='{ID}'");
                            }
                        }

                        GlobalFunction.Door.Clear(door);

                        if (@lock == e_Door_Lock.Unlock)
                        {
                            GlobalFunction.Door.SetLock(door, e_Door_Lock.Lock);
                        }
                        break;
                    case "Home":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.Controller1.Instance.SetDoor(door, e_Device_Controller1_Door_Command.Tumbler_Home) ? Color.Lime : Color.Red; });
                        break;
                    case "Ready":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.Controller1.Instance.SetDoor(door, e_Device_Controller1_Door_Command.Tumbler_Ready) ? Color.Lime : Color.Red; });
                        break;
                    case "Check":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.Controller1.Instance.SetDoor(door, e_Device_Controller1_Door_Command.Tumbler_Check) ? Color.Lime : Color.Red; });
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Write(call, GlobalFunction.GetString(ex));
            }
            finally
            {
                #region Control

                foreach (Control control in GlobalFunction.GetControls(grp_ControlPanel_Controller1_Door4))
                {
                    control.Enabled = true;
                }

                #endregion
            }
        }

        private async void ControlPanel_Button_Click_Controller2_Turn(object sender, EventArgs e)
        {
            string call = "ControlPanel_Button_Click_Controller2_Turn";
            try
            {
                #region Control

                foreach (Control control in GlobalFunction.GetControls(grp_ControlPanel_Controller2))
                {
                    if (control is Button)
                    {
                        control.BackColor = SystemColors.Control;
                    }

                    control.Enabled = false;
                }

                #endregion

                Button button = sender as Button;
                string buttonName = button.Name.Substring(button.Name.LastIndexOf("_") + 1);

                Log.Write(call, buttonName);

                #region CheckDevice

                if (GlobalDevice.Controller2.Instance == null)
                {
                    Log.Write(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_CheckDevice_Controller2_Instance));
                    return;
                }
                else if (GlobalDevice.Controller2.Status.StatusBase == false)
                {
                    Log.Write(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_CheckDevice_Controller2_StatusBase));
                    return;
                }

                #endregion

                #region Interlock

                bool process = false;

                switch (buttonName)
                {
                    case "Home":
                    case "Move":
                        if (GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D2B0_펌핑_Home])
                        {
                            process = true;
                        }
                        break;
                }

                if (process == false)
                {
                    switch (buttonName)
                    {
                        case "Home":
                        case "Move":
                            Log.Write(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_Interlock_Motor_Turn));
                            break;
                    }
                    return;
                }

                #endregion

                button.BackColor = Color.Yellow;

                switch (buttonName)
                {
                    case "Home":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.Controller2.Instance.SetMotor(e_Device_Controller2_Motor_Command.회전_Home) ? Color.Lime : Color.Red; });
                        break;
                    case "Move":
                        e_Device_Controller2_Sauce sauce = (e_Device_Controller2_Sauce)Enum.Parse(typeof(e_Device_Controller2_Sauce), cb_ControlPanel_Controller2_Turn_Sauce.Text);

                        await Task.Run(delegate { button.BackColor = GlobalDevice.Controller2.Instance.SetMotor(e_Device_Controller2_Motor_Command.회전_Move, sauce) ? Color.Lime : Color.Red; });
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Write(call, GlobalFunction.GetString(ex));
            }
            finally
            {
                #region Control

                foreach (Control control in GlobalFunction.GetControls(grp_ControlPanel_Controller2))
                {
                    control.Enabled = true;
                }

                #endregion
            }
        }

        private async void ControlPanel_Button_Click_Controller2_Sauce(object sender, EventArgs e)
        {
            string call = "ControlPanel_Button_Click_Controller2_Sauce";
            try
            {
                #region Control

                foreach (Control control in GlobalFunction.GetControls(grp_ControlPanel_Controller2))
                {
                    if (control is Button)
                    {
                        control.BackColor = SystemColors.Control;
                    }

                    control.Enabled = false;
                }

                #endregion

                Button button = sender as Button;
                string buttonName = button.Name.Substring(button.Name.LastIndexOf("_") + 1);

                Log.Write(call, buttonName);

                #region CheckDevice

                if (GlobalDevice.Controller2.Instance == null)
                {
                    Log.Write(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_CheckDevice_Controller2_Instance));
                    return;
                }
                else if (GlobalDevice.Controller2.Status.StatusBase == false)
                {
                    Log.Write(call, GlobalFunction.GetMessage(e_Message.ControlPanel_Button_Click_CheckDevice_Controller2_StatusBase));
                    return;
                }

                #endregion

                #region Interlock

                bool process = false;

                switch (buttonName)
                {
                    case "Home":
                        break;
                    case "Push":
                        break;
                    case "Mix":
                        break;
                    case "Both":
                        break;
                }

                process = true;

                if (process == false)
                {
                    switch (buttonName)
                    {
                        case "Home":
                            break;
                        case "Push":
                            break;
                        case "Mix":
                            break;
                        case "Both":
                            break;
                    }
                    return;
                }

                #endregion

                button.BackColor = Color.Yellow;

                switch (buttonName)
                {
                    case "Home":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.Controller2.Instance.SetMotor(e_Device_Controller2_Motor_Command.펌핑_Home) ? Color.Lime : Color.Red; });
                        break;
                    case "Push":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.Controller2.Instance.SetMotor(e_Device_Controller2_Motor_Command.펌핑_Push, num_ControlPanel_Controller2_Sauce_Count_Push.Value.ToString()) ? Color.Lime : Color.Red; });
                        break;
                    case "Mix":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.Controller2.Instance.SetMotor(e_Device_Controller2_Motor_Command.믹서_회전, num_ControlPanel_Controller2_Sauce_Count_Mix.Value.ToString()) ? Color.Lime : Color.Red; });
                        break;
                    case "Both":
                        await Task.Run(delegate { button.BackColor = GlobalDevice.Controller2.Instance.SetMotor(e_Device_Controller2_Motor_Command.펌핑_Push, num_ControlPanel_Controller2_Sauce_Count_Push.Value.ToString(), num_ControlPanel_Controller2_Sauce_Count_Mix.Value.ToString()) ? Color.Lime : Color.Red; });
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Write(call, GlobalFunction.GetString(ex));
            }
            finally
            {
                #region Control

                foreach (Control control in GlobalFunction.GetControls(grp_ControlPanel_Controller2))
                {
                    control.Enabled = true;
                }

                #endregion
            }
        }

        #endregion
    }
}
