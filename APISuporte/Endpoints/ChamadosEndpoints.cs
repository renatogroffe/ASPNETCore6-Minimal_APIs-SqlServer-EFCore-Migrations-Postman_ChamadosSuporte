using FluentValidation;
using APISuporte.Services;
using APISuporte.Models;

namespace APISuporte.Endpoints;

public static class ChamadosEndpoints
{
    private const string Route = "/suporte";

    public static IApplicationBuilder MapEndpointsChamados(this WebApplication app)
    {
        app.MapGet(Route + "/{idChamado}", (int idChamado, ChamadosService chamadosService) =>
        {
            var detalhesChamado = chamadosService.Get(idChamado);
            if (detalhesChamado is not null)
            {
                app.Logger.LogInformation($"Retornando dados do Chamado {idChamado}...");
                return Results.Ok(detalhesChamado);
            }
            else
            {
                app.Logger.LogError($"Nao foram encontradas informacoes para o Chamado {idChamado}!");
                return Results.NotFound();
            }
        }).Produces<DetalhesChamado>()
          .Produces(StatusCodes.Status404NotFound);

        app.MapPost(Route, (RequisicaoSuporte requisicaoSuporte,
            IValidator<RequisicaoSuporte> validator,
            ChamadosService chamadosService) =>
        {
            var validationResult = validator.Validate(requisicaoSuporte);
            if (!validationResult.IsValid)
            {
                app.Logger.LogError($"Dados invalidos para inclusao de novo chamado...");
                return Results.ValidationProblem(validationResult.ToDictionary());
            }
            var resultado = chamadosService.Save(requisicaoSuporte);
            app.Logger.LogInformation($"Id do novo Chamado: {resultado.IdChamado}");
            return Results.Ok(resultado);
        }).Produces<ResultadoInclusao>()
          .Produces(StatusCodes.Status400BadRequest);

        return app;
    }
}