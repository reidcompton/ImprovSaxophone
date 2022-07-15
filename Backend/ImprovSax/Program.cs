var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("remote",
        builder => builder.WithOrigins("https://improvsax.reidcompton.com"));
    options.AddPolicy("local",
        builder => builder.WithOrigins("http://localhost:3000"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("remote");
app.UseCors("local");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
