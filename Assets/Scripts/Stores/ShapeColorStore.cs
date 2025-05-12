using System.Linq;
using GameEnums;
using GameRecords;
using UnityEngine;

namespace GameStores
{
    [CreateAssetMenu(fileName = "ShapeColorStore",
                     menuName = "Stores/ShapeColorStore")]
    public class ShapeColorStore : ScriptableObject
    {
        [SerializeField] private ShapeColorRecord[] shapeColorRecords;

        public ShapeColorRecord GetRecordByShapeColor(ShapeColor shapeColor)
            => shapeColorRecords.FirstOrDefault(scr => scr.shapeColor == shapeColor);
    }

}