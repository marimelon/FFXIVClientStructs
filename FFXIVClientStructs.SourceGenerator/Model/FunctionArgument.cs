namespace FFXIVClientStructs.SourceGenerator.Model
{
    public class FunctionArgument
    {
        public Type Type { get; }
        public string Name { get; }
        
        public FunctionArgument(Type type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}