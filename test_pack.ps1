dotnet clean 
dotnet clean -c Release

dotnet build -c Release

dotnet pack -c Release .\src\W4k.Either\
dotnet pack -c Release .\src\W4k.Either.CodeGeneration\
