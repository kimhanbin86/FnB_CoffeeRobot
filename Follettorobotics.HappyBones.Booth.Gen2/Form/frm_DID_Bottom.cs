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
    public partial class frm_DID_Bottom : Form
    {
        private const string _className = "frm_DID_Bottom::";

        #region Timer

        private Timer _TimerDID_Bottom = null;
        private void Tick_DID_Bottom(object sender, EventArgs e)
        {
            _TimerDID_Bottom?.Stop();
            try
            {
                for (int i = (int)e_Door.Door1; i < Enum.GetNames(typeof(e_Door)).Length; i++)
                {
                    e_Door_Lock @lock = GlobalFunction.Door.GetLock((e_Door)i);

                    string ID = GlobalFunction.Door.GetID((e_Door)i);

                    switch (@lock)
                    {
                        case e_Door_Lock.Lock:
                            if (string.IsNullOrEmpty(ID) == false)
                            {
                                #region

                                DataTable data = GlobalFunction.DB.MySQL.GetDataTable($"SELECT * FROM {e_DB._Order} WHERE {e_DB_Order.Column01}='{ID}'");

                                switch ((e_Door)i)
                                {
                                    case e_Door.Door1:
                                        lbl_DID_Bottom_Door1_Order_No.ForeColor = Color.White;
                                        lbl_DID_Bottom_Door1_Order_No.Text = data.Rows[0][e_DB_Order.Column05.ToString()].ToString();
                                        lbl_DID_Bottom_Door1_Product_Name.ForeColor = Color.White;
                                        lbl_DID_Bottom_Door1_Product_Name.Text = data.Rows[0][e_DB_Order.Column08.ToString()].ToString();
                                        break;
                                    case e_Door.Door2:
                                        lbl_DID_Bottom_Door2_Order_No.ForeColor = Color.White;
                                        lbl_DID_Bottom_Door2_Order_No.Text = data.Rows[0][e_DB_Order.Column05.ToString()].ToString();
                                        lbl_DID_Bottom_Door2_Product_Name.ForeColor = Color.White;
                                        lbl_DID_Bottom_Door2_Product_Name.Text = data.Rows[0][e_DB_Order.Column08.ToString()].ToString();
                                        break;
                                    case e_Door.Door3:
                                        lbl_DID_Bottom_Door3_Order_No.ForeColor = Color.White;
                                        lbl_DID_Bottom_Door3_Order_No.Text = data.Rows[0][e_DB_Order.Column05.ToString()].ToString();
                                        lbl_DID_Bottom_Door3_Product_Name.ForeColor = Color.White;
                                        lbl_DID_Bottom_Door3_Product_Name.Text = data.Rows[0][e_DB_Order.Column08.ToString()].ToString();
                                        break;
                                    case e_Door.Door4:
                                        lbl_DID_Bottom_Door4_Order_No.ForeColor = Color.White;
                                        lbl_DID_Bottom_Door4_Order_No.Text = data.Rows[0][e_DB_Order.Column05.ToString()].ToString();
                                        lbl_DID_Bottom_Door4_Product_Name.ForeColor = Color.White;
                                        lbl_DID_Bottom_Door4_Product_Name.Text = data.Rows[0][e_DB_Order.Column08.ToString()].ToString();
                                        break;
                                }

                                #endregion
                            }
                            else
                            {
                                #region Lock

                                if (false)
                                {
                                    switch ((e_Door)i)
                                    {
                                        case e_Door.Door1:
                                            lbl_DID_Bottom_Door1_Order_No.ForeColor = Color.Red;
                                            lbl_DID_Bottom_Door1_Order_No.Text = "Lock";
                                            lbl_DID_Bottom_Door1_Product_Name.Text = string.Empty;
                                            break;
                                        case e_Door.Door2:
                                            lbl_DID_Bottom_Door2_Order_No.ForeColor = Color.Red;
                                            lbl_DID_Bottom_Door2_Order_No.Text = "Lock";
                                            lbl_DID_Bottom_Door2_Product_Name.Text = string.Empty;
                                            break;
                                        case e_Door.Door3:
                                            lbl_DID_Bottom_Door3_Order_No.ForeColor = Color.Red;
                                            lbl_DID_Bottom_Door3_Order_No.Text = "Lock";
                                            lbl_DID_Bottom_Door3_Product_Name.Text = string.Empty;
                                            break;
                                        case e_Door.Door4:
                                            lbl_DID_Bottom_Door4_Order_No.ForeColor = Color.Red;
                                            lbl_DID_Bottom_Door4_Order_No.Text = "Lock";
                                            lbl_DID_Bottom_Door4_Product_Name.Text = string.Empty;
                                            break;
                                    }
                                }
                                else
                                {
                                    switch ((e_Door)i)
                                    {
                                        case e_Door.Door1:
                                            lbl_DID_Bottom_Door1_Order_No.Text = string.Empty;
                                            lbl_DID_Bottom_Door1_Product_Name.Text = string.Empty;
                                            break;
                                        case e_Door.Door2:
                                            lbl_DID_Bottom_Door2_Order_No.Text = string.Empty;
                                            lbl_DID_Bottom_Door2_Product_Name.Text = string.Empty;
                                            break;
                                        case e_Door.Door3:
                                            lbl_DID_Bottom_Door3_Order_No.Text = string.Empty;
                                            lbl_DID_Bottom_Door3_Product_Name.Text = string.Empty;
                                            break;
                                        case e_Door.Door4:
                                            lbl_DID_Bottom_Door4_Order_No.Text = string.Empty;
                                            lbl_DID_Bottom_Door4_Product_Name.Text = string.Empty;
                                            break;
                                    }
                                }

                                #endregion
                            }
                            break;
                        case e_Door_Lock.Unlock:
                            #region

                            switch ((e_Door)i)
                            {
                                case e_Door.Door1:
                                    lbl_DID_Bottom_Door1_Order_No.Text = string.Empty;
                                    lbl_DID_Bottom_Door1_Product_Name.Text = string.Empty;
                                    break;
                                case e_Door.Door2:
                                    lbl_DID_Bottom_Door2_Order_No.Text = string.Empty;
                                    lbl_DID_Bottom_Door2_Product_Name.Text = string.Empty;
                                    break;
                                case e_Door.Door3:
                                    lbl_DID_Bottom_Door3_Order_No.Text = string.Empty;
                                    lbl_DID_Bottom_Door3_Product_Name.Text = string.Empty;
                                    break;
                                case e_Door.Door4:
                                    lbl_DID_Bottom_Door4_Order_No.Text = string.Empty;
                                    lbl_DID_Bottom_Door4_Product_Name.Text = string.Empty;
                                    break;
                            }

                            #endregion
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            finally
            {
                _TimerDID_Bottom?.Start();
            }
        }

        #endregion

        public frm_DID_Bottom()
        {
            InitializeComponent();
        }

        private void frm_DID_Bottom_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalVariable.Form.DID_Bottom = null;
        }

        private void frm_DID_Bottom_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_TimerDID_Bottom != null)
            {
                if (_TimerDID_Bottom.Enabled)
                {
                    _TimerDID_Bottom.Stop();
                }

                _TimerDID_Bottom.Dispose();
                _TimerDID_Bottom = null;
            }
        }

        private void frm_DID_Bottom_Load(object sender, EventArgs e)
        {
            InitializeForm();

            InitializeLabel();

            _TimerDID_Bottom = new Timer();
            _TimerDID_Bottom.Tick += new EventHandler(Tick_DID_Bottom);
            _TimerDID_Bottom.Interval = 100;
            _TimerDID_Bottom.Start();





        }

        private void InitializeForm()
        {
            try
            {
                #region Font

                TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Font));

                Font = (Font)typeConverter.ConvertFromString(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Font].Replace("/", ","));

                #endregion

                #region Location

                Location = new Point(Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Form_Location_X]),
                                     Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Form_Location_Y])
                                    );

                #endregion

                #region Size

                Size = new Size(Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Form_Size_Width]),
                                Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Form_Size_Height])
                               );

                #endregion
            }
            catch (Exception ex)
            {
                Log.Write(_className + MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        private void InitializeLabel()
        {
            try
            {
                #region Door1

                lbl_DID_Bottom_Door1_Order_No.Text = string.Empty;

                lbl_DID_Bottom_Door1_Order_No.Location = new Point(Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Door1_Location_X]),
                                                                   Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Door1_Location_Y])
                                                                  );

                lbl_DID_Bottom_Door1_Order_No.Size = new Size(Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Door_Size_Width]),
                                                              Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Door_Size_Height])
                                                             );

                lbl_DID_Bottom_Door1_Product_Name.Text = string.Empty;

                lbl_DID_Bottom_Door1_Product_Name.Location = new Point(lbl_DID_Bottom_Door1_Order_No.Location.X, lbl_DID_Bottom_Door1_Order_No.Location.Y + lbl_DID_Bottom_Door1_Order_No.Size.Height);

                lbl_DID_Bottom_Door1_Product_Name.Size = lbl_DID_Bottom_Door1_Order_No.Size;

                #endregion

                #region Door2

                lbl_DID_Bottom_Door2_Order_No.Text = string.Empty;

                lbl_DID_Bottom_Door2_Order_No.Location = new Point(Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Door2_Location_X]),
                                                                   Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Door2_Location_Y])
                                                                  );

                lbl_DID_Bottom_Door2_Order_No.Size = new Size(Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Door_Size_Width]),
                                                              Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Door_Size_Height])
                                                             );

                lbl_DID_Bottom_Door2_Product_Name.Text = string.Empty;

                lbl_DID_Bottom_Door2_Product_Name.Location = new Point(lbl_DID_Bottom_Door2_Order_No.Location.X, lbl_DID_Bottom_Door2_Order_No.Location.Y + lbl_DID_Bottom_Door2_Order_No.Size.Height);

                lbl_DID_Bottom_Door2_Product_Name.Size = lbl_DID_Bottom_Door2_Order_No.Size;

                #endregion

                #region Door3

                lbl_DID_Bottom_Door3_Order_No.Text = string.Empty;

                lbl_DID_Bottom_Door3_Order_No.Location = new Point(Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Door3_Location_X]),
                                                                   Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Door3_Location_Y])
                                                                  );

                lbl_DID_Bottom_Door3_Order_No.Size = new Size(Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Door_Size_Width]),
                                                              Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Door_Size_Height])
                                                             );

                lbl_DID_Bottom_Door3_Product_Name.Text = string.Empty;

                lbl_DID_Bottom_Door3_Product_Name.Location = new Point(lbl_DID_Bottom_Door3_Order_No.Location.X, lbl_DID_Bottom_Door3_Order_No.Location.Y + lbl_DID_Bottom_Door3_Order_No.Size.Height);

                lbl_DID_Bottom_Door3_Product_Name.Size = lbl_DID_Bottom_Door3_Order_No.Size;

                #endregion

                #region Door4

                lbl_DID_Bottom_Door4_Order_No.Text = string.Empty;

                lbl_DID_Bottom_Door4_Order_No.Location = new Point(Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Door4_Location_X]),
                                                                   Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Door4_Location_Y])
                                                                  );

                lbl_DID_Bottom_Door4_Order_No.Size = new Size(Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Door_Size_Width]),
                                                              Convert.ToInt32(GlobalVariable.Parameter[(int)e_Parameter.DID_Bottom][CONST.S_KEY][(int)e_Parameter_DID_Bottom.Door_Size_Height])
                                                             );

                lbl_DID_Bottom_Door4_Product_Name.Text = string.Empty;

                lbl_DID_Bottom_Door4_Product_Name.Location = new Point(lbl_DID_Bottom_Door4_Order_No.Location.X, lbl_DID_Bottom_Door4_Order_No.Location.Y + lbl_DID_Bottom_Door4_Order_No.Size.Height);

                lbl_DID_Bottom_Door4_Product_Name.Size = lbl_DID_Bottom_Door4_Order_No.Size;

                #endregion
            }
            catch (Exception ex)
            {
                Log.Write(_className + MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }
    }
}
