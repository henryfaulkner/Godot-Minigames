using System.ComponentModel.DataAnnotations;

public class Log
{
    [Key]
    public int Id { get; set; }
    public string Level { get; set; }
    public string Message { get; set; }
    public string? StackTrace { get; set; }
}