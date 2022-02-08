using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MinesweeperModel;

namespace Minesweeper5
{
    public partial class Form1 : Form
    {
        
        private Button[,] buttons;
        private Model model = new Model(DifficultyLevel.Easy);
        private int RowCount;
        private int ColCount; //model.GetDifficultyLevel().GetSize().X , ColCount = 
        
        public Form1()
        {
            InitializeComponent();

            InitializeComponent2();
        }

        private void InitializeComponent2()
        {
            RowCount = model.GetDifficultyLevel().GetSize().Y;
            ColCount = model.GetDifficultyLevel().GetSize().X;
            this.comboBox1.SelectedIndex = 0;
            MaximizeBox = false;
            MinimizeBox = false;

        }
        
        
        private void difficultyLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            DifficultyLevel dl = (DifficultyLevel)((ComboBox)sender).SelectedIndex;
            model.Setup(dl);
            GridSetup(dl);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void OnButtonClick(object sender, MouseEventArgs e)
        {
            Button sourceButton = (Button)sender;
            Point p = (Point) sourceButton.Tag;
            if (e.Button == MouseButtons.Right)
            {
                model.FlagCell(p.X, p.Y);
                buttons[p.Y, p.X].Text = model.GetCell(p.X, p.Y).ToString();
                label2.Text = model.FlagCount.ToString();
                return;
            }
            var toOpen = model.OpenCell(p.X, p.Y);

            foreach (var point in toOpen)
            {
                buttons[point.Y,point.X].Text = model.GetCell(point.X,point.Y).ToString();
                buttons[point.Y, point.X].BackColor = Color.Azure;
            }

            label2.Text = model.FlagCount.ToString();
            CheckGameOver();
        }

        private void CheckGameOver()
        {
            bool isGameOver = false;
            if (model.GameWon)
            {
                isGameOver = true;
                label3.Text = "You Won!";

            }
            if (model.GameLost)
            {
                isGameOver = true;
                label3.Text = "You Lose!";
            }
            if (isGameOver)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    for (int j = 0; j < ColCount; j++)
                    {
                        buttons[i, j].Enabled = false;
                    }
                }

                button1.Visible = true;
            }
        }

        private void New_Game_Button_Click(object sender, EventArgs e)
        {
            DifficultyLevel dl = model.GetDifficultyLevel();
            model.Setup(dl); ;
            GridSetup(dl);
        }

        private void GridSetup(DifficultyLevel dl)
        {

            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();

            RowCount = dl.GetSize().Y;
            ColCount = dl.GetSize().X;
            this.tableLayoutPanel1.Controls.Clear();
            this.tableLayoutPanel1.ColumnStyles.Clear();
            this.tableLayoutPanel1.RowStyles.Clear();


            // Resize Window not allowed
            button1.Visible = false;
            label2.Text = dl.GetMineCount().ToString();
            label3.Text = "";
            buttons = new Button[RowCount, ColCount];
            bool color = false;
            for (int r = 0; r < RowCount; r++)
                for (int c = 0; c < ColCount; c++)
                {
                    buttons[r, c] = new System.Windows.Forms.Button();
                    buttons[r, c].Dock = DockStyle.Fill;
                    // TabIndex in order 0..RowCount*ColCount-1
                    this.tableLayoutPanel1.Controls.Add(buttons[r, c], c, r);
                    buttons[r, c].Text = "";
                    buttons[r, c].UseVisualStyleBackColor = true;
                    buttons[r, c].MouseDown += OnButtonClick;
                    buttons[r, c].Tag = new Point(c, r);
                    //  buttons[r, c].BackColor = color ? Color.Green : Color.GreenYellow;
                    color = !color;
                }

            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ColumnCount = ColCount;
            for (int c = 0; c < ColCount; c++)
            {
                this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / ColCount));
            }


            this.tableLayoutPanel1.RowCount = RowCount;
            for (int r = 0; r < RowCount; r++)
                this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / RowCount));

            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(true);
            this.ResumeLayout(true);

            //Refresh();
        }


    }
}
