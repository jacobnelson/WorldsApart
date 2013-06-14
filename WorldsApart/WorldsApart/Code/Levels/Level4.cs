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

            //player1Pos = GridToPosition(229, 48);
            //player2Pos = GridToPosition(229, 48);

            portalPos = GridToPosition(232, 48);
            pItemPos = GridToPosition(194, 48);

            SetupLevel();

            atmosphereLight = new Color(0, 0, 0);

            gsPlay.AddPointLight(gsPlay.LoadTexture("ShaderAssets/pointLight"), GridToPosition(240, 44), new Vector2(4));

            gsPlay.AddPointLight(gsPlay.LoadTexture("ShaderAssets/pointLight"), GridToPosition(15, 10), new Vector2(5));
            player1Light = gsPlay.AddPointLight(gsPlay.LoadTexture("ShaderAssets/pointLight"), Vector2.Zero, new Vector2(2));
            player1Light.SetGlowing(2, 2.4f, 120);
            player2Light = gsPlay.AddPointLight(gsPlay.LoadTexture("ShaderAssets/pointLight"), Vector2.Zero, new Vector2(2));
            player2Light.SetGlowing(2, 2.4f, 120);

            //Doors and switches
            Door d1 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(47, 42)), OpenState.Closed);
            FlipSwitch s1 = gsPlay.AddSwitch(new EventTrigger(this, d1), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(28, 54)));
            s1.SetPlayerMode(PlayerObjectMode.One);
            Door d2 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(108, 58)), OpenState.Closed);
            FlipSwitch s2 = gsPlay.AddSwitch(new EventTrigger(this, d2), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(62, 57)));
            s2.SetPlayerMode(PlayerObjectMode.One);
            Door d3 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(100, 39)), OpenState.Closed);
            FlipSwitch s3 = gsPlay.AddSwitch(new EventTrigger(this, d3), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(103, 40)));
            s3.SetPlayerMode(PlayerObjectMode.Two);
            Door d4 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(134, 23)), OpenState.Closed);
            FlipSwitch s4 = gsPlay.AddSwitch(new EventTrigger(this, d4), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(120, 33)));
            s4.SetPlayerMode(PlayerObjectMode.One);
            Door d5 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(159, 23)), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d5), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(156, 29)));
            Door d6 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(164, 28)) + new Vector2(-16, 0), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d6), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(162, 24)));
            Door d7 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(188, 33)) + new Vector2(-16, 0), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d7), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(192, 29)));
            Door d8 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(178, 28)), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d8), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(176, 29)));
            Door d9 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(169, 33)) + new Vector2(-16, 0), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d9), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(172, 39)));
            Door d10 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(174, 63)), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d10), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(173, 69)));
            Door d11 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(187, 73)), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d11), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(173, 74)));
            Door d12 = gsPlay.AddOpeningDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(180, 73)), GridToPosition(new Point(180, 68)), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d12), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(173, 74)));
            Door d13 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(171, 78)), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d13), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(174, 79)));
            Door d14 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(197, 78)) + new Vector2(-16, 0), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d14), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(199, 74)));
            Door d15 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(202, 58)), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d15), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(200, 64)));
            Door d16 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(212, 63)), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d16), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(209, 60)));
            Door d17 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(211, 38)) + new Vector2(-16, 0), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d17), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(209, 44)));
            Door d26 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(212, 38)) + new Vector2(-16, 0), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d26), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(209, 44)));
            Door d27 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(213, 38)) + new Vector2(-16, 0), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d27), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(209, 44)));
            Door d28 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(214, 38)) + new Vector2(-16, 0), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d28), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(209, 44)));
            Door d29 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(215, 38)) + new Vector2(-16, 0), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d29), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(209, 44)));
            Door d30 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(216, 38)) + new Vector2(-16, 0), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d30), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(209, 44)));
            Door d31 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(189, 48)), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d31), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(214, 39)));
            Door d32 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(200, 48)), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d32), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(214, 39)));


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
            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(74, 67)), .8f);

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
            LightConsole console1 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/winSpot"), GridToPosition(15, 58));
            console1.AddLight(player1Light);
            console1.SetPlayerMode(PlayerObjectMode.One);
            LightConsole console2 = gsPlay.AddLinkedLightConsole(console1, console1.texture, GridToPosition(0, 0));
            console2.AddLight(player2Light);
            console2.SetPlayerMode(PlayerObjectMode.One);

            LightConsole console3 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/winSpot"), GridToPosition(30, 45));
            LightConsole console4 = gsPlay.AddLinkedLightConsole(console3, console3.texture, GridToPosition(0, 0));

            LightConsole console5 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/winSpot"), GridToPosition(51, 67));
            LightConsole console6 = gsPlay.AddLinkedLightConsole(console5, console5.texture, GridToPosition(0, 0));

            LightConsole console7 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/winSpot"), GridToPosition(94, 59));
            gsPlay.AddLinkedLightConsole(console7, console7.texture, GridToPosition(0, 0));

            LightConsole console8 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/winSpot"), GridToPosition(105, 52));
            gsPlay.AddLinkedLightConsole(console8, console8.texture, GridToPosition(0, 0));

            LightConsole console9 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/winSpot"), GridToPosition(129, 23));
            gsPlay.AddLinkedLightConsole(console9, console9.texture, GridToPosition(0, 0));

            LightConsole console10 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/winSpot"), GridToPosition(147, 24));
            console10.SetPlayerMode(PlayerObjectMode.One);
            LightConsole console11 = gsPlay.AddLinkedLightConsole(console10, console10.texture, GridToPosition(147, 29));
            console11.SetPlayerMode(PlayerObjectMode.Two);

            LightConsole console12 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/winSpot"), GridToPosition(205, 24));
            console12.SetPlayerMode(PlayerObjectMode.Two);
            LightConsole console13 = gsPlay.AddLinkedLightConsole(console12, console12.texture, GridToPosition(205, 39));
            console13.SetPlayerMode(PlayerObjectMode.One);

            LightConsole console14 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/winSpot"), GridToPosition(191, 34));
            console14.SetPlayerMode(PlayerObjectMode.One);
            LightConsole console15 = gsPlay.AddLinkedLightConsole(console14, console14.texture, GridToPosition(191, 39));
            console15.SetPlayerMode(PlayerObjectMode.Two);

            LightConsole console16 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/winSpot"), GridToPosition(157, 69));
            console16.SetPlayerMode(PlayerObjectMode.Two);
            LightConsole console17 = gsPlay.AddLinkedLightConsole(console16, console16.texture, GridToPosition(162, 64));
            console17.SetPlayerMode(PlayerObjectMode.One);

            LightConsole console18 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/winSpot"), GridToPosition(190, 79));
            console18.SetPlayerMode(PlayerObjectMode.Two);
            LightConsole console19 = gsPlay.AddLinkedLightConsole(console18, console18.texture, GridToPosition(185, 84));
            console19.SetPlayerMode(PlayerObjectMode.One);

            LightConsole console20 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/winSpot"), GridToPosition(189, 69));
            console20.SetPlayerMode(PlayerObjectMode.One);
            LightConsole console21 = gsPlay.AddLinkedLightConsole(console20, console20.texture, GridToPosition(209, 69));
            console21.SetPlayerMode(PlayerObjectMode.Two);

            LightConsole console22 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/winSpot"), GridToPosition(212, 39));
            LightConsole console23 = gsPlay.AddLinkedLightConsole(console22, console22.texture, GridToPosition(198, 49));

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
