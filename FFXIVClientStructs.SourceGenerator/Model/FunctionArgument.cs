namespace FFXIVClientStructs.SourceGenerator.Model
{
    public class FunctionArgument
    {
        public string Type { get; }
        public string Name { get; }
        
        public FunctionArgument(string type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}