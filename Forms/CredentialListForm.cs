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
        Text = "KeyPes — Записи";
        Size = new Size(500, 700);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.LightGray;
        MaximizeBox = false;
        FormBorderStyle = FormBorderStyle.FixedSingle;

        // Верхняя панель
        var topPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60,
            BackColor = Color.FromArgb(40, 40, 40)
        };

        var titleLabel = new Label
        {
            Text = "Ваши данные",
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
            // Контейнер для записи
            var container = new Panel
            {
                Width = flowPanel.Width - 70,
                Height = 80,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.LightBlue,
                Margin = new Padding(5)
            };

            // Панель для меток (чтобы они не перекрывали кнопку)
            var labelPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = container.Width - 50, // Оставляем место для кнопки
                Height = container.Height,
                BackColor = Color.Transparent
            };

            // Текстовые метки
            var nameLabel = new Label
            {
                Text = $"Сервис: {cred.ServiceName}",
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Height = 25
            };

            var loginLabel = new Label
            {
                Text = $"Логин: {cred.Login}",
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Height = 25
            };

            var passLabel = new Label
            {
                Text = $"Пароль: {AesEncryption.Decrypt(cred.EncryptedPassword, _masterPassword)}",
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Height = 25
            };

            // Кнопка удаления ❌
            var deleteButton = new Button
            {
                Text = "✕",
                Width = 30,
                Height = 30,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Red,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F),
                Tag = cred.Id,
            };
            deleteButton.Click += (s, e) =>
            {
                int id = (int)deleteButton.Tag!;
                _storageService.DeleteCredential(id);
                Controls.Clear();
                InitializeUI(); // Перезагружаем интерфейс
            };

            deleteButton.Location = new Point(container.Width - deleteButton.Width - 10, 10);

           // Добавляем метки в labelPanel
            labelPanel.Controls.Add(passLabel);
            labelPanel.Controls.Add(loginLabel);
            labelPanel.Controls.Add(nameLabel);

           // Добавляем элементы в контейнер
            container.Controls.Add(labelPanel);
            container.Controls.Add(deleteButton);
            flowPanel.Controls.Add(container);
        }

        Controls.Add(flowPanel);

        // ➕ Кнопка добавления
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
        _addButton.BringToFront();

        Load += (s, e) => UpdateAddButtonLocation();
        Resize += (s, e) => UpdateAddButtonLocation();

        UpdateAddButtonLocation();
    }

    private void UpdateAddButtonLocation()
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
        addForm.FormClosed += (sender, args) =>
        {
            Controls.Clear();
            InitializeUI(); // Обновляем форму
        };
        addForm.Show(this);
    }
}