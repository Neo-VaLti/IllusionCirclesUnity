using UnityEngine;
using System.Collections;

public class TextureMover : MonoBehaviour {

    public float scrollSpeed;
    public Material material;

    public void MoveTexture()
    {
        float offset = -Time.time * scrollSpeed;
        material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
    void FixedUpdate()
    {
        float offset = Time.time * scrollSpeed;
        material.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }
}
