using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SyokumotsuRensa
{
    class Count
    {

        public Count()
        {

        }
        public void Initialize()
        {

        }
        public void Update()
        {

        }
        public void Draw(Renderer renderer)
        {
            renderer.DrawNumber("number", new Vector2(100, 50), Player.playerStock);
            renderer.DrawNumber("number", new Vector2(100, 150), Player2.player2Stock);
            renderer.DrawNumber("number", new Vector2(100, 250), Player3.player3Stock);
            renderer.DrawNumber("number", new Vector2(100, 350), Glass.glassStock);
        }
    }
}
