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
using SkinnedModel;

namespace BillboardForest
{
    /// <summary>
    /// 基底 Game クラスから派生した、ゲームのメイン クラスです。
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// グラフィックスデバイスマネージャー
        /// </summary>
        GraphicsDeviceManager graphics;

        private const int width = 1280;
        private const int height = 720;

        /// <summary>
        /// カメラ
        /// </summary>
        private Camera camera;

        private Player player;

        private List<DrawObject> drawObject = new List<DrawObject>();

        /// <summary>
        /// スプライトバッチ
        /// </summary>
        private SpriteBatch spriteBatch;

        public bool keyInput = false;

        Game game;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Game1()
        {
            // デバイスマネージャの生成する
            graphics = new GraphicsDeviceManager(this);

            this.graphics.PreferredBackBufferWidth = width;
            this.graphics.PreferredBackBufferHeight = height;

            //アンチエイリアシング
            graphics.PreferMultiSampling = true;

            Window.Title = "BillboardForest";

            // コンテントのディレクトリを"Content"に設定する
            Content.RootDirectory = "Content";

            game = this;
        }

        /// <summary>
        /// 初期化のタイミングにフレームワークから呼び出されます
        /// </summary>
        protected override void Initialize()
        {
            // インプットマネージャーの初期化
            Input.Initialize();

            player = new Player();
            player.Initialize();

            drawObject.Add(new Ground());
            drawObject.Add(new Tree());
            foreach (DrawObject dObj in drawObject)
            {
                dObj.Initialize(this);
            }

            // カメラの初期化
            InitializeCamera();

            base.Initialize();
        }

        /// <summary>
        /// カメラの初期化
        /// </summary>
        private void InitializeCamera()
        {
            // カメラを生成する
            camera = new Camera();

            // パラメータを設定
            camera.FieldOfView = MathHelper.ToRadians(45.0f);
            camera.AspectRatio = (float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height;
            camera.NearPlaneDistance = 1.0f;
            camera.FarPlaneDistance = 5000.0f;
            camera.ReferenceTranslate = new Vector3(0.0f, 0.0f, 300.0f);
            camera.Target = new Vector3(0.0f, 100.0f, 0.0f);
        }

        /// <summary>
        /// コンテンツ読み込みのタイミングにフレームワークから呼び出されます
        /// </summary>
        protected override void LoadContent()
        {
            // スプライトバッチの作成
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player.LoadSkinnedModel(@"C_Skinman", game);

            foreach (DrawObject dObj in drawObject)
            {
                dObj.Load(game);
            }
        }

        /// <summary>
        /// コンテンツ解放のタイミングにフレームワークから呼び出されます
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// アップデートのタイミングにフレームワークから呼び出されます
        /// </summary>
        /// <param name="gameTime">ゲームタイム</param>
        protected override void Update(GameTime gameTime)
        {
            // インプットマネージャーのアップデート
            InputManager.Update();

            // 終了ボタンのチェック
            if (InputManager.IsJustKeyDown(Keys.Escape))
                Exit();

            // 入力を取得する
            player.UpdateInput(gameTime);
            // アニメーションの更新
            player.UpdateAnimation(gameTime, true);

            // カメラの更新
            camera.Update(gameTime);

            foreach (DrawObject dObj in drawObject)
            {
                dObj.Update();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// 描画のタイミングにフレームワークから呼び出されます
        /// </summary>
        /// <param name="gameTime">ゲームタイム</param>
        protected override void Draw(GameTime gameTime)
        {
            // 背景を塗りつぶす
            graphics.GraphicsDevice.Clear(ConstantMacro.backColor);

            graphics.GraphicsDevice.BlendState = BlendState.Opaque;
            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphics.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            player.Draw(camera);

            foreach (DrawObject dObj in drawObject)
            {
                dObj.Draw(camera, ConstantMacro.backColor);
            }

            base.Draw(gameTime);
        }
    }
}
