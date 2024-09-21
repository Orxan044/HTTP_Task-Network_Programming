using Data.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace Client;

public partial class MainWindow : Window
{
    HttpClient client = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void BtnGetAll_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var response = await client.GetAsync("http://localhost:27001/");

            var jsonRead = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<ObservableCollection<User>>(jsonRead);


            var task = Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    ListView1.Items.Clear();
                    foreach (var user in users!) ListView1.Items.Add(user);
                });
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

    }
    
    private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var selectedUser = ListView1.SelectedItem as User ?? throw new Exception();

            var updateWindow = new UpdateWindow(selectedUser);
            updateWindow.ShowDialog();

            selectedUser = updateWindow.ReturnUser();

            var json = JsonSerializer.Serialize(selectedUser);
            StringContent content = new(json, Encoding.UTF8, "text/plain");

            string url = $"http://localhost:27001/{selectedUser.Id}";
            HttpResponseMessage response = await client.PutAsync(url, content);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private async void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var selectedUser = ListView1.SelectedItem as User;

            if (selectedUser is not null) 
            {
                var userIdToDelete = selectedUser.Id;
                StringContent content = new(userIdToDelete.ToString(), Encoding.UTF8, "text/plain");

                string url = $"http://localhost:27001/users/{userIdToDelete}";
                HttpResponseMessage response = await client.DeleteAsync(url);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private async void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        var _name = txtBoxName.Text;
        var _surname = txtBoxSurname.Text;
        int _age = int.Parse(txtBoxAge.Text);

        string url = $"http://localhost:27001/users";

        if (_name is not null && _surname is not null && 0 < _age)
        {
            var newUser = new User() { Name = _name, Surname = _surname, Age = _age };

            var jsonUser = JsonSerializer.Serialize(newUser);
            var content = new StringContent(jsonUser);
            _ = await client.PostAsync(url, content);


            txtBoxName.Text = string.Empty;
            txtBoxSurname.Text = string.Empty;
            txtBoxAge.Text = string.Empty;
        }


    }
}