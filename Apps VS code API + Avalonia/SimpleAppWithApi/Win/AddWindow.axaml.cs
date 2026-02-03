using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using SimpleAppWithApi.DTO;
using SimpleAppWithApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Win;

public partial class AddWindow : Window
{
    private readonly HttpClient _client;
    private List<Role> _roles = [];

    public AddWindow(HttpClient client)
    {
        InitializeComponent();
        _client = client;
    }

    public async Task LoadRolesAsync()
    {
        _roles = await _client.GetFromJsonAsync<List<Role>>("/api/Posishons") ?? [];
        CbRole.ItemsSource = _roles;
        CbRole.DisplayMemberBinding = new Binding("Name");

        if (_roles.Count > 0)
            CbRole.SelectedIndex = 0;
    }

    public EmployeeCreateDto GetCreateDto()
    {
        if (!DpBirthday.SelectedDate.HasValue){
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Ошибка","Выберите дату рождения", ButtonEnum.Ok);
            _ = messageBox.ShowWindowDialogAsync(this);}


        var role = CbRole.SelectedItem as Role ?? throw new Exception("Не выбрана должность");
        return new EmployeeCreateDto
        {
            LastName = TbSurname.Text.Trim(),
            FirstName = TbName.Text.Trim(),
            MiddleName = string.IsNullOrWhiteSpace(TbPatronymic.Text) ? null : TbPatronymic.Text.Trim(),
            BirthDate = DateOnly.FromDateTime(DpBirthday.SelectedDate.Value.DateTime),
            PositionCode = role.Id
            
        };
    }

    private void BtnSave_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            CreatedDto = GetCreateDto();
            Close(true);
        }
        catch (Exception ex)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", ex.Message, ButtonEnum.Ok);
            _ = messageBox.ShowWindowDialogAsync(this);
        }
    }
    private void BtnCancel_Click(object? sender, RoutedEventArgs e) => Close(false);
    public async void BtnSelectPhoto_Click(object? s,RoutedEventArgs f)
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
    public EmployeeCreateDto? CreatedDto { get; private set; }
    public string? SelectedPhotoPath => _selectedPhotoPath;
    private string? _selectedPhotoPath;
}
