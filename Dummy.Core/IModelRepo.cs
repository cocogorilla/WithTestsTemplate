using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dummy.Core
{
    public interface IModelRepo
    {
        Task PostData(IEnumerable<MainModel> input);
    }
}