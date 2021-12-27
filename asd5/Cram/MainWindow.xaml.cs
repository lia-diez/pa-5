using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using asd5;

namespace Cram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Button[,] _buttons;
        private List<(int, int)> _coordinates;
        private List<Brush> _prevColors;
        private int _difficulty = 2;
        private Board _board;
        private int _height;
        private int _width;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SetClick(object sender, RoutedEventArgs e)
        {
            int.TryParse(TextHeight.Text, out _height);
            int.TryParse(TextWidth.Text, out _width);
            if (_height <= 1 || _height > 8 || _width <= 1 || _width > 8)
            {
                TextHeight.Text = "6";
                TextWidth.Text = "6";
            }
            else
                InitMatrix();
        }
        
        private void RadioButton1Checked(object sender, RoutedEventArgs e)
        {
            _difficulty = 2;
        }
        
        private void RadioButton2Checked(object sender, RoutedEventArgs e)
        {
            _difficulty = 3;
        }
        
        private void RadioButton3Checked(object sender, RoutedEventArgs e)
        {
            _difficulty = 5;
        }

        private void InitMatrix()
        {
            UniformGrid matrix = new UniformGrid();
            matrix.Columns = _width;
            matrix.Rows = _height;
            _buttons = new Button[_height, _width];
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    Button button = new Button();
                    button.Background = Brushes.Snow;
                    _buttons[i, j] = button;
                    SetStyle(button);
                    matrix.Children.Add(button);
                }
            }

            MatrixGrid.Children.Add(matrix);
            _coordinates = new List<(int, int)>();
            _board = new Board(_height, _width, _difficulty);
            _prevColors = new List<Brush>();
            OpponentWins.Opacity = 0;
            PlayerWins.Opacity = 0;
        }

        private void SetStyle(Button button)
        {
            button.Style = (Style) FindResource("MyButtonStyle");
        }

        private void CellClick(object sender, RoutedEventArgs e)
        {
            _coordinates.Add(FindIndex((Button) sender));
            _prevColors.Add(_buttons[_coordinates[^1].Item1, _coordinates[^1].Item2].Background);
            _buttons[_coordinates[^1].Item1, _coordinates[^1].Item2].Background = Brushes.MediumBlue;
            if (_coordinates.Count != 2) return;
            int deltaX = Math.Abs(_coordinates[0].Item1 - _coordinates[1].Item1);
            int deltaY = Math.Abs(_coordinates[0].Item2 - _coordinates[1].Item2);

            if (_board.CurrentState[_coordinates[0].Item1, _coordinates[0].Item2] != 0 ||
                _board.CurrentState[_coordinates[1].Item1, _coordinates[1].Item2] != 0 || 
                deltaX > 1 || deltaY > 1 || deltaX == deltaY)
            {
                _buttons[_coordinates[0].Item1, _coordinates[0].Item2].Background = _prevColors[0];
                _buttons[_coordinates[1].Item1, _coordinates[1].Item2].Background = _prevColors[1];
                _coordinates = new List<(int, int)>();
                _prevColors = new List<Brush>();
            }
            else
            {
                _board.CurrentState[_coordinates[0].Item1, _coordinates[0].Item2] = 1;
                _board.CurrentState[_coordinates[1].Item1, _coordinates[1].Item2] = 1;
                _coordinates = new List<(int, int)>();
                _prevColors = new List<Brush>();
                if (new Node(_board.CurrentState, _board.Player, 2).GetValue(true).Equals(Int32.MaxValue))
                    PlayerWins.Opacity = 1;
                else if (OpponentStep()) OpponentWins.Opacity = 1;
            }
        }

        private bool OpponentStep()
        {
            int[,] prevState = _board.CurrentState;
            _board.Player.MakeStep();
            var coords = Node.FindDifference(prevState, _board.CurrentState);
            _buttons[coords[0].Item1, coords[0].Item2].Background = Brushes.Firebrick;
            _buttons[coords[1].Item1, coords[1].Item2].Background = Brushes.Firebrick;
            return new Node(_board.CurrentState, _board.Player, 2).GetValue(true).Equals(Int32.MaxValue);
        }
        
        private (int, int) FindIndex(Button button)
        {
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    if (_buttons[i, j] == button) return (i, j);
                }
            }

            return (-1, -1);
        }
    }
}