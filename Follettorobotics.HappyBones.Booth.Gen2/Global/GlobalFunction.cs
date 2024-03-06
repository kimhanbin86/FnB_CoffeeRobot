using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using Library.Log;

using MySql.Data.MySqlClient;

namespace Follettorobotics.HappyBones.Booth.Gen2
{
    public static class GlobalFunction
    {
        private static bool BackupParameter(e_Parameter parameter, string sourceFileName)
        {
            bool result = false;
            try
            {
                DateTime now = DateTime.Now;

                string path = $@"{GlobalVariable.Directory.Backup}\{now:yyyy-MM-dd}";

                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                }

                string destFileName = $@"{path}\{now:HHmmss}_{parameter}.{CONST.S_CSV}";

                File.Copy(sourceFileName, destFileName, true);

                result = true;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            if (result == false)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, $"{parameter} Parameter Backup NG");
            }
            return result;
        }

        public static bool CheckProcess()
        {
            bool result = false;
            try
            {
                result = GlobalVariable.Form.ControlPanel?._curr_Sequence_Main > frm_ControlPanel.e_Sequence_Main.대기;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return result;
        }

        public static bool CheckProcessOrOrder()
        {
            bool result = false;
            try
            {
                result = CheckProcess() || CheckOrder();
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return result;
        }

        public static bool CheckOrder()
        {
            bool result = false;
            try
            {
                result = GetOrder().Rows.Count > 0;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return result;
        }

        public static bool CheckProcess(string processName)
        {
            bool result = false;
            try
            {
                result = Process.GetProcessesByName(processName).Length < 2;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return result;
        }

        public static bool CheckProductDevice(string productKey, bool check, ref string NG)
        {
            bool result = true;
            try
            {
                NG = string.Empty;

                #region CoffeeMaker

                if (result)
                {
                    bool NBox = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B7_NBox];
                    int Coffee_ID = 0;

                    bool Clean = GlobalDevice.CoffeeMaker.Clean;

                    if (int.TryParse(GlobalVariable.Parameter[(int)e_Parameter.Product][productKey][(int)e_Parameter_Product.Coffee_ID], out Coffee_ID))
                    {
                        result &= NBox;
                        result &= Coffee_ID > 0;

                        if (check)
                        {
                            result &= GlobalDevice.CoffeeMaker.IgnoreStatus;
                        }
                        else
                        {
                            result &= GlobalDevice.CoffeeMaker.Status.Status;
                        }

                        result &= !Clean;
                    }

                    if (result == false)
                    {
                        if (NBox == false)
                        {
                            NG = GetAlarmText(GetAlarmKey(e_Alarm.NBOX_ALARM));
                        }
                        else if (Coffee_ID <= 0)
                        {
                            NG = "Please check the product parameter [Coffee ID] value";
                        }
                        else if (Clean)
                        {
                            NG = "CoffeeMaker Clean";
                        }
                        else
                        {
                            NG = GetAlarmText(GetAlarmKey(e_Alarm.ERROR_DEVICE_STATUS_COFFEE_MAKER));
                        }
                    }
                }

                #endregion

                #region Controller1

                if (result)
                {
                    result &= GlobalDevice.Controller1.Status.Status;

                    if (result == false)
                    {
                        NG = GetAlarmText(GetAlarmKey(e_Alarm.ERROR_DEVICE_STATUS_CONTROLLER1));
                    }
                }

                #endregion

                #region Controller2

                e_ComboBox_Sauce_Type Sauce_Type = e_ComboBox_Sauce_Type.Unused;

                if (result)
                {
                    try
                    {
                        Sauce_Type = (e_ComboBox_Sauce_Type)Enum.Parse(typeof(e_ComboBox_Sauce_Type), GlobalVariable.Parameter[(int)e_Parameter.Product][productKey][(int)e_Parameter_Product.Sauce_Type]);
                    }
                    catch
                    {
                        result = false;

                        NG = "Please check the product parameter [Sauce Type] value";
                    }
                }

                if (result)
                {
                    if (Sauce_Type != e_ComboBox_Sauce_Type.Unused)
                    {
                        result &= GlobalDevice.Controller2.Status.Status;
                    }

                    if (result == false)
                    {
                        NG = GetAlarmText(GetAlarmKey(e_Alarm.ERROR_DEVICE_STATUS_CONTROLLER2));
                    }
                }

                #endregion

                #region IceMaker

                if (result)
                {
                    if (double.TryParse(GlobalVariable.Parameter[(int)e_Parameter.Product][productKey][(int)e_Parameter_Product.Ice_Time  ], out double Ice_Time  ) ||
                        double.TryParse(GlobalVariable.Parameter[(int)e_Parameter.Product][productKey][(int)e_Parameter_Product.Water_Time], out double Water_Time)
                       )
                    {
                        result &= GlobalDevice.IceMaker.Status.Status;
                    }

                    if (result == false)
                    {
                        NG = GetAlarmText(GetAlarmKey(e_Alarm.ERROR_DEVICE_STATUS_ICE_MAKER));
                    }
                }

                #endregion

                #region Robot

                if (result)
                {
                    result &= GlobalDevice.Robot.Status.Status;

                    if (result == false)
                    {
                        NG = GetAlarmText(GetAlarmKey(e_Alarm.ERROR_DEVICE_STATUS_ROBOT));
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return result;
        }

        public static bool CheckProductSensor(string productKey, ref string NG)
        {
            bool result = true;
            try
            {
                NG = string.Empty;

                #region Cup

                e_ComboBox_Cup Cup = e_ComboBox_Cup.Hot;

                if (result)
                {
                    try
                    {
                        Cup = (e_ComboBox_Cup)Enum.Parse(typeof(e_ComboBox_Cup), GlobalVariable.Parameter[(int)e_Parameter.Product][productKey][(int)e_Parameter_Product.Cup]);
                    }
                    catch
                    {
                        result = false;

                        NG = "Please check the product parameter [Cup] value";
                    }
                }

                if (result)
                {
                    switch (Cup)
                    {
                        case e_ComboBox_Cup.Hot: result &= GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B0_Cup1] || GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B1_Cup2]; break;
                        case e_ComboBox_Cup.Ice: result &= GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B2_Cup3] || GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B3_Cup4]; break;
                    }

                    if (result == false)
                    {
                        switch (Cup)
                        {
                            case e_ComboBox_Cup.Hot: NG = GetAlarmText(GetAlarmKey(e_Alarm.CUP1_EMPTY)) + ", " + GetAlarmText(GetAlarmKey(e_Alarm.CUP2_EMPTY)); break;
                            case e_ComboBox_Cup.Ice: NG = GetAlarmText(GetAlarmKey(e_Alarm.CUP3_EMPTY)) + ", " + GetAlarmText(GetAlarmKey(e_Alarm.CUP4_EMPTY)); break;
                        }
                    }
                }

                #endregion

                #region Milk

                e_ComboBox_Use Milk = e_ComboBox_Use.Unused;

                if (result)
                {
                    try
                    {
                        Milk = (e_ComboBox_Use)Enum.Parse(typeof(e_ComboBox_Use), GlobalVariable.Parameter[(int)e_Parameter.Product][productKey][(int)e_Parameter_Product.Milk]);
                    }
                    catch
                    {
                        result = false;

                        NG = "Please check the product parameter [Milk] value";
                    }
                }

                if (result)
                {
                    if (Milk == e_ComboBox_Use.Use)
                    {
                        result &= GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B6_Milk];
                    }

                    if (result == false)
                    {
                        NG = GetAlarmText(GetAlarmKey(e_Alarm.MILK_EMPTY));
                    }
                }

                #endregion

                #region Sauce

                e_Device_Controller2_Sauce Sauce_No = e_Device_Controller2_Sauce.Unused;

                if (result)
                {
                    if ((e_ComboBox_Sauce_Type)Enum.Parse(typeof(e_ComboBox_Sauce_Type), GlobalVariable.Parameter[(int)e_Parameter.Product][productKey][(int)e_Parameter_Product.Sauce_Type]) != e_ComboBox_Sauce_Type.Unused)
                    {
                        try
                        {
                            Sauce_No = (e_Device_Controller2_Sauce)Enum.Parse(typeof(e_Device_Controller2_Sauce), GlobalVariable.Parameter[(int)e_Parameter.Product][productKey][(int)e_Parameter_Product.Sauce_No]);
                        }
                        catch
                        {
                            result = false;

                            NG = "Please check the product parameter [Sauce No] value";
                        }
                    }
                }

                if (result)
                {
                    if ((e_ComboBox_Sauce_Type)Enum.Parse(typeof(e_ComboBox_Sauce_Type), GlobalVariable.Parameter[(int)e_Parameter.Product][productKey][(int)e_Parameter_Product.Sauce_Type]) != e_ComboBox_Sauce_Type.Unused)
                    {
                        switch (Sauce_No)
                        {
                            case e_Device_Controller2_Sauce.Unused: result = false; break;

                            case e_Device_Controller2_Sauce.Sauce1: result &= GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D1B3_Sauce1]; break;
                            case e_Device_Controller2_Sauce.Sauce2: result &= GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D1B2_Sauce2]; break;
                            case e_Device_Controller2_Sauce.Sauce3: result &= GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D1B1_Sauce3]; break;
                            case e_Device_Controller2_Sauce.Sauce4: result &= GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D1B0_Sauce4]; break;
                            case e_Device_Controller2_Sauce.Sauce5: result &= GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D2B3_Sauce5]; break;
                            case e_Device_Controller2_Sauce.Sauce6: result &= GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D2B2_Sauce6]; break;
                        }
                    }

                    if (result == false)
                    {
                        switch (Sauce_No)
                        {
                            case e_Device_Controller2_Sauce.Unused: NG = "Please check the product parameter [Sauce No] value"; break;

                            case e_Device_Controller2_Sauce.Sauce1: NG = GetAlarmText(GetAlarmKey(e_Alarm.SAUCE1_EMPTY)); break;
                            case e_Device_Controller2_Sauce.Sauce2: NG = GetAlarmText(GetAlarmKey(e_Alarm.SAUCE2_EMPTY)); break;
                            case e_Device_Controller2_Sauce.Sauce3: NG = GetAlarmText(GetAlarmKey(e_Alarm.SAUCE3_EMPTY)); break;
                            case e_Device_Controller2_Sauce.Sauce4: NG = GetAlarmText(GetAlarmKey(e_Alarm.SAUCE4_EMPTY)); break;
                            case e_Device_Controller2_Sauce.Sauce5: NG = GetAlarmText(GetAlarmKey(e_Alarm.SAUCE5_EMPTY)); break;
                            case e_Device_Controller2_Sauce.Sauce6: NG = GetAlarmText(GetAlarmKey(e_Alarm.SAUCE6_EMPTY)); break;
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return result;
        }

        public static void DoubleBuffered(object obj, object value)
        {
            try
            {
                Type type = obj.GetType();
                PropertyInfo property = type.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                property.SetValue(obj, value);
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
        }

        public static string GetAlarmKey(e_Alarm alarm)
        {
            string result = string.Empty;
            try
            {
                foreach (string key in GlobalVariable.Parameter[(int)e_Parameter.Alarm].Keys)
                {
                    if (GlobalVariable.Parameter[(int)e_Parameter.Alarm][key][(int)e_Parameter_Alarm.Alarm] == alarm.ToString())
                    {
                        result = key;

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return result;
        }

        public static string GetAlarmText(string key)
        {
            string result = string.Empty;
            try
            {
                switch (GetLanguage())
                {
                    case e_ComboBox_Language.en: result = GlobalVariable.Parameter[(int)e_Parameter.Alarm][key][(int)e_Parameter_Alarm.en_Text]; break;
                    case e_ComboBox_Language.ko: result = GlobalVariable.Parameter[(int)e_Parameter.Alarm][key][(int)e_Parameter_Alarm.ko_Text]; break;
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return result;
        }
        public static string GetAlarmAction(string key)
        {
            string result = string.Empty;
            try
            {
                switch (GetLanguage())
                {
                    case e_ComboBox_Language.en: result = GlobalVariable.Parameter[(int)e_Parameter.Alarm][key][(int)e_Parameter_Alarm.en_Action]; break;
                    case e_ComboBox_Language.ko: result = GlobalVariable.Parameter[(int)e_Parameter.Alarm][key][(int)e_Parameter_Alarm.ko_Action]; break;
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return result;
        }

        public static Control[] GetControls(Control container)
        {
            List<Control> controls = new List<Control>();
            try
            {
                foreach (Control control in container.Controls)
                {
                    controls.Add(control);

                    if (control.Controls.Count > 0)
                    {
                        controls.AddRange(GetControls(control));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return controls.ToArray();
        }

        public static string GetDateTimeString(DateTime value)
        {
            string result = string.Empty;
            try
            {
                result = $"{value:yyyy-MM-dd HH:mm:ss.fff}";
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return result;
        }

        public static bool GetEnabled(e_Parameter parameter, string key = "")
        {
            string result = string.Empty;
            try
            {
                // Enabled
                switch (parameter)
                {
                    case e_Parameter.DID_Bottom:
                        result = GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Enabled];
                        break;
                    case e_Parameter.Door:
                        result = GlobalVariable.Parameter[(int)e_Parameter.Door][CONST.S_KEY][(int)e_Parameter_Door.Enabled];
                        break;
                    case e_Parameter.CoffeeMaker:
                        result = GlobalVariable.Parameter[(int)e_Parameter.CoffeeMaker][CONST.S_KEY][(int)e_Parameter_CoffeeMaker.Enabled];
                        break;
                    case e_Parameter.Controller1:
                        result = GlobalVariable.Parameter[(int)e_Parameter.Controller1][CONST.S_KEY][(int)e_Parameter_Controller1.Enabled];
                        break;
                    case e_Parameter.Controller2:
                        result = GlobalVariable.Parameter[(int)e_Parameter.Controller2][CONST.S_KEY][(int)e_Parameter_Controller2.Enabled];
                        break;
                    case e_Parameter.IceMaker:
                        result = GlobalVariable.Parameter[(int)e_Parameter.IceMaker][CONST.S_KEY][(int)e_Parameter_IceMaker.Enabled];
                        break;
                    case e_Parameter.Robot:
                        result = GlobalVariable.Parameter[(int)e_Parameter.Robot][CONST.S_KEY][(int)e_Parameter_Robot.Enabled];
                        break;
                    case e_Parameter.Barcode:
                        result = GlobalVariable.Parameter[(int)e_Parameter.Barcode][CONST.S_KEY][(int)e_Parameter_Barcode.Enabled];
                        break;
                    case e_Parameter.DID:
                        result = GlobalVariable.Parameter[(int)e_Parameter.DID][CONST.S_KEY][(int)e_Parameter_DID.Enabled];
                        break;
                    case e_Parameter.Kiosk:
                        result = GlobalVariable.Parameter[(int)e_Parameter.Kiosk][CONST.S_KEY][(int)e_Parameter_Kiosk.Enabled];
                        break;
                    case e_Parameter.Remote:
                        result = GlobalVariable.Parameter[(int)e_Parameter.Remote][CONST.S_KEY][(int)e_Parameter_Remote.Enabled];
                        break;
                    case e_Parameter.Alarm:
                        result = GlobalVariable.Parameter[(int)e_Parameter.Alarm][key][(int)e_Parameter_Alarm.Enabled];
                        break;
                }
            }
            catch
            {
            }
            return result == e_ComboBox_Use.Use.ToString();
        }

        public static e_ComboBox_Language GetLanguage()
        {
            e_ComboBox_Language result = e_ComboBox_Language.en;
            try
            {
                result = (e_ComboBox_Language)Enum.Parse(typeof(e_ComboBox_Language), GlobalVariable.Parameter[(int)e_Parameter.Booth][CONST.S_KEY][(int)e_Parameter_Booth.Language]);
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return result;
        }

        public static string GetMessage(e_Message message)
        {
            string result = string.Empty;
            try
            {
                // e_Message
                switch (message)
                {
                    case e_Message.ControlPanel_Button_Click_Initialize:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "The system is initialized. Do you want to proceed?"; break;
                            case e_ComboBox_Language.ko: result = "시스템이 초기화됩니다. 진행하시겠습니까?"; break;
                        }
                        break;
                    case e_Message.ControlPanel_Button_Click_Clear:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "The entire order will be cancelled. Do you want to proceed?"; break;
                            case e_ComboBox_Language.ko: result = "전체 주문이 취소됩니다. 진행하시겠습니까?"; break;
                        }
                        break;
                    case e_Message.ControlPanel_Button_Click_CheckDevice_CoffeeMaker_Instance:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "Coffee machine initialization error"; break;
                            case e_ComboBox_Language.ko: result = "커피머신 초기화 오류"; break;
                        }
                        break;
                    case e_Message.ControlPanel_Button_Click_CheckDevice_CoffeeMaker_StatusBase:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "Coffee machine communication error"; break;
                            case e_ComboBox_Language.ko: result = "커피머신 통신 오류"; break;
                        }
                        break;
                    case e_Message.ControlPanel_Button_Click_CheckDevice_Controller1_Instance:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "Controller 1 initialization error"; break;
                            case e_ComboBox_Language.ko: result = "컨트롤러1 초기화 오류"; break;
                        }
                        break;
                    case e_Message.ControlPanel_Button_Click_CheckDevice_Controller1_StatusBase:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "Controller 1 communication error"; break;
                            case e_ComboBox_Language.ko: result = "컨트롤러1 통신 오류"; break;
                        }
                        break;
                    case e_Message.ControlPanel_Button_Click_CheckDevice_Controller2_Instance:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "Controller 2 initialization error"; break;
                            case e_ComboBox_Language.ko: result = "컨트롤러2 초기화 오류"; break;
                        }
                        break;
                    case e_Message.ControlPanel_Button_Click_CheckDevice_Controller2_StatusBase:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "Controller 2 communication error"; break;
                            case e_ComboBox_Language.ko: result = "컨트롤러2 통신 오류"; break;
                        }
                        break;
                    case e_Message.ControlPanel_Button_Click_CheckDevice_IceMaker_Instance:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "Ice maker initialization error"; break;
                            case e_ComboBox_Language.ko: result = "제빙기 초기화 오류"; break;
                        }
                        break;
                    case e_Message.ControlPanel_Button_Click_CheckDevice_IceMaker_StatusBase:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "Ice maker communication error"; break;
                            case e_ComboBox_Language.ko: result = "제빙기 통신 오류"; break;
                        }
                        break;
                    case e_Message.ControlPanel_Button_Click_CheckDevice_Robot_Instance:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "Robot initialization error"; break;
                            case e_ComboBox_Language.ko: result = "로봇 초기화 오류"; break;
                        }
                        break;
                    case e_Message.ControlPanel_Button_Click_CheckDevice_Robot_StatusBase:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "Robot communication error"; break;
                            case e_ComboBox_Language.ko: result = "로봇 통신 오류"; break;
                        }
                        break;
                    case e_Message.ControlPanel_Button_Click_Interlock_Door_Close:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "It's already closed"; break;
                            case e_ComboBox_Language.ko: result = "이미 닫혀있습니다"; break;
                        }
                        break;
                    case e_Message.ControlPanel_Button_Click_Interlock_Door_Open:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "It's already open"; break;
                            case e_ComboBox_Language.ko: result = "이미 열려있습니다"; break;
                        }
                        break;
                    case e_Message.ControlPanel_Button_Click_Interlock_Door_Tumbler_Home:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "LM already in home position"; break;
                            case e_ComboBox_Language.ko: result = "LM이 이미 홈 위치에 있습니다"; break;
                        }
                        break;
                    case e_Message.ControlPanel_Button_Click_Interlock_Door_Tumbler_Ready:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "LM already in home position"; break;
                            case e_ComboBox_Language.ko: result = "LM이 이미 홈 위치에 있습니다"; break;
                        }
                        break;
                    case e_Message.ControlPanel_Button_Click_Interlock_Door_Tumbler_Check:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "LM is not home position"; break;
                            case e_ComboBox_Language.ko: result = "LM이 홈 위치가 아닙니다"; break;
                        }
                        break;
                    case e_Message.ControlPanel_Button_Click_Interlock_Motor_Turn:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "Pumping motor not home position"; break;
                            case e_ComboBox_Language.ko: result = "펌핑 모터가 홈 위치가 아닙니다"; break;
                        }
                        break;
                    case e_Message.ControlPanel_CheckProcess:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "Program is already running"; break;
                            case e_ComboBox_Language.ko: result = "프로그램이 이미 실행 중입니다"; break;
                        }
                        break;
                    case e_Message.ControlPanel_UserClosing:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "Are you sure you want to exit the program?"; break;
                            case e_ComboBox_Language.ko: result = "프로그램을 종료하시겠습니까?"; break;
                        }
                        break;
                    case e_Message.Login_PW:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "You entered the wrong password"; break;
                            case e_ComboBox_Language.ko: result = "비밀번호를 잘못 입력했습니다"; break;
                        }
                        break;
                    case e_Message.Parameter_Door:
                        switch (GetLanguage())
                        {
                            case e_ComboBox_Language.en: result = "Initialize the door database. Do you want to proceed?"; break;
                            case e_ComboBox_Language.ko: result = "도어 데이터베이스를 초기화합니다. 진행하시겠습니까?"; break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return result;
        }

        public static string[] GetNames(e_Parameter parameter)
        {
            string[] result = null;
            try
            {
                // e_Parameter
                switch (parameter)
                {
                    case e_Parameter.Product:
                        result = Enum.GetNames(typeof(e_Parameter_Product));
                        break;
                    case e_Parameter.Booth:
                        result = Enum.GetNames(typeof(e_Parameter_Booth));
                        break;
                    case e_Parameter.DID_Bottom:
                        result = Enum.GetNames(typeof(e_Parameter_DID_Bottom));
                        break;
                    case e_Parameter.Door:
                        result = Enum.GetNames(typeof(e_Parameter_Door));
                        break;

                    case e_Parameter.CoffeeMaker:
                        result = Enum.GetNames(typeof(e_Parameter_CoffeeMaker));
                        break;
                    case e_Parameter.Controller1:
                        result = Enum.GetNames(typeof(e_Parameter_Controller1));
                        break;
                    case e_Parameter.Controller2:
                        result = Enum.GetNames(typeof(e_Parameter_Controller2));
                        break;
                    case e_Parameter.IceMaker:
                        result = Enum.GetNames(typeof(e_Parameter_IceMaker));
                        break;
                    case e_Parameter.Robot:
                        result = Enum.GetNames(typeof(e_Parameter_Robot));
                        break;
                    case e_Parameter.Barcode:
                        result = Enum.GetNames(typeof(e_Parameter_Barcode));
                        break;
                    case e_Parameter.DID:
                        result = Enum.GetNames(typeof(e_Parameter_DID));
                        break;
                    case e_Parameter.Kiosk:
                        result = Enum.GetNames(typeof(e_Parameter_Kiosk));
                        break;
                    case e_Parameter.Remote:
                        result = Enum.GetNames(typeof(e_Parameter_Remote));
                        break;

                    case e_Parameter.Alarm:
                        result = Enum.GetNames(typeof(e_Parameter_Alarm));
                        break;
                    case e_Parameter.Control:
                        result = Enum.GetNames(typeof(e_Parameter_Control));
                        break;
                    case e_Parameter.DB:
                        result = Enum.GetNames(typeof(e_Parameter_DB));
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return result;
        }

        public static DataTable GetOrder()
        {
            DataTable result = new DataTable();
            try
            {
                result = DB.MySQL.GetDataTable($"SELECT * FROM {e_DB._Order} WHERE {e_DB_Order.Column02}='{e_Order_Status.주문}' ORDER BY {e_DB_Order.Column01} ASC");
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return result;
        }

        public static string GetProductKey(string productCode)
        {
            string result = string.Empty;
            try
            {
                foreach (string key in GlobalVariable.Parameter[(int)e_Parameter.Product].Keys)
                {
                    if (GlobalVariable.Parameter[(int)e_Parameter.Product][key][(int)e_Parameter_Product.Product_Code] == productCode)
                    {
                        result = key;

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return result;
        }

        public static bool GetSimulation()
        {
            bool result = false;
            try
            {
                result = GlobalVariable.Parameter[(int)e_Parameter.Booth][CONST.S_KEY][(int)e_Parameter_Booth.Simulation] == e_ComboBox_Use.Use.ToString();
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return result;
        }

        public static string GetString(Exception ex)
        {
            return string.Format("try catch error (message=[{0}])", ex);
        }
        public static string GetString(object value)
        {
            return string.Format("{0}", value);
        }

        public static string[] GetStrings(string input, string pattern, StringSplitOptions options = StringSplitOptions.None)
        {
            return input.Split(new string[] { pattern }, options);
        }

        public static bool LoadParameter()
        {
            bool result = false;
            try
            {
                for (int i = 0; i < Enum.GetNames(typeof(e_Parameter)).Length; i++)
                {
                    result = LoadParameter((e_Parameter)i);

                    if (result == false)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return result;
        }
        public static bool LoadParameter(e_Parameter parameter)
        {
            bool result = false;
            try
            {
                if (GlobalVariable.Parameter[(int)parameter] == null)
                {
                    GlobalVariable.Parameter[(int)parameter] = new Dictionary<string, string[]>();
                }

                string path = $@"{GlobalVariable.Directory.Parameter}\{parameter}.{CONST.S_CSV}";

                if (File.Exists(path))
                {
                    GlobalVariable.Parameter[(int)parameter].Clear();

                    using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
                    {
                        while (streamReader.EndOfStream == false)
                        {
                            // step.1
                            string line = streamReader.ReadLine();
                            // step.2
                            line = line.Replace("\t", "");
                            // step.3
                            string[] value = line.Split(',');
                            // step.4
                            for (int i = 0; i < value.Length; i++)
                            {
                                value[i] = value[i].Trim();
                            }
                            // step.5
                            if (int.TryParse(value[0], out int no))
                            {
                                GlobalVariable.Parameter[(int)parameter].Add(value[0], value);
                            }
                        }
                    }

                    result = true;
                }
                else
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, $"File not found ({path})");
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            if (result == false)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, $"{parameter} Parameter Load NG");
            }
            return result;
        }

        public static DialogResult MessageBox(string call, string text, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            DialogResult result = DialogResult.None;
            try
            {
                Log.Write(call, $"MessageBox=[{text.Replace("\r\n", " ")}]");

                result = System.Windows.Forms.MessageBox.Show(text, MethodBase.GetCurrentMethod().Name, buttons, icon);

                Log.Write(call, $"[{buttons}]=[{result}]");
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return result;
        }

        public static bool SaveParameter(string call, System.Windows.Forms.DataGridView dgv, e_Parameter parameter, bool backup = true)
        {
            bool result = false;
            try
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, $"call=[{call}]");

                string path = $@"{GlobalVariable.Directory.Parameter}\{parameter}.{CONST.S_CSV}";

                if (File.Exists(path))
                {
                    if (backup)
                    {
                        GlobalVariable.Alarm[(int)e_Alarm.ERROR_PARAMETER_BACKUP] = !BackupParameter(parameter, path);
                    }

                    #region File Delete

                    // step.1
                    File.SetAttributes(path, FileAttributes.Normal);
                    // step.2
                    FileInfo fileInfo = new FileInfo(path);
                    fileInfo.IsReadOnly = false;
                    // step.3
                    System.Threading.Thread.Sleep(10);

                    File.Delete(path);

                    #endregion
                }

                if (dgv != null)
                {
                    GlobalVariable.Alarm[(int)e_Alarm.ERROR_PARAMETER_UPDATE] = !UpdateParameter(dgv, parameter);
                }

                using (FileStream fileStream = new FileStream(path, FileMode.Append))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                    {
                        streamWriter.WriteLine("==================================================");
                        //string value = $@"{parameter} ({GetDateTimeString(DateTime.Now)})";
                        string value = $@"{parameter} Parameter";
                        streamWriter.WriteLine(value);
                        streamWriter.WriteLine("==================================================");

                        string[] columns = GetNames(parameter);

                        string line = string.Empty;
                        for (int i = 0; i < columns.Length; i++)
                        {
                            line += columns[i];

                            if (i < columns.Length - 1)
                            {
                                line += ",";
                            }
                        }
                        streamWriter.WriteLine(line);

                        string text = Environment.NewLine + line + Environment.NewLine;

                        line = string.Empty;
                        foreach (string key in GlobalVariable.Parameter[(int)parameter].Keys)
                        {
                            for (int col = 0; col < GlobalVariable.Parameter[(int)parameter][key].Length; col++)
                            {
                                line += GlobalVariable.Parameter[(int)parameter][key][col];

                                if (col < GlobalVariable.Parameter[(int)parameter][key].Length - 1)
                                {
                                    line += ",";
                                }
                            }

                            line += "\r\n";
                        }
                        streamWriter.Write(line);

                        Log.Write(MethodBase.GetCurrentMethod().Name, text + line);
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            if (result == false)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, $"{parameter} Parameter Save NG");
            }
            GlobalVariable.Alarm[(int)e_Alarm.ERROR_PARAMETER_SAVE] = !result;
            return result;
        }

        public static void SetControlsProperties(Control[] controls)
        {
            try
            {
                foreach (Control control in controls)
                {
                    if (control is Button)
                    {
                        control.BackColor = SystemColors.Control;
                        control.ForeColor = SystemColors.ControlText;
                    }

                    control.TabStop = false;
                }

                foreach (Control control in controls)
                {
                    foreach (string key in GlobalVariable.Parameter[(int)e_Parameter.Control].Keys)
                    {
                        if (control.Name == GlobalVariable.Parameter[(int)e_Parameter.Control][key][(int)e_Parameter_Control.Control_Name])
                        {
                            control.Visible = GlobalVariable.Parameter[(int)e_Parameter.Control][key][(int)e_Parameter_Control.Visible] == e_ComboBox_Use.Use.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
        }

        public static void SetControlsText(Control[] controls)
        {
            try
            {
                foreach (Control control in controls)
                {
                    foreach (string key in GlobalVariable.Parameter[(int)e_Parameter.Control].Keys)
                    {
                        if (control.Name == GlobalVariable.Parameter[(int)e_Parameter.Control][key][(int)e_Parameter_Control.Control_Name])
                        {
                            #region Font

                            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Font));

                            switch (GetLanguage())
                            {
                                case e_ComboBox_Language.en: control.Font = (Font)typeConverter.ConvertFromString(GlobalVariable.Parameter[(int)e_Parameter.Control][key][(int)e_Parameter_Control.en_Font].Replace("/", ",")); break;
                                case e_ComboBox_Language.ko: control.Font = (Font)typeConverter.ConvertFromString(GlobalVariable.Parameter[(int)e_Parameter.Control][key][(int)e_Parameter_Control.ko_Font].Replace("/", ",")); break;
                            }

                            #endregion

                            #region Text

                            switch (GetLanguage())
                            {
                                case e_ComboBox_Language.en: control.Text = GlobalVariable.Parameter[(int)e_Parameter.Control][key][(int)e_Parameter_Control.en_Text].Replace("\\r\\n", "\r\n"); break;
                                case e_ComboBox_Language.ko: control.Text = GlobalVariable.Parameter[(int)e_Parameter.Control][key][(int)e_Parameter_Control.ko_Text].Replace("\\r\\n", "\r\n"); break;
                            }

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
        }

        private static bool UpdateParameter(System.Windows.Forms.DataGridView dgv, e_Parameter parameter)
        {
            bool result = false;
            try
            {
                GlobalVariable.Parameter[(int)parameter].Clear();

                for (int row = 0; row < dgv.Rows.Count - 1; row++)
                {
                    string[] value = new string[dgv.Columns.Count + 1];

                    value[0] = (row + 1).ToString();

                    for (int col = 1; col <= dgv.Columns.Count; col++)
                    {
                        value[col] = GetString(dgv.Rows[row].Cells[col - 1].Value).Trim();
                    }

                    GlobalVariable.Parameter[(int)parameter].Add(value[0], value);
                }

                result = true;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            if (result == false)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, $"{parameter} Parameter Update NG");
            }
            return result;
        }

        public static bool UpdateProductStatus()
        {
            bool result = false;
            try
            {
                string NG = string.Empty;

                foreach (string key in GlobalVariable.Parameter[(int)e_Parameter.Product].Keys)
                {
                    GlobalVariable.Parameter[(int)e_Parameter.Product][key][(int)e_Parameter_Product.Status] = CheckProductDevice(key, false, ref NG) && CheckProductSensor(key, ref NG) ? CONST.S_OK : CONST.S_NG;

                    switch (GlobalVariable.Parameter[(int)e_Parameter.Product][key][(int)e_Parameter_Product.Product_Code])
                    {
                        case "02-I":
                        case "06-H":
                        case "06-I":
                        case "07-H":
                        case "07-I":
                        case "17-H":
                        case "17-I":
                        case "18-H":
                        case "18-I":
                            GlobalVariable.Parameter[(int)e_Parameter.Product][key][(int)e_Parameter_Product.Status] = CONST.S_NG;
                            break;
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return result;
        }
        public static bool CheckOrderProhibited()
        {
            bool result = false;
            try
            {
                result = GlobalDevice.Robot.Clean ||
                         GlobalDevice.Robot.Maintenance;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            return result;
        }

        public static class CoffeeMaker
        {
            public static void LogWrite(string call, string text)
            {
                #region flag

                bool flag = true;
                try
                {
                    flag = GlobalVariable.Parameter[(int)e_Parameter.CoffeeMaker][CONST.S_KEY][(int)e_Parameter_CoffeeMaker.LogEnabled] == e_ComboBox_Use.Use.ToString();
                }
                catch
                {
                }

                #endregion

                #region file

                string file = string.Empty;
                try
                {
                    file = GlobalVariable.Parameter[(int)e_Parameter.CoffeeMaker][CONST.S_KEY][(int)e_Parameter_CoffeeMaker.Log_File];
                }
                catch
                {
                }

                #endregion

                if (flag)
                {
                    Log.Write(call, text, file);
                }
            }

            public static class Water
            {
                public static bool CheckIdle(string call)
                {
                    bool result = false;
                    try
                    {
                        result = GlobalDevice.CoffeeMaker.WaterAction == EversysApi.Defines.ModuleAction_t.ActionIdle_e ||
                                 GlobalDevice.CoffeeMaker.WaterAction == EversysApi.Defines.ModuleAction_t.ActionEnding_e;

                        Log.Write(call, $"WaterAction=[{GlobalDevice.CoffeeMaker.WaterAction}]");
                    }
                    catch (Exception ex)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                    }
                    return result;
                }

                public static bool CheckRun()
                {
                    bool result = false;
                    try
                    {
                        result = GlobalDevice.CoffeeMaker.WaterAction != EversysApi.Defines.ModuleAction_t.ActionIdle_e;
                    }
                    catch (Exception ex)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                    }
                    return result;
                }
                public static bool CheckRun(string call, int productId)
                {
                    bool result = false;
                    try
                    {
                        result = GlobalDevice.CoffeeMaker.WaterProductKeyIdL == productId;

                        Log.Write(call, $"WaterProductKeyIdL=[{GlobalDevice.CoffeeMaker.WaterProductKeyIdL}]");
                    }
                    catch (Exception ex)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                    }
                    return result;
                }
            }

            public static class CoffeeMilkL
            {
                public static bool CheckIdle(string call)
                {
                    bool result = false;
                    try
                    {
                        result = GlobalDevice.CoffeeMaker.CoffeeMilkLAction == EversysApi.Defines.ModuleAction_t.ActionIdle_e ||
                                 GlobalDevice.CoffeeMaker.CoffeeMilkLAction == EversysApi.Defines.ModuleAction_t.ActionEnding_e;

                        Log.Write(call, $"CoffeeMilkLAction=[{GlobalDevice.CoffeeMaker.CoffeeMilkLAction}]");
                    }
                    catch (Exception ex)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                    }
                    return result;
                }

                public static bool CheckRun()
                {
                    bool result = false;
                    try
                    {
                        result = GlobalDevice.CoffeeMaker.CoffeeMilkLAction != EversysApi.Defines.ModuleAction_t.ActionIdle_e;
                    }
                    catch (Exception ex)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                    }
                    return result;
                }
                public static bool CheckRun(string call, int productId)
                {
                    bool result = false;
                    try
                    {
                        result = GlobalDevice.CoffeeMaker.CoffeeMilkLProductKeyId == productId;

                        Log.Write(call, $"CoffeeMilkLProductKeyId=[{GlobalDevice.CoffeeMaker.CoffeeMilkLProductKeyId}]");
                    }
                    catch (Exception ex)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                    }
                    return result;
                }

                public static bool CheckClean()
                {
                    bool result = false;
                    try
                    {
                        result = GlobalDevice.CoffeeMaker.CoffeeMilkLProcess == EversysApi.Defines.ModuleProcess_t.ProcessClean_e;
                    }
                    catch (Exception ex)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                    }
                    return result;
                }

                public static bool CheckRinse()
                {
                    bool result = false;
                    try
                    {
                        result = GlobalDevice.CoffeeMaker.CoffeeMilkLProcess == EversysApi.Defines.ModuleProcess_t.ProcessRinse_e;
                    }
                    catch (Exception ex)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                    }
                    return result;
                }
            }
        }

        public static class DataGridView
        {
            public static void SetProperties(System.Windows.Forms.DataGridView dgv)
            {
                try
                {
                    DoubleBuffered(dgv, true);

                    dgv.AllowUserToAddRows = false;
                    dgv.AllowUserToDeleteRows = false;
                    dgv.AllowUserToOrderColumns = false;
                    dgv.AllowUserToResizeColumns = false;
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
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
            }

            public static void AddColumns(System.Windows.Forms.DataGridView dgv, string[] columns)
            {
                try
                {
                    dgv.Columns.Clear();

                    for (int i = 0; i < columns.Length; i++)
                    {
                        dgv.Columns.Add(columns[i], columns[i].Replace("_", " "));

                        dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                        dgv.Columns[i].ReadOnly = true;
                    }

                    switch (dgv.Name)
                    {
                        case "dgv_Alarm":
                            switch (GetLanguage())
                            {
                                case e_ComboBox_Language.en:
                                    dgv.Columns[e_DB_Alarm.Column00.ToString()].HeaderText = "DateTime";
                                    dgv.Columns[e_DB_Alarm.Column02.ToString()].HeaderText = "Code";
                                    dgv.Columns[e_DB_Alarm.Column03.ToString()].HeaderText = "Alarm";
                                    dgv.Columns[e_DB_Alarm.Column04.ToString()].HeaderText = "Action";
                                    break;
                                case e_ComboBox_Language.ko:
                                    dgv.Columns[e_DB_Alarm.Column00.ToString()].HeaderText = "날짜 시간";
                                    dgv.Columns[e_DB_Alarm.Column02.ToString()].HeaderText = "코드";
                                    dgv.Columns[e_DB_Alarm.Column03.ToString()].HeaderText = "알람";
                                    dgv.Columns[e_DB_Alarm.Column04.ToString()].HeaderText = "조치 방법";
                                    break;
                            }

                            dgv.Columns[e_DB_Alarm.Column01.ToString()].Visible = false;
                            for (int i = (int)e_DB_Alarm.Column05; i < Enum.GetNames(typeof(e_DB_Alarm)).Length; i++)
                            {
                                dgv.Columns[i].Visible = false;
                            }
                            break;
                        case "dgv_Door":
                            dgv.Columns[e_DB_Door.Column01.ToString()].HeaderText = "Lock";
                            dgv.Columns[e_DB_Door.Column02.ToString()].HeaderText = "Trigger";
                            dgv.Columns[e_DB_Door.Column03.ToString()].HeaderText = "DateTime";
                            dgv.Columns[e_DB_Door.Column04.ToString()].HeaderText = "ID";
                            dgv.Columns[e_DB_Door.Column05.ToString()].HeaderText = "Barcode";
                            dgv.Columns[e_DB_Door.Column06.ToString()].HeaderText = "Cup";
                            dgv.Columns[e_DB_Door.Column07.ToString()].HeaderText = "Sensor";

                            dgv.Columns[e_DB_Door.Column00.ToString()].Visible = false;
                            for (int i = (int)e_DB_Door.Column08; i < Enum.GetNames(typeof(e_DB_Door)).Length; i++)
                            {
                                dgv.Columns[i].Visible = false;
                            }
                            break;
                        case "dgv_Order":
                            dgv.Columns[e_DB_Order.Column00.ToString()].HeaderText = "DateTime";
                            dgv.Columns[e_DB_Order.Column01.ToString()].HeaderText = "ID";
                            dgv.Columns[e_DB_Order.Column02.ToString()].HeaderText = "Status";
                            dgv.Columns[e_DB_Order.Column03.ToString()].HeaderText = "Status_Sub";
                            dgv.Columns[e_DB_Order.Column04.ToString()].HeaderText = "Source";
                            dgv.Columns[e_DB_Order.Column05.ToString()].HeaderText = "Order_No";
                            dgv.Columns[e_DB_Order.Column06.ToString()].HeaderText = "Barcode";
                            dgv.Columns[e_DB_Order.Column07.ToString()].HeaderText = "Product_Code";
                            dgv.Columns[e_DB_Order.Column08.ToString()].HeaderText = "Product_Name";
                            dgv.Columns[e_DB_Order.Column11.ToString()].HeaderText = "Door";
                            dgv.Columns[e_DB_Order.Column12.ToString()].HeaderText = "Order_Cup";

                            dgv.Columns[e_DB_Order.Column09.ToString()].Visible = false;
                            dgv.Columns[e_DB_Order.Column10.ToString()].Visible = false;
                            for (int i = (int)e_DB_Order.Column13; i < Enum.GetNames(typeof(e_DB_Order)).Length; i++)
                            {
                                dgv.Columns[i].Visible = false;
                            }
                            break;
                    }

                    dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                    DataGridViewCellStyle defaultCellStyle = new DataGridViewCellStyle();
                    defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    defaultCellStyle.BackColor = SystemColors.Highlight;
                    defaultCellStyle.ForeColor = SystemColors.HighlightText;
                    defaultCellStyle.Font = new Font(e_Font.Tahoma.ToString(), 11f);
                    dgv.ColumnHeadersDefaultCellStyle = defaultCellStyle;

                    dgv.ColumnHeadersHeight = 30;
                    dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                    dgv.EnableHeadersVisualStyles = false;

                    dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
            }
        }

        public static class DB
        {
            public static class MySQL
            {
                private static string GetConnectionString()
                {
                    string result = string.Empty;
                    try
                    {
                        result = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};Connection Timeout={5};", GlobalVariable.Parameter[(int)e_Parameter.DB][CONST.S_KEY][(int)e_Parameter_DB.Server],
                                                                                                                           GlobalVariable.Parameter[(int)e_Parameter.DB][CONST.S_KEY][(int)e_Parameter_DB.Port],
                                                                                                                           GlobalVariable.Parameter[(int)e_Parameter.DB][CONST.S_KEY][(int)e_Parameter_DB.Database],
                                                                                                                           GlobalVariable.Parameter[(int)e_Parameter.DB][CONST.S_KEY][(int)e_Parameter_DB.Uid],
                                                                                                                           GlobalVariable.Parameter[(int)e_Parameter.DB][CONST.S_KEY][(int)e_Parameter_DB.Pwd],
                                                                                                                           GlobalVariable.Parameter[(int)e_Parameter.DB][CONST.S_KEY][(int)e_Parameter_DB.Connection_Timeout_sec]
                                              );
                    }
                    catch (Exception ex)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                    }
                    return result;
                }

                public static DataTable GetDataTable(string cmdText)
                {
                    DataTable result = new DataTable();
                    try
                    {
                        using (MySqlConnection connection = new MySqlConnection(GetConnectionString()))
                        {
                            connection.Open();

                            using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmdText, connection))
                            {
                                dataAdapter.Fill(result);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                    }
                    if (result.Columns.Count <= 0)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, $"cmdText=[{cmdText}]");
                    }
                    GlobalVariable.Alarm[(int)e_Alarm.ERROR_DB_GET_DATA] = result.Columns.Count <= 0;
                    return result;
                }

                // TODO
                public static bool Query(string cmdText)
                {
                    bool result = false;
                    try
                    {
                        using (MySqlConnection connection = new MySqlConnection(GetConnectionString()))
                        {
                            connection.Open();

                            using (MySqlCommand command = new MySqlCommand(cmdText, connection))
                            {
                                result = command.ExecuteNonQuery() > 0;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                    }
                    if (result == false)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, $"cmdText=[{cmdText}]");
                    }
                    GlobalVariable.Alarm[(int)e_Alarm.ERROR_DB_SET_DATA] = !result;
                    return result;
                }
            }
        }

        public static class Door
        {
            public static bool Initialize()
            {
                bool result = false;
                try
                {
                    DB.MySQL.Query($"DELETE FROM {e_DB._Door}");

                    for (int i = (int)e_Door.Door1; i < Enum.GetNames(typeof(e_Door)).Length; i++)
                    {
                        result = DB.MySQL.Query($"INSERT INTO {e_DB._Door}({e_DB_Door.Column00},{e_DB_Door.Column01},{e_DB_Door.Column02}) VALUES('{(e_Door)i}','{e_Door_Lock.Unlock}','{e_Door_Trigger.Clear}')");

                        if (result == false)
                        {
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
                return result;
            }

            private static string GetData(e_Door door, e_DB_Door column)
            {
                string result = string.Empty;
                try
                {
                    DataTable data = DB.MySQL.GetDataTable($"SELECT * FROM {e_DB._Door} WHERE {e_DB_Door.Column00}='{door}'");

                    if (data.Rows.Count > 0)
                    {
                        result = data.Rows[0][column.ToString()].ToString();
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
                return result;
            }
            private static bool SetData(e_Door door, e_DB_Door column, string value)
            {
                bool result = false;
                try
                {
                    result = DB.MySQL.Query($"UPDATE {e_DB._Door} SET {column}='{value}' WHERE {e_DB_Door.Column00}='{door}'");
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
                return result;
            }

            public static e_Door_Lock GetLock(e_Door door)
            {
                e_Door_Lock result = e_Door_Lock.Lock;
                try
                {
                    result = (e_Door_Lock)Enum.Parse(typeof(e_Door_Lock), GetData(door, e_DB_Door.Column01));
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
                return result;
            }
            public static bool SetLock(e_Door door, e_Door_Lock value)
            {
                bool result = false;
                try
                {
                    result = SetData(door, e_DB_Door.Column01, value.ToString());
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
                return result;
            }

            public static e_Door_Trigger GetTrigger(e_Door door)
            {
                e_Door_Trigger result = e_Door_Trigger.Clear;
                try
                {
                    result = (e_Door_Trigger)Enum.Parse(typeof(e_Door_Trigger), GetData(door, e_DB_Door.Column02));
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
                return result;
            }
            public static bool SetTrigger(e_Door door, e_Door_Trigger value)
            {
                bool result = false;
                try
                {
                    result = SetData(door, e_DB_Door.Column02, value.ToString());
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
                return result;
            }

            public static string GetDateTime(e_Door door)
            {
                string result = string.Empty;
                try
                {
                    result = GetData(door, e_DB_Door.Column03);
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
                return result;
            }
            public static bool SetDateTime(e_Door door, DateTime value)
            {
                bool result = false;
                try
                {
                    result = SetData(door, e_DB_Door.Column03, GetDateTimeString(value));
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
                return result;
            }

            public static string GetID(e_Door door)
            {
                string result = string.Empty;
                try
                {
                    result = GetData(door, e_DB_Door.Column04);
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
                return result;
            }
            public static bool SetID(e_Door door, string value)
            {
                bool result = false;
                try
                {
                    result = SetData(door, e_DB_Door.Column04, value);
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
                return result;
            }

            public static string GetBarcode(e_Door door)
            {
                string result = string.Empty;
                try
                {
                    result = GetData(door, e_DB_Door.Column05);
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
                return result;
            }
            public static bool SetBarcode(e_Door door, string value)
            {
                bool result = false;
                try
                {
                    result = SetData(door, e_DB_Door.Column05, value);
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
                return result;
            }

            public static e_Order_Cup GetCup(e_Door door)
            {
                e_Order_Cup result = e_Order_Cup.TAKEOUT;
                try
                {
                    result = (e_Order_Cup)Enum.Parse(typeof(e_Order_Cup), GetData(door, e_DB_Door.Column06));
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
                return result;
            }
            public static bool SetCup(e_Door door, e_Order_Cup value)
            {
                bool result = false;
                try
                {
                    result = SetData(door, e_DB_Door.Column06, value.ToString());
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
                return result;
            }

            public static string GetSensor(e_Door door)
            {
                string result = string.Empty;
                try
                {
                    result = GetData(door, e_DB_Door.Column07);
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
                return result;
            }
            public static bool SetSensor(e_Door door, string value)
            {
                bool result = false;
                try
                {
                    result = SetData(door, e_DB_Door.Column07, value);
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
                return result;
            }

            public static bool Clear(e_Door door, bool @lock = false)
            {
                bool result = false;
                try
                {
                    #region cmdText

                    string cmdText = $"UPDATE {e_DB._Door} SET";
                    for (int i = (int)e_DB_Door.Column01; i < Enum.GetNames(typeof(e_DB_Door)).Length; i++)
                    {
                        switch ((e_DB_Door)i)
                        {
                            case e_DB_Door.Column01:
                                cmdText += $" {(e_DB_Door)i}='{(@lock ? e_Door_Lock.Lock : e_Door_Lock.Unlock)}'";
                                break;
                            case e_DB_Door.Column02:
                                cmdText += $" {(e_DB_Door)i}='{e_Door_Trigger.Clear}'";
                                break;
                            default:
                                cmdText += $" {(e_DB_Door)i}=NULL";
                                break;
                        }

                        if (i < Enum.GetNames(typeof(e_DB_Door)).Length - 1)
                        {
                            cmdText += ",";
                        }
                    }
                    cmdText += $" WHERE {e_DB_Door.Column00}='{door}'";

                    #endregion

                    result = DB.MySQL.Query(cmdText);
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
                return result;
            }
        }
    }
}
