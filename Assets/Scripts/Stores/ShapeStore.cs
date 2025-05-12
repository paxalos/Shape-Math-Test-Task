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
    }
}