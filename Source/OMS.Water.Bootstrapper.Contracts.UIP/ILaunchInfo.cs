namespace OMS.Water.Bootstrapper
{
    public interface ILaunchInfo
    {
        ELaunchAction Action { get; }

        string[] CommandLineArgs { get; }

        EDisplay Display { get; }

        ERelationType Relation { get; }

        ERestartType Restart { get; }

        EResumeType Resume { get; }
    }
}
