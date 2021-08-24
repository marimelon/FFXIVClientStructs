using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FFXIVClientStructs.SourceGenerator.Model
{
    public class Function
    {
        public string Name { get; }
        public Type ReturnType { get; }
        public List<FunctionArgument>? Arguments { get; }
        public FunctionKind FunctionKind { get; }
        
        public Function(MethodDeclarationSyntax methodDeclarationSyntax, IMethodSymbol methodSymbol,
            SemanticModel model)
        {
            Name = methodSymbol.Name;
            ReturnType = new Type(methodSymbol.ReturnType);
            foreach (var param in methodSymbol.Parameters)
            {
                Arguments ??= new();
                Arguments.Add(new FunctionArgument(new Type(param.Type), param.Name));
            }

            var attributes = methodDeclarationSyntax.AttributeLists.SelectMany(x => x.Attributes);
            switch (attributes)
            {
                case { } attr when attr.Any(a => a.Name.ToString() == "StaticFunction"):
                    FunctionKind = FunctionKind.StaticFunction;
                    break;
                case { } attr when attr.Any(a => a.Name.ToString() == "MemberFunction"):
                    FunctionKind = FunctionKind.MemberFunction;
                    break;
                case { } attr when attr.Any(a => a.Name.ToString() == "VirtualFunction"):
                    FunctionKind = FunctionKind.VirtualFunction;
                    break;
            }
        }
    }
}