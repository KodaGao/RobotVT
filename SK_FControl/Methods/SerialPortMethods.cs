using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK_FControl
{
    public class SerialPortMethods
    {
        public SK_FDBUtility.DB.S_D_SERIALPORT GetSerialPortSet(string spkey)
        {
            try
            {
                string _SqlScript = string.Format("SELECT SP_KEY, SP_PORT, SP_BAUDRATE, SP_DATABIT, SP_PRATITY,SP_STOPBIT, SP_TIMEOUT, SP_SAMPLE, SP_ACTIVE FROM S_D_SERIALPORT WHERE SP_KEY ='{0}';", spkey);
                System.Data.DataTable _DTTemp = StaticInfo.FirebirdDBOperator.ReturnDataTable(_SqlScript);

                if (_DTTemp.Rows.Count <= 0) return null;
                SK_FDBUtility.DB.S_D_SERIALPORT _S_D_SPort = new SK_FDBUtility.DB.S_D_SERIALPORT();
                foreach (System.Data.DataRow _DR in _DTTemp.Rows)
                {
                    _S_D_SPort.SP_KEY = _DR["SP_KEY"].ToString();
                    _S_D_SPort.SP_PORT = _DR["SP_PORT"].ToString();
                    _S_D_SPort.SP_BAUDRATE = _DR["SP_BAUDRATE"].ToString();
                    _S_D_SPort.SP_DATABIT = _DR["SP_DATABIT"].ToString();
                    _S_D_SPort.SP_PRATITY = _DR["SP_PRATITY"].ToString();
                    _S_D_SPort.SP_STOPBIT = _DR["SP_STOPBIT"].ToString();
                    _S_D_SPort.SP_TIMEOUT = _DR["SP_TIMEOUT"].ToString();
                    _S_D_SPort.SP_SAMPLE = _DR["SP_SAMPLE"].ToString();
                }
                return _S_D_SPort;
            }
            catch (Exception ex)
            {
                throw new Exception("查询" + spkey + "串口参数信息失败，错误信息：" + ex.Message);
            }
        }

        public void InsertSerialPortSet(SK_FDBUtility.DB.S_D_SERIALPORT s_D_)
        {
            try
            {
                string _SqlScript = string.Format(" INSERT INTO S_D_SERIALPORT "
                                     + " (SP_KEY, SP_PORT, SP_BAUDRATE, SP_DATABIT, SP_PRATITY, SP_STOPBIT, SP_TIMEOUT, SP_SAMPLE, SP_ACTIVE) "
                            + "   VALUES ('{0}','{1}','{2}', '{3}','{4}','{5}', '{6}' ,'{7}','{8}' ); "
                            , s_D_.SP_KEY, s_D_.SP_PORT, s_D_.SP_BAUDRATE, s_D_.SP_DATABIT, s_D_.SP_PRATITY, s_D_.SP_STOPBIT, s_D_.SP_TIMEOUT, s_D_.SP_SAMPLE, "");
                StaticInfo.FirebirdDBOperator.ExeCmd(_SqlScript);
            }
            catch (Exception ex)
            {
                throw new Exception("新增" + s_D_.SP_KEY + "串口参数信息失败，错误信息：" + ex.Message);
            }
        }

        public void UpdateSerialPortSet(SK_FDBUtility.DB.S_D_SERIALPORT s_D_)
        {
            try
            {
                string _SqlScript = string.Format("UPDATE S_D_SERIALPORT SET SP_PORT='{0}', SP_BAUDRATE='{1}', SP_DATABIT='{2}', SP_PRATITY='{3}', SP_STOPBIT='{4}', SP_TIMEOUT='{5}', SP_SAMPLE='{6}'  WHERE SP_KEY ='{7}';"
                                                                          , s_D_.SP_PORT, s_D_.SP_BAUDRATE, s_D_.SP_DATABIT, s_D_.SP_PRATITY, s_D_.SP_STOPBIT, s_D_.SP_TIMEOUT, s_D_.SP_SAMPLE, s_D_.SP_KEY);
                StaticInfo.FirebirdDBOperator.ExeCmd(_SqlScript);
            }
            catch (Exception ex)
            {
                throw new Exception("更新" + s_D_.SP_KEY + "串口参数信息失败，错误信息：" + ex.Message);
            }
        }

        public void DelSerialPortSet(string spkey)
        {
            try
            {
                string _SqlScript = string.Format(" DELETE FROM S_D_SERIALPORT WHERE SP_KEY = '{0}';" , spkey);
                StaticInfo.FirebirdDBOperator.ExeCmd(_SqlScript);
            }
            catch (Exception ex)
            {
                throw new Exception("删除" + spkey + "串口参数信息失败，错误信息：" + ex.Message);
            }
        }

        //SP_KEY, SP_PORT, SP_BAUDRATE, SP_DATABIT, SP_PRATITY, SP_STOPBIT, SP_TIMEOUT, SP_SAMPLE, SP_ACTIVE
    }
}
