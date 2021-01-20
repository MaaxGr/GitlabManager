namespace GitlabManager.Services.DI
{
    public interface IDynamicDependencyProvider
    {

        public T GetInstance<T>();
        
    }
}