using PasswordManagerApp.Models;
using PasswordManagerApp.Utils;
using PasswordManagerApp.Data;
using System.Linq;

namespace PasswordManagerApp.Services;

public class AuthService
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context) => _context = context;

    public bool Register(string username, string email, string password)
    {
        if (_context.Users.Any(u => u.Username == username || u.Email == email))
        {
            Console.WriteLine("Пользователь уже существует.");
            return false;
        }

        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = HashingService.HashPassword(password)
        };

        _context.Users.Add(user);
        _context.SaveChanges();
        Console.WriteLine("Регистрация успешна.");
        return true;
    }

    public bool Login(string username, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user != null && HashingService.VerifyPassword(password, user.PasswordHash))
        {
            Console.WriteLine("Вы вошли!");
            return true;
        }

        Console.WriteLine("Неверный логин или пароль.");
        return false;
    }
}