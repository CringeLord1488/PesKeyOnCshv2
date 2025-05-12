using System.Drawing;
using System.Windows.Forms;
using PasswordManagerApp.Services;
using System.Diagnostics;

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
        BackColor = Color.Purple;
        MaximizeBox = false;
        FormBorderStyle = FormBorderStyle.FixedSingle;

        // Верхняя панель с заголовком
        var topPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60,
            BackColor = Color.DarkViolet
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

        // Центральная часть — надпись, кнопки и ссылка
        var centerPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.WhiteSmoke
        };

        // Надпись "Ваши данные - наша забота"
        var sloganLabel = new Label
        {
            Text = "Ваши данные - наша забота",
            ForeColor = Color.HotPink,
            Font = new Font("Segoe UI", 12F, FontStyle.Regular),
            AutoSize = true,
            BackColor = Color.Transparent,
            TextAlign = ContentAlignment.MiddleCenter
        };
        sloganLabel.Location = new Point((Width - sloganLabel.PreferredWidth) / 2, 150);

        var loginButton = new Button
        {
            Text = "Войти",
            Width = 200,
            Height = 40,
            Location = new Point((Width - 200) / 2, 220),
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
            Location = new Point((Width - 200) / 2, 270),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Pink,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 12F, FontStyle.Regular)
        };
        registerButton.Click += (s, e) => ShowRegisterForm();

        // Ссылка внизу
        var linkLabel = new LinkLabel
        {
            Text = "https://vk.com/public213597544",
            Font = new Font("Segoe UI", 8F, FontStyle.Regular),
            AutoSize = true,
            LinkColor = Color.HotPink, // Розовый цвет для соответствия стилю
            ActiveLinkColor = Color.DeepPink,
            VisitedLinkColor = Color.Pink,
            BackColor = Color.Transparent,
            TextAlign = ContentAlignment.MiddleCenter
        };
        linkLabel.Location = new Point((Width - linkLabel.PreferredWidth) / 2, Height - 60); // Внизу формы
        linkLabel.LinkClicked += (s, e) =>
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://vk.com/public213597544",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть ссылку: {ex.Message}");
            }
        };

        centerPanel.Controls.Add(sloganLabel);
        centerPanel.Controls.Add(loginButton);
        centerPanel.Controls.Add(registerButton);
        centerPanel.Controls.Add(linkLabel);
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