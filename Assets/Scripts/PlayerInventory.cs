using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfCollectables { get; private set; }

    public UnityEvent<PlayerInventory> onCollectableCollected;
    public void CollectableCollected()
    {
        NumberOfCollectables++;
        onCollectableCollected.Invoke(this);
    }
}
