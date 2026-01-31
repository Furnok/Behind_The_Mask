using Sirenix.OdinInspector;
using System;

[Serializable]
public class S_ClassTimeFloat
{
    [OnValueChanged(nameof(ClampTime))]
    [SuffixLabel("s", Overlay = true)]
    public float time;

    private void ClampTime()
    {
        time = Math.Max(0f, time);
    }
}