﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Net;
//using Microsoft.Xna.Framework.GamerServices;

using WorldsApart.Code.Graphics;
using WorldsApart.Code.Controllers;

using System.Diagnostics;

namespace WorldsApart.Code.Gamestates
{
    class GSMenu : GameState
    {
        int menuIndex = 0;

        SpriteIMG logo;

        SpriteIMG singlePlayer;
        SpriteIMG multiPlayer;
        SpriteIMG exit;

        ScrollingBackground backdropBase;
        ScrollingBackground backdropOverlay;
        ScrollingBackground backdropMatte;

        bool startPressed = false;
        bool firstBit = true;

        public bool ableToThumbUp = false;
        public bool ableToThumbDown = false;

        float selectScale = 1.2f;
        float unselectScale = 1f;

        byte selectAlpha = 255;
        byte unselectAlpha = 128;

        public GSMenu(GameStateManager gsm)
            : base(gsm)
        {
            logo = new SpriteIMG(LoadTexture("TitleAssets/logoWorldsApart"), Game1.GetScreenCenter() + new Vector2(0, -200));
            singlePlayer = new SpriteIMG(LoadTexture("TitleAssets/multiPlayer"), Game1.GetScreenCenter() + new Vector2(0, 0));
            multiPlayer = new SpriteIMG(LoadTexture("TitleAssets/singlePlayer"), Game1.GetScreenCenter() + new Vector2(0, 100));
            exit = new SpriteIMG(LoadTexture("TitleAssets/exit"), Game1.GetScreenCenter() + new Vector2(0, 200));


            multiPlayer.alpha = 128;
            exit.alpha = 128;

            backdropBase = new ScrollingBackground(LoadTexture("BGs/fieldBackdropBase1"), false);
            backdropOverlay = new ScrollingBackground(LoadTexture("BGs/fieldBackdropOverlay2"), false);
            backdropMatte = new ScrollingBackground(LoadTexture("TitleAssets/menuBackdrop"), false);
        }

        public void MenuUp()
        {
            menuIndex--;
            if (menuIndex < 0) menuIndex = 0;
        }

        public void MenuDown()
        {
            menuIndex++;
            if (menuIndex > 2) menuIndex = 2;
        }

        public void GetInput()
        {
            if (stopInput) return;

            if (InputManager.IsButtonPressed(Buttons.DPadUp) || InputManager.IsKeyPressed(Keys.W) || InputManager.IsKeyPressed(Keys.Up))
            {
                MenuUp();
            }

            if (InputManager.IsButtonPressed(Buttons.DPadDown) || InputManager.IsKeyPressed(Keys.S) || InputManager.IsKeyPressed(Keys.Down))
            {
                MenuDown();
            }

            if (InputManager.GetLeftThumbstick().Y > .5f)
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

            if (InputManager.GetLeftThumbstick().Y > -.5f)
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

            if (InputManager.IsButtonPressed(Buttons.A) || InputManager.IsButtonPressed(Buttons.Start) || InputManager.IsKeyPressed(Keys.Enter))
            {
                startPressed = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            singlePlayer.Update();
            multiPlayer.Update();
            exit.Update();

            backdropBase.UpdateScroll(-.25f);
            backdropOverlay.UpdateScroll(-.5f);
            backdropMatte.UpdateScroll(-1f);

            startPressed = false;

            GetInput();

            if (firstBit)
            {
                startPressed = false;
                firstBit = false;
            }

            switch (menuIndex)
            {
                case 0:
                    ActivateItem(singlePlayer);
                    DeactivateItem(multiPlayer);
                    DeactivateItem(exit);
                    if (startPressed)
                    {
                        GameStateManager.isMultiplayer = false;
                        //gameStateManager.SwitchToGSPlay();
                        gameStateManager.TransitionToGameState(this, GameStateType.GSPlay, 30);
                    }
                    break;
                case 1:
                    DeactivateItem(singlePlayer);
                    ActivateItem(multiPlayer);
                    DeactivateItem(exit);
                    if (startPressed)
                    {
                        GameStateManager.isMultiplayer = true;
                        //gameStateManager.SwitchToGSPlay();
                        gameStateManager.TransitionToGameState(this, GameStateType.GSPlay, 30);
                    }
                    break;
                case 2:
                    DeactivateItem(singlePlayer);
                    DeactivateItem(multiPlayer);
                    ActivateItem(exit);
                    if (startPressed)
                    {
                        gameStateManager.game.Exit();
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

        public void DimAll()
        {
            singlePlayer.alpha = 128;
            singlePlayer.alpha = 128;
            singlePlayer.alpha = 128;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            gameStateManager.game.GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            backdropBase.Draw(spriteBatch);
            backdropOverlay.Draw(spriteBatch);
            backdropMatte.Draw(spriteBatch);

            logo.Draw(spriteBatch);
            singlePlayer.Draw(spriteBatch);
            multiPlayer.Draw(spriteBatch);
            exit.Draw(spriteBatch);

            spriteBatch.End();
        }
       

    }
}
