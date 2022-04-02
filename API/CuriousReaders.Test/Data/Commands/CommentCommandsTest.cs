using CuriousReadersData;
using CuriousReadersData.Commands;
using CuriousReadersData.Entities;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CuriousReaders.Test.Data.Commands;

public class CommentCommandsTest
{
    private LibraryDbContext fakeDbContext = A.Fake<LibraryDbContext>();
    private Comment fakeComment = A.Fake<Comment>();

    private void SetupFakeCommentsDbSet(IQueryable<Comment> fakeIQueryable)
    {
        var fakeDbSet = A.Fake<DbSet<Comment>>((d =>
                 d.Implements(typeof(IQueryable<Comment>))));

        A.CallTo(() => ((IQueryable<Comment>)fakeDbSet).GetEnumerator())
            .Returns(fakeIQueryable.GetEnumerator());

        A.CallTo(() => ((IQueryable<Comment>)fakeDbSet).Provider)
            .Returns(fakeIQueryable.Provider);

        A.CallTo(() => ((IQueryable<Comment>)fakeDbSet).Expression)
            .Returns(fakeIQueryable.Expression);

        A.CallTo(() => ((IQueryable<Comment>)fakeDbSet).ElementType)
           .Returns(fakeIQueryable.ElementType);

        A.CallTo(() => fakeDbContext.Comments).Returns(fakeDbSet);
    }

    [Fact]
    public void CreateComment_ShouldCreate_NewComment()
    {
        //Arrange
        var fakeIQueryableComments = new List<Comment>()
        {
            new Comment() { Id = 1, BookId = 1,Content = "test", CreatedOn = DateTime.Now, Username = "Pesho", IsAproved = true, Rating = 5 },
        }
        .AsQueryable();

        SetupFakeCommentsDbSet(fakeIQueryableComments);

        var commentCommand = new CommentCommands(fakeDbContext);

        //Act
        var result = commentCommand.Create(fakeComment);

        //Assert
        A.CallTo(() => fakeDbContext.Comments.Add(fakeComment))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeDbContext.SaveChanges())
           .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void ApproveComment_ShouldApprove_Comment()
    {
        //Arrange
        var fakeIQueryableComments = new List<Comment>()
        {
            new Comment() { Id = 1, BookId = 1,Content = "test", CreatedOn = DateTime.Now, Username = "Pesho", IsAproved = true, Rating = 5 },
        }
        .AsQueryable();

        SetupFakeCommentsDbSet(fakeIQueryableComments);

        var commentCommand = new CommentCommands(fakeDbContext);

        var commentId = 1;
        var comment = fakeIQueryableComments.Where(c => c.Id == commentId).FirstOrDefault();

        //Act
        commentCommand.Approve(commentId);

        //Assert
        A.CallTo(() => fakeDbContext.Update(comment))
            .MustHaveHappened();
        A.CallTo(() => fakeDbContext.SaveChanges())
           .MustHaveHappenedOnceExactly();
    }

}
