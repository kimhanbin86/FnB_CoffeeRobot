using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using Library;
using Library.SerialPorts;

namespace Follettorobotics.HappyBones.Booth.Gen2
{
    #region enum

    public enum e_Device_Controller2_Motor_Command
    {
        회전_Home = 0x10,
        회전_Move = 0x11,
        펌핑_Home = 0x20,
        펌핑_Push = 0x21,
        믹서_회전 = 0x30,
    }

    public enum e_Device_Controller2_Sauce
    {
        Unused = 0x00,
        Sauce1 = 0x01,
        Sauce2 = 0x02,
        Sauce3 = 0x03,
        Sauce4 = 0x04,
        Sauce5 = 0x05,
        Sauce6 = 0x06,
    }

    public enum e_Device_Controller2_Sensor
    {
        D1B0_Sauce4,
        D1B1_Sauce3,
        D1B2_Sauce2,
        D1B3_Sauce1,
        D1B4_,
        D1B5_,
        D1B6_,
        D1B7_,
        D2B0_펌핑_Home,
        D2B1_회전_Home,
        D2B2_Sauce6,
        D2B3_Sauce5,
        D2B4_,
        D2B5_,
        D2B6_,
        D2B7_,
    }

    public enum e_Device_Controller2_Sol
    {
        Sol1 = 0x01, // 
        Sol2 = 0x02, // 
        Sol3 = 0x03, // 
        Sol4 = 0x04, // 
        Sol5 = 0x05, // 로봇 원격 연결
        Sol6 = 0x06, // 로봇 원격 연결
    }

    public enum e_Device_Controller2_Sol_Trigger
    {
        OFF = 0x00,
        ON = 0xFF,
    }

    #endregion

    public class CController2 : RS232
    {
        #region const

        private const byte c_S = 0x53;
        private const byte c_E = 0x45;
        private const byte c_T = 0x54;

        private const byte c_A = 0x41;
        private const byte c_C = 0x43;
        private const byte c_K = 0x4B;

        private const byte c_EOT = 0x04;

        #endregion

        #region protocol, command

        private enum e_Protocol
        {
            Header1,
            Header2,
            Header3,
            Command,
            Data1,
            Data2,
            Data3,
            Data4,
            EOT,
        }

        private enum e_Command
        {
            A0 = 0xA0,
            DD = 0xDD,
            B0 = 0xB0,
        }

        #endregion

        #region 필드

        private readonly object _lockObject = new object();

        private System.Diagnostics.Stopwatch _stopwatchTimeout = new System.Diagnostics.Stopwatch();

        private int _timeout = 1000;

        #endregion

        #region 속성

        public int Timeout
        {
            set
            {
                _timeout = value;
            }
        }

        #endregion

        #region 메서드

        private byte GetByte(int count)
        {
            byte result = 0x00;
            try
            {
                switch (count)
                {
                    case 01: result = 0x01; break;
                    case 02: result = 0x02; break;
                    case 03: result = 0x03; break;
                    case 04: result = 0x04; break;
                    case 05: result = 0x05; break;
                    case 06: result = 0x06; break;
                    case 07: result = 0x07; break;
                    case 08: result = 0x08; break;
                    case 09: result = 0x09; break;
                    case 10: result = 0x10; break;
                    case 11: result = 0x11; break;
                    case 12: result = 0x12; break;
                    case 13: result = 0x13; break;
                    case 14: result = 0x14; break;
                    case 15: result = 0x15; break;
                    case 16: result = 0x16; break;
                    case 17: result = 0x17; break;
                    case 18: result = 0x18; break;
                    case 19: result = 0x19; break;
                    case 20: result = 0x20; break;
                    case 21: result = 0x21; break;
                    case 22: result = 0x22; break;
                    case 23: result = 0x23; break;
                    case 24: result = 0x24; break;
                    case 25: result = 0x25; break;
                    case 26: result = 0x26; break;
                    case 27: result = 0x27; break;
                    case 28: result = 0x28; break;
                    case 29: result = 0x29; break;
                    case 30: result = 0x30; break;
                    case 31: result = 0x31; break;
                    case 32: result = 0x32; break;
                    case 33: result = 0x33; break;
                    case 34: result = 0x34; break;
                    case 35: result = 0x35; break;
                    case 36: result = 0x36; break;
                    case 37: result = 0x37; break;
                    case 38: result = 0x38; break;
                    case 39: result = 0x39; break;
                    case 40: result = 0x40; break;
                    case 41: result = 0x41; break;
                    case 42: result = 0x42; break;
                    case 43: result = 0x43; break;
                    case 44: result = 0x44; break;
                    case 45: result = 0x45; break;
                    case 46: result = 0x46; break;
                    case 47: result = 0x47; break;
                    case 48: result = 0x48; break;
                    case 49: result = 0x49; break;
                    case 50: result = 0x50; break;
                    case 51: result = 0x51; break;
                    case 52: result = 0x52; break;
                    case 53: result = 0x53; break;
                    case 54: result = 0x54; break;
                    case 55: result = 0x55; break;
                    case 56: result = 0x56; break;
                    case 57: result = 0x57; break;
                    case 58: result = 0x58; break;
                    case 59: result = 0x59; break;
                    case 60: result = 0x60; break;
                    case 61: result = 0x61; break;
                    case 62: result = 0x62; break;
                    case 63: result = 0x63; break;
                    case 64: result = 0x64; break;
                    case 65: result = 0x65; break;
                    case 66: result = 0x66; break;
                    case 67: result = 0x67; break;
                    case 68: result = 0x68; break;
                    case 69: result = 0x69; break;
                    case 70: result = 0x70; break;
                    case 71: result = 0x71; break;
                    case 72: result = 0x72; break;
                    case 73: result = 0x73; break;
                    case 74: result = 0x74; break;
                    case 75: result = 0x75; break;
                    case 76: result = 0x76; break;
                    case 77: result = 0x77; break;
                    case 78: result = 0x78; break;
                    case 79: result = 0x79; break;
                    case 80: result = 0x80; break;
                    case 81: result = 0x81; break;
                    case 82: result = 0x82; break;
                    case 83: result = 0x83; break;
                    case 84: result = 0x84; break;
                    case 85: result = 0x85; break;
                    case 86: result = 0x86; break;
                    case 87: result = 0x87; break;
                    case 88: result = 0x88; break;
                    case 89: result = 0x89; break;
                    case 90: result = 0x90; break;
                    case 91: result = 0x91; break;
                    case 92: result = 0x92; break;
                    case 93: result = 0x93; break;
                    case 94: result = 0x94; break;
                    case 95: result = 0x95; break;
                    case 96: result = 0x96; break;
                    case 97: result = 0x97; break;
                    case 98: result = 0x98; break;
                    case 99: result = 0x99; break;
                }
            }
            catch (Exception ex)
            {
                LogWrite(MethodBase.GetCurrentMethod().Name, Utility.GetString(ex));
            }
            return result;
        }

        private bool GetLogEnabled_Process()
        {
            bool result = false;
            try
            {
                result = GlobalVariable.Parameter[(int)e_Parameter.Controller2][CONST.S_KEY][(int)e_Parameter_Controller2.LogEnabled_Process] == e_ComboBox_Use.Use.ToString();
            }
            catch
            {
            }
            return result;
        }

        private byte[] MakeCommand()
        {
            byte[] result = new byte[Enum.GetNames(typeof(e_Protocol)).Length];
            try
            {
                result[(int)e_Protocol.Header1] = c_S;
                result[(int)e_Protocol.Header2] = c_E;
                result[(int)e_Protocol.Header3] = c_T;
                result[(int)e_Protocol.EOT] = c_EOT;
            }
            catch (Exception ex)
            {
                LogWrite(MethodBase.GetCurrentMethod().Name, Utility.GetString(ex));
            }
            return result;
        }
        private byte[] MakeCommand(e_Command command)
        {
            byte[] result = MakeCommand();
            try
            {
                result[(int)e_Protocol.Command] = (byte)command;
                result[(int)e_Protocol.Data1] = 0x01;
            }
            catch (Exception ex)
            {
                LogWrite(MethodBase.GetCurrentMethod().Name, Utility.GetString(ex));
            }
            return result;
        }
        private byte[] MakeCommand(e_Device_Controller2_Motor_Command command, e_Device_Controller2_Sauce sauce)
        {
            byte[] result = MakeCommand(e_Command.DD);
            try
            {
                result[(int)e_Protocol.Data1] = (byte)command;

                switch (command)
                {
                    case e_Device_Controller2_Motor_Command.회전_Move: result[(int)e_Protocol.Data2] = (byte)sauce; break;
                }
            }
            catch (Exception ex)
            {
                LogWrite(MethodBase.GetCurrentMethod().Name, Utility.GetString(ex));
            }
            return result;
        }
        private byte[] MakeCommand(e_Device_Controller2_Motor_Command command, int count, bool Data3)
        {
            byte[] result = MakeCommand(e_Command.DD);
            try
            {
                result[(int)e_Protocol.Data1] = (byte)command;
                result[(int)e_Protocol.Data2] = GetByte(count);

                if (Data3)
                {
                    result[(int)e_Protocol.Data3] = 0x01;
                }
            }
            catch (Exception ex)
            {
                LogWrite(MethodBase.GetCurrentMethod().Name, Utility.GetString(ex));
            }
            return result;
        }
        private byte[] MakeCommand(e_Device_Controller2_Motor_Command command, int pumpingCount, bool Data3, int mixingCount)
        {
            byte[] result = MakeCommand(command, pumpingCount, Data3);
            try
            {
                result[(int)e_Protocol.Data4] = GetByte(mixingCount);
            }
            catch (Exception ex)
            {
                LogWrite(MethodBase.GetCurrentMethod().Name, Utility.GetString(ex));
            }
            return result;
        }
        private byte[] MakeCommand(e_Device_Controller2_Sol sol, e_Device_Controller2_Sol_Trigger trigger)
        {
            byte[] result = MakeCommand(e_Command.B0);
            try
            {
                result[(int)e_Protocol.Data1] = (byte)sol;
                result[(int)e_Protocol.Data2] = (byte)trigger;
            }
            catch (Exception ex)
            {
                LogWrite(MethodBase.GetCurrentMethod().Name, Utility.GetString(ex));
            }
            return result;
        }

        public bool GetSensor(ref byte[] bytes)
        {
            lock (_lockObject)
            {
                bool result = false;
                try
                {
                    if (Write(MakeCommand(e_Command.A0), MethodBase.GetCurrentMethod().Name, GetLogEnabled_Process()))
                    {
                        byte[] ack = null;

                        _stopwatchTimeout.Restart();

                        while (_stopwatchTimeout.ElapsedMilliseconds <= _timeout)
                        {
                            if (BytesToRead == Enum.GetNames(typeof(e_Protocol)).Length)
                            {
                                if (Read(ref ack, MethodBase.GetCurrentMethod().Name, GetLogEnabled_Process()))
                                {
                                    if (ack[(int)e_Protocol.Header1] == c_A &&
                                        ack[(int)e_Protocol.Header2] == c_C &&
                                        ack[(int)e_Protocol.Header3] == c_K &&
                                        ack[(int)e_Protocol.Command] == 0xC0 &&
                                        ack[(int)e_Protocol.Data3] == 0x00 &&
                                        ack[(int)e_Protocol.Data4] == 0x00 &&
                                        ack[(int)e_Protocol.EOT] == c_EOT
                                       )
                                    {
                                        bytes = new byte[2];

                                        for (int i = 0; i < bytes.Length; i++)
                                        {
                                            bytes[i] = ack[i + (int)e_Protocol.Data1];
                                        }

                                        result = true;
                                    }
                                }

                                break;
                            }

                            System.Threading.Thread.Sleep(10);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogWrite(MethodBase.GetCurrentMethod().Name, Utility.GetString(ex));
                }
                return result;
            }
        }

        public bool SetMotor(e_Device_Controller2_Motor_Command command, e_Device_Controller2_Sauce sauce = e_Device_Controller2_Sauce.Unused)
        {
            lock (_lockObject)
            {
                bool result = false;
                try
                {
                    if (Write(MakeCommand(command, sauce), MethodBase.GetCurrentMethod().Name))
                    {
                        byte[] ack = null;

                        _stopwatchTimeout.Restart();

                        while (_stopwatchTimeout.ElapsedMilliseconds <= _timeout)
                        {
                            if (BytesToRead == Enum.GetNames(typeof(e_Protocol)).Length)
                            {
                                if (Read(ref ack, MethodBase.GetCurrentMethod().Name))
                                {
                                    if (ack[(int)e_Protocol.Header1] == c_A &&
                                        ack[(int)e_Protocol.Header2] == c_C &&
                                        ack[(int)e_Protocol.Header3] == c_K &&
                                        ack[(int)e_Protocol.Command] == 0xDD &&
                                        ack[(int)e_Protocol.Data1] == (byte)command &&
                                        ack[(int)e_Protocol.Data3] == 0x00 &&
                                        ack[(int)e_Protocol.Data4] == 0x00 &&
                                        ack[(int)e_Protocol.EOT] == c_EOT
                                       )
                                    {
                                        switch (command)
                                        {
                                            case e_Device_Controller2_Motor_Command.회전_Move:
                                                if (ack[(int)e_Protocol.Data2] == (byte)sauce)
                                                {
                                                    result = true;
                                                }
                                                break;
                                            default:
                                                if (ack[(int)e_Protocol.Data2] == 0x00)
                                                {
                                                    result = true;
                                                }
                                                break;
                                        }
                                    }
                                }

                                break;
                            }

                            System.Threading.Thread.Sleep(10);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogWrite(MethodBase.GetCurrentMethod().Name, Utility.GetString(ex));
                }
                return result;
            }
        }

        public bool SetMotor(e_Device_Controller2_Motor_Command command, string count, string mixingCount = "")
        {
            lock (_lockObject)
            {
                bool result = false;
                try
                {
                    decimal decimalNumber = 0;

                    #region count

                    int number1 = 0;
                    int number2 = 0;

                    if (decimal.TryParse(count, out decimalNumber))
                    {
                        string decimalString = $"{decimalNumber:0.0}";

                        if (decimalString.Contains(","))
                        {
                            LogWrite("count", $"decimalString=[{decimalString}]");

                            decimalString = decimalString.Replace(",", ".");

                            LogWrite("count", $"decimalString=[{decimalString}]");
                        }

                        int point = decimalString.IndexOf(".");

                        number1 = Convert.ToInt32(decimalString.Substring(0, point));
                        number2 = Convert.ToInt32(decimalString.Substring(point + 1, 1));
                    }

                    #endregion

                    #region mixingCount

                    int number3 = 0;

                    if (decimal.TryParse(mixingCount, out decimalNumber))
                    {
                        string decimalString = $"{decimalNumber:0.0}";

                        if (decimalString.Contains(","))
                        {
                            LogWrite("mixingCount", $"decimalString=[{decimalString}]");

                            decimalString = decimalString.Replace(",", ".");

                            LogWrite("mixingCount", $"decimalString=[{decimalString}]");
                        }

                        int point = decimalString.IndexOf(".");

                        number3 = Convert.ToInt32(decimalString.Substring(0, point));
                    }

                    #endregion

                    #region bytes

                    byte[] bytes = null;

                    if (number3 == 0)
                    {
                        bytes = MakeCommand(command, number1, number2 > 0);
                    }
                    else
                    {
                        bytes = MakeCommand(command, number1, number2 > 0, number3);
                    }

                    #endregion

                    if (Write(bytes, MethodBase.GetCurrentMethod().Name))
                    {
                        byte[] ack = null;

                        _stopwatchTimeout.Restart();

                        while (_stopwatchTimeout.ElapsedMilliseconds <= _timeout)
                        {
                            if (BytesToRead == Enum.GetNames(typeof(e_Protocol)).Length)
                            {
                                if (Read(ref ack, MethodBase.GetCurrentMethod().Name))
                                {
                                    if (ack[(int)e_Protocol.Header1] == c_A &&
                                        ack[(int)e_Protocol.Header2] == c_C &&
                                        ack[(int)e_Protocol.Header3] == c_K &&
                                        ack[(int)e_Protocol.Command] == 0xDD &&
                                        ack[(int)e_Protocol.Data1] == (byte)command &&
                                        ack[(int)e_Protocol.Data2] == GetByte(number1) &&
                                        ack[(int)e_Protocol.Data3] == (number2 > 0 ? 0x01 : 0x00) &&
                                        ack[(int)e_Protocol.Data4] == GetByte(number3) &&
                                        ack[(int)e_Protocol.EOT] == c_EOT
                                       )
                                    {
                                        result = true;
                                    }
                                }

                                break;
                            }

                            System.Threading.Thread.Sleep(10);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogWrite(MethodBase.GetCurrentMethod().Name, Utility.GetString(ex));
                }
                return result;
            }
        }

        public bool SetSol(e_Device_Controller2_Sol sol, e_Device_Controller2_Sol_Trigger trigger)
        {
            lock (_lockObject)
            {
                bool result = false;
                try
                {
                    if (Write(MakeCommand(sol, trigger), MethodBase.GetCurrentMethod().Name))
                    {
                        byte[] ack = null;

                        _stopwatchTimeout.Restart();

                        while (_stopwatchTimeout.ElapsedMilliseconds <= _timeout)
                        {
                            if (BytesToRead == Enum.GetNames(typeof(e_Protocol)).Length)
                            {
                                if (Read(ref ack, MethodBase.GetCurrentMethod().Name))
                                {
                                    if (ack[(int)e_Protocol.Header1] == c_A &&
                                        ack[(int)e_Protocol.Header2] == c_C &&
                                        ack[(int)e_Protocol.Header3] == c_K &&
                                        ack[(int)e_Protocol.Command] == 0xE0 &&
                                        ack[(int)e_Protocol.Data1] == (byte)sol &&
                                        ack[(int)e_Protocol.Data2] == (byte)trigger &&
                                        ack[(int)e_Protocol.Data3] == 0x00 &&
                                        ack[(int)e_Protocol.Data4] == 0x00 &&
                                        ack[(int)e_Protocol.EOT] == c_EOT
                                       )
                                    {
                                        result = true;
                                    }
                                }

                                break;
                            }

                            System.Threading.Thread.Sleep(10);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogWrite(MethodBase.GetCurrentMethod().Name, Utility.GetString(ex));
                }
                return result;
            }
        }

        #endregion
    }
}
