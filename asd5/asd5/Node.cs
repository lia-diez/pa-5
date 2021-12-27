using System;
using System.Collections.Generic;

namespace asd5
{
    public class Node
    {
        private readonly Player _player;
        private int Height => _player.Board.Height;
        private int Width => _player.Board.Width;
        public readonly int[,] State;
        public int? Value;
        public readonly int Depth;
        public List<Node> Children = null;


        public Node(int[,] newState, Player player, int depth)
        {
            State = newState;
            _player = player;
            Depth = depth;
        }

        public int GetValue(bool isMax)
        {
            int value = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (State[i, j] == 0 && j != Width - 1 && State[i, j + 1] == 0)
                        value++;
                    if (State[i, j] == 0 && i != Height - 1 && State[i + 1, j] == 0)
                        value++;
                }
            }

            if (value == 0) value = Int32.MaxValue;
            else if (value == 1) value = Int32.MinValue;
            else if (value % 2 == 0) value *= 2;
            else  value *= -1;
            return isMax ? value : value * -1;
        }

        public void GetChildren()
        {
            Children = new List<Node>();
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (State[i, j] == 0) 
                    {
                        if (j != Width - 1 && State[i, j + 1] == 0)
                        {
                            Copy(State, out int[,] arr);
                            Node child = new(arr, _player, Depth + 1);

                            child.State[i, j] = _player.Id;
                            child.State[i, j + 1] = _player.Id;
                            Children.Add(child);
                        }

                        if (i != Height - 1 && State[i + 1, j] == 0)
                        {
                            Copy(State, out int[,] arr);
                            Node child = new(arr, _player, Depth + 1);

                            child.State[i, j] = _player.Id;
                            child.State[i + 1, j] = _player.Id;
                            Children.Add(child);
                        }
                    }
                }
            }
        }
        
        public static void Copy (int[,] sourceArray, out int[,] destinationArray)
        {
            destinationArray = new int[sourceArray.GetLength(0), sourceArray.GetLength(1)];
            for (int i = 0; i < destinationArray.GetLength(0); i++)
            {
                for (int j = 0; j < destinationArray.GetLength(1); j++)
                {
                    destinationArray[i, j] = sourceArray[i, j];
                }
            }
        }
        
        public static bool Equals (int[,] first, int[,] second)
        {
            for (int i = 0; i < second.GetLength(0); i++)
            {
                for (int j = 0; j < second.GetLength(1); j++)
                {
                    if (second[i, j] != first[i, j]) return false;
                }
            }

            return true;
        }
        
        public static List<(int, int)> FindDifference (int[,] first, int[,] second)
        {
            List<(int, int)> difference = new List<(int, int)>();
            for (int i = 0; i < second.GetLength(0); i++)
            {
                for (int j = 0; j < second.GetLength(1); j++)
                {
                    if (second[i, j] != first[i, j]) difference.Add((i, j));
                }
            }

            return difference;
        }
    }
}