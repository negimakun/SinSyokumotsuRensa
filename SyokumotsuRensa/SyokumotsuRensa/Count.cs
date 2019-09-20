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
            renderer.DrawNumber("number", new Vector2(120, 50), Player.playerStock);
            renderer.DrawNumber("number", new Vector2(120, 150), Glass.glassStock);
        }
    }
}
