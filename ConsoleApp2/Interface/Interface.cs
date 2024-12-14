using Domain;
using JobSystemApp.Data;
using System.Text.Json;
using UserManagementApp;

namespace JobSystemApp.Interface
{
    public class Interface
    {
        private readonly ApplicationDbContext _context;

        public Interface(ApplicationDbContext context)
        {
            _context = context;
        }

        static List<JobOffer> jobOffers = new List<JobOffer>();
        static User currentUser = null;

        public void Menu()
        {
            string choice;
            do
            {
                Console.Clear();
                Console.WriteLine("=== System Ofert Pracy ===");
                if (currentUser == null)
                {
                    Console.WriteLine("1. Rejestracja");
                    Console.WriteLine("2. Logowanie");
                    Console.WriteLine("0. Wyjście");
                }
                else
                {
                    Console.WriteLine("3. Wyświetl oferty pracy");
                    if (currentUser.Role == "admin")
                    {
                        Console.WriteLine("4. Dodaj ofertę pracy");
                        Console.WriteLine("5. Importuj użytkowników z pliku JSON do bazy danych");
                    }
                    Console.WriteLine("0. Wyloguj");
                }

                Console.Write("Wybierz opcję: ");
                choice = Console.ReadLine();

                if (currentUser == null)
                {
                    switch (choice)
                    {
                        case "1":
                            RegisterUser();
                            break;
                        case "2":
                            LoginUser();
                            break;
                        case "0":
                            Console.WriteLine("Do widzenia!");
                            return;
                    }
                }
                else
                {
                    switch (choice)
                    {
                        case "3":
                            if (IsLoggedIn()) DisplayJobOffers();
                            break;
                        case "4":
                            if (IsLoggedIn() && currentUser.Role == "admin") AddJobOffer();
                            break;
                        case "5":
                            if (IsLoggedIn() && currentUser.Role == "admin") ImportUsersFromJSONToDatabase();
                            break;
                        case "0":
                            currentUser = null;
                            Console.WriteLine("Wylogowano. Wróć do logowania.");
                            Thread.Sleep(2000);
                            break;
                    }
                }

            } while (true);
        }

        void ImportFromJSONToDatabase()
        {
            Console.Write("Podaj nazwę pliku (np. oferty.json): ");
            string fileName = Console.ReadLine();

            if (File.Exists(fileName))
            {
                string json = File.ReadAllText(fileName);
                var offers = JsonSerializer.Deserialize<List<JobOffer>>(json);

                if (offers != null)
                {
                    foreach (var offer in offers)
                    {
                        var existingOffer = _context.JobOffers
                            .FirstOrDefault(j => j.Title == offer.Title && j.Company == offer.Company && j.Location == offer.Location);

                        if (existingOffer == null)
                        {
                            _context.JobOffers.Add(offer);
                            Console.WriteLine($"Dodano ofertę: {offer.Title} w firmie {offer.Company}");
                        }
                        else
                        {
                            Console.WriteLine($"Oferta {offer.Title} w firmie {offer.Company} już istnieje.");
                        }
                    }

                    
                    _context.SaveChanges();
                    Console.WriteLine("Oferty zostały zaimportowane do bazy danych.");
                }
                else
                {
                    Console.WriteLine("Błąd przy deserializacji danych z pliku.");
                }
            }
            else
            {
                Console.WriteLine("Plik nie istnieje.");
            }
            Thread.Sleep(2000);
        }
        public void ImportUsersFromJSONToDatabase()
        {
            Console.Write("Podaj nazwę pliku (np. users.json): ");
            string fileName = Console.ReadLine();

            if (File.Exists(fileName))
            {
                string json = File.ReadAllText(fileName);
                var usersList = JsonSerializer.Deserialize<List<User>>(json);

                if (usersList != null)
                {
                    foreach (var user in usersList)
                    {
                        var existingUser = _context.Users
                            .FirstOrDefault(u => u.Username == user.Username);

                        if (existingUser == null)
                        {
                            _context.Users.Add(user);
                            Console.WriteLine($"Dodano użytkownika: {user.Username} z rolą {user.Role}");
                        }
                        else
                        {
                            Console.WriteLine($"Użytkownik {user.Username} już istnieje.");
                        }
                    }

                    _context.SaveChanges();
                    Console.WriteLine("Użytkownicy zostali zaimportowani do bazy danych.");
                }
                else
                {
                    Console.WriteLine("Błąd przy deserializacji danych z pliku.");
                }
            }
            else
            {
                Console.WriteLine("Plik nie istnieje.");
            }
            Thread.Sleep(2000);
        }
        bool IsLoggedIn()
        {
            if (currentUser == null)
            {
                Console.WriteLine("Musisz być zalogowany, aby wykonać tą operację.");
                Thread.Sleep(2000);
                return false;
            }
            return true;
        }

        void RegisterUser()
        {
            UserManager userManager = new UserManager(_context); 
            Console.Write("Podaj nazwę użytkownika: ");
            string username = Console.ReadLine();
            Console.Write("Podaj hasło: ");
            string password = Console.ReadLine();
            Console.Write("Podaj rolę (admin/user): ");
            string role = Console.ReadLine().ToLower();
            userManager.RegisterUser(username, password, role);
        }

        void LoginUser()
        {
            UserManager userManager = new UserManager(_context);
            Console.Write("Podaj nazwę użytkownika: ");
            string loginUsername = Console.ReadLine();
            Console.Write("Podaj hasło: ");
            string loginPassword = Console.ReadLine();
            User user = new User()
            {
                Username = loginUsername,
                Password = loginPassword
            };
            var loggedInUser = userManager.Login(user);
            if (loggedInUser != null)
            {
                currentUser = loggedInUser;
            }
        }

        void AddJobOffer()
        {
            if (currentUser == null)
            {
                Console.WriteLine("Musisz być zalogowany, aby dodać ofertę.");
                Thread.Sleep(2000);
                return;
            }

            Console.Write("Podaj nazwę stanowiska: ");
            string title = Console.ReadLine();
            Console.Write("Podaj nazwę firmy: ");
            string company = Console.ReadLine();
            Console.Write("Podaj lokalizację: ");
            string location = Console.ReadLine();

            var offer = new JobOffer
            {
                Title = title,
                Company = company,
                Location = location
            };

            _context.JobOffers.Add(offer);
            _context.SaveChanges(); 

            Console.WriteLine("Oferta została dodana.");
            Thread.Sleep(2000);
        }

        void DisplayJobOffers()
        {
            if (!IsLoggedIn()) return;

            
            var jobOffers = _context.JobOffers.ToList();

            if (jobOffers.Count == 0)
            {
                Console.WriteLine("Brak ofert pracy.");
            }
            else
            {
                foreach (var offer in jobOffers)
                {
                    Console.WriteLine(offer.ToString());
                }
            }
            Thread.Sleep(2000);
        }

        
    }
}
