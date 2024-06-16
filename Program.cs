using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MinimalAPI.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<DetailDto>(new DetailDto(1, "Demo", false ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// By using [FromServices] we are DI DetailDto to the endpoint.
app.MapPost("/test",([FromServices]DetailDto details, IHttpClientFactory _httpClientFactory) => {
    var client = _httpClientFactory.CreateClient();
    client.PostAsJsonAsync("https://webhook.site/a3a39914-5b85-4b38-8490-2357f9b22789",details);
    return Results.Ok();
});

app.MapPost("/demo",([FromBody]DemoDto demoDto,[FromServices]DetailDto details, HttpRequest request,IHttpClientFactory _httpClientFactory) => {
    var client = _httpClientFactory.CreateClient();
    client.PostAsJsonAsync("https://webhook.site/a3a39914-5b85-4b38-8490-2357f9b22789",demoDto);
    client.PostAsJsonAsync("https://webhook.site/a3a39914-5b85-4b38-8490-2357f9b22789",details);
    var queryString = request.Query["myParam"];

    var headers = request.Headers["MyHeader"];

    var body = request.Body;
    return Results.Ok();
}).WithName("PostDemo");

app.Run();

