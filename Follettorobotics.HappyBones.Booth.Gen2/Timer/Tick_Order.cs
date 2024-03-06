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
        private Timer _TimerOrder = null;
        private void Tick_Order(object sender, EventArgs e)
        {
            _TimerOrder?.Stop();
            try
            {
                UpdateOrder();
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            finally
            {
                _TimerOrder?.Start();
            }
        }

        private void UpdateOrder()
        {
            try
            {
                DataGridView dgv = dgv_Order;

                dgv.Rows.Clear();

                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                #region _ID

                UpdateOrder(dgv, GlobalFunction.DB.MySQL.GetDataTable($"SELECT * FROM {e_DB._Order} WHERE {e_DB_Order.Column01}='{_ID}'"));

                #endregion

                #region 배출

                UpdateOrder(dgv, GlobalFunction.DB.MySQL.GetDataTable($"SELECT * FROM {e_DB._Order} WHERE {e_DB_Order.Column02}='{e_Order_Status.배출}' ORDER BY {e_DB_Order.Column01} ASC"));

                #endregion

                if (GlobalFunction.CheckOrder())
                {
                    #region 주문

                    UpdateOrder(dgv, GlobalFunction.DB.MySQL.GetDataTable($"SELECT * FROM {e_DB._Order} WHERE {e_DB_Order.Column02}='{e_Order_Status.주문}' ORDER BY {e_DB_Order.Column01} ASC"));

                    #endregion
                }
                else
                {
                    #region DateTime

                    DateTime now = DateTime.Now;

                    UpdateOrder(dgv, GlobalFunction.DB.MySQL.GetDataTable($"SELECT * FROM {e_DB._Order} WHERE {e_DB_Order.Column00}>='{GlobalFunction.GetDateTimeString(now.AddHours(-1))}' AND {e_DB_Order.Column00}<='{GlobalFunction.GetDateTimeString(now)}' ORDER BY {e_DB_Order.Column01} DESC"));

                    #endregion
                }

                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                #region BackColor

                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    switch ((e_Order_Status)Enum.Parse(typeof(e_Order_Status), GlobalFunction.GetString(dgv.Rows[i].Cells[e_DB_Order.Column02.ToString()].Value)))
                    {
                        case e_Order_Status.주문취소:
                            dgv.Rows[i].DefaultCellStyle.BackColor = Color.Blue;
                            dgv.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                            break;
                        case e_Order_Status.시작:
                        case e_Order_Status.텀블러:
                        case e_Order_Status.픽업:
                        case e_Order_Status.스팀피처:
                        case e_Order_Status.소스:
                        case e_Order_Status.얼음:
                        case e_Order_Status.커피:
                        case e_Order_Status.도어:
                            dgv.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                            break;
                        case e_Order_Status.배출:
                            dgv.Rows[i].DefaultCellStyle.BackColor = Color.Black;
                            dgv.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                            break;
                        case e_Order_Status.배출완료:
                            dgv.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                            break;
                        case e_Order_Status.강제배출:
                        case e_Order_Status.수동배출:
                        case e_Order_Status.오류:
                            dgv.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                            dgv.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                            break;
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }
        private void UpdateOrder(DataGridView dgv, DataTable data)
        {
            try
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    if (dgv.Rows.Count < 5)
                    {
                        #region 중복 체크

                        bool check = false;

                        for (int row = 0; row < dgv.Rows.Count; row++)
                        {
                            if (GlobalFunction.GetString(dgv.Rows[row].Cells[e_DB_Order.Column01.ToString()].Value) == data.Rows[i][e_DB_Order.Column01.ToString()].ToString())
                            {
                                check = true;

                                break;
                            }
                        }

                        #endregion

                        if (check == false)
                        {
                            dgv.Rows.Add();

                            for (int j = 0; j < Enum.GetNames(typeof(e_DB_Order)).Length; j++)
                            {
                                dgv.Rows[dgv.Rows.Count - 1].Cells[j].Value = data.Rows[i][j].ToString();
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }
    }
}
