using HW4.Controllers;
using System.Net;

namespace HW4.Models
{
    public class Game
    {
        int appID;
        string name;
        string releaseDate;
        double price;
        string description;
        string headerImage;
        string website;
        bool windows;
        bool mac;
        bool linux;
        int scoreRank;
        string recommendations;
        string publisher;
        int numberOfPurchases;

        public Game(int AppID,
        string Name,
        string ReleaseDate,double Price,string Description,
        string Website,bool Windows,bool Mac,bool Linux,int ScoreRank, string Recommendations, 
        string HeaderImage,
        string Publisher, int NumberOfPurchases) //constractor 
        {
            AppID=appID;
            Name= name;
             ReleaseDate=releaseDate;
             Price=price;
             Description=description;
             HeaderImage=headerImage;
             Website=website;
             Windows=windows;
             Mac=mac;
             Linux=linux;
             ScoreRank=scoreRank;
             Recommendations=recommendations;
             Publisher=publisher;
            NumberOfPurchases=numberOfPurchases;
        }
        /// properties
        public int AppID { get; set; }
        public string Name { get; set; }
        public string ReleaseDate { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string HeaderImage { get; set; }
        public string Website { get; set; }
        public bool Windows { get; set; }
        public bool Mac { get; set; }
        public bool Linux { get; set; }
        public int ScoreRank { get; set; }
        public string Recommendations { get; set; }
        public string Publisher { get; set; }
        public int NumberOfPurchases { get; set; }   
        public Game()//empty ctor
        {
            
        }

        public int insert()
        {
            List<Game> GamesList = new List<Game>();
            GamesList.Add(this);
            DBservices dbs = new DBservices();
            return dbs.InitInsert(GamesList);
        }
        public List<Game> read()
        {
            DBservices dbs = new DBservices();

            return dbs.Read();

           // return GamesList;   
        }
        public List<Game> GetByPrice(double price,int userid) //this method recieves price and returns a list of
                                                   //games whose price is above the given price 
        {
            DBservices dbs = new DBservices();
            return dbs.filterbyPrice(price, userid);
        }

        public List<Game> GetByRankScore(int rank,int userId)//this method recieves price and returns a list of
                                                      //games whose rank is above the given price
        {
            DBservices db = new DBservices();
            return db.filterbyRank(rank, userId);

        }
        public List<Game> DeleteById(int gameid,int userid)
        {
            DBservices dbs = new DBservices();
            return dbs.DeleteGameforUser(gameid, userid);
        }
        public List<Game> readUserGames(int userId)
        {
            DBservices dbs = new DBservices();
            return dbs.usersGamesList(userId);

        }
        public List<Object> gameInfo()
        {
            DBservices dbs = new DBservices();
            return dbs.GetGameinfo();
        }

        /// <summary>
        /// DONT NEED THIS ANYMORE
     /*   static public bool InsertAllGamesOnce(List<Game> AllGames)
        {
            DBservices db = new DBservices();

            int sumOfNumEff = db.InitInsert(AllGames);
            if (sumOfNumEff < 99)
            {
                throw new Exception("not all Games was Inserted");
            }
            else
            {
                return true;
            }
        }
     */
    }

}
