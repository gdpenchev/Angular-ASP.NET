namespace CuriousReaders.Test.Services;

using AutoMapper;
using CuriousReadersData.Entities;
using CuriousReadersData.Profiles;
using CuriousReadersData.Queries;
using CuriousReadersService.Profiles;
using CuriousReadersService.Services.Author;
using CuriousReadersService.Services.Genre;
using FakeItEasy;
using System.Collections.Generic;
using Xunit;

public class AuthorServiceTest
{
    private readonly IAuthorQueries authorQueriesMock = A.Fake<IAuthorQueries>();
    private readonly IEnumerable<Author> authorsMocks = A.Fake<IEnumerable<Author>>();
    private readonly int authorsTotalCount = 0;
    private AuthorService authorService;
    private readonly IMapper mapper;


    public AuthorServiceTest()
    {
        var config = new MapperConfiguration(config =>
        {
            config.AddProfile(new AuthorsProfile());
        });

        this.mapper = new Mapper(config);

        authorService = new AuthorService(authorQueriesMock, mapper);
    }

    [Fact]
    public void GetAllAuthors_Returns_AllAuthors()
    {
        //Arrange
        A.CallTo(() => authorQueriesMock.GetAllAuthors())
            .Returns(authorsMocks);

        //Act
        var result = authorService.GetAllAuthors();

        //Assert
        A.CallTo(() => authorQueriesMock.GetAllAuthors())
            .MustHaveHappenedOnceExactly();
        Assert.NotNull(result);
    }
}
