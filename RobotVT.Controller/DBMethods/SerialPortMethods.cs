using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotVT.Controller
{
    class SerialPortMethods
    {
        public List<SK_FDBUtility.DB.S_D_SERIALPORT> GetS_D_CameraSetList(int PRODUCTIDLen)
        {
            try
            {
                List<SK_FDBUtility.DB.S_D_SERIALPORT> _Result = new List<SK_FDBUtility.DB.S_D_SERIALPORT>();
                if (StaticInfo.FirebirdDBOperator == null) return _Result;

                SK_FDBUtility.DB.S_D_SERIALPORT _S_D_SerialPort;
                string _SqlScript = string.Format("Select VT_ID, VT_NAME, VT_PASSWORD, VT_IP, VT_PORT From S_D_CameraSet;", PRODUCTIDLen);
                System.Data.DataTable _DTTemp;
                _DTTemp = StaticInfo.FirebirdDBOperator.ReturnDataTable(_SqlScript);
                foreach (System.Data.DataRow _DR in _DTTemp.Rows)
                {
                    //_S_D_SerialPort = new Model.S_D_CameraSet();
                    //_S_D_SerialPort.VT_ID = _DR["VT_ID"].ToString();
                    //_S_D_SerialPort.VT_NAME = _DR["VT_NAME"].ToString();
                    //_S_D_SerialPort.VT_PASSWORD = _DR["VT_PASSWORD"].ToString();
                    //_S_D_SerialPort.VT_IP = _DR["VT_IP"].ToString();
                    //_S_D_SerialPort.VT_PORT = _DR["VT_PORT"].ToString();
                    _Result.Add(_S_D_SerialPort);
                }
                return _Result;
            }
            catch (Exception ex)
            {
                throw new Exception("获取摄像头参数信息失败，错误信息：" + ex.Message);
            }
        }

        public void UpdateCameraSet(SK_FDBUtility.DB.S_D_SERIALPORT s_D_)
        {
            //try
            //{
            //    string _SqlScript = string.Format("UPDATE S_D_CameraSet SET VT_NAME='{0}', VT_PASSWORD='{1}', VT_IP='{2}', VT_PORT='{3}'  WHERE VT_ID ='{4}';", s_D_.VT_NAME, s_D_.VT_PASSWORD, s_D_.VT_IP, s_D_.VT_PORT, s_D_.VT_ID);
            //    StaticInfo.FirebirdDBOperator.ExeCmd(_SqlScript);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("更新摄像头参数信息失败，错误信息：" + ex.Message);
            //}
        }


        public SK_FDBUtility.DB.S_D_SERIALPORT GetSerialPortSet(string vtid)
        {
            try
            {
                string _SqlScript = string.Format("Select VT_ID, VT_NAME, VT_PASSWORD, VT_IP, VT_PORT From S_D_CameraSet WHERE VT_ID ='{0}';", vtid);
                System.Data.DataTable _DTTemp = StaticInfo.FirebirdDBOperator.ReturnDataTable(_SqlScript);


                SK_FDBUtility.DB.S_D_SERIALPORT _S_D_SPort = new SK_FDBUtility.DB.S_D_SERIALPORT();
                foreach (System.Data.DataRow _DR in _DTTemp.Rows)
                {
                    //_S_D_SPort.VT_ID = _DR["VT_ID"].ToString();
                    //_S_D_SPort.VT_NAME = _DR["VT_NAME"].ToString();
                    //_S_D_SPort.VT_PASSWORD = _DR["VT_PASSWORD"].ToString();
                    //_S_D_SPort.VT_IP = _DR["VT_IP"].ToString();
                    //_S_D_SPort.VT_PORT = _DR["VT_PORT"].ToString();
                }
                return _S_D_SPort;
            }
            catch (Exception ex)
            {
                throw new Exception("获取" + vtid + "摄像头参数信息失败，错误信息：" + ex.Message);
            }
        }

    }
}
