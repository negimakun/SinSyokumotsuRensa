using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyokumotsuRensa
{
    class Player
    {
        Vector2 secondPos;
        public Vector2 movePos;//動いている間の位置
        public Vector2 playerPos;

        Vector2 stockPos;//UIボタン
       public Vector2 spowPos;//出現位置

        Vector2 limit;
        public Vector2 plMasu;
        public static int playerStock = 15;
        bool clickFlag = false;
        bool glassEatFlag = false;
        public bool isDeadFlag = false;
        public bool moveFlag = false;
        bool moveStart = false;
        public List<Glass> glasses;
        int targetGlassNom;
        public Direction direction;
        public List<Wall> walls;
        public int colWallNum;
        public bool avoidFlag = true;
        //int nowCnt;
        float playerMoveTime;
        float playerMoveTimeSet;
        float collisionCoolTime = 0;
        int wallNowCnt;


        float time;

        public int stuff;//肉食が食べた時にたまる満腹度

        public readonly int TextureSize = 50;

        public Player(List<Glass> glasses, List<Wall> walls)
        {
            //this.direction = direction;
            this.walls = walls;
            stuff = 1;
            this.glasses = glasses;
        }

        public void Initialize()
        {

            stockPos = new Vector2(50,50);

            spowPos = new Vector2(850, 450);
            playerPos = spowPos;

            playerMoveTimeSet = 1 * walls.Count;
        }
        public void Update()
        {
            if (avoidFlag)
            {
                wallNowCnt = 0;
            }
            if (collisionCoolTime <= 0)
            {
                collisionCoolTime--;
            }

            if (playerStock > 0)
            {
                if (Input.IsMouseLButtonDown())
                {
                    if (stockPos == new Vector2((int)(Input.MousePosition.X / TextureSize)
                                 * TextureSize, (int)(Input.MousePosition.Y / TextureSize) * TextureSize) || clickFlag)
                    {
                        if (!clickFlag)
                        {
                            spowPos = new Vector2((int)(spowPos.X / TextureSize)/*何マス目か*/ * TextureSize,
                                (int)(spowPos.Y / TextureSize) * TextureSize);
                            movePos = spowPos;
                            clickFlag = true;
                        }
                        else if(clickFlag && Input.MousePosition.X > 300 )
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
                EatGlass();
            }

            //ここが怪しい
            foreach (var wall in walls)
            {
                if (!Collision.WallXPlayer(wall, this) && avoidFlag)
                {
                    wallNowCnt++;
                }

                else
                {
                    // wallNowCnt = colWallNum;
                    colWallNum = wallNowCnt;
                    PlayerWallAvoid();

                }
            }
            plMasu = new Vector2((int)playerPos.X / TextureSize, (int)playerPos.Y / TextureSize);
        }

        public void Draw(Renderer renderer)
        {

            renderer.DrawTexture("pig", stockPos);
            if (!isDeadFlag)
            {
                renderer.DrawTexture("pig", playerPos);
            }
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
                        playerStock += 2;
                        gls.isDeadFlag = true;
                      
                    }

                    nowCnt++;
                }
            }

        }
        public void PlayerWallAvoid()
        {
        //    if (collisionCoolTime <= 0)
        //    {
        //        float Down;
        //        float Right;
        //        if (avoidFlag)
        //        {
        //            direction = Collision.WallXPlayerDirection(walls[colWallNum], this);
        //            avoidFlag = false;
        //        }
        //        Vector2 moveWall;
        //        Vector2 center = new Vector2(playerPos.X + TextureSize / 2, playerPos.Y + TextureSize / 2);
        //        center.X = center.X - walls[colWallNum].position.X;
        //        center.Y = center.Y - walls[colWallNum].position.Y;

        //        playerMoveTime = 50 / playerMoveTimeSet / 60.0f;

        //        switch (direction)
        //        {
        //            case Direction.RIGHT:
        //                Down = walls[colWallNum].rectangle.Height - center.Y;
        //                if (Down/*下側の距離*/ < center.Y && Down > 0)//下の方が距離が短いとき
        //                {
        //                    movePos += new Vector2(0, playerMoveTime);
        //                }
        //                else if (Down >= center.Y && 0 < center.Y)
        //                {
        //                    movePos += new Vector2(0, -playerMoveTime);
        //                }

        //                moveWall = new Vector2(walls[colWallNum].rectangle.Width, 0);
        //                if (Down < 0 || center.Y < 0)
        //                {
        //                    movePos += new Vector2(playerMoveTime, 0);
        //                }

        //                if (movePos.X - walls[colWallNum].position.X > moveWall.X)
        //                {
        //                    avoidFlag = true;
        //                    collisionCoolTime = 10;
        //                }
        //                playerPos = new Vector2((int)(movePos.X / TextureSize) * TextureSize,
        //               (int)(movePos.Y / TextureSize) * TextureSize);
        //                break;

        //            case Direction.LEFT:
        //                Down = walls[colWallNum].rectangle.Height - center.Y;
        //                if (Down/*下側の距離*/ < center.Y && Down > 0)
        //                {
        //                    movePos += new Vector2(0, playerMoveTime);
        //                }
        //                else if (Down >= center.Y && 0 < center.Y)
        //                {
        //                    movePos += new Vector2(0, -playerMoveTime);
        //                }

        //                moveWall = new Vector2(-walls[colWallNum].rectangle.Width, 0);
        //                if (Down < 0 || center.Y < 0)
        //                {
        //                    movePos += new Vector2(-playerMoveTime, 0);
        //                }

        //                if (playerPos.X - walls[colWallNum].position.X < moveWall.X)
        //                {
        //                    avoidFlag = true;
        //                    collisionCoolTime = 10;
        //                }
        //                playerPos = new Vector2((int)(movePos.X / TextureSize) * TextureSize,
        //               (int)(movePos.Y / TextureSize) * TextureSize);
        //                break;

        //            case Direction.TOP:
        //                Right = walls[colWallNum].rectangle.Width - center.X;
        //                if (Right/*右側の距離*/ < center.X/*左側の距離*/ && Right > 0)
        //                {
                         
        //                        movePos += new Vector2(playerMoveTime, 0);
                          
        //                }
        //                else if (Right >= center.X && 0 < center.X)
        //                {
        //                    movePos += new Vector2(-playerMoveTime, 0);
        //                }

        //                moveWall = new Vector2(0, -walls[colWallNum].rectangle.Height);

        //                if (Right < 0 || center.X < 0)
        //                {
        //                    movePos += new Vector2(0, -playerMoveTime);
        //                }

        //                if (playerPos.Y - walls[colWallNum].position.Y < moveWall.Y)
        //                {
        //                    avoidFlag = true;
        //                    collisionCoolTime = 10;
        //                }
        //                playerPos = new Vector2((int)(movePos.X / TextureSize) * TextureSize,
        //               (int)(movePos.Y / TextureSize) * TextureSize);
        //                break;

        //            case Direction.BOTTOM:

        //                Right = walls[colWallNum].rectangle.Width - center.X;
        //                if (Right/*右側の距離*/ < center.X/*左側の距離*/ && Right > 0)
        //                {
        //                    movePos += new Vector2(playerMoveTime, 0);
        //                }
        //                else if (Right >= center.X && 0 < center.X)
        //                {
        //                    movePos += new Vector2(-playerMoveTime, 0);
        //                }

        //                moveWall = new Vector2(0, walls[colWallNum].rectangle.Height);

        //                if (Right < 0 || center.X < 0)
        //                {
        //                    movePos += new Vector2(0, playerMoveTime);
        //                }

        //                if (playerPos.Y - walls[colWallNum].position.Y > moveWall.Y)
        //                {
        //                    avoidFlag = true;
        //                    collisionCoolTime = 10;
        //                }
        //                playerPos = new Vector2((int)(movePos.X / TextureSize) * TextureSize,
        //               (int)(movePos.Y / TextureSize) * TextureSize);
        //                break;

        //            case Direction.NULL:
        //                break;
        //            default:
        //                break;
        //        }
           }

        }
    }


