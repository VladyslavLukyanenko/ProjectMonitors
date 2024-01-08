// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
//
// namespace Resolff.Olympus.Web.Foundation.Middlewares.Serilog
// {
//   public class HttpRequestDetailsLogginMiddleware
//   {
//     private readonly RequestDelegate _next;
//
//     public HttpRequestDetailsLogginMiddleware(RequestDelegate next)
//     {
//       _next = next;
//     }
//
//     public async Task Invoke(HttpContext context)
//     {
//       using (LogContext.Push(new HttpRequestDetailsEnricher(context)))
//       {
//         await _next.Invoke(context);
//       }
//     }
//   }
// }