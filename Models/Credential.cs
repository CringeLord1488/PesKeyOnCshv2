namespace PasswordManagerApp.Models;

public class Credential
{
    public int Id { get; set; }
    public required string ServiceName { get; set; }
    public required string Login { get; set; }
    public required string EncryptedPassword { get; set; }
}