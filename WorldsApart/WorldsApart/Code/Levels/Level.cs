using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using WorldsApart.Code.Graphics;
using WorldsApart.Code.Controllers;
using WorldsApart.Code.Entities;
using WorldsApart.Code.Gamestates;

using System.Diagnostics;

namespace WorldsApart.Code.Levels
{
    class Level
    {
        protected GSPlay gsPlay;
        public Texture2D levelDataTexture;
        static public Point levelGridSize = Point.Zero;
        static public float collisionSize = 32;
        static public float levelWidth;
        static public float levelHeight;
        static public float leftLimit = 0;
        static public float rightLimit = 0;
        public static EnvironmentData[,] environmentData;
        public List<InventoryItem> itemList = new List<InventoryItem>();

        public bool renderCollision = false;

        public Vector2 player1Pos = Vector2.Zero;
        public Vector2 player2Pos = Vector2.Zero;
        public Vector2 portalPos = Vector2.Zero;
        public Vector2 pItemPos = Vector2.Zero;

        public Color atmosphereLight = Color.White;

        static public float deathHeight = 0;

        static public SpriteIMG player1Checkpoint;
        static public SpriteIMG player2Checkpoint;

        Portal glados;
        public bool hasPortal = true;

        public Level(GSPlay gsPlay)
        {
            this.gsPlay = gsPlay;
        }

        public void SetupLevel()
        {
            Color[] color1D = new Color[levelDataTexture.Width * levelDataTexture.Height];
            levelDataTexture.GetData(color1D);
            Color[,] colorData = new Color[levelDataTexture.Width, levelDataTexture.Height];
            for (int x = 0; x < levelDataTexture.Width; x++)
                for (int y = 0; y < levelDataTexture.Height; y++)
                    colorData[x, y] = color1D[x + y * levelDataTexture.Width];

            levelWidth = levelDataTexture.Width * collisionSize;
            levelHeight = levelDataTexture.Height * collisionSize;
            deathHeight = levelHeight;
            levelGridSize = new Point(levelDataTexture.Width, levelDataTexture.Height);
            environmentData = new EnvironmentData[levelDataTexture.Width, levelDataTexture.Height];
            for (int x = 0; x < levelDataTexture.Width; x++)
            {
                for (int y = 0; y < levelDataTexture.Height; y++)
                {
                    if (colorData[x, y] == Color.Black)
                    {
                        environmentData[x, y] = new EnvironmentData(true);
                        if (renderCollision)
                        {
                            SpriteIMG tile = new SpriteIMG(gsPlay.LoadTexture("TestSprites/tile"), GridToPosition(new Point(x, y)));
                            tile.origin = Vector2.Zero;
                            gsPlay.backFGList.Add(tile);
                            gsPlay.AddGroundTile(GridToPosition(new Point(x, y)) + new Vector2(16, 16));
                        }
                    }
                    else if (colorData[x, y] == new Color(0, 0, 255))
                    {
                        environmentData[x, y] = new EnvironmentData(FrictionType.Ice);
                        SpriteIMG tile = new SpriteIMG(gsPlay.LoadTexture("TestSprites/tile"), GridToPosition(new Point(x, y)));
                        tile.origin = Vector2.Zero;
                        tile.color = new Color(0, 0, 255);
                        gsPlay.backFGList.Add(tile);
                    }
                    else if (colorData[x, y] == new Color(0, 255, 0))
                    {
                        environmentData[x, y] = new EnvironmentData(new Vector2(0, -1f));
                        gsPlay.AddWindEmitter(GridToPosition(x, y) + new Vector2(0, 16), new Vector2(0, -2));
                        //SpriteIMG tile = new SpriteIMG(gsPlay.LoadTexture("TestSprites/airCurrentUp"), GridToPosition(new Point(x, y)));
                        //tile.origin = Vector2.Zero;
                        //tile.alpha = 128;
                        //gsPlay.tileList.Add(tile);
                    }
                    else if (colorData[x, y] == new Color(255, 0, 0))
                    {
                        environmentData[x, y] = new EnvironmentData(new Vector2(0, 1f));
                        gsPlay.AddWindEmitter(GridToPosition(x, y) + new Vector2(0, 16), new Vector2(0, 2));
                        //SpriteIMG tile = new SpriteIMG(gsPlay.LoadTexture("TestSprites/airCurrentDown"), GridToPosition(new Point(x, y)));
                        //tile.origin = Vector2.Zero;
                        //tile.alpha = 128;
                        //gsPlay.tileList.Add(tile);
                    }
                    else if (colorData[x, y] == new Color(255, 255, 0))
                    {
                        environmentData[x, y] = new EnvironmentData(new Vector2(4, 0));
                        gsPlay.AddWindEmitter(GridToPosition(x, y) + new Vector2(0, 16), new Vector2(2, 0));
                        //SpriteIMG tile = new SpriteIMG(gsPlay.LoadTexture("TestSprites/airCurrentRight"), GridToPosition(new Point(x, y)));
                        //tile.origin = Vector2.Zero;
                        //tile.alpha = 128;
                        //gsPlay.tileList.Add(tile);
                    }
                    else if (colorData[x, y] == new Color(0, 255, 255))
                    {
                        environmentData[x, y] = new EnvironmentData(new Vector2(-4, 0));
                        gsPlay.AddWindEmitter(GridToPosition(x, y) + new Vector2(0, 16), new Vector2(-2, 0));
                        //SpriteIMG tile = new SpriteIMG(gsPlay.LoadTexture("TestSprites/airCurrentLeft"), GridToPosition(new Point(x, y)));
                        //tile.origin = Vector2.Zero;
                        //tile.alpha = 128;
                        //gsPlay.tileList.Add(tile);
                    }
                    else if (colorData[x, y] == new Color(255, 255, 150))
                    {
                        environmentData[x, y] = new EnvironmentData(InclineType.ThreeTileRight1, FrictionType.Normal);
                        if (renderCollision)
                        {
                            SpriteIMG tile = new SpriteIMG(gsPlay.LoadTexture("TestSprites/tileInclineThreeRight1"), GridToPosition(new Point(x, y)));
                            tile.origin = Vector2.Zero;
                            gsPlay.backFGList.Add(tile);
                        }
                    }
                    else if (colorData[x, y] == new Color(255, 255, 100))
                    {
                        environmentData[x, y] = new EnvironmentData(InclineType.ThreeTileRight2, FrictionType.Normal);
                        if (renderCollision)
                        {
                            SpriteIMG tile = new SpriteIMG(gsPlay.LoadTexture("TestSprites/tileInclineThreeRight2"), GridToPosition(new Point(x, y)));
                            tile.origin = Vector2.Zero;
                            gsPlay.backFGList.Add(tile);
                        }
                    }
                    else if (colorData[x, y] == new Color(255, 255, 50))
                    {
                        environmentData[x, y] = new EnvironmentData(InclineType.ThreeTileRight3, FrictionType.Normal);
                        if (renderCollision)
                        {
                            SpriteIMG tile = new SpriteIMG(gsPlay.LoadTexture("TestSprites/tileInclineThreeRight3"), GridToPosition(new Point(x, y)));
                            tile.origin = Vector2.Zero;
                            gsPlay.backFGList.Add(tile);
                        }
                    }
                    else if (colorData[x, y] == new Color(50, 255, 255))
                    {
                        environmentData[x, y] = new EnvironmentData(InclineType.ThreeTileLeft1, FrictionType.Normal);
                        if (renderCollision)
                        {
                            SpriteIMG tile = new SpriteIMG(gsPlay.LoadTexture("TestSprites/tileInclineThreeLeft1"), GridToPosition(new Point(x, y)));
                            tile.origin = Vector2.Zero;
                            gsPlay.backFGList.Add(tile);
                        }
                    }
                    else if (colorData[x, y] == new Color(100, 255, 255))
                    {
                        environmentData[x, y] = new EnvironmentData(InclineType.ThreeTileLeft2, FrictionType.Normal);
                        if (renderCollision)
                        {
                            SpriteIMG tile = new SpriteIMG(gsPlay.LoadTexture("TestSprites/tileInclineThreeLeft2"), GridToPosition(new Point(x, y)));
                            tile.origin = Vector2.Zero;
                            gsPlay.backFGList.Add(tile);
                        }
                    }
                    else if (colorData[x, y] == new Color(150, 255, 255))
                    {
                        environmentData[x, y] = new EnvironmentData(InclineType.ThreeTileLeft3, FrictionType.Normal);
                        if (renderCollision)
                        {
                            SpriteIMG tile = new SpriteIMG(gsPlay.LoadTexture("TestSprites/tileInclineThreeLeft3"), GridToPosition(new Point(x, y)));
                            tile.origin = Vector2.Zero;
                            gsPlay.backFGList.Add(tile);
                        }
                    }
                    else if (colorData[x, y] == new Color(64, 255, 255))
                    {
                        environmentData[x, y] = new EnvironmentData(InclineType.TwoTileLeft1, FrictionType.Normal);
                        if (renderCollision)
                        {
                            SpriteIMG tile = new SpriteIMG(gsPlay.LoadTexture("TestSprites/tileInclineTwoLeft1"), GridToPosition(new Point(x, y)));
                            tile.origin = Vector2.Zero;
                            gsPlay.backFGList.Add(tile);
                        }
                    }
                    else if (colorData[x, y] == new Color(128, 255, 255))
                    {
                        environmentData[x, y] = new EnvironmentData(InclineType.TwoTileLeft2, FrictionType.Normal);
                        if (renderCollision)
                        {
                            SpriteIMG tile = new SpriteIMG(gsPlay.LoadTexture("TestSprites/tileInclineTwoLeft2"), GridToPosition(new Point(x, y)));
                            tile.origin = Vector2.Zero;
                            gsPlay.backFGList.Add(tile);
                        }
                    }
                    else if (colorData[x, y] == new Color(255, 255, 128))
                    {
                        environmentData[x, y] = new EnvironmentData(InclineType.TwoTileRight1, FrictionType.Normal);
                        if (renderCollision)
                        {
                            SpriteIMG tile = new SpriteIMG(gsPlay.LoadTexture("TestSprites/tileInclineTwoRight1"), GridToPosition(new Point(x, y)));
                            tile.origin = Vector2.Zero;
                            gsPlay.backFGList.Add(tile);
                        }
                    }
                    else if (colorData[x, y] == new Color(255, 255, 64))
                    {
                        environmentData[x, y] = new EnvironmentData(InclineType.TwoTileRight2, FrictionType.Normal);
                        if (renderCollision)
                        {
                            SpriteIMG tile = new SpriteIMG(gsPlay.LoadTexture("TestSprites/tileInclineTwoRight2"), GridToPosition(new Point(x, y)));
                            tile.origin = Vector2.Zero;
                            gsPlay.backFGList.Add(tile);
                        }
                    }
                    else if (colorData[x, y] == new Color(50, 50, 50))
                    {
                        environmentData[x, y] = new EnvironmentData(true);
                        environmentData[x, y].oneWayPlatform = true;
                        if (renderCollision)
                        {
                            SpriteIMG tile = new SpriteIMG(gsPlay.LoadTexture("TestSprites/jumpThru"), GridToPosition(new Point(x, y)));
                            tile.origin = Vector2.Zero;
                            gsPlay.backFGList.Add(tile);
                        }
                    }
                    else if (colorData[x, y] == new Color(255, 128, 255))
                    {
                        environmentData[x, y] = new EnvironmentData(false);
                        environmentData[x, y].checkpoint = true;
                        SpriteIMG tile = new SpriteIMG(gsPlay.LoadTexture("TestSprites/checkpoint"), GridToCenterPosition(new Point(x, y)));
                        tile.scale = new Vector2(1.5f);
                        gsPlay.backFGList.Add(tile);
                    }
                    else if (colorData[x, y] == new Color(255, 128, 0))
                    {
                        environmentData[x, y] = new EnvironmentData(false);
                        environmentData[x, y].killZone = true;
                        //SpriteIMG tile = new SpriteIMG(gsPlay.LoadTexture("TestSprites/killZone"), GridToPosition(new Point(x, y)));
                        //tile.origin = Vector2.Zero;
                        //gsPlay.backFGList.Add(tile);
                        //gsPlay.AddFireParticle(GridToCenterPosition(x, y));
                    }
                    else if (colorData[x, y] == new Color(255, 128, 1))
                    {
                        environmentData[x, y] = new EnvironmentData(false);
                        environmentData[x, y].killZone = true;
                        gsPlay.AddFireParticle(GridToCenterPosition(x, y));
                    }
                    else if (colorData[x, y] == new Color(255, 128, 2))
                    {
                        environmentData[x, y] = new EnvironmentData(false);
                        environmentData[x, y].killZone = true;
                        gsPlay.AddSpinningFireParticle(GridToCenterPosition(x, y));
                    }
                    else
                    {
                        environmentData[x, y] = new EnvironmentData(false);
                    }
                }
            }

            gsPlay.player1 = new Player(gsPlay, PlayerObjectMode.One, gsPlay.LoadTexture("player1"), player1Pos);
            gsPlay.player1.SetAnimationStuff(1, 1, 8, 8, 256, 256, 64, 5);
            gsPlay.player1.SetCollisionBox(48, 96, Vector2.Zero);
            gsPlay.player2 = new Player(gsPlay, PlayerObjectMode.Two, gsPlay.LoadTexture("player2"), player2Pos);
            gsPlay.player2.SetAnimationStuff(1, 1, 8, 8, 256, 256, 64, 5);
            gsPlay.player2.SetCollisionBox(48, 96, Vector2.Zero);

            player1Checkpoint = gsPlay.AddBackFGTile(gsPlay.LoadTexture("TestSprites/checkpointWarm"), new Vector2(-64, -64));
            player1Checkpoint.SetPlayerMode(PlayerObjectMode.One);
            player1Checkpoint.scale = new Vector2(1.5f);
            player1Checkpoint.illuminatingAllTheTime = true;
            player2Checkpoint = gsPlay.AddBackFGTile(gsPlay.LoadTexture("TestSprites/checkpointCool"), new Vector2(-64, -64));
            player2Checkpoint.SetPlayerMode(PlayerObjectMode.Two);
            player2Checkpoint.scale = new Vector2(1.5f);
            player2Checkpoint.illuminatingAllTheTime = true;
            
            if (hasPortal)
            {
                glados = gsPlay.AddPortal(new EventTrigger(this, 0), gsPlay.LoadTexture("TestSprites/portal"), portalPos);
                glados.SetAnimationStuff(2, 1, 2, 8, 96, 192, 8, 5);
                PortalItem goody = new PortalItem(new EventTrigger(this, glados), gsPlay.LoadTexture("TestSprites/Cursor"), pItemPos);
                gsPlay.areaList.Add(goody);
                goody.SetCollisionBox(32, 32, Vector2.Zero);
                goody.visible = false;
                GlyphEmitter ge = new GlyphEmitter(gsPlay, Art.portalItem1, pItemPos);
                ge.AddTexture(Art.portalItem2);
                ge.AddTexture(Art.portalItem3);
                ge.AddTexture(Art.portalItem4);
                ge.AddTexture(Art.portalItem5);
                ge.selfIlluminated = true;
                ge.startAlpha = 0;
                ge.endAlpha = 255;
                ge.fadeInOut = true;
                ge.startScale = .75f;
                ge.endScale = 1.25f;
                ge.spawnRate = 10;
                ge.life = 40;
                //ge.randomDisplacement = new Vector2(8, 8);
                ge.randomSpeedX = new Vector2(-.75f, .75f);
                ge.randomSpeedY = new Vector2(-.75f, .75f);
                gsPlay.emitterList.Add(ge);
                goody.emitter = ge;

                AnimatedSprite pulseSprite = new AnimatedSprite(Art.portalPulse);
                ParticleEmitter pulse = gsPlay.AddEmitter(pulseSprite, glados.position);
                pulse.bgParticles = true;
                pulse.color = new Color(128, 0, 0);
                pulse.startScale = 1;
                pulse.endScale = 2;
                pulse.startAlpha = 255;
                pulse.endAlpha = 0;
                pulse.randomRotation = false;
                pulse.rotationSpeed = 0;
                pulse.spawnRate = 30;
                pulse.life = 60;
                glados.pulse = pulse;

                LightningChain portalLightning = gsPlay.AddLightning(glados.position + new Vector2(0, -48), glados.position + new Vector2(20, -27), Color.Red);
                portalLightning.AddVertex(glados.position + new Vector2(20, 27));
                portalLightning.AddVertex(glados.position + new Vector2(0, 48));
                portalLightning.AddVertex(glados.position + new Vector2(-20, 27));
                portalLightning.AddVertex(glados.position + new Vector2(-20, -27));
                portalLightning.AddVertex(glados.position + new Vector2(0, -48));
                portalLightning.SetActive(true);
                portalLightning.defaultActive = true;
                goody.AddEvent(new EventTrigger(this, portalLightning));
                //40x55

                List<Particle> particleList = new List<Particle>();
                for (int i = 1; i <= 6; i++)
                {
                    Particle p = gsPlay.AddParticle(Art.sparkle, Vector2.Zero);
                    p.canDie = false;
                    if (i % 2 == 0) p.color = new Color(246, 133, 23);
                    else p.color = new Color(145, 127, 185);
                    p.alpha = 128;
                    particleList.Add(p);
                }
                PortalParticles pp = new PortalParticles(gsPlay, particleList);
                pp.AddVertex(glados.position + new Vector2(0, -53));
                pp.AddVertex(glados.position + new Vector2(25, -27));
                pp.AddVertex(glados.position + new Vector2(25, 27));
                pp.AddVertex(glados.position + new Vector2(0, 53));
                pp.AddVertex(glados.position + new Vector2(-25, 27));
                pp.AddVertex(glados.position + new Vector2(-25, -27));
                pp.SetUpParticles();
                pp.Deactivate();
                gsPlay.ppList.Add(pp);
                goody.AddEvent(new EventTrigger(this, pp));
                
            }

            gsPlay.cameraPlayer1 = new Camera(gsPlay.player1, gsPlay.player2, Vector2.Zero);
            gsPlay.cameraPlayer2 = new Camera(gsPlay.player2, gsPlay.player1, Vector2.Zero);

            if (glados != null)
            {
                gsPlay.cameraPlayer1.AddTarget(glados);
                gsPlay.cameraPlayer2.AddTarget(glados);
            }
        }

        public virtual void ActivateEvent(int eventID, TriggerState triggerState)
        {
        }

        public static Point PositionToGrid(Vector2 position)
        {
            int x = (int)Math.Floor(position.X / collisionSize);
            int y = (int)Math.Floor(position.Y / collisionSize);
            return new Point(x, y);
        }

        public static Vector2 GridToPosition(int x, int y)
        {
            return GridToPosition(new Point(x, y));
        }
        public static Vector2 GridToPosition(Point gridSpace)
        {
            return new Vector2(gridSpace.X * collisionSize, gridSpace.Y * collisionSize);
        }

        public static Vector2 GridToCenterPosition(int x, int y)
        {
            return GridToPosition(x, y) + new Vector2(16, 16);
        }
        public static Vector2 GridToCenterPosition(Point gridSpace)
        {
            return GridToCenterPosition(gridSpace.X, gridSpace.Y);
        }

        public static void CheckForPlayerStuff(Player player)
        {
            Point cornerTopLeft = PositionToGrid(new Vector2(player.hitBox.Left(), player.hitBox.Top()));
            Point cornerBottomRight = PositionToGrid(new Vector2(player.hitBox.Right(), player.hitBox.Bottom()));

            for (int y = cornerTopLeft.Y; y <= cornerBottomRight.Y; y++)
            {
                for (int x = cornerTopLeft.X; x <= cornerBottomRight.X; x++)
                {
                    if (IsLegalXY(x, y))
                    {
                        if (environmentData[x, y].checkpoint)
                        {

                            if (player.checkpoint != GridToCenterPosition(x, y))
                            {
                                player.checkpoint = GridToCenterPosition(new Point(x, y));
                                if (player.playerObjectMode == PlayerObjectMode.One)
                                {
                                    player1Checkpoint.position = GridToCenterPosition(x, y);
                                    GSPlay.AddCheckpointParticles(GridToCenterPosition(x, y), true);
                                    GSPlay.AddCheckpointParticles(GridToCenterPosition(x, y), true);
                                    GSPlay.AddCheckpointParticles(GridToCenterPosition(x, y), true);
                                }
                                else
                                {
                                    player2Checkpoint.position = GridToCenterPosition(x, y);
                                    GSPlay.AddCheckpointParticles(GridToCenterPosition(x, y), false);
                                    GSPlay.AddCheckpointParticles(GridToCenterPosition(x, y), false);
                                    GSPlay.AddCheckpointParticles(GridToCenterPosition(x, y), false);
                                }
                            }
                        }
                        
                    }
                }
            }
        }

        public static void CheckEnvironment(PhysObj obj)
        {
            Point cornerTopLeft = PositionToGrid(new Vector2(obj.hitBox.Left(), obj.hitBox.Top()));
            Point cornerBottomRight = PositionToGrid(new Vector2(obj.hitBox.Right(), obj.hitBox.Bottom()));
            bool collision = false;
            float friction = obj.frictionMultiplier;
            List<AABB> collisionList = new List<AABB>();
            List<float> frictionList = new List<float>();
            for (int y = cornerTopLeft.Y; y <= cornerBottomRight.Y; y++)
            {
                for (int x = cornerTopLeft.X; x <= cornerBottomRight.X; x++)
                {
                    if (CheckCollision(new Point(x, y)))
                    {
                        AABB aabb = new AABB(null, GridToPosition(new Point(x, y)), new Vector2(collisionSize));
                        if (environmentData[x, y].inclineType != InclineType.Flat)
                        {
                            aabb = SolveIncline(obj, x, y);
                            
                        }
                        if (environmentData[x, y].oneWayPlatform)
                        {
                            aabb.omitLeft = true;
                            aabb.omitRight = true;
                            aabb.omitBottom = true;
                            if (obj.speed.Y < 0 || obj.ignoreOneWay)
                            {
                                aabb.omitTop = true;
                            }
                        }

                        if ((CheckCollision(x - 1, y) && !environmentData[x - 1, y].oneWayPlatform) || environmentData[x, y].omitLeft) aabb.omitLeft = true;
                        if ((CheckCollision(x + 1, y) && !environmentData[x + 1, y].oneWayPlatform) || environmentData[x, y].omitRight) aabb.omitRight = true;
                        if ((CheckCollision(x, y + 1) && !environmentData[x, y + 1].oneWayPlatform) || environmentData[x, y].omitBottom) aabb.omitBottom = true;
                        if ((CheckCollision(x, y - 1) && !environmentData[x, y - 1].oneWayPlatform) || environmentData[x, y].omitTop) aabb.omitTop = true;

                        //if (CheckCollision(x - 1, y) && environmentData[x,y].inclineType == InclineType.Flat)
                        //{
                        //    if (environmentData[x - 1, y].inclineType != InclineType.Flat && (obj.position.X > aabb.x1 && obj.position.X < aabb.x2)) aabb.omitTop = true;
                        //}

                        collisionList.Add(aabb);
                        collision = true;
                        frictionList.Add(GetFriction(obj, environmentData[x, y]));
                    }



                    CheckCurrent(obj, x, y);

                    if (IsLegalXY(x,y) && environmentData[x, y].killZone)
                    {
                        obj.Die();
                    }

                }
            }

            Point checkBelow = PositionToGrid(new Vector2(obj.position.X, obj.hitBox.Bottom() + 1));
            if (CheckCollision(checkBelow))
            {
                if (environmentData[checkBelow.X, checkBelow.Y].inclineType != InclineType.Flat)
                {
                    //Trace.WriteLine("Barf!");
                    if (!obj.jumpingLegacy)
                    {
                        //AABB aabb = new AABB(null, GridToPosition(checkBelow), new Vector2(collisionSize));
                        //aabb = SolveIncline(obj, checkBelow.X, checkBelow.Y);
                        //obj.hitBox.SetPosition(obj.hitBox.GetPosition() + new Vector2(0, aabb.y2 - obj.hitBox.Bottom()));
                        //collisionList.Add(aabb);
                        //frictionList.Add(GetFriction(obj, environmentData[checkBelow.X, checkBelow.Y]));
                        obj.nextForce.Y += 20;
                    }
                }
            }

            //int y1 = cornerBottomRight.Y + 1;
            //for (int x = cornerTopLeft.X; x <= cornerBottomRight.X; x++)
            //{
            //    if (CheckCollision(x, y1))
            //    {
            //        AABB aabb = new AABB(null, GridToPosition(new Point(x, y1)), new Vector2(collisionSize));
            //        if (environmentData[x, y1].inclineType != InclineType.Flat)
            //        {
            //            obj.position += new Vector2(0, 32);
            //            collisionList.Add(aabb);
            //            frictionList.Add(GetFriction(obj, environmentData[x, y1]));
            //        }
            //    }
            //}



            //if (collision == false)
            //{
            //    if (obj.state == PhysState.Grounded)
            //{ //check to see if there's ground beneath the object:


            //Point under = PositionToGrid(obj.tempPosition);
            //under.Y += 1;
            //if (CheckCollision(under.X, under.Y))
            //{
            //    collision = true;
            //}
            //if (!collision) obj.state = PhysState.Air;

            //int y = cornerBottomRight.Y + 1;
            //for (int x = cornerTopLeft.X; x < cornerBottomRight.X; x++)
            //{
            //    if (CheckCollision(x, y))
            //    {
            //        collision = true;
            //        break;
            //    }
            //}
            //if (!collision) obj.state = PhysState.Air;
            //    }
            //    return;
            //}

            foreach (AABB aabb in collisionList)
            {
                Vector2 solution = aabb.SolveCollision(obj.hitBox.GetAABB());
                obj.hitBox.SetPosition(obj.hitBox.GetPosition() + solution);
                if (solution.Y < 0)
                {
                    friction = frictionList.ElementAt(collisionList.IndexOf(aabb));
                }
            }
            Vector2 newpos = obj.hitBox.GetPosition();
            if (newpos.Y != obj.tempPosition.Y) obj.speed.Y = SolveBounce(obj, true);
            if (newpos.X != obj.tempPosition.X) obj.speed.X = SolveBounce(obj, false);
            if (newpos.Y < obj.tempPosition.Y)
            {
                obj.state = PhysState.Grounded;
            }

            obj.tempPosition = newpos;

            if (obj.state == PhysState.Grounded)
            {
                obj.frictionMultiplier = friction;
                if (friction == EnvironmentData.NORMAL)
                {
                    obj.moveForce = 1;
                }
                else if (friction == EnvironmentData.ICE)
                {
                    obj.moveForce = .2f;
                }
            }
            else
            {
                obj.frictionMultiplier = EnvironmentData.AIR;
                obj.moveForce = .2f;
            }
        }

        public static float SolveBounce(PhysObj obj, bool bounceVertical)
        {
            if (obj.canBounce)
            {
                if (bounceVertical) return -obj.speed.Y * obj.bounceMultiplier;
                else return -obj.speed.X / 2;
            }
            return 0;
        }

        public static AABB SolveIncline(PhysObj obj, int x, int y)
        {
            bool omitTop = false;
            Vector2 cornerTopLeft = GridToPosition(new Point(x, y));
            Vector2 cornerBottomRight = cornerTopLeft + new Vector2(collisionSize);
            float thirdSize = collisionSize / 3;
            float halfSize = collisionSize / 2;
            switch (environmentData[x, y].inclineType)
            {
                case InclineType.ThreeTileRight1:
                    float ratio = (cornerTopLeft.X - obj.tempPosition.X) / collisionSize;
                    if (ratio > 0 || ratio < -1) omitTop = true;
                    cornerTopLeft.Y = cornerBottomRight.Y + ratio * (thirdSize);
                    break;
                case InclineType.ThreeTileRight2:
                    ratio = (cornerTopLeft.X - obj.tempPosition.X) / collisionSize;
                    cornerTopLeft.Y = cornerBottomRight.Y - (thirdSize) + ratio * (thirdSize);
                    if (ratio > 0 || ratio < -1) omitTop = true;
                    break;
                case InclineType.ThreeTileRight3:
                    ratio = (cornerTopLeft.X - obj.tempPosition.X) / collisionSize;
                    if (ratio > 0 || ratio < -1) omitTop = true;
                    cornerTopLeft.Y = cornerBottomRight.Y - (2 * thirdSize) + ratio * (thirdSize);
                    break;
                case InclineType.ThreeTileLeft1:
                    ratio = (cornerTopLeft.X - obj.tempPosition.X) / collisionSize;
                    if (ratio > 0 || ratio < -1) omitTop = true;
                    cornerTopLeft.Y = cornerTopLeft.Y - ratio * (thirdSize);
                    break;
                case InclineType.ThreeTileLeft2:
                    ratio = (cornerTopLeft.X - obj.tempPosition.X) / collisionSize;
                    //if (ratio > 0 || ratio < -1) omitTop = true;
                    cornerTopLeft.Y = cornerTopLeft.Y + (thirdSize) - ratio * (thirdSize);
                    break;
                case InclineType.ThreeTileLeft3:
                    ratio = (cornerTopLeft.X - obj.tempPosition.X) / collisionSize;
                    if (ratio > 0 || ratio < -1) omitTop = true;
                    cornerTopLeft.Y = cornerTopLeft.Y + (2 * thirdSize) - ratio * (thirdSize);
                    break;
                case InclineType.TwoTileRight1:
                    ratio = (cornerTopLeft.X - obj.tempPosition.X) / collisionSize;
                    if (ratio > 0 || ratio < -1) omitTop = true;
                    cornerTopLeft.Y = cornerBottomRight.Y + ratio * (halfSize);
                    break;
                case InclineType.TwoTileRight2:
                    ratio = (cornerTopLeft.X - obj.tempPosition.X) / collisionSize;
                    if (ratio > 0 || ratio < -1) omitTop = true;
                    cornerTopLeft.Y = cornerBottomRight.Y - (halfSize) + ratio * (halfSize);
                    break;
                case InclineType.TwoTileLeft1:
                    ratio = (cornerTopLeft.X - obj.tempPosition.X) / collisionSize;
                    if (ratio > 0 || ratio < -1) omitTop = true;
                    cornerTopLeft.Y = cornerTopLeft.Y - ratio * (halfSize);
                    break;
                case InclineType.TwoTileLeft2:
                    ratio = (cornerTopLeft.X - obj.tempPosition.X) / collisionSize;
                    if (ratio > 0 || ratio < -1) omitTop = true;
                    cornerTopLeft.Y = cornerTopLeft.Y + (halfSize) - ratio * (halfSize);
                    break;
            }
            AABB aabb = new AABB(null, cornerTopLeft.X, cornerTopLeft.Y, cornerBottomRight.X, cornerBottomRight.Y);
            aabb.omitLeft = true;
            aabb.omitRight = true;
            if (omitTop)
            {
                aabb.omitTop = true;
                aabb.omitBottom = true;
            }
            aabb.incline = true;
            return aabb;
        }

        public static void CheckCurrent(PhysObj obj, int x, int y)
        {
            if (!IsLegalXY(x, y)) return;
            if (environmentData[x, y].airCurrent != Vector2.Zero)
                obj.nextForce = environmentData[x, y].airCurrent;
            //if (obj.nextForce.X != 0) obj.terminalSpeed.X = 10;
            //else obj.terminalSpeed.X = 6;
        }

        public static bool CheckCollision(Point gridSpace)
        {
            return CheckCollision(gridSpace.X, gridSpace.Y);
        }
        public static bool CheckCollision(int x, int y)
        {
            if (IsLegalXY(x, y)) return (environmentData[x, y].isSolid);
            return false;
        }

        public static bool IsLegalXY(int x, int y)
        {
            return (x >= 0 && x < levelGridSize.X && y >= 0 && y < levelGridSize.Y);
        }

        public static float GetFriction(PhysObj obj, EnvironmentData data)
        {
            switch (data.frictionType)
            {
                case FrictionType.Normal:
                    return EnvironmentData.NORMAL;
                case FrictionType.Ice:
                    return EnvironmentData.ICE;
            }
            return 1;
        }

        public void DrawItems(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            int drawnIndex = 0;
            foreach (InventoryItem item in itemList)
            {
                if (item.hasItem)
                {
                    item.prevTarget = item.targetPosition;
                    item.targetPosition = new Vector2(48 + drawnIndex * 32, 48);
                    if (item.targetPosition != item.prevTarget)
                    {
                        item.itemImage.am.StartNewAnimation(AnimationType.EaseOutQuint, item.itemImage.position, item.targetPosition, 60);
                    }
                    item.itemImage.Update();
                    item.itemImage.Draw(spriteBatch);
                    drawnIndex++;
                }
            }
            spriteBatch.End();
        }
    }
}
