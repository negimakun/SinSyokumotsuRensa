using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SyokumotsuRensa
{
    class Player2 : PlayerMather
    {

        Vector2 limit;
        Vector2 stockPos2;//UIボタン
        public static int player2Stock = 15;
        public static int stock2Cnt = 15;

        float time;
        public Player2(List<Glass> glasses, List<Wall> walls) : base(glasses, walls)
        {
            this.walls = walls;
            stuff = 1;
            this.glasses = glasses;
          //stockPos2 = StocPos.stocPos2UI;
        }



        public override void Initialize()
        {

            stockPos2 = StocPos.stocPos2UI;
            spowPos = new Vector2(850, 450);
            playerPos = spowPos;
            clickFlag = true;
            movePos = spowPos;

            playerMoveTimeSet = 1 * walls.Count;
        }
        public override void Draw(Renderer renderer)
        {
           
            if (!isDeadFlag)
            {
                renderer.DrawTexture("pig", playerPos);
            }
            else
            {
                renderer.DrawTexture("pig", playerPos, 0.7f);
                renderer.DrawTexture("ring", playerPos + new Vector2(0, -5));
            }
            if (clickFlag)
            {
                renderer.DrawTexture("RedTile", new Vector2((int)(Input.MousePosition.X / 50) * 50, (int)(Input.MousePosition.Y / 50) * 50), 0.5f);

                renderer.DrawTexture("pig", new Vector2((int)(Input.MousePosition.X / 50) * 50, (int)(Input.MousePosition.Y / 50) * 50), 0.8f);

            }
        }

        public override void Update()
        {
            if (isDeadFlag)
            {
                syoutenTime -= 1;

                playerPos += new Vector2(0, -2f);


                if (syoutenTime < 0)
                {
                    player2Stock--;
                }
                return;
            }
            if (stock2Cnt > 0)
            {


                if (avoidFlag)
                {
                    wallNowCnt = 0;
                }
                if (collisionCoolTime <= 0)
                {
                    collisionCoolTime--;
                }


                if (Input.IsMouseLButtonDown())
                {
                    if (stockPos2 == new Vector2((int)(Input.MousePosition.X / TextureSize)
                                 * TextureSize, (int)(Input.MousePosition.Y / TextureSize) * TextureSize) || clickFlag)
                    {
                        if (!clickFlag)
                        {
                            spowPos = new Vector2((int)(spowPos.X / TextureSize)/*何マス目か*/ * TextureSize,
                                (int)(spowPos.Y / TextureSize) * TextureSize);
                            movePos = spowPos;
                            clickFlag = true;
                        }
                        else if (clickFlag && Input.MousePosition.X > 300)
                        {
                            secondPos = new Vector2((int)(Input.MousePosition.X / TextureSize)/*何マス目か*/ * TextureSize,
                                (int)(Input.MousePosition.Y / TextureSize) * TextureSize);

                            limit = new Vector2((int)(secondPos.X - spowPos.X) / TextureSize,
                                (int)(secondPos.Y - spowPos.Y) / TextureSize);
                            if (limit.X < 0) limit.X++;
                            if (limit.Y < 0) limit.Y++;

                            //時間 ＝ フレーム　一マス辺りの時間　移動マス
                            if (Math.Abs(limit.X) < Math.Abs(limit.Y)) time = 60 / 10 * Math.Abs(limit.Y);
                            else time = 60 / 10 * Math.Abs(limit.X);

                            clickFlag = false;

                            moveStart = true;

                        }
                    }
                }
            }

            if (playerPos != secondPos && moveStart)
            {
                if (Math.Abs(limit.X) >= Math.Abs((movePos.X / TextureSize) - (spowPos.X / TextureSize)))
                {
                    movePos += new Vector2((secondPos.X - spowPos.X)/*何マス離れてるか*/ / (time/*60f×秒数*/), 0);
                }
                if (Math.Abs(limit.Y) >= Math.Abs((movePos.Y / TextureSize) - (spowPos.Y / TextureSize)))
                {
                    movePos += new Vector2(0, (secondPos.Y - spowPos.Y) / (time/*×秒数*/));
                }

                playerPos = new Vector2((int)(movePos.X / TextureSize) * TextureSize,
                    (int)(movePos.Y / TextureSize) * TextureSize);
            }
            else if (playerPos == secondPos)
            {
                moveFlag = true;
                stock2Cnt--;
                EatGlass();
            }



            plMasu = new Vector2((int)playerPos.X / TextureSize, (int)playerPos.Y / TextureSize);
        }

        public void EatGlass()
        {
            int nowCnt = 0;
            if (glasses != null)
            {
                foreach (var gls in glasses)
                {

                    if (Vector2.Distance(plMasu, gls.glassMasu) <= 0 && !gls.isDeadFlag)
                    {
                        glassEatFlag = true;
                        targetGlassNom = nowCnt;
                        player2Stock += 2;
                        stock2Cnt += 2;
                        gls.isDeadFlag = true;

                    }

                    nowCnt++;
                }
            }

        }
    }
}

