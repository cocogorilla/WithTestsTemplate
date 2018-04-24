using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dummy.Core;

namespace Dummy.Infrastructure
{
    public class ModelRepo : IModelRepo
    {
        private readonly AppConfig _config;

        public ModelRepo(AppConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            _config = config;
        }
        public async Task PostData(IEnumerable<MainModel> input)
        {
            using (var conn = new SqlConnection(_config.ConnectionString))
            {
                foreach (var val in input)
                {
                    conn.Execute("insert into DummyModel (ModelChars) values (@input)", new
                    {
                        input = val.ModelChars
                    });
                }
            }
        }
    }
}
