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

namespace WindowsGame10
{
    class Player
    {
        private Model model;

        /// <summary>
        /// アニメーションデータ
        /// </summary>
        private SkinningData skinningData;

        /// <summary>
        /// アニメーションプレーヤー
        /// </summary>
        private AnimationPlayer animationPlayer;

        /// <summary>
        /// クリップ名配列のインデックス
        /// </summary>
        int clipIndex;

        /// <summary>
        /// クリップ名配列
        /// これはこのサンプルで使用しているC_Skinman.fbxに組み込まれているものです。
        /// </summary>
        string[] clipNames = { "idle", "walk", "jump", "run", "set", "ready", "winner", "appeal" };

        private enum ClipNames
        {
            idle,
            walk,
            jump,
            run,
            set,
            ready,
            winner,
            appeal
        };

        /// <summary>
        /// アニメーションのループ再生フラグ
        /// </summary>
        bool loopEnable;

        /// <summary>
        /// アニメーションの一時停止フラグ
        /// </summary>
        bool pauseEnable;

        /// <summary>
        /// アニメーションのスローモーション再生速度
        /// １より大きくなるにしたがって再生速度が遅くなります。
        /// </summary>
        int slowMotionOrder;
        int slowMotionCount;

        /// <summary>
        /// ワールド変換行列
        /// </summary>
        private Matrix worldMatrix;

        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 position;

        /// <summary>
        /// 回転量
        /// </summary>
        private Vector3 rotation;

        private AnimationClip oldClip;

        private int countRun = 0;
        private const int countRunMax = 100;

        private bool isKeyDown = false;

        public void Initialize()
        {
            // アニメーション用データを初期化
            InitializeAnimationValue();

            // 各種座標データを初期化
            InitializeCoordinateValue();
        }

        /// <summary>
        /// アニメーション用の変数を初期化
        /// </summary>
        private void InitializeAnimationValue()
        {
            // クリップ名配列インデックスを初期化
            clipIndex = 0;

            // ループ再生を有効
            loopEnable = true;

            // 一時停止フラグを無効
            pauseEnable = false;

            // スローモーション速度を等速
            slowMotionOrder = 0;
            slowMotionCount = 0;
        }

        /// <summary>
        /// 座標データの初期化
        /// </summary>
        private void InitializeCoordinateValue()
        {
            position = Vector3.Zero;
            rotation = Vector3.Zero;
            worldMatrix = Matrix.Identity;
        }

        /// <summary>
        /// スキンモデルの読み込み処理
        /// </summary>
        public void LoadSkinnedModel(string assetName, Game game)
        {
            // モデルを読み込む
            model = game.Content.Load<Model>(assetName);

            // SkinningDataを取得
            skinningData = model.Tag as SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            // AnimationPlayerを作成
            animationPlayer = new AnimationPlayer(skinningData);

            // アニメーションを再生する
            ChangeAnimationClip(clipNames[clipIndex], loopEnable, 0.0f);
        }

        /// <summary>
        /// アニメーションの切替処理
        /// </summary>
        public void ChangeAnimationClip(string clipName, bool loop, float weight)
        {
            // クリップ名からAnimationClipを取得して再生する
            AnimationClip clip = skinningData.AnimationClips[clipName];

            if (clip == oldClip) return;

            oldClip = clip;
            animationPlayer.StartClip(clip, loop, weight);
        }

        /// <summary>
        /// 入力による処理
        /// </summary>
        public void UpdateInput(GameTime gameTime)
        {
            // モデルの座標を更新
            UpdateModelCoordinates(gameTime);

            // アニメーションの操作
            UpdateAnimationControl(gameTime);
        }

        /// <summary>
        /// アニメーションの更新
        /// </summary>
        public void UpdateAnimation(GameTime gameTime, bool relativeToCurrentTime)
        {
            // 一時停止状態でないか？
            if (pauseEnable)
                return;

            // スローモーションが有効か？
            if (slowMotionOrder > 0)
            {
                if (slowMotionCount > 0)
                {
                    slowMotionCount--;
                    return;
                }
                slowMotionCount = slowMotionOrder;
            }

            // アニメーションの更新
            animationPlayer.Update(gameTime.ElapsedGameTime, true, worldMatrix);
        }

        /// <summary>
        /// モデルの座標を更新
        /// </summary>
        private void UpdateModelCoordinates(GameTime gameTime)
        {
            // 左右スティックの入力を取得する
            rotation.Y = MathHelper.ToRadians(Input.GetRotationAngle());

            isKeyDown = Input.IsMoveKeyDown();

            if (isKeyDown) 
            {
                if (countRun < countRunMax)
                {
                    countRun++;
                    ChangeAnimationClip("walk", true, 0.0f);
                }
                else
                    ChangeAnimationClip("run", true, 0.0f);
            }
            else
            {
                countRun = 0;
                ChangeAnimationClip("idle", true, 0.0f);
            }

            // 回転行列の作成
            Matrix rotationMatrix = Matrix.CreateRotationX(rotation.X) *
                                    Matrix.CreateRotationY(rotation.Y) *
                                    Matrix.CreateRotationZ(rotation.Z);

            // 平行移動行列の作成
            Matrix translationMatrix = Matrix.CreateTranslation(position);

            // ワールド変換行列を計算する
            // モデルを拡大縮小し、回転した後、指定の位置へ移動する。
            worldMatrix = rotationMatrix * translationMatrix;

            // 初期値に戻す
            if (InputManager.IsJustKeyDown(Keys.R))
            {
                // 各種座標データを初期化
                InitializeCoordinateValue();
            }
        }

        /// <summary>
        /// アニメーションの操作
        /// </summary>
        private void UpdateAnimationControl(GameTime gameTime)
        {
            // 一時停止操作
            if (InputManager.IsJustButtonDown(PlayerIndex.One, Buttons.Start) || InputManager.IsJustKeyDown(Keys.V))
                pauseEnable = (pauseEnable) ? false : true;

            // クリップ名変更操作
            if (InputManager.IsJustPressedDPadUp(PlayerIndex.One) || InputManager.IsJustKeyDown(Keys.Up))
            {
                clipIndex++;
                loopEnable = true;

                // ループ再生を禁止するか？
                if (InputManager.IsButtonDown(PlayerIndex.One, Buttons.B) || InputManager.IsKeyDown(Keys.LeftControl))
                    loopEnable = false;

            }
            if (InputManager.IsJustPressedDPadDown(PlayerIndex.One) || InputManager.IsJustKeyDown(Keys.Down))
            {
                clipIndex--;
                loopEnable = true;

                // ループ再生を禁止するか？
                if (InputManager.IsButtonDown(PlayerIndex.One, Buttons.B) || InputManager.IsKeyDown(Keys.LeftControl))
                    loopEnable = false;
            }

            // 範囲を超えたら初期化
            if (clipIndex >= clipNames.Length)
                clipIndex = clipNames.Length - 1;
            if (clipIndex < 0)
                clipIndex = 0;

            // 初期値に戻す
            if (InputManager.IsJustButtonDown(PlayerIndex.One, Buttons.X) || InputManager.IsJustKeyDown(Keys.N))
            {
                // アニメーション用データを初期化
                InitializeAnimationValue();
                // クリップを切り替える
                ChangeAnimationClip(clipNames[0], true, 0.0f);
            }
        }

        public void Draw(Camera camera)
        {
            Matrix[] bones = animationPlayer.GetSkinTransforms();
            Matrix view = camera.View;
            Matrix projection = camera.Projection;

            // モデルを描画
            foreach (ModelMesh mesh in model.Meshes)
            {
                string name = mesh.Name;
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }
    }
}
