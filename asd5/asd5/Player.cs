using System;

namespace asd5
{
    public class Player
    {
        public readonly Board Board;
        private Node? _decisionTreeRoot;
        private readonly int _maxDepth;
        private Node _nextStep;
        public readonly int Id;


        public Player(Board board, int maxDepth, int id)
        {
            Board = board;
            _maxDepth = maxDepth;
            Id = id;
        }

        public bool MakeStep()
        {
            _decisionTreeRoot = new Node(Board.CurrentState, this, 0);
            Node current = _decisionTreeRoot;
            int alpha = Int32.MinValue;
            int beta = Int32.MaxValue;
            Max(current, alpha, beta);
            Board.CurrentState = _nextStep.State;
            if (Node.Equals(current.State,_nextStep.State)) return false;
            return true;
        }
        
        private int Max(Node current, int alpha, int beta)
        {
            if (current.Depth == _maxDepth)
            {
                return current.GetValue(true);
            }

            bool haveChildren = false;
            int maxValue = Int32.MinValue;
            current.GetChildren();
            if (current.Depth == 0 && current.Children.Count != 0)
            {
                _nextStep = current.Children[0];
                _nextStep.Value = maxValue;
            }

            foreach (var child in current.Children)
            {
                int newValue = Min(child, alpha, beta);
                if (newValue > maxValue)
                {
                    maxValue = newValue;
                }

                child.Value = newValue;
                if (current.Depth == 0 && maxValue > _nextStep.Value) _nextStep = child;
                if (newValue >= beta) return maxValue;
                if (newValue > alpha) alpha = newValue;
                haveChildren = true;
            }

            if (current.Depth == 0 && !haveChildren) _nextStep = current;
            return maxValue;
        }
        
        private int Min(Node current, int alpha, int beta)
        {
            if (current.Depth == _maxDepth)
            {
                return current.GetValue(false);
            }
            
            bool haveChildren = false;
            int minValue = Int32.MaxValue;
            current.GetChildren();
            if (current.Depth == 0 && current.Children.Count != 0)
            {
                _nextStep = current.Children[0];
                _nextStep.Value = minValue;
            }
            
            foreach (var child in current.Children)
            {
                int newValue = Max(child, alpha, beta);
                if (newValue < minValue)
                {
                    minValue = newValue;
                }

                child.Value = newValue;
                if (current.Depth == 0 && minValue < _nextStep.Value) _nextStep = child;
                if (newValue <= alpha) return minValue;
                if (newValue < beta) beta = newValue;
                haveChildren = true;
            }

            if (current.Depth == 0 && !haveChildren) _nextStep = current;
            return minValue;
        }

    }
}