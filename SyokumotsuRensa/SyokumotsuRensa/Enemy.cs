using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyokumotsuRensa
{
    abstract class Enemy
    {
        public Camp baseCamp;

        protected float enemyMoveTime; //一マス当たりの移動時間の計算用小数
        protected float moveTimeSet;//移動する時間は何秒？

        protected Vector2 enemySpawnPos; //スポーン位置
        public Vector2 enemyMovePos; //移動量
        public Vector2 enemyHeadPos; //向かう場所
        protected Vector2 enemyLimit; //移動量の限界値
        public Vector2 enemyMasu; //マスの位置
        public Vector2 enemyPos; //ポジション
        public Vector2 enemyCenterPosition;
        public Direction direction;
        protected readonly int UIWidth = 250;

        public int eatTime = 3 * 60;

        public readonly int TextureSize = 50;
        public List<Wall> walls;
        public List<Player> players;
        public List<Unchi> unchis;
        public int targetPlayerNom;
        public int colWallNum;

        protected int stuff;//満腹いくつ？
        public bool stuffMAXFlag = false; //満足して帰ったらtrue

        public float spawnTime;

        protected bool neerGlassEaterFlag = false;
        protected bool glassEatTargetFlag = false;
        public bool moveEndFlag = false;

        public bool avoidFlag = true;
        protected int nowCnt;

        protected bool eatFlag = false; //今食べてるよ！！！ってフラグ

         protected float collisionCoolTime = 0;

        protected List<Glass> glasses;

        public abstract void Initialize();

        public abstract void Update();

        public abstract void Draw(Renderer renderer);

        public void NeerGlassEater()
        {
            int nowCount = 0;
            if (players != null)
            {
                foreach (var ge in players)
                {
                    if (((Vector2.Distance(enemyPos, ge.playerPos) <= 4 * TextureSize && !ge.isDeadFlag
                        && NeerGlassEaterAble(ge, direction) && !glassEatTargetFlag)
                        /*|| (glassEatTargetFlag && GEDistance(players[targetPlayerNom], ge))*/) && ge.playerPos != ge.spowPos)
                    {
                        neerGlassEaterFlag = true;
                        enemyHeadPos = ge.playerPos;
                        targetPlayerNom = nowCount;
                        glassEatTargetFlag = true;
                    }
                    else if (!glassEatTargetFlag || ge.isDeadFlag)
                    {
                        neerGlassEaterFlag = false;
                    }
                    nowCount++;
                }
            }
        }

        public bool GEDistance(Player now, Player judge)
        {
            Vector2 nowPlayerXEnemy = new Vector2(Math.Abs(now.playerPos.X - enemyPos.X), Math.Abs(now.playerPos.Y - enemyPos.Y));
            Vector2 judgePlayerXEnemy = new Vector2(Math.Abs(judge.playerPos.X - enemyPos.X), Math.Abs(judge.playerPos.Y - enemyPos.Y));

            if (nowPlayerXEnemy.X > judgePlayerXEnemy.X || nowPlayerXEnemy.Y > judgePlayerXEnemy.Y)
            {
                return true;
            }

            return false;
        }

        public bool NeerGlassEaterAble(Player player, Direction direct)
        {
            float GEDistance; //ターゲットの草食獣との距離
            switch (direct)
            {
                case Direction.RIGHT:

                    GEDistance = player.playerPos.X + (TextureSize / 2) - enemyPos.X + (TextureSize / 2);
                    if (GEDistance > 0)
                    {
                        return false;
                    }

                    break;

                case Direction.LEFT:

                    GEDistance = player.playerPos.X + (TextureSize / 2) - enemyPos.X + (TextureSize / 2);
                    if (GEDistance < 0)
                    {
                        return false;
                    }

                    break;

                case Direction.TOP:
                    //Y座標の距離
                    GEDistance = player.playerPos.Y + (TextureSize / 2) - enemyPos.Y + (TextureSize / 2);
                    if (GEDistance < 0)
                    {
                        return false;
                    }

                    break;

                case Direction.BOTTOM:
                    //Y座標の距離
                    GEDistance = (player.playerPos.Y + (TextureSize / 2)) - (enemyPos.Y + (TextureSize / 2));
                    if (GEDistance > 0)
                    {
                        return false;
                    }

                    break;
                case Direction.NULL:
                    break;
                default:
                    break;
            }
            return true;
        }

        public void WallAvoid()
        {
            if (collisionCoolTime <= 0)
            {
                float Down;
                float Right;
                //walls[colWallNum]; 当たってる壁
                if (avoidFlag)
                {
                    direction = Collision.WallXEnemyDirection(walls[colWallNum], this);
                    avoidFlag = false;
                }

                Vector2 moveWall;

                Vector2 center = new Vector2(enemyPos.X + TextureSize / 2, enemyPos.Y + TextureSize / 2);
                center.X = center.X - walls[colWallNum].position.X;
                center.Y = center.Y - walls[colWallNum].position.Y;

                enemyMoveTime = 50 / moveTimeSet / 60.0f;
                switch (direction)
                {
                    case Direction.RIGHT:
                        Down = walls[colWallNum].rectangle.Height - center.Y;
                        if (Down/*下側の距離*/ < center.Y && Down > 0)//下の方が距離が短いとき
                        {
                            enemyMovePos += new Vector2(0, enemyMoveTime);
                        }
                        else if (Down >= center.Y && 0 < center.Y)
                        {
                            enemyMovePos += new Vector2(0, -enemyMoveTime);
                        }

                        moveWall = new Vector2(walls[colWallNum].rectangle.Width, 0);
                        if (Down < 0 || center.Y < 0)
                        {
                            enemyMovePos += new Vector2(enemyMoveTime, 0);
                        }

                        if (enemyMovePos.X - walls[colWallNum].position.X > moveWall.X)
                        {
                            avoidFlag = true;
                            collisionCoolTime = 10;
                        }
                        enemyPos = new Vector2((int)(enemyMovePos.X / TextureSize) * TextureSize,
                       (int)(enemyMovePos.Y / TextureSize) * TextureSize);
                        break;

                    case Direction.LEFT:
                        Down = walls[colWallNum].rectangle.Height - center.Y;
                        if (Down/*下側の距離*/ < center.Y && Down > 0)
                        {
                            enemyMovePos += new Vector2(0, enemyMoveTime);
                        }
                        else if (Down >= center.Y && 0 < center.Y)
                        {
                            enemyMovePos += new Vector2(0, -enemyMoveTime);
                        }

                        moveWall = new Vector2(-walls[colWallNum].rectangle.Width, 0);
                        if (Down < 0 || center.Y < 0)
                        {
                            enemyMovePos += new Vector2(-enemyMoveTime, 0);
                        }

                        if (enemyPos.X - walls[colWallNum].position.X < moveWall.X)
                        {
                            avoidFlag = true;
                            collisionCoolTime = 10;
                        }
                        enemyPos = new Vector2((int)(enemyMovePos.X / TextureSize) * TextureSize,
                       (int)(enemyMovePos.Y / TextureSize) * TextureSize);
                        break;

                    case Direction.TOP:
                        Right = walls[colWallNum].rectangle.Width - center.X;
                        if (Right/*右側の距離*/ < center.X/*左側の距離*/ && Right > 0)
                        {
                            enemyMovePos += new Vector2(enemyMoveTime, 0);
                        }
                        else if (Right >= center.X && 0 < center.X)
                        {
                            enemyMovePos += new Vector2(-enemyMoveTime, 0);
                        }

                        moveWall = new Vector2(0, -walls[colWallNum].rectangle.Height);

                        if (Right < 0 || center.X < 0)
                        {
                            enemyMovePos += new Vector2(0, -enemyMoveTime);
                        }

                        if (enemyPos.Y - walls[colWallNum].position.Y < moveWall.Y)
                        {
                            avoidFlag = true;
                            collisionCoolTime = 10;
                        }
                        enemyPos = new Vector2((int)(enemyMovePos.X / TextureSize) * TextureSize,
                       (int)(enemyMovePos.Y / TextureSize) * TextureSize);
                        break;

                    case Direction.BOTTOM:

                        Right = walls[colWallNum].rectangle.Width - center.X;
                        if (Right/*右側の距離*/ < center.X/*左側の距離*/ && Right > 0)
                        {
                            enemyMovePos += new Vector2(enemyMoveTime, 0);
                        }
                        else if (Right >= center.X && 0 < center.X)
                        {
                            enemyMovePos += new Vector2(-enemyMoveTime, 0);
                        }

                        moveWall = new Vector2(0, walls[colWallNum].rectangle.Height);

                        if (Right < 0 || center.X < 0)
                        {
                            enemyMovePos += new Vector2(0, enemyMoveTime);
                        }

                        if (enemyPos.Y - walls[colWallNum].position.Y > moveWall.Y)
                        {
                            avoidFlag = true;
                            collisionCoolTime = 10;
                        }
                        enemyPos = new Vector2((int)(enemyMovePos.X / TextureSize) * TextureSize,
                       (int)(enemyMovePos.Y / TextureSize) * TextureSize);
                        break;

                    case Direction.NULL:
                        break;
                    default:
                        break;
                }
            }
        }




        public void MoveToCamp()
        {
            //拠点移動
            enemyHeadPos = new Vector2((int)((baseCamp.campPos.X + TextureSize) / TextureSize)/*何マス目か*/ * TextureSize,
                (int)((baseCamp.campPos.Y + TextureSize) / TextureSize) * TextureSize);
            enemyLimit = new Vector2((int)(enemyHeadPos.X - enemyPos.X) / TextureSize,
                (int)(enemyHeadPos.Y - enemyPos.Y) / TextureSize);

            //時間 ＝ フレーム　一マス辺りの時間　移動マス
            if (Math.Abs(enemyLimit.X) < Math.Abs(enemyLimit.Y)) enemyMoveTime = 60 * moveTimeSet * Math.Abs(enemyLimit.Y);
            else enemyMoveTime = 60 * moveTimeSet * Math.Abs(enemyLimit.X);

            if (enemyPos != enemyHeadPos)
            {
                if (Math.Abs(enemyLimit.X) > Math.Abs((enemyMovePos.X / TextureSize) - (enemyPos.X / TextureSize)))
                {
                    enemyMovePos += new Vector2((enemyHeadPos.X - enemyPos.X)/*何マス離れてるか*/ / (enemyMoveTime/*60f×秒数*/), 0);
                }
                if (Math.Abs(enemyLimit.Y) > Math.Abs((enemyMovePos.Y / TextureSize) - (enemyPos.Y / TextureSize)))
                {
                    enemyMovePos += new Vector2(0, (enemyHeadPos.Y - enemyPos.Y) / (enemyMoveTime/*×秒数*/));
                }

                enemyPos = new Vector2((int)(enemyMovePos.X / TextureSize) * TextureSize,
                    (int)(enemyMovePos.Y / TextureSize) * TextureSize);
            }
            else
            {
                moveEndFlag = true;
            }
        }

        public void MoveToGE()
        {
            enemyLimit = new Vector2((int)(enemyHeadPos.X - enemyPos.X) / TextureSize,
                   (int)(enemyHeadPos.Y - enemyPos.Y) / TextureSize);

            //時間 ＝ フレーム　一マス辺りの時間　移動マス
            if (Math.Abs(enemyLimit.X) < Math.Abs(enemyLimit.Y)) enemyMoveTime = 60 * 2 * Math.Abs(enemyLimit.Y);
            else enemyMoveTime = 60 * 2 * Math.Abs(enemyLimit.X);

            if (enemyPos != enemyHeadPos)
            {
                if (Math.Abs(enemyLimit.X) >= Math.Abs((enemyMovePos.X / TextureSize) - (enemyPos.X / TextureSize)))
                {
                    enemyMovePos += new Vector2((enemyHeadPos.X - enemyPos.X)/*何マス離れてるか*/ / (enemyMoveTime/*60f×秒数*/), 0);
                }
                if (Math.Abs(enemyLimit.Y) >= Math.Abs((enemyMovePos.Y / TextureSize) - (enemyPos.Y / TextureSize)))
                {
                    enemyMovePos += new Vector2(0, (enemyHeadPos.Y - enemyPos.Y) / (enemyMoveTime/*×秒数*/));
                }

                enemyPos = new Vector2((int)(enemyMovePos.X / TextureSize) * TextureSize,
                    (int)(enemyMovePos.Y / TextureSize) * TextureSize);
            }
            else
            {
                if (players != null && players.Count != 0 && !players[targetPlayerNom].isDeadFlag
                    && players[targetPlayerNom].playerPos == enemyHeadPos)
                {
                    players[targetPlayerNom].isDeadFlag = true;
                    stuff -= players[targetPlayerNom].stuff;
                    Player.playerStock--;
                    glassEatTargetFlag = false;
                    eatFlag = true;
                }
            }
        }

        public void MoveToSpawn()
        {
            enemyHeadPos = enemySpawnPos;
            enemyLimit = new Vector2((int)(enemyHeadPos.X - enemyPos.X) / TextureSize,
                  (int)(enemyHeadPos.Y - enemyPos.Y) / TextureSize);

            //時間 ＝ フレーム　一マス辺りの時間　移動マス
            if (Math.Abs(enemyLimit.X) < Math.Abs(enemyLimit.Y)) enemyMoveTime = 60 * Math.Abs(enemyLimit.Y);
            else enemyMoveTime = 60 * Math.Abs(enemyLimit.X);

            if (enemyPos != enemyHeadPos)
            {
                if (Math.Abs(enemyLimit.X) > Math.Abs((enemyMovePos.X / TextureSize) - (enemyPos.X / TextureSize)))
                {
                    enemyMovePos += new Vector2((enemyHeadPos.X - enemyPos.X)/*何マス離れてるか*/ / (enemyMoveTime/*60f×秒数*/), 0);
                }
                if (Math.Abs(enemyLimit.Y) > Math.Abs((enemyMovePos.Y / TextureSize) - (enemyPos.Y / TextureSize)))
                {
                    enemyMovePos += new Vector2(0, (enemyHeadPos.Y - enemyPos.Y) / (enemyMoveTime/*×秒数*/));
                }

                enemyPos = new Vector2((int)(enemyMovePos.X / TextureSize) * TextureSize,
                    (int)(enemyMovePos.Y / TextureSize) * TextureSize);
            }
            else
            {
                stuffMAXFlag = true;
            }
        }
    }
}
