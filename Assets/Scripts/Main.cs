using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public AssetReference spherePrefabRef;
    public RawImage img;
    private void Start()
    {
        Addressables.InstantiateAsync("Assets/Prefabs/Cube.prefab").Completed += (handle) =>
        {
            GameObject prefabObj = handle.Result;
        };
        spherePrefabRef.InstantiateAsync().Completed += (handle) =>
        {
            GameObject prefabObj = handle.Result;
        };
        Addressables.LoadAssetAsync<Texture2D>("Assets/Textures/xiaolan.png").Completed += (handle) =>
        {
            Texture2D tex2D = handle.Result;
            img.texture = tex2D;
            img.GetComponent<RectTransform>().sizeDelta = new Vector2(tex2D.width, tex2D.height);
        };
    }
}
