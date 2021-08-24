using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;
using FFXIVClientStructs.SourceGenerator.Model;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;

namespace FFXIVClientStructs.StructExporter
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            if (!MSBuildLocator.IsRegistered)
            {
                var instances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
                MSBuildLocator.RegisterInstance(instances.OrderByDescending(x => x.Version).First());
            }


            var workspace = MSBuildWorkspace.Create();
            workspace.SkipUnrecognizedProjects = true;
            workspace.WorkspaceFailed += (sender, args) =>
            {
                if (args.Diagnostic.Kind == WorkspaceDiagnosticKind.Failure)
                    Console.Error.WriteLine(args.Diagnostic.Message);
            };

            var project = await workspace.OpenProjectAsync(@"FFXIVClientStructs\FFXIVClientStructs.csproj");
            if (await project.GetCompilationAsync() is not { } compilation) return;

            var structWalker = new StructWalker();
            
            foreach (var syntaxTree in compilation.SyntaxTrees)
            {
                if (syntaxTree.FilePath.Contains("generated")) continue;
                var model = compilation.GetSemanticModel(syntaxTree);
                structWalker.CurrentModel = model;

                structWalker.Visit(await syntaxTree.GetRootAsync());
            }

            await using FileStream createStream = File.Create(@"DisasmTools\structs.json");
            await JsonSerializer.SerializeAsync(createStream, structWalker.Structs, 
                new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    WriteIndented = true });
            await createStream.DisposeAsync();
        }

        class StructWalker : CSharpSyntaxWalker
        {
            public List<Struct> Structs = new();

            public SemanticModel CurrentModel;

            public override void VisitStructDeclaration(StructDeclarationSyntax node)
            {
                if (CurrentModel.GetDeclaredSymbol(node) is not { } typeSymbol) return;
                if (!typeSymbol.ContainingNamespace.ToDisplayString().StartsWith("FFXIVClientStructs.FFXIV")) return;
                var newStruct = new Struct(node, typeSymbol, CurrentModel);
                Structs.Add(newStruct);
            }
        }
    }
}