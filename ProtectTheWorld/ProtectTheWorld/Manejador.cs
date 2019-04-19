using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Windows.UI.ViewManagement;

namespace ProtectTheWorld
{
    enum EstadoJuego
    {
        Menu,
        Gameplay,
        Creditos,
        Records,
        Opciones
    }
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Manejador : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private EstadoJuego estadoActualJuego;
        private ButtonState estadoClickIzq;
        private int AnchoPantalla, AltoPantalla;
        private SpriteFont fuente;
        private string titulo;
        private List<Boton> botonesMenu;
        private Texture2D fondoMenu;
        private Boton jugar;
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
            //ANCHO Y ALTO PANTALLA
            AltoPantalla = (int)ApplicationView.GetForCurrentView().VisibleBounds.Height;
            AnchoPantalla = (int)ApplicationView.GetForCurrentView().VisibleBounds.Width;

            //PANTALLA COMPLETA
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;

            //INICIALIZO LA PRIMERA PANTALLA
            estadoActualJuego = EstadoJuego.Menu;

            //HAGO VISIBLE EL RATÓN
            this.IsMouseVisible = true;
            //STRINGS
            titulo = "Protect the World";

            //ESTADO ACTUAL BOTON CLICK IZQ
            estadoClickIzq = ButtonState.Released;

            

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {


            // TODO: use this.Content to load your game content here
            fondoMenu = Content.Load<Texture2D>("fondomenu");
            fuente = Content.Load<SpriteFont>("Fuentes/FuenteTitulo");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);    
            //ARRAYLIST DE BOTONES
            //MENU
            botonesMenu = new List<Boton>();    //aqui porque si lo pongo en el método instance da null reference exception
            jugar = new Boton(this.graphics, this.spriteBatch, 50, 0, 50, 50, Color.Blue);
            botonesMenu.Add(jugar);
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
            //prueba set texto set imagen
            jugar.SetTexto(titulo, fuente, Color.White);
            //jugar.SetImagen(fondoMenu);
            jugar.Dibuja();
            spriteBatch.End();
        }
        //MÉTODO ENCARGADO DE GESTIONAR LA LÓGICA DEL MENÚ PRINCIPAL
        public void GestionaMenu()
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
                        //veo si he pulsado en algun boton, de ser así, pongo su bandera a true
                        foreach (Boton btn in botonesMenu)
                        {
                            if (ClickIzq(btn))
                                btn.SetBandera(true);
                        }

                        break;
                    //si el boton izquierdo no está pulsado, se ha levantado, hago lo que obedezca a dicho boton
                    case ButtonState.Released:
                        if (LevantoIzq(jugar))
                            titulo = "FUNCIONA";
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

        }
        //MÉTODO ENCARGADO DE GESTIONAR LA LÓGICA DEL JUEGO
        public void GestionaJuego()
        {

        }

        //OPCIONES
        //MÉTODO ENCARGADO DE DIBUJAR EL MENÚ OPCIONES
        public void DibujaOpciones()
        {

        }
        //MÉTODO ENCARGADO DE GESTIONAR LA LÓGICA DEL MENÚ OPCIONES
        public void GestionaOpciones()
        {

        }
        //RÉCORDS        
        //MÉTODO ENCARGADO DE DIBUJAR EL MENÚ RÉCORDS

        public void DibujaRecords()
        {

        }
        //MÉTODO ENCARGADO DE GESTIONAR LA LÓGICA DEL MENÚ RÉCORDS
        public void GestionaRecords()
        {

        }
        //CRÉDITOS
        //MÉTODO ENCARGADO DE DIBUJAR EL MENÚ CRÉDITOS
        public void DibujaCreditos()
        {

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
    }
}
