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
        private System.Threading.Thread _ThreadController2 = null;
        private bool _isThreadController2 = false;
        private void Process_Controller2()
        {
            while (_isThreadController2)
            {
                try
                {
                    if (GlobalFunction.GetEnabled(e_Parameter.Controller2) == false)
                    {
                        continue;
                    }

                    if (GlobalDevice.Controller2.Instance != null)
                    {
                        if (GlobalDevice.Controller2.Instance.IsOpen)
                        {
                            byte[] bytes = null;

                            if (GlobalDevice.Controller2.Status.Comm = GlobalDevice.Controller2.Instance.GetSensor(ref bytes))
                            {
                                byte mask = 0x01;

                                for (int data = 0; data < bytes.Length; data++)
                                {
                                    for (int bit = 0; bit < 8; bit++)
                                    {
                                        GlobalDevice.Controller2.Sensor[data * 8 + bit] = ((bytes[data] >> bit) & mask) == mask;
                                    }
                                }
                            }
                        }
                    }

                    #region Status

                    GlobalDevice.Controller2.Status.StatusBase = GlobalDevice.Controller2.Instance != null && GlobalDevice.Controller2.Instance.IsOpen && GlobalDevice.Controller2.Status.Comm;

                    GlobalDevice.Controller2.Status.Status = GlobalDevice.Controller2.Status.StatusBase;

                    #endregion

                    #region Alarm

                    GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_COMM_CONTROLLER2] = !GlobalDevice.Controller2.Status.Comm;

                    GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_STATUS_CONTROLLER2] = !GlobalDevice.Controller2.Status.Status;

                    GlobalVariable.Alarm[(int)e_Alarm.SAUCE1_EMPTY] = !GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D1B3_Sauce1];
                    GlobalVariable.Alarm[(int)e_Alarm.SAUCE2_EMPTY] = !GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D1B2_Sauce2];
                    GlobalVariable.Alarm[(int)e_Alarm.SAUCE3_EMPTY] = !GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D1B1_Sauce3];
                    GlobalVariable.Alarm[(int)e_Alarm.SAUCE4_EMPTY] = !GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D1B0_Sauce4];
                    GlobalVariable.Alarm[(int)e_Alarm.SAUCE5_EMPTY] = !GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D2B3_Sauce5];
                    GlobalVariable.Alarm[(int)e_Alarm.SAUCE6_EMPTY] = !GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D2B2_Sauce6];

                    #endregion

                    if (GlobalDevice.Controller2.Status.StatusBase == false)
                    {
                        GlobalDevice.Stop(e_Device.Controller2);

                        GlobalDevice.Start(e_Device.Controller2);
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
