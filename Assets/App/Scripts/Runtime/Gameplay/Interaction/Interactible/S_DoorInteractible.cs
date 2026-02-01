using DG.Tweening;
using UnityEngine;

public class S_DoorInteractible : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] int _priority = 0;
    [SerializeField] bool _isInteractable = true;
    [SerializeField] Mask _requiredMask;
    [SerializeField] Vector3 _openOffset = new Vector3(2f, 0f, 0f);
    [SerializeField] float _openDuration = 0.5f;
    [SerializeField] Ease _ease = Ease.OutCubic;

    [Header("References")]
    [SerializeField] GameObject _doorVisuals;

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] RSO_PlayerCurrentMaskEquipped _playerCurrentMaskEquipped;
    [SerializeField] private RSE_OnUIError rseOnUIError;

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
        if(_playerCurrentMaskEquipped.Value == _requiredMask)
        {
            // Logic to open the door and animation
            Debug.Log("Door opened with mask: " + _requiredMask.ToString());
            //_doorVisuals.SetActive(false); // Simulate door opening by disabling visuals for now

            Open();

        }
        else
        {
            rseOnUIError.Call("You don't have the right Mask!", Color.red);
        }
    }

    public void Open()
    {
        if (_isOpen) return;

        _isOpen = true;
        KillTween();

        _currentTween = _doorVisuals.transform
            .DOLocalMove(_openLocalPos, _openDuration)
            .SetEase(_ease);
    }

    public void Close()
    {
        if (!_isOpen) return;

        _isOpen = false;
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