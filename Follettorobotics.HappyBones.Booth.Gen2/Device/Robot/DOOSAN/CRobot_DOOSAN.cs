using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using Library;
using Library.Sockets;

namespace Follettorobotics.HappyBones.Booth.Gen2
{
    public class CRobot_DOOSAN : SocketClient, IRobot
    {
        #region const

        private const byte c_STX = 0x02;
        private const byte c_ETX = 0x03;

        private const byte c_PCToRobot = 0x01;
        private const byte c_RobotToPC = 0x00;

        #endregion

        #region protocol

        private enum e_Protocol
        {
            STX,
            Command,
            Data1,
            Data2,
            Data3,
            Data4,
            Data5,
            Data6,
            Data7,
            Data8,
            Data9,
            ETX,
        }

        #endregion

        #region 필드

        private readonly object _lockObject = new object();

        #endregion

        #region 메서드

        private bool GetLogEnabled_Process()
        {
            bool result = false;
            try
            {
                result = GlobalVariable.Parameter[(int)e_Parameter.Robot][CONST.S_KEY][(int)e_Parameter_Robot.LogEnabled_Process] == e_ComboBox_Use.Use.ToString();
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
                result[(int)e_Protocol.Command] = c_PCToRobot;
                result[(int)e_Protocol.ETX] = c_ETX;
            }
            catch (Exception ex)
            {
                LogWrite(MethodBase.GetCurrentMethod().Name, Utility.GetString(ex));
            }
            return result;
        }
        private byte[] MakeCommand(e_Device_Robot_Control control, ref byte Data1)
        {
            byte[] result = MakeCommand();
            try
            {
                switch (control)
                {
                    case e_Device_Robot_Control.Stop       : result[(int)e_Protocol.Data1] |= 0x02; break;
                    case e_Device_Robot_Control.Initialize : result[(int)e_Protocol.Data1] |= 0x04; break;
                    case e_Device_Robot_Control.Clean      : result[(int)e_Protocol.Data1] |= 0x08; break;
                    case e_Device_Robot_Control.Maintenance: result[(int)e_Protocol.Data1] |= 0x20; break;
                }

                Data1 = result[(int)e_Protocol.Data1];
            }
            catch (Exception ex)
            {
                LogWrite(MethodBase.GetCurrentMethod().Name, Utility.GetString(ex));
            }
            return result;
        }
        private byte[] MakeCommand(e_Device_Robot_Command command, e_Device_Robot_Cup cup, bool ade, ref byte Data2, ref byte Data3, ref byte Data4, ref byte Data5, ref byte Data6, ref byte Data7, ref byte Data8, ref byte Data9)
        {
            byte[] result = MakeCommand();
            try
            {
                switch (command)
                {
                    case e_Device_Robot_Command.스팀피처_Pickup_요청                   : result[(int)e_Protocol.Data2] |= 0x01; break;
                    case e_Device_Robot_Command.스팀피처_세척_위치_이동_요청           : result[(int)e_Protocol.Data2] |= 0x02; break;
                    case e_Device_Robot_Command.스팀피처_Place_요청                    : result[(int)e_Protocol.Data2] |= 0x04; break;
                    case e_Device_Robot_Command.컵1_Pickup_요청                        : result[(int)e_Protocol.Data2] |= 0x10; break;
                    case e_Device_Robot_Command.컵2_Pickup_요청                        : result[(int)e_Protocol.Data2] |= 0x20; break;
                    case e_Device_Robot_Command.컵3_Pickup_요청                        : result[(int)e_Protocol.Data2] |= 0x40; break;
                    case e_Device_Robot_Command.컵4_Pickup_요청                        : result[(int)e_Protocol.Data2] |= 0x80; break;

                    case e_Device_Robot_Command.커피머신_Hot_Water_위치_이동_요청      : result[(int)e_Protocol.Data3] |= 0x02; break;
                    case e_Device_Robot_Command.커피머신_Coffee_위치_이동_요청         : result[(int)e_Protocol.Data3] |= 0x04; break;
                    case e_Device_Robot_Command.커피머신_Coffee_위치에서_컵_재정렬_요청: result[(int)e_Protocol.Data3] |= 0x08; break;
                    case e_Device_Robot_Command.제빙기_위치_이동_요청                  : result[(int)e_Protocol.Data3] |= 0x10; break;
                    case e_Device_Robot_Command.소스_공급기_위치_이동_요청             : result[(int)e_Protocol.Data3] |= 0x20; break;
                    case e_Device_Robot_Command.소스_공급기에_컵_Place_요청            : result[(int)e_Protocol.Data3] |= 0x40; break;
                    case e_Device_Robot_Command.소스_공급기에서_컵_Pickup_요청         : result[(int)e_Protocol.Data3] |= 0x80; break;

                    case e_Device_Robot_Command.도어1에_컵_Place_요청                  : result[(int)e_Protocol.Data4] |= 0x01; break;
                    case e_Device_Robot_Command.도어2에_컵_Place_요청                  : result[(int)e_Protocol.Data4] |= 0x02; break;
                    case e_Device_Robot_Command.도어3에_컵_Place_요청                  : result[(int)e_Protocol.Data4] |= 0x04; break;
                    case e_Device_Robot_Command.도어4에_컵_Place_요청                  : result[(int)e_Protocol.Data4] |= 0x08; break;
                    case e_Device_Robot_Command.도어4_텀블러에_음료_따르기_요청        : result[(int)e_Protocol.Data4] |= 0x10; break;

                    case e_Device_Robot_Command.테이블1에_컵_Place_요청                : result[(int)e_Protocol.Data5] |= 0x01; break;
                    case e_Device_Robot_Command.테이블2에_컵_Place_요청                : result[(int)e_Protocol.Data5] |= 0x02; break;
                    case e_Device_Robot_Command.테이블3에_컵_Place_요청                : result[(int)e_Protocol.Data5] |= 0x04; break;
                    case e_Device_Robot_Command.테이블1에서_컵_Pickup_요청             : result[(int)e_Protocol.Data5] |= 0x08; break;
                    case e_Device_Robot_Command.테이블2에서_컵_Pickup_요청             : result[(int)e_Protocol.Data5] |= 0x10; break;
                    case e_Device_Robot_Command.테이블3에서_컵_Pickup_요청             : result[(int)e_Protocol.Data5] |= 0x20; break;
                    case e_Device_Robot_Command.음료_버리기_요청                       : result[(int)e_Protocol.Data5] |= 0x40; break;
                    case e_Device_Robot_Command.컵_버리기_요청                         : result[(int)e_Protocol.Data5] |= 0x80; break;

                    case e_Device_Robot_Command.도어1에서_컵_Pickup_요청               : result[(int)e_Protocol.Data6] |= 0x01; break;
                    case e_Device_Robot_Command.도어2에서_컵_Pickup_요청               : result[(int)e_Protocol.Data6] |= 0x02; break;
                    case e_Device_Robot_Command.도어3에서_컵_Pickup_요청               : result[(int)e_Protocol.Data6] |= 0x04; break;
                    case e_Device_Robot_Command.도어4에서_컵_Pickup_요청               : result[(int)e_Protocol.Data6] |= 0x08; break;
                }

                switch (cup)
                {
                    case e_Device_Robot_Cup.스팀피처:
                        result[(int)e_Protocol.Data4] |= 0x20;
                        break;
                    case e_Device_Robot_Cup.컵1:
                    case e_Device_Robot_Cup.컵2:
                        result[(int)e_Protocol.Data4] |= 0x40;
                        break;
                    case e_Device_Robot_Cup.컵3:
                    case e_Device_Robot_Cup.컵4:
                        result[(int)e_Protocol.Data4] |= 0x80;
                        break;
                }

                if (ade)
                {
                    result[(int)e_Protocol.Data2] |= 0x08;
                }

                Data2 = result[(int)e_Protocol.Data2];
                Data3 = result[(int)e_Protocol.Data3];
                Data4 = result[(int)e_Protocol.Data4];
                Data5 = result[(int)e_Protocol.Data5];
                Data6 = result[(int)e_Protocol.Data6];
                Data7 = result[(int)e_Protocol.Data7];
                Data8 = result[(int)e_Protocol.Data8];
                Data9 = result[(int)e_Protocol.Data9];
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
                    if (Send(new byte[] { 0x02, 0x00, 0x03 }, MethodBase.GetCurrentMethod().Name, GetLogEnabled_Process()))
                    {
                        byte[] ack = null;

                        if (Receive(ref ack, MethodBase.GetCurrentMethod().Name, GetLogEnabled_Process()))
                        {
                            if (ack[(int)e_Protocol.STX] == c_STX &&
                                ack[(int)e_Protocol.Command] == c_RobotToPC &&
                                ack[(int)e_Protocol.ETX] == c_ETX
                               )
                            {
                                bytes = new byte[9];

                                for (int i = 0; i < bytes.Length; i++)
                                {
                                    bytes[i] = ack[i + (int)e_Protocol.Data1];
                                }

                                result = true;
                            }
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

        public bool SetRobot(e_Device_Robot_Control control)
        {
            lock (_lockObject)
            {
                bool result = false;
                try
                {
                    byte Data1 = 0x00;

                    if (Send(MakeCommand(control, ref Data1), MethodBase.GetCurrentMethod().Name))
                    {
                        byte[] ack = null;

                        if (Receive(ref ack, MethodBase.GetCurrentMethod().Name))
                        {
                            if (ack[(int)e_Protocol.STX] == c_STX &&
                                ack[(int)e_Protocol.Command] == c_PCToRobot &&
                                ack[(int)e_Protocol.Data1] == Data1 &&
                                ack[(int)e_Protocol.Data2] == 0x00 &&
                                ack[(int)e_Protocol.Data3] == 0x00 &&
                                ack[(int)e_Protocol.Data4] == 0x00 &&
                                ack[(int)e_Protocol.Data5] == 0x00 &&
                                ack[(int)e_Protocol.Data6] == 0x00 &&
                                ack[(int)e_Protocol.Data7] == 0x00 &&
                                ack[(int)e_Protocol.Data8] == 0x00 &&
                                ack[(int)e_Protocol.Data9] == 0x00 &&
                                ack[(int)e_Protocol.ETX] == c_ETX
                               )
                            {
                                result = true;
                            }
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

        public bool SetRobot(e_Device_Robot_Command command, e_Device_Robot_Cup cup, bool ade)
        {
            lock (_lockObject)
            {
                bool result = false;
                try
                {
                    byte Data2 = 0x00;
                    byte Data3 = 0x00;
                    byte Data4 = 0x00;
                    byte Data5 = 0x00;
                    byte Data6 = 0x00;
                    byte Data7 = 0x00;
                    byte Data8 = 0x00;
                    byte Data9 = 0x00;

                    if (Send(MakeCommand(command, cup, ade, ref Data2, ref Data3, ref Data4, ref Data5, ref Data6, ref Data7, ref Data8, ref Data9), MethodBase.GetCurrentMethod().Name))
                    {
                        byte[] ack = null;

                        if (Receive(ref ack, MethodBase.GetCurrentMethod().Name))
                        {
                            if (ack[(int)e_Protocol.STX] == c_STX &&
                                ack[(int)e_Protocol.Command] == c_PCToRobot &&
                                ack[(int)e_Protocol.Data1] == 0x00 &&
                                ack[(int)e_Protocol.Data2] == Data2 &&
                                ack[(int)e_Protocol.Data3] == Data3 &&
                                ack[(int)e_Protocol.Data4] == Data4 &&
                                ack[(int)e_Protocol.Data5] == Data5 &&
                                ack[(int)e_Protocol.Data6] == Data6 &&
                                ack[(int)e_Protocol.Data7] == Data7 &&
                                ack[(int)e_Protocol.Data8] == Data8 &&
                                ack[(int)e_Protocol.Data9] == Data9 &&
                                ack[(int)e_Protocol.ETX] == c_ETX
                               )
                            {
                                result = true;
                            }
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
