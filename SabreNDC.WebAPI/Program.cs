using SabreNDC.WebAPI.DependencyExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServices();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapAllEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
