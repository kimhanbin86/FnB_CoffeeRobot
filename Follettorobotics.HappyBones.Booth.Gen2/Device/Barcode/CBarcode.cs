using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;
using System.Reflection;

using Library;
using Library.SerialPorts;

namespace Follettorobotics.HappyBones.Booth.Gen2
{
    public class CBarcode : RS232
    {
        private string _data = string.Empty;

        public override void Close()
        {
            try
            {
                if (_SerialPort != null)
                {
                    _SerialPort.DataReceived -= new SerialDataReceivedEventHandler(DataReceived);
                }
            }
            catch (Exception ex)
            {
                LogWrite(MethodBase.GetCurrentMethod().Name, Utility.GetString(ex));
            }
            finally
            {
                base.Close();
            }
        }

        public override bool Open()
        {
            bool result = false;
            try
            {
                if (result = base.Open())
                {
                    _SerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
                }
            }
            catch (Exception ex)
            {
                LogWrite(MethodBase.GetCurrentMethod().Name, Utility.GetString(ex));
            }
            return result;
        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                System.Threading.Thread.Sleep(100);

                _data += _SerialPort.ReadExisting();

                int idx = _data.IndexOf(CONST.S_CRLF);

                if (idx >= 0)
                {
                    GlobalDevice.Barcode.Data = _data.Substring(0, idx);

                    _data = string.Empty;

                    LogWrite(MethodBase.GetCurrentMethod().Name, $"[{GlobalDevice.Barcode.Data}]");
                }
            }
            catch (Exception ex)
            {
                LogWrite(MethodBase.GetCurrentMethod().Name, Utility.GetString(ex));
            }
        }
    }
}
