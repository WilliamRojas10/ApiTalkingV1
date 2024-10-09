using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dao_library; // Contexto de base de datos
using EntitiesLibrary.Entities;
using EntitiesLibrary.Entities.Enum;
using ApiTalking.DTO.Post;
using ApiTalking.DTO.common;
using System.IO;

namespace ApiTalking.Controllers;
[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly MyDbContext _context;

    public PostController(MyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult> Get10Posts()
    {
        try
        {
            var posts = await _context.Posts
                .Where(pDB => pDB.PostStatus == PostStatus.Active)
                .Select(pDB => new ResponsePostDTO
                {
                    id = pDB.Id,
                    description = pDB.Description,
                    postStatus = pDB.PostStatus.ToString(),
                    registrationDate = pDB.RegistrationDate,
                    idUser = pDB.IdUser,
                    idFile = pDB.IdFile
                })
                .Take(10) // Limita el número de registros a 10
                .ToListAsync();

            return Ok(posts);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = ex.Message
            });
        }
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetPost(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        return post;
    }


    [HttpPost]
    public async Task<ActionResult> CreatePost([FromBody] RequestPostDTO post)
    {
        try
        {
            if (post == null)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "Datos ingresados erroneos"
                });
            }
            var postSaved = new Post
            {
                Id = 0,
                Description = post.description,
                PostStatus = post.postStatus,
                IdUser = post.idUser,
                IdFile = post.idFile
            };
            _context.Posts.Add(postSaved);
            await _context.SaveChangesAsync();

            return Ok(new ResponseDTO
            {
                sucess = true,
                message = "Post creado con exito"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = ex.Message
            });
        }
    }


    //TODO To Define if it is necessary
    [HttpPost("withfile")]
    public async Task<ActionResult<Post>> CreatePostWithFile([FromForm] RequestPostWithFileDTO postWithFileDTO)
    {
        if (postWithFileDTO.file == null || postWithFileDTO.file.Length == 0)
        {
            return BadRequest("File is required.");
        }
        // Guardar el archivo en el servidor
        var file = await SaveFileAsync(postWithFileDTO.file);

        // Crear la entrada de Post
        var post = new Post
        {
            Id = 0,
            Description = postWithFileDTO.description,
            PostStatus = postWithFileDTO.postStatus,
            IdUser = postWithFileDTO.idUser,
            IdFile = file.Id
        };
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
    }

    private async Task<PublishedFile> SaveFileAsync(IFormFile formFile)
    {
        // Define la ruta del directorio donde se guardará el archivo
        var directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "imagenesPruebaApi");

        // Verifica si el directorio existe; si no, lo crea
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Define la ruta completa del archivo
        var filePath = Path.Combine(directoryPath, formFile.FileName);

        // Guarda el archivo en el sistema
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await formFile.CopyToAsync(stream);
        }

        // Crea la entidad PublishedFile y la guarda en la base de datos
        var file = new PublishedFile
        {
            Id = 0,
            Name = formFile.FileName,
            Type = Path.GetExtension(formFile.FileName),
            Path = filePath
        };

        _context.Files.Add(file);
        await _context.SaveChangesAsync();

        return file;
    }



    [HttpPut("Eliminar/{id}")]
    public async Task<IActionResult> DeletedPost(int id)
    {
        try
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            if (post.PostStatus != PostStatus.Active)
            {
                return BadRequest("User is not active, so it cannot be deleted.");
            }
            post.PostStatus = PostStatus.Deleted;
            await _context.SaveChangesAsync();

            return Ok(new ResponseDTO
            {
                sucess = true,
                message = "Posteo eliminado."
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = ex.Message
            });
        }
    }
}