using UnityEngine;

public class S_EnemyPatrol : MonoBehaviour
{
    [SerializeField] Transform[] _points;
    [Header("Mode")]
    [SerializeField] bool _pingPong = true;

    int _index = 0;
    int _dir = 1; // 1 => forward, -1 => reverse

    public bool HasPoints => _points != null && _points.Length > 0;

    public Vector3 GetCurrentPoint()
    {
        if (!HasPoints) return transform.position;
        return _points[_index].position;
    }

    public Vector3 GetNextDestinationPointOnly()
    {
        if (!HasPoints) return transform.position;

        if (_pingPong)
        {
            _index += _dir;

            if (_index >= _points.Length)
            {
                _dir = -1;
                _index = Mathf.Max(_points.Length - 2, 0);
            }
            else if (_index < 0)
            {
                _dir = 1;
                _index = Mathf.Min(1, _points.Length - 1);
            }
        }
        else
        {
            _index = (_index + 1) % _points.Length;
        }

        return _points[_index].position;
    }

    public void SetClosestAsCurrent(Vector3 from)
    {
        if (!HasPoints) return;

        int best = 0;
        float bestD = float.PositiveInfinity;

        for (int i = 0; i < _points.Length; i++)
        {
            float d = (from - _points[i].position).sqrMagnitude;
            if (d < bestD) { bestD = d; best = i; }
        }

        _index = best;

        _dir = 1;
    }

    public void ResetIfNeeded() { }
}