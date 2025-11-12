using Supabase.Postgrest.Attributes;

namespace QuizzGenerate.Models.supabase;

[System.ComponentModel.DataAnnotations.Schema.Table("tblusers")]
public class TblUser
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }
    [Column("name")]
    public string Name { get; set; }
    [Column("last_name")]
    public string LastName { get; set; }
    [Column("email")]
    public string Email { get; set; }
    [Column("password")]
    public string Password { get; set; }
    [Column("is_verified")]
    public int IsVerified { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("status")]
    public string Status { get; set; }
}