using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace MagicDye
{
    internal class ColorModifier // class(es) for the color modifier system
    {
        internal string name;
        internal int itemType;
        internal Func<Entity, Vector3, Vector3, float, float, (Vector3 primaryColor, Vector3 secondaryColor, float saturation, float opacity)> colorFunction;

        public ColorModifier(string name, int itemType, Func<Entity, Vector3, Vector3, float, float, (Vector3, Vector3, float, float)> colorFunction)
        {
            this.name = name;
            this.itemType = itemType;
            this.colorFunction = colorFunction;
        }
    }
}