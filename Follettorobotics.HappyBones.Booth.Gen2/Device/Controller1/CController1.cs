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

    public enum e_Device_Controller1_Door_Command
    {
        Close,
        Open,
        Tumbler_Home,
        Tumbler_Ready,
        Tumbler_Check,
    }

    public enum e_Device_Controller1_Sensor
    {
        D1B0_Door1_Open,
        D1B1_Door2_Open,
        D1B2_Door3_Open,
        D1B3_Door4_Open,
        D1B4_Door1_Close,
        D1B5_Door2_Close,
        D1B6_Door3_Close,
        D1B7_Door4_Close,
        D2B0_텀블러높이,
        D2B1_LM_Down,
        D2B2_LM_Up,
        D2B3_,
        D2B4_Coffee4,
        D2B5_Coffee3,
        D2B6_Coffee2,
        D2B7_Coffee1,
        D3B0_Cup1,
        D3B1_Cup2,
        D3B2_Cup3,
        D3B3_Cup4,
        D3B4_,
        D3B5_,
        D3B6_Milk,
        D3B7_NBox,
    }

    public enum e_Device_Controller1_Sol
    {
        Sol1 = 0x01, // 
        Sol2 = 0x02, // 
        Sol3 = 0x03, // 
        Sol4 = 0x04, // 로봇 Servo ON
        Sol5 = 0x05, // 로봇 프로그램 Play
        Sol6 = 0x06, // 제빙기 레버
    }

    public enum e_Device_Controller1_Sol_Trigger
    {
        OFF = 0x00,
        ON = 0xFF,
    }

    #endregion

    public class CController1 : RS232
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
            D0 = 0xD0,
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

        private bool GetLogEnabled_Process()
        {
            bool result = false;
            try
            {
                result = GlobalVariable.Parameter[(int)e_Parameter.Controller1][CONST.S_KEY][(int)e_Parameter_Controller1.LogEnabled_Process] == e_ComboBox_Use.Use.ToString();
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
        private byte[] MakeCommand(e_Door door, e_Device_Controller1_Door_Command command, ref byte Data1, ref byte Data2)
        {
            byte[] result = MakeCommand(e_Command.D0);
            try
            {
                switch (door)
                {
                    case e_Door.Door1:
                        switch (command)
                        {
                            case e_Device_Controller1_Door_Command.Close: result[(int)e_Protocol.Data1] = 0x10; break;
                            case e_Device_Controller1_Door_Command.Open : result[(int)e_Protocol.Data1] = 0x11; break;
                        }
                        break;
                    case e_Door.Door2:
                        switch (command)
                        {
                            case e_Device_Controller1_Door_Command.Close: result[(int)e_Protocol.Data1] = 0x20; break;
                            case e_Device_Controller1_Door_Command.Open : result[(int)e_Protocol.Data1] = 0x21; break;
                        }
                        break;
                    case e_Door.Door3:
                        switch (command)
                        {
                            case e_Device_Controller1_Door_Command.Close: result[(int)e_Protocol.Data1] = 0x30; break;
                            case e_Device_Controller1_Door_Command.Open : result[(int)e_Protocol.Data1] = 0x31; break;
                        }
                        break;
                    case e_Door.Door4:
                        switch (command)
                        {
                            case e_Device_Controller1_Door_Command.Close: result[(int)e_Protocol.Data1] = 0x60; break;
                            case e_Device_Controller1_Door_Command.Open : result[(int)e_Protocol.Data1] = 0x61; break;
                            case e_Device_Controller1_Door_Command.Tumbler_Home:
                                result[(int)e_Protocol.Data1] = 0x40;
                                result[(int)e_Protocol.Data2] = 0x40;
                                break;
                            case e_Device_Controller1_Door_Command.Tumbler_Ready:
                                result[(int)e_Protocol.Data1] = 0x41;
                                result[(int)e_Protocol.Data2] = 0x41;
                                break;
                            case e_Device_Controller1_Door_Command.Tumbler_Check:
                                result[(int)e_Protocol.Data1] = 0x50;
                                result[(int)e_Protocol.Data2] = 0x50;
                                break;
                        }
                        break;
                }

                Data1 = result[(int)e_Protocol.Data1];
                Data2 = result[(int)e_Protocol.Data2];
            }
            catch (Exception ex)
            {
                LogWrite(MethodBase.GetCurrentMethod().Name, Utility.GetString(ex));
            }
            return result;
        }
        private byte[] MakeCommand(e_Device_Controller1_Sol sol, e_Device_Controller1_Sol_Trigger trigger)
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
                                        ack[(int)e_Protocol.Data4] == 0x00 &&
                                        ack[(int)e_Protocol.EOT] == c_EOT
                                       )
                                    {
                                        bytes = new byte[3];

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

        public bool SetDoor(e_Door door, e_Device_Controller1_Door_Command command)
        {
            lock (_lockObject)
            {
                bool result = false;
                try
                {
                    byte Data1 = 0x00;
                    byte Data2 = 0x00;

                    if (Write(MakeCommand(door, command, ref Data1, ref Data2), MethodBase.GetCurrentMethod().Name))
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
                                        ack[(int)e_Protocol.Command] == 0xD0 &&
                                        ack[(int)e_Protocol.Data1] == Data1 &&
                                        ack[(int)e_Protocol.Data2] == Data2 &&
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

        public bool SetSol(e_Device_Controller1_Sol sol, e_Device_Controller1_Sol_Trigger trigger)
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
