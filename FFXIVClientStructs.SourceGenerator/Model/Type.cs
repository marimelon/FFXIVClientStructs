using Microsoft.CodeAnalysis;

namespace FFXIVClientStructs.SourceGenerator.Model
{
    public class Type
    {
        public string CsType { get; }
        public string CppType { get; }

        public Type(string csType)
        {
            CsType = csType;
            CppType = GetCppTypeString(csType);
        }
        
        private string GetCppTypeString(string typeString)
        {
            string outString;
            
            switch (typeString)
            {
                case { } s when s.StartsWith(Struct.StdStructNamespace + "PointerVector") || s.StartsWith(Struct.StdStructNamespace + "Vector"):
                    var containedType = s.Substring(s.IndexOf('\u003C') + 1,
                        s.IndexOf('\u003E') - s.IndexOf('\u003C') - 1);
                    var cppContainedType = GetCppTypeString(containedType);
                    outString = s.StartsWith(Struct.StdStructNamespace + "PointerVector") ? $"std::vector<{cppContainedType}*>" : $"std::vector<{cppContainedType}>";
                    break;
                case { } s when s.StartsWith(Struct.StdStructNamespace + "String"):
                    outString = "std::string";
                    break;
                case { } s when s.StartsWith(Struct.FfxivStructNamespace):
                    outString = s.Substring(Struct.FfxivStructNamespace.Length);
                    break;
                default:
                    outString = typeString;
                    break;
            }

            return outString.Replace(".", "::");
        }
    }
}