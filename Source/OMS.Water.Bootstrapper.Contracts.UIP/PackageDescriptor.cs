namespace OMS.Water.Bootstrapper
{
    public class PackageDescriptor : IPackageDescriptor
    {
        public PackageDescriptor(string packageId, string packageName, string installHeader, string installDescription, string uninstallHeader, string uninstallDescription)
        {
            PackageId = packageId;
            PackageName = packageName;
            InstallHeader = installHeader;
            UninstallHeader = uninstallHeader;
            InstallDescription = installDescription;
            UninstallDescription = uninstallDescription;
        }


        #region IPackageDescriptor Members

        public string PackageId { get; private set; }

        public string PackageName { get; private set; }

        public string InstallHeader { get; private set; }

        public string UninstallHeader { get; private set; }

        public string InstallDescription { get; private set; }

        public string UninstallDescription { get; private set; }

        #endregion
    }
}
