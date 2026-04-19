using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using WpfSignalApp.Models;

namespace WpfSignalApp.Services
{
    public static class UserService
    {
        private static readonly string FilePath =
            Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "users.json");

        private static List<User> LoadAll()
        {
            if (!File.Exists(FilePath)) return new List<User>();
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        private static void SaveAll(List<User> users)
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        public static bool Register(string username, string email, string password)
        {
            var users = LoadAll();
            if (users.Exists(u => u.Username == username || u.Email == email))
                return false;

            var user = new User
            {
                Id = users.Count > 0 ? users[^1].Id + 1 : 1,
                Username = username,
                Email = email,
                Password = password
            };
            users.Add(user);
            SaveAll(users);
            return true;
        }

        public static User? Login(string username, string password)
        {
            var users = LoadAll();
            return users.Find(u => u.Username == username && u.Password == password);
        }
    }
}
