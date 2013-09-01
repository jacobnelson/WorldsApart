using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using WorldsApart.Code.Graphics;
using WorldsApart.Code.Controllers;

namespace WorldsApart.Code.Gamestates
{
    class GSPause : GameState
    {
        SpriteIMG backdrop;

        SpriteIMG resume;
        SpriteIMG mainMenu;

        bool firstBit = true;

        public int menuIndex = 0;

        public bool ableToThumbUp = false;
        public bool ableToThumbDown = false;

        float selectScale = 1.2f;
        float unselectScale = 1f;

        byte selectAlpha = 255;
        byte unselectAlpha = 128;

        public GSPause(GameStateManager gsm) : base(gsm)
        {
            resume = new SpriteIMG(LoadTexture("TitleAssets/resume"), Game1.GetScreenCenter() + new Vector2(0, -50));
            mainMenu = new SpriteIMG(LoadTexture("TitleAssets/mainMenu"), Game1.GetScreenCenter() + new Vector2(0, 50));

            backdrop = new SpriteIMG(LoadTexture("TitleAssets/pauseBG"), Game1.GetScreenCenter());

        }

        public void MenuUp()
        {
            menuIndex--;
            if (menuIndex < 0) menuIndex = 0;
        }

        public void MenuDown()
        {
            menuIndex++;
            if (menuIndex > 1) menuIndex = 1;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            resume.Update();
            mainMenu.Update();

            if (InputManager.IsButtonPressed(Buttons.DPadUp) || InputManager.IsKeyPressed(Keys.W) || InputManager.IsKeyPressed(Keys.Up) || InputManager.IsButtonPressed2(Buttons.DPadUp))
            {
                MenuUp();
            }

            if (InputManager.IsButtonPressed(Buttons.DPadDown) || InputManager.IsKeyPressed(Keys.S) || InputManager.IsKeyPressed(Keys.Down) || InputManager.IsButtonPressed2(Buttons.DPadDown))
            {
                MenuDown();
            }

            if (InputManager.GetLeftThumbstick().Y > .5f || InputManager.GetLeftThumbstick2().Y > .5f)
            {
                if (ableToThumbDown)
                {
                    MenuUp();
                    ableToThumbDown = false;
                }
            }
            else
            {
                ableToThumbDown = true;
            }

            if (InputManager.GetLeftThumbstick().Y > -.5f || InputManager.GetLeftThumbstick2().Y < - .5f)
            {
                if (ableToThumbUp)
                {
                    MenuDown();
                    ableToThumbUp = false;
                }
            }
            else
            {
                ableToThumbUp = true;
            }

            bool startPressed = false;
            if (InputManager.IsButtonPressed(Buttons.A) || InputManager.IsButtonPressed(Buttons.Start) || InputManager.IsKeyPressed(Keys.Enter) || InputManager.IsButtonPressed2(Buttons.Start) || InputManager.IsButtonPressed2(Buttons.A))
            {
                startPressed = true;
            }

            if (firstBit)
            {
                startPressed = false;
                firstBit = false;
            }

            switch (menuIndex)
            {
                case 0:
                    ActivateItem(resume);
                    DeactivateItem(mainMenu);
                    if (startPressed)
                        gameStateManager.SwitchToGSPlay();
                    break;
                case 1:
                    DeactivateItem(resume);
                    ActivateItem(mainMenu);
                    if (startPressed)
                    {
                        AudioManager.PlayMusic("Title");
                        gameStateManager.currentLevel = 1;
                        gameStateManager.SwitchToGSMenu();
                    }
                    break;
            }
        }

        public void ActivateItem(SpriteIMG item)
        {
            if (!item.am.scaling && item.scale != new Vector2(selectScale))
            {
                item.am.StartScale(20, item.scale, new Vector2(selectScale));
            }
            if (!item.am.fading && item.alpha != selectAlpha)
            {
                item.am.StartFade(20, item.alpha, selectAlpha);
            }
        }

        public void DeactivateItem(SpriteIMG item)
        {
            if (!item.am.scaling && item.scale != new Vector2(unselectScale))
            {
                item.am.StartScale(20, item.scale, new Vector2(unselectScale));
            }
            if (!item.am.fading && item.alpha != unselectAlpha)
            {
                item.am.StartFade(20, item.alpha, unselectAlpha);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            backdrop.Draw(spriteBatch);
            resume.Draw(spriteBatch);
            mainMenu.Draw(spriteBatch);


            spriteBatch.End();
        }
    }
}
