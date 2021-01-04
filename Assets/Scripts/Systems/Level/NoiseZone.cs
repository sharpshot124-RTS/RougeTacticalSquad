using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise.Generator;

[CreateAssetMenu(fileName = "New_NoiseZone", menuName = "Custom/Levels/New Noise Zone")]
public class NoiseZone : ScriptableObject, IZone
{
    [SerializeField] Vector2Int dataSize;

    [SerializeField] Object generator;
    public INoise Generator { get => generator as INoise; }

    [SerializeField] List<Object> _plots;
    public List<ILandPlot> Plots => _plots.ConvertAll((p) => p as ILandPlot);

    public string Name { get => name; set => name = value; }

    public ILandPlot GetPlot(int x, int y)
    {        
        var result = _plots[Mathf.FloorToInt((_plots.Count - 1) * Generator.GetValue(x / 10, y / 10))] as ILandPlot;
        result.Transform = new Vector3Int(x, y, Random.Range(0, 4));

        return result;
    }

    [ContextMenu("Generate Texture")]
    public void GenerateTexture()
    {
        var tex = Generator.GetTexture(Vector2.zero, .1f, dataSize);
        Generator.Seed = Random.Range(0, 9999);

        byte[] _bytes = tex.EncodeToPNG();
        string path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/Image.png");
        System.IO.File.WriteAllBytes("D:/Projects/Mini RTS/rougetacticalsquad/" + path, _bytes);
        Debug.Log(_bytes.Length / 1024 + "Kb was saved as: " + path);
    }


}