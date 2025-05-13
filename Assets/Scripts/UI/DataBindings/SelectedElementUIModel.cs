using UnityEngine;

namespace GameModels
{
    public record SelectedElementUIModel
    {
        public Sprite shapeSprite;
        public Sprite shapeBorderSprite;
        public Color shapeColorValue;
        public Sprite animalSprite;

        public SelectedElementUIModel() { }

        public SelectedElementUIModel(Sprite shapeSprite,
                                      Sprite shapeBorderSprite,
                                      Color shapeColorValue,
                                      Sprite animalSprite)
        {
            this.shapeSprite = shapeSprite;
            this.shapeBorderSprite = shapeBorderSprite;
            this.shapeColorValue = shapeColorValue;
            this.animalSprite = animalSprite;
        }

        public void SetValues(SelectedElementUIModel model)
        {
            shapeSprite = model.shapeSprite;
            shapeBorderSprite = model.shapeBorderSprite;
            shapeColorValue = model.shapeColorValue;
            animalSprite = model.animalSprite;
        }

        public void Clear()
        {
            shapeSprite = null;
            shapeBorderSprite = null;
            shapeColorValue = Color.white;
            animalSprite = null;
        }
    }
}