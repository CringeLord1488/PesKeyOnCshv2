using System.Windows.Forms;
using PasswordManagerApp.Models;
using PasswordManagerApp.Utils;
using PasswordManagerApp.Services;

namespace PasswordManagerApp.Forms;

public class AddCredentialForm : Form
{
    private readonly StorageService _storageService;

    public AddCredentialForm(StorageService storageService)
    {
        _storageService = storageService;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "Добавить запись";
        Size = new Size(300, 250);
        StartPosition = FormStartPosition.CenterParent;

        var serviceBox = new TextBox { Location = new Point(50, 30), Width = 200 };
        var loginBox = new TextBox { Location = new Point(50, 80), Width = 200 };
        var passwordBox = new TextBox { Location = new Point(50, 130), Width = 200, PasswordChar = '*' };

        var saveBtn = new Button
        {
            Text = "Сохранить",
            Location = new Point(50, 180),
            Width = 200,
            BackColor = Color.FromArgb(0, 120, 215),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        saveBtn.Click += (s, e) =>
        {
            if (string.IsNullOrWhiteSpace(serviceBox.Text) ||
                string.IsNullOrWhiteSpace(loginBox.Text) ||
                string.IsNullOrWhiteSpace(passwordBox.Text))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            var credential = new Credential
            {
                ServiceName = serviceBox.Text,
                Login = loginBox.Text,
                EncryptedPassword = AesEncryption.Encrypt(passwordBox.Text)
            };

            _storageService.SaveCredential(credential);
            DialogResult = DialogResult.OK;
            Close();
        };

        Controls.Add(new Label { Text = "Сервис", Location = new Point(50, 10), AutoSize = true });
        Controls.Add(serviceBox);
        Controls.Add(new Label { Text = "Логин", Location = new Point(50, 60), AutoSize = true });
        Controls.Add(loginBox);
        Controls.Add(new Label { Text = "Пароль", Location = new Point(50, 110), AutoSize = true });
        Controls.Add(passwordBox);
        Controls.Add(saveBtn);
    }
}