using UnityEngine;

public class S_EnemyPerception : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform _eyes;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] LayerMask _obstacleLayer;

    [Header("Settings")]
    [SerializeField] float _range = 8f;

    [Tooltip("Total Angle(example: 60 = 30° left + 30° right)")]
    [SerializeField] float _coneAngle = 60f;

    [SerializeField] float _sightRadius = 0.25f;

    [SerializeField] float _targetHeightOffset = 1.2f;

    [Header("Debug")]
    [SerializeField] bool _drawGizmos = true;
    [SerializeField] bool _drawInPlayOnly = true;
    [SerializeField] int _coneArcSegments = 16;

    public bool PlayerInCone { get; private set; }
    public bool PlayerInObserveRadius { get; private set; }
    public Vector3 PlayerPosition { get; private set; }

    Transform _player;

    // Debug runtime
    bool _debugBlocked;
    Vector3 _debugCastOrigin;
    Vector3 _debugCastDir;
    float _debugCastDist;
    Vector3 _debugHitPoint;
    bool _debugHasHit;

    public void SetPlayer(Transform player) => _player = player;

    private void Update()
    {
        Evaluate();
    }

    void Evaluate()
    {
        PlayerInCone = false;
        PlayerInObserveRadius = false;
        _debugBlocked = false;
        _debugHasHit = false;

        if (_player == null || _eyes == null) return;

        PlayerPosition = _player.position;

        Vector3 origin = _eyes.position;

        Vector3 target = _player.position;
        target.y += _targetHeightOffset;

        Vector3 toPlayer = target - origin;
        float dist = toPlayer.magnitude;

        PlayerInObserveRadius = dist <= _range;

        _debugCastOrigin = origin;
        _debugCastDist = dist;

        if (!PlayerInObserveRadius || dist <= 0.0001f)
        {
            _debugCastDir = _eyes.forward;
            return;
        }

        Vector3 dir = toPlayer / dist;
        _debugCastDir = dir;

        float angle = Vector3.Angle(_eyes.forward, dir);
        if (angle > _coneAngle * 0.5f)
        {
            return;
        }

        if (Physics.SphereCast(origin, _sightRadius, dir, out RaycastHit hit, dist, _obstacleLayer, QueryTriggerInteraction.Ignore))
        {
            _debugBlocked = true;
            _debugHasHit = true;
            _debugHitPoint = hit.point;
            PlayerInCone = false;
            return;
        }

        PlayerInCone = true;
    }

    private void OnDrawGizmos()
    {
        if (!_drawGizmos) return;
        if (_drawInPlayOnly && !Application.isPlaying) return;

        Transform eyes = _eyes != null ? _eyes : transform;
        Vector3 origin = eyes.position;

        Gizmos.color = new Color(1f, 1f, 1f, 0.12f);
        Gizmos.DrawSphere(origin, _range);
        Gizmos.color = new Color(1f, 1f, 1f, 0.35f);
        Gizmos.DrawWireSphere(origin, _range);

        float half = _coneAngle * 0.5f;
        Vector3 forward = eyes.forward;
        Vector3 leftDir = Quaternion.Euler(0f, -half, 0f) * forward;
        Vector3 rightDir = Quaternion.Euler(0f, half, 0f) * forward;

        Gizmos.color = new Color(1f, 1f, 0f, 0.8f);
        Gizmos.DrawLine(origin, origin + leftDir.normalized * _range);
        Gizmos.DrawLine(origin, origin + rightDir.normalized * _range);

        if (_coneArcSegments < 2) return;
        Vector3 prev = origin + leftDir.normalized * _range;

        for (int i = 1; i <= _coneArcSegments; i++)
        {
            float t = (float)i / _coneArcSegments;
            float ang = Mathf.Lerp(-half, half, t);
            Vector3 dir = Quaternion.Euler(0f, ang, 0f) * forward;
            Vector3 p = origin + dir.normalized * _range;

            Gizmos.DrawLine(prev, p);
            prev = p;
        }

        if (!Application.isPlaying) return;

        Color castColor =
            PlayerInCone ? new Color(0f, 1f, 0f, 0.9f) :
            _debugBlocked ? new Color(1f, 0f, 0f, 0.9f) :
            new Color(0.6f, 0.6f, 0.6f, 0.7f);

        Gizmos.color = castColor;

        Vector3 end = _debugCastOrigin + _debugCastDir.normalized * Mathf.Min(_debugCastDist, _range);
        Gizmos.DrawLine(_debugCastOrigin, end);

        Gizmos.DrawWireSphere(end, _sightRadius);

        if (_debugHasHit)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 1f);
            Gizmos.DrawSphere(_debugHitPoint, 0.08f);
            Gizmos.DrawWireSphere(_debugHitPoint, _sightRadius);
        }
    }
}