using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace lab1.Controllers.Middlewares {
public class DbExceptionHandler : IMiddleware {
    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        try {
            await next(context);
        }
        catch (DbUpdateConcurrencyException e) {
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            await context.Response.WriteAsync("Somebody has already edited the model. Try to run the operation again");
        }
    }
}
}