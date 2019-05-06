using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtectTheWorld
{
    public class Boton
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D rectangulo;
        private bool bandera;
        private Color[] data;
        private Vector2 punto;
        private int x, y, ancho, alto;
        //Rectangulo contenedor
        private Rectangle contenedor;
        //Para el texto del boton
        private string texto;
        private SpriteFont fuente;
        private Color colorTexto;
        //Imagen boton
        private Texture2D imagen;
        private bool centrado;
        public Boton(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, int x, int y, int ancho, int alto, Color color)
        {
            //propiedades boton
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
            this.x = x;
            this.y = y;
            this.ancho = ancho;
            this.alto = alto;
            this.bandera = false;
            this.contenedor = new Rectangle(x, y, ancho, alto);
            this.rectangulo = new Texture2D(graphics.GraphicsDevice, ancho, alto);
            data = new Color[ancho * alto];
            for (int i = 0; i < data.Length; ++i) data[i] = color;
            rectangulo.SetData(data);
            this.punto = new Vector2(x, y);
            centrado = false;
        }
        public bool GetBandera() { return this.bandera; }
        public void SetBandera(bool booleana) { this.bandera = booleana; }
        public Rectangle GetContenedor() { return this.contenedor; }
        public void Dibuja()
        {
            spriteBatch.Draw(rectangulo, punto, Color.White);
            if (texto != null)
                if (!centrado)
                {
                    spriteBatch.DrawString(fuente, texto, punto, colorTexto);
                }
                else
                {
                    //int a=  (int)fuente.MeasureString(texto).Y / 2;
                    spriteBatch.DrawString(fuente, texto, 
                        new Vector2(punto.X+ancho/2 - (int)fuente.MeasureString(texto).X/2,
                        punto.Y+alto/ 2-(int)fuente.MeasureString(texto).Y / 2),
                        colorTexto);
                }
            if (imagen != null)
                spriteBatch.Draw(this.imagen, this.contenedor, Color.White);
        }
        //Método que se utilizará para introducir texto dentro del boton
        public void SetTexto(string texto, SpriteFont fuente, Color color,bool centrado)
        {
            if (centrado)
                this.centrado = true;
            this.texto = texto;
            this.fuente = fuente;
            this.colorTexto = color;
        }
        //Método que permite introducir una imagen dentro de nuestro boton
        public void SetImagen(Texture2D imagen)
        {
            this.imagen = imagen;
        }
    }
}
