

// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using EntitiesLibrary.Entities;
// using EntitiesLibrary.Entities.Enum;
// using ApiTalking.DTO.common;
// using ApiTalking.DTO.Reaction;
// using dao_library;


// namespace ApiTalking.Controllers;

// [ApiController]
// [Route("[controller]")]
// public class ReactionController : ControllerBase
// {

//     private readonly MyDbContext _context;
//     public ReactionController(MyDbContext context)
//     {
//         _context = context;
//     }

//     [HttpGet("{postId}/reaction")]
//     public async Task<ActionResult<IEnumerable<ResponseReactionDTO>>> GetAllReactionsByPostId(int postId)
//     {
//         try
//         {
//             // Verifica si el Post existe
//             var postExists = await _context.Posts.AnyAsync(p => p.Id == postId);
//             if (!postExists)
//             {
//                 return NotFound($"No se encontró el post con id: {postId}");
//             }

//             // Obtiene todas las reacciones asociadas al post
//             var reactions = await _context.Reactions
//                 .Where(r => r.IdPost == postId)
//                 .Select(rDB => new ResponseReactionDTO
//                 {
//                     idUser = rDB.IdUser,
//                     idPost = rDB.IdPost,
//                     reactionStatus = rDB.ReactionStatus.ToString()
//                 })
//                 .ToListAsync();

//             if (reactions == null || reactions.Count == 0)
//             {
//                 return NotFound($"No se encontró ninguna reacción para el post con id: {postId}");
//             }

//             return Ok(reactions);
//         }
//         catch (Exception ex)
//         {
//             return BadRequest(new ErrorResponseDTO
//             {
//                 sucess = false,
//                 message = ex.Message
//             });
//         }
//     }



//     [HttpPost]
//     public async Task<ActionResult> CreateReaction([FromBody] RequestReactionDTO reaction)
//     {
//         try
//         {
//             if (reaction == null)
//             {
//                 return BadRequest(new ErrorResponseDTO
//                 {
//                     sucess = false,
//                     message = "Datos ingresados erroneos"
//                 });
//             }
//             // Verificar que IdUser y IdPost existan en la base de datos
//             var userExists = await _context.Users.AnyAsync(u => u.Id == reaction.idUser);
//             var postExists = await _context.Posts.AnyAsync(p => p.Id == reaction.idPost);

//             if (!userExists || !postExists)
//             {
//                 return BadRequest(new ErrorResponseDTO
//                 {
//                     sucess = false,
//                     message = "El usuario o el post no existen."
//                 });
//             }
//             var reactionSaved = new Reaction
//             {
//                 Id = 0,
//                 ReactionStatus = reaction.reactionStatus,
//                 IdUser = reaction.idUser,
//                 IdPost = reaction.idPost
//             };
//             _context.Reactions.Add(reactionSaved);
//             await _context.SaveChangesAsync();

//             return Ok(new ResponseDTO
//             {
//                 sucess = true,
//                 message = "Reaccion creada."
//             });
//         }
//         catch (DbUpdateException dbEx)
//         {
//             return BadRequest(new ErrorResponseDTO
//             {
//                 sucess = false,
//                 message = $"Error al guardar los cambios: {dbEx.InnerException?.Message ?? dbEx.Message}"
//             });
//         }
//         catch (Exception ex)
//         {
//             return BadRequest(new ErrorResponseDTO
//             {
//                 sucess = false,
//                 message = ex.Message
//             });
//         }
//     }



//     [HttpDelete("{id}")]
//     public IActionResult DeleteReaction(int id)
//     {
//         var reaction = _context.Reactions
//         .FirstOrDefault(r => r.Id == id &&
//         Enum.IsDefined(typeof(ReactionStatus), r.ReactionStatus));
//         // r.ReactionStatus == ReactionStatus.Celebrar ||
//         // r.ReactionStatus == ReactionStatus.MeInteresa ||
//         // r.ReactionStatus == ReactionStatus.Recomendar ||
//         // r.ReactionStatus == ReactionStatus.NoMeInteresa);
//         if (reaction == null)
//         {
//             return NotFound();
//         }

//         _context.Reactions.Remove(reaction);
//         return NoContent();
//     }
// }