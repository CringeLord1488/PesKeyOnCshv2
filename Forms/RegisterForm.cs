using System.Windows.Forms;
using PasswordManagerApp.Services;

namespace PasswordManagerApp.Forms;

public class RegisterForm : Form
{
    private readonly AuthService _authService;

    public RegisterForm(AuthService authService)
    {
        _authService = authService;
        InitializeUI();
        BackColor = Color.LightGray;
    }

    private void InitializeUI()
    {
        Text = "Регистрация";
        Size = new Size(300, 250);
        StartPosition = FormStartPosition.CenterParent;

        var usernameBox = new TextBox { Location = new Point(50, 30), Width = 200 };
        var emailBox = new TextBox { Location = new Point(50, 80), Width = 200 };
        var passwordBox = new TextBox { Location = new Point(50, 130), Width = 200, PasswordChar = '*' };

        var registerBtn = new Button
        {
            Text = "Зарегистрироваться",
            Location = new Point(50, 180),
            Width = 200,
            BackColor = Color.HotPink,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        registerBtn.Click += (s, e) =>
        {
            if (_authService.Register(usernameBox.Text, emailBox.Text, passwordBox.Text))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Ошибка регистрации.");
            }
        };

        Controls.Add(new Label { Text = "Имя пользователя", Location = new Point(50, 10), AutoSize = true });
        Controls.Add(usernameBox);
        Controls.Add(new Label { Text = "Email", Location = new Point(50, 60), AutoSize = true });
        Controls.Add(emailBox);
        Controls.Add(new Label { Text = "Пароль", Location = new Point(50, 110), AutoSize = true });
        Controls.Add(passwordBox);
        Controls.Add(registerBtn);
    }
}