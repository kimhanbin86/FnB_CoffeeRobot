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
        private const int _parameterKey = (int)e_Parameter.Product;

        #region Sequence_Main

        #region enum

        public enum e_Sequence_Main
        {
            대기,
            시작,
            텀블러,
            픽업,
            스팀피처,
            소스,
            얼음,
            커피,
            도어,
            리셋,
        }
        private e_Sequence_Main _prev_Sequence_Main = e_Sequence_Main.대기;
        public  e_Sequence_Main _curr_Sequence_Main = e_Sequence_Main.대기;

        #endregion

        #region 필드

        private string _ID = string.Empty;

        private string _productCode = string.Empty;

        private string _productKey = string.Empty;

        private bool _coffee = false;

        #endregion

        private System.Diagnostics.Stopwatch _timeSub = new System.Diagnostics.Stopwatch();
        private System.Diagnostics.Stopwatch _timeMain = new System.Diagnostics.Stopwatch();

        private System.Threading.Thread _ThreadSequence_Main = null;
        private bool _isThreadSequence_Main = false;
        private void Process_Sequence_Main()
        {
            string call = "Sequence_Main";

            #region local

            DataTable Order = null;

            int row = -1;

            string NG = string.Empty;

            // Order
            string Order_No = string.Empty;
            string Barcode = string.Empty;
            string Product_Name = string.Empty;
            e_Order_Cup Order_Cup = e_Order_Cup.TAKEOUT;

            // Product
            int Coffee_ID = 0;
            double End_Delay = 0;
            e_ComboBox_Cup Cup = e_ComboBox_Cup.Hot;
            string Ice_Time = string.Empty;
            string Water_Time = string.Empty;
            e_ComboBox_Sauce_Type Sauce_Type = e_ComboBox_Sauce_Type.Unused;
            e_Device_Controller2_Sauce Sauce_No = e_Device_Controller2_Sauce.Unused;
            string Pumping_Count = string.Empty;
            string Mixing_Count = string.Empty;
            string Remark = string.Empty;
            bool ade = false;

            // ref
            e_Device_Robot_Cup Robot_Cup = e_Device_Robot_Cup.Undefined;
            e_Door Door = e_Door.Undefined;

            #endregion

            while (_isThreadSequence_Main)
            {
                try
                {
                    #region prev != curr

                    if (_prev_Sequence_Main != _curr_Sequence_Main)
                    {
                        Log.Write(call, $"----------------------------------------------------------------------");
                        _prev_Sequence_Main = _curr_Sequence_Main;
                        Log.Write(call, $"---------------------------------------------------------------------- [{_curr_Sequence_Main}]");

                        switch (_curr_Sequence_Main)
                        {
                            case e_Sequence_Main.시작:
                                GlobalFunction.DB.MySQL.Query($"UPDATE {e_DB._Order} SET {e_DB_Order.Column02}='{_curr_Sequence_Main}' WHERE {e_DB_Order.Column01}='{_ID}'");
                                break;
                            case e_Sequence_Main.텀블러:
                            case e_Sequence_Main.픽업:
                            case e_Sequence_Main.스팀피처:
                            case e_Sequence_Main.소스:
                            case e_Sequence_Main.얼음:
                            case e_Sequence_Main.커피:
                            case e_Sequence_Main.도어:
                                _prev_Sequence_Sub = e_Sequence_Sub.Sub00;
                                _curr_Sequence_Sub = e_Sequence_Sub.Sub00;

                                GlobalFunction.DB.MySQL.Query($"UPDATE {e_DB._Order} SET {e_DB_Order.Column02}='{_curr_Sequence_Main}',{e_DB_Order.Column03}='{_curr_Sequence_Sub}' WHERE {e_DB_Order.Column01}='{_ID}'");

                                _logSub00 = true;

                                _stopwatchSub.Reset();
                                break;
                        }

                        #region Stopwatch

                        switch (_curr_Sequence_Main)
                        {
                            case e_Sequence_Main.시작:
                                _timeMain.Restart();
                                break;
                            case e_Sequence_Main.텀블러:
                            case e_Sequence_Main.픽업:
                            case e_Sequence_Main.스팀피처:
                            case e_Sequence_Main.소스:
                            case e_Sequence_Main.얼음:
                            case e_Sequence_Main.커피:
                                _timeSub.Restart();
                                break;
                            case e_Sequence_Main.리셋:
                                if (GlobalFunction.CheckOrder() == false)
                                {
                                    if (Sauce_Type != e_ComboBox_Sauce_Type.Ade)
                                    {
                                        _stopwatchRinse.Restart();
                                    }
                                }
                                break;
                            default:
                                _timeSub.Stop();
                                _timeMain.Stop();
                                break;
                        }

                        #endregion

                        #region Motion

                        switch (_curr_Sequence_Main)
                        {
                            case e_Sequence_Main.대기:     CDID.Motion(e_Motion.텀블러_투입_0); break;
                            case e_Sequence_Main.시작:     CDID.Motion(e_Motion.텀블러_투입_0); break;
                            case e_Sequence_Main.픽업:
                                switch (Order_Cup)
                                {
                                    case e_Order_Cup.TAKEOUT:
                                        CDID.Motion(e_Motion.컵_pickup_중);
                                        break;
                                    case e_Order_Cup.TUMBLER:
                                        CDID.Motion(e_Motion.텀블러_투입_1);
                                        break;
                                }
                                break;
                            case e_Sequence_Main.스팀피처: CDID.Motion(e_Motion.제빙기_투출_중); break;
                            case e_Sequence_Main.소스:     CDID.Motion(e_Motion.소스_펌핑_및_믹싱_중); break;
                            case e_Sequence_Main.얼음:     CDID.Motion(e_Motion.제빙기_투출_중); break;
                            case e_Sequence_Main.커피:     CDID.Motion(e_Motion.커피_투출_중); break;
                            case e_Sequence_Main.도어:     CDID.Motion(e_Motion.제조_완료_및_픽업대에_place_중); break;
                            case e_Sequence_Main.리셋:     CDID.Motion(e_Motion.텀블러_투입_0); break;
                        }

                        #endregion
                    }

                    #endregion

                    #region Waiting

                    _curr_Waiting = GetWaiting();

                    if (_prev_Waiting != _curr_Waiting)
                    {
                        _prev_Waiting = _curr_Waiting;

                        CDID.Waiting(_curr_Waiting);
                    }

                    #endregion

                    switch (_curr_Sequence_Main)
                    {
                        case e_Sequence_Main.대기:
                            bool standby  = GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D3B0_대기_위치];
                                 standby &= GlobalFunction.CheckOrder();
                                 standby &= CheckUnlockDoor();

                            if (standby)
                            {
                                Order = GlobalFunction.GetOrder();

                                #region Log (Order)

                                string logOrder = CONST.S_CRLF;

                                for (int i = 0; i < Order.Rows.Count; i++)
                                {
                                    logOrder += $"Order[{i}] - ID=[{Order.Rows[i][e_DB_Order.Column01.ToString()]}] / Source=[{Order.Rows[i][e_DB_Order.Column04.ToString()]}] / Order_No=[{Order.Rows[i][e_DB_Order.Column05.ToString()]}] / Barcode=[{Order.Rows[i][e_DB_Order.Column06.ToString()]}] / Product_Code=[{Order.Rows[i][e_DB_Order.Column07.ToString()]}] / Product_Name=[{Order.Rows[i][e_DB_Order.Column08.ToString()]}] / Order_Cup=[{Order.Rows[i][e_DB_Order.Column12.ToString()]}]" + CONST.S_CRLF;
                                }

                                Log.Write(call, logOrder);

                                #endregion

                                #region row

                                for (int i = 0; i < Order.Rows.Count; i++)
                                {
                                    Order_Cup = (e_Order_Cup)Enum.Parse(typeof(e_Order_Cup), Order.Rows[i][e_DB_Order.Column12.ToString()].ToString());

                                    if (Order_Cup == e_Order_Cup.TAKEOUT)
                                    {
                                        row = i;

                                        break;
                                    }
                                    else if (Order_Cup == e_Order_Cup.TUMBLER)
                                    {
                                        if (GlobalFunction.Door.GetLock(e_Door.Door4) == e_Door_Lock.Unlock)
                                        {
                                            row = i;

                                            break;
                                        }
                                    }
                                }

                                #endregion

                                if (row >= 0)
                                {
                                    #region SET _ID, _productCode, _productKey

                                    _ID = Order.Rows[row][e_DB_Order.Column01.ToString()].ToString();
                                    Log.Write(call, $"_ID=[{_ID}]");

                                    _productCode = Order.Rows[row][e_DB_Order.Column07.ToString()].ToString();
                                    Log.Write(call, $"_productCode=[{_productCode}]");

                                    _productKey = GlobalFunction.GetProductKey(_productCode);
                                    Log.Write(call, $"_productKey=[{_productKey}]");

                                    #endregion

                                    #region SET check

                                    string ID = GetID(_ID);
                                    Log.Write(call, $"ID=[{ID}]");

                                    bool check = CheckID(ID);
                                    Log.Write(call, $"check=[{check}]");

                                    #endregion

                                    bool process = true;

                                    if (check == false)
                                    {
                                        #region Group check

                                        DataTable data = GlobalFunction.DB.MySQL.GetDataTable($"SELECT * FROM {e_DB._Order} WHERE {e_DB_Order.Column01} LIKE '{ID}%' AND {e_DB_Order.Column02}='{e_Order_Status.주문}' ORDER BY {e_DB_Order.Column01} ASC");

                                        int groupCount = data.Rows.Count;
                                        Log.Write(call, $"Group check - Count=[{groupCount}]");

                                        process = groupCount > 0;

                                        string orderNo = string.Empty;
                                        string productName = string.Empty;

                                        for (int i = 0; i < groupCount; i++)
                                        {
                                            #region Device check

                                            if (process)
                                            {
                                                process = GlobalFunction.CheckProductDevice(GlobalFunction.GetProductKey(data.Rows[i][e_DB_Order.Column07.ToString()].ToString()), check, ref NG);
                                            }

                                            #endregion

                                            #region Sensor check

                                            if (process)
                                            {
                                                process = GlobalFunction.CheckProductSensor(GlobalFunction.GetProductKey(data.Rows[i][e_DB_Order.Column07.ToString()].ToString()), ref NG);
                                            }

                                            #endregion

                                            if (process == false)
                                            {
                                                orderNo = data.Rows[i][e_DB_Order.Column05.ToString()].ToString();
                                                productName = data.Rows[i][e_DB_Order.Column08.ToString()].ToString();

                                                Log.Write(call, $"Group item NG - ID=[{data.Rows[i][e_DB_Order.Column01.ToString()]}] / Order_No=[{data.Rows[i][e_DB_Order.Column05.ToString()]}] / Product_Code=[{data.Rows[i][e_DB_Order.Column07.ToString()]}] / Product_Name=[{data.Rows[i][e_DB_Order.Column08.ToString()]}] = [{NG}]");

                                                break;
                                            }
                                        }

                                        if (process == false)
                                        {
                                            CDID.Error(orderNo, productName, NG);
                                        }

                                        #endregion
                                    }

                                    #region Device check

                                    if (process)
                                    {
                                        process = GlobalFunction.CheckProductDevice(_productKey, check, ref NG);
                                    }

                                    #endregion

                                    #region Sensor check

                                    if (process)
                                    {
                                        if (check == false)
                                        {
                                            process = GlobalFunction.CheckProductSensor(_productKey, ref NG);
                                        }
                                    }

                                    #endregion

                                    if (GlobalFunction.GetSimulation())
                                    {
                                        process = true;
                                    }

                                    if (process && _rinse == false)
                                    {
                                        CDID.Error();

                                        if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D3B0_대기_위치])
                                        {
                                            _curr_Sequence_Main = e_Sequence_Main.시작;
                                        }
                                    }
                                    else
                                    {
                                        if (_rinse)
                                        {
                                        }
                                        else
                                        {
                                            CDID.Error(Order.Rows[row][e_DB_Order.Column05.ToString()].ToString(),
                                                       Order.Rows[row][e_DB_Order.Column08.ToString()].ToString(),
                                                       NG
                                                      );

                                            Log.Write(call, $"Process item NG - ID=[{_ID}] / Order_No=[{Order.Rows[row][e_DB_Order.Column05.ToString()]}] / Product_Code=[{_productCode}] / Product_Name=[{Order.Rows[row][e_DB_Order.Column08.ToString()]}] = [{NG}]");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                CDID.Error();
                            }
                            break;
                        case e_Sequence_Main.시작:
                            #region local

                            Order_No = Order.Rows[row][e_DB_Order.Column05.ToString()].ToString();
                            Barcode = Order.Rows[row][e_DB_Order.Column06.ToString()].ToString();
                            Product_Name = Order.Rows[row][e_DB_Order.Column08.ToString()].ToString();
                            Order_Cup = (e_Order_Cup)Enum.Parse(typeof(e_Order_Cup), Order.Rows[row][e_DB_Order.Column12.ToString()].ToString());

                            Log.Write(call, $"Order_No=[{Order_No}]");
                            Log.Write(call, $"Barcode=[{Barcode}]");
                            Log.Write(call, $"Product_Name=[{Product_Name}]");
                            Log.Write(call, $"Order_Cup=[{Order_Cup}]");

                            Coffee_ID = int.TryParse(GlobalVariable.Parameter[_parameterKey][_productKey][(int)e_Parameter_Product.Coffee_ID], out Coffee_ID) ? Coffee_ID : 0;
                            End_Delay = double.TryParse(GlobalVariable.Parameter[_parameterKey][_productKey][(int)e_Parameter_Product.End_Delay], out End_Delay) ? End_Delay : 0;
                            Cup = (e_ComboBox_Cup)Enum.Parse(typeof(e_ComboBox_Cup), GlobalVariable.Parameter[_parameterKey][_productKey][(int)e_Parameter_Product.Cup]);
                            Ice_Time = GlobalVariable.Parameter[_parameterKey][_productKey][(int)e_Parameter_Product.Ice_Time];
                            Water_Time = GlobalVariable.Parameter[_parameterKey][_productKey][(int)e_Parameter_Product.Water_Time];
                            Sauce_Type = (e_ComboBox_Sauce_Type)Enum.Parse(typeof(e_ComboBox_Sauce_Type), GlobalVariable.Parameter[_parameterKey][_productKey][(int)e_Parameter_Product.Sauce_Type]);
                            Sauce_No = (e_Device_Controller2_Sauce)Enum.Parse(typeof(e_Device_Controller2_Sauce), GlobalVariable.Parameter[_parameterKey][_productKey][(int)e_Parameter_Product.Sauce_No]);
                            Pumping_Count = GlobalVariable.Parameter[_parameterKey][_productKey][(int)e_Parameter_Product.Pumping_Count];
                            Mixing_Count = GlobalVariable.Parameter[_parameterKey][_productKey][(int)e_Parameter_Product.Mixing_Count];
                            Remark = GlobalVariable.Parameter[_parameterKey][_productKey][(int)e_Parameter_Product.Remark];
                            ade = Sauce_Type == e_ComboBox_Sauce_Type.Ade;

                            Log.Write(call, $"Coffee_ID=[{Coffee_ID}]");
                            Log.Write(call, $"End_Delay=[{End_Delay}]");
                            Log.Write(call, $"Cup=[{Cup}]");
                            Log.Write(call, $"Ice_Time=[{Ice_Time}]");
                            Log.Write(call, $"Water_Time=[{Water_Time}]");
                            Log.Write(call, $"Sauce_Type=[{Sauce_Type}]");
                            Log.Write(call, $"Sauce_No=[{Sauce_No}]");
                            Log.Write(call, $"Pumping_Count=[{Pumping_Count}]");
                            Log.Write(call, $"Mixing_Count=[{Mixing_Count}]");
                            Log.Write(call, $"Remark=[{Remark}]");
                            Log.Write(call, $"ade=[{ade}]");

                            #endregion

                            #region 병렬처리

                            if (GlobalFunction.GetSimulation() == false)
                            {
                                switch (Sauce_Type)
                                {
                                    case e_ComboBox_Sauce_Type.Syrup:
                                        switch (Order_Cup)
                                        {
                                            case e_Order_Cup.TAKEOUT:
                                                _startLittle = true;
                                                break;
                                        }
                                        break;
                                }

                                switch (Sauce_Type)
                                {
                                    case e_ComboBox_Sauce_Type.Unused:
                                    case e_ComboBox_Sauce_Type.Syrup:
                                        switch (Order_Cup)
                                        {
                                            case e_Order_Cup.TAKEOUT:
                                                _startCoffee = Coffee_ID > 0;
                                                break;
                                        }
                                        break;
                                }
                            }

                            #endregion

                            _curr_Sequence_Main = e_Sequence_Main.픽업;

                            // 병렬처리
                            switch (Order_Cup)
                            {
                                case e_Order_Cup.TUMBLER:
                                    _startTumbler = true;
                                    break;
                            }

                            CDID.Current(Order_No, Product_Name);
                            break;
                        case e_Sequence_Main.텀블러:
                            if (Process_Sequence_Sub_텀블러())
                            {
                                #region 병렬처리

                                if (GlobalFunction.GetSimulation() == false)
                                {
                                    switch (Sauce_Type)
                                    {
                                        case e_ComboBox_Sauce_Type.Unused:
                                        case e_ComboBox_Sauce_Type.Syrup:
                                            _startCoffee = Coffee_ID > 0;
                                            break;
                                    }
                                }

                                #endregion

                                _curr_Sequence_Main = e_Sequence_Main.픽업;
                            }
                            break;
                        case e_Sequence_Main.픽업:
                            if (Process_Sequence_Sub_픽업(Order_Cup, Cup, ref Robot_Cup, ade))
                            {
                                switch (Sauce_Type)
                                {
                                    case e_ComboBox_Sauce_Type.Syrup:
                                    case e_ComboBox_Sauce_Type.Ade:
                                        _curr_Sequence_Main = e_Sequence_Main.소스;

                                        if (_productCode == "06-I" ||
                                            _productCode == "07-I" ||
                                            _productCode == "08-I" ||
                                            _productCode == "17-I" ||
                                            _productCode == "18-I"
                                           )
                                        {
                                            if (Robot_Cup == e_Device_Robot_Cup.스팀피처)
                                            {
                                                _curr_Sequence_Main = e_Sequence_Main.스팀피처;
                                            }
                                        }
                                        break;
                                    case e_ComboBox_Sauce_Type.Unused:
                                        if (_productCode.Contains("I"))
                                        {
                                            _curr_Sequence_Main = e_Sequence_Main.얼음;
                                        }
                                        else
                                        {
                                            _curr_Sequence_Main = e_Sequence_Main.커피;
                                        }
                                        break;
                                }
                            }
                            break;
                        case e_Sequence_Main.스팀피처:
                            if (Process_Sequence_Sub_스팀피처(Robot_Cup, Ice_Time, ade))
                            {
                                _curr_Sequence_Main = e_Sequence_Main.소스;
                            }
                            break;
                        case e_Sequence_Main.소스:
                            if (Process_Sequence_Sub_소스(Robot_Cup, Sauce_Type, Sauce_No, Pumping_Count, Mixing_Count, ade))
                            {
                                if (Sauce_Type == e_ComboBox_Sauce_Type.Ade)
                                {
                                    _curr_Sequence_Main = e_Sequence_Main.얼음;
                                }
                                else
                                {
                                    switch (Robot_Cup)
                                    {
                                        case e_Device_Robot_Cup.스팀피처:
                                            if (_productCode == "06-I" ||
                                                _productCode == "07-I" ||
                                                _productCode == "08-I" ||
                                                _productCode == "17-I" ||
                                                _productCode == "18-I"
                                               )
                                            {
                                                _curr_Sequence_Main = e_Sequence_Main.커피;
                                            }
                                            else
                                            {
                                                if (_productCode.Contains("I"))
                                                {
                                                    _curr_Sequence_Main = e_Sequence_Main.얼음;
                                                }
                                                else
                                                {
                                                    _curr_Sequence_Main = e_Sequence_Main.커피;
                                                }
                                            }
                                            break;
                                        case e_Device_Robot_Cup.컵1:
                                        case e_Device_Robot_Cup.컵2:
                                        case e_Device_Robot_Cup.컵3:
                                        case e_Device_Robot_Cup.컵4:
                                            if (_productCode.Contains("I"))
                                            {
                                                _curr_Sequence_Main = e_Sequence_Main.얼음;
                                            }
                                            else
                                            {
                                                _curr_Sequence_Main = e_Sequence_Main.커피;
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                        case e_Sequence_Main.얼음:
                            if (Process_Sequence_Sub_얼음(Robot_Cup, Sauce_Type, Ice_Time, Water_Time, _productCode, ade))
                            {
                                if (Sauce_Type == e_ComboBox_Sauce_Type.Ade)
                                {
                                    _curr_Sequence_Main = e_Sequence_Main.도어;
                                }
                                else
                                {
                                    _curr_Sequence_Main = e_Sequence_Main.커피;
                                }
                            }
                            break;
                        case e_Sequence_Main.커피:
                            if (Process_Sequence_Sub_커피(Robot_Cup, _productCode, End_Delay, Water_Time, ade))
                            {
                                _curr_Sequence_Main = e_Sequence_Main.도어;
                            }
                            break;
                        case e_Sequence_Main.도어:
                            if (Process_Sequence_Sub_도어(Robot_Cup, ref Door, Barcode, ade))
                            {
                                DataTable data = GlobalFunction.DB.MySQL.GetDataTable($"SELECT * FROM {e_DB._Order} WHERE {e_DB_Order.Column01}='{_ID}'");

                                if (data.Rows[0][e_DB_Order.Column02.ToString()].ToString() == e_Order_Status.도어.ToString())
                                {
                                    GlobalFunction.DB.MySQL.Query($"UPDATE {e_DB._Order} SET {e_DB_Order.Column02}='{e_Order_Status.배출}' WHERE {e_DB_Order.Column01}='{_ID}'");
                                }

                                _curr_Sequence_Main = e_Sequence_Main.리셋;
                            }
                            break;
                        case e_Sequence_Main.리셋:
                            #region 필드

                            _ID = string.Empty;

                            _productCode = string.Empty;

                            _productKey = string.Empty;

                            _coffee = false;

                            #endregion

                            #region local

                            Order = null;

                            row = -1;

                            NG = string.Empty;

                            // Order
                            Order_No = string.Empty;
                            Barcode = string.Empty;
                            Product_Name = string.Empty;
                            Order_Cup = e_Order_Cup.TAKEOUT;

                            // Product
                            Coffee_ID = 0;
                            End_Delay = 0;
                            Cup = e_ComboBox_Cup.Hot;
                            Ice_Time = string.Empty;
                            Water_Time = string.Empty;
                            Sauce_Type = e_ComboBox_Sauce_Type.Unused;
                            Sauce_No = e_Device_Controller2_Sauce.Unused;
                            Pumping_Count = string.Empty;
                            Mixing_Count = string.Empty;
                            Remark = string.Empty;
                            ade = false;

                            // ref
                            Robot_Cup = e_Device_Robot_Cup.Undefined;
                            Door = e_Door.Undefined;

                            #endregion

                            #region 병렬처리

                            if (_curr_Sequence_Little > e_Sequence_Sub.Sub00)
                            {
                                _resetLittle = true;
                            }

                            if (_curr_Sequence_Coffee > e_Sequence_Sub.Sub00)
                            {
                                _resetCoffee = true;
                            }

                            if (_curr_Sequence_Tumbler > e_Sequence_Sub.Sub00)
                            {
                                _resetTumbler = true;
                            }

                            #endregion

                            #region Sub

                            _logSub00 = false;

                            _stopwatchSub.Reset();

                            #endregion

                            _curr_Sequence_Main = e_Sequence_Main.대기;

                            CDID.Current(string.Empty, string.Empty);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
                }

                System.Threading.Thread.Sleep(100);
            }
        }

        #region 메서드

        private string GetID(string value)
        {
            string result = string.Empty;
            try
            {
                result = value.Substring(0, value.LastIndexOf("_"));
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        private bool CheckID(string ID)
        {
            bool result = false;
            try
            {
                result = GlobalFunction.DB.MySQL.GetDataTable($"SELECT * FROM {e_DB._Order} WHERE {e_DB_Order.Column01} LIKE '{ID}%' AND {e_DB_Order.Column02}!='{e_Order_Status.주문}' ORDER BY {e_DB_Order.Column01} ASC").Rows.Count > 0;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        private bool CheckUnlockDoor()
        {
            bool result = false;
            try
            {
                for (int i = (int)e_Door.Door1; i < Enum.GetNames(typeof(e_Door)).Length; i++)
                {
                    if (GlobalFunction.Door.GetLock((e_Door)i) == e_Door_Lock.Unlock)
                    {
                        result = true;

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        private string _prev_Waiting = string.Empty;
        private string _curr_Waiting = string.Empty;

        private string GetWaiting()
        {
            string result = string.Empty;
            try
            {
                DataTable data = GlobalFunction.DB.MySQL.GetDataTable($"SELECT DISTINCT {e_DB_Order.Column05} FROM {e_DB._Order} WHERE {e_DB_Order.Column02}='{e_Order_Status.주문}' ORDER BY {e_DB_Order.Column05} ASC");

                for (int i = 0; i < data.Rows.Count; i++)
                {
                    result += data.Rows[i][e_DB_Order.Column05.ToString()].ToString();

                    if (i < data.Rows.Count - 1)
                    {
                        result += "/";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        #endregion

        #endregion

        #region Sequence_Little

        private e_Sequence_Sub _prev_Sequence_Little = e_Sequence_Sub.Sub00;
        private e_Sequence_Sub _curr_Sequence_Little = e_Sequence_Sub.Sub00;

        #region 필드

        private bool _startLittle = false;
        private bool _resetLittle = false;

        #endregion

        private System.Threading.Thread _ThreadSequence_Little = null;
        private bool _isThreadSequence_Little = false;
        private void Process_Sequence_Little()
        {
            string call = "Sequence_Little";

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

            #region local

            double Little_Delay = 0;
            int Coffee_ID = 0;

            #endregion

            while (_isThreadSequence_Little)
            {
                try
                {
                    #region Reset

                    if (_resetLittle)
                    {
                        _resetLittle = false;

                        _curr_Sequence_Little = e_Sequence_Sub.Sub09;
                    }

                    #endregion

                    #region prev != curr

                    if (_prev_Sequence_Little != _curr_Sequence_Little)
                    {
                        Log.Write(call, $"[{_prev_Sequence_Little}] Complete");
                        _prev_Sequence_Little = _curr_Sequence_Little;
                        Log.Write(call, $"[{_curr_Sequence_Little}] Start");
                    }

                    #endregion

                    switch (_curr_Sequence_Little)
                    {
                        case e_Sequence_Sub.Sub00:
                            if (_startLittle)
                            {
                                _startLittle = false;

                                _curr_Sequence_Little = e_Sequence_Sub.Sub01;
                            }
                            break;
                        case e_Sequence_Sub.Sub01:
                            Little_Delay = double.TryParse(GlobalVariable.Parameter[_parameterKey][_productKey][(int)e_Parameter_Product.Little_Delay], out Little_Delay) ? Little_Delay : 0;

                            switch (_productCode)
                            {
                                case "06-H":
                                case "06-I":
                                case "07-H":
                                case "07-I":
                                case "08-H":
                                case "08-I":
                                    Coffee_ID = CONST.N_EVERSYS_PRODUCT_ID_ESPRESSO;
                                    break;
                                case "17-H":
                                case "17-I":
                                case "18-H":
                                case "18-I":
                                    Coffee_ID = CONST.N_EVERSYS_PRODUCT_ID_HOT_MILK;
                                    break;
                            }

                            Log.Write(call, $"Little_Delay=[{Little_Delay}]");
                            Log.Write(call, $"Coffee_ID=[{Coffee_ID}]");

                            stopwatch.Restart();

                            _curr_Sequence_Little = e_Sequence_Sub.Sub02;
                            break;
                        case e_Sequence_Sub.Sub02:
                            if (stopwatch.ElapsedMilliseconds >= 1000 * Little_Delay)
                            {
                                stopwatch.Stop();

                                _curr_Sequence_Little = e_Sequence_Sub.Sub03;
                            }
                            break;
                        case e_Sequence_Sub.Sub03:
                            if (GlobalFunction.CoffeeMaker.CoffeeMilkL.CheckIdle(call))
                            {
                                if (GlobalDevice.CoffeeMaker.Instance.DoProduct(Coffee_ID))
                                {
                                    _curr_Sequence_Little = e_Sequence_Sub.Sub04;
                                }
                            }
                            else
                            {
                                _curr_Sequence_Little = e_Sequence_Sub.Sub04;
                            }
                            break;
                        case e_Sequence_Sub.Sub04:
                            if (GlobalFunction.CoffeeMaker.CoffeeMilkL.CheckRun(call, Coffee_ID))
                            {
                                _curr_Sequence_Little = e_Sequence_Sub.Sub09;
                            }
                            else
                            {
                                _curr_Sequence_Little = e_Sequence_Sub.Sub03;
                            }
                            break;
                        case e_Sequence_Sub.Sub05:
                            break;
                        case e_Sequence_Sub.Sub06:
                            break;
                        case e_Sequence_Sub.Sub07:
                            break;
                        case e_Sequence_Sub.Sub08:
                            break;
                        case e_Sequence_Sub.Sub09:
                            #region 필드

                            _startLittle = false;
                            _resetLittle = false;

                            #endregion

                            #region local

                            Little_Delay = 0;
                            Coffee_ID = 0;

                            #endregion

                            stopwatch.Reset();

                            _curr_Sequence_Little = e_Sequence_Sub.Sub00;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
                }

                System.Threading.Thread.Sleep(100);
            }
        }

        #endregion

        #region Sequence_Coffee

        private e_Sequence_Sub _prev_Sequence_Coffee = e_Sequence_Sub.Sub00;
        private e_Sequence_Sub _curr_Sequence_Coffee = e_Sequence_Sub.Sub00;

        #region 필드

        private bool _startCoffee = false;
        private bool _resetCoffee = false;

        #endregion

        private System.Threading.Thread _ThreadSequence_Coffee = null;
        private bool _isThreadSequence_Coffee = false;
        private void Process_Sequence_Coffee()
        {
            string call = "Sequence_Coffee";

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

            #region local

            int Coffee_ID = 0;
            double Start_Delay = 0;
            e_ComboBox_Sauce_Type Sauce_Type = e_ComboBox_Sauce_Type.Unused;

            #endregion

            while (_isThreadSequence_Coffee)
            {
                try
                {
                    #region Reset

                    if (_resetCoffee)
                    {
                        _resetCoffee = false;

                        _curr_Sequence_Coffee = e_Sequence_Sub.Sub09;
                    }

                    #endregion

                    #region prev != curr

                    if (_prev_Sequence_Coffee != _curr_Sequence_Coffee)
                    {
                        Log.Write(call, $"[{_prev_Sequence_Coffee}] Complete");
                        _prev_Sequence_Coffee = _curr_Sequence_Coffee;
                        Log.Write(call, $"[{_curr_Sequence_Coffee}] Start");
                    }

                    #endregion

                    switch (_curr_Sequence_Coffee)
                    {
                        case e_Sequence_Sub.Sub00:
                            if (_startCoffee)
                            {
                                _startCoffee = false;

                                _curr_Sequence_Coffee = e_Sequence_Sub.Sub01;
                            }
                            break;
                        case e_Sequence_Sub.Sub01:
                            Coffee_ID = int.TryParse(GlobalVariable.Parameter[_parameterKey][_productKey][(int)e_Parameter_Product.Coffee_ID], out Coffee_ID) ? Coffee_ID : 0;
                            Start_Delay = double.TryParse(GlobalVariable.Parameter[_parameterKey][_productKey][(int)e_Parameter_Product.Start_Delay], out Start_Delay) ? Start_Delay : 0;
                            Sauce_Type = (e_ComboBox_Sauce_Type)Enum.Parse(typeof(e_ComboBox_Sauce_Type), GlobalVariable.Parameter[_parameterKey][_productKey][(int)e_Parameter_Product.Sauce_Type]);

                            Log.Write(call, $"Coffee_ID=[{Coffee_ID}]");
                            Log.Write(call, $"Start_Delay=[{Start_Delay}]");
                            Log.Write(call, $"Sauce_Type=[{Sauce_Type}]");

                            _curr_Sequence_Coffee = e_Sequence_Sub.Sub02;
                            break;
                        case e_Sequence_Sub.Sub02:
                            switch (Sauce_Type)
                            {
                                case e_ComboBox_Sauce_Type.Unused:
                                    stopwatch.Restart();

                                    _curr_Sequence_Coffee = e_Sequence_Sub.Sub03;
                                    break;
                                case e_ComboBox_Sauce_Type.Syrup:
                                    switch (_productCode)
                                    {
                                        case "17-H":
                                        case "17-I":
                                        case "18-H":
                                        case "18-I":
                                            if (_curr_Sequence_Main > e_Sequence_Main.소스)
                                            {
                                                _curr_Sequence_Coffee = e_Sequence_Sub.Sub06;
                                            }
                                            break;
                                        default:
                                            if ( _curr_Sequence_Main > e_Sequence_Main.소스 ||
                                                (_curr_Sequence_Main == e_Sequence_Main.소스 && _curr_Sequence_Sub > e_Sequence_Sub.Sub03)
                                               )
                                            {
                                                stopwatch.Restart();

                                                _curr_Sequence_Coffee = e_Sequence_Sub.Sub03;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case e_Sequence_Sub.Sub03:
                            if (stopwatch.ElapsedMilliseconds >= 1000 * Start_Delay)
                            {
                                stopwatch.Stop();

                                _curr_Sequence_Coffee = e_Sequence_Sub.Sub04;
                            }
                            break;
                        case e_Sequence_Sub.Sub04:
                            if (GlobalFunction.CoffeeMaker.CoffeeMilkL.CheckIdle(call))
                            {
                                if (GlobalDevice.CoffeeMaker.Instance.DoProduct(Coffee_ID))
                                {
                                    _curr_Sequence_Coffee = e_Sequence_Sub.Sub05;
                                }
                            }
                            else
                            {
                                _curr_Sequence_Coffee = e_Sequence_Sub.Sub05;
                            }
                            break;
                        case e_Sequence_Sub.Sub05:
                            if (GlobalFunction.CoffeeMaker.CoffeeMilkL.CheckRun(call, Coffee_ID))
                            {
                                _curr_Sequence_Coffee = e_Sequence_Sub.Sub09;
                            }
                            else
                            {
                                _curr_Sequence_Coffee = e_Sequence_Sub.Sub04;
                            }
                            break;
                        case e_Sequence_Sub.Sub06:
                            if (GlobalDevice.Robot.Feedback[(int)e_Device_Robot_Feedback.D3B2_커피머신_Coffee_위치_이동_완료])
                            {
                                _curr_Sequence_Coffee = e_Sequence_Sub.Sub04;
                            }
                            break;
                        case e_Sequence_Sub.Sub07:
                            break;
                        case e_Sequence_Sub.Sub08:
                            break;
                        case e_Sequence_Sub.Sub09:
                            #region 필드

                            _startCoffee = false;
                            _resetCoffee = false;

                            #endregion

                            #region local

                            Coffee_ID = 0;
                            Start_Delay = 0;
                            Sauce_Type = e_ComboBox_Sauce_Type.Unused;

                            #endregion

                            stopwatch.Reset();

                            _curr_Sequence_Coffee = e_Sequence_Sub.Sub00;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
                }

                System.Threading.Thread.Sleep(100);
            }
        }

        #endregion

        #region Sequence_Tumbler

        private e_Sequence_Sub _prev_Sequence_Tumbler = e_Sequence_Sub.Sub00;
        private e_Sequence_Sub _curr_Sequence_Tumbler = e_Sequence_Sub.Sub00;

        #region 필드

        private bool _startTumbler = false;
        private bool _resetTumbler = false;

        #endregion

        private System.Threading.Thread _ThreadSequence_Tumbler = null;
        private bool _isThreadSequence_Tumbler = false;
        private void Process_Sequence_Tumbler()
        {
            string call = "Sequence_Tumbler";

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

            #region local

            int Coffee_ID = 0;

            #endregion

            while (_isThreadSequence_Tumbler)
            {
                try
                {
                    #region Reset

                    if (_resetTumbler)
                    {
                        _resetTumbler = false;

                        _curr_Sequence_Tumbler = e_Sequence_Sub.Sub09;
                    }

                    #endregion

                    #region prev != curr

                    if (_prev_Sequence_Tumbler != _curr_Sequence_Tumbler)
                    {
                        Log.Write(call, $"[{_prev_Sequence_Tumbler}] Complete");
                        _prev_Sequence_Tumbler = _curr_Sequence_Tumbler;
                        Log.Write(call, $"[{_curr_Sequence_Tumbler}] Start");
                    }

                    #endregion

                    switch (_curr_Sequence_Tumbler)
                    {
                        case e_Sequence_Sub.Sub00:
                            if (_startTumbler)
                            {
                                _startTumbler = false;

                                _curr_Sequence_Tumbler = e_Sequence_Sub.Sub01;
                            }
                            break;
                        case e_Sequence_Sub.Sub01:
                            if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B2_LM_Up] == false)
                            {
                                if (GlobalDevice.Controller1.Instance.SetDoor(e_Door.Door4, e_Device_Controller1_Door_Command.Tumbler_Home))
                                {
                                    _curr_Sequence_Tumbler = e_Sequence_Sub.Sub02;
                                }
                            }
                            else
                            {
                                _curr_Sequence_Tumbler = e_Sequence_Sub.Sub02;
                            }
                            break;
                        case e_Sequence_Sub.Sub02:
                            if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B3_Door4_Open] == false)
                            {
                                if (GlobalDevice.Controller1.Instance.SetDoor(e_Door.Door4, e_Device_Controller1_Door_Command.Open))
                                {
                                    _curr_Sequence_Tumbler = e_Sequence_Sub.Sub03;
                                }
                            }
                            else
                            {
                                _curr_Sequence_Tumbler = e_Sequence_Sub.Sub03;
                            }
                            break;
                        case e_Sequence_Sub.Sub03:
                            if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B4_Coffee4])
                            {
                                stopwatch.Restart();

                                _curr_Sequence_Tumbler = e_Sequence_Sub.Sub04;
                            }
                            break;
                        case e_Sequence_Sub.Sub04:
                            if (stopwatch.ElapsedMilliseconds >= 1000 * 5)
                            {
                                stopwatch.Stop();

                                if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D2B4_Coffee4])
                                {
                                    _curr_Sequence_Tumbler = e_Sequence_Sub.Sub05;
                                }
                                else
                                {
                                    _curr_Sequence_Tumbler = e_Sequence_Sub.Sub03;
                                }
                            }
                            break;
                        case e_Sequence_Sub.Sub05:
                            if (GlobalDevice.Controller1.Sensor[(int)e_Device_Controller1_Sensor.D1B7_Door4_Close] == false)
                            {
                                if (GlobalDevice.Controller1.Instance.SetDoor(e_Door.Door4, e_Device_Controller1_Door_Command.Close))
                                {
                                    _curr_Sequence_Tumbler = e_Sequence_Sub.Sub06;
                                }
                            }
                            else
                            {
                                _curr_Sequence_Tumbler = e_Sequence_Sub.Sub06;
                            }
                            break;
                        case e_Sequence_Sub.Sub06:
                            if (GlobalDevice.Controller1.Instance.SetDoor(e_Door.Door4, e_Device_Controller1_Door_Command.Tumbler_Check))
                            {
                                _curr_Sequence_Tumbler = e_Sequence_Sub.Sub07;
                            }
                            break;
                        case e_Sequence_Sub.Sub07:
                            if (GlobalDevice.Controller1.Instance.SetDoor(e_Door.Door4, e_Device_Controller1_Door_Command.Tumbler_Ready))
                            {
                                _curr_Sequence_Tumbler = e_Sequence_Sub.Sub08;
                            }
                            break;
                        case e_Sequence_Sub.Sub08:
                            Coffee_ID = int.TryParse(GlobalVariable.Parameter[_parameterKey][_productKey][(int)e_Parameter_Product.Coffee_ID], out Coffee_ID) ? Coffee_ID : 0;

                            Log.Write(call, $"Coffee_ID=[{Coffee_ID}]");

                            #region 병렬처리

                            if (GlobalFunction.GetSimulation() == false)
                            {
                                _startCoffee = Coffee_ID > 0;
                            }

                            #endregion

                            _curr_Sequence_Tumbler = e_Sequence_Sub.Sub09;
                            break;
                        case e_Sequence_Sub.Sub09:
                            #region 필드

                            _startTumbler = false;
                            _resetTumbler = false;

                            #endregion

                            #region local

                            Coffee_ID = 0;

                            #endregion

                            stopwatch.Reset();

                            _curr_Sequence_Tumbler = e_Sequence_Sub.Sub00;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
                }

                System.Threading.Thread.Sleep(100);
            }
        }

        #endregion
    }
}
