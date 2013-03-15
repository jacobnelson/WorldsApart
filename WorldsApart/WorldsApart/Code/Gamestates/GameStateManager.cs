using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using WorldsApart.Code.Graphics;

namespace WorldsApart.Code.Gamestates
{
    class GameStateManager
    {
        public Game1 game;

        GSTitle gsTitle;
        GSPlay gsPlay;
        GSWin gsWin;

        public int currentLevel = 0;
        public int goodness = 5;


        public GameStateManager(Game1 game)
        {
            this.game = game;
            
            SwitchToGSTitle();
        }

        public void Update(GameTime gameTime)
        {
            if (gsPlay != null) gsPlay.Update(gameTime);
            if (gsTitle != null) gsTitle.Update(gameTime);
            if (gsWin != null) gsWin.Update(gameTime);
        }

        public void SwitchToLevel(int levelID)
        {

        }

        public void SwitchToGSPlay()
        {
            gsPlay = new GSPlay(this, currentLevel);
            gsTitle = null;
            gsWin = null;
        }

        public void SwitchToGSTitle()
        {
            if (gsTitle == null)
            {
                gsTitle = new GSTitle(this);
            }
            gsPlay = null;
            gsWin = null;
        }

        public void SwitchToGSWin()
        {
            if (gsWin == null)
            {
                gsWin = new GSWin(this);
            }
            gsPlay = null;
            gsTitle = null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            if (gsPlay != null) gsPlay.Draw(spriteBatch);
            if (gsTitle != null) gsTitle.Draw(spriteBatch);
            if (gsWin != null) gsWin.Draw(spriteBatch);
        }

        public ContentManager NewContentManager()
        {
            return new ContentManager(game.Services, "Content");
        }
    }
}
