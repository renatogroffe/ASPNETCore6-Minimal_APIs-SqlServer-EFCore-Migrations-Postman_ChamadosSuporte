using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FluentValidation;
using APISuporte.Data;
using APISuporte.Endpoints;
using APISuporte.Models;
using APISuporte.Services;
using APISuporte.Validations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SuporteContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("BaseSuporte"))
);
builder.Services.AddScoped<ChamadosService>();

builder.Services.AddTransient<IValidator<RequisicaoSuporte>, RequisicaoSuporteValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "APISuporte",
            Description = "API REST para controle de chamados de suporte", 
            Version = "v1",
            Contact = new OpenApiContact()
            {
                Name = "Renato Groffe",
                Url = new Uri("https://github.com/renatogroffe"),
            },
            License = new OpenApiLicense()
            {
                Name = "MIT",
                Url = new Uri("http://opensource.org/licenses/MIT"),
            }
        });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapEndpointsChamados();

app.Run();