# W4k.Either

![W4k.Either Build](https://github.com/wdolek/w4k-either/workflows/Build%20and%20test/badge.svg) 
[![NuGet Version](https://img.shields.io/nuget/v/W4k.Either?label=W4k.Either)](https://www.nuget.org/packages/W4k.Either)
[![NuGet Version](https://img.shields.io/nuget/v/W4k.Either.CodeGeneration?label=W4k.Either.CodeGeneration)](https://www.nuget.org/packages/W4k.Either.CodeGeneration)
[![CodeQL](https://github.com/wdolek/w4k-either/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/wdolek/w4k-either/security/code-scanning)

## Why?

There are several ways to handle the results of operations in C#. For instance, you could:

- Throw an exception: However, this can be costly and isn't always appropriate. Exceptions can be thought of as goto commands within the entire call stack.
- Return a tuple: While this method is feasible, it is not very descriptive and can clutter your code.
- Write a unique result type for every operation: This approach requires significant effort, and we know developers prefer efficiency.

A more practical solution is to use this project.

## What?

This project is source for two packages:

- [`W4k.Either`](src/W4k.Either): Predefined types that can be used to represent discriminated union.
- [`W4k.Either.CodeGeneration`](src/W4k.Either.CodeGeneration): Provides source generator that can be used to generate your own types with custom logic/rules.

Please follow links above to get more information about each package.

Example of `Either<TLeft, TRight>`:

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

## Types provided by library

| Type                             | Description                                                             |
|----------------------------------|-------------------------------------------------------------------------|
| `Either<TLeft, TRight>`          | Discriminated union of two or more types                                |
| `Maybe<TValue>`                  | Optional value representation                                           |
| `Result<TError>`                 | Result representation without a value                                   |
| `Result<TValue, TError>`         | Result representation with a value                                      |
| `OptionalResult<TValue, TError>` | Optional result with a value (similar to Result<Maybe<TValue>, TError>) |

More details about each type can be found [here](./src/W4k.Either/README.md).

## Generating own types

You can generate your own types with custom logic/rules:

```csharp
[Either]
public readonly partial struct Gelf<TCamille, TCat, TCrichton>
{
}
```

Please follow [this](./src/W4k.Either.CodeGeneration/README.md) link to get more information.

## Alternative/similar packages

Listed alphabetically

- [Ardalis.Result](https://github.com/ardalis/result): A simple implementation of Result for C#
- [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions/): Functional extensions for C#
- [dunet](https://github.com/domn1995/dunet): Simple source generator for discriminated unions in C#
- [ErrorOr](https://github.com/amantinband/error-or): A simple, fluent discriminated union of an error or a result
- [FluentResults](https://github.com/altmann/FluentResults): A lightweight .NET library to handle errors and failures in a fluent way
- [LanguageExt](https://github.com/louthy/language-ext): Provides functional-programming "base class" library
- [Nut.Result](https://github.com/Archway-SharedLib/Nut.Results): Provides an object in .NET that represents the result of a simple process
- [OneOf](https://github.com/mcintyre321/OneOf): Discriminated unions for C#
- [Optional](https://github.com/nlkl/Optional): A robust option/maybe type for C#
- [Result.Net](https://github.com/YoussefSell/Result.Net): A simple wrapper over an operation execution results to indicate success or failure
- [SimpleResults](https://github.com/MrDave1999/SimpleResults): A simple library to implement the Result pattern for returning from services

## Additional resources

Technical notes describing possible states of monad regarding to C# type system, [here](./docs/notes.md).

---

[Shapes and symbols icons](https://www.flaticon.com/free-icons/shapes-and-symbols) created by [Freepik](https://www.flaticon.com/authors/freepik) - [Flaticon](https://www.flaticon.com/)
