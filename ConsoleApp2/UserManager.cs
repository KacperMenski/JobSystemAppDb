using JobSystemApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserManagementApp;

namespace JobSystemApp
{
    public class UserManager
    {
        private readonly ApplicationDbContext _context;
        private const string FilePath = "users.json";
        private List<User> users;
        private int nextId;

        public UserManager(ApplicationDbContext context)
        {
            _context = context;
            users = LoadUsers();
            nextId = users.Count > 0 ? users[^1].Id + 1 : 1;
        }

        private List<User> LoadUsers()
        {
            if (!File.Exists(FilePath))
            {
                return new List<User>();
            }

            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        private void SaveUsers()
        {
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        public void RegisterUser(string username, string password, string role)
        {

            if (users.Exists(u => u.Username == username))
            {
                Console.WriteLine("Użytkownik o tej nazwie już istnieje.");
                Thread.Sleep(2000);
                return;
            }

            if (role != "admin" && role != "user")
            {
                Console.WriteLine("Niepoprawna rola. Wybierz 'admin' lub 'user'.");
                Thread.Sleep(2000);
                return;
            }

            var newUser = new User()
            {
                Id = nextId,
                Username = username,
                Password = password,
                Role = role

            };
            users.Add(newUser);
            SaveUsers();
            Console.WriteLine($"Użytkownik został pomyślnie zarejestrowany. ID: {newUser.Id}");
            Thread.Sleep(2000);
        }



        public User Login(User user)
        {
            var entity = users.Find(u => u.Username == user.Username && u.Password == user.Password);

            if (entity == null)
            {
                Console.WriteLine("Nieprawidłowa nazwa użytkownika lub hasło.");
                Thread.Sleep(2000);
                return null;
            }

            Console.WriteLine($"Zalogowano jako {entity.Username}. Rola: {entity.Role}");
            user.SayHello(user);
            Thread.Sleep(2000);
            return entity;
        }

        public bool IsAdmin(User user)
        {
            
            return user != null && user.Role == "admin";
        }
    }
}
