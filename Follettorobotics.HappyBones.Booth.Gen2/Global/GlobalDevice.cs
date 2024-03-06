using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using Library.Log;

namespace Follettorobotics.HappyBones.Booth.Gen2
{
    public enum e_Device
    {
        CoffeeMaker,
        Controller1,
        Controller2,
        IceMaker,
        Robot,
        Barcode,
        Kiosk,
        Remote,
    }

    public static class GlobalDevice
    {
        public struct Status_t
        {
            public bool Comm;
            public bool StatusBase;
            public bool Run;
            public bool Status;

            public string MachineCode;
            public string ErrorCode;
        }

        public static class CoffeeMaker
        {
            public static ICoffeeMaker Instance = null;

            public static Status_t Status = new Status_t();

            #region Eversys

            public static string Warnings = string.Empty;
            public static string Stops = string.Empty;
            public static string Errors = string.Empty;

            public static bool IgnoreWarnings = false;

            public static EversysApi.Defines.ModuleStatus_t WaterStatus;
            public static EversysApi.Defines.ModuleAction_t WaterAction;
            public static EversysApi.Defines.ModuleProcess_t WaterProcess;
            public static byte WaterProductKeyIdL;

            public static EversysApi.Defines.ModuleStatus_t CoffeeMilkLStatus;
            public static EversysApi.Defines.ModuleAction_t CoffeeMilkLAction;
            public static EversysApi.Defines.ModuleProcess_t CoffeeMilkLProcess;
            public static byte CoffeeMilkLProductKeyId;

            public static bool Clean = false;
            public static bool Rinse = false;

            public static bool IgnoreStatus = false;

            #endregion
        }

        public static class Controller1
        {
            public static CController1 Instance = null;

            public static Status_t Status = new Status_t();

            public static bool[] Sensor = new bool[Enum.GetNames(typeof(e_Device_Controller1_Sensor)).Length];
        }

        public static class Controller2
        {
            public static CController2 Instance = null;

            public static Status_t Status = new Status_t();

            public static bool[] Sensor = new bool[Enum.GetNames(typeof(e_Device_Controller2_Sensor)).Length];
        }

        public static class IceMaker
        {
            public static IIceMaker Instance = null;

            public static Status_t Status = new Status_t();
        }

        public static class Robot
        {
            public static IRobot Instance = null;

            public static Status_t Status = new Status_t();

            public static bool[] Feedback = new bool[Enum.GetNames(typeof(e_Device_Robot_Feedback)).Length];

            public static bool Clean = false;
            public static bool Maintenance = false;
        }

        public static class Barcode
        {
            public static CBarcode Instance = null;

            public static Status_t Status = new Status_t();

            public static string Data = string.Empty;
        }

        public static class Kiosk
        {
            public static CKiosk Instance = null;

            public static Status_t Status = new Status_t();
        }

        public static class Remote
        {
            public static CRemote Instance = null;

            public static Status_t Status = new Status_t();
        }

        public static void Stop()
        {
            for (int i = 0; i < Enum.GetNames(typeof(e_Device)).Length; i++)
            {
                Stop((e_Device)i);
            }
        }
        public static void Stop(e_Device device)
        {
            try
            {
                // Parameter_Button_Click
                switch (device)
                {
                    case e_Device.CoffeeMaker:
                        if (CoffeeMaker.Instance != null)
                        {
                            CoffeeMaker.Instance.Disconnect();
                            CoffeeMaker.Instance = null;
                        }
                        break;
                    case e_Device.Controller1:
                        if (Controller1.Instance != null)
                        {
                            Controller1.Instance.Dispose();
                            Controller1.Instance = null;
                        }
                        break;
                    case e_Device.Controller2:
                        if (Controller2.Instance != null)
                        {
                            Controller2.Instance.Dispose();
                            Controller2.Instance = null;
                        }
                        break;
                    case e_Device.IceMaker:
                        if (IceMaker.Instance != null)
                        {
                            IceMaker.Instance.Dispose();
                            IceMaker.Instance = null;
                        }
                        break;
                    case e_Device.Robot:
                        if (Robot.Instance != null)
                        {
                            Robot.Instance.Dispose();
                            Robot.Instance = null;
                        }
                        break;
                    case e_Device.Barcode:
                        if (Barcode.Instance != null)
                        {
                            Barcode.Instance.Dispose();
                            Barcode.Instance = null;
                        }
                        break;
                    case e_Device.Kiosk:
                        if (Kiosk.Instance != null)
                        {
                            Kiosk.Instance.Dispose();
                            Kiosk.Instance = null;
                        }
                        break;
                    case e_Device.Remote:
                        if (Remote.Instance != null)
                        {
                            Remote.Instance.Dispose();
                            Remote.Instance = null;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        public static bool Start()
        {
            bool result = false;
            try
            {
                for (int i = 0; i < Enum.GetNames(typeof(e_Device)).Length; i++)
                {
                    result = Start((e_Device)i);

                    //if (result == false)
                    //{
                    //    break;
                    //}
                }

                result = true;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }
        public static bool Start(e_Device device)
        {
            bool result = false;
            try
            {
                switch (device)
                {
                    case e_Device.CoffeeMaker:
                        result = GlobalFunction.GetEnabled(e_Parameter.CoffeeMaker) == false;

                        if (result == false)
                        {
                            if (CoffeeMaker.Instance == null)
                            {
                                switch ((e_Device_CoffeeMaker)Enum.Parse(typeof(e_Device_CoffeeMaker), GlobalVariable.Parameter[(int)e_Parameter.CoffeeMaker][CONST.S_KEY][(int)e_Parameter_CoffeeMaker.Device]))
                                {
                                    case e_Device_CoffeeMaker.Eversys:
                                        CoffeeMaker.Instance = new CCoffeeMaker_Eversys(new NetSerialPort());

                                        result = CoffeeMaker.Instance.Connect(GlobalVariable.Parameter[(int)e_Parameter.CoffeeMaker][CONST.S_KEY][(int)e_Parameter_CoffeeMaker.PortName]);
                                        break;
                                }
                            }
                        }

                        GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_START_COFFEE_MAKER] = !result;
                        break;
                    case e_Device.Controller1:
                        result = GlobalFunction.GetEnabled(e_Parameter.Controller1) == false;

                        if (result == false)
                        {
                            if (Controller1.Instance == null)
                            {
                                Controller1.Instance = new CController1();

                                Controller1.Instance.LogMsgEvent += new Library.LogMsgEventHandler(Controller1_LogMsgEvent);

                                Controller1.Instance.LogEnabled = GlobalVariable.Parameter[(int)e_Parameter.Controller1][CONST.S_KEY][(int)e_Parameter_Controller1.LogEnabled] == e_ComboBox_Use.Use.ToString();
                                Controller1.Instance.Device = GlobalVariable.Parameter[(int)e_Parameter.Controller1][CONST.S_KEY][(int)e_Parameter_Controller1.Device];
                                Controller1.Instance.PortName = GlobalVariable.Parameter[(int)e_Parameter.Controller1][CONST.S_KEY][(int)e_Parameter_Controller1.PortName];
                                Controller1.Instance.BaudRate = Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.Controller1][CONST.S_KEY][(int)e_Parameter_Controller1.BaudRate]);
                                Controller1.Instance.Parity = (System.IO.Ports.Parity)Enum.Parse(typeof(System.IO.Ports.Parity), GlobalVariable.Parameter[(int)e_Parameter.Controller1][CONST.S_KEY][(int)e_Parameter_Controller1.Parity]);
                                Controller1.Instance.DataBits = Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.Controller1][CONST.S_KEY][(int)e_Parameter_Controller1.DataBits]);
                                Controller1.Instance.StopBits = (System.IO.Ports.StopBits)Enum.Parse(typeof(System.IO.Ports.StopBits), GlobalVariable.Parameter[(int)e_Parameter.Controller1][CONST.S_KEY][(int)e_Parameter_Controller1.StopBits]);

                                Controller1.Instance.Timeout = Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.Controller1][CONST.S_KEY][(int)e_Parameter_Controller1.Timeout]);

                                result = Controller1.Instance.Open();
                            }
                        }

                        GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_START_CONTROLLER1] = !result;
                        break;
                    case e_Device.Controller2:
                        result = GlobalFunction.GetEnabled(e_Parameter.Controller2) == false;

                        if (result == false)
                        {
                            if (Controller2.Instance == null)
                            {
                                Controller2.Instance = new CController2();

                                Controller2.Instance.LogMsgEvent += new Library.LogMsgEventHandler(Controller2_LogMsgEvent);

                                Controller2.Instance.LogEnabled = GlobalVariable.Parameter[(int)e_Parameter.Controller2][CONST.S_KEY][(int)e_Parameter_Controller2.LogEnabled] == e_ComboBox_Use.Use.ToString();
                                Controller2.Instance.Device = GlobalVariable.Parameter[(int)e_Parameter.Controller2][CONST.S_KEY][(int)e_Parameter_Controller2.Device];
                                Controller2.Instance.PortName = GlobalVariable.Parameter[(int)e_Parameter.Controller2][CONST.S_KEY][(int)e_Parameter_Controller2.PortName];
                                Controller2.Instance.BaudRate = Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.Controller2][CONST.S_KEY][(int)e_Parameter_Controller2.BaudRate]);
                                Controller2.Instance.Parity = (System.IO.Ports.Parity)Enum.Parse(typeof(System.IO.Ports.Parity), GlobalVariable.Parameter[(int)e_Parameter.Controller2][CONST.S_KEY][(int)e_Parameter_Controller2.Parity]);
                                Controller2.Instance.DataBits = Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.Controller2][CONST.S_KEY][(int)e_Parameter_Controller2.DataBits]);
                                Controller2.Instance.StopBits = (System.IO.Ports.StopBits)Enum.Parse(typeof(System.IO.Ports.StopBits), GlobalVariable.Parameter[(int)e_Parameter.Controller2][CONST.S_KEY][(int)e_Parameter_Controller2.StopBits]);

                                Controller2.Instance.Timeout = Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.Controller2][CONST.S_KEY][(int)e_Parameter_Controller2.Timeout]);

                                result = Controller2.Instance.Open();
                            }
                        }

                        GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_START_CONTROLLER2] = !result;
                        break;
                    case e_Device.IceMaker:
                        result = GlobalFunction.GetEnabled(e_Parameter.IceMaker) == false;

                        if (result == false)
                        {
                            if (IceMaker.Instance == null)
                            {
                                switch ((e_Device_IceMaker)Enum.Parse(typeof(e_Device_IceMaker), GlobalVariable.Parameter[(int)e_Parameter.IceMaker][CONST.S_KEY][(int)e_Parameter_IceMaker.Device]))
                                {
                                    case e_Device_IceMaker.ICETRO:
                                        IceMaker.Instance = new CIceMaker_ICETRO();
                                        break;
                                }

                                IceMaker.Instance.LogMsgEvent += new Library.LogMsgEventHandler(IceMaker_LogMsgEvent);

                                IceMaker.Instance.LogEnabled = GlobalVariable.Parameter[(int)e_Parameter.IceMaker][CONST.S_KEY][(int)e_Parameter_IceMaker.LogEnabled] == e_ComboBox_Use.Use.ToString();
                                IceMaker.Instance.Device = GlobalVariable.Parameter[(int)e_Parameter.IceMaker][CONST.S_KEY][(int)e_Parameter_IceMaker.Device];
                                IceMaker.Instance.PortName = GlobalVariable.Parameter[(int)e_Parameter.IceMaker][CONST.S_KEY][(int)e_Parameter_IceMaker.PortName];
                                IceMaker.Instance.BaudRate = Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.IceMaker][CONST.S_KEY][(int)e_Parameter_IceMaker.BaudRate]);
                                IceMaker.Instance.Parity = (System.IO.Ports.Parity)Enum.Parse(typeof(System.IO.Ports.Parity), GlobalVariable.Parameter[(int)e_Parameter.IceMaker][CONST.S_KEY][(int)e_Parameter_IceMaker.Parity]);
                                IceMaker.Instance.DataBits = Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.IceMaker][CONST.S_KEY][(int)e_Parameter_IceMaker.DataBits]);
                                IceMaker.Instance.StopBits = (System.IO.Ports.StopBits)Enum.Parse(typeof(System.IO.Ports.StopBits), GlobalVariable.Parameter[(int)e_Parameter.IceMaker][CONST.S_KEY][(int)e_Parameter_IceMaker.StopBits]);

                                IceMaker.Instance.Timeout = Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.IceMaker][CONST.S_KEY][(int)e_Parameter_IceMaker.Timeout]);

                                result = IceMaker.Instance.Open();
                            }
                        }

                        GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_START_ICE_MAKER] = !result;
                        break;
                    case e_Device.Robot:
                        result = GlobalFunction.GetEnabled(e_Parameter.Robot) == false;

                        if (result == false)
                        {
                            if (Robot.Instance == null)
                            {
                                switch ((e_Device_Robot)Enum.Parse(typeof(e_Device_Robot), GlobalVariable.Parameter[(int)e_Parameter.Robot][CONST.S_KEY][(int)e_Parameter_Robot.Device]))
                                {
                                    case e_Device_Robot.DOOSAN:
                                        Robot.Instance = new CRobot_DOOSAN();
                                        break;
                                }

                                Robot.Instance.LogMsgEvent += new Library.LogMsgEventHandler(Robot_LogMsgEvent);

                                Robot.Instance.LogEnabled = GlobalVariable.Parameter[(int)e_Parameter.Robot][CONST.S_KEY][(int)e_Parameter_Robot.LogEnabled] == e_ComboBox_Use.Use.ToString();
                                Robot.Instance.Device = GlobalVariable.Parameter[(int)e_Parameter.Robot][CONST.S_KEY][(int)e_Parameter_Robot.Device];
                                Robot.Instance.IP = GlobalVariable.Parameter[(int)e_Parameter.Robot][CONST.S_KEY][(int)e_Parameter_Robot.IP];
                                Robot.Instance.Port = Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.Robot][CONST.S_KEY][(int)e_Parameter_Robot.Port]);
                                Robot.Instance.ConnectTimeout = Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.Robot][CONST.S_KEY][(int)e_Parameter_Robot.ConnectTimeout]);
                                Robot.Instance.ReceiveTimeout = Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.Robot][CONST.S_KEY][(int)e_Parameter_Robot.ReceiveTimeout]);

                                result = Robot.Instance.Connect();
                            }
                        }

                        GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_START_ROBOT] = !result;
                        break;
                    case e_Device.Barcode:
                        result = GlobalFunction.GetEnabled(e_Parameter.Barcode) == false;

                        if (result == false)
                        {
                            if (Barcode.Instance == null)
                            {
                                Barcode.Instance = new CBarcode();

                                Barcode.Instance.LogMsgEvent += new Library.LogMsgEventHandler(Barcode_LogMsgEvent);

                                Barcode.Instance.LogEnabled = GlobalVariable.Parameter[(int)e_Parameter.Barcode][CONST.S_KEY][(int)e_Parameter_Barcode.LogEnabled] == e_ComboBox_Use.Use.ToString();
                                Barcode.Instance.Device = GlobalVariable.Parameter[(int)e_Parameter.Barcode][CONST.S_KEY][(int)e_Parameter_Barcode.Device];
                                Barcode.Instance.PortName = GlobalVariable.Parameter[(int)e_Parameter.Barcode][CONST.S_KEY][(int)e_Parameter_Barcode.PortName];
                                Barcode.Instance.BaudRate = Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.Barcode][CONST.S_KEY][(int)e_Parameter_Barcode.BaudRate]);
                                Barcode.Instance.Parity = (System.IO.Ports.Parity)Enum.Parse(typeof(System.IO.Ports.Parity), GlobalVariable.Parameter[(int)e_Parameter.Barcode][CONST.S_KEY][(int)e_Parameter_Barcode.Parity]);
                                Barcode.Instance.DataBits = Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.Barcode][CONST.S_KEY][(int)e_Parameter_Barcode.DataBits]);
                                Barcode.Instance.StopBits = (System.IO.Ports.StopBits)Enum.Parse(typeof(System.IO.Ports.StopBits), GlobalVariable.Parameter[(int)e_Parameter.Barcode][CONST.S_KEY][(int)e_Parameter_Barcode.StopBits]);

                                result = Barcode.Instance.Open();
                            }
                        }

                        GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_START_BARCODE] = !result;
                        break;
                    case e_Device.Kiosk:
                        result = GlobalFunction.GetEnabled(e_Parameter.Kiosk) == false;

                        if (result == false)
                        {
                            if (Kiosk.Instance == null)
                            {
                                Kiosk.Instance = new CKiosk();

                                Kiosk.Instance.LogMsgEvent += new Library.LogMsgEventHandler(Kiosk_LogMsgEvent);

                                Kiosk.Instance.RecvStringEvent += new Library.Sockets.SocketServer.RecvStringEventHandler(Kiosk.Instance.RecvString);

                                Kiosk.Instance.LogEnabled = GlobalVariable.Parameter[(int)e_Parameter.Kiosk][CONST.S_KEY][(int)e_Parameter_Kiosk.LogEnabled] == e_ComboBox_Use.Use.ToString();
                                Kiosk.Instance.Device = GlobalVariable.Parameter[(int)e_Parameter.Kiosk][CONST.S_KEY][(int)e_Parameter_Kiosk.Device];
                                Kiosk.Instance.IP = GlobalVariable.Parameter[(int)e_Parameter.Kiosk][CONST.S_KEY][(int)e_Parameter_Kiosk.IP];
                                Kiosk.Instance.Port = Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.Kiosk][CONST.S_KEY][(int)e_Parameter_Kiosk.Port]);

                                result = Kiosk.Instance.Open();
                            }
                        }

                        GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_START_KIOSK] = !result;
                        break;
                    case e_Device.Remote:
                        result = GlobalFunction.GetEnabled(e_Parameter.Remote) == false;

                        if (result == false)
                        {
                            if (Remote.Instance == null)
                            {
                                Remote.Instance = new CRemote();

                                Remote.Instance.LogMsgEvent += new Library.LogMsgEventHandler(Remote_LogMsgEvent);

                                Remote.Instance.RecvStringEvent += new Library.Sockets.SocketServer.RecvStringEventHandler(Remote.Instance.RecvString);

                                Remote.Instance.LogEnabled = GlobalVariable.Parameter[(int)e_Parameter.Remote][CONST.S_KEY][(int)e_Parameter_Remote.LogEnabled] == e_ComboBox_Use.Use.ToString();
                                Remote.Instance.Device = GlobalVariable.Parameter[(int)e_Parameter.Remote][CONST.S_KEY][(int)e_Parameter_Remote.Device];
                                Remote.Instance.IP = GlobalVariable.Parameter[(int)e_Parameter.Remote][CONST.S_KEY][(int)e_Parameter_Remote.IP];
                                Remote.Instance.Port = Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.Remote][CONST.S_KEY][(int)e_Parameter_Remote.Port]);

                                result = Remote.Instance.Open();
                            }
                        }

                        GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_START_REMOTE] = !result;
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        #region LogMsgEvent

        private static void Controller1_LogMsgEvent(string call, string text)
        {
            try
            {
                Log.Write(call, text, GlobalVariable.Parameter[(int)e_Parameter.Controller1][CONST.S_KEY][(int)e_Parameter_Controller1.Log_File]);
            }
            catch
            {
                Log.Write(call, text);
            }
        }

        private static void Controller2_LogMsgEvent(string call, string text)
        {
            try
            {
                Log.Write(call, text, GlobalVariable.Parameter[(int)e_Parameter.Controller2][CONST.S_KEY][(int)e_Parameter_Controller2.Log_File]);
            }
            catch
            {
                Log.Write(call, text);
            }
        }

        private static void IceMaker_LogMsgEvent(string call, string text)
        {
            try
            {
                Log.Write(call, text, GlobalVariable.Parameter[(int)e_Parameter.IceMaker][CONST.S_KEY][(int)e_Parameter_IceMaker.Log_File]);
            }
            catch
            {
                Log.Write(call, text);
            }
        }

        private static void Robot_LogMsgEvent(string call, string text)
        {
            try
            {
                Log.Write(call, text, GlobalVariable.Parameter[(int)e_Parameter.Robot][CONST.S_KEY][(int)e_Parameter_Robot.Log_File]);
            }
            catch
            {
                Log.Write(call, text);
            }
        }

        private static void Barcode_LogMsgEvent(string call, string text)
        {
            try
            {
                Log.Write(call, text, GlobalVariable.Parameter[(int)e_Parameter.Barcode][CONST.S_KEY][(int)e_Parameter_Barcode.Log_File]);
            }
            catch
            {
                Log.Write(call, text);
            }
        }

        private static void Kiosk_LogMsgEvent(string call, string text)
        {
            try
            {
                Log.Write(call, text, GlobalVariable.Parameter[(int)e_Parameter.Kiosk][CONST.S_KEY][(int)e_Parameter_Kiosk.Log_File], GlobalVariable.Parameter[(int)e_Parameter.Kiosk][CONST.S_KEY][(int)e_Parameter_Kiosk.Log_View] == e_ComboBox_Use.Use.ToString());
            }
            catch
            {
                Log.Write(call, text);
            }
        }

        private static void Remote_LogMsgEvent(string call, string text)
        {
            try
            {
                Log.Write(call, text, GlobalVariable.Parameter[(int)e_Parameter.Remote][CONST.S_KEY][(int)e_Parameter_Remote.Log_File], GlobalVariable.Parameter[(int)e_Parameter.Remote][CONST.S_KEY][(int)e_Parameter_Remote.Log_View] == e_ComboBox_Use.Use.ToString());
            }
            catch
            {
                Log.Write(call, text);
            }
        }

        #endregion
    }
}
