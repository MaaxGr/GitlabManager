namespace GitlabManager.Services.DI
{
    /// <summary>
    /// Services, that let's your create dependency instances based on class name
    /// Needed e.g for WindowOpener-Service
    /// </summary>
    public interface IDynamicDependencyProvider
    {

        /// <summary>
        /// Get Dependency Instance of specified type T
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <returns></returns>
        public T GetInstance<T>();
        
    }
}