
using Data.Context;
using Data.Models;
using Data.Repositories;
using System.Net;
using System.Text.Json;

var _port = 27001;

var _listener = new HttpListener();

_listener.Prefixes.Add($"http://localhost:{_port}/");
_listener.Start();

Console.WriteLine($"Http server started on -> {_port}");

var dataContext = new UserDbContext(); 
var _userRepository = new Repository<User, UserDbContext>(dataContext);


while (true)
{
    var context = _listener.GetContext();

    var request = context.Request;
    var response = context.Response;

    using var writer = new StreamWriter(response.OutputStream);
    using var reader = new StreamReader(request.InputStream);

    if (request.HttpMethod == HttpMethod.Get.Method) GetAllUser(_userRepository, writer);

    else if (request.HttpMethod == HttpMethod.Put.Method) UpdateUser(_userRepository, reader);

    else if (request.HttpMethod == HttpMethod.Delete.Method) DeleteUser(_userRepository, request);

    else if (request.HttpMethod == HttpMethod.Post.Method) AddUser(_userRepository, reader); 

    
}

static void GetAllUser(Repository<User,UserDbContext> userRepository , StreamWriter writer)
{
    Console.WriteLine($"Client Command -> {HttpMethod.Get}");
    try
    {
        var users = userRepository.GetAll().ToList();
        var jsonUsers = JsonSerializer.Serialize(users);
 
        writer.WriteLine(jsonUsers);
        writer.Flush();

        Console.WriteLine("All User Send Client");
    }
    catch (Exception)
    {
        Console.WriteLine("Cannot Get All User");
    }
}

static async void UpdateUser(Repository<User, UserDbContext> userRepository, StreamReader reader)
{
    Console.WriteLine($"Client Command -> {HttpMethod.Put}");

    try
    {
        var body = await reader.ReadToEndAsync();
        var updatedUser = JsonSerializer.Deserialize<User>(body);

        if (updatedUser is not null)
        {
            var existingUser = userRepository.Get(updatedUser.Id);
            if (existingUser is not null)
            {
                existingUser.Name = updatedUser.Name;
                existingUser.Surname = updatedUser.Surname;
                existingUser.Age = updatedUser.Age;

                userRepository.SaveChanges();
                Console.WriteLine("User updated from Client.");
            }
        }
    }
    catch (Exception)
    {
        Console.WriteLine("Cannot User Update from Client");
    }

}

static void DeleteUser(Repository<User, UserDbContext> userRepository, HttpListenerRequest request)
{
    Console.WriteLine($"Client Command -> {HttpMethod.Delete}");

    try
    {

        var rawUrl = request.RawUrl;
        var segments = rawUrl!.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length > 0)
        {
            string lastSegment = segments[^1];
            if (int.TryParse(lastSegment, out int userId))
            {
                userRepository.Delete(userId);
                userRepository.SaveChanges();
                Console.WriteLine("Delete User from Client");
            }
        }
    }
    catch (Exception)
    {
        Console.WriteLine("Cannot Delete User from Client");
    }
}



static void AddUser(Repository<User, UserDbContext> userRepository, StreamReader reader)
{
    Console.WriteLine($"Client Command -> {HttpMethod.Post}");
    try
    {
        var newUser = JsonSerializer.Deserialize<User>(reader.ReadToEnd());

        if (newUser is not null) 
        {
            userRepository.Add(newUser);
            userRepository.SaveChanges();
        }
        Console.WriteLine("Adding User from Client");
    }
    catch (Exception)
    {
        Console.WriteLine("Cannot User Add from Client");
    }
}