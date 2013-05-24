using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WorldsApart.Code.Graphics
{
    /// <summary>
    /// This class renders text to the screen.
    /// </summary>
    class SpriteText
    {

        public Vector2 position = Vector2.Zero;
        public float rotation = 0;
        public Vector2 scale = Vector2.One;
        public Color color = Color.White;
        public Vector2 anchor = Vector2.Zero;
        public SpriteEffects spriteEffects = SpriteEffects.None;

        private float _alpha = 1;
        public float alpha
        {
            get
            {
                return _alpha;
            }
            set
            {
                if (value > 1) _alpha = 1;
                else if (value < 0) _alpha = 0;
                else _alpha = value;

                alphaByte = (byte)(_alpha * 255);
            }
        }
        protected byte alphaByte = 255;
        private string _text;
        public string text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                if (maxWidth > 0)
                {
                    while (font.MeasureString(_text).X > maxWidth)
                    {
                        for (int i = 0; i < _text.Length; i++)
                        {
                            if (font.MeasureString(_text.Substring(0, i)).X > maxWidth)
                            {
                                _text = string.Format("{0}\n{1}", _text.Substring(0, i - 2), _text.Substring(i - 2));
                            }
                        }
                    }
                }
            }
        }
        public int maxWidth = 0;
        protected SpriteFont font;

        public SpriteText(SpriteFont font, Vector2 position, string text = "")
        {

            this.font = font;
            this.position = position;
            this.text = text;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            color.A = alphaByte;
            spriteBatch.DrawString(font, text, position, color, rotation, anchor, scale, spriteEffects, 1);
        }
    }
}
