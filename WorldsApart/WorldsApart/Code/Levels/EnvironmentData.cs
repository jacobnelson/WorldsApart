using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace WorldsApart.Code.Levels
{
    enum FrictionType
    {
        Normal,
        Ice,
        Boost
    }

    enum InclineType
    {
        Flat,
        ThreeTileRight1,
        ThreeTileRight2,
        ThreeTileRight3,
        ThreeTileLeft1,
        ThreeTileLeft2,
        ThreeTileLeft3,
        TwoTileRight1,
        TwoTileRight2,
        TwoTileLeft1,
        TwoTileLeft2
    }

    class EnvironmentData
    {
        public bool isSolid = false;
        public FrictionType frictionType = FrictionType.Normal;
        public Vector2 airCurrent = Vector2.Zero;
        public InclineType inclineType = InclineType.Flat;
        public bool inclineRight = true;

        public bool checkpoint = false;
        public bool killZone = false;

        static public float NORMAL = .9f;
        static public float AIR = .99f;
        static public float ICE = .99f;
        static public float BOOST = 1.2f;

        public bool omitBottom = false;
        public bool omitTop = false;
        public bool omitRight = false;
        public bool omitLeft = false;

        public bool oneWayPlatform = false;

        public EnvironmentData(bool isSolid)
        {
            this.isSolid = isSolid;
        }
        public EnvironmentData(FrictionType type)
            : this(true)
        {
            this.frictionType = type;
        }
        public EnvironmentData(Vector2 airCurrent)
            : this(false)
        {
            this.airCurrent = airCurrent;
        }

        public EnvironmentData(InclineType incline, FrictionType friction)
            : this(friction)
        {
            inclineType = incline;
            switch (inclineType)
            {
                case InclineType.ThreeTileLeft1:
                case InclineType.ThreeTileLeft2:
                case InclineType.ThreeTileLeft3:
                case InclineType.TwoTileLeft1:
                case InclineType.TwoTileLeft2:
                    inclineRight = false;
                    break;
                case InclineType.ThreeTileRight1:
                case InclineType.ThreeTileRight2:
                case InclineType.ThreeTileRight3:
                case InclineType.TwoTileRight1:
                case InclineType.TwoTileRight2:
                    inclineRight = true;
                    break;

            }
        }
    }
}
