// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace MedicalCardTracker.Client.Utils;

public class MutexHelper : IDisposable
{
    private readonly Mutex _mutex;
    private readonly StreamReader _pipeReader;
    private readonly NamedPipeServerStream _pipeServer;
    private readonly StreamWriter _pipeWriter;

    public MutexHelper(string mutexName, string pipeName)
    {
        _mutex = new Mutex(true, mutexName, out var createdNew);

        if (!createdNew)
        {
            var pipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.Out);
            pipeClient.Connect();
            var pipeWriter = new StreamWriter(pipeClient);
            pipeWriter.WriteLine("NewInstanceStarted");
            pipeWriter.Flush();
            pipeClient.Close();

            Environment.Exit(0);
        }

        _pipeServer = new NamedPipeServerStream(pipeName);
        _pipeReader = new StreamReader(_pipeServer);
        _pipeWriter = new StreamWriter(_pipeServer);

        new Thread(() =>
        {
            while (true)
            {
                _pipeServer.WaitForConnection();
                var message = _pipeReader.ReadLine();
                if (message == "NewInstanceStarted")
                    DuplicateInstanceStartup?.Invoke(this, EventArgs.Empty);
                _pipeServer.Disconnect();
            }
        }).Start();
    }

    public void Dispose()
    {
        _mutex?.ReleaseMutex();
        _pipeReader?.Dispose();
        _pipeWriter?.Dispose();
        _pipeServer?.Dispose();
    }

    public event EventHandler? DuplicateInstanceStartup;
}
