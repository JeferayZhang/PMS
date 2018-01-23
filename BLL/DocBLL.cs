using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Common;
using System.Data;


namespace BLL
{
    public class DocBLL
    {
        DocDAL dal = new DocDAL();

        #region 获取报刊信息
        /// <summary>
        /// 获取报刊信息
        /// </summary>
        /// <param name="ID">编号</param>
        /// <param name="TYPEID">类型编号</param>
        /// <param name="NAME">名称</param>
        /// <param name="ISSN">ISSN</param>
        /// <param name="PUBLISHAREA">出版地</param>
        /// <param name="PUBLISHER">出版社</param>
        /// <param name="ADDPERSON">添加人</param>
        /// <param name="DocRegData_Begin">新增报刊时间范围,起始值</param>
        /// <param name="DocRegData_End">新增报刊时间范围,截止值</param>
        /// <returns></returns>
        public retValue GetDoc(string ID, string TYPEID, string NAME, string ISSN, string PUBLISHAREA, string PUBLISHER, string ADDPERSON, string DocRegData_Begin, string DocRegData_End)
        {
            retValue ret = new retValue();
            DataTable dt = dal.GetDoc(ID, TYPEID, NAME, ISSN, PUBLISHAREA, PUBLISHER, ADDPERSON, DocRegData_Begin, DocRegData_End);
            if (dt != null && dt.Rows.Count > 0)
            {
                ret.result = true;
                ret.data = dt;
            }
            else
            {
                ret.result = false;
                ret.reason = "未能查询到数据";
            }
            return ret;
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
        public retValue UpdateByPK(int ID, string Name, string ISSN, string TypeID, string PublishArea, string Publisher, string Price, string PL, string BKDH, string guid)
        {
            retValue ret = new retValue();
            string res = dal.UpdateByPK(ID, Name, ISSN, TypeID, PublishArea, Publisher,  Price, PL, BKDH, guid);
            if (string.IsNullOrEmpty(res))
            {
                ret.result = true; ret.data = "保存成功";
            }
            else
            {
                ret.result = false; ret.reason = res;
            }
            return ret;
        }
        #endregion

        #region 根据主键删除数据
        /// <summary>
        /// 根据主键删除数据
        /// </summary>
        /// <param name="IDs">报刊编号,多个用英文逗号分隔</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public retValue DeleteByPK(string IDs)
        {
            retValue ret = new retValue();
            string res = dal.DeleteByPK(IDs);
            if (string.IsNullOrEmpty(res))
            {
                ret.result = true; ret.data = "操作成功";
            }
            else
            {
                ret.result = false; ret.reason = res;
            }
            return ret;
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
        public retValue Insert(string Name, string ISSN, string TypeID, string PublishArea, 
            string Publisher,  string Price, string PL, string BKDH, string AddPerson)
        {
            retValue ret = new retValue();
            string res = dal.Insert(Name, ISSN, TypeID, PublishArea,
             Publisher, Price, PL, BKDH, AddPerson);
            if (string.IsNullOrEmpty(res))
            {
                ret.result = true; ret.data = "操作成功";
            }
            else
            {
                ret.result = false; ret.reason = res;
            }
            return ret;
        }
        #endregion
    }
}
