using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtectTheWorld
{
    class Boton
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        Texture2D rectangulo;
        Color[] data;
        Vector2 punto;
        public Boton(GraphicsDeviceManager graphics, SpriteBatch spriteBatch,int x, int y, int ancho, int alto,Color color)
        {
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
            rectangulo=new Texture2D(graphics.GraphicsDevice, ancho, alto);
            data = new Color[ancho * alto];
            for (int i = 0; i < data.Length; ++i) data[i] =color;
            rectangulo.SetData(data);
            punto = new Vector2(x, y);
        }
        public void Dibuja()
        {
            spriteBatch.Draw(rectangulo, punto, Color.White);
        }
    }
}
