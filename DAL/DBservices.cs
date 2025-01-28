using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Xml.Linq;
using HW4.Controllers;
using HW4.Models;
using System.Diagnostics.Eventing.Reader;
using System.Text.Json;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{
    

    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    //--------------------------------------------------------------------------------------------------
    // This method inserts all games to the games table - used it once
    //--------------------------------------------------------------------------------------------------
    public int InitInsert(List<Game> listOfGames)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        int sumOfNumEff = 0;
        for (int i = 0; i < listOfGames.Count; i++)
        {
            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            Game game = listOfGames[i];
            paramDic.Add("@AppID", game.AppID);
            paramDic.Add("@Name", game.Name);
            paramDic.Add("@ReleaseDate", game.ReleaseDate);
            paramDic.Add("@Price", game.Price);
            paramDic.Add("@Description", game.Description);
            paramDic.Add("@HeaderImage", game.HeaderImage);
            paramDic.Add("@Website", game.Website);
            paramDic.Add("@Windows", game.Windows);
            paramDic.Add("@Mac", game.Mac);
            paramDic.Add("@Linux", game.Linux);
            paramDic.Add("@ScoreRank", game.ScoreRank);
            paramDic.Add("@Recommendations", game.Recommendations);
            paramDic.Add("@Publisher", game.Publisher);
            paramDic.Add("@numberOfPurchases", game.NumberOfPurchases);



            cmd = CreateCommandWithStoredProcedureGeneral("SP_InsertGame", con, paramDic);        // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                sumOfNumEff+=numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
                throw (ex);
            }


        }
        if (con != null)
        {
            // close the db connection
            con.Close();
        }
        return sumOfNumEff;

       

    }


    //---------------------------------------------------------------------------------
    // Create the SqlCommand
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureGeneral(String spName, SqlConnection con, Dictionary<string, object> paramDic)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        if (paramDic != null)
            foreach (KeyValuePair<string, object> param in paramDic)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);

            }


        return cmd;
    }



    /// <summary>
    /// /DATA READER FOR TAKING ALL 99 GAMES FROM THE SQL DB
    /// </summary>
    /// <returns></returns>
    public List<Game> Read()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Game> games = new List<Game>();

        cmd = CreateCommandWithStoredProcedureGeneral("SP_ReadGame", con, null);

        try
        {

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Game g = new Game();
                g.AppID = Convert.ToInt32(dataReader["AppID"]);
                g.Name = dataReader["Name"].ToString();
                g.ReleaseDate =dataReader["ReleaseDate"].ToString();
                g.Price = Convert.ToDouble(dataReader["Price"]);
                g.Description= dataReader["Description"].ToString();
                g.HeaderImage = dataReader["HeaderImage"].ToString();
                g.Website = dataReader["Website"].ToString();
                g.Windows = Convert.ToBoolean(dataReader["Windows"]);
                g.Mac = Convert.ToBoolean(dataReader["Mac"]);
                g.Linux = Convert.ToBoolean(dataReader["Linux"]);
                g.ScoreRank = Convert.ToInt32(dataReader["ScoreRank"]);
                g.Recommendations = dataReader["Recommendations"].ToString();
                g.Publisher = dataReader["Publisher"].ToString();
                g.NumberOfPurchases = Convert.ToInt32(dataReader["NumberOfPurchases"]);

                games.Add(g);
            }
            return games;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
  
    // method to bring back a list of games owned by a certain user

    public List<Game> usersGamesList(int userID)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Game> games = new List<Game>();

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId", userID);

        cmd = CreateCommandWithStoredProcedureGeneral("SP_ReadGameofUser_Batel", con, paramDic);

        try
        {

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Game g = new Game();
                g.AppID = Convert.ToInt32(dataReader["AppID"]);
                g.Name = dataReader["Name"].ToString();
                g.ReleaseDate = dataReader["ReleaseDate"].ToString();
                g.Price = Convert.ToDouble(dataReader["Price"]);
                g.Description = dataReader["Description"].ToString();
                g.HeaderImage = dataReader["HeaderImage"].ToString();
                g.Website = dataReader["Website"].ToString();
                g.Windows = Convert.ToBoolean(dataReader["Windows"]);
                g.Mac = Convert.ToBoolean(dataReader["Mac"]);
                g.Linux = Convert.ToBoolean(dataReader["Linux"]);
                g.ScoreRank = Convert.ToInt32(dataReader["ScoreRank"]);
                g.Recommendations = dataReader["Recommendations"].ToString();
                g.Publisher = dataReader["Publisher"].ToString();
                g.NumberOfPurchases = Convert.ToInt32(dataReader["NumberOfPurchases"]);

                games.Add(g);
            }
            return games;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    // method to bring back a list of games owned by a user after deleting a certain game by id

    public List<Game> DeleteGameforUser(int gameid, int userid)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        int sumOfNumEff = 0;


       
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId", userid);
        paramDic.Add("@GameId", gameid);

        cmd = CreateCommandWithStoredProcedureGeneral("SP_DeleteGameofUser_Batel", con, paramDic);

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            sumOfNumEff += numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
            throw (ex);
        }

        if (con != null)
        {
            // close the db connection
            con.Close();
        }
        //after i will delete the game , i will read all the games again 
        List<Game> games = new List<Game>();
        games = usersGamesList(userid);
        return games;

    }

    ///filter user's games by rankscore
    ///
    public List<Game> filterbyRank(int rank, int userid)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Game> games = new List<Game>();


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@userid", userid);
        paramDic.Add("@rank", rank);

        cmd = CreateCommandWithStoredProcedureGeneral("SP_FilterbyRank_Batel", con, paramDic);

        try
        {

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Game g = new Game();
                g.AppID = Convert.ToInt32(dataReader["AppID"]);
                g.Name = dataReader["Name"].ToString();
                g.ReleaseDate = dataReader["ReleaseDate"].ToString();
                g.Price = Convert.ToDouble(dataReader["Price"]);
                g.Description = dataReader["Description"].ToString();
                g.HeaderImage = dataReader["HeaderImage"].ToString();
                g.Website = dataReader["Website"].ToString();
                g.Windows = Convert.ToBoolean(dataReader["Windows"]);
                g.Mac = Convert.ToBoolean(dataReader["Mac"]);
                g.Linux = Convert.ToBoolean(dataReader["Linux"]);
                g.ScoreRank = Convert.ToInt32(dataReader["ScoreRank"]);
                g.Recommendations = dataReader["Recommendations"].ToString();
                g.Publisher = dataReader["Publisher"].ToString();
                g.NumberOfPurchases = Convert.ToInt32(dataReader["NumberOfPurchases"]);

                games.Add(g);
            }
            return games;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    ///filter user's games by price
    ///
    public List<Game> filterbyPrice(double price, int userid)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Game> games = new List<Game>();


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@userid", userid);
        paramDic.Add("@price", price);

        cmd = CreateCommandWithStoredProcedureGeneral("SP_FilterbyPrice_Batel", con, paramDic);

        try
        {

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Game g = new Game();
                g.AppID = Convert.ToInt32(dataReader["AppID"]);
                g.Name = dataReader["Name"].ToString();
                g.ReleaseDate = dataReader["ReleaseDate"].ToString();
                g.Price = Convert.ToDouble(dataReader["Price"]);
                g.Description = dataReader["Description"].ToString();
                g.HeaderImage = dataReader["HeaderImage"].ToString();
                g.Website = dataReader["Website"].ToString();
                g.Windows = Convert.ToBoolean(dataReader["Windows"]);
                g.Mac = Convert.ToBoolean(dataReader["Mac"]);
                g.Linux = Convert.ToBoolean(dataReader["Linux"]);
                g.ScoreRank = Convert.ToInt32(dataReader["ScoreRank"]);
                g.Recommendations = dataReader["Recommendations"].ToString();
                g.Publisher = dataReader["Publisher"].ToString();
                g.NumberOfPurchases = Convert.ToInt32(dataReader["NumberOfPurchases"]);

                games.Add(g);
            }
            return games;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //get userinfo
    public List<Object> GetGameinfo()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        List<object> result = new List<object>();

        cmd = CreateCommandWithStoredProcedureGeneral("SP_ReturnInfoGames", con, null);

        try
        {

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())  // Check if a row is returned
            {
                var userGameInfo = new
                {
                    GameId = Convert.ToInt32(dataReader["AppID"]),
                    Title = dataReader["Name"].ToString(),
                    Downloads = Convert.ToInt32(dataReader["numberOfPurchases"]),
                    TotalRevenue = Convert.ToDouble(dataReader["TotalAmountSpent"])
                };
                result.Add(userGameInfo);
            }

        }

        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
        return result; // Return the Ad-Hoc objects

    }

    //--------------------------------------------------------------------------------------------------
    // FROM HERE ON , ALL THIS CODE IS FOR USERS - NOT GAMES 
    //--------------------------------------------------------------------------------------------------

    ///method for reading all existing users
    ///
    public List<User> ReadUsers()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<User> users = new List<User>();

        cmd = CreateCommandWithStoredProcedureGeneral("SP_ReadUser_Batel", con, null);

        try
        {

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                User u = new User();
                u.Id = Convert.ToInt32(dataReader["Id"]);
                u.Name = dataReader["Name"].ToString();
                u.Email = dataReader["Email"].ToString();
                u.Password = dataReader["Password"].ToString();
                u.IsActive = Convert.ToBoolean(dataReader["isActive"]);


                users.Add(u);
            }
            return users;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }


    ///method to  find user and return him if found

    public User findUser(string usermail, string userpassword)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

      //  List<User> users = new List<User>();
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@Email", usermail);
        paramDic.Add("@Password", userpassword);


        cmd = CreateCommandWithStoredProcedureGeneral("SP_FindUser_Batel", con, paramDic);

        try
        {

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (dataReader.Read())  // Check if a row is returned
            {
                User u = new User
                {
                    Id = Convert.ToInt32(dataReader["Id"]),
                    Name = dataReader["Name"].ToString(),
                    Email = dataReader["Email"].ToString(),
                    Password = dataReader["Password"].ToString(),
                    IsActive= Convert.ToBoolean(dataReader["isActive"])
                };
                return u;
            }
            return null;  // Return null if no user is found


        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

        ///method for inserting loggeduser to loggedusers table
        public int InsertUser(User Newuser)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        int sumOfNumEff = 0;


        ///i want to check first in the users list , if this user exists
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@Name", Newuser.Name);
        paramDic.Add("@Email", Newuser.Email);
        paramDic.Add("@Password", Newuser.Password);



        cmd = CreateCommandWithStoredProcedureGeneral("SP_InsertUser_Batel", con, paramDic);        // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            sumOfNumEff += numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
            throw (ex);
        }

        if (con != null)
        {
            // close the db connection
            con.Close();
        }
        return sumOfNumEff;
    }

    ///method for inserting gameid and userid to usergame table
    public int AddGametoUser(int userId,int gameId)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        int sumOfNumEff = 0;


       
        ///i want to check first in the users list , if this user exists
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@GameId", gameId);
        paramDic.Add("@UserId", userId);



        cmd = CreateCommandWithStoredProcedureGeneral("SP_UserBuysGame_Batel", con, paramDic);        // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            sumOfNumEff += numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
            throw (ex);
        }

        if (con != null)
        {
            // close the db connection
            con.Close();
        }
        return sumOfNumEff;
    }
    //method for updating user details
    public int updateUserDetails(int id, string email, string name, string password)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        int sumOfNumEff = 0;


        ///i want to check first in the users list , if this user exists
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@Id", id);
        paramDic.Add("@Name", name);
        paramDic.Add("@Email", email);
        paramDic.Add("@Password", password);



        cmd = CreateCommandWithStoredProcedureGeneral("SP_UpdateUserInfo_Batel", con, paramDic);        // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            sumOfNumEff += numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
            throw (ex);
        }

        if (con != null)
        {
            // close the db connection
            con.Close();
        }
        return sumOfNumEff;
    }

    //update user isACtive field 
    public int updateisActiveU(int id, bool isActive)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        int sumOfNumEff = 0;


        ///i want to check first in the users list , if this user exists
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@id", id);
        paramDic.Add("@isActive", isActive);


        cmd = CreateCommandWithStoredProcedureGeneral("SP_UpdateisActive", con, paramDic);        // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            sumOfNumEff += numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
            throw (ex);
        }

        if (con != null)
        {
            // close the db connection
            con.Close();
        }
        return sumOfNumEff;
    }
    //get userinfo
    public List<Object> GetUserinfo()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        List<object> result = new List<object>();

        cmd = CreateCommandWithStoredProcedureGeneral("SP_ReturnInfoUsers", con, null);

        try
        {

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())  // Check if a row is returned
            {
                var userGameInfo = new
                {
                    Id = Convert.ToInt32(dataReader["Id"]),
                    UserName= Convert.ToString(dataReader["Name"]),
                    ActiveStatus = Convert.ToBoolean(dataReader["isActive"]),
                    GamesPurchased = Convert.ToInt32(dataReader["NumOfGames"]),
                    MoneySpent= Convert.ToDouble(dataReader["MoneySpent"])
            };
                result.Add(userGameInfo);
            }

        }

        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
        return result; // Return the Ad-Hoc objects

    }

   

}
