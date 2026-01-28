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

namespace SimpleAppWithApi;

public partial class MainWindow : Window
{
    private readonly HttpClient _client = new HttpClient { BaseAddress = new Uri("http://localhost:5093") };
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
    #region Sort
    private void SortComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        IEnumerable<User> sorted = _allUsers;

        switch (SortComboBox.SelectedIndex)
        {
            case 1:
            sorted = _allUsers.OrderBy(u => u.Surname);
                break;
            case 2:
            sorted = _allUsers.OrderBy(u => u.Name);
                break;
            case 3:
            sorted = _allUsers.OrderBy(u => u.Birthday);
                break;
        }

        _users.Clear();
        foreach (var user in sorted)
            _users.Add(user);
    }

    private void SearchBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        var text = SearchBox.Text?.ToLower() ?? "";

        var filtered = _allUsers.Where(u =>u.Surname.ToLower().Contains(text) ||u.Name.ToLower().Contains(text) ||(u.Patronymic?.ToLower().Contains(text) ?? false));

        _users.Clear();
            foreach (var user in filtered)
            _users.Add(user);
    }

    public void SortComboBoxRole_SelectionChanged(object? s,SelectionChangedEventArgs r)
    {
        if (_allUsers == null || SortComboBoxRole.SelectedItem is not Role selectedRole)
            return;

        IEnumerable<User> filtered;

        if (selectedRole.Id == 0)
        {
       
            filtered = _allUsers;
        }
        else
        {
            filtered = _allUsers.Where(u => u.Roleid  == selectedRole.Id);
        }

        _users.Clear();
        foreach (var user in filtered)
            _users.Add(user);
    }
    private List<Role> _roles = new();

#endregion
    #region  ЗАгрузка данных
private async Task LoadRolesAsync()
{
    var rolesFromApi =
        await _client.GetFromJsonAsync<List<Role>>("/api/Posishons")
        ?? [];

    _roles =
    [
        new Role
        {
            Id = 0,
            Name = "Все"
        },
        .. rolesFromApi,
    ];

    SortComboBoxRole.ItemsSource = _roles;
    SortComboBoxRole.DisplayMemberBinding = new Binding("Name");
    SortComboBoxRole.SelectedIndex = 0;
}

    private async Task LoadUsers()
    {
         var response = await _client.GetAsync("/api/employees");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var users = JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];

        _allUsers = users;

        _users.Clear();
        foreach (var user in users)
            _users.Add(user);
    }
    #endregion
    #region REST API
    private async Task DELETE(int id)
        {
            var response = await _client.DeleteAsync($"/api/employees/{id}");
            response.EnsureSuccessStatusCode();
            await LoadUsers(); 
        }
    private async Task POST(EmployeeCreateDto newUser)
    {
        var json = JsonSerializer.Serialize(newUser);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/employees", content);
        response.EnsureSuccessStatusCode();
        await LoadUsers();
    }

    private async Task PUT(int id, EmployeeEditDto updatedUser)
    {
        var json = JsonSerializer.Serialize(updatedUser);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PutAsync($"/api/employees/{id}", content);
        response.EnsureSuccessStatusCode();
        await LoadUsers();
    }
   #endregion
   
    private async void BtnAdd_Click(object? sender, RoutedEventArgs e)
    {
       var addWindow = new AddWindow(_client);
        await addWindow.LoadRolesAsync();

    var result = await addWindow.ShowDialog<bool?>(this);
    if (result == true)
    {
       
        var dto = addWindow.GetCreateDto();
        await POST(dto);
    }
    }

    private async void BtnEdit_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button || button.DataContext is not User user) return;

    var editWindow = new EditWindow(user, _client);
    await editWindow.LoadRolesAsync();
    var result = await editWindow.ShowDialog<bool?>(this);
    if (result == true)
    {
        var dto = editWindow.GetEditDto();
        await PUT(user.Id, dto);
    }
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
}
