using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Data.SqlClient;

namespace DAL
{
    public class DocDAL
    {
        SqlHelp dbhelper = new SqlHelp();

        #region 获取报刊信息
        /// <summary>
        /// 获取报刊信息
        /// </summary>
        /// <param name="ID">编号</param>
        /// <param name="TYPEID">类型编号</param>
        /// <param name="NAME">名称</param>
        /// <param name="ISSN">ISSN</param>
        /// <param name="PUBLISHAREA">出版地</param>
        /// <param name="PUBLISHYEAR">出版年</param>
        /// <param name="ADDPERSON">添加人</param>
        /// <param name="DocRegData_Begin">新增报刊时间范围,起始值</param>
        /// <param name="DocRegData_End">新增报刊时间范围,截止值</param>
        /// <returns></returns>
        public DataTable GetDoc(string ID, string TYPEID, string NAME, string ISSN, string PUBLISHAREA, string PUBLISHER, string ADDPERSON, string DocRegData_Begin, string DocRegData_End)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT DOC.ID,DOC.TYPEID, DOCTYPE.TYPENAME, DOC.NAME DOCNAME, ISSN, PUBLISHAREA,
       PUBLISHER,  PRICE, PL, BKDH, ISNULL(USERS.NAME ,'')ADDPERSON,BKDH,
       CONVERT(VARCHAR(100), ADDDATE, 23) ADDDATE,DOC.NGUID
FROM DOC
LEFT JOIN DOCTYPE ON DOC.TYPEID = DOCTYPE.TYPEID
LEFT JOIN USERS ON USERS.ID = DOC.ADDPERSON  WHERE 1=1 ";
            if (!string.IsNullOrEmpty(ID._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("ID", ID._ToInt32());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND DOC.ID =@ID";
            }
            if (!string.IsNullOrEmpty(TYPEID._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("TYPEID", TYPEID._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND DOC.TYPEID =@TYPEID";
            }
            if (!string.IsNullOrEmpty(NAME._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("NAME", NAME._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND DOC.NAME like '%'+@NAME+'%'";
            }
            if (!string.IsNullOrEmpty(ISSN._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("ISSN", ISSN._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND DOC.ISSN like '%'+@ISSN+'%'";
            }
            if (!string.IsNullOrEmpty(PUBLISHAREA._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("PUBLISHAREA", PUBLISHAREA._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND DOC.PUBLISHAREA  like '%'+@PUBLISHAREA+'%'";
            }
            if (!string.IsNullOrEmpty(PUBLISHER._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("PUBLISHER", PUBLISHER._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND DOC.PUBLISHER  like '%'+@PUBLISHER+'%'";
            }
            if (!string.IsNullOrEmpty(ADDPERSON._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("ADDPERSON", ADDPERSON._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND DOC.ADDPERSON =@ADDPERSON";
            }
            if (!string.IsNullOrEmpty(DocRegData_Begin._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("INDATE1", DocRegData_Begin._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND CONVERT(varchar(100),DOC.AddDate, 23)  >= CONVERT(varchar(100),@INDATE1, 23)";
            }
            if (!string.IsNullOrEmpty(DocRegData_End._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("INDATE2", DocRegData_End._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND  CONVERT(varchar(100),DOC.AddDate, 23)  <= CONVERT(varchar(100),@INDATE2, 23) ";
            }
            dt = dbhelper.ExecuteSql(sql + " ORDER BY DOC.ID");
            return dt;
        }
        #endregion

        #region 根据报刊编号更新数据
        /// <summary>
        /// 根据报刊编号更新数据
        /// </summary>
        /// <param name="ID">报刊编号</param>
        /// <param name="Name">名称</param>
        /// <param name="ISSN">ISSN</param>
        /// <param name="TypeID">类型ID</param>
        /// <param name="PublishArea">出版地</param>
        /// <param name="Publisher">出版社</param>
        /// <param name="PublishYear">出版年</param>
        /// <param name="C200F">责任者</param>
        /// <param name="Price">单价</param>
        /// <param name="PL">出版频率</param>
        /// <param name="BKDH">报刊代号</param>
        /// <param name="guid">唯一标识</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public string UpdateByPK(int ID, string Name, string ISSN, string TypeID, string PublishArea, string Publisher, string Price,string PL,string BKDH, string guid)
        {
            string res = "";
            try
            {
                if (!checkDoc(guid))
                {
                    res = "数据已被修改,请刷新后尝试";
                }
                else
                {
                    string sql = @" UPDATE  Doc 
   SET   ";
                    SqlParameter Para = null;
                    if (!string.IsNullOrEmpty(TypeID._ToStrTrim()))
                    {
                        sql += " TypeID  = @TypeID,";
                        Para= new SqlParameter("TypeID", TypeID._ToInt32());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    if (!string.IsNullOrEmpty(Name._ToStrTrim()))
                    {
                        sql += " Name  = @Name,";
                        Para= new SqlParameter("Name", Name._ToStrTrim());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    if (!string.IsNullOrEmpty(ISSN._ToStrTrim()))
                    {
                        sql += " ISSN  = @ISSN,";
                        Para= new SqlParameter("ISSN", ISSN._ToStrTrim());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    if (!string.IsNullOrEmpty(PublishArea._ToStrTrim()))
                    {
                        sql += " PublishArea  = @PublishArea,";
                        Para= new SqlParameter("PublishArea", PublishArea._ToStrTrim());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    if (!string.IsNullOrEmpty(PublishArea._ToStrTrim()))
                    {
                        sql += " Publisher  = @Publisher,";
                        Para= new SqlParameter("Publisher", Publisher._ToStrTrim());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    if (!string.IsNullOrEmpty(Price._ToStrTrim()))
                    {
                        sql += " Price  = @Price,";
                        Para= new SqlParameter("Price", Price._ToDecimal());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    if (!string.IsNullOrEmpty(PL._ToStrTrim()))
                    {
                        sql += " PL  = @PL,";
                        Para = new SqlParameter("PL", PL._ToStrTrim());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    if (!string.IsNullOrEmpty(BKDH._ToStrTrim()))
                    {
                        sql += " BKDH  = @BKDH,";
                        Para = new SqlParameter("BKDH", BKDH._ToStrTrim());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    sql += " NGuid  = NEWID()  WHERE ID = @ID ";
                    Para = new SqlParameter("ID", ID._ToInt32());
                    dbhelper.SqlParameterList.Add(Para);
                    int num = dbhelper.ExecuteNonQuery(sql);
                    if (num == 0)
                    {
                        res = "操作失败";
                    }
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        #endregion

        #region 检测报刊是否可以被修改或者删除
        /// <summary>
        /// 检测报刊是否可以被修改或者删除
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool checkDoc(string guid)
        {
            string sql = @" select 1 from Doc where NGuid =@NGuid";
            SqlParameter Para = new SqlParameter("NGuid", guid._ToStrTrim());
            dbhelper.SqlParameterList.Add(Para);
            return dbhelper.ExecuteSql(sql).Rows.Count > 0;
        }
        #endregion

        #region 根据主键删除数据
        /// <summary>
        /// 根据主键删除数据
        /// </summary>
        /// <param name="IDs">报刊编号,多个用英文逗号分隔</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public string DeleteByPK(string IDs)
        {
            string res = "";
            try
            {
                string[] list = IDs.Split(',');
                int num = 0;
                foreach (string item in list)
                {
                    string sql = @" DELETE FROM  Doc  WHERE ID = @ID ";
                    SqlParameter Para = new SqlParameter("ID", item._ToInt32());
                    dbhelper.SqlParameterList.Add(Para);
                    num += dbhelper.ExecuteNonQuery(sql);
                }
                if (num == 0)
                {
                    res = "操作失败";
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        #endregion

        #region 插入报刊
        /// <summary>
        /// 插入报刊
        /// </summary>
        /// <param name="Name">名称</param>
        /// <param name="ISSN">ISSN</param>
        /// <param name="TypeID">类型ID</param>
        /// <param name="PublishArea">出版地</param>
        /// <param name="Publisher">出版社</param>
        /// <param name="PublishYear">出版年</param>
        /// <param name="C200F">责任者</param>
        /// <param name="Price">单价,一个月的单价</param>
        /// <param name="PL">出版频率</param>
        /// <param name="BKDH">报刊代号</param>
        /// <param name="AddPerson">添加人</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public string Insert(string Name, string ISSN, string TypeID, string PublishArea, string Publisher,  string Price, string PL, string BKDH, string AddPerson)
        {
            string res = "";
            try
            {
                if (dbhelper.Count("select count(1) from Doc where bkdh='" + BKDH + "'") > 0)
                {
                    res = "报刊代号已存在";
                }
                string sql = @"INSERT INTO Doc 
           ( TypeID 
           , Name 
           , ISSN 
           , PublishArea 
           , Publisher 
           , Price 
           , PL 
           , BKDH 
           , AddPerson ) VALUES (@TypeID 
           , @NAME 
           , @ISSN 
           , @PublishArea 
           , @Publisher 
           , @Price 
           , @PL 
           , @BKDH 
           , @AddPerson)";
                SqlParameter Para = new SqlParameter("TypeID", TypeID._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("NAME", Name._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("ISSN", ISSN._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("PublishArea", PublishArea._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("Publisher", Publisher._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("Price", Price._ToDecimal());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("PL", PL._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("BKDH", BKDH._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("AddPerson", AddPerson._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                
                int num = dbhelper.ExecuteNonQuery(sql);
                if (num == 0)
                {
                    res = "操作失败";
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        #endregion

        public decimal GetPrice(string bkdh) 
        {
            string sql = @" select price from doc where bkdh =@bkdh";
            SqlParameter Para = new SqlParameter("bkdh", bkdh._ToStr());
            dbhelper.SqlParameterList.Add(Para);
            DataTable dt = dbhelper.ExecuteSql(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0][0]._ToDecimal();
            }
            else
            {
                return 0;
            }
        }
    }
}
