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
        SpriteIMG winScreen;

        SpriteIMG win;
        SpriteIMG prize;

        OBB buttonYay;
        OBB cursor;

        int timeCounter = 0;

        public GSWin(GameStateManager gsm)
            : base(gsm)
        {
            winScreen = new SpriteIMG(LoadTexture("WinAssets/winScreen"), Game1.GetScreenCenter());
            win = new SpriteIMG(LoadTexture("WinAssets/win"), new Vector2(Game1.screenWidth / 2, -400));
            prize = new SpriteIMG(LoadTexture("WinAssets/prize"), new Vector2(Game1.screenWidth / 2, 900));

            win.am.StartNewAnimation(AnimationType.EaseOutCirc, win.position, new Vector2(Game1.screenWidth / 2, 150), 120);
            prize.am.StartNewAnimation(AnimationType.EaseOutSine, prize.position, new Vector2(Game1.screenWidth / 2, 300), 120);

            AnimatedSprite img = new AnimatedSprite(LoadTexture("TestSprites/buttonYay"));
            img.SetAnimationStuff(1, 1, 1, 2, 64, 64, 2, 5);
            img.isAnimating = false;
            img.alpha = 0;
            buttonYay = new OBB(LoadTexture("TestSprites/puff"), img, new Vector2(Game1.screenWidth / 2, 400));
            buttonYay.AddVertex(new Vector2(16, -28));
            buttonYay.AddVertex(new Vector2(28, -16));
            buttonYay.AddVertex(new Vector2(28, 16));
            buttonYay.AddVertex(new Vector2(16, 28));
            buttonYay.AddVertex(new Vector2(-12, 28));
            buttonYay.AddVertex(new Vector2(-28, 16));
            buttonYay.AddVertex(new Vector2(-28, -16));
            buttonYay.AddVertex(new Vector2(-12, -28));

            img = new AnimatedSprite(LoadTexture("TestSprites/cursor"));
            img.SetAnimationStuff(1, 1, 1, 2, 64, 64, 2, 5);
            img.isAnimating = false;
            img.alpha = 0;
            cursor = new OBB(LoadTexture("TestSprites/puff"),img, Game1.GetScreenCenter());
            cursor.AddVertex(new Vector2(16, 16));
            cursor.AddVertex(new Vector2(-16, 16));
            cursor.AddVertex(new Vector2(-16, -16));
            cursor.AddVertex(new Vector2(16, -16));
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);

            if (timeCounter == 120)
            {
                buttonYay.image.am.StartFade(60, 0, 255);
                cursor.image.am.StartFade(60, 0, 255);
                timeCounter++;
            }
            else if (timeCounter > 120) {}
            else timeCounter++;

            win.Update();
            prize.Update();

            //MouseState ms = Mouse.GetState();

            //cursor.position = new Vector2(ms.X, ms.Y);
            float speed = 2;
            if (InputManager.IsButtonDown(Buttons.DPadUp) || InputManager.GetLeftThumbstick().Y > 0 || InputManager.IsKeyDown(Keys.Up))
            {
                cursor.position += new Vector2(0, -speed);
            }
            if (InputManager.IsButtonDown(Buttons.DPadDown) || InputManager.GetLeftThumbstick().Y < 0 || InputManager.IsKeyDown(Keys.Down))
            {
                cursor.position += new Vector2(0, speed);
            }
            if (InputManager.IsButtonDown(Buttons.DPadRight) || InputManager.GetLeftThumbstick().X > 0 || InputManager.IsKeyDown(Keys.Right))
            {
                cursor.position += new Vector2(speed, 0);
            }
            if (InputManager.IsButtonDown(Buttons.DPadLeft) || InputManager.GetLeftThumbstick().X < 0 || InputManager.IsKeyDown(Keys.Left))
            {
                cursor.position += new Vector2(-speed, 0);
            }

            buttonYay.CheckCollision(cursor);

            buttonYay.Update();
            cursor.Update();

            if (InputManager.IsButtonPressed(Buttons.A) || InputManager.IsKeyPressed(Keys.Enter))
            {
                gameStateManager.SwitchToGSTitle();
            }


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Begin();
            winScreen.Draw(spriteBatch);
            win.Draw(spriteBatch);
            prize.Draw(spriteBatch);
            spriteBatch.End();
            buttonYay.Draw(spriteBatch);
            cursor.Draw(spriteBatch);
        }
    }
}
