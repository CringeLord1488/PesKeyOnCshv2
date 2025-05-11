using System.Drawing;
using System.Windows.Forms;
using PasswordManagerApp.Services;

namespace PasswordManagerApp.Forms;

public class MainForm : Form
{
    private readonly AuthService _authService;
    private readonly StorageService _storageService;

    public MainForm(AuthService authService, StorageService storageService)
    {
        _authService = authService;
        _storageService = storageService;

        InitializeUI();
    }

    private void InitializeUI()
    {
        Text = "KeyPes";
        Size = new Size(500, 700);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.LightGray;
        MaximizeBox = false;
        FormBorderStyle = FormBorderStyle.FixedSingle;

        // Верхняя панель с заголовком
        var topPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60,
            BackColor = Color.FromArgb(40, 40, 40)
        };

        var titleLabel = new Label
        {
            Text = "KeyPes",
            ForeColor = Color.HotPink,
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        };
        topPanel.Controls.Add(titleLabel);
        Controls.Add(topPanel);

        // Центральная часть — кнопки входа и регистрации
        var centerPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.WhiteSmoke
        };

        var loginButton = new Button
        {
            Text = "Войти",
            Width = 200,
            Height = 40,
            Location = new Point((Width - 200) / 2, (Height - 80) / 2),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.HotPink,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 12F, FontStyle.Regular)
        };
        loginButton.Click += (s, e) => ShowLoginForm();

        var registerButton = new Button
        {
            Text = "Зарегистрироваться",
            Width = 200,
            Height = 40,
            Location = new Point((Width - 200) / 2, (Height - 80) / 2 + 50),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Pink,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 12F, FontStyle.Regular)
        };
        registerButton.Click += (s, e) => ShowRegisterForm();

        centerPanel.Controls.Add(loginButton);
        centerPanel.Controls.Add(registerButton);
        Controls.Add(centerPanel);
    }

    private void ShowLoginForm()
    {
        var loginForm = new LoginForm(_authService);
        if (loginForm.ShowDialog() == DialogResult.OK)
        {
            var credentialListForm = new CredentialListForm(_authService, _storageService, loginForm.MasterPassword);
            credentialListForm.FormClosed += (s, e) => this.Show();
            credentialListForm.Show();
            this.Hide();
        }
    }

    private void ShowRegisterForm()
    {
        var registerForm = new RegisterForm(_authService);
        registerForm.FormClosed += (s, e) =>
        {
            if (registerForm.DialogResult == DialogResult.OK)
            {
                MessageBox.Show("Вы зарегистрировались. Теперь войдите.");
                ShowLoginForm();
            }
        };
        registerForm.Show(this);
        Hide();
    }
}