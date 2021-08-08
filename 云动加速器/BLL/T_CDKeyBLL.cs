using CloudsMove.DAL;
using CloudsMove.ViewModels;
using System.Collections.Generic;

namespace CloudsMove.BLL
{
    public class T_CDKeyBLL
    {
        private readonly T_CDKeyDAL dal = new T_CDKeyDAL();

        public bool Add(T_CDKey model)
        {
            return dal.Add(model);
        }

        public bool Update(T_CDKey model)
        {
            return dal.Update(model);
        }

        public bool Delete(string serialCode)
        {
            return dal.Delete(serialCode);
        }

        public T_CDKey Find(string serialCode)
        {
            return dal.Find(serialCode);
        }

        public List<T_CDKey> FindAll ()
        {
            return dal.FindAll();
        }
    }
}
