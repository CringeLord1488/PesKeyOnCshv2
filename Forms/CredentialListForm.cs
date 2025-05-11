using System.Drawing;
using System.Windows.Forms;
using PasswordManagerApp.Services;
using PasswordManagerApp.Models;
using PasswordManagerApp.Utils;

namespace PasswordManagerApp.Forms;

public class CredentialListForm : Form
{
    private readonly AuthService _authService;
    private readonly StorageService _storageService;
    private readonly string _masterPassword;
    private Button _addButton = null!;

    public CredentialListForm(AuthService authService, StorageService storageService, string masterPassword)
    {
        _authService = authService;
        _storageService = storageService;
        _masterPassword = masterPassword;

        InitializeUI();
    }

    private void InitializeUI()
    {
        Text = "Ваши данные";
        Size = new Size(500, 700);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.LightGray;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        // Верхняя панель с заголовком
        var topPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60,
            BackColor = Color.FromArgb(64, 64, 64)
        };

        var titleLabel = new Label
        {
            Text = "KeyPes — Записи",
            ForeColor = Color.HotPink,
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        };
        topPanel.Controls.Add(titleLabel);
        Controls.Add(topPanel);

        // Список записей
        var flowPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(10),
            BackColor = Color.WhiteSmoke
        };

        foreach (var cred in _storageService.ReadCredentials())
        {
            var panel = new Panel
            {
                Width = flowPanel.Width - 40,
                Height = 60,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.LightBlue,
                Margin = new Padding(5)
            };

            var nameLabel = new Label { Text = $"Сервис: {cred.ServiceName}", Location = new Point(10, 10), AutoSize = true };
            var loginLabel = new Label { Text = $"Логин: {cred.Login}", Location = new Point(10, 30), AutoSize = true };
            var passLabel = new Label { Text = $"Пароль: {AesEncryption.Decrypt(cred.EncryptedPassword, _masterPassword)}", Location = new Point(10, 50), AutoSize = true };

            panel.Controls.AddRange(new Control[] { nameLabel, loginLabel, passLabel });
            flowPanel.Controls.Add(panel);
        }

        Controls.Add(flowPanel);

        // Кнопка +
        _addButton = new Button
        {
            Text = "+",
            Width = 60,
            Height = 60,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.HotPink,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 20F, FontStyle.Regular),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right
        };

        _addButton.Click += (s, e) => ShowAddForm();

        Controls.Add(_addButton);
        _addButton.BringToFront(); // Не забываем про эту хуйлушу, чтоб не пряталась где попало

        // Позиционируем кнопку на форме
        UpdateButtonLocation();

        // Подписываемся на Resize и Load, чтобы корректно позиционировать
        Load += (s, e) => UpdateButtonLocation();
        Resize += (s, e) => UpdateButtonLocation();
    }

    private void UpdateButtonLocation()
    {
        if (_addButton != null)
        {
            _addButton.Location = new Point(
                (ClientSize.Width - _addButton.Width) / 2,
                ClientSize.Height - _addButton.Height - 80
            );
        }
    }

    private void ShowAddForm()
    {
        var addForm = new AddCredentialForm(_storageService, _masterPassword);
        addForm.FormClosed += (s, e) =>
        {
            Controls.Clear();     // Очищаем форму
            InitializeUI();       // Пересоздаём интерфейс
        };
        addForm.Show(this);
    }
}