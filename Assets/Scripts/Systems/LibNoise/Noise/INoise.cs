using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INoise
{
    float Frequency { get; set; }

    float Lacunarity { get; set; }

    float Persistence { get; set; }

    int Octaves { get; set; }

    int Seed { get; set; }

    float GetValue(float x, float y);

    IEnumerable<float> GetBlock(Vector2 start, float sampleSize, Vector2Int size);

    Texture2D GetTexture(Vector2 start, float sampleSize, Vector2Int size);
}