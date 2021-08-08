using CloudsMove.DAL;
using CloudsMove.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudsMove.BLL
{
    public class T_SettingsBLL
    {
        private readonly T_SettingsDAL dal = new T_SettingsDAL();

        public bool Add(T_Settings model)
        {
            return dal.Add(model);
        }

        public bool Update(T_Settings model)
        {
            return dal.Update(model);
        }

        public T_Settings Find()
        {
            return dal.Find();
        }
    }
}
