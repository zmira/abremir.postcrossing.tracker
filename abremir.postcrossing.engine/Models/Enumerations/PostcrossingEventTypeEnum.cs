using System;

namespace abremir.postcrossing.engine.Models.Enumerations
{
    [Flags]
    public enum PostcrossingEventTypeEnum
    {
        Unknown = 0,
        Register = 1 << 0,
        Send = 1 << 1,
        Upload = 1 << 2,
        SignUp = 1 << 3,
        All = ~0
    }
}
