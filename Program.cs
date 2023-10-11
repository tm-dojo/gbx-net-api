var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowSpecificOrigins, 
        policy =>
        {
            policy.WithOrigins("http://localhost:4200");
        });
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Init S3 for uploading generated json blocks files
S3Credentials s3Credentials = new S3Credentials();
app.Configuration.GetSection("S3").Bind(s3Credentials);
S3.Init(s3Credentials);

// Init Nadeo service for getting map informations
NadeoAccount nadeoAccount = new NadeoAccount();
app.Configuration.GetSection("NadeoAccount").Bind(nadeoAccount);
NadeoService.Init(nadeoAccount);

app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHttpLogging();

app.Run();
