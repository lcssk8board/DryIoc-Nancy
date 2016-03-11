﻿using DryIoc;
using Nancy.Hosting.Self;
using System;
using System.Diagnostics;
using Nancy.Bootstrappers.DryIoc;

namespace Nancy.SelfHosted.HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new NancyHost(new Bootstrapper(), new Uri("http://localhost:5050")))
            {
                host.Start();
                Process.Start("http://localhost:5050/home");
                Console.WriteLine("The server is listening on: localhost:1234");
                Console.ReadLine();
            }
        }
    }

    public interface ITransient
    {
        void Test123();

        IRequestScoped RequestScoped { get; }
    }

    public class Transient : ITransient
    {
        public IRequestScoped RequestScoped { get; private set; }

        public Transient(IRequestScoped requestScoped)
        {
            RequestScoped = requestScoped;
        }

        public void Test123()
        {
        }
    }

    public interface IRequestScoped { }

    public class RequestScoped : IRequestScoped
    {
    }

    public class Bootstrapper : DryIocNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(IContainer existingContainer)
        {
            existingContainer.Register<ITransient, Transient>(Reuse.Transient);
            existingContainer.Register<IRequestScoped, RequestScoped>(Reuse.InWebRequest);
            base.ConfigureApplicationContainer(existingContainer);
        }

        protected override void ConfigureRequestContainer(IContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
        }
    }
}
