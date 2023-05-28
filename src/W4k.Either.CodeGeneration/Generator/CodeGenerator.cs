﻿namespace W4k.Either.CodeGeneration.Generator;

internal class CodeGenerator
{
    private readonly GeneratorContext _context;
    private readonly IMemberCodeGenerator[] _memberGenerators;

    public CodeGenerator(GeneratorContext context)
    {
        _context = context;
        _memberGenerators = new IMemberCodeGenerator[]
        {
            new FieldsGenerator(context),
            new ConstructorsGenerator(context),
            new SerializableGenerator(context),
            new PropertiesGenerator(context),
            new OperatorsGenerator(context),
            new ObjectOverridesGenerator(context),
            new EquatableGenerator(context),
            new TryPickGenerator(context),
            new MatchGenerator(context),
            new SwitchGenerator(context),
        };
    }
    
    public void Generate(IndentedWriter writer)
    {
        GenerateFileHeader(writer);
        GenerateUsing(writer);
        GenerateNamespace(writer);
    }

    private static void GenerateFileHeader(IndentedWriter writer)
    {
        writer.AppendIndentedLine("// <auto-generated />");
        writer.AppendLineBreak();
        writer.AppendIndentedLine("#nullable enable");
        writer.AppendLineBreak();
    }

    private static void GenerateUsing(IndentedWriter writer)
    {
        writer.AppendIndentedLine("using System;");
        writer.AppendIndentedLine("using System.Diagnostics.CodeAnalysis;");
        writer.AppendIndentedLine("using System.Diagnostics.Contracts;");
        writer.AppendIndentedLine("using System.Runtime.Serialization;");
        writer.AppendIndentedLine("using System.Threading;");
        writer.AppendIndentedLine("using System.Threading.Tasks;");
        writer.AppendIndentedLine("using W4k.Either;");
        writer.AppendLineBreak();
    }    
    
    private void GenerateNamespace(IndentedWriter writer)
    {
        writer.AppendIndentedLine($"namespace {_context.TypeDeclaration.TargetNamespace}");
        writer.AppendIndentedLine("{");
        
        GenerateContainingType(writer.Indent());
        
        writer.AppendIndentedLine("}");
    }

    private void GenerateContainingType(IndentedWriter writer)
    {
        if (_context.ContainingTypeDeclaration is null)
        {
            GenerateType(writer);
            return;
        }
        
        writer.AppendIndentedLine(_context.ContainingTypeDeclaration.FullDeclaration);
        writer.AppendIndentedLine("{");

        GenerateType(writer.Indent());
        
        writer.AppendIndentedLine("}");
    }

    private void GenerateType(IndentedWriter writer)
    {
        writer.AppendIndentedLine(_context.TypeDeclaration.FullDeclaration);
        writer.AppendIndentedLine("{");
        
        var indentedWriter = writer.Indent();
        foreach (var generator in _memberGenerators)
        {
            if (generator.CanGenerate())
            {
                generator.Generate(indentedWriter);
            }
        }
        
        writer.AppendIndentedLine("}");
    }
}