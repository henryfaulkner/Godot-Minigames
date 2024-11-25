public interface ICharacter<TModel> where TModel : CreatureModel
{
	Subject<TModel> Subject { get; set; }
}
