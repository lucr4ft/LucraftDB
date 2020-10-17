using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lucraft.Database
{
    public class Users
    {

        private static List<User> users { get; }

        public static readonly Users Instance = JsonConvert.DeserializeObject<Users>(File.ReadAllText(DatabaseServer.ROOT_PATH + "/users.json"));

        public void Load() { }

        //public static void LoadRegisteredUser()
        //{
        //    JsonConvert.DeserializeObject<Users>(File.ReadAllText(DatabaseServer.ROOT_PATH + "/users.json"));
        //}

        public User GetUserByID(long id)
        {
            foreach (User user in users)
            {
                if (user.ID == id) return user;
            }
            return null;
        }

        public User GetUserByName(string name)
        {
            throw new NotImplementedException();
        }
    }

    public class User
    {
        public long ID { get; }
        public string Name { get; }
        public int PermissionLevel { get; }
        public string Password { get; }
    }
}
