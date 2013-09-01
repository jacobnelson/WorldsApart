using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

using WorldsApart.Code.Graphics;
using WorldsApart.Code.Entities;
using WorldsApart.Code.Controllers;

using System.Diagnostics;

namespace WorldsApart.Code.Gamestates
{
    class GSWin : GameState
    {
        SpriteIMG backdrop;

        SpriteIMG credits;
        SpriteIMG thanks;

        float timeCounter = 0;
        float timeRate = 5;


        public GSWin(GameStateManager gsm)
            : base(gsm)
        {
            backdrop = new SpriteIMG(LoadTexture("whitePixel"), Vector2.Zero);
            backdrop.scale = new Vector2(800, 600);
            backdrop.color = Color.Black;

            credits = new SpriteIMG(LoadTexture("WinAssets/credits"), new Vector2(0, Game1.screenHeight));
            credits.origin = Vector2.Zero;

            thanks = new SpriteIMG(LoadTexture("WinAssets/thanks"), new Vector2(Game1.screenWidth / 2, credits.position.Y + credits.texture.Height + Game1.screenHeight / 2 + Game1.screenHeight));


            credits.am.StartNewAnimation(AnimationType.Linear, credits.position, new Vector2(0, -2000), 1024);
            thanks.am.StartNewAnimation(AnimationType.Linear, thanks.position, Game1.GetScreenCenter(), 1024);
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);

            credits.Update();
            thanks.Update();

            if (!thanks.am.animating)
            {
                timeCounter++;
                if (timeCounter >= timeRate)
                {
                    AudioManager.PlayMusic("Title");
                    gameStateManager.currentLevel = 1;
                    gameStateManager.TransitionToGameState(this, GameStateType.GSTitle, 120);
                }
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Begin();

            backdrop.Draw(spriteBatch);
            credits.Draw(spriteBatch);
            thanks.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
