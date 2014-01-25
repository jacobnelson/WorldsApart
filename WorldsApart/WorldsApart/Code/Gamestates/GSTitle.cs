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
    class GSTitle : GameState
    {
        SpriteIMG titleSplashArt;
        SpriteIMG titleLogo;
        SpriteIMG titlePressStart;
        SpriteIMG sparkles1;
        SpriteIMG sparkles2;
        SpriteIMG titleCopyright;

        bool titleFadedIn = false;

        int timerCounter = 0;
       

        public GSTitle(GameStateManager gsm)
            : base(gsm)
        {


            titleSplashArt = new SpriteIMG(LoadTexture("TitleAssets/titleSplashArt"), Game1.GetScreenCenter());
            titleLogo = new SpriteIMG(LoadTexture("TitleAssets/titleLogo"), Game1.GetScreenCenter());
            titlePressStart = new SpriteIMG(LoadTexture("TitleAssets/titlePressStart"), Game1.GetScreenCenter());
            sparkles1 = new SpriteIMG(LoadTexture("TitleAssets/titleSparkles1"), Game1.GetScreenCenter());
            sparkles2 = new SpriteIMG(LoadTexture("TitleAssets/titleSparkles2"), Game1.GetScreenCenter());
            titleCopyright = new SpriteIMG(LoadTexture("TitleAssets/titleCopyright"), Game1.GetScreenCenter());

            titleLogo.alpha = 0;
            titlePressStart.alpha = 0;
            titleCopyright.alpha = 0;
            sparkles2.alpha = 0;
            
        }

        public void GetInput()
        {
            if (stopInput) return;

            if (InputManager.IsButtonPressed(Buttons.Start) || InputManager.IsKeyPressed(Keys.Enter))
            {
                //gameStateManager.SwitchToGSMenu();
                gameStateManager.TransitionToGameState(this, GameStateType.GSMenu, 30);
                AudioManager.worldShatter.Play();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!titleFadedIn)
            {
                timerCounter++;

                if (timerCounter == 120)
                {
                    titleLogo.am.StartFade(120, 0, 255);
                }

                if (titleLogo.alpha == 255)
                {
                    timerCounter = 0;
                    titleFadedIn = true;
                    titleCopyright.am.StartFade(120, 0, 255);
                }
            }
            else
            {
                AnimationManager.FadeInAndOut(titlePressStart, 120, 0, 255);
            }

            AnimationManager.FadeInAndOut(sparkles1, 120, 0, 255);
            AnimationManager.FadeInAndOut(sparkles2, 120, 0, 255);

            titlePressStart.Update();
            titleLogo.Update();
            titleCopyright.Update();
            sparkles1.Update();
            sparkles2.Update();

            GetInput();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);


            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            titleSplashArt.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            sparkles1.Draw(spriteBatch);
            sparkles2.Draw(spriteBatch);
            sparkles1.Draw(spriteBatch);
            sparkles2.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            titlePressStart.Draw(spriteBatch);
            titleCopyright.Draw(spriteBatch);
            titleLogo.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
