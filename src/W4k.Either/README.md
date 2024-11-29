# W4k.Either

![W4k.Either Build](https://github.com/wdolek/w4k-either/workflows/Build%20and%20test/badge.svg)

This library provides a variety of types for handling the results of operations or representing discriminated unions of two or more types.

## Installation

To install the library, add the following package reference to your project file:

```shell
dotnet add package W4k.Either
```

## Description

Types provided by this library are intended to be used as return types of methods and functions.
They are designed to be used in functional programming style, but can be used in any other style as well.

| Type                                                 | Description                                                             |
|------------------------------------------------------|-------------------------------------------------------------------------|
| [`Either<TLeft, TRight>`](#either)                   | Discriminated union of two or more types                                |
| [`Maybe<TValue>`](#maybe)                            | Optional value representation                                           |
| [`Result<TError>`](#result)                          | Result representation without a value                                   |
| [`Result<TValue, TError>`](#result)                  | Result representation with a value                                      |
| [`OptionalResult<TValue, TError>`](#optional_result) | Optional result with a value (similar to Result<Maybe<TValue>, TError>) |

All types are generated using [`W4k.Either.CodeGeneration` package](https://www.nuget.org/packages/W4k.Either.CodeGeneration/).

## Usage

Common API for all provided types:

| Method/Property                 | Description                                                                                                                                          |
|---------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------|
| constructor                     | Creates a new instance of the type with a value                                                                                                      |
|                                 |                                                                                                                                                      |
| `Case` (property)               | Property for use with pattern matching (returns current value as `object`)                                                                           |
| `TryPick`                       | Allows retrieval of value from the type (if present)                                                                                                 |
| `Bind<TNew>` *                  | Applies the provided function to the current value, resulting in a new instance of monad with `TNew` value                                           |
| `Bind<TState, TNew>` *          | Applies the provided function to the current value, resulting in a new instance of monad with `TNew` value using a state                             |
| `Map<TNew>` **                  | Transforms current value using the supplied function, if the value exists, and wraps the result in a new instance of monad with `TNew`               |
| `Map<TState, TNew>` **          | Transforms current value using the supplied function, if the value exists, and wraps the result in a new instance of monad with `TNew` using a state |
| `Match<TResult>`                | Handles all possible cases, returning `TResult`                                                                                                      |
| `Match<TState, TResult>`        | Handles all possible cases returning `TResult` using a state                                                                                         |
| `MatchAsync<TResult>`           | Asynchronously handles all possible cases returning `TResult`                                                                                        |
| `MatchAsync<TState, TResult>`   | Asynchronously handles all possible cases returning `TResult` using a state                                                                          |
| `Switch`                        | Handles all possible cases (`void`)                                                                                                                  |
| `Switch<TState>`                | Handles all possible cases using a state                                                                                                             |
| `SwitchAsync`                   | Asynchronously handles all possible cases                                                                                                            |
| `SwitchAsync<TState>`           | Asynchronously handles all possible cases using a state                                                                                              |
|                                 |                                                                                                                                                      |
| `ToString`                      | Returns the string representation of the value                                                                                                       |
| `GetHashCode`                   | Returns the hash code of the value                                                                                                                   |
| `Equals`                        | Compares equality between monads                                                                                                                     |
|                                 |                                                                                                                                                      |
| `==` and `!=`                   | Compares equality between monads                                                                                                                     |
|                                 |                                                                                                                                                      |
| conversion, `implicit operator` | Enables implicit conversion from a value to a monad                                                                                                  |

\* `Bind` allows for chaining of operations where the result of one operation can affect subsequent ones.

\** `Map` is used for pure transformation of the value without affecting the flow of operations.

<a id="either"></a>
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

For `Either<TLeft, TRight>`, there are additional properties for inspecting the state:

| Method/Property | Description                                                    |
|-----------------|----------------------------------------------------------------|
| `IsLeft`        | Returns `true` if value is of type `TLeft`, otherwise `false`  |
| `IsRight`       | Returns `true` if value is of type `TRight`, otherwise `false` |

<a id="maybe"></a>
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

<a id="result"></a>
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

// ... or if you prefer errlang - I mean Go
var (user, err) = GetUser(userId);
if (err != null)
{
    Environment.Exit(1);
}

Console.WriteLine($"User found: {user.Name}");
```

| Method/Property                                 | Description                                                     |
|-------------------------------------------------|-----------------------------------------------------------------|
| `IsSuccess`                                     | Returns `true` if value is present, otherwise `false`           |
| `IsFailed`                                      | Returns `true` if error is present, otherwise `false`           |
| `Value`                                         | Returns value if present, otherwise throws exception            |
| `Error`                                         | Returns error if present, otherwise throws exception            |
|                                                 |                                                                 |
| `Deconstruct`                                   | Deconstructs the result into two variables: `value` and `error` |
|                                                 |                                                                 |
| `static Result.Success<TError>()`               | Creates new instance of `Result<TError>` without value          |
| `static Result.Success<TValue, TError>(TValue)` | Creates new instance of `Result<TValue, TError>` with value     |
| `static Result.Failed<TError>(TError)`          | Creates new instance of `Result<TError>` with error             |
| `static Result.Failed<TValue, TError>(TError)`  | Creates new instance of `Result<TValue, TError>` with error     |

<a id="optional_result"></a>
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

| Method/Property                                         | Description                                                                 |
|---------------------------------------------------------|-----------------------------------------------------------------------------|
| `IsSuccess`                                             | Returns `true` if error is **not** present, otherwise `false`               |
| `HasValue`                                              | Returns `true` if value is present (implies `IsSuccess`), otherwise `false` |
| `IsFailed`                                              | Returns `true` if error is present, otherwise `false`                       |
| `Value`                                                 | Returns value if present, otherwise throws exception                        |
| `Error`                                                 | Returns error if present, otherwise throws exception                        |
|                                                         |                                                                             |
| `Deconstruct`                                           | Deconstructs the result into two variables: `value` and `error`             |
|                                                         |                                                                             |
| `static OptionalResult.Success<TError, TError>(TValue)` | Creates new instance of `OptionalResult<TValue, TError>` with value         |
| `static OptionalResult.Empty<TValue, TError>()`         | Creates new instance of `OptionalResult<TValue, TError>` without value      |
| `static OptionalResult.Failed<TValue, TError>(TError)`  | Creates new instance of `OptionalResult<TValue, TError>` with error         |

---

[Shapes and symbols icons](https://www.flaticon.com/free-icons/shapes-and-symbols) created by [Freepik](https://www.flaticon.com/authors/freepik) - [Flaticon](https://www.flaticon.com/)
