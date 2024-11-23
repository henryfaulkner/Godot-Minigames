public interface ICharacterWithBackgroundSubject<TModel> where TModel : CreatureModel
{
	BackgroundSubject<TModel> BackgroundSubject { get; set; }
}