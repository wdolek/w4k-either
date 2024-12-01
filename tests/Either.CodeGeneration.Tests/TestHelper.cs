using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Either.CodeGeneration;

public static class TestHelper
{
    public static (ImmutableArray<Diagnostic> Diagnostics, string Output) GenerateSourceCode(string source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source);
        var references = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
            .Select(a => MetadataReference.CreateFromFile(a.Location))
            .Concat(
                new[]
                {
                    MetadataReference.CreateFromFile(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(EitherAttribute).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(EitherGenerator).Assembly.Location),
                });

        var compilation = CSharpCompilation.Create(
            assemblyName: "EitherTests",
            syntaxTrees: new[] { syntaxTree },
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var originalTreeCount = compilation.SyntaxTrees.Length;
        var generator = new EitherGenerator();

        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
        driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

        var trees = outputCompilation.SyntaxTrees.ToList();

        // ReSharper disable once UseIndexFromEndExpression
        var output = trees.Count != originalTreeCount
            ? trees[trees.Count - 1].ToString()
            : string.Empty;

        return (diagnostics, output);
    }
}