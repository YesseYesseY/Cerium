using System.Linq.Expressions;
using System.Reflection;
using Cerium;
using Cerium.Attributes;
using Cerium.Controller;
using Cerium.JsonConverters;
using Cerium.Managers;

AccountManager.Init();

var builder =  WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new DateTimeJsonConverter());
    // options.SerializerOptions.Converters.Add(new GuidJsonConverter());
});

var app = builder.Build();

var classTypes = Assembly.GetExecutingAssembly().GetTypes()
    .Where(t => t.IsClass && t.GetCustomAttribute<CeriumControllerAttribute>() is not null)
    .ToList();

foreach (var type in classTypes)
{
    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

    foreach (var method in methods)
    {
        var routeAttribute = method.GetCustomAttribute<CeriumRouteAttribute>();
        if (routeAttribute is null)
        {
            continue;
        }

        string path = routeAttribute.Path;
        string httpMethod = routeAttribute.Method;

        var paramTypes = method.GetParameters()
            .Select(p => p.ParameterType)
            .Append(method.ReturnType)
            .ToArray();

        Type delegateType = Expression.GetFuncType(paramTypes.ToArray());

        switch (httpMethod)
        {
            case "GET":
                app.MapGet(path, method.CreateDelegate(delegateType));
                break;
            case "POST":
                app.MapPost(path, method.CreateDelegate(delegateType));
                break;
        }
    }
}

app.MapFallback((HttpContext context) =>
{
    Console.WriteLine($"[{context.Request.Method}] \"{context.Request.Path.Value}\"");
    return Results.NotFound();
});

StorefrontController.Init();

Console.WriteLine("Running!");
app.Run("http://127.0.0.1:3551");