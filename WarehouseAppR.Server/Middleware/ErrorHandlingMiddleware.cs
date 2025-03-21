﻿
using AutoMapper;
using System.Text.Json;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Models.DTO;

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
            catch(LoginException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDTO { Message = ex.Message }));
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDTO { Message = ex.Message }));
            }
            catch (ItemAlreadyExistsException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDTO { Message = ex.Message }));
            }
            catch (QuantityTypeAndCountTypeMismatch ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDTO { Message = ex.Message }));
            }
            catch (AutoMapperMappingException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDTO { Message = ex.InnerException?.Message ?? "Something went wrong" }));
            }
            catch(ForbiddenActionPerformedException ex) {
                context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDTO { Message = ex.Message }));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
#if DEBUG
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDTO { Message = ex.Message + " | " + ex.InnerException }));

#endif
#if !DEBUG
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDTO { Message = "Something went wrong" }));
#endif
            }
        }
    }
}
