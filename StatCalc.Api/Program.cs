using StatCalc.Api.Extensions;
using StatCalc.Infrastructure.AutoMapperProfiles;
using StatCalc.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    // custom extensions
    
    builder.Services.AddDI();
    builder.Services.ApplySwaggerSettings();
    builder.Services.AddAuth(builder.Configuration);
    builder.Services.AddAutoMapper(typeof(TstProfile));
    builder.Services.ConfigurationsSetUp(builder.Configuration);
}


var app = builder.Build();
{
    var isSwaggerEnabled = app.Configuration.GetSection("EnableSwagger").Value;
    if (isSwaggerEnabled != null && bool.Parse(isSwaggerEnabled))
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StatCalc"));
    }

    app.UseMiddleware<ErrorHandlerMiddleware>();
    
    app.UseCors(cfg => cfg.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
