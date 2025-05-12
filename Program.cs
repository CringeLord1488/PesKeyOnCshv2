using System;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using PasswordManagerApp.Data;
using PasswordManagerApp.Services;
using PasswordManagerApp.Utils;

Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);

// Путь к базе в AppData
string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
string dbPath = Path.Combine(appDataPath, "KeyPes", "passwords.db");

var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionsBuilder.UseSqlite($"Data Source={dbPath}");

using var context = new AppDbContext(optionsBuilder.Options);
context.Database.Migrate(); // Или EnsureCreated(), если не используешь миграции

var authService = new AuthService(context);
authService.StorageService = new StorageService(context);

Application.Run(new LoginForm(authService));