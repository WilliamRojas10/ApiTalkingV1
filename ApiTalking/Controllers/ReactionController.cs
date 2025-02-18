using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EntitiesLibrary.Reaction;
using DaoLibrary.Interfaces.Reaction;
using ApiTalking.DTOs.common;
using ApiTalking.DTOs.Reaction;
using ApiTalking.Helpers;
using ApiTalking.DTOs.Post;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DaoLibrary.Interfaces.User;
using DaoLibrary.Interfaces.Post;

namespace ApiTalking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReactionController : ControllerBase
{
    private readonly IDAOUser _daoUser;
    private readonly IDAOReaction _daoReaction;
    private readonly IDAOPost _daoPost;

    public ReactionController(IDAOReaction daoReaction, IDAOUser daoUser, IDAOPost daoPost)
    {
        _daoReaction = daoReaction;
        _daoPost = daoPost;
        _daoUser = daoUser;
    }

  
        [HttpGet("all")]
        public IActionResult GetEntityStatuses()
        {
            var enumValues = Enum.GetValues(typeof(EntitiesLibrary.Reaction.ReactionStatus))
                                 .Cast<EntitiesLibrary.Reaction.ReactionStatus>()
                                 .Select(e => new
                                 {
                                     Name = e.ToString(),
                                     Value = (int)e
                                 })
                                 .ToList();

            return Ok(enumValues);
        }
    


    //    [Authorize(Roles = "Administrator")]
        //[HttpGet("reactionsByIdPost")]
        //public async Task<IActionResult> GetReactionsByPost(int idPost)
        //{
        //    try
           
        //{
        //        var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
        //    //(var Reactions, int totalRecords) = await _daoReaction.GetReactionsPaged
        //        var reactions = await _daoReaction.GetAllReactionsByIdPost(idPost);
               
        //        if (reactions == null || !reactions.Any())
        //        {
        //            return BadRequest(new ErrorResponseDTO
        //            {
        //                success = false,
        //                message = "No se encontraron reacciones"
        //            });
        //        }
        //        //var reactionDTO = reactions.Select(Reaction => new ResponseReactionDTO
        //        //{
        //        //    reactionStatus = reactions[0],
        //        //    countReactions = reactions[1]
        //        //});
        //        return Ok(new ResponseDTO
        //        {
        //            success = true,
        //            message = "Se ha obtenido las reacciones de un posteo",
        //            data = reactions
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ErrorResponseDTO
        //        {
        //            success = false,
        //            message = "Error en getReactions(): " + ex.Message
        //        });
        //    }
        //}

    //    [Authorize(Roles = "Administrator")]
    //    [HttpGet("{idReaction}")]
    //    public async Task<IActionResult> GetReactionById
    //    (
    //        int idReaction,
    //        EntitiesLibrary.Common.EntityStatus entityStatus
    //    )
    //    {
    //        try
    //        {
    //            var Reaction = await _daoReaction.GetReactionById(idReaction, entityStatus);
    //            if (Reaction == null)
    //            {
    //                return BadRequest(new ErrorResponseDTO
    //                {
    //                    success = false,
    //                    message = "No se encontró el usuario con el Id: " + idReaction
    //                });
    //            }
    //            return Ok(new ResponseReactionDTO
    //            {
    //                idReaction = Reaction.Id,
    //                name = Reaction.Name,
    //                lastName = Reaction.LastName,
    //                email = Reaction.Email,
    //                birthDate = Reaction.BirthDate.ToString(),
    //                nationality = Reaction.Nationality,
    //                province = Reaction.Province
    //            });
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(new ErrorResponseDTO
    //            {
    //                success = false,
    //                message = "Error al obtener un usuario usando GetReactionById(): " + ex.Message
    //            });
    //        }
    //    }


        [HttpPost]
        public async Task<IActionResult> CreateReaction([FromBody] RequestReactionDTO reactionDTO)
        {
            try
            {
                if (reactionDTO == null)
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
                EntitiesLibrary.User.User? user = await _daoUser.GetUserById(userId);
                EntitiesLibrary.Post.Post? post = await _daoPost.GetPostById(reactionDTO.idPost);


            var Reaction = new Reaction
                {
                    User = user,
                    Post = post,
                    ReactionStatus = (EntitiesLibrary.Reaction.ReactionStatus)reactionDTO.idReaction, 
                    EntityStatus = EntitiesLibrary.Common.EntityStatus.Active,
                  
                };

                await _daoReaction.AddReaction(Reaction);

                return Ok(new ResponseDTO
                {
                    success = true,
                    message = "Reacción guardada correctamente",
                    data = Reaction
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "Error al reaccionar a un posteo: " + ex.Message
                });
            }
        }


    //    [HttpPut("modificar/{idReaction}")]
    //    public async Task<IActionResult> UpdateReaction(int idReaction, [FromBody] RequestReactionDTO reactionDTO)
    //    {
    //        try
    //        {
    //            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
    //            if (reactionDTO == null)
    //            {
    //                return BadRequest(new ErrorResponseDTO
    //                {
    //                    success = false,
    //                    message = "Datos del usuario no válidos"
    //                });
    //            }

    //            var Reaction = await _daoReaction.GetReactionById(idReaction, activeStatus);
    //            if (Reaction == null)
    //            {
    //                return NotFound(new ErrorResponseDTO
    //                {
    //                    success = false,
    //                    message = "No se encontró el usuario con el Id: " + idReaction
    //                });
    //            }
    //            Reaction.Name = reactionDTO.name;
    //            Reaction.LastName = reactionDTO.lastName;
    //            Reaction.Email = reactionDTO.email;
    //            Reaction.BirthDate = Converter.convertStringToDateOnly(reactionDTO.birthDate);
    //            Reaction.Nationality = reactionDTO.nationality;
    //            Reaction.Province = reactionDTO.province;

    //            await _daoReaction.UpdateReaction(Reaction);

    //            return Ok(new ResponseDTO
    //            {
    //                success = true,
    //                message = "Usuario actualizado correctamente"
    //            });
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(new ErrorResponseDTO
    //            {
    //                success = false,
    //                message = "Error al actualizar el usuario: " + ex.Message
    //            });
    //        }
    //    }

    //    [Authorize(Roles = "Administrator")]
    //    [HttpPut("bloquear/{idReaction}")]
    //    public async Task<IActionResult> BlockReaction(int idReaction)
    //    {
    //        try
    //        {
    //            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;

    //            var Reaction = await _daoReaction.GetReactionById(idReaction, activeStatus);
    //            if (Reaction == null)
    //            {
    //                return NotFound(new ErrorResponseDTO
    //                {
    //                    success = false,
    //                    message = "No se encontró el usuario con el Id: " + idReaction
    //                });
    //            }
    //            Reaction.EntityStatus = EntitiesLibrary.Common.EntityStatus.Blocked;

    //            await _daoReaction.UpdateReaction(Reaction);

    //            return Ok(new ResponseDTO
    //            {
    //                success = true,
    //                message = "Usuario eliminado correctamente"
    //            });
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(new ErrorResponseDTO
    //            {
    //                success = false,
    //                message = "Error al actualizar el usuario: " + ex.Message
    //            });
    //        }
    //    }

    //    [Authorize(Roles = "Administrator")]
    //    [HttpPut("activar/{idReaction}")]
    //    public async Task<IActionResult> ActivateReaction(int idReaction)
    //    {
    //        try
    //        {
    //            var Reaction = await _daoReaction.GetReactionById(idReaction);
    //            if (Reaction == null)
    //            {
    //                return NotFound(new ErrorResponseDTO
    //                {
    //                    success = false,
    //                    message = "No se encontró el usuario con el Id: " + idReaction
    //                });
    //            }
    //            Reaction.EntityStatus = EntitiesLibrary.Common.EntityStatus.Active;

    //            await _daoReaction.UpdateReaction(Reaction);

    //            return Ok(new ResponseDTO
    //            {
    //                success = true,
    //                message = "Usuario activado correctamente"
    //            });
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(new ErrorResponseDTO
    //            {
    //                success = false,
    //                message = "Error al activar el usuario: " + ex.Message
    //            });
    //        }
    //    }

    //    [Authorize(Roles = "Administrator")]
    //    [HttpDelete("eliminar/{idReaction}")]
    //    public async Task<IActionResult> DeleteReaction(int idReaction)
    //    {
    //        try
    //        {
    //            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
    //            var Reaction = await _daoReaction.GetReactionById(idReaction, activeStatus);
    //            if (Reaction == null)
    //            {
    //                return NotFound(new ErrorResponseDTO
    //                {
    //                    success = false,
    //                    message = "No se encontró el usuario con el Id: " + idReaction
    //                });
    //            }
    //            Reaction.EntityStatus = EntitiesLibrary.Common.EntityStatus.Deleted;

    //            await _daoReaction.UpdateReaction(Reaction);

    //            return Ok(new ResponseDTO
    //            {
    //                success = true,
    //                message = "Usuario eliminado correctamente"
    //            });
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(new ErrorResponseDTO
    //            {
    //                success = false,
    //                message = "Error al actualizar el usuario: " + ex.Message
    //            });
    //        }
    //    }


    //    // agregar imagen desde el ordenador -------------------------------------------
    //    [HttpPost("subir-imagen")]
    //    public async Task<IActionResult> UploadReactionImage([FromForm] UploadFileDTO imageDTO)
    //    {
    //        try
    //        {
    //            // Validar si la imagen se proporciona correctamente
    //            if (imageDTO.image == null || imageDTO.image.Length == 0)
    //            {
    //                return BadRequest(new ErrorResponseDTO
    //                {
    //                    success = false,
    //                    message = "No se proporcionó ninguna imagen o el archivo está vacío."
    //                });
    //            }

    //            // Verificar si el usuario existe
    //            var Reaction = await _daoReaction.GetReactionById(imageDTO.ReactionId);
    //            if (Reaction == null)
    //            {
    //                return NotFound(new ErrorResponseDTO
    //                {
    //                    success = false,
    //                    message = $"No se encontró el usuario con el Id: {imageDTO.ReactionId}"
    //                });
    //            }

    //            // Validar la extensión del archivo
    //            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
    //            var extension = Path.GetExtension(imageDTO.image.FileName).ToLower();
    //            if (!allowedExtensions.Contains(extension))
    //            {
    //                return BadRequest(new ErrorResponseDTO
    //                {
    //                    success = false,
    //                    message = "El formato del archivo no es válido. Solo se permiten .jpg, .jpeg, .png, .gif."
    //                });
    //            }

    //            // Preparar el directorio de almacenamiento
    //            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Reactions");
    //            if (!Directory.Exists(uploadPath))
    //            {
    //                Directory.CreateDirectory(uploadPath);
    //            }

    //            // Generar un nombre único para la imagen
    //            var imageFileName = $"{imageDTO.ReactionId}_{Guid.NewGuid()}{extension}";
    //            var imagePath = Path.Combine(uploadPath, imageFileName);

    //            // Guardar la imagen físicamente en el servidor
    //            using (var fileStream = new FileStream(imagePath, FileMode.Create))
    //            {
    //                await imageDTO.image.CopyToAsync(fileStream);
    //            }

    //            // Actualizar la información del usuario con la nueva ruta de imagen
    //            Reaction.ProfileImagePath = $"/images/Reactions/{imageFileName}";
    //            await _daoReaction.UpdateReaction(Reaction);

    //            return Ok(new ResponseDTO
    //            {
    //                success = true,
    //                message = "La imagen fue subida exitosamente.",
    //                data = new { ReactionId = imageDTO.ReactionId, imagePath = Reaction.ProfileImagePath }
    //            });
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(new ErrorResponseDTO
    //            {
    //                success = false,
    //                message = "Error al subir la imagen: " + ex.Message
    //            });
    //        }
    //    }



}

