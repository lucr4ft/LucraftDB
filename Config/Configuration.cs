namespace Lucraft.Database.Config
{
    public class Configuration
    {
        public int Port { get; init; }
        public int MaxConnections { get; init; }
        public bool Debug { get; init; }
        public DataOptions DataOptions { get; init; }
    }
}
