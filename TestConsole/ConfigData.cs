namespace TestConsole
{
    internal class ConfigData
    {
        public string ConfigFolder { get; internal set; }
        public string ToolName { get; internal set; }
        public string Pipeline { get; internal set; }
        public string Environment { get; internal set; }
        public bool UpdatePlan { get; internal set; }
        public string Key { get; internal set; }
    }
}