using Sirenix.OdinInspector;
using UnityEngine;

public class S_PlayerMask : MonoBehaviour
{
    //[Header("Settings")]

    //[Header("References")]

    [Header("Inputs")]
    [SerializeField] RSE_OnPickUpMask _onPickUpMask;
    [SerializeField] RSE_OnMask1Input _onMask1Input;
    [SerializeField] RSE_OnMask2Input _onMask2Input;
    [SerializeField] RSE_OnMask3Input _onMask3Input;

    [Header("Outputs")]
    [SerializeField] RSO_PlayerCurrentMaskUnlocked _playerCurrentMaskUnlocked;
    [SerializeField] RSO_PlayerCurrentMaskEquipped _playerCurrentMaskEquipped;
    [SerializeField] RSE_OnUpdateUIInventory rseOnUpdateUIInventory;

    private void Awake()
    {
        _playerCurrentMaskUnlocked.Value = new S_SerializableDictionary<Mask, bool>();
        _playerCurrentMaskUnlocked.Value.Add(Mask.BlueMask, false);
        _playerCurrentMaskUnlocked.Value.Add(Mask.GreenMask, false);
        _playerCurrentMaskUnlocked.Value.Add(Mask.RedMask, false);

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
        TryEquippedMask(Mask.GreenMask);
    }

    void OnMaskBlueInput()
    {
        TryEquippedMask(Mask.BlueMask);
    }

    void OnMaskRedInput()
    {
        TryEquippedMask(Mask.RedMask);
    }

    void TryEquippedMask(Mask mask)
    {
        if (_playerCurrentMaskUnlocked.Value[mask] == false)
            return;

        if (_playerCurrentMaskEquipped.Value == mask)
        {
            _playerCurrentMaskEquipped.Value = Mask.None;
        }
        else
        {
            _playerCurrentMaskEquipped.Value = mask;
        }
    }
}