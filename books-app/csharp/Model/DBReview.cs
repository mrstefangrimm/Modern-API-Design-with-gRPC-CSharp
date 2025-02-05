using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

public class DBReview
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  [Key, Column(Order = 0)]
  public int Id { get; set; }

  [Required]
  public int Isbn { get; set; }

  [Required]
  public string Reviewer { get; set; }

  [Required]
  public string Comment { get; set; }

  [Required]
  public int Rating { get; set; }
}
