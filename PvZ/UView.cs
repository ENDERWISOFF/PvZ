using System;
using System.Drawing;
using System.Windows.Forms;
using UBoard;

namespace UView
{
    public class TView : Panel
    {
        private TBoard FBoard;
        private Button[,] FCells;
        private int FCellWidth;
        private int FCellHeight;
        private ImageList FImageList;

        int BOARD_ROWS = 5;
        int BOARD_COLS = 9;

        public TView()
        {                     
            // Set default cell size
            FCellWidth = 100;
            FCellHeight = 100;

            // Set panel size to match game board size
            Width = BOARD_COLS * FCellWidth;
            Height = BOARD_ROWS * FCellHeight;

            // Create image list for sprites
            FImageList = new ImageList();

            // Create cells
            FCells = new Button[BOARD_ROWS, BOARD_COLS];
            for (int y = 0; y < BOARD_ROWS; y++)
                for (int x = 0; x < BOARD_COLS; x++)
                {
                    FCells[y, x] = new Button();
                    Controls.Add(FCells[y, x]);
                    FCells[y, x].Width = FCellWidth;
                    FCells[y, x].Height = FCellHeight;
                    FCells[y, x].Left = x * FCellWidth;
                    FCells[y, x].Top = y * FCellHeight;
                }
        }

        public void SetImageList(ImageList AImageList)
        {
            FImageList.Images.Clear();
            foreach (Image image in AImageList.Images)
                FImageList.Images.Add(image);
        }

        public void LinkModelView(TBoard ABoard)
        {
            FBoard = ABoard;
            UpdateView();
        }

        public void UpdateView()
        {
            // Get the current state of the game board from the model
            var board = FBoard.GetBoard();

            // Update the sprites on the buttons to match the state of the game board
            for (int y = 0; y < BOARD_ROWS; y++)
            {
                var row = board[y];
                for (int x = 0; x < BOARD_COLS; x++)
                {
                    var node = row.getRow()[x];
                    int spriteIndex;
                    // If there is both a plant and a zombie on this node, set the sprite index based on the zombie's name
                    if (node.hasPlant() && node.hasZombie())
                        if (node.getZombie().getName() == "Zombie1") spriteIndex = 4; else spriteIndex = 7;
                    // If there is both a pea and a zombie on this node, set the sprite index to show a pea hitting a zombie
                    else if (node.hasPea() && node.hasZombie())
                        spriteIndex = 6;
                    // If there is only a plant on this node, set the sprite index based on the plant's name
                    else if (node.hasPlant())
                        if (node.getPlant().getName() == "Plant1") spriteIndex = 1; else spriteIndex = -1;
                    // If there is only a zombie on this node, set the sprite index based on the zombie's name
                    else if (node.hasZombie())
                        if (node.getZombie().getName() == "Zombie1") spriteIndex = 2; else if (node.getZombie().getName() == "Zombie2") spriteIndex = 3; else spriteIndex = -1;
                    // If there is nothing on this node, set the sprite index to show an empty cell
                    else
                        spriteIndex = 0;
                    // If there is only a pea on this node, set the sprite index to show a pea flying through the air
                    if (node.hasPea() && !(node.hasPlant() || node.hasZombie())) spriteIndex = 5;
                    // Set the glyph of this cell's button to show the appropriate sprite from the FImageList image list based on the calculated sprite index
                    if (spriteIndex >= 0)
                        FCells[y, x].BackgroundImage = FImageList.Images[spriteIndex];
                }
            }
        }

        public Panel GetPanel()
        {
            return this;
        }

        public Button[,] GetCells()
        {
            return FCells;
        }
    }
}
