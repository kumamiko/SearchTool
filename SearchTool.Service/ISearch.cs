using SearchTool.Data;
using System.Threading.Tasks;

namespace SearchTool.Service
{
    public interface ISearch
    {
        Task<Result> GetResult(string keyword);
    }
}
