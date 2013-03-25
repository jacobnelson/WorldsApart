﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using WorldsApart.Code.Levels;
using WorldsApart.Code.Controllers;
using WorldsApart.Code.Graphics;

using System.Diagnostics;

namespace WorldsApart.Code.Entities
{
    enum Facing
    {
        Left,
        Right
    }

    enum PlayerMode
    {
        Idle,
        RunningLead,
        Running,
        RunningEnd,
        JumpingUpLead,
        JumpingUp,
        JumpingDownLead,
        JumpingDown,
        JumpingDownEnd,
        Dying,
        Reviving
    }

    class Player : PhysObj
    {

        public bool showingRegular = true;

        public Texture2D indicatorTexture;
        public byte indicatorAlpha = 255;

        public Texture2D regularTexture;

        PlayerObjectMode playerIndex = PlayerObjectMode.One;
        public bool stopInput = false;

        bool isJumping = false;
        float maxJumpForce = -2;
        float jumpForce = -2;
        float maxJumpAccel = .2f;
        float jumpAccel = .2f;
        int jumpCounter = 0;
        int jumpRate = 10;

        float runningSpeed = 6;
        float runningSpeedConst = 6;

        bool rightDown = false;
        //bool rightUp = false;
        bool leftDown = false;
        //bool leftUp = false;
        bool upDown = false;
        //bool upUp = false;
        bool downDown = false;
        //bool downUp = false;
        bool jumpDown = false;
        bool jumpPressed = false;
        bool jumpReleased = false;
        bool actionDown = false;
        bool actionPressed = false;
        //bool actionReleased = false;

        bool ableToPressDrop = false;
        int dropCounter = 0;
        int dropRate = 10;

        Vector2 thumb = Vector2.Zero;

        //PlayerMode currentMode = PlayerMode.Idle;
        PlayerMode currentSet = PlayerMode.Idle;

        Facing facing = Facing.Right;

        public PickUpObj pickUp;
        public CollisionBox grabBox;
        bool holding = false;
        bool grabbing = false;
        Moveable pushTarget;
        public bool pressing = false;
        public bool pushing = false;
        public PointLight light;
        public PointLight sleeperLight;


        public IdealAnimationSet idleSet;
        public IdealAnimationSet runningLeadSet;
        public IdealAnimationSet runningSet;
        public IdealAnimationSet runningEndSet;
        public IdealAnimationSet jumpingUpLeadSet;
        public IdealAnimationSet jumpingUpSet;
        public IdealAnimationSet jumpingDownLeadSet;
        public IdealAnimationSet jumpingDownSet;
        public IdealAnimationSet jumpingDownEndSet;
        public IdealAnimationSet DyingSet;
        public IdealAnimationSet RevivingSet;

        public Player(PlayerObjectMode playerIndex, Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            regularTexture = texture;
            this.playerIndex = playerIndex;
            playerTangible = playerIndex;
            playerVisible = playerIndex;
            gravity = new Vector2(0, .5f);
            grabBox = new CollisionBox(this, new Vector2(halfWidth * 2, halfHeight * 2));

            if (playerIndex == PlayerObjectMode.One) auraColor = new Color(255, 128, 0);
            else auraColor = new Color(0, 128, 255);
        }

        public void ChangeCurrentSet(PlayerMode currentSet)
        {
            this.currentSet = currentSet;
            switch (currentSet)
            {
                case PlayerMode.Dying:
                    ChangeAnimationSet(DyingSet);
                    break;
                case PlayerMode.Idle:
                    ChangeAnimationSet(idleSet);
                    break;
                case PlayerMode.JumpingDown:
                    ChangeAnimationSet(jumpingDownSet);
                    break;
                case PlayerMode.JumpingDownEnd:
                    ChangeAnimationSet(jumpingDownEndSet);
                    break;
                case PlayerMode.JumpingDownLead:
                    ChangeAnimationSet(jumpingDownLeadSet);
                    break;
                case PlayerMode.JumpingUp:
                    ChangeAnimationSet(jumpingUpSet);
                    break;
                case PlayerMode.JumpingUpLead:
                    ChangeAnimationSet(jumpingUpLeadSet);
                    break;
                case PlayerMode.Reviving:
                    ChangeAnimationSet(RevivingSet);
                    break;
                case PlayerMode.Running:
                    ChangeAnimationSet(runningSet);
                    break;
                case PlayerMode.RunningEnd:
                    ChangeAnimationSet(runningEndSet);
                    break;
                case PlayerMode.RunningLead:
                    ChangeAnimationSet(runningLeadSet);
                    break;
            }
        }

        public void ChangeAnimationSet(IdealAnimationSet set)
        {
            texture = set.texture;
            indicatorTexture = set.idealTexture;
            SetPlayerAnimationStuff(set.minRow, set.minCol, set.rows, set.cols, cellW, cellH, set.frames, set.animationRate);
        }

        public void SetPlayerAnimationStuff(int _minRow, int _minCol, int _rows, int _cols, int _cellW, int _cellH, int _frames, int _animationRate)
        {
            ChangeAnimationBounds(_minRow, _minCol, _frames);
            rows = _rows;
            cols = _cols;
            animationRate = _animationRate;
            isAnimating = true;
        }

        public override void SetAnimationStuff(int _minRow, int _minCol, int _rows, int _cols, int _cellW, int _cellH, int _frames, int _animationRate)
        {
            base.SetAnimationStuff(_minRow, _minCol, _rows, _cols, _cellW, _cellH, _frames, _animationRate);
            grabBox = new CollisionBox(this, new Vector2(halfWidth * 3, halfHeight));
        }

        public override void Update()
        {
            ChangeSprite();

            thumb = Vector2.Zero;
            rightDown = false;
            leftDown = false;
            upDown = false;
            downDown = false;
            jumpDown = false;
            jumpPressed = false;
            jumpReleased = false;
            actionDown = false;
            actionPressed = false;
            GetInput();

            if (pushTarget == null) runningSpeed = runningSpeedConst;

            if (leftDown)
            {
                if (!IsTopRunningSpeed(false)) force.X += -moveForce;
                else SlowX();
                facing = Facing.Left;
                leftDown = true;
            }
            else if (rightDown)
            {
                if (!IsTopRunningSpeed(true)) force.X += moveForce;
                else SlowX();
                facing = Facing.Right;
                rightDown = true;
            }
            else if (thumb.X > 0)
            {
                if (!IsTopRunningSpeed(true)) force.X += moveForce * Math.Abs(thumb.X);
                else SlowX();
                facing = Facing.Right;
                rightDown = true;
            }
            else if (thumb.X < 0)
            {
                if (!IsTopRunningSpeed(false)) force.X += -moveForce * Math.Abs(thumb.X);
                else SlowX();
                facing = Facing.Left;
                leftDown = true;
            }
            else SlowX();

            if (actionDown)
            {
                grabbing = true;
                pushing = true;
                psyHold = true;
            }

            pressing = false;
            if (actionPressed)
            {
                pressing = true;
            }


            
            if (dropCounter >= dropRate)
            {
                ignoreOneWay = false;
            }
            else dropCounter++;

            if (downDown)
            {
                if (ableToPressDrop)
                {
                    ignoreOneWay = true;
                    ableToPressDrop = false;
                    dropCounter = 0;
                }
            }
            else ableToPressDrop = true;



            if (!actionDown)
            {
                psyHold = false;
                grabbing = false;
                pushing = false;
                if (holding)
                {
                    holding = false;
                    Vector2 throwSpeed = Vector2.Zero;
                    if (upDown) throwSpeed.Y = -10;
                    if (downDown) throwSpeed.Y = 10;
                    if (rightDown) throwSpeed.X = 10;
                    if (leftDown) throwSpeed.X = -10;
                    pickUp.GetDropped(throwSpeed);
                    pickUp = null;
                }
            }

            if (jumpPressed && state == PhysState.Grounded)
            {
                force.Y += jumpForce;
                isJumping = true;
            }
            if (jumpDown)
            {

                if (isJumping)
                {
                    force.Y += jumpForce;
                    jumpCounter++;
                    if (jumpCounter >= jumpRate)
                    {
                        StopJump();
                    }
                }
            }
            if (jumpReleased)
            {
                if (isJumping)
                {
                    StopJump();
                }
            }

            if (light != null) psyHold = true;

            base.Update();
            grabBox.SetPosition(hitBox.GetPosition());
            Level.CheckForPlayerStuff(this);

        }

        public override void AdjustCollision(PhysObj obj)
        {
            base.AdjustCollision(obj);
            grabBox.SetPosition(position);
        }

        public override void Die()
        {
            if (currentSet != PlayerMode.Dying)
            {
                am.Pause();
                ChangeAnimationBounds(5, 1, 4);
                currentSet = PlayerMode.Dying;
            }
        }

        public void GetInput()
        {
            if (stopInput) return;

            thumb = InputManager.GetLeftThumbstick();

            

            if (InputManager.IsKeyDown(Keys.W) || InputManager.IsButtonDown(Buttons.DPadUp) || thumb.Y > 0) upDown = true;
            if (InputManager.IsKeyDown(Keys.S) || InputManager.IsButtonDown(Buttons.DPadDown) || thumb.Y < 0) downDown = true;

            if (playerIndex == PlayerObjectMode.One)
            {
                if (InputManager.IsButtonDown(Buttons.DPadLeft) || InputManager.IsKeyDown(Keys.A)) leftDown = true;
                if (InputManager.IsButtonDown(Buttons.DPadRight) || InputManager.IsKeyDown(Keys.D)) rightDown = true;
                if (InputManager.IsKeyUp(Keys.A) && InputManager.IsButtonUp(Buttons.DPadLeft) && thumb.X >= 0) leftDown = false;
                if (InputManager.IsKeyUp(Keys.D) && InputManager.IsButtonUp(Buttons.DPadRight) && thumb.X <= 0) rightDown = false;
            }
            else if (playerIndex == PlayerObjectMode.Two)
            {
                thumb.X = -thumb.X;
                if (InputManager.IsButtonDown(Buttons.DPadLeft) || InputManager.IsKeyDown(Keys.A)) rightDown = true;
                if (InputManager.IsButtonDown(Buttons.DPadRight) || InputManager.IsKeyDown(Keys.D)) leftDown = true;
                if (InputManager.IsKeyUp(Keys.A) && InputManager.IsButtonUp(Buttons.DPadLeft) && thumb.X >= 0) rightDown = false;
                if (InputManager.IsKeyUp(Keys.D) && InputManager.IsButtonUp(Buttons.DPadRight) && thumb.X <= 0) leftDown = false;
            }
            if (InputManager.IsKeyUp(Keys.W) && InputManager.IsButtonUp(Buttons.DPadUp) && thumb.Y <= 0) upDown = false;
            if (InputManager.IsKeyUp(Keys.S) && InputManager.IsButtonUp(Buttons.DPadDown) && thumb.Y >= 0) downDown = false;

            if ((InputManager.IsKeyDown(Keys.RightControl) || InputManager.IsButtonDown(Buttons.X))) actionDown = true;
            if (InputManager.IsKeyPressed(Keys.RightControl) || InputManager.IsButtonPressed(Buttons.X)) actionPressed = true;

            if ((InputManager.IsKeyPressed(Keys.Space) || InputManager.IsButtonPressed(Buttons.A)) && state == PhysState.Grounded) jumpPressed = true;
            if (InputManager.IsKeyDown(Keys.Space) || InputManager.IsButtonDown(Buttons.A)) jumpDown = true;
            if (InputManager.IsKeyReleased(Keys.Space) || InputManager.IsButtonReleased(Buttons.A)) jumpReleased = true;

        }

        public override void nextCell() //Goes to the next cell, and loops based on your mins and maxes
        {
            currentCellCol++;
            if (currentCellCol > cols)
            {
                currentCellRow++;
                currentCellCol = 1;
            }
            frameCounter++;
            if (frameCounter >= frames)
            {
                frameCounter = 0;
                currentCellCol = minCol;
                currentCellRow = minRow;
            }
        }

        public void ChangeSprite()
        {
            isAnimating = true;
            if (facing == Facing.Right) spriteEffects = SpriteEffects.None;
            else spriteEffects = SpriteEffects.FlipHorizontally;

            if (currentSet == PlayerMode.Dying)
            {
                stopInput = true;
                if (CheckForFrameStop(DyingSet))
                {
                    ChangeCurrentSet(PlayerMode.Reviving);
                    am.pauseMovement = false;
                    base.Die();
                    
                }
                return;
            }
            else if (currentSet == PlayerMode.Reviving)
            {
                stopInput = true;
                //Trace.WriteLine("Row: " + currentCellRow + " | Col: " + currentCellCol);
                if (CheckForFrameStop(RevivingSet))
                {
                    ChangeCurrentSet(PlayerMode.Idle);
                }
                return;
            }

            if (state == PhysState.Grounded)
            {
                bool landed = false;
                if (currentSet == PlayerMode.JumpingDown && currentSet != PlayerMode.JumpingDownEnd)
                {
                    ChangeCurrentSet(PlayerMode.JumpingDownEnd);
                }
                else if (currentSet == PlayerMode.JumpingDownEnd)
                {
                    if (CheckForFrameStop(jumpingDownEndSet))
                    {
                        landed = true;
                    }
                }
                else if (currentSet != PlayerMode.JumpingDown && currentSet != PlayerMode.JumpingDownEnd)
                {
                    landed = true;
                }

                if (landed)
                {
                    if (speed.X != 0)
                    {
                        if (currentSet != PlayerMode.Running && currentSet != PlayerMode.RunningLead)
                        {
                            ChangeCurrentSet(PlayerMode.RunningLead);
                        }
                        else if (currentSet == PlayerMode.RunningLead)
                        {
                            if (CheckForFrameStop(runningLeadSet))
                            {
                                ChangeCurrentSet(PlayerMode.Running);
                            }
                        }
                        //if (currentFrame == PlayerMode.Running)
                        //{
                        //    animationRate = 12 - (int)((Math.Abs(speed.X) / terminalSpeed.X) * 8);
                        //}
                    }
                    else if (currentSet != PlayerMode.Idle && currentSet != PlayerMode.RunningEnd && currentSet == PlayerMode.Running) 
                    {
                        ChangeCurrentSet(PlayerMode.RunningEnd);
                    }
                    else if (currentSet == PlayerMode.RunningEnd)
                    {
                        if (CheckForFrameStop(runningEndSet))
                        {
                            ChangeCurrentSet(PlayerMode.Idle);
                        }
                    }
                    else if (currentSet != PlayerMode.Idle) 
                    {
                        ChangeCurrentSet(PlayerMode.Idle);
                    }
                }
            }
            else if (state == PhysState.Air)
            {
                if (speed.Y < 0)
                {
                    if (currentSet != PlayerMode.JumpingUpLead && currentSet != PlayerMode.JumpingUp)
                    {
                        ChangeCurrentSet(PlayerMode.JumpingUpLead);
                    }
                    else if (currentSet == PlayerMode.JumpingUpLead)
                    {
                        if (jumpingUpLeadSet.endingRowCol == new Point(currentCellRow, currentCellCol))
                        {
                            ChangeCurrentSet(PlayerMode.JumpingUp);
                        }
                    }
                }
                else
                {
                    if (currentSet != PlayerMode.JumpingDownLead && currentSet != PlayerMode.JumpingDown)
                    {
                        ChangeCurrentSet(PlayerMode.JumpingDownLead);
                    }
                    else if (currentSet == PlayerMode.JumpingDownLead)
                    {
                        if (CheckForFrameStop(jumpingDownLeadSet))
                        {
                            ChangeCurrentSet(PlayerMode.JumpingDown);
                        }
                    }
                }
            }


            if (showingRegular)
            {
                texture = regularTexture;
            }
            else
            {
                texture = indicatorTexture;
            }
            //animationRate = 5;
            //if (currentSet == PlayerMode.Running) animationRate = 8;
            //if (currentSet == PlayerMode.Idle) animationRate = 8;

        }

        public bool CheckForFrameStop(IdealAnimationSet set)
        {
            return (set.endingRowCol == new Point(currentCellRow, currentCellCol) && animationCounter == animationRate - 1);
        }

        public void StopJump()
        {
            isJumping = false;
            jumpForce = maxJumpForce;
            jumpAccel = maxJumpAccel;
            jumpCounter = 0;
        }

        public bool IsTopRunningSpeed(bool right)
        {
            if (speed.X > runningSpeed && right) return true;
            if (speed.X < -runningSpeed && !right) return true;
            return false;
        }

        

        public bool CheckForAlreadyHeld()
        {
            return (holding || pushTarget != null || light != null || sleeperLight != null);
        }

        public void CheckForGrab(PickUpObj obj)
        {
            if (playerTangible != PlayerObjectMode.None && obj.playerTangible != PlayerObjectMode.None)
            {
                if (playerTangible != obj.playerTangible) return;
            }
            
            if (hitBox.CheckCollision(obj.hitBox))
            {
                if (CheckForAlreadyHeld()) return;
                if (grabbing)
                {
                    obj.GetPickedUp(this);
                    grabbing = false;
                    holding = true;
                    //obj.psyHold = true;
                }
            }
            else
            {
                if (pickUp == obj)
                {
                    obj.GetDropped(Vector2.Zero);
                    holding = false;
                }
            }
        }

        public void CheckForArea(TriggerArea area)
        {
            if (playerTangible != PlayerObjectMode.None && area.playerTangible != PlayerObjectMode.None)
            {
                if (playerTangible != area.playerTangible) return;
            }
            if (hitBox.CheckCollision(area.hitBox))
            {
                area.touching = true;
            }
        }

        public void CheckForPressure(FlipSwitch s)
        {
            if (playerTangible != PlayerObjectMode.None && s.playerTangible != PlayerObjectMode.None)
            {
                if (playerTangible != s.playerTangible) return;
            }
            
        }

        public void CheckForPress(FlipSwitch s)
        {
            if (playerTangible != PlayerObjectMode.None && s.playerTangible != PlayerObjectMode.None)
            {
                if (playerTangible != s.playerTangible) return;
            }
            if (CheckForAlreadyHeld()) return;
            if (s.pressureCooker)
            {
                if (pushing)
                {
                    if (hitBox.CheckCollision(s.hitBox)) s.touching = true;
                }
            }
            else
            {
                if (!pressing) return;
                if (hitBox.CheckCollision(s.hitBox))
                {
                    s.PressSwitch();
                }
            }
        }

        public void CheckForPush(Moveable move)
        {
            if (playerTangible != PlayerObjectMode.None && move.playerTangible != PlayerObjectMode.None)
            {
                if (playerTangible != move.playerTangible) return;
            }
            float pushX = 0;
            if (hitBox.CheckCollision(move.hitBox))
            {
                if (pushing && (pushTarget == move || !CheckForAlreadyHeld()))
                {
                    pushX = speed.X * move.moveModifier;
                }
                AdjustCollision(move);
                if (pushing && speed.X == 0 && (pushTarget == move || !CheckForAlreadyHeld()))
                {
                    speed.X = pushX;
                    move.speed.X = pushX;
                    if (hitBox.WorldCornerMax.Y > move.hitBox.WorldCornerMin.Y)
                    {
                        if (move.parent == null)
                        {
                            pushTarget = move;
                            move.parent = this;
                        }
                    }
                    else if (move.parent == this)
                    {
                        move.parent = null;
                        pushTarget = null;
                    }
                }
                else if (move.parent == this)
                {
                    move.parent = null;
                    pushTarget = null;
                }
            }
            else if (grabBox.CheckCollision(move.hitBox))
            {
                if (pushing && (pushTarget == move || !CheckForAlreadyHeld()))
                {
                    pushX = speed.X * move.moveModifier;
                    //runningSpeed = pushX;
                    move.speed.X = pushX;
                    speed.X = pushX;
                    if (move.parent == null)
                    {
                        pushTarget = move;
                        move.parent = this;
                    }
                }
                else if (move.parent == this)
                {
                    move.parent = null;
                    pushTarget = null;
                }
            }
            else if (move.parent == this)
            {
                move.parent = null;
                pushTarget = null;
            }

        }

        public void CheckLightConsole(LightConsole console)
        {
            if (playerTangible != PlayerObjectMode.None && console.playerTangible != PlayerObjectMode.None)
            {
                if (playerTangible != console.playerTangible) return;
            }
            
            if (!pressing) return;
            if (hitBox.CheckCollision(console.hitBox))
            {
                console.PressConsole(this, !CheckForAlreadyHeld());
            }
        }

        public override void DrawAura(SpriteBatch spriteBatch, Vector2 screenOrigin)
        {
            base.DrawAura(spriteBatch, screenOrigin);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
