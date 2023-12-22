﻿using ShopV2.Exceptions;
using ShopV2.Objects.Models;
using System.Data.Common;
using System.Security;

namespace ShopV2.Middleware;

public class ErrorChecking
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorChecking> _logger;

    public ErrorChecking(RequestDelegate next, ILogger<ErrorChecking> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await this._next(context);
        }
        catch (Exception e)
        {
            string message;
            int statusCode;

            switch (e)
            {
                case NotImplementedException:
                    message = "Not implemented";
                    statusCode = StatusCodes.Status501NotImplemented;
                    break;
                case SecurityException:
                    message = "Authentication error";
                    statusCode = StatusCodes.Status401Unauthorized;
                    break;
                case NullReferenceException:
                    message = "Null reference caught";
                    statusCode = StatusCodes.Status404NotFound;
                    break;
                case ArgumentException:
                    message = "Argument is incorrect";
                    statusCode = StatusCodes.Status400BadRequest;
                    break;
                case NotFoundException:
                    message = "Entity not found";
                    statusCode = StatusCodes.Status404NotFound;
                    break;
                case DbException:
                    message = "Database error";
                    statusCode = StatusCodes.Status500InternalServerError;
                    break;
                default:
                    message = "General error";
                    statusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            ErrorViewModel errorMessage = new(message, statusCode, e);
            await UpdateContextAndLog(errorMessage, context);
        }
    }

    private async Task UpdateContextAndLog(ErrorViewModel errorMessage, HttpContext context)
    {
        context.Response.StatusCode = errorMessage.StatusCode;

        _logger.LogError("{Message}", errorMessage.Message);
        await context.Response.WriteAsJsonAsync<ErrorViewModel>(errorMessage);
    }
}
