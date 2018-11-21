namespace ExampleService
{
    /// <summary>
    /// AN interface to provide support for custom command events in a service.
    /// </summary>
    public interface ICommandableService
    {
        /// <summary>
        /// Invoked when a custom command is sent to a service.
        /// </summary>
        /// <param name="command">The command sent to the service.</param>
        void OnCustomCommand(int command);
    }
}
