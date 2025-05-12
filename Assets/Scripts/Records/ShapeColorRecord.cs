using System;
using GameEnums;
using UnityEngine;

namespace GameRecords
{
    [Serializable]
    public record ShapeColorRecord
    {
        public ShapeColor shapeColor;
        public Color shapeColorValue;
    }
}