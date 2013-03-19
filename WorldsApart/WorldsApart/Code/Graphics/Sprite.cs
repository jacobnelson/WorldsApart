using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using WorldsApart.Code.Controllers;

namespace WorldsApart.Code.Graphics
{
    class Sprite
    {
        public PlayerObjectMode playerTangible;
        public PlayerObjectMode playerVisible;

        public AnimationManager am;

        public Vector2 sPosition = Vector2.Zero; //Where on the screen
        public virtual Vector2 position
        {
            get { return sPosition; }
            set { sPosition = value; }
        }
        public Vector2 scale = Vector2.One; //How big you scale it by
        public float rotation = 0; //What angle you rotate it
        public Color color = Color.White; //The color
        public Vector2 origin = Vector2.Zero; //The point on the object where it calculates from
        public Rectangle crop; //How much of the texture you show
        public SpriteEffects spriteEffects = SpriteEffects.None; //What spriteEffects you want
        public float depth = 0;

        //Variables for if you can see the sprite
        public bool blinking = false;
        public int blinkRate = 5; //The life of a blink
        public int blinkCounter = 0; //The counter for life of a blink
        public bool visible = true; //If you can see the sprite or not

        public byte alpha = 255;

        public bool onceDrawn = false;

        public Sprite(Vector2 position)
        {
            this.position = position;
            am = new AnimationManager(this);
        }

        public virtual void Update()
        {
            if (blinking)
            {
                Blink();
            }
            GetMovement();
            am.Update();
        }

        public virtual void GetMovement()
        {
        }


        public virtual void SetPlayerMode(PlayerObjectMode pi)
        {
            playerTangible = pi;
            playerVisible = pi;
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!visible) return;
            color.A = alpha;

        }

        public void Blink() //This can make a sprite blink! It's like Animate in Animated_Sprite, only with differentness. 
        {
            if (blinkCounter == blinkRate) //If you have reached the end of a blink life
            {
                //Then we flip the visiblity
                visible = !visible;
                blinkCounter = 0; //And reset our counter
            }
            blinkCounter++; //And increase it, too
        }
    }
}
