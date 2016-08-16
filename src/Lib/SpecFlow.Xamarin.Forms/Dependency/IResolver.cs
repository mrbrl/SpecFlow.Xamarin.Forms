using System;

namespace SpecFlow.XamarinForms.Dependency
{
    public interface IResolver
    {
        T Resolve<T>() where T : class;

        object Resolve(Type type);

        void Register<TInterface, TImplementation>(LifetimeScopeEnum lifetimeScope = LifetimeScopeEnum.InstancePerDependency);

        void Register<TInterface>(LifetimeScopeEnum lifetimeScope = LifetimeScopeEnum.InstancePerDependency);

        void RegisterMultiple<TImplementation>(LifetimeScopeEnum lifetimeScope, params Type[] interfaces) where TImplementation : class;

        void RegisterMultiple<TImplementation>(TImplementation instance, LifetimeScopeEnum lifetimeScope,
            params Type[] interfaces) where TImplementation : class;

        void RegisterGeneric(Type interfaceType, Type instanceType, LifetimeScopeEnum lifetimeScope);

        void Initialise();

        T GetContainer<T>();

        bool IsInitialised { get; set; }

        void Register<TInterface>(TInterface instance) where TInterface : class;

        void Update<TInterface>(TInterface instance) where TInterface : class;

        void UpdateMultiple<TImplementation>(TImplementation instance, LifetimeScopeEnum lifetimeScope,
            params Type[] interfaces) where TImplementation : class;

        void Update<TInterface, TImplementation>(LifetimeScopeEnum lifetimeScope);
    }
}
