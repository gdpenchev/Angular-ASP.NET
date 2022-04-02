namespace CuriousReaders.Test.Controllers
{
    using CuriousReadersAPI.Controllers;
    using CuriousReadersData.Dto.Comments;
    using CuriousReadersData.Entities;
    using CuriousReadersService;
    using CuriousReadersService.Services.Comments;
    using FakeItEasy;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using Xunit;

    public class CommentsControllerTest
    {
        private ICommentService commentServiceMock = A.Fake<ICommentService>();

        [Fact]
        public void CreateComment_Returns_CreatedResult()
        {
            //Arrange

            A.CallTo(() => this.commentServiceMock.Create(A<CreateCommentModel>.Ignored))
                .Returns(new Comment());

            var commentsController = new CommentsController(this.commentServiceMock);

            var createCommentModel = A.Fake<CreateCommentModel>();

            //Act
            var result = commentsController.Create(createCommentModel);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void CreateComment_Returns_BadRequest_IfNull()
        {
            //Arrange

            A.CallTo(() => this.commentServiceMock.Create(A<CreateCommentModel>.Ignored))
                .Returns(null);

            var commentsController = new CommentsController(this.commentServiceMock);

            var createCommentModel = A.Fake<CreateCommentModel>();

            //Act
            var result = commentsController.Create(createCommentModel);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public void AllComments_Returns_OkObjectResult_If_CommentAreAvailable()
        {
            //Arrange
            var Id = 1;
            var comment = 1;
            var perPage = 5;

            A.CallTo(() => this.commentServiceMock.GetComments(Id,comment,perPage))
                .Returns(new List<ReadCommentModel>());

            var commentsController = new CommentsController(this.commentServiceMock);

            //Act
            var result = commentsController.AllComments(Id, comment, perPage);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<ActionResult<IEnumerable<ReadCommentModel>>>(result);
        }
        [Fact]
        public void AllComments_Returns_BadRequest_If_NoCommentAreAvailable()
        {
            //Arrange
            var Id = 1;
            var comment = 1;
            var perPage = 5;

            A.CallTo(() => this.commentServiceMock.GetComments(Id, comment, perPage))
                .Returns(null);

            var commentsController = new CommentsController(this.commentServiceMock);

            //Act
            var result = commentsController.AllComments(Id, comment, perPage);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<ActionResult<IEnumerable<ReadCommentModel>>>(result);
        }
        [Fact]
        public void AllUnapprovedComments_Returns_BadRequest_If_NoCommentAreAvailable()
        {
            //Arrange
            var page = 1;
            var commentsPerPage = 5;

            A.CallTo(() => this.commentServiceMock.GetUnapprovedComments(page, commentsPerPage))
                .Returns(null);

            var commentsController = new CommentsController(this.commentServiceMock);

            //Act
            var result = commentsController.AllUnapprovedComments(page, commentsPerPage);

            //Assert
            Assert.Equal(null, result.Value);
            Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<ActionResult<IEnumerable<ReadCommentModel>>>(result);
        }
        [Fact]
        public void AllUnapprovedComments_Returns_OkResult_IfCommentsAreAvailable()
        {
            //Arrange
            var page = 1;
            var commentsPerPage = 5;

            A.CallTo(() => this.commentServiceMock.GetUnapprovedComments(page, commentsPerPage))
                .Returns(new List<ReadCommentModel>());

            var commentsController = new CommentsController(this.commentServiceMock);

            //Act
            var result = commentsController.AllUnapprovedComments(page, commentsPerPage);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<ActionResult<IEnumerable<ReadCommentModel>>>(result);
        }
        [Fact]
        public void ApproveComment_Returns_OkResult_IfCommentsIsApproved()
        {
            //Arrange
            var commentId = 1;

            A.CallTo(() => this.commentServiceMock.Approve(commentId))
                .DoesNothing();

            var commentsController = new CommentsController(this.commentServiceMock);

            //Act
            var result = commentsController.Approve(commentId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public void Count_Returns_Ok_IfCommentsAreAvailable()
        {
            //Arrange

            A.CallTo(() => this.commentServiceMock.CountUnapproved())
                .Returns(1);

            var commentsController = new CommentsController(this.commentServiceMock);

            //Act
            var result = commentsController.CountUnapproved();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
