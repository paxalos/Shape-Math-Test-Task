using System;
using GameEnums;
using UnityEngine;

namespace GameRecords
{
    [Serializable]
    public record AnimalRecord
    {
        public Animal animal;
        public Sprite animalSprite;
    }
}