namespace CuriousReaders.Test.Services;

using AutoMapper;
using CuriousReadersData.Commands;
using CuriousReadersData.Dto.Comments;
using CuriousReadersData.Entities;
using CuriousReadersData.Profiles;
using CuriousReadersData.Queries;
using CuriousReadersService.Services.Comments;
using FakeItEasy;
using System.Collections.Generic;
using Xunit;

public class CommentServiceTest
{
    private readonly IMapper mapper;
    private readonly ICommentQueries commentQueryMock = A.Fake<ICommentQueries>();
    private readonly ICommentCommands commentCommandMock = A.Fake<ICommentCommands>();
    private readonly CreateCommentModel addCommentModelMock = A.Fake<CreateCommentModel>();
    private CommentService commentService;

    public CommentServiceTest()
    {
        var config = new MapperConfiguration(config =>
        {
            config.AddProfile(new CommentsProfile());
        });

        this.mapper = new Mapper(config);
    }
    private void SetupService()
    {
        commentService = new CommentService(commentCommandMock, commentQueryMock,mapper);
    }
    [Fact]
    public void CreateComment_Creates_NewComment()
    {
        //Arrange
        A.CallTo(() => commentCommandMock.Create(A<Comment>.Ignored))
            .Returns(new Comment());

        SetupService();

        //Act
        commentService.Create(addCommentModelMock);

        //Assert
        A.CallTo(() => commentCommandMock.Create(A<Comment>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
    [Fact]
    public void GetComments_ByBookId_ShouldReturn_Comments_ForTheBook()
    {
        //Arrange
        var id = 1;
        var skip = 1;
        var perPage = 5;

        SetupService();

        A.CallTo(() => commentQueryMock.GetComments(A<int>.Ignored,A<int>.Ignored,A<int>.Ignored))
            .Returns(new List<Comment>());

        //Act
        commentService.GetComments(id,skip,perPage);

        //Assert
        A.CallTo(() => commentQueryMock.GetComments(id, skip, perPage))
            .MustHaveHappenedOnceExactly();
    }
    [Fact]
    public void GetComments_ByBookId_ShouldReturn_Null_IfNoComment()
    {
        //Arrange
        var id = 1;
        var skip = 1;
        var perPage = 5;

        SetupService();

        A.CallTo(() => commentQueryMock.GetComments(A<int>.Ignored, A<int>.Ignored, A<int>.Ignored))
            .Returns(null);

        //Act
        var result = commentService.GetComments(id, skip, perPage);

        //Assert
        Assert.Equal(null, result);
    }
    [Fact]
    public void GetUnapprovedComments_ShouldReuturn_Null_IfNoComment()
    {
        //Arrange
        var page = 1;
        var perPage = 5;

        SetupService();

        A.CallTo(() => commentQueryMock.GetUnapprovedComments(A<int>.Ignored, A<int>.Ignored))
            .Returns(null);

        //Act
        var result = commentService.GetUnapprovedComments(page, perPage);

        //Assert
        Assert.Equal(null, result);
    }
    [Fact]
    public void GetUnapprovedComments_ShouldReuturn_UnapprovedComment()
    {
        //Arrange
        var page = 1;
        var perPage = 5;

        SetupService();

        A.CallTo(() => commentQueryMock.GetUnapprovedComments(A<int>.Ignored, A<int>.Ignored))
            .Returns(new List<Comment>());

        //Act
        var result = commentService.GetUnapprovedComments(page, perPage);

        //Assert
        A.CallTo(() => commentQueryMock.GetUnapprovedComments(page, perPage))
            .MustHaveHappenedOnceExactly();
        Assert.NotNull(result);
    }
    [Fact]
    public void ApproveComment_Approves_ExistingComment()
    {
        //Arrange
        var commentId = 1;

        SetupService();

        A.CallTo(() => commentCommandMock.Approve(A<int>.Ignored))
            .DoesNothing();

        //Act
        commentService.Approve(commentId);

        //Assert
        A.CallTo(() => commentCommandMock.Approve(commentId))
            .MustHaveHappenedOnceExactly();
    }
    [Fact]
    public void CountUnapproved_Returns_UnapprovedComments()
    {
        //Arrange

        SetupService();

        A.CallTo(() => commentQueryMock.CountUnapproved()).Returns(1);

        //Act
        var result = commentService.CountUnapproved();

        //Assert
        A.CallTo(() => commentQueryMock.CountUnapproved())
            .MustHaveHappenedOnceExactly();
        Assert.Equal(1, result);
    }
}
