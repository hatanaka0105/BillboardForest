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
    class DrawObject
    {
        protected Model model;
        protected Material material;

        protected Vector3 position;
        protected Vector3 rotation;
        protected Vector3 scale;
        protected Matrix world;

        protected BoundingBox boundingBox;
        protected BoundingSphere boundingSphere;

        public virtual void Initialize()
        {
            world = Matrix.Identity;
        }

        public virtual void Load(Game game)
        {
        }

        public virtual void Update()
        {
        }

        public virtual void Draw(Camera camera, Color fogColor)
        {
            if (material == null)
                DrawBasic(camera, fogColor);
        }

        public void DrawBasic(Camera camera, Color fogColor)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.FogEnabled = true;
                    effect.FogColor = fogColor.ToVector3();
                    effect.FogStart = ConstantMacro.fogStart;
                    effect.FogEnd = ConstantMacro.fogEnd;

                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.World = world;
                }

                mesh.Draw();
            }
        }
    }
}
