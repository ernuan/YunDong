using CloudsMove.DAL;
using CloudsMove.ViewModels;
using System.Collections.Generic;

namespace CloudsMove.BLL
{
    public class T_UserBLL
    {
        private readonly T_UserDAL dal = new T_UserDAL();

        public bool Add(T_User model)
        {
            return dal.Add(model);
        }

        public bool Update(T_User model)
        {
            return dal.Update(model);
        }

        public bool Delete(string serialCode)
        {
            return dal.Delete(serialCode);
        }

        public T_User Find(string mail)
        {
            return dal.Find(mail);
        }

        public T_User Find(string mail,string pwd)
        {
            return dal.Find(mail,pwd);
        }

        public List<T_User> FindAll ()
        {
            return dal.FindAll();
        }
    }
}
