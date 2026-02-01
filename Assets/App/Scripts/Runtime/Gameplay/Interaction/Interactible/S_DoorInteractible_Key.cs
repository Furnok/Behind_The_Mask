using DG.Tweening;
using UnityEngine;

public class S_DoorInteractible_Key : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] int _priority = 0;
    [SerializeField] bool _isInteractable = true;
    [SerializeField] Vector3 _openOffset = new Vector3(2f, 0f, 0f);
    [SerializeField] float _openDuration = 0.5f;
    [SerializeField] Ease _ease = Ease.OutCubic;
    [SerializeField] private S_ClassAudio audioDoor;

    [Header("References")]
    [SerializeField] GameObject _doorVisuals;

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] RSO_HaveKey _haveKey;
    [SerializeField] private RSE_OnUIError rseOnUIError;
    [SerializeField] private RSE_OnUIInterract rseOnUIInterract;
    [SerializeField] private RSE_PlayAudio rsePlayAudio;

    public int Priority => _priority;
    public Transform Transform => transform;
    public bool IsInteractable => _isInteractable;

    Vector3 _closedLocalPos;
    Vector3 _openLocalPos;

    Tween _currentTween;
    bool _isOpen = false;

    void Awake()
    {
        _closedLocalPos = _doorVisuals.transform.localPosition;
        _openLocalPos = _closedLocalPos + _openOffset;
    }

    public void Interact()
    {
        if (_haveKey.Value == true)
        {
            // Logic to open the door and animation
            Debug.Log("Door opened with key ");
            //_doorVisuals.SetActive(false); // Simulate door opening by disabling visuals for now

            Open();
        }
        else
        {
            rseOnUIError.Call("You don't have the Key!", Color.red);
        }
    }

    public void Open()
    {
        if (_isOpen) return;

        _isOpen = true;
        _isInteractable = false;
        rsePlayAudio.Call(audioDoor);
        KillTween();

        _currentTween = _doorVisuals.transform
            .DOLocalMove(_openLocalPos, _openDuration)
            .SetEase(_ease);

        rseOnUIInterract.Call(false);
    }

    public void Close()
    {
        if (!_isOpen) return;

        _isOpen = false;
        _isInteractable = true;
        rsePlayAudio.Call(audioDoor);
        KillTween();

        _currentTween = _doorVisuals.transform
            .DOLocalMove(_closedLocalPos, _openDuration)
            .SetEase(_ease);
    }

    void KillTween()
    {
        if (_currentTween != null && _currentTween.IsActive())
            _currentTween.Kill();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Detect"))
        {
            Close();
        }
    }
}