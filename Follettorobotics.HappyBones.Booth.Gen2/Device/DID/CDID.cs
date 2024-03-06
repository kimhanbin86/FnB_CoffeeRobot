using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using Library.Log;
using Library.Sockets;

namespace Follettorobotics.HappyBones.Booth.Gen2
{
    public enum e_MsgType
    {
        Main,
        Notice,
    }

    public enum e_Motion
    {
        텀블러_투입_0,
        텀블러_투입_1,
        컵_pickup_중,
        소스_펌핑_및_믹싱_중,
        제빙기_투출_중,
        커피_투출_중,
        제조_완료_및_픽업대에_place_중,
        스팀피처_세척_중,
        로봇_초기화_중,
        로봇_세척_자세_중,
        로봇_유지보수_자세_중,
        픽업대에서_음료_pickup_중_OR_예비_테이블에_음료_place_중,
        예비_테이블에서_음료_pickup_중,
        음료_OR_컵_버리기_중,
    }

    public static class CDID
    {
        private const string _ClassName = "CDID::";

        private static SocketClient _Client = null;

        #region public

        public static bool CheckEnabled()
        {
            bool result = false;
            try
            {
                result = GlobalVariable.Parameter[(int)e_Parameter.DID][CONST.S_KEY][(int)e_Parameter_DID.Enabled] == e_ComboBox_Use.Use.ToString();
            }
            catch (Exception ex)
            {
                Log.Write(_ClassName + MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        public static bool CheckProcess()
        {
            bool result = false;
            try
            {
                result = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(GlobalVariable.Parameter[(int)e_Parameter.DID][CONST.S_KEY][(int)e_Parameter_DID.Process_Path])).Length > 0;
            }
            catch (Exception ex)
            {
                Log.Write(_ClassName + MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        public static void Current(string orderNo, string productName)
        {
            try
            {
                if (CheckEnabled())
                {
                    if (CheckProcess())
                    {
                        Send(Encoding.UTF8.GetBytes($"DID|CURRENT|{orderNo}/{productName}|\r\n"));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(_ClassName + MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        public static void Error(string orderNo = "", string productName = "", string text = "")
        {
            try
            {
                if (CheckEnabled())
                {
                    if (CheckProcess())
                    {
                        Send(Encoding.UTF8.GetBytes($"DID|ERROR|{orderNo}/{productName}/{text}|\r\n"));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(_ClassName + MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        public static void Hide()
        {
            try
            {
                if (CheckEnabled())
                {
                    if (CheckProcess())
                    {
                        Send($"DID|HIDE|\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(_ClassName + MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        public static void KillProcess()
        {
            try
            {
                Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(GlobalVariable.Parameter[(int)e_Parameter.DID][CONST.S_KEY][(int)e_Parameter_DID.Process_Path]));

                if (processes.Length > 0)
                {
                    for (int i = 0; i < processes.Length; i++)
                    {
                        processes[i].Kill();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(_ClassName + MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        public static async Task Show()
        {
            try
            {
                if (CheckEnabled())
                {
                    if (CheckProcess())
                    {
                        Send($"DID|SHOW|\r\n");
                    }
                    else
                    {
                        if (StartProcess(GlobalVariable.Parameter[(int)e_Parameter.DID][CONST.S_KEY][(int)e_Parameter_DID.Process_Path]))
                        {
                            await Task.Delay(Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID][CONST.S_KEY][(int)e_Parameter_DID.Process_Start_Delay]));
                        }
                    }

                    Msg(e_MsgType.Main, GlobalVariable.Parameter[(int)e_Parameter.DID][CONST.S_KEY][(int)e_Parameter_DID.Message_1]);

                    Msg(e_MsgType.Notice, GlobalVariable.Parameter[(int)e_Parameter.DID][CONST.S_KEY][(int)e_Parameter_DID.Message_2]);
                }
            }
            catch (Exception ex)
            {
                Log.Write(_ClassName + MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        public static void Waiting(string list = "")
        {
            try
            {
                if (CheckEnabled())
                {
                    if (CheckProcess())
                    {
                        Send($"DID|WAITING|{list}|\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(_ClassName + MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        public static void Motion(e_Motion motion)
        {
            try
            {
                if (CheckEnabled())
                {
                    if (CheckProcess())
                    {
                        Send($"DID|MOTION|{(int)motion}|\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(_ClassName + MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        #endregion

        #region private

        private static void Msg(e_MsgType msg, string text)
        {
            try
            {
                if (CheckProcess())
                {
                    Send(Encoding.UTF8.GetBytes($"DID|MESSAGE|{msg.ToString().ToUpper()}/{text}|\r\n"));
                }
            }
            catch (Exception ex)
            {
                Log.Write(_ClassName + MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        private static bool StartProcess(string path)
        {
            bool result = false;
            try
            {
                result = Process.Start(path).ProcessName == Path.GetFileNameWithoutExtension(path);
            }
            catch (Exception ex)
            {
                Log.Write(_ClassName + MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        public static void Dispose()
        {
            try
            {
                if (_Client != null)
                {
                    _Client.Dispose();
                    _Client = null;
                }
            }
            catch (Exception ex)
            {
                Log.Write(_ClassName + MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }
        public static bool Connect()
        {
            bool result = false;
            try
            {
                if (_Client == null)
                {
                    _Client = new SocketClient();

                    _Client.LogMsgEvent += new Library.LogMsgEventHandler(LogMsgEvent);

                    _Client.LogEnabled = GlobalVariable.Parameter[(int)e_Parameter.DID][CONST.S_KEY][(int)e_Parameter_DID.LogEnabled] == e_ComboBox_Use.Use.ToString();
                    _Client.Device = "DID";
                    _Client.IP = GlobalVariable.Parameter[(int)e_Parameter.DID][CONST.S_KEY][(int)e_Parameter_DID.IP];
                    _Client.Port = Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID][CONST.S_KEY][(int)e_Parameter_DID.Port]);
                    _Client.ConnectTimeout = Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID][CONST.S_KEY][(int)e_Parameter_DID.ConnectTimeout]);

                    result = _Client.Connect();
                }
            }
            catch (Exception ex)
            {
                Log.Write(_ClassName + MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }
        private static void LogMsgEvent(string call, string text)
        {
            Log.Write(call, text);
        }

        private static bool Send(byte[] bytes)
        {
            bool result = false;
            try
            {
                if (Connect())
                {
                    result = _Client.Send(bytes, "UTF-8");
                }
            }
            catch (Exception ex)
            {
                Log.Write(_ClassName + MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            finally
            {
                Dispose();
            }
            return result;
        }
        private static bool Send(string str)
        {
            bool result = false;
            try
            {
                if (Connect())
                {
                    result = _Client.Send(str, "string");
                }
            }
            catch (Exception ex)
            {
                Log.Write(_ClassName + MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            finally
            {
                Dispose();
            }
            return result;
        }

        #endregion

        #region GetWindowPlacement

        public static ShowWindowCommands GetWindowState()
        {
            ShowWindowCommands result = ShowWindowCommands.Normal;
            try
            {
                if (CheckProcess())
                {
                    Process[] processes = Process.GetProcesses();

                    foreach (Process process in processes)
                    {
                        if (process.ProcessName == Path.GetFileNameWithoutExtension(GlobalVariable.Parameter[(int)e_Parameter.DID][CONST.S_KEY][(int)e_Parameter_DID.Process_Path]))
                        {
                            var placement = GetPlacement(process.MainWindowHandle);

                            result = placement.showCmd;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(_ClassName + MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        #region internal

        private static WINDOWPLACEMENT GetPlacement(IntPtr hwnd)
        {
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(hwnd, ref placement);
            return placement;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public ShowWindowCommands showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        public enum ShowWindowCommands : int
        {
            Hide = 0,
            Normal = 1,
            Minimized = 2,
            Maximized = 3,
        }

        #endregion

        #endregion
    }
}
