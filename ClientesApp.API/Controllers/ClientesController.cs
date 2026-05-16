using System.ComponentModel.DataAnnotations;
using ClientesApp.API.Contexts;
using ClientesApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientesApp.API.Controllers;

[Route("api/v1/clientes")]
[ApiController]
public class ClientesController : ControllerBase
{
    private readonly DataContext _dataContext;

    public ClientesController(DataContext dataContext)
       => _dataContext = dataContext;

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] ClienteRequest request)
    {
        var cliente = new Cliente { Nome = request.Nome, Email = request.Email };

        await _dataContext.Clientes.AddAsync(cliente);
        await _dataContext.SaveChangesAsync();

        return StatusCode(201, new
        {
            message = "Cliente cadastrado com sucesso.",
            cliente
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(Guid id, [FromBody] ClienteRequest request)
    {
        var cliente = await _dataContext.Clientes.FindAsync(id);

        if (cliente == null)
            return NotFound(new { message = "Cliente não encontrado." });

        cliente.Nome = request.Nome;
        cliente.Email = request.Email;

        _dataContext.Clientes.Update(cliente);
        await _dataContext.SaveChangesAsync();

        return Ok(new
        {
            message = "Cliente atualizado com sucesso.",
            cliente
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var cliente = await _dataContext.Clientes.FindAsync(id);

        if (cliente == null)
            return NotFound(new { message = "Cliente não encontrado." });

        _dataContext.Clientes.Remove(cliente);
        await _dataContext.SaveChangesAsync();

        return Ok(new
        {
            message = "Cliente excluído com sucesso.",
            cliente
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var clientes = await _dataContext.Clientes
                        .OrderByDescending(c => c.DataHoraCadastro)
                        .ToListAsync();

        if (!clientes.Any())
            return NoContent();

        return Ok(clientes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var cliente = await _dataContext.Clientes.FindAsync(id);

        if (cliente == null)
            return NotFound(new { message = "Cliente não encontrado." });

        return Ok(cliente);
    }
}

//Record para entrada de dados (REQUEST)
public record ClienteRequest(
    [Required(ErrorMessage = "Nome é obrigatório.")]
    [MaxLength(150, ErrorMessage = "Nome deve ter no máximo 150 caracteres.")]
    string Nome,

    [Required(ErrorMessage = "Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
    string Email
);