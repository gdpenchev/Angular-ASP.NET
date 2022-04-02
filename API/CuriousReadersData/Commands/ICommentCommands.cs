using CuriousReadersData.Entities;

namespace CuriousReadersData.Commands;

public interface ICommentCommands
{
    Comment Create(Comment comment);

    void Approve(int commentId);
}
