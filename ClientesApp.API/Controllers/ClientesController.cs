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

    public ClientesController()
       => _dataContext = new DataContext();

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] ClienteRequest request)
    {
        var cliente = new Cliente { Nome = request.nome, Email = request.email };

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

        cliente.Nome = request.nome;
        cliente.Email = request.email;

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
            return NoContent();

        return Ok(cliente);
    }
}

//Record para entrada de dados (REQUEST)
public record ClienteRequest(
    string nome,
    string email
);