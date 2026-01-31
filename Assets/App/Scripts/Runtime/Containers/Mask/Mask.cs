using System;

[Flags]
public enum Mask
{
    None = 1 << 0,
    GreenMask = 1 << 1,
    RedMask = 1 << 2,
    BlueMask = 1 << 3,
}
