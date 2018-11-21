using System.ServiceProcess;

namespace ExampleService
{
    /// <summary>
    /// Describes the funcionality required by a service to support session change notifications.
    /// </summary>
    public interface ISessionAwareService
    {
        /// <summary>
        /// Executes when a change event is received from a Terminal Server session. 
        /// </summary>
        /// <param name="changeDescription">A structure that identifies the change type.</param>
        void OnSessionChange(SessionChangeDescription changeDescription);
    }
}
