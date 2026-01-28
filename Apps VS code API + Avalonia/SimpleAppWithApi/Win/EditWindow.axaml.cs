using Avalonia.Controls;
using Avalonia.Data;
using SimpleAppWithApi.DTO;
using SimpleAppWithApi.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using System.Threading.Tasks;

namespace Win;

public partial class EditWindow : Window
{
    private readonly User _user;
    private readonly HttpClient _client;
    private List<Role> _roles = new();
    public Role? SelectedRole { get; set; }

    public EditWindow(User user, HttpClient client)
    {
        InitializeComponent();
        _user = user;
        _client = client;
        DataContext = this;

        TbSurname.Text = user.Surname;
        TbName.Text = user.Name;
        TbPatronymic.Text = user.Patronymic;
        DpBirthday.SelectedDate = user.Birthday;
    }

    public async Task LoadRolesAsync()
    {
        _roles = await _client.GetFromJsonAsync<List<Role>>("/api/Posishons") ?? [];
        CbRole.ItemsSource = _roles;
        CbRole.DisplayMemberBinding = new Binding("Name");
        SelectedRole = _roles.FirstOrDefault(r => r.Id == _user.Roleid);
        CbRole.SelectedItem = SelectedRole;
    }
    public EmployeeEditDto GetEditDto()
    {
        var role = CbRole.SelectedItem as Role;
        return new EmployeeEditDto
        {
            LastName = TbSurname.Text,
            FirstName = TbName.Text,
            MiddleName = TbPatronymic.Text,
            BirthDate = DpBirthday.SelectedDate?.DateTime,
            PositionCode = role?.Id
        };
    }

    private void BtnSave_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) => Close(true);
}
