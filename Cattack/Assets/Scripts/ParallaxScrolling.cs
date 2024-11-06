using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    public Transform[] backgrounds;
    public float[] parallaxScales;

    private float[] startPositions;
    private float cameraStartPosition;
    private float cameraWidth;

    private void Start()
    {
        cameraStartPosition = transform.position.x;
        cameraWidth = Camera.main.orthographicSize * Camera.main.aspect;

        startPositions = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            startPositions[i] = backgrounds[i].position.x;
        }
    }

    private void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (transform.position.x - cameraStartPosition) * parallaxScales[i];
            float backgroundTargetPosition = startPositions[i] + parallax;
            Vector3 backgroundPosition = backgrounds[i].position;
            backgroundPosition.x = backgroundTargetPosition;
            backgrounds[i].position = backgroundPosition;
        }
    }
}