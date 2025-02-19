namespace MoneyManager.Shared.Components
{
    public class SelectOptionSet
    {
        public string? PropertyName { get; set; }  // The name of the property
        public Type? PropertyType { get; set; }    // The type of the property
        public List<object> Options { get; set; } = new(); // List of available select options
    }
}
