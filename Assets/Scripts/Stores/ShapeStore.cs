using System.Linq;
using GameEnums;
using GameRecords;
using UnityEngine;

namespace GameStores
{
    [CreateAssetMenu(fileName = "ShapeStore",
                     menuName = "Stores/ShapeStore")]
    public class ShapeStore : ScriptableObject
    {
        [SerializeField] private ShapeRecord[] shapeRecords;

        public ShapeRecord GetRecordByShape(Shape shape)
            => shapeRecords.FirstOrDefault(sr => sr.shape == shape);

        public Vector2 GetMaxShapeSpriteSize()
        {
            float maxSpriteWidth = 0f;
            float maxSpriteHeight = 0f;

            for (int i = 0; i < shapeRecords.Length; i++)
            {
                var shapeRecord = shapeRecords[i];
                var shapeSpriteRect = shapeRecord.shapeSprite.rect;
                float spriteWidth = shapeSpriteRect.width;
                float spriteHeight = shapeSpriteRect.height;
                if (maxSpriteWidth < spriteWidth)
                    maxSpriteWidth = spriteWidth;
                if (maxSpriteHeight < spriteHeight)
                    maxSpriteHeight = spriteHeight;
            }

            return new Vector2 (maxSpriteWidth, maxSpriteHeight);
        }
    }
}