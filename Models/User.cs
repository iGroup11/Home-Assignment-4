using System.Diagnostics.Eventing.Reader;
using System.Text.Json;

namespace HW4.Models
{
    public class User
    {
        int id;
        string name;
        string email;
        string password;
        bool isActive;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public User(int id,string name,string email,string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            IsActive = true;
        }

        public User() //empty ctor
        {
            
        }
        public List<User> UsersList = new List<User>(); //creating user list

        DBservices dbs = new DBservices();

        public int insert()
        {
            return dbs.InsertUser(this);
        }

        public List<User> read() 
        {
            return dbs.ReadUsers();
        }
        /*  public User searchUser(string email)
          {

          DBservices dbs = new DBservices();
             return dbs.findUser(email);
          }
        */


        /*
      public int updateUserDetails(int id, string email, string name, string password)
        {
            return dbs.updateUserDetails(id, email, name, password);
        }
        public User searchUser(string email,string password)
        {

            return dbs.findUser(email, password);
        }
        */
        public int updateUserDetails(int id)
        {
            return dbs.updateUserDetails(id, this.Email, this.Name, this.Password);
        }
        public User searchUser()
        {

            return dbs.findUser(this.Email, this.Password);
        }

        public int AddGametoUser(int uId, int gId)
        {
            return dbs.AddGametoUser(uId, gId);

        }
        public int updateisActive(int id, bool isActive)
        {
            return dbs.updateisActiveU(id, isActive);
        }

        public List<Object> userInfo()
        {
            return dbs.GetUserinfo();
        }
    }
}
