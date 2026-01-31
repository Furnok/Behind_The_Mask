using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class S_UISelectable : MonoBehaviour
{
    [TabGroup("Settings")]
    [Title("Colors")]
    [SerializeField] private Color32 colorMouseEnter = new(200, 200, 200, 255);

    [TabGroup("Settings")]
    [SerializeField] private Color32 colorMouseDown = new(150, 150, 150, 255);

    [TabGroup("References")]
    //[Title("Audio")]
    //[SerializeField] private EventReference uiClick;

    //[TabGroup("References")]
    //[SerializeField] private EventReference uiHover;

    [TabGroup("References")]
    [Title("Images")]
    [SerializeField] private List<Image> images;

    [TabGroup("Outputs")]
    [SerializeField] private RSO_Navigation rsoNavigation;

    [TabGroup("Outputs")]
    [SerializeField] private SSO_ButtonTransitionTime ssoButtonTransitionTime;

    private List<Color32> colorsBase = new();

    private bool mouseOver = false;
    private bool isPressed = false;
    private bool isSelected = false;

    private List<Tween> colorsTween = new();

    private void OnEnable()
    {
        foreach (Image img in images)
        {
            colorsBase.Add(img.color);
        }
    }

    private void OnDisable()
    {
        ResetColorsToDefault(true);

        mouseOver = false;
        isPressed = false;
        isSelected = false;
    }

    public void MouseEnter(Selectable uiElement)
    {
        if (uiElement.interactable)
        {
            //RuntimeManager.PlayOneShot(uiHover);

            if (!isPressed) PlayColorTransition(colorMouseEnter);

            mouseOver = true;
        }
    }

    public void MouseExit(Selectable uiElement)
    {
        if (uiElement.interactable)
        {
            //RuntimeManager.PlayOneShot(uiHover);

            if (!isPressed) ResetColorsToDefault(false);

            mouseOver = false;
        }
    }


    public void MouseDown(Selectable uiElement)
    {
        if (uiElement.interactable)
        {
            PlayColorTransition(colorMouseDown);

            isPressed = true;
        }
    }

    public void MouseUp(Selectable uiElement)
    {
        if (uiElement.interactable)
        {
            if (mouseOver) PlayColorTransition(colorMouseEnter);
            else ResetColorsToDefault(false);

            isPressed = false;
        }
    }

    public void Selected(Selectable uiElement)
    {
        if (uiElement.interactable && Gamepad.current != null)
        {
            //RuntimeManager.PlayOneShot(uiHover);

            PlayColorTransition(colorMouseEnter);
            rsoNavigation.Value.selectableFocus = uiElement;

            isSelected = true;
        }
    }

    public void Unselected(Selectable uiElement)
    {
        if (uiElement.interactable && Gamepad.current != null) ResetColorsToDefault(false);
        else if (isSelected)
        {
            ResetColorsToDefault(false);

            isSelected = false;
        }
    }

    public void Clicked(Selectable uiElement)
    {
        if (uiElement.interactable)
        {
            //RuntimeManager.PlayOneShot(uiClick);

            if (Gamepad.current != null)
            {
                rsoNavigation.Value.selectablePressOld = rsoNavigation.Value.selectablePress;
                rsoNavigation.Value.selectablePress = uiElement;
            }
        }
    }

    public void ClickedNotAudio(Selectable uiElement)
    {
        if (uiElement.interactable)
        {
            if (Gamepad.current != null)
            {
                rsoNavigation.Value.selectablePressOld = rsoNavigation.Value.selectablePress;
                rsoNavigation.Value.selectablePress = uiElement;
            }
        }
    }

    public void PlayAudio(Selectable uiElement)
    {
        //if (uiElement.interactable) RuntimeManager.PlayOneShot(uiClick);
    }

    public void ClickedWindow(Selectable uiElement)
    {
        rsoNavigation.Value.selectablePressOldWindow = uiElement;
    }

    private void PlayColorTransition(Color32 targetColor)
    {
        foreach (Tween tween in colorsTween)
        {
            tween?.Kill();
        }

        Sequence seq = DOTween.Sequence().SetUpdate(true);

        for (int i = 0; i < images.Count; i++)
        {
            seq.Join(images[i].DOColor(targetColor, ssoButtonTransitionTime.Value.time).SetEase(Ease.Linear));
        }

        colorsTween.Add(seq);

        seq.Play();
    }

    private void ResetColorsToDefault(bool instant)
    {
        if (colorsTween != null)
        {
            foreach (var tween in colorsTween) tween?.Kill();
            colorsTween.Clear();
        }

        Sequence seq = DOTween.Sequence().SetUpdate(true);

        for (int i = 0; i < images.Count; i++)
        {
            seq.Join(images[i].DOColor(colorsBase[i], instant ? 0 : ssoButtonTransitionTime.Value.time).SetEase(Ease.Linear));
        }

        colorsTween.Add(seq);
        seq.Play();
    }
}