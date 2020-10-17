using Lucraft.Database.Query;

namespace Lucraft.Database
{
    class Program
    {
        static void Main(string[] args)
        {
            //Parser.GetInstructions("get /database/collection/* where name equals 'Daytimer' || name contains 'ti'");
            DatabaseServer.Instance.Start();
        }
    }
}
