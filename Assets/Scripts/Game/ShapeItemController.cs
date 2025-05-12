using System;
using System.Collections.Generic;
using GameEventArgs;
using GameRecords;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameLogic
{
    public class ShapeItemController : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private SpriteRenderer shapeRenderer;
        [SerializeField] private SpriteRenderer borderRenderer;
        [SerializeField] private SpriteRenderer animalRenderer;
        [SerializeField] private PolygonCollider2D shapeItemCollider;

        private ShapeRecord shapeRecord;
        private ShapeColorRecord shapeColorRecord;
        private AnimalRecord animalRecord;

        public EventHandler ShapeItemClick;

        public void Initialize(ShapeRecord shapeRecord,
                               ShapeColorRecord shapeColorRecord,
                               AnimalRecord animalRecord)
        {
            this.shapeRecord = shapeRecord;
            this.shapeColorRecord = shapeColorRecord;
            this.animalRecord = animalRecord;

            borderRenderer.sprite = shapeRecord.borderShapeSprite;
            animalRenderer.sprite = animalRecord.animalSprite;

            shapeRenderer.color = shapeColorRecord.shapeColorValue;

            var shapeSprite = shapeRecord.shapeSprite;
            shapeRenderer.sprite = shapeSprite;
            List<Vector2> physicsShape = new();
            shapeSprite.GetPhysicsShape(0, physicsShape);
            shapeItemCollider.SetPath(0, physicsShape);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var handler = ShapeItemClick;
            handler?.Invoke(this,
                            new ShapeItemClickArgs(shapeRecord,
                                                   shapeColorRecord,
                                                   animalRecord));
        }
    }
}