using UnityEngine;

public class WhiteboardDrawing : MonoBehaviour
{
    public RenderTexture whiteboardTexture;
    public Color drawColor = Color.black;
    public int brushSize = 10;

    public Transform rightHandAnchor;
    public Transform leftHandAnchor;
    public Renderer boardRenderer;

    private Texture2D drawTexture;

    void Start()
    {
        drawTexture = new Texture2D(
            whiteboardTexture.width,
            whiteboardTexture.height,
            TextureFormat.RGBA32,
            false
        );

        drawTexture.wrapMode = TextureWrapMode.Clamp;
        drawTexture.filterMode = FilterMode.Bilinear;

        ClearLocalTexture();

        // Make sure the material is actually showing the render texture
        if (boardRenderer != null)
        {
            boardRenderer.material.mainTexture = whiteboardTexture;
        }

        Graphics.Blit(drawTexture, whiteboardTexture);
    }

    void Update()
    {
        OVRInput.Update();

        // Right controller - draw
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.1f)
        {
            TryDraw(rightHandAnchor, drawColor);
        }

        // Left controller - erase
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.1f)
        {
            TryDraw(leftHandAnchor, Color.white);
        }
    }

    void TryDraw(Transform controllerTransform, Color colorToUse)
    {
        if (controllerTransform == null) return;

        if (Physics.Raycast(controllerTransform.position, controllerTransform.forward, out RaycastHit hit, 5f))
        {
            if (hit.collider.gameObject != gameObject) return;

            DrawOnTexture(hit.textureCoord, colorToUse);
        }
    }

    void DrawOnTexture(Vector2 textureCoord, Color colorToUse)
    {
        int x = Mathf.RoundToInt(textureCoord.x * drawTexture.width);
        int y = Mathf.RoundToInt(textureCoord.y * drawTexture.height);

        for (int i = -brushSize; i <= brushSize; i++)
        {
            for (int j = -brushSize; j <= brushSize; j++)
            {
                if (i * i + j * j <= brushSize * brushSize)
                {
                    int px = Mathf.Clamp(x + i, 0, drawTexture.width - 1);
                    int py = Mathf.Clamp(y + j, 0, drawTexture.height - 1);
                    drawTexture.SetPixel(px, py, colorToUse);
                }
            }
        }

        drawTexture.Apply();
        Graphics.Blit(drawTexture, whiteboardTexture);
    }

    public void ClearWhiteboard()
    {
        ClearLocalTexture();
        Graphics.Blit(drawTexture, whiteboardTexture);
    }

    void ClearLocalTexture()
    {
        Color[] pixels = new Color[drawTexture.width * drawTexture.height];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.white;

        drawTexture.SetPixels(pixels);
        drawTexture.Apply();
    }
}