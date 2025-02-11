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

namespace ApiTalking.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IDAOPost _daoPost;
        private readonly IDAOUser _daoUser;
        private readonly FileService _fileService;


    public PostController(IDAOPost daoPost, IDAOUser daoUser, FileService fileService)
        {
            _daoPost = daoPost;
            _daoUser = daoUser;
        _fileService = fileService;
        }

        
        [HttpGet("paginado")]
        public async Task<IActionResult> GetPosts(int page, int pageSize)
        {
            try
            {
                var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
                (var posts, int totalRecords) = await _daoPost.GetPostsPaged
                (
                page,
                pageSize,
                activeStatus
                );
                if (posts == null || !posts.Any())
                {
                    return BadRequest(new ErrorResponseDTO
                    {
                        success = false,
                        message = "No se encontraron los posteos"
                    });
                }
                var postDTO = posts.Select(post => new ResponsePostDTO
                {
                    idPost = post.Id,
                    description = post.Description,
                    registrationDateTime = post.RegistrationDateTime.ToString(),
                    idUser = post.User.Id,
                    nameUser = post.User.Name,
                    lastNameUser = post.User.LastName,
                    idFile = post.File.Id,
                    path = post.File.Path
                });
                return Ok(new
                {
                    totalRecords = totalRecords,
                    posts = postDTO
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

        [HttpGet("{idUser}")]
        public async Task<IActionResult> GetUserById(int idPost)
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
        //[Authorize( "Administrator")]
        //[Authorize("User")]
        //[HttpPost("Post antiguo")]
        //public async Task<IActionResult> CreatePost([FromBody] RequestPostDTO postDTO)
        //{
        //    try
        //    {
        //        var userId = User.FindFirstValue("UserId"); // Usamos "UserId" que se agregó en los claims
        //        //var userRole = User.FindFirstValue(ClaimTypes.Role); // Obtener el rol (por ejemplo, "user")
        //        //var userEmail = User.FindFirstValue(ClaimTypes.Name); // Obtener el email del administrador

        //        int idUserLogin = Int32.Parse(userId);
        //        if (postDTO == null)
        //        {
        //            return BadRequest(new ErrorResponseDTO
        //            {
        //                success = false,
        //                message = "Datos del usuario no válidos"
        //            });
        //        }
        //        var userStatusActive = EntitiesLibrary.Common.EntityStatus.Active;
        //        var post = new Post
        //        {
        //            Description = postDTO.description,
        //            EntityStatus = EntitiesLibrary.Common.EntityStatus.Active,
        //            User = await _daoUser.GetUserById(idUserLogin, userStatusActive),
        //            //TODO: Se tiene que obtener por id de file
        //            //File = postDTO.idFile

        //        };
        //        await _daoPost.AddPost(post);


        //        return Ok(new ResponseDTO
        //        {
        //            success = true,
        //            message = "Post creado correctamente"
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ErrorResponseDTO
        //        {
        //            success = false,
        //            message = "Error al crear un post: " + ex.Message
        //        });
        //    }
        //}

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

        [HttpPut("activar/{idPost}")]
        public async Task<IActionResult> ActivatePost(int idPost)
        {
            try
            {
                var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
                var deletedStatus = EntitiesLibrary.Common.EntityStatus.Deleted;
                var post = await _daoPost.GetPostById(idPost, activeStatus);
                if (post == null || post.EntityStatus != deletedStatus)
                {
                    return NotFound(new ErrorResponseDTO
                    {
                        success = false,
                        message = "No se encontró el usuario con el Id: " + idPost
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

        [HttpPut("eliminar/{idPost}")]
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
            var userIdClaim = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new ErrorResponseDTO { success = false, message = "Usuario no autenticado." });

            int userId = int.Parse(userIdClaim);
            var user = await _daoUser.GetUserById(userId);
            if (user == null)
                return NotFound(new ErrorResponseDTO { success = false, message = $"Usuario con ID {userId} no encontrado." });

            if (requestPostDTO.image == null)
                return BadRequest(new ErrorResponseDTO { success = false, message = "No se proporcionó una imagen." });

            // Guardar la imagen y obtener la entidad `PublishedFile`
            var file = await _fileService.SaveImage(requestPostDTO.image, userId, "Posts");

            // Crear el post
            var post = new EntitiesLibrary.Post.Post
            {
                Description = requestPostDTO.description,
                User = user,
                EntityStatus = EntitiesLibrary.Common.EntityStatus.Active,
                File = file
            };
            await _daoPost.AddPost(post);

            return Ok(new ResponseDTO
            {
                success = true,
                message = "Post subido exitosamente.",
                data = new { postId = post.Id, imagePath = file.Path }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO { success = false, message = "Error al subir post: " + ex.Message });
        }
    }


    // agregar imagen desde el ordenador -------------------------------------------
    [Authorize(Roles = "Administrator, User")]

    [HttpPost("upload-post")]
public async Task<IActionResult> UploadPost ([FromForm] RequestPostDTO requestPostDTO)
{
    try
    {
            var userId = User.FindFirstValue("UserId"); // Usamos "UserId" que se agregó en los claims
                                                        //var userRole = User.FindFirstValue(ClaimTypes.Role); // Obtener el rol (por ejemplo, "user")
                                                        //var userEmail = User.FindFirstValue(ClaimTypes.Name); // Obtener el email del administrador

            int idUserLogin = Int32.Parse(userId);
            // Validar si se proporcionó la imagen
            if (requestPostDTO.image == null || requestPostDTO.image == null || requestPostDTO.image.Length == 0)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "No se proporcionó ninguna imagen o el archivo está vacío."
            });
        }

        // Verificar si el usuario existe
        var user = await _daoUser.GetUserById(idUserLogin);
        if (user == null)
        {
            return NotFound(new ErrorResponseDTO
            {
                success = false,
                message = $"No se encontró el usuario con el Id: {idUserLogin}"
            });
        }

        // Validar la extensión del archivo
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(requestPostDTO.image.FileName).ToLower();
        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "El formato del archivo no es válido. Solo se permiten .jpg, .jpeg, .png, .gif."
            });
        }

        // Definir el directorio de almacenamiento
        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "FIlesSystem", "Images", "Posts");
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        // Crear un nombre único para la imagen
        var imageFileName = $"{idUserLogin}_{Guid.NewGuid()}{extension}";
        var imagePath = Path.Combine(uploadPath, imageFileName);

        // Guardar la imagen físicamente en el servidor
        using (var fileStream = new FileStream(imagePath, FileMode.Create))
        {
            await requestPostDTO.image.CopyToAsync(fileStream);
        }

        // Crear una entidad para almacenar la imagen
        var fileEntity = new EntitiesLibrary.File.File
        {
            Id = 0, 
            Name = imageFileName,
            Path = $"/FilesSystem/Images/Posts/{imageFileName}",
            EntityStatus = EntitiesLibrary.Common.EntityStatus.Active,
            Type = new EntitiesLibrary.File.FileType
            {
                Id = 0, 
                TypeFile = "image"
            }
            
        };
        //await _daoFile.AddFile(fileEntity); // Guardar en la base de datos

        // Crear el post con la imagen asociada
        var post = new Post
        {
            Description = requestPostDTO.description,
            EntityStatus = EntitiesLibrary.Common.EntityStatus.Active,
            User = user,
            File = fileEntity
        };
        await _daoPost.AddPost(post);

        return Ok(new ResponseDTO
        {
            success = true,
            message = "Posteo subido exitosamente.",
            //data = new { userId = requestPostDTO.idUser, imagePath = fileEntity.FilePath }
        });
    }
    catch (Exception ex)
    {
        return BadRequest(new ErrorResponseDTO
        {
            success = false,
            message = "Error al subir un posteo: " + ex.Message
        });
    }
}
    }