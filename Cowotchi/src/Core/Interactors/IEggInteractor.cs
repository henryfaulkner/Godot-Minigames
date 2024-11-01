using System.Threading.Tasks;

public interface IEggInteractor
{
    Task<Egg> CreateEgg();
    Task<Egg> GetEgg(int id);
    Task RenameEgg(int id, string name);
    Task<Animal> HatchEgg(int id);
}