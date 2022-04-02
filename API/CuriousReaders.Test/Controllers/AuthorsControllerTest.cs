namespace CuriousReaders.Test.Controllers;

using CuriousReadersAPI.Controllers;
using CuriousReadersData.Dto.Authors;
using CuriousReadersService.Services.Author;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

public class AuthorsControllerTest
{
    private readonly IEnumerable<ReadAuthorModel> readAuthorModelsMocks = A.Fake<IEnumerable<ReadAuthorModel>>();
    private readonly IAuthorService authorServiceMock = A.Fake<IAuthorService>();
    private readonly int genresCount = 0;
    private AuthorsController controller;

    private void SetupController()
    {
        controller = new AuthorsController(authorServiceMock);
    }

    [Fact]
    public void GetAllGenres_ShouldReturn_AllGenres()
    {
        //Arrange
        A.CallTo(() => authorServiceMock.GetAllAuthors())
            .Returns(readAuthorModelsMocks);

        SetupController();

        //Act
        var result = controller.GetAllAuthors();

        //Assert
        A.CallTo(() => authorServiceMock.GetAllAuthors())
            .MustHaveHappenedOnceExactly();

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<ActionResult<IEnumerable<ReadAuthorModel>>>(result);
    }
}
