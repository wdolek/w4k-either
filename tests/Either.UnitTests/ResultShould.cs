namespace Either;

public class ResultShould
{
    [Fact]
    public void CreateSuccessResultWithoutValue()
    {
        var result = Result.Success<IError>();

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailed);
    }

    [Fact]
    public void CreateFailedResultWithoutValue()
    {
        var error = new TestError();
        var result = Result.Failed<IError>(error);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailed);

        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void CreateSuccessResultWithValue()
    {
        const string value = "le success";
        var result = Result.Success<string, IError>(value);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailed);

        Assert.Equal(value, result.Value);
        Assert.Throws<InvalidOperationException>(() => result.Error);
    }

    [Fact]
    public void CreateFailedResultWithValue()
    {
        var error = new TestError();
        var result = Result.Failed<string, IError>(error);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailed);

        Assert.Throws<InvalidOperationException>(() => result.Value);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void DeconstructSuccessResultWithValue()
    {
        var result = Result.Success<string, IError>("le success");

        var (v, e) = result;

        Assert.NotNull(v);
        Assert.Null(e);
    }

    [Fact]
    public void DeconstructFailedResultWithValue()
    {
        var result = Result.Failed<string, IError>(new TestError());

        var (v, e) = result;

        Assert.Null(v);
        Assert.NotNull(e);
    }

    private interface IError
    {
        public string Message { get; }
    }

    private sealed class TestError : IError
    {
        public string Message => "le error";
    }
}