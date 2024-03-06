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
        private Timer _TimerDevice = null;
        private void Tick_Device(object sender, EventArgs e)
        {
            _TimerDevice?.Stop();
            try
            {
                #region Barcode

                if (toolStripStatusLabel_Barcode.Visible = GlobalFunction.GetEnabled(e_Parameter.Barcode))
                {
                    GlobalDevice.Barcode.Status.Status = GlobalDevice.Barcode.Instance != null && GlobalDevice.Barcode.Instance.IsOpen;

                    GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_STATUS_BARCODE] = !GlobalDevice.Barcode.Status.Status;

                    if (GlobalDevice.Barcode.Status.Status)
                    {
                        toolStripStatusLabel_Barcode.BackColor = Color.Lime;
                    }
                    else
                    {
                        toolStripStatusLabel_Barcode.BackColor = Color.Red;

                        GlobalDevice.Stop(e_Device.Barcode);

                        GlobalDevice.Start(e_Device.Barcode);
                    }
                }

                #endregion

                #region Kiosk

                if (toolStripStatusLabel_Kiosk.Visible = GlobalFunction.GetEnabled(e_Parameter.Kiosk))
                {
                    GlobalDevice.Kiosk.Status.Status = GlobalDevice.Kiosk.Instance != null && GlobalDevice.Kiosk.Instance.IsOpen;

                    GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_STATUS_KIOSK] = !GlobalDevice.Kiosk.Status.Status;

                    if (GlobalDevice.Kiosk.Status.Status)
                    {
                        toolStripStatusLabel_Kiosk.BackColor = GlobalDevice.Kiosk.Instance.IsConnected ? Color.Lime : Color.Yellow;
                    }
                    else
                    {
                        toolStripStatusLabel_Kiosk.BackColor = Color.Red;

                        GlobalDevice.Stop(e_Device.Kiosk);

                        GlobalDevice.Start(e_Device.Kiosk);
                    }
                }

                #endregion

                #region Remote

                if (toolStripStatusLabel_Remote.Visible = GlobalFunction.GetEnabled(e_Parameter.Remote))
                {
                    GlobalDevice.Remote.Status.Status = GlobalDevice.Remote.Instance != null && GlobalDevice.Remote.Instance.IsOpen;

                    GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_STATUS_REMOTE] = !GlobalDevice.Remote.Status.Status;

                    if (GlobalDevice.Remote.Status.Status)
                    {
                        toolStripStatusLabel_Remote.BackColor = GlobalDevice.Remote.Instance.IsConnected ? Color.Lime : Color.Yellow;
                    }
                    else
                    {
                        toolStripStatusLabel_Remote.BackColor = Color.Red;

                        GlobalDevice.Stop(e_Device.Remote);

                        GlobalDevice.Start(e_Device.Remote);
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            finally
            {
                _TimerDevice?.Start();
            }
        }
    }
}
