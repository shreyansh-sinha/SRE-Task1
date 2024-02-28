namespace StudentAPI.Model
{
    public static class Container
    {
        public static ContainerSetting Students => new("Students", "/PartitionKey");

        public static List<ContainerSetting> GetContainers() => [Students];
    }
}
