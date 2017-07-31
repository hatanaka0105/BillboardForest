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
    class TreeManager
    {
        private Model model;
        private Material material;

        private Material billboardMaterial;

        private List<DrawObject> tree = new List<DrawObject>();
        private List<DrawBillboardObject> billboardTree = new List<DrawBillboardObject>();

        private Game game;

        public void Initialzie(Game game)
        {
            this.game = game;

            tree.Add(new Tree());
            billboardTree.Add(new BillboardTree());

            foreach (DrawObject dObj in tree)
            {
                dObj.Initialize(game);
            }

            foreach (DrawBillboardObject dBObj in billboardTree)
            {
                dBObj.Initialize(game);
            }
        }

        public void Load()
        {
            model = game.Content.Load<Model>("Tree");
            material = new Material(game);
            material.diffuseMap = game.Content.Load<Texture2D>("Tree_D");
            foreach (DrawObject dObj in tree)
            {
                dObj.Load(game, model, material.diffuseMap);
            }

            billboardMaterial = new Material(game);
            billboardMaterial.diffuseMap = game.Content.Load<Texture2D>("Tree_Billboard");
            foreach (DrawBillboardObject dBObj in billboardTree)
            {
                dBObj.Load(game, billboardMaterial.diffuseMap);
            }
        }

        public void Update()
        {

        }

        public void Draw(Camera camera)
        {
            foreach (DrawObject dObj in tree)
            {
                dObj.Draw (camera);
            }

            foreach (DrawBillboardObject dBObj in billboardTree)
            {
                dBObj.Draw (game, camera);
            }
        }
    }
}
