using Godot;

public interface IForegroundSubject<TModel> where TModel : CreatureModel
{
	CharacterBody3D CharacterBody3D { get; set; }
	TModel Model { get; set; }
	IExecuter Executer { get; set; }
	void ReadyInstance(CharacterBody3D character, TModel model, IExecuter executer);
}
