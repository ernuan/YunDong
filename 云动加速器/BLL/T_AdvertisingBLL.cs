using CloudsMove.DAL;
using CloudsMove.ViewModels;
using System.Collections.Generic;

namespace CloudsMove.BLL
{
    public class T_AdvertisingBLL
    {
        private readonly T_AdvertisingDAL dal = new T_AdvertisingDAL();

        public bool Add(T_Advertising model)
        {
            return dal.Add(model);
        }

        public bool Update(T_Advertising model)
        {
            return dal.Update(model);
        }

        public bool Delete(string serialCode)
        {
            return dal.Delete(serialCode);
        }

        public T_Advertising Find(string serialCode)
        {
            return dal.Find(serialCode);
        }

        public List<T_Advertising> FindAll ()
        {
            return dal.FindAll();
        }
    }
}
