namespace CuriousReadersAPI.Controllers;

using CuriousReadersData.Dto.Comments;
using CuriousReadersService.Services.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static CuriousReadersData.DataConstants;

[Route("api/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly ICommentService commentService;

    public CommentsController(ICommentService commentService)
    {
        this.commentService = commentService;
    }

    [HttpPost("Add")]
    [Authorize]
    public IActionResult Create(CreateCommentModel model)
    {

        var result = this.commentService.Create(model);

        if (result is null)
        {
            return BadRequest("Comment was not added!");
        }

        return Ok(model);
    }

    [HttpGet("All")]
    public ActionResult<IEnumerable<ReadCommentModel>> AllComments(int bookId, int skip, int commentsPerPage)
    {

        var result = this.commentService.GetComments(bookId, skip, commentsPerPage);

        if (result is null)
        {
            return BadRequest("No comments found!");
        }

        return Ok(result);
    }

    [HttpGet("Unapproved")]
    [Authorize(Roles = Librarian)]
    public ActionResult<IEnumerable<ReadCommentModel>> AllUnapprovedComments(int page, int commentsPerPage)
    {

        var result = this.commentService.GetUnapprovedComments(page, commentsPerPage);

        if (result is null)
        {
            return BadRequest("No comments found!");
        }

        return Ok(result);
    }
    [HttpPatch("ApproveComment")]
    [Authorize(Roles = Librarian)]
    public IActionResult Approve(int commentId)
    {
        this.commentService.Approve(commentId);

        return Ok();
    }
    [HttpGet("Count")]
    [Authorize(Roles = Librarian)]
    public IActionResult CountUnapproved()
    {
        var result = this.commentService.CountUnapproved();

        return Ok(result);
    }
}
