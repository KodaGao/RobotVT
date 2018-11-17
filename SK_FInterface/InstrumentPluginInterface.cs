using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK_FInterface
{
    public interface InstrumentPluginInterface
    {
        #region 事件

        //event SystemDelegate.del_SubmitUploadData Event_SubmitUploadData;

        ///// <summary>
        ///// 运行消息事件
        ///// </summary>
        //event SystemDelegate.del_RuningMessage Event_RuningMessage;

        ///// <summary>
        ///// Debug消息事件
        ///// </summary>
        //event SystemDelegate.del_DebugMessage Event_DebugMessage;

        ///// <summary>
        ///// 获取仪器信息列表
        ///// </summary>
        ///// <param name="InstrumentType"></param>
        ///// <returns></returns>
        //event SystemDelegate.del_GetM_D_PRODUCTSList Event_GetM_D_PRODUCTSList;

        ///// <summary>
        /////获取仪器信息
        ///// </summary>
        //event SystemDelegate.del_GetM_D_PRODUCTS Event_GetM_D_PRODUCTS;

        ///// <summary>
        ///// 更新插件实时信息
        ///// </summary>
        //event SystemDelegate.del_UpdateInstrumentPluginRealTimeInfo Event_UpdateInstrumentPluginRealTimeInfo;

        ///// <summary>
        ///// 获取插件所需要的其它插件的实时信息
        ///// </summary>
        //event SystemDelegate.del_GetInstrumentPluginRealTimeInfo Event_GetInstrumentPluginRealTimeInfo;

        ///// <summary>
        ///// 启动关联的插件
        ///// </summary>
        //event SystemDelegate.del_StartLinkInstrumentPlugin Event_StartLinkInstrumentPlugin;

        ///// <summary>
        ///// 停止关联的插件
        ///// </summary>
        //event SystemDelegate.del_StopLinkInstrumentPlugin Event_StopLinkInstrumentPlugin;

        ///// <summary>
        ///// 完成查询传感器基本信息事件
        ///// </summary>
        //event SystemDelegate.del_SearchSensorBaseInfoFinish Event_SearchSensorBaseInfoFinish;

        ///// <summary>
        /////完成向关联的插件提交指令事件
        ///// </summary>
        //event SystemDelegate.del_CommitOrderToLinkPluginFinish Event_CommitOrderToLinkPluginFinish;

        ///// <summary>
        /////完成向关联的插件提交指令
        ///// </summary>
        //event SystemDelegate.del_CommitOrderToLinkPlugin Event_CommitOrderToLinkPlugin;

        #endregion 事件

        #region 方法

        ///// <summary>
        ///// 初始化
        ///// </summary>
        ///// <param name="SerialPortInfo"></param>
        //void Init(Model.DB.M_D_PRODUCTS M_D_PRODUCTS);

        void InitSerialPortInfo(SK_FModel.SerialPortInfo SerialPortInfo);

        //int GetStationQuantity();


        ///// <summary>
        ///// 生成检定报告
        ///// </summary>
        ///// <param name="CheckedInstrument"></param>
        //void CreateReport(Model.DB.M_D_CheckedInstrument CheckedInstrument, string ReportDateTime);

        //void PrintQRCodeString(Model.SystemEnum.QRCodeType QRCodeType, List<Model.DB.M_D_CheckedInstrument> CheckedInstrumentList);

        ///// <summary>
        ///// 向插件设置配置信息
        ///// </summary>
        ///// <param name="Config"></param>
        //void SetConfigInfo(Model.Config Config);

        //string SearchSensorBaseInfo(int AddressNo);

        //Model.SystemEnum.RuningStatus GetRuningStatus();

        //string CreateFactoryNumber(Model.DB.S_F_ParaInfo GasType);

        //void UpdateFactoryNumber(string Number);

        ///// <summary>
        ///// 获取插件信息
        ///// </summary>
        ///// <returns></returns>
        //Model.InstrumentPluginInfo GetInstrumentPluginInfo();

        //List<Model.DB.S_F_ParaInfo> GetGasType();

        /// <summary>
        /// 启动插件
        /// </summary>
        bool Start();

        /// <summary>
        /// 停止插件
        /// </summary>
        bool Stop();

        ///// <summary>
        ///// 获取校验控制模块
        ///// </summary>
        ///// <returns></returns>
        //void GetCheckModule(ref UserControl CheckControl, ref DockStyle DocStyle);

        ///// <summary>
        ///// 获取参数设置模块
        ///// </summary>
        ///// <param name="CheckControl"></param>
        ///// <param name="DocStyle"></param>
        //void GetParaSetup(ref UserControl ParaSetup, ref DockStyle DocStyle);

        /// <summary>
        /// 设置接收数据信息
        /// </summary>
        /// <param name="ReceiveDataInfoList"></param>
        void SetReceiveDataInfo(List<SK_FModel.ReceiveDataInfo> ReceiveDataInfoList);


        ///// <summary>
        ///// 获取仪器参数信息
        ///// </summary>
        ///// <returns></returns>
        //List<Model.InstrumentParaInfo> GetInstrumentParaInfo();

        /// <summary>
        /// 参数信息
        /// </summary>
        /// <param name="paras"></param>
        void Parameters(string[] paras);

        ///// <summary>
        ///// 绑定传感器信息
        ///// </summary>
        ///// <param name="CheckedInstrument"></param>
        //bool BindSensorInfo(Model.DB.M_D_CheckedInstrument CheckedInstrument, out string ErrorInfo);

        ///// <summary>
        ///// 向关联的插件提交指令
        ///// </summary>
        ///// <param name="value"></param>
        //object CommitOrderToLinkPlugin(Model.DB.M_D_PRODUCTS sender, Model.SystemEnum.InstrumentPluginOrder Order, object value);

        ///// <summary>
        ///// 完成向关联的插件提交指令
        ///// </summary>
        ///// <param name="value"></param>
        //void CommitOrderToLinkPluginFinish(Model.DB.M_D_PRODUCTS from, Model.SystemEnum.InstrumentPluginOrder Order, object value);

        //void ShowGasCylindersStatus();

        #endregion 方法
    }
}
