namespace ZooApi.Application.Common;

public class CacheKeys
{
    public static string GetAnimalKey(Guid AnimalCreated) => $"Animal:{AnimalCreated}";
}