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

//using Lidgren.Network;

namespace WorldsApart.Code.Gamestates
{
    class GSPlay : GameState
    {

        SpriteIMG verticalDivide;

        Viewport mainViewport;
        Viewport player1Viewport;
        Viewport player2Viewport;

        private Thread backgroundThread;
        private bool isLoading = false;
        private bool okayToDraw = true;

        Level level;

        public Camera cameraPlayer1;
        public Camera cameraPlayer2;
        public PlayerIndex playerIndex = PlayerIndex.One;

        public SpriteIMG alphaDot;
        Effect alphaShader;
        Effect lightingShader;
        Effect colorShader;
        Effect glowShader;
        Effect warpShader;
        RenderTarget2D alphaMask;
        RenderTarget2D lightMask;
        RenderTarget2D renderTarget;
        RenderTarget2D player1Objects;
        RenderTarget2D player2Objects;
        RenderTarget2D alphaPlayer;
        RenderTarget2D nonAlphaPlayer;
        RenderTarget2D finalTarget;
        RenderTarget2D bgTarget;
        RenderTarget2D lightningTarget;

        //RenderTarget2D alphaMask2;
        //RenderTarget2D lightMask2;
        //RenderTarget2D renderTarget2;
        //RenderTarget2D player1Objects2;
        //RenderTarget2D player2Objects2;
        //RenderTarget2D alphaPlayer2;
        //RenderTarget2D nonAlphaPlayer2;
        //RenderTarget2D finalTarget2;
        //RenderTarget2D bgTarget2;

        RenderTarget2D multiFinalTarget1;
        RenderTarget2D multiFinalTarget2;

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
        RenderTarget2D warpTarget;
        RenderTarget2D warpTarget2;

        SpriteIMG loadingScreen;
        Texture2D signalTexture;


        public Player player1;
        public Player player2;
        public List<SpriteIMG> backFGList = new List<SpriteIMG>();
        public List<SpriteIMG> frontFGList = new List<SpriteIMG>();
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
        static public List<Particle> particleList = new List<Particle>();
        static public List<Particle> bgParticleList = new List<Particle>();
        public List<ParticleEmitter> emitterList = new List<ParticleEmitter>();
        public List<ParallaxLayer> bgLayerList = new List<ParallaxLayer>();
        public List<PointLight> lightList = new List<PointLight>();
        public List<LightConsole> consoleList = new List<LightConsole>();
        public List<Portal> portalList = new List<Portal>();
        public List<LightningChain> lightningList = new List<LightningChain>();
        public List<PortalParticles> ppList = new List<PortalParticles>();


        public float dreamParticleCounter = 0;
        public float dreamParticleRate = .2f;

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

            mainViewport = gameStateManager.game.GraphicsDevice.Viewport;
            player1Viewport = mainViewport;
            player2Viewport = mainViewport;

            if (GameStateManager.isMultiplayer)
            {
                player1Viewport.Width = player1Viewport.Width / 2;
                player2Viewport.Width = player2Viewport.Width / 2;
                player1Viewport.X = 0;
                player2Viewport.X = player1Viewport.Width;

                verticalDivide = new SpriteIMG(LoadTexture("blackLine"), new Vector2(mainViewport.Width / 2, mainViewport.Height / 2));
                verticalDivide.scale = new Vector2(1, 32);
            }

            alphaDot = new SpriteIMG(LoadTexture("ShaderAssets/playerAlphaMask"));
            alphaDot.scale = new Vector2(2);

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
                case 50:
                    level = new theBridge(this);
                    break;
                case 99:
                    level = new FinalBridge(this);
                    break;
                default:
                    level = new TestLevel(this);
                    break;
            }

            
            
            
            alphaMask = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            renderTarget = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            player1Objects = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            player2Objects = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            alphaPlayer = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            nonAlphaPlayer = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            finalTarget = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);

            //alphaMask2 = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            //renderTarget2 = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            //player1Objects2 = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            //player2Objects2 = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            //alphaPlayer2 = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            //nonAlphaPlayer2 = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            //finalTarget2 = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            //lightMask2 = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            //bgTarget2 = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            multiFinalTarget1 = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            multiFinalTarget2 = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            alphaShader = LoadEffect("ShaderAssets/AlphaMapping");
            lightingShader = LoadEffect("ShaderAssets/Lighting");
            colorShader = LoadEffect("ShaderAssets/ColorTransform");
            glowShader = LoadEffect("ShaderAssets/StrokeGlow");
            warpShader = LoadEffect("ShaderAssets/Warp");
            lightMask = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            auraTarget = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            auraTargetOrigin = new Vector2(Game1.screenWidth / 2, Game1.screenHeight / 2);
            player2Aura = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            bgTarget = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            lightningTarget = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            warpTarget = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);
            warpTarget2 = new RenderTarget2D(gameStateManager.game.GraphicsDevice, Game1.screenWidth, Game1.screenHeight);

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


            List<IdealAnimationSet> player1Textures = new List<IdealAnimationSet>();
            player1.idleSet = new IdealAnimationSet(player1.regularTexture, player1.indicatorTexture, 1, 1, 8, 8, 6, 8);
            player1Textures.Add(player1.idleSet);
            player1.runningLeadSet = new IdealAnimationSet(player1.regularTexture, player1.indicatorTexture, 1, 8, 8, 8, 1, 5, new Point(1,8));
            player1Textures.Add(player1.runningLeadSet);
            player1.runningSet = new IdealAnimationSet(player1.regularTexture, player1.indicatorTexture, 2, 1, 8, 8, 8 ,8);
            player1Textures.Add(player1.runningSet);
            player1.runningEndSet = new IdealAnimationSet(player1.regularTexture, player1.indicatorTexture, 1, 8, 8, 8, 1, 5, new Point(1,8));
            player1Textures.Add(player1.runningEndSet);
            player1.jumpingUpLeadSet = new IdealAnimationSet(player1.regularTexture, player1.indicatorTexture, 5, 1, 8, 8, 2,5, new Point(5,1));
            player1Textures.Add(player1.jumpingUpLeadSet);
            player1.jumpingUpSet = new IdealAnimationSet(player1.regularTexture, player1.indicatorTexture, 5, 2, 8, 8, 4,8);
            player1Textures.Add(player1.jumpingUpSet);
            player1.jumpingDownLeadSet = new IdealAnimationSet(player1.regularTexture, player1.indicatorTexture, 5, 6, 8, 8, 4,5, new Point(5,6));
            player1Textures.Add(player1.jumpingDownLeadSet);
            player1.jumpingDownSet = new IdealAnimationSet(player1.regularTexture, player1.indicatorTexture, 5, 7, 8, 8, 4,8);
            player1Textures.Add(player1.jumpingDownSet);
            player1.jumpingDownEndSet = new IdealAnimationSet(player1.regularTexture, player1.indicatorTexture, 6, 3, 8, 8, 1,5, new Point(6,3));
            player1Textures.Add(player1.jumpingDownEndSet);
            player1.DyingSet = new IdealAnimationSet(player1.regularTexture, player1.indicatorTexture, 3, 1, 8, 8, 8,5, new Point(3,8));
            player1Textures.Add(player1.DyingSet);
            player1.RevivingSet = new IdealAnimationSet(player1.regularTexture, player1.indicatorTexture, 4, 1, 8, 8, 5,8, new Point(4,5)); 
            player1Textures.Add(player1.RevivingSet);

            List<IdealAnimationSet> player2Textures = new List<IdealAnimationSet>();
            player2.idleSet = new IdealAnimationSet(player2.regularTexture, player2.indicatorTexture, 1, 1, 8, 8, 6, 8);
            player2Textures.Add(player2.idleSet);
            player2.runningLeadSet = new IdealAnimationSet(player2.regularTexture, player2.indicatorTexture, 1, 8, 8, 8, 1, 5, new Point(1, 8));
            player2Textures.Add(player2.runningLeadSet);
            player2.runningSet = new IdealAnimationSet(player2.regularTexture, player2.indicatorTexture, 2, 1, 8, 8, 8, 8);
            player2Textures.Add(player2.runningSet);
            player2.runningEndSet = new IdealAnimationSet(player2.regularTexture, player2.indicatorTexture, 1, 8, 8, 8, 1, 5, new Point(1, 8));
            player2Textures.Add(player2.runningEndSet);
            player2.jumpingUpLeadSet = new IdealAnimationSet(player2.regularTexture, player2.indicatorTexture, 5, 1, 8, 8, 2, 5, new Point(5, 1));
            player2Textures.Add(player2.jumpingUpLeadSet);
            player2.jumpingUpSet = new IdealAnimationSet(player2.regularTexture, player2.indicatorTexture, 5, 2, 8, 8, 4, 8);
            player2Textures.Add(player2.jumpingUpSet);
            player2.jumpingDownLeadSet = new IdealAnimationSet(player2.regularTexture, player2.indicatorTexture, 5, 6, 8, 8, 4, 5, new Point(5, 6));
            player2Textures.Add(player2.jumpingDownLeadSet);
            player2.jumpingDownSet = new IdealAnimationSet(player2.regularTexture, player2.indicatorTexture, 5, 7, 8, 8, 4, 8);
            player2Textures.Add(player2.jumpingDownSet);
            player2.jumpingDownEndSet = new IdealAnimationSet(player2.regularTexture, player2.indicatorTexture, 6, 3, 8, 8, 1, 5, new Point(6, 3));
            player2Textures.Add(player2.jumpingDownEndSet);
            player2.DyingSet = new IdealAnimationSet(player2.regularTexture, player2.indicatorTexture, 3, 1, 8, 8, 8, 5, new Point(3, 8));
            player2Textures.Add(player2.DyingSet);
            player2.RevivingSet = new IdealAnimationSet(player2.regularTexture, player2.indicatorTexture, 4, 1, 8, 8, 5, 8, new Point(4, 5));
            player2Textures.Add(player2.RevivingSet);

            player1.showingRegular = true;
            player2.showingRegular = true;

            signalTexture = LoadTexture("signal");

            

            
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

            if (gameStateManager.goodness < -5) gameStateManager.goodness = -5;

            switch (gameStateManager.goodness)
            {
                case -1:
                    player1.indicatorAlpha = 204;
                    player2.indicatorAlpha = 204;
                    break;
                case -2:
                    player1.indicatorAlpha = 153;
                    player2.indicatorAlpha = 153;
                    break;
                case -3:
                    player1.indicatorAlpha = 102;
                    player2.indicatorAlpha = 102;
                    break;
                case -4:
                    player1.indicatorAlpha = 51;
                    player2.indicatorAlpha = 51;
                    break;
                case -5:
                    player1.indicatorAlpha = 0;
                    player2.indicatorAlpha = 0;
                    break;
                case -6:
                    player1.indicatorAlpha = 38;
                    player2.indicatorAlpha = 38;
                    break;
                case -7:
                    player1.indicatorAlpha = 0;
                    player2.indicatorAlpha = 0;
                    break;
                default:
                    player1.indicatorAlpha = 255;
                    player2.indicatorAlpha = 255;
                    break;
            }

            //RenderIdealTextures(player1Textures, player1.indicatorAlpha);

            //RenderIdealTextures(player2Textures, player2.indicatorAlpha);
        }

        //public void DrawLoadingScreen(SpriteBatch spriteBatch)
        //{
        //    gameStateManager.game.GraphicsDevice.SetRenderTarget(null);
        //    spriteBatch.Begin();
        //    loadingScreen.Draw(spriteBatch);
        //    spriteBatch.End();
        //}

        public void RenderIdealTextures(List<IdealAnimationSet> playerTextures, byte playerAlpha)
        {
            List<Texture2D> usedTextures = new List<Texture2D>();
            List<Texture2D> completedTextures = new List<Texture2D>();

            foreach (IdealAnimationSet set in playerTextures)
            {
                if (usedTextures.Contains(set.idealTexture))
                {
                    set.idealTexture = completedTextures[usedTextures.IndexOf(set.idealTexture)];
                    return;
                }

                RenderTarget2D playerTexture = new RenderTarget2D(GraphicsDevice, set.texture.Width, set.texture.Height);
                GraphicsDevice.SetRenderTarget(playerTexture);
                GraphicsDevice.Clear(Color.Transparent);
                gameStateManager.game.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                if (gameStateManager.goodness > 0 && gameStateManager.goodness < 4)
                {
                    gameStateManager.game.spriteBatch.Draw(set.idealTexture, Vector2.Zero, Color.White);
                    //gameStateManager.game.spriteBatch.Draw(set.texture, Vector2.Zero, new Color(0, 0, 0, playerAlpha));

                }
                else if (gameStateManager.goodness >= 4)
                {
                    //gameStateManager.game.spriteBatch.Draw(set.texture, Vector2.Zero, Color.White);
                    gameStateManager.game.spriteBatch.Draw(set.idealTexture, Vector2.Zero, Color.White);
                }
                else if (gameStateManager.goodness <= 0)
                {
                    byte c = playerAlpha;
                    gameStateManager.game.spriteBatch.Draw(set.idealTexture, Vector2.Zero, new Color(c, c, c));
                }
                gameStateManager.game.spriteBatch.End();

                usedTextures.Add(set.idealTexture);
                completedTextures.Add(playerTexture);
                set.idealTexture = playerTexture;
            }
        }

        #region Add Objects
        public void AddGroundTile(Vector2 tilePosition)
        {
            SpriteIMG tile = new SpriteIMG(LoadTexture("TestSprites/tile"), tilePosition);
            backFGList.Add(tile);

        }

        public SpriteIMG AddFrontFGTile(Texture2D texture, Vector2 position)
        {
            SpriteIMG tile = new SpriteIMG(texture, position);
            frontFGList.Add(tile);
            return tile;
        }

        public SpriteIMG AddBackBGTile(Texture2D texture, Vector2 position)
        {
            SpriteIMG tile = new SpriteIMG(texture, position);
            backFGList.Add(tile);
            return tile;
        }

        public void AddPickUp(Texture2D texture, Vector2 position, PlayerObjectMode playerMode)
        {
            PickUpObj obj = new PickUpObj(texture, position);
            obj.playerTangible = playerMode;
            obj.playerVisible = playerMode;
            obj.SetCollisionBox(22, 22, Vector2.Zero);
            pickUpList.Add(obj);
        }

        public PickUpObj AddBouncyBall(float bounceMultiplier, Texture2D texture, Vector2 position)
        {
            PickUpObj obj = new PickUpObj(bounceMultiplier, texture, position);
            pickUpList.Add(obj);
            pickUpAuras.Add(new GaussianTargets(gameStateManager.game.GraphicsDevice));
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

            SpriteIMG bBase = AddFrontFGTile(LoadTexture("TestSprites/buttonBase"), position);
            bBase.origin = button.origin;

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
            PointLight light = AddPointLight(LoadTexture("ShaderAssets/pointLight"), s.position, new Vector2(.1f));
            light.SetGlowing(.1f, .2f, 60);
            light.target = s;
            light.color = Color.Green;
            s.light = light;
            s.LightsOff();
            switchList.Add(s);
            return s;
        }

        public FlipSwitch AddOnSwitch(EventTrigger eventTrigger, Texture2D texture, Vector2 position)
        {
            FlipSwitch s = AddSwitch(eventTrigger, texture, position);
            s.defaultOn = true;
            s.LightsOn();
            return s;
        }

        public FlipSwitch AddMultiSwitch(EventTrigger eventTrigger, Texture2D texture, Vector2 position)
        {
            FlipSwitch s = AddSwitch(eventTrigger, texture, position);
            s.multiSwitch = true;
            s.LightsOff();
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

        public Particle AddAnimatedParticle(AnimatedSprite image, Vector2 position, Vector2 speed)
        {
            Particle p = new Particle(image.texture, position);
            p.SetAnimationStuff(image.minRow, image.minCol, image.rows, image.cols, image.cellW, image.cellH, image.frames, image.animationCounter);
            p.speed = speed;
            particleList.Add(p);
            return p;
        }

        public Particle AddParticle(Texture2D texture, Vector2 position)
        {
            Particle p = new Particle(texture, position);
            particleList.Add(p);
            return p;
        }

        public Particle AddAnimatedBGParticle(AnimatedSprite image, Vector2 position, Vector2 speed)
        {
            Particle p = new Particle(image.texture, position);
            p.SetAnimationStuff(image.minRow, image.minCol, image.rows, image.cols, image.cellW, image.cellH, image.frames, image.animationCounter);
            p.speed = speed;
            bgParticleList.Add(p);
            return p;
        }

        public Particle AddBGParticle(Texture2D texture, Vector2 position)
        {
            Particle p = new Particle(texture, position);
            bgParticleList.Add(p);
            return p;
        }

        public Particle AddRain(bool isPlayer1)
        {
            Vector2 position = Vector2.Zero;

            Player player = player1;
            if (!isPlayer1) player = player2;

            position = new Vector2(player.position.X + Mathness.RandomNumber(-Game1.screenWidth * 2, Game1.screenWidth * 2), player.position.Y - Game1.screenHeight + Mathness.RandomNumber(-100f, 100f));

            Particle p = new Particle(Art.rain, position);

            if (isPlayer1) p.SetPlayerMode(PlayerObjectMode.One);
            else p.SetPlayerMode(PlayerObjectMode.Two);

            p.speed = new Vector2(0, 12);
            p.scale = new Vector2(2);
            p.life = 240;
            particleList.Add(p);
            return p;

        }

        public Particle AddDreamSparkle(bool isPlayer1)
        {
            Particle p = new Particle(Art.sparkle, Vector2.Zero);
            p.speed = new Vector2(Mathness.RandomNumber(-2f, 2f), Mathness.RandomNumber(-2f, 2f));
            p.startScale = Mathness.RandomNumber(.5f, .8f);
            p.endScale = Mathness.RandomNumber(1.2f, 1.5f);
            p.rotationSpeed = Mathness.RandomNumber(.05f, .1f);
            //p.startScale = 1;
            //p.endScale = 1;
            p.fadeInOut = true;
            p.startAlpha = 0;
            p.endAlpha = 255;
            p.life = Mathness.RandomNumber(360, 480);
            p.alpha = 0;
            p.randomMinForce = new Vector2(-.1f);
            p.randomMaxForce = new Vector2(.1f);
            p.illuminatingAllTheTime = true;
            if (isPlayer1)
            {
                p.color = new Color(255, Mathness.RandomNumber(50, 150), Mathness.RandomNumber(50, 150));
                p.position = new Vector2(player1.position.X + Mathness.RandomNumber(-Game1.screenWidth, Game1.screenWidth), player1.position.Y + Mathness.RandomNumber(-Game1.screenHeight, Game1.screenHeight));
                p.SetPlayerMode(PlayerObjectMode.One);
            }
            else
            {
                p.color = new Color(Mathness.RandomNumber(50, 150), Mathness.RandomNumber(50, 150), 255);
                p.position = new Vector2(player2.position.X + Mathness.RandomNumber(-Game1.screenWidth, Game1.screenWidth), player2.position.Y + Mathness.RandomNumber(-Game1.screenHeight, Game1.screenHeight));
                p.SetPlayerMode(PlayerObjectMode.Two);
            }

            p.StartParticleSystems();

            particleList.Add(p);
            return p;

        }

        static public void AddObjectDeath(Vector2 position)
        {
            for (int i = 0; i < 8; i++)
            {
                Particle p = new Particle(Art.smokePuff, position);
                p.life = 30;
                p.speed = new Vector2(Mathness.RandomNumber(-1.5f, 1.5f), Mathness.RandomNumber(-1.5f, 1.5f));
                p.startAlpha = 255;
                p.endAlpha = 0;
                p.startScale = Mathness.RandomNumber(.8f, 1.2f);
                p.endScale = Mathness.RandomNumber(1.8f, 2.2f);
                p.StartParticleSystems();
                particleList.Add(p);
            }
        }

        public Particle AddSignal(Player player)
        {
            Particle p = new Particle(signalTexture, player.position);
            p.startScale = .2f;
            p.endScale = 4;
            p.startAlpha = 255;
            p.endAlpha = 0;
            p.target = player;
            p.life = 60;
            p.color = player.auraColor;
            p.illuminatingAllTheTime = true;
            p.StartParticleSystems();
            particleList.Add(p);
            return p;
        }

        public LightningChain AddLightning(Vector2 start, Vector2 end, Color color)
        {
            LightningChain l = new LightningChain(start, end, color);
            lightningList.Add(l);
            return l;
        }

        public ParticleEmitter AddEmitter(AnimatedSprite image, Vector2 position)
        {
            ParticleEmitter emitter = new ParticleEmitter(this, image, position);
            emitterList.Add(emitter);
            return emitter;
        }

        public CircularEmitter AddCircularEmitter(AnimatedSprite image, Vector2 position)
        {
            CircularEmitter emitter = new CircularEmitter(this, image, position);
            emitterList.Add(emitter);
            return emitter;
        }

        public ParticleEmitter AddWindEmitter(Vector2 position, Vector2 speed)
        {
            ParticleEmitter pe = AddEmitter(new AnimatedSprite(LoadTexture("TestSprites/particleWind")), position);
            pe.speed = speed;
            pe.randomRotation = true;
            pe.rotationSpeed = .2f;
            pe.fadeInOut = true;
            pe.randomDisplacement.X = 16;
            return pe;
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
            img.screenCull = false;
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

        public void AddFireParticle(Vector2 position)
        {
            AnimatedSprite particle2 = new AnimatedSprite(LoadTexture("GameObjects/orangeFire"), position);
            ParticleEmitter pe2 = AddEmitter(particle2, position);
            pe2.fadeInOut = true;
            pe2.startAlpha = 0;
            pe2.endAlpha = 128;
            pe2.startScale = 3;
            pe2.endScale = 4;
            pe2.spawnRate = 1;
            pe2.color = Color.Orange;
            pe2.randomDisplacement = new Vector2(10, 8);
            pe2.randomSpeedX = new Vector2(-.25f, .25f);
            pe2.randomSpeedY = new Vector2(-.6f, -.4f);
            pe2.life = 30;
            pe2.selfIlluminated = true;


            AnimatedSprite particle = new AnimatedSprite(LoadTexture("GameObjects/whiteFire"), position);
            ParticleEmitter pe = AddEmitter(particle, position);
            pe.startAlpha = 0;
            pe.endAlpha = 255;
            pe.fadeInOut = true;
            pe.startScale = 2f;
            pe.endScale = 2.5f;
            pe.spawnRate = 40;
            pe.life = 180;
            pe.color = new Color(255, 242, 93);
            pe.randomDisplacement = new Vector2(5, 5);
            pe.selfIlluminated = true;

            
        }

        public void AddSpinningFireParticle(Vector2 position)
        {
            float angle = Mathness.RandomNumber(0, (float)Math.PI * 2);

            AnimatedSprite particle2 = new AnimatedSprite(LoadTexture("GameObjects/orangeFire"), position);
            CircularEmitter pe2 = AddCircularEmitter(particle2, position);
            pe2.fadeInOut = true;
            pe2.startAlpha = 0;
            pe2.endAlpha = 128;
            pe2.startScale = 3;
            pe2.endScale = 4;
            pe2.spawnRate = 1;
            pe2.color = Color.Orange;
            pe2.randomDisplacement = new Vector2(10, 8);
            pe2.randomSpeedX = new Vector2(-.25f, .25f);
            pe2.randomSpeedY = new Vector2(-.6f, -.4f);
            pe2.life = 30;
            pe2.selfIlluminated = true;
            pe2.angle = angle;

            //AnimatedSprite particle3 = new AnimatedSprite(LoadTexture("GameObjects/orangeFire"), position);
            //CircularEmitter pe3 = AddCircularEmitter(particle3, position);
            //pe3.fadeInOut = true;
            //pe3.startAlpha = 0;
            //pe3.endAlpha = 128;
            //pe3.startScale = 3;
            //pe3.endScale = 4;
            //pe3.spawnRate = 1;
            //pe3.color = Color.Orange;
            //pe3.randomDisplacement = new Vector2(10, 8);
            //pe3.randomSpeedX = new Vector2(-.25f, .25f);
            //pe3.randomSpeedY = new Vector2(-.6f, -.4f);
            //pe3.life = 30;
            //pe3.selfIlluminated = true;
            //pe3.angle = (float)Math.PI;

            AnimatedSprite particle = new AnimatedSprite(LoadTexture("GameObjects/whiteFire"), position);
            CircularEmitter pe = AddCircularEmitter(particle, position);
            pe.startAlpha = 0;
            pe.endAlpha = 255;
            pe.fadeInOut = true;
            pe.startScale = 2f;
            pe.endScale = 2.5f;
            pe.spawnRate = 5;
            pe.life = 40;
            pe.color = new Color(255, 242, 93);
            pe.randomDisplacement = new Vector2(5, 5);
            pe.selfIlluminated = true;
            pe.angle = angle;

            //AnimatedSprite particle4 = new AnimatedSprite(LoadTexture("GameObjects/whiteFire"), position);
            //CircularEmitter pe4 = AddCircularEmitter(particle4, position);
            //pe4.startAlpha = 0;
            //pe4.endAlpha = 255;
            //pe4.fadeInOut = true;
            //pe4.startScale = 2f;
            //pe4.endScale = 2.5f;
            //pe4.spawnRate = 5;
            //pe4.life = 40;
            //pe4.color = new Color(255, 242, 93);
            //pe4.randomDisplacement = new Vector2(5, 5);
            //pe4.selfIlluminated = true;
            //pe4.angle = (float)Math.PI;
        }

        #endregion

        public void ShiftPlayer()
        {
            if (playerIndex == PlayerIndex.One) playerIndex = PlayerIndex.Two;
            else playerIndex = PlayerIndex.One;
        }

        public void GetInput()
        {
            if (stopInput)
            {
                player1.superStopInput = true;
                player2.superStopInput = true;
                return;
            }

            if (!GameStateManager.isMultiplayer)
            {
                if (InputManager.IsButtonPressed(Buttons.Back) || InputManager.IsKeyPressed(Keys.LeftShift))
                {
                    ShiftPlayer();
                }
            }


            if (InputManager.IsKeyPressed(Keys.Y))
            {
                level.ActivateEvent(0, TriggerState.Triggered);
            }

            //if (InputManager.IsKeyPressed(Keys.P))
            //{
            //    if (player1.am.pauseMovement)
            //        player1.am.pauseMovement = false;
            //    else
            //        player1.am.Pause();
            //}

            if (InputManager.IsKeyPressed(Keys.Enter) || InputManager.IsButtonPressed(Buttons.Start))
            {
                gameStateManager.SwitchToGSPause();
            }

            
        }

        public void PauseInput()
        {
            stopInput = true;
        }

        public void UnpauseInput()
        {
            stopInput = false;
            player1.superStopInput = false;
            player2.superStopInput = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (isLoading) return;
            if (paused) return;

            base.Update(gameTime);

            GetInput();

            if (!GameStateManager.isMultiplayer)
            {
                switch (playerIndex)
                {
                    case PlayerIndex.One:
                        player1.stopInput = false;
                        player2.stopInput = true;

                        break;
                    case PlayerIndex.Two:
                        player1.stopInput = true;
                        player2.stopInput = false;
                        break;
                }
            }
            else
            {
                player1.stopInput = false;
                player2.stopInput = false;
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

            foreach (PortalParticles pp in ppList)
            {
                pp.Update();
            }

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

            tempParticles = new Particle[bgParticleList.Count];
            bgParticleList.CopyTo(tempParticles);
            foreach (Particle particle in tempParticles)
            {
                particle.Update();
                if (particle.isDead) bgParticleList.Remove(particle);
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
            foreach (LightningChain lightning in lightningList)
            {
                lightning.Update();
            }
            foreach (SpriteIMG img in frontFGList) img.Update();

            #endregion


            dreamParticleCounter += Time.GetSeconds();

            if (dreamParticleCounter >= dreamParticleRate)
            {
                AddDreamSparkle(true);
                AddDreamSparkle(false);
                dreamParticleCounter = 0;
            }


            
            

            cameraPlayer1.Update();
            cameraPlayer2.Update();
        }

        public void DrawWithCamera(SpriteBatch spriteBatch, Camera camera)
        {

            #region Set Regular Showing
            if (playerIndex == PlayerIndex.One)
            {
                player1.showingRegular = true; 
                player2.showingRegular = false; 
            }
            else
            {
                player1.showingRegular = false;
                player2.showingRegular = true;
            }
            #endregion

            #region Create Alpha Mask
            if (playerIndex == PlayerIndex.One) alphaDot.position = player2.position;
            else alphaDot.position = player1.position;

            //if (playerIndex == PlayerIndex.One) 
            gameStateManager.game.GraphicsDevice.SetRenderTarget(alphaMask);
            //else gameStateManager.game.GraphicsDevice.SetRenderTarget(alphaMask2);
            gameStateManager.game.GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.transform);
            alphaDot.Draw(spriteBatch, camera);
            spriteBatch.End();
            #endregion

            #region Aura drawing
            gameStateManager.game.GraphicsDevice.SetRenderTarget(auraTarget);
            if (player1.psyHold)
            {
                colorShader.Parameters["DestColor"].SetValue(player1.auraColor.ToVector4());
                gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, colorShader);
                player1.DrawAura(spriteBatch, auraTargetOrigin);
                spriteBatch.End();

                //warpShader.Parameters["resolution"].SetValue(new Vector2(500, 500));
                //warpShader.Parameters["ringSize"].SetValue(1);
                warpShader.Parameters["magnitude"].SetValue(.02f);
                warpShader.Parameters["scale"].SetValue(player1.auraScale);

                player1.auraTexture = gaussianBlur.PerformGaussianBlur(auraTarget, blurX, blurY, spriteBatch);

                gameStateManager.game.GraphicsDevice.SetRenderTarget(warpTarget);
                gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, warpShader);
                spriteBatch.Draw(player1.auraTexture, Vector2.Zero, Color.White);
                spriteBatch.End();

                player1.auraTexture = warpTarget;
                //gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
                //colorShader.Parameters["DestColor"].SetValue(player1.auraColor.ToVector4());
                //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, colorShader);
                //player1.DrawAura(spriteBatch, auraTargetOrigin);
                //spriteBatch.End();
                //player1.auraTexture = gaussianBlur.PerformGaussianBlur(auraTarget, blurX, blurY, spriteBatch);
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

                //warpShader.Parameters["resolution"].SetValue(new Vector2(500, 500));
                //warpShader.Parameters["ringSize"].SetValue(1);
                warpShader.Parameters["magnitude"].SetValue(.02f);
                warpShader.Parameters["scale"].SetValue(player2.auraScale);

                player2.auraTexture = gaussianBlur.PerformGaussianBlur(player2Aura, blurX2, blurY2, spriteBatch);

                gameStateManager.game.GraphicsDevice.SetRenderTarget(warpTarget2);
                gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, warpShader);
                spriteBatch.Draw(player2.auraTexture, Vector2.Zero, Color.White);
                spriteBatch.End();

                player2.auraTexture = warpTarget2;
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

                    //warpShader.Parameters["resolution"].SetValue(new Vector2(500, 500));
                    //warpShader.Parameters["ringSize"].SetValue(1);
                    warpShader.Parameters["magnitude"].SetValue(.02f);
                    warpShader.Parameters["scale"].SetValue(pickUp.auraScale);

                    pickUp.auraTexture = gaussianBlur.PerformGaussianBlur(aurTex.target, aurTex.blurX, aurTex.blurY, spriteBatch);

                    gameStateManager.game.GraphicsDevice.SetRenderTarget(aurTex.warpTarget);
                    gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, warpShader);
                    spriteBatch.Draw(pickUp.auraTexture, Vector2.Zero, Color.White);
                    spriteBatch.End();

                    pickUp.auraTexture = aurTex.warpTarget;
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

                    //warpShader.Parameters["resolution"].SetValue(new Vector2(500, 500));
                    //warpShader.Parameters["ringSize"].SetValue(1);
                    warpShader.Parameters["magnitude"].SetValue(.02f);
                    warpShader.Parameters["scale"].SetValue(move.auraScale);

                    move.auraTexture = gaussianBlur.PerformGaussianBlur(aurTex.target, aurTex.blurX, aurTex.blurY, spriteBatch);

                    gameStateManager.game.GraphicsDevice.SetRenderTarget(aurTex.warpTarget);
                    gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, warpShader);
                    spriteBatch.Draw(move.auraTexture, Vector2.Zero, Color.White);
                    spriteBatch.End();

                    move.auraTexture = aurTex.warpTarget;

                }
                else move.auraTexture = null;
            }
            #endregion

            #region Create Light pass
            //if (playerIndex == PlayerIndex.One) 
            gameStateManager.game.GraphicsDevice.SetRenderTarget(lightMask);
            //else gameStateManager.game.GraphicsDevice.SetRenderTarget(lightMask2);
            gameStateManager.game.GraphicsDevice.Clear(level.atmosphereLight);
            //spriteBatch.Begin();
            ////spriteBatch.Draw(blackSquare, new Vector2(0, 0), new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight), Color.White);
            //spriteBatch.End();
            colorShader.Parameters["DestColor"].SetValue(Color.White.ToVector4());
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, colorShader, camera.transform);
            if (player1.selfIlluminating)
            {
                spriteBatch.End();
                colorShader.Parameters["DestColor"].SetValue(Color.White.ToVector4());
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, colorShader, camera.transform);
                player1.Draw(spriteBatch, camera);
            }
            else
            {
                spriteBatch.End();
                colorShader.Parameters["DestColor"].SetValue(new Color(50, 50, 50).ToVector4());
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, colorShader, camera.transform);
                player1.Draw(spriteBatch, camera);

            }
            if (player2.selfIlluminating)
            {
                spriteBatch.End();
                colorShader.Parameters["DestColor"].SetValue(Color.White.ToVector4());
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, colorShader, camera.transform);
                player2.Draw(spriteBatch, camera);
            }
            else
            {
                spriteBatch.End();
                colorShader.Parameters["DestColor"].SetValue(new Color(50, 50, 50).ToVector4());
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, colorShader, camera.transform);
                player2.Draw(spriteBatch, camera);

            }
            foreach (PointLight light in lightList) if (light.playerVisible == PlayerObjectMode.None)
            {
                spriteBatch.End();
                colorShader.Parameters["DestColor"].SetValue(light.color.ToVector4());
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, colorShader, camera.transform);
                light.Draw(spriteBatch, camera);
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, colorShader, camera.transform);
            colorShader.Parameters["DestColor"].SetValue(Color.White.ToVector4());
            if (playerIndex == PlayerIndex.One)
            {
                foreach (SpriteIMG tile in backFGList) if (tile.selfIlluminating && (tile.playerVisible == PlayerObjectMode.None || tile.playerVisible == PlayerObjectMode.One)) tile.Draw(spriteBatch, camera);
                foreach (FlipSwitch s in switchList) if (s.selfIlluminating && (s.playerVisible == PlayerObjectMode.None || s.playerVisible == PlayerObjectMode.One)) s.Draw(spriteBatch, camera);
                foreach (Button button in buttonList) if (button.selfIlluminating && (button.playerVisible == PlayerObjectMode.None || button.playerVisible == PlayerObjectMode.One)) button.Draw(spriteBatch, camera);
                foreach (TriggerArea area in areaList) if (area.selfIlluminating && (area.playerVisible == PlayerObjectMode.None || area.playerVisible == PlayerObjectMode.One)) area.Draw(spriteBatch, camera);
                foreach (LightConsole console in consoleList) if (console.selfIlluminating && (console.playerVisible == PlayerObjectMode.None || console.playerVisible == PlayerObjectMode.One)) console.Draw(spriteBatch, camera);
                foreach (Door door in doorList) if (door.selfIlluminating && (door.playerVisible == PlayerObjectMode.None || door.playerVisible == PlayerObjectMode.One)) door.Draw(spriteBatch, camera);
                foreach (MovingPlatform platform in platformList) if (platform.selfIlluminating && (platform.playerVisible == PlayerObjectMode.None || platform.playerVisible == PlayerObjectMode.One)) platform.Draw(spriteBatch, camera);
                foreach (CircularPlatform platform in cPlatformList) if (platform.selfIlluminating && (platform.playerVisible == PlayerObjectMode.None || platform.playerVisible == PlayerObjectMode.One)) platform.Draw(spriteBatch, camera);
                foreach (Moveable move in moveList) if (move.selfIlluminating && (move.playerVisible == PlayerObjectMode.None || move.playerVisible == PlayerObjectMode.One)) move.Draw(spriteBatch, camera);
                foreach (PickUpObj pickUp in pickUpList) if (pickUp.selfIlluminating && (pickUp.playerVisible == PlayerObjectMode.None || pickUp.playerVisible == PlayerObjectMode.One)) pickUp.Draw(spriteBatch, camera);
                foreach (Portal portal in portalList) if (portal.selfIlluminating && (portal.playerVisible == PlayerObjectMode.None || portal.playerVisible == PlayerObjectMode.One)) portal.Draw(spriteBatch, camera);
                foreach (SpriteIMG tile in frontFGList) if (tile.selfIlluminating && (tile.playerVisible == PlayerObjectMode.None || tile.playerVisible == PlayerObjectMode.One)) tile.Draw(spriteBatch, camera);
                foreach (PointLight light in lightList) if (light.playerVisible == PlayerObjectMode.One) light.Draw(spriteBatch, camera);
                spriteBatch.End();
               
                foreach (Particle particle in particleList) if (particle.selfIlluminating && (particle.playerVisible == PlayerObjectMode.One || particle.playerVisible == PlayerObjectMode.None))
                {
                    colorShader.Parameters["DestColor"].SetValue(particle.color.ToVector4());
                    spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, null, null, null, null, camera.transform);
                    particle.Draw(spriteBatch, camera);
                    spriteBatch.End();
                }
                
                spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, null, null, null, null, camera.transform);
                foreach (LightningChain lightning in lightningList) if (lightning.playerObjectMode == PlayerObjectMode.One || lightning.playerObjectMode == PlayerObjectMode.None) lightning.Draw(spriteBatch, camera);
                spriteBatch.End();

            }
            else
            {
                foreach (SpriteIMG tile in backFGList) if (tile.selfIlluminating && (tile.playerVisible == PlayerObjectMode.None || tile.playerVisible == PlayerObjectMode.Two)) tile.Draw(spriteBatch, camera);
                foreach (FlipSwitch s in switchList) if (s.selfIlluminating && (s.playerVisible == PlayerObjectMode.None || s.playerVisible == PlayerObjectMode.Two)) s.Draw(spriteBatch, camera);
                foreach (Button button in buttonList) if (button.selfIlluminating && (button.playerVisible == PlayerObjectMode.None && button.playerVisible == PlayerObjectMode.Two)) button.Draw(spriteBatch, camera);
                foreach (TriggerArea area in areaList) if (area.selfIlluminating && (area.playerVisible == PlayerObjectMode.None || area.playerVisible == PlayerObjectMode.Two)) area.Draw(spriteBatch, camera);
                foreach (LightConsole console in consoleList) if (console.selfIlluminating && (console.playerVisible == PlayerObjectMode.None || console.playerVisible == PlayerObjectMode.Two)) console.Draw(spriteBatch, camera);
                foreach (Door door in doorList) if (door.selfIlluminating && (door.playerVisible == PlayerObjectMode.None || door.playerVisible == PlayerObjectMode.Two)) door.Draw(spriteBatch, camera);
                foreach (MovingPlatform platform in platformList) if (platform.selfIlluminating && (platform.playerVisible == PlayerObjectMode.None || platform.playerVisible == PlayerObjectMode.Two)) platform.Draw(spriteBatch, camera);
                foreach (CircularPlatform platform in cPlatformList) if (platform.selfIlluminating && (platform.playerVisible == PlayerObjectMode.None || platform.playerVisible == PlayerObjectMode.Two)) platform.Draw(spriteBatch, camera);
                foreach (Moveable move in moveList) if (move.selfIlluminating && (move.playerVisible == PlayerObjectMode.None || move.playerVisible == PlayerObjectMode.Two)) move.Draw(spriteBatch, camera);
                foreach (PickUpObj pickUp in pickUpList) if (pickUp.selfIlluminating && (pickUp.playerVisible == PlayerObjectMode.None || pickUp.playerVisible == PlayerObjectMode.Two)) pickUp.Draw(spriteBatch, camera);
                foreach (Portal portal in portalList) if (portal.selfIlluminating && (portal.playerVisible == PlayerObjectMode.None || portal.playerVisible == PlayerObjectMode.Two)) portal.Draw(spriteBatch, camera);
                foreach (SpriteIMG tile in frontFGList) if (tile.selfIlluminating && (tile.playerVisible == PlayerObjectMode.None || tile.playerVisible == PlayerObjectMode.Two)) tile.Draw(spriteBatch, camera);
                foreach (PointLight light in lightList) if (light.playerVisible == PlayerObjectMode.Two) light.Draw(spriteBatch, camera);
                spriteBatch.End();

                foreach (Particle particle in particleList) if (particle.selfIlluminating && (particle.playerVisible == PlayerObjectMode.Two || particle.playerVisible == PlayerObjectMode.None))
                {
                    colorShader.Parameters["DestColor"].SetValue(particle.color.ToVector4());
                    spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, null, null, null, null, camera.transform);
                    particle.Draw(spriteBatch, camera);
                    spriteBatch.End();
                }

                spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, null, null, null, null, camera.transform);
                foreach (LightningChain lightning in lightningList) if (lightning.playerObjectMode == PlayerObjectMode.Two || lightning.playerObjectMode == PlayerObjectMode.None) lightning.Draw(spriteBatch, camera);
                spriteBatch.End();
            }
    
            #endregion

            #region Player neutral drawing
            //if (playerIndex == PlayerIndex.One) 
            gameStateManager.game.GraphicsDevice.SetRenderTarget(renderTarget);
            //else gameStateManager.game.GraphicsDevice.SetRenderTarget(renderTarget2);
            gameStateManager.game.GraphicsDevice.Clear(Color.SlateBlue); //TODO: change here for background color
            spriteBatch.Begin();
            //if (playerIndex == PlayerIndex.One) 
            spriteBatch.Draw(bgTarget, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            //else spriteBatch.Draw(bgTarget2, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();
            //colorShader.Parameters["DestColor"].SetValue(Color.White.ToVector4());
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, camera.transform);
            foreach (SpriteIMG tile in backFGList) if (tile.playerVisible == PlayerObjectMode.None) tile.Draw(spriteBatch, camera);
            foreach (Particle particle in bgParticleList) if (particle.playerVisible == PlayerObjectMode.None) particle.Draw(spriteBatch, camera);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, null, null, null, null, camera.transform);
            if (playerIndex == PlayerIndex.One)
            {
                foreach (LightningChain lightning in lightningList)
                    if (lightning.playerObjectMode == PlayerObjectMode.None || lightning.playerObjectMode == PlayerObjectMode.One) lightning.Draw(spriteBatch, camera);
            }
            else
            {
                foreach (LightningChain lightning in lightningList)
                    if (lightning.playerObjectMode == PlayerObjectMode.None || lightning.playerObjectMode == PlayerObjectMode.Two) lightning.Draw(spriteBatch, camera);
            }
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, camera.transform);
           
            foreach (FlipSwitch s in switchList) if (s.playerVisible == PlayerObjectMode.None) s.Draw(spriteBatch, camera);
            foreach (Button button in buttonList) if (button.playerVisible == PlayerObjectMode.None) button.Draw(spriteBatch, camera);
            foreach (TriggerArea area in areaList) if (area.playerVisible == PlayerObjectMode.None) area.Draw(spriteBatch, camera);
            foreach (LightConsole console in consoleList)
            {
                if (console.playerVisible == PlayerObjectMode.None)
                {
                    if (console.light == null) console.Draw(spriteBatch, camera);
                    else
                    {
                        spriteBatch.End();
                        colorShader.Parameters["DestColor"].SetValue(Color.White.ToVector4());
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, colorShader, camera.transform);
                        console.Draw(spriteBatch, camera);
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, camera.transform);
                    }
                }
            }
            foreach (Door door in doorList) if (door.playerVisible == PlayerObjectMode.None) door.Draw(spriteBatch, camera);
            foreach (MovingPlatform platform in platformList) if (platform.playerVisible == PlayerObjectMode.None) platform.Draw(spriteBatch, camera);
            foreach (CircularPlatform platform in cPlatformList) if (platform.playerVisible == PlayerObjectMode.None) platform.Draw(spriteBatch, camera);
            foreach (Moveable move in moveList) if (move.playerVisible == PlayerObjectMode.None) move.Draw(spriteBatch, camera);
            foreach (Portal portal in portalList) if (portal.playerVisible == PlayerObjectMode.None) portal.Draw(spriteBatch, camera);
            foreach (Particle particle in particleList) if (particle.playerVisible == PlayerObjectMode.None) particle.Draw(spriteBatch, camera);

            if (playerIndex == PlayerIndex.One)
            {
                if (player2.light == null) player2.Draw(spriteBatch, camera); 
                else
                {
                    spriteBatch.End();
                    colorShader.Parameters["DestColor"].SetValue(Color.White.ToVector4());
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, colorShader, camera.transform);
                    player2.Draw(spriteBatch, camera);
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, camera.transform);

                }
            }

            if (player1.light == null) player1.Draw(spriteBatch, camera);
            else
            {
                spriteBatch.End();
                colorShader.Parameters["DestColor"].SetValue(Color.White.ToVector4());
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, colorShader, camera.transform);
                player1.Draw(spriteBatch, camera);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, camera.transform);
            }

            if (playerIndex == PlayerIndex.Two)
            {
                if (player2.light == null) player2.Draw(spriteBatch, camera);
                else
                {
                    spriteBatch.End();
                    colorShader.Parameters["DestColor"].SetValue(Color.White.ToVector4());
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, colorShader, camera.transform);
                    player2.Draw(spriteBatch, camera);
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, camera.transform);
                }
            }

            foreach (PickUpObj pickUp in pickUpList) if (pickUp.playerVisible == PlayerObjectMode.None) pickUp.Draw(spriteBatch, camera);
            foreach (SpriteIMG tile in frontFGList) if (tile.playerVisible == PlayerObjectMode.None) tile.Draw(spriteBatch, camera);
            spriteBatch.End();
            #endregion

            #region Player 1 Objects

            //if (playerIndex == PlayerIndex.One) 
            gameStateManager.game.GraphicsDevice.SetRenderTarget(player1Objects);
            //else gameStateManager.game.GraphicsDevice.SetRenderTarget(player1Objects2);
            gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, camera.transform);
            foreach (SpriteIMG tile in backFGList) if (tile.playerVisible == PlayerObjectMode.One) tile.Draw(spriteBatch, camera);
            foreach (FlipSwitch s in switchList) if (s.playerVisible == PlayerObjectMode.One) s.Draw(spriteBatch, camera);
            foreach (Button button in buttonList) if (button.playerVisible == PlayerObjectMode.One) button.Draw(spriteBatch, camera);
            foreach (TriggerArea area in areaList) if (area.playerVisible == PlayerObjectMode.One) area.Draw(spriteBatch, camera);
            foreach (LightConsole console in consoleList)
            {
                if (console.playerVisible == PlayerObjectMode.One)
                {
                    if (console.light == null) console.Draw(spriteBatch, camera);
                    else
                    {
                        spriteBatch.End();
                        colorShader.Parameters["DestColor"].SetValue(Color.White.ToVector4());
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, colorShader, camera.transform);
                        console.Draw(spriteBatch, camera);
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, camera.transform);
                    }
                }
            }
            foreach (Door door in doorList) if (door.playerVisible == PlayerObjectMode.One) door.Draw(spriteBatch, camera);
            foreach (MovingPlatform platform in platformList) if (platform.playerVisible == PlayerObjectMode.One) platform.Draw(spriteBatch, camera);
            foreach (CircularPlatform platform in cPlatformList) if (platform.playerVisible == PlayerObjectMode.One) platform.Draw(spriteBatch, camera);
            foreach (Moveable move in moveList) if (move.playerVisible == PlayerObjectMode.One) move.Draw(spriteBatch, camera);
            foreach (PickUpObj pickUp in pickUpList) if (pickUp.playerVisible == PlayerObjectMode.One) pickUp.Draw(spriteBatch, camera);
            foreach (SpriteIMG tile in frontFGList) if (tile.playerVisible == PlayerObjectMode.One) tile.Draw(spriteBatch, camera);
            foreach (Particle particle in particleList) if (particle.playerVisible == PlayerObjectMode.One) particle.Draw(spriteBatch, camera);
            spriteBatch.End();
            #endregion

            #region Player 2 Objects
            //if (playerIndex == PlayerIndex.One) 
            gameStateManager.game.GraphicsDevice.SetRenderTarget(player2Objects);
            //else gameStateManager.game.GraphicsDevice.SetRenderTarget(player2Objects2);
            gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, camera.transform);
            foreach (SpriteIMG tile in backFGList) if (tile.playerVisible == PlayerObjectMode.Two) tile.Draw(spriteBatch, camera);
            foreach (FlipSwitch s in switchList) if (s.playerVisible == PlayerObjectMode.Two) s.Draw(spriteBatch, camera);
            foreach (Button button in buttonList) if (button.playerVisible == PlayerObjectMode.Two) button.Draw(spriteBatch, camera);
            foreach (TriggerArea area in areaList) if (area.playerVisible == PlayerObjectMode.Two) area.Draw(spriteBatch, camera);
            foreach (LightConsole console in consoleList)
            {
                if (console.playerVisible == PlayerObjectMode.Two)
                {
                    if (console.light == null) console.Draw(spriteBatch, camera);
                    else
                    {
                        spriteBatch.End();
                        colorShader.Parameters["DestColor"].SetValue(Color.White.ToVector4());
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, colorShader, camera.transform);
                        console.Draw(spriteBatch, camera);
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, camera.transform);
                    }
                }
            }
            foreach (Door door in doorList) if (door.playerVisible == PlayerObjectMode.Two) door.Draw(spriteBatch, camera);
            foreach (MovingPlatform platform in platformList) if (platform.playerVisible == PlayerObjectMode.Two) platform.Draw(spriteBatch, camera);
            foreach (CircularPlatform platform in cPlatformList) if (platform.playerVisible == PlayerObjectMode.Two) platform.Draw(spriteBatch, camera);
            foreach (Moveable move in moveList) if (move.playerVisible == PlayerObjectMode.Two) move.Draw(spriteBatch, camera);
            foreach (PickUpObj pickUp in pickUpList) if (pickUp.playerVisible == PlayerObjectMode.Two) pickUp.Draw(spriteBatch, camera);
            foreach (SpriteIMG tile in frontFGList) if (tile.playerVisible == PlayerObjectMode.Two) tile.Draw(spriteBatch, camera);
            foreach (Particle particle in particleList) if (particle.playerVisible == PlayerObjectMode.Two) particle.Draw(spriteBatch, camera);
            spriteBatch.End();
            #endregion

            #region AlphaMaskingPlayerObjects
            alphaShader.Parameters["MaskTexture"].SetValue(alphaMask);
            if (playerIndex == PlayerIndex.One)
            {

                gameStateManager.game.GraphicsDevice.SetRenderTarget(alphaPlayer);
                gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, alphaShader);
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
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, alphaShader); 
                spriteBatch.Draw(player1Objects, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.End();

                gameStateManager.game.GraphicsDevice.SetRenderTarget(nonAlphaPlayer);
                gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin();
                spriteBatch.Draw(player2Objects, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.End();
            }
            #endregion

            #region Objects Composite
            //if (playerIndex == PlayerIndex.One) 
            gameStateManager.game.GraphicsDevice.SetRenderTarget(finalTarget);
            //else gameStateManager.game.GraphicsDevice.SetRenderTarget(finalTarget2);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            //if (playerIndex == PlayerIndex.One)
            //{
                spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.Draw(alphaPlayer, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.Draw(nonAlphaPlayer, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            //}
            //else
            //{
            //    spriteBatch.Draw(renderTarget2, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            //    spriteBatch.Draw(alphaPlayer2, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            //    spriteBatch.Draw(nonAlphaPlayer2, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            //}
            spriteBatch.End();
            #endregion

            #region Light pass and mirror pass


            if (GameStateManager.isMultiplayer)
            {
                if (playerIndex == PlayerIndex.One)
                {
                    gameStateManager.game.GraphicsDevice.SetRenderTarget(multiFinalTarget1);
                    gameStateManager.game.GraphicsDevice.Viewport = player1Viewport;
                }
                else
                {
                    gameStateManager.game.GraphicsDevice.SetRenderTarget(multiFinalTarget2);
                    gameStateManager.game.GraphicsDevice.Viewport = player2Viewport;
                }
            }
            else gameStateManager.game.GraphicsDevice.SetRenderTarget(null);
           

            gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
            //if (playerIndex == PlayerIndex.One) 
            lightingShader.Parameters["lightMask"].SetValue(lightMask);
            //else lightingShader.Parameters["lightMask"].SetValue(lightMask2);
            lightingShader.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, null, null, lightingShader); //Alpha testing
            SpriteEffects mirror = SpriteEffects.None;
            if (playerIndex == PlayerIndex.Two) mirror = SpriteEffects.FlipHorizontally;

            Vector2 drawPos = Vector2.Zero;
            Rectangle srcRect = finalTarget.Bounds;

            if (GameStateManager.isMultiplayer)
            {

                drawPos.X = -player1Viewport.Width / 2;

                //srcRect = new Rectangle(finalTarget.Width / 4, 0, finalTarget.Width / 2, finalTarget.Height);
                
            }

            //if (playerIndex == PlayerIndex.One) 
            spriteBatch.Draw(finalTarget, drawPos, null, Color.White, 0, Vector2.Zero, 1, mirror, 0);
            //else spriteBatch.Draw(finalTarget2, drawPos, null, Color.White, 0, Vector2.Zero, 1, mirror, 0);
            spriteBatch.End();
            #endregion
        }

        public void DrawParallaxLayers(SpriteBatch spriteBatch, Camera camera)
        {
            //if (playerIndex == PlayerIndex.One) 
            gameStateManager.game.GraphicsDevice.SetRenderTarget(bgTarget);
            //else gameStateManager.game.GraphicsDevice.SetRenderTarget(bgTarget2);
            gameStateManager.game.GraphicsDevice.Clear(Color.Transparent);
            foreach (ParallaxLayer bg in bgLayerList)
            {

                camera.parallaxRatio = bg.parallaxRatio;
                camera.UpdateMatrixValues();
                GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, null, null, null, camera.transform);
                foreach (SpriteIMG img in bg.imageList)
                {
                    img.Draw(spriteBatch, camera);
                }
                spriteBatch.End();
            }
            camera.parallaxRatio = 1;
            camera.UpdateMatrixValues();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            #region Loading screen maybe
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
            #endregion
            //gameStateManager.game.GraphicsDevice.SetRenderTarget(null);
            //gameStateManager.game.GraphicsDevice.Clear(Color.CornflowerBlue);

            if (GameStateManager.isMultiplayer)
            {
                #region Multiplayer
                gameStateManager.game.GraphicsDevice.SetRenderTarget(null);
                gameStateManager.game.GraphicsDevice.Viewport = mainViewport;

                playerIndex = PlayerIndex.One;
                //gameStateManager.game.GraphicsDevice.Viewport = player1Viewport;
                DrawParallaxLayers(spriteBatch, cameraPlayer1);
                DrawWithCamera(spriteBatch, cameraPlayer1);

                //gameStateManager.game.GraphicsDevice.SetRenderTarget(null);
                //gameStateManager.game.GraphicsDevice.Viewport = mainViewport;

                playerIndex = PlayerIndex.Two;
                //gameStateManager.game.GraphicsDevice.Viewport = player2Viewport;
                DrawParallaxLayers(spriteBatch, cameraPlayer2);
                DrawWithCamera(spriteBatch, cameraPlayer2);

                gameStateManager.game.GraphicsDevice.SetRenderTarget(null);
                gameStateManager.game.GraphicsDevice.Viewport = mainViewport;

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                spriteBatch.Draw(multiFinalTarget2, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.Draw(multiFinalTarget1, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                verticalDivide.Draw(spriteBatch);
                spriteBatch.End();

                
                #endregion
            }
            else
            {
                #region Single Player
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
                #endregion
            }

            level.DrawItems(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
