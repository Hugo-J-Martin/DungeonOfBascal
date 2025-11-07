using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] public AudioClip pickupSfx;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>(); 
    }
    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        PlayerInventory playerInventory = other.GetComponentInParent<PlayerInventory>();
        PlaySFX(pickupSfx);
        if (playerInventory != null)
        {
            playerInventory.CollectableCollected();
            gameObject.SetActive(false);
        }
    }
    
    void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}
