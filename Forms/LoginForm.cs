using System.Windows.Forms;
using PasswordManagerApp.Services;

namespace PasswordManagerApp.Forms;

public class LoginForm : Form
{
    // 🔐 Свойство для получения мастер-пароля после входа
    public string MasterPassword { get; private set; } = "";

    private readonly AuthService _authService;

    public LoginForm(AuthService authService)
    {
        _authService = authService;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "Вход";
        Size = new Size(300, 250);
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        BackColor = Color.LightGray;

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
                // ✅ Сохраняем мастер-пароль из поля пароля
                MasterPassword = passwordBox.Text;

                DialogResult = DialogResult.OK;
                Close();
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