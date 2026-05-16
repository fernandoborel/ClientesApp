using ClientesApp.API.Contexts;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins(builder.Configuration.GetValue<string>("AllowedOrigins") ?? "http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer(); //Swagger
builder.Services.AddSwaggerGen(); //Swagger

var app = builder.Build();

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            message = "Ocorreu um erro interno. Tente novamente mais tarde."
        });
    });
});

app.UseCors("AllowFrontend");

app.UseSwagger();
app.UseSwaggerUI(); //Swagger

//Scalar
app.MapScalarApiReference(options =>
{
    options.WithTheme(ScalarTheme.BluePlanet);
});

app.MapOpenApi();

app.UseAuthorization();
app.MapControllers();
app.Run();