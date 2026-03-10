using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class S_PlayerMask : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] S_SerializableDictionary<Mask, GameObject> _masksFrontCam;
    [SerializeField] Transform _maskFinalAnchor;
    [SerializeField] Transform _maskStartLeftAnchor;
    [SerializeField] Transform _maskMidAnchor;
    [SerializeField] float _equipRotateZ = 90f;
    [SerializeField] Transform _maskOutFinalAnchor;
    [SerializeField] Transform _maskOutStartLeftAnchor;
    [SerializeField] Transform _maskOutMidAnchor;
    [SerializeField] float _equipOutRotateZ = 90f;

    [Header("Inputs")]
    [SerializeField] RSE_OnPickUpMask _onPickUpMask;
    [SerializeField] RSE_OnMask1Input _onMask1Input;
    [SerializeField] RSE_OnMask2Input _onMask2Input;
    [SerializeField] RSE_OnMask3Input _onMask3Input;

    [Header("Outputs")]
    [SerializeField] RSO_PlayerCurrentMaskUnlocked _playerCurrentMaskUnlocked;
    [SerializeField] RSO_PlayerCurrentMaskEquipped _playerCurrentMaskEquipped;
    [SerializeField] RSE_OnUpdateUIInventory rseOnUpdateUIInventory;
    [SerializeField] RSE_OnEquippedMaskUI rseOnEquippedMaskUI;
    [SerializeField] SSO_PlayerSettings _playerSettings;

    Coroutine _coroutineMask;

    private void Awake()
    {
        _playerCurrentMaskUnlocked.Value = new S_SerializableDictionary<Mask, bool>();
        _playerCurrentMaskUnlocked.Value.Add(Mask.GreenMask, false);
        _playerCurrentMaskUnlocked.Value.Add(Mask.RedMask, false);
        _playerCurrentMaskUnlocked.Value.Add(Mask.BlueMask, false);

        //AddAllMAsk(); // For testing purposes, add all masks at start

        _playerCurrentMaskEquipped.Value = Mask.None;
    }

    private void OnEnable()
    {
        _onMask1Input.action += OnMaskGreenInput;
        _onMask2Input.action += OnMaskRedInput;
        _onMask3Input.action += OnMaskBlueInput;

        _onPickUpMask.action += AddMask;
    }

    private void OnDisable()
    {
        _onMask1Input.action -= OnMaskGreenInput;
        _onMask2Input.action -= OnMaskRedInput;
        _onMask3Input.action -= OnMaskBlueInput;

        _onPickUpMask.action -= AddMask;

        _playerCurrentMaskEquipped.Value = Mask.None;
        _playerCurrentMaskUnlocked.Value.Clear();
    }

    void AddMask(Mask mask)
    {
        if (mask == Mask.None) return;

        if (!_playerCurrentMaskUnlocked.Value.ContainsKey(mask))
        {
            _playerCurrentMaskUnlocked.Value.Add(mask, true);
        }
        else if (_playerCurrentMaskUnlocked.Value[mask] == false)
        {
            _playerCurrentMaskUnlocked.Value[mask] = true;
        }
        else
        {
            
        }

        rseOnUpdateUIInventory.Call();
    }

    void AddAllMAsk()
    {
        var keys = new Mask[_playerCurrentMaskUnlocked.Value.Count];
        _playerCurrentMaskUnlocked.Value.Keys.CopyTo(keys, 0);
        foreach (var key in keys)
        {
            if (key != Mask.None)
            {
                _playerCurrentMaskUnlocked.Value[key] = true;
            }
        }
    }

    void OnMaskGreenInput()
    {
        TryEquippedMask(Mask.GreenMask, 0);
    }

    void OnMaskRedInput()
    {
        TryEquippedMask(Mask.RedMask, 1);
    }

    void OnMaskBlueInput()
    {
        TryEquippedMask(Mask.BlueMask, 2);
    }

    void TryEquippedMask(Mask mask, int index)
    {
        if (_playerCurrentMaskUnlocked.Value[mask] == false)
            return;

        if (_playerCurrentMaskEquipped.Value == mask)
        {
            if (_coroutineMask != null) return;

            StartCoroutine(MaskUnequippedCoroutine(_playerCurrentMaskEquipped.Value));

            _playerCurrentMaskEquipped.Value = Mask.None;

            rseOnEquippedMaskUI.Call(index);
        }
        else
        {
            if(_coroutineMask == null)
            {
                _coroutineMask = StartCoroutine(MaskEquippedCoroutine(mask));
                rseOnEquippedMaskUI.Call(index);
            }
        }
    }

    void UpdateMaskVisuals(Mask mask)
    {
        foreach (var kvp in _masksFrontCam)
        {
            kvp.Value.SetActive(false);
        }

        if (mask != Mask.None)
        {
            _masksFrontCam[mask].SetActive(true);
        }
    }

    IEnumerator MaskEquippedCoroutine(Mask mask)
    {
        float duration = _playerSettings.Value.MaskDelayToEquipped;

        PlayEquipAnim(mask, duration);

        yield return new WaitForSeconds(duration);

        _playerCurrentMaskEquipped.Value = mask;

        var go = _masksFrontCam[mask];
        go.transform.position = _maskFinalAnchor.position;
        go.transform.rotation = _maskFinalAnchor.rotation;

        _coroutineMask = null;
    }

    void PlayEquipAnim(Mask mask, float duration)
    {
        var go = _masksFrontCam[mask];
        var t = go.transform;

        foreach (var kvp in _masksFrontCam)
        {
            if (kvp.Key == _playerCurrentMaskEquipped.Value)
            {
                StartCoroutine(MaskUnequippedCoroutine(kvp.Key));
            }
            else
            {
                kvp.Value.SetActive(kvp.Key == mask);
            }
        }

        t.DOKill();

        t.localPosition = _maskStartLeftAnchor.localPosition;
        t.localRotation = _maskStartLeftAnchor.localRotation * Quaternion.Euler(0f, _equipRotateZ, 0f);

        Vector3[] path = new Vector3[]
        {
        _maskStartLeftAnchor.localPosition,
        _maskMidAnchor.localPosition,
        _maskFinalAnchor.localPosition
        };

        Sequence s = DOTween.Sequence();
        s.Join(t.DOLocalPath(path, duration, PathType.CatmullRom).SetEase(Ease.OutCubic));
        s.Join(t.DOLocalRotateQuaternion(_maskFinalAnchor.localRotation, duration).SetEase(Ease.OutCubic));
    }

    IEnumerator MaskUnequippedCoroutine(Mask mask)
    {
        float duration = _playerSettings.Value.MaskDelayToUnequipped;
        PlayUnequippedMask(mask, duration);
        yield return new WaitForSeconds(duration);
        var go = _masksFrontCam[mask];
        go.transform.position = _maskOutFinalAnchor.position;
        go.transform.rotation = _maskOutFinalAnchor.rotation;
        go.SetActive(false);
    }

    void PlayUnequippedMask(Mask mask, float duration)
    {
        var go = _masksFrontCam[mask];
        var t = go.transform;
        t.DOKill();
        Vector3[] path = new Vector3[]
        {
        _maskOutStartLeftAnchor.localPosition,
        _maskOutMidAnchor.localPosition,
        _maskOutFinalAnchor.localPosition
        };
        Sequence s = DOTween.Sequence();
        s.Join(t.DOLocalPath(path, duration, PathType.CatmullRom).SetEase(Ease.InCubic));
        s.Join(t.DOLocalRotateQuaternion(_maskOutFinalAnchor.localRotation * Quaternion.Euler(0f, _equipOutRotateZ, 0f), duration).SetEase(Ease.InCubic));
    }
}