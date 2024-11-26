using Godot;
using System;
using System.Collections.Generic;

public interface IGameStateInteractor
{
	public void ReadyInstance(List<CreatureModel> CreatureList, CharacterBody3D placeholder);
	public ICharacter<CreatureModel> GetForegroundCharacter();
	public List<CreatureModel> GetCreatureList();
	public void RotateForegroundSubjects(ICharacter<CreatureModel> fgCharacter);
	public void RemoveBackgroundSubject(Subject<CreatureModel> bgSubject);	
	public void AddBackgroundSubject(CreatureModel model);
}
