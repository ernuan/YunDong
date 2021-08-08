using CloudsMove.DAL;
using CloudsMove.ViewModels;
using System.Collections.Generic;

namespace CloudsMove.BLL
{
    public class T_ShadowsocksRBLL
    {
        private readonly T_ShadowsocksRDAL dal = new T_ShadowsocksRDAL();

        public bool Add(T_ShadowsocksR model)
        {
            return dal.Add(model);
        }

        public bool Update(T_ShadowsocksR model)
        {
            return dal.Update(model);
        }

        public bool Delete(string serialCode)
        {
            return dal.Delete(serialCode);
        }

        public T_ShadowsocksR Find(string serialCode)
        {
            return dal.Find(serialCode);
        }

        public List<T_ShadowsocksR> FindAll()
        {
            return dal.FindAll();
        }
    }
}
