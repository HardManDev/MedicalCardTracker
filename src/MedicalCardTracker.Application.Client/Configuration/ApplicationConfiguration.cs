// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Extensions.Configuration;

namespace MedicalCardTracker.Application.Client.Configuration;

public class ApplicationConfiguration
{
    private readonly string _basePath;

    public ApplicationConfiguration(string basePath)
    {
        _basePath = basePath;
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("settings.json", true, true)
            .AddCommandLine(Environment.GetCommandLineArgs())
            .Build();

        ApiBaseUrl = configuration.GetValue<string>(nameof(ApiBaseUrl)) ?? "http://localhost:5445/api";
        CustomerName = configuration.GetValue<string>(nameof(CustomerName)) ?? string.Empty;
        TargetAddress = configuration.GetValue<string>(nameof(TargetAddress)) ?? "каб. ";
        IsRegistrar = configuration.GetValue<bool>(nameof(IsRegistrar));
        IsWriteLog = configuration.GetValue<bool>(nameof(IsWriteLog));
    }

    public string ApiBaseUrl { get; set; }
    public string CustomerName { get; set; }
    public string TargetAddress { get; set; }
    public bool IsRegistrar { get; set; }
    public bool IsWriteLog { get; set; }

    public async Task SaveToJsonAsync(CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(this,
            new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
            });

        await File.WriteAllTextAsync(Path.Combine(_basePath, "settings.json"),
            json,
            Encoding.Unicode,
            cancellationToken);
    }
}
