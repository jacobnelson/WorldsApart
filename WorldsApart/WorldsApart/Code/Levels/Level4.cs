using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using WorldsApart.Code.Entities;
using WorldsApart.Code.Gamestates;
using WorldsApart.Code.Controllers;
using WorldsApart.Code.Graphics;


namespace WorldsApart.Code.Levels
{
    class Level4 : Level
    {

        PointLight player1Light;
        PointLight player2Light;


        public Level4(GSPlay gsPlay)
            : base(gsPlay)
        {
            levelDataTexture = gsPlay.LoadTexture("Levels/level4Data");

            player1Pos = GridToPosition(15, 10);
            player2Pos = GridToPosition(17, 10);

            //player1Pos = GridToPosition(228, 48);
            //player2Pos = player1Pos;

            portalPos = GridToPosition(232, 48);
            pItemPos = GridToPosition(194, 48);

            SetupLevel();

            rightLimit = levelWidth - 32;

            atmosphereLight = new Color(0,0,0);

            gsPlay.AddPointLight(gsPlay.LoadTexture("ShaderAssets/pointLight"), GridToPosition(240, 44), new Vector2(4));

            //gsPlay.AddPointLight(gsPlay.LoadTexture("ShaderAssets/pointLight"), GridToPosition(15, 10), new Vector2(5));
            player1Light = gsPlay.AddPointLight(gsPlay.LoadTexture("ShaderAssets/pointLight"), Vector2.Zero, new Vector2(2));
            player1Light.SetGlowing(2, 2.4f, 120);
            player2Light = gsPlay.AddPointLight(gsPlay.LoadTexture("ShaderAssets/pointLight"), Vector2.Zero, new Vector2(2));
            player2Light.SetGlowing(2, 2.4f, 120);

            //Doors and switches
            Door d1 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(47, 42)), OpenState.Closed);
            FlipSwitch s1 = gsPlay.AddOnSwitch(new EventTrigger(this, d1), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(28, 54)));
            s1.SetPlayerMode(PlayerObjectMode.One);
            LightningChain lc1 = gsPlay.AddLightning(s1.position, s1.position + new Vector2(0, -208), Color.Orange);
            lc1.AddVertex(s1.position + new Vector2(384, -208));
            lc1.AddVertex(d1.position + new Vector2(0, 80));
            lc1.AddVertex(d1.position + new Vector2(0, 64));
            s1.AddEvent(new EventTrigger(this, lc1));
            lc1.defaultActive = true;
            lc1.SetActive(true);

            Door d2 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(108, 58)), OpenState.Closed);
            FlipSwitch s2 = gsPlay.AddOnSwitch(new EventTrigger(this, d2), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(62, 57)));
            s2.SetPlayerMode(PlayerObjectMode.One);
            LightningChain lc2 = gsPlay.AddLightning(s2.position, s2.position + new Vector2(0, 112), Color.Orange);
            lc2.AddVertex(d2.position + new Vector2(0, 80));
            lc2.AddVertex(d2.position + new Vector2(0, 64));
            s2.AddEvent(new EventTrigger(this, lc2));
            lc2.defaultActive = true;
            lc2.SetActive(true);

            Door d3 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(100, 39)), OpenState.Closed);
            FlipSwitch s3 = gsPlay.AddOnSwitch(new EventTrigger(this, d3), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(103, 40)));
            s3.SetPlayerMode(PlayerObjectMode.Two);
            LightningChain lc3 = gsPlay.AddLightning(s3.position, s3.position + new Vector2(0, 48), Color.Blue);
            lc3.AddVertex(d3.position + new Vector2(0, 80));
            lc3.AddVertex(d3.position + new Vector2(0, 64));
            s3.AddEvent(new EventTrigger(this, lc3));
            lc3.defaultActive = true;
            lc3.SetActive(true);

            Door d4 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(134, 23)), OpenState.Closed);
            FlipSwitch s4 = gsPlay.AddOnSwitch(new EventTrigger(this, d4), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(120, 33)));
            s4.SetPlayerMode(PlayerObjectMode.One);
            LightningChain lc4 = gsPlay.AddLightning(s4.position, s4.position + new Vector2(0, -192), Color.Orange);
            lc4.AddVertex(s4.position + new Vector2(112, -240));
            lc4.AddVertex(d4.position + new Vector2(0, 80));
            lc4.AddVertex(d4.position + new Vector2(0, 64));
            s4.AddEvent(new EventTrigger(this, lc4));
            lc4.defaultActive = true;
            lc4.SetActive(true);

            Door d5 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(159, 23)), OpenState.Closed);
            FlipSwitch s5 = gsPlay.AddOnSwitch(new EventTrigger(this, d5), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(156, 29)));
            LightningChain lc5 = gsPlay.AddLightning(s5.position, s5.position + new Vector2(0, -112), Color.Yellow);
            lc5.AddVertex(d5.position + new Vector2(0, 80));
            lc5.AddVertex(d5.position + new Vector2(0, 64));
            s5.AddEvent(new EventTrigger(this, lc5));
            lc5.defaultActive = true;
            lc5.SetActive(true);

            Door d6 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(164, 28)) + new Vector2(-16, 0), OpenState.Closed);
            FlipSwitch s6 = gsPlay.AddOnSwitch(new EventTrigger(this, d6), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(162, 24)));
            LightningChain lc6 = gsPlay.AddLightning(s6.position, s6.position + new Vector2(0, 48), Color.Yellow);
            lc6.AddVertex(d6.position + new Vector2(0, -80));
            lc6.AddVertex(d6.position + new Vector2(0, -64));
            s6.AddEvent(new EventTrigger(this, lc6));
            lc6.defaultActive = true;
            lc6.SetActive(true);

            Door d7 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(188, 33)) + new Vector2(-16, 0), OpenState.Closed);
            FlipSwitch s7 = gsPlay.AddOnSwitch(new EventTrigger(this, d7), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(192, 29)));
            LightningChain lc7 = gsPlay.AddLightning(s7.position, s7.position + new Vector2(0, 48), Color.Yellow);
            lc7.AddVertex(d7.position + new Vector2(0, -80));
            lc7.AddVertex(d7.position + new Vector2(0, -64));
            s7.AddEvent(new EventTrigger(this, lc7));
            lc7.defaultActive = true;
            lc7.SetActive(true);

            Door d8 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(178, 28)), OpenState.Closed);
            FlipSwitch s8 = gsPlay.AddOnSwitch(new EventTrigger(this, d8), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(176, 29)));
            LightningChain lc8 = gsPlay.AddLightning(s8.position, s8.position + new Vector2(0, 48), Color.Yellow);
            lc8.AddVertex(d8.position + new Vector2(0, 80));
            lc8.AddVertex(d8.position + new Vector2(0, 64));
            s8.AddEvent(new EventTrigger(this, lc8));
            lc8.defaultActive = true;
            lc8.SetActive(true);

            Door d9 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(169, 33)) + new Vector2(-16, 0), OpenState.Closed);
            FlipSwitch s9 = gsPlay.AddOnSwitch(new EventTrigger(this, d9), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(172, 39)));
            LightningChain lc9 = gsPlay.AddLightning(s9.position, s9.position + new Vector2(0, 48), Color.Yellow);
            lc9.AddVertex(s9.position + new Vector2(-112, 48));
            lc9.AddVertex(d9.position + new Vector2(0, 64));
            s9.AddEvent(new EventTrigger(this, lc9));
            lc9.defaultActive = true;
            lc9.SetActive(true);

            Door d10 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(174, 63)), OpenState.Closed);
            FlipSwitch s10 = gsPlay.AddOnSwitch(new EventTrigger(this, d10), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(173, 69)));
            LightningChain lc10 = gsPlay.AddLightning(s10.position, s10.position + new Vector2(0, -112), Color.Yellow);
            lc10.AddVertex(d10.position + new Vector2(0, 80));
            lc10.AddVertex(d10.position + new Vector2(0, 64));
            s10.AddEvent(new EventTrigger(this, lc10));
            lc10.defaultActive = true;
            lc10.SetActive(true);

            Door d11 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(187, 73)), OpenState.Closed);
            FlipSwitch s11 = gsPlay.AddOnSwitch(new EventTrigger(this, d11), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(173, 74)));
            LightningChain lc11 = gsPlay.AddLightning(s11.position, s11.position + new Vector2(0, 48), Color.Yellow);
            lc11.AddVertex(d11.position + new Vector2(0, 80));
            lc11.AddVertex(d11.position + new Vector2(0, 64));
            s11.AddEvent(new EventTrigger(this, lc11));
            lc11.defaultActive = true;
            lc11.SetActive(true);

            Door d12 = gsPlay.AddOpeningDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(180, 73)), GridToPosition(new Point(180, 68)), OpenState.Closed);
            FlipSwitch s12 = gsPlay.AddSwitch(new EventTrigger(this, d12), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(173, 74)));
            LightningChain lc12 = gsPlay.AddLightning(s12.position, s12.position + new Vector2(0, -112), Color.Yellow);
            lc12.AddVertex(d12.position + new Vector2(0, 80));
            lc12.AddVertex(d12.position + new Vector2(0, 64));
            lc12.ConvertEndPointToTarget(d12);
            s12.AddEvent(new EventTrigger(this, lc12));

            Door d13 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(171, 78)), OpenState.Closed);
            FlipSwitch s13 = gsPlay.AddOnSwitch(new EventTrigger(this, d13), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(174, 79)));
            LightningChain lc13 = gsPlay.AddLightning(s13.position, s13.position + new Vector2(0, 48), Color.Yellow);
            lc13.AddVertex(d13.position + new Vector2(0, 80));
            lc13.AddVertex(d13.position + new Vector2(0, 64));
            s13.AddEvent(new EventTrigger(this, lc13));
            lc13.defaultActive = true;
            lc13.SetActive(true);

            Door d14 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(197, 78)) + new Vector2(-16, 0), OpenState.Closed);
            FlipSwitch s14 = gsPlay.AddOnSwitch(new EventTrigger(this, d14), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(199, 74)));
            LightningChain lc14 = gsPlay.AddLightning(s14.position, s14.position + new Vector2(0, 48), Color.Yellow);
            lc14.AddVertex(d14.position + new Vector2(0, -80));
            lc14.AddVertex(d14.position + new Vector2(0, -64));
            s14.AddEvent(new EventTrigger(this, lc14));
            lc14.defaultActive = true;
            lc14.SetActive(true);

            Door d15 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(202, 58)), OpenState.Closed);
            FlipSwitch s15 = gsPlay.AddOnSwitch(new EventTrigger(this, d15), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(200, 64)));
            LightningChain lc15 = gsPlay.AddLightning(s15.position, s15.position + new Vector2(0, -112), Color.Yellow);
            lc15.AddVertex(d15.position + new Vector2(0, 80));
            lc15.AddVertex(d15.position + new Vector2(0, 64));
            s15.AddEvent(new EventTrigger(this, lc15));
            lc15.defaultActive = true;
            lc15.SetActive(true);

            Door d16 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(212, 63)), OpenState.Closed);
            FlipSwitch s16 = gsPlay.AddOnSwitch(new EventTrigger(this, d16), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(209, 60)));
            LightningChain lc16 = gsPlay.AddLightning(s16.position, s16.position + new Vector2(0, 48), Color.Yellow);
            lc16.AddVertex(d16.position + new Vector2(0, -80));
            lc16.AddVertex(d16.position + new Vector2(0, -64));
            s16.AddEvent(new EventTrigger(this, lc16));
            lc16.defaultActive = true;
            lc16.SetActive(true);

            Door d17 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(211, 38)) + new Vector2(-16, 0), OpenState.Closed);
            Door d26 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(212, 38)) + new Vector2(-16, 0), OpenState.Closed);
            Door d27 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(213, 38)) + new Vector2(-16, 0), OpenState.Closed);
            Door d28 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(214, 38)) + new Vector2(-16, 0), OpenState.Closed);
            Door d29 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(215, 38)) + new Vector2(-16, 0), OpenState.Closed);
            Door d30 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(216, 38)) + new Vector2(-16, 0), OpenState.Closed);
            FlipSwitch s17 = gsPlay.AddOnSwitch(new EventTrigger(this, d17), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(209, 44)));
            s17.AddEvent(new EventTrigger(this, d26));
            s17.AddEvent(new EventTrigger(this, d27));
            s17.AddEvent(new EventTrigger(this, d28));
            s17.AddEvent(new EventTrigger(this, d29));
            s17.AddEvent(new EventTrigger(this, d30));
            LightningChain lc17 = gsPlay.AddLightning(s17.position, s17.position + new Vector2(0, 48), Color.Yellow);
            lc17.AddVertex(s17.position + new Vector2(80, 48));
            lc17.AddVertex(s17.position + new Vector2(80, -112));
            lc17.AddVertex(s17.position + new Vector2(166, -112));
            s17.AddEvent(new EventTrigger(this, lc17));
            lc17.defaultActive = true;
            lc17.SetActive(true);

            Door d31 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(189, 48)), OpenState.Closed);
            FlipSwitch s18 = gsPlay.AddSwitch(new EventTrigger(this, d31), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(214, 39)));
            LightningChain lc18 = gsPlay.AddLightning(s18.position, GridToCenterPosition(214, 40) + new Vector2(-16,0), Color.Red);
            lc18.AddVertex(GridToCenterPosition(211, 40));
            lc18.AddVertex(GridToCenterPosition(211, 45));
            lc18.AddVertex(GridToCenterPosition(200, 45) + new Vector2(-16, 0));
            lc18.AddVertex(GridToPosition(200, 46));
            s18.AddEvent(new EventTrigger(this, lc18));

            Door d32 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(200, 48)), OpenState.Closed);
            FlipSwitch s19 = gsPlay.AddSwitch(new EventTrigger(this, d32), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(214, 39)));
            LightningChain lc19 = gsPlay.AddLightning(GridToCenterPosition(200, 45) + new Vector2(-16, 0), GridToCenterPosition(189, 45) + new Vector2(-16,0), Color.Red);
            lc19.AddVertex(GridToPosition(189, 46));
            s19.AddEvent(new EventTrigger(this, lc19));

            //static doors to block a players path
            Door d18 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(196, 33)) + new Vector2(-16, 0), OpenState.Closed);
            d18.SetPlayerMode(PlayerObjectMode.Two);
            Door d19 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(159, 48)) + new Vector2(-16, 0), OpenState.Closed);
            d19.SetPlayerMode(PlayerObjectMode.One);
            Door d20 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(197, 73)) + new Vector2(-16, 0), OpenState.Closed);
            d20.SetPlayerMode(PlayerObjectMode.Two);
            Door d21 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(212, 58)) + new Vector2(-16, 0), OpenState.Closed);
            d21.SetPlayerMode(PlayerObjectMode.Two);
            Door d22 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(212, 48)) + new Vector2(-16, 0), OpenState.Closed);
            d22.SetPlayerMode(PlayerObjectMode.One);
            Door d23 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(197, 78)) + new Vector2(-16, 0), OpenState.Closed);
            d23.SetPlayerMode(PlayerObjectMode.One);
            Door d24 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(164, 48)) + new Vector2(-16, 0), OpenState.Closed);
            d24.SetPlayerMode(PlayerObjectMode.One);
            gsPlay.AddSwitch(new EventTrigger(this, d24), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(209, 60)));
            Door d25 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(159, 23)), OpenState.Closed);
            d25.SetPlayerMode(PlayerObjectMode.Two);

            //movables
            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveableMechanical"), Level.GridToPosition(new Point(74, 67)), .8f);

            //platforms used to block a players path
            MovingPlatform p1 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform2"), Level.GridToPosition(new Point(144, 26)) + new Vector2(0, -16), Level.GridToPosition(new Point(144, 26)) + new Vector2(0, -16));
            p1.SetPlayerMode(PlayerObjectMode.One);
            MovingPlatform p2 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform2"), Level.GridToPosition(new Point(166, 26)) + new Vector2(0, -16), Level.GridToPosition(new Point(166, 26)) + new Vector2(0, -16));
            p2.SetPlayerMode(PlayerObjectMode.One);
            MovingPlatform p3 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform2"), Level.GridToPosition(new Point(198, 26)) + new Vector2(0, -16), Level.GridToPosition(new Point(198, 26)) + new Vector2(0, -16));
            p3.SetPlayerMode(PlayerObjectMode.One);
            MovingPlatform p4 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform2"), Level.GridToPosition(new Point(208, 26)) + new Vector2(0, -16), Level.GridToPosition(new Point(208, 26)) + new Vector2(0, -16));
            p4.SetPlayerMode(PlayerObjectMode.Two);
            MovingPlatform p5 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform2"), Level.GridToPosition(new Point(193, 36)) + new Vector2(0, -16), Level.GridToPosition(new Point(193, 36)) + new Vector2(0, -16));
            p5.SetPlayerMode(PlayerObjectMode.One);
            MovingPlatform p6 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform2"), Level.GridToPosition(new Point(161, 51)) + new Vector2(0, -16), Level.GridToPosition(new Point(161, 51)) + new Vector2(0, -16));
            p6.SetPlayerMode(PlayerObjectMode.Two);
            MovingPlatform p7 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform2"), Level.GridToPosition(new Point(219, 61)) + new Vector2(0, -16), Level.GridToPosition(new Point(219, 61)) + new Vector2(0, -16));
            p7.SetPlayerMode(PlayerObjectMode.One);
            MovingPlatform p8 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform2"), Level.GridToPosition(new Point(214, 56)) + new Vector2(0, -16), Level.GridToPosition(new Point(214, 56)) + new Vector2(0, -16));
            p8.SetPlayerMode(PlayerObjectMode.Two);
            MovingPlatform p9 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform2"), Level.GridToPosition(new Point(219, 51)) + new Vector2(0, -16), Level.GridToPosition(new Point(219, 51)) + new Vector2(0, -16));
            p9.SetPlayerMode(PlayerObjectMode.Two);
            MovingPlatform p10 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform2"), Level.GridToPosition(new Point(219, 56)) + new Vector2(0, -16), Level.GridToPosition(new Point(219, 56)) + new Vector2(0, -16));
            p10.SetPlayerMode(PlayerObjectMode.One);
            MovingPlatform p11 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform2"), Level.GridToPosition(new Point(214, 51)) + new Vector2(0, -16), Level.GridToPosition(new Point(214, 51)) + new Vector2(0, -16));
            p11.SetPlayerMode(PlayerObjectMode.One);
            MovingPlatform p12 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform2"), Level.GridToPosition(new Point(56, 69)) + new Vector2(0, -16), Level.GridToPosition(new Point(56, 69)) + new Vector2(0, -16));
            p12.SetPlayerMode(PlayerObjectMode.Two);
            MovingPlatform p13 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform2"), Level.GridToPosition(new Point(60, 69)) + new Vector2(0, -16), Level.GridToPosition(new Point(59, 69)) + new Vector2(0, -16));
            p13.SetPlayerMode(PlayerObjectMode.Two);
            MovingPlatform p14 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform2"), Level.GridToPosition(new Point(64, 69)) + new Vector2(0, -16), Level.GridToPosition(new Point(62, 69)) + new Vector2(0, -16));
            p14.SetPlayerMode(PlayerObjectMode.Two);
            MovingPlatform p16 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform2"), Level.GridToPosition(new Point(89, 39)) + new Vector2(0, -16), Level.GridToPosition(new Point(89, 39)) + new Vector2(0, -16));
            p16.SetPlayerMode(PlayerObjectMode.Two);
            MovingPlatform p17 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform2"), Level.GridToPosition(new Point(85, 38)) + new Vector2(0, -16), Level.GridToPosition(new Point(86, 38)) + new Vector2(0, -16));
            p17.SetPlayerMode(PlayerObjectMode.Two);

            //Lights
            //15,58
            LightConsole console1 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/LightConsole"), GridToPosition(15, 58));
            //LightConsole console1 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/LightConsole"), GridToPosition(140, 24));
            console1.AddLight(player1Light);
            console1.SetPlayerMode(PlayerObjectMode.One);
            LightConsole console2 = gsPlay.AddLinkedLightConsole(console1, console1.texture, GridToPosition(0, 0));
            console2.AddLight(player2Light); 
            console2.SetPlayerMode(PlayerObjectMode.One);

            LightConsole console3 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/LightConsole"), GridToPosition(30, 45));
            LightConsole console4 = gsPlay.AddLinkedLightConsole(console3, console3.texture, GridToPosition(0, 0));

            LightConsole console5 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/LightConsole"), GridToPosition(51, 67));
            LightConsole console6 = gsPlay.AddLinkedLightConsole(console5, console5.texture, GridToPosition(0, 0));

            LightConsole console7 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/LightConsole"), GridToPosition(94, 59));
            gsPlay.AddLinkedLightConsole(console7, console7.texture, GridToPosition(0, 0));

            LightConsole console8 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/LightConsole"), GridToPosition(105, 52));
            gsPlay.AddLinkedLightConsole(console8, console8.texture, GridToPosition(0, 0));

            LightConsole console9 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/LightConsole"), GridToPosition(129, 23));
            LightConsole console9a = gsPlay.AddLinkedLightConsole(console9, console9.texture, GridToPosition(0, 0));
            //console9.AddLight(player1Light);
            //console9a.AddLight(player2Light);

            LightConsole console10 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/LightConsole"), GridToPosition(147, 24));
            console10.SetPlayerMode(PlayerObjectMode.One);
            LightConsole console11 = gsPlay.AddLinkedLightConsole(console10, console10.texture, GridToPosition(147, 29));
            console11.SetPlayerMode(PlayerObjectMode.Two);
            LightningChain clc1 = gsPlay.AddLightning(console10.position, console11.position, Color.White);
            console10.AddEvent(new EventTrigger(this, clc1));
            console11.AddEvent(new EventTrigger(this, clc1)); 


            LightConsole console12 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/LightConsole"), GridToPosition(205, 24));
            console12.SetPlayerMode(PlayerObjectMode.Two);
            LightConsole console13 = gsPlay.AddLinkedLightConsole(console12, console12.texture, GridToPosition(205, 39));
            console13.SetPlayerMode(PlayerObjectMode.One);
            LightningChain clc2 = gsPlay.AddLightning(console12.position, console13.position, Color.White);
            console12.AddEvent(new EventTrigger(this, clc2));
            console13.AddEvent(new EventTrigger(this, clc2)); 

            LightConsole console14 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/LightConsole"), GridToPosition(191, 34));
            console14.SetPlayerMode(PlayerObjectMode.One);
            LightConsole console15 = gsPlay.AddLinkedLightConsole(console14, console14.texture, GridToPosition(191, 39));
            console15.SetPlayerMode(PlayerObjectMode.Two);
            LightningChain clc3 = gsPlay.AddLightning(console14.position, console15.position, Color.White);
            console14.AddEvent(new EventTrigger(this, clc3));
            console15.AddEvent(new EventTrigger(this, clc3)); 

            LightConsole console16 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/LightConsole"), GridToPosition(157, 69));
            console16.SetPlayerMode(PlayerObjectMode.Two);
            LightConsole console17 = gsPlay.AddLinkedLightConsole(console16, console16.texture, GridToPosition(162, 64));
            console17.SetPlayerMode(PlayerObjectMode.One);
            LightningChain clc4 = gsPlay.AddLightning(console16.position, console17.position, Color.White);
            console16.AddEvent(new EventTrigger(this, clc4));
            console17.AddEvent(new EventTrigger(this, clc4)); 

            LightConsole console18 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/LightConsole"), GridToPosition(190, 79));
            console18.SetPlayerMode(PlayerObjectMode.Two);
            LightConsole console19 = gsPlay.AddLinkedLightConsole(console18, console18.texture, GridToPosition(185, 84));
            console19.SetPlayerMode(PlayerObjectMode.One);
            LightningChain clc5 = gsPlay.AddLightning(console18.position, console19.position, Color.White);
            console18.AddEvent(new EventTrigger(this, clc5));
            console19.AddEvent(new EventTrigger(this, clc5)); 

            LightConsole console20 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/LightConsole"), GridToPosition(189, 69));
            console20.SetPlayerMode(PlayerObjectMode.One);
            LightConsole console21 = gsPlay.AddLinkedLightConsole(console20, console20.texture, GridToPosition(209, 69));
            console21.SetPlayerMode(PlayerObjectMode.Two);
            LightningChain clc6 = gsPlay.AddLightning(console20.position, console21.position, Color.White);
            console20.AddEvent(new EventTrigger(this, clc6));
            console21.AddEvent(new EventTrigger(this, clc6)); 

            LightConsole console22 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/LightConsole"), GridToPosition(212, 39));
            LightConsole console23 = gsPlay.AddLinkedLightConsole(console22, console22.texture, GridToPosition(198, 49));
            LightningChain clc7 = gsPlay.AddLightning(console22.position, console23.position, Color.White);
            console22.AddEvent(new EventTrigger(this, clc1));
            console23.AddEvent(new EventTrigger(this, clc1));


            AudioManager.PlayMusic("Cave");

        }

        public override void ActivateEvent(int eventID, TriggerState triggerState)
        {
            switch (eventID)
            {
                case 0:
                    if (triggerState == TriggerState.Triggered && !gsPlay.gameStateManager.screenTransition)
                    {
                        bool isGood = true;
                        foreach (Portal portal in gsPlay.portalList)
                        {
                            if (portal.goodMode == false) isGood = false;
                        }
                        if (isGood) gsPlay.gameStateManager.goodness++;
                        else gsPlay.gameStateManager.goodness--;
                        gsPlay.player1.visible = false;
                        gsPlay.player2.visible = false;
                        gsPlay.gameStateManager.currentLevel = 7;
                        //gsPlay.gameStateManager.SwitchToGSPlay();
                        gsPlay.gameStateManager.TransitionToGameState(gsPlay, GameStateType.GSPlay, 30);
                    }
                    break;
            }
        }
    }
}
