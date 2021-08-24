using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace FFXIVClientStructs.SourceGenerator.Model
{
    public class Type
    {
        public string CsType { get; }
        public string CppType { get; }
        public bool IsPointer { get; }
        public bool IsGeneric { get; }
        public List<Type>? TypeArguments { get; }

        public Type(ITypeSymbol type)
        {
            switch (type)
            {
                case IPointerTypeSymbol pt:
                    
            }
            CsType = typeName.TrimEnd('*');
            CppType = GetCppTypeString(typeName);
            IsPointer = type.TypeKind == TypeKind.Pointer;
            if (type is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType)
            {
                IsGeneric = true;
                TypeArguments = new();
                foreach(var typeArgument in namedTypeSymbol.TypeArguments)
                    TypeArguments.Add(new Type(typeArgument));
            }
        }
        
        private string GetCppTypeString(string typeString)
        {
            string outString = "";
            
            switch (typeString.TrimEnd('*'))
            {
                case { } s when s.StartsWith(Struct.StdStructNamespace + "PointerVector") || s.StartsWith(Struct.StdStructNamespace + "Vector"):
                    var containedType = s.Substring(s.IndexOf('\u003C') + 1,
                        s.IndexOf('\u003E') - s.IndexOf('\u003C') - 1);
                    var cppContainedType = GetCppTypeString(containedType);
                    outString = s.StartsWith(Struct.StdStructNamespace + "PointerVector") && cppContainedType != "T" ? $"std::vector<{cppContainedType}*>" : $"std::vector<{cppContainedType}>";
                    break;
                case { } s when s.StartsWith(Struct.StdStructNamespace + "String"):
                    outString = "std::string";
                    break;
                case { } s when s.StartsWith(Struct.FfxivStructNamespace):
                    outString = s.Substring(Struct.FfxivStructNamespace.Length);
                    break;
                case "byte":
                    outString = "uint8_t";
                    break;
                case "sbyte":
                    outString = "int8_t";
                    break;
                case "ushort":
                    outString = "uint16_t";
                    break;
                case "short":
                    outString = "int16_t";
                    break;
                case "uint":
                    outString = "uint32_t";
                    break;
                case "int":
                    outString = "int32_t";
                    break;
                case "ulong":
                    outString = "uint64_t";
                    break;
                case "long":
                    outString = "int64_t";
                    break;
                case "nuint":
                    outString = "uintptr_t";
                    break;
                case "nint":
                    outString = "intptr_t";
                    break;
                case { } s:
                    outString = s;
                    break;
            }

            return outString.Replace(".", "::");
        }
    }
}