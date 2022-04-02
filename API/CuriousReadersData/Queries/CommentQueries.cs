using CuriousReadersData.Entities;
using Microsoft.EntityFrameworkCore;
using CuriousReadersData.Queries;

namespace CuriousReadersData.Queries
{
    public class CommentQueries : ICommentQueries
    {
        private readonly LibraryDbContext libraryDbContext;

        public CommentQueries(LibraryDbContext libraryDbContext)
        {
            this.libraryDbContext = libraryDbContext;
        }

        public int CountUnapproved()
        {
            return this.libraryDbContext.Comments
                .Where(c => !c.IsAproved)
                .Count();
        }

        public IEnumerable<Comment> GetComments(int bookId, int skip, int commentsPerPage)
        {
            var comments = this.libraryDbContext.Comments
                .Where(c => c.BookId == bookId && c.IsAproved)
                .OrderByDescending(c => c.CreatedOn)
                .Skip(skip * commentsPerPage)
                .Take(commentsPerPage)
                .ToList();

            return comments;
        }

        public IEnumerable<Comment> GetUnapprovedComments(int page, int commentsPerPage)
        {
            var pageNumber = page <= 0 ? 1 : page;

            return this.libraryDbContext.Comments
                .Where(c => !c.IsAproved)
                .Include(c => c.Book)
                .OrderByDescending(c => c.CreatedOn)
                .Skip((pageNumber - 1) * commentsPerPage)
                .Take(commentsPerPage)
                .ToList();
        }
    }
}
