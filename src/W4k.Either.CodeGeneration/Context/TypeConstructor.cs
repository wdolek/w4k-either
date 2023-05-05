namespace W4k.Either.CodeGeneration.Context;

internal class TypeConstructor
{
    public static TypeConstructor Parameterless { get; } = new(true, string.Empty); 
    
    private TypeConstructor(bool isParameterless, string paramTypeName)
    {
        IsParameterless = isParameterless;
        ParamTypeName = paramTypeName;
    }

    public bool IsParameterless { get; }
    public string ParamTypeName { get; }
    
    public static TypeConstructor WithParameter(string paramTypeName) => new(false, paramTypeName);
}
