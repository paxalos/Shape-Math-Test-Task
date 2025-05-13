using GameEnums;

namespace GameRecords
{
    public record SelectedElementRecord
    {
        public Shape shape;
        public ShapeColor shapeColor;
        public Animal animal;

        public SelectedElementRecord(Shape shape,
                                     ShapeColor shapeColor,
                                     Animal animal)
        {
            this.shape = shape;
            this.shapeColor = shapeColor;
            this.animal = animal;
        }
    }
}