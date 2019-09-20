using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyokumotsuRensa
{
    class Unchi
    {
        Vector2 position;
        float deleteTime;
        List<Glass> glasses;

        public bool iswwwFlag = false;

        public Unchi(Vector2 pos,List<Glass> glasses)
        {
            position = pos;
            deleteTime = 3;
            this.glasses = glasses;
        }

        public void Update()
        {
            if (deleteTime > 0)
            {
                deleteTime -= 1.0f / 60.0f;
            }
            else
            {
                //草生える処理
                glasses.Add(new Glass(position));
                glasses[glasses.Count - 1].Initialize();
                iswwwFlag = true;
               
            }
        }

        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture("unchi", position);
        }
    }
}
