# Either

Library providing various types (and source generator) for handling results of operations or representing discriminated union of two or more types.

## Why?

There are several ways how to handle results of operations in C#:

- you can throw exception - but that's expensive and not always appropriate, consider exception as `goto` within whole call stack
- you can return tuple - but that's not very descriptive and adds a lot of noise to the code
- you can write your own result type for every operation - but that's a lot of work and you are lazy developer

... or you can use this library.

Types included:

| Type                             | Description                                                                      |
|----------------------------------|----------------------------------------------------------------------------------|
| `Either<TLeft, TRight>`          | Represents discriminated union of two types (up to 8 types in total)             |
| `Maybe<TValue>`                  | Represents optional value                                                        |
| `Result<TError>`                 | Represents result without value                                                  |
| `Result<TValue, TError>`         | Represents result with value                                                     |
| `OptionalResult<TValue, TError>` | Represents optional result with value (consider `Result<Maybe<TValue>, TError>`) |

## How?

Common API for all types:

| Method/Property                 | Description                                                                |
|---------------------------------|----------------------------------------------------------------------------|
| ctor                            | Creates new instance of type with value (e.g. `Either(TLeft value)`)       |
|                                 |                                                                            |
| `Case` (property)               | Property to be used with pattern matching                                  | 
| `TryPick`                       | Allows you to pick value from type (if present)                            |
| `Match<TResult>`                | Handling all possible cases returning `TResult`                            |
| `Match<TState, TResult>`        | Handling all possible cases returning `TResult` using state                |
| `MatchAsync<TResult>`           | Handling all possible cases returning `TResult` asynchronously             |
| `MatchAsync<TState, TResult>`   | Handling all possible cases returning `TResult` asynchronously using state |
| `Switch`                        | Handling all possible cases (`void`)                                       |
| `Switch<TState>`                | Handling all possible cases using state                                    |
| `SwitchAsync`                   | Handling all possible cases asynchronously                                 |
| `SwitchAsync<TState>`           | Handling all possible cases asynchronously using state                     |
|                                 |                                                                            |
| `ToString`                      | Returns string representation of value                                     |
| `GetHashCode`                   | Returns hash code of value                                                 |
| `Equals`                        | Equality comparison between monads                                         |
|                                 |                                                                            |
| `==` and `!=`                   | Equality comparison between monads                                         |
|                                 |                                                                            |
| conversion, `implicit operator` | Implicit conversion from value to monad                                    |

### Either

To not propagate exception, you can use `Either` to communicate to the caller that operation ended with either value or error.

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

To communicate to the caller that operation ended up with no value, instead of returning `null`, you can use `Maybe`.

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

To avoid throwing exceptions or using tuples, you can simply indicate result of operation. This is same as `Either<TValue, TError>`
with additional properties to check state of result object.

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

Similarly to `Result`, with `OptionalResult` you can indicate result of operation with optional value. This is same as `Result<Maybe<TValue>, TError>`.

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

...

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
