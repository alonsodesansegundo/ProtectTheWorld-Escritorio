using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtectTheWorld
{
    class BalaMarciano
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        //propiedades
        private Texture2D img;
        private Rectangle contenedor;
        private int vBalaMarciano;
        private int ancho;
        private int alto;

        public BalaMarciano(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, int x, int y, int ancho, int alto, Texture2D img, int vBalaMarciano)
        {
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;

            this.img = img;
            this.vBalaMarciano = vBalaMarciano;
            this.contenedor = new Rectangle(x, y, ancho, alto);
            this.ancho = ancho;
            this.alto = alto;
        }

        public Rectangle getContenedor() { return this.contenedor; }

        public void bajar()
        {
            this.contenedor.Y += vBalaMarciano;
        }

        public void Dibuja()
        {
            spriteBatch.Draw(this.img, this.contenedor, Color.White);
        }
    }
}
