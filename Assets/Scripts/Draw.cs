using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class Draw : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private bool userInterpolation = true;

    [SerializeField]
    private int totalXPixels = 512;

    [SerializeField]
    private int totalYPixels = 512;

    [SerializeField]
    private int brushSize = 4;

    [SerializeField]
    private Color brushColor;

    [SerializeField]
    private Transform topLeftCorner;

    [SerializeField]
    private Transform bottomRightCorner;

    [SerializeField]
    private Transform point;

    [SerializeField]
    private Material material;

    private Texture2D generatedTexture;

    private Color[] colorMap;

    int xPixel = 0;
    int yPixel = 0;

    bool pressedLastFrame = false;
    int lastX = 0, lastY = 0;

    float xMult;
    float yMult;

    [SerializeField] private Puzzle puzzleScript;

    [Header("Puzzle Detection")]
    [SerializeField] private LayerMask startLayer;
    [SerializeField] private LayerMask safeLayer;
    [SerializeField] private LayerMask exitLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask boardLayer;

    private bool hasStartedCorrectly = false;
    public bool isPathValid = true;
    private bool hasWon = false;

    [Header("Checkpoint Settings")]
    private bool hasHitCheckpoint = false;
    [SerializeField] private LayerMask checkpointLayer;
    [SerializeField] private bool useCheckpoint = false;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 0.5f; // How long the fade takes
    private Coroutine fadeRoutine;

    private void Start()
    {
        colorMap = new Color[totalXPixels * totalYPixels];
        generatedTexture = new Texture2D(totalYPixels, totalXPixels, TextureFormat.RGBA32, false);
        generatedTexture.filterMode = FilterMode.Point;
        material.SetTexture("_BaseMap", generatedTexture);

        ResetColor();

        xMult = totalXPixels / (bottomRightCorner.localPosition.x - topLeftCorner.localPosition.x);
        yMult = totalYPixels / (bottomRightCorner.localPosition.y - topLeftCorner.localPosition.y);
    }

    private void Update()
    {
        //Cursor.lockState = CursorLockMode.None;

        if (Input.GetMouseButton(0))
        {
            if (fadeRoutine != null)
            {
                StopCoroutine(fadeRoutine);
                fadeRoutine = null;
                ResetColor();
            }

            CalculatePixel();
        }
    
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse released! Resetting puzzle...");

            // Logic to reset the puzzle state
            hasStartedCorrectly = false;
            isPathValid = true;
            pressedLastFrame = false; // Reset this too just in case
            hasHitCheckpoint = false;

            // Start the visual fade out
            if (fadeRoutine != null) StopCoroutine(fadeRoutine);
            fadeRoutine = StartCoroutine(FadeOutStroke());
        }

    }

    private void CalculatePixel()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 1. Raycast ONLY to the Board layer for smooth position
        if (Physics.Raycast(ray, out hit, 10f, boardLayer))
        {
            point.position = hit.point;
            xPixel = (int)((point.localPosition.x - topLeftCorner.localPosition.x) * xMult);
            yPixel = (int)((point.localPosition.y - topLeftCorner.localPosition.y) * yMult);

            // Reduced radius to 0.02f for higher precision
            Collider[] hitColliders = Physics.OverlapSphere(hit.point, 0.01f);

            bool currentlyOnValidSurface = false;
            bool currentlyTouchingWall = false;
            string wallName = ""; // To store the name of the object that failed us

            foreach (var col in hitColliders)
            {
                int colLayer = col.gameObject.layer;
                int colLayerMask = 1 << colLayer;

                // WALL CHECK
                if ((colLayerMask & wallLayer) != 0)
                {
                    currentlyTouchingWall = true;
                    wallName = col.gameObject.name; // Capture the name for debugging
                }

                // SAFE / START / EXIT CHECK
                if ((colLayerMask & (safeLayer | startLayer | exitLayer | checkpointLayer)) != 0)
                {
                    currentlyOnValidSurface = true;
                }

                // START TRIGGER
                if ((colLayerMask & startLayer) != 0 && !hasStartedCorrectly)
                {
                    hasStartedCorrectly = true;
                    isPathValid = true;
                    hasHitCheckpoint = false;
                    Debug.Log("Path Started!");
                }

                if ((colLayerMask & checkpointLayer) != 0)
                {
                    if (hasStartedCorrectly && !hasHitCheckpoint)
                    {
                        hasHitCheckpoint = true;
                        Debug.Log("Checkpoint Reached!");
                    }
                }

                // WIN TRIGGER
                if ((colLayerMask & exitLayer) != 0 && hasStartedCorrectly && isPathValid && !hasWon)
                {
                    if (hasHitCheckpoint || !useCheckpoint)
                    {
                        hasWon = true;
                        puzzleScript.SolvePuzzle();
                        Debug.Log("Maze Complete!");
                    }
                }
            }

            // --- FINAL VALIDATION --- Additional Check
            if (hasStartedCorrectly && isPathValid)
            {

                if (currentlyTouchingWall)
                {
                    isPathValid = false;
                    Debug.Log($"Hit a Wall! Object causing fail: {wallName}");
                }
                else if (!currentlyOnValidSurface)
                {
                    isPathValid = false;
                    Debug.Log("Left the Correct Path!");
                }
            }

            ChangePixelsAroundPoint();
        }
        else
        {
            pressedLastFrame = false;
        }
    }

    private void ChangePixelsAroundPoint()
    {
        if (userInterpolation && pressedLastFrame && (lastX != xPixel || lastY != yPixel))
        {
            int dist = (int) Mathf.Sqrt((xPixel-lastX) * (xPixel-lastX)+(yPixel-lastY)*(yPixel-lastY));

            for (int i = 1; i <= dist; i++)
            {
                DrawBrush((i * xPixel + (dist - i) * lastX) / dist, (i * yPixel + (dist - i) * lastY) / dist);
            }

        }
        DrawBrush(xPixel, yPixel);
        pressedLastFrame = true;
        lastX = xPixel;
        lastY = yPixel;
        SetTexture();
    }

    void DrawBrush(int xPix, int yPix)
    {
        int i = Mathf.Max(0, xPix - brushSize);
        int j = Mathf.Max(0, yPix - brushSize);
        int maxi = Mathf.Min(totalXPixels - 1, xPix + brushSize);
        int maxj = Mathf.Min(totalYPixels - 1, yPix + brushSize);
        for (int x = i; x <= maxi; x++)
        {
            for (int y = j; y <= maxj; y++)
            {

                if ((x - xPix) * (x - xPix) + (y - yPix) * (y - yPix) <= brushSize * brushSize)
                {
                    int index = x * totalYPixels + y;
                    if (index >= 0 && index < colorMap.Length)
                    {
                        colorMap[index] = brushColor;
                    }
                }
            }
        }
    }

    void SetTexture()
    {
        generatedTexture.SetPixels(colorMap);
        generatedTexture.Apply();
    }

    public void ResetColor()
    {
        for (int i = 0; i < colorMap.Length; i++)
            colorMap[i] = Color.white;
        SetTexture();
        isPathValid = true;
    }



    IEnumerator FadeOutStroke()
    {
        float elapsedTime = 0f;

        // 1. Create a copy of the drawn pixels so we know what we are fading FROM
        Color[] startColors = (Color[])colorMap.Clone();

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration; // 0 to 1 value based on time

            // 2. Loop through all pixels and interpolate them towards White
            // (Note: This loop runs every frame. For 512x512 it should be fine on PC, 
            // but if it lags, consider lowering resolution or shortening duration)
            for (int i = 0; i < colorMap.Length; i++)
            {
                // Optimization: Only process pixels that aren't already white
                if (startColors[i] != Color.white)
                {
                    // Lerp from the specific drawn color to White
                    colorMap[i] = Color.Lerp(startColors[i], Color.white, t);
                }
            }

            SetTexture(); // Apply changes to the visual texture
            yield return null; // Wait for the next frame
        }

        // 3. Ensure it is perfectly clean at the end
        ResetColor();
        fadeRoutine = null;
    }
}
