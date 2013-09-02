﻿using System;
using Rebus.Configuration;
using Rebus.Transports.Msmq;
using Rebus.Logging;

namespace Client
{
    class Program
    {
        static void Main()
        {
            using (var adapter = new BuiltinContainerAdapter())
            {
                adapter.Handle<string>(reply => Console.WriteLine("Got reply: {0}" + Environment.NewLine, reply));

                Configure.With(adapter)
                         .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
                         .Transport(t => t.UseMsmqAndGetInputQueueNameFromAppConfig())
                         .MessageOwnership(o => o.FromRebusConfigurationSection())
                         .CreateBus()
                         .Start();

                var bus = adapter.Bus;

                Console.WriteLine("Type a greeting + ENTER to send to server" + Environment.NewLine);
                while (true)
                {
                    var greeting = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(greeting)) break;

                    bus.Send(greeting);
                }
            }
        }
    }
}
