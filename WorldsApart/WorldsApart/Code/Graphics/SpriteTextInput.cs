using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WorldsApart.Code.Graphics
{

    /// <summary>
    /// Delegate to fire from a SpriteTextInput object when the enter key is pressed.
    /// </summary>
    /// <param name="text">The contents of the SpriteTextInput's text string.</param>
    public delegate void InputEnterDelegate(string text);

    /// <summary>
    /// A sophisticated text-input field that is a subclass of SpriteText. Currently only interacts with keyboard.
    /// </summary>
    /// <remarks>Should probably add blur/focus delegates and properties. We could also add more support for alignment, etc.</remarks>
    class SpriteTextInput : SpriteText
    {

        /// <summary>
        /// An array of all of the keys currently depressed.
        /// </summary>
        Keys[] previousKeys;
        /// <summary>
        /// The position of the primary cursor.
        /// </summary>
        int cursor = 0;
        /// <summary>
        /// The position of the secondary cursor. If the primary and secondary cursors are different, than a selection is being made.
        /// </summary>
        int selector = 0;
        /// <summary>
        /// Whether or not the input field currently has focus.
        /// </summary>
        public bool hasFocus = false;
        /// <summary>
        /// The uneditable pre-text before the input. This is useful for command prompts.
        /// </summary>
        public string preText;
        /// <summary>
        /// The delegate to fire if the enter button is depressed.
        /// </summary>
        InputEnterDelegate delegateEnter;

        /// <summary>
        /// The constructor. Creates a new SpriteTextInput.
        /// </summary>
        /// <param name="font">The SpriteFont to use.</param>
        /// <param name="position">The position to render the text.</param>
        /// <param name="text">The string of text.</param>
        /// <param name="delegateEnter">The delegate to fire when the enter button is depressed.</param>
        /// <param name="preText">The pre-text prompt in the textfield.</param>
        public SpriteTextInput(SpriteFont font, Vector2 position, string text = "", InputEnterDelegate delegateEnter = null, string preText = "")
            : base(font, position, text)
        {
            SetText(string.Concat(preText, text));
            this.delegateEnter = delegateEnter;
            this.preText = preText;

            cursor = this.text.Length;
            selector = cursor;
        }
        /// <summary>
        /// Checks to see what keys are depressed, and updates text accordingly.
        /// </summary>
        /// <param name="gameTime">The current GameTime passed along from the Game.Update method.</param>
        /// <param name="keyboardState">The current KeyboardState, passed along from the Game.Update method.</param>
        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (!hasFocus) return;
            Keys[] keys = keyboardState.GetPressedKeys();
            int cursorMin = preText.Length;

            if (previousKeys != null)
            {

                bool shift = false;
                bool ctrl = false;
                if (keys.Contains<Keys>(Keys.LeftShift) || keys.Contains<Keys>(Keys.RightShift)) shift = true;
                if (keys.Contains<Keys>(Keys.LeftControl) || keys.Contains<Keys>(Keys.RightControl)) ctrl = true;

                foreach (Keys key in keys)
                {
                    if (!previousKeys.Contains<Keys>(key))
                    {
                        char add = '\0';
                        if (key >= Keys.A && key <= Keys.Z)
                        {
                            if (shift)
                            {
                                add = (char)key;
                            }
                            else
                            {
                                add = (char)(key + 32);
                            }
                        }
                        else if (key >= Keys.D0 && key <= Keys.D9)
                        {
                            if (shift)
                            {
                                switch (key)
                                {
                                    case Keys.D1:
                                        add = '!';
                                        break;
                                    case Keys.D2:
                                        add = '@';
                                        break;
                                    case Keys.D3:
                                        add = '#';
                                        break;
                                    case Keys.D4:
                                        add = '$';
                                        break;
                                    case Keys.D5:
                                        add = '%';
                                        break;
                                    case Keys.D6:
                                        add = '^';
                                        break;
                                    case Keys.D7:
                                        add = '&';
                                        break;
                                    case Keys.D8:
                                        add = '*';
                                        break;
                                    case Keys.D9:
                                        add = '(';
                                        break;
                                    case Keys.D0:
                                        add = ')';
                                        break;
                                }
                            }
                            else
                            {
                                add = (char)key;
                            }
                        }
                        else if ((key >= Keys.NumPad0 && key <= Keys.NumPad9)
                          || key == Keys.Multiply
                          || key == Keys.Divide
                          || key == Keys.Add
                          || key == Keys.Subtract)
                        {
                            add = (char)key;
                        }
                        else if (key == Keys.Back)
                        {
                            if (selector == cursor)
                            {
                                if (cursor > cursorMin && text.Length > cursorMin)
                                {
                                    text = text.Remove(cursor - 1, 1);
                                    cursor--;
                                    selector = cursor;
                                }
                            }
                            else if (selector > cursor)
                            {
                                if (selector <= text.Length && cursor >= cursorMin)
                                {
                                    text = text.Remove(cursor, selector - cursor);
                                    selector = cursor;
                                }
                            }
                            else if (cursor > selector)
                            {
                                if (cursor <= text.Length && selector >= cursorMin)
                                {
                                    text = text.Remove(selector, cursor - selector);
                                    cursor = selector;
                                }
                            }

                        }
                        else if (key == Keys.Delete)
                        {
                            if (selector == cursor)
                            {
                                if (cursor < text.Length && text.Length > cursorMin)
                                {
                                    text = text.Remove(cursor, 1);
                                    selector = cursor;
                                }
                            }
                            else if (selector > cursor)
                            {
                                if (selector <= text.Length && cursor >= cursorMin)
                                {
                                    text = text.Remove(cursor, selector - cursor);
                                    selector = cursor;
                                }
                            }
                            else if (cursor > selector)
                            {
                                if (cursor <= text.Length && selector >= cursorMin)
                                {
                                    text = text.Remove(selector, cursor - selector);
                                    cursor = selector;
                                }
                            }
                        }
                        else if (key == Keys.Space)
                        {
                            add = ' ';
                        }
                        else if (key == Keys.OemSemicolon)
                        {
                            add = shift ? ':' : ';';
                        }
                        else if (key == Keys.OemComma)
                        {
                            add = shift ? '<' : ',';
                        }
                        else if (key == Keys.OemPeriod)
                        {
                            add = shift ? '>' : '.';
                        }
                        else if (key == Keys.OemQuestion)
                        {
                            add = shift ? '?' : '/';
                        }
                        else if (key == Keys.Oem8)
                        {
                            add = shift ? '~' : '`';
                        }
                        else if (key == Keys.OemPipe || key == Keys.OemQuotes)
                        {
                            add = shift ? '|' : '\\';
                        }
                        else if (key == Keys.OemTilde)
                        {
                            add = shift ? '"' : '\'';
                        }
                        else if (key == Keys.OemPlus)
                        {
                            add = shift ? '+' : '=';
                        }
                        else if (key == Keys.OemMinus)
                        {
                            add = shift ? '_' : '-';
                        }
                        else if (key == Keys.OemOpenBrackets)
                        {
                            add = shift ? '{' : '[';
                        }
                        else if (key == Keys.OemCloseBrackets)
                        {
                            add = shift ? '}' : ']';
                        }
                        else if (key == Keys.Left)
                        {
                            cursor--;
                            if (cursor < cursorMin) cursor = cursorMin;
                            if (!shift) selector = cursor;
                        }
                        else if (key == Keys.Right)
                        {
                            cursor++;
                            if (cursor > text.Length) cursor = text.Length;
                            if (!shift) selector = cursor;
                        }
                        else if (key == Keys.Home)
                        {
                            cursor = cursorMin;
                            if (!shift) selector = cursor;
                        }
                        else if (key == Keys.End)
                        {
                            cursor = text.Length;
                            if (!shift) selector = cursor;
                        }
                        else if (key == Keys.Enter)
                        {
                            delegateEnter(GetText());
                            //text = preText;
                            //selector = cursor = text.Length;
                            break;
                        }
                        else
                        {
                            //text = string.Format("{0}", key);
                            //break;
                        }
                        if (add != '\0')
                        {
                            if (cursor == selector)
                            {
                                text = text.Insert(cursor, add.ToString());
                                cursor++;
                            }
                            else if (cursor < selector)
                            {
                                text = string.Concat(text.Substring(0, cursor), add.ToString(), text.Substring(selector));
                                cursor++;
                            }
                            else if (cursor > selector)
                            {
                                text = string.Concat(text.Substring(0, selector), add.ToString(), text.Substring(cursor));
                                cursor = selector + 1;
                            }

                            selector = cursor;
                        }
                        break; // assume the user only pushed one new key
                    }
                }
            }

            previousKeys = keys;
        }
        /// <summary>
        /// Returns the string of text (without the pre-text).
        /// </summary>
        /// <returns>Returns the string of text (without the pre-text).</returns>
        public string GetText()
        {
            return text.Substring(preText.Length);
        }
        /// <summary>
        /// Sets the text while appending any pre-text.
        /// </summary>
        /// <param name="text">What to set the text string to.</param>
        public void SetText(string text)
        {
            if (text != null)
            {
                this.text = string.Concat(preText, text);
                cursor = this.text.Length;
                selector = cursor;
            }
        }
        /// <summary>
        /// Overrides superclass SpriteText to draw the text to the screen. This method also draws a cursor and highlights any selected text.
        /// </summary>
        /// <param name="spriteBatch">Game.spriteBatch should be passed in here.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {

            Vector2 typeSize = font.MeasureString(text.Substring(0, cursor));
            Color color;

            int cursorOffset = (int)typeSize.X;

            int X = (int)position.X + cursorOffset;
            int Y = (int)position.Y;
            int W = 1;
            int H = (int)typeSize.Y;

            if (selector != cursor)
            {
                if (selector > cursor)
                {
                    W = (int)font.MeasureString(text.Substring(cursor, selector - cursor)).X;
                }
                else
                {
                    W = (int)font.MeasureString(text.Substring(selector, cursor - selector)).X;
                    X -= W;
                }
                color = Color.Lerp(Color.White, Color.Transparent, 0.7F);
            }
            else
            {
                color = Color.Lerp(Color.White, Color.Transparent, 0.2F);
            }

            //spriteBatch.Draw(Game1.pixel, new Rectangle(X, Y, W, H), color);

            base.Draw(spriteBatch);
        }
    }
}
