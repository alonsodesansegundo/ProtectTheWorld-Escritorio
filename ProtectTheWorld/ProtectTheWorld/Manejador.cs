using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.ViewManagement;

namespace ProtectTheWorld
{
    //ENUMERADO PARA EL CONTROL DE ESCENAS
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
        private string modo, preguntaPausa, txtReanudar, txtSalir;
        private int filas, columnas, nivel, primeraX, primeraY, altoMarciano, anchoMarciano,
            altoProyectilNave, anchoProyectilNave, altoNave, anchoNave, vNave, probabilidadDisparoMarcianos, aleatorio,
            puntuacionGlobal, auxiliar, anchoProyectilMarciano, altoProyectilMarciano, vBalaMarciano;
        private Random generador;
        private double vMarciano, vBala;
        private Marciano[,] marcianos;
        private ArrayList misColumnas;
        private List<BalaMarciano> balasMarcianos;
        private Boton btnPausa, btnReanudar, btnSalir, btnMusica;
        private Texture2D imgMarciano1, imgMarciano2, imgNave, imgBala, imgBalaMarciano, explosion, imgBtnPausa, imgBtnPlay,imgMusicaOn,imgMusicaOff;
        private Nave miNave;
        private bool voyIzquierda, voyAbajo, estoyJugando, mueveNave, mejoraPuntuacion, pideSiglas, perdi;

        //**********************OPCIONES**********************
        private Boton btnVolverMenu;
        private Texture2D imgBtnVolver;

        //**********************AYUDA**********************
        private string txtFindalidad, txtNave, txtNiveles, txtMarcianos, tFin, tNave, tNiveles, tMarcianos,impacto1,impacto2,txtP,txtP2;
        //**********************CONSTRUCTOR**********************
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

            //TODAS LAS STRINGS (TEXTOS)
            CargarTextos();

            //TODAS LAS FUENTES
            CargarFuentes();

            //TODAS LAS IMAGENES
            CargarImagenes();

            //Botones del menú
            CrearBotonesMenu();

            //Botones del juego
            CrearBotonesJuego();

            //Botones de opciones
            CrearBotonesOpciones();
            int ancho = AnchoPantalla / 25;
            btnVolverMenu = new Boton(graphics, spriteBatch, AnchoPantalla - ancho, 0, ancho, ancho, Color.Transparent);
            btnVolverMenu.SetImagen(imgBtnVolver);

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
                    GestionaCreditos();
                    break;
                case EstadoJuego.Records:
                    GestionaRecords();
                    break;
                case EstadoJuego.Opciones:
                    GestionaOpciones();
                    break;
                case EstadoJuego.Ayuda:
                    GestionaAyuda();
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
                case EstadoJuego.Ayuda:
                    DibujaAyuda();
                    break;
                case EstadoJuego.Records:
                    DibujaRecords();
                    break;
                case EstadoJuego.Opciones:
                    DibujaOpciones();
                    break;
            }
        }
        //TODOS LOS TEXTOS
        public void CargarTextos()
        {
            titulo = "Protect the World";
            txtJugar = "Jugar";
            txtOpciones = "Opciones";
            txtAyuda = "Ayuda";
            txtCreditos = "Creditos";
            txtRecords = "Records";
            preguntaPausa = "Que deseas hacer?";
            txtReanudar = "Reanudar";
            txtSalir = "Salir";
            CargarTextosAyuda();
        }
        public void CargarTextosAyuda()
        {
            txtFindalidad = "La finalidad de este juego es sobrevivir el mayor tiempo posible y eliminar todos los marcianos que podamos antes de que estos nos invadan o nos eliminen.";
            txtNave = "Nuestra nave espacial disparará a través de la barra espaciadora, y de manera que solo habrá un único proyectil de nuestra nave en la pantalla. Podremos mover dicha nave a través de las flechas (izquierda y/o derecha).";
            txtNiveles = "Este juego contará con niveles infinitos, pero que irán aumentando en cuanto a dificultad. A medida que vayamos completando niveles, se cambiará una fila de marcianos de un impacto por marcianos de dos impactos. Una vez pasemos el nivel en el que solamente hay marcianos de dos impactos, volveremos al comienzo de los niveles, pero los marcianos se moverán más rápido.";
            txtMarcianos = "Contamos con dos tipos de marcianos: marcianos que son eliminados tras recibir un único impacto, y otros marcianos que son eliminados tras recibir dos impactos. Estos últimos, en el momento que reciben el primer impacto se convierten en un marciano de un impacto.";
            tFin = "Finalidad";
            tNiveles = "Niveles";
            tNave = "Nave";
            tMarcianos = "Marcianos";
            impacto1 = "Un impacto";
            impacto2 = "Dos impactos";
            txtP = "10 puntos";
            txtP2 = "25 puntos";
        }
        //TODAS LAS FUENTES
        public void CargarFuentes()
        {
            fuenteTitulo = Content.Load<SpriteFont>("Fuentes/FuenteTitulo");
            fuenteBotones = Content.Load<SpriteFont>("Fuentes/FuenteBotones");
        }
        //TODAS LAS IMAGENES
        public void CargarImagenes()
        {
            fondoMenu = Content.Load<Texture2D>("fondomenu");
            imgMarciano1 = Content.Load<Texture2D>("mio1");
            imgMarciano2 = Content.Load<Texture2D>("mio2");
            imgBala = Content.Load<Texture2D>("proyectilnave");
            imgBalaMarciano = Content.Load<Texture2D>("bombamarciano");
            imgNave = Content.Load<Texture2D>("nave1");
            explosion = Content.Load<Texture2D>("explosion");
            imgBtnVolver = Content.Load<Texture2D>("back");
            imgBtnPausa = Content.Load<Texture2D>("pause");
            imgBtnPlay = Content.Load<Texture2D>("play");
            imgMusicaOff = Content.Load<Texture2D>("musicano");
            imgMusicaOn = Content.Load<Texture2D>("musica");
        }
        //MENÚ PRINCIPAL
        public void CrearBotonesMenu()
        {
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
        }
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
            btnJugar.SetTexto(txtJugar, fuenteBotones, Color.White,false);
            btnOpciones.SetTexto(txtOpciones, fuenteBotones, Color.White,false);
            btnAyuda.SetTexto(txtAyuda, fuenteBotones, Color.White,false);
            btnRecords.SetTexto(txtRecords, fuenteBotones, Color.White,false);
            btnCreditos.SetTexto(txtCreditos, fuenteBotones, Color.White,false);
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
                        {
                            estadoActualJuego = EstadoJuego.Gameplay;
                            //**********************JUEGO**********************
                            CargarJuego();
                        }
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

        //*****************************************************************JUEGO*****************************************************************

        //------------------------GESTION TECLADO JUEGO------------------------
        public void gestionaTeclado()
        {
            //TECLADO
            teclado = Keyboard.GetState();
            //CONFIGURACION FLECHAS
            //SI PULSA FLECHA ABAJO
            //if (teclado.IsKeyDown(Keys.Down))
            //    estoyJugando = false;

            if (estoyJugando)
                if (estoyJugando)
                {
                    //SI PULSA LA FLECHA DRCH
                    if (teclado.IsKeyDown(Keys.Right))
                        miNave.moverNave(miNave.getX() + vNave, AnchoPantalla - anchoNave);
                    //SI PULSA LA FLECHA IZQ
                    if (teclado.IsKeyDown(Keys.Left))
                        miNave.moverNave(miNave.getX() - vNave, AnchoPantalla - anchoNave);
                    //SI PULSA EL ESPACIO
                    if (teclado.IsKeyDown(Keys.Space))
                        miNave.disparar();
                }


            //CONFIGURACION ALTERNATIVA
            //SI PULSA W
            //if (teclado.IsKeyDown(Keys.S))
            //    estadoActualJuego = EstadoJuego.Menu;
            ////SI PULSA LA D
            //if (teclado.IsKeyDown(Keys.D))
            //    miNave.moverNave(miNave.getX() + vNave, AnchoPantalla - anchoNave);
            ////SI PULSA LA A
            //if (teclado.IsKeyDown(Keys.A))
            //    miNave.moverNave(miNave.getX() - vNave, AnchoPantalla - anchoNave);
            ////SI PULSA FLECHA ARRIBA
            //if (teclado.IsKeyDown(Keys.W))
            //    miNave.disparar();
        }
        //------------------------RELLENA EL ARRAY DE MARCIANOS------------------------
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
        //------------------------BOTONES JUEGO------------------------
        public void CrearBotonesJuego()
        {
            int ancho = AnchoPantalla / 25;
            btnPausa = new Boton(graphics, spriteBatch, AnchoPantalla - ancho, 0, ancho, ancho, Color.Transparent);
            btnPausa.SetImagen(imgBtnPausa);

            btnMusica = new Boton(graphics, spriteBatch, AnchoPantalla - ancho * 2, 0, ancho, ancho, Color.Transparent);
            btnMusica.SetImagen(imgMusicaOn);

            //int espacio = AnchoPantalla / 20;
            //(int)fuenteTitulo.MeasureString(titulo).X)
            btnReanudar = new Boton(graphics, spriteBatch,
                AnchoPantalla / 2 - (int)fuenteBotones.MeasureString(txtReanudar).X /*- espacio*/,
                AltoPantalla / 2 - (int)fuenteBotones.MeasureString(txtReanudar).Y,
                (int)fuenteBotones.MeasureString(txtReanudar).X,
                (int)fuenteBotones.MeasureString(txtReanudar).Y,
                Color.Green);
            btnReanudar.SetTexto(txtReanudar, fuenteBotones, Color.Black,true);

            btnSalir = new Boton(graphics, spriteBatch,
                AnchoPantalla / 2 + espacio,
                AltoPantalla / 2 - (int)fuenteBotones.MeasureString(txtSalir).Y ,
                (int)fuenteBotones.MeasureString(txtSalir).X,
                (int)fuenteBotones.MeasureString(txtSalir).Y*2,
                Color.Red);
            btnSalir.SetTexto(txtSalir, fuenteBotones, Color.Black,true);
        }
        //------------------------HAY MARCIANOS------------------------
        public bool hayMarcianos()
        {
            for (int i = 0; i < marcianos.GetLength(0); i++)
            {
                for (int j = 0; j < marcianos.GetLength(1); j++)
                {
                    if (marcianos[i, j] != null)
                    {
                        return true;
                    }
                }
            }
            //si no he encontrado ningun null
            return false;
        }
        //------------------------MOVIMIENTO BALA NAVE------------------------
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
                    if (misColumnas.IndexOf(j) == -1 && marcianos[i, j] != null)
                    {
                        //añado la columna a mi arraylist
                        misColumnas.Add(j);
                        //si se decide que el marciano dispare (porque sale x probabilidad)
                        //probabilidadDisparoMarcianos++;

                        aleatorio = generador.Next(0, 101);
                        if (marcianos[i, j].dispara(probabilidadDisparoMarcianos, aleatorio))
                        {
                            //genero una nueva bala marciano que añado a su array
                            balasMarcianos.Add(new BalaMarciano(graphics, spriteBatch, (int)marcianos[i, j].getContenedor().Center.X -
                                    anchoProyectilMarciano / 2,
                                    (int)marcianos[i, j].getPos().Y + altoMarciano,
                                    anchoProyectilMarciano,
                                   altoProyectilMarciano, imgBalaMarciano, vBalaMarciano));
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
                    estoyJugando = false;
                    mueveNave = false;
                    //acabaMusica();
                    //if (mejoraPuntuacion())
                    //{
                    //    pideSiglas = true;
                    //}
                    //else
                    //{
                    //    perdi = true;
                    //}
                    perdi = true;
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
                    if (marcianos[i, j] != null)
                    {
                        //si un marciano llega al limite de la derecha
                        if (marcianos[i, j].limiteDerecha(AnchoPantalla))
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
                            if (marcianos[i, j].limiteIzquierda())
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
                    if (marcianos[i, j] != null)
                    {
                        //lo muevo de manera lateral segun en que dirección tengo que ir
                        marcianos[i, j].moverLateral(voyIzquierda);
                        //en caso de tener que descender un nivel, lo hace
                        marcianos[i, j].moverAbajo(voyAbajo);
                        if (marcianos[i, j].limiteAbajo(AltoPantalla - miNave.getAlto()))
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
        //------------------------INICIALIZA LAS VARIABLES DEL JUEGO------------------------
        public void CargarJuego()
        {
            //control
            modo = "jugando";
            //booleanas de control
            estoyJugando = true;
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
            vBalaMarciano = 3;
            vMarciano = 1;
            marcianos = new Marciano[filas, columnas];
            balasMarcianos = new List<BalaMarciano>();
            probabilidadDisparoMarcianos = 33;
            generador = new Random();
            misColumnas = new ArrayList();

            //relleno el array bidimensional de marcianos
            rellenaMarcianos();

            voyAbajo = false;
            voyIzquierda = false;

            //nave
            vNave = 10;
            vBala = 6;
            altoNave = AnchoPantalla / 20;
            anchoNave = AnchoPantalla / 20;
            altoProyectilNave = altoNave / 2;
            anchoProyectilNave = anchoNave / 4;
            miNave = new Nave(graphics, spriteBatch, imgNave,
                AnchoPantalla / 2 - anchoNave / 2, AltoPantalla - altoNave, anchoNave, altoNave,
               anchoProyectilNave, altoProyectilNave, vBala, imgBala);
        }
        //MÉTODO ENCARGADO DE DIBUJAR EL JUEGO
        public void DibujaJuego()
        {
            //dibujo el fondo negro
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            //dibujo la puntuacion, marcianos, nave, balas marcianos, bala nave
            DibujaGameplay();
            switch (modo)
            {
                case "pausa":
                    DibujaPausa();
                    break;
            }
            //dibujo los botones
            btnPausa.Dibuja();
            btnMusica.Dibuja();
            spriteBatch.End();
        }
        public void DibujaPausa()
        {
            //dibujo el rectangulo
            Texture2D rectangulo = new Texture2D(graphics.GraphicsDevice, AnchoPantalla/3, AltoPantalla/3);
           Color[] data = new Color[AnchoPantalla/3 * AltoPantalla/3];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.LightGray;
            rectangulo.SetData(data);
            Vector2 punto = new Vector2(AnchoPantalla/2-AnchoPantalla/6,AltoPantalla/2-AltoPantalla/6);
            spriteBatch.Draw(rectangulo, punto, Color.White);

            //dibujo la pregunta
            spriteBatch.DrawString(fuenteBotones, preguntaPausa, new Vector2(AnchoPantalla/2-(int)fuenteBotones.MeasureString(preguntaPausa).X/2,punto.Y), Color.Black);

            //dibujo los botones
            btnReanudar.Dibuja();
            btnSalir.Dibuja();

        }
        public void DibujaGameplay()
        {
            //dibujo la puntuacion
            spriteBatch.DrawString(fuenteBotones,
                puntuacionGlobal.ToString(),
                new Vector2(AnchoPantalla / 2 - fuenteBotones.MeasureString(puntuacionGlobal.ToString()).X / 2,
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
        }

        //MÉTODO ENCARGADO DE GESTIONAR LA LÓGICA DEL JUEGO
        public void GestionaJuego()
        {
            switch (modo)
            {
                case "jugando":
                    auxiliar++;
                    //BALAS MARCIANOS
                    if (auxiliar == 100)
                    {
                        auxiliar = 0;
                        disparanMarcianos();
                    }
                    //MOVIMIENTO BALAS MARCIANOS
                    actualizaBalasMarcianos();

                    //BANDERAS MOVIMIENTO MARCIANOS
                    actualizaBanderasMovimiento();

                    //MOVIMIENTO MARCIANOS
                    mueveMarcianos();

                    //MOVIMIENTO BALA NAVE
                    mueveBalaNave();
                    //GESTION PULSACION BOTON PAUSA
                    GestionaMenuPausa();
                    break;
                case "pausa":
                    GestionaMenuPausa();
                    break;
                case "hacerInsert":
                    break;
            }
            //TECLADO
            gestionaTeclado();
        }
        //------------------------PULSA PAUSA------------------------
        public void GestionaMenuPausa()
        {
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
                        //veo si he pulsado en el boton volver, de ser así, pongo su bandera a true
                        if (ClickIzq(btnPausa))
                            btnPausa.SetBandera(true);

                        if (ClickIzq(btnReanudar))
                            btnReanudar.SetBandera(true);
                        if (ClickIzq(btnSalir))
                            btnSalir.SetBandera(true);
                        break;
                    //si el boton izquierdo no está pulsado, se ha levantado, hago lo que obedezca a dicho boton
                    case ButtonState.Released:
                        if (LevantoIzq(btnPausa))
                        {
                            switch (modo)
                            {
                                case "jugando":
                                    modo = "pausa";
                                    btnPausa.SetImagen(imgBtnPlay);
                                    break;
                                case "pausa":
                                    btnPausa.SetImagen(imgBtnPausa);
                                    modo = "jugando";
                                    break;
                            }
                        }
                        if (LevantoIzq(btnReanudar))
                        {
                            btnPausa.SetImagen(imgBtnPausa);
                            modo = "jugando";
                        }

                        if (LevantoIzq(btnSalir))
                            estadoActualJuego = EstadoJuego.Menu;

                        //pongo a false las banderas de todos los botones del menu opciones
                        btnPausa.SetBandera(false);
                        btnReanudar.SetBandera(false);
                        btnSalir.SetBandera(false);
                        break;
                }
            }
        }
        public void PulsaReanudar()
        {
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
                        //veo si he pulsado en el boton volver, de ser así, pongo su bandera a true
                        if (ClickIzq(btnReanudar))
                            btnReanudar.SetBandera(true);
                        break;
                    //si el boton izquierdo no está pulsado, se ha levantado, hago lo que obedezca a dicho boton
                    case ButtonState.Released:
                        if (LevantoIzq(btnReanudar))
                            modo = "jugando";

                        //pongo a false las banderas de todos los botones del menu opciones
                        btnReanudar.SetBandera(false);
                        break;
                }
            }
        }
        public void PulsaSalir()
        {

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
                        //veo si he pulsado en el boton volver, de ser así, pongo su bandera a true
                        if (ClickIzq(btnSalir))
                            btnSalir.SetBandera(true);
                        break;
                    //si el boton izquierdo no está pulsado, se ha levantado, hago lo que obedezca a dicho boton
                    case ButtonState.Released:
                        if (LevantoIzq(btnSalir))
                            estadoActualJuego = EstadoJuego.Menu;

                        //pongo a false las banderas de todos los botones del menu opciones
                        btnSalir.SetBandera(false);
                        break;
                }
            }
        }
        public void gestionaMenuPausa()
        {
            GestionaMenuPausa();

            PulsaReanudar();
            PulsaSalir();
        }

        //*****************************************************************OPCIONES*****************************************************************
        public void CrearBotonesOpciones()
        {
            
        }
        public void DibujaOpciones()
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.DrawString(fuenteTitulo, txtOpciones, new Vector2(AnchoPantalla / 2 - fuenteTitulo.MeasureString(txtOpciones).X / 2, AltoPantalla / 20), Color.White);
            btnVolverMenu.Dibuja();
            spriteBatch.End();
        }
        //MÉTODO ENCARGADO DE GESTIONAR LA LÓGICA DEL MENÚ OPCIONES
        public void GestionaOpciones()
        {
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
                        //veo si he pulsado en el boton volver, de ser así, pongo su bandera a true
                        if (ClickIzq(btnVolverMenu))
                            btnVolverMenu.SetBandera(true);
                        break;
                    //si el boton izquierdo no está pulsado, se ha levantado, hago lo que obedezca a dicho boton
                    case ButtonState.Released:
                        if (LevantoIzq(btnVolverMenu))
                            estadoActualJuego = EstadoJuego.Menu;

                        //pongo a false las banderas de todos los botones del menu opciones
                        btnVolverMenu.SetBandera(false);
                        break;
                }
            }
        }

        //AYUDA
        //MÉTODO ENCARGADO DE DIBUJAR EL MENÚ AYUDA
        public void DibujaAyuda()
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.DrawString(fuenteTitulo, txtAyuda, new Vector2(AnchoPantalla / 2 - fuenteTitulo.MeasureString(txtOpciones).X / 2, AltoPantalla / 20), Color.White);
            btnVolverMenu.Dibuja();
            spriteBatch.End();
        }
        //MÉTODO ENCARGADO DE GESTIONAR LA LÓGICA DEL MENÚ OPCIONES
        public void GestionaAyuda()
        {
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
                        //veo si he pulsado en el boton volver, de ser así, pongo su bandera a true
                        if (ClickIzq(btnVolverMenu))
                            btnVolverMenu.SetBandera(true);
                        break;
                    //si el boton izquierdo no está pulsado, se ha levantado, hago lo que obedezca a dicho boton
                    case ButtonState.Released:
                        if (LevantoIzq(btnVolverMenu))
                            estadoActualJuego = EstadoJuego.Menu;

                        //pongo a false las banderas de todos los botones del menu opciones
                        btnVolverMenu.SetBandera(false);
                        break;
                }
            }
        }

        //RÉCORDS        
        //MÉTODO ENCARGADO DE DIBUJAR EL MENÚ RÉCORDS
        public void DibujaRecords()
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.DrawString(fuenteTitulo, txtRecords, new Vector2(AnchoPantalla / 2 - fuenteTitulo.MeasureString(txtOpciones).X / 2, AltoPantalla / 20), Color.White);
            btnVolverMenu.Dibuja();
            spriteBatch.End();
        }
        //MÉTODO ENCARGADO DE GESTIONAR LA LÓGICA DEL MENÚ RÉCORDS
        public void GestionaRecords()
        {
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
                        //veo si he pulsado en el boton volver, de ser así, pongo su bandera a true
                        if (ClickIzq(btnVolverMenu))
                            btnVolverMenu.SetBandera(true);
                        break;
                    //si el boton izquierdo no está pulsado, se ha levantado, hago lo que obedezca a dicho boton
                    case ButtonState.Released:
                        if (LevantoIzq(btnVolverMenu))
                            estadoActualJuego = EstadoJuego.Menu;

                        //pongo a false las banderas de todos los botones del menu opciones
                        btnVolverMenu.SetBandera(false);
                        break;
                }
            }

           
        }
        //CRÉDITOS
        //MÉTODO ENCARGADO DE DIBUJAR EL MENÚ CRÉDITOS
        public void DibujaCreditos()
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.DrawString(fuenteTitulo, txtCreditos, new Vector2(AnchoPantalla / 2 - fuenteTitulo.MeasureString(txtOpciones).X / 2, AltoPantalla / 20), Color.White);
            btnVolverMenu.Dibuja();
            spriteBatch.End();
        }
        //MÉTODO ENCARGADO DE GESTIONAR LA LÓGICA DEL MENÚ CRÉDITOS
        public void GestionaCreditos()
        {
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
                        //veo si he pulsado en el boton volver, de ser así, pongo su bandera a true
                        if (ClickIzq(btnVolverMenu))
                            btnVolverMenu.SetBandera(true);
                        break;
                    //si el boton izquierdo no está pulsado, se ha levantado, hago lo que obedezca a dicho boton
                    case ButtonState.Released:
                        if (LevantoIzq(btnVolverMenu))
                            estadoActualJuego = EstadoJuego.Menu;

                        //pongo a false las banderas de todos los botones del menu opciones
                        btnVolverMenu.SetBandera(false);
                        break;
                }
            }
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
