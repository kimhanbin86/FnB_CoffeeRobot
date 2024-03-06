using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

using Library;
using Library.Sockets;

namespace Follettorobotics.HappyBones.Booth.Gen2
{
    public class CRemote : SocketServer
    {
        public void RecvString(int clientIndex, string str)
        {
            string call = "RecvString";
            try
            {
                #region ORDER

                string[] commands = GlobalFunction.GetStrings(str, CONST.S_CRLF);

                if (Array.Exists(commands, match => match.ToUpper().Contains("ORDER")))
                {
                    LogWrite(call, $"ORDER Command");

                    commands = commands.Where(match => match.ToUpper().Contains("ORDER")).ToArray();

                    str = commands[0] + CONST.S_CRLF;
                }

                #endregion

                LogWrite(call, $"str=[{str}]");

                string[] msg = GlobalFunction.GetStrings(str, "|");

                if (Array.Exists(msg, match => match.Equals(CONST.S_CRLF)))
                {
                    switch (msg[0].ToUpper().Trim())
                    {
                        #region KIOSK

                        case "STATUS":
                            bool update = GlobalFunction.UpdateProductStatus() && !GlobalFunction.CheckOrderProhibited();

                            string status = string.Empty;
                            foreach (string key in GlobalVariable.Parameter[(int)e_Parameter.Product].Keys)
                            {
                                status += $"{GlobalVariable.Parameter[(int)e_Parameter.Product][key][(int)e_Parameter_Product.Product_Code]}/{(update ? GlobalVariable.Parameter[(int)e_Parameter.Product][key][(int)e_Parameter_Product.Status] : CONST.S_NG)}/{GlobalVariable.Parameter[(int)e_Parameter.Product][key][(int)e_Parameter_Product.Product_Time]}|";
                            }

                            GlobalDevice.Remote.Instance?.Send(clientIndex, $"Status|{GlobalVariable.Parameter[(int)e_Parameter.Booth][CONST.S_KEY][(int)e_Parameter_Booth.Booth_No]}|{status}{CONST.S_CRLF}", call);
                            break;
                        case "WAITING":
                            DataTable data = GlobalFunction.GetOrder();
                            int 총대기건수 = data.Rows.Count;

                            int 대기예상시간 = 0;
                            for (int i = 0; i < 총대기건수; i++)
                            {
                                대기예상시간 += int.TryParse(GlobalVariable.Parameter[(int)e_Parameter.Product][GlobalFunction.GetProductKey(data.Rows[i][e_DB_Order.Column07.ToString()].ToString())][(int)e_Parameter_Product.Product_Time], out int Product_Time) ? Product_Time : 60;
                            }

                            GlobalDevice.Remote.Instance?.Send(clientIndex, $"Waiting|{총대기건수}/{대기예상시간}|{CONST.S_CRLF}", call);
                            break;
                        case "ORDER":
                            string DateTime = GlobalFunction.GetDateTimeString(System.DateTime.Now);
                            string ID = DateTime.Replace("-", "").Replace(" ", "").Replace(":", "").Replace(".", "").Replace(",", "");

                            bool order = true;

                            for (int i = 5; i < Array.IndexOf(msg, CONST.S_CRLF); i++)
                            {
                                string[] orders = GlobalFunction.GetStrings(msg[i], "/");

                                for (int col = 0; col < orders.Length; col++)
                                {
                                    orders[col] = orders[col].Trim();
                                }

                                #region cmdText

                                string cmdText = string.Empty;

                                if (orders.Length == 4)
                                {
                                    cmdText = $" INSERT INTO {e_DB._Order}({e_DB_Order.Column00},{e_DB_Order.Column01},{e_DB_Order.Column02},{e_DB_Order.Column04},{e_DB_Order.Column05},{e_DB_Order.Column06},{e_DB_Order.Column07},{e_DB_Order.Column08},{e_DB_Order.Column09},{e_DB_Order.Column10},{e_DB_Order.Column12})" +
                                              $" VALUES('{DateTime}','{$"{ID}_{i - 4:000}"}','{e_Order_Status.주문}','{e_Order_Source.REMOTE}','{msg[2/*오더번호*/]}','{msg[3/*QR코드Data*/]}','{orders[0/*제품코드*/].ToUpper()}','{orders[1/*제품명*/]}','{orders[2/*결제구분*/]}','{orders[3/*상품금액*/]}','{e_Order_Cup.TAKEOUT}')";
                                }
                                else
                                {
                                    LogWrite(call, $"UNKNOWN Protocol (ORDER)");
                                }

                                #endregion

                                order &= GlobalFunction.DB.MySQL.Query(cmdText);

                                if (order == false)
                                {
                                    break;
                                }
                            }

                            if (order)
                            {
                                GlobalDevice.Remote.Instance?.Send(clientIndex, $"Order|{msg[2/*오더번호*/]}|{CONST.S_CRLF}", call);
                            }
                            break;
                        case "CONNECT":
                            GlobalDevice.Remote.Instance?.Send(clientIndex, $"Connect|{GlobalVariable.Parameter[(int)e_Parameter.Booth][CONST.S_KEY][(int)e_Parameter_Booth.Booth_No]}|{CONST.S_CRLF}", call);
                            break;

                        #endregion

                        #region REMOTE

                        case "REMOTE":
                            switch (msg[1].ToUpper().Trim())
                            {
                                case "DEVICE_STATUS":
                                    GlobalDevice.Remote.Instance?.Send(clientIndex, msg[1].ToUpper().Trim() + "|" + CONST.S_CRLF, call);
                                    break;
                                case "SENSOR_STATUS":
                                    GlobalDevice.Remote.Instance?.Send(clientIndex, msg[1].ToUpper().Trim() + "|" + CONST.S_CRLF, call);
                                    break;
                                case "ROBOT_STATUS":
                                    GlobalDevice.Remote.Instance?.Send(clientIndex, msg[1].ToUpper().Trim() + "|" + CONST.S_CRLF, call);
                                    break;
                                case "ALARM_LIST":
                                    GlobalDevice.Remote.Instance?.Send(clientIndex, msg[1].ToUpper().Trim() + "|" + CONST.S_CRLF, call);
                                    break;
                                default:
                                    LogWrite(call, $"UNKNOWN Command (REMOTE)");
                                    break;
                            }
                            break;

                        #endregion

                        default:
                            LogWrite(call, $"UNKNOWN Command (KIOSK)");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWrite(call, Utility.GetString(ex));
            }
        }
    }
}
