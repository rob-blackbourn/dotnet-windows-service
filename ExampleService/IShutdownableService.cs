namespace ExampleService
{
    /// <summary>
    /// Describes the functionality required to support service shutdown requests.
    /// </summary>
    public interface IShutdownableService
    {
        /// <summary>
        /// Executes when the system is shutting down. Specifies what should occur
        /// immediately prior to the system shutting down.
        /// </summary>
        void OnShutdown();
    }
}
