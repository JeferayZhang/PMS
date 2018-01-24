using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Common;
using PMS.Models;

namespace PMS
{
    public static class authorize
    {
        public static bool checkFilterContext()
        {
            string[] path = HttpContext.Current.Request.Path.Split('/');
            if (path.Length < 3)
                return true;
            return checkFilterContext(path[1], path[2], HttpContext.Current.Session);

        }


        public static bool checkFilterContext(string controllername, string actionname, HttpSessionState session)
        {
            controllername = controllername.ToUpper();
            actionname = actionname.ToLower();
            UserModel user = session["UserModel"] as UserModel;
            if (user==null)
            {
                return false;
            }
            if (controllername == "HOME")
                return true;
            int user_id = user._ID;
            if (user_id <= 0)
                return false;
            int usertype = user.Role;
            if (usertype <= 0 || usertype > 4)
                return false;
            int school_id = user.OrgID;
            if (school_id <= 0)
                return false;
            return true;
        }

        public static ActionResult returnNeedLogin()
        {
            ContentResult ret = new ContentResult();
            if (HttpContext.Current.Request.AcceptTypes[0] == "text/plain")
            {
                ret.Content = "NEEDLOGIN";
                return ret;
            }
            else
            {
                ret.Content = "[{\"ERRORCODE\":\"NEEDLOGIN\"}]";
                return ret;
            }

        }
    }
}