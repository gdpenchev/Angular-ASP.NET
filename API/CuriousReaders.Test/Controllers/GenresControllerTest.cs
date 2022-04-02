namespace CuriousReaders.Test.Controllers;

using CuriousReadersAPI.Controllers;
using CuriousReadersData.Dto.Genres;
using CuriousReadersService;
using CuriousReadersService.Services.Genre;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;

public class GenresControllerTest
{
    private readonly IEnumerable<ReadGenreModel> readGenreModelsMocks = A.Fake<IEnumerable<ReadGenreModel>>();
    private readonly IGenreService genreServiceMock = A.Fake<IGenreService>();
    private readonly int genresCount = 0;
    private GenresController controller;

    private void SetupController()
    {
        controller = new GenresController(genreServiceMock);
    }

    [Fact]
    public void GetAllGenres_ShouldReturn_AllGenres()
    {
        //Arrange
        A.CallTo(() => genreServiceMock.GetAllGenres())
            .Returns(readGenreModelsMocks);

        SetupController();

        //Act
        var result = controller.GetAllGenres();

        //Assert
        A.CallTo(() => genreServiceMock.GetAllGenres())
            .MustHaveHappenedOnceExactly();
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<ActionResult<IEnumerable<ReadGenreModel>>>(result);
    }

    [Fact]
    public void GetAssignedBookGenresCount_ShouldReturn_AllGenresCount()
    {
        //Arrange
        A.CallTo(() => genreServiceMock.GetAssignedBookGenresCount())
            .Returns(genresCount);

        SetupController();

        //Act
        var result = controller.GetAssignedBookGenresCount();

        //Assert
        A.CallTo(() => genreServiceMock.GetAssignedBookGenresCount())
            .MustHaveHappenedOnceExactly();
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<ActionResult<IEnumerable<ReadGenreModel>>>(result);
    }
}
