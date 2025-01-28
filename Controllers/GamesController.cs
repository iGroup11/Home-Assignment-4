using HW4.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HW4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        // GET: api/<GamesController>
        [HttpGet]
        public IEnumerable<Game> Get()
        {
            Game game = new Game();
            return game.read();
        }

        // GET api/<GamesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("SearchByRankScore")] ///resource routing for rank score
        public IEnumerable<Game> GetByRankScore(int rank,int userid) 
        {
            Game game = new Game();
            return game.GetByRankScore(rank, userid);
        }
        [HttpGet("SearchByPrice")] ///QueryString for rank score
        public IEnumerable<Game> GetByPrice(double price,int userid)
        {
            Game game = new Game();
            return game.GetByPrice(price,userid);
        }
        /// <summary>
        /// / this is the code for inserting all games - DONT NEED IT ANYMORE
        /// </summary>
        /*
            [HttpPost("InitPostForAllGames")]
            public bool Post([FromBody] List<Game> games)
            {
                return Game.InsertAllGamesOnce(games);
            }
 */
  

        // POST api/<GamesController>
        [HttpPost]
        public int Post([FromBody] Game game)
        {
            return game.insert();   
        }

        // PUT api/<GamesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GamesController>/5
        [HttpDelete("Deletebyid")]
        public IEnumerable<Game> Delete(int gameid, int userid)
        {
            Game game = new Game();
            return game.DeleteById( gameid, userid);
        }
        [HttpGet("UserGames")]
        public IEnumerable<Game> GetUserGames(int userId)
        {
            Game game = new Game();
            return game.readUserGames(userId);
        }
        [HttpGet("GetGameInfo")]
        public List<Object> gameInfo()
        {
            Game game = new Game();
            return game.gameInfo();

        }
    }
}
