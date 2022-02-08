using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Collections.Generic;
using System;

namespace MinesweeperModel.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestSetNeighbors()
        {
            Model m = new Model(DifficultyLevel.Easy);
            m.bombLocations = new List<Point>();
            
            m.board[0, 0] = new Cell() { IsBomb = true };
            m.bombLocations.Add(new Point(0, 0));
            m.bombLocations.Add(new Point(0, 1));
            
            m.SetNeighborCount();

            m.board[1, 0].BombCount.Should().Be(2);
            m.board[1, 1].BombCount.Should().Be(2);
        }

        [TestMethod]
        public void TestOpenCells()
        {
            Model m = new Model(DifficultyLevel.Easy);
            SetupBoardWithBombsTopRow(m);

            m.SetNeighborCount();

            List<Point> toOpen = m.OpenCell(0, 1);

            Assert.AreEqual(1, toOpen.Count);
            Assert.AreEqual(2, m.board[toOpen[0].Y, toOpen[0].X].BombCount);

        }

        [TestMethod]
        public void TestOpenZeros()
        {
            Model m = new Model(DifficultyLevel.Easy);
            SetupBoardWithBombsTopRow(m);

            m.SetNeighborCount();

            List<Point> toOpen = m.OpenCell(9, 7);

            Assert.AreEqual(70, toOpen.Count); // will open everything but top row

        }


        [TestMethod]
        public void TestGameWon()
        {
            Model m = new Model(DifficultyLevel.Easy);
            SetupBoardWithBombsTopRow(m);

            m.SetNeighborCount();

            m.OpenCell(9, 7);

            Assert.IsTrue(m.GameWon);

        }

        [TestMethod]
        public void TestFlagCount()
        {
            Model m = new Model(DifficultyLevel.Easy);
            m.FlagCell(0, 0);
            m.FlagCell(0, 1);
            Assert.IsTrue(m.GetCell(0, 0).IsFlagged);
            Assert.IsTrue(m.GetCell(0, 1).IsFlagged);
            Assert.IsTrue(m.FlagCount == m.GetDifficultyLevel().GetMineCount() - 2);
        }

        [TestMethod]
        public void TestUnflag()
        {
            Model m = new Model(DifficultyLevel.Easy);
            m.FlagCell(0, 0);
            m.FlagCell(0, 1);
            Assert.IsTrue(m.GetCell(0, 0).IsFlagged);
            Assert.IsTrue(m.GetCell(0, 1).IsFlagged);
            m.FlagCell(0, 0);
            m.FlagCell(0, 1);
            Assert.IsFalse(m.GetCell(0, 0).IsFlagged);
            Assert.IsFalse(m.GetCell(0, 1).IsFlagged);
            Assert.IsTrue(m.FlagCount == m.GetDifficultyLevel().GetMineCount());
        }

        [TestMethod]
        public void TestGameLost()
        {
            Model m = new Model(DifficultyLevel.Easy);
            SetupBoardWithBombsTopRow(m);

            m.SetNeighborCount();

            m.OpenCell(0, 0);

            Assert.IsTrue(m.GameLost);

        }
        private void SetupBoardWithBombsTopRow(Model m)
        {
            m.bombLocations = new List<Point>();
            for (int i = 0; i <m.GetDifficultyLevel().GetSize().X; i++)
            {
                m.bombLocations.Add(new Point(i, 0));
                m.board[0, i].IsBomb = true;
            }
        }
    }
}
