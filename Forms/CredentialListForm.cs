using System.Drawing;
using System.Windows.Forms;
using PasswordManagerApp.Models;
using PasswordManagerApp.Services;
using PasswordManagerApp.Utils;

namespace PasswordManagerApp.Forms;

public class CredentialListForm : Form
{
    private readonly AuthService _authService;
    private readonly StorageService _storageService;

    public CredentialListForm(AuthService authService, StorageService storageService)
    {
        _authService = authService;
        _storageService = storageService;

        InitializeUI();
    }

    private void InitializeUI()
    {
        Text = "Записи";
        Size = new Size(500, 700);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.WhiteSmoke;

        var titleLabel = new Label
        {
            Text = "Ваши учётные данные",
            Dock = DockStyle.Top,
            Height = 60,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = Color.HotPink
        };
        Controls.Add(titleLabel);

        var flowPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false
        };

        foreach (var cred in _storageService.ReadCredentials())
        {
            var panel = new Panel
            {
                Width = flowPanel.Width - 40,
                Height = 60,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.LightBlue,
                Margin = new Padding(10)
            };

            var nameLabel = new Label { Text = $"Сервис: {cred.ServiceName}", Location = new Point(10, 10), AutoSize = true };
            var loginLabel = new Label { Text = $"Логин: {cred.Login}", Location = new Point(10, 30), AutoSize = true };
            var passLabel = new Label { Text = $"Пароль: {AesEncryption.Decrypt(cred.EncryptedPassword)}", Location = new Point(10, 50), AutoSize = true };

            panel.Controls.AddRange(new Control[] { nameLabel, loginLabel, passLabel });
            flowPanel.Controls.Add(panel);
        }

        Controls.Add(flowPanel);

        // Кнопка добавления +
        var addButton = new Button
        {
            Text = "+",
            Font = new Font("Segoe UI", 20F),
            Width = 60,
            Height = 60,
            BackColor = Color.HotPink,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };

        Load += (s, e) =>
        {
            addButton.Location = new Point((ClientSize.Width - addButton.Width) / 2, ClientSize.Height - addButton.Height - 80);
        };

        Resize += (s, e) =>
        {
            addButton.Location = new Point((ClientSize.Width - addButton.Width) / 2, ClientSize.Height - addButton.Height - 80);
        };

        addButton.Click += (s, e) => ShowAddForm();

        Controls.Add(addButton);
    }

    private void ShowAddForm()
    {
        var addForm = new AddCredentialForm(_storageService);
        addForm.FormClosed += (s, e) =>
        {
            Controls.Clear();
            InitializeUI(); // Обновляем интерфейс
        };
        addForm.Show(this);
        Hide();
    }
}