/// <summary>
///   Describes why a bundle or packaged is being resumed.
/// </summary>
public enum EResumeType
{
    None,

    /// <summary>
    ///   Resume information exists but is invalid.
    /// </summary>
    Invalid,

    /// <summary>
    ///   The bundle was re-launched after an unexpected interruption.
    /// </summary>
    Interrupted,

    /// <summary>
    ///   A reboot is pending.
    /// </summary>
    RebootPending,

    /// <summary>
    ///   The bundle was re-launched after a reboot.
    /// </summary>
    Reboot,

    /// <summary>
    ///   The bundle was re-launched after being suspended.
    /// </summary>
    Suspend,

    /// <summary>
    ///   The bundle was launched from Add/Remove Programs.
    /// </summary>
    Arp,
}
