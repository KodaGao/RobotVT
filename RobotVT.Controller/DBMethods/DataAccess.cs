
using System;
using System.Collections.Generic;
using RobotVT.Model;

namespace RobotVT.Controller
{
    public class DataAccess
    {
        public List<Model.S_D_CameraSet> GetS_D_CameraSetList(int PRODUCTIDLen)
        {
            try
            {
                List<Model.S_D_CameraSet> _Result = new List<Model.S_D_CameraSet>();
                if (StaticInfo.FirebirdDBOperator == null) return _Result;

                Model.S_D_CameraSet _S_D_CameraSet;
                string _SqlScript = string.Format("Select VT_ID, VT_NAME, VT_PASSWORD, VT_IP, VT_PORT From S_D_CameraSet;", PRODUCTIDLen);
                System.Data.DataTable _DTTemp;
                _DTTemp = StaticInfo.FirebirdDBOperator.ReturnDataTable(_SqlScript);
                foreach (System.Data.DataRow _DR in _DTTemp.Rows)
                {
                    _S_D_CameraSet = new Model.S_D_CameraSet();
                    _S_D_CameraSet.VT_ID = _DR["VT_ID"].ToString();
                    _S_D_CameraSet.VT_NAME = _DR["VT_NAME"].ToString();
                    _S_D_CameraSet.VT_PASSWORD = _DR["VT_PASSWORD"].ToString();
                    _S_D_CameraSet.VT_IP = _DR["VT_IP"].ToString();
                    _S_D_CameraSet.VT_PORT = _DR["VT_PORT"].ToString();
                    _Result.Add(_S_D_CameraSet);
                }
                return _Result;
            }
            catch (Exception ex)
            {
                throw new Exception("获取摄像头参数信息失败，错误信息：" + ex.Message);
            }
        }

        public void UpdateCameraSet(RobotVT.Model.S_D_CameraSet s_D_)
        {
            try
            {
                string _SqlScript = string.Format("UPDATE S_D_CameraSet SET VT_NAME='{0}', VT_PASSWORD='{1}', VT_IP='{2}', VT_PORT='{3}'  WHERE VT_ID ='{4}';", s_D_.VT_NAME, s_D_.VT_PASSWORD, s_D_.VT_IP, s_D_.VT_PORT, s_D_.VT_ID);
                StaticInfo.FirebirdDBOperator.ExeCmd(_SqlScript);               
            }
            catch (Exception ex)
            {
                throw new Exception("更新摄像头参数信息失败，错误信息：" + ex.Message);
            }
        }


        public RobotVT.Model.S_D_CameraSet GetCameraSet(string vtid)
        {
            try
            {
                string _SqlScript = string.Format("Select VT_ID, VT_NAME, VT_PASSWORD, VT_IP, VT_PORT From S_D_CameraSet WHERE VT_ID ='{0}';", vtid);
                System.Data.DataTable _DTTemp = StaticInfo.FirebirdDBOperator.ReturnDataTable(_SqlScript);


                Model.S_D_CameraSet _S_D_CameraSet = new Model.S_D_CameraSet();
                foreach (System.Data.DataRow _DR in _DTTemp.Rows)
                {
                    _S_D_CameraSet.VT_ID = _DR["VT_ID"].ToString();
                    _S_D_CameraSet.VT_NAME = _DR["VT_NAME"].ToString();
                    _S_D_CameraSet.VT_PASSWORD = _DR["VT_PASSWORD"].ToString();
                    _S_D_CameraSet.VT_IP = _DR["VT_IP"].ToString();
                    _S_D_CameraSet.VT_PORT = _DR["VT_PORT"].ToString();
                }
                return _S_D_CameraSet;
            }
            catch (Exception ex)
            {
                throw new Exception("获取"+ vtid + "摄像头参数信息失败，错误信息：" + ex.Message);
            }
        }

    }
}
