using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Digger.Architecture;
using System.Collections.Generic;
using System.IO;

namespace Digger.UI
{
    public class Frame : UserControl
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new();
        private readonly GameState gameState;

        public Frame()
        {
            gameState = new GameState();
            Game.CreateMap();

            var imagesDirectory = new DirectoryInfo("Images");
            foreach (var file in imagesDirectory.GetFiles("*.png"))
            {
                bitmaps[file.Name] = new Bitmap(file.FullName);
            }

            this.KeyDown += OnKeyDown;

            this.Focusable = true;
            this.Focus();
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    HandleMovement(0, -1);
                    break;
                case Key.Down:
                    HandleMovement(0, 1);
                    break;
                case Key.Left:
                    HandleMovement(-1, 0);
                    break;
                case Key.Right:
                    HandleMovement(1, 0);
                    break;
            }

            InvalidateVisual();
        }

        private void HandleMovement(int deltaX, int deltaY)
        {
            foreach (var creature in Game.Map)
            {
                if (creature is Player player)
                {
                    player.Move(deltaX, deltaY);
                    break;
                }
            }
        }

        public override void Render(DrawingContext context)
        {
            context.FillRectangle(Brushes.Black, new Rect(0, 0,
                GameState.ElementSize * Game.MapWidth,
                GameState.ElementSize * Game.MapHeight));

            for (int x = 0; x < Game.MapWidth; x++)
            {
                for (int y = 0; y < Game.MapHeight; y++)
                {
                    var creature = Game.Map[x, y];
                    if (creature is EndPoint)
                    {
                        // Проверяем, занят ли EndPoint
                        if (IsOccupiedByBoxOrPlayer(x, y))
                        {
                            var image = bitmaps[creature.GetImageFileName()];
                            var rect = new Rect(
                                x * GameState.ElementSize,
                                y * GameState.ElementSize,
                                GameState.ElementSize,
                                GameState.ElementSize);
                            context.DrawImage(image, rect);
                        }
                        else
                        {
                            var image = bitmaps["EndPoint.png"];
                            var rect = new Rect(
                                x * GameState.ElementSize,
                                y * GameState.ElementSize,
                                GameState.ElementSize,
                                GameState.ElementSize);
                            context.DrawImage(image, rect);
                        }
                    }
                    else if (creature != null)
                    {
                        var image = bitmaps[creature.GetImageFileName()];
                        var rect = new Rect(
                            x * GameState.ElementSize,
                            y * GameState.ElementSize,
                            GameState.ElementSize,
                            GameState.ElementSize);
                        context.DrawImage(image, rect);
                    }
                }
            }
        }

        private bool IsOccupiedByBoxOrPlayer(int x, int y)
        {
            var creature = Game.Map[x, y];
            return creature is Player || creature is Box;
        }
    }
}
