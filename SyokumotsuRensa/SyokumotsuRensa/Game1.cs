// このファイルで必要なライブラリのnamespaceを指定
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using SyokumotsuRensa.CSV;
using SyokumotsuRensa.Music;

/// <summary>
/// プロジェクト名がnamespaceとなります
/// </summary>
namespace SyokumotsuRensa
{
    /// <summary>
    /// ゲームの基盤となるメインのクラス
    /// 親クラスはXNA.FrameworkのGameクラス
    /// </summary>
    public class Game1 : Game
    {
        // フィールド（このクラスの情報を記述）
        private GraphicsDeviceManager graphicsDeviceManager;//グラフィックスデバイスを管理するオブジェクト
        private SpriteBatch spriteBatch;//画像をスクリーン上に描画するためのオブジェクト

        private GameDevice gameDevice;

        Renderer renderer;

        private BGMLoader bgmLoader;

        List<Player> players;

        Count count;
        List<Glass> glasses;

        Wall wall;
        List<Wall> walls;

        //EnemyLevel1 Top;
        //EnemyLevel1 Bottom;
        //EnemyLevel1 Right;
        //EnemyLevel1 Left;
        List<Enemy> eL1List;

        List<Unchi> unchis;

        Camp camp;

        bool isEndFlag = false;
        bool isClearFlag = false;

        Wave wave;

        /// <summary>
        /// コンストラクタ
        /// （new で実体生成された際、一番最初に一回呼び出される）
        /// </summary>
        public Game1()
        {
            //グラフィックスデバイス管理者の実体生成
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            //コンテンツデータ（リソースデータ）のルートフォルダは"Contentに設定
            Content.RootDirectory = "Content";
            


            graphicsDeviceManager.PreferredBackBufferWidth = Screen.ScreenWidth;
            graphicsDeviceManager.PreferredBackBufferHeight = Screen.ScreenHeight;
        }

        /// <summary>
        /// 初期化処理（起動時、コンストラクタの後に1度だけ呼ばれる）
        /// </summary>
        protected override void Initialize()
        {
            // この下にロジックを記述
            gameDevice = GameDevice.Instance(Content, GraphicsDevice);

            bgmLoader = new BGMLoader(new string[,] { { "GamePlay1", "./Sound/" } });
            bgmLoader.Initialize();

            //CSVReader csvReader = new CSVReader();
            //csvReader.Read("spawn.csv");

            isEndFlag = false;

            unchis = new List<Unchi>();
            camp = new Camp();
            count = new Count();

            glasses = new List<Glass>();
            glasses.Add(new Glass());

            foreach (var g in glasses)
            {
                g.Initialize();
            }


            wall = new Wall(new Vector2(700, 200), new Rectangle(0, 0, 10 * 50, 1 * 50));
            walls = new List<Wall>();
            walls.Add(wall);
            walls.Add(new Wall(new Vector2(500, 400), new Rectangle(0, 0, 1 * 50, 5 * 50)));
            walls.Add(new Wall(new Vector2(700, 800), new Rectangle(0, 0, 8 * 50, 1 * 50)));
            walls.Add(new Wall(new Vector2(1250, 400), new Rectangle(0, 0, 1 * 50, 5 * 50)));

            foreach (var wa in walls)
            {
                wa.Initialize();
            }

            players = new List<Player>();
            //players.Add(new Player(glasses, walls));

            foreach (var pl in players)
            {
                pl.Initialize();
            }
            
            eL1List = new List<Enemy>();

            //EnemyCSVParser parser = new EnemyCSVParser(camp, players, walls, unchis, glasses);
            //var dataList = parser.Parse("spawn.csv", "./");
            //foreach (var data in dataList)
            //{
            //    eL1List.Add(data);
            //}


            //foreach (var el1 in eL1List)
            //{
            //    el1.Initialize();
            //}

            wave = new Wave(camp,players, walls, unchis, glasses, isClearFlag, isEndFlag);
            wave.Initialize();


            // この上にロジックを記述
            base.Initialize();// 親クラスの初期化処理呼び出し。絶対に消すな！！
        }

        /// <summary>
        /// コンテンツデータ（リソースデータ）の読み込み処理
        /// （起動時、１度だけ呼ばれる）
        /// </summary>
        protected override void LoadContent()
        {
            // 画像を描画するために、スプライトバッチオブジェクトの実体生成
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // この下にロジックを記述
            renderer = new Renderer(Content, GraphicsDevice);
            renderer.LoadContent("BlueTile");
            renderer.LoadContent("RedTile");
            renderer.LoadContent("1000");
            renderer.LoadContent("chicken");
            renderer.LoadContent("glass");
            renderer.LoadContent("house");
            renderer.LoadContent("number");
            renderer.LoadContent("pig");
            renderer.LoadContent("tile");
            renderer.LoadContent("unchi");
            renderer.LoadContent("wolf");
            renderer.LoadContent("GameOver");
            renderer.LoadContent("horizontalFence");
            renderer.LoadContent("verticalFence");
            renderer.LoadContent("GameClear");
            renderer.LoadContent("cow");
            renderer.LoadContent("eagle");
            renderer.LoadContent("exclamation");
            renderer.LoadContent("hand");
            renderer.LoadContent("hand2");
            renderer.LoadContent("heart");
            renderer.LoadContent("meat");
            renderer.LoadContent("rion");
            renderer.LoadContent("UI");


            // この上にロジックを記述
        }

        /// <summary>
        /// コンテンツの解放処理
        /// （コンテンツ管理者以外で読み込んだコンテンツデータを解放）
        /// </summary>
        protected override void UnloadContent()
        {
            // この下にロジックを記述


            // この上にロジックを記述
        }

        /// <summary>
        /// 更新処理
        /// （1/60秒の１フレーム分の更新内容を記述。音再生はここで行う）
        /// </summary>
        /// <param name="gameTime">現在のゲーム時間を提供するオブジェクト</param>
        protected override void Update(GameTime gameTime)
        {
            // ゲーム終了処理（ゲームパッドのBackボタンかキーボードのエスケープボタンが押されたら終了）
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) ||
                 (Keyboard.GetState().IsKeyDown(Keys.Escape)))
            {
                Exit();
            }

            // この下に更新ロジックを記述

            Input.Update();
            bgmLoader.Update();
            gameDevice.GetSound().PlayBGM("GamePlay1");

            
            foreach (var gl in glasses)
            {
                if (!gl.setGlassFlag)
                {
                    gl.Update();
                }
            }

            if (glasses[glasses.Count - 1].setGlassFlag && Glass.glassStock > 0)
            {
                glasses.Add(new Glass());
                glasses[glasses.Count - 1].Initialize();
            }

            for (int g = glasses.Count - 1; g > 0; g--)
            {
                if (glasses[g].isDeadFlag)
                {
                    glasses.RemoveAt(g);
                }

            }

            foreach (var pl in players)
            {
                if (!pl.moveFlag)
                {
                    pl.Update();
                }
            }

            if (players.Count == 0)
            {
                players.Add(new Player(glasses, walls));
                players[players.Count - 1].Initialize();
            }

            if ((players[players.Count - 1].moveFlag && Player.playerStock > 0) /*|| players.Count == 0*/)
            {
                players.Add(new Player(glasses, walls));
                players[players.Count - 1].Initialize();
            }
            

            for (int p = players.Count - 1; p >= 0; p--)
            {
                if (players[p].isDeadFlag)
                {
                    players.RemoveAt(p);
                }
            }

            

            foreach (var wa in walls)
            {
                wa.Update();
            }

            foreach (var un in unchis)
            {
                un.Update();
            }

            wave.Update();

            // この上にロジックを記述
            base.Update(gameTime); // 親クラスの更新処理呼び出し。絶対に消すな！！
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="gameTime">現在のゲーム時間を提供するオブジェクト</param>
        protected override void Draw(GameTime gameTime)
        {
            // 画面クリア時の色を設定
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // この下に描画ロジックを記述
            renderer.Begin();

            //仮マップ
            for (int i = 0; i < Screen.ScreenWidth / 50 + 50; i++)
            {
                for (int j = 0; j < Screen.ScreenHeight / 50 + 50; j++)
                {
                    renderer.DrawTexture("tile", new Vector2(i * 50, j * 50));
                }
            }


            foreach (var un in unchis)
            {
                un.Draw(renderer);
            }

            glasses.ForEach(g => g.Draw(renderer));

            //仮壁
            foreach (var wa in walls)
            {
                wa.Draw(renderer);
            }




            //仮UI位置
            renderer.DrawTexture("UI", Vector2.Zero);

            //仮選択位置
            if (Input.IsMouseLButton())
            {
                renderer.DrawTexture("RedTile", new Vector2((int)(Input.MousePosition.X / 50) * 50, (int)(Input.MousePosition.Y / 50) * 50));
            }

            count.Draw(renderer);


            players.ForEach(p => p.Draw(renderer));


            wave.Draw(renderer);


            camp.Draw(renderer);


            

            renderer.DrawTexture("hand", new Vector2(Input.MousePosition.X - 25, Input.MousePosition.Y - 25));


            

            renderer.End();

            //この上にロジックを記述
            base.Draw(gameTime); // 親クラスの更新処理呼び出し。絶対に消すな！！
        }
    }
}
