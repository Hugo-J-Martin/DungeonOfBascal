using UnityEngine;

public class PlayerMeshBob : MonoBehaviour
{
    public float amplitude = 0.05f;  // How high the bob goes
    public float frequency = 2f;     // How fast it bobs

    private Vector3 _startPos;

    void Start()
    {
        _startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = _startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = new Vector3(_startPos.x, newY, _startPos.z);
    }
}
