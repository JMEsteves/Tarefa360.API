using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Projeto360.Aplicacao.Contratos;
using Projeto360.Dominio.Entidades;
using Projeto360.Aplicacao.DTOs;
using Projeto360.Api.Dtos;

namespace Projeto360.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioAplicacao _usuarioAplicacao;

    public UsuarioController(IUsuarioAplicacao usuarioAplicacao)
    {
        _usuarioAplicacao = usuarioAplicacao;
    }

    
    [HttpGet("ListarTiposUsuario")]
    public IActionResult ListarTiposUsuario()
    {
        var tipos = new[]
        {
            new { Id = 1, Descricao = "Administrador" },
            new { Id = 2, Descricao = "Gestor" },
            new { Id = 3, Descricao = "Desenvolvedor" }
        };

        return Ok(tipos);
    }

    
    [HttpPost("Criar")]
    public async Task<IActionResult> Criar([FromBody] UsuarioCriarDto dados)
    {
        if (string.IsNullOrWhiteSpace(dados.Nome) ||
            string.IsNullOrWhiteSpace(dados.Email) ||
            string.IsNullOrWhiteSpace(dados.Senha))
        {
            return BadRequest("Preencha todos os campos.");
        }

        if (dados.TipoUsuario < 1 || dados.TipoUsuario > 3)
        {
            return BadRequest("Tipo de usuário inválido.");
        }

        var usuario = new Usuario
        {
            Nome = dados.Nome,
            Email = dados.Email,
            TipoUsuario = dados.TipoUsuario
        };

        
        var passwordHasher = new PasswordHasher<Usuario>();

        usuario.SenhaHash = passwordHasher.HashPassword(
            usuario,
            dados.Senha
        );

        var usuarioCriado = await _usuarioAplicacao.CriarAsync(usuario);

        return Ok(new
        {
            usuarioCriado.Id,
            usuarioCriado.Nome,
            usuarioCriado.Email,
            usuarioCriado.TipoUsuario
        });
    }

    
    [HttpGet("Listar")]
    public async Task<IActionResult> Listar()
    {
        var usuarios = await _usuarioAplicacao.ListarAsync();

        var resultado = usuarios.Select(usuario => new
        {
            usuario.Id,
            usuario.Nome,
            usuario.Email,
            usuario.TipoUsuario
        });

        return Ok(resultado);
    }

    
    [HttpDelete("Excluir/{id}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var excluido = await _usuarioAplicacao.ExcluirAsync(id);

        if (!excluido)
        {
            return NotFound("Usuário não encontrado.");
        }

        return Ok("Usuário excluído com sucesso.");
    }

    
    [HttpPut("Atualizar")]
    public async Task<IActionResult> Atualizar([FromBody] UsuarioAtualizarDto dados)
    {
        if (string.IsNullOrWhiteSpace(dados.Nome) ||
            string.IsNullOrWhiteSpace(dados.Email))
        {
            return BadRequest("Preencha todos os campos.");
        }

        if (dados.TipoUsuario < 1 || dados.TipoUsuario > 3)
        {
            return BadRequest("Tipo de usuário inválido.");
        }

        var usuario = new Usuario
        {
            Id = dados.Id,
            Nome = dados.Nome,
            Email = dados.Email,
            TipoUsuario = dados.TipoUsuario
        };

        var atualizado = await _usuarioAplicacao.AtualizarAsync(usuario);

        if (!atualizado)
        {
            return NotFound("Usuário não encontrado.");
        }

        return Ok("Usuário atualizado com sucesso.");
    }


    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UsuarioLoginDto dados)
    {
        if (string.IsNullOrWhiteSpace(dados.Email) || 
        string.IsNullOrWhiteSpace(dados.Senha))
        {
            return BadRequest("Informe e-mail e senha.");
        }

        var usuario = await _usuarioAplicacao.ObterPorEmailAsync(dados.Email);

        if(usuario == null)
        {
            return Unauthorized("E-mail ou senha inválidos");
        }

        var passwordHasher = new PasswordHasher<Usuario>();

        var resultado = passwordHasher.VerifyHashedPassword(
            usuario,
            usuario.SenhaHash,
            dados.Senha
        );

        if (resultado == PasswordVerificationResult.Failed)
        {
            return Unauthorized("E-mail ou senha inválidos");
        }

        return Ok(new
        {
            usuario.Id,
            usuario.Nome,
            usuario.Email,
            usuario.TipoUsuario

        });

    }
}

    


public class UsuarioCriarDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public int TipoUsuario { get; set; }
}


public class UsuarioAtualizarDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int TipoUsuario { get; set; }
}