using Godot;
using System;
using System.Threading.Tasks;

public partial class RenameModal : PopupPanel
{
	[Export]
	private LineEdit NameInput { get; set; }
	[Export]
	private Button SaveButton { get; set; }
	
	private ILoggerService _logger { get; set; }
	private Observables _observables { get; set; }
	private IGameStateInteractor _gameStateInteractor { get; set;}
	private IEggInteractor _eggInteractor { get; set;}
	private IAnimalInteractor _animalInteractor { get; set;}
	
	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
		_gameStateInteractor = GetNode<IGameStateInteractor>(Constants.SingletonNodes.GameStateInteractor);
		_eggInteractor = GetNode<IEggInteractor>(Constants.SingletonNodes.EggInteractor);
		_animalInteractor = GetNode<IAnimalInteractor>(Constants.SingletonNodes.AnimalInteractor);

		SaveButton.Pressed += async () => await SaveAsync();
		_observables.OpenCloseRenameWindow += HandleOpenCloseRenameWindow;
	}

	public async Task SaveAsync()
	{
		var fgCharacter = _gameStateInteractor.GetForegroundCharacter();
		var newName = NameInput.Text;

		if (fgCharacter.Model.CreatureType == Enumerations.CreatureTypes.Egg)
		{
			await _eggInteractor.RenameEgg(fgCharacter.Model.Id, newName);
		}
		else
		{
			await _animalInteractor.RenameAnimal(fgCharacter.Model.Id, newName);
		}

		_observables.EmitUpdateSubjectNameLabel(newName);

		Visible = false;
	}
	
	public void HandleOpenCloseRenameWindow(bool isOpen)
	{
		Visible = isOpen;
	}
}
