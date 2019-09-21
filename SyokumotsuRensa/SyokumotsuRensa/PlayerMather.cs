using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace SyokumotsuRensa
{
    abstract class PlayerMather
    {
       public Vector2 secondPos;
        public Vector2 movePos;//動いている間の位置
        public Vector2 playerPos;

        public Vector2 spowPos;//出現位置

      //  Vector2 limit;
        public Vector2 plMasu;

       public bool clickFlag = false;
       public bool glassEatFlag = false;
        public bool isDeadFlag = false;
        public bool moveFlag = false;
        public  bool moveStart = false;
        public List<Glass> glasses;
         public int targetGlassNom;
         public Direction direction;
        public List<Wall> walls;
        public int colWallNum;
        public bool avoidFlag = true;
       public int nowCnt;
      public  float playerMoveTime;
        public float syoutenTime = 0.7f * 60;
      public  float playerMoveTimeSet;
       public float collisionCoolTime = 0;
      public  int wallNowCnt;
        public Vector2 syoutenPos;

        float time;

        public int stuff;//肉食が食べた時にたまる満腹度

        public readonly int TextureSize = 50;
        public PlayerMather(List<Glass> glasses, List<Wall> walls)
        {
            this.walls = walls;

           this.glasses = glasses;
        }
        public abstract void Initialize();
        public abstract void Update();
        public abstract void Draw(Renderer renderer);
     
    }
}
