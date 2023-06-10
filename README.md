# W4k.Either

![W4k.Either Build](https://github.com/wdolek/w4k-either/workflows/Build%20and%20test/badge.svg) [![NuGet Badge](https://buildstats.info/nuget/W4k.Either)](https://www.nuget.org/packages/W4k.Either/) [![NuGet Badge](https://buildstats.info/nuget/W4k.Either.CodeGeneration)](https://www.nuget.org/packages/W4k.Either.CodeGeneration/)

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
