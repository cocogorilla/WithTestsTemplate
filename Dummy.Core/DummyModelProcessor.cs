using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dummy.Core
{
    public interface IDummyModelProcessor
    {
        IEnumerable<MainModel> Process(string input);
    }
    public class DummyModelProcessor : IDummyModelProcessor
    {
        public IEnumerable<MainModel> Process(string input)
        {
            return new[] { new MainModel { ModelChars = input } };
        }
    }
}
