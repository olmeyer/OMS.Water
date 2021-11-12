namespace OMS.Water.Bootstrapper
{
    internal class LaunchInfo : ILaunchInfo
    {
        #region ILaunchInfo Members

        public ELaunchAction Action { get; set; }

        public string[] CommandLineArgs { get; set; }

        public EDisplay Display { get; set; }

        public ERelationType Relation { get; set; }

        public ERestartType Restart { get; set; }

        public EResumeType Resume { get; set; }

        #endregion
    }
}
