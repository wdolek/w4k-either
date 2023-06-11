﻿# W4k.Either.CodeGeneration

:warning: Code generator - despite targeting `netstandard2.0` - requires project to use at least `net6.0` (due to features used in generated code).

All types distributed with `W4k.Either` package are generated using _this_ source generator.
To see how to use those types and what is being generated, please refer to [`W4k.Either`](https://www.nuget.org/packages/W4k.Either/) package.

The `W4k.Either.CodeGeneration` package enables you to generate your own types with custom logic. It generates types in two ways:

## Generic type

Whenever you need another type with different properties representing several different things, you can declare generic
type using `EitherAttribute`. Compiler will generate rest of type for you:

```csharp
[Either]
public readonly struct Gelf<TCamille, TCat, TCrichton>
{
}
```

## Predefined types

In case you know what types you want to use, you can specify them using `EitherAttribute` following way:

```csharp
[Either(typeof(string), typeof(int))]
public readonly struct StringOrInt
{
}
```

## Installation

Reference `W4k.Either.CodeGeneration` package in your project and use generated types:

```xml
  <ItemGroup>
    <PackageReference Include="W4k.Either.CodeGeneration" Version="0.0.0" PrivateAssets="All" />
  </ItemGroup>
```

## Usage

### Decorating your types

Declare your type as `partial` and decorate it with `EitherAttribute`. Remember to mark the containing type as `partial`
as well if the type is nested.

Please adhere to these rules:

- The type itself **MUST** be `partial`.
- If the type is nested, the containing type **MUST** be partial as well.
- There must be type defined either as generic type or as predefined type using attribute.

Other properties and behaviors include:

- When declaring a generic type, it is up to you to define the type parameter constraint.
- When implementing `IEquatable<T>`, the `Equals` and `GetHashCode` methods are automatically generated for you, unless you have implemented them yourself.
- When implementing `ISerializable`, the serializable constructor and the `GetObjectData` method are automatically generated for you, unless you have implemented them yourself.
- You are allowed to declare constructors matching those which would be generated. In that case, the constructors are not generated to avoid causing conflicts.

```csharp
[Either]
[Serializable] // <- make type serializable
public readonly partial struct Polymorph<T1, T2> : IEquatable<Polymorph<T1, T2>>, ISerializable // <- `IEquatable<>` and `ISerializable` are implemented by code generator for you 
    where T1 : struct // <- generator reflects constraints in generated code
    where T2 : notnull, ICrewMember
{
    // you can declare constructor with same signature as generator would normaly produce (generator will skip it)
    public Polymorph(T2 value)
    {
        // NB! you are responsible for checking input - if you care about it ¯\_(ツ)_/¯
        ArgumentNullException.ThrowIfNull(value);
    
        // your custom logic
        if (value is Lister)
        {
            throw new ArgumentException("...");
        }
        
        // NB! you are responsible for proper initialization of the instance;
        _idx = 2;
        _v1 = default;
        _v2 = value;
    }
    
    // custom property
    public bool IsRimmer => _idx == 2 && _v2 is Rimmer;
}
```

Notice that value fields starts at `1`, as well as state index value:

- State is stored in field `_idx`:
    - `0` is reserved for invalid state (as this is default value)
    - `1`, `2`, ..., `n` are used as state index (up to `255`)
- Values are indexed from `1` as well corresponding to state index (`_idx = 1` means that value is kept in field `_v1`)

You can declare any constructor you want, but keep in mind you need to initialize the monad properly - always set `_idx` and `_v*` fields!