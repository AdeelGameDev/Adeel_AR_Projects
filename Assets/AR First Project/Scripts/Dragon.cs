using UnityEngine;

public class Dragon : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    private DragonSO dragonSO;


    public void UpdateSkin(Material material)
    {
        skinnedMeshRenderer.material = material;
    }

    public void PlayScreamSound()
    {
        AudioSource.PlayClipAtPoint(dragonSO.roarSound, transform.localPosition);
    }

    public void SetDragonSO(DragonSO dragonSO)
    {
        this.dragonSO = dragonSO;
    }
}
