using Godot;

public interface ISubject<TModel> where TModel : CreatureModel
{
	CharacterBody3D CharacterBody3D { get; set; }
	TModel Model { get; set; }
	void ReadyInstance(CharacterBody3D character, TModel model);
}
