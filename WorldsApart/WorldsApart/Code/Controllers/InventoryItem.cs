using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using WorldsApart.Code.Graphics;

namespace WorldsApart.Code.Controllers
{
    class InventoryItem
    {
        public SpriteIMG itemImage;
        public bool hasItem = false;
        public Vector2 targetPosition = Vector2.Zero;
        public Vector2 prevTarget = Vector2.Zero;

        public InventoryItem(Texture2D texture)
        {
            itemImage = new SpriteIMG(texture);
        }
    }
}
