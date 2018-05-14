using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankzC
{
    class TextObject:IActivable
    {
        List<TextChar> sprites;
        protected string text;
        protected bool isActive;

        public Vector2 Position;
        protected int width;
        protected int height;

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        protected float scale;

        public bool IsActive {
            get { return isActive; }
            set { isActive = value; UpdateCharStatus(value); }
        }

        public string Text
        {
            get { return text; }
            set { SetText(value); }
        }

        public Font Font { get; protected set; }

        public TextObject(Vector2 spritePos, string textString = "", Font font = null, float scale = 1)
        {
            if (font == null)
                font = FontManager.GetFont("stdFont");

            Font = font;
            this.scale = scale;
            Position = spritePos;
            sprites = new List<TextChar>();

            if (textString != "")
                SetText(textString);
        }

        private void SetText(string newText)
        {
            if (newText != text)
            {
                text = newText;
                int numChars = text.Length;
                int charX = (int)Position.X;
                int charY = (int)Position.Y;

                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];

                    if (i > sprites.Count - 1)
                    {
                        sprites.Add(new TextChar(new Vector2(charX, charY), c, Font));
                    }

                    else if(sprites[i].Character != c)
                    {
                        sprites[i].Character = c;
                    }

                    if (sprites.Count > text.Length)
                    {
                        int count = sprites.Count - text.Length;
                        int from = text.Length;

                        for (int j = from; j < sprites.Count; j++)
                        {
                            sprites[j].Destroy();
                        }

                        sprites.RemoveRange(from, count);
                    }

                    if (scale != 1)
                    {
                        foreach (var item in sprites)
                        {
                            item.ScaleChar(scale);
                        }
                    }

                    charX += sprites[i].Width;
                }
            }
        }

        protected virtual void UpdateCharStatus(bool activeStatus)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i] == null)
                    return;
                sprites[i].IsActive = activeStatus;
            }
        }
    }
}
