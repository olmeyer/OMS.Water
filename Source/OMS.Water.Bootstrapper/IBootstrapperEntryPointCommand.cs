#region

using Systecs.Framework;

#endregion


namespace OMS.Water.Bootstrapper
{
    public interface IBootstrapperEntryPointCommand : IEntryPointCommand
    {
        IFrameworkApplication Application { get; set; }
    }
}
