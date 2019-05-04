using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.ViewManagement;

namespace ProtectTheWorld
{
    enum EstadoJuego
    {
        Menu,
        Gameplay,
        Ayuda,
        Creditos,
        Records,
        Opciones
    }
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Manejador : Game
    {
        //GENERAL
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private KeyboardState teclado;
        //**********************MENU**********************
        private int espacio;
        private EstadoJuego estadoActualJuego;
        private ButtonState estadoClickIzq;
        private int AnchoPantalla, AltoPantalla;
        private SpriteFont fuenteTitulo, fuenteBotones;
        private string titulo, txtJugar, txtOpciones, txtAyuda, txtRecords, txtCreditos;
        private List<Boton> botonesMenu;
        private Texture2D fondoMenu;
        private Boton btnJugar, btnOpciones, btnAyuda, btnRecords, btnCreditos;

        //**********************JUEGO**********************
        private int filas, columnas, nivel, primeraX, primeraY, altoMarciano, anchoMarciano,
            altoProyectilNave, anchoProyectilNave, altoNave, anchoNave, vNave, probabilidadDisparoMarcianos,aleatorio,
            puntuacionGlobal,auxiliar,anchoProyectilMarciano,altoProyectilMarciano;
        private Random generador;
        private double vMarciano, vBala;
        private Marciano[,] marcianos;
        private ArrayList misColumnas;
        List<BalaMarciano> balasMarcianos;

        private Texture2D imgMarciano1, imgMarciano2, imgNave, imgBala,imgBalaMarciano,explosion;
        private Nave miNave;
        private bool voyIzquierda, voyAbajo;
        public Manejador()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            //PANTALLA COMPLETA
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;

            //INICIALIZO LA PRIMERA PANTALLA
            estadoActualJuego = EstadoJuego.Menu;

            //HAGO VISIBLE EL RATÓN
            this.IsMouseVisible = true;

            //ESTADO ACTUAL BOTON CLICK IZQ
            estadoClickIzq = ButtonState.Released;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //ANCHO Y ALTO PANTALLA
            AltoPantalla = (int)ApplicationView.GetForCurrentView().VisibleBounds.Height;
            AnchoPantalla = (int)ApplicationView.GetForCurrentView().VisibleBounds.Width;

            //**********************MENU**********************
            //STRINGS
            titulo = "Protect the World";
            txtJugar = "Jugar";
            txtOpciones = "Opciones";
            txtAyuda = "Ayuda";
            txtCreditos = "Creditos";
            txtRecords = "Records";

            fondoMenu = Content.Load<Texture2D>("fondomenu");
            fuenteTitulo = Content.Load<SpriteFont>("Fuentes/FuenteTitulo");
            fuenteBotones = Content.Load<SpriteFont>("Fuentes/FuenteBotones");

            //ARRAYLIST DE BOTONES
            botonesMenu = new List<Boton>();    //aqui porque si lo pongo en el método instance da null reference exception
                                                //punto x, punto y, alto y ancho
            espacio = (int)fuenteTitulo.MeasureString(titulo).X -
               (int)fuenteBotones.MeasureString(txtJugar).X -
               (int)fuenteBotones.MeasureString(txtOpciones).X -
                (int)fuenteBotones.MeasureString(txtAyuda).X -
                 (int)fuenteBotones.MeasureString(txtRecords).X -
                  (int)fuenteBotones.MeasureString(txtCreditos).X;
            espacio = espacio / 4;
            int ancho;
            int alto = (int)fuenteBotones.MeasureString(txtJugar).Y;
            //Boton jugar
            btnJugar = new Boton(this.graphics, this.spriteBatch,
                (AnchoPantalla - (int)fuenteTitulo.MeasureString(titulo).X) / 2,
                AltoPantalla / 6 + (int)fuenteTitulo.MeasureString(titulo).Y,
                (int)fuenteBotones.MeasureString(txtJugar).X,
                alto,
                Color.Transparent);
            botonesMenu.Add(btnJugar);
            ancho = (int)fuenteBotones.MeasureString(txtJugar).X;

            //Boton opciones
            btnOpciones = new Boton(this.graphics, this.spriteBatch,
                (AnchoPantalla - (int)fuenteTitulo.MeasureString(titulo).X) / 2 + ancho + espacio,
               AltoPantalla / 6 + (int)fuenteTitulo.MeasureString(titulo).Y,
               (int)fuenteBotones.MeasureString(txtOpciones).X,
               alto,
               Color.Transparent);
            botonesMenu.Add(btnOpciones);
            ancho += (int)fuenteBotones.MeasureString(txtOpciones).X + espacio;

            //Boton ayuda
            btnAyuda = new Boton(this.graphics, this.spriteBatch,
                (AnchoPantalla - (int)fuenteTitulo.MeasureString(titulo).X) / 2 + ancho + espacio,
                AltoPantalla / 6 + (int)fuenteTitulo.MeasureString(titulo).Y,
                (int)fuenteBotones.MeasureString(txtAyuda).X,
                alto,
                Color.Transparent);
            botonesMenu.Add(btnAyuda);
            ancho += (int)fuenteBotones.MeasureString(txtAyuda).X + espacio;

            //Boton records
            btnRecords = new Boton(this.graphics, this.spriteBatch,
                (AnchoPantalla - (int)fuenteTitulo.MeasureString(titulo).X) / 2 + ancho + espacio,
                AltoPantalla / 6 + (int)fuenteTitulo.MeasureString(titulo).Y,
                (int)fuenteBotones.MeasureString(txtRecords).X,
                alto,
                Color.Transparent);
            botonesMenu.Add(btnRecords);
            ancho += (int)fuenteBotones.MeasureString(txtRecords).X + espacio;

            //Boton creditos
            btnCreditos = new Boton(this.graphics, this.spriteBatch,
                (AnchoPantalla - (int)fuenteTitulo.MeasureString(titulo).X) / 2 + ancho + espacio,
                AltoPantalla / 6 + (int)fuenteTitulo.MeasureString(titulo).Y,
                (int)fuenteBotones.MeasureString(txtCreditos).X,
                alto,
                Color.Transparent);
            botonesMenu.Add(btnCreditos);


            //**********************JUEGO**********************
            auxiliar = 0;
            puntuacionGlobal = 0;
            //marcianos
            filas = 5;
            columnas = 10;
            nivel = 0;
            primeraX = 0;
            primeraY = 0;
            anchoMarciano = AnchoPantalla / 40;
            altoMarciano = AnchoPantalla / 40;
            anchoProyectilMarciano = anchoMarciano / 2;
            altoProyectilMarciano = altoMarciano / 2;
            vMarciano = 1;
            marcianos = new Marciano[filas, columnas];
            balasMarcianos = new List<BalaMarciano>();
            probabilidadDisparoMarcianos = 33;
            generador = new Random();
            misColumnas = new ArrayList();
            imgMarciano1 = Content.Load<Texture2D>("mio1");
            imgMarciano2 = Content.Load<Texture2D>("mio2");

            //relleno el array bidimensional de marcianos
            rellenaMarcianos();

            voyAbajo = false;
            voyIzquierda = false;

            //nave
            vNave = 10;
            vBala = 6;
            imgBala = Content.Load<Texture2D>("proyectilnave");
            imgBalaMarciano= Content.Load<Texture2D>("bombamarciano");
            imgNave = Content.Load<Texture2D>("nave1");
            explosion =Content.Load<Texture2D>("explosion");
            altoNave = AnchoPantalla / 20;
            anchoNave = AnchoPantalla / 20;
            altoProyectilNave = altoNave/2;
            anchoProyectilNave = anchoNave / 4;
            miNave = new Nave(graphics, spriteBatch, imgNave,
                AnchoPantalla / 2 - anchoNave / 2, AltoPantalla - altoNave, anchoNave, altoNave,
               anchoProyectilNave, altoProyectilNave, vBala, imgBala);

        }

        /// <summary>
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            base.Update(gameTime);
            switch (estadoActualJuego)
            {
                case EstadoJuego.Menu:
                    GestionaMenu();
                    break;
                case EstadoJuego.Gameplay:
                    GestionaJuego();
                    break;
                case EstadoJuego.Creditos:
                    GestionaCreditos();
                    break;
                case EstadoJuego.Records:
                    GestionaRecords();
                    break;
                case EstadoJuego.Opciones:
                    GestionaOpciones();
                    break;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            switch (estadoActualJuego)
            {
                case EstadoJuego.Menu:
                    DibujaMenu();
                    break;
                case EstadoJuego.Gameplay:
                    DibujaJuego();
                    break;
                case EstadoJuego.Creditos:
                    DibujaCreditos();
                    break;
                case EstadoJuego.Records:
                    DibujaCreditos();
                    break;
                case EstadoJuego.Opciones:
                    DibujaOpciones();
                    break;
            }
        }
        //MENÚ PRINCIPAL
        //MÉTODO ENCARGADO DE DIBUJAR EL MENÚ PRINCIPAL
        public void DibujaMenu()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            //dibujo el fondo
            spriteBatch.Draw(fondoMenu, new Rectangle(0, 0, AnchoPantalla, AltoPantalla), Color.White);
            //dibujo el titulo del juego
            spriteBatch.DrawString(fuenteTitulo, titulo, new Vector2(AnchoPantalla / 2 - fuenteTitulo.MeasureString(titulo).X / 2, AltoPantalla / 6), Color.White);
            // set texto set imagen
            btnJugar.SetTexto(txtJugar, fuenteBotones, Color.White);
            btnOpciones.SetTexto(txtOpciones, fuenteBotones, Color.White);
            btnAyuda.SetTexto(txtAyuda, fuenteBotones, Color.White);
            btnRecords.SetTexto(txtRecords, fuenteBotones, Color.White);
            btnCreditos.SetTexto(txtCreditos, fuenteBotones, Color.White);
            //jugar.SetImagen(fondoMenu);
            //BOTONES
            foreach (Boton b in botonesMenu)
            {
                b.Dibuja();
            }
            spriteBatch.End();
        }
        //MÉTODO ENCARGADO DE GESTIONAR LA LÓGICA DEL MENÚ PRINCIPAL
        public void GestionaMenu()
        {
            //TECLADO
            teclado = Keyboard.GetState();
            //si pulsa la tecla ESC
            if (teclado.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            //si cambia el estado del click izq
            if (estadoClickIzq != Mouse.GetState().LeftButton)
            {
                //cambio mi variable
                estadoClickIzq = Mouse.GetState().LeftButton;
                //dependiendo del estado actual del click izquierdo, hago una cosa u otra
                switch (estadoClickIzq)
                {
                    //si el boton izq está pulsado
                    case ButtonState.Pressed:
                        //veo si he pulsado en algun boton, de ser así, pongo su bandera a true
                        foreach (Boton btn in botonesMenu)
                        {
                            if (ClickIzq(btn))
                                btn.SetBandera(true);
                        }

                        break;
                    //si el boton izquierdo no está pulsado, se ha levantado, hago lo que obedezca a dicho boton
                    case ButtonState.Released:
                        if (LevantoIzq(btnJugar))
                            estadoActualJuego = EstadoJuego.Gameplay;
                        //titulo = "FUNCIONA";
                        if (LevantoIzq(btnOpciones))
                            estadoActualJuego = EstadoJuego.Opciones;
                        if (LevantoIzq(btnAyuda))
                            estadoActualJuego = EstadoJuego.Ayuda;
                        if (LevantoIzq(btnRecords))
                            estadoActualJuego = EstadoJuego.Records;
                        if (LevantoIzq(btnCreditos))
                            estadoActualJuego = EstadoJuego.Creditos;

                        //pongo a false las banderas de todos los botones del menu
                        foreach (Boton btn in botonesMenu)
                        {
                            btn.SetBandera(false);
                        }
                        break;
                }
            }
        }

        //JUEGO
        //MÉTODO ENCARGADO DE DIBUJAR EL JUEGO
        public void DibujaJuego()
        {
            //dibujo el fondo negro
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            //dibujo la puntuacion
            spriteBatch.DrawString(fuenteBotones,
                puntuacionGlobal.ToString(), 
                new Vector2(AnchoPantalla/2- fuenteBotones.MeasureString(puntuacionGlobal.ToString()).X / 2,
                fuenteBotones.MeasureString(puntuacionGlobal.ToString()).Y),
                Color.White);
            //dibujo los marcianos
            foreach (Marciano m in marcianos)
            {
                if (m != null)
                    m.Dibujar();
            }
            //dibujo todas las balas marcianos
            for (int i = 0; i < balasMarcianos.Count; i++)
            {
                balasMarcianos[i].Dibuja();
            }

            //dibujo la nave
            miNave.Dibujar();
            spriteBatch.End();
        }
        //MÉTODO ENCARGADO DE GESTIONAR LA LÓGICA DEL JUEGO
        public void GestionaJuego()
        {
            auxiliar++;
            //BALAS MARCIANOS
            if (auxiliar == 100)
            {
                auxiliar = 0;
                disparanMarcianos();
            }
            actualizaBalasMarcianos();
            //TECLADO
            gestionaTeclado();

            //MOVIMIENTO BALA NAVE
            mueveBalaNave();
            
            //BANDERAS MOVIMIENTO MARCIANOS
            actualizaBanderasMovimiento();

            //MOVIMIENTO MARCIANOS
            mueveMarcianos();
        }

        //Método que será llamado cuando no haya más marcianos en el arraybidimensional. 
        //Incrementa el nivel y se encarga de rellenar de marcianos el array bidimensional dependiendo del nivel en el que estemos.
        //También se encarga de aumentar la velocidad de los marcianos en el momento adecuado
        public void rellenaMarcianos()
        {
            try
            {
                //incremento el nivel
                nivel++;
                //recorro las filas
                for (int i = 0; i < marcianos.GetLength(0); i++)
                {
                    //incremento la pos y
                    primeraY += altoMarciano * 2;
                    //recorro las columnas
                    for (int j = 0; j < marcianos.GetLength(1); j++)
                    {
                        //dependiendo del nivel y de la fila en la que esté
                        //pongo un marciano nivel 1 o marciano nivel 2
                        //por ejemplo, si estoy en la ultima fila y en el nivel 2-1, sera de marcianos de dos impactos
                        if (i >= marcianos.GetLength(0) - (nivel - 1))
                        {
                            marcianos[i, j] = new Marciano(graphics, spriteBatch, imgMarciano2, primeraX, primeraY, anchoMarciano, altoMarciano, 2, vMarciano, 25);
                        }
                        else
                        {
                            marcianos[i, j] = new Marciano(graphics, spriteBatch, imgMarciano1, primeraX, primeraY, anchoMarciano, altoMarciano, 1, vMarciano, 10);
                        }
                        //aumento la posX
                        primeraX += anchoMarciano * 2;
                    }
                    //una vez recorro todas las columnas de la fila actual
                    primeraX = 0;
                }
                //una vez recorro todas las filas y columnas
                primeraY = 0;
                //si el nivel - 1 es igual al numero de filas, es decir, he llegado a cubrir la pantalla de marcianos de dos impactos
                if (nivel - 1 == filas)
                {
                    nivel = 0;
                    vMarciano = vMarciano * 2;
                }
            }
            catch
            {

            }

        }

        public bool hayMarcianos()
        {
            for (int i = 0; i < marcianos.GetLength(0); i++)
            {
                for (int j = 0; j < marcianos.GetLength(1); j++)
                {
                    if (marcianos[i,j] != null)
                    {
                        return true;
                    }
                }
            }
            //si no he encontrado ningun null
            return false;
        }

        public void gestionaTeclado()
        {
            //TECLADO
            teclado = Keyboard.GetState();
            //CONFIGURACION FLECHAS
            //SI PULSA FLECHA ABAJO
            if (teclado.IsKeyDown(Keys.Down))
                estadoActualJuego = EstadoJuego.Menu;
            //SI PULSA LA FLECHA DRCH
            if (teclado.IsKeyDown(Keys.Right) && (!teclado.IsKeyUp(Keys.Right)))
                miNave.moverNave(miNave.getX() + vNave, AnchoPantalla - anchoNave);
            //SI PULSA LA FLECHA IZQ
            if (teclado.IsKeyDown(Keys.Left) && (!teclado.IsKeyUp(Keys.Left)))
                miNave.moverNave(miNave.getX() - vNave, AnchoPantalla - anchoNave);
            //SI PULSA FLECHA ARRIBA
            if (teclado.IsKeyDown(Keys.Up))
                miNave.disparar();

            //CONFIGURACION ALTERNATIVA
            //SI PULSA W
            if (teclado.IsKeyDown(Keys.S))
                estadoActualJuego = EstadoJuego.Menu;
            //SI PULSA LA D
            if (teclado.IsKeyDown(Keys.D) && (!teclado.IsKeyUp(Keys.D)))
                miNave.moverNave(miNave.getX() + vNave, AnchoPantalla - anchoNave);
            //SI PULSA LA A
            if (teclado.IsKeyDown(Keys.A) && (!teclado.IsKeyUp(Keys.A)))
                miNave.moverNave(miNave.getX() - vNave, AnchoPantalla - anchoNave);
            //SI PULSA FLECHA ARRIBA
            if (teclado.IsKeyDown(Keys.W))
                miNave.disparar();
        }

        public void mueveBalaNave()
        {
            //SI HAY BALA EN PANTALLA
            if (miNave.getHayBala())
            {
                //ACTUALIZO EL PROYECTIL
                miNave.actualizaProyectil();
                //RECORRO LOS MARCIANOS PARA VER SI HE IMPACTADO EN ALGUNO
                for (int i = 0; i < marcianos.GetLength(0); i++)
                {
                    for (int j = 0; j < marcianos.GetLength(1); j++)
                    {
                        //si hay un marciano
                        if (marcianos[i, j] != null)
                        {
                            //si la bala impacta en un marciano
                            if (marcianos[i, j].getContenedor().Intersects(miNave.getBala()))
                            {
                                //le resto uno de salud al marciano
                                marcianos[i, j].setSalud(marcianos[i, j].getSalud() - 1);
                                //si salud es cero
                                if (marcianos[i, j].getSalud() == 0)
                                {

                                    //sumo a mi puntuacion global los puntos del marciano
                                    puntuacionGlobal += marcianos[i, j].getPuntuacion();
                                    //elimino el marciano
                                    marcianos[i, j] = null;
                                }
                                else
                                {
                                    //si continua con salud
                                    //cambio la imagen del marciano por una de marciano nivel 1
                                    marcianos[i, j].setImagen(imgMarciano1);
                                }
                                //quito la bala
                                miNave.setHayBala(false);

                                //si no hay marcianos
                                //relleno el array segun el nivel, de ello se encarga rellena marcianos
                                if (!hayMarcianos())
                                {
                                    //vaciaBalas();
                                    rellenaMarcianos();
                                }
                                //salgo del bucle porque no hace falta seguir recorriendo todos los marcianos, ya que solo es posible que haya un impacto
                                break;
                            }
                        }
                    }
                }
            }
        }

        //------------------------DISPARO MARCIANOS------------------------
        public void disparanMarcianos()
        {
            //solo podrán disparar los últimos marcianos de cada columna
            //recorro las filas de abajo a arriba
            for (int i = marcianos.GetLength(0) - 1; i >= 0; i--)
            {
                //recorro las columnas de izq a drch
                for (int j = 0; j < marcianos.GetLength(1); j++)
                {
                    //si esa columna no está en mi arraylist de columnas y en la posicion actual hay un marciano
                    if (misColumnas.IndexOf(j) == -1 && marcianos[i,j] != null)
                    {
                        //añado la columna a mi arraylist
                        misColumnas.Add(j);
                        //si se decide que el marciano dispare (porque sale x probabilidad)
                        //probabilidadDisparoMarcianos++;

                        aleatorio = generador.Next(0, 101);
                        if (marcianos[i,j].dispara(probabilidadDisparoMarcianos,aleatorio))
                        {
                            //genero una nueva bala marciano que añado a su array
                            balasMarcianos.Add(new BalaMarciano(graphics,spriteBatch,(int)marcianos[i,j].getContenedor().Center.X -
                                    anchoProyectilMarciano / 2,
                                    (int)marcianos[i,j].getPos().Y + altoMarciano,
                                    anchoProyectilMarciano,
                                   altoProyectilMarciano, imgBalaMarciano, 5));
                        }
                    }
                }
            }
            //limpio el arraylist que uso de contenedor de las columnas
            misColumnas.Clear();
        }

        //------------------------MOVIMIENTO BALAS MARCIANOS------------------------
        public void actualizaBalasMarcianos()
        {
            //además de mover las balas, gestiono si chocan o no con la nave, y si desaparecen de la pantalla las elimino
            //de atrás alante para no tener problemas al eliminar
            for (int i = balasMarcianos.Count - 1; i >= 0; i--)
            {
                //muevo la bala actual hacia abajo
                balasMarcianos[i].bajar();
                //si choca con la nave
                if (balasMarcianos[i].getContenedor().Intersects(miNave.getContenedor()))
                {

                    miNave.setImagen(explosion);
                    //perdi
                    //estoyJugando = false;
                    //mueveNave = false;
                    //acabaMusica();
                    //if (mejoraPuntuacion())
                    //{
                    //    pideSiglas = true;
                    //}
                    //else
                    //{
                    //    perdi = true;
                    //}
                }
                else
                {
                    //si no ha chocado con la nave
                    //veo si ha chocado o no con la bala de la nave, si es asi, elimino ambas balas
                    if (balasMarcianos[i].getContenedor().Intersects(miNave.getBala()))
                    {

                        //elimino ambas balas
                        //elimino la bala marciano
                        balasMarcianos.Remove(balasMarcianos[i]);

                        //elimino la bala de la nave
                        miNave.setHayBala(false);

                        //si no choca con la nave ni con la bala de la nave
                    }
                    else
                    {
                        //veo si desaparece de la pantalla, si es así
                        if (balasMarcianos[i].getContenedor().Y >= AltoPantalla)
                        {
                            //la elimino para no mover balas que no se ven
                            balasMarcianos.Remove(balasMarcianos[i]);
                        }
                    }

                }
            }
        }

        //------------------------MOVIMIENTO VERTICAL Y HORIZONTAL DE LOS MARCIANOS------------------------
        public void actualizaBanderasMovimiento()
        {
            //aqui veré en que dirección tienen que ir los marcianos (izq o drch) -> bandera voyIzquierda
            //también veré si descienden un nivel o no -> bandera voy abajo
            //recorro las filas de marcianos
            for (int i = 0; i < marcianos.GetLength(0); i++)
            {
                //recorro las columnas
                for (int j = 0; j < marcianos.GetLength(1); j++)
                {
                    //si hay un marciano
                    if (marcianos[i,j] != null)
                    {
                        //si un marciano llega al limite de la derecha
                        if (marcianos[i,j].limiteDerecha(AnchoPantalla))
                        {
                            //bandera voy abajo a true
                            voyAbajo = true;
                            //pongo la bandera voyIzquierda a true
                            voyIzquierda = true;
                            //salgo del for porque uno de ellos a llegado al limite
                            break;
                        }
                        else
                        {
                            //si no llegue al limite por la derecha, miro si llegue al limite por la izquierda
                            if (marcianos[i,j].limiteIzquierda())
                            {
                                //bandera voy abajo a true
                                voyAbajo = true;
                                //pongo la bandera voyIzquierda a false
                                voyIzquierda = false;
                                //salgo del for porque uno de ellos a llegado al limite
                                break;
                            }
                        }
                    }
                }
            }
        }
        
        public void mueveMarcianos()
        {
            //recorro las filas
            for (int i = 0; i < marcianos.GetLength(0); i++)
            {
                //recorro las columnas
                for (int j = 0; j < marcianos.GetLength(1); j++)
                {
                    //si hay un marciano
                    if (marcianos[i,j] != null)
                    {
                        //lo muevo de manera lateral segun en que dirección tengo que ir
                        marcianos[i,j].moverLateral(voyIzquierda);
                        //en caso de tener que descender un nivel, lo hace
                        marcianos[i,j].moverAbajo(voyAbajo);
                        if (marcianos[i,j].limiteAbajo(AltoPantalla - miNave.getAlto()))
                        {
                            //if (mejoraPuntuacion())
                            //{
                            //    pideSiglas = true;
                            //}
                            //else
                            //{
                            //    perdi = true;
                            //}
                            //estoyJugando = false;
                        }
                    }
                }
            }
            //después de mover todos los marcianos
            //pongo la bandera voyAbajo a false
            voyAbajo = false;
        }

        public void DibujaOpciones()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.DrawString(fuenteTitulo, "OPCIONES", new Vector2(AnchoPantalla / 2 - fuenteTitulo.MeasureString(titulo).X / 2, AltoPantalla / 6), Color.White);
            spriteBatch.End();
        }
        //MÉTODO ENCARGADO DE GESTIONAR LA LÓGICA DEL MENÚ OPCIONES
        public void GestionaOpciones()
        {

        }

        //AYUDA
        //MÉTODO ENCARGADO DE DIBUJAR EL MENÚ AYUDA
        public void DibujaAyuda()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.DrawString(fuenteTitulo, "AYUDA", new Vector2(AnchoPantalla / 2 - fuenteTitulo.MeasureString(titulo).X / 2, AltoPantalla / 6), Color.White);
            spriteBatch.End();
        }
        //MÉTODO ENCARGADO DE GESTIONAR LA LÓGICA DEL MENÚ OPCIONES
        public void GestionaAyuda()
        {

        }

        //RÉCORDS        
        //MÉTODO ENCARGADO DE DIBUJAR EL MENÚ RÉCORDS
        public void DibujaRecords()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.DrawString(fuenteTitulo, "RECORDS", new Vector2(AnchoPantalla / 2 - fuenteTitulo.MeasureString(titulo).X / 2, AltoPantalla / 6), Color.White);
            spriteBatch.End();
        }
        //MÉTODO ENCARGADO DE GESTIONAR LA LÓGICA DEL MENÚ RÉCORDS
        public void GestionaRecords()
        {

        }
        //CRÉDITOS
        //MÉTODO ENCARGADO DE DIBUJAR EL MENÚ CRÉDITOS
        public void DibujaCreditos()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.DrawString(fuenteTitulo, "CREDITOS", new Vector2(AnchoPantalla / 2 - fuenteTitulo.MeasureString(titulo).X / 2, AltoPantalla / 6), Color.White);
            spriteBatch.End();
        }
        //MÉTODO ENCARGADO DE GESTIONAR LA LÓGICA DEL MENÚ CRÉDITOS
        public void GestionaCreditos()
        {

        }

        //PULSA
        //MÉTODO PARA SABER SI HEMOS PULSADO CON EL BOTON IZQUIERDO DEL MOUSE UNA REGION ESPECIFICA DE LA PANTALLA
        public bool ClickIzq(Boton b)
        {
            //SI PULSO EL BOTON IZQUIERDO DEL RATON SOBRE UN RECTANGULO DADO, DEVUELVO TRUE
            if (b.GetContenedor().Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y))) return true;

            //EN CASO CONTRARIO, DEVUELVO FALSE
            return false;
        }

        public bool LevantoIzq(Boton b)
        {
            if (b.GetBandera() && b.GetContenedor().Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y))) return true;
            return false;
        }

        public void TeclaPulsada()
        {
        }
    }
}
