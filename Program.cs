namespace Lucraft.Database
{
    class Program
    {
        static void Main(string[] args)
        {
            //string json = "{\"array\":[\"a\",\"b\"]}";
            //Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            //Console.WriteLine(data["array"].GetType());
            //JArray array = (JArray)data["array"];
            //Console.WriteLine(array);
            //foreach (JValue val in array)
            //{
            //    Console.WriteLine(val.Value.Equals("a"));
            //}
            //Console.WriteLine("* " + array.Contains(new JValue("a")));

            DatabaseServer.Instance.Start();
        }
    }
}
