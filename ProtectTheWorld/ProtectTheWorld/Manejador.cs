using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        private Texture2D fondoSubmenu;
        //**********************MENU**********************
        private int espacio;
        private EstadoJuego estadoActualJuego;
        private ButtonState estadoClickIzq;
        private int AnchoPantalla, AltoPantalla;
        private SpriteFont fuenteTitulo, fuenteBotones, fuenteSub;
        private string titulo, txtJugar, txtOpciones, txtAyuda, txtRecords, txtCreditos;
        private List<Boton> botonesMenu;
        private Texture2D fondoMenu;
        private Boton btnJugar, btnOpciones, btnAyuda, btnRecords, btnCreditos;
        private Song cancionMenu;

        //**********************JUEGO**********************
        private string modo, preguntaPausa,preguntaRepetir, txtReanudar, txtSalir;
        private int filas, columnas, nivel, primeraX, primeraY, altoMarciano, anchoMarciano,
            altoProyectilNave, anchoProyectilNave, altoNave, anchoNave, vNave, probabilidadDisparoMarcianos, aleatorio,
            puntuacionGlobal, auxiliar, anchoProyectilMarciano, altoProyectilMarciano, vBalaMarciano;
        private Random generador;
        private double vMarciano, vBala;
        private Marciano[,] marcianos;
        private ArrayList misColumnas;
        private List<BalaMarciano> balasMarcianos;
        private Boton btnPausa, btnReanudar, btnSalir,btnRepetirSi,btnRepetirNo, btnMusica;
        private Texture2D imgMarciano1, imgMarciano2, imgNave1, imgNave2, imgNave3, imgBala, imgBalaMarciano, explosion, imgBtnPausa, imgBtnPlay, imgMusicaOn, imgMusicaOff;
        private Nave miNave;
        private bool voyIzquierda, voyAbajo;
        //estoyJugando, mueveNave, mejoraPuntuacion, pideSiglas, perdi;

        //menu pausa
        private int altoMenuPausa, anchoMenuPausa, espacioMenuPausa;
        private Texture2D rectanguloPausa;
        private Color[] dataPausa;
        private Vector2 puntoPausa;
        private Song cancionJuego;

        //menu introduce tus siglas
        private List<string> datos;
        private Texture2D rectanguloSiglas;
        private Color[] dataSiglas;
        private string txtIntroduceSiglas, txtEnviar;
        private int altoMenuSiglas, anchoMenuSiglas, espacioMenuSiglas;
        private Vector2 puntoSiglas;
        private Boton btnSiglaArriba, btnSiglaArriba2, btnSiglaArriba3, btnSiglaAbajo, btnSiglaAbajo2, btnSiglaAbajo3, btnEnviarRecord;
        private Texture2D trianguloArriba, trianguloAbajo;
        private ArrayList abecedario;
        private char[] siglas;

        private Song cancionSubmenu;
        //**********************OPCIONES**********************
        private Boton btnVolverMenu, btnNave1, btnNave2, btnNave3, btnMusicaSi, btnMusicaNo;
        private string txtSeleccionaNave, txtMusica, txtSi, txtNo;
        private Texture2D imgBtnVolver;
        private bool boolMusica;
        private int numNave;

        //**********************AYUDA**********************
        private string txtFinalidad, txtNave, txtNiveles, txtMarcianos, tFin, tNave, tNiveles, tMarcianos, impacto1, impacto2, txtP, txtP2, modoAyuda;
        private Boton btnFinalidad, btnNiveles, btnNave, btnMarcianos;
        private string[] infoFinalidad, infoNave, infoNiveles, infoMarcianos;

        //**********************RECORDS**********************
        private List<string> listaPuntuaciones, listaSiglas;
        private Texture2D imgMedallaOro, imgMedallaPlata, imgMedallaBronce;
        int anchoMedalla;
        //**********************CREDITOS**********************
        private string txtFuente, txtImagenes, txtImg, txtImg2, txtMusic, txtHecho,txtYoutube,txtCanal;
        private bool moverCreditos;
        private int posCreditos;
        private int modoCreditos;
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
            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();


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


            //BASE DE DATOS
            DataAccessLibrary.DataAcess.InitializeDatabase();

            //NAVE CON LA QUE VOY A JUGAR
            List<String> op = DataAccessLibrary.DataAcess.DameOpciones();
            int aux1 = Convert.ToInt32(op[0]);
            switch (aux1)
            {
                case 3:
                    numNave = 3;
                    break;
                case 1:
                    numNave = 1;
                    break;
                case 2:
                    numNave = 2;
                    break;
            }

            ////MUSICA SI O NO
            int aux2 = Convert.ToInt32(op[1]);
            if (aux2 == 1)
            {
                boolMusica = true;
            }
            else
            {
                boolMusica = false;
            }



            //TODAS LAS FUENTES
            CargarFuentes();

            //TODAS LAS IMAGENES
            CargarImagenes();

            //TODOS LOS SONIDOS 
            CargarAudio();

            //CARGO LOS DATOS DEL MENU
            CargarMenu();

            //CARGO LOS DATOS DEL GAMEPLAY
            CargarJuego();

            //CARGO LOS DATOS DE LAS OPCIONES
            CargarOpciones();

            //CARGO LOS DATOS DE LA AYUDA
            CargarAyuda();

            //CARGO LOS DATOS DE LOS RECORDS
            CargarRecords();

            //CARGO LOS DATOS DE LOS CREDITOS
            CargarCreditos();

            //BOTON PARA VOLVER DESDE LAS DIFERENTES PANTALLAS
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
            //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;

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

        //*****************************************************************FUENTES*****************************************************************
        public void CargarFuentes()
        {
            fuenteTitulo = Content.Load<SpriteFont>("Fuentes/FuenteTitulo");
            fuenteBotones = Content.Load<SpriteFont>("Fuentes/FuenteBotones");
            fuenteSub = Content.Load<SpriteFont>("Fuentes/FuenteSub");
        }

        //*****************************************************************IMAGENES*****************************************************************
        public void CargarImagenes()
        {
            fondoMenu = Content.Load<Texture2D>("fondomenu");
            fondoSubmenu = Content.Load<Texture2D>("fondoSubmenu");
            imgMarciano1 = Content.Load<Texture2D>("mio1");
            imgMarciano2 = Content.Load<Texture2D>("mio2");
            imgBala = Content.Load<Texture2D>("proyectilnave");
            imgBalaMarciano = Content.Load<Texture2D>("bombamarciano");
            imgNave1 = Content.Load<Texture2D>("nave1");
            imgNave2 = Content.Load<Texture2D>("nave2");
            imgNave3 = Content.Load<Texture2D>("nave3");
            explosion = Content.Load<Texture2D>("explosion");
            imgBtnVolver = Content.Load<Texture2D>("back");
            imgBtnPausa = Content.Load<Texture2D>("pause");
            imgBtnPlay = Content.Load<Texture2D>("play");
            imgMusicaOff = Content.Load<Texture2D>("musicano");
            imgMusicaOn = Content.Load<Texture2D>("musica");
            trianguloAbajo = Content.Load<Texture2D>("triangulodown");
            trianguloArriba = Content.Load<Texture2D>("trianguloup");
            imgMedallaOro = Content.Load<Texture2D>("oro");
            imgMedallaPlata = Content.Load<Texture2D>("plata");
            imgMedallaBronce = Content.Load<Texture2D>("bronce");
        }

        //*****************************************************************CANCIONES*****************************************************************
        public void CargarAudio()
        {
            cancionMenu = Content.Load<Song>("musicamenu");
            cancionJuego = Content.Load<Song>("game");
            cancionSubmenu = Content.Load<Song>("submenus");
        }

        //*****************************************************************MENU*****************************************************************
        public void CargarMenu()
        {
            CargarTextosMenu();
            CrearBotonesMenu();
            MediaPlayer.IsRepeating = true;
            if (boolMusica)
                SuenaCancion(cancionMenu);
        }
        public void CargarTextosMenu()
        {
            titulo = "Protect the World";
            txtJugar = "Jugar";
            txtOpciones = "Opciones";
            txtAyuda = "Ayuda";
            txtCreditos = "Creditos";
            txtRecords = "Records";
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
            btnJugar.SetTexto(txtJugar, fuenteBotones, Color.White, false);
            btnOpciones.SetTexto(txtOpciones, fuenteBotones, Color.White, false);
            btnAyuda.SetTexto(txtAyuda, fuenteBotones, Color.White, false);
            btnRecords.SetTexto(txtRecords, fuenteBotones, Color.White, false);
            btnCreditos.SetTexto(txtCreditos, fuenteBotones, Color.White, false);
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
                            InicializaVariablesJuego();
                            //pongo la musica del juego
                            if (boolMusica)
                                SuenaCancion(cancionJuego);
                        }
                        //titulo = "FUNCIONA";
                        if (LevantoIzq(btnOpciones))
                        {
                            estadoActualJuego = EstadoJuego.Opciones;
                            //pongo la musica del submenu
                            if (boolMusica)
                                SuenaCancion(cancionSubmenu);
                        }
                        if (LevantoIzq(btnAyuda))
                        {
                            estadoActualJuego = EstadoJuego.Ayuda;
                            if (boolMusica)
                                SuenaCancion(cancionSubmenu);
                        }
                        if (LevantoIzq(btnRecords))
                        {
                            estadoActualJuego = EstadoJuego.Records;
                            if (boolMusica)
                                SuenaCancion(cancionSubmenu);
                        }
                        if (LevantoIzq(btnCreditos))
                        {
                            estadoActualJuego = EstadoJuego.Creditos;
                            if (boolMusica)
                                SuenaCancion(cancionSubmenu);
                        }

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
        public void CargarJuego()
        {
            CargarTextosJuego();
            CrearBotonesJuego();
            InicializaVariablesJuego();
        }
        public void InicializaVariablesJuego()
        {
            //siglas primeras
            siglas = new char[3];
            siglas[0] = 'A';
            siglas[1] = 'A';
            siglas[2] = 'A';
            if (!boolMusica)
            {
                btnMusica.SetImagen(imgMusicaOn);
            }
            else
            {
                btnMusica.SetImagen(imgMusicaOff);
            }
            //para el menu pausa
            //espacioMenuPausa = 20;
            //altoMenuPausa = AltoPantalla / 4;
            anchoMenuPausa = (int)fuenteSub.MeasureString(preguntaPausa).X + espacioMenuPausa * 2;

            rectanguloPausa = new Texture2D(graphics.GraphicsDevice,
              anchoMenuPausa,
              altoMenuPausa);
            dataPausa = new Color[anchoMenuPausa * altoMenuPausa];
            for (int i = 0; i < dataPausa.Length; ++i) dataPausa[i] = Color.LightGray;
            rectanguloPausa.SetData(dataPausa);
            puntoPausa = new Vector2(AnchoPantalla / 2 - (int)fuenteSub.MeasureString(preguntaPausa).X / 2 - espacioMenuPausa
                , AltoPantalla / 2 - AltoPantalla / 6);

            //para el menu siglas
            //espacioMenuSiglas = 20;
            //altoMenuSiglas = AltoPantalla / 4;
            anchoMenuSiglas = (int)fuenteSub.MeasureString(txtIntroduceSiglas).X + espacioMenuSiglas * 2;

            rectanguloSiglas = new Texture2D(graphics.GraphicsDevice,
                anchoMenuSiglas,
                altoMenuSiglas);
            dataSiglas = new Color[anchoMenuSiglas * altoMenuSiglas];
            for (int i = 0; i < dataSiglas.Length; ++i) dataSiglas[i] = Color.LightGray;
            rectanguloSiglas.SetData(dataSiglas);
            puntoSiglas = new Vector2(AnchoPantalla / 2 - (int)fuenteSub.MeasureString(txtIntroduceSiglas).X / 2 - espacioMenuSiglas
                , AltoPantalla / 2 - AltoPantalla / 6);

            //control
            modo = "jugando";
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
            vNave = 6;
            vBala = 6;
            altoNave = AnchoPantalla / 20;
            anchoNave = AnchoPantalla / 20;
            altoProyectilNave = altoNave / 2;
            anchoProyectilNave = anchoNave / 4;
            miNave = new Nave(graphics, spriteBatch, imgNave1,
                AnchoPantalla / 2 - anchoNave / 2, AltoPantalla - altoNave, anchoNave, altoNave,
               anchoProyectilNave, altoProyectilNave, vBala, imgBala);
            switch (numNave)
            {
                case 1:
                    miNave.setImagen(imgNave1);
                    break;
                case 2:
                    miNave.setImagen(imgNave2);
                    break;
                case 3:
                    miNave.setImagen(imgNave3);
                    break;
            }
        }

        public void CargarTextosJuego()
        {
            txtSi = "Si";
            txtNo = "No";
            preguntaPausa = "Que deseas hacer?";
            preguntaRepetir = "Otra partida?";
            txtReanudar = "Reanudar";
            txtSalir = "Salir";
            txtIntroduceSiglas = "Introduce tus siglas";
            txtEnviar = "Enviar";
            abecedario = new ArrayList();
            for (int i = 0; i < 26; i++)
            {
                abecedario.Add((char)('A' + i));
            }
        }
        public void CrearBotonesJuego()
        {
            int ancho = AnchoPantalla / 25;
            btnPausa = new Boton(graphics, spriteBatch, AnchoPantalla - ancho, 0, ancho, ancho, Color.Transparent);
            btnPausa.SetImagen(imgBtnPausa);

            btnMusica = new Boton(graphics, spriteBatch, AnchoPantalla - ancho * 2, 0, ancho, ancho, Color.Transparent);

            espacioMenuPausa = 20;
            altoMenuPausa = AltoPantalla / 4;
            Vector2 punto = new Vector2(AnchoPantalla / 2 - (int)fuenteSub.MeasureString(preguntaPausa).X / 2 - espacioMenuPausa
               , AltoPantalla / 2 - AltoPantalla / 6);

            anchoMenuPausa = (int)fuenteSub.MeasureString(preguntaPausa).X + espacioMenuPausa * 2;
            btnReanudar = new Boton(graphics, spriteBatch,
                (int)punto.X + espacioMenuPausa,
                AltoPantalla / 2 - (int)fuenteBotones.MeasureString(txtReanudar).Y,
                anchoMenuPausa / 2 - espacioMenuPausa * 2,
                (int)fuenteBotones.MeasureString(txtReanudar).Y * 2,
                Color.Green);
            btnReanudar.SetTexto(txtReanudar, fuenteBotones, Color.Black, true);

            btnRepetirSi = new Boton(graphics, spriteBatch,
                (int)punto.X + espacioMenuPausa,
                AltoPantalla / 2 - (int)fuenteBotones.MeasureString(txtReanudar).Y,
                anchoMenuPausa / 2 - espacioMenuPausa * 2,
                (int)fuenteBotones.MeasureString(txtReanudar).Y * 2,
                Color.Green);
            btnRepetirSi.SetTexto(txtSi, fuenteBotones, Color.Black, true);

            btnSalir = new Boton(graphics, spriteBatch,
                AnchoPantalla / 2 + espacioMenuPausa,
                AltoPantalla / 2 - (int)fuenteBotones.MeasureString(txtSalir).Y,
                anchoMenuPausa / 2 - espacioMenuPausa * 2,
                (int)fuenteBotones.MeasureString(txtReanudar).Y * 2,
                Color.Red);
            btnSalir.SetTexto(txtSalir, fuenteBotones, Color.Black, true);

            btnRepetirNo= new Boton(graphics, spriteBatch,
                AnchoPantalla / 2 + espacioMenuPausa,
                AltoPantalla / 2 - (int)fuenteBotones.MeasureString(txtSalir).Y,
                anchoMenuPausa / 2 - espacioMenuPausa * 2,
                (int)fuenteBotones.MeasureString(txtReanudar).Y * 2,
                Color.Red);
            btnRepetirNo.SetTexto(txtNo, fuenteBotones, Color.Black, true);

            //introduce tus siglas
            espacioMenuSiglas = 20;
            altoMenuSiglas = AltoPantalla / 3;
            anchoMenuSiglas = (int)fuenteSub.MeasureString(txtIntroduceSiglas).X + espacioMenuSiglas * 2;

            puntoSiglas = new Vector2(AnchoPantalla / 2 - (int)fuenteSub.MeasureString(txtIntroduceSiglas).X / 2 - espacioMenuSiglas
                , AltoPantalla / 2 - AltoPantalla / 6);
            //botones arriba
            btnSiglaArriba = new Boton(graphics, spriteBatch,
             (int)puntoSiglas.X + espacioMenuSiglas,
                AltoPantalla / 2 - ancho,
                ancho, ancho, Color.Transparent);
            btnSiglaArriba.SetImagen(trianguloArriba);

            btnSiglaArriba2 = new Boton(graphics, spriteBatch,
             AnchoPantalla / 2 - ancho / 2,
                AltoPantalla / 2 - ancho,
                ancho, ancho, Color.Transparent);
            btnSiglaArriba2.SetImagen(trianguloArriba);

            btnSiglaArriba3 = new Boton(graphics, spriteBatch,
             (int)puntoSiglas.X + anchoMenuSiglas - ancho - espacioMenuSiglas,
                AltoPantalla / 2 - ancho,
                ancho, ancho, Color.Transparent);
            btnSiglaArriba3.SetImagen(trianguloArriba);

            //botones abajo
            btnSiglaAbajo = new Boton(graphics, spriteBatch,
                             (int)puntoSiglas.X + espacioMenuSiglas,
                              (int)puntoSiglas.Y + espacioMenuSiglas * 2 + (int)fuenteSub.MeasureString(txtIntroduceSiglas).Y + ancho + (int)fuenteBotones.MeasureString('A'.ToString()).Y,
                              ancho, ancho, Color.Transparent);
            btnSiglaAbajo.SetImagen(trianguloAbajo);

            btnSiglaAbajo2 = new Boton(graphics, spriteBatch,
                           AnchoPantalla / 2 - ancho / 2,
                              (int)puntoSiglas.Y + espacioMenuSiglas * 2 + (int)fuenteSub.MeasureString(txtIntroduceSiglas).Y + ancho + (int)fuenteBotones.MeasureString('A'.ToString()).Y,
                             ancho, ancho, Color.Transparent);
            btnSiglaAbajo2.SetImagen(trianguloAbajo);

            btnSiglaAbajo3 = new Boton(graphics, spriteBatch,
                         (int)puntoSiglas.X + anchoMenuSiglas - ancho - espacioMenuSiglas,
                              (int)puntoSiglas.Y + espacioMenuSiglas * 2 + (int)fuenteSub.MeasureString(txtIntroduceSiglas).Y + ancho + (int)fuenteBotones.MeasureString('A'.ToString()).Y,
                             ancho, ancho, Color.Transparent);
            btnSiglaAbajo3.SetImagen(trianguloAbajo);

            //boton enviar record
            btnEnviarRecord = new Boton(graphics, spriteBatch,
                (int)puntoSiglas.X,
                (int)puntoSiglas.Y + altoMenuSiglas,
                anchoMenuSiglas, ancho, Color.Green);
            btnEnviarRecord.SetTexto(txtEnviar, fuenteBotones, Color.Black, true);
        }

        //------------------------GESTION TECLADO JUEGO------------------------
        public void gestionaTeclado()
        {
            //TECLADO
            teclado = Keyboard.GetState();

            if (modo == "jugando")
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
                    ParaMusica();
                    if (mejoraPuntuacion())
                    {
                        modo = "introduceSiglas";
                    }
                    else
                    {
                        modo = "perdi";
                    }
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
        public bool mejoraPuntuacion()
        {
            datos = DataAccessLibrary.DataAcess.PeorPuntuacion();
            if (puntuacionGlobal > Convert.ToInt32(datos[1]))
            {
                return true;
            }
            return false;
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
                        //if (marcianos[i, j].limiteAbajo(AltoPantalla - miNave.getAlto()))
                        if (marcianos[i, j].getContenedor().Intersects(miNave.getContenedor()))
                        {

                            modo = "perdi";
                            miNave.setImagen(explosion);
                            ParaMusica();
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
                case "introduceSiglas":
                    DibujaIntroduceSiglas();
                    break;
                case "perdi":
                    DibujaRepetir();
                    break;
            }
            //dibujo los botones
            btnPausa.Dibuja();
            btnMusica.Dibuja();

            spriteBatch.End();
        }
        public void DibujaRepetir()
        {

            //dibujo el rectangulo
            spriteBatch.Draw(rectanguloPausa, puntoPausa, Color.White);

            //dibujo la pregunta
            spriteBatch.DrawString(fuenteSub, preguntaRepetir, new Vector2(AnchoPantalla/2- fuenteSub.MeasureString(preguntaRepetir).X/2, puntoPausa.Y), Color.Black);

            //botones
            btnRepetirSi.Dibuja();
            btnRepetirNo.Dibuja();
        }
        public void DibujaPausa()
        {
            //dibujo el rectangulo
            spriteBatch.Draw(rectanguloPausa, puntoPausa, Color.White);

            //dibujo la pregunta
            spriteBatch.DrawString(fuenteSub, preguntaPausa, new Vector2(puntoPausa.X + espacioMenuPausa, puntoPausa.Y), Color.Black);

            //dibujo los botones
            btnReanudar.Dibuja();
            btnSalir.Dibuja();

        }
        public void DibujaIntroduceSiglas()
        {
            //dibujo el rectangulo
            spriteBatch.Draw(rectanguloSiglas, puntoSiglas, Color.White);
            //dibujo el texto
            spriteBatch.DrawString(fuenteSub, txtIntroduceSiglas, new Vector2(puntoSiglas.X + espacioMenuSiglas, puntoSiglas.Y), Color.Black);
            //botones sigla arriba
            btnSiglaArriba.Dibuja();
            btnSiglaArriba2.Dibuja();
            btnSiglaArriba3.Dibuja();
            //siglas
            spriteBatch.DrawString(fuenteBotones, siglas[0].ToString(),
                new Vector2(puntoSiglas.X + espacioMenuSiglas + (AnchoPantalla / 25) / 2 - fuenteBotones.MeasureString(siglas[0].ToString()).X / 2,
                puntoSiglas.Y + espacioMenuSiglas + fuenteSub.MeasureString(txtIntroduceSiglas).Y + AnchoPantalla / 25),
                Color.Black);

            spriteBatch.DrawString(fuenteBotones, siglas[1].ToString(),
                new Vector2(AnchoPantalla / 2 - fuenteBotones.MeasureString(siglas[1].ToString()).X / 2,
                puntoSiglas.Y + espacioMenuSiglas + fuenteSub.MeasureString(txtIntroduceSiglas).Y + AnchoPantalla / 25),
                Color.Black);

            spriteBatch.DrawString(fuenteBotones, siglas[2].ToString(),
               new Vector2((int)puntoSiglas.X + anchoMenuSiglas - espacioMenuSiglas - fuenteBotones.MeasureString(siglas[2].ToString()).X / 2 - AnchoPantalla / 25 / 2,
               puntoSiglas.Y + espacioMenuSiglas + fuenteSub.MeasureString(txtIntroduceSiglas).Y + AnchoPantalla / 25),
               Color.Black);

            //botones abajo
            btnSiglaAbajo.Dibuja();
            btnSiglaAbajo2.Dibuja();
            btnSiglaAbajo3.Dibuja();

            //boton enviar record
            btnEnviarRecord.Dibuja();
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

        //PARA LAS SIGLAS
        public char RetrocedeSigla(char letra)
        {
            int pos = abecedario.IndexOf(letra);
            if (pos == 0)
            {
                pos = abecedario.Count - 1;
            }
            else
            {
                pos--;
            }
            letra = (char)abecedario[pos];
            return letra;
        }
        public char AvanzaSigla(char letra)
        {
            int pos = abecedario.IndexOf(letra);
            if (pos == abecedario.Count - 1)
            {
                pos = 0;
            }
            else
            {
                pos++;
            }
            letra = (char)abecedario[pos];
            return letra;
        }
        //MÉTODO ENCARGADO DE GESTIONAR LA LÓGICA DEL JUEGO
        public void GestionaJuego()
        {
            switch (modo)
            {
                case "jugando":
                    auxiliar++;
                    //BALAS MARCIANOS
                    if (auxiliar == 200)
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
                    GestionaBotonesJuego();
                    break;
                case "pausa":
                    GestionaBotonesJuego();
                    break;
                case "introduceSiglas":
                    GestionaBotonesJuego();
                    break;
                case "perdi":
                    GestionaBotonesJuego();
                    break;
            }
            //TECLADO
            gestionaTeclado();
        }
        //------------------------PULSA PAUSA------------------------
        public void GestionaBotonesJuego()
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

                        if (ClickIzq(btnMusica) && modo == "jugando")
                            btnMusica.SetBandera(true);
                        if (modo == "pausa")
                        {
                            if (ClickIzq(btnReanudar))
                                btnReanudar.SetBandera(true);

                            if (ClickIzq(btnSalir))
                                btnSalir.SetBandera(true);
                        }
                        if (modo == "perdi")
                        {
                            if (ClickIzq(btnRepetirNo))
                            btnRepetirNo.SetBandera(true);

                            if (ClickIzq(btnRepetirSi))
                                btnRepetirSi.SetBandera(true);
                        }
                        if (modo == "introduceSiglas")
                        {
                            if (ClickIzq(btnEnviarRecord))
                                btnEnviarRecord.SetBandera(true);

                            if (ClickIzq(btnSiglaArriba))
                                btnSiglaArriba.SetBandera(true);

                            if (ClickIzq(btnSiglaArriba2))
                                btnSiglaArriba2.SetBandera(true);

                            if (ClickIzq(btnSiglaArriba3))
                                btnSiglaArriba3.SetBandera(true);

                            if (ClickIzq(btnSiglaAbajo))
                                btnSiglaAbajo.SetBandera(true);

                            if (ClickIzq(btnSiglaAbajo2))
                                btnSiglaAbajo2.SetBandera(true);

                            if (ClickIzq(btnSiglaAbajo3))
                                btnSiglaAbajo3.SetBandera(true);
                        }

                        break;
                    //si el boton izquierdo no está pulsado, se ha levantado, hago lo que obedezca a dicho boton
                    case ButtonState.Released:
                        if (LevantoIzq(btnPausa))
                        {
                            switch (modo)
                            {
                                case "jugando":
                                    modo = "pausa";
                                    ParaMusica();
                                    btnPausa.SetImagen(imgBtnPlay);
                                    break;
                                case "pausa":
                                    btnPausa.SetImagen(imgBtnPausa);
                                    SuenaCancion(cancionJuego);
                                    modo = "jugando";
                                    break;
                            }
                        }
                        if (LevantoIzq(btnReanudar))
                        {
                            btnPausa.SetImagen(imgBtnPausa);
                            SuenaCancion(cancionJuego);
                            modo = "jugando";
                        }

                        if (LevantoIzq(btnSalir))
                        {
                            estadoActualJuego = EstadoJuego.Menu;
                            if (boolMusica)
                                SuenaCancion(cancionMenu);
                        }

                        if (LevantoIzq(btnRepetirNo))
                        {
                            estadoActualJuego = EstadoJuego.Menu;
                            if (boolMusica)
                                SuenaCancion(cancionMenu);
                        }
                        if (LevantoIzq(btnRepetirSi))
                        {
                            InicializaVariablesJuego();
                            if (boolMusica)
                                SuenaCancion(cancionJuego);
                        }
                        if (LevantoIzq(btnMusica))
                        {
                            if (boolMusica)
                            {
                                btnMusica.SetImagen(imgMusicaOn);
                                ParaMusica();
                            }
                            else
                            {
                                btnMusica.SetImagen(imgMusicaOff);
                                SuenaCancion(cancionJuego);
                            }
                            boolMusica = !boolMusica;
                            if (boolMusica)
                            {
                                //actualizo a un uno
                                DataAccessLibrary.DataAcess.EjecutaQuery("UPDATE OPCIONES SET Musica= ('1')");
                            }
                            else
                            {
                                //actualizo un 0
                                DataAccessLibrary.DataAcess.EjecutaQuery("UPDATE OPCIONES SET Musica = ('0')");
                            }
                        }
                        if (LevantoIzq(btnEnviarRecord))
                        {
                            //guardo el record
                            GuardarRecord();
                            //para preguntar si desea volver a jugar o no
                            modo = "perdi";
                        }
                        if (LevantoIzq(btnSiglaArriba))
                            siglas[0] = RetrocedeSigla(siglas[0]);

                        if (LevantoIzq(btnSiglaArriba2))
                            siglas[1] = RetrocedeSigla(siglas[1]);

                        if (LevantoIzq(btnSiglaArriba3))
                            siglas[2] = RetrocedeSigla(siglas[2]);

                        if (LevantoIzq(btnSiglaAbajo))
                            siglas[0] = AvanzaSigla(siglas[0]);

                        if (LevantoIzq(btnSiglaAbajo2))
                            siglas[1] = AvanzaSigla(siglas[1]);

                        if (LevantoIzq(btnSiglaAbajo3))
                            siglas[2] = AvanzaSigla(siglas[2]);

                        //pongo a false las banderas de todos los botones del menu opciones
                        btnPausa.SetBandera(false);
                        btnReanudar.SetBandera(false);
                        btnSalir.SetBandera(false);
                        btnMusica.SetBandera(false);
                        btnEnviarRecord.SetBandera(false);
                        btnSiglaAbajo.SetBandera(false);
                        btnSiglaAbajo2.SetBandera(false);
                        btnSiglaAbajo3.SetBandera(false);
                        btnSiglaArriba.SetBandera(false);
                        btnSiglaArriba2.SetBandera(false);
                        btnSiglaArriba3.SetBandera(false);
                        btnRepetirSi.SetBandera(false);
                        btnRepetirNo.SetBandera(false);
                        break;
                }
            }
        }
        public void GuardarRecord()
        {
            DataAccessLibrary.DataAcess.EjecutaQuery("DELETE FROM puntuaciones WHERE ID=" + datos[0]);
            int m =DataAccessLibrary.DataAcess.MaximoId();
            m+=1;
            DataAccessLibrary.DataAcess.EjecutaQuery("INSERT INTO PUNTUACIONES(id,siglas,puntuacion) VALUES ("+m+",'"+siglas[0] + siglas[1] + siglas[2]+"',"+puntuacionGlobal+")");
            CargarRecords();
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
                        {
                            estadoActualJuego = EstadoJuego.Menu;
                            if (boolMusica)
                                SuenaCancion(cancionMenu);
                        }

                        //pongo a false las banderas de todos los botones del menu opciones
                        btnSalir.SetBandera(false);
                        break;
                }
            }
        }
        public void gestionaMenuPausa()
        {
            GestionaBotonesJuego();

            PulsaReanudar();
            PulsaSalir();
        }

        //*****************************************************************OPCIONES*****************************************************************
        public void CargarOpciones()
        {
            CargarTextosOpciones();
            CrearBotonesOpciones();
        }
        public void CargarTextosOpciones()
        {
            txtSeleccionaNave = "Selecciona tu nave";
            txtMusica = "Musica";
            txtSi = "Si";
            txtNo = "No";
        }
        public void CrearBotonesOpciones()
        {
            int tamaño = AnchoPantalla / 20;
            //botones nave
            btnNave1 = new Boton(graphics, spriteBatch,
            (int)AnchoPantalla / 2 - (int)fuenteSub.MeasureString(txtSeleccionaNave).X / 2,
            (int)AltoPantalla / 4 + (int)fuenteSub.MeasureString(txtSeleccionaNave).Y,
            tamaño, tamaño, Color.Red);
            btnNave1.SetImagen(imgNave1);

            btnNave2 = new Boton(graphics, spriteBatch,
                (int)AnchoPantalla / 2 - tamaño / 2,
                 (int)AltoPantalla / 4 + (int)fuenteSub.MeasureString(txtSeleccionaNave).Y,
                 tamaño, tamaño, Color.Red);
            btnNave2.SetImagen(imgNave2);

            btnNave3 = new Boton(graphics, spriteBatch,
                (int)AnchoPantalla / 2 + (int)fuenteSub.MeasureString(txtSeleccionaNave).X / 2 - tamaño,
                 (int)AltoPantalla / 4 + (int)fuenteSub.MeasureString(txtSeleccionaNave).Y,
                 tamaño, tamaño, Color.Red);
            btnNave3.SetImagen(imgNave3);

            switch (numNave)
            {
                case 1:
                    btnNave1.SetColor(Color.Green);
                    break;
                case 2:
                    btnNave2.SetColor(Color.Green);
                    break;
                case 3:
                    btnNave3.SetColor(Color.Green);
                    break;
            }
            //botones musica
            btnMusicaSi = new Boton(graphics, spriteBatch,
                (int)AnchoPantalla / 2 - (int)fuenteSub.MeasureString(txtMusica).X / 2,
                (int)AltoPantalla / 2 + (int)fuenteSub.MeasureString(txtMusica).Y,
                (int)fuenteSub.MeasureString(txtMusica).X / 2,
                tamaño,
                Color.Red);
            btnMusicaSi.SetTexto(txtSi, fuenteBotones, Color.Black, true);

            btnMusicaNo = new Boton(graphics, spriteBatch,
               (int)AnchoPantalla / 2,
               (int)AltoPantalla / 2 + (int)fuenteSub.MeasureString(txtMusica).Y,
               (int)fuenteSub.MeasureString(txtMusica).X / 2,
               tamaño,
               Color.Red);
            btnMusicaNo.SetTexto(txtNo, fuenteBotones, Color.Black, true);

            if (boolMusica)
            {
                btnMusicaSi.SetColor(Color.Green);
            }
            else
            {
                btnMusicaNo.SetColor(Color.Green);
            }
        }
        public void DibujaOpciones()
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(fondoSubmenu, new Rectangle(0, 0, AnchoPantalla, AltoPantalla), Color.White);
            spriteBatch.DrawString(fuenteTitulo, txtOpciones, new Vector2(AnchoPantalla / 2 - fuenteTitulo.MeasureString(txtOpciones).X / 2, AltoPantalla / 20), Color.White);
            //boton volver
            btnVolverMenu.Dibuja();
            //pregunta nave
            spriteBatch.DrawString(fuenteSub, txtSeleccionaNave, new Vector2(AnchoPantalla / 2 - fuenteSub.MeasureString(txtSeleccionaNave).X / 2, AltoPantalla / 4), Color.White);
            //naves
            btnNave1.Dibuja();
            btnNave2.Dibuja();
            btnNave3.Dibuja();
            //pregunta musica
            spriteBatch.DrawString(fuenteSub, txtMusica, new Vector2(AnchoPantalla / 2 - fuenteSub.MeasureString(txtMusica).X / 2, AltoPantalla / 2), Color.White);
            //botones musica
            btnMusicaSi.Dibuja();
            btnMusicaNo.Dibuja();
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
                        //pongo la bandera a true del boton que pulse
                        if (ClickIzq(btnVolverMenu))
                            btnVolverMenu.SetBandera(true);

                        if (ClickIzq(btnNave1))
                            btnNave1.SetBandera(true);

                        if (ClickIzq(btnNave2))
                            btnNave2.SetBandera(true);

                        if (ClickIzq(btnNave3))
                            btnNave3.SetBandera(true);

                        if (ClickIzq(btnMusicaSi))
                            btnMusicaSi.SetBandera(true);

                        if (ClickIzq(btnMusicaNo))
                            btnMusicaNo.SetBandera(true);
                        break;
                    //si el boton izquierdo no está pulsado, se ha levantado, hago lo que obedezca a dicho boton
                    case ButtonState.Released:
                        if (LevantoIzq(btnVolverMenu))
                        {
                            estadoActualJuego = EstadoJuego.Menu;
                            if (boolMusica)
                                SuenaCancion(cancionMenu);
                        }

                        if (LevantoIzq(btnNave1) || LevantoIzq(btnNave2) || LevantoIzq(btnNave3))
                            ElegirNave();

                        if (LevantoIzq(btnMusicaSi) || LevantoIzq(btnMusicaNo))
                            ElegirMusica();

                        //pongo a false las banderas de todos los botones del menu opciones
                        btnVolverMenu.SetBandera(false);
                        btnNave1.SetBandera(false);
                        btnNave2.SetBandera(false);
                        btnNave3.SetBandera(false);
                        btnMusicaSi.SetBandera(false);
                        btnMusicaNo.SetBandera(false);
                        break;
                }
            }
        }
        public void ElegirNave()
        {
            if (ClickIzq(btnNave1))
            {
                numNave = 1;
                btnNave1.SetColor(Color.Green);
                btnNave2.SetColor(Color.Red);
                btnNave3.SetColor(Color.Red);
            }
            if (ClickIzq(btnNave2))
            {
                numNave = 2;
                btnNave1.SetColor(Color.Red);
                btnNave2.SetColor(Color.Green);
                btnNave3.SetColor(Color.Red);
            }
            if (ClickIzq(btnNave3))
            {
                numNave = 3;
                btnNave1.SetColor(Color.Red);
                btnNave2.SetColor(Color.Red);
                btnNave3.SetColor(Color.Green);
            }
            DataAccessLibrary.DataAcess.EjecutaQuery("UPDATE OPCIONES SET Nave = ('" + numNave + "')");

        }
        public void ElegirMusica()
        {
            if (ClickIzq(btnMusicaSi))
            {
                SuenaCancion(cancionSubmenu);
                boolMusica = true;
                btnMusicaSi.SetColor(Color.Green);
                btnMusicaNo.SetColor(Color.Red);
                DataAccessLibrary.DataAcess.EjecutaQuery("UPDATE opciones SET Musica = ('1')");
            }
            if (ClickIzq(btnMusicaNo))
            {
                ParaMusica();
                boolMusica = false;
                btnMusicaNo.SetColor(Color.Green);
                btnMusicaSi.SetColor(Color.Red);
                DataAccessLibrary.DataAcess.EjecutaQuery("UPDATE opciones SET Musica = ('0')");
            }
        }
        //AYUDA
        public void CargarAyuda()
        {
            CargarTextosAyuda();
            CrearBotonesAyuda();
        }
        //MÉTODO ENCARGADO DE DIBUJAR EL MENÚ AYUDA
        public int DameMasAncho(string[] lista, SpriteFont fuente)
        {
            float auxiliar = 0;
            foreach (string a in lista)
            {
                if (fuente.MeasureString(a).X > auxiliar)
                    auxiliar = fuente.MeasureString(a).X;
            }
            return (int)auxiliar;
        }
        public void CrearBotonesAyuda()
        {
            string[] textoBotonesAyuda = new string[4];
            textoBotonesAyuda[0] = tFin;
            textoBotonesAyuda[1] = tNiveles;
            textoBotonesAyuda[2] = tNave;
            textoBotonesAyuda[3] = tMarcianos;

            int ancho = DameMasAncho(textoBotonesAyuda, fuenteBotones);
            int masLateral = AnchoPantalla / 50;
            ancho += masLateral;

            int espacio = AltoPantalla / 100;
            int alto = 60;
            btnFinalidad = new Boton(graphics, spriteBatch, AnchoPantalla / 2 - ancho / 2, AltoPantalla / 4, ancho, alto, Color.White);
            btnFinalidad.SetTexto(tFin, fuenteBotones, Color.Black, true);

            btnNiveles = new Boton(graphics, spriteBatch, AnchoPantalla / 2 - ancho / 2, AltoPantalla / 4 + espacio + alto, ancho, alto, Color.White);
            btnNiveles.SetTexto(tNiveles, fuenteBotones, Color.Black, true);

            btnNave = new Boton(graphics, spriteBatch, AnchoPantalla / 2 - ancho / 2, AltoPantalla / 4 + espacio * 2 + alto * 2, ancho, alto, Color.White);
            btnNave.SetTexto(tNave, fuenteBotones, Color.Black, true);

            btnMarcianos = new Boton(graphics, spriteBatch, AnchoPantalla / 2 - ancho / 2, AltoPantalla / 4 + espacio * 3 + alto * 3, ancho, alto, Color.White);
            btnMarcianos.SetTexto(tMarcianos, fuenteBotones, Color.Black, true);
        }
        public void CargarTextosAyuda()
        {
            modoAyuda = "principal";
            txtFinalidad = "La finalidad de este juego es sobrevivir el mayor tiempo posible y eliminar todos los marcianos que podamos antes de que estos nos invadan o nos eliminen.";
            txtNave = "Nuestra nave espacial disparara a traves de la barra espaciadora, y de manera que solo habra un unico proyectil de nuestra nave en la pantalla. Podremos mover dicha nave a traves de las flechas (izquierda y/o derecha).";
            txtNiveles = "Este juego contara con niveles infinitos, pero que iran aumentando en cuanto a dificultad. A medida que vayamos completando niveles, se cambiara una fila de marcianos de un impacto por marcianos de dos impactos. Una vez pasemos el nivel en el que solamente hay marcianos de dos impactos, volveremos al comienzo de los niveles, pero los marcianos se moveran mas rapido.";
            txtMarcianos = "Contamos con dos tipos de marcianos: marcianos que son eliminados tras recibir un unico impacto, y otros marcianos que son eliminados tras recibir dos impactos. Estos ultimos, en el momento que reciben el primer impacto se convierten en un marciano de un impacto.";
            tFin = "Finalidad";
            tNiveles = "Niveles";
            tNave = "Nave";
            tMarcianos = "Marcianos";
            impacto1 = "Un impacto";
            impacto2 = "Dos impactos";
            txtP = "10 puntos";
            txtP2 = "25 puntos";

            infoFinalidad = txtFinalidad.Split(' ');
            infoNave = txtNave.Split(' ');
            infoNiveles = txtNiveles.Split(' ');
            infoMarcianos = txtMarcianos.Split(' ');
        }

        public void dibujaTexto(String[] texto, SpriteFont fuente)
        {
            int espacio = 20;
            int posX = espacio;
            int primeraY = (int)fuenteTitulo.MeasureString(txtAyuda).Y + AltoPantalla / 20 + espacio;
            foreach (string s in texto)
            {
                posX += (int)fuente.MeasureString(s).X;
                if (posX >= AnchoPantalla)
                {
                    posX = espacio;
                    primeraY += (int)fuente.MeasureString(s).Y;
                    //c.drawText(s, posX, primeraY, pTexto);
                    spriteBatch.DrawString(fuente, s, new Vector2(posX, primeraY), Color.White);
                }
                else
                {
                    posX = posX - (int)fuente.MeasureString(s).X;
                    spriteBatch.DrawString(fuente, s, new Vector2(posX, primeraY), Color.White);
                }

                posX += (int)fuente.MeasureString(s).X + espacio;
            }
        }
        public void DibujaFinalidad()
        {
            dibujaTexto(infoFinalidad, fuenteSub);
        }
        public void DibujaNiveles()
        {
            dibujaTexto(infoNiveles, fuenteSub);
        }
        public void DibujaNave()
        {
            dibujaTexto(infoNave, fuenteSub);
        }
        public void DibujaMarcianos()
        {
            dibujaTexto(infoMarcianos, fuenteSub);
        }
        public void MarcianosPuntuaciones()
        {
            int tamaño = AnchoPantalla / 20;
            int espacio = AnchoPantalla / 20;
            //dibujo puntuacion 10
            Vector2 p = new Vector2(espacio, AltoPantalla - fuenteBotones.MeasureString(txtP).Y - espacio / 2);
            spriteBatch.DrawString(fuenteBotones, txtP, p, Color.White);
            Rectangle r = new Rectangle(espacio + (int)fuenteBotones.MeasureString(txtP).X / 2 - tamaño / 2,
              (int)p.Y - tamaño,
              tamaño, tamaño);
            spriteBatch.Draw(imgMarciano1, r, Color.White);

            //dibujo puntuacion 25
            spriteBatch.DrawString(fuenteBotones, txtP2, new Vector2(AnchoPantalla - espacio - (int)fuenteBotones.MeasureString(txtP2).X, p.Y), Color.White);
            Rectangle r2 = new Rectangle(AnchoPantalla - espacio - (int)fuenteBotones.MeasureString(txtP2).X + tamaño / 2,
                (int)p.Y - tamaño,
                tamaño, tamaño);
            spriteBatch.Draw(imgMarciano2, r2, Color.White);


        }
        public void DibujaAyuda()
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(fondoSubmenu, new Rectangle(0, 0, AnchoPantalla, AltoPantalla), Color.White);
            spriteBatch.DrawString(fuenteTitulo, txtAyuda, new Vector2(AnchoPantalla / 2 - fuenteTitulo.MeasureString(txtAyuda).X / 2, AltoPantalla / 20), Color.White);
            btnVolverMenu.Dibuja();

            switch (modoAyuda)
            {
                case "principal":
                    //botones para acceder a informacion
                    btnFinalidad.Dibuja();
                    btnNiveles.Dibuja();
                    btnNave.Dibuja();
                    btnMarcianos.Dibuja();
                    break;
                case "finalidad":
                    DibujaFinalidad();
                    break;
                case "niveles":
                    DibujaNiveles();
                    break;
                case "nave":
                    DibujaNave();
                    break;
                case "marcianos":
                    DibujaMarcianos();
                    MarcianosPuntuaciones();
                    break;
            }
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
                        //si estoy en prinicpal y pulso alguno de los botones de informacion
                        if (modoAyuda == "principal")
                        {
                            if (ClickIzq(btnFinalidad))
                                btnFinalidad.SetBandera(true);

                            if (ClickIzq(btnNiveles))
                                btnNiveles.SetBandera(true);

                            if (ClickIzq(btnNave))
                                btnNave.SetBandera(true);

                            if (ClickIzq(btnMarcianos))
                                btnMarcianos.SetBandera(true);
                        }

                        //veo si he pulsado en el boton volver, de ser así, pongo su bandera a true
                        if (ClickIzq(btnVolverMenu))
                            btnVolverMenu.SetBandera(true);
                        break;
                    //si el boton izquierdo no está pulsado, se ha levantado, hago lo que obedezca a dicho boton
                    case ButtonState.Released:
                        //si pulso el boton volver
                        if (LevantoIzq(btnVolverMenu))
                        {
                            //estoy en principal, vuelvo al menu
                            if (modoAyuda == "principal")
                            {
                                estadoActualJuego = EstadoJuego.Menu;
                                if (boolMusica)
                                    SuenaCancion(cancionMenu);
                            }
                            else
                            {
                                //si no estoy en principal, voy
                                modoAyuda = "principal";
                            }
                        }

                        //si estoy en prinicpal y pulso alguno de los botones de informacion
                        if (modoAyuda == "principal")
                        {
                            if (LevantoIzq(btnFinalidad))
                                modoAyuda = "finalidad";

                            if (LevantoIzq(btnNiveles))
                                modoAyuda = "niveles";

                            if (LevantoIzq(btnNave))
                                modoAyuda = "nave";

                            if (LevantoIzq(btnMarcianos))
                                modoAyuda = "marcianos";
                        }
                        //pongo a false las banderas de todos los botones del menu opciones
                        btnVolverMenu.SetBandera(false);
                        btnFinalidad.SetBandera(false);
                        btnNiveles.SetBandera(false);
                        btnNave.SetBandera(false);
                        btnMarcianos.SetBandera(false);
                        break;
                }
            }
        }

        //*****************************************************************RECORDS*****************************************************************

        public void CargarRecords()
        {
            listaPuntuaciones = DataAccessLibrary.DataAcess.DamePuntuaciones();
            listaSiglas = DataAccessLibrary.DataAcess.DameSiglas();
        }
        //MÉTODO ENCARGADO DE DIBUJAR EL MENÚ RÉCORDS
        public void DibujaRecords()
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(fondoSubmenu, new Rectangle(0, 0, AnchoPantalla, AltoPantalla), Color.White);
            spriteBatch.DrawString(fuenteTitulo, txtRecords, new Vector2(AnchoPantalla / 2 - (int)fuenteTitulo.MeasureString(txtRecords).X / 2, AltoPantalla / 20), Color.White);
            btnVolverMenu.Dibuja();
            //dibujo los records
            int posY = AltoPantalla / 4;
            int ancho = (int)fuenteBotones.MeasureString("A").Y;
            int espacio = (int)fuenteBotones.MeasureString("A").Y + 10;
            for (int i = 0; i < listaPuntuaciones.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        spriteBatch.Draw(this.imgMedallaOro,
                            new Rectangle((int)AnchoPantalla / 2 - (int)fuenteBotones.MeasureString("10.- " + listaSiglas[i] + " " + listaPuntuaciones[i]).X / 2,
                            posY,
                            ancho,
                            ancho),
                            Color.White);
                        spriteBatch.DrawString(fuenteBotones,
                            " " + listaSiglas[i] + " " + listaPuntuaciones[i],
                            new Vector2((int)AnchoPantalla / 2 - (int)fuenteBotones.MeasureString(" " + listaSiglas[i] + " " + listaPuntuaciones[i]).X / 2,
                            posY),
                            Color.White);

                        break;
                    case 1:
                        spriteBatch.Draw(this.imgMedallaPlata,
                        new Rectangle((int)AnchoPantalla / 2 - (int)fuenteBotones.MeasureString("10.- " + listaSiglas[i] + " " + listaPuntuaciones[i]).X / 2,
                        posY,
                        ancho,
                        ancho),
                        Color.White);
                        spriteBatch.DrawString(fuenteBotones,
                            " " + listaSiglas[i] + " " + listaPuntuaciones[i],
                            new Vector2((int)AnchoPantalla / 2 - (int)fuenteBotones.MeasureString(" " + listaSiglas[i] + " " + listaPuntuaciones[i]).X / 2,
                            posY),
                            Color.White);
                        break;
                    case 2:
                        spriteBatch.Draw(this.imgMedallaBronce,
                       new Rectangle((int)AnchoPantalla / 2 - (int)fuenteBotones.MeasureString("10.- " + listaSiglas[i] + " " + listaPuntuaciones[i]).X / 2,
                       posY,
                       ancho,
                       ancho),
                       Color.White);
                        spriteBatch.DrawString(fuenteBotones,
                            " " + listaSiglas[i] + " " + listaPuntuaciones[i],
                            new Vector2((int)AnchoPantalla / 2 - (int)fuenteBotones.MeasureString(" " + listaSiglas[i] + " " + listaPuntuaciones[i]).X / 2,
                            posY),
                            Color.White);
                        break;
                    default:
                        spriteBatch.DrawString(fuenteBotones,
                             i + 1 + ".- " + listaSiglas[i] + " " + listaPuntuaciones[i],
                            new Vector2((int)AnchoPantalla / 2 - (int)fuenteBotones.MeasureString("10.- " + listaSiglas[i] + " " + listaPuntuaciones[i]).X / 2,
                            posY),
                            Color.White);
                        break;
                }
                //espacio entre records
                posY += espacio;
            }
            //vuelvo a la posY inicial
            posY = AltoPantalla / 4;
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
                        {
                            estadoActualJuego = EstadoJuego.Menu;
                            if (boolMusica)
                                SuenaCancion(cancionMenu);
                        }

                        //pongo a false las banderas de todos los botones del menu opciones
                        btnVolverMenu.SetBandera(false);
                        break;
                }
            }


        }

        //*****************************************************************CRÉDITOS*****************************************************************
        public void CargarCreditos()
        {
            modoCreditos = 0;
            moverCreditos = true;
            posCreditos = AltoPantalla / 20+(int)fuenteTitulo.MeasureString(txtCreditos).Y;
            CargarTextosCreditos();
        }
        public void CargarTextosCreditos()
        {
            //txtFuente = "Fuente";
            txtImagenes = "Imagenes";
            txtImg = "https://game-icons.net";
            txtImg2 = "https://pixabay.com";
            txtMusic = "https://patrickdearteaga.com/ ";
            //txtFont = contexto.getString(R.string.font1);
            txtHecho = "Realizado y dirigido por";
            txtYoutube = "https://www.youtube.com/";
            txtCanal = "Canal: Free Music for Commercial Use";
        }
        //MÉTODO ENCARGADO DE DIBUJAR EL MENÚ CRÉDITOS
        public void DibujaCreditos()
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(fondoSubmenu, new Rectangle(0, 0, AnchoPantalla, AltoPantalla), Color.White);
            spriteBatch.DrawString(fuenteTitulo, txtCreditos, new Vector2(AnchoPantalla / 2 - fuenteTitulo.MeasureString(txtCreditos).X / 2, AltoPantalla / 20), Color.White);
            //agradecimientos y reconocimiento
            switch (modoCreditos)
            {
                case 0:
                    DibujaImagenes();
                    break;
                case 1:
                    DibujaMusica();
                    break;
                case 2:
                    DibujaAgradecimientos();
                    break;
            }
            btnVolverMenu.Dibuja();
            spriteBatch.End();
        }
        public void DibujaImagenes()
        {
            spriteBatch.DrawString(fuenteSub, txtImagenes,
           new Vector2(AnchoPantalla / 2 - fuenteSub.MeasureString(txtImagenes).X / 2, posCreditos),
           Color.White);

            spriteBatch.DrawString(fuenteSub, txtImg,
                new Vector2(AnchoPantalla / 2 - fuenteSub.MeasureString(txtImg).X / 2, posCreditos + fuenteSub.MeasureString(txtImagenes).Y),
                Color.White);

            spriteBatch.DrawString(fuenteSub, txtImg2,
               new Vector2(AnchoPantalla / 2 - fuenteSub.MeasureString(txtImg2).X / 2, posCreditos + fuenteSub.MeasureString(txtImagenes).Y + fuenteSub.MeasureString(txtImg).Y),
               Color.White);
        }
        public void DibujaMusica()
        {
            spriteBatch.DrawString(fuenteSub, txtMusica,
                          new Vector2(AnchoPantalla / 2 - fuenteSub.MeasureString(txtMusica).X / 2, posCreditos),
                          Color.White);

            spriteBatch.DrawString(fuenteSub, txtMusic,
               new Vector2(AnchoPantalla / 2 - fuenteSub.MeasureString(txtMusic).X / 2, posCreditos + fuenteSub.MeasureString(txtMusica).Y ),
               Color.White);

            spriteBatch.DrawString(fuenteSub, txtYoutube,
               new Vector2(AnchoPantalla / 2 - fuenteSub.MeasureString(txtYoutube).X / 2, posCreditos +  fuenteSub.MeasureString(txtMusic).Y + fuenteSub.MeasureString(txtMusica).Y),
               Color.White);

            spriteBatch.DrawString(fuenteSub, txtCanal,
         new Vector2(AnchoPantalla / 2 - fuenteSub.MeasureString(txtCanal).X / 2, posCreditos +  fuenteSub.MeasureString(txtMusic).Y + fuenteSub.MeasureString(txtMusica).Y + fuenteSub.MeasureString(txtYoutube).Y),
         Color.White);
        }
        public void DibujaAgradecimientos()
        {
            spriteBatch.DrawString(fuenteSub, txtHecho,
                        new Vector2(AnchoPantalla / 2 - fuenteSub.MeasureString(txtHecho).X / 2, posCreditos),
                        Color.White);

            spriteBatch.DrawString(fuenteSub, "Lucas Alonso",
                        new Vector2(AnchoPantalla / 2 - fuenteSub.MeasureString("Lucas Alonso").X / 2, posCreditos+fuenteSub.MeasureString(txtHecho).Y),
                        Color.White);
        }
        //MÉTODO ENCARGADO DE GESTIONAR LA LÓGICA DEL MENÚ CRÉDITOS
        public void GestionaCreditos()
        {
            if(moverCreditos)
            posCreditos += 1;
            if (posCreditos >= AltoPantalla)
            {

                posCreditos = AltoPantalla / 20 + (int)fuenteTitulo.MeasureString(txtCreditos).Y;
                modoCreditos++;
                if (modoCreditos == 3)
                    modoCreditos = 0;
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
                        moverCreditos = false;
                        //veo si he pulsado en el boton volver, de ser así, pongo su bandera a true
                        if (ClickIzq(btnVolverMenu))
                            btnVolverMenu.SetBandera(true);

                        break;
                    //si el boton izquierdo no está pulsado, se ha levantado, hago lo que obedezca a dicho boton
                    case ButtonState.Released:
                        moverCreditos = true;
                        if (LevantoIzq(btnVolverMenu))
                        {
                            estadoActualJuego = EstadoJuego.Menu;
                            if (boolMusica)
                                SuenaCancion(cancionMenu);
                        }

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

        //PARA LA MUSICA
        public void SuenaCancion(Song futura)
        {
            MediaPlayer.Play(futura);
        }
        public void ParaMusica()
        {
            MediaPlayer.Pause();
        }
    }
}
