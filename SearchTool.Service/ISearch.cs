using SearchTool.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTool.Service
{
    public interface ISearch
    {
        Task<Result> GetResult(string keyword);
    }
}
