﻿using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace FFXIVClientStructs.SourceGenerator.Model
{
    public class Type
    {
        public string? Name { get; }
        public string? Namespace { get; }
        public TypeKind TypeKind { get; }
        public Type? PointedAtType { get; }
        public List<Type>? TypeArguments { get; }

        public Type(ITypeSymbol type)
        {
            var format = new SymbolDisplayFormat(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypes,
                miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes);
            switch (type)
            {
                case IPointerTypeSymbol pt:
                    TypeKind = TypeKind.Pointer;
                    PointedAtType = new Type(pt.PointedAtType);
                    return;
                case INamedTypeSymbol { IsGenericType: true } nt:
                    TypeKind = TypeKind.Generic;
                    TypeArguments = new();
                    foreach(var typeArgument in nt.TypeArguments)
                        TypeArguments.Add(new Type(typeArgument));
                    break;
                default:
                    TypeKind = TypeKind.Simple;
                    break;
            }
            Name = type.ToDisplayString(format);
            var containingType = type;
            while (containingType.ContainingType != null) containingType = containingType.ContainingType;
            var nsType = containingType.ContainingNamespace;
            Namespace = nsType.ToDisplayString();
        }
    }
}