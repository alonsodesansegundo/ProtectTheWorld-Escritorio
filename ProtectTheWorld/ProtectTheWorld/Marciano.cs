using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtectTheWorld
{
    class Marciano
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        //propiedades
        private Point pos;
        private Texture2D imagen;
        private int salud,ancho,alto;
        private double vMovimiento;
        private Rectangle contenedor;
        private int puntuacion;

        //constructor
        public Marciano(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D imagen, int x, int y,int ancho,int alto, int salud, double velocidad, int puntuacion)
        {
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;

            this.pos = new Point(x, y);
            this.imagen = imagen;
            this.salud = salud;
            this.vMovimiento = velocidad;
            this.puntuacion = puntuacion;
            this.ancho = ancho;
            this.alto = alto;
            //inicializo el contenedor
            this.contenedor = new Rectangle(this.pos.X, this.pos.Y, ancho, alto);
        }

        //metodos
        //------------------------GETTER AND SETTER------------------------
        public int getAncho() { return this.ancho; }

        public int getAlto() { return this.alto; }

        public int getPuntuacion() { return this.puntuacion; }

        public Rectangle getContenedor() { return this.contenedor; }

        public Point getPos() { return this.pos; }

        public Texture2D getImagen() { return this.imagen; }

        public void setImagen(Texture2D imagen) { this.imagen = imagen; }

        public int getSalud() { return this.salud; }


        //------------------------MOVIMIENTO DE LOS MARCIANOS------------------------
        //metodo mover abajo
        public void moverAbajo(bool abajo)
        {
            if (abajo)
            {
                //aumento la posY el alto de la imagen
                this.pos.Y += imagen.Height / 2;
                //actualizo el contenedor
                this.contenedor.Y = this.pos.Y;

                //this.contenedor.top = this.pos.Y;
                //this.contenedor.bottom = this.pos.Y + this.imagen.Height;
            }
        }

        public bool limiteAbajo(int limiteY)
        {
            if (this.contenedor.Y + this.imagen.Height > limiteY)
                return true;

            return false;

            //if (this.contenedor.bottom > limiteY)
            //{
            //    return true;
            //}
            //return false;
        }

        public void moverLateral(bool izq)
        {
            if (izq)
            {
                this.pos.X -= (int)vMovimiento;
                //acutalizo la pos x del contenedor
                //this.contenedor.left = this.pos.x;
                //this.contenedor.right = this.pos.x + imagen.Width;
            }
            else
            {
                this.pos.X += (int)vMovimiento;
                //acutalizo la pos x del contenedor
                //this.contenedor.left = this.pos.x;
                //this.contenedor.right = this.pos.x + imagen.Width;
            }

            this.contenedor.X = this.pos.X;
        }

        public bool limiteDerecha(int anchoPantalla)
        {
            if (this.pos.X >= anchoPantalla - imagen.Width)
                return true;
            return false;
        }

        public bool limiteIzquierda()
        {
            if (this.pos.X <= 0)
                return true;
            return false;
        }


        //------------------------RECIBE PROYECTIL------------------------
        public bool colisiona(Rectangle bala)
        {
            //si la bala que recibo colisiona con el
            if (bala.Intersects(this.contenedor))
                return true;
            return false;
        }

        //------------------------DIBUJO EL MARCIANO------------------------
        public void Dibujar()
        {
            spriteBatch.Draw(this.imagen, this.contenedor, Color.White);
        }

        //------------------------PROBABILIDAD DISPARO MARCIANO------------------------
        public bool dispara(int probabilidad)
        {
            //probabilidad de disparo
            //numero aleatorio entre 1 y 100
            Random r = new Random();
            int numero = r.Next(0, 101);
            if (numero <= probabilidad)
                return true;
            return false;
        }
    }
}
