using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldsApart.Code.Graphics;
using WorldsApart.Code.Entities;
using WorldsApart.Code.Controllers;

using System.Diagnostics;

namespace WorldsApart.Code.Gamestates
{
    class GSOverlay
    {
        static public SpriteIMG fadeOverlay;
        static bool isFadingIn = false;
        static int currentDuration = 0;
        static RenderTarget2D overlayTarget;
        static GameStateManager gsm;

        static public SpriteIMG words1;
        static public SpriteIMG words2;
        static public SpriteIMG words3;
        static public SpriteIMG words4;
        static public SpriteIMG words5;
        static public SpriteIMG words6;


        static public void InitializeGSOverlay(GameStateManager gsm1)
        {
            fadeOverlay = new SpriteIMG(Art.whitePixel, Vector2.Zero);
            fadeOverlay.scale = new Vector2(800, 600);
            fadeOverlay.origin = Vector2.Zero;
            fadeOverlay.alpha = 0;
            gsm = gsm1;
            overlayTarget = new RenderTarget2D(gsm.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);

            words1 = new SpriteIMG(Art.words1, Game1.GetScreenCenter() + new Vector2(0, -50));
            words2 = new SpriteIMG(Art.words2, Game1.GetScreenCenter() + new Vector2(0, -50));
            words3 = new SpriteIMG(Art.words3, Game1.GetScreenCenter() + new Vector2(0, -50));
            words4 = new SpriteIMG(Art.words4, Game1.GetScreenCenter() + new Vector2(0, -50));
            words5 = new SpriteIMG(Art.words5, Game1.GetScreenCenter() + new Vector2(0, -50));
            words6 = new SpriteIMG(Art.words6, Game1.GetScreenCenter() + new Vector2(0, -50));

            words1.alpha = 0;
            words2.alpha = 0;
            words3.alpha = 0;
            words4.alpha = 0;
            words5.alpha = 0;
            words6.alpha = 0;
        }

        static public void FadeInOut(int duration, Color color)
        {
            fadeOverlay.color = color;
            fadeOverlay.am.StartFade(duration / 2, 0, 255);
            isFadingIn = true;
            currentDuration = duration;
        }

        static public void FadeIn(int duration, Color color)
        {
            fadeOverlay.color = color;
            fadeOverlay.am.StartFade(duration, fadeOverlay.alpha, 255);
            currentDuration = duration;
        }

        static public void FadeOut(int duration)
        {
            fadeOverlay.am.StartFade(duration, fadeOverlay.alpha, 0);
            currentDuration = duration;
        }

        static public void Update(GameTime gameTime)
        {

            fadeOverlay.Update();
            words1.Update(); 
            words2.Update(); 
            words3.Update(); 
            words4.Update(); 
            words5.Update();
            words6.Update(); 

            if (isFadingIn)
            {
                if (!fadeOverlay.am.fading)
                {
                    fadeOverlay.am.StartFade(currentDuration / 2, fadeOverlay.alpha, 0);
                    isFadingIn = false;
                }
            }

            if (fadeOverlay.alpha == 0) fadeOverlay.visible = false;
            else fadeOverlay.visible = true;
        }

        static public void Draw(SpriteBatch spriteBatch)
        {
            //gsm.game.GraphicsDevice.SetRenderTarget(overlayTarget);
            //gsm.game.GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            words1.Draw(spriteBatch); 
            words2.Draw(spriteBatch); 
            words3.Draw(spriteBatch); 
            words4.Draw(spriteBatch); 
            words5.Draw(spriteBatch);
            words6.Draw(spriteBatch); 

            spriteBatch.End();

            if (fadeOverlay.color == Color.Black)
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            else if (fadeOverlay.color == Color.White)
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            else
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            fadeOverlay.Draw(spriteBatch);
            spriteBatch.End();

            //gsm.game.GraphicsDevice.SetRenderTarget(null);
            //spriteBatch.Begin();
            //spriteBatch.Draw(overlayTarget, Vector2.Zero, Color.White);
            //spriteBatch.End();
        }
        
    }
}
