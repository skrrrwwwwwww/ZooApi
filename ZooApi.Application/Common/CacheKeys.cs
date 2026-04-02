namespace ZooApi.Application.Common;

public class CacheKeys
{
    public static string GetAnimalKey(Guid AnimalCreated) => $"Animal:{AnimalCreated}";
    public static string GetOwnerKey(Guid id) => $"Owner:{id}";
    
    public const string AllAnimals = "Animals:All";
}