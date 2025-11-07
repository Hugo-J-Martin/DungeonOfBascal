using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _playerModel;
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _loseScreen;
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] PlayerInventory _playerInventory;
    [SerializeField] Canvas _playerHealthCanvas;
    private PlayerInputController _playerInputController;
    public Transform cameraTransform;
    public float rotationSpeed = 10f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 0.3f;
    [SerializeField] private Animator animator;
    [SerializeField] public AudioClip spellSfx;
    private AudioSource audioSource;

    private float _nextShootTime = 0f;
    

    private void Awake()
    {
        _playerInputController = GetComponent<PlayerInputController>();
        audioSource = GetComponent<AudioSource>(); 
    }

    private void Update()
    {
        /*Vector3 positionChange = new Vector3(
            _playerInputController.MovementInputVector.x,
            0,
            _playerInputController.MovementInputVector.y)
            * Time.deltaTime
            * _speed;
        positionChange.Rotatio(cameraTransform.eulerAngles.y);
        transform.position += positionChange;
        */
        // Get input (example: from Unity’s new Input System or old Input.GetAxis)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical);

        if (direction.magnitude >= 0.1f)
        {
            direction.Normalize();
            // Get target rotation based on camera direction
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg 
                                + cameraTransform.eulerAngles.y;
            
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

            // Smoothly rotate player toward that direction
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move player forward relative to the camera
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            transform.Translate(moveDir * 5f * Time.deltaTime, Space.World);
           
        }
        _playerModel.transform.eulerAngles = new Vector3(0f, cameraTransform.eulerAngles.y, 0f);
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }
        _playerHealthCanvas.transform.LookAt(cameraTransform);
        if (Input.GetMouseButtonDown(0) && Time.time >= _nextShootTime)
        {
            PlaySFX(spellSfx);
            animator.SetTrigger("CastSpell");
            Invoke("ShootProjectile", .75f);
            
            _nextShootTime = Time.time + shootCooldown;
            
        }
    }

    private void ShootProjectile()
    {
        if (projectilePrefab == null || firePoint == null) return;

        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        
    }
    public void UpdateWinScreen()
    { 
        bool isWin = _playerInventory.NumberOfCollectables > 13;
        //if(isWin){SceneManager.LoadScene("MainMenu");}
        _winScreen.SetActive(isWin);
        Time.timeScale = isWin ? 0 : 1;
    }

    public void PauseGame()
    {
            _pauseScreen.SetActive(true);
            Time.timeScale = 0;
    }
    

    private void OnDestroy()
    {
        if (gameObject != null && _loseScreen != null)
        {
            _loseScreen.SetActive(true);
            Time.timeScale = 0;
        }

    }
    void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}
