
namespace CuriousReaders.Test.Data.Queries;
using CuriousReadersData;
using CuriousReadersData.Entities;
using CuriousReadersData.Queries;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class UserQueriesTest
{
    private LibraryDbContext fakeDbContext = A.Fake<LibraryDbContext>();

    private void SetupFakeDbSet(IQueryable<User> fakeIQueryable)
    {
        var fakeDbSet = A.Fake<DbSet<User>>((d =>
                 d.Implements(typeof(IQueryable<User>))));

        A.CallTo(() => ((IQueryable<User>)fakeDbSet).GetEnumerator())
            .Returns(fakeIQueryable.GetEnumerator());

        A.CallTo(() => ((IQueryable<User>)fakeDbSet).Provider)
            .Returns(fakeIQueryable.Provider);

        A.CallTo(() => ((IQueryable<User>)fakeDbSet).Expression)
            .Returns(fakeIQueryable.Expression);

        A.CallTo(() => ((IQueryable<User>)fakeDbSet).ElementType)
           .Returns(fakeIQueryable.ElementType);

        A.CallTo(() => fakeDbContext.Users).Returns(fakeDbSet);
    }


    [Fact]
    public void GetAllUsers_Should_Return_AllNotActiveUsers_FromDb_IfIsActiveIsFalse()
    {
        //Arrange
        var fakeIQueryable = new List<User>()
        {
            new User() { IsActive = false },
            new User() { IsActive = true },
            new User() { IsActive = true }
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var genreQueries = new UserQueries(fakeDbContext);

        var isActive = false;
        var expectedResult = fakeIQueryable.Where(u => u.IsActive == isActive);

        //Act
        var result = genreQueries.GetAllUsers(1, 12, isActive);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetAllUsers_Should_Return_AllActiveUsers_FromDb_IfIsActiveIsTrue()
    {
        //Arrange
        var fakeIQueryable = new List<User>()
        {
            new User() { IsActive = false },
            new User() { IsActive = true },
            new User() { IsActive = true }
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var genreQueries = new UserQueries(fakeDbContext);

        var isActive = true;
        var expectedResult = fakeIQueryable.Where(u => u.IsActive == isActive);

        //Act
        var result = genreQueries.GetAllUsers(1, 12, isActive);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetAllUsers_Should_Return_AllNotActiveUsersCount_FromDb_IfIsActiveIsFalse()
    {
        //Arrange
        var fakeIQueryable = new List<User>()
        {
            new User() { IsActive = false },
            new User() { IsActive = true },
            new User() { IsActive = true }
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var genreQueries = new UserQueries(fakeDbContext);

        var isActive = false;
        var expectedResult = fakeIQueryable.Where(u => u.IsActive == isActive).Count();

        //Act
        var result = genreQueries.GetAllUsersCount(isActive);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetAllUsers_Should_Return_AllNotActiveUsersCount_FromDb_IfIsActiveIsTrue()
    {
        //Arrange
        var fakeIQueryable = new List<User>()
        {
            new User() { IsActive = false },
            new User() { IsActive = true },
            new User() { IsActive = true }
        }
        .AsQueryable();

        SetupFakeDbSet(fakeIQueryable);

        var genreQueries = new UserQueries(fakeDbContext);

        var isActive = true;
        var expectedResult = fakeIQueryable.Where(u => u.IsActive == isActive).Count();

        //Act
        var result = genreQueries.GetAllUsersCount(isActive);

        //Assert
        Assert.Equal(expectedResult, result);
    }
}