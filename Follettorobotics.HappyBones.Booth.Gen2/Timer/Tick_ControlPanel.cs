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
        private Timer _TimerControlPanel = null;
        private void Tick_ControlPanel(object sender, EventArgs e)
        {
            _TimerControlPanel?.Stop();
            try
            {
                #region CoffeeMaker

                if (grp_ControlPanel_CoffeeMaker.Visible = lbl_ControlPanel_CoffeeMaker_Status.Visible = GlobalFunction.GetEnabled(e_Parameter.CoffeeMaker))
                {
                    if (GlobalDevice.CoffeeMaker.Clean)
                    {
                        lbl_ControlPanel_CoffeeMaker_Status.BackColor = Color.Yellow;
                        lbl_ControlPanel_CoffeeMaker_Status.Text = CONST.S_CLEAN;
                    }
                    else if (GlobalDevice.CoffeeMaker.Rinse)
                    {
                        lbl_ControlPanel_CoffeeMaker_Status.BackColor = Color.Yellow;
                        lbl_ControlPanel_CoffeeMaker_Status.Text = CONST.S_RINSE;
                    }
                    else if (GlobalDevice.CoffeeMaker.Status.Run)
                    {
                        lbl_ControlPanel_CoffeeMaker_Status.BackColor = Color.Yellow;
                        lbl_ControlPanel_CoffeeMaker_Status.Text = CONST.S_RUN;
                    }
                    else
                    {
                        lbl_ControlPanel_CoffeeMaker_Status.BackColor = GlobalDevice.CoffeeMaker.Status.Status ? Color.Lime : Color.Red;
                        lbl_ControlPanel_CoffeeMaker_Status.Text = GlobalDevice.CoffeeMaker.Status.Status ? CONST.S_OK : CONST.S_NG;
                    }

                    lbl_ControlPanel_CoffeeMaker_Water_Status.Text = GlobalDevice.CoffeeMaker.WaterStatus.ToString();
                    lbl_ControlPanel_CoffeeMaker_Water_Action.Text = GlobalDevice.CoffeeMaker.WaterAction.ToString();
                    lbl_ControlPanel_CoffeeMaker_Water_Process.Text = GlobalDevice.CoffeeMaker.WaterProcess.ToString();
                    lbl_ControlPanel_CoffeeMaker_Water_KeyId.Text = GlobalDevice.CoffeeMaker.WaterProductKeyIdL.ToString();

                    lbl_ControlPanel_CoffeeMaker_CoffeeMilkL_Status.Text = GlobalDevice.CoffeeMaker.CoffeeMilkLStatus.ToString();
                    lbl_ControlPanel_CoffeeMaker_CoffeeMilkL_Action.Text = GlobalDevice.CoffeeMaker.CoffeeMilkLAction.ToString();
                    lbl_ControlPanel_CoffeeMaker_CoffeeMilkL_Process.Text = GlobalDevice.CoffeeMaker.CoffeeMilkLProcess.ToString();
                    lbl_ControlPanel_CoffeeMaker_CoffeeMilkL_KeyId.Text = GlobalDevice.CoffeeMaker.CoffeeMilkLProductKeyId.ToString();

                    lbl_ControlPanel_CoffeeMaker_Warnings.Text = GlobalFunction.GetStrings(GlobalDevice.CoffeeMaker.Warnings, ",", StringSplitOptions.RemoveEmptyEntries).Length.ToString();
                    lbl_ControlPanel_CoffeeMaker_Stops.Text = GlobalFunction.GetStrings(GlobalDevice.CoffeeMaker.Stops, ",", StringSplitOptions.RemoveEmptyEntries).Length.ToString();
                    lbl_ControlPanel_CoffeeMaker_Errors.Text = GlobalFunction.GetStrings(GlobalDevice.CoffeeMaker.Errors, ",", StringSplitOptions.RemoveEmptyEntries).Length.ToString();
                }

                #endregion

                #region Controller1

                if (grp_ControlPanel_Controller1.Visible = lbl_ControlPanel_Controller1_Status.Visible = GlobalFunction.GetEnabled(e_Parameter.Controller1))
                {
                    lbl_ControlPanel_Controller1_Status.BackColor = GlobalDevice.Controller1.Status.Status ? Color.Lime : Color.Red;
                    lbl_ControlPanel_Controller1_Status.Text = GlobalDevice.Controller1.Status.Status ? CONST.S_OK : CONST.S_NG;

                    lbl_ControlPanel_Controller1_Door1_Close.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B4_Door1_Close] ? Color.Lime : SystemColors.Control;
                    lbl_ControlPanel_Controller1_Door2_Close.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B5_Door2_Close] ? Color.Lime : SystemColors.Control;
                    lbl_ControlPanel_Controller1_Door3_Close.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B6_Door3_Close] ? Color.Lime : SystemColors.Control;
                    lbl_ControlPanel_Controller1_Door4_Close.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B7_Door4_Close] ? Color.Lime : SystemColors.Control;

                    lbl_ControlPanel_Controller1_Door1_Open.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B0_Door1_Open] ? Color.Lime : SystemColors.Control;
                    lbl_ControlPanel_Controller1_Door2_Open.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B1_Door2_Open] ? Color.Lime : SystemColors.Control;
                    lbl_ControlPanel_Controller1_Door3_Open.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B2_Door3_Open] ? Color.Lime : SystemColors.Control;
                    lbl_ControlPanel_Controller1_Door4_Open.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B3_Door4_Open] ? Color.Lime : SystemColors.Control;

                    btn_ControlPanel_Controller1_Door1_Lock.BackColor = GlobalFunction.Door.GetLock(e_Door.Door1) == e_Door_Lock.Unlock ? SystemColors.Control : Color.Red;
                    btn_ControlPanel_Controller1_Door2_Lock.BackColor = GlobalFunction.Door.GetLock(e_Door.Door2) == e_Door_Lock.Unlock ? SystemColors.Control : Color.Red;
                    btn_ControlPanel_Controller1_Door3_Lock.BackColor = GlobalFunction.Door.GetLock(e_Door.Door3) == e_Door_Lock.Unlock ? SystemColors.Control : Color.Red;
                    btn_ControlPanel_Controller1_Door4_Lock.BackColor = GlobalFunction.Door.GetLock(e_Door.Door4) == e_Door_Lock.Unlock ? SystemColors.Control : Color.Red;

                    lbl_ControlPanel_Controller1_Door1_Coffee.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B7_Coffee1] ? Color.Lime : SystemColors.Control;
                    lbl_ControlPanel_Controller1_Door2_Coffee.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B6_Coffee2] ? Color.Lime : SystemColors.Control;
                    lbl_ControlPanel_Controller1_Door3_Coffee.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B5_Coffee3] ? Color.Lime : SystemColors.Control;
                    lbl_ControlPanel_Controller1_Door4_Coffee.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B4_Coffee4] ? Color.Lime : SystemColors.Control;

                    lbl_ControlPanel_Controller1_Door4_Tumbler_Home.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B2_LM_Up] ? Color.Lime : SystemColors.Control;
                    lbl_ControlPanel_Controller1_Door4_Tumbler_Check.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B0_텀블러높이] ? Color.Lime : SystemColors.Control;

                    lbl_ControlPanel_Sensor_LM_Down.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B1_LM_Down] ? Color.Lime : SystemColors.Control;

                    lbl_ControlPanel_Sensor_Cup1.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B0_Cup1] ? Color.Lime : Color.Red;
                    lbl_ControlPanel_Sensor_Cup2.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B1_Cup2] ? Color.Lime : Color.Red;
                    lbl_ControlPanel_Sensor_Cup3.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B2_Cup3] ? Color.Lime : Color.Red;
                    lbl_ControlPanel_Sensor_Cup4.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B3_Cup4] ? Color.Lime : Color.Red;

                    lbl_ControlPanel_Sensor_Milk.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B6_Milk] ? Color.Lime : Color.Red;
                    lbl_ControlPanel_Sensor_NBox.BackColor = GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D3B7_NBox] ? Color.Lime : Color.Red;
                }

                #endregion

                #region Controller2

                if (grp_ControlPanel_Controller2.Visible = lbl_ControlPanel_Controller2_Status.Visible = GlobalFunction.GetEnabled(e_Parameter.Controller2))
                {
                    lbl_ControlPanel_Controller2_Status.BackColor = GlobalDevice.Controller2.Status.Status ? Color.Lime : Color.Red;
                    lbl_ControlPanel_Controller2_Status.Text = GlobalDevice.Controller2.Status.Status ? CONST.S_OK : CONST.S_NG;

                    lbl_ControlPanel_Controller2_Turn_Home.BackColor = GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D2B1_회전_Home] ? Color.Lime : SystemColors.Control;
                    lbl_ControlPanel_Controller2_Sauce_Home.BackColor = GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D2B0_펌핑_Home] ? Color.Lime : SystemColors.Control;

                    lbl_ControlPanel_Sensor_Sauce1.BackColor = GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D1B3_Sauce1] ? Color.Lime : Color.Red;
                    lbl_ControlPanel_Sensor_Sauce2.BackColor = GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D1B2_Sauce2] ? Color.Lime : Color.Red;
                    lbl_ControlPanel_Sensor_Sauce3.BackColor = GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D1B1_Sauce3] ? Color.Lime : Color.Red;
                    lbl_ControlPanel_Sensor_Sauce4.BackColor = GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D1B0_Sauce4] ? Color.Lime : Color.Red;
                    lbl_ControlPanel_Sensor_Sauce5.BackColor = GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D2B3_Sauce5] ? Color.Lime : Color.Red;
                    lbl_ControlPanel_Sensor_Sauce6.BackColor = GlobalDevice.Controller2.Sensor[(int)e_Device_Controller2_Sensor.D2B2_Sauce6] ? Color.Lime : Color.Red;
                }

                #endregion

                #region IceMaker

                if (grp_ControlPanel_IceMaker.Visible = lbl_ControlPanel_IceMaker_Status.Visible = GlobalFunction.GetEnabled(e_Parameter.IceMaker))
                {
                    if (GlobalDevice.IceMaker.Status.Run)
                    {
                        lbl_ControlPanel_IceMaker_Status.BackColor = Color.Yellow;
                        lbl_ControlPanel_IceMaker_Status.Text = CONST.S_RUN;
                    }
                    else
                    {
                        lbl_ControlPanel_IceMaker_Status.BackColor = GlobalDevice.IceMaker.Status.Status ? Color.Lime : Color.Red;
                        lbl_ControlPanel_IceMaker_Status.Text = GlobalDevice.IceMaker.Status.Status ? CONST.S_OK : CONST.S_NG;
                    }

                    lbl_ControlPanel_IceMaker_MachineCode.Text = GlobalDevice.IceMaker.Status.MachineCode;
                    lbl_ControlPanel_IceMaker_ErrorCode.Text = GlobalDevice.IceMaker.Status.ErrorCode;
                }

                #endregion

                #region Robot

                if (grp_ControlPanel_Robot.Visible = lbl_ControlPanel_Robot_Status.Visible = GlobalFunction.GetEnabled(e_Parameter.Robot))
                {
                    lbl_ControlPanel_Robot_Status.BackColor = GlobalDevice.Robot.Status.Status ? Color.Lime : Color.Red;
                    lbl_ControlPanel_Robot_Status.Text = GlobalDevice.Robot.Status.Status ? CONST.S_OK : CONST.S_NG;
                }

                #endregion

                #region Time

                lbl_Time_Sub.Text = $"{(double)_timeSub.ElapsedMilliseconds / 1000:0.0}";
                lbl_Time_Main.Text = $"{(double)_timeMain.ElapsedMilliseconds / 1000:0.0}";

                #endregion

                #region StatusStrip

                toolStripStatusLabel_DateTime.Text = GlobalFunction.GetDateTimeString(DateTime.Now);

                #endregion
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            finally
            {
                _TimerControlPanel?.Start();
            }
        }
    }
}
