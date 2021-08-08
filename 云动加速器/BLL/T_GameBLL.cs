using CloudsMove.DAL;
using CloudsMove.ViewModels;
using System.Collections.Generic;

namespace CloudsMove.BLL
{
    public class T_GameBLL
    {
        private readonly T_GameDAL dal = new T_GameDAL();

        public bool Add(T_Game model)
        {
            return dal.Add(model);
        }

        public bool Update(T_Game model)
        {
            return dal.Update(model);
        }

        public bool Delete(string serialCode)
        {
            return dal.Delete(serialCode);
        }

        public T_Game Find(string serialCode)
        {
            return dal.Find(serialCode);
        }

        public List<T_Game> FindAll ()
        {
            return dal.FindAll();
        }
    }
}
