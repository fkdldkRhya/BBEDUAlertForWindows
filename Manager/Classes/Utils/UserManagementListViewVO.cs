using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Classes.Utils
{
    public class UserManagementListViewVO
    {
        public string uid { get; set; }
        public string email { get; set; }
        public string regNum { get; set; }
        public string regName { get; set; }
        public string regFounder { get; set; }
        public string loginDate { get; set; }
        public string token { get; set; }
    }
}
