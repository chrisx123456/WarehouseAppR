
using AutoMapper;
using WarehouseAppR.Server.Exceptions;

namespace WarehouseAppR.Server.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(LoginException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(ex.Message);

            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (ItemAlreadyExistsException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (QuantityTypeAndCountTypeMismatch ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (AutoMapperMappingException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync(ex.InnerException?.Message ?? "Something went wrong");

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
#if DEBUG
                await context.Response.WriteAsync(ex.Message);
#endif
#if !DEBUG
                await context.Response.WriteAsync("Something went wrong");
#endif
            }
        }
    }
}
