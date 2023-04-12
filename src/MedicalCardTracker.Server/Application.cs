// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MedicalCardTracker.Application;
using MedicalCardTracker.Application.Server.Requests;
using MedicalCardTracker.Database;

namespace MedicalCardTracker.Server;

public class Application
{
    private readonly WebApplicationBuilder _builder;
    private readonly WebApplication _app;

    public Application(string[] args)
    {
        _builder = WebApplication.CreateBuilder(args);

        ConfigureService(_builder.Services);

        _app = _builder.Build();

        if (_app.Environment.IsDevelopment())
        {
            _app.UseSwagger();
            _app.UseSwaggerUI();
        }

        _app.MapControllers();
        _app.UseAuthorization();
        _app.UseHttpsRedirection();
    }

    public void Run()
    {
        if (_app.Environment.IsDevelopment())
            _app.Run();
        else
            _app.Run(_builder.Configuration.GetValue<string>("Url"));
    }

    private void ConfigureService(IServiceCollection services)
    {
        services.AddDatabase(_builder.Configuration);
        services.AddApplication();

        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(typeof(BaseRequestHandler).Assembly));

        services.AddControllers();

        services.AddSwaggerGen();
        services.AddEndpointsApiExplorer();
    }
}
