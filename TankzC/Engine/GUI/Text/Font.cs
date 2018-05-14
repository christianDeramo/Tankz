using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankzC
{
    class Font
    {
        protected int numCol;
        protected int firstVal;
        protected int charW;
        protected int charH;
        public string TextureName { get; protected set; }
        public int CharWidth { get { return charW; } protected set { charW = value; } }
        public int CharHeight { get { return charH; } protected set { charH = value; } }

        public Texture Texture { get; protected set; }

        public Font(string textureName, string texturePath, int numColumns, int firstCharASCIIvalue, int charWidth, int charHeight)
        {
            TextureName = textureName;
            numCol = numColumns;
            firstVal = firstCharASCIIvalue;
            charW = charWidth;
            charH = charHeight;

            GfxManager.AddTexture(textureName, texturePath);
            Texture = GfxManager.GetTexture(textureName);
        }

        public Vector2 GetOffset(char c)
        {
            int cVal = c;
            int delta = cVal - firstVal;
            int x = delta % numCol;
            int y = delta / numCol;

            return new Vector2(x * CharWidth, y * CharHeight);
        }
        
    }
}
