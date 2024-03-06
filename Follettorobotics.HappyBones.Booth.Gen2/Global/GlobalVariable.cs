using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Follettorobotics.HappyBones.Booth.Gen2
{
    #region enum

    #region Alarm

    // Alarm Parameter
    public enum e_Alarm
    {
        #region ERROR_DB

        ERROR_DB_GET_DATA,
        ERROR_DB_SET_DATA,

        #endregion
        #region ERROR_DEVICE_COMM

        ERROR_DEVICE_COMM_COFFEE_MAKER,
        ERROR_DEVICE_COMM_CONTROLLER1,
        ERROR_DEVICE_COMM_CONTROLLER2,
        ERROR_DEVICE_COMM_ICE_MAKER,
        ERROR_DEVICE_COMM_ROBOT,

        #endregion
        #region ERROR_DEVICE_START

        ERROR_DEVICE_START,
        ERROR_DEVICE_START_BARCODE,
        ERROR_DEVICE_START_COFFEE_MAKER,
        ERROR_DEVICE_START_CONTROLLER1,
        ERROR_DEVICE_START_CONTROLLER2,
        ERROR_DEVICE_START_ICE_MAKER,
        ERROR_DEVICE_START_KIOSK,
        ERROR_DEVICE_START_REMOTE,
        ERROR_DEVICE_START_ROBOT,

        #endregion
        #region ERROR_DEVICE_STATUS

        ERROR_DEVICE_STATUS_BARCODE,
        ERROR_DEVICE_STATUS_COFFEE_MAKER,
        ERROR_DEVICE_STATUS_CONTROLLER1,
        ERROR_DEVICE_STATUS_CONTROLLER2,
        ERROR_DEVICE_STATUS_ICE_MAKER,
        ERROR_DEVICE_STATUS_KIOSK,
        ERROR_DEVICE_STATUS_REMOTE,
        ERROR_DEVICE_STATUS_ROBOT,

        #endregion
        #region ERROR_PARAMETER

        ERROR_PARAMETER_BACKUP,
        ERROR_PARAMETER_LOAD,
        ERROR_PARAMETER_SAVE,
        ERROR_PARAMETER_UPDATE,

        #endregion
        #region CoffeeMaker - Eversys - Warnings

        WarnCoffeeReserve01_e,
        WarnCoffeeReserve02_e,
        WarnCoffeeBoilerHeatLft_e,
        WarnCoffeeBoilerHeatRgt_e,
        WarnCoffeeFlowWaterTooLowLft_e,
        WarnCoffeeFlowWaterTooLowRgt_e,
        WarnCoffeeCleanIfrFail_e,
        WarnCoffeeCleanTabsEmpty_e,
        WarnCoffeeCleanCreditOn_e,
        WarnCoffeeCleanBallDispMiss_e,
        WarnCoffeeProDataInvalid_e,
        WarnCoffeeBrewChInitFailLft_e,
        WarnCoffeeBrewChInitFailRgt_e,
        WarnCoffeeDoService_e,
        WarnCoffeeChangeWaterFilter_e,
        WarnCoffeeBopHallFailLft_e,
        WarnCoffeeBopHallFailRgt_e,
        WarnCoffeeNoGrinderTurnsLft_e,
        WarnCoffeeMilkTankEmptyLft_e,
        WarnCoffeeStmPressForMlk_e,
        WarnCoffeeTopMotorFailLft_e,
        WarnCoffeeTopMotorFailRgt_e,
        WarnCoffeeBopMotorFailLft_e,
        WarnCoffeeBopMotorFailRgt_e,
        WarnCoffeeMilkTankEmptyRgt_e,
        WarnCoffeeNoGrinderTurnsRgt_e,
        WarnCoffeeMachDataCorrupt_e,
        WarnCoffeeDateTimeIncorrectLeft_e,
        WarnCoffeeDateTimeIncorrectRight_e,
        WarnCoffeeExternEepromFail_e,
        WarnCoffeeExternEepromBackupFail_e,
        WarnCoffeeInternalRinse_e,
        WarnCoffeeMilkShortRinseLft,
        WarnCoffeeMilkShortRinseRgt,
        WarnCoffeeNoPowderInChuteLft_e,
        WarnCoffeeNoPowderInChuteRgt_e,
        WarnCoffeePushBrewChamberBack_e,
        WarnCoffeeNoMilkDetergent_e,
        WarnCoffeeNTCBoilerFailLeft_e,
        WarnCoffeeNTCBoilerFailRight_e,
        WarnCoffeeNTCMilkHeatOutFailLeft_e,
        WarnCoffeeNTCMilkHeatOutFailRight_e,
        WarnCoffeeMachCounterRequested_e,
        WarnCoffeeMemoryUsageTooHigh_e,
        WarnCoffeeBackupOfMachParamsFailed_e,
        WarnCoffeeMilkOverTempLft_e,
        WarnCoffeeMilkOverTempRgt_e,
        WarnCoffeeRestartMessage_e,
        WarnCoffeeCleanWeekendMode_e,
        WarnCoffeeMilkControlFailLeft_e,
        WarnCoffeeFanFront_e,
        WarnCoffeeFanGrinderLeft_e,
        WarnCoffeeFanGrinderRight_e,
        WarnCoffeeMilkControlFailRight_e,
        WarnCoffeeMilkPumpMotorFailLft_e,
        WarnCoffeeMilkPumpMotorFailRgt_e,
        WarnCoffeeNTCMilkInFailLeft_e,
        WarnCoffeeNTCMilkInFailRight_e,
        WarnCoffeeMilkTankTempLft_e,
        WarnCoffeeMilkTankTempRgt_e,
        WarnCoffeeMilkUnitConnect_e,
        WarnCoffeeExtractCtrlErrorLeft_e,
        WarnCoffeeExtractCtrlErrorRight_e,
        WarnCoffeeExtractCtrlLimitLeft_e,
        WarnCoffeeExtractCtrlLimitRight_e,
        WarnCoffeeUpcomingService_e,
        WarnCoffeeUpcomingWaterFilter_e,
        WarnCoffeeExtractCtrlSideDiff_e,
        WarnCoffeeGrinderBlockedLft_e,
        WarnCoffeeGrinderBlockedRgt_e,
        WarnCoffeeMilkTempTooLowLeft_e,
        WarnCoffeeMilkTempTooLowRight_e,
        WarnCoffeeFlowRateSideDiff_e,
        WarnCoffeeRinseFlowRateTooLowLft_e,
        WarnCoffeeRinseFlowRateTooLowRgt_e,
        WarnCoffeeExtractTimeTooHighLft_e,
        WarnCoffeeExtractTimeTooHighRgt_e,
        WarnCoffeeRinseFlowRateTooHighLft_e,
        WarnCoffeeRinseFlowRateTooHighRgt_e,
        WarnCoffeeFlowRateUnkwownNozzle_e,
        WarnCoffeeGrinderDeblockFailLft_e,
        WarnCoffeeGrinderDeblockFailRgt_e,
        WarnCoffeeWaterPumpMotorFail_e,
        WarnCoffeeNTCBoilerOutletFail_e,
        WarnCoffeeHopperLevelLeft_e,
        WarnCoffeeHopperLevelRight_e,
        WarnCoffeeMilkInitRinse_e,
        WarnCoffeeTopNullPosFailLft_e,
        WarnCoffeeTopNullPosFailRgt_e,
        WarnCoffeeLinePressureLow_e,
        WarnFanPowder_e,
        WarnPowderDispenserMotorLeft_e,
        WarnPowderDispenserMotorRight_e,
        WarnPowderPump_e,
        WarnPowderMixerMotor_e,
        WarnMilkUnitNotSupported_e,
        WarnMilkInputTempColdFoamLft_e,
        WarnMilkInputTempColdFoamRgt_e,
        WarnPowderMixerMiss_e,
        WarnCoffeeSdCardMissing_e,
        WarnCoffeeMax_e,
        WarnSteamPressureMax_e,
        WarnSteamPressureTooHigh_e,
        WarnSteamPurgeLeak_e,
        WarnSteamNTCSteamWandLeft_e,
        WarnSteamNTCSteamWandRight_e,
        WarnSteamMax_e,
        WarnUndef_e,

        #endregion
        #region CoffeeMaker - Eversys - Stops

        StopCoffeePowerUp_e,
        StopCoffeeBoilerHeat_e,
        StopCoffeeBeanHopperMissRear_e,
        StopCoffeeBeanHopperMissFront_e,
        StopCoffeeEmptyGrdsDrwrBefCln_e,
        StopCoffeeEmptyGrdsDrwrAftCln_e,
        StopCoffeeGrdsDrwrFull_e,
        StopCoffeeGrdsDrwrTime_e,
        StopCoffeeGrdsDrwrEmpty_e,
        StopCoffeeGrdsDrwrMiss_e,
        StopCoffeeHopperEmptyFront_e,
        StopCoffeeHopperEmptyRear_e,
        StopCoffeeHopperEmpty_e,
        StopCoffeeCofQntyTooHighLft_e,
        StopCoffeePushRinse_e,
        StopCoffeeCleanIfrFail_e,
        StopCoffeeCleanTabsEmpty_e,
        StopCoffeeCleanBrewGroup_e,
        StopCoffeeCleanNotFinish_e,
        StopCoffeePushContinue_e,
        StopCoffeeBrewChamberFail_e,
        StopCoffeeGroundsBinFull_e,
        StopCoffeeCofQntyTooHighRgt_e,
        StopCoffeeCleanWaitAllReady_e,
        StopCoffeeCleanMilkTankAfter_e,
        StopCoffeeGrdsDrwrAlert_e,
        StopCoffeeCleanMilkTankBefore_e,
        StopCoffeeInterfaceNotDetected_e,
        StopCoffeeNoMilkDetergent_e,
        StopCoffeeMilkFridgeDoorOpen_e,
        StopCoffeeSourceWaterTankEmpty_e,
        StopCoffeeWasteWaterTankFull_e,
        StopCoffeeMilkUnitConnect_e,
        StopCoffeeDisplayOpen_e,
        StopCoffeeHopperLevelLeft_e,
        StopCoffeeHopperLevelRight_e,
        StopCoffeeSourceWaterTankRefilled_e,
        StopCoffeeWasteWaterTankEmptied_e,
        StopCoffeeCleanWaterTankBefore_e,
        StopCoffeeReplaceGrdsDrwrBefCln_e,
        StopCoffeeReplaceGrdsDrwrAftCln_e,
        StopCleanPowderUnitBefore_e,
        StopCoffeeMax_e,
        StopSteamTeaBoilerHeat_e,
        StopSteamReBoot_e,
        StopSteamEmptyBoiler_e,
        StopSteamDepressurize_e,
        StopSteamFillBoiler_e,
        StopSteamPressRinse_e,
        StopSteamPowerInit_e,
        StopSteamSecurityProbe_e,
        StopSteamTsDisplayOpen_e,
        StopSteamMax_e,
        StopUndef_e,

        #endregion
        #region CoffeeMaker - Eversys - Errors

        ErrorCoffeeBeanHopperMissRear_e,
        ErrorCoffeeBeanHopperMissFront_e,
        ErrorCoffeeGrdsDrwrMiss_e,
        ErrorCoffeeMotorTimeout_e,
        ErrorCoffeeMotorErrorCoffee_e,
        ErrorCoffeeMotorInit_e,
        ErrorCoffeeBoardType_e,
        ErrorCoffeeHydrUnitConnect_e,
        ErrorCoffeeMilkUnitConnect_e,
        ErrorCoffeeBrewUnitConnect_e,
        ErrorCoffeeGrindUnitConnect_e,
        ErrorCoffeePowerConfigNotDetected_e,
        ErrorCoffeeSoftwareUpdateRunning_e,
        WrongMachineType_e,
        MachineType_e,
        SoftwareVersionControl_e,
        ComTimeout_e,
        ErrorCoffeeMax_e,
        ErrorSteamPressureSensZero_e,
        ErrorSteamPressureSensTooHigh_e,
        ErrorSteamTimeoutDeprEmptyBoiler_e,
        ErrorSteamTimeoutWaterFill_e,
        ErrorSteamTimeoutSteamHeating_e,
        ErrorSteamBoilerConnector_e,
        ErrorSteamMax_e,
        ErrorUndef_e,

        #endregion
        #region IceMaker - ICETRO

        ICETRO_CMD2_0x00,
        ICETRO_CMD2_0x01,
        ICETRO_CMD2_0x02,
        ICETRO_CMD2_0x03,
        ICETRO_CMD2_0x04,
        ICETRO_CMD2_0x05,
        ICETRO_CMD2_0x06,
        ICETRO_CMD2_0x07,
        ICETRO_CMD2_0x08,
        ICETRO_CMD2_0x09,
        ICETRO_CMD2_0x0A,
        ICETRO_CMD2_0x0B,
        ICETRO_CMD2_0x0C,

        #endregion
        #region Sensor

        CUP1_EMPTY,
        CUP2_EMPTY,
        CUP3_EMPTY,
        CUP4_EMPTY,

        MILK_EMPTY,
        NBOX_ALARM,

        SAUCE1_EMPTY,
        SAUCE2_EMPTY,
        SAUCE3_EMPTY,
        SAUCE4_EMPTY,
        SAUCE5_EMPTY,
        SAUCE6_EMPTY,

        #endregion
    }

    #endregion

    #region ComboBox

    public enum e_ComboBox_Cup
    {
        Hot,
        Ice,
    }

    public enum e_ComboBox_DB_Type
    {
        MySQL,
    }

    public enum e_ComboBox_Language
    {
        en,
        ko,
    }

    public enum e_ComboBox_Sauce_Type
    {
        Unused,
        Syrup,
        Ade,
    }

    public enum e_ComboBox_Use
    {
        Unused,
        Use,
    }

    public enum e_ComboBox_DID_Thema
    {
        Thema,
    }

    #endregion

    #region DB

    public enum e_DB
    {
        _Alarm,
        _Door,
        _Order,
    }

    public enum e_DB_Alarm
    {
        Column00, // DateTime_Event
        Column01, // DateTime_Close
        Column02, // Code
        Column03, // Text
        Column04, // Action
        Column05, // 
        Column06, // 
        Column07, // 
        Column08, // 
        Column09, // 
        Column10, // 
        Column11, // 
        Column12, // 
        Column13, // 
        Column14, // 
        Column15, // 
        Column16, // 
        Column17, // 
        Column18, // 
        Column19, // 
    }

    public enum e_DB_Door
    {
        Column00, // Door
        Column01, // Lock
        Column02, // Trigger
        Column03, // DateTime
        Column04, // ID
        Column05, // Barcode
        Column06, // Cup
        Column07, // Sensor
        Column08, // 
        Column09, // 
        Column10, // 
        Column11, // 
        Column12, // 
        Column13, // 
        Column14, // 
        Column15, // 
        Column16, // 
        Column17, // 
        Column18, // 
        Column19, // 
    }
    public enum e_Door
    {
        Undefined,
        Door1,
        Door2,
        Door3,
        Door4,
    }
    public enum e_Door_Lock
    {
        Lock,
        Unlock,
    }
    public enum e_Door_Trigger
    {
        Clear,
        Set,
    }

    public enum e_DB_Order
    {
        Column00, // DateTime
        Column01, // ID
        Column02, // Status
        Column03, // Status_Sub
        Column04, // Source
        Column05, // Order_No
        Column06, // Barcode
        Column07, // Product_Code
        Column08, // Product_Name
        Column09, // Payment
        Column10, // Price
        Column11, // Door
        Column12, // Order_Cup
        Column13, // 
        Column14, // 
        Column15, // 
        Column16, // 
        Column17, // 
        Column18, // 
        Column19, // 
    }
    public enum e_Order_Status
    {
        주문,
        주문취소,
        시작,
        텀블러,
        픽업,
        스팀피처,
        소스,
        얼음,
        커피,
        도어,
        배출,
        배출완료,
        강제배출,
        수동배출,
        오류,
    }
    public enum e_Order_Source
    {
        APP,
        KIOSK,
        REMOTE,
        TEST,
    }
    public enum e_Order_Cup
    {
        TAKEOUT,
        TUMBLER,
    }

    #endregion

    #region Font

    public enum e_Font
    {
        Tahoma,
    }

    #endregion

    #region Message

    // e_Message
    public enum e_Message
    {
        ControlPanel_Button_Click_Initialize,
        ControlPanel_Button_Click_Clear,
        ControlPanel_Button_Click_CheckDevice_CoffeeMaker_Instance,
        ControlPanel_Button_Click_CheckDevice_CoffeeMaker_StatusBase,
        ControlPanel_Button_Click_CheckDevice_Controller1_Instance,
        ControlPanel_Button_Click_CheckDevice_Controller1_StatusBase,
        ControlPanel_Button_Click_CheckDevice_Controller2_Instance,
        ControlPanel_Button_Click_CheckDevice_Controller2_StatusBase,
        ControlPanel_Button_Click_CheckDevice_IceMaker_Instance,
        ControlPanel_Button_Click_CheckDevice_IceMaker_StatusBase,
        ControlPanel_Button_Click_CheckDevice_Robot_Instance,
        ControlPanel_Button_Click_CheckDevice_Robot_StatusBase,
        ControlPanel_Button_Click_Interlock_Door_Close,
        ControlPanel_Button_Click_Interlock_Door_Open,
        ControlPanel_Button_Click_Interlock_Door_Tumbler_Home,
        ControlPanel_Button_Click_Interlock_Door_Tumbler_Ready,
        ControlPanel_Button_Click_Interlock_Door_Tumbler_Check,
        ControlPanel_Button_Click_Interlock_Motor_Turn,
        ControlPanel_CheckProcess,
        ControlPanel_UserClosing,
        Login_PW,
        Parameter_Door,
    }

    #endregion

    #region Parameter

    // e_Parameter
    public enum e_Parameter
    {
        Product,
        Booth,
        DID_Bottom,
        Door,
        // =========================================== Device
        CoffeeMaker,
        Controller1,
        Controller2,
        IceMaker,
        Robot,
        Barcode,
        DID,
        Kiosk,
        Remote,
        // ==================================================
        Alarm,
        Control,
        DB,
    }

    #region // frm_Parameter::GetParameterDescription, AddDataGridViewColumns, AddDataGridViewRows

    public enum e_Parameter_Product
    {
        No,
        Status,
        Product_Code,
        Product_Name,
        Little_Delay,
        Coffee_ID,
        Hot_Water_Timeout,
        Start_Delay,
        End_Delay,
        Cup,
        Milk,
        Ice_Time,
        Water_Time,
        Sauce_Type,
        Sauce_No,
        Pumping_Count,
        Mixing_Count,
        Product_Time,
        Remark,
    }
    public enum e_Parameter_Booth
    {
        No,
        Booth_No,
        Language,
        Log_Enabled,
        Simulation,
        Title,
    }
    public enum e_Parameter_DID_Bottom
    {
        No,
        Enabled, // Enabled
        Form_Location_X,
        Form_Location_Y,
        Form_Size_Width,
        Form_Size_Height,
        Font,
        Door_Size_Width,
        Door_Size_Height,
        Door1_Location_X,
        Door1_Location_Y,
        Door2_Location_X,
        Door2_Location_Y,
        Door3_Location_X,
        Door3_Location_Y,
        Door4_Location_X,
        Door4_Location_Y,
    }
    public enum e_Parameter_Door
    {
        No,
        Enabled, // Enabled
        DB_Initialize,
        Pickup_Delay_Sensor_OK,
        Pickup_Delay_Sensor_NG,
    }

    public enum e_Parameter_CoffeeMaker
    {
        No,
        Enabled, // Enabled
        LogEnabled,
        Log_File,
        Device,
        PortName,
        Rinse_Interval_min,
        Ignore_Warnings,
    }
    public enum e_Parameter_Controller1
    {
        No,
        Enabled, // Enabled
        LogEnabled,
        Log_File,
        LogEnabled_Process,
        Device,
        PortName,
        BaudRate,
        Parity,
        DataBits,
        StopBits,
        Timeout,
    }
    public enum e_Parameter_Controller2
    {
        No,
        Enabled, // Enabled
        LogEnabled,
        Log_File,
        LogEnabled_Process,
        Device,
        PortName,
        BaudRate,
        Parity,
        DataBits,
        StopBits,
        Timeout,
    }
    public enum e_Parameter_IceMaker
    {
        No,
        Enabled, // Enabled
        LogEnabled,
        Log_File,
        LogEnabled_Process,
        Device,
        PortName,
        BaudRate,
        Parity,
        DataBits,
        StopBits,
        Timeout,
    }
    public enum e_Parameter_Robot
    {
        No,
        Enabled, // Enabled
        LogEnabled,
        Log_File,
        LogEnabled_Process,
        Device,
        IP,
        Port,
        ConnectTimeout,
        ReceiveTimeout,
    }
    public enum e_Parameter_Barcode
    {
        No,
        Enabled, // Enabled
        LogEnabled,
        Log_File,
        Device,
        PortName,
        BaudRate,
        Parity,
        DataBits,
        StopBits,
    }
    public enum e_Parameter_DID
    {
        No,
        Enabled, // Enabled
        LogEnabled,
        Log_File,
        Thema,
        Message_1,
        Message_2,
        Process_Path,
        Process_Start_Delay,
        IP,
        Port,
        ConnectTimeout,
    }
    public enum e_Parameter_Kiosk
    {
        No,
        Enabled, // Enabled
        LogEnabled,
        Log_File,
        Log_View,
        Device,
        IP,
        Port,
    }
    public enum e_Parameter_Remote
    {
        No,
        Enabled, // Enabled
        LogEnabled,
        Log_File,
        Log_View,
        Device,
        IP,
        Port,
    }

    public enum e_Parameter_Alarm
    {
        No,
        Enabled, // Enabled
        Alarm,
        Code,
        en_Text,
        en_Action,
        ko_Text,
        ko_Action,
    }
    public enum e_Parameter_Control
    {
        No,
        Control_Name,
        en_Font,
        en_Text,
        ko_Font,
        ko_Text,
        Visible,
    }
    public enum e_Parameter_DB
    {
        No,
        Type,
        Server,
        Port,
        Database,
        Uid,
        Pwd,
        Connection_Timeout_sec,
    }

    #endregion

    #endregion

    #endregion

    public static class GlobalVariable
    {
        public static Dictionary<string, string[]>[] Parameter = new Dictionary<string, string[]>[Enum.GetNames(typeof(e_Parameter)).Length];

        public static bool[] Alarm = new bool[Enum.GetNames(typeof(e_Alarm)).Length];

        public static DateTime ProgramStarted = DateTime.Now;

        public static class Directory
        {
            public static string Application = System.Windows.Forms.Application.StartupPath;

            public static string Backup = $@"{Application}\Backup";
            public static string Log = $@"{Application}\Log";
            public static string Parameter = $@"{Application}\Parameter";
        }

        public static class Form
        {
            public static frm_Alarm Alarm = null;
            public static frm_ControlPanel ControlPanel = null;
            public static frm_DID_Bottom DID_Bottom = null;
            public static frm_Monitoring Monitoring = null;
            public static frm_Parameter Parameter = null;
        }
    }

    #region CONST

    public static class CONST
    {
        public const int N_EVERSYS_PRODUCT_ID_HOT_WATER = 13;
        public const int N_EVERSYS_PRODUCT_ID_HOT_MILK = 22;
        public const int N_EVERSYS_PRODUCT_ID_ESPRESSO = 23;

        public const string S_CLEAN = "CLEAN";
        public const string S_RINSE = "RINSE";

        public const string S_CRLF = "\r\n";

        public const string S_CSV = "csv";

        public const string S_KEY = "1";

        public const string S_NG = "NG";
        public const string S_OK = "OK";

        public const string S_PW = "CONST";

        public const string S_RUN = "RUN";
    }

    #endregion
}
