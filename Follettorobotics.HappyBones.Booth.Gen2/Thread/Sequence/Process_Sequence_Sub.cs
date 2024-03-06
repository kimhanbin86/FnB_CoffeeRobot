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
        #region enum

        private enum e_Sequence_Sub
        {
            Sub00,
            Sub01,
            Sub02,
            Sub03,
            Sub04,
            Sub05,
            Sub06,
            Sub07,
            Sub08,
            Sub09,
            Sub10,
            Sub11,
            Sub12,
            Sub13,
            Sub14,
            Sub15,
            Sub16,
            Sub17,
            Sub18,
            Sub19,
        }
        private e_Sequence_Sub _prev_Sequence_Sub = e_Sequence_Sub.Sub00;
        private e_Sequence_Sub _curr_Sequence_Sub = e_Sequence_Sub.Sub00;

        #endregion

        #region 필드

        private bool _logSub00 = false;

        private System.Diagnostics.Stopwatch _stopwatchSub = new System.Diagnostics.Stopwatch();

        #endregion

        private bool Process_Sequence_Sub_텀블러()
        {
            string call = "Sequence_Sub_텀블러";

            bool result = false;
            try
            {
                #region prev != curr

                if (_prev_Sequence_Sub != _curr_Sequence_Sub)
                {
                    Log.Write(call, $"[{_prev_Sequence_Sub}] Complete");
                    _prev_Sequence_Sub = _curr_Sequence_Sub;
                    string text = $"[{_curr_Sequence_Sub}] Start - ";
                    switch (_curr_Sequence_Sub)
                    {
                        case e_Sequence_Sub.Sub01: text += $"Door4 Open"; break;
                        case e_Sequence_Sub.Sub02: text += $"감지 센서 확인"; break;
                        case e_Sequence_Sub.Sub03: text += $"5초 대기"; break;
                        case e_Sequence_Sub.Sub04: text += $"Door4 Close"; break;
                        case e_Sequence_Sub.Sub05: text += $"Door4 Tumbler_Check"; break;
                        case e_Sequence_Sub.Sub06: text += $"Door4 Tumbler_Ready"; break;
                        case e_Sequence_Sub.Sub07: text += $""; break;
                        case e_Sequence_Sub.Sub08: text += $""; break;
                        case e_Sequence_Sub.Sub09: text += $""; break;
                        case e_Sequence_Sub.Sub10: text += $""; break;
                        case e_Sequence_Sub.Sub11: text += $""; break;
                        case e_Sequence_Sub.Sub12: text += $""; break;
                        case e_Sequence_Sub.Sub13: text += $""; break;
                        case e_Sequence_Sub.Sub14: text += $""; break;
                        case e_Sequence_Sub.Sub15: text += $""; break;
                        case e_Sequence_Sub.Sub16: text += $""; break;
                        case e_Sequence_Sub.Sub17: text += $""; break;
                        case e_Sequence_Sub.Sub18: text += $""; break;
                        case e_Sequence_Sub.Sub19: text += $""; break;
                    }
                    Log.Write(call, text);

                    GlobalFunction.DB.MySQL.Query($"UPDATE {e_DB._Order} SET {e_DB_Order.Column03}='{_curr_Sequence_Sub}' WHERE {e_DB_Order.Column01}='{_ID}'");
                }

                #endregion

                // ..

                if (_logSub00)
                {
                    _logSub00 = false;

                    Log.Write(call, $"[{_curr_Sequence_Sub}] Start - Door4 Tumbler_Home");
                }

                switch (_curr_Sequence_Sub)
                {
                    case e_Sequence_Sub.Sub00:
                        if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B2_LM_Up] == false)
                        {
                            if (GlobalDevice.Controller1.Instance.SetDoor(e_Door.Door4, e_Device_Controller1_Door_Command.Tumbler_Home))
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub01;
                            }
                        }
                        else
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub01;
                        }
                        break;
                    case e_Sequence_Sub.Sub01:
                        if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B3_Door4_Open] == false)
                        {
                            if (GlobalDevice.Controller1.Instance.SetDoor(e_Door.Door4, e_Device_Controller1_Door_Command.Open))
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub02;
                            }
                        }
                        else
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub02;
                        }
                        break;
                    case e_Sequence_Sub.Sub02:
                        if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B4_Coffee4])
                        {
                            _stopwatchSub.Restart();

                            _curr_Sequence_Sub = e_Sequence_Sub.Sub03;
                        }
                        break;
                    case e_Sequence_Sub.Sub03:
                        if (_stopwatchSub.ElapsedMilliseconds >= 1000 * 5)
                        {
                            _stopwatchSub.Stop();

                            if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B4_Coffee4])
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub04;
                            }
                            else
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub02;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub04:
                        if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B7_Door4_Close] == false)
                        {
                            if (GlobalDevice.Controller1.Instance.SetDoor(e_Door.Door4, e_Device_Controller1_Door_Command.Close))
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub05;
                            }
                        }
                        else
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub05;
                        }
                        break;
                    case e_Sequence_Sub.Sub05:
                        if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B2_LM_Up])
                        {
                            if (GlobalDevice.Controller1.Instance.SetDoor(e_Door.Door4, e_Device_Controller1_Door_Command.Tumbler_Check))
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub06;
                            }
                        }
                        else
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub06;
                        }
                        break;
                    case e_Sequence_Sub.Sub06:
                        if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B2_LM_Up] == false)
                        {
                            if (GlobalDevice.Controller1.Instance.SetDoor(e_Door.Door4, e_Device_Controller1_Door_Command.Tumbler_Ready))
                            {
                                result = true;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub07:
                        break;
                    case e_Sequence_Sub.Sub08:
                        break;
                    case e_Sequence_Sub.Sub09:
                        break;
                    case e_Sequence_Sub.Sub10:
                        break;
                    case e_Sequence_Sub.Sub11:
                        break;
                    case e_Sequence_Sub.Sub12:
                        break;
                    case e_Sequence_Sub.Sub13:
                        break;
                    case e_Sequence_Sub.Sub14:
                        break;
                    case e_Sequence_Sub.Sub15:
                        break;
                    case e_Sequence_Sub.Sub16:
                        break;
                    case e_Sequence_Sub.Sub17:
                        break;
                    case e_Sequence_Sub.Sub18:
                        break;
                    case e_Sequence_Sub.Sub19:
                        break;
                }

                if (result)
                {
                    Log.Write(call, $"[{_curr_Sequence_Sub}] Complete");
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        private bool Process_Sequence_Sub_픽업(e_Order_Cup Order_Cup, e_ComboBox_Cup Cup, ref e_Device_Robot_Cup Robot_Cup, bool ade)
        {
            string call = "Sequence_Sub_픽업";

            bool result = false;
            try
            {
                #region prev != curr

                if (_prev_Sequence_Sub != _curr_Sequence_Sub)
                {
                    Log.Write(call, $"[{_prev_Sequence_Sub}] Complete");
                    _prev_Sequence_Sub = _curr_Sequence_Sub;
                    string text = $"[{_curr_Sequence_Sub}] Start - ";
                    switch (_curr_Sequence_Sub)
                    {
                        case e_Sequence_Sub.Sub01: text += $"스팀피처 또는 컵 Pickup 요청"; break;
                        case e_Sequence_Sub.Sub02: text += $"스팀피처 또는 컵 Pickup 확인"; break;
                        case e_Sequence_Sub.Sub03: text += $""; break;
                        case e_Sequence_Sub.Sub04: text += $""; break;
                        case e_Sequence_Sub.Sub05: text += $""; break;
                        case e_Sequence_Sub.Sub06: text += $""; break;
                        case e_Sequence_Sub.Sub07: text += $""; break;
                        case e_Sequence_Sub.Sub08: text += $""; break;
                        case e_Sequence_Sub.Sub09: text += $""; break;
                        case e_Sequence_Sub.Sub10: text += $""; break;
                        case e_Sequence_Sub.Sub11: text += $""; break;
                        case e_Sequence_Sub.Sub12: text += $""; break;
                        case e_Sequence_Sub.Sub13: text += $""; break;
                        case e_Sequence_Sub.Sub14: text += $""; break;
                        case e_Sequence_Sub.Sub15: text += $""; break;
                        case e_Sequence_Sub.Sub16: text += $""; break;
                        case e_Sequence_Sub.Sub17: text += $""; break;
                        case e_Sequence_Sub.Sub18: text += $""; break;
                        case e_Sequence_Sub.Sub19: text += $""; break;
                    }
                    Log.Write(call, text);

                    GlobalFunction.DB.MySQL.Query($"UPDATE {e_DB._Order} SET {e_DB_Order.Column03}='{_curr_Sequence_Sub}' WHERE {e_DB_Order.Column01}='{_ID}'");
                }

                #endregion

                // ..

                if (_logSub00 && _curr_Sequence_Sub == e_Sequence_Sub.Sub00)
                {
                    _logSub00 = false;

                    Log.Write(call, $"[{_curr_Sequence_Sub}] Start - 스팀피처 또는 컵 결정");
                }

                switch (_curr_Sequence_Sub)
                {
                    case e_Sequence_Sub.Sub00:
                        switch (Order_Cup)
                        {
                            case e_Order_Cup.TAKEOUT:
                                switch (Cup)
                                {
                                    case e_ComboBox_Cup.Hot:
                                        if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B0_Cup1])
                                        {
                                            Robot_Cup = e_Device_Robot_Cup.컵1;
                                        }
                                        else if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B1_Cup2])
                                        {
                                            Robot_Cup = e_Device_Robot_Cup.컵2;
                                        }
                                        break;
                                    case e_ComboBox_Cup.Ice:
                                        if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B2_Cup3])
                                        {
                                            Robot_Cup = e_Device_Robot_Cup.컵3;
                                        }
                                        else if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B3_Cup4])
                                        {
                                            Robot_Cup = e_Device_Robot_Cup.컵4;
                                        }
                                        break;
                                }
                                break;
                            case e_Order_Cup.TUMBLER:
                                Robot_Cup = e_Device_Robot_Cup.스팀피처;
                                break;
                        }

                        if (Robot_Cup > e_Device_Robot_Cup.Undefined)
                        {
                            Log.Write(call, $"Robot_Cup=[{Robot_Cup}]");

                            _curr_Sequence_Sub = e_Sequence_Sub.Sub01;
                        }
                        break;
                    case e_Sequence_Sub.Sub01:
                        bool robot = false;

                        switch (Robot_Cup)
                        {
                            case e_Device_Robot_Cup.스팀피처: robot = GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.스팀피처_Pickup_요청, Robot_Cup, ade); break;

                            case e_Device_Robot_Cup.컵1: robot = GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.컵1_Pickup_요청, Robot_Cup, ade); break;
                            case e_Device_Robot_Cup.컵2: robot = GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.컵2_Pickup_요청, Robot_Cup, ade); break;
                            case e_Device_Robot_Cup.컵3: robot = GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.컵3_Pickup_요청, Robot_Cup, ade); break;
                            case e_Device_Robot_Cup.컵4: robot = GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.컵4_Pickup_요청, Robot_Cup, ade); break;
                        }

                        if (robot)
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub02;
                        }
                        break;
                    case e_Sequence_Sub.Sub02:
                        robot = false;

                        switch (Robot_Cup)
                        {
                            case e_Device_Robot_Cup.스팀피처: robot = GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D2B0_스팀피처_Pickup_완료]; break;

                            case e_Device_Robot_Cup.컵1: robot = GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D2B4_컵1_Pickup_완료]; break;
                            case e_Device_Robot_Cup.컵2: robot = GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D2B5_컵2_Pickup_완료]; break;
                            case e_Device_Robot_Cup.컵3: robot = GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D2B6_컵3_Pickup_완료]; break;
                            case e_Device_Robot_Cup.컵4: robot = GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D2B7_컵4_Pickup_완료]; break;
                        }

                        if (robot)
                        {
                            result = true;
                        }
                        break;
                    case e_Sequence_Sub.Sub03:
                        break;
                    case e_Sequence_Sub.Sub04:
                        break;
                    case e_Sequence_Sub.Sub05:
                        break;
                    case e_Sequence_Sub.Sub06:
                        break;
                    case e_Sequence_Sub.Sub07:
                        break;
                    case e_Sequence_Sub.Sub08:
                        break;
                    case e_Sequence_Sub.Sub09:
                        break;
                    case e_Sequence_Sub.Sub10:
                        break;
                    case e_Sequence_Sub.Sub11:
                        break;
                    case e_Sequence_Sub.Sub12:
                        break;
                    case e_Sequence_Sub.Sub13:
                        break;
                    case e_Sequence_Sub.Sub14:
                        break;
                    case e_Sequence_Sub.Sub15:
                        break;
                    case e_Sequence_Sub.Sub16:
                        break;
                    case e_Sequence_Sub.Sub17:
                        break;
                    case e_Sequence_Sub.Sub18:
                        break;
                    case e_Sequence_Sub.Sub19:
                        break;
                }

                if (result)
                {
                    if (_curr_Sequence_Tumbler > e_Sequence_Sub.Sub00)
                    {
                        result = false;

                        if (_logSub00 == false)
                        {
                            _logSub00 = true;

                            Log.Write(call, $"텀블러 대기 중");
                        }
                    }
                }

                if (result)
                {
                    Log.Write(call, $"[{_curr_Sequence_Sub}] Complete");
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        private bool Process_Sequence_Sub_스팀피처(e_Device_Robot_Cup Robot_Cup, string Ice_Time, bool ade)
        {
            string call = "Sequence_Sub_스팀피처";

            bool result = false;
            try
            {
                #region prev != curr

                if (_prev_Sequence_Sub != _curr_Sequence_Sub)
                {
                    Log.Write(call, $"[{_prev_Sequence_Sub}] Complete");
                    _prev_Sequence_Sub = _curr_Sequence_Sub;
                    string text = $"[{_curr_Sequence_Sub}] Start - ";
                    switch (_curr_Sequence_Sub)
                    {
                        case e_Sequence_Sub.Sub01: text += $"제빙기 위치 이동 확인"; break;
                        case e_Sequence_Sub.Sub02: text += $"얼음 받기 시작"; break;
                        case e_Sequence_Sub.Sub03: text += $"얼음 받기 완료"; break;
                        case e_Sequence_Sub.Sub04: text += $"텀블러에 음료 따르기 요청"; break;
                        case e_Sequence_Sub.Sub05: text += $"텀블러에 음료 따르기 확인"; break;
                        case e_Sequence_Sub.Sub06: text += $"스팀피처 세척 위치 이동 요청"; break;
                        case e_Sequence_Sub.Sub07: text += $"스팀피처 세척 위치 이동 확인"; break;
                        case e_Sequence_Sub.Sub08: text += $"스팀피처 Place 요청"; break;
                        case e_Sequence_Sub.Sub09: text += $"스팀피처 Place 확인"; break;
                        case e_Sequence_Sub.Sub10: text += $"스팀피처 Pickup 요청"; break;
                        case e_Sequence_Sub.Sub11: text += $"스팀피처 Pickup 확인"; break;
                        case e_Sequence_Sub.Sub12: text += $""; break;
                        case e_Sequence_Sub.Sub13: text += $""; break;
                        case e_Sequence_Sub.Sub14: text += $""; break;
                        case e_Sequence_Sub.Sub15: text += $""; break;
                        case e_Sequence_Sub.Sub16: text += $""; break;
                        case e_Sequence_Sub.Sub17: text += $""; break;
                        case e_Sequence_Sub.Sub18: text += $""; break;
                        case e_Sequence_Sub.Sub19: text += $""; break;
                    }
                    Log.Write(call, text);

                    GlobalFunction.DB.MySQL.Query($"UPDATE {e_DB._Order} SET {e_DB_Order.Column03}='{_curr_Sequence_Sub}' WHERE {e_DB_Order.Column01}='{_ID}'");
                }

                #endregion

                // ..

                if (_logSub00)
                {
                    _logSub00 = false;

                    Log.Write(call, $"[{_curr_Sequence_Sub}] Start - 제빙기 위치 이동 요청");
                }

                switch (_curr_Sequence_Sub)
                {
                    case e_Sequence_Sub.Sub00:
                        if (GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.제빙기_위치_이동_요청, Robot_Cup, ade))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub01;
                        }
                        break;
                    case e_Sequence_Sub.Sub01:
                        if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D3B4_제빙기_위치_이동_완료])
                        {
                            if (GlobalFunction.GetSimulation())
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub04;
                            }
                            else
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub02;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub02:
                        if (GlobalDevice.IceMaker.Instance.SetTime(Ice_Time, string.Empty))
                        {
                            GlobalDevice.Controller1.Instance.SetSol(e_Device_Controller1_Sol.Sol6, e_Device_Controller1_Sol_Trigger.ON);
                            {
                                _stopwatchSub.Restart();

                                _curr_Sequence_Sub = e_Sequence_Sub.Sub03;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub03:
                        double ice = double.TryParse(Ice_Time, out ice) ? ice : 0;

                        if (_stopwatchSub.ElapsedMilliseconds >= 1000 * ice)
                        {
                            _stopwatchSub.Stop();

                            if (GlobalDevice.Controller1.Instance.SetSol(e_Device_Controller1_Sol.Sol6, e_Device_Controller1_Sol_Trigger.OFF))
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub04;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub04:
                        if (GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.도어4_텀블러에_음료_따르기_요청, Robot_Cup, ade))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub05;
                        }
                        break;
                    case e_Sequence_Sub.Sub05:
                        if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D4B4_도어4_텀블러에_음료_따르기_완료])
                        {
                            // 얼음 받아서 따른 건데 세척이 필요하나?
                            //_curr_Sequence_Sub = e_Sequence_Sub.Sub06;

                            result = true;
                        }
                        break;
                    case e_Sequence_Sub.Sub06:
                        if (GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.스팀피처_세척_위치_이동_요청, Robot_Cup, ade))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub07;
                        }
                        break;
                    case e_Sequence_Sub.Sub07:
                        if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D2B1_스팀피처_세척_위치_이동_완료])
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub08;
                        }
                        break;
                    case e_Sequence_Sub.Sub08:
                        if (GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.스팀피처_Place_요청, Robot_Cup, ade))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub09;
                        }
                        break;
                    case e_Sequence_Sub.Sub09:
                        if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D2B2_스팀피처_Place_완료])
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub10;
                        }
                        break;
                    case e_Sequence_Sub.Sub10:
                        if (GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.스팀피처_Pickup_요청, Robot_Cup, ade))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub11;
                        }
                        break;
                    case e_Sequence_Sub.Sub11:
                        if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D2B0_스팀피처_Pickup_완료])
                        {
                            result = true;
                        }
                        break;
                    case e_Sequence_Sub.Sub12:
                        break;
                    case e_Sequence_Sub.Sub13:
                        break;
                    case e_Sequence_Sub.Sub14:
                        break;
                    case e_Sequence_Sub.Sub15:
                        break;
                    case e_Sequence_Sub.Sub16:
                        break;
                    case e_Sequence_Sub.Sub17:
                        break;
                    case e_Sequence_Sub.Sub18:
                        break;
                    case e_Sequence_Sub.Sub19:
                        break;
                }

                if (result)
                {
                    Log.Write(call, $"[{_curr_Sequence_Sub}] Complete");
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        private bool Process_Sequence_Sub_소스(e_Device_Robot_Cup Robot_Cup, e_ComboBox_Sauce_Type Sauce_Type, e_Device_Controller2_Sauce Sauce_No, string Pumping_Count, string Mixing_Count, bool ade)
        {
            string call = "Sequence_Sub_소스";

            bool result = false;
            try
            {
                #region prev != curr

                if (_prev_Sequence_Sub != _curr_Sequence_Sub)
                {
                    Log.Write(call, $"[{_prev_Sequence_Sub}] Complete");
                    _prev_Sequence_Sub = _curr_Sequence_Sub;
                    string text = $"[{_curr_Sequence_Sub}] Start - ";
                    switch (_curr_Sequence_Sub)
                    {
                        case e_Sequence_Sub.Sub01: text += $"커피머신 Coffee 위치 이동 요청"; break;
                        case e_Sequence_Sub.Sub02: text += $"커피머신 Coffee 위치 이동 확인"; break;
                        case e_Sequence_Sub.Sub03: text += $"커피머신(CoffeeMilkL) Idle 상태 및 HOT(에스프레소 또는 우유) 소량 스레드 종료 확인"; break;
                        case e_Sequence_Sub.Sub04: text += $"제빙기 위치 이동 요청"; break;
                        case e_Sequence_Sub.Sub05: text += $"제빙기 위치 이동 확인"; break;
                        case e_Sequence_Sub.Sub06: text += $"탄산수 소량 받기 시작"; break;
                        case e_Sequence_Sub.Sub07: text += $"탄산수 소량 받기 완료"; break;
                        case e_Sequence_Sub.Sub08: text += $""; break;
                        case e_Sequence_Sub.Sub09: text += $"소스 공급기 위치 이동 요청"; break;
                        case e_Sequence_Sub.Sub10: text += $"소스 공급기 위치 이동 확인"; break;
                        case e_Sequence_Sub.Sub11: text += $"소스 공급기에 컵 Place 요청"; break;
                        case e_Sequence_Sub.Sub12: text += $"소스 공급기에 컵 Place 확인"; break;
                        case e_Sequence_Sub.Sub13: text += $"소스 공급기 회전"; break;
                        case e_Sequence_Sub.Sub14: text += $"소스 공급기 펌핑 및 믹싱"; break;
                        case e_Sequence_Sub.Sub15: text += $"소스 공급기에서 컵 Pickup 요청"; break;
                        case e_Sequence_Sub.Sub16: text += $"소스 공급기에서 컵 Pickup 확인"; break;
                        case e_Sequence_Sub.Sub17: text += $""; break;
                        case e_Sequence_Sub.Sub18: text += $""; break;
                        case e_Sequence_Sub.Sub19: text += $""; break;
                    }
                    Log.Write(call, text);

                    GlobalFunction.DB.MySQL.Query($"UPDATE {e_DB._Order} SET {e_DB_Order.Column03}='{_curr_Sequence_Sub}' WHERE {e_DB_Order.Column01}='{_ID}'");
                }

                #endregion

                // ..

                if (_logSub00)
                {
                    _logSub00 = false;

                    Log.Write(call, $"[{_curr_Sequence_Sub}] Start - ");
                }

                switch (_curr_Sequence_Sub)
                {
                    case e_Sequence_Sub.Sub00:
                        switch (Robot_Cup)
                        {
                            case e_Device_Robot_Cup.스팀피처:
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub09;
                                break;
                            case e_Device_Robot_Cup.컵1:
                            case e_Device_Robot_Cup.컵2:
                            case e_Device_Robot_Cup.컵3:
                            case e_Device_Robot_Cup.컵4:
                                switch (Sauce_Type)
                                {
                                    case e_ComboBox_Sauce_Type.Syrup:
                                        _curr_Sequence_Sub = e_Sequence_Sub.Sub01;
                                        break;
                                    case e_ComboBox_Sauce_Type.Ade:
                                        _curr_Sequence_Sub = e_Sequence_Sub.Sub04;
                                        break;
                                }
                                break;
                        }
                        break;
                    case e_Sequence_Sub.Sub01:
                        if (GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.커피머신_Coffee_위치_이동_요청, Robot_Cup, ade))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub02;
                        }
                        break;
                    case e_Sequence_Sub.Sub02:
                        if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D3B2_커피머신_Coffee_위치_이동_완료])
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub03;
                        }
                        break;
                    case e_Sequence_Sub.Sub03:
                        if (GlobalFunction.CoffeeMaker.CoffeeMilkL.CheckIdle(call) && _curr_Sequence_Little == e_Sequence_Sub.Sub00)
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub08;
                        }
                        break;
                    case e_Sequence_Sub.Sub04:
                        if (GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.제빙기_위치_이동_요청, Robot_Cup, ade))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub05;
                        }
                        break;
                    case e_Sequence_Sub.Sub05:
                        if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D3B4_제빙기_위치_이동_완료])
                        {
                            if (GlobalFunction.GetSimulation())
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub08;
                            }
                            else
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub06;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub06:
                        if (GlobalDevice.IceMaker.Instance.SetTime(string.Empty, "1"))
                        {
                            GlobalDevice.Controller1.Instance.SetSol(e_Device_Controller1_Sol.Sol6, e_Device_Controller1_Sol_Trigger.ON);
                            {
                                _stopwatchSub.Restart();

                                _curr_Sequence_Sub = e_Sequence_Sub.Sub07;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub07:
                        if (_stopwatchSub.ElapsedMilliseconds >= 1000 * 1)
                        {
                            _stopwatchSub.Stop();

                            if (GlobalDevice.Controller1.Instance.SetSol(e_Device_Controller1_Sol.Sol6, e_Device_Controller1_Sol_Trigger.OFF))
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub08;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub08:
                        _curr_Sequence_Sub = e_Sequence_Sub.Sub11;
                        break;
                    case e_Sequence_Sub.Sub09:
                        if (GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.소스_공급기_위치_이동_요청, Robot_Cup, ade))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub10;
                        }
                        break;
                    case e_Sequence_Sub.Sub10:
                        if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D3B5_소스_공급기_위치_이동_완료])
                        {
                            if (GlobalFunction.GetSimulation())
                            {
                                result = true;
                            }
                            else
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub13;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub11:
                        if (GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.소스_공급기에_컵_Place_요청, Robot_Cup, ade))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub12;
                        }
                        break;
                    case e_Sequence_Sub.Sub12:
                        if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D3B6_소스_공급기에_컵_Place_완료])
                        {
                            if (GlobalFunction.GetSimulation())
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub15;
                            }
                            else
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub13;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub13:
                        if (GlobalDevice.Controller2.Instance.SetMotor(e_Device_Controller2_Motor_Command.회전_Move, Sauce_No))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub14;
                        }
                        break;
                    case e_Sequence_Sub.Sub14:
                        //if (GlobalDevice.Controller2.Instance.SetMotor(e_Device_Controller2_Motor_Command.펌핑_Push, Pumping_Count, Robot_Cup == e_Device_Robot_Cup.스팀피처 ? string.Empty : Mixing_Count))
                        //{
                        //    switch (Robot_Cup)
                        //    {
                        //        case e_Device_Robot_Cup.스팀피처:
                        //            result = true;
                        //            break;
                        //        case e_Device_Robot_Cup.컵1:
                        //        case e_Device_Robot_Cup.컵2:
                        //        case e_Device_Robot_Cup.컵3:
                        //        case e_Device_Robot_Cup.컵4:
                        //            _curr_Sequence_Sub = e_Sequence_Sub.Sub15;
                        //            break;
                        //    }
                        //}

                        if (GlobalDevice.Controller2.Instance.SetMotor(e_Device_Controller2_Motor_Command.펌핑_Push, Pumping_Count))
                        {
                            switch (Robot_Cup)
                            {
                                case e_Device_Robot_Cup.스팀피처:
                                    result = true;
                                    break;
                                case e_Device_Robot_Cup.컵1:
                                case e_Device_Robot_Cup.컵2:
                                case e_Device_Robot_Cup.컵3:
                                case e_Device_Robot_Cup.컵4:
                                    _curr_Sequence_Sub = e_Sequence_Sub.Sub17;
                                    break;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub15:
                        if (GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.소스_공급기에서_컵_Pickup_요청, Robot_Cup, ade))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub16;
                        }
                        break;
                    case e_Sequence_Sub.Sub16:
                        if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D3B7_소스_공급기에서_컵_Pickup_완료])
                        {
                            result = true;
                        }
                        break;
                    case e_Sequence_Sub.Sub17:
                        if (GlobalDevice.Controller2.Instance.SetMotor(e_Device_Controller2_Motor_Command.믹서_회전, Mixing_Count))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub15;
                        }
                        break;
                    case e_Sequence_Sub.Sub18:
                        break;
                    case e_Sequence_Sub.Sub19:
                        break;
                }

                if (result)
                {
                    Log.Write(call, $"[{_curr_Sequence_Sub}] Complete");
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        private bool Process_Sequence_Sub_얼음(e_Device_Robot_Cup Robot_Cup, e_ComboBox_Sauce_Type Sauce_Type, string Ice_Time, string Water_Time, string Product_Code, bool ade)
        {
            string call = "Sequence_Sub_얼음";

            bool result = false;
            try
            {
                #region prev != curr

                if (_prev_Sequence_Sub != _curr_Sequence_Sub)
                {
                    Log.Write(call, $"[{_prev_Sequence_Sub}] Complete");
                    _prev_Sequence_Sub = _curr_Sequence_Sub;
                    string text = $"[{_curr_Sequence_Sub}] Start - ";
                    switch (_curr_Sequence_Sub)
                    {
                        case e_Sequence_Sub.Sub01: text += $"제빙기 위치 이동 확인"; break;
                        case e_Sequence_Sub.Sub02: text += $""; break;
                        case e_Sequence_Sub.Sub03: text += $"얼음, 탄산수 받기 시작"; break;
                        case e_Sequence_Sub.Sub04: text += $"얼음, 탄산수 받기 완료"; break;
                        case e_Sequence_Sub.Sub05: text += $"얼음, (정수물) 받기 시작"; break;
                        case e_Sequence_Sub.Sub06: text += $"얼음, (정수물) 받기 완료"; break;
                        case e_Sequence_Sub.Sub07: text += $""; break;
                        case e_Sequence_Sub.Sub08: text += $"텀블러에 음료 따르기 요청"; break;
                        case e_Sequence_Sub.Sub09: text += $"텀블러에 음료 따르기 확인"; break;
                        case e_Sequence_Sub.Sub10: text += $""; break;
                        case e_Sequence_Sub.Sub11: text += $""; break;
                        case e_Sequence_Sub.Sub12: text += $""; break;
                        case e_Sequence_Sub.Sub13: text += $""; break;
                        case e_Sequence_Sub.Sub14: text += $""; break;
                        case e_Sequence_Sub.Sub15: text += $""; break;
                        case e_Sequence_Sub.Sub16: text += $""; break;
                        case e_Sequence_Sub.Sub17: text += $""; break;
                        case e_Sequence_Sub.Sub18: text += $""; break;
                        case e_Sequence_Sub.Sub19: text += $""; break;
                    }
                    Log.Write(call, text);

                    GlobalFunction.DB.MySQL.Query($"UPDATE {e_DB._Order} SET {e_DB_Order.Column03}='{_curr_Sequence_Sub}' WHERE {e_DB_Order.Column01}='{_ID}'");
                }

                #endregion

                // ..

                if (_logSub00)
                {
                    _logSub00 = false;

                    Log.Write(call, $"[{_curr_Sequence_Sub}] Start - 제빙기 위치 이동 요청");
                }

                switch (_curr_Sequence_Sub)
                {
                    case e_Sequence_Sub.Sub00:
                        if (GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.제빙기_위치_이동_요청, Robot_Cup, ade))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub01;
                        }
                        break;
                    case e_Sequence_Sub.Sub01:
                        if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D3B4_제빙기_위치_이동_완료])
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub02;
                        }
                        break;
                    case e_Sequence_Sub.Sub02:
                        if (Sauce_Type == e_ComboBox_Sauce_Type.Ade)
                        {
                            if (GlobalFunction.GetSimulation())
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub07;
                            }
                            else
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub03;
                            }
                        }
                        else
                        {
                            if (GlobalFunction.GetSimulation())
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub07;
                            }
                            else
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub05;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub03:
                        if (GlobalDevice.IceMaker.Instance.SetTime(Ice_Time, Water_Time))
                        {
                            GlobalDevice.Controller1.Instance.SetSol(e_Device_Controller1_Sol.Sol6, e_Device_Controller1_Sol_Trigger.ON);
                            {
                                _stopwatchSub.Restart();

                                _curr_Sequence_Sub = e_Sequence_Sub.Sub04;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub04:
                        double ice = double.TryParse(Ice_Time, out ice) ? ice : 0;
                        double water = double.TryParse(Water_Time, out water) ? water : 0;
                        double delay = ice >= water ? ice : water;

                        if (_stopwatchSub.ElapsedMilliseconds >= 1000 * delay)
                        {
                            _stopwatchSub.Stop();

                            if (GlobalDevice.Controller1.Instance.SetSol(e_Device_Controller1_Sol.Sol6, e_Device_Controller1_Sol_Trigger.OFF))
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub07;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub05:
                        bool onlyIce = false;

                        if (Product_Code == "01-I" ||
                            Product_Code == "02-I"
                           )
                        {
                            switch (Robot_Cup)
                            {
                                case e_Device_Robot_Cup.스팀피처:
                                    onlyIce = true;
                                    break;
                            }
                        }

                        if (GlobalDevice.IceMaker.Instance.SetTime(Ice_Time, onlyIce ? string.Empty : Water_Time))
                        {
                            GlobalDevice.Controller1.Instance.SetSol(e_Device_Controller1_Sol.Sol6, e_Device_Controller1_Sol_Trigger.ON);
                            {
                                _stopwatchSub.Restart();

                                _curr_Sequence_Sub = e_Sequence_Sub.Sub06;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub06:
                        ice = double.TryParse(Ice_Time, out ice) ? ice : 0;
                        water = double.TryParse(Water_Time, out water) ? water : 0;
                        delay = ice >= water ? ice : water;

                        if (_stopwatchSub.ElapsedMilliseconds >= 1000 * delay)
                        {
                            _stopwatchSub.Stop();

                            if (GlobalDevice.Controller1.Instance.SetSol(e_Device_Controller1_Sol.Sol6, e_Device_Controller1_Sol_Trigger.OFF))
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub07;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub07:
                        switch (Robot_Cup)
                        {
                            case e_Device_Robot_Cup.스팀피처:
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub08;
                                break;
                            case e_Device_Robot_Cup.컵1:
                            case e_Device_Robot_Cup.컵2:
                            case e_Device_Robot_Cup.컵3:
                            case e_Device_Robot_Cup.컵4:
                                result = true;
                                break;
                        }
                        break;
                    case e_Sequence_Sub.Sub08:
                        if (GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.도어4_텀블러에_음료_따르기_요청, Robot_Cup, ade))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub09;
                        }
                        break;
                    case e_Sequence_Sub.Sub09:
                        if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D4B4_도어4_텀블러에_음료_따르기_완료])
                        {
                            result = true;
                        }
                        break;
                    case e_Sequence_Sub.Sub10:
                        break;
                    case e_Sequence_Sub.Sub11:
                        break;
                    case e_Sequence_Sub.Sub12:
                        break;
                    case e_Sequence_Sub.Sub13:
                        break;
                    case e_Sequence_Sub.Sub14:
                        break;
                    case e_Sequence_Sub.Sub15:
                        break;
                    case e_Sequence_Sub.Sub16:
                        break;
                    case e_Sequence_Sub.Sub17:
                        break;
                    case e_Sequence_Sub.Sub18:
                        break;
                    case e_Sequence_Sub.Sub19:
                        break;
                }

                if (result)
                {
                    Log.Write(call, $"[{_curr_Sequence_Sub}] Complete");
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        private bool Process_Sequence_Sub_커피(e_Device_Robot_Cup Robot_Cup, string Product_Code, double End_Delay, string Water_Time, bool ade)
        {
            string call = "Sequence_Sub_커피";

            bool result = false;
            try
            {
                #region prev != curr

                if (_prev_Sequence_Sub != _curr_Sequence_Sub)
                {
                    Log.Write(call, $"[{_prev_Sequence_Sub}] Complete");
                    _prev_Sequence_Sub = _curr_Sequence_Sub;
                    string text = $"[{_curr_Sequence_Sub}] Start - ";
                    switch (_curr_Sequence_Sub)
                    {
                        case e_Sequence_Sub.Sub01: text += $"커피머신 Hot Water 위치 이동 요청"; break;
                        case e_Sequence_Sub.Sub02: text += $"커피머신 Hot Water 위치 이동 확인"; break;
                        case e_Sequence_Sub.Sub03: text += $"커피머신(Water) Idle 상태이면 DoProduct(13)"; break;
                        case e_Sequence_Sub.Sub04: text += $"커피머신(Water) Key 값 확인"; break;
                        case e_Sequence_Sub.Sub05: text += $"커피머신(Water) Idle 상태 확인"; break;
                        case e_Sequence_Sub.Sub06: text += $"커피머신 Coffee 위치 이동 요청"; break;
                        case e_Sequence_Sub.Sub07: text += $"커피머신 Coffee 위치 이동 확인"; break;
                        case e_Sequence_Sub.Sub08: text += $"커피머신(CoffeeMilkL) Idle 상태 및 커피 스레드 종료 확인"; break;
                        case e_Sequence_Sub.Sub09: text += $"End_Delay"; break;
                        case e_Sequence_Sub.Sub10: text += $"커피머신 Coffee 위치에서 컵 재정렬 요청"; break;
                        case e_Sequence_Sub.Sub11: text += $"커피머신 Coffee 위치에서 컵 재정렬 확인"; break;
                        case e_Sequence_Sub.Sub12: text += $"제빙기 위치 이동 요청"; break;
                        case e_Sequence_Sub.Sub13: text += $"제빙기 위치 이동 확인"; break;
                        case e_Sequence_Sub.Sub14: text += $"정수물 받기 시작"; break;
                        case e_Sequence_Sub.Sub15: text += $"정수물 받기 완료"; break;
                        case e_Sequence_Sub.Sub16: text += $"텀블러에 음료 따르기 요청"; break;
                        case e_Sequence_Sub.Sub17: text += $"텀블러에 음료 따르기 확인"; break;
                        case e_Sequence_Sub.Sub18: text += $""; break;
                        case e_Sequence_Sub.Sub19: text += $""; break;
                    }
                    Log.Write(call, text);

                    GlobalFunction.DB.MySQL.Query($"UPDATE {e_DB._Order} SET {e_DB_Order.Column03}='{_curr_Sequence_Sub}' WHERE {e_DB_Order.Column01}='{_ID}'");
                }

                #endregion

                #region Timeout - Hot Water

                if (_curr_Sequence_Sub >= e_Sequence_Sub.Sub03 &&
                    _curr_Sequence_Sub <= e_Sequence_Sub.Sub05
                   )
                {
                    if (_stopwatchSub.ElapsedMilliseconds >= 1000 * (double.TryParse(GlobalVariable.Parameter[_parameterKey][_productKey][(int)e_Parameter_Product.Hot_Water_Timeout], out double Hot_Water_Timeout) ? Hot_Water_Timeout : 12))
                    {
                        Log.Write(call, $"---------------------------------------------------------------------- [Timeout - Hot Water]");

                        _stopwatchSub.Stop();

                        _curr_Sequence_Sub = e_Sequence_Sub.Sub06;

                        return false;
                    }
                }

                #endregion

                // ..

                if (_logSub00)
                {
                    _logSub00 = false;

                    Log.Write(call, $"[{_curr_Sequence_Sub}] Start - ");
                }

                switch (_curr_Sequence_Sub)
                {
                    case e_Sequence_Sub.Sub00:
                        if (Product_Code == "01-H" ||
                            Product_Code == "02-H"
                           )
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub01;
                        }
                        else
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub06;
                        }
                        break;
                    case e_Sequence_Sub.Sub01:
                        if (GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.커피머신_Hot_Water_위치_이동_요청, Robot_Cup, ade))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub02;
                        }
                        break;
                    case e_Sequence_Sub.Sub02:
                        if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D3B1_커피머신_Hot_Water_위치_이동_완료])
                        {
                            if (GlobalFunction.GetSimulation())
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub06;
                            }
                            else
                            {
                                _stopwatchSub.Restart();

                                _curr_Sequence_Sub = e_Sequence_Sub.Sub03;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub03:
                        if (GlobalFunction.CoffeeMaker.Water.CheckIdle(call))
                        {
                            if (GlobalDevice.CoffeeMaker.Instance.DoProduct(CONST.N_EVERSYS_PRODUCT_ID_HOT_WATER))
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub04;
                            }
                        }
                        else
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub04;
                        }
                        break;
                    case e_Sequence_Sub.Sub04:
                        if (GlobalFunction.CoffeeMaker.Water.CheckRun(call, CONST.N_EVERSYS_PRODUCT_ID_HOT_WATER))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub05;
                        }
                        else
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub03;
                        }
                        break;
                    case e_Sequence_Sub.Sub05:
                        if (GlobalFunction.CoffeeMaker.Water.CheckIdle(call))
                        {
                            _stopwatchSub.Stop();

                            _curr_Sequence_Sub = e_Sequence_Sub.Sub06;
                        }
                        break;
                    case e_Sequence_Sub.Sub06:
                        if (GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.커피머신_Coffee_위치_이동_요청, Robot_Cup, ade))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub07;
                        }
                        break;
                    case e_Sequence_Sub.Sub07:
                        if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D3B2_커피머신_Coffee_위치_이동_완료])
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub08;
                        }
                        break;
                    case e_Sequence_Sub.Sub08:
                        if (GlobalFunction.CoffeeMaker.CoffeeMilkL.CheckIdle(call) && _curr_Sequence_Coffee == e_Sequence_Sub.Sub00)
                        {
                            _stopwatchSub.Restart();

                            _curr_Sequence_Sub = e_Sequence_Sub.Sub09;
                        }
                        break;
                    case e_Sequence_Sub.Sub09:
                        if (_stopwatchSub.ElapsedMilliseconds >= 1000 * End_Delay)
                        {
                            _stopwatchSub.Stop();

                            switch (Robot_Cup)
                            {
                                case e_Device_Robot_Cup.스팀피처:
                                    if (Product_Code == "01-I" ||
                                        Product_Code == "02-I"
                                       )
                                    {
                                        _curr_Sequence_Sub = e_Sequence_Sub.Sub12;
                                    }
                                    else
                                    {
                                        _curr_Sequence_Sub = e_Sequence_Sub.Sub16;
                                    }
                                    break;
                                case e_Device_Robot_Cup.컵1:
                                case e_Device_Robot_Cup.컵2:
                                case e_Device_Robot_Cup.컵3:
                                case e_Device_Robot_Cup.컵4:
                                    _curr_Sequence_Sub = e_Sequence_Sub.Sub10;
                                    break;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub10:
                        if (GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.커피머신_Coffee_위치에서_컵_재정렬_요청, Robot_Cup, ade))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub11;
                        }
                        break;
                    case e_Sequence_Sub.Sub11:
                        if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D3B3_커피머신_Coffee_위치에서_컵_재정렬_완료])
                        {
                            result = true;
                        }
                        break;
                    case e_Sequence_Sub.Sub12:
                        if (GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.제빙기_위치_이동_요청, Robot_Cup, ade))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub13;
                        }
                        break;
                    case e_Sequence_Sub.Sub13:
                        if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D3B4_제빙기_위치_이동_완료])
                        {
                            if (GlobalFunction.GetSimulation())
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub16;
                            }
                            else
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub14;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub14:
                        if (GlobalDevice.IceMaker.Instance.SetTime(string.Empty, Water_Time))
                        {
                            GlobalDevice.Controller1.Instance.SetSol(e_Device_Controller1_Sol.Sol6, e_Device_Controller1_Sol_Trigger.ON);
                            {
                                _stopwatchSub.Restart();

                                _curr_Sequence_Sub = e_Sequence_Sub.Sub15;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub15:
                        double water = double.TryParse(Water_Time, out water) ? water : 0;

                        if (_stopwatchSub.ElapsedMilliseconds >= 1000 * water)
                        {
                            _stopwatchSub.Stop();

                            if (GlobalDevice.Controller1.Instance.SetSol(e_Device_Controller1_Sol.Sol6, e_Device_Controller1_Sol_Trigger.OFF))
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub16;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub16:
                        if (GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.도어4_텀블러에_음료_따르기_요청, Robot_Cup, ade))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub17;
                        }
                        break;
                    case e_Sequence_Sub.Sub17:
                        if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D4B4_도어4_텀블러에_음료_따르기_완료])
                        {
                            result = true;
                        }
                        break;
                    case e_Sequence_Sub.Sub18:
                        break;
                    case e_Sequence_Sub.Sub19:
                        break;
                }

                if (result)
                {
                    Log.Write(call, $"[{_curr_Sequence_Sub}] Complete");
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        private bool Process_Sequence_Sub_도어(e_Device_Robot_Cup Robot_Cup, ref e_Door Door, string Barcode, bool ade)
        {
            string call = "Sequence_Sub_도어";

            bool result = false;
            try
            {
                #region prev != curr

                if (_prev_Sequence_Sub != _curr_Sequence_Sub)
                {
                    Log.Write(call, $"[{_prev_Sequence_Sub}] Complete");
                    _prev_Sequence_Sub = _curr_Sequence_Sub;
                    string text = $"[{_curr_Sequence_Sub}] Start - ";
                    switch (_curr_Sequence_Sub)
                    {
                        case e_Sequence_Sub.Sub01: text += $"DB 업데이트"; break;
                        case e_Sequence_Sub.Sub02: text += $"스팀피처 세척 위치 이동 요청"; break;
                        case e_Sequence_Sub.Sub03: text += $"스팀피처 세척 위치 이동 확인"; break;
                        case e_Sequence_Sub.Sub04: text += $"스팀피처 Place 요청"; break;
                        case e_Sequence_Sub.Sub05: text += $"스팀피처 Place 또는 대기 위치 확인"; break;
                        case e_Sequence_Sub.Sub06: text += $"도어 결정"; break;
                        case e_Sequence_Sub.Sub07: text += $"도어에 컵 Place 요청"; break;
                        case e_Sequence_Sub.Sub08: text += $"도어에 컵 Place 확인"; break;
                        case e_Sequence_Sub.Sub09: text += $"감지 센서 확인 (최대 5초)"; break;
                        case e_Sequence_Sub.Sub10: text += $""; break;
                        case e_Sequence_Sub.Sub11: text += $""; break;
                        case e_Sequence_Sub.Sub12: text += $""; break;
                        case e_Sequence_Sub.Sub13: text += $""; break;
                        case e_Sequence_Sub.Sub14: text += $""; break;
                        case e_Sequence_Sub.Sub15: text += $""; break;
                        case e_Sequence_Sub.Sub16: text += $""; break;
                        case e_Sequence_Sub.Sub17: text += $""; break;
                        case e_Sequence_Sub.Sub18: text += $""; break;
                        case e_Sequence_Sub.Sub19: text += $""; break;
                    }
                    Log.Write(call, text);

                    GlobalFunction.DB.MySQL.Query($"UPDATE {e_DB._Order} SET {e_DB_Order.Column03}='{_curr_Sequence_Sub}' WHERE {e_DB_Order.Column01}='{_ID}'");
                }

                #endregion

                // ..

                if (_logSub00)
                {
                    _logSub00 = false;

                    Log.Write(call, $"[{_curr_Sequence_Sub}] Start - ");
                }

                switch (_curr_Sequence_Sub)
                {
                    case e_Sequence_Sub.Sub00:
                        switch (Robot_Cup)
                        {
                            case e_Device_Robot_Cup.스팀피처:
                                Door = e_Door.Door4;

                                Log.Write(call, $"Door=[{Door}]");

                                GlobalFunction.DB.MySQL.Query($"UPDATE {e_DB._Order} SET {e_DB_Order.Column11}='{Door}' WHERE {e_DB_Order.Column01}='{_ID}'");

                                _curr_Sequence_Sub = e_Sequence_Sub.Sub02;

                                CDID.Motion(e_Motion.스팀피처_세척_중);
                                break;
                            case e_Device_Robot_Cup.컵1:
                            case e_Device_Robot_Cup.컵2:
                            case e_Device_Robot_Cup.컵3:
                            case e_Device_Robot_Cup.컵4:
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub06;
                                break;
                        }
                        break;
                    case e_Sequence_Sub.Sub01:
                        if (GlobalFunction.Door.SetDateTime(Door, DateTime.Now) &&
                            GlobalFunction.Door.SetID(Door, _ID) &&
                            GlobalFunction.Door.SetBarcode(Door, Barcode) &&
                            GlobalFunction.Door.SetCup(Door, Robot_Cup == e_Device_Robot_Cup.스팀피처 ? e_Order_Cup.TUMBLER : e_Order_Cup.TAKEOUT) &&
                            GlobalFunction.Door.SetSensor(Door, Robot_Cup == e_Device_Robot_Cup.스팀피처 ? CONST.S_OK : _coffee ? CONST.S_OK : CONST.S_NG) &&
                            GlobalFunction.Door.SetLock(Door, e_Door_Lock.Lock) &&
                            GlobalFunction.Door.SetTrigger(Door, e_Door_Trigger.Set)
                           )
                        {
                            switch (Robot_Cup)
                            {
                                case e_Device_Robot_Cup.스팀피처:
                                    _curr_Sequence_Sub = e_Sequence_Sub.Sub03;
                                    break;
                                case e_Device_Robot_Cup.컵1:
                                case e_Device_Robot_Cup.컵2:
                                case e_Device_Robot_Cup.컵3:
                                case e_Device_Robot_Cup.컵4:
                                    result = true;
                                    break;
                            }
                        }
                        break;
                    case e_Sequence_Sub.Sub02:
                        if (GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.스팀피처_세척_위치_이동_요청, Robot_Cup, ade))
                        {
                            System.Threading.Thread.Sleep(1000 * 10);

                            _curr_Sequence_Sub = e_Sequence_Sub.Sub01;
                        }
                        break;
                    case e_Sequence_Sub.Sub03:
                        if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D2B1_스팀피처_세척_위치_이동_완료])
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub04;
                        }
                        break;
                    case e_Sequence_Sub.Sub04:
                        if (GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.스팀피처_Place_요청, Robot_Cup, ade))
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub05;
                        }
                        break;
                    case e_Sequence_Sub.Sub05:
                        if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D2B2_스팀피처_Place_완료] || GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D3B0_대기_위치])
                        {
                            result = true;
                        }
                        break;
                    case e_Sequence_Sub.Sub06:
                        Random r = new Random();
                        foreach (int i in Enumerable.Range((int)e_Door.Door1, (int)e_Door.Door3).OrderBy(x => r.Next()))
                        {
                            if (GlobalFunction.Door.GetLock((e_Door)i) == e_Door_Lock.Unlock)
                            {
                                Door = (e_Door)i;

                                break;
                            }
                        }

                        if (Door == e_Door.Undefined)
                        {
                            if (GlobalFunction.Door.GetLock(e_Door.Door4) == e_Door_Lock.Unlock)
                            {
                                Door = e_Door.Door4;
                            }
                        }

                        if (Door > e_Door.Undefined)
                        {
                            bool sensor = false;

                            switch (Door)
                            {
                                case e_Door.Door1: sensor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B4_Door1_Close] && !GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B7_Coffee1]; break;
                                case e_Door.Door2: sensor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B5_Door2_Close] && !GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B6_Coffee2]; break;
                                case e_Door.Door3: sensor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B6_Door3_Close] && !GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B5_Coffee3]; break;
                                case e_Door.Door4: sensor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B7_Door4_Close] && !GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B4_Coffee4] && GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B2_LM_Up]; break;
                            }

                            if (sensor == false)
                            {
                                Door = e_Door.Undefined;
                            }
                        }

                        if (Door > e_Door.Undefined)
                        {
                            Log.Write(call, $"Door=[{Door}]");

                            GlobalFunction.DB.MySQL.Query($"UPDATE {e_DB._Order} SET {e_DB_Order.Column11}='{Door}' WHERE {e_DB_Order.Column01}='{_ID}'");

                            _curr_Sequence_Sub = e_Sequence_Sub.Sub07;
                        }
                        break;
                    case e_Sequence_Sub.Sub07:
                        bool robot = false;

                        switch (Door)
                        {
                            case e_Door.Door1: robot = GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.도어1에_컵_Place_요청, Robot_Cup, ade); break;
                            case e_Door.Door2: robot = GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.도어2에_컵_Place_요청, Robot_Cup, ade); break;
                            case e_Door.Door3: robot = GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.도어3에_컵_Place_요청, Robot_Cup, ade); break;
                            case e_Door.Door4: robot = GlobalDevice.Robot.Instance.SetRobot(e_Device_Robot_Command.도어4에_컵_Place_요청, Robot_Cup, ade); break;
                        }

                        if (robot)
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub08;
                        }
                        break;
                    case e_Sequence_Sub.Sub08:
                        robot = false;

                        switch (Door)
                        {
                            case e_Door.Door1: robot = GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D4B0_도어1에_컵_Place_완료] || GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D3B0_대기_위치]; break;
                            case e_Door.Door2: robot = GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D4B1_도어2에_컵_Place_완료] || GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D3B0_대기_위치]; break;
                            case e_Door.Door3: robot = GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D4B2_도어3에_컵_Place_완료] || GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D3B0_대기_위치]; break;
                            case e_Door.Door4: robot = GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D4B3_도어4에_컵_Place_완료] || GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D3B0_대기_위치]; break;
                        }

                        if (robot)
                        {
                            _stopwatchSub.Restart();

                            _curr_Sequence_Sub = e_Sequence_Sub.Sub09;
                        }
                        break;
                    case e_Sequence_Sub.Sub09:
                        #region GetSensor

                        byte[] bytes = null;

                        if (GlobalDevice.Controller1.Status.Comm = GlobalDevice.Controller1.Instance.GetSensor(ref bytes))
                        {
                            byte mask = 0x01;

                            for (int data = 0; data < bytes.Length; data++)
                            {
                                for (int bit = 0; bit < 8; bit++)
                                {
                                    GlobalDevice.Controller1.Sensor[data * 8 + bit] = ((bytes[data] >> bit) & mask) == mask;
                                }
                            }

                            #region 신호 반전

                            GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B0_텀블러높이] = !GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B0_텀블러높이];

                            GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B0_Cup1] = !GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B0_Cup1];
                            GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B1_Cup2] = !GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B1_Cup2];
                            GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B2_Cup3] = !GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B2_Cup3];
                            GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B3_Cup4] = !GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B3_Cup4];

                            #endregion
                        }

                        #endregion

                        switch (Door)
                        {
                            case e_Door.Door1: _coffee = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B7_Coffee1]; break;
                            case e_Door.Door2: _coffee = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B6_Coffee2]; break;
                            case e_Door.Door3: _coffee = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B5_Coffee3]; break;
                            case e_Door.Door4: _coffee = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B4_Coffee4]; break;
                        }

                        if (_coffee)
                        {
                            _curr_Sequence_Sub = e_Sequence_Sub.Sub01;
                        }
                        else
                        {
                            if (_stopwatchSub.ElapsedMilliseconds >= 1000 * 5)
                            {
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub01;
                            }
                        }

                        if (_curr_Sequence_Sub != e_Sequence_Sub.Sub09)
                        {
                            _stopwatchSub.Stop();

                            Log.Write(call, $"{Door} Coffee=[{_coffee}]");
                        }
                        break;
                    case e_Sequence_Sub.Sub10:
                        break;
                    case e_Sequence_Sub.Sub11:
                        break;
                    case e_Sequence_Sub.Sub12:
                        break;
                    case e_Sequence_Sub.Sub13:
                        break;
                    case e_Sequence_Sub.Sub14:
                        break;
                    case e_Sequence_Sub.Sub15:
                        break;
                    case e_Sequence_Sub.Sub16:
                        break;
                    case e_Sequence_Sub.Sub17:
                        break;
                    case e_Sequence_Sub.Sub18:
                        break;
                    case e_Sequence_Sub.Sub19:
                        break;
                }

                if (result)
                {
                    Log.Write(call, $"[{_curr_Sequence_Sub}] Complete");
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }
    }
}
