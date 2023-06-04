# Either

This library provides a variety of types and a source generator for handling the results of operations or representing discriminated unions of two or more types.

## Why?

There are several ways to handle the results of operations in C#. For instance, you could:

- Throw an exception: However, this can be costly and isn't always appropriate. Exceptions can be thought of as goto commands within the entire call stack.
- Return a tuple: While this method is feasible, it is not very descriptive and can clutter your code.
- Write a unique result type for every operation: This approach requires significant effort, and we know developers prefer efficiency.

A more practical solution is to use this library.

Included Types:

| Type                             | Description                                                             |
|----------------------------------|-------------------------------------------------------------------------|
| `Either<TLeft, TRight>`          | Discriminated union of two types                                        |
| `Maybe<TValue>`                  | Optional value representation                                           |
| `Result<TError>`                 | Result representation without a value                                   |
| `Result<TValue, TError>`         | Result representation with a value                                      |
| `OptionalResult<TValue, TError>` | Optional result with a value (similar to Result<Maybe<TValue>, TError>) |

## How?

Common API for all types:

| Method/Property                 | Description                                                                 |
|---------------------------------|-----------------------------------------------------------------------------|
| ctor                            | Creates a new instance of the type with a value                             |
|                                 |                                                                             |
| `Case` (property)               | Property for use with pattern matching                                      | 
| `TryPick`                       | Allows retrieval of value from the type (if present)                        |
| `Match<TResult>`                | Handles all possible cases, returning `TResult`                             |
| `Match<TState, TResult>`        | Handles all possible cases returning `TResult` using a state                |
| `MatchAsync<TResult>`           | Asynchronously handles all possible cases returning `TResult`               |
| `MatchAsync<TState, TResult>`   | Asynchronously handles all possible cases returning `TResult` using a state |
| `Switch`                        | Handles all possible cases (`void`)                                         |
| `Switch<TState>`                | Handles all possible cases using a state                                    |
| `SwitchAsync`                   | Asynchronously handles all possible cases                                   |
| `SwitchAsync<TState>`           | Asynchronously handles all possible cases using a state                     |
|                                 |                                                                             |
| `ToString`                      | Returns the string representation of the value                              |
| `GetHashCode`                   | Returns the hash code of the value                                          |
| `Equals`                        | Compares equality between monads                                            |
|                                 |                                                                             |
| `==` and `!=`                   | Compares equality between monads                                            |
|                                 |                                                                             |
| conversion, `implicit operator` | Enables implicit conversion from a value to a monad                         |

### Either

Use `Either` to communicate the result of an operation without propagating an exception.

```csharp
Either<User, FetchError> GetUser(UserIdentifier userId)
{
    try
    {
        // ...
        return new User(...);
    }
    catch (Exception ex)
    {
        return new FetchError(ex);
    }
}
```
```csharp
var result = GetUser(userId);
var message = result.Match(
    user => $"User {user.Name} fetched",
    error => $"Error {error.Message} occured"
);
```

### Maybe

Use `Maybe` to communicate that an operation resulted in no value, as an alternative to returning `null`.

```csharp
Maybe<User> FindUser(UserIdentifier userId)
{
    // ...
    return userFound 
        ? new User(...) // or Maybe<User>.Some(new User(...))
        : Maybe<User>.None;
}
```
```csharp
var user = FindUser(...);
if (user.HasValue)
{
    Console.WriteLine($"User found: {user.Value.Name}");
}
```

| Method/Property           | Description                                           |
|---------------------------|-------------------------------------------------------|
| `HasValue`                | Returns `true` if value is present, otherwise `false` |
| `Value`                   | Returns value if present, otherwise `default`         |
|                           |                                                       |
| `static Maybe.Some<T>(T)` | Creates new instance of `Maybe<T>` with value         |
| `static Maybe.None<T>()`  | Creates new instance of `Maybe<T>` without value      |

### Result

To indicate the result of an operation without throwing exceptions or using tuples, use `Result`.
This type is the same as `Either<TValue, TError>`, but with additional properties to inspect the state of the result object.

```csharp
Result<User, FetchError> GetUser(UserIdentifier userId)
{
    try
    {
        // ...
        return new User(...);
    }
    catch (Exception ex)
    {
        return new FetchError(ex);
    }
}
```
```csharp
var result = GetUser(userId);
if (result.IsSuccess)
{
    Console.WriteLine($"User found: {result.Value.Name}");
}
else
{
    Console.WriteLine($"Error {result.Error.Message} occured");
}
```

| Property                                        | Description                                                 |
|-------------------------------------------------|-------------------------------------------------------------|
| `IsSuccess`                                     | Returns `true` if value is present, otherwise `false`       |
| `IsFailed`                                      | Returns `true` if error is present, otherwise `false`       |
| `Value`                                         | Returns value if present, otherwise throws exception        |
| `Error`                                         | Returns error if present, otherwise throws exception        |
|                                                 |                                                             |
| `static Result.Success<TError>()`               | Creates new instance of `Result<TError>` without value      |
| `static Result.Success<TValue, TError>(TValue)` | Creates new instance of `Result<TValue, TError>` with value |
| `static Result.Failed<TError>(TError)`          | Creates new instance of `Result<TError>` with error         |
| `static Result.Failed<TValue, TError>(TError)`  | Creates new instance of `Result<TValue, TError>` with error |

### OptionalResult

`OptionalResult` can indicate the result of an operation with an optional value. It's the same as `Result<Maybe<TValue>, TError>`.

```csharp
OptionalResult<User, FetchError> FindUser(UserIdentifier userId)
{
    try
    {
        // ...
        return userFound
            ? new User(...)
            : OptionalResult<User>.Empty;
    }
    catch (Exception ex)
    {
        return new FetchError(ex);
    }
}
```
```csharp
var result = FindUser(userId);
if (result.IsSuccess)
{
    if (result.HasValue)
    {
        Console.WriteLine($"User found: {result.Value.Name}");
    }
    else
    {
        Console.WriteLine("User not found");
    }
}
else
{
    Console.WriteLine($"Error {result.Error.Message} occured");
}
```

| Property                                                | Description                                                                 |
|---------------------------------------------------------|-----------------------------------------------------------------------------|
| `IsSuccess`                                             | Returns `true` if error is **not** present, otherwise `false`               |
| `HasValue`                                              | Returns `true` if value is present (implies `IsSuccess`), otherwise `false` |
| `IsFailed`                                              | Returns `true` if error is present, otherwise `false`                       |
| `Value`                                                 | Returns value if present, otherwise throws exception                        |
| `Error`                                                 | Returns error if present, otherwise throws exception                        |
|                                                         |                                                                             |
| `static OptionalResult.Success<TError, TError>(TValue)` | Creates new instance of `OptionalResult<TValue, TError>` with value         |
| `static OptionalResult.Empty<TValue, TError>()`         | Creates new instance of `OptionalResult<TValue, TError>` without value      |
| `static OptionalResult.Failed<TValue, TError>(TError)`  | Creates new instance of `OptionalResult<TValue, TError>` with error         |

## Code generator

:warning: Code generator, despite targeting `netstandard2.0`, requires project to use at least `net6.0` (due to features used in generated code).

The `W4k.Either.CodeGeneration` package enables you to generate your own types with custom logic. It generates types in two ways:

### Generic type

Whenever you need another type with different properties representing several different things, you can declare generic
type using `EitherAttribute`. Compiler will generate rest of type for you:

```csharp
[Either]
public readonly struct Gelf<TCamille, TCat, TCrichton>
{
}
```

### Predefined types

In case you know what types you want to use, you can specify them using `EitherAttribute` following way:

```csharp
[Either(typeof(string), typeof(int))]
public readonly struct StringOrInt
{
}
```

### Usage

#### Installation

Reference `W4k.Either.CodeGeneration` package in your project and use generated types:

```xml
  <ItemGroup>
    <PackageReference Include="W4k.Either.CodeGeneration" Version="0.0.0" PrivateAssets="All" />
  </ItemGroup>
```

#### Decorating your types

Declare your type as `partial` and decorate it with `EitherAttribute`. Remember to mark the containing type as `partial`
as well if the type is nested.

Please adhere to these rules:

- The type itself **MUST** be `partial`.
- If the type is nested, the containing type **MUST** be partial as well.
- There must be type defined either as generic type or as predefined type using attribute.

Other properties and behaviors include:

- When declaring a generic type, it is up to you to define the type parameter constraint.
- When implementing IEquatable<T>, the Equals and GetHashCode methods are automatically generated for you, unless you have implemented them yourself.
- When implementing ISerializable, the serializable constructor and the GetObjectData method are automatically generated for you, unless you have implemented them yourself.
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
        
        // NB! you are responsible for proper initialization of the monad;
        //     see rules below
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

## Alternative/similar packages

Listed alphabetically

- [Ardalis.Result](https://github.com/ardalis/result): A simple implementation of Result for C#
- [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions/): Functional extensions for C#
- [ErrorOr](https://github.com/amantinband/error-or): A simple, fluent discriminated union of an error or a result
- [FluentResults](https://github.com/altmann/FluentResults): A lightweight .NET library to handle errors and failures in a fluent way
- [LanguageExt](https://github.com/louthy/language-ext): Provides functional-programming "base class" library
- [Nut.Result](https://github.com/Archway-SharedLib/Nut.Results): Provides an object in .NET that represents the result of a simple process
- [OneOf](https://github.com/mcintyre321/OneOf): Discriminated unions for C#
- [Optional](https://github.com/nlkl/Optional): A robust option/maybe type for C#
- [Result.Net](https://github.com/YoussefSell/Result.Net): A simple wrapper over an operation execution results to indicate success or failure

## Additional resources

Technical notes describing possible states of monad regarding to C# type system, [here](./docs/notes.md).
