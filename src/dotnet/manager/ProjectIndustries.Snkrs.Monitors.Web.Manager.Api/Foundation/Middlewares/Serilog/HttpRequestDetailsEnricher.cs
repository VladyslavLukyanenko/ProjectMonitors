// using System;
// using Microsoft.AspNetCore.Http;
//
// namespace Resolff.Olympus.Web.Foundation.Middlewares.Serilog
// {
//   public class HttpRequestDetailsEnricher : ILogEventEnricher
//   {
//     public const string HttpRequestClientHostIPPropertyName = "HttpRequestClientHostIP";
//     public const string UserNamePropertyName = "UserName";
//     public const string HttpRequestUrlReferrerPropertyName = "HttpRequestUrlReferrer";
//
//     private readonly HttpContext _httpContext;
//
//     public HttpRequestDetailsEnricher(HttpContext httpContext)
//     {
//       _httpContext = httpContext;
//     }
//
//     public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
//     {
//       if (logEvent == null)
//       {
//         throw new ArgumentNullException(nameof(logEvent));
//       }
//
//       AddClientIpAddressProperty(logEvent);
//       AddUserNameProperty(logEvent);
//       AddRefererProperty(logEvent);
//     }
//
//     private void AddRefererProperty(LogEvent logEvent)
//     {
//       var requestUrlReferrer = _httpContext.Request.Headers["Referer"].ToString();
//       var httpRequestUrlReferrerProperty =
//         new LogEventProperty(HttpRequestUrlReferrerPropertyName, new ScalarValue(requestUrlReferrer));
//       logEvent.AddPropertyIfAbsent(httpRequestUrlReferrerProperty);
//     }
//
//     private void AddUserNameProperty(LogEvent logEvent)
//     {
//       var userNameProperty = new LogEventProperty(UserNamePropertyName,
//         new ScalarValue(_httpContext.User.Identity.Name ?? "[[Anonymous]]"));
//       logEvent.AddPropertyIfAbsent(userNameProperty);
//     }
//
//     private void AddClientIpAddressProperty(LogEvent logEvent)
//     {
//       var httpRequestClientHostIPProperty = new LogEventProperty(HttpRequestClientHostIPPropertyName,
//         new ScalarValue(_httpContext.Connection.RemoteIpAddress));
//       logEvent.AddPropertyIfAbsent(httpRequestClientHostIPProperty);
//     }
//   }
// }