using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FFXIVClientStructs.SourceGenerator.Model
{
    public class Function
    {
        public string Name { get; }
        public string ReturnType { get; }
        public List<FunctionArgument> Arguments { get; } = new();
        public FunctionType FunctionType { get; }
        
        public Function(MethodDeclarationSyntax methodDeclarationSyntax, IMethodSymbol methodSymbol,
            SemanticModel model)
        {
            Name = methodSymbol.Name;
            ReturnType = Helpers.NormalizeTypeString(methodSymbol.ReturnType.ToDisplayString());
            foreach (var param in methodSymbol.Parameters)
            {
                Arguments.Add(new FunctionArgument(Helpers.NormalizeTypeString(param.Type.ToDisplayString()), param.Name));
            }

            var attributes = methodDeclarationSyntax.AttributeLists.SelectMany(x => x.Attributes);
            switch (attributes)
            {
                case { } attr when attr.Any(a => a.Name.ToString() == "StaticFunction"):
                    FunctionType = FunctionType.StaticFunction;
                    break;
                case { } attr when attr.Any(a => a.Name.ToString() == "MemberFunction"):
                    FunctionType = FunctionType.MemberFunction;
                    break;
                case { } attr when attr.Any(a => a.Name.ToString() == "VirtualFunction"):
                    FunctionType = FunctionType.VirtualFunction;
                    break;
            }
        }
    }
}