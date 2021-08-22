using System;
using System.Runtime.InteropServices.ComTypes;

namespace FFXIVClientStructs.SourceGenerator.Model
{
    public static class Helpers
    {
        public const string FfxivStructNamespace = "FFXIVClientStructs.FFXIV.";
        public const string StdStructNamespace = "FFXIVClientStructs.STD.";
        
        public static string NormalizeTypeString(string typeString)
        {
            if (typeString.StartsWith(StdStructNamespace))
            {
                switch (typeString.Substring(StdStructNamespace.Length))
                {
                    case { } s when s.StartsWith("PointerVector") || s.StartsWith("Vector"):
                        var containedType = s.Substring(s.IndexOf('\u003C') + 1,
                            s.IndexOf('\u003E') - s.IndexOf('\u003C') - 1);
                        var normalizedContainedType = NormalizeTypeString(containedType);
                        return s.StartsWith("PointerVector") ? $"std::vector<{normalizedContainedType}*>" : $"std::vector<{normalizedContainedType}>";
                    case { } s when s.StartsWith("String"):
                        return "std::string";
                }
            }
            
            if (!typeString.StartsWith(FfxivStructNamespace)) return typeString;

            return typeString.Substring(FfxivStructNamespace.Length);
        }
    }
}