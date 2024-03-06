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
    public class CIceMaker_ICETRO : RS232, IIceMaker
    {
        #region const

        private const byte c_STX = 0x7A;
        private const byte c_ETX = 0x7B;

        #endregion

        #region protocol, command

        private enum e_Protocol
        {
            STX,
            CMD1,
            CMD2,
            CMD3,
            ETX,
        }

        private enum e_Command
        {
            상태문의 = 0x10,
            판매개시 = 0x11,
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

        private byte GetByte(string time)
        {
            byte result = 0x00;
            try
            {
                if (string.IsNullOrEmpty(time) == false)
                {
                    if (double.TryParse(time, out double value))
                    {
                        value = Math.Truncate(value * 10);

                        if (value < 0)
                        {
                            value = 0;
                        }
                        else if (value > 255)
                        {
                            value = 255;
                        }

                        result = Convert.ToByte(value);
                    }
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
                result = GlobalVariable.Parameter[(int)e_Parameter.IceMaker][CONST.S_KEY][(int)e_Parameter_IceMaker.LogEnabled_Process] == e_ComboBox_Use.Use.ToString();
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
                result[(int)e_Protocol.STX] = c_STX;
                result[(int)e_Protocol.ETX] = c_ETX;
            }
            catch (Exception ex)
            {
                LogWrite(MethodBase.GetCurrentMethod().Name, Utility.GetString(ex));
            }
            return result;
        }
        private byte[] MakeCommand(e_Command command, string iceTime = "", string waterTime = "")
        {
            byte[] result = MakeCommand();
            try
            {
                result[(int)e_Protocol.CMD1] = (byte)command;

                switch (command)
                {
                    case e_Command.판매개시:
                        result[(int)e_Protocol.CMD2] = GetByte(iceTime);
                        result[(int)e_Protocol.CMD3] = GetByte(waterTime);
                        break;
                }
            }
            catch (Exception ex)
            {
                LogWrite(MethodBase.GetCurrentMethod().Name, Utility.GetString(ex));
            }
            return result;
        }

        public bool GetStatus(ref byte[] bytes)
        {
            lock (_lockObject)
            {
                bool result = false;
                try
                {
                    if (Write(MakeCommand(e_Command.상태문의), MethodBase.GetCurrentMethod().Name, GetLogEnabled_Process()))
                    {
                        byte[] ack = null;

                        _stopwatchTimeout.Restart();

                        while (_stopwatchTimeout.ElapsedMilliseconds <= _timeout)
                        {
                            if (BytesToRead >= Enum.GetNames(typeof(e_Protocol)).Length)
                            {
                                if (BytesToRead % Enum.GetNames(typeof(e_Protocol)).Length == 0)
                                {
                                    if (Read(ref ack, MethodBase.GetCurrentMethod().Name, GetLogEnabled_Process()))
                                    {
                                        if (ack[(int)e_Protocol.STX] == c_STX &&
                                            ack[(int)e_Protocol.CMD3] == 0x00 &&
                                            ack[(int)e_Protocol.ETX] == c_ETX
                                           )
                                        {
                                            bytes = new byte[2];

                                            for (int i = 0; i < bytes.Length; i++)
                                            {
                                                bytes[i] = ack[i + (int)e_Protocol.CMD1];
                                            }

                                            result = true;
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

        public bool SetTime(string iceTime, string waterTime)
        {
            lock (_lockObject)
            {
                bool result = false;
                try
                {
                    if (Write(MakeCommand(e_Command.판매개시, iceTime, waterTime), MethodBase.GetCurrentMethod().Name))
                    {
                        result = true;
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
