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
    class BillboardTree : DrawBillboardObject
    {
        public override void Initialize(Game game)
        {
            material = new Material(game);
            world = Matrix.Identity;

            rotation = 0;
            position = Vector3.UnitZ * 500 + Vector3.UnitY * 100;
            scale = Vector3.One * 100;
        }

        protected override void SetVertexPositionTexture()
        {
            vertexPositionTexture[0] = new VertexPositionTexture(
                 new Vector3(-(float)Math.Cos(MathHelper.ToRadians(rotation)),
                    1,
                    -(float)Math.Sin(MathHelper.ToRadians(rotation))) * scale + position,
                    new Vector2(0.0f, 0.0f));

            vertexPositionTexture[1] = new VertexPositionTexture(
                new Vector3((float)Math.Cos(MathHelper.ToRadians(rotation)),
                    1,
                    (float)Math.Sin(MathHelper.ToRadians(rotation))) * scale + position,
                    new Vector2(1.0f, 0.0f));

            vertexPositionTexture[2] = new VertexPositionTexture(
                new Vector3(-(float)Math.Cos(MathHelper.ToRadians(rotation)),
                    -1,
                    -(float)Math.Sin(MathHelper.ToRadians(rotation))) * scale + position,
                    new Vector2(0.0f, 1.0f));
        }

        public override void Load(Game game)
        {
            if (material.diffuseMap == null)
            {
                vertexBuffer = new VertexBuffer(game.GraphicsDevice,
                               typeof(VertexPositionColor),
                               vertexNum,
                               BufferUsage.None);

                SetVertexPositionColor();

                vertexBuffer.SetData(vertexPositionColor);
            }
            else
            {
                vertexBuffer = new VertexBuffer(game.GraphicsDevice,
                               typeof(VertexPositionTexture),
                               vertexNum,
                               BufferUsage.None);

                SetVertexPositionTexture();

                vertexBuffer.SetData(vertexPositionTexture);
                material.basicEffect.Texture = material.diffuseMap;
            }

            if (material.diffuseMap != null)
            {
                material.basicEffect.TextureEnabled = true;
                material.basicEffect.Texture = material.diffuseMap;
            }
        }

        public virtual void Update()
        {
        }
    }
}
