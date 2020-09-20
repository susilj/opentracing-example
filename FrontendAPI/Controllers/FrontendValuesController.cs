using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Tag;

namespace FrontendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class FrontendValuesController : ControllerBase
    {
        private readonly ITracer _tracer;

        private readonly HttpClient httpClient;


        public FrontendValuesController(
            ITracer tracer,
            HttpClient httpClient)
        {
            _tracer = tracer;

            this.httpClient = httpClient;

            this.httpClient.BaseAddress = new Uri("http://backend/api/");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            using (IScope scope = _tracer.BuildSpan("waitingForValues").StartActive(finishSpanOnDispose: true))
            {
                var response = await this.httpClient.GetStringAsync("values");

                return JsonConvert.DeserializeObject<List<string>>(response);
            }
        }
    }
}