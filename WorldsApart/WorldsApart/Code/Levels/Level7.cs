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


            player1Pos = GridToPosition(5, 280);
            player2Pos = GridToPosition(7, 280);

            //player1Pos = GridToPosition(322, 117);
            //player2Pos = player1Pos;

            portalPos = GridToPosition(394, 41);
            pItemPos = GridToPosition(441, 119);

            SetupLevel();

            leftLimit = 32;
            rightLimit = levelWidth;

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

            MovingPlatform underCrane = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(153, 275)), Level.GridToPosition(new Point(153, 282)));
            FlipSwitch crane = gsPlay.AddSwitch(new EventTrigger(this, underCrane), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(126, 265)));                                                  //crane switch and platform
            LightningChain lc1 = gsPlay.AddLightning(crane.position, GridToPosition(130, 265), Color.Red);
            lc1.AddVertex(GridToPosition(137, 274));
            lc1.AddVertex(GridToPosition(141, 274));
            lc1.AddVertex(underCrane.position);
            lc1.ConvertEndPointToTarget(underCrane);
            crane.AddEvent(new EventTrigger(this, lc1));
            

            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(195, 278)), .8f);                     //box before saw machine

            gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(235, 277)), Level.GridToPosition(new Point(243, 277)));       //platform over saw machine

            gsPlay.AddBouncyBall(0, gsPlay.LoadTexture("TestSprites/pickUp"), Level.GridToPosition(new Point(307, 286)));                     //ball under building

            Door leftBasement = gsPlay.AddOpeningDoor(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(313, 283)), Level.GridToPosition(new Point(309, 283)), OpenState.Closed);
            FlipSwitch s1 = gsPlay.AddSwitch(new EventTrigger(this, leftBasement), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(314, 281)));     //first horiz door
            s1.SetPlayerMode(PlayerObjectMode.One);
            LightningChain lc2 = gsPlay.AddLightning(s1.position, leftBasement.position, Color.Red);
            lc2.ConvertEndPointToTarget(leftBasement);
            s1.AddEvent(new EventTrigger(this, lc2));

            Door rightBasement = gsPlay.AddOpeningDoor(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(319, 283)), Level.GridToPosition(new Point(323, 283)), OpenState.Closed);
            FlipSwitch s2 = gsPlay.AddSwitch(new EventTrigger(this, rightBasement), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(322, 288)));     //2nd horiz door
            s2.SetPlayerMode(PlayerObjectMode.Two);
            LightningChain lc3 = gsPlay.AddLightning(s2.position, rightBasement.position, Color.Red);
            lc3.ConvertEndPointToTarget(rightBasement);
            s2.AddEvent(new EventTrigger(this, lc3));

            gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(290, 282)), Level.GridToPosition(new Point(290, 209)));       //elevator

            Door d1 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(332, 213)), OpenState.Closed);                        //door to let ball in
            FlipSwitch ballDoor =  gsPlay.AddSwitch(new EventTrigger(this, d1), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(328, 208)));
            LightningChain lc4 = gsPlay.AddLightning(ballDoor.position, GridToPosition(328, 209) + new Vector2(0, 16), Color.Red);
            lc4.AddVertex(GridToPosition(332, 211) + new Vector2(0, -16));
            lc4.AddVertex(GridToPosition(332, 211) + new Vector2(0, 0));
            //lc6.ConvertEndPointToTarget(d5);
            ballDoor.AddEvent(new EventTrigger(this, lc4));

            Door d2 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(332, 207)), OpenState.Closed);                        //door opened by ball
            Button ballOn = gsPlay.AddButton(new EventTrigger(this, d2), 1, gsPlay.LoadTexture("TestSprites/button"), Level.GridToPosition(new Point(335, 216)));
            LightningChain lc5 = gsPlay.AddLightning(ballOn.position,  GridToPosition(338, 216), Color.Red);
            lc5.AddVertex(GridToPosition(338, 210));
            lc5.AddVertex(GridToPosition(332, 210));
            lc5.AddVertex(GridToPosition(332, 209) + new Vector2(0, 0));
            ballOn.AddEvent(new EventTrigger(this, lc5));
            //338, 216
            //338, 210
            //332, 210
            //332, 209

            d3 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(370, 207)), OpenState.Closed);                               //fading doors over pit
            d5 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(376, 207)), OpenState.Open);
            FlipSwitch FS1 = gsPlay.AddMultiSwitch(new EventTrigger(this, 1), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(373, 202)));
            LightningChain lc6 = gsPlay.AddLightning(FS1.position, GridToPosition(370, 204) + new Vector2(0, 16), Color.Red);
            lc6.AddVertex(GridToPosition(370, 204) + new Vector2(0, 32));
            FS1.AddEvent(new EventTrigger(this, lc6));
            LightningChain lc7 = gsPlay.AddLightning(FS1.position, GridToPosition(376, 204) + new Vector2(0, 16), Color.Red);
            lc7.AddVertex(GridToPosition(376, 204) + new Vector2(0, 32));
            FS1.AddEvent(new EventTrigger(this, lc7));
            lc6.defaultActive = true;
            lc6.SetActive(true);

            MovingPlatform pitPlatform1 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(365, 210)), Level.GridToPosition(new Point(381, 210)));
            FlipSwitch pitPlatform1Switch = gsPlay.AddSwitch(new EventTrigger(this, pitPlatform1), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(364, 202)));     //platform with fading doors 1
            LightningChain lc8 = gsPlay.AddLightning(pitPlatform1Switch.position, GridToPosition(364, 210), Color.Red);
            lc8.AddVertex(pitPlatform1.position);
            lc8.ConvertEndPointToTarget(pitPlatform1);
            pitPlatform1Switch.AddEvent(new EventTrigger(this, lc8));

            Door d4 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(384, 201)), OpenState.Closed);                        //ease of access
            FlipSwitch easeOfAccess =  gsPlay.AddOnSwitch(new EventTrigger(this, d4), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(384, 208)));
            LightningChain lc9 = gsPlay.AddLightning(easeOfAccess.position, GridToPosition(384, 203) + new Vector2(0, 16), Color.Red);
            lc9.AddVertex(GridToPosition(384, 203) + new Vector2(0, 0));
            lc9.defaultActive = true;
            lc9.SetActive(true);
            easeOfAccess.AddEvent(new EventTrigger(this, lc9));

            MovingPlatform toCrane2p = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(384, 216)), Level.GridToPosition(new Point(360, 216)));
            FlipSwitch toCrane2s = gsPlay.AddSwitch(new EventTrigger(this, toCrane2p), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(393, 212)));     //platform to crane 2 switch
            LightningChain lc10 = gsPlay.AddLightning(toCrane2s.position, GridToPosition(393, 216), Color.Red);
            lc10.AddVertex(GridToPosition(384, 216));
            lc10.AddVertex(toCrane2p.position);
            lc10.ConvertEndPointToTarget(toCrane2p);
            toCrane2s.AddEvent(new EventTrigger(this, lc10));
            //393, 216
            //384, 216

            MovingPlatform crane2 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(343, 198)), Level.GridToPosition(new Point(343, 206)));
            FlipSwitch crane2switch = gsPlay.AddSwitch(new EventTrigger(this, crane2), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(350, 214)));     //crane 2 switch and platform
            LightningChain lc11 = gsPlay.AddLightning(crane2switch.position, GridToPosition(343, 214), Color.Red);
            lc11.AddVertex(crane2.position);
            lc11.ConvertEndPointToTarget(crane2);
            crane2switch.AddEvent(new EventTrigger(this, lc11));

            MovingPlatform crane3 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(252, 185)), Level.GridToPosition(new Point(267, 185)));
            FlipSwitch crane3switch = gsPlay.AddSwitch(new EventTrigger(this, crane3), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToCenterPosition(new Point(259, 178)) + new Vector2(0, -16));     //crane 3
            LightningChain lc12 = gsPlay.AddLightning(crane3switch.position, GridToCenterPosition(259,180), Color.Red);
            lc12.AddVertex(crane3.position);
            lc12.ConvertEndPointToTarget(crane3);
            crane3switch.AddEvent(new EventTrigger(this, lc12));

            Door d6 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(306, 177)), OpenState.Closed);
            FlipSwitch d6s = gsPlay.AddOnSwitch(new EventTrigger(this, d6), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(267, 183)));                       //door switch under crane 3
            LightningChain lc13 = gsPlay.AddLightning(d6s.position, GridToPosition(282, 183), Color.Red);
            lc13.AddVertex(GridToPosition(282, 168));
            lc13.AddVertex(GridToPosition(306, 168));
            lc13.AddVertex(GridToPosition(306, 175));
            d6s.AddEvent(new EventTrigger(this, lc13));
            lc13.defaultActive = true;
            lc13.SetActive(true);
            //267, 185
            //282, 185
            //282, 168
            //306, 168
            //306, 175

            d7 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(315, 177)), OpenState.Closed);                         //multiple fading doors 1
            d8 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(321, 177)), OpenState.Closed);
            d9 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(327, 177)), OpenState.Open);
            d10 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(333, 177)), OpenState.Open);
            FlipSwitch FS2 = gsPlay.AddMultiSwitch(new EventTrigger(this, 2), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(321, 172)));
            FlipSwitch FS3 = gsPlay.AddMultiSwitch(new EventTrigger(this, 3), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(327, 172)));
            LightningChain lc14 = gsPlay.AddLightning(FS2.position, GridToPosition(315, 174) + new Vector2(0, 16), Color.Red);
            lc14.AddVertex(GridToPosition(315, 174) + new Vector2(0, 32));
            FS2.AddEvent(new EventTrigger(this, lc14));
            LightningChain lc15 = gsPlay.AddLightning(FS3.position, GridToPosition(321, 174) + new Vector2(0, 16), Color.Red);
            lc15.AddVertex(GridToPosition(321, 174) + new Vector2(0, 32));
            FS3.AddEvent(new EventTrigger(this, lc15));
            LightningChain lc16 = gsPlay.AddLightning(FS2.position, GridToPosition(327, 174) + new Vector2(0, 16), Color.Red);
            lc16.AddVertex(GridToPosition(327, 174) + new Vector2(0, 32));
            FS2.AddEvent(new EventTrigger(this, lc16));
            lc14.defaultActive = true;
            lc14.SetActive(true);
            LightningChain lc17 = gsPlay.AddLightning(FS3.position, GridToPosition(333, 174) + new Vector2(0, 16), Color.Red);
            lc17.AddVertex(GridToPosition(333, 174) + new Vector2(0, 32));
            FS3.AddEvent(new EventTrigger(this, lc17));
            lc15.defaultActive = true;
            lc15.SetActive(true);

            Door d11 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(333, 171)), OpenState.Closed);
            FlipSwitch d11s = gsPlay.AddOnSwitch(new EventTrigger(this, d11), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(335, 178)));
            LightningChain lc18 = gsPlay.AddLightning(d11s.position, GridToPosition(333, 173) + new Vector2(0, 16), Color.Red);
            lc18.AddVertex(GridToPosition(333, 173) + new Vector2(0, 0));
            d11s.AddEvent(new EventTrigger(this, lc18));
            lc18.defaultActive = true;
            lc18.SetActive(true);

            gsPlay.AddCircularPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(275, 145)), 128, 240);    //circular platform 1
            CircularPlatform c1 = gsPlay.AddCircularPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(271, 137)), 128, 240);    //circular platform 2
            c1.increment = -c1.increment;
            gsPlay.AddCircularPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(275, 129)), 128, 240);    //circular platform 3

            gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(300, 126)), Level.GridToPosition(new Point(319, 126)));       //slide under wind

            d12 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(341, 127)), OpenState.Closed);                               //fading platforms to portal item
            d13 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(354, 127)), OpenState.Open);
            FlipSwitch FS4 = gsPlay.AddMultiSwitch(new EventTrigger(this, 4), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToCenterPosition(new Point(347, 112)) + new Vector2(0, -16)); //multi-state!
            LightningChain lc19 = gsPlay.AddLightning(FS4.position, GridToCenterPosition(347, 114), Color.Red);
            lc19.AddVertex(d12.position);
            FS4.AddEvent(new EventTrigger(this, lc19));
            LightningChain lc20 = gsPlay.AddLightning(FS4.position, GridToCenterPosition(347, 114), Color.Red);
            lc20.AddVertex(d13.position);
            FS4.AddEvent(new EventTrigger(this, lc20));
            lc19.defaultActive = true;
            lc19.SetActive(true);
            

            Door d14 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(356, 111)), OpenState.Closed);                               //ease of access
            FlipSwitch d14s = gsPlay.AddOnSwitch(new EventTrigger(this, d14), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(360, 124))); 
            LightningChain lc21 = gsPlay.AddLightning(d14s.position, GridToPosition(356, 113) + new Vector2(0, 16), Color.Red);
            lc21.AddVertex(GridToPosition(356, 113) + new Vector2(0, 0));
            d14s.AddEvent(new EventTrigger(this, lc21));
            lc21.defaultActive = true;
            lc21.SetActive(true);

            gsPlay.AddCircularPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(322, 95)), 256, 360);                         
            
            CircularPlatform c2 = gsPlay.AddActivateCircularPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(300, 98)), 256, 360);
            c2.increment = -c2.increment;
            FlipSwitch FS5 = gsPlay.AddSwitch(new EventTrigger(this, c2), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(322, 95))); 
            FS5.SetPlayerMode(PlayerObjectMode.Two);
            LightningChain lc22 = gsPlay.AddLightning(FS5.position, GridToPosition(322, 98), Color.Red);
            lc22.AddVertex(GridToPosition(300, 98));
            lc22.AddVertex(GridToPosition(300, 98));
            lc22.ConvertEndPointToTarget(c2);
            FS5.AddEvent(new EventTrigger(this, lc22));
            Moveable m3 = gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveableMechanical"), Level.GridToPosition(new Point(302, 97)), .8f);
            m3.SetPlayerMode(PlayerObjectMode.One);

            c3 = gsPlay.AddActivateCircularPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(342, 98)), 256, 360);
            c3.increment = -c3.increment;
            c3.SetPlayerMode(PlayerObjectMode.Two);
            c4 = gsPlay.AddActivateCircularPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(348, 98)), 256, 360);
            c4.angle = (float)Math.PI;
            c4.startAngle = c4.angle;
            c4.SetPlayerMode(PlayerObjectMode.One);

            c5 = gsPlay.AddActivateCircularPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(343 , 78)), 256, 360);
            c5.increment = -c5.increment;
            c5.SetPlayerMode(PlayerObjectMode.Two);
            c6 = gsPlay.AddActivateCircularPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(343, 78)), 256, 360);
            c5.angle = (float)Math.PI;
            c5.startAngle = c5.angle;
            c6.SetPlayerMode(PlayerObjectMode.One);

            Button b1 = gsPlay.AddButton(new EventTrigger(this, 5), 1, gsPlay.LoadTexture("TestSprites/button"), Level.GridToPosition(new Point(287, 99)));
            //287, 87
            //343, 87
            //Down below: 343, 98
            LightningChain lc23 = gsPlay.AddLightning(b1.position, GridToPosition(287, 87), Color.Red);
            lc23.AddVertex(GridToPosition(343, 87));
            lc23.AddVertex(GridToPosition(343, 98));
            lc23.AddVertex(GridToPosition(342, 98));
            lc23.AddVertex(c3.position);
            lc23.ConvertEndPointToTarget(c3);
            b1.AddEvent(new EventTrigger(this, lc23));
            lc23.SetPlayerMode(PlayerObjectMode.Two);
            LightningChain lc24 = gsPlay.AddLightning(b1.position, GridToPosition(287, 87), Color.Red);
            lc24.AddVertex(GridToPosition(343, 87));
            lc24.AddVertex(GridToPosition(343, 98));
            lc24.AddVertex(GridToPosition(348, 98));
            lc24.AddVertex(c4.position);
            lc24.ConvertEndPointToTarget(c4);
            b1.AddEvent(new EventTrigger(this, lc24));
            lc24.SetPlayerMode(PlayerObjectMode.One);
            LightningChain lc25 = gsPlay.AddLightning(b1.position, GridToPosition(287, 87), Color.Red);
            lc25.AddVertex(GridToPosition(343, 87));
            lc25.AddVertex(GridToPosition(343, 78));
            lc25.AddVertex(c5.position);
            lc25.ConvertEndPointToTarget(c5);
            b1.AddEvent(new EventTrigger(this, lc25));
            lc25.SetPlayerMode(PlayerObjectMode.Two);
            LightningChain lc26 = gsPlay.AddLightning(b1.position, GridToPosition(287, 87), Color.Red);
            lc26.AddVertex(GridToPosition(343, 87));
            lc26.AddVertex(GridToPosition(343, 78));
            lc26.AddVertex(c6.position);
            lc26.ConvertEndPointToTarget(c6);
            b1.AddEvent(new EventTrigger(this, lc26));
            lc26.SetPlayerMode(PlayerObjectMode.One);


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
            LightningChain lcpuzzle1 = gsPlay.AddLightning(FS6.position, p2.position, Color.Aqua);
            lcpuzzle1.AddVertex(p1.position);
            lcpuzzle1.ConvertEndPointToTarget(p2);
            FS6.AddEvent(new EventTrigger(this, lcpuzzle1));

            FlipSwitch FS7 = gsPlay.AddSwitch(new EventTrigger(this, p5), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(376, 124)));
            FS7.SetPlayerMode(PlayerObjectMode.One);
            LightningChain lcpuzzle2 = gsPlay.AddLightning(FS7.position, p1.position, Color.Orange);
            lcpuzzle2.AddVertex(p1.position);
            lcpuzzle2.ConvertEndPointToTarget(p1);
            FS7.AddEvent(new EventTrigger(this, lcpuzzle2));


            MovingPlatform p3 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(359, 44)), Level.GridToPosition(new Point(380, 44)));

            Door p6 = gsPlay.AddOpeningDoor(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(90, 116)), Level.GridToPosition(new Point(90, 113)), OpenState.Closed);
            gsPlay.AddBouncyBall(0, gsPlay.LoadTexture("TestSprites/pickUp"), Level.GridToPosition(new Point(90, 112)));

            Door p7 = gsPlay.AddOpeningDoor(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(92, 116)), Level.GridToPosition(new Point(92, 113)), OpenState.Closed);
            gsPlay.AddBouncyBall(0, gsPlay.LoadTexture("TestSprites/pickUp"), Level.GridToPosition(new Point(92, 112)));

            Button b3 = gsPlay.AddButton(new EventTrigger(this, p3), 2, gsPlay.LoadTexture("TestSprites/button"), Level.GridToPosition(new Point(91, 115)));

            FlipSwitch FS8 = gsPlay.AddSwitch(new EventTrigger(this, p6), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(355, 42)));
            FS8.SetPlayerMode(PlayerObjectMode.Two);
            LightningChain lcfinal1 = gsPlay.AddLightning(FS8.position, p3.position, Color.Aqua);
            lcfinal1.ConvertEndPointToTarget(p3);
            FS8.AddEvent(new EventTrigger(this, lcfinal1));

            FlipSwitch FS9 = gsPlay.AddSwitch(new EventTrigger(this, p7), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(355, 42)));
            FS9.SetPlayerMode(PlayerObjectMode.One);
            LightningChain lcfinal2 = gsPlay.AddLightning(FS9.position, p3.position, Color.Orange);
            lcfinal2.ConvertEndPointToTarget(p3);
            FS9.AddEvent(new EventTrigger(this, lcfinal2));

            AudioManager.PlayMusic("Construction");
            
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
                        GSPlay.AddCheckpointParticles(gsPlay.player1.position, true);
                        GSPlay.AddCheckpointParticles(gsPlay.player2.position, false);
                        gsPlay.gameStateManager.currentLevel = 99;
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