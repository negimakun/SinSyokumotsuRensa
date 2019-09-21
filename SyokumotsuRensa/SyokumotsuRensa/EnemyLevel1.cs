using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyokumotsuRensa
{
    class EnemyLevel1:Enemy
    {
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction">向かう方向</param>
        /// <param name="camp">キャンプ、拠点</param>
        /// <param name="players">草食獣のリスト</param>
        /// <param name="walls">壁のリスト</param>
        /// <param name="spawnTimeSet">どのタイミングで出てくるかの設定</param>
        /// <param name="unchis">うんこのリスト</param>
        /// <param name="glasses">草のリスト</param>
        public EnemyLevel1(Direction direction, Camp camp, List<PlayerMather> players, List<Wall> walls,
            float spawnTimeSet, List<Unchi> unchis, List<Glass> glasses)
        {
            this.direction = direction;
            baseCamp = camp;
            this.players = players;
            this.walls = walls;
            this.unchis = unchis;
            spawnTime = spawnTimeSet;
            this.glasses = glasses;
        }


        public override void Initialize()
        {
            stuff = 1;
            neerGlassEaterFlag = false;
            glassEatTargetFlag = false;
            switch (direction)
            {
                case Direction.LEFT:
                    enemySpawnPos = new Vector2((int)(Screen.ScreenWidth / TextureSize) * TextureSize, (int)((Screen.ScreenHeight / 2) / TextureSize) * TextureSize);
                    break;
                case Direction.RIGHT:
                    enemySpawnPos = new Vector2((int)(UIWidth / TextureSize) * TextureSize - TextureSize, (int)((Screen.ScreenHeight / 2) / TextureSize) * TextureSize);
                    break;
                case Direction.BOTTOM:
                    enemySpawnPos = new Vector2((int)(((Screen.ScreenWidth - UIWidth) / 2 + UIWidth) / TextureSize) * TextureSize, -TextureSize);
                    break;
                case Direction.TOP:
                    enemySpawnPos = new Vector2((int)(((Screen.ScreenWidth - UIWidth) / 2 + UIWidth) / TextureSize) * TextureSize, Screen.ScreenHeight);
                    break;
                default:
                    break;
            }
            enemyMovePos = enemySpawnPos;
            enemyPos = enemySpawnPos;

            moveTimeSet = 1 * walls.Count;
        }

        public override void Update()
        {

            if (spawnTime > 0)//スポーンしないとき
            {
                spawnTime -= 1.0f / 60.0f;
                return;
            }

            if (avoidFlag)
            {
                nowCnt = 0;
            }

            if (collisionCoolTime <= 0)
            {
                collisionCoolTime--;
            }

            if (stuff <= 0 && eatTime > 0)//満腹で食べきってないとき
            {
                eatTime -= 1;
            }
            else if (eatTime == 0)
            {
                unchis.Add(new Unchi(enemyPos, glasses));
                eatTime--;
            }
            else if (stuff <= 0 && eatTime <= 0)
            {
                MoveToSpawn();
            }

            NeerGlassEater();//近くに草食動物がいるかどうか


            foreach (var wall in walls)//壁
            {
                if (eatTime <= 0 || stuff <= 0)
                {
                    break;
                }

                if (!Collision.WallXEnemy(wall, this) && avoidFlag /*&&*/)
                {
                    if (stuff > 0 && !moveEndFlag)//壁に当たってないとき//満腹でなくて移動が終わってないとき
                    {
                        if (!neerGlassEaterFlag)//いないとき
                        {
                            MoveToCamp();//真ん中に向かう
                        }
                        else
                        {
                            MoveToGE();
                        }
                    }

                    nowCnt++;
                }
                else if (neerGlassEaterFlag)
                {
                    MoveToGE();
                }
                else//当たっているとき
                {
                    colWallNum = nowCnt;
                    WallAvoid();
                }

            }

            enemyCenterPosition = new Vector2(enemyPos.X + (TextureSize / 2), enemyPos.Y + (TextureSize / 2));

            if (Vector2.Distance(baseCamp.centerPosition, enemyCenterPosition) <= TextureSize)
            {
                moveEndFlag = true;
            }
            if (moveEndFlag)
            {
                //ここに草食獣のストックを減らす処理
                if(Player.playerStock>0)
                {
                    Player.playerStock -= 3;
                }
                else
                {
                    Player2.player2Stock -= 2;
                }
               
            }

            enemyMasu = new Vector2(enemyPos.X / TextureSize, enemyPos.Y / TextureSize);
        }

       

        public override void Draw(Renderer renderer)
        {
            if (enemyPos.X < 300)
            {
                return;
            }

            renderer.DrawTexture("wolf", enemyPos);
            if (neerGlassEaterFlag&&stuff>0)//発見
            {
                renderer.DrawTexture("exclamation", enemyPos); 
            }
            if (stuff <= 0 && eatTime > 0)//満腹で食べきってないとき
            {
                eatTime -= 1;
                renderer.DrawTexture("meat", new Vector2( enemyPos.X +10,enemyPos.Y));

            }
            if (stuff <= 0 && eatTime <= 0)
            {
                renderer.DrawTexture("heart",new Vector2( enemyPos.X+10,enemyPos.Y));
            }


        }



      
    }
}
