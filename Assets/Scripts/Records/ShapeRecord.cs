using System;
using GameEnums;
using UnityEngine;

namespace GameRecords
{
    [Serializable]
    public record ShapeRecord
    {
        public Shape shape;
        public Sprite shapeSprite;
        public Sprite borderShapeSprite;
    }
}