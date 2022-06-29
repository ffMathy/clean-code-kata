using System.Security.Cryptography;
using System.Text;

namespace CleanCodeKata.Readability;

public class UserService
{
    private readonly List<User> _allUsers = new List<User>();
    
    public void UpsertUser(
        string firstName, 
        string lastName,
        string email,
        string plainTextPassword)
    {
        //check if user already exists, and insert one if it doesn't. if it does exist, update it (if something has changed).
        User? existingUser = null;
        bool isIdenticalUser = false;
        foreach (var user in _allUsers)
        {
            if (user.Email == email)
            {
                //generate hashed version of plain text password
                using MD5 md5Hash = MD5.Create();
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(plainTextPassword));
                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                string hashedPassword = sBuilder.ToString();
                
                if (user.Password == hashedPassword)
                {
                    //we found an existing user with the given credentials!
                    existingUser = user;

                    if (user.FirstName == firstName)
                    {
                        if (user.LastName == lastName)
                        {
                            //we don't need to update anything!
                            isIdenticalUser = true;
                        }
                    }
                }
                else
                {
                    existingUser = null;
                }
            }
        }

        if (existingUser != null)
        {
            if (isIdenticalUser)
            {
                return;
            }
            else
            {
                //generate hashed version of plain text password
                using MD5 md5Hash = MD5.Create();
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(plainTextPassword));
                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                string hashedPassword = sBuilder.ToString();
                
                //update the existing user
                existingUser.Email = email;
                existingUser.Password = hashedPassword;
                existingUser.FirstName = firstName;
                existingUser.LastName = lastName;
            }
        }
        else
        {
            //generate hashed version of plain text password
            using MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(plainTextPassword));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            string hashedPassword = sBuilder.ToString();
            
            //create new user
            User newUser = new User();
            newUser.Email = email;
            newUser.Password = hashedPassword;
            newUser.FirstName = firstName;
            newUser.LastName = lastName;

            _allUsers.Add(newUser);
        }
    }
}

#region Outside scope

public class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

#endregion