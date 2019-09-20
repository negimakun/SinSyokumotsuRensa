using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SyokumotsuRensa
{
    class Glass
    {
        Vector2 glassStockPos;
        public Vector2 glassPos;
        Vector2 glassSpowPos;
        public Vector2 glassMasu;
        public static int glassStock = 10;
        public bool isDeadFlag = false;
        public bool setGlassFlag = false;
        bool clickFlag = false;
        bool unchiFlag = false;
        readonly int TextureSize = 50;



        public Glass()
        {
            glassSpowPos = new Vector2(850, 450);
        }
        
        /// <summary>
        /// うんこ産の草
        /// </summary>
        /// <param name="pos"></param>
        public Glass(Vector2 pos)
        {
            glassSpowPos = pos;
           // clickFlag = true;
           // setGlassFlag = true;
        }

        public void Initialize()
        {
            glassStockPos = new Vector2(50, 150);
            glassPos = glassSpowPos;
        }
        public void Update()
        {
            if (glassStock > 0 || true)
            {
                glassMasu = new Vector2(glassPos.X / TextureSize, glassPos.Y / TextureSize);
                if (Input.IsMouseLButtonDown())
                {
                    if ((glassStockPos == new Vector2((int)(Input.MousePosition.X / TextureSize)
                                 * TextureSize, (int)(Input.MousePosition.Y / TextureSize) * TextureSize) || clickFlag)&&!setGlassFlag)
                    {
                        if (!clickFlag)
                        {
                            glassPos = new Vector2((int)(glassPos.X / TextureSize)/*何マス目か*/ * TextureSize,
                                (int)(glassPos.Y / TextureSize) * TextureSize);

                            clickFlag = true;
                        }
                        else if(clickFlag && Input.MousePosition.X > 300)
                        {
                            glassPos = new Vector2((int)(Input.MousePosition.X / TextureSize)/*何マス目か*/ * TextureSize,
                                (int)(Input.MousePosition.Y / TextureSize) * TextureSize);

                            Glass.glassStock--;
                            setGlassFlag = true;
                            //clickFlag = false;
                            glassMasu = new Vector2(glassPos.X / TextureSize, glassPos.Y / TextureSize);

                        }
                    }
                }
            }

        }
        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture("glass", glassStockPos);
            if (!isDeadFlag)
            {
                renderer.DrawTexture("glass", glassPos);
            }

        }

    }
}
