using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FFXIVClientStructs.SourceGenerator.Model
{
    public class Struct
    { 
        public string Name { get; }
        public string Namespace { get; }
        public int Size { get; }
        public List<Struct> ChildStructs { get; } = new();
        public List<Field> Fields { get; } = new();
        public List<Function> Functions { get; } = new();

        public Struct(StructDeclarationSyntax structDeclarationSyntax, INamedTypeSymbol typeSymbol, SemanticModel model)
        {
            Name = typeSymbol.Name;
            Namespace = Helpers.NormalizeTypeString(typeSymbol.ContainingNamespace.ToDisplayString());

            var structLayoutAttr = structDeclarationSyntax.AttributeLists.SelectMany(x => x.Attributes)
                .FirstOrDefault(attr => attr.Name.ToString() == "StructLayout");
            if (structLayoutAttr?.ArgumentList?.Arguments.Count >= 2)
            {
                var sizeArg = structLayoutAttr.ArgumentList.Arguments[1];
                var sizeExpr = sizeArg.Expression;
                Size = (int?) model.GetConstantValue(sizeExpr).Value ?? -1;
            }
            else
                Size = -1;

            var childStructSyntaxList = structDeclarationSyntax.ChildNodes().OfType<StructDeclarationSyntax>();
            foreach (var childStructSyntax in childStructSyntaxList)
            {
                if (model.GetDeclaredSymbol(childStructSyntax) is not { } childTypeSymbol) continue;
                ChildStructs.Add(new Struct(childStructSyntax, childTypeSymbol, model));
            }

            var fieldSyntaxList = structDeclarationSyntax.ChildNodes().OfType<FieldDeclarationSyntax>();
            foreach (var fieldSyntax in fieldSyntaxList)
            {
                foreach (var fieldVariableSyntax in fieldSyntax.Declaration.Variables)
                {
                    if (model.GetDeclaredSymbol(fieldVariableSyntax) is not IFieldSymbol fieldSymbol) continue;
                    Fields.Add(new Field(fieldSyntax, fieldVariableSyntax, fieldSymbol, model));
                }
            }

            var methodSyntaxList = structDeclarationSyntax.ChildNodes().OfType<MethodDeclarationSyntax>();
            foreach (var methodSyntax in methodSyntaxList)
            {
                if (model.GetDeclaredSymbol(methodSyntax) is not { } methodSymbol) continue;
                if (methodSyntax.AttributeLists.SelectMany(x => x.Attributes).Any(attr => 
                    attr.Name.ToString() == "MemberFunction" || attr.Name.ToString() == "VirtualFunction" || attr.Name.ToString() == "StaticFunction"))
                    Functions.Add(new Function(methodSyntax, methodSymbol, model));
            }
        }
    }
}