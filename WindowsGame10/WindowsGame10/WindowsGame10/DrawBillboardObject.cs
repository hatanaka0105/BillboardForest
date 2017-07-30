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
    class DrawBillboardObject
    {
        protected Model model;
        protected Material material;

        protected Vector3 position;
        protected float rotation;
        protected Vector3 scale;
        protected Matrix world;

        protected BoundingBox boundingBox;
        protected BoundingSphere boundingSphere;

        protected const int vertexNum = 4;
        protected VertexBuffer vertexBuffer;
        protected VertexPositionColor[] vertexPositionColor = new VertexPositionColor[vertexNum];
        protected VertexPositionTexture[] vertexPositionTexture = new VertexPositionTexture[vertexNum];

        public virtual void Initialize(Game game)
        {
            material = new Material(game);
            world = Matrix.Identity;

            rotation = 0;
            position = Vector3.UnitZ* 500 + Vector3.UnitY * 100;
            scale = Vector3.One * 100;
        }

        protected virtual void SetVertexPositionColor()
        {
            vertexPositionColor[0] = new VertexPositionColor(
                new Vector3(-(float)Math.Cos(MathHelper.ToRadians(rotation)),
                    1,
                    (float)Math.Sin(MathHelper.ToRadians(rotation))) * scale + position,
                    Color.Blue);

            vertexPositionColor[1] = new VertexPositionColor(
                new Vector3((float)Math.Cos(MathHelper.ToRadians(rotation)),
                    1,
                    (float)Math.Sin(MathHelper.ToRadians(rotation))) * scale + position,
                    Color.Blue);

            vertexPositionColor[2] = new VertexPositionColor(
                new Vector3(-(float)Math.Cos(MathHelper.ToRadians(rotation)),
                    1,
                    -(float)Math.Sin(MathHelper.ToRadians(rotation))) * scale + position,
                    Color.Blue);

            vertexPositionColor[3] = new VertexPositionColor(
                new Vector3((float)Math.Cos(MathHelper.ToRadians(rotation)),
                    1,
                    -(float)Math.Sin(MathHelper.ToRadians(rotation))) * scale + position,
                    Color.Blue);
        }

        protected virtual void SetVertexPositionTexture()
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

            vertexPositionTexture[3] = new VertexPositionTexture(
                new Vector3((float)Math.Cos(MathHelper.ToRadians(rotation)),
                    -1,
                    (float)Math.Sin(MathHelper.ToRadians(rotation))) * scale + position,
                    new Vector2(1.0f, 1.0f));
        }

        public virtual void Load(Game game)
        {
            material.diffuseMap = game.Content.Load<Texture2D>("Tree_Billboard");

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

        public virtual void Draw(Game game, Camera camera)
        {
            game.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            material.basicEffect.View = camera.View;
            material.basicEffect.Projection = camera.Projection;
            material.basicEffect.World = world;

            material.basicEffect.FogEnabled = true;
            material.basicEffect.FogColor = ConstantMacro.backColor.ToVector3();
            material.basicEffect.FogStart = ConstantMacro.fogStart;
            material.basicEffect.FogEnd = ConstantMacro.fogEnd;

            // 頂点バッファをセットします
            game.GraphicsDevice.SetVertexBuffer(vertexBuffer);

            // パスの数だけ繰り替えし描画
            foreach (EffectPass pass in material.basicEffect.CurrentTechnique.Passes)
            {
                // パスの開始
                pass.Apply();

                // ポリゴン描画する
                game.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }
        }
    }
}
