namespace ClientesApp.API.Models;

public class Cliente
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DataHoraCadastro { get; set; } = DateTime.Now;
}