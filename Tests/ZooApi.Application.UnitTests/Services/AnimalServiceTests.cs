namespace ZooApi.Application.UnitTests.Services;

public class AnimalServiceTests : IDisposable
{
    private readonly ZooDbContext _context;
    private readonly SqliteConnection _connection;
    private readonly IPublishEndpoint _publishEndpoint = Substitute.For<IPublishEndpoint>();
    private readonly AnimalService _service;
    
    public AnimalServiceTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        
        var options = new DbContextOptionsBuilder<ZooDbContext>()
            .UseSqlite(_connection)
            .Options;
        
        _context = new ZooDbContext(options);
        
        _service = new AnimalService(_context, _publishEndpoint);
    }
    
    public void Dispose()
    {
        _connection.Dispose();
        _connection.Close();
        (_context as IDisposable)?.Dispose();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldSaveAnimalAndPublishEvent()
    {
        // Arrange
        var dto = new CreateAnimalDto("Simba", "Lion");

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert: Проверяем, что в базе появилось животное
        var animalInDb = await _context.Animals.FirstOrDefaultAsync(a => a.Id == result.Id);
        animalInDb.Should().NotBeNull();
        animalInDb!.Name.Should().Be("Simba");

        // Assert: Проверяем, что MassTransit отправил сообщение 
        await _publishEndpoint.Received(1).Publish(
            Arg.Is<AnimalCreated>(e => e.Id == result.Id && e.Name == "Simba"),
            Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldRemoveAnimalFromDatabase()
    {
        // Arrange: Добавляем животное, которое будем удалять
        var animal = new Animal("To be deleted", "Unknown");
        _context.Animals.Add(animal);
        await _context.SaveChangesAsync();

        // Act
        await _service.DeleteAsync(animal.Id);

        // Assert: Проверяем, что его больше нет
        var exists = await _context.Animals.AnyAsync(a => a.Id == animal.Id);
        exists.Should().BeFalse();
    }
}