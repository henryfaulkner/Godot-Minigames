using System.Threading.Tasks;

public interface ICharacter<TModel> where TModel : CreatureModel
{
	Subject<TModel> Subject { get; set; }
	TModel Model { get; set; }
	IController Controller { get; set; }
	Task ExecuteActionAsync(Enumerations.CharacterActions actions);
}
