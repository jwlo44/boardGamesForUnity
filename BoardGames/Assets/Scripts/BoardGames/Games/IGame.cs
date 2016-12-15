using BoardGames.Games.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoardGames.Games
{
    public interface IGame
    {
        bool isOver();
        Player getActivePlayer();
        Player getWinner();
        void takeTurn();
        void init();
    }
}
