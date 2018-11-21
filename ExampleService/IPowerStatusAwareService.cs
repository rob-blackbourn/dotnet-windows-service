using System.ServiceProcess;

namespace ExampleService
{
    /// <summary>
    /// Describes the functionality required to support power status events sent to a service.
    /// </summary>
    public interface IPowerStatusAwareService
    {
        /// <summary>
        /// Executes when the computer's power status has changed. This
        /// applies to laptop computers when they go into suspended mode, which
        /// is not the same as a system shutdown.
        /// </summary>
        /// <param name="powerStatus">A <see cref="T:System.ServiceProcess.PowerBroadcastStatus" /> that indicates a notification from the system about its power status. </param>
        /// <returns>
        /// The needs of your application determine what value to return. For example,
        /// if a QuerySuspend broadcast status is passed, you could cause your application
        /// to reject the query by returning false.
        /// </returns>
        bool OnPowerEvent(PowerBroadcastStatus powerStatus);
    }
}
