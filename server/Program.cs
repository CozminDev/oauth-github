var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("github-oauth", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://github.com/login/oauth/");
    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient("github-api", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://api.github.com");
    httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
    httpClient.DefaultRequestHeaders.Add("User-Agent", "oauth-github-cozmin");
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins("http://127.0.0.1:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("CorsPolicy");

app.Run();
