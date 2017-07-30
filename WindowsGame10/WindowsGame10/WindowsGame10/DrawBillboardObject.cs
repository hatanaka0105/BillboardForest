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
        protected Vector3 rotation;
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

            position = Vector3.UnitY * 100;
            scale = Vector3.One * 100;
        }

        protected virtual void SetVertexPositionColor()
        {
            vertexPositionColor[0] = new VertexPositionColor(
                new Vector3(-1, 1, 0) * scale + position, Color.Blue);

            vertexPositionColor[1] = new VertexPositionColor(
                new Vector3(1, 1, 0) * scale + position, Color.Green);

            vertexPositionColor[2] = new VertexPositionColor(
                new Vector3(-1, -1, 0) * scale + position, Color.Red);

            vertexPositionColor[3] = new VertexPositionColor(
                new Vector3(1, -1, 0) * scale + position, Color.Yellow);
        }

        protected virtual void SetVertexPositionTexture()
        {
            vertexPositionTexture[0] = new VertexPositionTexture(
                new Vector3(-1, 1, 0) * scale + position, new Vector2(0.0f, 0.0f));

            vertexPositionTexture[1] = new VertexPositionTexture(
                new Vector3(1, 1, 0) * scale + position, new Vector2(1.0f, 0.0f));

            vertexPositionTexture[2] = new VertexPositionTexture(
                new Vector3(-1, -1, 0) * scale + position, new Vector2(0.0f, 1.0f));

            vertexPositionTexture[3] = new VertexPositionTexture(
                new Vector3(1, -1, 0) * scale + position, new Vector2(1.0f, 1.0f));
        }

        public virtual void Load(Game game)
        {
            if (material.diffuseMap == null)
            {
                SetVertexPositionColor();
                vertexBuffer = new VertexBuffer(game.GraphicsDevice,
                               typeof(VertexPositionColor),
                               vertexPositionColor.Length,
                               BufferUsage.None);

                vertexBuffer.SetData(vertexPositionColor);
            }
            else
            {
                SetVertexPositionTexture();
                vertexBuffer = new VertexBuffer(game.GraphicsDevice,
                               typeof(VertexPositionTexture),
                               vertexPositionTexture.Length,
                               BufferUsage.None);

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
            material.basicEffect.View = camera.View;
            material.basicEffect.Projection = camera.Projection;
            material.basicEffect.World = world;

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
