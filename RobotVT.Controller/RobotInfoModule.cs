using System;
using System.Collections.Generic;

namespace RobotVT.Controller
{
    public class RobotInfoModule : SK_FInterface.InstrumentPluginInterface
    {
        public void Parameters(string[] paras)
        {
        }


        /// <summary>
        /// 开始
        /// </summary>
        public bool Start()
        {
            try
            {
                //StaticInfo.ParaInfo.ModbusHelper.Start();
                //if (Event_RuningMessage != null)
                //    Event_RuningMessage(StaticInfo.ParaInfo.M_D_PRODUCTS, "仪器已连接！", Relations.Model.CommonEnum.MessageType.Normal);
                //if (CheckControl != null)
                //{
                //    CheckControl.Connect();
                //    StaticInfo.ParaInfo.RuningStatus = SystemEnum.RuningStatus.Run;
                //    return true;
                //}
                //else
                    return false;
            }
            catch (Exception ex)
            {
                //Event_RuningMessage?.Invoke(StaticInfo.ParaInfo.M_D_PRODUCTS, "启动失败，错误信息：" + ex.Message, Relations.Model.CommonEnum.MessageType.Exception);
                return false;
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public bool Stop()
        {
            try
            {
                try
                {
                    //if (StaticInfo.ParaInfo.ModbusHelper != null)
                    //{
                    //    StaticInfo.ParaInfo.ModbusHelper.ClearSendOrder();
                    //    StaticInfo.ParaInfo.ModbusHelper.Stop();
                    //}
                }
                catch { }

                //if (CheckControl != null)
                //    CheckControl.Disconnect();
                //if (Event_RuningMessage != null)
                //    Event_RuningMessage(StaticInfo.ParaInfo.M_D_PRODUCTS, "仪器已断开！", Relations.Model.CommonEnum.MessageType.Normal);
                //StaticInfo.ParaInfo.RuningStatus = SystemEnum.RuningStatus.Stop;
                return true;
            }
            catch (Exception ex)
            {
                //Event_RuningMessage?.Invoke(StaticInfo.ParaInfo.M_D_PRODUCTS, "停止失败，错误信息：" + ex.Message, Relations.Model.CommonEnum.MessageType.Exception);
                return false;
            }
        }

        public void InitSerialPortInfo(SK_FModel.SerialPortInfo SerialPortInfo)
        {
            //SerialPortInfo.SenderOrderInterval = StaticInfo.ParaInfo.SenderOrderInterval;
            //StaticInfo.ParaInfo.ModbusHelper.Init(SerialPortInfo);
        }

        public void SetReceiveDataInfo(List<SK_FModel.ReceiveDataInfo> ReceiveDataInfoList)
        {
            //receiveDataInfoList = ReceiveDataInfoList;
        }

    }
}