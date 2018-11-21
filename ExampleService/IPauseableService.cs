namespace ExampleService
{
    /// <summary>
    /// This interface describes the functionality to implement pause/continue functionality in a service.
    /// </summary>
    public interface IPauseableService
    {
        /// <summary>
        /// Invoked when a pause request is issued by the service.
        /// </summary>
        void OnPause();

        /// <summary>
        /// Invoked when a continue request is sent to a service.
        /// </summary>
        void OnContinue();
    }
}
