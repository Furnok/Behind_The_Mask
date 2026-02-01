using UnityEngine;

public class S_EnemyTextWorld : MonoBehaviour
{
    [SerializeField] bool followCam = true;
    [SerializeField] float maxDistance = 10f;

    MeshRenderer _meshRenderer;
    Transform _cam;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _cam = Camera.main != null ? Camera.main.transform : null;
    }

    private void Update()
    {
        if (_cam == null)
        {
            if (Camera.main != null) _cam = Camera.main.transform;
            return;
        }

        float distance = Vector3.Distance(_cam.position, transform.position);
        bool show = distance <= maxDistance;

        if (_meshRenderer != null) _meshRenderer.enabled = show;
    }

    private void LateUpdate()
    {
        if (!followCam) return;
        if (_cam == null) return;

        Vector3 dir = transform.position - _cam.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;

        transform.rotation = Quaternion.LookRotation(dir);
    }
}