namespace CuriousReaders.Test.Services;

using AutoMapper;
using CuriousReadersData.Entities;
using CuriousReadersData.Queries;
using CuriousReadersService.Profiles;
using CuriousReadersService.Services.Genre;
using FakeItEasy;
using System.Collections.Generic;
using Xunit;

public class GenreServiceTest
{
    private readonly IGenreQueries genreQueriesMock = A.Fake<IGenreQueries>();
    private readonly IEnumerable<Genre> genresMocks = A.Fake<IEnumerable<Genre>>();
    private readonly int genresTotalCount = 0;
    private GenreService genreService;
    private readonly IMapper mapper;


    public GenreServiceTest()
    {
        var config = new MapperConfiguration(config =>
        {
            config.AddProfile(new GenreProfile());
        });

        this.mapper = new Mapper(config);

        genreService = new GenreService(genreQueriesMock, mapper);
    }

    [Fact]
    public void GetAllGenres_Returns_AllGenres()
    {
        //Arrange
        A.CallTo(() => genreQueriesMock.GetAllGenres())
            .Returns(genresMocks);

        //Act
        var result = genreService.GetAllGenres();

        //Assert
        A.CallTo(() => genreQueriesMock.GetAllGenres())
            .MustHaveHappenedOnceExactly();
        Assert.NotNull(result);
    }

    [Fact]
    public void GetAssignedBookGenresCount_Returns_TotalCount_OfGenres()
    {
        //Arrange
        A.CallTo(() => genreQueriesMock.GetAssignedBookGenresCount())
            .Returns(genresTotalCount);

        //Act
        var result = genreService.GetAssignedBookGenresCount();


        //Assert
        A.CallTo(() => genreQueriesMock.GetAssignedBookGenresCount())
            .MustHaveHappenedOnceExactly();
        Assert.NotNull(result);
        Assert.Equal(genresTotalCount, result);
    }
}
