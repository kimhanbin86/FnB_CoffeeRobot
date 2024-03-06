using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Follettorobotics.HappyBones.Booth.Gen2
{
    #region enum

    public enum e_Device_Robot
    {
        DOOSAN,
    }

    public enum e_Device_Robot_Control
    {
        Clear,
        Stop,
        Initialize,
        Clean,
        Maintenance,
    }

    public enum e_Device_Robot_Command
    {
        #region Data2

        스팀피처_Pickup_요청,
        스팀피처_세척_위치_이동_요청,
        스팀피처_Place_요청,
        컵1_Pickup_요청,
        컵2_Pickup_요청,
        컵3_Pickup_요청,
        컵4_Pickup_요청,

        #endregion
        #region Data3

        커피머신_Hot_Water_위치_이동_요청,
        커피머신_Coffee_위치_이동_요청,
        커피머신_Coffee_위치에서_컵_재정렬_요청,
        제빙기_위치_이동_요청,
        소스_공급기_위치_이동_요청,
        소스_공급기에_컵_Place_요청,
        소스_공급기에서_컵_Pickup_요청,

        #endregion
        #region Data4

        도어1에_컵_Place_요청,
        도어2에_컵_Place_요청,
        도어3에_컵_Place_요청,
        도어4에_컵_Place_요청,
        도어4_텀블러에_음료_따르기_요청,

        #endregion
        #region Data5

        테이블1에_컵_Place_요청,
        테이블2에_컵_Place_요청,
        테이블3에_컵_Place_요청,
        테이블1에서_컵_Pickup_요청,
        테이블2에서_컵_Pickup_요청,
        테이블3에서_컵_Pickup_요청,
        음료_버리기_요청,
        컵_버리기_요청,

        #endregion
        #region Data6

        도어1에서_컵_Pickup_요청,
        도어2에서_컵_Pickup_요청,
        도어3에서_컵_Pickup_요청,
        도어4에서_컵_Pickup_요청,

        #endregion
    }

    public enum e_Device_Robot_Cup
    {
        Undefined,
        스팀피처,
        컵1,
        컵2,
        컵3,
        컵4,
    }

    public enum e_Device_Robot_Feedback
    {
        D1B0_Running,
        D1B1_,
        D1B2_Initialize,
        D1B3_Clean,
        D1B4_Servo_ON,
        D1B5_Maintenance,
        D1B6_,
        D1B7_,
        D2B0_스팀피처_Pickup_완료,
        D2B1_스팀피처_세척_위치_이동_완료,
        D2B2_스팀피처_Place_완료,
        D2B3_에이드_음료,
        D2B4_컵1_Pickup_완료,
        D2B5_컵2_Pickup_완료,
        D2B6_컵3_Pickup_완료,
        D2B7_컵4_Pickup_완료,
        D3B0_대기_위치,
        D3B1_커피머신_Hot_Water_위치_이동_완료,
        D3B2_커피머신_Coffee_위치_이동_완료,
        D3B3_커피머신_Coffee_위치에서_컵_재정렬_완료,
        D3B4_제빙기_위치_이동_완료,
        D3B5_소스_공급기_위치_이동_완료,
        D3B6_소스_공급기에_컵_Place_완료,
        D3B7_소스_공급기에서_컵_Pickup_완료,
        D4B0_도어1에_컵_Place_완료,
        D4B1_도어2에_컵_Place_완료,
        D4B2_도어3에_컵_Place_완료,
        D4B3_도어4에_컵_Place_완료,
        D4B4_도어4_텀블러에_음료_따르기_완료,
        D4B5_스팀피처,
        D4B6_HOT_컵,
        D4B7_ICE_컵,
        D5B0_테이블1에_컵_Place_완료,
        D5B1_테이블2에_컵_Place_완료,
        D5B2_테이블3에_컵_Place_완료,
        D5B3_테이블1에서_컵_Pickup_완료,
        D5B4_테이블2에서_컵_Pickup_완료,
        D5B5_테이블3에서_컵_Pickup_완료,
        D5B6_음료_버리기_완료,
        D5B7_컵_버리기_완료,
        D6B0_도어1에서_컵_Pickup_완료,
        D6B1_도어2에서_컵_Pickup_완료,
        D6B2_도어3에서_컵_Pickup_완료,
        D6B3_도어4에서_컵_Pickup_완료,
        D6B4_,
        D6B5_,
        D6B6_,
        D6B7_,
        D7B0_,
        D7B1_,
        D7B2_,
        D7B3_,
        D7B4_,
        D7B5_,
        D7B6_,
        D7B7_,
        D8B0_,
        D8B1_,
        D8B2_,
        D8B3_,
        D8B4_,
        D8B5_,
        D8B6_,
        D8B7_,
        D9B0_,
        D9B1_,
        D9B2_,
        D9B3_,
        D9B4_,
        D9B5_,
        D9B6_,
        D9B7_,
    }

    #endregion

    public interface IRobot
    {
        #region AClass

        bool LogEnabled { get; set; }

        event Library.LogMsgEventHandler LogMsgEvent;

        #endregion

        #region ADevice

        string Device { get; set; }

        #endregion

        #region ASocketClient

        bool IsConnected { get; }
        string IP { get; set; }
        int Port { get; set; }
        int ConnectTimeout { get; set; }
        int ReceiveTimeout { get; set; }
        int ReceiveBufferSize { get; set; }
        int SendTimeout { get; set; }
        int SendBufferSize { get; set; }
        System.Net.Sockets.ProtocolType ClientType { get; set; }

        #endregion

        #region SocketClient

        void Dispose();

        bool Connect();

        #endregion

        bool GetStatus(ref byte[] bytes);

        bool SetRobot(e_Device_Robot_Control control);

        bool SetRobot(e_Device_Robot_Command command, e_Device_Robot_Cup cup, bool ade);
    }
}
