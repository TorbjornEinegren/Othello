﻿using System;

namespace TestApplikation
{
    public class Game
    {
        public RulesEngine rulesEngine;
        public PlayerAbstract player1;
        public PlayerAbstract player2;
        public PlayerAbstract currentPlayer;
        private MainWindow mainWindow;
        private Boolean allowMoves;

        public Game(MainWindow mainWindow, RulesEngine rulesEngine)
        {
            this.mainWindow = mainWindow;
            this.rulesEngine = rulesEngine;
            allowMoves = true;
        }

        private void startingCurrentPlayer(PlayerAbstract startingPlayer)
        {
            currentPlayer = startingPlayer;
            currentPlayer.onPlayerChange += mainWindow.textChange;
            currentPlayer.doThings(this);
        }

        public void changeCurrentPlayer()
        {
            if (currentPlayer == player1)
            {
                currentPlayer = player2;
            }
            else
            {
                currentPlayer = player1;
            }
            allowMovesAgain();
            currentPlayer.doThings(this);
        }

        public void allowMovesAgain()
        {
            allowMoves = true;
        }
        
        public void setStartingColor(String colorStr, String playerStr)
        {
            if (playerStr.Equals("AI"))
            {
                if (colorStr.Equals("light"))
                {
                    player1 = new AI("Dator", 1);
                    player2 = new AI("Dumburk", 2);
                    startingCurrentPlayer(player2);
                }
                else if (colorStr.Equals("dark"))
                {
                    player1 = new AI("Dator", 2);
                    player2 = new AI("Dumburk", 1);
                    startingCurrentPlayer(player1);
                }
            }
            else if (playerStr.Equals("Human"))
            {
                if (colorStr.Equals("light"))
                {
                    player1 = new Human("Människa", 1);
                    player2 = new Human("En annan människa", 2);
                    startingCurrentPlayer(player2);
                }
                else if (colorStr.Equals("dark"))
                {
                    player1 = new Human("Människa", 2);
                    player2 = new Human("En annan människa", 1);
                    startingCurrentPlayer(player1);
                }
            }
            else
            {
                if (colorStr.Equals("light"))
                {
                    player1 = new Human("Människa", 1);
                    player2 = new AI("Dumburk", 2);
                    startingCurrentPlayer(player2);
                }
                else if (colorStr.Equals("dark"))
                {
                    player1 = new Human("Människa", 2);
                    player2 = new AI("Dumburk", 1);
                    startingCurrentPlayer(player1);
                }
            }

        }

        public void initateMove(int row, int column)
        {
            if (allowMoves && rulesEngine._move <= 60)
            {
                allowMoves = false;
                rulesEngine.makeMove(row, column, currentPlayer._color);
            }
        }
    }
}
