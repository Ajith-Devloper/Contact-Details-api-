using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactDetails_Api.Custom_Execption_Filtter
{
    public class CustomExpextion:ExceptionFilterAttribute
    {

        public override void OnException(ExceptionContext context)
        {
            context.Result = new JsonResult(new
            {
                statuscode = 500,
                message = "Something went Wrong"
            });
            base.OnException(context);
        }





    }
}
