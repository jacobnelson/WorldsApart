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

using System.Diagnostics;

namespace WorldsApart.Code.Levels
{

    class Level7 : Level
    {
        //Barf!
        /*InventoryItem item1;
        InventoryItem item2;
        InventoryItem item3;
        TriggerArea item1Area;
        TriggerArea item2Area;
        TriggerArea item3Area;
        TriggerArea final1;
        TriggerArea final2;
        TriggerArea final3;
        bool final1Unlock = false;
        bool final2Unlock = false;
        bool final3Unlock = false;
        Door finalDoor;
        FlipSwitch timedSwitch;

        PointLight player1Light;
        PointLight player2Light;
         * */

        Door d3;
        Door d5;
        Door d7;
        Door d8;
        Door d9;
        Door d10;
        Door d12;
        Door d13;
        CircularPlatform c3;
        CircularPlatform c4;
        CircularPlatform c5;
        CircularPlatform c6;
        MovingPlatform p1;
        MovingPlatform p2;

        float thunderCounter = 0;
        float thunderRate = 7;


        public Level7(GSPlay gsPlay)
            : base(gsPlay)
        {
           
            levelDataTexture = gsPlay.LoadTexture("Levels/level7Data");
           // player1Pos = GridToPosition(5, 280);
            player1Pos = GridToPosition(284, 148);
            player2Pos = player1Pos;

            portalPos = GridToPosition(394, 41);
            pItemPos = GridToPosition(441, 119);

            SetupLevel();

            TriggerArea update = gsPlay.AddTriggerArea(new EventTrigger(this, 99), Art.smoke, Vector2.Zero);
            update.visible = false;

            Sprite cameraTarget = new Sprite(GridToPosition(239, 278));
            gsPlay.cameraPlayer1.AddTarget(cameraTarget);
            gsPlay.cameraPlayer2.AddTarget(cameraTarget);

            SpriteIMG s = new SpriteIMG(gsPlay.LoadTexture("bgSky"), new Vector2(levelWidth / 2, levelHeight / 2));
            gsPlay.AddParallax(s, .5f);

            atmosphereLight = new Color(100, 100, 100);

            Moveable m1 = gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(95, 280)), .8f);                      //box under backhoe

            Moveable m2 = gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(161, 290)), .8f);                     //box in pit under platform

            gsPlay.AddSwitch(new EventTrigger(this, gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(153, 275)), Level.GridToPosition(new Point(153, 282)))), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(126, 265)));     //crane switch and platform

            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(195, 278)), .8f);                     //box before saw machine

            gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(235, 277)), Level.GridToPosition(new Point(243, 277)));       //platform over saw machine

            gsPlay.AddBouncyBall(0, gsPlay.LoadTexture("TestSprites/pickUp"), Level.GridToPosition(new Point(307, 286)));                     //ball under building

            FlipSwitch s1 = gsPlay.AddSwitch(new EventTrigger(this, gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(309, 283)), Level.GridToPosition(new Point(313, 283)))), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(314, 281)));     //first horiz door
            s1.SetPlayerMode(PlayerObjectMode.One);

            FlipSwitch s2 = gsPlay.AddSwitch(new EventTrigger(this, gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(323, 283)), Level.GridToPosition(new Point(319, 283)))), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(322, 288)));     //2nd horiz door
            s2.SetPlayerMode(PlayerObjectMode.Two);

            gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(290, 282)), Level.GridToPosition(new Point(290, 209)));       //elevator

            Door d1 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(332, 213)), OpenState.Closed);                        //door to let ball in
            gsPlay.AddSwitch(new EventTrigger(this, d1), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(328, 208)));

            Door d2 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(332, 207)), OpenState.Closed);                        //door opened by ball
            gsPlay.AddButton(new EventTrigger(this, d2), 1, gsPlay.LoadTexture("TestSprites/button"), Level.GridToPosition(new Point(335, 216)));

            d3 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(370, 207)), OpenState.Closed);                               //fading doors over pit
            d5 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(376, 207)), OpenState.Open);
            FlipSwitch FS1 = gsPlay.AddSwitch(new EventTrigger(this, 1), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(374, 202)));

            gsPlay.AddSwitch(new EventTrigger(this, gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(365, 210)), Level.GridToPosition(new Point(381, 210)))), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(364, 202)));     //platform with fading doors 1

            Door d4 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(384, 201)), OpenState.Closed);                        //ease of access
            gsPlay.AddSwitch(new EventTrigger(this, d4), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(384, 208)));

            gsPlay.AddSwitch(new EventTrigger(this, gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(384, 216)), Level.GridToPosition(new Point(360, 216)))), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(393, 212)));     //platform to crane 2 switch

            gsPlay.AddSwitch(new EventTrigger(this, gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(343, 198)), Level.GridToPosition(new Point(343, 206)))), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(349, 214)));     //crane 2 switch and platform

            gsPlay.AddSwitch(new EventTrigger(this, gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(252, 185)), Level.GridToPosition(new Point(267, 185)))), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(264, 178)));     //crane 3

            Door d6 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(306, 177)), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d6), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(267, 183)));                       //door switch under crane 3

            d7 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(315, 177)), OpenState.Closed);                         //multiple fading doors 1
            d8 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(321, 177)), OpenState.Closed);
            d9 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(327, 177)), OpenState.Open);
            d10 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(333, 177)), OpenState.Open);
            FlipSwitch FS2 = gsPlay.AddSwitch(new EventTrigger(this, 2), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(321, 172)));
            FlipSwitch FS3 = gsPlay.AddSwitch(new EventTrigger(this, 3), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(327, 172)));

            Door d11 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(333, 171)), OpenState.Closed);
            gsPlay.AddSwitch(new EventTrigger(this, d11), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(335, 178)));

            gsPlay.AddCircularPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(275, 145)), 128, 240);    //circular platform 1
            CircularPlatform c1 = gsPlay.AddCircularPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(271, 137)), 128, 240);    //circular platform 2
            c1.increment = -c1.increment;
            gsPlay.AddCircularPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(275, 129)), 128, 240);    //circular platform 3

            gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(300, 126)), Level.GridToPosition(new Point(319, 126)));       //slide under wind

            d12 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(341, 127)), OpenState.Closed);                               //fading platforms to portal item
            d13 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(354, 127)), OpenState.Open);
            FlipSwitch FS4 = gsPlay.AddSwitch(new EventTrigger(this, 4), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(347, 112)));

            Door d14 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(356, 111)), OpenState.Closed);                               //ease of access
            gsPlay.AddSwitch(new EventTrigger(this, d14), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(360, 124)));

            gsPlay.AddCircularPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(322, 95)), 256, 360);                         

            CircularPlatform c2 = gsPlay.AddActivateCircularPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(300, 98)), 256, 360);
            c2.increment = -c2.increment;
            FlipSwitch FS5 = gsPlay.AddSwitch(new EventTrigger(this, c2), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(322, 95)));
            FS5.SetPlayerMode(PlayerObjectMode.Two);
            Moveable m3 = gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(302, 97)), .8f);
            m3.SetPlayerMode(PlayerObjectMode.One);

            c3 = gsPlay.AddActivateCircularPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(338, 98)), 256, 360);
            c3.increment = -c3.increment;
            c3.SetPlayerMode(PlayerObjectMode.Two);
            c4 = gsPlay.AddActivateCircularPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(348, 98)), 256, 360);
            c4.angle = (float)Math.PI;
            c4.startAngle = c4.angle;
            c4.SetPlayerMode(PlayerObjectMode.One);

            c5 = gsPlay.AddActivateCircularPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(343, 78)), 256, 360);
            c5.increment = -c5.increment;
            c5.SetPlayerMode(PlayerObjectMode.Two);
            c6 = gsPlay.AddActivateCircularPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(343, 78)), 256, 360);
            c5.angle = (float)Math.PI;
            c5.startAngle = c5.angle;
            c6.SetPlayerMode(PlayerObjectMode.One);

            Button b1 = gsPlay.AddButton(new EventTrigger(this, 5), 1, gsPlay.LoadTexture("TestSprites/button"), Level.GridToPosition(new Point(287, 99)));

            Door p4 = gsPlay.AddOpeningDoor(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(80, 116)), Level.GridToPosition(new Point(80, 113)), OpenState.Closed);
            gsPlay.AddBouncyBall(0, gsPlay.LoadTexture("TestSprites/pickUp"), Level.GridToPosition(new Point(80, 112)));

            Door p5 = gsPlay.AddOpeningDoor(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(82, 116)), Level.GridToPosition(new Point(82, 113)), OpenState.Closed);
            gsPlay.AddBouncyBall(0, gsPlay.LoadTexture("TestSprites/pickUp"), Level.GridToPosition(new Point(82, 112)));

            p1 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(376, 126)), Level.GridToPosition(new Point(402, 126)));
            p1.SetPlayerMode(PlayerObjectMode.One);

            p2 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(376, 120)), Level.GridToPosition(new Point(402, 120)));
            p2.SetPlayerMode(PlayerObjectMode.Two);

            Button b2 = gsPlay.AddButton(new EventTrigger(this, p1), 2, gsPlay.LoadTexture("TestSprites/button"), Level.GridToPosition(new Point(81, 115)));
            b2.AddEvent(new EventTrigger(this, p2));


            FlipSwitch FS6 = gsPlay.AddSwitch(new EventTrigger(this, p4), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(376, 118)));
            FS6.SetPlayerMode(PlayerObjectMode.Two);

            
            FlipSwitch FS7 = gsPlay.AddSwitch(new EventTrigger(this, p5), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(376, 124)));
            FS7.SetPlayerMode(PlayerObjectMode.One);


            MovingPlatform p3 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(380, 44)), Level.GridToPosition(new Point(359, 44)));

            Door p6 = gsPlay.AddOpeningDoor(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(90, 116)), Level.GridToPosition(new Point(90, 113)), OpenState.Closed);
            gsPlay.AddBouncyBall(0, gsPlay.LoadTexture("TestSprites/pickUp"), Level.GridToPosition(new Point(90, 112)));

            Door p7 = gsPlay.AddOpeningDoor(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(92, 116)), Level.GridToPosition(new Point(92, 113)), OpenState.Closed);
            gsPlay.AddBouncyBall(0, gsPlay.LoadTexture("TestSprites/pickUp"), Level.GridToPosition(new Point(92, 112)));

            Button b3 = gsPlay.AddButton(new EventTrigger(this, p3), 2, gsPlay.LoadTexture("TestSprites/button"), Level.GridToPosition(new Point(91, 115)));

            FlipSwitch FS8 = gsPlay.AddSwitch(new EventTrigger(this, p6), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(355, 42)));
            FS8.SetPlayerMode(PlayerObjectMode.Two);

            FlipSwitch FS9 = gsPlay.AddSwitch(new EventTrigger(this, p7), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(355, 42)));
            FS9.SetPlayerMode(PlayerObjectMode.One);
            
        }

        public override void ActivateEvent(int eventID, TriggerState triggerState)
        {
            switch (eventID)
            {
                case 0:
                    if (triggerState == TriggerState.Triggered)
                    {
                        gsPlay.gameStateManager.currentLevel = 0;
                        //gsPlay.gameStateManager.SwitchToGSPlay();
                        gsPlay.gameStateManager.TransitionToGameState(gsPlay, GameStateType.GSPlay, 30);
                    }
                    break;
                case 1:
                    if (triggerState == TriggerState.Triggered)
                    {
                        d3.ActivateEvent(TriggerState.Triggered);
                        d5.ActivateEvent(TriggerState.Untriggered);
                    }
                    else
                    {
                        d3.ActivateEvent(TriggerState.Untriggered);
                        d5.ActivateEvent(TriggerState.Triggered);
                    }
                    break;
                case 2:
                    if (triggerState == TriggerState.Triggered)
                    {
                        d7.ActivateEvent(TriggerState.Triggered);
                        d9.ActivateEvent(TriggerState.Untriggered);
                    }
                    else
                    {
                        d7.ActivateEvent(TriggerState.Untriggered);
                        d9.ActivateEvent(TriggerState.Triggered);
                    }
                    break;
                case 3:
                    if (triggerState == TriggerState.Triggered)
                    {
                        d8.ActivateEvent(TriggerState.Triggered);
                        d10.ActivateEvent(TriggerState.Untriggered);
                    }
                    else
                    {
                        d8.ActivateEvent(TriggerState.Untriggered);
                        d10.ActivateEvent(TriggerState.Triggered);
                    }
                    break;
                case 4:
                    if (triggerState == TriggerState.Triggered)
                    {
                        d12.ActivateEvent(TriggerState.Triggered);
                        d13.ActivateEvent(TriggerState.Untriggered);
                    }
                    else
                    {
                        d12.ActivateEvent(TriggerState.Untriggered);
                        d13.ActivateEvent(TriggerState.Triggered);
                    }
                    break;
                case 5:
                    if (triggerState == TriggerState.Triggered)
                    {
                        c3.ActivateEvent(TriggerState.Triggered);
                        c4.ActivateEvent(TriggerState.Triggered);
                        c5.ActivateEvent(TriggerState.Triggered);
                        c6.ActivateEvent(TriggerState.Triggered);
                    }
                    else
                    {
                        c3.ActivateEvent(TriggerState.Untriggered);
                        c4.ActivateEvent(TriggerState.Untriggered);
                        c5.ActivateEvent(TriggerState.Untriggered);
                        c6.ActivateEvent(TriggerState.Untriggered);
                    }
                    break;
                case 6:
                    //if (triggerState == TriggerState.Triggered)
                    //{
                    //    p1.ActivateEvent(TriggerState.Triggered);
                    //    p2.ActivateEvent(TriggerState.Triggered);
                    //}
                    //else
                    //{
                    //    p1.ActivateEvent(TriggerState.Untriggered);
                    //    p2.ActivateEvent(TriggerState.Untriggered);
                    //}
                    break;
                case 99:

                    for (int i = 0; i < 2; i++)
                    {
                        gsPlay.AddRain(true);
                        gsPlay.AddRain(false);
                    }

                    if (atmosphereLight != new Color(100, 100, 100))
                    {
                        if (atmosphereLight.R > 100) atmosphereLight.R -= 2;
                        if (atmosphereLight.G > 100) atmosphereLight.G -= 2;
                        if (atmosphereLight.B > 100) atmosphereLight.B -= 2;

                        if (atmosphereLight.R < 100) atmosphereLight.R = 100;
                        if (atmosphereLight.G < 100) atmosphereLight.G = 100;
                        if (atmosphereLight.B < 100) atmosphereLight.B = 100;
                    }
                    else
                    {
                        thunderCounter += Time.GetSeconds();
                    }

                    
                    if (thunderCounter >= thunderRate)
                    {
                        atmosphereLight = Color.White;
                        thunderCounter = Mathness.RandomNumber(-1f, 1f);
                    }

                    break;
            }

        }
    }
}