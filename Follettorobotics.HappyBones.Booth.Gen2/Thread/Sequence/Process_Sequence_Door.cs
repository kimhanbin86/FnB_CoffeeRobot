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

        private enum e_Sequence_Door
        {
            대기,
            텀블러복귀,
            열기,
            픽업대기_센서_OK,
            픽업대기_센서_NG,
            센서확인,
            닫기,
            리셋,
        }

        #endregion

        #region 필드

        #endregion

        private System.Threading.Thread _ThreadSequence_Door = null;
        private bool _isThreadSequence_Door = false;
        private void Process_Sequence_Door(object obj)
        {
            e_Door door = (e_Door)obj;

            string call = $"Sequence_{door}";

            e_Sequence_Door _prev_Sequence_Door = e_Sequence_Door.대기;
            e_Sequence_Door _curr_Sequence_Door = e_Sequence_Door.대기;

            #region local

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

            #endregion

            while (_isThreadSequence_Door)
            {
                try
                {
                    #region prev != curr

                    if (_prev_Sequence_Door != _curr_Sequence_Door)
                    {
                        Log.Write(call, $"----------------------------------------------------------------------");
                        _prev_Sequence_Door = _curr_Sequence_Door;
                        Log.Write(call, $"---------------------------------------------------------------------- [{_curr_Sequence_Door}]");

                        switch (_curr_Sequence_Door)
                        {
                            case e_Sequence_Door.리셋:
                                string ID = GlobalFunction.Door.GetID(door);

                                if (string.IsNullOrEmpty(ID) == false)
                                {
                                    bool @lock = GlobalFunction.Door.GetSensor(door) == CONST.S_NG;

                                    GlobalFunction.DB.MySQL.Query($"UPDATE {e_DB._Order} SET {e_DB_Order.Column02}='{(@lock ? e_Order_Status.강제배출 : e_Order_Status.배출완료)}' WHERE {e_DB_Order.Column01}='{ID}'");
                                }
                                break;
                        }
                    }

                    #endregion

                    #region Stopwatch

                    switch (_curr_Sequence_Door)
                    {
                        case e_Sequence_Door.센서확인:
                            switch (door)
                            {
                                case e_Door.Door1: Invoke(new MethodInvoker(delegate () { lbl_Stopwatch_Door1_1.Text = $"{(double)stopwatch.ElapsedMilliseconds / 1000:0.0}"; })); break;
                                case e_Door.Door2: Invoke(new MethodInvoker(delegate () { lbl_Stopwatch_Door2_1.Text = $"{(double)stopwatch.ElapsedMilliseconds / 1000:0.0}"; })); break;
                                case e_Door.Door3: Invoke(new MethodInvoker(delegate () { lbl_Stopwatch_Door3_1.Text = $"{(double)stopwatch.ElapsedMilliseconds / 1000:0.0}"; })); break;
                                case e_Door.Door4: Invoke(new MethodInvoker(delegate () { lbl_Stopwatch_Door4_1.Text = $"{(double)stopwatch.ElapsedMilliseconds / 1000:0.0}"; })); break;
                            }
                            break;
                        case e_Sequence_Door.픽업대기_센서_NG:
                            switch (door)
                            {
                                case e_Door.Door1: Invoke(new MethodInvoker(delegate () { lbl_Stopwatch_Door1_2.Text = $"{(double)stopwatch.ElapsedMilliseconds / 1000:0.0}"; })); break;
                                case e_Door.Door2: Invoke(new MethodInvoker(delegate () { lbl_Stopwatch_Door2_2.Text = $"{(double)stopwatch.ElapsedMilliseconds / 1000:0.0}"; })); break;
                                case e_Door.Door3: Invoke(new MethodInvoker(delegate () { lbl_Stopwatch_Door3_2.Text = $"{(double)stopwatch.ElapsedMilliseconds / 1000:0.0}"; })); break;
                                case e_Door.Door4: Invoke(new MethodInvoker(delegate () { lbl_Stopwatch_Door4_2.Text = $"{(double)stopwatch.ElapsedMilliseconds / 1000:0.0}"; })); break;
                            }
                            break;
                    }

                    #endregion

                    switch (_curr_Sequence_Door)
                    {
                        case e_Sequence_Door.대기:
                            if (GlobalFunction.Door.GetLock(door) == e_Door_Lock.Lock && GlobalFunction.Door.GetTrigger(door) == e_Door_Trigger.Set)
                            {
                                _curr_Sequence_Door = e_Sequence_Door.열기;

                                switch (door)
                                {
                                    case e_Door.Door4:
                                        if (GlobalFunction.Door.GetCup(door) == e_Order_Cup.TUMBLER)
                                        {
                                            _curr_Sequence_Door = e_Sequence_Door.텀블러복귀;
                                        }
                                        break;
                                }
                            }
                            break;
                        case e_Sequence_Door.텀블러복귀:
                            if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B2_LM_Up] == false)
                            {
                                if (GlobalDevice.Controller1.Instance.SetDoor(door, e_Device_Controller1_Door_Command.Tumbler_Home))
                                {
                                    _curr_Sequence_Door = e_Sequence_Door.열기;
                                }
                            }
                            else
                            {
                                _curr_Sequence_Door = e_Sequence_Door.열기;
                            }
                            break;
                        case e_Sequence_Door.열기:
                            bool interlock = false;

                            switch (door)
                            {
                                case e_Door.Door1: interlock = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B0_Door1_Open] == false; break;
                                case e_Door.Door2: interlock = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B1_Door2_Open] == false; break;
                                case e_Door.Door3: interlock = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B2_Door3_Open] == false; break;
                                case e_Door.Door4: interlock = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B3_Door4_Open] == false; break;
                            }

                            if (interlock)
                            {
                                if (GlobalDevice.Controller1.Instance.SetDoor(door, e_Device_Controller1_Door_Command.Open))
                                {
                                    if (GlobalFunction.Door.GetSensor(door) == CONST.S_OK)
                                    {
                                        _curr_Sequence_Door = e_Sequence_Door.픽업대기_센서_OK;
                                    }
                                    else
                                    {
                                        stopwatch.Restart();

                                        _curr_Sequence_Door = e_Sequence_Door.픽업대기_센서_NG;
                                    }
                                }
                            }
                            else
                            {
                                if (GlobalFunction.Door.GetSensor(door) == CONST.S_OK)
                                {
                                    _curr_Sequence_Door = e_Sequence_Door.픽업대기_센서_OK;
                                }
                                else
                                {
                                    stopwatch.Restart();

                                    _curr_Sequence_Door = e_Sequence_Door.픽업대기_센서_NG;
                                }
                            }
                            break;
                        case e_Sequence_Door.픽업대기_센서_OK:
                            bool Coffee = true;

                            switch (door)
                            {
                                case e_Door.Door1: Coffee = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B7_Coffee1]; break;
                                case e_Door.Door2: Coffee = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B6_Coffee2]; break;
                                case e_Door.Door3: Coffee = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B5_Coffee3]; break;
                                case e_Door.Door4: Coffee = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B4_Coffee4]; break;
                            }

                            if (Coffee == false)
                            {
                                stopwatch.Restart();

                                _curr_Sequence_Door = e_Sequence_Door.센서확인;
                            }
                            break;
                        case e_Sequence_Door.픽업대기_센서_NG:
                            if (stopwatch.ElapsedMilliseconds >= 1000 * (int.TryParse(GlobalVariable.Parameter[(int)e_Parameter.Door][CONST.S_KEY][(int)e_Parameter_Door.Pickup_Delay_Sensor_NG], out int Pickup_Delay_Sensor_NG) ? Pickup_Delay_Sensor_NG : 50))
                            {
                                stopwatch.Stop();

                                _curr_Sequence_Door = e_Sequence_Door.닫기;
                            }
                            break;
                        case e_Sequence_Door.센서확인:
                            if (stopwatch.ElapsedMilliseconds >= 1000 * (int.TryParse(GlobalVariable.Parameter[(int)e_Parameter.Door][CONST.S_KEY][(int)e_Parameter_Door.Pickup_Delay_Sensor_OK], out int Pickup_Delay_Sensor_OK) ? Pickup_Delay_Sensor_OK : 5))
                            {
                                stopwatch.Stop();

                                Coffee = true;

                                switch (door)
                                {
                                    case e_Door.Door1: Coffee = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B7_Coffee1]; break;
                                    case e_Door.Door2: Coffee = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B6_Coffee2]; break;
                                    case e_Door.Door3: Coffee = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B5_Coffee3]; break;
                                    case e_Door.Door4: Coffee = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B4_Coffee4]; break;
                                }

                                if (Coffee == false)
                                {
                                    _curr_Sequence_Door = e_Sequence_Door.닫기;
                                }
                                else
                                {
                                    _curr_Sequence_Door = e_Sequence_Door.픽업대기_센서_OK;
                                }
                            }
                            break;
                        case e_Sequence_Door.닫기:
                            interlock = false;

                            switch (door)
                            {
                                case e_Door.Door1: interlock = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B4_Door1_Close] == false; break;
                                case e_Door.Door2: interlock = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B5_Door2_Close] == false; break;
                                case e_Door.Door3: interlock = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B6_Door3_Close] == false; break;
                                case e_Door.Door4: interlock = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B7_Door4_Close] == false; break;
                            }

                            if (interlock)
                            {
                                if (GlobalDevice.Controller1.Instance.SetDoor(door, e_Device_Controller1_Door_Command.Close))
                                {
                                    _curr_Sequence_Door = e_Sequence_Door.리셋;
                                }
                            }
                            else
                            {
                                _curr_Sequence_Door = e_Sequence_Door.리셋;
                            }
                            break;
                        case e_Sequence_Door.리셋:
                            bool @lock = GlobalFunction.Door.GetSensor(door) == CONST.S_NG;

                            if (GlobalFunction.Door.Clear(door, @lock))
                            {
                                #region local

                                stopwatch.Reset();

                                #endregion

                                _curr_Sequence_Door = e_Sequence_Door.대기;
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
                }

                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
