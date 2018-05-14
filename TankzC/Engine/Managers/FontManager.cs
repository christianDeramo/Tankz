using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankzC
{
    static class FontManager
    {
        static Dictionary<string, Font> fonts;

        public static void Init()
        {
            fonts = new Dictionary<string, Font>();
        }

        public static Font GetFont(string fontName)
        {
            Font f = null;

            if (fonts.ContainsKey(fontName))
                f = fonts[fontName];

            return f;
        }

        public static void AddFont(string textureName, string texturePath, int numColumns, int firstCharASCIIvalue, int charWidth, int charHeight)
        {
            Font f = new Font(textureName, texturePath, numColumns, firstCharASCIIvalue, charWidth, charHeight);
            fonts.Add(textureName, f);
        }
    }
}
