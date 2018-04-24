using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Dummy.Core;

namespace Dummy.Api
{
    [RoutePrefix("")]
    public class DummyController : ApiController
    {
        private readonly IDummyModelProcessor _processor;
        private readonly IModelRepo _repo;

        public DummyController(IDummyModelProcessor processor, IModelRepo repo)
        {
            if (processor == null) throw new ArgumentNullException(nameof(processor));
            if (repo == null) throw new ArgumentNullException(nameof(repo));
            _processor = processor;
            _repo = repo;
        }

        [Route("ping")]
        [HttpPost]
        public async Task<IHttpActionResult> Test([FromBody] string input)
        {
            var payload = _processor.Process(input);
            await _repo.PostData(payload);
            return Ok(payload);
        }
    }
}