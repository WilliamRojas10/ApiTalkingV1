using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EntitiesLibrary.Entities;
using EntitiesLibrary.Entities.Enum;
using ApiTalking.DTO.common;
using ApiTalking.DTO.Comment;
using dao_library;


namespace ApiTalking.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{

    private readonly MyDbContext _context;
    public CommentController(MyDbContext context)
    {
        _context = context;
    }

    [HttpGet("{postId}/reaction")]
    public async Task<ActionResult<IEnumerable<ResponseCommentDTO>>> GetAllReactionsByPostId(int postId)
    {
        try
        {
            // Verifica si el Post existe
            var postExists = await _context.Posts.AnyAsync(p => p.Id == postId);
            if (!postExists)
            {
                return NotFound($"No se encontró el post con id: {postId}");
            }

            // Obtiene todas las reacciones asociadas al post
            var comment = await _context.Comments
                .Where(r => r.IdPost == postId)
                .Select(rDB => new ResponseCommentDTO
                {
                    id = rDB.Id,
                    text = rDB.Text,
                    registrationDate = rDB.RegistrationDate,
                    // idUser = rDB.IdUser,
                    //idPost = rDB.IdPost,
                    commentStatus = rDB.CommentStatus.ToString()
                })
                .ToListAsync();

            if (comment == null || comment.Count == 0)
            {
                return NotFound($"No se encontró ningun comentario para el post con id: {postId}");
            }

            return Ok(comment);
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


    [HttpPost]
    public async Task<ActionResult> CreateComment([FromBody] RequestCommentDTO comment)
    {
        try
        {
            if (comment == null)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "Datos ingresados erroneos"
                });
            }

            // Verificar que IdUser y IdPost existan en la base de datos
            var userExists = await _context.Users.AnyAsync(u => u.Id == comment.idUser);
            var postExists = await _context.Posts.AnyAsync(p => p.Id == comment.idPost);

            if (!userExists || !postExists)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "El usuario o el post no existen."
                });
            }

            var commentSaved = new Comment
            {
                Id = 0,
                Text = comment.text,
                //RegistrationDate = DateTime.UtcNow,
                CommentStatus = comment.commentStatus,
                IdUser = comment.idUser,
                IdPost = comment.idPost
            };

            _context.Comments.Add(commentSaved);
            await _context.SaveChangesAsync();

            return Ok(new ResponseDTO
            {
                sucess = true,
                message = "Comentario creado."
            });
        }
        catch (DbUpdateException dbEx)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = $"Error al guardar los cambios: {dbEx.InnerException?.Message ?? dbEx.Message}"
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




    [HttpPut("Eliminar/{id}")]
    public async Task<IActionResult> DeletedComment(int id)
    {
        try
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            if (comment.CommentStatus == CommentStatus.Deleted)
            {
                return BadRequest("El comentario está eliminado, por lo que no puede ser eliminado.");
            }
            comment.CommentStatus = CommentStatus.Deleted;
            await _context.SaveChangesAsync();

            return Ok(new ResponseDTO
            {
                sucess = true,
                message = "Comentario eliminado."
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