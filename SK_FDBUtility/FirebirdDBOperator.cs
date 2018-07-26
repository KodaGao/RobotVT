using FirebirdSql.Data.FirebirdClient;
using System;
using System.Data;

namespace SK_FDBUtility
{
    public class FirebirdDBOperator
    {
        FbConnection DBConnection = new FbConnection();

        private string sDatabase = String.Empty;
        public string Database
        {
            get { return sDatabase; }
            set { sDatabase = value; }
        }
        private string sUserID = String.Empty;
        public string UserID
        {
            get { return sUserID; }
            set { sUserID = value; }
        }
        private string sPassword = String.Empty;
        public string Password
        {
            get { return sPassword; }
            set { sPassword = value; }
        }
        private string sCharset = String.Empty;
        public string Charset
        {
            get { return sCharset; }
            set { sCharset = value; }
        }

        private string GetConnstr()
        {
            FbConnectionStringBuilder cs = new FbConnectionStringBuilder();
            cs.Database = sDatabase;// "Rabase.fdb";
            cs.UserID = sUserID;// "sysdba";
            cs.Password = sPassword;// "masterkey";
            cs.Charset = sCharset;// "NONE";
            cs.ServerType = FbServerType.Embedded;

            return cs.ToString();
        }
        #region 开关，销毁连接,初始化成员

        public void Open()//打开连接
        {
            try
            {
                if (DBConnection != null && DBConnection.State != ConnectionState.Closed)
                {
                    DBConnection.Close();
                    DBConnection.Dispose();
                }
                DBConnection = new FbConnection(GetConnstr());
                DBConnection.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("打开数据库失败，错误信息：" + ex.Message);
            }
        }

        public void Close()// 关闭连接
        {
            if (DBConnection != null)
            {
                DBConnection.Close();
                InitMember();
            }
        }

        void InitMember()// 初始化成员
        {
            //listParameters = null;
        }

        public void Dispose()//销毁连接
        {
            if (DBConnection != null)
            {
                DBConnection.Close();
                DBConnection.Dispose();
            }
            GC.Collect();
        }

        #endregion 开关，销毁连接,初始化成员

        #region 查询返回datatable
        public DataTable ReturnDataTable(string SQL)
        {
            using (FbCommand selectData = DBConnection.CreateCommand())
            {
                DataTable dt = new DataTable();
                selectData.CommandText = SQL;
                FbDataAdapter dat = new FbDataAdapter(selectData);
                dat.Fill(dt);
                return dt;
            }
        }

        #endregion 查询返回datatable

        #region 判断表是否存在   返回 True/False   string SQL
        /// <summary>
        /// 判断数据库中表是否存在
        /// </summary>
        /// <param name="strTableName">表名称</param>
        /// <returns>返回值</returns>
        public bool ExistTable(string strTableName)
        {
            bool ret = false;
            using (FbCommand insertData = DBConnection.CreateCommand())
            {
                DataTable dt = new DataTable();
                //Select RDB$RELATION_NAME From RDB$RELATIONS WHERE (RDB$RELATION_NAME not like '%$%') AND RDB$VIEW_SOURCE IS NULL
                try
                {
                    insertData.CommandText = "Select RDB$RELATION_NAME From RDB$RELATIONS "
                                           + " WHERE (RDB$RELATION_NAME = '" + strTableName + "') AND RDB$VIEW_SOURCE IS NULL";
                    FbDataAdapter dat = new FbDataAdapter(insertData);
                    dat.Fill(dt);
                    if (dt.Rows.Count > 0) ret = true;
                }
                catch (FbException ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return ret;
        }
        /// <summary>
        /// 返回所有用户表
        /// </summary>
        /// <param name="strTableName"></param>
        /// <returns>返回datatable</returns>
        public DataTable AllUserTable()
        {
            DataTable dt = new DataTable();
            using (FbCommand insertData = DBConnection.CreateCommand())
            {
                //Select RDB$RELATION_NAME From RDB$RELATIONS WHERE (RDB$RELATION_NAME not like '%$%') AND RDB$VIEW_SOURCE IS NULL
                try
                {
                    insertData.CommandText = "Select RDB$RELATION_NAME From RDB$RELATIONS WHERE (RDB$RELATION_NAME not like '%$%') AND RDB$VIEW_SOURCE IS NULL";
                    FbDataAdapter dat = new FbDataAdapter(insertData);
                    dat.Fill(dt);
                }
                catch (FbException ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return dt;
        }

        #endregion 判断表是否存在   返回 True/False   string SQL
        #region 执行SQL操作      返回 -1为失败 >0为成功   string SQL
        /// <summary>
        /// 运行SQL语句
        /// </summary>
        /// <param name="SQL"></param>
        public int ExeCmd(string SQL)
        {
            int ret = -1;
            using (FbCommand insertData = DBConnection.CreateCommand())
            {
                insertData.CommandText = SQL;
                try
                {
                    ret = insertData.ExecuteNonQuery();
                }
                catch (FbException ex)
                {
                    throw new Exception(SQL + ex.ToString());
                }
            }
            return ret;
        }
        #endregion 执行SQL操作      返回 -1为失败 >0为成功   string SQL

        #region 返回DataReader数据  string SQL
        /// <summary>
        /// 返回SQL语句执行结果的第一行第一列
        /// </summary>
        /// <returns>字符串</returns>
        public string ReturnValue(string SQL)
        {
            string result = "";
            using (FbCommand insertData = DBConnection.CreateCommand())
            {
                insertData.CommandText = SQL;
                object obj = insertData.ExecuteScalar();
                if (obj == null || obj.Equals(null) || obj.Equals(System.DBNull.Value))
                {
                    result = "";
                }
                else
                {
                    result = obj.ToString();
                }
            }
            return result;
        }
        #endregion 返回DataReader数据  string SQL
    }
}
