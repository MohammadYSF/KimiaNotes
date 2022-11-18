using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KimiaNotes.Models;
using Moq;
using Xunit;
namespace KimiaNotes.UnitTest.Tests;

public class ArticleBusinessTests
{
    private readonly ArticleBusiness _business;
    private readonly ArticleRepositoryMoq _repositoryMoq;

    public ArticleBusinessTests()
    {
            _repositoryMoq = new ArticleRepositoryMoq();
            _business = new ArticleBusiness(_repositoryMoq);
    }
    [Fact]
    public void shouldReturnListOfArticles_normal(){
        var expectedResult = new List<Article>();
        
        expectedResult.Add(new Article(){
            Id=1,
            Title="Article 1"
        });
        expectedResult.Add(new Article(){
            Id=2,
            Title="Article 2"
        });
        expectedResult.Add(new Article(){
            Id=3,
            Title="Article 3"
        });
        var result = _business.GetAllArticles();
        //Assert.Equal<Article>(expectedResult , result);
        Assert.NotNull(result);
        Assert.True(expectedResult.Count == result.Count);
    }
    [Theory]
    [InlineData(2)]
    public void shouldFindAndReturnArticle_normal(int articleId){
        var expectedResult = new Article(){
            Id = 2,
            Title = "Article 2"
        };
        var result = _business.FindArticle(articleId);
        //Assert
        Assert.NotNull(result);
        Assert.Equal(result.Id,expectedResult.Id);
        Assert.Equal(result.Title,expectedResult.Title);
    }
    [Theory]
    [InlineData(1000)]
    public void FindArticle_shouldReturnNull(int articleId){
        var moqRepository = new ArticleRepositoryMoq();
        var business = new ArticleBusiness(moqRepository);
        var result = business.FindArticle(articleId);
        Assert.Null(result);        
    }
    [Fact]
    public void AddArticle_normal(){
        //arrange
        var article = new Article(){
            Title = "Article 4"
        };
        //act
        var result = _business.AddArticle(article);
        //Assert
        Assert.Equal(result ,"");
    }
    [Fact]
    public void AddArticle_shouldReturnErrorWhenIdIsGiven(){
        //arrange
        var article = new Article(){
            Id=4,
            Title="Article 4"
        };
        //act
        var result = _business.AddArticle(article);
        //Assert
        Assert.Equal(result , "Error");
    }
}

public class ArticleBusiness
{
    private IArticleRepository @object;

    public ArticleBusiness(IArticleRepository @object)
    {
        this.@object = @object;
        
    }

    public List<Article> GetAllArticles()
    {
        return @object.GetAllArticles().ToList();
    }

    public Article FindArticle(int articleId)
    {
        return @object.FindArticle(articleId);
    }

    public string AddArticle(Article article)
    {
        return article.Id == 0 ? "":"Error";
    }
}

public interface IArticleRepository
{
    Article FindArticle(int articleId);
    public IQueryable<Article> GetAllArticles();
}
public class ArticleRepositoryMoq : IArticleRepository
{
    private readonly List<Article> _mockData;
    public ArticleRepositoryMoq()
    {
        _mockData = new List<Article>();
        _mockData.Add(new Article(){
            Id = 1,
            Title="Article 1"
        });
        _mockData.Add(new Article(){
            Id = 2,
            Title="Article 2"
        });
        _mockData.Add(new Article(){
            Id = 3,
            Title="Article 3"
        });
    }
    public Article FindArticle(int articleId)
    {
        var article = _mockData.SingleOrDefault(a=> a.Id == articleId);
        return article;
    }

    public IQueryable<Article> GetAllArticles()
    {
        List<Article> result =_mockData;
        
        return result.AsQueryable();
    }
}