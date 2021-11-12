#region

using System;

#endregion


namespace OMS.Water.Bootstrapper
{
    public interface IPackage
    {
        string PackageId { get; }

        EPackageState State { get; set; }

        string ProductCode { get; }

        bool PerMachine { get; }

        Version Version { get; }

        IPackage RelatedPackage { get; set; }

        string PackageName { get; }

        string InstallHeader { get; }

        string UninstallHeader { get; }

        string InstallDescription { get; }

        string UninstallDescription { get; }
    }
}
