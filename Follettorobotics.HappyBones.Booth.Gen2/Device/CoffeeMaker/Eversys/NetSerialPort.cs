using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;
using System.Reflection;

using EversysApi.Services;

namespace Follettorobotics.HappyBones.Booth.Gen2
{
    public class NetSerialPort : ISerialPort
    {
        private const string _className = "NetSerialPort::";

        #region 필드

        public event Action DataReceived = delegate { };

        private SerialPort port;

        #endregion

        #region 속성

        public int BaudRate
        {
            get => port.BaudRate;
            set => port.BaudRate = value;
        }

        public bool IsOpen
        {
            get => port != null ? port.IsOpen : false;
        }

        public string PortName
        {
            get => port.PortName;
            set => port.PortName = value;
        }

        public int ReadBufferSize
        {
            get => port.ReadBufferSize;
            set => port.ReadBufferSize = value;
        }

        public int WriteBufferSize
        {
            get => port.WriteBufferSize;
            set => port.WriteBufferSize = value;
        }

        #endregion

        #region 메서드

        public void Close()
        {
            port.Close();

            GlobalFunction.CoffeeMaker.LogWrite(_className + MethodBase.GetCurrentMethod().Name, $"Close OK ({PortName})");
        }

        public string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        public ISerialPort Init(string portName, int baudRate)
        {
            if (port != null) port.Close();
            port = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
            //DataReceived = null;
            return this;
        }

        public void Open()
        {
            try
            {
                port.Open();
                port.DataReceived += delegate { DataReceived(); };
            }
            catch
            {
            }

            GlobalFunction.CoffeeMaker.LogWrite(_className + MethodBase.GetCurrentMethod().Name, $"Open {(IsOpen ? "OK" : "NG")} ({PortName})");
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            int nbrOfBytes = port.Read(buffer, offset, count);
            return nbrOfBytes;
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            port.Write(buffer, offset, count);
        }

        #endregion
    }
}
