using System;
using GameStores;
using UnityEngine;
using Random = UnityEngine.Random;
using GameEnums;
using System.Collections.Generic;
using GameEventArgs;
using GameUI;
using System.Linq;
using GameRecords;
using GameModels;


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
        [SerializeField] private MessageUIController messageUIController;
        [SerializeField] private int minSpawnCount;
        [SerializeField] private int maxSpawnCount;
        [SerializeField] private int matchElementsCount;

        private const int MAX_ELEMENTS_TO_SELECT_COUNT = 7;

        private List<ShapeItemController> shapeItems = new();
        private SelectedElementRecord[] selectedElementRecords = new SelectedElementRecord[7];
        private int currentSelectionCellIndex = 0;
        private int shapeSpawnCount;

        private void Awake()
        {
#if UNITY_ASSERTIONS
            Assert.IsTrue(minSpawnCount >= 3 &&
                          minSpawnCount % 3 == 0);
            Assert.IsTrue(maxSpawnCount >= 3 &&
                          maxSpawnCount % 3 == 0 &&
                          maxSpawnCount >= minSpawnCount);
            Assert.IsTrue(matchElementsCount > 1 &&
                          matchElementsCount <= MAX_ELEMENTS_TO_SELECT_COUNT);
#endif

            SubscribeOnUIEvents();

            SetShapeSpawnCount();

            SpawnShapes();
        }

        private void SetShapeSpawnCount()
        {
            shapeSpawnCount = Random.Range(minSpawnCount, maxSpawnCount);
        }

        private void SubscribeOnUIEvents()
        {
            gameUIController.RestartButtonClicked += GameUIController_RestartButtonClicked;
            messageUIController.PlayAgainButtonClicked += MessageUIController_PlayAgainButtonClicked;
        }

        private void MessageUIController_PlayAgainButtonClicked()
        {
            messageUIController.gameObject.SetActive(false);
            SetShapeSpawnCount();
            RespawnShapes();
        }

        private void GameUIController_RestartButtonClicked()
        {
            RespawnShapes();
        }

        private void SpawnShapes()
        {
            var maxshapeSpriteSize = shapeStore.GetMaxShapeSpriteSize();
            float shapeItemPrefabScale = shapeItemPrefab.transform.localScale.x;

            float maxSpriteWidth = shapeItemPrefabScale * maxshapeSpriteSize.x;
            float maxSpriteHeight = shapeItemPrefabScale * maxshapeSpriteSize.y;

            var topSpawnLeft = (Vector2)mainCamera.ScreenToWorldPoint(new Vector3(maxSpriteWidth,
                                                                                  mainCamera.pixelHeight,
                                                                                  mainCamera.nearClipPlane));
            var topSpawnRight = (Vector2)mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth - maxSpriteWidth,
                                                                                   mainCamera.pixelHeight * 2 - maxSpriteHeight,
                                                                                   mainCamera.nearClipPlane));

            for (int i = 0; i < shapeSpawnCount / matchElementsCount; i++)
            {
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

                for (int j = 0; j < matchElementsCount; j++)
                {
                    float spawnX = Random.Range(topSpawnLeft.x, topSpawnRight.x);
                    float spawnY = Random.Range(topSpawnLeft.y, topSpawnRight.y);

                    var shapeItem = Instantiate(shapeItemPrefab,
                                                new Vector2(spawnX, spawnY),
                                                Quaternion.identity,
                                                transform);

                    shapeItem.Initialize(shapeRecord,
                                         shapeColorRecord,
                                         animalRecord);

                    shapeItem.ShapeItemClick += ShapeItem_Click;

                    shapeItems.Add(shapeItem);
                }
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

            gameUIController.ClearElementModels(new Range(0, MAX_ELEMENTS_TO_SELECT_COUNT));

            for (int i = 0; i < selectedElementRecords.Length; i++)
                selectedElementRecords[i] = null;

            currentSelectionCellIndex = 0;

            SpawnShapes();
        }

        private void ShapeItem_Click(object sender, EventArgs args)
        {
            var shapeItem = (ShapeItemController)sender;
            shapeItems.Remove(shapeItem);
            Destroy(shapeItem.gameObject);

            var shapeItemClickArgs = (ShapeItemClickArgs)args;
            var selectedElement = new SelectedElementRecord(shapeItemClickArgs.shapeRecord.shape,
                                                            shapeItemClickArgs.shapeColorRecord.shapeColor,
                                                            shapeItemClickArgs.animalRecord.animal);
            selectedElementRecords[currentSelectionCellIndex] = selectedElement;
            var selectedElementUIModel = new SelectedElementUIModel(shapeItemClickArgs.shapeRecord.shapeSprite,
                                                                    shapeItemClickArgs.shapeRecord.borderShapeSprite,
                                                                    shapeItemClickArgs.shapeColorRecord.shapeColorValue,
                                                                    shapeItemClickArgs.animalRecord.animalSprite);
            gameUIController.UpdateElementModel(selectedElementUIModel,
                                                currentSelectionCellIndex);
            ++currentSelectionCellIndex;

            CheckGameResult();
        }

        private void CheckGameResult()
        {
            if (currentSelectionCellIndex > matchElementsCount - 1)
            {
                Range checkRange = new Range(currentSelectionCellIndex - matchElementsCount,
                                             currentSelectionCellIndex);
                var elementForCheck = selectedElementRecords[checkRange];
                var firstElement = elementForCheck[0];
                if (elementForCheck.Skip(1).All(element => element.shape == firstElement.shape &&
                                                           element.shapeColor == firstElement.shapeColor &&
                                                           element.animal == firstElement.animal))
                {
                    for (int i = 0; i < elementForCheck.Length; i++)
                        elementForCheck[i] = null;
                    gameUIController.ClearElementModels(checkRange);
                    currentSelectionCellIndex -= matchElementsCount;
                }
            }

            if (shapeItems.Count == 0 &&
                currentSelectionCellIndex == 0)
            {
                messageUIController.gameObject.SetActive(true);
                messageUIController.SetMessage("You won!");
            }
            else if (currentSelectionCellIndex == MAX_ELEMENTS_TO_SELECT_COUNT ||
                     shapeItems.Count == 0 &&
                     currentSelectionCellIndex != 0)
            {
                messageUIController.gameObject.SetActive(true);
                messageUIController.SetMessage("You lost!");
            }
        }

        private T GetRandomEnumValue<T>()
        {
            var enumValues = Enum.GetValues(typeof(T));
            int randomIndex = Random.Range(0, enumValues.Length);
            return (T)enumValues.GetValue(randomIndex);
        }
    }
}