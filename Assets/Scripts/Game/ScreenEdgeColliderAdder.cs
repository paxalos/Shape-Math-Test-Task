using UnityEngine;

namespace GameLogic
{
    [RequireComponent(typeof(Camera),
                      typeof(EdgeCollider2D))]
    public class ScreenEdgeColliderAdder : MonoBehaviour
    {
        private Camera mainCamera;
        private EdgeCollider2D edgeCollider;

        private void Awake()
        {
            SetComponents();
            AddCollider();
        }

        private void SetComponents()
        {
            mainCamera = GetComponent<Camera>();
            edgeCollider = GetComponent<EdgeCollider2D>();
        }

        private void AddCollider()
        {
            if (!mainCamera.orthographic)
            {
                Debug.LogError("Main camera is not Orthographic, failed to create edge colliders");
                return;
            }

            float nearClipPlane = mainCamera.nearClipPlane;
            float floorPositionY = mainCamera.pixelHeight / 5f;
            float pixelHeight = mainCamera.pixelHeight * 2;
            float pixelWidth = mainCamera.pixelWidth;


            var bottomLeft = (Vector2)mainCamera.ScreenToWorldPoint(new Vector3(0,
                                                                                floorPositionY,
                                                                                nearClipPlane));
            var topLeft = (Vector2)mainCamera.ScreenToWorldPoint(new Vector3(0,
                                                                             pixelHeight,
                                                                             mainCamera.nearClipPlane));
            var topRight = (Vector2)mainCamera.ScreenToWorldPoint(new Vector3(pixelWidth,
                                                                              pixelHeight,
                                                                              nearClipPlane));
            var bottomRight = (Vector2)mainCamera.ScreenToWorldPoint(new Vector3(pixelWidth,
                                                                                 floorPositionY,
                                                                                 nearClipPlane));

            edgeCollider.points = new[] { bottomLeft,
                                      topLeft,
                                      topRight,
                                      bottomRight,
                                      bottomLeft };
        }
    }
}