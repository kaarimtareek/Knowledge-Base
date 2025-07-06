namespace KnowledgeBase.API.Middlewares;

public class LoggingMiddleware : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        //if the development environment is enabled, log the request details and the time taken to process the request
        if (context.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() == true)
        {
            return LogWithStopwatch(context, next);
        }

        // Log the request details
        Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");

        // Call the next middleware in the pipeline
        var result = next(context);
        Console.WriteLine($"Response: {context.Response.StatusCode}");
        return result;
        // Note: In production, you might want to use a logging framework instead of Console.WriteLine
        // log the response details
    }

    private static Task LogWithStopwatch(HttpContext context, RequestDelegate next)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        return next(context).ContinueWith(task =>
        {
            stopwatch.Stop();
            Console.WriteLine(
                $"Request: {context.Request.Method} {context.Request.Path} took {stopwatch.ElapsedMilliseconds} ms");
        });
    }
}