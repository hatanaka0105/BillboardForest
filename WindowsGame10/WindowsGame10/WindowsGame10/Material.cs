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
    class Material
    {
        public Texture2D diffuseMap;
        public Color diffuseColor;

        public Texture2D specularMap;
        public Color specularColor;
        public float specularPower;

        public Color emissiveColor;
        public float emissivePower;

        public Texture2D normalMap;

        public Effect effect;
        public BasicEffect basicEffect;
    }
}
