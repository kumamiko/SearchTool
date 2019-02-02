using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchTool.Service.Models;

namespace SearchTool.Service
{
    public class Hujiang : ISearch
    {
        private string _cookies;

        public string Cookies
        {
            get { return _cookies; }
            set { _cookies = value; }
        }

        public Task<Result> GetResult(string keyword)
        {
            
        }
    }
}
