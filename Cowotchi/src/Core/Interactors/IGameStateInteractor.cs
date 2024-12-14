using Godot;
using System;
using System.Collections.Generic;

interface IGameStateInteractor
{
	void ReadyInstance(List<CreatureModel> CreatureList, Vector3 initialPosition, Menu menu);
	ICharacter<CreatureModel> GetForegroundCharacter();
	List<CreatureModel> GetCreatureList();
	void RotateForegroundSubjects();
	void RemoveBackgroundSubject(CreatureModel bgSubject);	
	void AddBackgroundSubject(CreatureModel model);
	void ToggleInfoContainer();
	CreatureModel SwapBackgroundToForeground(CreatureModel newFgModel);
}
