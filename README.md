# w4k-either
Yet another choice monad implementation

## Overview of type categories and usage

|                               | reference type | nullable reference type | value type | nullable value type | unspecified |
|-------------------------------|----------------|-------------------------|------------|---------------------|-------------|
| ctor/method parameter         | T              | T?                      | T          | T?                  | T?          |
| field                         | T              | T?                      | T          | T?                  | T?          |
| typecasting                   | T              | T?                      | T          | T?                  | T?          |
| type category constraint      | class          | class?                  | struct     | N/A                 | default     |
| nullability constraint        | notnull        | N/A                     | notnull    | N/A                 | -           |

## Implementing `Either`

Since `Either` is a choice monad, we need to store values in "nullable" fields. That means that we still use `?` 
for reference type with `notnull` constraint. Only case where we may want to omit `?` is for non-nullable struct.

### Passing _current_ value

Consider value is stored in `_v1` field (and we use `T?` for type - since we don't know in which state monad is):

| type category | nullability | referencing field | description                                    |
|---------------|-------------|-------------------|------------------------------------------------|
| reference     | notnull     | `_v1!`            | we need to use null forgiving operator (`!`)   |
| reference     | nullable    | `_v1`             | value may be null, no need for `!`             |
| value         | notnull     | `_v1`             | value is struct, no need for `!`               |
| value         | nullable    | `_v1`             | value is struct `Nullable<T>`, no need for `!` |

### Implementing `Either.TryPick`

Consider value is present and we call `TryPick` method:

| type category | nullability | method signature                                 | description                                      |
|---------------|-------------|--------------------------------------------------|--------------------------------------------------|
| reference     | notnull     | `bool TryPick([NotNullWhen(true)] out T? value)` | if we pick value, we know that value is not null |
| reference     | nullable    | `bool TryPick(out T? value)`                     | value may be null                                | 
| value         | notnull     | `bool TryPick(out T value)`                      | value is struct, can't be null                   |
| value         | nullable    | `bool TryPick(out T? value)`                     | value is struct `Nullable<T>`, can't be null     |
