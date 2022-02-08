using System.Collections.Generic;

namespace MinesweeperModel
{
    // Reference Game: https://www.google.com/search?q=minesweeper&rlz=1C1EJFC_enUS867US867&oq=mine&aqs=chrome.0.69i59j46i67i433j46i67j46i67i433j0i20i263i433i512j46i433i512l2j69i60.1199j0j7&sourceid=chrome&ie=UTF-8
    // There is much more to the IModel than what we completed in class
    // the first casualty of war is the plan
    public interface IModel
    {
        //ops
        /// <summary>
        /// Opens Cell
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns>List of Points that changed due to this operation. If Bomb, that point will be only returned value</returns>
        List<System.Drawing.Point> OpenCell(int col, int row);
        void Setup(DifficultyLevel level);

        Cell GetCell(int row, int col);
        void FlagCell(int col, int row);

    }
}