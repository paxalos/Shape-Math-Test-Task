using System;
using GameStores;
using UnityEngine;
using Random = UnityEngine.Random;
using GameEnums;
using System.Collections.Generic;
using GameEventArgs;
using GameUI;




#if UNITY_ASSERTIONS
using UnityEngine.Assertions;
#endif

namespace GameLogic
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private ShapeStore shapeStore;
        [SerializeField] private ShapeColorStore shapeColorStore;
        [SerializeField] private AnimalStore animalStore;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private ShapeItemController shapeItemPrefab;
        [SerializeField] private GameUIController gameUIController;
        [SerializeField] private int minSpawnCount;
        [SerializeField] private int maxSpawnCount;

        private List<ShapeItemController> shapeItems;

        public event Action<ShapeItemClickArgs> AddShapeItem;

        private void Awake()
        {
#if UNITY_ASSERTIONS
            Assert.IsTrue(minSpawnCount >= 3 &&
                          minSpawnCount % 3 == 0 &&
                          maxSpawnCount >= 3 &&
                          maxSpawnCount % 3 == 0 &&
                          maxSpawnCount >= minSpawnCount);
#endif

            shapeItems = new();

            SubscribeOnUIEvents();

            SpawnShapes();
        }

        private void SubscribeOnUIEvents()
        {
            gameUIController.RestartButtonClicked += GameUIController_RestartButtonClicked;
        }

        private void GameUIController_RestartButtonClicked()
        {
            SpawnShapes();
        }

        private void SpawnShapes()
        {
            float maxSpriteWidth = 0f;
            float maxSpriteHeight = 0f;
            float shapePrefabScale = shapeItemPrefab.transform.localScale.x;
            //for (int i = 0; i < shapeSprites.Length; i++)
            //{
            //    var shapeSpriteRect = shapeSprites[i].rect;
            //    float spriteWidth = shapeSpriteRect.width * shapePrefabScale;
            //    float spriteHeight = shapeSpriteRect.height * shapePrefabScale;
            //    if (maxSpriteWidth < spriteWidth)
            //        maxSpriteWidth = spriteWidth;
            //    if (maxSpriteHeight < spriteHeight)
            //        maxSpriteHeight = spriteHeight;
            //}

            var topSpawnLeft = (Vector2)mainCamera.ScreenToWorldPoint(new Vector3(maxSpriteWidth,
                                                                                  mainCamera.pixelHeight,
                                                                                  mainCamera.nearClipPlane));
            var topSpawnRight = (Vector2)mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth - maxSpriteWidth,
                                                                                   mainCamera.pixelHeight * 2 - maxSpriteHeight,
                                                                                   mainCamera.nearClipPlane));

            int shapeSpawnCount = Random.Range(minSpawnCount, maxSpawnCount);
            for (int i = 0; i < shapeSpawnCount; i++)
            {
                float spawnX = Random.Range(topSpawnLeft.x, topSpawnRight.x);
                float spawnY = Random.Range(topSpawnLeft.y, topSpawnRight.y);

                var shapeItem = Instantiate(shapeItemPrefab,
                                            new Vector2(spawnX, spawnY),
                                            Quaternion.identity,
                                            transform);

                var shape = GetRandomEnumValue<Shape>();
                var shapeRecord = shapeStore.GetRecordByShape(shape);
                if (shapeRecord == null)
                {
                    Debug.LogError($"Can't find shapeRecord for {shape}");
                    return;
                }

                var shapeColor = GetRandomEnumValue<ShapeColor>();
                var shapeColorRecord = shapeColorStore.GetRecordByShapeColor(shapeColor);
                if (shapeColorRecord == null)
                {
                    Debug.LogError($"Can't find shapeColorRecord for {shapeColor}");
                    return;
                }

                var animal = GetRandomEnumValue<Animal>();
                var animalRecord = animalStore.GetRecordByAnimal(animal);
                if (animalRecord == null)
                {
                    Debug.LogError($"Can't find animalRecord for {animal}");
                    return;
                }

                shapeItem.Initialize(shapeRecord,
                                     shapeColorRecord,
                                     animalRecord);

                shapeItem.ShapeItemClick += ShapeItem_Click;

                shapeItems.Add(shapeItem);
            }
        }

        private void RespawnShapes()
        {
            for (int i = 0; i < shapeItems.Count; i++)
            {
                var shapeItem = shapeItems[i];
                Destroy(shapeItem.gameObject);
            }

            shapeItems.Clear();

            SpawnShapes();
        }

        private void ShapeItem_Click(object sender, EventArgs args)
        {
            var shapeItem = (ShapeItemController)sender;
            shapeItems.Remove(shapeItem);
            Destroy(shapeItem.gameObject);

            var shapeItemClickArgs = (ShapeItemClickArgs)args;

        }

        private T GetRandomEnumValue<T>()
        {
            var enumValues = Enum.GetValues(typeof(T));
            int randomIndex = Random.Range(0, enumValues.Length);
            return (T)enumValues.GetValue(randomIndex);
        }
    }
}