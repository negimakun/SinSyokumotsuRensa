using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SyokumotsuRensa.CSV;
using SyokumotsuRensa.Music;
using Microsoft.Xna.Framework.Input;


namespace SyokumotsuRensa.Scene
{
    class GamePlay : IScene
    {
        Count count;
        List<PlayerMather> players;
        List<Glass> glasses;
        Wall wall;
        List<Wall> walls;
        List<Unchi> unchis;
        Camp camp;
        bool isEndFlag;
        bool isClearFlag;
        public bool returnTitleFlag = false;
        int playerSyoki = 15;
        int player2Syoki = 10;
        int player3Syoki = 5;
        int glassSyoki = 10;
        Sound sound;
        private BGMLoader bgmLoader;
        bool handFlag;


        Button titleButton;
        Button nextDay;

        Vector2 goTitlePos;
        Vector2 nextDayPos;

        Wave wave;

        public GamePlay()
        {
            var gameDevise = GameDevice.Instance();
            sound = gameDevise.GetSound();
        }



        public void Initialize()
        {
            Player.playerStock = playerSyoki;
            Player2.player2Stock = player2Syoki;
            Player3.player3Stock = player3Syoki;
            Glass.glassStock = glassSyoki;
            bgmLoader = new BGMLoader(new string[,] { { "GamePlay1", "./Sound/" } });
            bgmLoader.Initialize();


            unchis = new List<Unchi>();
            camp = new Camp();
            count = new Count();
            glasses = new List<Glass>();
            glasses.Add(new Glass());
            returnTitleFlag = false;
            isClearFlag = false;
            isEndFlag = false;
            handFlag = false;

            foreach (var g in glasses)
            {
                g.Initialize();
            }


            wall = new Wall(new Vector2(650, 200), new Rectangle(0, 0, 9 * 50, 1 * 50));
            walls = new List<Wall>();
            walls.Add(wall);
            walls.Add(new Wall(new Vector2(500, 400), new Rectangle(0, 0, 1 * 50, 5 * 50)));
            walls.Add(new Wall(new Vector2(650, 800), new Rectangle(0, 0, 9 * 50, 1 * 50)));
            walls.Add(new Wall(new Vector2(1200, 400), new Rectangle(0, 0, 1 * 50, 5 * 50)));

            foreach (var wa in walls)
            {
                wa.Initialize();
            }

            players = new List<PlayerMather>();


            foreach (var pl in players)
            {
                pl.Initialize();
            }



            wave = new Wave(camp, players, walls, unchis, glasses, isClearFlag, isEndFlag);
            wave.Initialize();

            nextDayPos = new Vector2((Screen.ScreenWidth / 2) - 150, (Screen.ScreenHeight / 2) + 80);
            goTitlePos = new Vector2((Screen.ScreenWidth / 2) - 150, (Screen.ScreenHeight / 2) + 80);


            titleButton = new Button(ButtonType.resultUI_title, goTitlePos, 400, 100, this);
            nextDay = new Button(ButtonType.nextday, nextDayPos, 400, 100, wave);


        }

        public bool IsEnd()
        {
            return returnTitleFlag;
        }

        public SceneName Next()
        {
            return SceneName.Title;
        }

        public void Shutdown()
        {

        }

        public void Update(GameTime gameTime)
        {
            if (wave.isClearFlag && !wave.IsFinalWave())
            {
                nextDay.Update();
            }
            if (wave.IsFinalWave() && wave.isClearFlag || wave.isEndFlag)
            {
                titleButton.Update();
            }

            wave.Update();
            if (Player.playerStock <= 0 && Player2.player2Stock <= 0 && Player3.player3Stock <= 0)
            {
                isEndFlag = true;
                if (Input.GetKeyTrigger(Keys.Space))
                {
                    returnTitleFlag = true;
                }
            }
            if (!isClearFlag || !isEndFlag)
            {


                bgmLoader.Update();
                sound.PlayBGM("GamePlay1");



                foreach (var gl in glasses)
                {
                    if (!gl.setGlassFlag)
                    {
                        gl.Update();
                    }
                }

                if (Input.getMasu() == StocPos.stockGlassUI && Input.IsMouseLButtonDown() && !handFlag)
                {
                    glasses.Add(new Glass());
                    glasses[glasses.Count - 1].Initialize();
                    handFlag = true;
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
                    if (!pl.moveFlag || pl.isDeadFlag)
                    {
                        pl.Update();
                    }
                }

                if (Input.getMasu() == StocPos.stocPosUI && Input.IsMouseLButtonDown() && !handFlag && Player.playerStock > 0)
                {

                    players.Add(new Player(glasses, walls));
                    players[players.Count - 1].Initialize();
                    handFlag = true;

                }
                if (Input.getMasu() == StocPos.stocPos2UI && Input.IsMouseLButtonDown() && !handFlag && Player2.player2Stock > 0)
                {
                    players.Add(new Player2(glasses, walls));
                    players[players.Count - 1].Initialize();
                    handFlag = true;
                }
                if (Input.getMasu() == StocPos.stocPos3UI && Input.IsMouseLButtonDown() && !handFlag && Player3.player3Stock > 0)
                {
                    players.Add(new Player3(glasses, walls));
                    players[players.Count - 1].Initialize();
                    handFlag = true;
                }




                for (int p = players.Count - 1; p >= 0; p--)
                {
                    if (players[p].syoutenTime < 0)
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


                for (int i = unchis.Count - 1; i >= 0; i--)
                {
                    if (unchis[i].iswwwFlag)
                    {
                        unchis.RemoveAt(i);
                    }
                }
                return;
            }
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin();
            //仮マップ
            for (int i = 0; i < Screen.ScreenWidth / 50 + 50; i++)
            {
                for (int j = 0; j < Screen.ScreenHeight / 50 + 50; j++)
                {
                    renderer.DrawTexture("tile", new Vector2(i * 50, j * 50));
                }
            }



            renderer.DrawTexture("UI", Vector2.Zero, new Rectangle(0, 0, 300, Screen.ScreenHeight));


            count.Draw(renderer);



            unchis.ForEach(u => u.Draw(renderer));
            walls.ForEach(w => w.Draw(renderer));
            glasses.ForEach(g => g.Draw(renderer));
            players.ForEach(p => p.Draw(renderer));
            renderer.DrawTexture("chicken", StocPos.stocPosUI);
            renderer.DrawTexture("pig", StocPos.stocPos2UI);
            renderer.DrawTexture("cow", StocPos.stocPos3UI);
            renderer.DrawTexture("glass", StocPos.stockGlassUI);

            
            camp.Draw(renderer);

            wave.Draw(renderer);

            if (wave.isClearFlag && !wave.IsFinalWave())
            {
                renderer.DrawTexture("nextday", nextDayPos);
                renderer.DrawTexture("hand", new Vector2((int)(Input.MousePosition.X - 25), (int)(Input.MousePosition.Y - 25)));
                handFlag = false;
                renderer.End();
                return;
            }


            if (wave.IsFinalWave() && wave.isClearFlag || wave.isEndFlag)
            {
                renderer.DrawTexture("resultUI_title", goTitlePos);
                renderer.DrawTexture("hand", new Vector2((int)(Input.MousePosition.X - 25), (int)(Input.MousePosition.Y - 25)));
                handFlag = false;
                renderer.End();
                return;
            }

            if (!handFlag)
            {
                renderer.DrawTexture("hand", new Vector2((int)(Input.MousePosition.X - 25), (int)(Input.MousePosition.Y - 25)));
            }
            else
            {

                renderer.DrawTexture("hand2", new Vector2((int)(Input.MousePosition.X - 25), (int)(Input.MousePosition.Y - 25)));
                if (Input.IsMouseLButtonDown() && Input.MousePosition.X > 300)
                {
                    handFlag = false;
                }

            }


            renderer.End();
        }
    }
}
