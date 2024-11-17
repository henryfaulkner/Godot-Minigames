using System;

public class CreatureModel
{
	public int Id { get; set; }
	public ulong InstanceId { get; set; }
	public string Name { get; set; }
	public Enumerations.CreatureTypes CreatureType { get; protected set; }
	public DateTime BirthDate { get; set; }
}
