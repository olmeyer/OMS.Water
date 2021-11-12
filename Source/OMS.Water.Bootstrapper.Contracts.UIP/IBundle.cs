#region

using System;

#endregion


namespace OMS.Water.Bootstrapper
{
    public interface IBundle
    {
        bool PerMachine { get; }

        string ProductCode { get; }

        Version Version { get; }

        string Tag { get; }
    }
}
