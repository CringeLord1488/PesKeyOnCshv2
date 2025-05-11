using PasswordManagerApp.Models;
using PasswordManagerApp.Data;
using System.Linq;

namespace PasswordManagerApp.Services;

public class StorageService
{
    private readonly AppDbContext _context;

    public StorageService(AppDbContext context) => _context = context;

    public void SaveCredential(Credential credential)
    {
        _context.Credentials.Add(credential);
        _context.SaveChanges();
    }

    public void DeleteCredential(int id)
    {
    var credential = _context.Credentials.Find(id);
    if (credential != null)
    {
        _context.Credentials.Remove(credential);
        _context.SaveChanges();
    }
    }
    public List<Credential> ReadCredentials() => _context.Credentials.ToList();
}