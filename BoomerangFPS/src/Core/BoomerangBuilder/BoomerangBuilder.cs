public class BoomerangBuilder : IBoomerangBuilder
{
	private Boomerang Result { get; set; }

	public void Reset() 
	{
		Result = new Boomerang();
	}

	public void BuildExplosive() {}

	public void BuildMulti() {}

	public Boomerang GetResult()
	{
		return Result;
	}
}
