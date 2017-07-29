using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame10
{
    public struct MoveVectorKey
    {
        public Keys up;
        public Keys down;
        public Keys right;
        public Keys left;
    }

    public static class Input
    {
        static float angle = 0;

        static MoveVectorKey moveVecKey;

        public static void Initialize()
        {
            InputManager.Initialize();
            moveVecKey.up = Keys.W;
            moveVecKey.down = Keys.S;
            moveVecKey.right = Keys.D;
            moveVecKey.left = Keys.A;
        }

        public static float GetRotationAngle()
        {
            if (InputManager.IsKeyDown(moveVecKey.left))
            {
                angle =  - 90;
            }

            if (InputManager.IsKeyDown(moveVecKey.right))
            {
                angle = 90;
            }

            if (InputManager.IsKeyDown(moveVecKey.down))
            {
                angle = 0;
            }

            if (InputManager.IsKeyDown(moveVecKey.up))
            {
                angle = 180;
            }

            if (InputManager.IsKeyDown(moveVecKey.up) && InputManager.IsKeyDown(moveVecKey.right))
            {
                angle = 135;
            }

            if (InputManager.IsKeyDown(moveVecKey.up) && InputManager.IsKeyDown(moveVecKey.left))
            {
                angle = -135;
            }

            if (InputManager.IsKeyDown(moveVecKey.down) && InputManager.IsKeyDown(moveVecKey.right))
            {
                angle = 45;
            }

            if (InputManager.IsKeyDown(moveVecKey.down) && InputManager.IsKeyDown(moveVecKey.left))
            {
                angle = -45;
            }

            return angle;
        }

        public static bool IsMoveKeyDown()
        {
            return InputManager.IsKeyDown(moveVecKey.left)
                || InputManager.IsKeyDown(moveVecKey.right)
                || InputManager.IsKeyDown(moveVecKey.down)
                || InputManager.IsKeyDown(moveVecKey.up);
        }
    }
}
