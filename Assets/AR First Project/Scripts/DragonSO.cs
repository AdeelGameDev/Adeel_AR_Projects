using UnityEngine;

[CreateAssetMenu()]
public class DragonSO : ScriptableObject
{
    public string dragonName;
    public GameObject dragonObject;
    public AudioClip roarSound;
    public Material[] skins;
}
