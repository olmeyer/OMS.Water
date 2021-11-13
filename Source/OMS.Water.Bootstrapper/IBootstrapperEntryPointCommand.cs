#region

using Systecs.Framework;
using Systecs.Framework.FrameworkApplication;

#endregion


namespace OMS.Water.Bootstrapper
{
    public interface IBootstrapperEntryPointCommand : IEntryPointCommand
    {
        IFrameworkApplication Application { get; set; }
    }
}
