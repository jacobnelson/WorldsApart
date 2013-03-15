using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace WorldsApart.Code.Gamestates
{
    class GameState
    {
        public GameStateManager gameStateManager;
        protected ContentManager contentManager;
        protected GraphicsDevice GraphicsDevice;

        public bool paused = false;
        public bool visible = true;
        public bool stopInput = false;


        public GameState(GameStateManager gsm)
        {
            gameStateManager = gsm;
            contentManager = gsm.NewContentManager();



        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public Texture2D LoadTexture(string stringPath)
        {
            return contentManager.Load<Texture2D>(stringPath);
        }

        public SpriteFont LoadFont(string stringPath)
        {
            return contentManager.Load<SpriteFont>(stringPath);
        }

        public SoundEffect LoadSoundEffect(string stringPath)
        {
            return contentManager.Load<SoundEffect>(stringPath);
        }

        public Effect LoadEffect(string stringPath)
        {
            return contentManager.Load<Effect>(stringPath);
        }
    }
}
