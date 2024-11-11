using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EntitiesLibrary.Post;
using DaoLibrary.Interfaces.Post;
using ApiTalking.DTO.common;
using ApiTalking.DTO.Post;
using ApiTalking.Helpers;
using DaoLibrary.Interfaces.User;

namespace ApiTalking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IDAOPost _daoPost;
        private readonly IDAOUser _daoUser;

        public PostController(IDAOPost daoPost, IDAOUser daoUser)
        {
            _daoPost = daoPost;
            _daoUser = daoUser;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetPosts(int page, int pageSize)
        {
            try
            {
                var activeStatus = EntitiesLibrary.Post.PostStatus.Active;
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
                        sucess = false,
                        message = "No se encontraron los posteos"
                    });
                }
                var postDTO = posts.Select(post => new ResponsePostDTO
                {
                    idPost = post.Id,
                    description = post.Description,
                    registrationDateTime = Converter.convertDateTimeToString(post.RegistrationDateTime),
                    idUser = post.User.Id,
                    nameUser = post.User.Name,
                    lastNameUser = post.User.LastName,
                    idFile = post.File.Id
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
                    sucess = false,
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
                        sucess = false,
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
                    sucess = false,
                    message = "Error al obtener un post usando GetPostById(): " + ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] RequestPostDTO postDTO)
        {
            try
            {
                if (postDTO == null)
                {
                    return BadRequest(new ErrorResponseDTO
                    {
                        sucess = false,
                        message = "Datos del usuario no válidos"
                    });
                }
                var userStatusActive = EntitiesLibrary.User.UserStatus.Active;
                var post = new Post
                {
                    Description = postDTO.description,
                    PostStatus = EntitiesLibrary.Post.PostStatus.Active,
                    User = await _daoUser.GetUserById(postDTO.idUser, userStatusActive),
                    //TODO: Se tiene que obtener por id de file
                    //File = postDTO.idFile

                };
                await _daoPost.AddPost(post);


                return Ok(new ResponseDTO
                {
                    sucess = true,
                    message = "Post creado correctamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "Error al crear un post: " + ex.Message
                });
            }
        }

        [HttpPut("modificar/{idPost}")]
        public async Task<IActionResult> UpdatePost(int idPost, [FromBody] RequestPostDTO postDTO)
        {
            try
            {
                var activeStatus = EntitiesLibrary.Post.PostStatus.Active;
                if (postDTO == null)
                {
                    return BadRequest(new ErrorResponseDTO
                    {
                        sucess = false,
                        message = "Datos del post no válidos"
                    });
                }
                var post = await _daoPost.GetPostById(idPost, activeStatus);
                if (post == null)
                {
                    return NotFound(new ErrorResponseDTO
                    {
                        sucess = false,
                        message = "No se encontró el post con el Id: " + idPost
                    });
                }

                post.Description = postDTO.description;
                //TODO: Se tiene que obtener por id de file
                //post.File = postDTO.idFile;
                post.PostStatus = (EntitiesLibrary.Post.PostStatus)postDTO.postStatus;


                await _daoPost.UpdatePost(post);

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

        [HttpPut("bloquear/{idPost}")]
        public async Task<IActionResult> BlockPost(int idPost)
        {
            try
            {
                var activeStatus = EntitiesLibrary.Post.PostStatus.Active;
                var deletedStatus = EntitiesLibrary.Post.PostStatus.Deleted;
                var post = await _daoPost.GetPostById(idPost, activeStatus);
                if (post == null || post.PostStatus == deletedStatus)
                {
                    return NotFound(new ErrorResponseDTO
                    {
                        sucess = false,
                        message = "No se encontró el post con el Id: " + idPost
                    });
                }
                post.PostStatus = EntitiesLibrary.Post.PostStatus.Blocked;
                await _daoPost.UpdatePost(post);

                return Ok(new ResponseDTO
                {
                    sucess = true,
                    message = "Post bloqueado correctamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "Error al bloquear post: " + ex.Message
                });
            }
        }

        [HttpPut("activar/{idPost}")]
        public async Task<IActionResult> ActivatePost(int idPost)
        {
            try
            {
                var statusDeleted = EntitiesLibrary.Post.PostStatus.Deleted;
                var statusActive = EntitiesLibrary.Post.PostStatus.Active;
                var post = await _daoPost.GetPostById(idPost, statusActive);
                if (post == null || post.PostStatus != statusDeleted)
                {
                    return NotFound(new ErrorResponseDTO
                    {
                        sucess = false,
                        message = "No se encontró el usuario con el Id: " + idPost
                    });
                }
                post.PostStatus = EntitiesLibrary.Post.PostStatus.Active;
                await _daoPost.UpdatePost(post);

                return Ok(new ResponseDTO
                {
                    sucess = true,
                    message = "Post activado correctamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "Error al activar el post: " + ex.Message
                });
            }
        }

        [HttpPut("eliminar/{idPost}")]
        public async Task<IActionResult> DeletePost(int idPost)
        {
            try
            {
                var activeStatus = EntitiesLibrary.Post.PostStatus.Active;
                var post = await _daoPost.GetPostById(idPost, activeStatus);
                if (post == null)
                {
                    return NotFound(new ErrorResponseDTO
                    {
                        sucess = false,
                        message = "No se encontró el post con el Id: " + idPost
                    });
                }
                post.PostStatus = EntitiesLibrary.Post.PostStatus.Deleted;

                await _daoPost.UpdatePost(post);

                return Ok(new ResponseDTO
                {
                    sucess = true,
                    message = "Post eliminado correctamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "Error al eliminar el post: " + ex.Message
                });
            }
        }




    }



}
