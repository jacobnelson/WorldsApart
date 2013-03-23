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
    class Level1 : Level
    {
        Door doubleButtonDoor; 
        bool buttonOneDown = false;
        bool buttonTwoDown = false;

        public Level1(GSPlay gsPlay)
            : base(gsPlay)
        {
            levelDataTexture = gsPlay.LoadTexture("Levels/level1Data");

            player1Pos = GridToPosition(39, 24);
            player2Pos = GridToPosition(39, 24);

            portalPos = GridToPosition(681, 20);
            pItemPos = GridToPosition(672, 11);

            SetupLevel();

            atmosphereLight = new Color(255, 255, 255);

            SpriteIMG s = new SpriteIMG(gsPlay.LoadTexture("bgSky"), new Vector2(64, levelHeight / 2));
            gsPlay.AddParallax(s, .5f);


            doubleButtonDoor = gsPlay.AddOpeningDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(214, 23)), Level.GridToPosition(new Point(214, 27)), OpenState.Closed);  //first door puzzle
            gsPlay.AddButton(new EventTrigger(this, 1), 1, gsPlay.LoadTexture("TestSprites/button"), Level.GridToPosition(new Point(206, 29)));                                            //first door puzzle
            gsPlay.AddButton(new EventTrigger(this, 2), 1, gsPlay.LoadTexture("TestSprites/button"), Level.GridToPosition(new Point(222, 29)));                                            //first door puzzle
            //gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), Level.GridToPosition(new Point(222, 16)));

            Door d2 = gsPlay.AddOpeningDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(305, 23)), Level.GridToPosition(new Point(305, 27)), OpenState.Closed);  //jump from box, ball on button
            gsPlay.AddButton(new EventTrigger(this, d2), 1, gsPlay.LoadTexture("TestSprites/button"), Level.GridToPosition(new Point(297, 29)));                                            //jump from box, ball on button
            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(257, 16)), .8f);                                                                  //jump from box, ball on button
            gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), Level.GridToPosition(new Point(280, 16)));                                                                  //jump from box, ball on button

            Door d3 = gsPlay.AddOpeningDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(329, 23)), Level.GridToPosition(new Point(329, 27)), OpenState.Closed);  //throw ball onto button
            gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), Level.GridToPosition(new Point(312, 16)));                                                                  //throw ball onto button
            gsPlay.AddButton(new EventTrigger(this, d3), 1, gsPlay.LoadTexture("TestSprites/button"), Level.GridToPosition(new Point(326, 22)));                                            //throw ball onto button

            Door d4 = gsPlay.AddOpeningDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(373, 23)), Level.GridToPosition(new Point(373, 27)), OpenState.Closed);  //box drop
            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(339, 16)), .8f);                                                                  //box drop
            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(358, 16)), .8f);                                                                  //box drop
            gsPlay.AddButton(new EventTrigger(this, d4), 2, gsPlay.LoadTexture("TestSprites/button"), Level.GridToPosition(new Point(365, 29)));                                            //box drop
            gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), Level.GridToPosition(new Point(370, 16)));                                                                  //box drop
            gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(351, 21)), Level.GridToPosition(new Point(346, 21)));                             //box drop

            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(378, 16)), .8f);                                                                  //box accross moving
            gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(398, 26)), Level.GridToPosition(new Point(406, 26)));                             //box accross moving
            gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(386, 26)), Level.GridToPosition(new Point(386, 30)));                             //box accross moving

            Door d5 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(561, 24)), OpenState.Closed);                                             //barn door
            gsPlay.AddSwitch(new EventTrigger(this, d5), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(556, 25)));                                               //barn door

            Door d6 = gsPlay.AddOpeningDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(570, 15)), Level.GridToPosition(new Point(570, 23)), OpenState.Closed);
            gsPlay.AddButton(new EventTrigger(this, d6), 1, gsPlay.LoadTexture("TestSprites/button"), Level.GridToPosition(new Point(573, 19)));
            gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), Level.GridToPosition(new Point(564, 24)));

            Door d7 = gsPlay.AddOpeningDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(576, 23)), Level.GridToPosition(new Point(576, 17)), OpenState.Closed);
            gsPlay.AddButton(new EventTrigger(this, d7), 1, gsPlay.LoadTexture("TestSprites/button"), Level.GridToPosition(new Point(579, 19)));

            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(579, 22)), .8f);
            //gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(635, 06)), Level.GridToPosition(new Point(635, 25)));
            gsPlay.AddSwitch(new EventTrigger(this, gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(595, 06)), Level.GridToPosition(new Point(595, 25)))), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(602, 25)));

            Door d8 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(605, 18)), OpenState.Closed);                                                //lower barn exit
            gsPlay.AddSwitch(new EventTrigger(this, d8), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(601, 19)));                                                  //lower barn exit 

            Door d9 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(605, 09)), OpenState.Closed);                                                //upper barn exit
            gsPlay.AddSwitch(new EventTrigger(this, d9), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(601, 10)));                                                  //upper barn exit 

            Door d10 = gsPlay.AddOpeningDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(600, 28)), Level.GridToPosition(new Point(600, 24)), OpenState.Closed);
            gsPlay.AddButton(new EventTrigger(this, d10), 2, gsPlay.LoadTexture("TestSprites/button"), Level.GridToPosition(new Point(591, 26)));

        }

        public override void ActivateEvent(int eventID, TriggerState triggerState)
        {
            base.ActivateEvent(eventID, triggerState);

            switch (eventID)
            {
                case 0:
                    if (triggerState == TriggerState.Triggered)
                    {
                        bool isGood = true;
                        foreach (Portal portal in gsPlay.portalList)
                        {
                            if (portal.goodMode == false) isGood = false;
                        }
                        if (isGood) gsPlay.gameStateManager.goodness++;
                        else gsPlay.gameStateManager.goodness--;
                        gsPlay.gameStateManager.currentLevel = 2;
                        gsPlay.gameStateManager.SwitchToGSPlay();
                    }
                    break;
                case 1:
                    if (triggerState == TriggerState.Triggered)
                    {
                        
                        buttonOneDown = true;
                    }
                    else 
                    {
                        buttonOneDown = false;
                    }
                    ActivateEvent(3, TriggerState.Triggered);
                    break;
                case 2:
                    if (triggerState == TriggerState.Triggered)
                    {
                        buttonTwoDown = true;
                    }
                    else
                    {
                        buttonTwoDown = false;
                    }
                    ActivateEvent(3, TriggerState.Triggered);
                    break;
                case 3:
                    if (buttonOneDown || buttonTwoDown)
                    {
                        if (doubleButtonDoor.triggerState == TriggerState.Untriggered)
                        {
                            doubleButtonDoor.triggerState = TriggerState.Triggered;
                            doubleButtonDoor.ActivateEvent(TriggerState.Triggered);
                        }
                    }
                    else
                    {
                        if (doubleButtonDoor.triggerState == TriggerState.Triggered)
                        {
                            doubleButtonDoor.triggerState = TriggerState.Untriggered;
                            doubleButtonDoor.ActivateEvent(TriggerState.Untriggered);
                        }
                    }
                    break;
            }
        }
    }
}