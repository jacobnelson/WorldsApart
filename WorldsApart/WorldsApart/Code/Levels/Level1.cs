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
        int introCounter = 0;
        bool introScene = true;
        AnimatedSprite player1Intro;
        AnimatedSprite player2Intro;
        SpriteIMG fadeOut;


        Door doubleButtonDoor;
        bool buttonOneDown = false;
        bool buttonTwoDown = false;

        Door finalTopDoor;
        Door finalTrapDoor;

        public Level1(GSPlay gsPlay)
            : base(gsPlay)
        {
            levelDataTexture = gsPlay.LoadTexture("Levels/level1Data");

            player1Pos = GridToPosition(42, 25);
            player2Pos = GridToPosition(40, 25);

            portalPos = GridToPosition(681, 20);
            pItemPos = GridToPosition(672, 11);

            SetupLevel();


            //Deactivate players for intro scene:
            StartIntro();

            TriggerArea update = gsPlay.AddTriggerArea(new EventTrigger(this, 99), Art.barrier, Vector2.Zero);

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
            gsPlay.AddBouncyBall(.8f,gsPlay.LoadTexture("TestSprites/pickUp"), Level.GridToPosition(new Point(358, 16)));                                                                  //box drop
            gsPlay.AddButton(new EventTrigger(this, d4), 2, gsPlay.LoadTexture("TestSprites/button"), Level.GridToPosition(new Point(365, 29)));                                            //box drop
            gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), Level.GridToPosition(new Point(370, 16)));                                                                  //box drop
            gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(351, 21)), Level.GridToPosition(new Point(346, 21)));                             //box drop

            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(378, 16)), .8f);                                                                  //box accross moving
            gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(398, 25)), Level.GridToPosition(new Point(406, 25)));                             //box accross moving
            gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(386, 25)), Level.GridToPosition(new Point(386, 30)));                             //box accross moving

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

            

            finalTrapDoor = gsPlay.AddOpeningDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(600, 28)), Level.GridToPosition(new Point(600, 24)), OpenState.Closed);
            Button finalButton = gsPlay.AddButton(new EventTrigger(this, finalTrapDoor), 2, gsPlay.LoadTexture("TestSprites/button"), Level.GridToPosition(new Point(591, 26)));

            finalTopDoor = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(605, 09)), OpenState.Closed);                                                //upper barn exit
            FlipSwitch finalSwitch = gsPlay.AddSwitch(new EventTrigger(this, finalTopDoor), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(601, 10)));                                                  //upper barn exit 
            finalSwitch.AddEvent(new EventTrigger(this, 4));
            

        }

        public void StartIntro()
        {
            gsPlay.player1.superStopInput = true;
            gsPlay.player2.superStopInput = true;
            gsPlay.player1.visible = false;
            gsPlay.player2.visible = false;

            Vector2 introPos = GridToPosition(41, 25) + new Vector2(0, 20);
            player1Intro = new AnimatedSprite(gsPlay.LoadTexture("Cutscene/cutscenePlayers"), introPos);
            player2Intro = new AnimatedSprite(gsPlay.LoadTexture("Cutscene/cutscenePlayers"), introPos);

            player1Intro.SetAnimationStuff(1, 1, 3, 8, 256, 256, 4, 12);
            player2Intro.SetAnimationStuff(1, 5, 3, 8, 256, 256, 4, 12);

            player1Intro.SetPlayerMode(PlayerObjectMode.One);
            player2Intro.SetPlayerMode(PlayerObjectMode.Two);

            gsPlay.frontFGList.Add(player1Intro);
            gsPlay.frontFGList.Add(player2Intro);
        }



        public override void ActivateEvent(int eventID, TriggerState triggerState)
        {
            base.ActivateEvent(eventID, triggerState);

            switch (eventID)
            {
                case 99:
                    //UPDATE METHOD


                    
                    if (introScene)
                    {
                        introCounter++;

                        if (introCounter == 30)
                        {
                            GSOverlay.words1.am.StartFade(30, 0, 255);
                        }
                        if (introCounter == 300)
                        {
                            GSOverlay.words1.am.StartFade(30, 255, 0);
                            GSOverlay.words2.am.StartFade(30, 0, 255);
                        }
                        if (introCounter == 600)
                        {
                            GSOverlay.FadeIn(30, Color.White);
                        }
                        if (introCounter == 630)
                        {
                            player1Intro.ChangeAnimationBounds(2, 1, 4);
                            player2Intro.ChangeAnimationBounds(2, 5, 4);
                            GSOverlay.words1.visible = false;
                            GSOverlay.words2.visible = false;
                            GSOverlay.FadeOut(30);
                        }
                        if (introCounter == 660)
                        {
                            GSOverlay.words3.am.StartFade(30, 0, 255);
                        }
                        if (introCounter == 900)
                        {
                            GSOverlay.words3.am.StartFade(30, 255, 0);
                            GSOverlay.words4.am.StartFade(30, 0, 255);
                        }
                        if (introCounter == 1200)
                        {
                            GSOverlay.FadeIn(30, Color.White);
                        }
                        if (introCounter == 1230)
                        {
                            player1Intro.ChangeAnimationBounds(3, 1, 4);
                            player2Intro.ChangeAnimationBounds(3, 5, 4);
                            GSOverlay.words3.visible = false;
                            GSOverlay.words4.visible = false;
                            GSOverlay.FadeOut(30);
                        }
                        if (introCounter == 1260)
                        {
                            GSOverlay.words5.am.StartFade(30, 0, 255);
                        }
                        if (introCounter == 1500)
                        {
                            GSOverlay.words5.am.StartFade(30, 255, 0);
                            GSOverlay.words6.am.StartFade(30, 0, 255);
                        }
                        if (introCounter == 1800)
                        {
                            GSOverlay.FadeIn(30, Color.White);
                        }
                        if (introCounter == 1830)
                        {
                            GSOverlay.FadeOut(30);
                            player1Intro.visible = false;
                            player2Intro.visible = false;
                            GSOverlay.words5.visible = false;
                            GSOverlay.words6.visible = false;

                            gsPlay.player1.superStopInput = false;
                            gsPlay.player2.superStopInput = false;
                            gsPlay.player1.visible = true;
                            gsPlay.player2.visible = true;
                        }

                    }


                    break;


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
                        gsPlay.gameStateManager.TransitionToGameState(gsPlay, GameStateType.GSPlay, 30);
                        //gsPlay.gameStateManager.SwitchToGSPlay();
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
                case 4:
                    if (triggerState == TriggerState.Triggered)
                    {
                        finalTrapDoor.ActivateEvent(triggerState);
                    }
                    break;
            }
        }
    }
}