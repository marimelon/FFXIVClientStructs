using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FFXIVClientStructs.SourceGenerator.Model
{
    public struct StaticField
    {
        public string Name { get; }
        public string Type { get; }

        public StaticField(MethodDeclarationSyntax methodDeclarationSyntax, IMethodSymbol methodSymbol,
            SemanticModel model)
        {
            Name = methodSymbol.Name;
            Type = Helpers.NormalizeTypeString(methodSymbol.ReturnType.ToDisplayString());
        }
    }
}