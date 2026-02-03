using Avalonia.Controls;
using SimpleAppWithApi.DTO;
using SimpleAppWithApi.Models;
using Win;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using System;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using Avalonia.Data;
using System.IO;
using Tmds.DBus.Protocol;
using Avalonia.VisualTree;
using Avalonia.Platform.Storage;
using SimpleAppWithApi.Services;

namespace SimpleAppWithApi;

public partial class MainWindow : Window
{
    private readonly HttpClient _client = new() { BaseAddress = new Uri("http://localhost:5093") };
     private ObservableCollection<User> _users;
     private List<User>? _allUsers;
    public MainWindow()
    {
        InitializeComponent();

        _users = [];
        UserListControl.ItemsSource = _users;

        _=LoadUsers();
        _=LoadRolesAsync();
    }
    private void ApplyFilters()
    {
        if (_allUsers == null)
            return;

        IEnumerable<User> query = _allUsers;

        var text = SearchBox.Text?.ToLower();
        if (!string.IsNullOrWhiteSpace(text))
        {
            query = query.Where(u =>u.Surname.ToLower().Contains(text) ||u.Name.ToLower().Contains(text) ||(u.Patronymic?.ToLower().Contains(text) ?? false));
        }

        if (SortComboBoxRole.SelectedItem is Role role && role.Id != 0)
        query = query.Where(u => u.Roleid == role.Id);
    

        if (DateFromPicker.SelectedDate.HasValue)
            query = query.Where(u => u.Birthday >= DateFromPicker.SelectedDate.Value);

        if (DateToPicker.SelectedDate.HasValue)
            query = query.Where(u => u.Birthday <= DateToPicker.SelectedDate.Value);   

        switch (SortComboBox.SelectedIndex)
        {
            case 1:
                query = query.OrderBy(u => u.Surname);
                break;
            case 2:
                query = query.OrderBy(u => u.Name);
                break;
            case 3:
                query = query.OrderBy(u => u.Birthday);
                break;
        }

        _users.Clear();
        foreach (var user in query)
            _users.Add(user);
}
    private List<Role> _roles = [];
    private async Task LoadRolesAsync()
    {
        var rolesFromApi = await _client.GetFromJsonAsync<List<Role>>("/api/Posishons")?? [];
        _roles =[new Role{Id = 0,Name = "Все"}, .. rolesFromApi];

        SortComboBoxRole.ItemsSource = _roles;
        SortComboBoxRole.DisplayMemberBinding = new Binding("Name");
        SortComboBoxRole.SelectedIndex = 0;
    }

    private async Task LoadUsers()
    {
         var response = await _client.GetAsync("/api/employees");

        var json = await response.Content.ReadAsStringAsync();
        var users = JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];

        _allUsers = users;

        _users.Clear();
        foreach (var user in users)
            _users.Add(user);
    }
    private void SortComboBox_SelectionChanged(object? s, SelectionChangedEventArgs e)=> ApplyFilters();

    private void SearchBox_TextChanged(object? s, TextChangedEventArgs e)=> ApplyFilters();
    private void SortComboBoxRole_SelectionChanged(object? s, SelectionChangedEventArgs e)=> ApplyFilters();
    private void DateChanged(object? s, DatePickerSelectedValueChangedEventArgs  e)=> ApplyFilters();

    #region REST API
    private async Task DELETE(int id)
        {
            var response = await _client.DeleteAsync($"/api/employees/{id}");
            response.EnsureSuccessStatusCode();
            await LoadUsers(); 
        }
    private async Task<CreatedEmployeeResponse> POST(EmployeeCreateDto dto,string? photoPath)
    {
        var content = new MultipartFormDataContent
        {
            { new StringContent(dto.LastName), "LastName" },
            { new StringContent(dto.FirstName), "FirstName" }
        };

        if (!string.IsNullOrEmpty(dto.MiddleName))
        content.Add(new StringContent(dto.MiddleName), "MiddleName");

        content.Add(new StringContent(dto.BirthDate.ToString("O")),"BirthDate");
        content.Add(new StringContent(dto.PositionCode.ToString()), "PositionCode");

        if (!string.IsNullOrEmpty(photoPath))
        {
           var fs = new FileStream(photoPath, FileMode.Open, FileAccess.Read);
           content.Add(new StreamContent(fs), "Photo", Path.GetFileName(photoPath));
        }

        var response = await _client.PostAsync("/api/employees", content);

        var responseText = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<CreatedEmployeeResponse>(responseText)?? throw new Exception("нету ответа");}

    private async Task PUT(int id, EmployeeEditDto updatedUser)
    {
        var json = JsonSerializer.Serialize(updatedUser);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PutAsync($"/api/employees/{id}", content);
        response.EnsureSuccessStatusCode();
        await LoadUsers();
    }
    #endregion
   
    #region кнопки
    private async void BtnAdd_Click(object? sender, RoutedEventArgs e)
    {
    
        var addWindow = new AddWindow(_client);
        await addWindow.LoadRolesAsync();

        var result = await addWindow.ShowDialog<bool?>(this);
        if (result != true || addWindow.CreatedDto == null)
            return;

        //  один запрос — multipart/form-data
        var created = await POST(addWindow.CreatedDto,addWindow.SelectedPhotoPath);

        await LoadUsers();
    
    }
    private async void BtnEdit_Click(object? sender, RoutedEventArgs e)
{
    if (sender is not Button button || button.DataContext is not User user)
        return;

    var editWindow = new EditWindow(user, _client);
    var result = await editWindow.ShowDialog<bool?>(this);

    if (result != true || editWindow.EditedDto == null)
        return;

    await PUT(user.Id, editWindow.EditedDto);

    var photoPath = editWindow.SelectedPhotoPath;
    if (!string.IsNullOrEmpty(photoPath) && photoPath != user.PhotoPath)
    {
        using var fs = File.OpenRead(photoPath);
        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fs), "file", Path.GetFileName(photoPath));

        var response =
            await _client.PostAsync($"/api/employees/{user.Id}/uploadPhoto", content);
        response.EnsureSuccessStatusCode();
    }

    await LoadUsers(); 
}



    private async void BtnDelete_Click(object? sender, RoutedEventArgs e)
    {
  
        if (sender is not Button button || button.DataContext is not User user) return;
        var box = MessageBoxManager.GetMessageBoxStandard("Удаление",$"Подтверждение",ButtonEnum.YesNo);
        var result = await box.ShowWindowDialogAsync(this);
        if (result == ButtonResult.Yes)
        {
            await DELETE(user.Id);
        }
    }
    private async void BtnRefresh_Click(object? sender, RoutedEventArgs e)
    {
        await LoadUsers();
    }
    private void ButtonFiltr_Click(object? s, RoutedEventArgs e)
    {
        SearchBox.Text = "";
        SortComboBox.SelectedIndex = 0;
        SortComboBoxRole.SelectedIndex = 0;
        DateFromPicker.SelectedDate = null;
        DateToPicker.SelectedDate = null;

        ApplyFilters();
    }

    private async void Report_Click(object? s, RoutedEventArgs e)
    {
        var window = this.GetVisualRoot() as Window;
    if (window == null)
        return;

    var file = await window.StorageProvider.SaveFilePickerAsync(
        new FilePickerSaveOptions
        {
            Title = "Сохранить отчёт",
            SuggestedFileName = "EmployeesReport",
            FileTypeChoices =
            [
                new FilePickerFileType("Excel")
                {
                    Patterns = ["*.xlsx"]
                },
                new FilePickerFileType("HTML")
                {
                    Patterns = ["*.html"]
                }
            ]
        });

    if (file == null)
        return;

    var path = file.Path.LocalPath;
    var reportService = new ReportService();

    if (path.EndsWith(".xlsx"))
        reportService.GenerateExcel(path, _users);
    else
        reportService.GenerateHtml(path, _users);
    }
    #endregion
}
