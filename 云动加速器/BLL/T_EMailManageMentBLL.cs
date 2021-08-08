using CloudsMove.DAL;
using CloudsMove.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudsMove.BLL
{
    public class T_EMailManageMentBLL
    {
        private readonly T_EMailManageMentDAL dal = new T_EMailManageMentDAL();

        public bool Add(T_EMailManageMent model)
        {
            return dal.Add(model);
        }

        public bool Update(T_EMailManageMent model)
        {
            return dal.Update(model);
        }

        public T_EMailManageMent Find()
        {
            return dal.Find();
        }
    }
}
