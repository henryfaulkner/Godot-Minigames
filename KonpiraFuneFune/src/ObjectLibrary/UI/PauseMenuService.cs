using System.Collections.Generic;
using Godot;

public partial class PauseMenuService : Node
{
	public Stack<BaseMenuPanel> WorkingDirectory { get; set; }
	public List<BaseMenuPanel> PanelList { get; set; }

	public override void _Ready()
	{
		WorkingDirectory = new Stack<BaseMenuPanel>();
	}

	public void SetPanelList(List<BaseMenuPanel> panelList)
	{
		PanelList = panelList;
	}

	public void PushPanel(BaseMenuPanel priorPanel)
	{
		WorkingDirectory.Push(priorPanel);
		return;
	}

	public BaseMenuPanel PopPanel()
	{
		return WorkingDirectory.Pop();
	}
}
