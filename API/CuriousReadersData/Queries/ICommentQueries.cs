using CuriousReadersData.Entities;

namespace CuriousReadersData.Queries
{
    public interface ICommentQueries
    {
        IEnumerable<Comment> GetComments(int bookId, int skip, int commentsPerPage);

        IEnumerable<Comment> GetUnapprovedComments(int page, int commentsPerPage);

        int CountUnapproved();
    }
}
