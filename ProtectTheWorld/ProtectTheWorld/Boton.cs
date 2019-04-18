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
        private Texture2D rectangulo;
        private Color[] data;
        private Vector2 punto;
        private int x,y,ancho, alto;
        //Para el texto del boton
        private string texto;
        private SpriteFont fuente;
        private Color colorTexto;
        //Imagen boton
        private Texture2D imagen;
        public Boton(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, int x, int y, int ancho, int alto, Color color)
      
        {
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
            this.x = x;
            this.y = y;
            this.ancho = ancho;
            this.alto = alto;
            rectangulo = new Texture2D(graphics.GraphicsDevice, ancho, alto);
            data = new Color[ancho * alto];
            for (int i = 0; i < data.Length; ++i) data[i] = color;
            rectangulo.SetData(data);
            punto = new Vector2(x, y);
        }
        public void Dibuja()
        {
            spriteBatch.Draw(rectangulo, punto, Color.White);
            if (texto != null)
            {
                spriteBatch.DrawString(fuente, texto, punto, colorTexto);
            }
            if (imagen != null)
            {
                spriteBatch.Draw(imagen, new Rectangle(x, y,
               ancho, alto), Color.White);
            }
        }
        public void SetTexto(string texto, SpriteFont fuente, Color color)
        {
            this.texto = texto;
            this.fuente = fuente;
            this.colorTexto = color;
        }
        public void SetImagen(Texture2D imagen)
        {
            this.imagen = imagen;

        }
    }
}
