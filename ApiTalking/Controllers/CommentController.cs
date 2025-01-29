using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EntitiesLibrary.Comment;
using DaoLibrary.Interfaces.Comment;
using ApiTalking.DTO.common;
using ApiTalking.DTO.Comment;
using ApiTalking.Helpers;

namespace ApiTalking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly IDAOComment _daoComment;

    public CommentController(IDAOComment daoComment)
    {
        _daoComment = daoComment;
    }

    [HttpGet("paginado")]
    public async Task<IActionResult> GetComments(int page, int pageSize)
    {
        try
        {
            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
            (var comments, int totalRecords) = await _daoComment.GetCommentsPaged
            (
            page,
            pageSize,
            activeStatus
            );
            if (comments == null || !comments.Any())
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "No se encontraron usuarios"
                });
            }
            var commentDTO = comments.Select(comment => new ResponseCommentDTO
            {
                idComment = comment.Id,
                name = comment.Name,
                lastName = comment.LastName,
                email = comment.Email,
                birthDate = comment.BirthDate.ToString(),
                nationality = comment.Nationality,
                province = comment.Province
            });
            return Ok(new
            {
                totalRecords = totalRecords,
                comments = commentDTO
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = "Error en getComments(): " + ex.Message
            });
        }
    }

    [HttpGet("{idComment}")]
    public async Task<IActionResult> GetCommentById
    (
        int idComment,
        EntitiesLibrary.Common.EntityStatus entityStatus
    )
    {
        try
        {
            var comment = await _daoComment.GetCommentById(idComment, entityStatus);
            if (comment == null)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "No se encontró el usuario con el Id: " + idComment
                });
            }
            return Ok(new ResponseCommentDTO
            {
                idComment = comment.Id,
                name = comment.Name,
                lastName = comment.LastName,
                email = comment.Email,
                birthDate = comment.BirthDate.ToString(),
                nationality = comment.Nationality,
                province = comment.Province
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = "Error al obtener un usuario usando GetCommentById(): " + ex.Message
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] RequestCommentDTO commentDTO)
    {
        try
        {
            if (commentDTO == null)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "Datos del usuario no válidos"
                });
            }

            var comment = new Comment
            {
                Name = commentDTO.name,
                LastName = commentDTO.lastName,
                Email = commentDTO.email,
                Password = commentDTO.password,
                BirthDate = Converter.convertStringToDateOnly(commentDTO.birthDate),
                Nationality = commentDTO.nationality,
                Province = commentDTO.province,
                EntityStatus = EntitiesLibrary.Common.EntityStatus.Active
            };

            await _daoComment.AddComment(Comment);

            return Ok(new ResponseDTO
            {
                sucess = true,
                message = "Usuario guardado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = "Error al crear el usuario: " + ex.Message
            });
        }
    }

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
                    sucess = false,
                    message = "Datos del usuario no válidos"
                });
            }

            var comment = await _daoComment.GetCommentById(idComment, activeStatus);
            if (comment == null)
            {
                return NotFound(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "No se encontró el usuario con el Id: " + idComment
                });
            }
            comment.Name = commentDTO.name;
            comment.LastName = commentDTO.lastName;
            comment.Email = commentDTO.email;
            comment.BirthDate = Converter.convertStringToDateOnly(commentDTO.birthDate);
            comment.Nationality = commentDTO.nationality;
            comment.Province = commentDTO.province;

            await _daoComment.UpdateComment(comment);

            return Ok(new ResponseDTO
            {
                sucess = true,
                message = "Usuario actualizado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = "Error al actualizar el usuario: " + ex.Message
            });
        }
    }

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
                    sucess = false,
                    message = "No se encontró el usuario con el Id: " + idComment
                });
            }
            comment.EntityStatus = EntitiesLibrary.Common.EntityStatus.Blocked;

            await _daoComment.UpdateComment(comment);

            return Ok(new ResponseDTO
            {
                sucess = true,
                message = "Usuario eliminado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = "Error al actualizar el usuario: " + ex.Message
            });
        }
    }

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
                    sucess = false,
                    message = "No se encontró el usuario con el Id: " + idComment
                });
            }
            comment.CommentStatus = EntitiesLibrary.Common.EntityStatus.Active;

            await _daoComment.UpdateComment(comment);

            return Ok(new ResponseDTO
            {
                sucess = true,
                message = "Usuario activado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = "Error al activar el usuario: " + ex.Message
            });
        }
    }

    [HttpPut("eliminar/{idComment}")]
    public async Task<IActionResult> DeleteComment(int idComment)
    {
        try
        {
            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
            var comment = await _daoComment.GetCommentById(idComment, activeStatus);
            if (comment == null)
            {
                return NotFound(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "No se encontró el usuario con el Id: " + idComment
                });
            }
            comment.EntityStatus = EntitiesLibrary.Common.EntityStatus.Deleted;

            await _daoComment.UpdateComment(comment);

            return Ok(new ResponseDTO
            {
                sucess = true,
                message = "Usuario eliminado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = "Error al actualizar el usuario: " + ex.Message
            });
        }
    }







}
