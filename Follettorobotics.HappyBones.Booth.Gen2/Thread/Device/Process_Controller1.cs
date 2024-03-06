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
        private System.Threading.Thread _ThreadController1 = null;
        private bool _isThreadController1 = false;
        private void Process_Controller1()
        {
            while (_isThreadController1)
            {
                try
                {
                    if (GlobalFunction.GetEnabled(e_Parameter.Controller1) == false)
                    {
                        continue;
                    }

                    if (GlobalDevice.Controller1.Instance != null)
                    {
                        if (GlobalDevice.Controller1.Instance.IsOpen)
                        {
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
                        }
                    }

                    #region Status

                    GlobalDevice.Controller1.Status.StatusBase = GlobalDevice.Controller1.Instance != null && GlobalDevice.Controller1.Instance.IsOpen && GlobalDevice.Controller1.Status.Comm;

                    GlobalDevice.Controller1.Status.Status = GlobalDevice.Controller1.Status.StatusBase;

                    #endregion

                    #region Alarm

                    GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_COMM_CONTROLLER1] = !GlobalDevice.Controller1.Status.Comm;

                    GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_STATUS_CONTROLLER1] = !GlobalDevice.Controller1.Status.Status;

                    GlobalVariable.Alarm[(int)e_Alarm.CUP1_EMPTY] = !GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B0_Cup1];
                    GlobalVariable.Alarm[(int)e_Alarm.CUP2_EMPTY] = !GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B1_Cup2];
                    GlobalVariable.Alarm[(int)e_Alarm.CUP3_EMPTY] = !GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B2_Cup3];
                    GlobalVariable.Alarm[(int)e_Alarm.CUP4_EMPTY] = !GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B3_Cup4];

                    GlobalVariable.Alarm[(int)e_Alarm.MILK_EMPTY] = !GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B6_Milk];
                    GlobalVariable.Alarm[(int)e_Alarm.NBOX_ALARM] = !GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B7_NBox];

                    #endregion

                    if (GlobalDevice.Controller1.Status.StatusBase == false)
                    {
                        GlobalDevice.Stop(e_Device.Controller1);

                        GlobalDevice.Start(e_Device.Controller1);
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
