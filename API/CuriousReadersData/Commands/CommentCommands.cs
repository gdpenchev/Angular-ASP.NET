using CuriousReadersData.Entities;

namespace CuriousReadersData.Commands;

public class CommentCommands : ICommentCommands
{
    private readonly LibraryDbContext libraryDbContext;

    public CommentCommands(LibraryDbContext libraryDbContext)
    {
        this.libraryDbContext = libraryDbContext;
    }

    public void Approve(int commentId)
    {
        var comment = this.libraryDbContext.Comments.Where(c => c.Id == commentId).FirstOrDefault();

        comment.IsAproved = true;

        this.libraryDbContext.Update(comment);
        this.libraryDbContext.SaveChanges();
    }

    public Comment Create(Comment comment)
    {
        this.libraryDbContext.Comments.Add(comment);
        this.libraryDbContext.SaveChanges();
        return comment;
    }
}
