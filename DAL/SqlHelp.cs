using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;


namespace DAL
{
    public  class SqlHelp : IDisposable
    {
        #region 成员定义
        //定义SqlServer链接字符串
        public string SqlConnectionString;
        //定义存储过程参数列表
        public IList SqlParameterList = new List<SqlParameter>();
        //定义SqlServer连接对象
        private SqlConnection SqlCon;
        //定义参数输出对象
        private Dictionary<string, object> dicPara = new Dictionary<string, object>();
        //是否包含输出参数
        private bool hasOutput = false;
        #endregion

        #region 构造方法，实例化连接字符串

        /// <summary>
        /// 读取WebConfig链接字符串
        /// </summary>
        public SqlHelp()
        {
            SqlConnectionString = //将AppConfig链接字符串的值给SqlConnectionString变量
                // " server = .;database = hwitdb;Integrated security=SSPI;";
                //ConfigurationManager.AppSettings["Local"];
            System.Configuration.ConfigurationSettings.AppSettings["SqlConnStr"].ToString();

        }

        /// <summary>
        /// 有参构造，实例化连接字符串
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SqlHelp(string connectionString)
        {
            SqlConnectionString = connectionString;
        }

        /// <summary>
        /// 有参构造，实例化连接字符串
        /// </summary>
        /// <param name="server">服务器地址</param>
        /// <param name="intance">数据库名称</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        public SqlHelp(string server, string intance, string userName, string password)
        {
            SqlConnectionString = string.Format("server={0};database={1};uid={2};pwd={3}", server, intance, userName, password);
        }
        #endregion

        #region 实现接口IDisposable
        /// <释放资源接口>
        /// 实现接口IDisposable
        /// </释放资源接口>
        public void Dispose()
        {
            if (SqlCon != null)
            {
                if (SqlCon.State == ConnectionState.Open)//判断数据库连接池是否打开
                {
                    SqlCon.Close();
                }

                if (SqlParameterList.Count > 0)//判断参数列表是否清空
                {
                    SqlParameterList.Clear();
                }
                SqlCon.Dispose();//释放连接池资源
                GC.SuppressFinalize(this);//垃圾回收
            }
        }

        void IDisposable.Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

        #region 析构数据库访问类
        /// <数据库访问类的析构函数>
        /// 析构数据库访问类
        /// </数据库访问类的析构函数>
        ~SqlHelp()
        {
            Dispose();
        }
        #endregion

        #region 传入过程参数
        /// <summary>
        /// 传入过程输入参数
        /// </summary>
        /// <param name="ParaName">参数名</param>
        /// <param name="ParaValue">参数值</param>
        public void SetParaInput(string ParaName, object ParaValue)
        {
            SqlParameter Para = new SqlParameter(ParaName, ParaValue);
            SqlParameterList.Add(Para);//将传进来的参数添加到SqlParameterList列表
        }

        /// <summary>
        /// 传入过程输出参数
        /// </summary>
        /// <param name="ParaName">参数名</param>
        public void SetParaInput(string ParaName)
        {
            //传入整形输出参数
            SqlParameter Para = new SqlParameter(ParaName, SqlDbType.Int);
            Para.Direction = ParameterDirection.Output;
            SqlParameterList.Add(Para);//将传进来的参数添加到SqlParameterList列表
            if (!hasOutput) hasOutput = true;
        }

        /// <summary>
        /// 传入过程输出参数
        /// </summary>
        /// <param name="Para">参数名</param>
        public void SetParaInput(ref SqlParameter Para)
        {
            Para.Direction = ParameterDirection.Output;
            SqlParameterList.Add(Para);//将传进来的参数添加到SqlParameterList列表
            if (!hasOutput) hasOutput = true;
        }
        #endregion

        #region 执行存储过程,返回SqlDataReader
        /// <summary>
        /// 执行存储过程,返回SqlDataReader
        /// </summary>
        /// <param name="StoreProcedureName">存储过程名</param>
        /// <returns></returns>
        public DbDataReader ExecuteProcReader(string StoreProcedureName)
        {
            SqlCon = new SqlConnection(SqlConnectionString);
            using (SqlCommand SqlCMD = new SqlCommand())
            {
                try
                {
                    SqlCon.Open();//打开数据库连接池
                    SqlCMD.Connection = SqlCon;
                    SqlCMD.CommandTimeout = 0;
                    SqlCMD.CommandType = CommandType.StoredProcedure;
                    SqlCMD.CommandText = StoreProcedureName;
                    if (SqlParameterList.Count != 0)
                    {
                        foreach (SqlParameter Para in SqlParameterList)//循环添加数据到SqlCommand对象里面
                        {
                            SqlCMD.Parameters.Add(Para);
                        }
                    }
                    SqlParameterList.Clear();
                    SqlDataReader reader = SqlCMD.ExecuteReader(CommandBehavior.CloseConnection);
                    return reader;//返回结果集
                }
                catch (Exception ex)
                {
                    Dispose();
                    throw ex;
                }
            }
        }
        #endregion

        #region 执行存储过程，返回DataTable
        /// <执行返回DataTable的存储过程>
        /// 执行存储过程返回DataTable
        /// </执行返回DataTable的存储过程>
        /// <存储过程名 name="StoreProcedureName">存储过程名</存储过程名>
        /// <返回>返回DataTable类型的结果集</返回>
        public DataTable ExecuteProc(string StoreProcedureName)
        {
            using (SqlCon = new SqlConnection(SqlConnectionString))
            {
                using (SqlCommand SqlCMD = new SqlCommand())
                {
                    try
                    {
                        SqlCon.Open();//打开数据库连接池
                        SqlCMD.Connection = SqlCon;
                        SqlCMD.CommandTimeout = 0;
                        SqlCMD.CommandType = CommandType.StoredProcedure;
                        SqlCMD.CommandText = StoreProcedureName;

                        if (SqlParameterList.Count != 0)
                        {
                            foreach (SqlParameter Para in SqlParameterList)//循环添加数据到SqlCommand对象里面
                            {
                                SqlCMD.Parameters.Add(Para);
                            }
                        }
                        SqlParameterList.Clear();
                        using (SqlDataAdapter Sqladapter = new SqlDataAdapter(SqlCMD))//创建适配器
                        {
                            DataTable SqlDataTable = new DataTable();
                            Sqladapter.SelectCommand.CommandTimeout = 0;
                            Sqladapter.Fill(SqlDataTable);
                            return SqlDataTable;//返回结果集
                        }
                    }
                    catch (Exception ex)
                    {
                        SqlCon.Close();//执行失败则立刻关闭链接
                        throw ex;
                    }
                    finally
                    {
                        Dispose();//释放资源
                    }
                }
            }
        }
        #endregion

        #region 执行存储过程返回影响的行数
        /// <执行存储过程有参数返回>
        /// 执行存储过程
        /// </执行存储过程有参数返回>
        /// <存储过程名 name="StoreProcedureName">存储过程名参数</存储过程名>
        public int ExecProceudre(string StoreProcedureName)
        {
            using (SqlCon = new SqlConnection(SqlConnectionString))//创建SqlServer连接对象
            {
                using (SqlCommand SqlCMD = new SqlCommand())//创建SqlServer命令对象
                {
                    try
                    {
                        SqlCon.Open();
                        SqlCMD.Connection = SqlCon;
                        SqlCMD.CommandTimeout = 0;
                        SqlCMD.CommandType = CommandType.StoredProcedure;
                        SqlCMD.CommandText = StoreProcedureName;
                        if (SqlParameterList.Count != 0)
                        {
                            foreach (SqlParameter Para in SqlParameterList)
                            {
                                SqlCMD.Parameters.Add(Para);
                            }
                        }
                        SqlParameterList.Clear();
                        var ret = SqlCMD.ExecuteNonQuery();//执行增删改操作
                        InitDic(SqlCMD.Parameters);
                        return ret;
                    }
                    catch (Exception ex)
                    {
                        SqlCon.Close();
                        throw ex;
                    }
                    finally
                    {
                        Dispose();
                    }
                }
            }
        }
        #endregion

        #region 执行SQL文本查询,返回SqlDataReader
        /// <执行返回SqlDataReader的存储过程>
        /// 执行SQL返回SqlDataReader，使用完毕一定要记得关闭 reader.Close();
        /// </执行返回SqlDataReader的存储过程>
        /// <存储过程名 name="strSql">执行的SQL语句</存储过程名>
        /// <返回>返回SqlDataReader类型的结果集</返回>
        public DbDataReader ExecuteSqlReader(string strSql)
        {
            SqlCon = new SqlConnection(SqlConnectionString);
            using (SqlCommand SqlCMD = new SqlCommand())
            {
                try
                {
                    SqlCon.Open();//打开数据库连接池
                    SqlCMD.Connection = SqlCon;
                    SqlCMD.CommandTimeout = 0;
                    SqlCMD.CommandType = CommandType.Text;
                    SqlCMD.CommandText = strSql;
                    if (SqlParameterList.Count != 0)
                    {
                        foreach (SqlParameter Para in SqlParameterList)//循环添加数据到SqlCommand对象里面
                        {
                            SqlCMD.Parameters.Add(Para);
                        }
                    }
                    SqlParameterList.Clear();
                    SqlDataReader reader = SqlCMD.ExecuteReader(CommandBehavior.CloseConnection);
                    return reader;//返回结果集
                }
                catch (Exception ex)
                {
                    Dispose();
                    throw ex;
                }
            }
        }
        #endregion

        #region 执行Sql文本查询，返回DataTable
        /// <summary>
        /// 执行Sql文本查询，返回DataTable
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        public DataTable ExecuteSql(string strSql)
        {
            using (SqlCon = new SqlConnection(SqlConnectionString))
            {
                using (SqlCommand SqlCMD = new SqlCommand())
                {
                    try
                    {
                        SqlCon.Open();//打开数据库连接池
                        SqlCMD.Connection = SqlCon;
                        SqlCMD.CommandTimeout = 0;
                        SqlCMD.CommandType = CommandType.Text;
                        SqlCMD.CommandText = strSql;
                        if (SqlParameterList.Count != 0)
                        {
                            foreach (SqlParameter Para in SqlParameterList)
                            {
                                SqlCMD.Parameters.Add(Para);
                            }
                        }
                        SqlParameterList.Clear();
                        using (SqlDataAdapter Sqladapter = new SqlDataAdapter(SqlCMD))//创建适配器
                        {
                            DataTable SqlDataTable = new DataTable();
                            Sqladapter.SelectCommand.CommandTimeout = 0;
                            Sqladapter.Fill(SqlDataTable);
                            return SqlDataTable;//返回结果集
                        }

                    }
                    catch (Exception ex)
                    {
                        SqlCon.Close();//执行失败则立刻关闭链接
                        throw ex;
                    }
                    finally
                    {
                        Dispose();//释放资源
                    }
                }
            }
        }
        #endregion

        #region 执行Sql文本，返回影响的行数
        /// <summary>
        /// 执行Sql文本，返回影响的行数
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string strSql)
        {
            using (SqlCon = new SqlConnection(SqlConnectionString))
            {
                using (SqlCommand SqlCMD = new SqlCommand())
                {
                    try
                    {
                        SqlCon.Open();//打开数据库连接池
                        SqlCMD.Connection = SqlCon;
                        SqlCMD.CommandType = CommandType.Text;
                        SqlCMD.CommandText = strSql;
                        SqlCMD.CommandTimeout = 0;
                        if (SqlParameterList.Count != 0)
                        {
                            foreach (SqlParameter Para in SqlParameterList)
                            {
                                SqlCMD.Parameters.Add(Para);
                            }
                        }
                        SqlParameterList.Clear();
                        var ret = SqlCMD.ExecuteNonQuery();
                        InitDic(SqlCMD.Parameters);
                        return ret;//返回结果集
                    }
                    catch (Exception ex)
                    {
                        SqlCon.Close();//执行失败则立刻关闭链接
                        throw ex;
                    }
                    finally
                    {
                        Dispose();//释放资源
                    }
                }
            }
        }
        public int ExecuteNonQuery(SqlTransaction tran, string strSql)
        {
            SqlCon = tran.Connection;
            SqlCommand SqlCMD = new SqlCommand();
            try
            {
                //SqlCon.Open();//打开数据库连接池
                SqlCMD.Connection = SqlCon;
                SqlCMD.CommandType = CommandType.Text;
                SqlCMD.CommandText = strSql;
                SqlCMD.CommandTimeout = 0;
                SqlCMD.Transaction = tran;
                if (SqlParameterList.Count != 0)
                {
                    foreach (SqlParameter Para in SqlParameterList)
                    {
                        SqlCMD.Parameters.Add(Para);
                    }
                }
                SqlParameterList.Clear();
                var ret = SqlCMD.ExecuteNonQuery();
                InitDic(SqlCMD.Parameters);
                return ret;//返回结果集
            }
            catch (Exception ex)
            {
                //SqlCon.Close();//执行失败则立刻关闭链接
                throw ex;
            }
            finally
            {
                //Dispose();//释放资源
            }
        }
        #endregion

        #region 测试连接是否成功
        /// <summary>
        /// 测试连接是否成功
        /// </summary>
        /// <returns></returns>
        public bool HasConnection
        {
            get
            {
                bool flag;
                try
                {
                    SqlCon = new SqlConnection(SqlConnectionString);
                    SqlCon.Open();
                    flag = true;
                }
                catch
                {
                    flag = false;
                    //throw ex;
                }
                finally
                {
                    SqlCon.Close();
                    Dispose();
                }
                return flag;
            }
        }
        #endregion

        #region 属性访问
        public object this[string name]
        {
            set
            {
                if (value == null) value = DBNull.Value;
                SqlParameter Para = new SqlParameter(name, value);
                SqlParameterList.Add(Para);
            }
            get
            {
                if (dicPara.ContainsKey(name)) return dicPara[name];
                else return null;
            }
        }

        public object this[string name, DbType dbtype]
        {
            set
            {
                if (value == null) value = DBNull.Value;
                SqlParameter Para = new SqlParameter(name, value);
                Para.DbType = dbtype;
                SqlParameterList.Add(Para);
            }
        }
        #endregion

        #region 执行Insert语句返回自增
        /// <summary>
        /// 执行Insert语句返回自增
        /// </summary>
        /// <param name="strSql">strSql</param>
        /// <returns></returns>
        public int ExecuteInsert(string strSql)
        {
            using (SqlCon = new SqlConnection(SqlConnectionString))
            {
                using (SqlCommand SqlCMD = new SqlCommand())
                {
                    try
                    {
                        SqlCon.Open();//打开数据库连接池
                        SqlCMD.Connection = SqlCon;
                        SqlCMD.CommandType = CommandType.Text;
                        SqlCMD.CommandText = strSql;
                        SqlCMD.CommandTimeout = 0;
                        if (SqlParameterList.Count != 0)
                        {
                            foreach (SqlParameter Para in SqlParameterList)
                            {
                                SqlCMD.Parameters.Add(Para);
                            }
                        }
                        SqlParameterList.Clear();
                        var ret = SqlCMD.ExecuteNonQuery();
                        if (ret > 0)
                        {
                            SqlCMD.CommandText = "select @@IDENTITY";
                            return Convert.ToInt32(SqlCMD.ExecuteScalar());
                        }
                        return ret;//返回结果集
                    }
                    catch (Exception ex)
                    {
                        SqlCon.Close();//执行失败则立刻关闭链接
                        throw ex;
                    }
                    finally
                    {
                        Dispose();//释放资源
                    }
                }
            }
        }
        public int ExecuteInsert(SqlTransaction tran, string strSql)
        {
            SqlCon = tran.Connection;
            SqlCommand SqlCMD = new SqlCommand();
            try
            {
                //SqlCon.Open();//打开数据库连接池
                SqlCMD.Connection = SqlCon;
                SqlCMD.CommandType = CommandType.Text;
                SqlCMD.CommandText = strSql;
                SqlCMD.CommandTimeout = 0;
                SqlCMD.Transaction = tran;
                if (SqlParameterList.Count != 0)
                {
                    foreach (SqlParameter Para in SqlParameterList)
                    {
                        SqlCMD.Parameters.Add(Para);
                    }
                }
                SqlParameterList.Clear();
                var ret = SqlCMD.ExecuteNonQuery();
                if (ret > 0)
                {
                    SqlCMD.CommandText = "select @@IDENTITY";
                    return Convert.ToInt32(SqlCMD.ExecuteScalar());
                }
                return ret;//返回结果集
            }
            catch (Exception ex)
            {
                //SqlCon.Close();//执行失败则立刻关闭链接
                throw ex;
            }
            finally
            {
                //Dispose();//释放资源
            }
        }
        #endregion

        #region 执行统计语句返回统计结果
        /// <summary>
        /// 执行统计语句返回统计结果
        /// </summary>
        /// <param name="strSql">strSql</param>
        /// <returns></returns>
        public int Count(string strSql)
        {
            using (SqlCon = new SqlConnection(SqlConnectionString))
            {
                using (SqlCommand SqlCMD = new SqlCommand())
                {
                    try
                    {
                        SqlCon.Open();//打开数据库连接池
                        SqlCMD.Connection = SqlCon;
                        SqlCMD.CommandType = CommandType.Text;
                        SqlCMD.CommandText = strSql;
                        SqlCMD.CommandTimeout = 0;
                        if (SqlParameterList.Count != 0)
                        {
                            foreach (SqlParameter Para in SqlParameterList)
                            {
                                SqlCMD.Parameters.Add(Para);
                            }
                        }
                        SqlParameterList.Clear();
                        var obj = SqlCMD.ExecuteScalar();
                        int ret;
                        if (int.TryParse(obj.ToString(), out ret))
                        {
                            return ret;
                        }
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        SqlCon.Close();//执行失败则立刻关闭链接
                        throw ex;
                    }
                    finally
                    {
                        Dispose();//释放资源
                    }
                }
            }
        }
        #endregion

        #region 执行一组Sql语句，选择性使用事务
        /// <summary>
        /// 执行一组Sql语句，使用事务
        /// </summary>
        /// <param name="SqlList">SqlList</param>
        public void ExecuteSqlByTran(List<string> SqlList)
        {
            using (SqlCon = new SqlConnection(SqlConnectionString))
            {
                using (SqlCommand SqlCMD = new SqlCommand())
                {
                    SqlCon.Open();
                    SqlTransaction tran = SqlCon.BeginTransaction();
                    SqlCMD.Connection = SqlCon;
                    SqlCMD.CommandType = CommandType.Text;
                    SqlCMD.CommandTimeout = 0;
                    SqlCMD.Transaction = tran;
                    try
                    {
                        foreach (string sql in SqlList)
                        {
                            SqlCMD.CommandText = sql.ToUpper();
                            SqlCMD.ExecuteNonQuery();
                        }
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        SqlCon.Close();
                        throw ex;
                    }
                    finally
                    {
                        Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// 不使用事务执行Sql语句
        /// </summary>
        /// <param name="strSql"></param>
        public void ExecuteSqlNoTran(List<string> strSql)
        {
            using (SqlCon = new SqlConnection(SqlConnectionString))
            {
                using (SqlCommand SqlCMD = new SqlCommand())
                {
                    SqlCon.Open();
                    SqlCMD.Connection = SqlCon;
                    SqlCMD.CommandType = CommandType.Text;
                    SqlCMD.CommandTimeout = 0;
                    try
                    {
                        foreach (var sql in strSql)
                        {
                            SqlCMD.CommandText = sql.ToUpper();
                            SqlCMD.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        SqlCon.Close();
                        throw ex;
                    }
                    finally
                    {
                        Dispose();
                    }
                }
            }
        }
        #endregion

        #region  直接将DataTable的数据按同样的结构复制到指定的表中
        /// <summary>
        /// 直接将DataTable的数据按同样的结构复制到指定的表中
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool SqlBulkCopy(DataTable dt)
        {
            try
            {
                //数据批量导入sqlserver,创建实例    SqlBulkCopyOptions.UseInternalTransaction采用事务  复制失败自动回滚
                using (var sqlbulk = new System.Data.SqlClient.SqlBulkCopy(SqlConnectionString, SqlBulkCopyOptions.UseInternalTransaction))
                {
                    sqlbulk.NotifyAfter = dt.Rows.Count;
                    //目标数据库表名
                    sqlbulk.DestinationTableName = dt.TableName;
                    //数据集字段索引与数据库字段索引映射
                    foreach (DataColumn dc in dt.Columns)
                    {
                        sqlbulk.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                    }
                    //导入数据
                    sqlbulk.WriteToServer(dt);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt.Dispose();
            }
        }
        #endregion

        #region 辅助方法

        #region 将参数化Sql转换成纯Sql
        /// <summary>
        /// 将参数化Sql转换成纯Sql
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        public string getSqlOnly(string strSql)
        {
            foreach (DbParameter para in SqlParameterList)
            {
                if (para.DbType == DbType.Int16 || para.DbType == DbType.Int32 || para.DbType == DbType.Int64 || para.DbType == DbType.UInt16 || para.DbType == DbType.UInt32 || para.DbType == DbType.UInt64 || para.DbType == DbType.Decimal || para.DbType == DbType.Double || para.DbType == DbType.Single)
                {
                    strSql = strSql.Replace(para.ParameterName, para.Value.ToString());
                }
                else
                {
                    strSql = strSql.Replace(para.ParameterName, "'" + para.Value.ToString() + "'");
                }
            }
            return strSql;
        }
        #endregion

        #region 获取输出参数 并添加到字典中
        /// <summary>
        /// 获取输出参数 并添加到字典中 仅当执行非查询时才调用
        /// </summary>
        /// <param name="Parameters"></param>
        private void InitDic(DbParameterCollection Parameters)
        {
            if (hasOutput)
            {
                dicPara.Clear();
                foreach (DbParameter Para in Parameters)
                {
                    if (Para.Direction != ParameterDirection.Input)
                    {
                        dicPara.Add(Para.ParameterName, Para.Value);
                    }
                }
                hasOutput = false;
            }
        }
        #endregion

        #endregion
    }
}
