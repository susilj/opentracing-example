using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Tag;

namespace opentracing_example
{
    [Route("api/[controller]")]
    public class FormatController : Controller
    {
        private readonly ITracer _tracer;

        public FormatController(ITracer tracer)
        {
            _tracer = tracer;
        }

        // GET: api/format
        [HttpGet]
        public string Get()
        {
            return "Hello!";
        }

        // GET: api/format/helloTo
        [HttpGet("{helloTo}", Name = "GetFormat")]
        public string Get(string helloTo)
        {
            Console.WriteLine(Json(Request.Headers));
            using (var scope = _tracer.BuildSpan("format-controller").StartActive(true))
            {
                var greeting = scope.Span.GetBaggageItem("greeting") ?? "Hello";
                var formattedHelloString = $"{greeting}, {helloTo}!";
                scope.Span.Log(new Dictionary<string, object>
                {
                    [LogFields.Event] = "string-format",
                    ["value"] = formattedHelloString
                });
                return formattedHelloString;
            }

            // var headers = Request.Headers.ToDictionary(k => k.Key, v => v.Value.First());
            // using (var scope = StartServerSpan(_tracer, headers, "format-controller"))
            // {
            //     var formattedHelloString = $"Hello, {helloTo}!";
            //     scope.Span.Log(new Dictionary<string, object>
            //     {
            //         [LogFields.Event] = "string-format",
            //         ["value"] = formattedHelloString
            //     });
            //     return formattedHelloString;
            // }
        }
        public static IScope StartServerSpan(ITracer tracer, IDictionary<string, string> headers, string operationName)
        {
            ISpanBuilder spanBuilder;
            try
            {
                ISpanContext parentSpanCtx = tracer.Extract(BuiltinFormats.HttpHeaders, new TextMapExtractAdapter(headers));

                spanBuilder = tracer.BuildSpan(operationName);
                if (parentSpanCtx != null)
                {
                    spanBuilder = spanBuilder.AsChildOf(parentSpanCtx);
                }
            }
            catch (Exception)
            {
                spanBuilder = tracer.BuildSpan(operationName);
            }

            // TODO could add more tags like http.url
            return spanBuilder.WithTag(Tags.SpanKind, Tags.SpanKindServer).StartActive(true);
        }
    }
}