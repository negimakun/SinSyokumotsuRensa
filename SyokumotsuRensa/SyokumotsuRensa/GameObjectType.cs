using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyokumotsuRensa
{
    class Screen
    {
        public static int ScreenWidth = 1450;
        public static int ScreenHeight = 950;
    }
    enum GameObjectType
    {
        meatEat,
        glassEat,
        glass,
        unchi,
        NULL
    }

    enum Direction
    {
        RIGHT,
        LEFT,
        TOP,
        BOTTOM,
        NULL
    }

    static class Collision
    {
        public static bool WallXEnemy(Wall wall, Enemy el1)
        {
            if (wall.position.X <= el1.enemyMovePos.X + el1.TextureSize//壁の左側
                && wall.position.X + wall.rectangle.Width >= el1.enemyMovePos.X - el1.TextureSize//壁の右側
                && wall.position.Y + wall.rectangle.Height >= el1.enemyMovePos.Y - el1.TextureSize &&//壁の下側
                    wall.position.Y <= el1.enemyMovePos.Y + el1.TextureSize)//壁の上側
            {
                return true;//当たってる
            }
            return false;
        }

        public static Direction WallXEnemyDirection(Wall wall, Enemy el1)
        {
            if (wall.position.X + wall.rectangle.Width <= el1.enemyPos.X)
            {//壁の右側（進行方向は左）
                return Direction.LEFT;
            }
            else if (wall.position.X >= el1.enemyPos.X + el1.TextureSize)
            {//壁の左側（進行方向は右）
                return Direction.RIGHT;
            }
            else if (wall.position.Y >= el1.enemyPos.Y)
            {//壁の上側（進行方向は下）
                return Direction.BOTTOM;
            }
            else if (wall.position.Y + wall.rectangle.Height >= el1.enemyPos.Y)
            {//壁の側（進行方向は）
                return Direction.TOP;
            }
            return Direction.NULL;
        }
        public static bool WallXPlayer(Wall wall, Player player)
        {
            if (wall.position.X <= player.movePos.X + player.TextureSize//壁の左側
                  && wall.position.X + wall.rectangle.Width >= player.movePos.X - player.TextureSize//壁の右側
                  && wall.position.Y + wall.rectangle.Height >= player.movePos.Y - player.TextureSize &&//壁の下側
                      wall.position.Y <= player.movePos.Y + player.TextureSize)//壁の上側
            {
                return true;//当たってる
            }
            return false;
        }

        public static Direction WallXPlayerDirection(Wall wall, Player player)
        {
            if (wall.position.X + wall.rectangle.Width <= player.playerPos.X)
            {//壁の右側（進行方向は左）
                return Direction.LEFT;
            }
            else if (wall.position.X >= player.playerPos.X + player.TextureSize)
            {//壁の左側（進行方向は右）
                return Direction.RIGHT;
            }
            else if (wall.position.Y >= player.playerPos.Y + player.TextureSize)
            {//壁の上側（進行方向は下）
                return Direction.BOTTOM;
            }
            else if (wall.position.Y + wall.rectangle.Height >= player.playerPos.Y)
            {//壁の側（進行方向は）
                return Direction.TOP;
            }
            return Direction.NULL;
        }
    }
}
