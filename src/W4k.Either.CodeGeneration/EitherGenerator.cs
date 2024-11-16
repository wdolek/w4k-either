using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using W4k.Either.Generator;

namespace W4k.Either;

/// <summary>
/// Either choice monad code generator.
/// </summary>
[Generator]
public class EitherGenerator : IIncrementalGenerator
{
    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<TransformationResult> toGenerate = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => IsStructOrClassDeclarationSyntax(node),
                static (ctx, ct) => Transformator.Transform(ctx, ct))
            .Where(c => c is not null)!;

        context.RegisterSourceOutput(
            toGenerate,
            static (ctx, transformationResult) => Execute(ctx, transformationResult));
    }

    private static bool IsStructOrClassDeclarationSyntax(SyntaxNode node) =>
        node is StructDeclarationSyntax or ClassDeclarationSyntax;

    private static void Execute(SourceProductionContext context, TransformationResult transformationResult)
    {
        var diagnostics = transformationResult.Diagnostics;
        if (!transformationResult.IsValid)
        {
            foreach (var diagnostic in diagnostics)
            {
                context.ReportDiagnostic(diagnostic);
            }

            return;
        }

        var generatorContext = new GeneratorContext(transformationResult);
        var generator = new CodeGenerator(generatorContext);

        var sb = new StringBuilder(4096);
        generator.Generate(new IndentedWriter(sb));

        context.AddSource(
            generatorContext.GetFileName(),
            SourceText.From(sb.ToString(), Encoding.UTF8));
    }
}