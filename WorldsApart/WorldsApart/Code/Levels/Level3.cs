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
    class Level3 : Level
    {
        public Level3(GSPlay gsPlay)
            : base(gsPlay)
        {
            levelDataTexture = gsPlay.LoadTexture("Levels/level3Data");

            player1Pos = GridToPosition(23, 150);
            player2Pos = GridToPosition(20, 150);

            //player1Pos = GridToPosition(489, 32);
            //player2Pos = GridToPosition(489, 32);

            portalPos = GridToPosition(492, 32);
            pItemPos = GridToPosition(152, 90) + new Vector2(-16, 0);

            SetupLevel();

            //part1
            Door d1 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(137, 101)) + new Vector2(-16, 0), OpenState.Closed);
            FlipSwitch s1 = gsPlay.AddSwitch(new EventTrigger(this, d1), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(144, 111)));
            s1.SetPlayerMode(PlayerObjectMode.Two);
            Door bd1 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(137, 101)) + new Vector2(-16, 0), OpenState.Closed);
            bd1.SetPlayerMode(PlayerObjectMode.Two);

            Door d2 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(157, 107)) + new Vector2(-16, -16), OpenState.Closed);
            FlipSwitch s2 = gsPlay.AddSwitch(new EventTrigger(this, d2), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(148, 101)));
            s2.SetPlayerMode(PlayerObjectMode.One);
            Door bd2 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(157, 107)) + new Vector2(-16, -16), OpenState.Closed);
            bd2.SetPlayerMode(PlayerObjectMode.One);

            //part2
            Door d3 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(249, 87)), OpenState.Closed);
            FlipSwitch s3 = gsPlay.AddSwitch(new EventTrigger(this, d3), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(247, 88)) + new Vector2(-16, 0));
            s3.SetPlayerMode(PlayerObjectMode.Two);
            Door bd3 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(249, 87)), OpenState.Closed);
            bd3.SetPlayerMode(PlayerObjectMode.Two);

            Door d4 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(247, 85)) + new Vector2(-16, -16), OpenState.Closed);
            FlipSwitch s4 = gsPlay.AddSwitch(new EventTrigger(this, d4), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(252, 88)) + new Vector2(-16, 0));
            s2.SetPlayerMode(PlayerObjectMode.One);
            Door bd4 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(247, 85)) + new Vector2(-16, -16), OpenState.Closed);
            bd4.SetPlayerMode(PlayerObjectMode.One);

            Door d5 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(252, 85)) + new Vector2(-16, -16), OpenState.Closed);
            FlipSwitch s5 = gsPlay.AddSwitch(new EventTrigger(this, d5), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(247, 82)) + new Vector2(-16, 0));

            Door d6 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(247, 80)) + new Vector2(-16, -16), OpenState.Closed);
            FlipSwitch s6 = gsPlay.AddSwitch(new EventTrigger(this, d6), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(252, 78)) + new Vector2(-16, 0));

            Door d7 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(252, 76)) + new Vector2(-16, -16), OpenState.Closed);
            FlipSwitch s7 = gsPlay.AddSwitch(new EventTrigger(this, d7), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(247, 74)) + new Vector2(-16, 0));

            Door d8 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(247, 72)) + new Vector2(-16, -16), OpenState.Closed);
            FlipSwitch s8 = gsPlay.AddSwitch(new EventTrigger(this, d8), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(252, 70)) + new Vector2(-16, 0));

            Door d9 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(252, 68)) + new Vector2(-16, -16), OpenState.Closed);
            FlipSwitch s9 = gsPlay.AddSwitch(new EventTrigger(this, d9), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(247, 64)) + new Vector2(-16, 0));

            //part3

            Door d10 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(327, 68)) + new Vector2(-16, 0), OpenState.Closed);
            FlipSwitch s10 = gsPlay.AddSwitch(new EventTrigger(this, d10), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(330, 61)));
            d10.SetPlayerMode(PlayerObjectMode.One);
            s10.SetPlayerMode(PlayerObjectMode.Two);

            Door d11 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(334, 62)) + new Vector2(-16, 0), OpenState.Closed);
            FlipSwitch s11 = gsPlay.AddSwitch(new EventTrigger(this, d11), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(337, 61)));
            d11.SetPlayerMode(PlayerObjectMode.Two);
            s11.SetPlayerMode(PlayerObjectMode.One);

            Door d12 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(341, 68)) + new Vector2(-16, 0), OpenState.Closed);
            FlipSwitch s12 = gsPlay.AddSwitch(new EventTrigger(this, d12), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(344, 61)));
            d12.SetPlayerMode(PlayerObjectMode.One);
            s12.SetPlayerMode(PlayerObjectMode.Two);

            Door d13 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(348, 62)) + new Vector2(-16, 0), OpenState.Closed);
            FlipSwitch s13 = gsPlay.AddSwitch(new EventTrigger(this, d13), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(351, 61)));
            d13.SetPlayerMode(PlayerObjectMode.Two);
            s13.SetPlayerMode(PlayerObjectMode.One);

            //part4
            MovingPlatform mp1 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(399, 25)) + new Vector2(0, -16), Level.GridToPosition(new Point(407, 25)) + new Vector2(0, -16));
            mp1.SetPlayerMode(PlayerObjectMode.One);
            FlipSwitch ms1 = gsPlay.AddSwitch(new EventTrigger(this, mp1), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(403, 29)));
            ms1.SetPlayerMode(PlayerObjectMode.Two);

            Door d14 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(393, 9)) + new Vector2(-16, 0), OpenState.Closed);
            FlipSwitch s14 = gsPlay.AddSwitch(new EventTrigger(this, d14), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(398, 21)));

            Door d15 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(366, 33)) + new Vector2(0, -16), OpenState.Closed);
            gsPlay.AddButton(new EventTrigger(this, d15), 1, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(new Point(370, 32)));

            Door d16 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(444, 69)) + new Vector2(0, -16), OpenState.Closed);
            FlipSwitch s16 = gsPlay.AddSwitch(new EventTrigger(this, d16), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(440, 71)));
            s16.SetPlayerMode(PlayerObjectMode.Two);

            gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), GridToPosition(new Point(426, 68))).selfIlluminating = true;
            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(429, 46)), .8f);
            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(388, 10)), .8f);
            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(366, 31)), .8f);
            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(443, 71 )), .8f);

            //secret
            Door mp2 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(152, 69)) + new Vector2(-16, -16), OpenState.Closed);
            FlipSwitch ms2 = gsPlay.AddSwitch(new EventTrigger(this, mp2), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(152, 67)) + new Vector2(-16, 0));
            ms2.pressureCooker = true;
            ms2.SetPlayerMode(PlayerObjectMode.One);
            Door mp3 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(152, 69)) + new Vector2(-16, -16), OpenState.Closed);
            FlipSwitch ms3 = gsPlay.AddSwitch(new EventTrigger(this, mp3), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(152, 67)) + new Vector2(-16, 0));
            ms3.pressureCooker = true;
            ms3.SetPlayerMode(PlayerObjectMode.Two);

            Door bd5 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(150, 66)) + new Vector2(-16, 0), OpenState.Closed);
            bd5.SetPlayerMode(PlayerObjectMode.One);
            Door bd6 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(154, 66)) + new Vector2(-16, 0), OpenState.Closed);
            bd6.SetPlayerMode(PlayerObjectMode.Two);

            AudioManager.PlayMusic("Mountain");

        }

        public override void ActivateEvent(int eventID, TriggerState triggerState)
        {
            base.ActivateEvent(eventID, triggerState);

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
                        gsPlay.gameStateManager.currentLevel = 50;
                        //gsPlay.gameStateManager.SwitchToGSPlay();
                        gsPlay.gameStateManager.TransitionToGameState(gsPlay, GameStateType.GSPlay, 30);
                    }
                    break;
            }
        }
    }
}
