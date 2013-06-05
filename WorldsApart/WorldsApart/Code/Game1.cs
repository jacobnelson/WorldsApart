using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using WorldsApart.Code.Controllers;
using WorldsApart.Code.Gamestates;

namespace WorldsApart.Code
{
    enum PlayerObjectMode
    {
        None,
        One,
        Two
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        GameStateManager gsm;

        static public GraphicsDevice gd;

        static public int screenWidth = 800;
        static public int screenHeight = 600;

        static public Vector2 GetScreenCenter()
        {
            return new Vector2(screenWidth / 2, screenHeight / 2);
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TOD: Add your initialization logic here
            graphics.SynchronizeWithVerticalRetrace = false;

            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gsm = new GameStateManager(this);
            gd = GraphicsDevice;
            // TDO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TOO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            Time.UpdateTime(gameTime);
            InputManager.UpdateStates(GamePad.GetState(PlayerIndex.One), GamePad.GetState(PlayerIndex.Two), Keyboard.GetState(), Mouse.GetState());
            if (InputManager.IsKeyPressed(Keys.F11)) graphics.ToggleFullScreen();
            gsm.Update(gameTime);
            // TOO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            gsm.Draw(spriteBatch);
            // TOO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
