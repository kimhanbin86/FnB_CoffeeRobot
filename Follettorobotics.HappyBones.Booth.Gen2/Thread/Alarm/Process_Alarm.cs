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
        private System.Threading.Thread _ThreadAlarm = null;
        private bool _isThreadAlarm = false;
        private void Process_Alarm()
        {
            bool[] prevAlarm = new bool[GlobalVariable.Alarm.Length];

            while (_isThreadAlarm)
            {
                try
                {
                    string now = GlobalFunction.GetDateTimeString(DateTime.Now);

                    for (int i = 0; i < GlobalVariable.Alarm.Length; i++)
                    {
                        if (prevAlarm[i] != GlobalVariable.Alarm[i])
                        {
                            prevAlarm[i] = GlobalVariable.Alarm[i];

                            string key = GlobalFunction.GetAlarmKey((e_Alarm)i);

                            if (string.IsNullOrEmpty(key))
                            {
                                #region Event

                                if (prevAlarm[i])
                                {
                                    string cmdText = string.Format("INSERT INTO {0}({1},{2},{3}) VALUES('{4}','{5}','{6}')", e_DB._Alarm,
                                                                                                                             e_DB_Alarm.Column00, e_DB_Alarm.Column02, e_DB_Alarm.Column03,
                                                                                                                             now, $"A-{i:000}", (e_Alarm)i
                                                                  );

                                    GlobalFunction.DB.MySQL.Query(cmdText);
                                }

                                #endregion

                                #region Close

                                if (prevAlarm[i] == false)
                                {
                                    string cmdText = string.Format("UPDATE {0} SET {1}='{2}' WHERE {3} IS NULL AND {4}='{5}'", e_DB._Alarm,
                                                                                                                               e_DB_Alarm.Column01, now,
                                                                                                                               e_DB_Alarm.Column01,
                                                                                                                               e_DB_Alarm.Column02, $"A-{i:000}"
                                                                  );

                                    GlobalFunction.DB.MySQL.Query(cmdText);
                                }

                                #endregion
                            }
                            else
                            {
                                if (GlobalFunction.GetEnabled(e_Parameter.Alarm, key))
                                {
                                    #region Get Code, Text

                                    string code = GlobalVariable.Parameter[(int)e_Parameter.Alarm][key][(int)e_Parameter_Alarm.Code];

                                    string text = GlobalFunction.GetAlarmText(key);

                                    #endregion

                                    #region Event

                                    if (prevAlarm[i])
                                    {
                                        Log.Write(MethodBase.GetCurrentMethod().Name, $"Set Code=[{code}], Text=[{text}]");

                                        string action = GlobalFunction.GetAlarmAction(key);

                                        string cmdText = string.Format("INSERT INTO {0}({1},{2},{3},{4}) VALUES('{5}','{6}','{7}','{8}')", e_DB._Alarm,
                                                                                                                                           e_DB_Alarm.Column00, e_DB_Alarm.Column02, e_DB_Alarm.Column03, e_DB_Alarm.Column04,
                                                                                                                                           now, code, text, action
                                                                      );

                                        GlobalFunction.DB.MySQL.Query(cmdText);
                                    }

                                    #endregion

                                    #region Close

                                    if (prevAlarm[i] == false)
                                    {
                                        Log.Write(MethodBase.GetCurrentMethod().Name, $"Clear Code=[{code}], Text=[{text}]");

                                        string cmdText = string.Format("UPDATE {0} SET {1}='{2}' WHERE {3} IS NULL AND {4}='{5}'", e_DB._Alarm,
                                                                                                                                   e_DB_Alarm.Column01, now,
                                                                                                                                   e_DB_Alarm.Column01,
                                                                                                                                   e_DB_Alarm.Column02, code
                                                                      );

                                        GlobalFunction.DB.MySQL.Query(cmdText);
                                    }

                                    #endregion
                                }
                            }
                        }
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
