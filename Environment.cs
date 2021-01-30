namespace Lucraft.Database
{
    public class DevelopmentEnvironment : IEnvironment
    {
        public bool IsDevelopment()
        {
            return true;
        }
    }

    public class ProductionEnvironment : IEnvironment
    {
        public bool IsDevelopment()
        {
            return false;
        }
    }
}
