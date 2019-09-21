using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SyokumotsuRensa.Music;

namespace SyokumotsuRensa.Scene
{
    class Load : IScene
    {
         bool isEndFlag;
       // IScene backGround;
        private Sound sound;
        int loadCnt;
        Vector2 loadPos;

        public Load()
        {
           
            //backGround = scene;
            var gameDevise = GameDevice.Instance();
            sound = gameDevise.GetSound();
            loadCnt = 100;
        }
        public void Draw(Renderer renderer)
        {
            renderer.Begin();
            for (int i = 0; i < Screen.ScreenWidth / 50 + 50; i++)
            {
                for (int j = 0; j < Screen.ScreenHeight / 50 + 50; j++)
                {
                    renderer.DrawTexture("tile", new Vector2(i * 50, j * 50));
                }
            }
            renderer.DrawTexture("title_image", Vector2.Zero);

            for (int j = 0; j < Screen.ScreenHeight / 50 + 50; j++)
            {
                renderer.DrawTexture("cow", new Vector2(j * 50) + loadPos);
            }

            renderer.End();
            
        }

        public void Initialize()
        {
          
            loadPos = new Vector2(1000, 0);
            loadCnt = 100;
            isEndFlag = false;
        }

        public bool IsEnd()
        {
            return isEndFlag;
        }

        public SceneName Next()
        {
            return SceneName.GamePlay;
        }

        public void Shutdown()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            loadPos += new Vector2(-17, 0);
            loadCnt--;
            if (loadCnt < 0)
            {
                isEndFlag = true;
            }


        }
    }
}
