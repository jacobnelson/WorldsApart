using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using WorldsApart.Code.Graphics;
using WorldsApart.Code.Controllers;

namespace WorldsApart.Code.Gamestates
{
    
    class GSHowToPlay : GameState
    {
        static public bool gamepad = false;

        List<SpriteIMG> gamepadList = new List<SpriteIMG>();
        List<SpriteIMG> keyboardList = new List<SpriteIMG>();

        SpriteIMG gamepadNext;
        SpriteIMG keyboardNext;

        int currentFrame = 0;

        bool nextPressed = false;

        bool firstUpdate = true;

        public GSHowToPlay(GameStateManager gsm)
            : base(gsm)
        {
            gamepadList.Add(new SpriteIMG(LoadTexture("HowToPlay/gamepad1"), Game1.GetScreenCenter()));
            gamepadList.Add(new SpriteIMG(LoadTexture("HowToPlay/gamepad2"), Game1.GetScreenCenter()));
            gamepadList.Add(new SpriteIMG(LoadTexture("HowToPlay/gamepad3"), Game1.GetScreenCenter()));
            gamepadList.Add(new SpriteIMG(LoadTexture("HowToPlay/gamepad4"), Game1.GetScreenCenter()));
            gamepadList.Add(new SpriteIMG(LoadTexture("HowToPlay/gamepad5"), Game1.GetScreenCenter()));

            keyboardList.Add(new SpriteIMG(LoadTexture("HowToPlay/keyboard1"), Game1.GetScreenCenter()));
            keyboardList.Add(new SpriteIMG(LoadTexture("HowToPlay/keyboard2"), Game1.GetScreenCenter()));
            keyboardList.Add(new SpriteIMG(LoadTexture("HowToPlay/keyboard3"), Game1.GetScreenCenter()));
            keyboardList.Add(new SpriteIMG(LoadTexture("HowToPlay/keyboard4"), Game1.GetScreenCenter()));
            keyboardList.Add(new SpriteIMG(LoadTexture("HowToPlay/keyboard5"), Game1.GetScreenCenter()));

            gamepadNext = new SpriteIMG(LoadTexture("HowToPlay/gamepadNext"), Game1.GetScreenCenter());
            keyboardNext = new SpriteIMG(LoadTexture("HowToPlay/keyboardNext"), Game1.GetScreenCenter());

            
        }

        public void GetInput()
        {
            if (stopInput) return;

            if (firstUpdate)
            {
                firstUpdate = false;
                return;
            }

            if (InputManager.IsButtonPressed(Buttons.A) || InputManager.IsKeyPressed(Keys.Enter))
            {
                nextPressed = true;
            }
            else
            {
                nextPressed = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            GetInput();

            if (nextPressed)
            {
                if (currentFrame == 4)
                {
                    //Switch back to menu
                    gameStateManager.SwitchToGSMenu();
                }
                else
                {
                    currentFrame++;
                }

            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);


            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            if (gamepad)
            {
                gamepadList[currentFrame].Draw(spriteBatch);
                gamepadNext.Draw(spriteBatch);
            }
            else
            {
                keyboardList[currentFrame].Draw(spriteBatch);
                keyboardNext.Draw(spriteBatch);
            }

            


            spriteBatch.End();
        }
    }
}
