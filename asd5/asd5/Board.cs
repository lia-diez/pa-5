namespace asd5
{
    public class Board
    {
        public readonly int Height;
        public readonly int Width;
        public Player Player;
        public int[,]? CurrentState;

        public Board(int height, int width, int maxDepth)
        {
            Width = width;
            Height = height;
            CurrentState = new int[height,width];
            Player = new Player(this, maxDepth, 3);
        }
    }
}