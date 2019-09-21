using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
namespace SyokumotsuRensa.Scene
{
    class SceneManager
    {
        private Dictionary<SceneName, IScene> scenes = new Dictionary<SceneName, IScene>();
        //現在のシーン
        private IScene currentScene = null;

        public SceneManager()
        {

        }

        public void Add(SceneName name, IScene scene)
        {
            if (scenes.ContainsKey(name))
            {
                return;
            }
            //シーン追加
            scenes.Add(name, scene);
        }
        public void Change(SceneName name)
        {
            //何かシーンが登録されていたら
            if (currentScene != null)
            {
                currentScene.Shutdown();
            }
            //ディクショナリから次のシーンを取り出す
            currentScene = scenes[name];
            currentScene.Initialize();
        }
        public void Update(GameTime gameTime)
        {
            if (currentScene == null)
            {
                return;
            }

            currentScene.Update(gameTime);

            if (currentScene.IsEnd())
            {
                //シーンを切り替え
                Change(currentScene.Next());
            }

        }
        public void Draw(Renderer renderer)
        {
            if (currentScene == null)
            {
                return;
            }
            currentScene.Draw(renderer);
        }

    }
}
