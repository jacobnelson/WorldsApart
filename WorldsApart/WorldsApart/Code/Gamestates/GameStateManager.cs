﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

//using Lidgren.Network;

using WorldsApart.Code.Graphics;
using WorldsApart.Code.Entities;
using WorldsApart.Code.Controllers;
using WorldsApart.Code.Levels;

using System.Diagnostics;

namespace WorldsApart.Code.Gamestates
{
    public enum ServerPacketType
    {
        NONE,
        USERID,
        WORLD
    }

    public enum GameStateType
    {
        GSTitle,
        GSMenu,
        GSPlay,
        GSWin
    }

    class GameStateManager
    {
        static public bool isMultiplayer = false;
        //public bool isNetwork = false;
        //public bool isHost = false;

        //const string appID = "worldsapart";
        //const int appPort = 14242;

        public Game1 game;

        public GSTitle gsTitle;
        public GSMenu gsMenu;
        public GSPlay gsPlay;
        public GSWin gsWin;
        public GSPause gsPause;
        public GSHowToPlay gsHow;
        //GSOverlay gsOverlay;

        public bool screenTransition = false;
        public int screenTransitionCounter = 0;
        public int screenTransitionRate = 60;
        public GameStateType transitionType;

        public int currentLevel = 1; 
        public int goodness = 0;

        //NetServer server;
        //NetClient client;


        public GameStateManager(Game1 game)
        {
            this.game = game;

            Art.lineEnd = game.Content.Load<Texture2D>("ShaderAssets/lineEndRight");
            Art.lineMiddle = game.Content.Load<Texture2D>("ShaderAssets/lineMiddle");
            Art.smoke = game.Content.Load<Texture2D>("TestSprites/puff");
            Art.barrier = game.Content.Load<Texture2D>("ShaderAssets/barrier");
            Art.sparkle = game.Content.Load<Texture2D>("ShaderAssets/sparkle");
            Art.rain = game.Content.Load<Texture2D>("ShaderAssets/rain");
            Art.cutscenePlayers = game.Content.Load<Texture2D>("Cutscene/cutscenePlayers");
            Art.whitePixel = game.Content.Load<Texture2D>("whitePixel");
            Art.words1 = game.Content.Load<Texture2D>("Cutscene/words1");
            Art.words2 = game.Content.Load<Texture2D>("Cutscene/words2");
            Art.words3 = game.Content.Load<Texture2D>("Cutscene/words3");
            Art.words4 = game.Content.Load<Texture2D>("Cutscene/words4");
            Art.words5 = game.Content.Load<Texture2D>("Cutscene/words5");
            Art.words6 = game.Content.Load<Texture2D>("Cutscene/words6");
            Art.portalPulse = game.Content.Load<Texture2D>("GameObjects/portalPulse");
            Art.portalItem1 = game.Content.Load<Texture2D>("GameObjects/portalItem1");
            Art.portalItem2 = game.Content.Load<Texture2D>("GameObjects/portalItem2");
            Art.portalItem3 = game.Content.Load<Texture2D>("GameObjects/portalItem3");
            Art.portalItem4 = game.Content.Load<Texture2D>("GameObjects/portalItem4");
            Art.portalItem5 = game.Content.Load<Texture2D>("GameObjects/portalItem5");
            Art.smokePuff = game.Content.Load<Texture2D>("TestSprites/SmokePuff");
            Art.bridgeParticle1 = game.Content.Load<Texture2D>("GameObjects/bridgeDebrisParticle1");
            Art.bridgeParticle2 = game.Content.Load<Texture2D>("GameObjects/bridgeDebrisParticle2");
            Art.bridgeParticle3 = game.Content.Load<Texture2D>("GameObjects/bridgeDebrisParticle3");
            Art.bridgeParticle4 = game.Content.Load<Texture2D>("GameObjects/bridgeDebrisParticle4");
            Art.bridgeParticle5 = game.Content.Load<Texture2D>("GameObjects/bridgeDebrisParticle5");
            Art.bridgeParticle6 = game.Content.Load<Texture2D>("GameObjects/bridgeDebrisParticle6");
            Art.bridgeParticle7 = game.Content.Load<Texture2D>("GameObjects/bridgeDebrisParticle7");
            Art.bridgeParticle8 = game.Content.Load<Texture2D>("GameObjects/bridgeDebrisParticle8");
            Art.bridgeParticle9 = game.Content.Load<Texture2D>("GameObjects/bridgeDebrisParticle9");
            Art.bridgeParticle10 = game.Content.Load<Texture2D>("GameObjects/bridgeDebrisParticle10");
            Art.bridgeParticle11 = game.Content.Load<Texture2D>("GameObjects/bridgeDebrisParticle11");
            Art.bridgeParticle12 = game.Content.Load<Texture2D>("GameObjects/bridgeDebrisParticle12");
            Art.bridgeParticle13 = game.Content.Load<Texture2D>("GameObjects/bridgeDebrisParticle13");
            Art.bridgeParticle14 = game.Content.Load<Texture2D>("GameObjects/bridgeDebrisParticle14");
            Art.bridgeParticle15 = game.Content.Load<Texture2D>("GameObjects/bridgeDebrisParticle15");
            Art.bridgeParticle16 = game.Content.Load<Texture2D>("GameObjects/bridgeDebrisParticle16");
            Art.bridgeParticle17 = game.Content.Load<Texture2D>("GameObjects/bridgeDebrisParticle17");
            Art.bridgeParticle18 = game.Content.Load<Texture2D>("GameObjects/bridgeDebrisParticle18");
            Art.door = game.Content.Load<Texture2D>("TestSprites/door");
            Art.platform = game.Content.Load<Texture2D>("TestSprites/platform");


            GSOverlay.InitializeGSOverlay(this);

            

            SwitchToGSTitle();

            AudioManager.PlayMusic("Title");
        }

        public void Update(GameTime gameTime)
        {
            if (screenTransition)
            {
                screenTransitionCounter++;
                if (screenTransitionCounter >= screenTransitionRate)
                {
                    screenTransition = false;
                    screenTransitionCounter = 0;
                    switch (transitionType)
                    {
                        case GameStateType.GSMenu:
                            GSOverlay.fadeOverlay.alpha = 255;
                            SwitchToGSMenu();
                            GSOverlay.FadeOut(30);
                            break;
                        case GameStateType.GSPlay:
                            GSOverlay.fadeOverlay.alpha = 255;
                            gsPlay = null;
                            SwitchToGSPlay();
                            GSOverlay.FadeOut(30);
                            break;
                        case GameStateType.GSTitle:
                            GSOverlay.fadeOverlay.alpha = 255;
                            SwitchToGSTitle();
                            GSOverlay.FadeOut(30);
                            break;
                        case GameStateType.GSWin:
                            GSOverlay.fadeOverlay.alpha = 0;
                            SwitchToGSWin();
                            break;
                    }
                }
            }

            GSOverlay.Update(gameTime);
            if (gsPlay != null) gsPlay.Update(gameTime);
            if (gsPause != null) gsPause.Update(gameTime);
            if (gsMenu != null) gsMenu.Update(gameTime);
            if (gsTitle != null) gsTitle.Update(gameTime);
            if (gsWin != null) gsWin.Update(gameTime);
            if (gsHow != null) gsHow.Update(gameTime);

            //if (server != null)
            //{
            //    HostReceive();
            //    if (server.Connections.Count > 0)
            //    {
            //        HostSend();
            //    }
            //}
            //if (client != null)
            //{
                
            //    ClientSend();
            //    ClientReceive();
            //}
        }

        public void TransitionToGameState(GameState current, GameStateType type, int duration)
        {
            GSOverlay.FadeIn(duration - 5, Color.Black);
            current.stopInput = true;
            screenTransition = true;
            screenTransitionRate = duration;
            transitionType = type;
        }

        public void SwitchToLevel(int levelID)
        {

        }

        public void SwitchToGSPlay()
        {
            if (gsPlay != null && gsPlay.paused)
            {
                gsPlay.paused = false;
            }
            else
            {
                gsPlay = new GSPlay(this, currentLevel);
            }
            gsPause = null;
            gsTitle = null;
            gsWin = null;
            gsMenu = null;
            gsHow = null;
            
        }

        public void SwitchToGSMenu()
        {
            gsPlay = null;
            gsTitle = null;
            gsWin = null;
            gsMenu = new GSMenu(this);
            gsPause = null;
            gsHow = null;
        }

        public void SwitchToGSTitle()
        {
            if (gsTitle == null)
            {
                gsTitle = new GSTitle(this);
            }
            gsPlay = null;
            gsWin = null;
            gsMenu = null;
            gsHow = null;
        }

        public void SwitchToGSWin()
        {
            if (gsWin == null)
            {
                gsWin = new GSWin(this);
            }
            gsPlay = null;
            gsTitle = null;
            gsMenu = null;
            gsHow = null;
        }

        public void SwitchToGSPause()
        {
            if (gsPause == null)
            {
                gsPause = new GSPause(this);
            }
            gsPlay.paused = true;
            gsTitle = null;
            gsMenu = null;
            gsHow = null;
        }

        public void SwitchToGSHow()
        {
            if (gsHow == null)
            {
                gsHow = new GSHowToPlay(this);
            }
            gsPlay = null;
            gsTitle = null;
            gsMenu = null;
            gsPause = null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            if (gsPlay != null) gsPlay.Draw(spriteBatch);
            if (gsPause != null) gsPause.Draw(spriteBatch);
            if (gsTitle != null) gsTitle.Draw(spriteBatch);
            if (gsMenu != null) gsMenu.Draw(spriteBatch);
            if (gsWin != null) gsWin.Draw(spriteBatch);
            if (gsHow != null) gsHow.Draw(spriteBatch);
            game.GraphicsDevice.SetRenderTarget(null);
            GSOverlay.Draw(spriteBatch);
        }

        public ContentManager NewContentManager()
        {
            return new ContentManager(game.Services, "Content");
        }

        #region Network Stuff
        //public void HostStart()
        //{
        //    Console.WriteLine("host starting...");
        //    NetPeerConfiguration config = new NetPeerConfiguration(appID);
        //    config.Port = appPort;
        //    config.MaximumConnections = 8;
        //    config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
        //    server = new NetServer(config);
        //    server.Start();

        //    SwitchToGSPlay();
        //    //isNetwork = true;
        //    isHost = true;
        //    //Player player = gsPlay.AddPlayer();
        //    //player.userID = HostGetNewRandNum();
        //    //gsPlay.localPlayer = player;
        //    //gsPlay.camera.target = player;
        //    //player.isLocal = true;
        //}

        //public void HostReceive()
        //{
        //    NetIncomingMessage inc;
        //    while ((inc = server.ReadMessage()) != null)
        //    {
        //        //Console.WriteLine("packet!");
        //        switch (inc.MessageType)
        //        {
        //            case NetIncomingMessageType.ConnectionApproval:
        //                inc.SenderConnection.Approve();
        //                Console.WriteLine("client connected");
        //                isNetwork = true;
        //                //Player newplayer = gsPlay.AddPlayer(inc.SenderConnection);
        //                //newplayer.userID = HostGetNewRandNum();

        //                NetOutgoingMessage msg = server.CreateMessage();
        //                msg.Write((byte)ServerPacketType.USERID);
        //                //msg.Write(newplayer.userID);
        //                server.SendMessage(msg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered);

        //                break;
        //            case NetIncomingMessageType.Data:
        //                //foreach (Player player in gsPlay.players)
        //                //{
        //                //    if (player.netConnection != inc.SenderConnection) continue;
        //                //    player.userID = inc.ReadInt32();
        //                //    player.keyUp = inc.ReadBoolean();
        //                //    player.keyDown = inc.ReadBoolean();
        //                //    player.keyLeft = inc.ReadBoolean();
        //                //    player.keyRight = inc.ReadBoolean();
        //                //    player.keySpace = inc.ReadBoolean();
        //                //}
        //                float posX = inc.ReadFloat();
        //                float posY = inc.ReadFloat();
        //                gsPlay.player2.position = new Vector2(posX, posY);
        //                gsPlay.player2.speed.X = inc.ReadFloat();
        //                gsPlay.player2.speed.Y = inc.ReadFloat();
        //                gsPlay.player2.facing = (Facing)inc.ReadByte();
        //                gsPlay.player2.grabbing = inc.ReadBoolean();
        //                if (inc.ReadBoolean())
        //                {
        //                    gsPlay.player2.isSignaling = true;
        //                }

        //                foreach (FlipSwitch flip in gsPlay.switchList)
        //                {
        //                    int col = inc.ReadInt32();
        //                    if (flip.currentCellCol != col)
        //                    {
        //                        flip.PressSwitch();
        //                    }
        //                }
        //                //gsPlay.player2.rightDown = inc.ReadBoolean();
        //                //gsPlay.player2.leftDown = inc.ReadBoolean();
        //                //gsPlay.player2.upDown = inc.ReadBoolean();
        //                //gsPlay.player2.downDown = inc.ReadBoolean();
        //                //gsPlay.player2.jumpDown = inc.ReadBoolean();
        //                //gsPlay.player2.jumpPressed = inc.ReadBoolean();
        //                //gsPlay.player2.jumpReleased = inc.ReadBoolean();
        //                //gsPlay.player2.actionDown = inc.ReadBoolean();
        //                //gsPlay.player2.actionPressed = inc.ReadBoolean();
        //                //gsPlay.player2.signalPressed = inc.ReadBoolean();
                       
        //                break;
        //            case NetIncomingMessageType.StatusChanged:
        //                // handle connections, disconnections
        //                if (inc.SenderConnection.Status == NetConnectionStatus.Disconnected || inc.SenderConnection.Status == NetConnectionStatus.Disconnecting)
        //                {
        //                    Console.WriteLine("client disconnect");
        //                    isNetwork = false;
        //                    //foreach (Player player in gsPlay.players)
        //                    //{
        //                    //    if (player.netConnection == inc.SenderConnection)
        //                    //    {
        //                    //        gsPlay.players.Remove(player);
        //                    //        break;
        //                    //    }
        //                    //}
        //                }
        //                break;
        //            default:
        //                // other message types
        //                break;
        //        }
        //    }
        //}
        //public void HostSend()
        //{
        //    if (gsPlay == null) return;

        //    NetOutgoingMessage msg = server.CreateMessage();
        //    msg.Write((byte)ServerPacketType.WORLD);

        //    msg.Write(gsPlay.player1.position.X);
        //    msg.Write(gsPlay.player1.position.Y);
        //    msg.Write(gsPlay.player1.speed.X);
        //    msg.Write(gsPlay.player1.speed.Y);
        //    msg.Write(gsPlay.player1.grabbing);
        //    msg.Write(gsPlay.player1.signalPressed);

        //    foreach (PickUpObj pickUp in gsPlay.pickUpList)
        //    {
        //        msg.Write(pickUp.position.X);
        //        msg.Write(pickUp.position.Y);
        //        msg.Write(pickUp.speed.X);
        //        msg.Write(pickUp.speed.Y);
        //    }

        //    foreach (FlipSwitch flip in gsPlay.switchList)
        //    {
        //        msg.Write(flip.currentCellCol);
        //    }

        //    foreach (MovingPlatform platform in gsPlay.platformList)
        //    {
        //        msg.Write(platform.am.animationDuration);
        //        //msg.Write(platform.position.X);
        //        //msg.Write(platform.position.Y);
        //        //msg.Write(platform.speed.X);
        //        //msg.Write(platform.speed.Y);
        //        //msg.Write(platform.triggerState == TriggerState.Triggered);
        //    }

        //    foreach (CircularPlatform platform in gsPlay.cPlatformList)
        //    {
        //        msg.Write(platform.am.animationDuration);
        //        //msg.Write(platform.position.X);
        //        //msg.Write(platform.position.Y);
        //        //msg.Write(platform.speed.X);
        //        //msg.Write(platform.speed.Y);
        //        //msg.Write(platform.triggerState == TriggerState.Triggered);
        //    }

        //    foreach (Door door in gsPlay.doorList)
        //    {
        //        msg.Write(door.am.animationDuration);
        //        msg.Write(door.am.fadeDuration);
        //        //msg.Write(door.position.X);
        //        //msg.Write(door.position.Y);
        //        //msg.Write(door.alpha);
        //    }

        //    foreach (Moveable move in gsPlay.moveList)
        //    {
        //        msg.Write(move.position.X);
        //        msg.Write(move.position.Y);
        //        msg.Write(move.speed.X);
        //        msg.Write(move.speed.Y);
        //    }

        //    foreach (PointLight light in gsPlay.lightList)
        //    {
        //        msg.Write(light.position.X);
        //        msg.Write(light.position.Y);
        //    }
            

            

        //    //msg.Write(gsPlay.players.Count);
        //    //foreach (Player player in gsPlay.players)
        //    //{
        //    //    msg.Write(player.userID);
        //    //    msg.Write(player.position.X);
        //    //    msg.Write(player.position.Y);
        //    //    msg.Write(player.speed.X);
        //    //    msg.Write(player.speed.Y);
        //    //    msg.Write(player.rotation);
        //    //    msg.Write(player.speedRotation);
        //    //}
        //    server.SendMessage(msg, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        //}
        //public void ClientJoin(string ip)
        //{

        //    if (client != null)
        //    {
        //        client.Disconnect("");
        //        client.Shutdown("");
        //    }

        //    Console.WriteLine("client start. trying to connect...");
        //    NetPeerConfiguration Config = new NetPeerConfiguration(appID);
        //    Config.Port = appPort;
        //    client = new NetClient(Config);
        //    client.Start();

        //    // Create new outgoing message
        //    NetOutgoingMessage msg = client.CreateMessage();
        //    try
        //    {
        //        client.Connect(ip, appPort, msg);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Oh noes! It failed!");
        //    }

        //    isNetwork = true;
        //    isHost = false;
        //    SwitchToGSPlay();

        //    // wait for a gamestate before beginning
        //}
        //public void ClientSend()
        //{
        //    if (gsPlay != null)
        //    {
        //        NetOutgoingMessage msg = client.CreateMessage();
        //        msg.Write(gsPlay.player2.position.X);
        //        msg.Write(gsPlay.player2.position.Y);
        //        msg.Write(gsPlay.player2.speed.X);
        //        msg.Write(gsPlay.player2.speed.Y);
        //        msg.Write((byte)gsPlay.player2.facing);
        //        msg.Write(gsPlay.player2.grabbing);
        //        msg.Write(gsPlay.player2.isSignaling);

        //        foreach (FlipSwitch flip in gsPlay.switchList)
        //        {
        //            msg.Write(flip.currentCellCol);
        //        }
        //        //msg.Write(gsPlay.player2.rightDown);
        //        //msg.Write(gsPlay.player2.leftDown);
        //        //msg.Write(gsPlay.player2.upDown);
        //        //msg.Write(gsPlay.player2.downDown);
        //        //msg.Write(gsPlay.player2.jumpDown);
        //        //msg.Write(gsPlay.player2.jumpPressed);
        //        //msg.Write(gsPlay.player2.jumpReleased);
        //        //msg.Write(gsPlay.player2.actionDown);
        //        //msg.Write(gsPlay.player2.actionPressed);
        //        //msg.Write(gsPlay.player2.signalPressed);
        //        client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        //    }
        //}
        //public void ClientReceive()
        //{
        //    NetIncomingMessage inc;
        //    while ((inc = client.ReadMessage()) != null)
        //    {
        //        if (inc.MessageType == NetIncomingMessageType.Data)
        //        {
        //            switch (inc.ReadByte())
        //            {
        //                case (byte)ServerPacketType.USERID:
        //                    Console.WriteLine("Connected!");
        //                    isNetwork = true;
        //                    isHost = false;
        //                    SwitchToGSPlay();

        //                    break;
        //                case (byte)ServerPacketType.WORLD:

        //                    float posX = inc.ReadFloat();
        //                    float posY = inc.ReadFloat();
        //                    gsPlay.player1.position = new Vector2(posX, posY);
        //                    gsPlay.player1.speed.X = inc.ReadFloat();
        //                    gsPlay.player1.speed.Y = inc.ReadFloat();
        //                    gsPlay.player1.grabbing = inc.ReadBoolean();
        //                    if (inc.ReadBoolean())
        //                    {
        //                        gsPlay.player1.isSignaling = true;
        //                    }


        //                    foreach (PickUpObj pickUp in gsPlay.pickUpList)
        //                    {
        //                        posX = inc.ReadFloat();
        //                        posY = inc.ReadFloat();
        //                        pickUp.position = new Vector2(posX, posY);
        //                        pickUp.speed.X = inc.ReadFloat();
        //                        pickUp.speed.Y = inc.ReadFloat();
        //                    }

        //                    foreach (FlipSwitch flip in gsPlay.switchList)
        //                    {
        //                        int col = inc.ReadInt32();
        //                        if (flip.currentCellCol != col)
        //                        {
        //                            flip.PressSwitch();
        //                        }
        //                    }

                            

        //                    foreach (MovingPlatform platform in gsPlay.platformList)
        //                    {
        //                        platform.am.animationDuration = inc.ReadFloat();
        //                        //posX = inc.ReadFloat();
        //                        //posY = inc.ReadFloat();
        //                        //platform.position = new Vector2(posX, posY);
        //                        //platform.speed.X = inc.ReadFloat();
        //                        //platform.speed.Y = inc.ReadFloat();
        //                        //msg.Write(platform.triggerState == TriggerState.Triggered);
        //                    }

        //                    foreach (CircularPlatform platform in gsPlay.cPlatformList)
        //                    {
        //                        platform.am.animationDuration = inc.ReadFloat();
        //                        //posX = inc.ReadFloat();
        //                        //posY = inc.ReadFloat();
        //                        //platform.position = new Vector2(posX, posY);
        //                        //platform.speed.X = inc.ReadFloat();
        //                        //platform.speed.Y = inc.ReadFloat();
        //                        //msg.Write(platform.triggerState == TriggerState.Triggered);
        //                    }

        //                    foreach (Door door in gsPlay.doorList)
        //                    {
        //                        door.am.animationDuration = inc.ReadFloat();
        //                        door.am.fadeDuration = inc.ReadInt32();
        //                        //posX = inc.ReadFloat();
        //                        //posY = inc.ReadFloat();
        //                        //door.position = new Vector2(posX, posY);
        //                        //door.alpha = inc.ReadByte();
        //                    }

        //                    foreach (Moveable move in gsPlay.moveList)
        //                    {
        //                        posX = inc.ReadFloat();
        //                        posY = inc.ReadFloat();
        //                        move.position = new Vector2(posX, posY);
        //                        move.speed.X = inc.ReadFloat();
        //                        move.speed.Y = inc.ReadFloat();
        //                    }

        //                    foreach (PointLight light in gsPlay.lightList)
        //                    {
        //                        posX = inc.ReadFloat();
        //                        posY = inc.ReadFloat();
        //                        light.position = new Vector2(posX, posY);
        //                    }
        //                    //int numOfPlayers = inc.ReadInt32();
        //                    //gsPlay.players.Clear(); //?

        //                    //for (int i = 0; i < numOfPlayers; i++)
        //                    //{
        //                    //    Player player = gsPlay.AddPlayer();
        //                    //    player.userID = inc.ReadInt32();
        //                    //    player.position.X = inc.ReadFloat();
        //                    //    player.position.Y = inc.ReadFloat();
        //                    //    player.speed.X = inc.ReadFloat();
        //                    //    player.speed.Y = inc.ReadFloat();
        //                    //    player.rotation = inc.ReadFloat();
        //                    //    player.speedRotation = inc.ReadFloat();
        //                    //    if (player.userID == userID)
        //                    //    {
        //                    //        gsPlay.localPlayer = player;
        //                    //        gsPlay.camera.target = player;
        //                    //        player.isLocal = true;
        //                    //    }
        //                    //}
        //                    break;
        //            }
        //        }
        //    }
        //}
#endregion
    }
}
