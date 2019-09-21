using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SyokumotsuRensa.Scene
{
    interface IScene
    {
        void Initialize();
        void Update(GameTime gameTime);
        void Draw(Renderer renderer);
        void Shutdown();
     
        bool IsEnd();
        SceneName Next();
    }
}
