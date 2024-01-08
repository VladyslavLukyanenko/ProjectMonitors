using System.Linq;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.Foundation.FluentValidation
{
  public class ErrorCodesPopulatorValidatorInterceptor
    : IValidatorInterceptor
  {
    public IValidationContext BeforeMvcValidation(ControllerContext controllerContext, IValidationContext commonContext)
    {
      return commonContext;
    }

    public ValidationResult AfterMvcValidation(ControllerContext controllerContext, IValidationContext commonContext,
      ValidationResult result)
    {
      if (!result.IsValid)
      {
        foreach (var errorCode in result.Errors.Select(_ => _.ErrorCode))
        {
          controllerContext.ModelState.AddModelError(errorCode, "");
        }

        return new ValidationResult();
      }

      return result;
    }
  }
}