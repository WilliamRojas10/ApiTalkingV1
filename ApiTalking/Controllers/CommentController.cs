using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EntitiesLibrary.Comment;
using DaoLibrary.Interfaces.Comment;
using ApiTalking.DTOs.common;
using ApiTalking.DTOs.Comment;
using ApiTalking.Helpers;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using DaoLibrary.Interfaces.User;
using DaoLibrary.Interfaces.Post;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ApiTalking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly IDAOComment _daoComment;
    private readonly IDAOUser _daoUser;
    private readonly IDAOPost _daoPost;

    public CommentController(IDAOComment daoComment, IDAOUser daoUser, IDAOPost daoPost)
    {
        _daoComment = daoComment;
        _daoUser = daoUser;
        _daoPost = daoPost;

    }

    [Authorize(Roles = "Administrator")]
    [HttpGet("paginado-admin")]
    public async Task<IActionResult> GetCommentsAdmin (int page, int pageSize, int idPost)
    {
        try
        {
            (var comments, int totalRecords) = await _daoComment.GetCommentsPaged
            (
            page,
            pageSize,
            idPost
            );
            if (comments == null || !comments.Any())
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontraron usuarios"
                });
            }
            var commentDTO = comments.Select(comment => new ResponseCommentDTO
            {
                idComment = comment.Id,
                text = comment.Text,
                userName = comment.User.Name + " " + comment.User.LastName,
                registrationDate = comment.RegistrationDateTime.ToString(),
                entityStatus = (int)comment.EntityStatus
            });
            return Ok(new ResponseDTO
            {
                success = true,
                message = "Se obtuvo la lista de comentarios correctamente",
                data = new
                {
                    totalRecords,
                    comments = commentDTO
                }
            }
            );
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error en getComments(): " + ex.Message
            });
        }
    }

    [HttpGet("paginado")]
    public async Task<IActionResult> GetComments(int page, int pageSize, int idPost)
    {
        try
        {
            var statusActive = EntitiesLibrary.Common.EntityStatus.Active;
            (var comments, int totalRecords) = await _daoComment.GetCommentsPaged
            (
            page,
            pageSize,
            idPost,
            statusActive
            );
            if (comments == null || !comments.Any())
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontraron usuarios"
                });
            }
            var commentDTO = comments.Select(comment => new ResponseCommentDTO
            {
                idComment = comment.Id,
                text = comment.Text,
                userName = comment.User.Name + " " + comment.User.LastName, 
                registrationDate = comment.RegistrationDateTime.ToString(),
                entityStatus = (int)comment.EntityStatus
            });
            return Ok(new ResponseDTO
                {
                    success = true,
                    message = "Se obtuvo la lista de comentarios correctamente",
                    data = new{
                        totalRecords,
                        comments = commentDTO
                }
            }
            );
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error en getComments(): " + ex.Message
            });
        }
    }

    [HttpGet("{idComment}")]
    public async Task<IActionResult> GetCommentById
    (
        int idComment
    )
    {
        try
        {
            var comment = await _daoComment.GetCommentById(idComment);
            if (comment == null)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontró el usuario con el Id: " + idComment
                });
            }
            return Ok(new ResponseCommentDTO
            {
                idComment = comment.Id,
                text = comment.Text,
                userName = comment.User.Name + " " + comment.User.LastName,
                registrationDate = comment.RegistrationDateTime.ToString(),
                entityStatus = (int)comment.EntityStatus
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al obtener un usuario usando GetCommentById(): " + ex.Message
            });
        }
    }

    [Authorize(Roles = "Administrator, User")]
    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] RequestCommentDTO commentDTO)
    {
        try
        {
            if (commentDTO == null)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "Datos del usuario no válidos"
                });
            }
            var userIdClaim = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new ErrorResponseDTO { success = false, message = "Usuario no autenticado." });

            int userId = int.Parse(userIdClaim);
            var user = await _daoUser.GetUserById(userId);
            if (user == null)
                return NotFound(new ErrorResponseDTO { success = false, message = $"Usuario con ID {userId} no encontrado." });

            EntitiesLibrary.Post.Post? post = await _daoPost.
                                                GetPostById(commentDTO.idPost, EntitiesLibrary.Common.EntityStatus.Active);
            var comment = new Comment
            {
                User = user,
                Post = post,
                Text = commentDTO.text,
                EntityStatus = EntitiesLibrary.Common.EntityStatus.Active
            };

            await _daoComment.AddComment(comment);

            return Ok(new ResponseDTO
            {
                success = true,
                message = "Comentario guardado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al crear un comentario: " + ex.Message
            });
        }
    }

    [Authorize(Roles = "Administrator, User")]
    [HttpPut("modificar/{idComment}")]
    public async Task<IActionResult> UpdateComment(int idComment, [FromBody] RequestCommentDTO commentDTO)
    {
        try
        {
            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
            if (commentDTO == null)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "Datos del usuario no válidos"
                });
            }

            var comment = await _daoComment.GetCommentById(idComment, activeStatus);
            if (comment == null)
            {
                return NotFound(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontró el usuario con el Id: " + idComment
                });
            }
            comment.Text = commentDTO.text;

            await _daoComment.UpdateComment(comment);

            return Ok(new ResponseDTO
            {
                success = true,
                message = "Usuario actualizado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al actualizar el usuario: " + ex.Message
            });
        }
    }

    [Authorize(Roles = "Administrator")]
    [HttpPut("bloquear/{idComment}")]
    public async Task<IActionResult> BlockComment(int idComment)
    {
        try
        {
            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;

            var comment = await _daoComment.GetCommentById(idComment, activeStatus);
            if (comment == null)
            {
                return NotFound(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontró el usuario con el Id: " + idComment
                });
            }
            comment.EntityStatus = EntitiesLibrary.Common.EntityStatus.Blocked;

            await _daoComment.UpdateComment(comment);

            return Ok(new ResponseDTO
            {
                success = true,
                message = "Usuario eliminado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al actualizar el usuario: " + ex.Message
            });
        }
    }

    [Authorize(Roles = "Administrator")]
    [HttpPut("activar/{idComment}")]
    public async Task<IActionResult> ActivateComment(int idComment)
    {
        try
        {
            var comment = await _daoComment.GetCommentById(idComment);
            if (comment == null)
            {
                return NotFound(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontró el usuario con el Id: " + idComment
                });
            }
            comment.EntityStatus = EntitiesLibrary.Common.EntityStatus.Active;

            await _daoComment.UpdateComment(comment);

            return Ok(new ResponseDTO
            {
                success = true,
                message = "Usuario activado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al activar el usuario: " + ex.Message
            });
        }
    }

    [Authorize(Roles = "Administrator, User")]
    [HttpDelete("{idComment}")]
    public async Task<IActionResult> DeleteComment(int idComment)
    {
        try
        {
            var comment = await _daoComment.GetCommentById(idComment);
            if (comment == null)
            {
                return NotFound(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontró el comentario con el Id: " + idComment
                });
            }
            comment.EntityStatus = EntitiesLibrary.Common.EntityStatus.Deleted;

            await _daoComment.UpdateComment(comment);

            return Ok(new ResponseDTO
            {
                success = true,
                message = "Comentario eliminado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al eliminar el comentario: " + ex.Message
            });
        }
    }

}
