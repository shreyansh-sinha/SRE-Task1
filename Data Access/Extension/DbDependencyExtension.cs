using StudentAPI.Adapter;
using StudentAPI.Interface;
using StudentAPI.Model;
using StudentAPI.Repository;

namespace StudentAPI.Extension
{
    public static class DbDependencyExtension
    {
        /// <summary>
        /// This method creates cosmos db database and container
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dbConfiguration"></param>
        /// <returns></returns>
        public static async Task RegisterDbDependencies(this IServiceCollection services, DbConfiguration dbConfiguration)
        {
            services.AddSingleton(dbConfiguration);
            services.AddTransient<IContainerAdapter, ContainerAdapter>();
            using(ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                IContainerAdapter containerAdapter = serviceProvider.GetService<IContainerAdapter>();
                
                await containerAdapter.CreateDatabaseIfNotExistsAsync().ConfigureAwait(false);

                foreach (var container in Container.GetContainers()) 
                {
                    await containerAdapter.CreateContainerIfNotExistsAsync(container.containerId, container.partitionKeyPath).ConfigureAwait(false);
                }
            }
            services.AddTransient<IContainerRepository<Student>, ContainerRepository>(provider => new ContainerRepository(provider.GetRequiredService<IContainerAdapter>(), Container.Students.containerId));
        }
    }
}
