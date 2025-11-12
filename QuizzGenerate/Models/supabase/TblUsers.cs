using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace QuizzGenerate.Models.supabase;

[Table("tblusers")]
public class TblUsers: BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }
    [Column("name")]
    public string Name { get; set; }
    [Column("last_name")]
    public string LastName { get; set; }
    [Column("email")]
    public string Email { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("status")]
    public string Status { get; set; }
    [Column("id_auth")] 
    public string IdAuth { get; set; }
}