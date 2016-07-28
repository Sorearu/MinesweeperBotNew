﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperBot
{
    // Singleton class representing the minesweeper gamefield.
    class GameField
    {
        private static GameField instance;
        private Difficulty difficulty;
        private int gameWidth, gameHeight, tileSize, xStart, yStart, gap;
        private int x1, x2, x3, x4, y1, y2, y3, y4;
        private Tile[,] tileField;
        private State state;

        public static GameField Instance
        {           
            get
            {
                if (instance == null)
                {
                    instance = new GameField();
                }
                return instance;
            }
        }

        private GameField()
        {
            if (WindowController.WindowWidth == 170 && WindowController.WindowHeight == 256)
            {
                difficulty = Difficulty.Beginner;
                gameWidth = 9;
                gameHeight = 9;

            }
            else if (WindowController.WindowWidth == 282 && WindowController.WindowHeight == 368)
            {
                difficulty = Difficulty.Intermediate;
                gameWidth = 16;
                gameHeight = 16;
            }
            else if (WindowController.WindowWidth == 506 && WindowController.WindowHeight == 368)
            {
                difficulty = Difficulty.Expert;
                gameWidth = 30;
                gameHeight = 16;
            }

            tileField = new Tile[gameHeight, gameWidth];
  
            xStart = 16;
            yStart = 102;
            tileSize = 15;
            gap = 1;

            x1 = 7;
            y1 = 7;
            x2 = 10;
            y2 = 10;
            x3 = 5;
            y3 = 10;
            x4 = 0;
            y4 = 0;

            state = State.Initial;
        }

        public void Update()
        {
   
            Bitmap screenshot = WindowController.TakeScreenshot();

            for (int i = 0; i < tileField.GetLength(0); i++)
            {
                for(int j = 0; j < tileField.GetLength(1); j++)
                {
                    if (tileField[i, j] == null ||
                        (!tileField[i, j].IsNumber && tileField[i, j].Type != TileType.Empty))
                    {
                        // Calculate position of tile
                        int x = xStart + (j * (tileSize + gap));
                        int y = yStart + (i * (tileSize + gap));

                        // Retrieve colour for specified points of tile
                        Color c1 = screenshot.GetPixel(x + x1, y + y1);
                        Color c2 = screenshot.GetPixel(x + x2, y + y2);
                        Color c3 = screenshot.GetPixel(x + x3, y + y3);
                        Color c4 = screenshot.GetPixel(x + x4, y + y4);

                        // Find tile type and add to array
                        TileType type = TileDetector.DetermineTileType(c1, c2, c3, c4);
                        tileField[i, j] = new Tile(type, x, y, i, j);
                    }
                }
            }

        }

        public Tile Get(int i, int j)
        {
            return tileField[i, j];
        }

        public int Width
        {
            get { return gameWidth; }
        }

        public int Height
        {
            get { return gameHeight; }
        }

        public State State
        {
            get { return state; }
            set { state = value; }
        }


        public enum Difficulty
        {
            Beginner, Intermediate, Expert
        }
    }

    enum State
    {
        Initial, Simple, Complex, Luck
    }

   
}
