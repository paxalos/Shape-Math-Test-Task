using System;
using GameRecords;

namespace GameEventArgs
{
    public class ShapeItemClickArgs : EventArgs
    {
        public ShapeRecord shapeRecord;
        public ShapeColorRecord shapeColorRecord;
        public AnimalRecord animalRecord;

        public ShapeItemClickArgs(ShapeRecord shapeRecord, 
                                  ShapeColorRecord shapeColorRecord, 
                                  AnimalRecord animalRecord)
        {
            this.shapeRecord = shapeRecord;
            this.shapeColorRecord = shapeColorRecord;
            this.animalRecord = animalRecord;
        }
    }
}