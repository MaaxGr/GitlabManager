namespace GitlabManager.Services.DI
{
    /// <summary>
    /// Services, that let's your create dependency instances based on class name
    /// Needed e.g for WindowOpener-Service
    /// </summary>
    public interface IDynamicDependencyProvider
    {

        public T GetInstance<T>();
        
    }
}