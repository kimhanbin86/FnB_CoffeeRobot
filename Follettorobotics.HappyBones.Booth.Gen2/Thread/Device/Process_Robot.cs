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
        private System.Threading.Thread _ThreadRobot = null;
        private bool _isThreadRobot = false;
        private void Process_Robot()
        {
            while (_isThreadRobot)
            {
                try
                {
                    if (GlobalFunction.GetEnabled(e_Parameter.Robot) == false)
                    {
                        continue;
                    }

                    if (GlobalDevice.Robot.Instance != null)
                    {
                        if (GlobalDevice.Robot.Instance.IsConnected)
                        {
                            byte[] bytes = null;

                            if (GlobalDevice.Robot.Status.Comm = GlobalDevice.Robot.Instance.GetStatus(ref bytes))
                            {
                                byte mask = 0x01;

                                for (int data = 0; data < bytes.Length; data++)
                                {
                                    for (int bit = 0; bit < 8; bit++)
                                    {
                                        GlobalDevice.Robot.Feedback[data * 8 + bit] = ((bytes[data] >> bit) & mask) == mask;
                                    }
                                }
                            }
                        }
                    }

                    #region Status

                    GlobalDevice.Robot.Status.StatusBase = GlobalDevice.Robot.Instance != null && GlobalDevice.Robot.Instance.IsConnected && GlobalDevice.Robot.Status.Comm;

                    GlobalDevice.Robot.Status.Status = GlobalDevice.Robot.Status.StatusBase && GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D1B0_Running] && GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D1B4_Servo_ON];

                    #endregion

                    #region Alarm

                    GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_COMM_ROBOT] = !GlobalDevice.Robot.Status.Comm;

                    GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_STATUS_ROBOT] = !GlobalDevice.Robot.Status.Status;

                    #endregion

                    if (GlobalDevice.Robot.Status.StatusBase == false)
                    {
                        GlobalDevice.Stop(e_Device.Robot);

                        GlobalDevice.Start(e_Device.Robot);
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
