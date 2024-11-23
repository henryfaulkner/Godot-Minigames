public interface ICharacterWithForegroundSubject<TModel> where TModel : CreatureModel
{
	ForegroundSubject<TModel> ForegroundSubject { get; set; }
}
