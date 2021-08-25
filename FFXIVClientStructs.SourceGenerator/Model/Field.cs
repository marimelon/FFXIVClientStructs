using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FFXIVClientStructs.SourceGenerator.Model
{
    public class Field
    {
        public string Name { get; }
        public Type? Type { get; }
        public int Offset { get; }

        public Field(FieldDeclarationSyntax fieldDeclarationSyntax, VariableDeclaratorSyntax variableDeclaratorSyntax, IFieldSymbol fieldSymbol, SemanticModel model)
        {
            Name = fieldSymbol.Name;

            if (fieldSymbol.IsFixedSizeBuffer)
            {
                var fixedBufferAttr = fieldDeclarationSyntax.AttributeLists.SelectMany(x => x.Attributes)
                    .FirstOrDefault(attr => attr.Name.ToString() == "FixedBuffer");
                if (fixedBufferAttr?.ArgumentList?.Arguments.Count >= 2)
                {
                    if (fixedBufferAttr.ArgumentList.Arguments[0].Expression is TypeOfExpressionSyntax typeExpr
                        && model.GetTypeInfo(typeExpr.Type).Type is {} typeSymbol)
                    {
                        var length = (int?)
                            model.GetConstantValue(fixedBufferAttr.ArgumentList.Arguments[1].Expression).Value ?? -1;
                        Type = new Type(typeSymbol, true, length);
                    }
                }
            }
            else
                Type = new Type(fieldSymbol.Type);

            var fieldOffsetAttr = fieldDeclarationSyntax.AttributeLists.SelectMany(x => x.Attributes)
                .FirstOrDefault(attr => attr.Name.ToString() == "FieldOffset");
            if (fieldOffsetAttr?.ArgumentList?.Arguments.Count >= 1)
            {
                var sizeArg = fieldOffsetAttr.ArgumentList.Arguments[0];
                var sizeExpr = sizeArg.Expression;
                Offset = (int?) model.GetConstantValue(sizeExpr).Value ?? -1;
            }
            else
                Offset = -1;
        }
    }
}