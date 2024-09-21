using Data.Models;
using System.Windows;

namespace Client;

public partial class UpdateWindow : Window
{
    private User _user = new();

    public UpdateWindow(User user)
    {
        InitializeComponent();
        _user = user;

        txtBoxName.Text = _user.Name;
        txtBoxSurname.Text = _user.Surname;
        txtBoxAge.Text = $"{_user.Age}";

    }



    public void UpdateUser()
    {
        try
        {
            if (txtBoxName is not null && txtBoxSurname is not null && txtBoxAge is not null)
            {
                var _age = int.Parse(txtBoxAge.Text);

                _user.Name = txtBoxName.Text;
                _user.Surname = txtBoxSurname.Text;
                _user.Age = _age;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    public User ReturnUser()
    {
        return _user;
    }

    private void BtnUpdate_Click(object sender, RoutedEventArgs e)
    {
        UpdateUser();
    }
}
