using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public class UserModel
    {
        /// <summary>
        /// 标识用户是否需要登录
        /// </summary>
        public static bool needlogin = true;
        private int ID;
        /// <summary>
        /// 用户流水号
        /// </summary>
        public int _ID
        {
            get { return ID; }
            set { ID = value; }
        }

        private string _UserNo;
        /// <summary>
        /// 登录名
        /// </summary>
        public string UserNo
        {
            get { return _UserNo; }
            set { _UserNo = value; }
        }

        private string password;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string _Name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private int _Role;
        /// <summary>
        /// 角色类型:1管理员;2分发员;3投递员;4财务员
        /// </summary>
        public int Role
        {
            get { return _Role; }
            set { _Role = value; }
        }

        private int _Sex;
        /// <summary>
        /// 性别 1男0女
        /// </summary>
        public int Sex
        {
            get { return _Sex; }
            set { _Sex = value; }
        }

        private string _IDCard;
        /// <summary>
        /// 身份证
        /// </summary>
        public string IDCard
        {
            get { return _IDCard; }
            set { _IDCard = value; }
        }

        private int _State;
        /// <summary>
        /// 状态  0正常,1注销
        /// </summary>
        public int State
        {
            get { return _State; }
            set { _State = value; }
        }

        private int _OrgID;
        /// <summary>
        /// 所属机构ID
        /// </summary>
        public int OrgID
        {
            get { return _OrgID; }
            set { _OrgID = value; }
        }

        /// <summary>
        /// 用户机构等级
        /// </summary>

        private int level;

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        private string OrgName;

        public string _OrgName
        {
            get { return OrgName; }
            set { OrgName = value; }
        }

        private string orgNo;

        public string OrgNo
        {
            get { return orgNo; }
            set { orgNo = value; }
        }
    }
}