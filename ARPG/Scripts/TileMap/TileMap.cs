﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class TileMap
    {
        #region Constant variables
        public Tile[,] masterMap;
        public List<Room> rooms = new();
        #endregion

        public readonly int tileMapWidth = 350;
        public readonly int tileMapHeight = 350;

        public void GenerateNewMap()
        {
            if (Library.activeRoom != null)
            {
                for (int i = Library.gameObjects.Count - 1; i >= 0; i--)
                {
                    if (Library.gameObjects[i] != Library.playerInstance)
                    {
                        Library.gameObjects[i].Destroy();
                    }
                }
            }

            rooms.Clear();
            Library.activeRoom = null;
            masterMap = new Tile[tileMapWidth, tileMapHeight];

            RoomGeneration.CallGeneration();
        }

        public void DrawMap(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                rooms[i].Draw(spriteBatch);
            }
        }
    }
}
