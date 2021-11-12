namespace OMS.Water.Bootstrapper
{
    public enum EPackageState
    {
        Initializing,
        DetectedAbsent,
        DetectedPresent,
        DetectedNewer,
        Applying,
        Applied,
        Failed,
    }
}
