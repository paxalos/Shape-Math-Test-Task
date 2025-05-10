using System.Collections.Generic;
using UnityEngine;

public class ShapeController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer shape;
    [SerializeField] private SpriteRenderer border;
    [SerializeField] private SpriteRenderer animal;
    [SerializeField] private PolygonCollider2D shapeCollider;

    public void SetShapeSprites(Sprite shapeSprite,
                                Sprite borderSprite,
                                Sprite animalSprite,
                                Color shapeColor)
    {
        shape.sprite = shapeSprite;
        border.sprite = borderSprite;
        animal.sprite = animalSprite;

        shape.color = shapeColor;

        List<Vector2> physicsShape = new();
        shapeSprite.GetPhysicsShape(0, physicsShape);
        shapeCollider.SetPath(0, physicsShape);
    }
}
