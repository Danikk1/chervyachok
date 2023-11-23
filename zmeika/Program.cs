using System;
using System.Collections.Generic;
using System.Threading;

enum Border
{
    MaxRight = 40,
    MaxBottom = 20
}

class SnakeGame
{
    private List<Position> snake;
    private Position food;
    private Border border;
    private Direction direction;
    private bool isGameOver;

    public SnakeGame()
    {
        snake = new List<Position> { new Position(10, 5) };
        food = GenerateFoodPosition();
        border = new Border();
        direction = Direction.Right;
        isGameOver = false;
    }

    public void Run()
    {
        Console.CursorVisible = false;
        Thread inputThread = new Thread(ProcessInput);
        inputThread.Start();

        while (!isGameOver)
        {
            MoveSnake();
            Draw();
            CheckCollision();

            Thread.Sleep(50);
        }

        Console.SetCursorPosition(0, (int)Border.MaxBottom + 2);
        Console.WriteLine("Game Over!");
    }

    private void ProcessInput()
    {
        while (!isGameOver)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (direction != Direction.Down)
                            direction = Direction.Up;
                        break;
                    case ConsoleKey.DownArrow:
                        if (direction != Direction.Up)
                            direction = Direction.Down;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (direction != Direction.Right)
                            direction = Direction.Left;
                        break;
                    case ConsoleKey.RightArrow:
                        if (direction != Direction.Left)
                            direction = Direction.Right;
                        break;
                    case ConsoleKey.Escape:
                        isGameOver = true;
                        break;
                }
            }
        }
    }

    private void MoveSnake()
    {
        Position head = snake[0];
        Position newHead = new Position(head.X, head.Y);

        switch (direction)
        {
            case Direction.Up:
                newHead.Y--;
                break;
            case Direction.Down:
                newHead.Y++;
                break;
            case Direction.Left:
                newHead.X--;
                break;
            case Direction.Right:
                newHead.X++;
                break;
        }

        snake.Insert(0, newHead);

        if (newHead.Equals(food))
        {
            food = GenerateFoodPosition();
        }
        else
        {
            Console.SetCursorPosition(snake[snake.Count - 1].X, snake[snake.Count - 1].Y);
            Console.Write(" ");
            snake.RemoveAt(snake.Count - 1);
        }
    }

    private void CheckCollision()
    {
        Position head = snake[0];

        if (head.X < 0 || head.X >= (int)Border.MaxRight || head.Y < 0 || head.Y >= (int)Border.MaxBottom)
        {
            isGameOver = true;
        }

        for (int i = 1; i < snake.Count; i++)
        {
            if (head.Equals(snake[i]))
            {
                isGameOver = true;
                break;
            }
        }
    }

    private Position GenerateFoodPosition()
    {
        Random random = new Random();
        int x = random.Next(0, (int)Border.MaxRight);
        int y = random.Next(0, (int)Border.MaxBottom);
        return new Position(x, y);
    }

    private void Draw()
    {

        // Рисуем границу
        for (int i = 0; i <= (int)Border.MaxRight; i++)
        {
            Console.SetCursorPosition(i, 0);
            Console.Write("-");
            Console.SetCursorPosition(i, (int)Border.MaxBottom);
            Console.Write("-");
        }

        for (int i = 0; i <= (int)Border.MaxBottom; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write("|");
            Console.SetCursorPosition((int)Border.MaxRight, i);
            Console.Write("|");
        }

        Console.SetCursorPosition(0, (int)Border.MaxBottom);
        for (int i = 0; i <= (int)Border.MaxRight; i++)
        {
            Console.Write("-");
        }

        // Рисуем еду
        Console.SetCursorPosition(food.X, food.Y);
        Console.Write("F");

        // Рисуем змейку
        foreach (Position position in snake)
        {
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write("■");
        }
    }
}

class Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public bool Equals(Position other)
    {
        return X == other.X && Y == other.Y;
    }
}

enum Direction
{
    Up,
    Down,
    Left,
    Right
}

class Program
{
    static void Main()
    {
        SnakeGame snakeGame = new SnakeGame();
        snakeGame.Run();
    }
}
