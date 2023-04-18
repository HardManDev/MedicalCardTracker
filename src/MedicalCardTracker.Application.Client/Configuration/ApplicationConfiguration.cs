// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Extensions.Configuration;

namespace MedicalCardTracker.Application.Client.Configuration;

public class ApplicationConfiguration : INotifyPropertyChanged
{
    private readonly string _basePath;
    private string _apiBaseUrl;
    private string _customerName;
    private bool _isRegistrar;
    private bool _isWriteLog;
    private string _targetAddress;

    public ApplicationConfiguration(string basePath)
    {
        _basePath = basePath;
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("settings.json", true, true)
            .AddCommandLine(Environment.GetCommandLineArgs())
            .Build();

        ApiBaseUrl = configuration.GetValue<string>(nameof(ApiBaseUrl)) ?? "http://localhost:5445";
        CustomerName = configuration.GetValue<string>(nameof(CustomerName)) ?? string.Empty;
        TargetAddress = configuration.GetValue<string>(nameof(TargetAddress)) ?? "каб. ";
        IsRegistrar = configuration.GetValue<bool>(nameof(IsRegistrar));
        IsWriteLog = configuration.GetValue<bool>(nameof(IsWriteLog));
    }

    public string ApiBaseUrl
    {
        get => _apiBaseUrl;
        set
        {
            if (value == _apiBaseUrl) return;
            _apiBaseUrl = value ?? throw new ArgumentNullException(nameof(value));
            OnPropertyChanged();
        }
    }

    public string CustomerName
    {
        get => _customerName;
        set
        {
            if (value == _customerName) return;
            _customerName = value ?? throw new ArgumentNullException(nameof(value));
            OnPropertyChanged();
        }
    }

    public string TargetAddress
    {
        get => _targetAddress;
        set
        {
            if (value == _targetAddress) return;
            _targetAddress = value ?? throw new ArgumentNullException(nameof(value));
            OnPropertyChanged();
        }
    }

    public bool IsRegistrar
    {
        get => _isRegistrar;
        set
        {
            if (value == _isRegistrar) return;
            _isRegistrar = value;
            OnPropertyChanged();
        }
    }

    public bool IsWriteLog
    {
        get => _isWriteLog;
        set
        {
            if (value == _isWriteLog) return;
            _isWriteLog = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public async Task SaveToJsonAsync()
    {
        var json = JsonSerializer.Serialize(this,
            new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
            });

        await File.WriteAllTextAsync(Path.Combine(_basePath, "settings.json"),
            json,
            Encoding.Unicode);
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
