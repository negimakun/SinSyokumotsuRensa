using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SyokumotsuRensa.Music;
using Microsoft.Xna.Framework.Input;


namespace SyokumotsuRensa.Scene
{
    class Title : IScene
    {
        public bool isEndFlag = false;
        private Sound sound;
        
        public bool playStyleFlag = false;//操作方法の画像を出すためのフラグ
        public int playStylePage = 1;//操作方法のページ数
        public int playStyleEndPage = 4;
        Vector2 tutorialPos;
        Vector2 startButtonPos;
        Vector2 playStylePos;
        Vector2 gameEndPos;
        Vector2 rightArrow;
        Vector2 leftArrow;

        List<Button> buttons;

        private int kimeunchiR;
        private bool kimeunchiUpR;
      
        private int kimeunchiG;
        private bool kimeunchiUpG;
    
        private int kimeunchiB;
        private bool kimeunchiUpB;
    
        public Title()
        {
            var gameDevice = GameDevice.Instance();
            sound = gameDevice.GetSound();
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

            renderer.DrawTexture("title_image", Vector2.Zero);
            renderer.DrawTexture("titleUI_hajimeru", startButtonPos);
            renderer.DrawTexture("titleUI_setsumei", playStylePos);
            renderer.DrawTexture("titleUI_owaru", gameEndPos);
            renderer.DrawTexture("big_whiteunchi", new Vector2(660, 600), new Color(kimeunchiR, kimeunchiG, kimeunchiB));
            renderer.DrawTexture("hand", new Vector2((int)(Input.MousePosition.X - 25), (int)(Input.MousePosition.Y - 25)));
            renderer.End();
        }

        public void Initialize()
        {
            isEndFlag = false;
            tutorialPos = new Vector2(50, 50);
            buttons = new List<Button>();

            startButtonPos = new Vector2((Screen.ScreenWidth / 2) - 200, (Screen.ScreenHeight / 2) + 80);
            playStylePos = new Vector2((Screen.ScreenWidth / 2) - 200, (Screen.ScreenHeight / 2) + 210);
            gameEndPos = new Vector2((Screen.ScreenWidth / 2) - 200, (Screen.ScreenHeight / 2) + 335);
            rightArrow = new Vector2();
            leftArrow = new Vector2();

            buttons.Add(new Button(ButtonType.titleUI_hajimeru, startButtonPos, 400, 100, this));
            buttons.Add(new Button(ButtonType.titleUI_setsumei, playStylePos, 400, 100, this));
            buttons.Add(new Button(ButtonType.titleUI_owaru, gameEndPos, 400, 100));
            buttons.Add(new Button(ButtonType.yajirushi, rightArrow, 64, 64, this));
            buttons.Add(new Button(ButtonType.hidarikun, leftArrow, 64, 64, this));

            kimeunchiR = 0;
            kimeunchiUpR = true;
         

            kimeunchiG = 0;
            kimeunchiUpG = true;

            kimeunchiG = 0;
            kimeunchiUpG = true;

        }

        public bool IsEnd()
        {
            return isEndFlag;
        }

        public SceneName Next()
        {
            return SceneName.Load;
        }

        public void Shutdown()
        {
            sound.StopBGM();
        }

        public void Update(GameTime gameTime)
        {
            if (kimeunchiUpR)
            {
                kimeunchiR+=3;
              
            }
            if (kimeunchiR >= 255)
            {
                kimeunchiUpR = false;
                kimeunchiR = 0;
                kimeunchiUpG = true;
                
            }
            if (!kimeunchiUpR)
            {
                kimeunchiG+=3;
            }
            if (kimeunchiG >= 255)
            {
                kimeunchiUpG = false;
                kimeunchiG = 0;
                kimeunchiUpB = true;
            }

            if (!kimeunchiUpG)
            {
                kimeunchiB+=3;
            }
            if (kimeunchiB >= 255)
            {
                kimeunchiUpB = false;
                kimeunchiB = 0;
                kimeunchiUpR = true;
            }



            foreach (var but in buttons)
            {
                but.Update();
            }
        }
    }
}
