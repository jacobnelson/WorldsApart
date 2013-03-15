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
        SpriteIMG titleScreen;
        SpriteIMG title;
        SpriteIMG start;
        AnimatedSprite blob;

        int timerCounter = 0;
       

        public GSTitle(GameStateManager gsm)
            : base(gsm)
        {
            titleScreen = new SpriteIMG(LoadTexture("TitleAssets/TitleScreen"), new Vector2(Game1.screenWidth / 2, Game1.screenHeight / 2));
            title = new SpriteIMG(LoadTexture("TitleAssets/title"), new Vector2(1500, 100));
            start = new SpriteIMG(LoadTexture("TitleAssets/start"), new Vector2(Game1.screenWidth / 2, 500));
            start.alpha = 0;
            blob = new AnimatedSprite(LoadTexture("TitleAssets/blob"), new Vector2(-500, 300));
            blob.SetAnimationStuff(1, 1, 1, 3, 192, 192, 3, 10);

            title.am.StartNewAnimation(AnimationType.EaseOutQuart, new Vector2(1500, 100), new Vector2(Game1.screenWidth / 2, 100), 120);
            blob.am.StartNewAnimation(AnimationType.EaseOutQuart, new Vector2(-500, 300), new Vector2(Game1.screenWidth / 2, 300), 120);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            timerCounter++;

            if (timerCounter == 120)
            {
                start.am.StartFade(60, 0, 255);
            }

            blob.Update();
            start.Update();
            title.Update();
            blob.Update();

            if (InputManager.IsButtonPressed(Buttons.Start) || InputManager.IsKeyPressed(Keys.Enter))
            {
                gameStateManager.SwitchToGSPlay();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);


            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            titleScreen.Draw(spriteBatch);
            blob.Draw(spriteBatch);
            start.Draw(spriteBatch);
            title.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
