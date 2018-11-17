using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK_FControl
{
    public class SerialPortMethods
    {
        public void CommSet(string skey)
        {
            try
            {
                string sp_key = skey.ToUpper();
                string sp_active = "";

                SerialPortForm se = new SerialPortForm();
                System.Data.DataTable dt = new System.Data.DataTable();
                dt = StaticInfo.FirebirdDBOperator.ReturnDataTable("SELECT a.SP_KEY, a.SP_PORT, a.SP_BAUDRATE, a.SP_DATABIT, a.SP_PRATITY, a.SP_STOPBIT, a.SP_TIMEOUT, a.SP_SAMPLE, a.SP_ACTIVE "
                   + " FROM S_D_SERIALPORT a WHERE a.SP_KEY = '" + sp_key + "'");

                if (dt.Rows.Count <= 0)
                {
                    se.serialport = "COM1";
                    se.baudrate = "9600";
                    se.databit = "8";
                    se.parity = "None";
                    se.stopbit = "One";
                    se.timeout = "1000";
                    se.samplerate = "1000";
                    sp_active = "0";
                    if (sp_key == "METER") sp_active = "1,2,3,4";
                }
                else
                {
                    se.serialport = dt.Rows[0]["SP_PORT"].ToString();
                    se.baudrate = dt.Rows[0]["SP_BAUDRATE"].ToString();
                    se.databit = dt.Rows[0]["SP_DATABIT"].ToString();
                    se.parity = dt.Rows[0]["SP_PRATITY"].ToString();
                    se.stopbit = dt.Rows[0]["SP_STOPBIT"].ToString();
                    se.timeout = dt.Rows[0]["SP_TIMEOUT"].ToString();
                    se.samplerate = dt.Rows[0]["SP_SAMPLE"].ToString();
                    sp_active = dt.Rows[0]["SP_ACTIVE"].ToString();
                }
                se.ShowDialog();

                StaticInfo.FirebirdDBOperator.ExeCmd(" DELETE FROM S_D_SERIALPORT WHERE SP_KEY = '" + sp_key + "'");

                string insertsqlstring = " INSERT INTO S_D_SERIALPORT (SP_KEY, SP_PORT, SP_BAUDRATE, SP_DATABIT, SP_PRATITY, SP_STOPBIT, SP_TIMEOUT, SP_SAMPLE, SP_ACTIVE) "
                            + "   VALUES ('" + sp_key + "','" + se.serialport.ToString() + "','" + se.baudrate.ToString() + "', '" + se.databit.ToString() + "','" + se.parity.ToString() + "',  "
                            + "  '" + se.stopbit.ToString() + "', '" + se.timeout.ToString() + "' ,'" + se.samplerate.ToString() + "','" + sp_active + "' )";
                StaticInfo.FirebirdDBOperator.ExeCmd(insertsqlstring);
            }
            catch (Exception ex)
            {
                throw new Exception("获取摄像头参数信息失败，错误信息：" + ex.Message);
            }
        }


        public List<SK_FDBUtility.DB.S_D_SERIALPORT> GetS_D_CameraSetList(int PRODUCTIDLen)
        {
            //    SELECT r.SP_KEY, r.SP_PORT, r.SP_BAUDRATE, r.SP_DATABIT, r.SP_PRATITY,
            //    r.SP_STOPBIT, r.SP_TIMEOUT, r.SP_SAMPLE, r.SP_ACTIVE
            //     FROM S_D_SERIALPORT
            try
            {
                List<SK_FDBUtility.DB.S_D_SERIALPORT> _Result = new List<SK_FDBUtility.DB.S_D_SERIALPORT>();
                if (StaticInfo.FirebirdDBOperator == null) return _Result;

                SK_FDBUtility.DB.S_D_SERIALPORT _S_D_SerialPort;
                string _SqlScript = string.Format("SELECT SP_KEY, SP_PORT, SP_BAUDRATE, SP_DATABIT, SP_PRATITY,SP_STOPBIT, SP_TIMEOUT, SP_SAMPLE, SP_ACTIVE FROM S_D_SERIALPORT;", PRODUCTIDLen);
                System.Data.DataTable _DTTemp;
                _DTTemp = StaticInfo.FirebirdDBOperator.ReturnDataTable(_SqlScript);
                foreach (System.Data.DataRow _DR in _DTTemp.Rows)
                {
                    _S_D_SerialPort = new SK_FDBUtility.DB.S_D_SERIALPORT();
                    _S_D_SerialPort.SP_KEY = _DR["SP_KEY"].ToString();
                    _S_D_SerialPort.SP_PORT = _DR["SP_PORT"].ToString();
                    _S_D_SerialPort.SP_BAUDRATE = _DR["SP_BAUDRATE"].ToString();
                    _S_D_SerialPort.SP_DATABIT = _DR["SP_DATABIT"].ToString();
                    _S_D_SerialPort.SP_PRATITY = _DR["SP_PRATITY"].ToString();
                    _S_D_SerialPort.SP_STOPBIT = _DR["SP_STOPBIT"].ToString();
                    _S_D_SerialPort.SP_TIMEOUT = _DR["SP_TIMEOUT"].ToString();
                    _S_D_SerialPort.SP_SAMPLE = _DR["SP_SAMPLE"].ToString();
                    _Result.Add(_S_D_SerialPort);
                }
                return _Result;
            }
            catch (Exception ex)
            {
                throw new Exception("获取摄像头参数信息失败，错误信息：" + ex.Message);
            }
        }

        public SK_FDBUtility.DB.S_D_SERIALPORT GetSerialPortSet(string spkey)
        {
            try
            {
                string _SqlScript = string.Format("SELECT SP_KEY, SP_PORT, SP_BAUDRATE, SP_DATABIT, SP_PRATITY,SP_STOPBIT, SP_TIMEOUT, SP_SAMPLE, SP_ACTIVE FROM S_D_SERIALPORT WHERE SP_KEY ='{0}';", spkey);
                System.Data.DataTable _DTTemp = StaticInfo.FirebirdDBOperator.ReturnDataTable(_SqlScript);
                
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
                throw new Exception("获取" + spkey + "串口参数信息失败，错误信息：" + ex.Message);
            }
        }

    }
}
