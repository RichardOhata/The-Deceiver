using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Draw : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private int totalXPixels = 512;

    private bool userInterpolation = true;


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

    [Header("Maze Textures")]
    [SerializeField] private Texture2D mazeOutline;
    [SerializeField] private Texture2D solutionMask;

    private bool isOffPath = false;

    private void Start()
    {
        //cam = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>();
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
        Cursor.lockState = CursorLockMode.None;
        if (Input.GetMouseButton(0))
        {
            CalculatePixel();
        } else
        {
            pressedLastFrame = false;
        }
    }

    private void CalculatePixel()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f))
        {
            point.position = hit.point;
            xPixel = (int)((point.localPosition.x - topLeftCorner.localPosition.x) * xMult);
            yPixel = (int)((point.localPosition.y - topLeftCorner.localPosition.y) * yMult);
            Debug.Log("X :" + xPixel + "Y: " + yPixel);
            ChangePixelsAroundPoint();
        } else
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
    }
}
