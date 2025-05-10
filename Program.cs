using System;
using System.Windows.Forms;
using PasswordManagerApp.Data;
using PasswordManagerApp.Services;
using Microsoft.EntityFrameworkCore;
using PasswordManagerApp.Forms;

Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);

var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionsBuilder.UseSqlite("Data Source=passwords.db");

using var context = new AppDbContext(optionsBuilder.Options);
context.Database.EnsureCreated();

var authService = new AuthService(context);
var storageService = new StorageService(context);

Application.Run(new MainForm(authService, storageService));