using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ExampleService
{
    /// <summary>
    /// A service which is started on construction by a factory delegate and stopped by disposing.
    /// </summary>
    public class DisposableService<T> : ServiceBase
        where T : IDisposable
    {
        private readonly Func<string[], ServiceBase, T> _factory;

        /// <summary>
        /// Create the disposable service.
        /// </summary>
        /// <param name="serviceName">The service name.</param>
        /// <param name="factory">A factory delegate for creating the service with the givenn args and the service base instance.</param>
        public DisposableService(string serviceName, Func<string[], ServiceBase, T> factory)
        {
            ServiceName = serviceName;
            _factory = factory;
            CanPauseAndContinue = typeof(IPauseableService).IsAssignableFrom(typeof(T));
            CanShutdown = typeof(IShutdownableService).IsAssignableFrom(typeof(T));
            CanHandlePowerEvent = typeof(IPowerStatusAwareService).IsAssignableFrom(typeof(T));
        }

        /// <summary>
        /// Create the disposable service.
        /// </summary>
        /// <param name="serviceName">The service name.</param>
        /// <param name="create">A factory delegate for creating the service with the givenn args.</param>
        public DisposableService(string serviceName, Func<string[], T> create)
            : this(serviceName, (args, _) => create(args))
        {
        }

        /// <summary>
        /// The service instance. This gets set when the service is started, and cleared when the service is stopped.
        /// </summary>
        public T Service { get; private set; }

        /// <summary>
        /// Executes when a Start command is sent to the service by the 
        /// Service Control Manager (SCM) or when the operating system starts
        /// (for a service that starts automatically). Specifies actions to take
        /// when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            Service = _factory(args, this);
        }

        /// <summary>
        /// Executes when a Stop command is sent to the service by the Service
        /// Control Manager (SCM). Specifies actions to take when a service
        /// stops running.
        /// </summary>
        protected override void OnStop()
        {
            Service.Dispose();
            Service = default(T);
        }

        /// <summary>
        /// Executes when the Service Control Manager (SCM) passes a custom
        /// command to the service. Specifies actions to take when a command
        /// with the specified parameter value occurs.
        /// 
        /// To support this feature the class must implement <see cref="ICommandableService" />.
        /// </summary>
        /// <param name="command">The command message sent to the service.</param>
        protected override void OnCustomCommand(int command)
        {
            if (Service is ICommandableService)
                ((ICommandableService)Service).OnCustomCommand(command);
        }

        /// <summary>
        /// Executes when a Pause command is sent to the service by the Service
        /// Control Manager (SCM). Specifies actions to take when a service
        /// pauses.
        /// 
        /// To use this feature the class must implement <see cref="IPauseableService" />.
        /// </summary>
        protected override void OnPause()
        {
            if (Service is IPauseableService)
                ((IPauseableService)Service).OnPause();
        }

        /// <summary>
        /// Runs when a Continue command is sent to the service by the Service
        /// Control Manager (SCM). Specifies actions to take when a service
        /// resumes normal functioning after being paused.
        /// 
        /// To use this feature the class must implement <see cref="IPauseableService" />.
        /// </summary>
        protected override void OnContinue()
        {
            if (Service is IPauseableService)
                ((IPauseableService)Service).OnContinue();
        }

        /// <summary>
        /// Executes when the system is shutting down. Specifies what should occur
        /// immediately prior to the system shutting down.
        /// 
        /// To use this feature the class must implement <see cref="IShutdownableService" />.
        /// </summary>
        protected override void OnShutdown()
        {
            if (Service is IShutdownableService)
                ((IShutdownableService)Service).OnShutdown();
        }

        /// <summary>
        /// Executes when the computer's power status has changed. This
        /// applies to laptop computers when they go into suspended mode, which
        /// is not the same as a system shutdown.
        /// 
        /// To use this feature the class must implement <see cref="IPowerStatusAwareService" />.
        /// </summary>
        /// <returns>
        /// The needs of your application determine what value to return. For example,
        /// if a QuerySuspend broadcast status is passed, you could cause your application
        /// to reject the query by returning false.
        /// </returns>
        /// <param name="powerStatus">A <see cref="T:System.ServiceProcess.PowerBroadcastStatus" /> that indicates a notification from the system about its power status. </param>
        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            if (Service is IPowerStatusAwareService)
                return ((IPowerStatusAwareService)Service).OnPowerEvent(powerStatus);

            return base.OnPowerEvent(powerStatus);
        }

        /// <summary>
        /// Executes when a change event is received from a Terminal Server session. 
        /// 
        /// To use this feature the class must implement <see cref="ISessionAwareService" />.
        /// </summary>
        /// <param name="changeDescription">A structure that identifies the change type.</param>
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            if (Service is ISessionAwareService)
                ((ISessionAwareService)Service).OnSessionChange(changeDescription);
        }
    }

    /// <summary>
    /// A convenience class for creating disposable services.
    /// </summary>
    public class DisposableService
    {
        /// <summary>
        /// A convenience method for creating disposable services.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="serviceName">The service name.</param>
        /// <param name="create">A factory delegate for creating the service with the givenn args.</param>
        /// <returns>A disposable service.</returns>
        public static DisposableService<T> Create<T>(string serviceName, Func<string[], T> create) where T : IDisposable
        {
            return new DisposableService<T>(serviceName, create);
        }
    }
}
