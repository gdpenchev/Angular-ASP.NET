using AutoMapper;
using CuriousReadersData.Commands;
using CuriousReadersData.Dto.Comments;
using CuriousReadersData.Entities;
using CuriousReadersData.Queries;
using System.Globalization;

namespace CuriousReadersService.Services.Comments;

public class CommentService : ICommentService
{
    private readonly ICommentCommands commentCommands;
    private readonly ICommentQueries commentQueries;
    private readonly IMapper mapper;

    public CommentService(ICommentCommands commentCommands, ICommentQueries commentQueries, IMapper mapper)
    {
        this.commentCommands = commentCommands;
        this.commentQueries = commentQueries;
        this.mapper = mapper;
    }

    public void Approve(int commentId)
    {
        this.commentCommands.Approve(commentId);
    }

    public int CountUnapproved()
    {
        return this.commentQueries.CountUnapproved();
    }

    public Comment Create(CreateCommentModel model)
    {
       
        var comment = mapper.Map<CreateCommentModel, Comment>(model);

        comment.CreatedOn = DateTime.Now;
        comment.IsAproved = false;

        return this.commentCommands.Create(comment);
    }

    public IEnumerable<ReadCommentModel> GetComments(int bookId, int skip, int commentsPerPage)
    {
        var comments = this.commentQueries.GetComments(bookId, skip, commentsPerPage);

        if (comments is null)
        {
            return null;
        }

        return mapper.Map<IEnumerable<Comment>, List<ReadCommentModel>>(comments);
    }

    public IEnumerable<ReadCommentModel> GetUnapprovedComments(int page, int commentsPerPage)
    {
        var comments = this.commentQueries.GetUnapprovedComments(page, commentsPerPage);

        if (comments is null)
        {
            return null;
        }

        return mapper.Map<IEnumerable<Comment>, List<ReadCommentModel>>(comments);
    }
}
