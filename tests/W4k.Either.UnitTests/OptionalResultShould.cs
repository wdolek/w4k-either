namespace W4k.Either;

public class OptionalResultShould
{
    [Fact]
    public void CreateSuccessResultWithValue()
    {
        const string value = "le value";
        var result = OptionalResult.Success<string, IError>(value);

        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.False(result.IsFailed);

        Assert.Equal(value, result.Value);
        Assert.Throws<InvalidOperationException>(() => result.Error);
    }
    
    [Fact]
    public void CreateSuccessResultWithoutValue()
    {
        var result = OptionalResult.Empty<string, IError>();

        Assert.True(result.IsSuccess);
        Assert.False(result.HasValue);
        Assert.False(result.IsFailed);
        
        Assert.Equal(default, result.Value);
    }
    
    [Fact]
    public void CreateFailedResult()
    {
        var error = new TestError();
        var result = OptionalResult.Failed<string, IError>(error);

        Assert.False(result.IsSuccess);
        Assert.False(result.HasValue);
        Assert.True(result.IsFailed);

        Assert.Throws<InvalidOperationException>(() => result.Value);
        Assert.Equal(error, result.Error);
    }
    
    [Fact]
    public void DeconstructSuccessResultWithValue()
    {
        var result = OptionalResult.Success<string, IError>("le success");

        var (v, e) = result;
        
        Assert.NotNull(v);
        Assert.Null(e);
    }
    
    [Fact]
    public void DeconstructSuccessResultWithoutValue()
    {
        var result = OptionalResult.Empty<string, IError>();

        var (v, e) = result;
        
        Assert.Null(v);
        Assert.Null(e);
    }    
    
    [Fact]
    public void DeconstructFailedResultWithValue()
    {
        var result = OptionalResult.Failed<string, IError>(new TestError());

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
