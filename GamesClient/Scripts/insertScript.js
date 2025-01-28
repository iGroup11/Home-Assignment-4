$(document).ready(()=>{
    
    $('#insertBTN').click(()=>{
        const GamesList =[];
        GAME.forEach((game)=>{
            const serverGame = {
                appID: game.AppID,
                name: game.Name,
                releaseDate: new Date(game.Release_date).toISOString(),
                price: game.Price,
                description: game.description,
                headerImage: game.Header_image,
                website: game.Website,
                windows: game.Windows === "TRUE",
                mac: game.Mac === "TRUE",
                linux: game.Linux === "TRUE",
                scoreRank: game.Score_rank,
                recommendations: game.Recommendations,
                publisher: game.Developers 
            };
            GamesList.push(serverGame);
        });
        console.log(GamesList);

        $.ajax({
            url: 'https://localhost:7257/api/Games/InitPostForAllGames',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(GamesList),
            success: function(response) {
                alert('All Games inserted successfully:', response);
            },
            error: function(error) {

                console.error('Error inserting games:', error);
            }
        });

    })
});