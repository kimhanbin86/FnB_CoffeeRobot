using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Follettorobotics.HappyBones.Booth.Gen2
{
    #region enum

    public enum e_Device_IceMaker
    {
        ICETRO,
    }

    #endregion

    public interface IIceMaker
    {
        #region AClass

        bool LogEnabled { get; set; }

        event Library.LogMsgEventHandler LogMsgEvent;

        #endregion

        #region ADevice

        string Device { get; set; }

        #endregion

        #region ASerialPort

        bool IsOpen { get; }
        string PortName { get; set; }
        int BaudRate { get; set; }
        System.IO.Ports.Parity Parity { get; set; }
        int DataBits { get; set; }
        System.IO.Ports.StopBits StopBits { get; set; }
        int ReadTimeout { get; set; }
        int ReadBufferSize { get; set; }
        int WriteTimeout { get; set; }
        int WriteBufferSize { get; set; }

        #endregion

        #region RS232

        void Dispose();

        bool Open();

        #endregion

        int Timeout { set; }

        bool GetStatus(ref byte[] bytes);

        bool SetTime(string iceTime, string waterTime);
    }
}
