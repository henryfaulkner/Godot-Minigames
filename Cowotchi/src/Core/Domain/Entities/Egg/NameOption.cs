using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class NameOption
{
    [Key]
    public int Id { get; set; }
    public int Name { get; set; }
}