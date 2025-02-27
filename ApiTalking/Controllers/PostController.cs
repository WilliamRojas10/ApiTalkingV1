using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EntitiesLibrary.Post;
using DaoLibrary.Interfaces.Post;
using ApiTalking.DTOs.common;
using ApiTalking.DTOs.Post;
using ApiTalking.Helpers;
using DaoLibrary.Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ApiTalking.Service;
using DaoLibrary.Interfaces.Reaction;
using Microsoft.Extensions.Hosting;

namespace ApiTalking.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IDAOPost _daoPost;
        private readonly IDAOUser _daoUser;
        private readonly FileService _fileService;
        private readonly IDAOReaction _daoReaction;



    public PostController(IDAOPost daoPost, IDAOUser daoUser, FileService fileService, IDAOReaction daoReaction)
        {
            _daoPost = daoPost;
            _daoUser = daoUser;
            _daoReaction = daoReaction;
        _fileService = fileService;
        }


    [HttpGet("paginado")]
    public async Task<IActionResult> GetPosts(int page, int pageSize, string orden = "desc")
    {
        try
        {
            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
            (var posts, int totalRecords) = await _daoPost.GetPostsPaged(
                page,
                pageSize,
                activeStatus,
                orden
            );

            if (posts == null || !posts.Any())
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontraron los posteos"
                });
            }

            var listPostsDTO = new List<ResponsePostDTO>();

            foreach (var post in posts)
            {
                var reactions = await _daoReaction.GetAllReactionsByIdPost(post.Id);

                listPostsDTO.Add(new ResponsePostDTO
                {
                    idPost = post.Id,
                    description = post.Description,
                    registrationDateTime = post.RegistrationDateTime.ToString(),
                    reactions = reactions,
                    idUser = post.User.Id,
                    nameUser = post.User.Name,
                    lastNameUser = post.User.LastName,
                    idFile = post.File?.Id,
                    path = post.File?.Path
                });
            }

            return Ok(new ResponseDTO
            {
                success = true,
                message = "Lista de posteos paginado obtenido correctamente",
                data = new
                {
                    totalRecords,
                    posts = listPostsDTO
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al obtener posteos paginado GetPosts(): " + ex.Message
            });
        }
    }

    [Authorize(Roles = "Administrator, User")]
    [HttpGet("obtener-mis-posteos")]
    public async Task<IActionResult> GetMyPosts(int page, int pageSize, string orden = "desc")
    {
        try
        {
            EntitiesLibrary.File.File file = null;
            var userIdClaim = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new ErrorResponseDTO { success = false, message = "Usuario no autenticado." });

            int userId = int.Parse(userIdClaim);

            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
            (var posts, int totalRecords) = await _daoPost.GetUserPostsPaged(
                page,
                pageSize,
                activeStatus,
                orden,
                userId
            );

            if (posts == null || !posts.Any())
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontraron los posteos"
                });
            }

            var listPostsDTO = new List<ResponsePostDTO>();

            foreach (var post in posts)
            {
                var reactions = await _daoReaction.GetAllReactionsByIdPost(post.Id);

                listPostsDTO.Add(new ResponsePostDTO
                {
                    idPost = post.Id,
                    description = post.Description,
                    registrationDateTime = post.RegistrationDateTime.ToString(),
                    reactions = reactions,
                    idUser = post.User.Id,
                    nameUser = post.User.Name,
                    lastNameUser = post.User.LastName,
                    idFile = post.File?.Id,
                    path = post.File?.Path
                });
            }

            return Ok(new ResponseDTO
            {
                success = true,
                message = "Lista de posteos propios obtenido correctamente",
                data = new
                {
                    totalRecords,
                    posts = listPostsDTO
                }
                
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al obtener posteos paginado GetPosts(): " + ex.Message
            });
        }
    }


    [Authorize(Roles = "Administrator, User")]
    [HttpGet("{idPost}")]
        public async Task<IActionResult> GetPostById(int idPost)
        {
            try
            {
                var post = await _daoPost.GetPostById(idPost);
                if (post == null)
                {
                    return BadRequest(new ErrorResponseDTO
                    {
                        success = false,
                        message = "No se encontró el usuario con el Id: " + idPost
                    });
                }
                return Ok(new ResponsePostDTO
                {
                    idPost = post.Id,
                    nameUser = post.User.Name,
                    lastNameUser = post.User.LastName,
                    idUser = post.User.Id,
                    idFile = post.File.Id,
                    registrationDateTime = Converter.convertDateTimeToString(post.RegistrationDateTime)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "Error al obtener un post usando GetPostById(): " + ex.Message
                });
            }
        }

    
    [Authorize(Roles = "Administrator, User")]
    [HttpPut("modificar/{idPost}")]
    public async Task<IActionResult> UpdatePost(int idPost, [FromBody] RequestPostDTO postDTO)
        {
            try
            {
                var activeStatus = EntitiesLibrary.Common.EntityStatus.Active; 
                if (postDTO == null)
                {
                    return BadRequest(new ErrorResponseDTO
                    {
                        success = false,
                        message = "Datos del post no válidos"
                    });
                }
                var post = await _daoPost.GetPostById(idPost, activeStatus);
                if (post == null)
                {
                    return NotFound(new ErrorResponseDTO
                    {
                        success = false,
                        message = "No se encontró el post con el Id: " + idPost
                    });
                }

                post.Description = postDTO.description;
                //TODO: Se tiene que obtener por id de file
                //post.File = postDTO.idFile;
               // post.EntityStatus= (EntitiesLibrary.Common.EntityStatus)postDTO.postStatus;//TODO CAMBIA EL ATRIBUTO DEL DTO


                await _daoPost.UpdatePost(post);

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
    [HttpPut("bloquear/{idPost}")]
    public async Task<IActionResult> BlockPost(int idPost)
        {
            try
            {
                var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
                var deletedStatus = EntitiesLibrary.Common.EntityStatus.Deleted;
                var post = await _daoPost.GetPostById(idPost, activeStatus);
                if (post == null || post.EntityStatus == deletedStatus)
                {
                    return NotFound(new ErrorResponseDTO
                    {
                        success = false,
                        message = "No se encontró el post con el Id: " + idPost
                    });
                }
                post.EntityStatus = EntitiesLibrary.Common.EntityStatus.Blocked;
                await _daoPost.UpdatePost(post);

                return Ok(new ResponseDTO
                {
                    success = true,
                    message = "Post bloqueado correctamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "Error al bloquear post: " + ex.Message
                });
            }
        }


    [Authorize(Roles = "Administrator")]
    [HttpPut("activar/{idPost}")]
    public async Task<IActionResult> ActivatePost(int idPost)
        {
            try
            {
                var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
                var deletedStatus = EntitiesLibrary.Common.EntityStatus.Deleted;
                var post = await _daoPost.GetPostById(idPost);
                if (post == null)
                {
                    return NotFound(new ErrorResponseDTO
                    {
                        success = false,
                        message = "No se encontró el post con el Id: " + idPost
                    });
                }
                if (post.EntityStatus == activeStatus)
                {
                    return NotFound(new ErrorResponseDTO
                    {
                        success = false,
                        message = "No se puede activar, el post se encuentra activo"
                    });
                }
                post.EntityStatus = EntitiesLibrary.Common.EntityStatus.Active;
                await _daoPost.UpdatePost(post);

                return Ok(new ResponseDTO
                {
                    success = true,
                    message = "Post activado correctamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "Error al activar el post: " + ex.Message
                });
            }
        }


    [Authorize(Roles = "Administrator, User")]
    [HttpDelete("{idPost}")]
    public async Task<IActionResult> DeletePost(int idPost)
        {
            try
                {
                var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
                var post = await _daoPost.GetPostById(idPost, activeStatus);
                if (post == null)
                {
                    return NotFound(new ErrorResponseDTO
                    {
                        success = false,
                        message = "No se encontró el post con el Id: " + idPost
                    });
                }
                post.EntityStatus = EntitiesLibrary.Common.EntityStatus.Deleted;

                await _daoPost.UpdatePost(post);

                return Ok(new ResponseDTO
                {
                    success = true,
                    message = "Post eliminado correctamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "Error al eliminar el post: " + ex.Message
                });
            }
        }


    [Authorize(Roles = "Administrator, User")]
    [HttpPost]
    public async Task<IActionResult> CreatePost ([FromForm] RequestPostDTO requestPostDTO)
    {
        try
        {
            EntitiesLibrary.File.File file = null;
            var userIdClaim = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new ErrorResponseDTO { success = false, message = "Usuario no autenticado." });

            int userId = int.Parse(userIdClaim);
            var user = await _daoUser.GetUserById(userId);
            if (user == null)
                return NotFound(new ErrorResponseDTO { success = false, message = $"Usuario con ID {userId} no encontrado." });

            if (requestPostDTO.image != null)
            {
                file = await _fileService.SaveImage(requestPostDTO.image, userId, "Posts");
            }
         
            var post = new EntitiesLibrary.Post.Post
            {
                Description = requestPostDTO.description,
                User = user,
                EntityStatus = EntitiesLibrary.Common.EntityStatus.Active,
                File = file ?? null,

            };
            await _daoPost.AddPost(post);

            return Ok(new ResponseDTO
            {
                success = true,
                message = "Post subido exitosamente.",
                data = new { postId = post.Id, imagePath = file?.Path }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO { success = false, message = "Error al subir post: " + ex.Message });
        }
    }


}