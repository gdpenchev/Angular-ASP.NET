namespace CuriousReadersService.Services.Comments;
using CuriousReadersData.Dto.Comments;
using CuriousReadersData.Entities;

public interface ICommentService
{
    Comment Create(CreateCommentModel model);

    IEnumerable<ReadCommentModel> GetComments(int bookId, int skip, int commentsPerPage);

    IEnumerable<ReadCommentModel> GetUnapprovedComments(int page, int commentsPerPage);

    void Approve(int commentId);

    int CountUnapproved();
}
