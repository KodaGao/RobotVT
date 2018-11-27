using RobotVT.Controller.SerialPortMethods;
using SK_FModbus;
using SK_FModel;
using System;
using System.Collections.Generic;

namespace RobotVT.Controller
{
    public class RobotInfoModule : SK_FInterface.InstrumentPluginInterface
    {
        /// <summary>
        /// RobotInfo
        /// </summary>
        internal RobotInfo RobotInfo;

        /// <summary>
        /// 运行消息事件
        /// </summary>
        public event SystemDelegate.del_RuningMessage Event_RuningMessage;

        /// <summary>
        /// Debug消息事件
        /// </summary>
        public event SystemDelegate.del_DebugMessage Event_DebugMessage;

        /// <summary>
        /// 更新实时数据事件
        /// </summary>
        internal event SerialPortDelegate.UpdateRealTimeDataEventHandler Event_UpdateRealTimeData;

        private TargetFollowRecvInfo TargetRecvInfo;

        public void Parameters(string[] paras)
        {
        }

        public void Init()
        {
            if (StaticInfo.ModbusHelper == null)
            {
                StaticInfo.ModbusHelper = new SerialPortMethods.ModbusHelper();
                StaticInfo.ModbusHelper.Event_RuningMessage += ModbusHelper_Event_RuningMessage;
                StaticInfo.ModbusHelper.Event_DebugMessage += ModbusHelper_Event_DebugMessage;
                StaticInfo.ModbusHelper.Event_UpdateRealTimeData += ModbusHelper_Event_UpdateRealTimeData;
                StaticInfo.ModbusHelper.Event_ReceiveData += ModbusHelper_Event_ReceiveData;
                StaticInfo.ModbusHelper.Event_SendData += ModbusHelper_Event_SendData;
            }

            StaticInfo.TargetFollow.Event_TargetCoordinates += TargetFollow_Event_TargetCoordinates;
        }

        private void TargetFollow_Event_TargetCoordinates(TargetFollowRecvInfo targetInfo)
        {
            TargetRecvInfo = targetInfo;
        }

        private void ModbusHelper_Event_DebugMessage(object sender, object Message)
        {
            Methods.SaveModbusLog(SK_FModel.SystemEnum.LogType.Normal, Message.ToString());
            Event_DebugMessage?.Invoke(sender, Message);
        }
        private void ModbusHelper_Event_RuningMessage(object sender, string MessageInfo, SK_FModel.SystemEnum.MessageType Type, params object[] paras)
        {
            Event_RuningMessage?.Invoke(sender, MessageInfo, Type);
        }
        private void ModbusHelper_Event_UpdateRealTimeData(object sender)
        {
            ReceiveData receiveData = (ReceiveData)sender;
            if (RobotInfo == null)
            {
                RobotInfo = new RobotInfo();
            }

            List<short> receData = new List<short>(255);
            for (int i = 0; i < receiveData.DataItem.Length / 2; i++)
            {
                short v = receiveData.DataItem[2 * i];
                v <<= 8;
                v += receiveData.DataItem[2 * i + 1];

                receData.Add(v);
            }
            short[] ValidItem = receData.ToArray();

            RobotInfo.BatteryPowerOfRobot = (ushort)ValidItem[0];
            RobotInfo.BatteryPowerOfRemoteContorller = (ushort)ValidItem[1];
            RobotInfo.LaserDistance = (ushort)ValidItem[2];
            RobotInfo.RobotSpeed = ValidItem[3];
            RobotInfo.RobotStatus = ValidItem[4];
            RobotInfo.RobotTracking = (short)(TargetRecvInfo == null ? 0 : TargetRecvInfo.TargetStatus);
            RobotInfo.RobotTrackingX = (short)(TargetRecvInfo == null ? 0 : TargetRecvInfo.AzimuthCoordinate);
            RobotInfo.RobotTrackingY = (short)(TargetRecvInfo == null ? 0 : TargetRecvInfo.PitchCoordinate);

            Event_UpdateRealTimeData?.Invoke(RobotInfo);
        }
        private void ModbusHelper_Event_ReceiveData(ReceiveData receiveData)
        {
            
        }
        private void ModbusHelper_Event_SendData(ReceiveData receiveData)
        {
            List<byte> value = new List<byte>(receiveData.Quantity * 2);
            byte[] _TempByte;
            _TempByte = BitConverter.GetBytes((short)(TargetRecvInfo == null ? 0 : TargetRecvInfo.TargetStatus));
            Array.Reverse(_TempByte);
            value[0] = _TempByte[0];
            value[1] = _TempByte[1];
            _TempByte = BitConverter.GetBytes((short)(TargetRecvInfo == null ? 0 : TargetRecvInfo.AzimuthCoordinate));
            Array.Reverse(_TempByte);
            value[2] = _TempByte[0];
            value[3] = _TempByte[1];
            _TempByte = BitConverter.GetBytes((short)(TargetRecvInfo == null ? 0 : TargetRecvInfo.PitchCoordinate));
            Array.Reverse(_TempByte);
            value[4] = _TempByte[0];
            value[5] = _TempByte[1];

            ReturnOrder returnOrder = StaticInfo.ModbusHelper.CreateReturnOrder(receiveData.DeviceAddressId, receiveData.FunctionCode, receiveData.RegisterAddress, receiveData.Quantity, value);
            StaticInfo.ModbusHelper.SubmitOrder(returnOrder);
        }

        /// <summary>
        /// 开始
        /// </summary>
        public bool Start()
        {
            try
            {
                StaticInfo.ModbusHelper.Start();
                Event_RuningMessage?.Invoke(StaticInfo.ControlObject, "仪器已连接！", SK_FModel.SystemEnum.MessageType.Normal);
                return true;
            }
            catch (Exception ex)
            {
                Event_RuningMessage?.Invoke(StaticInfo.ControlObject, "启动失败，错误信息：" + ex.Message, SK_FModel.SystemEnum.MessageType.Exception);
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
                    if (StaticInfo.ModbusHelper != null)
                    {
                        StaticInfo.ModbusHelper.ClearSendOrder();
                        StaticInfo.ModbusHelper.Stop();
                        StaticInfo.ModbusHelper.DiscardOutBuffer();
                        StaticInfo.ModbusHelper.DiscardInBuffer();
                    }
                }
                catch { }
                Event_RuningMessage?.Invoke(StaticInfo.ControlObject, "仪器已断开！", SK_FModel.SystemEnum.MessageType.Normal);
                //StaticInfo.ParaInfo.RuningStatus = SystemEnum.RuningStatus.Stop;
                return true;
            }
            catch (Exception ex)
            {
                Event_RuningMessage?.Invoke(StaticInfo.ControlObject, "停止失败，错误信息：" + ex.Message, SK_FModel.SystemEnum.MessageType.Exception);
                return false;
            }
        }

        public void InitSerialPortInfo(SK_FModel.SerialPortInfo SerialPortInfo)
        {
            if (SerialPortInfo == null)
            {
                SerialPortInfo = new SK_FModel.SerialPortInfo();
            }
            SerialPortInfo.SenderOrderInterval = StaticInfo.SenderOrderInterval;
            StaticInfo.ModbusHelper.Init(SerialPortInfo);
        }

        public void SetReceiveDataInfo(List<SK_FModel.ReceiveDataInfo> ReceiveDataInfoList)
        {
            //receiveDataInfoList = ReceiveDataInfoList;
        }

    }

    internal class RobotInfo
    {
        /// <summary>
        /// 40001.0	工作状态 由主控制器写入, 等于1为机器人本体工作正常   等于1时在主画面显示"工作状态"为0时取消显示
        /// 40001.1	遥控模式 由主控制器写入, 等于1为遥控器来控制机器人本体的运动  等于1时在主画面显示"遥控模式"为0时取消显示
        /// 40001.2	巡逻模式 由主控制器写入, 等于1 为机器人本体正在进行自动巡逻运动 等于1时在主画面显示"巡逻模式"为0时取消显示
        /// 40001.3	执行充电 由主控制器写入, 等于1为机器人本体正在充电   等于1时在主画面显示"执行充电"为0时取消显示
        /// 40001.4			
        /// 40001.5	示教模式 由主控制器写入, 等于1为正在进行示教模式    等于1时在主画面显示"示教模式"为0时取消显示
        /// 40001.6	允许发射 由主控制器写入, 等于1为允许发射    等于1时跟踪画面显示"允许发射"为0时取消显示
        /// 40001.7	预留 根据实际需要双方商定
        /// 40001.8	预留 根据实际需要双方商定
        /// 40001.9	预留 根据实际需要双方商定
        /// 40001.10	预留 根据实际需要双方商定
        /// 40001.11	预留 根据实际需要双方商定
        /// 40001.12	预留 根据实际需要双方商定
        /// 40001.13	预留 根据实际需要双方商定
        /// 40001.14	预留 根据实际需要双方商定
        /// 40001.15	故障复位 由主控制器写入, 等于1为清除系统当前所有可复位的故障
        /// </summary>
        public Int16 RobotStatus;

        /// <summary>
        /// 40002	车辆运动速度 由主控制器写入, 范围为-1000~1000,	
        /// 1000表示运动速度为10米/秒,不等于零时在主画面显示
        /// </summary>
        /// 
        public Int16 RobotSpeed;


        /// <summary>
        /// 40003	遥控器电池电量 由主控制器写入, 范围为0~1000	
        /// 1000表示100%的电量,在主画面显示
        /// </summary>
        /// 
        public UInt16 BatteryPowerOfRemoteContorller;


        /// <summary>
        /// 40004	车体电池电量 由主控制器写入, 范围为0~1000	
        /// 1000表示100%的电量, 在主画面显示
        /// </summary>
        /// 
        public UInt16 BatteryPowerOfRobot;


        /// <summary>
        /// 40005	激光测距仪信号 由主控制器写入, 范围为0~2000	
        /// 2000表示云台前方物体距离为20.00米,在跟踪画面显示
        /// </summary>
        /// 
        public UInt16 LaserDistance;

        /// <summary>
        ///40010.1	已选中的跟踪目标丢失 由视觉跟踪系统写入, 等于1为已选中的跟踪目标丢失    
        ///         操作者在画面上选中跟踪目标后,如不是正常取消导致目标丢失,置1,待重新操作选中目标后,置0
        ///40010.2	预留 根据实际需要双方商定
        ///40010.3	预留 根据实际需要双方商定
        ///40010.4	预留 根据实际需要双方商定
        ///40010.5	预留 根据实际需要双方商定
        ///40010.6	预留 根据实际需要双方商定
        ///40010.7	预留 根据实际需要双方商定
        ///40010.8	预留 根据实际需要双方商定
        ///40010.9	预留 根据实际需要双方商定
        ///40010.10	预留 根据实际需要双方商定
        ///40010.11	预留 根据实际需要双方商定
        ///40010.12	预留 根据实际需要双方商定
        ///40010.13	预留 根据实际需要双方商定
        ///40010.14	预留 根据实际需要双方商定
        ///40010.15	预留 根据实际需要双方商定
        /// </summary>
        /// 
        public Int16 RobotTracking;

        /// <summary>
        /// 40011	选中的跟踪目标相对于屏幕的X坐标 由视觉跟踪系统写入, 
        /// 选中的跟踪目标相对于屏幕的X坐标值, 范围-9999~9999(或根据实际的分辨率来定) 如果目标位于屏幕正中间, 坐标值为0            
        /// </summary>
        /// 
        public Int16 RobotTrackingX;


        /// <summary>
        /// 40012	选中的跟踪目标相对于屏幕的Y坐标 由视觉跟踪系统写入, 
        /// 选中的跟踪目标相对于屏幕的Y坐标值, 范围-9999~9999(或根据实际的分辨率来定) 如果目标位于屏幕正中间, 坐标值为0        
        /// </summary>
        /// 
        public Int16 RobotTrackingY;
    }


}