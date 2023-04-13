// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MediatR;
using MedicalCardTracker.Application;
using MedicalCardTracker.Application.Server.Requests;
using MedicalCardTracker.Database;
using MedicalCardTracker.Server.Hubs;
using MedicalCardTracker.Server.Middlewares;
using MedicalCardTracker.Server.Requests.Behaviors;
using Serilog;

namespace MedicalCardTracker.Server;

public class Application
{
    private readonly WebApplication _app;
    private readonly WebApplicationBuilder _builder;

    public Application(string[] args)
    {
        _builder = WebApplication.CreateBuilder(args);
        _builder.Host.UseSerilog();

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

        _app.MapHub<NotificationHub>("/notifications");
        _app.UseMiddleware<MachineFingerprintMiddleware>();
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
        services.AddTransient(typeof(IPipelineBehavior<,>),
            typeof(RealTimeInteractionBehavior<,>));

        services.AddSignalR();
        services.AddControllers();
        services.AddWindowsService();

        services.AddSwaggerGen();
        services.AddEndpointsApiExplorer();
    }
}
