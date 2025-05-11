using UnityEngine;
#if UNITY_ASSERTIONS
using UnityEngine.Assertions;
#endif

public class ShapeSpawnController : MonoBehaviour
{
    [SerializeField] private Sprite[] animalSprites;
    [SerializeField] private Sprite[] shapeSprites;
    [SerializeField] private Sprite[] borderSprites;
    [SerializeField] private Color[] shapeColors;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private int minSpawnCount;
    [SerializeField] private int maxSpawnCount;
    [SerializeField] private ShapeController shapePrefab;

    private void Awake()
    {
#if UNITY_ASSERTIONS
        Assert.AreEqual(shapeSprites.Length, borderSprites.Length);
        Assert.IsTrue(minSpawnCount >= 3 &&
                      minSpawnCount % 3 == 0 &&
                      maxSpawnCount >= 3 &&
                      maxSpawnCount % 3 == 0 &&
                      maxSpawnCount >= minSpawnCount &&
                      shapeColors.Length >= 0 &&
                      shapeColors.Length % 3 == 0);
#endif

        SpawnShapes();
    }

    private void SpawnShapes()
    {
        float maxSpriteWidth = 0f;
        float maxSpriteHeight = 0f;
        float shapePrefabScale = shapePrefab.transform.localScale.x;
        for (int i = 0; i < shapeSprites.Length; i++)
        {
            var shapeSpriteRect = shapeSprites[i].rect;
            float spriteWidth = shapeSpriteRect.width * shapePrefabScale;
            float spriteHeight = shapeSpriteRect.height * shapePrefabScale;
            if (maxSpriteWidth < spriteWidth)
                maxSpriteWidth = spriteWidth;
            if (maxSpriteHeight < spriteHeight)
                maxSpriteHeight = spriteHeight;
        }

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

            var shape = Instantiate(shapePrefab,
                                    new Vector2(spawnX, spawnY),
                                    Quaternion.identity,
                                    transform);

            int shapeIndex = Random.Range(0, shapeSprites.Length);
            int animalIndex = Random.Range(0, animalSprites.Length);
            int colorIndex = Random.Range(0, shapeColors.Length);

            shape.SetShapeSprites(shapeSprites[shapeIndex],
                                  borderSprites[shapeIndex],
                                  animalSprites[animalIndex],
                                  shapeColors[colorIndex]);
        }
    }
}
