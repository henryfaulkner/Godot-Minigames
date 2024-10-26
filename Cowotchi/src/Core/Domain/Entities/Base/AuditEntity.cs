using System;

public class AuditEntity
{
	public DateTime CreatedDate { get; set; }
	public DateTime ModifiedDate { get; set; }
	public bool IsDeleted { get; set; }
}
