using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Data;
using SimpleAppWithApi.DTO;
using SimpleAppWithApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Win
{
    public partial class EditWindow : Window
    {
        private readonly User _user;
        private readonly HttpClient _client;
        private List<Role> _roles = new();
        private string? _selectedPhotoPath;

        public EmployeeEditDto? EditedDto { get; private set; }
        public string? SelectedPhotoPath => _selectedPhotoPath;

        public EditWindow(User user, HttpClient client)
        {
            InitializeComponent();
            _user = user;
            _client = client;

            TbSurname.Text = user.Surname;
            TbName.Text = user.Name;
            TbPatronymic.Text = user.Patronymic;
            DpBirthday.SelectedDate = user.Birthday;
            _selectedPhotoPath = user.PhotoPath;

            Opened += async (_, _) =>
            {
                await LoadRolesAsync();              
            };
        }

        private async Task LoadRolesAsync()
        {
            _roles = await _client.GetFromJsonAsync<List<Role>>("/api/Posishons") ?? new();
            CbRole.ItemsSource = _roles;
            CbRole.DisplayMemberBinding = new Binding("Name");
            CbRole.SelectedItem = _roles.FirstOrDefault(r => r.Id == _user.Roleid);
        }
        private EmployeeEditDto GetEditDto() => new()
        {
             Surname = TbSurname.Text,
            Name = TbName.Text,
            Patronymic = TbPatronymic.Text,
            Birthday = DpBirthday.SelectedDate?.DateTime,
            Roleid = (CbRole.SelectedItem as Role)?.Id
        };

        private async void BtnSelectPhoto_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog { AllowMultiple = false };
            var result = await dialog.ShowAsync(this);
            if (result?.Length > 0)
            {
                _selectedPhotoPath = result[0];
                using var stream = File.OpenRead(_selectedPhotoPath);
                ImgPhoto.Source = new Bitmap(stream);
            }
        }

        private void BtnSave_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            EditedDto = GetEditDto();
            Close(true);
        }

        private void BtnCancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            EditedDto = null;
            Close(false);
        }
    }
}
