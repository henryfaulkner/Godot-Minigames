using Godot;
using System;
using System.Collections.Generic;

public interface IGameStateInteractor
{
	public void ReadyInstance(List<CreatureModel> CreatureList, Vector3 initialPosition);
	public ICharacter<CreatureModel> GetForegroundCharacter();
	public List<CreatureModel> GetCreatureList();
	public void RotateForegroundSubjects();
	public void RemoveBackgroundSubject(Subject<CreatureModel> bgSubject);	
	public void AddBackgroundSubject(CreatureModel model);
}
