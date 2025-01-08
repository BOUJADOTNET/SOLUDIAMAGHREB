namespace SOLUDIAMAGHREB.Models;

public partial class Utilisateur
{
    public int IdUser { get; set; }

    public string? Nom { get; set; }

    public string? Prenom { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Clave { get; set; }
}
