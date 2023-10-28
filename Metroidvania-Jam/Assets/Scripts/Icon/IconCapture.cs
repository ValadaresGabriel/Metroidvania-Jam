using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IconCapture : MonoBehaviour
{
    public List<Mesh> itemModels; // Lista de Prefabs dos modelos
    public Camera captureCamera; // Referência para a câmera ortográfica
    private string savePath = Application.dataPath + "/Icons"; // Caminho de salvamento
    public MeshFilter meshFilter;

    private void Start()
    {
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);

        StartCoroutine(CaptureIcons());
    }

    IEnumerator CaptureIcons()
    {
        foreach (var model in itemModels)
        {
            meshFilter.mesh = model;

            yield return new WaitForEndOfFrame(); // Garante que o modelo foi atualizado antes de capturar a imagem

            CaptureImage(model.name);

            yield return new WaitForSeconds(1f); // Espera para garantir que a imagem foi processada
        }
    }

    void CaptureImage(string modelName)
    {
        Debug.Log("Capturando imagem para: " + modelName);

        RenderTexture renderTexture = captureCamera.targetTexture;

        Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        captureCamera.Render();
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        byte[] bytes = texture2D.EncodeToPNG();

        try
        {
            File.WriteAllBytes($"{savePath}/{modelName}.png", bytes);
            Debug.Log("Imagem salva em: " + $"{savePath}/{modelName}.png");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Erro ao salvar imagem: " + e.Message);
        }

        RenderTexture.active = null;
    }
}
