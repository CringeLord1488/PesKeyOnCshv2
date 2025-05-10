using System.Windows.Forms;
using PasswordManagerApp.Services;

namespace PasswordManagerApp.Forms;

public class LoginForm : Form
{
    private readonly AuthService _authService;
    private readonly StorageService _storageService; // ✅ Добавь это

    public LoginForm(AuthService authService, StorageService storageService) // ✅ Обнови конструктор
    {
        _authService = authService;
        _storageService = storageService;

        InitializeUI();
        BackColor = Color.LightGray;
    }

    private void InitializeUI()
    {
        Text = "Вход";
        Size = new Size(300, 250);
        StartPosition = FormStartPosition.CenterParent;

        var usernameBox = new TextBox { Location = new Point(50, 30), Width = 200 };
        var passwordBox = new TextBox { Location = new Point(50, 80), Width = 200, PasswordChar = '*' };

        var loginBtn = new Button
        {
            Text = "Войти",
            Location = new Point(50, 130),
            Width = 200,
            BackColor = Color.HotPink,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        loginBtn.Click += (s, e) =>
        {
            if (_authService.Login(usernameBox.Text, passwordBox.Text))
            {
                // Теперь можем использовать _storageService
                var credentialListForm = new CredentialListForm(_authService, _storageService);
                credentialListForm.Show();
                this.Hide(); // Скрываем текущую форму
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        };

        Controls.Add(new Label { Text = "Имя пользователя", Location = new Point(50, 10), AutoSize = true });
        Controls.Add(usernameBox);
        Controls.Add(new Label { Text = "Пароль", Location = new Point(50, 60), AutoSize = true });
        Controls.Add(passwordBox);
        Controls.Add(loginBtn);
    }
}