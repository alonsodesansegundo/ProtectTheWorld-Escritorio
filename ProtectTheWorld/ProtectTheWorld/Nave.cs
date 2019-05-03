using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtectTheWorld
{
    class Nave
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        //propiedades
        //private Point pos;
        private int X, Y;
        private Texture2D imagen;
        private Rectangle contenedor;
        private bool hayBala;
        private Rectangle bala;
        private Texture2D imgBala;
        private double vBala;
        private int ancho;
        private int alto;
        private int anchoBala;
        private int altoBala;

        //constructor
        public Nave(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D imagen, int x, int y, int ancho, int alto, 
            int anchoBala,int altoBala,double velocidadBala, Texture2D imgBala)
        {
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;

            //this.pos = new Point(x, y);
            this.X = x;
            this.Y = y;
            this.imagen = imagen;
            this.hayBala = false;
            this.vBala = velocidadBala;
            this.imgBala = imgBala;
            this.contenedor = new Rectangle(this.X, this.Y, ancho, alto);
            this.ancho = ancho;
            this.alto = alto;
            this.anchoBala = anchoBala;
            this.altoBala = altoBala;
        }

        //------------------------GETTER AND SETTER------------------------
        public int getX() { return this.X; }
        public bool getHayBala() { return this.hayBala; }

        public void setImagen(Texture2D imgagen) { this.imagen = imgagen; }

        public Texture2D getImagen() { return this.imagen; }

        public void setHayBala(bool hayBala) { this.hayBala = hayBala; }

        public Rectangle getContenedor() { return this.contenedor; }

        public Rectangle getBala() { return this.bala; }

        public int getAlto() { return this.alto; }

        //public Point getPos() { return this.pos; }

        //public void setX(int X) { pos.X = X; }

        //------------------------MOVER LA NAVE EN EL EJE X------------------------
        //metodo al que le paso la posicion x y se encarga de mover la nave (su pos x)
        public void moverNave(int nuevaX,int limite)
        {
            //this.pos.x = nuevaX - imagen.getWidth() / 2;
            //contenedor.right = pos.x + imagen.getWidth();
            //contenedor.left = pos.x;
            if(nuevaX>0 && nuevaX < limite)
            {
                this.X = nuevaX;
                this.contenedor = new Rectangle(this.X, this.Y, ancho, alto);
            }
        }

        //------------------------DISPARO DE LA NAVE------------------------
        public bool disparar()
        {
            if (!hayBala)
            {
                //genero el proyectil a traves de la posicion de la nave
                bala = new Rectangle(this.X +ancho/2-anchoBala/2, this.Y-altoBala , this.anchoBala, this.altoBala);
                hayBala = true;
                return true;
            }
            return false;
        }

        //------------------------MOVIMIENTO PROYECTIL NAVE------------------------
        public void actualizaProyectil()
        {
            //bala.top -= vBala;
            //bala.bottom -= vBala;
            ////si cuando actualizo el proyectil, llego al alto de la pantalla
            //if (bala.bottom <= 0)
            //{
            //    hayBala = false;
            //}

            bala.Y -= (int)vBala;
            //si cuando actualizo el proyectil, llego al alto de la pantalla
            if (bala.Bottom <= 0)
            {
                hayBala = false;
            }
        }

        //------------------------DIBUJO LA NAVE Y SU PROYECTIL------------------------
        public void Dibujar()
        {
            if (hayBala)
            {
                //dibujo la bala
                spriteBatch.Draw(this.imgBala, this.bala, Color.White);
            }
            spriteBatch.Draw(this.imagen, this.contenedor, Color.White);

        }
    }
}
