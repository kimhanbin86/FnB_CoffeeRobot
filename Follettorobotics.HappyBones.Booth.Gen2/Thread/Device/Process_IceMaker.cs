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
        private System.Threading.Thread _ThreadIceMaker = null;
        private bool _isThreadIceMaker = false;
        private void Process_IceMaker()
        {
            while (_isThreadIceMaker)
            {
                try
                {
                    if (GlobalFunction.GetEnabled(e_Parameter.IceMaker) == false)
                    {
                        continue;
                    }

                    if (GlobalDevice.IceMaker.Instance != null)
                    {
                        if (GlobalDevice.IceMaker.Instance.IsOpen)
                        {
                            byte[] bytes = null;

                            if (GlobalDevice.IceMaker.Status.Comm = GlobalDevice.IceMaker.Instance.GetStatus(ref bytes))
                            {
                                switch ((e_Device_IceMaker)Enum.Parse(typeof(e_Device_IceMaker), GlobalVariable.Parameter[(int)e_Parameter.IceMaker][CONST.S_KEY][(int)e_Parameter_IceMaker.Device]))
                                {
                                    case e_Device_IceMaker.ICETRO:
                                        GlobalDevice.IceMaker.Status.MachineCode = Library.Utility.ConvertByteToHex(bytes, 0, 1); // CMD1
                                        GlobalDevice.IceMaker.Status.ErrorCode = Library.Utility.ConvertByteToHex(bytes, 1, 1); // CMD2

                                        int index1 = Array.IndexOf(Enum.GetNames(typeof(e_Alarm)), e_Alarm.ICETRO_CMD2_0x00.ToString());
                                        int index2 = Array.IndexOf(Enum.GetNames(typeof(e_Alarm)), e_Alarm.ICETRO_CMD2_0x0C.ToString());
                                        bool[] array = new bool[index2 - index1 + 1];

                                        if (GlobalDevice.IceMaker.Status.MachineCode == "01")
                                        {
                                            index2 = Array.IndexOf(Enum.GetNames(typeof(e_Alarm)), $"ICETRO_CMD2_0x{GlobalDevice.IceMaker.Status.ErrorCode}");
                                            if (index2 > -1)
                                            {
                                                array[index2 - index1] = true;
                                            }
                                        }

                                        Array.Copy(array, 0, GlobalVariable.Alarm, index1, array.Length);
                                        break;
                                }
                            }
                        }
                    }

                    #region Status

                    GlobalDevice.IceMaker.Status.StatusBase = GlobalDevice.IceMaker.Instance != null && GlobalDevice.IceMaker.Instance.IsOpen && GlobalDevice.IceMaker.Status.Comm;

                    switch ((e_Device_IceMaker)Enum.Parse(typeof(e_Device_IceMaker), GlobalVariable.Parameter[(int)e_Parameter.IceMaker][CONST.S_KEY][(int)e_Parameter_IceMaker.Device]))
                    {
                        case e_Device_IceMaker.ICETRO:
                            GlobalDevice.IceMaker.Status.Run = GlobalDevice.IceMaker.Status.StatusBase && GlobalDevice.IceMaker.Status.MachineCode == "01" && GlobalDevice.IceMaker.Status.ErrorCode == "0C";

                            if (GlobalDevice.IceMaker.Status.Run)
                            {
                                GlobalDevice.IceMaker.Status.Status = true;
                            }
                            else
                            {
                                GlobalDevice.IceMaker.Status.Status = GlobalDevice.IceMaker.Status.StatusBase && (GlobalDevice.IceMaker.Status.MachineCode == "00" || GlobalDevice.IceMaker.Status.MachineCode == "02");
                            }
                            break;
                    }

                    #endregion

                    #region Alarm

                    GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_COMM_ICE_MAKER] = !GlobalDevice.IceMaker.Status.Comm;

                    GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_STATUS_ICE_MAKER] = !GlobalDevice.IceMaker.Status.Status;

                    #endregion

                    if (GlobalDevice.IceMaker.Status.StatusBase == false)
                    {
                        GlobalDevice.Stop(e_Device.IceMaker);

                        GlobalDevice.Start(e_Device.IceMaker);
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
