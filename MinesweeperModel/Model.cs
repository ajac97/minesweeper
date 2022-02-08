using System;
using System.Collections.Generic;
using System.Drawing;

namespace MinesweeperModel
{


    public class Model : IModel
    {
        //imp
        internal Cell[,] board;

        internal List<Point> bombLocations = new List<Point>();

        private DifficultyLevel level;

        public int FlagCount { get; private set; }

        public bool GameWon { get; private set; }

        public bool GameLost { get; private set; }

        private int openCells;

        public Model(DifficultyLevel level)
        {
            this.level = level;
            Setup(level);
        }

        public DifficultyLevel GetDifficultyLevel()
        {
            return level;
        }
        
        public Cell GetCell(int col, int row)
        {
            return board[row, col];
        }
        public void FlagCell(int col, int row)
        {
            Cell cell = GetCell(col, row);
            if(cell.IsOpen) return;
            if (cell.IsFlagged) FlagCount++;
            else FlagCount--;
            cell.IsFlagged = !cell.IsFlagged;
        }
        

        public void Setup(DifficultyLevel level)
        {
            openCells = 0;
            GameLost = false;
            GameWon = false;
            this.level = level;
            Point size = level.GetSize();
            int rows = size.Y;
            int cols = size.X;
            board = new Cell[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    board[i, j] = new Cell(){ IsOpen = false, IsFlagged = false};
                }
            }

            bombLocations = new List<Point>();
            FlagCount = level.GetMineCount();
        }

        internal void SetNeighborCount()
        {
            foreach (Point p in bombLocations)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <=1; j++)
                    {
                        int r = p.Y + i;
                        int c = p.X + j;
                        if(!InBounds(c, r)) continue;
                        if (!board[r, c].IsBomb)
                        {
                            board[r, c].BombCount++;
                        }
                    }
                }
            }
        }

        private bool InBounds(int col, int row)
        {
            return ((col >= 0 && col < level.GetSize().X) && (row >= 0 && row < level.GetSize().Y));
        }

        public List<Point> OpenCell(int col, int row)
        {
            Cell cell = GetCell(col, row);
            var toOpen = new List<Point>();
            if (cell.IsFlagged) return toOpen;
            if (cell.IsOpen) return toOpen;
            if (bombLocations.Count == 0)
            {
                InitialClick(row, col);
            }

            if (cell.IsBomb)
            {
                GameLost = true;
                foreach (var p in bombLocations)
                {
                    if(board[p.Y, p.X].IsFlagged) FlagCell(p.X, p.Y);
                    board[p.Y, p.X].IsOpen = true;
                }
                return bombLocations;
                
            }
            
            toOpen.Add(new Point(col,row));
            cell.IsOpen = true;
            if (cell.BombCount == 0)
            {
                    toOpen.AddRange(OpenZeros(col, row));
            }

            openCells += toOpen.Count;

            checkIfWon();
            
            return toOpen;
        }

        private void checkIfWon()
        {
            int totalSpots = (level.GetSize().Y * level.GetSize().X);
            GameWon = (openCells == totalSpots - level.GetMineCount());
            
        }

        private List<Point> OpenZeros(int col, int row)
        {
            List<Point> toOpen = new List<Point>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if(!InBounds(col+j, row+i ) || (row+i==row && col+j == col)) continue;

                    Cell cell = GetCell(col + j, row + i);
                    
                    if ((!cell.IsOpen && !cell.IsBomb))
                    {
                        toOpen.Add(new Point( col + j, row + i));
                        if (cell.IsFlagged) FlagCell(col + j, row + i);
                        cell.IsOpen = true;
                    }
                }    
            }
            

            for (int i =0; i < toOpen.Count; i++)
            {
                Point p = toOpen[i];
                Cell cell = GetCell(p.X, p.Y);
                if (cell.BombCount == 0)
                {
                    toOpen.AddRange(OpenZeros(p.X, p.Y));
                }
            }
            return toOpen;
        }

        private void InitialClick(int row, int col)
        {
            Random rand = new Random();
            int mineCount = level.GetMineCount();
            for (int i = 0; i < mineCount; i++)
            {
                int r = rand.Next(level.GetSize().Y);
                int c = rand.Next(level.GetSize().X);
                while ((r == row && c == col) || (bombLocations.Contains(new Point(c, r))) )
                {
                    r = rand.Next(level.GetSize().Y);
                    c = rand.Next(level.GetSize().X);
                }

                board[r, c].IsBomb = true;
                board[r, c].BombCount = 50;
                bombLocations.Add(new Point(c, r));
            }
            SetNeighborCount();
        }


    }
}
