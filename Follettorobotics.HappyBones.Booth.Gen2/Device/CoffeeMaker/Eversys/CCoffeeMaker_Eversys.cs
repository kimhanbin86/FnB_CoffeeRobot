using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using EversysApi.DataObject;
using EversysApi.Defines;
using EversysApi.Services;

namespace Follettorobotics.HappyBones.Booth.Gen2
{
    public class CCoffeeMaker_Eversys : SerialService, ICoffeeMaker
    {
        #region 필드

        private readonly object _lockObject = new object();

        private ApiFunctions _apiFunctions = null;

        #endregion

        #region 메서드

        public CCoffeeMaker_Eversys(ISerialPort serialPort) : base(serialPort)
        {
            _apiFunctions = new ApiFunctions(this);
        }

        public bool DoProduct(int productId)
        {
            lock (_lockObject)
            {
                bool result = false;
                try
                {
                    result = _apiFunctions.DoProductOfDisplay(ProcSide_t.Left_e, productId);
                }
                catch (Exception ex)
                {
                    GlobalFunction.CoffeeMaker.LogWrite(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
                }
                return result;
            }
        }

        public bool DoRinse()
        {
            lock (_lockObject)
            {
                bool result = false;
                try
                {
                    result = _apiFunctions.DoRinse(ProcSide_t.Left_e);
                }
                catch (Exception ex)
                {
                    GlobalFunction.CoffeeMaker.LogWrite(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
                }
                return result;
            }
        }

        public bool GetInfoMessages()
        {
            lock (_lockObject)
            {
                bool result = false;
                try
                {
                    ApiInfoMessages resp = new ApiInfoMessages();

                    result = _apiFunctions.GetInfoMessages(out resp);

                    if (result)
                    {
                        #region Warnings

                        int index1 = Array.IndexOf(Enum.GetNames(typeof(e_Alarm)), e_Alarm.WarnCoffeeReserve01_e.ToString());
                        int index2 = Array.IndexOf(Enum.GetNames(typeof(e_Alarm)), e_Alarm.WarnUndef_e.ToString());
                        bool[] array = new bool[index2 - index1 + 1];

                        string str = string.Empty;

                        if (resp.Warnings.Length > 0)
                        {
                            for (int i = 0; i < resp.Warnings.Length; i++)
                            {
                                #region str

                                str += resp.Warnings[i].ToString();

                                if (i < resp.Warnings.Length - 1)
                                {
                                    str += ",";
                                }

                                #endregion

                                #region Alarm

                                index2 = Array.IndexOf(Enum.GetNames(typeof(e_Alarm)), resp.Warnings[i].ToString());
                                if (index2 > -1)
                                {
                                    array[index2 - index1] = true;
                                }

                                #endregion
                            }

                            #region IgnoreWarnings

                            string[] strings = GlobalVariable.Parameter[(int)e_Parameter.CoffeeMaker][CONST.S_KEY][(int)e_Parameter_CoffeeMaker.Ignore_Warnings].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

                            int idx = 0;

                            for (; idx < resp.Warnings.Length; idx++)
                            {
                                if (Array.IndexOf(strings, resp.Warnings[idx].ToString()) < 0)
                                {
                                    break;
                                }
                            }

                            GlobalDevice.CoffeeMaker.IgnoreWarnings = idx == resp.Warnings.Length;

                            #endregion
                        }

                        GlobalDevice.CoffeeMaker.Warnings = str;

                        Array.Copy(array, 0, GlobalVariable.Alarm, index1, array.Length);

                        #endregion

                        #region Stops

                        index1 = Array.IndexOf(Enum.GetNames(typeof(e_Alarm)), e_Alarm.StopCoffeePowerUp_e.ToString());
                        index2 = Array.IndexOf(Enum.GetNames(typeof(e_Alarm)), e_Alarm.StopUndef_e.ToString());
                        array = new bool[index2 - index1 + 1];

                        str = string.Empty;

                        if (resp.Stops.Length > 0)
                        {
                            for (int i = 0; i < resp.Stops.Length; i++)
                            {
                                #region str

                                str += resp.Stops[i].ToString();

                                if (i < resp.Stops.Length - 1)
                                {
                                    str += ",";
                                }

                                #endregion

                                #region Alarm

                                index2 = Array.IndexOf(Enum.GetNames(typeof(e_Alarm)), resp.Stops[i].ToString());
                                if (index2 > -1)
                                {
                                    array[index2 - index1] = true;
                                }

                                #endregion
                            }
                        }

                        GlobalDevice.CoffeeMaker.Stops = str;

                        Array.Copy(array, 0, GlobalVariable.Alarm, index1, array.Length);

                        #endregion

                        #region Errors

                        index1 = Array.IndexOf(Enum.GetNames(typeof(e_Alarm)), e_Alarm.ErrorCoffeeBeanHopperMissRear_e.ToString());
                        index2 = Array.IndexOf(Enum.GetNames(typeof(e_Alarm)), e_Alarm.ErrorUndef_e.ToString());
                        array = new bool[index2 - index1 + 1];

                        str = string.Empty;

                        if (resp.Errors.Length > 0)
                        {
                            for (int i = 0; i < resp.Errors.Length; i++)
                            {
                                #region str

                                str += resp.Errors[i].ToString();

                                if (i < resp.Errors.Length - 1)
                                {
                                    str += ",";
                                }

                                #endregion

                                #region Alarm

                                index2 = Array.IndexOf(Enum.GetNames(typeof(e_Alarm)), resp.Errors[i].ToString());
                                if (index2 > -1)
                                {
                                    array[index2 - index1] = true;
                                }

                                #endregion
                            }
                        }

                        GlobalDevice.CoffeeMaker.Errors = str;

                        Array.Copy(array, 0, GlobalVariable.Alarm, index1, array.Length);

                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    GlobalFunction.CoffeeMaker.LogWrite(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
                }
                return result;
            }
        }

        public bool GetStatus()
        {
            lock (_lockObject)
            {
                bool result = false;
                try
                {
                    ApiStatus resp = new ApiStatus();

                    result = _apiFunctions.GetStatus(out resp);

                    if (result)
                    {
                        GlobalDevice.CoffeeMaker.WaterStatus = resp.Water.status;
                        GlobalDevice.CoffeeMaker.WaterAction = resp.Water.action;
                        GlobalDevice.CoffeeMaker.WaterProcess = resp.Water.process;
                        GlobalDevice.CoffeeMaker.WaterProductKeyIdL = resp.Water.productKeyIdLeft;

                        GlobalDevice.CoffeeMaker.CoffeeMilkLStatus = resp.CoffeeMilkL.status;
                        GlobalDevice.CoffeeMaker.CoffeeMilkLAction = resp.CoffeeMilkL.action;
                        GlobalDevice.CoffeeMaker.CoffeeMilkLProcess = resp.CoffeeMilkL.process;
                        GlobalDevice.CoffeeMaker.CoffeeMilkLProductKeyId = resp.CoffeeMilkL.productKeyId;
                    }
                }
                catch (Exception ex)
                {
                    GlobalFunction.CoffeeMaker.LogWrite(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
                }
                return result;
            }
        }

        #endregion
    }
}
