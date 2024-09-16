using System.Diagnostics;

namespace Minesweeper;

class Program
{
    static int width = 40;
    static int height = 12;
    static char[][] gameField = InitializeGameField();
    static bool[][] gameFieldVisibleSquares = InitializeGameFieldMask();
    static bool[][] gameFieldFlags = InitializeGameFieldMask();
    static bool[][] gameFieldCheckedSquares = InitializeGameFieldMask();
    static int[] selection = [6, 20];
    static int numberOfBombs = 50;

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
            Console.WriteLine($"Frame {frameCounter}");
            PrintDisplay();
            
            ConsoleKey input = GetInputKey();

            if (input == ConsoleKey.Q)
            {
                break;
            }
            switch (input)
            {
                case ConsoleKey.F:
                    gameFieldFlags[selection[0]][selection[1]] = !gameFieldFlags[selection[0]][selection[1]];
                    break;
                case ConsoleKey.Spacebar:
                    FloodFill(selection);
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
                Console.ForegroundColor = ConsoleColor.DarkGray;
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
        for (int j = 0; j < line.Length; j++)
        {
            PrintSquare(line[j], gameFieldVisibleSquares[lineNumber][j], gameFieldFlags[lineNumber][j], selectionOnLine && j == selection[1]);
            
            if (j < line.Length - 1)
            {
                Console.Write(" ");
            }
        }
    }

    private static void PrintDisplay()
    {
        for (int i = 0; i < gameField.Length; i++)
        {
            PrintLine(gameField[i], i, selection[0] == i);
            Console.Write("\n");
        }
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
            for (int square = 0; square < gameField[0].Length; square++)
            {
                if (gameField[row][square] == 'o')
                {
                    continue;
                }
                
                int[] coords = [row, square];
                int neighboringBombs = GetNumberOfNeighboringBombs(coords);
                if (neighboringBombs > 0)
                {
                    gameField[row][square] = char.Parse(neighboringBombs.ToString());
                }
            }
        }
    }

    private static void FloodFill(int[] coords)
    {
        if (SelectionOutOfBounds(coords))
        {
            return;
        }
        
        gameFieldVisibleSquares[coords[0]][coords[1]] = true;
        gameFieldFlags[coords[0]][coords[1]] = false;

        if (gameFieldCheckedSquares[coords[0]][coords[1]])
        {
            return;
        }
        
        gameFieldCheckedSquares[coords[0]][coords[1]] = true;

        if (gameField[coords[0]][coords[1]] != ' ') return;
        
        // above left
        coords[0] = coords[0] - 1;
        coords[1] = coords[1] - 1;

        if (!SelectionOutOfBounds(coords))
        {
            gameFieldVisibleSquares[coords[0]][coords[1]] = true;
            gameFieldFlags[coords[0]][coords[1]] = false;
        }
            
        // above mid
        coords[1] = coords[1] + 1;
        FloodFill(coords);
            
        // above right
        coords[1] = coords[1] + 1;
            
        if (!SelectionOutOfBounds(coords))
        {
            gameFieldVisibleSquares[coords[0]][coords[1]] = true;
            gameFieldFlags[coords[0]][coords[1]] = false;
        }
            
        // mid left
        coords[0] = coords[0] + 1;
        coords[1] = coords[1] - 2;
        FloodFill(coords);
            
        // mid right
        coords[1] = coords[1] + 2;
        FloodFill(coords);
            
        // bot left
        coords[0] = coords[0] + 1;
        coords[1] = coords[1] - 2;
        if (!SelectionOutOfBounds(coords))
        {
            gameFieldVisibleSquares[coords[0]][coords[1]] = true;
            gameFieldFlags[coords[0]][coords[1]] = false;
        }
            
        // bot mid
        coords[1] = coords[1] + 1;
        FloodFill(coords);
            
        // bot right
        coords[1] = coords[1] + 1;
        if (!SelectionOutOfBounds(coords))
        {
            gameFieldVisibleSquares[coords[0]][coords[1]] = true;
            gameFieldFlags[coords[0]][coords[1]] = false;
        }
            
        coords[0] = coords[0] - 1;
        coords[1] = coords[1] - 1;
    }

    private static int GetNumberOfNeighboringBombs(int[] coords)
    {
        int neighboringBombs = 0;
        
        // Above left
        int[] neighbor = [0, 0];
        neighbor[0] = coords[0] - 1;
        neighbor[1] = coords[1] - 1;
        if (!SelectionOutOfBounds(neighbor))
        {
            if (gameField[neighbor[0]][neighbor[1]] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Above mid
        neighbor[1]++;
        if (!SelectionOutOfBounds(neighbor))
        {
            if (gameField[neighbor[0]][neighbor[1]] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Above right
        neighbor[1]++;
        if (!SelectionOutOfBounds(neighbor))
        {
            if (gameField[neighbor[0]][neighbor[1]] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Mid left
        neighbor[0] = coords[0];
        neighbor[1] = coords[1] - 1;
        if (!SelectionOutOfBounds(neighbor))
        {
            if (gameField[neighbor[0]][neighbor[1]] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Mid right
        neighbor[0] = coords[0];
        neighbor[1] = coords[1] + 1;
        if (!SelectionOutOfBounds(neighbor))
        {
            if (gameField[neighbor[0]][neighbor[1]] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Below left
        neighbor[0] = coords[0] + 1;
        neighbor[1] = coords[1] - 1;
        if (!SelectionOutOfBounds(neighbor))
        {
            if (gameField[neighbor[0]][neighbor[1]] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Below mid
        neighbor[1]++;
        if (!SelectionOutOfBounds(neighbor))
        {
            if (gameField[neighbor[0]][neighbor[1]] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Below right
        neighbor[1]++;
        if (!SelectionOutOfBounds(neighbor))
        {
            if (gameField[neighbor[0]][neighbor[1]] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        return neighboringBombs;

    }

    private static bool SelectionOutOfBounds(int[] coords)
    {
        return coords[0] < 0 ||
               coords[1] < 0 ||
               coords[0] >= gameField.Length ||
               coords[1] >= gameField[0].Length;
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
                if (selection[0] > 0) selection[0]--;
                break;
            
            case ConsoleKey.DownArrow:
                if (selection[0] < height - 1) selection[0]++;
                break;
            
            case ConsoleKey.LeftArrow:
                if (selection[1] > 0) selection[1]--;
                break;
            
            case ConsoleKey.RightArrow:
                if (selection[1] < width - 1) selection[1]++;
                break;
        }
    }
}