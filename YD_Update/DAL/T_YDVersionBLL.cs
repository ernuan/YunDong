using CloudsMove.DAL;
using CloudsMove.ViewModels;
using System.Collections.Generic;

namespace CloudsMove.BLL
{
    public class T_YDVersionBLL
    {
        private readonly T_YDVersionDAL dal = new T_YDVersionDAL();

        public bool Add(T_YDVersion model)
        {
            return dal.Add(model);
        }

        public bool Update(T_YDVersion model)
        {
            return dal.Update(model);
        }

        public bool Delete(string serialCode)
        {
            return dal.Delete(serialCode);
        }

        public T_YDVersion Find(string serialCode)
        {
            return dal.Find(serialCode);
        }

        public List<T_YDVersion> FindAll ()
        {
            return dal.FindAll();
        }
    }
}
