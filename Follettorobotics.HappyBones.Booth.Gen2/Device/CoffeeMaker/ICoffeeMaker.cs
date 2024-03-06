using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Follettorobotics.HappyBones.Booth.Gen2
{
    #region enum

    public enum e_Device_CoffeeMaker
    {
        Eversys,
    }

    #endregion

    public interface ICoffeeMaker
    {
        #region EversysApi.Services.SerialService

        bool IsConnected { get; }

        void Disconnect();

        bool Connect(string port);

        #endregion

        bool DoProduct(int productId);

        bool DoRinse();

        bool GetInfoMessages();

        bool GetStatus();
    }
}
