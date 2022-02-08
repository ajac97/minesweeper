using System.Text;

namespace MinesweeperModel
{
    public class Cell
    {
        public bool IsBomb { get; set; }
        
        public int BombCount { get; set; }
        public bool IsFlagged { get; set; }
        
        public bool IsOpen { get; set; }

        public new string ToString()
        {
            if (IsFlagged) return "🚩";
            if (!IsOpen || BombCount == 0) return "";
            if (IsBomb) return "💣";
            return BombCount.ToString();

        }
    }
}