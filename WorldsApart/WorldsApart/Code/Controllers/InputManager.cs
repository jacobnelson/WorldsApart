using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WorldsApart.Code.Controllers
{
    static class InputManager
    {
        static GamePadState gps;
        static public KeyboardState ks;
        static MouseState ms;

        static GamePadState gpsPrev;
        static KeyboardState ksPrev;
        static MouseState msPrev;

        static GamePadState gps2;
        static GamePadState gps2Prev;

        public static void UpdateStates(GamePadState gpsNew, GamePadState gps2New, KeyboardState ksNew, MouseState msNew)
        {
            gpsPrev = gps;
            gps2Prev = gps2;
            ksPrev = ks;
            msPrev = ms;
            gps = gpsNew;
            gps2 = gps2New;
            ks = ksNew;
            ms = msNew;
        }

        public static bool IsKeyDown(Keys key)
        {
            return ks.IsKeyDown(key);
        }

        public static bool IsKeyUp(Keys key)
        {
            return ks.IsKeyUp(key);
        }

        public static bool IsKeyPressed(Keys key)
        {
            return ks.IsKeyDown(key) && ksPrev.IsKeyUp(key);
        }

        public static bool IsKeyReleased(Keys key)
        {
            return ks.IsKeyUp(key) && ksPrev.IsKeyDown(key);
        }

        public static bool IsButtonDown(Buttons button)
        {
            return gps.IsButtonDown(button);
        }

        public static bool IsButtonUp(Buttons button)
        {
            return gps.IsButtonUp(button);
        }

        public static bool IsButtonPressed(Buttons button)
        {
            return gps.IsButtonDown(button) && gpsPrev.IsButtonUp(button);
        }

        public static bool IsButtonReleased(Buttons button)
        {
            return gps.IsButtonUp(button) && gpsPrev.IsButtonDown(button);
        }

        public static Vector2 GetLeftThumbstick()
        {
            return gps.ThumbSticks.Left;
        }

        public static Vector2 GetRightThumbstick()
        {
            return gps.ThumbSticks.Right;
        }

        public static bool IsButtonDown2(Buttons button)
        {
            return gps2.IsButtonDown(button);
        }

        public static bool IsButtonUp2(Buttons button)
        {
            return gps2.IsButtonUp(button);
        }

        public static bool IsButtonPressed2(Buttons button)
        {
            return gps2.IsButtonDown(button) && gps2Prev.IsButtonUp(button);
        }

        public static bool IsButtonReleased2(Buttons button)
        {
            return gps2.IsButtonUp(button) && gps2Prev.IsButtonDown(button);
        }

        public static Vector2 GetLeftThumbstick2()
        {
            return gps2.ThumbSticks.Left;
        }

        public static Vector2 GetRightThumbstick2()
        {
            return gps2.ThumbSticks.Right;
        }

        public static bool GetLeftMouseDown()
        {
            return ms.LeftButton == ButtonState.Pressed;
        }
        public static bool GetLeftMousePressed()
        {
            return ms.LeftButton == ButtonState.Pressed && msPrev.LeftButton == ButtonState.Released;
        }
        public static bool GetLeftMouseReleased()
        {
            return ms.LeftButton == ButtonState.Released && msPrev.LeftButton == ButtonState.Pressed;
        }

        public static bool GetRightMouseDown()
        {
            return ms.RightButton == ButtonState.Pressed;
        }
        public static bool GetRightMousePressed()
        {
            return ms.RightButton == ButtonState.Pressed && msPrev.RightButton == ButtonState.Released;
        }
        public static bool GetRightMouseReleased()
        {
            return ms.RightButton == ButtonState.Released && msPrev.RightButton == ButtonState.Pressed;
        }
    }
}
