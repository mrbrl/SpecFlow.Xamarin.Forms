﻿using System;
using Autofac;

namespace SpecFlow.XFormsDependency
{
    public class AutoFacResolver : IResolver
    {
        private IContainer _container;
        private readonly ContainerBuilder _builder;
        private bool _isInitialised;

        public AutoFacResolver()
        {
            _builder = new ContainerBuilder();
        }

        public AutoFacResolver(ContainerBuilder builder)
        {
            _builder = builder;
        }

        public bool IsInitialised
        {
            get { return _isInitialised; }
            set { throw new NotImplementedException(); }
        }

        public void Register<TInterface, TImplementation>(LifetimeScopeEnum lifetimeScope)
        {
            ThrowIfInitialised();

            var registration = _builder.RegisterType<TImplementation>().As<TInterface>();

            switch (lifetimeScope)
            {
                case LifetimeScopeEnum.Singleton:
                    registration.SingleInstance();
                    break;
                case LifetimeScopeEnum.InstancePerDependency:
                    registration.InstancePerDependency();
                    break;
                default:
                    throw new ArgumentException($"LifetimeScope not supported: {lifetimeScope}", nameof(lifetimeScope));
            }
        }

        public void RegisterMultiple<TImplementation>(LifetimeScopeEnum lifetimeScope, params Type[] interfaces) where TImplementation : class
        {
            ThrowIfInitialised();

            var registration = _builder.RegisterType<TImplementation>().As(interfaces);

            switch (lifetimeScope)
            {
                case LifetimeScopeEnum.Singleton:
                    registration.SingleInstance();
                    break;
                case LifetimeScopeEnum.InstancePerDependency:
                    registration.InstancePerDependency();
                    break;
                default:
                    throw new ArgumentException($"LifetimeScope not supported: {lifetimeScope}", nameof(lifetimeScope));
            }
        }

        public void RegisterGeneric(Type interfaceType, Type instanceType, LifetimeScopeEnum lifetimeScope)
        {
            ThrowIfInitialised();

            var registration = _builder.RegisterGeneric(instanceType).As(interfaceType);

            switch (lifetimeScope)
            {
                case LifetimeScopeEnum.Singleton:
                    registration.SingleInstance();
                    break;
                case LifetimeScopeEnum.InstancePerDependency:
                    registration.InstancePerDependency();
                    break;
                default:
                    throw new ArgumentException($"LifetimeScope not supported: {lifetimeScope}", nameof(lifetimeScope));
            }
        }

        public void Register<T>(LifetimeScopeEnum lifetimeScope = LifetimeScopeEnum.InstancePerDependency)
        {
            Register<T, T>(lifetimeScope);
        }

        public void Register<TInterface>(TInterface instance) where TInterface : class
        {
            ThrowIfInitialised();
            var registration = _builder.RegisterInstance(instance);
        }

        public void Update<TInterface>(TInterface instance) where TInterface : class
        {
            var builder = new ContainerBuilder();
            var registration = builder.RegisterInstance(instance);
            builder.Update(_container);
        }

        public void Update<TInterface, TImplementation>(LifetimeScopeEnum lifetimeScope)
        {
            var builder = new ContainerBuilder();
            var registration = builder.RegisterType<TImplementation>().As<TInterface>();

            switch (lifetimeScope)
            {
                case LifetimeScopeEnum.Singleton:
                    registration.SingleInstance();
                    break;
                case LifetimeScopeEnum.InstancePerDependency:
                    registration.InstancePerDependency();
                    break;
                default:
                    throw new ArgumentException($"LifetimeScope not supported: {lifetimeScope}", nameof(lifetimeScope));
            }

            builder.Update(_container);
        }

        public void RegisterMultiple<TImplementation>(TImplementation instance, LifetimeScopeEnum lifetimeScope, params Type[] interfaces) where TImplementation : class
        {
            ThrowIfInitialised();

            var registration = _builder.RegisterInstance(instance).As(interfaces);

            switch (lifetimeScope)
            {
                case LifetimeScopeEnum.Singleton:
                    registration.SingleInstance();
                    break;
                case LifetimeScopeEnum.InstancePerDependency:
                    registration.InstancePerDependency();
                    break;
                default:
                    throw new ArgumentException($"LifetimeScope not supported: {lifetimeScope}", nameof(lifetimeScope));
            }
        }

        public void UpdateMultiple<TImplementation>(TImplementation instance, LifetimeScopeEnum lifetimeScope, params Type[] interfaces) where TImplementation : class
        {
            var builder = new ContainerBuilder();
            var registration = _builder.RegisterInstance(instance).As(interfaces);

            switch (lifetimeScope)
            {
                case LifetimeScopeEnum.Singleton:
                    registration.SingleInstance();
                    break;
                case LifetimeScopeEnum.InstancePerDependency:
                    registration.InstancePerDependency();
                    break;
                default:
                    throw new ArgumentException($"LifetimeScope not supported: {lifetimeScope}", nameof(lifetimeScope));
            }

            builder.Update(_container);
        }

        public void Initialise()
        {
            _container = _builder.Build();
            _isInitialised = true;
        }

        public T GetContainer<T>()
        {
            return (T)_container;
        }

        public T Resolve<T>() where T : class
        {
            ThrowIfNotInitialised();
            return _container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            ThrowIfNotInitialised();
            return _container.Resolve(type);
        }

        private void ThrowIfInitialised()
        {
            if (_isInitialised)
                throw new Exception("Resolver has already been initialised - you can't register components after Initialise() has been called.");
        }

        private void ThrowIfNotInitialised()
        {
            if (!_isInitialised)
                throw new Exception("Resolver has not been initialised - please invoke Initialise() first.");
        }
    }
}
