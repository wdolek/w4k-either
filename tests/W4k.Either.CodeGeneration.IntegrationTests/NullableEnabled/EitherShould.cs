using W4k.Either.Abstractions;

namespace W4k.Either.CodeGeneration.IntegrationTests.NullableEnabled;

[Either]
public readonly partial struct Either<TNonNullableRef, TNullableRef, TNotNull, TValue, TAny>
    where TNonNullableRef : class
    where TNullableRef : class?
    where TNotNull : notnull
    where TValue : struct
{
    public byte State => _idx;
}

public class EitherShould
{
    [Fact]
    public void InstantiateInvalidStateUsingDefaultCtor()
    {
        var either = new Either<Huey, Dewey?, Louie, Duckula, Scrooge?>();
        Assert.Equal(0, either.State);
    }
    
    [Fact]
    public void PreventNullForNonNullableArgs()
    {
        // non-nullable ref type
        Assert.Throws<ArgumentNullException>(() => new Either<Huey, Dewey?, Louie, Duckula, Scrooge?>((Huey)null!));

        // notnull constraint
        Assert.Throws<ArgumentNullException>(() => new Either<Huey, Dewey?, Louie, Duckula, Scrooge?>((Louie)null!));
    }
    
    [Fact]
    public void AllowNullForNullableArgs()
    {
        // class? constraint
        _ = new Either<Huey, Dewey?, Louie, Duckula, Scrooge?>((Dewey?)null);

        // unconstrained
        _ = new Either<Huey, Dewey?, Louie, Duckula, Scrooge?>((Scrooge?)null);
    }
}

public class Huey
{
}

public class Dewey
{
}

public class Louie
{
}

public class Scrooge
{
}

public struct Duckula
{
}
