using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FFXIVClientStructs.SourceGenerator.Model
{
    public class Field
    {
        public string Name { get; }
        public Type Type { get; }
        public int Offset { get; }
        
        public Field(FieldDeclarationSyntax fieldDeclarationSyntax, VariableDeclaratorSyntax variableDeclaratorSyntax, IFieldSymbol fieldSymbol, SemanticModel model)
        {
            Name = fieldSymbol.Name;
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