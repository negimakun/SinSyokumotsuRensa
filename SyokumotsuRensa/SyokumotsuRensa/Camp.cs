using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyokumotsuRensa
{
    class Camp
    {
        public Vector2 campPos;
        public Vector2 centerPosition;

        public Camp()
        {
            campPos = new Vector2(800, 400);
            centerPosition = new Vector2(campPos.X + 75, campPos.Y + 75);
        }

        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture("house", campPos);
        }
    }
}
