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


        public Level7(GSPlay gsPlay)
            : base(gsPlay)
        {

            levelDataTexture = gsPlay.LoadTexture("Levels/level7Data");
            player1Pos = GridToPosition(5, 270);
            player2Pos = player1Pos;

            portalPos = GridToPosition(211, 12);
            pItemPos = GridToPosition(204, 12);

            SetupLevel();

            SpriteIMG s = new SpriteIMG(gsPlay.LoadTexture("bgSky"), new Vector2(levelWidth / 2, levelHeight / 2));
            gsPlay.AddParallax(s, .5f);

            atmosphereLight = new Color(98, 102, 115);

            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(95, 280)), .8f);                      //box under backhoe
            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(161, 290)), .8f);                     //box in pit under platform
            gsPlay.AddSwitch(new EventTrigger(this, gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(153, 275)), Level.GridToPosition(new Point(153, 282)))), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(126, 265)));     //crane switch and platform

            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(195, 278)), .8f);                     //box before saw machine

            gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(235, 277)), Level.GridToPosition(new Point(246, 277)));       //platform over saw machine

            gsPlay.AddBouncyBall(0, gsPlay.LoadTexture("TestSprites/pickUp"), Level.GridToPosition(new Point(307, 286)));                     //ball under building

            gsPlay.AddSwitch(new EventTrigger(this, gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(309, 283)), Level.GridToPosition(new Point(313, 283)))), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(314, 281)));     //first horiz door

            gsPlay.AddSwitch(new EventTrigger(this, gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(323, 283)), Level.GridToPosition(new Point(319, 283)))), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(322, 289)));     //2nd horiz door

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

            gsPlay.AddSwitch(new EventTrigger(this, gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(384, 216)), Level.GridToPosition(new Point(360, 216)))), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(387, 214)));     //platform to crane 2 switch

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

            gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(300, 126)), Level.GridToPosition(new Point(319, 126)));       //slide under wind

        }

        public override void ActivateEvent(int eventID, TriggerState triggerState)
        {
            switch (eventID)
            {
                case 0:
                    if (triggerState == TriggerState.Triggered)
                    {
                        gsPlay.gameStateManager.currentLevel = 1;
                        gsPlay.gameStateManager.SwitchToGSPlay();
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
            }

        }
    }
}