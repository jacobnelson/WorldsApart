using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace WorldsApart.Code
{
    static class Mathness
    {
        static Random random = new Random();
        static public int RandomNumber()
        {
            return random.Next();
        }
        static public int RandomNumber(int maxValue)
        {
            return random.Next(maxValue + 1); //I increase the maxValue that you pass in by one, because most people don't think of the maxValue as exclusive.
        }
        static public int RandomNumber(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue + 1);
        }

        static public float RandomNumber(float minValue, float maxValue)
        {
            return (float)random.Next((int)(minValue * 1000), (int)(maxValue * 1000)) / 1000f;
        }

        static public Vector2 VectorToTarget(Vector2 targetPosition, Vector2 thisPosition, float innerDistance)
        {
            Vector2 distanceVector = thisPosition - targetPosition;
            float distance = Vector2.Distance(targetPosition, thisPosition);
            return distanceVector * innerDistance / distance;
        }
    }
}
