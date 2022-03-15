# Faster.Ioc.Zero - fastest ioc container

The goal of Faster.Ioc.Zero is to provide the fastest ioc container with zero overhead.

 ### Faster.Ioc.Zero uses the following:
   - SourceGenerators
   - Static code injection
   - Transient and Singleton lifetimes

## About

Faster.Ioc.Zero is a small ioc container with zero overhead and incredibly fast runtime speed. See benchmarks, or try it out yourself. Faster.Ioc.Zero evolved from the fact that almost all ioc containers have some major downsize which is wiring all registration together and everything is basically a lookup in a custom hashmap or dictionary. This creates an unpleasant overhead we dont want.
Faster.Ioc.Zero uses the newly added sourcegenerators implemented in .net5 and allows us to generate code ahead-Of-Time.

## How to use

1. Install nuget package Faster.Ioc.Zero to your project
```
dotnet add package Faster.Ioc.Zero
```

2. Create a ContainerBuilder which is inherited from IContainerBuilder. It's possible to have more than one ContainerBuilder in your project

``` cs

    public interface ITransient{}
	
    public interface ISingleton{}

    public class TransientOne : ITransient
    { 
        public TransientOne(ISingleton singleton){}
    }
	
	public class SingletonOne: ISingleton
	{	
		public SingletonOne(){}
	}
	
    public class ContainerBuilder : IContainerBuilder
    {
       public void Bootstrap(Builder builder)
       {
	   builder.Register<ITransient, TransientOne>();
	   builder.Register<ISingleton, SingletonOne>(Lifetime.Singleton);
       }
    }
```

4. Compile your project which will generate all the needed registrations in a static class. Generated code is located at Dependencies->Analyzers->Faster.Ioc.Zero.SourceGenerator
``` cs 
  
  var transient = Container.TransientOne;
  var singleton = Container.SingletonOne;
```

 ## Advanced
    
  ### By default Faster.Ioc.Zero will create an IEnumerable<> for each registration
```C#

   public class ContainerBuilder : IContainerBuilder
    {
       public void Bootstrap(Builder builder)
       {
	      builder.Register<ISingleton, SingletonOne>(Lifetime.Singleton);
	      builder.Register<ISingleton, SingletonTwo>(Lifetime.Singleton);
              builder.Register<ISingleton, SingletonThree>(Lifetime.Singleton);
       }
    }

	var singletons = Container.GetAllISingleton();
    
``` 
### Supports params of type IEnumerable<>, ICollections and Arrays 
### Override param while multiple concrete classes are registered. By default when multiple concrete classes are active and no override is given. the first registered concrete class is used. The 

```C#

    public interface ITransient{}
	
	public interface ISingleton{}

    public class TransientOne : ITransient
    { 
        public TransientOne(ISingleton singleton){}
    }
	
	public class SingletonOne: ISingleton
	{	
		public SingletonOne(){}
	}

   public class ContainerBuilder : IContainerBuilder
    {
       public void Bootstrap(Builder builder)
       {
	      // override registration with a custom implementation using a different concrete type
	   builder.Register<ITransient, TransientOne>(() => new TransientOne(new SingletonTwo()) );
	   builder.Register<ISingleton, SingletonOne>(Lifetime.Singleton);
           builder.Register<ISingleton, SingletonTwo>(Lifetime.Singleton);
           builder.Register<ISingleton, SingletonThree>(Lifetime.Singleton);
       }
    }

	var transientWithSingletonTwo = Container.TransientOne();
    
```
### the constructor with the largest param count is always resolved unless the resolver is overridden

## Benchmark

According to the benchmark https://github.com/danielpalme/IocPerformance/blob/main/README.md grace is one of the faster ioc frameworks. 

### Basic Features
|**Container**|**Singleton**|**Transient**|**Combined**|**Complex**|**Generics**|**IEnumerable**|
|:------------|------------:|------------:|-----------:|----------:|-----------:|--------------:|
|**Faster.Ioc.Zero**|1<br/>1|0<br/>0|11<br/>11|33<br/>33||10<br/>10|
|**[Grace 7.2.1](https://github.com/ipjohnson/Grace)**|**20**<br/>**31**|39<br/>**55**|52<br/>84|73<br/>83|50<br/>80|250<br/>210|

### Prepare
|**Container**|**Prepare And Register**|**Prepare And Register And Simple Resolve**|
|:------------|-----------------------:|------------------------------------------:|
|**Faser.Ioc.Zero**|0<br/>|0<br/>|
|**[Grace 7.2.1](https://github.com/ipjohnson/Grace)**|157<br/>|966<br/>|
