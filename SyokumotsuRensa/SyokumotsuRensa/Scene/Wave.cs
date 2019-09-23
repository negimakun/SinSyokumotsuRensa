﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SyokumotsuRensa.CSV;

namespace SyokumotsuRensa
{
    class Wave
    {
        public static int currentWave;
        int nextWave;

        readonly int FinalWave = 3;

        Vector2 waveDays;
        Vector2 waveDaysMid;
        float alpha;

        public bool isEndFlag;
        public bool isClearFlag;
        private List<Enemy> eL1List;
        private Camp camp;
        private List<PlayerMather> players;
        private List<Wall> walls;
        private List<Unchi> unchis;
        private List<Glass> glasses;
        EnemyCSVParser parser;

        public Wave(Camp camp, List<PlayerMather> players, List<Wall> walls, List<Unchi> unchis, List<Glass> glasses, bool isClearFlag, bool isEndFlag)
        {
            currentWave = 1;

            this.camp = camp;
            this.players = players;
            this.walls = walls;
            this.unchis = unchis;
            this.glasses = glasses;
            this.isClearFlag = isClearFlag;
            this.isEndFlag = isEndFlag;
        }

        public void Initialize()
        {
            CSVReader csvReader = new CSVReader();
            csvReader.Read("wave1.csv");

            eL1List = new List<Enemy>();

            parser = new EnemyCSVParser(camp, players, walls, unchis, glasses);
            var dataList = parser.Parse("wave1.csv", "./");
            foreach (var data in dataList)
            {
                eL1List.Add(data);
            }


            foreach (var el1 in eL1List)
            {
                el1.Initialize();
            }

            waveDays = new Vector2(50, 800);
            waveDaysMid = new Vector2(475, 350);
            alpha = 1.0f;
        }

        public void Update()
        {
            nextWave = currentWave + 1;

            if (Input.GetKeyState(Keys.N) && Input.GetKeyState(Keys.E) && Input.GetKeyTrigger(Keys.T))
            {
                isClearFlag = true;
            }

            if (Player.playerStock <= 0 && Player2.player2Stock <= 0 && Player3.player3Stock <= 0)
            {
                isEndFlag = true;
            }

            if (eL1List.Count == 0 && !isEndFlag)
            {
                isClearFlag = true;
            }



            foreach (var el1 in eL1List)
            {
                el1.Update();
            }


            for (int i = eL1List.Count - 1; i >= 0; i--)
            {
                if (eL1List[i].moveEndFlag)
                {
                    eL1List.RemoveAt(i);
                }
                else if (eL1List[i].stuffMAXFlag)
                {
                    eL1List.RemoveAt(i);
                }
            }

            for (int i = unchis.Count - 1; i >= 0; i--)
            {
                if (unchis[i].iswwwFlag)
                {
                    unchis.RemoveAt(i);
                }
            }

            alpha -= 0.7f / 60;

        }

        public void Draw(Renderer renderer)
        {
            foreach (var el1 in eL1List)
            {
                el1.Draw(renderer);
            }

            switch (currentWave)
            {
                case 1:
                    renderer.DrawTexture("ichinichime", waveDays, null, 0.0f, Vector2.Zero, new Vector2(0.25f, 0.3f));
                    renderer.DrawTexture("ichinichime", waveDaysMid,alpha);
                    break;
                case 2:
                    renderer.DrawTexture("futsukame", waveDays, null, 0.0f, Vector2.Zero, new Vector2(0.25f, 0.3f));
                    renderer.DrawTexture("futsukame", waveDaysMid, alpha);
                    break;
                case 3:
                    renderer.DrawTexture("mikkame", waveDays, null, 0.0f, Vector2.Zero, new Vector2(0.25f, 0.3f));
                    renderer.DrawTexture("mikkame", waveDaysMid, alpha);
                    break;
                default:
                    break;
            }

            //ゲームオーバー
            if (isEndFlag)
            {
                renderer.DrawTexture("GameOver", new Vector2(350, 0));
            }
            if (isClearFlag)
            {
                renderer.DrawTexture("GameClear", new Vector2(350, 0));
            }
        }

        public int NowWave()
        {
            return currentWave;
        }

        public int NextWave()
        {
            return nextWave;
        }

        public bool IsFinalWave()
        {
            return currentWave == FinalWave;
        }


        public void GotoWave()
        {
            currentWave = nextWave;

            players.Clear();
            foreach (var pl in players)
            {
                pl.Initialize();
            }


            CSVReader csvReader = new CSVReader();
            csvReader.Read("wave" + currentWave.ToString() + ".csv");

            eL1List = new List<Enemy>();

            var dataList = parser.Parse("wave" + currentWave.ToString() + ".csv", "./");
            foreach (var data in dataList)
            {
                eL1List.Add(data);
            }


            foreach (var el1 in eL1List)
            {
                el1.Initialize();
            }

            alpha = 1.0f;
        }
    }
}
