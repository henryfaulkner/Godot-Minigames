using System.Threading.Tasks;

public interface IExecuter 
{
	Task ExecuteAction(Enumerations.ForegroundActions menuAction);
}
