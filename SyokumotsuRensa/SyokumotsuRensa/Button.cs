using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyokumotsuRensa.Scene;

namespace SyokumotsuRensa
{
    class Button
    {

        Title title;
        Wave wave;
        GamePlay gamePlay;

        Vector2 position;

        int width;
        int height;
        float right;
        float bottom;

        Vector2 mousePosition;

        ButtonType buttonType;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="button">何のボタン？</param>
        /// <param name="pos">描画位置</param>
        /// <param name="width">横幅</param>
        /// <param name="height">縦幅</param>
        public Button(ButtonType button, Vector2 pos, int width, int height)
        {
            buttonType = button;
            position = pos;
            this.width = width;
            this.height = height;
            right = pos.X + width;
            bottom = pos.Y + height;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="button">何のボタン？</param>
        /// <param name="pos">描画位置</param>
        /// <param name="width">横幅</param>
        /// <param name="height">縦幅</param>
        /// <param name="title">タイトル</param>
        public Button(ButtonType button, Vector2 pos, int width, int height, Title title)
        {
            buttonType = button;
            position = pos;
            this.width = width;
            this.height = height;
            right = pos.X + width;
            bottom = pos.Y + height;
            this.title = title;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="button">何のボタン？</param>
        /// <param name="pos">描画位置</param>
        /// <param name="width">横幅</param>
        /// <param name="height">縦幅</param>
        /// <param name="wave">ウェーブ</param>
        public Button(ButtonType button, Vector2 pos, int width, int height, Wave wave)
        {
            buttonType = button;
            position = pos;
            this.width = width;
            this.height = height;
            right = pos.X + width;
            bottom = pos.Y + height;
            this.wave = wave;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="button">何のボタン？</param>
        /// <param name="pos">描画位置</param>
        /// <param name="width">横幅</param>
        /// <param name="height">縦幅</param>
        /// <param name="gamePlay">ゲームプレイ</param>
        public Button(ButtonType button, Vector2 pos, int width, int height, GamePlay gamePlay)
        {
            buttonType = button;
            position = pos;
            this.width = width;
            this.height = height;
            right = pos.X + width;
            bottom = pos.Y + height;
            this.gamePlay = gamePlay;
        }

        public void Update()
        {
            if (ButtonClick())
            {
                switch (buttonType)
                {
                    case ButtonType.titleUI_hajimeru:
                        Start();
                        break;
                    case ButtonType.titleUI_setsumei:
                        PlayStyle();
                        break;
                    case ButtonType.titleUI_setsumei_end:
                        PlayStyleEnd();
                        break;
                    case ButtonType.titleUI_owaru:
                        GameEnd();
                        break;
                    case ButtonType.yajirushi:
                        PlayStyleArrowRight();
                        break;
                    case ButtonType.hidarikun:
                        PlayStyleArrowLeft();
                        break;
                    case ButtonType.resultUI_retry:
                        Retry();
                        break;
                    case ButtonType.resultUI_title:
                        Title();
                        break;
                    case ButtonType.nextday:
                        NextWave();
                        break;
                    default:
                        break;
                }
            }
        }

        public bool ButtonClick()
        {
            mousePosition = Input.MousePosition;
            if (position.X < mousePosition.X && mousePosition.X < right &&
                position.Y < mousePosition.Y && mousePosition.Y < bottom &&
                Input.IsMouseLButtonDown())
            {
                return true;
            }
            return false;
        }



        public void Start()
        {
            if (!title.playStyleFlag)
            {
                title.isEndFlag = true;
            }
        }

        public void GameEnd()
        {
            GameEndFlag.gameEndFlag = true;
        }

        public void PlayStyle()
        {
            title.playStyleFlag = true;
        }

        public void PlayStyleEnd()
        {
            title.playStyleFlag = false;
        }

        public void PlayStyleArrowRight()
        {
            if (title.playStyleEndPage > title.playStylePage)
            {
                title.playStylePage++;
            }
        }
        public void PlayStyleArrowLeft()
        {
            if (1 < title.playStylePage)
            {
                title.playStylePage--;
            }
        }

        public void Retry()
        {
            gamePlay.Initialize();
        }

        public void Title()
        {
            gamePlay.returnTitleFlag = true;
        }

        public void NextWave()
        {
            wave.GotoWave();
            wave.isClearFlag = false;
        }
    }
}
