namespace OMS.Water.Bootstrapper
{
    public interface IPackageDescriptor
    {
        string PackageId { get; }

        string PackageName { get; }

        string InstallHeader { get; }

        string UninstallHeader { get; }

        string InstallDescription { get; }

        string UninstallDescription { get; }

    }
}
