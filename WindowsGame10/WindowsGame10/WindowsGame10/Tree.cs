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

namespace BillboardForest
{
    class Tree : DrawObject
    {
        public override void Load(Game game)
        {
            model = game.Content.Load<Model>("Tree");
            material.diffuseMap = game.Content.Load<Texture2D>("Tree_D");
            material.diffuseColor = Color.Green;

            scale = new Vector3 (50, 45, 50);
            position = new Vector3(200, 0, 300);

            world = Matrix.CreateScale(scale) * Matrix.CreateTranslation (position);

            base.Load(game);
        }
    }
}
