using System.Diagnostics;

namespace Minesweeper;

class Program
{
    static int width = 40;
    static int height = 16;
    static char[][] gameField = InitializeGameField();
    static bool[][] gameFieldVisibleSquares = InitializeGameFieldMask();
    static bool[][] gameFieldFlags = InitializeGameFieldMask();
    static bool[][] gameFieldCheckedSquares = InitializeGameFieldMask();
    // static int[] selection = [6, 20];
    static int selectedRow = 6;
    static int selectedColumn = 20;
    
    static int numberOfBombs = 75;

    private static void Main()
    {
        PutBombsOnGameField();
        FillNumbers();

        int frameCounter = 0;
        
        PrintDisplay();
        
        while (true)
        {
            Console.Clear();
            frameCounter++;
            Console.WriteLine("Arrow keys to move, F to flag, Spacebar to reveal and Q to quit");
            //Console.WriteLine($"Frame {frameCounter}");
            PrintDisplay();
            
            ConsoleKey input = GetInputKey();

            if (input == ConsoleKey.Q)
            {
                break;
            }
            switch (input)
            {
                case ConsoleKey.F:
                    gameFieldFlags[selectedRow][selectedColumn] = !gameFieldFlags[selectedRow][selectedColumn];
                    break;
                case ConsoleKey.Spacebar:
                    FloodFill(selectedRow, selectedColumn);
                    break;
                default:
                    UpdateSelection(input);
                    break;
            }
        }
    }

    private static void PrintSquare(char symbol, bool visible, bool flagged, bool selected)
    {
        if (!visible)
        {
            symbol = '#';
        }

        if (flagged)
        {
            symbol = '¶';
        }
        
        switch (symbol)
        {
            case ' ':
                Console.ResetColor();
                break;
            case 'o':
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Red;
                break;
            case '¶':
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.DarkGray;
                break;
            case '#':
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                break;
            case '1':
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                break;
            case '2':
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                break;
            case '3':
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                break;
            case '4':
                Console.ForegroundColor = ConsoleColor.DarkRed;
                break;
            case '5':
                Console.ForegroundColor = ConsoleColor.Magenta;
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Cyan;
                break;
        }

        if (selected)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        Console.Write(symbol);
        Console.ResetColor();
    }

    private static void PrintLine(char[] line, int lineNumber, bool selectionOnLine = false)
    {
        Console.Write("| ");
        
        for (int j = 0; j < line.Length; j++)
        {
            PrintSquare(line[j], gameFieldVisibleSquares[lineNumber][j], gameFieldFlags[lineNumber][j], selectionOnLine && j == selectedColumn);
            Console.Write(" ");
        }
        
        Console.Write("|");
    }

    private static void PrintDisplay()
    {
        Console.WriteLine(new string('-', gameField[0].Length * 2 + 3));
        
        for (int i = 0; i < gameField.Length; i++)
        {
            PrintLine(gameField[i], i, selectedRow == i);
            Console.Write("\n");
        }
        
        Console.WriteLine(new string('-', gameField[0].Length * 2 + 3));
    }

    private static char[][] InitializeGameField()
    {
        char[][] rows = new char[height][];
        
        for (int i = 0; i < height; i++)
        {
            char[] row = new char[width];
            for (int j = 0; j < width; j++)
            {
                row[j] = ' ';
            }

            rows[i] = row;
        }

        return rows;
    }

    private static bool[][] InitializeGameFieldMask()
    {
        bool[][] rows = new bool[height][];
        
        for (int i = 0; i < height; i++)
        {
            bool[] row = new bool[width];
            for (int j = 0; j < width; j++)
            {
                row[j] = false;
            }

            rows[i] = row;
        }

        return rows;
    }

    private static void PutBombsOnGameField()
    {
        Random rng = new();
        
        for (int i = 0; i < numberOfBombs; i++)
        {
            while (true)
            {
                int randomRow = rng.Next(gameField.Length);
                int randomColumn = rng.Next(gameField[0].Length);

                if (gameField[randomRow][randomColumn] != 'o')
                {
                    gameField[randomRow][randomColumn] = 'o';
                    break;
                }
            }
        }
    }

    private static void FillNumbers()
    {
        for (int row = 0; row < gameField.Length; row++)
        {
            for (int col = 0; col < gameField[0].Length; col++)
            {
                if (gameField[row][col] == 'o')
                {
                    continue;
                }
                
                int neighboringBombs = GetNumberOfNeighboringBombs(row, col);
                if (neighboringBombs > 0)
                {
                    gameField[row][col] = char.Parse(neighboringBombs.ToString());
                }
            }
        }
    }

    private static void FloodFill(int row, int col)
    {
        
        if (SelectionOutOfBounds(row, col))
        {
            return;
        }
        
        if (gameFieldCheckedSquares[row][col])
        {
            return;
        }
        
        gameFieldCheckedSquares[row][col] = true;
        gameFieldVisibleSquares[row][col] = true;

        if (gameField[row][col] != ' ')
        {
            return;
        }
        
        gameFieldFlags[row][col] = false;
        
        // above left
        row--;
        col--;
        FloodFill(row, col);
            
        // above mid
        col++;
        FloodFill(row, col);
            
        // above right
        col++;
        FloodFill(row, col);
            
        // mid left
        row++;
        col -= 2;
        FloodFill(row, col);
            
        // mid right
        col += 2;
        FloodFill(row, col);
            
        // bot left
        row++;
        col -= 2;
        FloodFill(row, col);
            
        // bot mid
        col++;
        FloodFill(row, col);
            
        // bot right
        col++;
        FloodFill(row, col);
    }

    private static int GetNumberOfNeighboringBombs(int row, int col)
    {
        int neighboringBombs = 0;
        
        // Above left
        row--;
        col--;
        
        if (!SelectionOutOfBounds(row, col))
        {
            if (gameField[row][col] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Above mid
        col++;
        if (!SelectionOutOfBounds(row, col))
        {
            if (gameField[row][col] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Above right
        col++;
        if (!SelectionOutOfBounds(row, col))
        {
            if (gameField[row][col] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Mid left
        row++;
        col -= 2;
        if (!SelectionOutOfBounds(row, col))
        {
            if (gameField[row][col] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Mid right
        col += 2;
        if (!SelectionOutOfBounds(row, col))
        {
            if (gameField[row][col] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Below left
        row++;
        col -= 2;
        if (!SelectionOutOfBounds(row, col))
        {
            if (gameField[row][col] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Below mid
        col++;
        if (!SelectionOutOfBounds(row, col))
        {
            if (gameField[row][col] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Below right
        col++;
        if (!SelectionOutOfBounds(row, col))
        {
            if (gameField[row][col] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        return neighboringBombs;

    }

    private static bool SelectionOutOfBounds(int row, int col)
    {
        return row < 0 ||
               col < 0 ||
               row >= gameField.Length ||
               col >= gameField[0].Length;
    }

    private static ConsoleKey GetInputKey()
    {
        ConsoleKey[] allowedInputs =
        [
            ConsoleKey.Q,
            ConsoleKey.UpArrow,
            ConsoleKey.DownArrow,
            ConsoleKey.LeftArrow,
            ConsoleKey.RightArrow,
            ConsoleKey.F,
            ConsoleKey.Spacebar
        ];
        
        while (true)
        {
            ConsoleKey input = Console.ReadKey(true).Key;

            if (allowedInputs.Contains(input))
            {
                return input;
            }
        }
    }

    private static void UpdateSelection(ConsoleKey pressedKey)
    {
        switch (pressedKey)
        {
            case ConsoleKey.UpArrow:
                if (selectedRow > 0)
                {
                    selectedRow--;
                }
                
                break;
            
            case ConsoleKey.DownArrow:
                if (selectedRow < height - 1)
                {
                    selectedRow++;
                }
                break;
            
            case ConsoleKey.LeftArrow:
                if (selectedColumn > 0)
                {
                    selectedColumn--;
                }
                break;
            
            case ConsoleKey.RightArrow:
                if (selectedColumn < width - 1)
                {
                    selectedColumn++;
                }
                break;
        }
    }
}