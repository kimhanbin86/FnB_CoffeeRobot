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
        #region Rinse

        private System.Diagnostics.Stopwatch _stopwatchRinse = new System.Diagnostics.Stopwatch();
        private bool _rinse = false;

        #endregion

        private System.Threading.Thread _ThreadCoffeeMaker = null;
        private bool _isThreadCoffeeMaker = false;
        private void Process_CoffeeMaker()
        {
            bool toggle = false;

            System.Diagnostics.Stopwatch stopwatchRinse = new System.Diagnostics.Stopwatch();
            bool prevRinse = false;

            while (_isThreadCoffeeMaker)
            {
                try
                {
                    if (GlobalFunction.GetEnabled(e_Parameter.CoffeeMaker) == false)
                    {
                        continue;
                    }

                    if (GlobalDevice.CoffeeMaker.Instance != null)
                    {
                        if (GlobalDevice.CoffeeMaker.Instance.IsConnected)
                        {
                            if (toggle = !toggle)
                            {
                                GlobalDevice.CoffeeMaker.Status.Comm = GlobalDevice.CoffeeMaker.Instance.GetInfoMessages();
                            }
                            else
                            {
                                GlobalDevice.CoffeeMaker.Status.Comm = GlobalDevice.CoffeeMaker.Instance.GetStatus();
                            }

                            #region Rinse

                            if (_stopwatchRinse.ElapsedMilliseconds >= 1000 * 60 * (int.TryParse(GlobalVariable.Parameter[(int)e_Parameter.CoffeeMaker][CONST.S_KEY][(int)e_Parameter_CoffeeMaker.Rinse_Interval_min], out int Rinse_Interval_min) ? Rinse_Interval_min : 5))
                            {
                                if (GlobalFunction.CheckProcessOrOrder() ||
                                    GlobalFunction.CoffeeMaker.CoffeeMilkL.CheckRun() ||
                                    GlobalFunction.CoffeeMaker.CoffeeMilkL.CheckClean() ||
                                    GlobalFunction.CoffeeMaker.CoffeeMilkL.CheckRinse()
                                   )
                                {
                                    _stopwatchRinse.Reset();

                                    Log.Write(MethodBase.GetCurrentMethod().Name, "Rinse skip");
                                }
                                else
                                {
                                    if (_rinse = GlobalDevice.CoffeeMaker.Status.Comm = GlobalDevice.CoffeeMaker.Instance.DoRinse())
                                    {
                                        _stopwatchRinse.Reset();

                                        stopwatchRinse.Restart();

                                        Log.Write(MethodBase.GetCurrentMethod().Name, "Rinse call");
                                    }
                                }
                            }

                            #endregion
                        }
                    }

                    #region Status

                    GlobalDevice.CoffeeMaker.Status.StatusBase = GlobalDevice.CoffeeMaker.Instance != null && GlobalDevice.CoffeeMaker.Instance.IsConnected && GlobalDevice.CoffeeMaker.Status.Comm;

                    GlobalDevice.CoffeeMaker.Status.Run = GlobalDevice.CoffeeMaker.Status.StatusBase && (GlobalFunction.CoffeeMaker.Water.CheckRun() || GlobalFunction.CoffeeMaker.CoffeeMilkL.CheckRun());

                    GlobalDevice.CoffeeMaker.Status.Status = GlobalDevice.CoffeeMaker.Status.StatusBase && string.IsNullOrEmpty(GlobalDevice.CoffeeMaker.Warnings)
                                                                                                        && string.IsNullOrEmpty(GlobalDevice.CoffeeMaker.Stops)
                                                                                                        && string.IsNullOrEmpty(GlobalDevice.CoffeeMaker.Errors);

                    GlobalDevice.CoffeeMaker.Clean = GlobalDevice.CoffeeMaker.Status.StatusBase && GlobalFunction.CoffeeMaker.CoffeeMilkL.CheckClean();
                    GlobalDevice.CoffeeMaker.Rinse = GlobalDevice.CoffeeMaker.Status.StatusBase && GlobalFunction.CoffeeMaker.CoffeeMilkL.CheckRinse();

                    GlobalDevice.CoffeeMaker.IgnoreStatus = GlobalDevice.CoffeeMaker.Status.StatusBase && (string.IsNullOrEmpty(GlobalDevice.CoffeeMaker.Warnings) || GlobalDevice.CoffeeMaker.IgnoreWarnings)
                                                                                                       &&  string.IsNullOrEmpty(GlobalDevice.CoffeeMaker.Stops)
                                                                                                       &&  string.IsNullOrEmpty(GlobalDevice.CoffeeMaker.Errors);

                    #endregion

                    #region Alarm

                    GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_COMM_COFFEE_MAKER] = !GlobalDevice.CoffeeMaker.Status.Comm;

                    GlobalVariable.Alarm[(int)e_Alarm.ERROR_DEVICE_STATUS_COFFEE_MAKER] = !GlobalDevice.CoffeeMaker.Status.Status;

                    #endregion

                    if (GlobalDevice.CoffeeMaker.Status.StatusBase == false)
                    {
                        GlobalDevice.Stop(e_Device.CoffeeMaker);

                        GlobalDevice.Start(e_Device.CoffeeMaker);
                    }

                    #region Rinse

                    if (_rinse)
                    {
                        if (stopwatchRinse.ElapsedMilliseconds >= 1000 * 90)
                        {
                            _rinse = false;

                            Log.Write(MethodBase.GetCurrentMethod().Name, "Rinse check timeout");
                        }
                        else
                        {
                            bool currRinse = GlobalFunction.CoffeeMaker.CoffeeMilkL.CheckRinse();

                            if (prevRinse != currRinse)
                            {
                                prevRinse = currRinse;

                                if (prevRinse)
                                {
                                    Log.Write(MethodBase.GetCurrentMethod().Name, "Rinse check");
                                }
                                else
                                {
                                    _rinse = false;

                                    Log.Write(MethodBase.GetCurrentMethod().Name, "Rinse done");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (stopwatchRinse.IsRunning)
                        {
                            stopwatchRinse.Reset();
                        }

                        if (prevRinse)
                        {
                            prevRinse = false;
                        }
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
                }

                System.Threading.Thread.Sleep(1100);
            }
        }
    }
}
