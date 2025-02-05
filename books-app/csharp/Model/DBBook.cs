using System.ComponentModel.DataAnnotations;

namespace Model;

public class DBBook
{
  [Key]
  public int Isbn { get; set; }

  [Required]
  public string Name { get; set; }

  [Required]
  public string Publisher { get; set; }
}
