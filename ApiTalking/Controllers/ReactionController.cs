//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using EntitiesLibrary.Reaction;
//using DaoLibrary.Interfaces.Reaction;
//using ApiTalking.DTOs.common;
//using ApiTalking.DTOs.Reaction;
//using ApiTalking.Helpers;
//using ApiTalking.DTOs.Post;
//using Microsoft.AspNetCore.Authorization;

//namespace ApiTalking.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public class ReactionReactionController : ControllerBase
//{
//    private readonly IDAOReaction _daoReaction;


//    public ReactionController(IDAOReaction daoReaction)
//    {
//        _daoReaction = daoReaction;
//    }

//    [Authorize(Roles = "Administrator")]
//    [HttpGet("paginado")]
//    public async Task<IActionResult> GetReactions(int page, int pageSize)
//    {
//        try
//        {
//            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
//            (var Reactions, int totalRecords) = await _daoReaction.GetReactionsPaged
//            (
//            page,
//            pageSize,
//            activeStatus
//            );
//            if (Reactions == null || !Reactions.Any())
//            {
//                return BadRequest(new ErrorResponseDTO
//                {
//                    sucess = false,
//                    message = "No se encontraron usuarios"
//                });
//            }
//            var ReactionDTO = Reactions.Select(Reaction => new ResponseReactionDTO
//            {
//                idReaction = Reaction.Id,
//                name = Reaction.Name,
//                lastName = Reaction.LastName,
//                email = Reaction.Email,
//                birthDate = Reaction.BirthDate.ToString(),
//                nationality = Reaction.Nationality,
//                province = Reaction.Province
//            });
//            return Ok(new
//            {
//                totalRecords = totalRecords,
//                Reactions = ReactionDTO
//            });
//        }
//        catch (Exception ex)
//        {
//            return BadRequest(new ErrorResponseDTO
//            {
//                sucess = false,
//                message = "Error en getReactions(): " + ex.Message
//            });
//        }
//    }

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
//                    sucess = false,
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
//                sucess = false,
//                message = "Error al obtener un usuario usando GetReactionById(): " + ex.Message
//            });
//        }
//    }


//    [HttpPost]
//    public async Task<IActionResult> CreateReaction([FromBody] RequestReactionDTO ReactionDTO)
//    {
//        try
//        {
//            if (ReactionDTO == null)
//            {
//                return BadRequest(new ErrorResponseDTO
//                {
//                    sucess = false,
//                    message = "Datos del usuario no válidos"
//                });
//            }

//            var Reaction = new Reaction
//            {
//                Name = ReactionDTO.name,
//                LastName = ReactionDTO.lastName,
//                Email = ReactionDTO.email,
//                Password = ReactionDTO.password,
//                BirthDate = Converter.convertStringToDateOnly(ReactionDTO.birthDate),
//                Nationality = ReactionDTO.nationality,
//                Province = ReactionDTO.province,
//                EntityStatus = EntitiesLibrary.Common.EntityStatus.Active,
//                ReactionType = EntitiesLibrary.Reaction.ReactionType.Reaction
//            };

//            await _daoReaction.AddReaction(Reaction);

//            return Ok(new ResponseDTO
//            {
//                sucess = true,
//                message = "Usuario guardado correctamente"
//            });
//        }
//        catch (Exception ex)
//        {
//            return BadRequest(new ErrorResponseDTO
//            {
//                sucess = false,
//                message = "Error al crear el usuario: " + ex.Message
//            });
//        }
//    }


//    [HttpPut("modificar/{idReaction}")]
//    public async Task<IActionResult> UpdateReaction(int idReaction, [FromBody] RequestReactionDTO ReactionDTO)
//    {
//        try
//        {
//            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
//            if (ReactionDTO == null)
//            {
//                return BadRequest(new ErrorResponseDTO
//                {
//                    sucess = false,
//                    message = "Datos del usuario no válidos"
//                });
//            }

//            var Reaction = await _daoReaction.GetReactionById(idReaction, activeStatus);
//            if (Reaction == null)
//            {
//                return NotFound(new ErrorResponseDTO
//                {
//                    sucess = false,
//                    message = "No se encontró el usuario con el Id: " + idReaction
//                });
//            }
//            Reaction.Name = ReactionDTO.name;
//            Reaction.LastName = ReactionDTO.lastName;
//            Reaction.Email = ReactionDTO.email;
//            Reaction.BirthDate = Converter.convertStringToDateOnly(ReactionDTO.birthDate);
//            Reaction.Nationality = ReactionDTO.nationality;
//            Reaction.Province = ReactionDTO.province;

//            await _daoReaction.UpdateReaction(Reaction);

//            return Ok(new ResponseDTO
//            {
//                sucess = true,
//                message = "Usuario actualizado correctamente"
//            });
//        }
//        catch (Exception ex)
//        {
//            return BadRequest(new ErrorResponseDTO
//            {
//                sucess = false,
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
//                    sucess = false,
//                    message = "No se encontró el usuario con el Id: " + idReaction
//                });
//            }
//            Reaction.EntityStatus = EntitiesLibrary.Common.EntityStatus.Blocked;

//            await _daoReaction.UpdateReaction(Reaction);

//            return Ok(new ResponseDTO
//            {
//                sucess = true,
//                message = "Usuario eliminado correctamente"
//            });
//        }
//        catch (Exception ex)
//        {
//            return BadRequest(new ErrorResponseDTO
//            {
//                sucess = false,
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
//                    sucess = false,
//                    message = "No se encontró el usuario con el Id: " + idReaction
//                });
//            }
//            Reaction.EntityStatus = EntitiesLibrary.Common.EntityStatus.Active;

//            await _daoReaction.UpdateReaction(Reaction);

//            return Ok(new ResponseDTO
//            {
//                sucess = true,
//                message = "Usuario activado correctamente"
//            });
//        }
//        catch (Exception ex)
//        {
//            return BadRequest(new ErrorResponseDTO
//            {
//                sucess = false,
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
//                    sucess = false,
//                    message = "No se encontró el usuario con el Id: " + idReaction
//                });
//            }
//            Reaction.EntityStatus = EntitiesLibrary.Common.EntityStatus.Deleted;

//            await _daoReaction.UpdateReaction(Reaction);

//            return Ok(new ResponseDTO
//            {
//                sucess = true,
//                message = "Usuario eliminado correctamente"
//            });
//        }
//        catch (Exception ex)
//        {
//            return BadRequest(new ErrorResponseDTO
//            {
//                sucess = false,
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
//                    sucess = false,
//                    message = "No se proporcionó ninguna imagen o el archivo está vacío."
//                });
//            }

//            // Verificar si el usuario existe
//            var Reaction = await _daoReaction.GetReactionById(imageDTO.ReactionId);
//            if (Reaction == null)
//            {
//                return NotFound(new ErrorResponseDTO
//                {
//                    sucess = false,
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
//                    sucess = false,
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
//                sucess = true,
//                message = "La imagen fue subida exitosamente.",
//                data = new { ReactionId = imageDTO.ReactionId, imagePath = Reaction.ProfileImagePath }
//            });
//        }
//        catch (Exception ex)
//        {
//            return BadRequest(new ErrorResponseDTO
//            {
//                sucess = false,
//                message = "Error al subir la imagen: " + ex.Message
//            });
//        }
//    }



//}

