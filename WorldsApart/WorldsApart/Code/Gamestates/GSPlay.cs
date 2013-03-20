using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

using WorldsApart.Code.Entities;
using WorldsApart.Code.Controllers;
using WorldsApart.Code.Levels;
using WorldsApart.Code.Graphics;

using System.Diagnostics;
using System.Threading;

namespace WorldsApart.Code.Gamestates
{
    class GSPlay : GameState
    {
        private Thread backgroundThread;
        private bool isLoading = false;
        private bool okayToDraw = true;

        Level level;


        public Camera cameraPlayer1;
        public Camera cameraPlayer2;
        PlayerIndex playerIndex = PlayerIndex.One;

        SpriteIMG alphaDot;
        Effect alphaShader;
        Effect lightingShader;
        Effect colorShader;
        RenderTarget2D alphaMask;
        RenderTarget2D lightMask;
        RenderTarget2D renderTarget;
        RenderTarget2D player1Objects;
        RenderTarget2D player2Objects;
        RenderTarget2D alphaPlayer;
        RenderTarget2D nonAlphaPlayer;
        RenderTarget2D finalTarget;
        RenderTarget2D bgTarget;

        GaussianBlur gaussianBlur;
        private const int BLUR_RADIUS = 7;
        private const float BLUR_AMOUNT = 2f;
        RenderTarget2D blurX;
        RenderTarget2D blurY;
        RenderTarget2D auraTarget;
        Vector2 auraTargetOrigin = Vector2.Zero;
        RenderTarget2D player2Aura;
        RenderTarget2D blurX2;
        RenderTarget2D blurY2;

        SpriteIMG loadingScreen;

        public Player player1;
        public Player player2;
        public List<SpriteIMG> tileList = new List<SpriteIMG>();
        public List<PickUpObj> pickUpList = new List<PickUpObj>();
        public List<GaussianTargets> pickUpAuras = new List<GaussianTargets>();
        public List<Button> buttonList = new List<Button>();
        public List<Door> doorList = new List<Door>();
        public List<FlipSwitch> switchList = new List<FlipSwitch>();
        public List<Moveable> moveList = new List<Moveable>();
        public List<GaussianTargets> moveAuras = new List<GaussianTargets>();
        public List<MovingPlatform> platformList = new List<MovingPlatform>();
        public List<CircularPlatform> cPlatformList = new List<CircularPlatform>();
        public List<TriggerArea> areaList = new List<TriggerArea>();
        public List<TimerObject> timerList = new List<TimerObject>();
        public List<Particle> particleList = new List<Particle>();
        public List<ParticleEmitter> emitterList = new List<ParticleEmitter>();
        public List<ParallaxLayer> bgLayerList = new List<ParallaxLayer>();
        public List<PointLight> lightList = new List<PointLight>();
        public List<LightConsole> consoleList = new List<LightConsole>();
        public List<Portal> portalList = new List<Portal>();

        public GSPlay(GameStateManager gsm, int levelIndex)
            : base(gsm)
        {
            //backgroundThread = new Thread(LoadTheStuffs);
            //isLoading = true;

            //backgroundThread.Start();
            LoadTheStuffs(levelIndex);

            

            loadingScreen = new SpriteIMG(LoadTexture("loadingScreen"), Game1.GetScreenCenter());
        }

        public void LoadTheStuffs(int levelIndex)
        {

            switch (levelIndex)
            {
                case 0:
                    level = new TestLevel(this);
                    break;
                case 1:
                    level = new Level1(this);
                    break;
                case 2:
                    level = new Level2(this);
                    break;
                case 3:
                    level = new Level3(this);
                    break;
                case 4:
                    level = new Level4(this);
                    break;
                case 5:
                    level = new Level5(this);
                    break;
                case 6:
                    level = new Level6(this);
                    break;
                case 7:
                    level = new Level7(this);
                    break;
                default:
                    level = new TestLevel(this);
                    break;
            }

            
            
            alphaDot = new SpriteIMG(LoadTexture("ShaderAssets/playerAlphaMask"));
            alphaMask = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            renderTarget = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            player1Objects = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            player2Objects = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            alphaPlayer = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            nonAlphaPlayer = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            finalTarget = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            alphaShader = LoadEffect("ShaderAssets/AlphaMapping");
            lightingShader = LoadEffect("ShaderAssets/Lighting");
            colorShader = LoadEffect("ShaderAssets/ColorTransform");
            lightMask = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            auraTarget = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            auraTargetOrigin = new Vector2(Game1.screenWidth / 2, Game1.screenHeight / 2);
            player2Aura = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            bgTarget = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);

            gaussianBlur = new GaussianBlur(gameStateManager.game);
            gaussianBlur.ComputeKernel(BLUR_RADIUS, BLUR_AMOUNT);

            int renderTargetWidth = Game1.screenWidth;
            int renderTargetHeight = Game1.screenHeight;

            blurX = new RenderTarget2D(gameStateManager.game.GraphicsDevice,
                renderTargetWidth, renderTargetHeight, false,
                gameStateManager.game.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.None);
            blurY = new RenderTarget2D(gameStateManager.game.GraphicsDevice,
                renderTargetWidth, renderTargetHeight, false,
                gameStateManager.game.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.None);
            blurX2 = new RenderTarget2D(gameStateManager.game.GraphicsDevice,
                renderTargetWidth, renderTargetHeight, false,
                gameStateManager.game.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.None);
            blurY2 = new RenderTarget2D(gameStateManager.game.GraphicsDevice,
                renderTargetWidth, renderTargetHeight, false,
                gameStateManager.game.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.None);

            


            gaussianBlur.ComputeOffsets(renderTargetWidth, renderTargetHeight);
            //okayToDraw = false;
            gameStateManager.game.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, colorShader);
            player1.Draw(gameStateManager.game.spriteBatch);
            gameStateManager.game.spriteBatch.End();
            gaussianBlur.PerformGaussianBlur(player1.texture, blurX, blurY, gameStateManager.game.spriteBatch);
            //okayToDraw = true;
            isLoading = false;



            player1.indicatorTexture = LoadTexture("player1Ideal");
            player2.indicatorTexture = LoadTexture("player2Ideal");
            //if (gameStateManager.goodness < 0)
            //{
            //    RenderTarget2D indicatorTarget = new RenderTarget2D(GraphicsDevice, player1.texture.Width, player1.texture.Height);
            //    GraphicsDevice.SetRenderTarget(indicatorTarget);
            //    GraphicsDevice.Clear(Color.Transparent);
            //    colorShader.Parameters["DestColor"].SetValue(Color.Black.ToVector4());
            //    gameStateManager.game.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, colorShader);
            //    gameStateManager.game.spriteBatch.Draw(player1.indicatorTexture, Vector2.Zero, Color.White);
            //    gameStateManager.game.spriteBatch.End();
            //    player1.indicatorTexture = indicatorTarget;

            //    RenderTarget2D indicatorTarget2 = new RenderTarget2D(GraphicsDevice, player2.texture.Width, player2.texture.Height);
            //    GraphicsDevice.SetRenderTarget(indicatorTarget2);
            //    GraphicsDevice.Clear(Color.Transparent);
            //    colorShader.Parameters["DestColor"].SetValue(Color.Black.ToVector4());
            //    gameStateManager.game.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, colorShader);
            //    gameStateManager.game.spriteBatch.Draw(player2.indicatorTexture, Vector2.Zero, Color.White);
            //    gameStateManager.game.spriteBatch.End();
            //    player2.indicatorTexture = indicatorTarget2;
            //}

            switch (gameStateManager.goodness)
            {
                case 0:
                    player1.indicatorAlpha = 255;
                    player2.indicatorAlpha = 255;
                    break;
                case -1:
                    player1.indicatorAlpha = 229;
                    player2.indicatorAlpha = 229;
                    break;
                case 1:
                    player1.indicatorAlpha = 26;
                    player2.indicatorAlpha = 26;
                    break;
                case -2:
                    player1.indicatorAlpha = 191;
                    player2.indicatorAlpha = 191;
                    break;
                case 2:
                    player1.indicatorAlpha = 64;
                    player2.indicatorAlpha = 64;
                    break;
                case -3:
                    player1.indicatorAlpha = 153;
                    player2.indicatorAlpha = 153;
                    break;
                case 3:
                    player1.indicatorAlpha = 102;
                    player2.indicatorAlpha = 102;
                    break;
                case -4:
                case 4:
                    player1.indicatorAlpha = 115;
                    player2.indicatorAlpha = 115;
                    break;
                case -5:
                case 5:
                    player1.indicatorAlpha = 77;
                    player2.indicatorAlpha = 77;
                    break;
                case -6:
                case 6:
                    player1.indicatorAlpha = 38;
                    player2.indicatorAlpha = 38;
                    break;
                case -7:
                case 7:
                    player1.indicatorAlpha = 0;
                    player2.indicatorAlpha = 0;
                    break;
            }

            RenderTarget2D player1Texture = new RenderTarget2D(GraphicsDevice, player1.texture.Width, player1.texture.Height);
            GraphicsDevice.SetRenderTarget(player1Texture);
            GraphicsDevice.Clear(Color.Transparent);
            gameStateManager.game.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            if (gameStateManager.goodness > 0 && gameStateManager.goodness < 4)
            {
                gameStateManager.game.spriteBatch.Draw(player1.indicatorTexture, Vector2.Zero, Color.White);
                gameStateManager.game.spriteBatch.Draw(player1.texture, Vector2.Zero, new Color(0, 0, 0, player1.indicatorAlpha));
                
            }
            else if (gameStateManager.goodness >= 4)
            {
                gameStateManager.game.spriteBatch.Draw(player1.texture, Vector2.Zero, Color.White);
                gameStateManager.game.spriteBatch.Draw(player1.indicatorTexture, Vector2.Zero, new Color(0, 0, 0, player1.indicatorAlpha));
            }
            else if (gameStateManager.goodness <= 0)
            {
                byte c = player1.indicatorAlpha;
                gameStateManager.game.spriteBatch.Draw(player1.indicatorTexture, Vector2.Zero, new Color(c, c, c));
            }
            gameStateManager.game.spriteBatch.End();
            player1.indicatorTexture = player1Texture;
            

            RenderTarget2D player2Texture = new RenderTarget2D(GraphicsDevice, player2.texture.Width, player2.texture.Height);
            GraphicsDevice.SetRenderTarget(player2Texture);
            GraphicsDevice.Clear(Color.Transparent);
            gameStateManager.game.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            if (gameStateManager.goodness > 0 && gameStateManager.goodness < 4)
            {
                gameStateManager.game.spriteBatch.Draw(player2.indicatorTexture, Vector2.Zero, Color.White);
                gameStateManager.game.spriteBatch.Draw(player2.texture, Vector2.Zero, new Color(0, 0, 0, player2.indicatorAlpha));
                
            }
            else if (gameStateManager.goodness >= 4)
            {
                gameStateManager.game.spriteBatch.Draw(player2.texture, Vector2.Zero, Color.White);
                gameStateManager.game.spriteBatch.Draw(player2.indicatorTexture, Vector2.Zero, new Color(0, 0, 0, player2.indicatorAlpha));
            }
            else if (gameStateManager.goodness <= 0)
            {
                byte c = player2.indicatorAlpha;
                gameStateManager.game.spriteBatch.Draw(player2.indicatorTexture, Vector2.Zero, new Color(c, c, c));
            }
            gameStateManager.game.spriteBatch.End();
            player2.indicatorTexture = player2Texture;
        }

        //public void DrawLoadingScreen(SpriteBatch spriteBatch)
        //{
        //    gameStateManager.game.GraphicsDevice.SetRenderTarget(null);
        //    spriteBatch.Begin();
        //    loadingScreen.Draw(spriteBatch);
        //    spriteBatch.End();
        //}

        #region Add Objects
        public void AddGroundTile(Vector2 tilePosition)
        {
            SpriteIMG tile = new SpriteIMG(LoadTexture("TestSprites/tile"), tilePosition);
            tileList.Add(tile);

        }

        public void AddPickUp(Texture2D texture, Vector2 position, PlayerObjectMode playerMode)
        {
            PickUpObj obj = new PickUpObj(texture, position);
            obj.playerTangible = playerMode;
            obj.playerVisible = playerMode;
            obj.SetAnimationStuff(1, 1, 1, 3, 32, 32, 3, 5);
            obj.SetCollisionBox(22, 22, Vector2.Zero);
            pickUpList.Add(obj);
        }

        public PickUpObj AddBouncyBall(float bounceMultiplier, Texture2D texture, Vector2 position)
        {
            PickUpObj obj = new PickUpObj(bounceMultiplier, texture, position);
            pickUpList.Add(obj);
            pickUpAuras.Add(new GaussianTargets(gameStateManager.game.GraphicsDevice));
            obj.SetAnimationStuff(1, 1, 1, 3, 32, 32, 3, 20);
            obj.SetCollisionBox(22, 22, Vector2.Zero);
            return obj;
        }
        public PickUpObj AddBouncyBall(float bounceMultiplier, Texture2D texture, Vector2 position, PlayerObjectMode playerMode)
        {
            PickUpObj obj = new PickUpObj(bounceMultiplier, texture, position);
            obj.playerTangible = playerMode;
            obj.playerVisible = playerMode;
            pickUpList.Add(obj);
            pickUpAuras.Add(new GaussianTargets(gameStateManager.game.GraphicsDevice));
            obj.SetAnimationStuff(1, 1, 1, 3, 32, 32, 3, 10);
            obj.SetCollisionBox(22, 22, Vector2.Zero);
            return obj;
        }

        public Moveable AddMoveable(Texture2D texture, Vector2 position, float moveModifier = .5f)
        {
            Moveable move = new Moveable(texture, position);
            move.moveModifier = moveModifier;
            moveList.Add(move);
            moveAuras.Add(new GaussianTargets(gameStateManager.game.GraphicsDevice));
            return move;
        }

        public CircularPlatform AddCircularPlatform(Texture2D texture, Vector2 centerPosition, float radius, float duration)
        {
            CircularPlatform platform = new CircularPlatform(texture, centerPosition, radius);
            platform.UpdateIncrement(duration);
            cPlatformList.Add(platform);
            return platform;
        }

        public CircularPlatform AddActivateCircularPlatform(Texture2D texture, Vector2 centerPosition, float radius, float duration)
        {
            CircularPlatform platform = new CircularPlatform(texture, centerPosition, radius);
            platform.moving = false;
            platform.platformMode = PlatformMode.PressToActivate;
            platform.UpdateIncrement(duration);
            cPlatformList.Add(platform);
            return platform;
        }

        public MovingPlatform AddPlatform(Texture2D texture, Vector2 startPosition, Vector2 endPosition)
        {
            MovingPlatform platform = new MovingPlatform(texture, startPosition, endPosition);
            platformList.Add(platform);
            return platform;
        }

        public MovingPlatform AddReversePlatform(Texture2D texture, Vector2 startPosition, Vector2 endPosition)
        {
            MovingPlatform platform = new MovingPlatform(texture, startPosition, endPosition);
            platform.looping = false;
            platformList.Add(platform);
            return platform;
        }

        public MovingPlatform AddActivatePlatform(Texture2D texture, Vector2 startPosition, Vector2 endPosition)
        {
            MovingPlatform platform = new MovingPlatform(texture, startPosition, endPosition);
            platform.platformerMode = PlatformMode.PressToActivate;
            platform.moving = false;
            platformList.Add(platform);
            return platform;
        }

        public Button AddButton(EventTrigger eventTrigger, float neededMass, Texture2D texture, Vector2 position)
        {
            Button button = new Button(eventTrigger, texture, position);
            button.neededMass = neededMass;
            buttonList.Add(button);
            return button;
        }

        public TriggerArea AddTriggerArea(EventTrigger eventTrigger, Texture2D texture, Vector2 position)
        {
            TriggerArea area = new TriggerArea(eventTrigger, texture, position);
            areaList.Add(area);
            return area;
        }

        public FlipSwitch AddSwitch(EventTrigger eventTrigger, Texture2D texture, Vector2 position)
        {
            FlipSwitch s = new FlipSwitch(eventTrigger, texture, position);
            switchList.Add(s);
            return s;
        }

        public Door AddOpeningDoor(Texture2D texture, Vector2 openPosition, Vector2 closePosition, OpenState state)
        {

            Door door = new Door(texture, openPosition, closePosition, state);
            doorList.Add(door);
            return door;
        }

        public Door AddFadingDoor(Texture2D texture, Vector2 position, OpenState state)
        {
            Door door = new Door(texture, position, state);
            doorList.Add(door);
            return door;
        }

        public Collectible AddCollectible(EventTrigger eventTrigger, Texture2D texture, Vector2 position)
        {
            Collectible collectible = new Collectible(eventTrigger, texture, position);
            areaList.Add(collectible);
            return collectible;
        }

        public Portal AddPortal(EventTrigger eventTrigger, Texture2D texture, Vector2 position)
        {
            Portal portal = new Portal(player1, player2, eventTrigger, texture, position);
            portalList.Add(portal);
            return portal;
        }

        public Particle AddParticle(AnimatedSprite image, Vector2 position, Vector2 speed)
        {
            Particle p = new Particle(this, image.texture, position);
            p.SetAnimationStuff(image.minRow, image.minCol, image.rows, image.cols, image.cellW, image.cellH, image.frames, image.animationCounter);
            p.speed = speed;
            particleList.Add(p);
            return p;
        }

        public ParticleEmitter AddEmitter(AnimatedSprite image, Vector2 position)
        {
            ParticleEmitter emitter = new ParticleEmitter(this, image, position);
            emitterList.Add(emitter);
            return emitter;
        }

        public TimerObject AddTimer(EventTrigger eventTrigger, int duration)
        {
            TimerObject to = new TimerObject(eventTrigger, duration);
            timerList.Add(to);
            return to;

        }

        public void AddParallax(SpriteIMG img, float parallaxRatio)
        {
            bool noMatches = true;
            img.position *= parallaxRatio;
            foreach (ParallaxLayer bg in bgLayerList)
            {
                if (bg.parallaxRatio == parallaxRatio)
                {
                    bg.imageList.Add(img); 
                    noMatches = false;
                }

            }
            if (noMatches) 
            {
                ParallaxLayer bg = new ParallaxLayer();
                bg.imageList.Add(img);
                bg.parallaxRatio = parallaxRatio;
                bgLayerList.Add(bg);
            }
        }

        public PointLight AddPointLight(Texture2D texture, Vector2 position, Vector2 scale)
        {
            PointLight light = new PointLight(texture, position, scale);
            lightList.Add(light);
            return light;
        }

        public LightConsole AddLightConsole(Texture2D texture, Vector2 position)
        {
            LightConsole console = new LightConsole(texture, position);
            consoleList.Add(console);
            return console;

        }

        public LightConsole AddLinkedLightConsole(LightConsole linkedConsole, Texture2D texture, Vector2 position)
        {
            LightConsole console = new LightConsole(texture, position);
            console.LinkConsole(linkedConsole);
            consoleList.Add(console);
            return console;
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            if (isLoading) return;
            base.Update(gameTime);

            if (InputManager.IsButtonPressed(Buttons.Back) || InputManager.IsKeyPressed(Keys.LeftShift))
            {
                if (playerIndex == PlayerIndex.One) playerIndex = PlayerIndex.Two;
                else playerIndex = PlayerIndex.One;
            }

            if (InputManager.IsButtonPressed(Buttons.Y) || InputManager.IsKeyPressed(Keys.Y))
            {
                level.ActivateEvent(0, TriggerState.Triggered);
            }
            

            switch (playerIndex)
            {
                case PlayerIndex.One:
                    player1.stopInput = false;
                    player2.stopInput = true;
                    player1.texture = player1.regularTexture;
                    player2.texture = player2.indicatorTexture;
                    break;
                case PlayerIndex.Two:
                    player1.stopInput = true;
                    player2.stopInput = false;
                    player1.texture = player1.indicatorTexture;
                    player2.texture = player2.regularTexture;
                    break;
            }

            player1.Update();
            player2.Update();


            #region Iterations

            foreach (PickUpObj pickUp in pickUpList)
            {
                pickUp.Update();
                player1.CheckForGrab(pickUp);
                player2.CheckForGrab(pickUp);
            }

            foreach (MovingPlatform platform in platformList)
            {
                player1.AdjustCollision(platform);
                player2.AdjustCollision(platform);
                foreach (PickUpObj pickUp in pickUpList)
                {
                    pickUp.AdjustCollision(platform);
                }
                foreach (Moveable move in moveList)
                {
                    move.AdjustCollision(platform);
                }
                platform.Update();
            }

            foreach (CircularPlatform platform in cPlatformList)
            {
                platform.Update();
                player1.AdjustCollision(platform);
                player2.AdjustCollision(platform);
                foreach (PickUpObj pickUp in pickUpList)
                {
                    pickUp.AdjustCollision(platform);
                }
                foreach (Moveable move in moveList)
                {
                    move.AdjustCollision(platform);
                }
                
            }

            foreach (Button button in buttonList)
            {
                player1.AdjustCollision(button);
                player2.AdjustCollision(button);
                foreach (PickUpObj pickUp in pickUpList)
                {
                    pickUp.AdjustCollision(button);
                }
                foreach (Moveable move in moveList)
                {
                    move.AdjustCollision(button);
                }
                button.Update();
            }

            foreach (FlipSwitch s in switchList)
            {
                s.Update();
                player1.CheckForPress(s);
                player2.CheckForPress(s);
            }

            foreach (TriggerArea area in areaList)
            {
                player1.CheckForArea(area);
                player2.CheckForArea(area);
                area.Update();
            }

            foreach (Portal portal in portalList)
            {
                portal.Update();
            }

            foreach (LightConsole console in consoleList)
            {
                console.Update();
                player1.CheckLightConsole(console);
                player2.CheckLightConsole(console);
            }

            foreach (Door door in doorList)
            {
                door.Update();
                player1.AdjustCollision(door);
                player2.AdjustCollision(door);
                foreach (PickUpObj pickUp in pickUpList) pickUp.AdjustCollision(door);
                foreach (Moveable move in moveList) move.AdjustCollision(door);
            }
            foreach (Moveable move in moveList)
            {
                move.Update();
                player1.CheckForPush(move);
                player2.CheckForPush(move);
                foreach (PickUpObj pickUp in pickUpList) pickUp.AdjustCollision(move);
                foreach (Door door in doorList) move.AdjustCollision(door);
            }
            foreach (Moveable move in moveList)
            {
                foreach (Moveable move2 in moveList) if (move != move2) move.AdjustToMoveable(move2);
            }
            //foreach (Moveable move in moveList)
            //{
            //    move.AdjustToPlayer(player1);
            //    move.AdjustToPlayer(player2);
            //}

            foreach (ParticleEmitter emitter in emitterList)
            {
                emitter.Update();
            }
            foreach (TimerObject timer in timerList)
            {
                timer.Update();
            }

            Particle[] tempParticles = new Particle[particleList.Count];
            particleList.CopyTo(tempParticles);
            foreach (Particle particle in tempParticles)
            {
                particle.Update();
                if (particle.isDead) particleList.Remove(particle);
            }

            foreach (PointLight light in lightList)
            {
                light.Update();
            }

            foreach (ParallaxLayer layer in bgLayerList)
            {
                foreach (SpriteIMG bg in layer.imageList)
                {
                    bg.Update();
                }
            }

            #endregion

            if (playerIndex == PlayerIndex.One) alphaDot.position = player2.position;
            else alphaDot.position = player1.position;
           

            cameraPlayer1.Update();
            cameraPlayer2.Update();
        }

        public void DrawWithCamera(SpriteBatch spriteBatch, Camera camera)
        {
            

            gameStateManager.game.GraphicsDevice.SetRenderTarget(alphaMask);
            gameStateManager.game.GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.transform);
            alphaDot.Draw(spriteBatch);
            spriteBatch.End();

            gameStateManager.game.GraphicsDevice.SetRenderTarget(lightMask);
            gameStateManager.game.GraphicsDevice.Clear(level.atmosphereLight);
            //spriteBatch.Begin();
            ////spriteBatch.Draw(blackSquare, new Vector2(0, 0), new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight), Color.White);
            //spriteBatch.End();
            colorShader.Parameters["DestColor"].SetValue(Color.White.ToVector4());
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, colorShader, camera.transform);
            if (player1.selfIlluminating) player1.Draw(spriteBatch);
            else
            {
                colorShader.Parameters["DestColor"].SetValue(new Color(50,50,50).ToVector4());
                player1.Draw(spriteBatch);
                colorShader.Parameters["DestColor"].SetValue(Color.White.ToVector4());

            }
            if (player2.selfIlluminating) player2.Draw(spriteBatch);
            else
            {
                colorShader.Parameters["DestColor"].SetValue(new Color(50, 50, 50).ToVector4());
                player2.Draw(spriteBatch);
                colorShader.Parameters["DestColor"].SetValue(Color.White.ToVector4());

            }
            foreach (PointLight light in lightList) if (light.playerVisible == PlayerObjectMode.None) light.Draw(spriteBatch);
            if (playerIndex == PlayerIndex.One)
            {
                foreach (SpriteIMG tile in tileList) if (tile.selfIlluminating && (tile.playerVisible == PlayerObjectMode.None || tile.playerVisible == PlayerObjectMode.One)) tile.Draw(spriteBatch);
                foreach (FlipSwitch s in switchList) if (s.selfIlluminating && (s.playerVisible == PlayerObjectMode.None || s.playerVisible == PlayerObjectMode.One)) s.Draw(spriteBatch);
                foreach (Button button in buttonList) if (button.selfIlluminating && (button.playerVisible == PlayerObjectMode.None || button.playerVisible == PlayerObjectMode.One)) button.Draw(spriteBatch);
                foreach (TriggerArea area in areaList) if (area.selfIlluminating && (area.playerVisible == PlayerObjectMode.None || area.playerVisible == PlayerObjectMode.One)) area.Draw(spriteBatch);
                foreach (LightConsole console in consoleList) if (console.selfIlluminating && (console.playerVisible == PlayerObjectMode.None || console.playerVisible == PlayerObjectMode.One)) console.Draw(spriteBatch);
                foreach (Door door in doorList) if (door.selfIlluminating && (door.playerVisible == PlayerObjectMode.None || door.playerVisible == PlayerObjectMode.One)) door.Draw(spriteBatch);
                foreach (MovingPlatform platform in platformList) if (platform.selfIlluminating && (platform.playerVisible == PlayerObjectMode.None || platform.playerVisible == PlayerObjectMode.One)) platform.Draw(spriteBatch);
                foreach (CircularPlatform platform in cPlatformList) if (platform.selfIlluminating && (platform.playerVisible == PlayerObjectMode.None || platform.playerVisible == PlayerObjectMode.One)) platform.Draw(spriteBatch);
                foreach (Moveable move in moveList) if (move.selfIlluminating && (move.playerVisible == PlayerObjectMode.None || move.playerVisible == PlayerObjectMode.One)) move.Draw(spriteBatch);
                foreach (PickUpObj pickUp in pickUpList) if (pickUp.selfIlluminating && (pickUp.playerVisible == PlayerObjectMode.None || pickUp.playerVisible == PlayerObjectMode.One)) pickUp.Draw(spriteBatch);
                foreach (Portal portal in portalList) if (portal.selfIlluminating && (portal.playerVisible == PlayerObjectMode.None || portal.playerVisible == PlayerObjectMode.One)) portal.Draw(spriteBatch);
                foreach (PointLight light in lightList) if (light.playerVisible == PlayerObjectMode.One) light.Draw(spriteBatch);
                
            }
            else
            {
                foreach (SpriteIMG tile in tileList) if (tile.selfIlluminating && (tile.playerVisible == PlayerObjectMode.None || tile.playerVisible == PlayerObjectMode.Two)) tile.Draw(spriteBatch);
                foreach (FlipSwitch s in switchList) if (s.selfIlluminating && (s.playerVisible == PlayerObjectMode.None || s.playerVisible == PlayerObjectMode.Two)) s.Draw(spriteBatch);
                foreach (Button button in buttonList) if (button.selfIlluminating && (button.playerVisible == PlayerObjectMode.None && button.playerVisible == PlayerObjectMode.Two)) button.Draw(spriteBatch);
                foreach (TriggerArea area in areaList) if (area.selfIlluminating && (area.playerVisible == PlayerObjectMode.None || area.playerVisible == PlayerObjectMode.Two)) area.Draw(spriteBatch);
                foreach (LightConsole console in consoleList) if (console.selfIlluminating && (console.playerVisible == PlayerObjectMode.None || console.playerVisible == PlayerObjectMode.Two)) console.Draw(spriteBatch);
                foreach (Door door in doorList) if (door.selfIlluminating && (door.playerVisible == PlayerObjectMode.None || door.playerVisible == PlayerObjectMode.Two)) door.Draw(spriteBatch);
                foreach (MovingPlatform platform in platformList) if (platform.selfIlluminating && (platform.playerVisible == PlayerObjectMode.None || platform.playerVisible == PlayerObjectMode.Two)) platform.Draw(spriteBatch);
                foreach (CircularPlatform platform in cPlatformList) if (platform.selfIlluminating && (platform.playerVisible == PlayerObjectMode.None || platform.playerVisible == PlayerObjectMode.Two)) platform.Draw(spriteBatch);
                foreach (Moveable move in moveList) if (move.selfIlluminating && (move.playerVisible == PlayerObjectMode.None || move.playerVisible == PlayerObjectMode.Two)) move.Draw(spriteBatch);
                foreach (PickUpObj pickUp in pickUpList) if (pickUp.selfIlluminating && (pickUp.playerVisible == PlayerObjectMode.None || pickUp.playerVisible == PlayerObjectMode.Two)) pickUp.Draw(spriteBatch);
                foreach (Portal portal in portalList) if (portal.selfIlluminating && (portal.playerVisible == PlayerObjectMode.None || portal.playerVisible == PlayerObjectMode.Two)) portal.Draw(spriteBatch);
                foreach (PointLight light in lightList) if (light.playerVisible == PlayerObjectMode.Two) light.Draw(spriteBatch);
            }

            spriteBatch.End();

            gameStateManager.game.GraphicsDevice.SetRenderTarget(auraTarget);
            if (player1.psyHold)
            {
                gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
                colorShader.Parameters["DestColor"].SetValue(player1.auraColor.ToVector4());
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, colorShader);
                player1.DrawAura(spriteBatch, auraTargetOrigin);
                spriteBatch.End();
                player1.auraTexture = gaussianBlur.PerformGaussianBlur(auraTarget, blurX, blurY, spriteBatch);
            }
            else player1.auraTexture = null;

            gameStateManager.game.GraphicsDevice.SetRenderTarget(player2Aura);
            if (player2.psyHold)
            {
                gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
                colorShader.Parameters["DestColor"].SetValue(player2.auraColor.ToVector4());
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, colorShader);
                player2.DrawAura(spriteBatch, auraTargetOrigin);
                spriteBatch.End();
                player2.auraTexture = gaussianBlur.PerformGaussianBlur(player2Aura, blurX2, blurY2, spriteBatch);
            }
            else player2.auraTexture = null;

            
            foreach (PickUpObj pickUp in pickUpList)
            {
                if (pickUp.psyHold)
                {
                    GaussianTargets aurTex = pickUpAuras[pickUpList.IndexOf(pickUp)];
                    gameStateManager.game.GraphicsDevice.SetRenderTarget(aurTex.target);
                    gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
                    colorShader.Parameters["DestColor"].SetValue(pickUp.auraColor.ToVector4());
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, colorShader);
                    pickUp.DrawAura(spriteBatch, auraTargetOrigin);
                    spriteBatch.End();
                    pickUp.auraTexture = gaussianBlur.PerformGaussianBlur(aurTex.target, aurTex.blurX, aurTex.blurY, spriteBatch);
                }
                else pickUp.auraTexture = null;
            }

            foreach (Moveable move in moveList)
            {
                if (move.psyHold)
                {
                    GaussianTargets aurTex = moveAuras[moveList.IndexOf(move)];
                    gameStateManager.game.GraphicsDevice.SetRenderTarget(aurTex.target);
                    gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
                    colorShader.Parameters["DestColor"].SetValue(move.auraColor.ToVector4());
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, colorShader);
                    move.DrawAura(spriteBatch, auraTargetOrigin);
                    spriteBatch.End();
                    move.auraTexture = gaussianBlur.PerformGaussianBlur(aurTex.target, aurTex.blurX, aurTex.blurY, spriteBatch);
                    
                }
                else move.auraTexture = null;
            }
            
            gameStateManager.game.GraphicsDevice.SetRenderTarget(renderTarget);
            gameStateManager.game.GraphicsDevice.Clear(Color.SlateBlue); //TODO: change here
            spriteBatch.Begin();
            spriteBatch.Draw(bgTarget, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();
            //colorShader.Parameters["DestColor"].SetValue(Color.White.ToVector4());
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, camera.transform);
            foreach (SpriteIMG tile in tileList) if (tile.playerVisible == PlayerObjectMode.None) tile.Draw(spriteBatch);
            foreach (FlipSwitch s in switchList) if (s.playerVisible == PlayerObjectMode.None) s.Draw(spriteBatch);
            foreach (Button button in buttonList) if (button.playerVisible == PlayerObjectMode.None) button.Draw(spriteBatch);
            foreach (TriggerArea area in areaList) if (area.playerVisible == PlayerObjectMode.None) area.Draw(spriteBatch);
            foreach (LightConsole console in consoleList) if (console.playerVisible == PlayerObjectMode.None) console.Draw(spriteBatch);
            foreach (Door door in doorList) if (door.playerVisible == PlayerObjectMode.None) door.Draw(spriteBatch);
            foreach (MovingPlatform platform in platformList) if (platform.playerVisible == PlayerObjectMode.None) platform.Draw(spriteBatch);
            foreach (CircularPlatform platform in cPlatformList) if (platform.playerVisible == PlayerObjectMode.None) platform.Draw(spriteBatch);
            foreach (Moveable move in moveList) if (move.playerVisible == PlayerObjectMode.None) move.Draw(spriteBatch);
            foreach (Portal portal in portalList) if (portal.playerVisible == PlayerObjectMode.None) portal.Draw(spriteBatch);
            foreach (Particle particle in particleList) if (particle.playerVisible == PlayerObjectMode.None) particle.Draw(spriteBatch);

            if (playerIndex == PlayerIndex.One) player2.Draw(spriteBatch);
            //spriteBatch.End();

            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            //spriteBatch.Draw(player1BlurTex, new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight), Color.White);
            //spriteBatch.End();

            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, camera.transform);
            player1.Draw(spriteBatch);
            if (playerIndex == PlayerIndex.Two) player2.Draw(spriteBatch);
            foreach (PickUpObj pickUp in pickUpList) if (pickUp.playerVisible == PlayerObjectMode.None) pickUp.Draw(spriteBatch);
            spriteBatch.End();

            alphaShader.Parameters["MaskTexture"].SetValue(alphaMask);
            
            gameStateManager.game.GraphicsDevice.SetRenderTarget(player1Objects);
            gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, camera.transform);
            foreach (SpriteIMG tile in tileList) if (tile.playerVisible == PlayerObjectMode.One) tile.Draw(spriteBatch);
            foreach (FlipSwitch s in switchList) if (s.playerVisible == PlayerObjectMode.One) s.Draw(spriteBatch);
            foreach (Button button in buttonList) if (button.playerVisible == PlayerObjectMode.One) button.Draw(spriteBatch);
            foreach (TriggerArea area in areaList) if (area.playerVisible == PlayerObjectMode.One) area.Draw(spriteBatch);
            foreach (LightConsole console in consoleList) if (console.playerVisible == PlayerObjectMode.One) console.Draw(spriteBatch);
            foreach (Door door in doorList) if (door.playerVisible == PlayerObjectMode.One) door.Draw(spriteBatch);
            foreach (MovingPlatform platform in platformList) if (platform.playerVisible == PlayerObjectMode.One) platform.Draw(spriteBatch);
            foreach (CircularPlatform platform in cPlatformList) if (platform.playerVisible == PlayerObjectMode.One) platform.Draw(spriteBatch);
            foreach (Moveable move in moveList) if (move.playerVisible == PlayerObjectMode.One) move.Draw(spriteBatch);
            foreach (PickUpObj pickUp in pickUpList) if (pickUp.playerVisible == PlayerObjectMode.One) pickUp.Draw(spriteBatch);
            spriteBatch.End();

            

            gameStateManager.game.GraphicsDevice.SetRenderTarget(player2Objects);
            gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, camera.transform);
            foreach (SpriteIMG tile in tileList) if (tile.playerVisible == PlayerObjectMode.Two) tile.Draw(spriteBatch);
            foreach (FlipSwitch s in switchList) if (s.playerVisible == PlayerObjectMode.Two) s.Draw(spriteBatch);
            foreach (Button button in buttonList) if (button.playerVisible == PlayerObjectMode.Two) button.Draw(spriteBatch);
            foreach (TriggerArea area in areaList) if (area.playerVisible == PlayerObjectMode.Two) area.Draw(spriteBatch);
            foreach (LightConsole console in consoleList) if (console.playerVisible == PlayerObjectMode.Two) console.Draw(spriteBatch);
            foreach (Door door in doorList) if (door.playerVisible == PlayerObjectMode.Two) door.Draw(spriteBatch);
            foreach (MovingPlatform platform in platformList) if (platform.playerVisible == PlayerObjectMode.Two) platform.Draw(spriteBatch);
            foreach (CircularPlatform platform in cPlatformList) if (platform.playerVisible == PlayerObjectMode.Two) platform.Draw(spriteBatch);
            foreach (Moveable move in moveList) if (move.playerVisible == PlayerObjectMode.Two) move.Draw(spriteBatch);
            foreach (PickUpObj pickUp in pickUpList) if (pickUp.playerVisible == PlayerObjectMode.Two) pickUp.Draw(spriteBatch);
            spriteBatch.End();

            if (playerIndex == PlayerIndex.One)
            {

                gameStateManager.game.GraphicsDevice.SetRenderTarget(alphaPlayer);
                gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, alphaShader);
                spriteBatch.Draw(player2Objects, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.End();

                gameStateManager.game.GraphicsDevice.SetRenderTarget(nonAlphaPlayer);
                gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin();
                spriteBatch.Draw(player1Objects, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.End();
            }
            else
            {
                gameStateManager.game.GraphicsDevice.SetRenderTarget(alphaPlayer);
                gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, alphaShader);
                spriteBatch.Draw(player1Objects, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.End();

                gameStateManager.game.GraphicsDevice.SetRenderTarget(nonAlphaPlayer);
                gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin();
                spriteBatch.Draw(player2Objects, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.End();
            }

            gameStateManager.game.GraphicsDevice.SetRenderTarget(finalTarget);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(alphaPlayer, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(nonAlphaPlayer, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();



            gameStateManager.game.GraphicsDevice.SetRenderTarget(null);
            gameStateManager.game.GraphicsDevice.Clear(Color.CornflowerBlue);
            lightingShader.Parameters["lightMask"].SetValue(lightMask);
            lightingShader.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, lightingShader);
            SpriteEffects mirror = SpriteEffects.None;
            if (playerIndex == PlayerIndex.Two) mirror = SpriteEffects.FlipHorizontally;
            
            spriteBatch.Draw(finalTarget, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, mirror, 0);
            spriteBatch.End();
        }

        public void DrawParallaxLayers(SpriteBatch spriteBatch, Camera camera)
        {
            gameStateManager.game.GraphicsDevice.SetRenderTarget(bgTarget);
            gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
            foreach (ParallaxLayer bg in bgLayerList)
            {

                camera.parallaxRatio = bg.parallaxRatio;
                camera.UpdateMatrixValues();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, camera.transform);
                foreach (SpriteIMG img in bg.imageList)
                {
                    img.Draw(spriteBatch);
                }
                spriteBatch.End();
            }
            camera.parallaxRatio = 1;
            camera.UpdateMatrixValues();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isLoading)
            {
                gameStateManager.game.GraphicsDevice.SetRenderTarget(null);
                if (okayToDraw)
                {
                    gameStateManager.game.GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Begin();
                    loadingScreen.Draw(spriteBatch);
                    spriteBatch.End();
                    
                }
                gameStateManager.game.GraphicsDevice.SetRenderTarget(null);
                return;
            }
            //gameStateManager.game.GraphicsDevice.SetRenderTarget(null);
            //gameStateManager.game.GraphicsDevice.Clear(Color.CornflowerBlue);
            if (playerIndex == PlayerIndex.One)
            {
                DrawParallaxLayers(spriteBatch, cameraPlayer1);
                DrawWithCamera(spriteBatch, cameraPlayer1);
            }
            else
            {
                DrawParallaxLayers(spriteBatch, cameraPlayer2);
                DrawWithCamera(spriteBatch, cameraPlayer2);
            }

            level.DrawItems(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
