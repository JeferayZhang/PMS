using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Common;

namespace BLL
{
    public class DocTypeBLL
    {
        DocTypeDAL dal = new DocTypeDAL();

        #region 获取类型信息
        /// <summary>
        /// 获取类型信息
        /// </summary>
        /// <param name="TypeName">名称</param>
        /// <param name="TypeID">类型ID</param>
        /// <returns></returns>
        public DataTable GetDocType(string TypeID, string TypeName)
        {
            return dal.GetDocType(TypeID, TypeName);
        }
        #endregion

        #region 根据类型编号更新数据
        /// <summary>
        /// 根据类型编号更新数据
        /// </summary>
        /// <param name="TypeID">类型编号</param>
        /// <param name="TypeName">名称</param>
        /// <param name="TypeDesc">备注</param>
        /// <param name="guid">唯一标识,每次修改数据会同时修改此列</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public string UpdateByPK(int TypeID, string TypeName, string TypeDesc, string guid)
        {
            return dal.UpdateByPK(TypeID, TypeName, TypeDesc, guid);
        }
        #endregion

        #region 根据主键删除数据
        /// <summary>
        /// 根据主键删除数据
        /// </summary>
        /// <param name="TypeIDs">类型编号,多个用英文逗号分隔</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public string DeleteByPK(string TypeIDs)
        {
            return dal.DeleteByPK(TypeIDs);
        }
        #endregion

        #region 插入类型
        /// <summary>
        /// 插入类型
        /// </summary>
        /// <param name="TypeName">名称</param>
        /// <param name="TypeDesc">说明</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public string Insert(string TypeName, string TypeDesc)
        {
            return dal.Insert(TypeName, TypeDesc);
        }
        #endregion
    }
}
