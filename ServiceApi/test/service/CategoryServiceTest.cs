using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using CategoryService.Models;
using CategoryService.Repository;
using CategoryService.Exceptions;
using Test;

namespace Service.Test
{
    [TestCaseOrderer("Test.PriorityOrderer", "Test")]
    public class CategoryServiceTest
    {
        #region Positive tests
        [Fact, TestPriority(1)]
        public void CreateCategoryShouldReturnCategory()
        {
            var mockRepo = new Mock<ICategoryRepository>();
            Category category = new Category { Id = 121, Name = "Entertainment", CreatedBy = "Mukesh", Description = "All about entertainment", CreationDate = DateTime.Now };
            mockRepo.Setup(repo => repo.GetAllCategoriesByUserId("Mukesh")).Returns(this.GetCategories());
            mockRepo.Setup(repo => repo.CreateCategory(category)).Returns(category);
            var service = new CategoryService.Service.CategoryService(mockRepo.Object);

            var actual = service.CreateCategory(category);

            Assert.NotNull(actual);
            Assert.IsAssignableFrom<Category>(actual);
        }

        [Fact, TestPriority(2)]
        public void GetCategoryByUserShouldReturnListOfcategory()
        {
            var mockRepo = new Mock<ICategoryRepository>();
            var userId = "Mukesh";
            mockRepo.Setup(repo => repo.GetAllCategoriesByUserId(userId)).Returns(this.GetCategories());
            var service = new CategoryService.Service.CategoryService(mockRepo.Object);

            var actual = service.GetAllCategoriesByUserId(userId);

            Assert.IsAssignableFrom<List<Category>>(actual);
        }

        [Fact, TestPriority(3)]
        public void GetCategoryByIdShouldReturnCategory()
        {
            var mockRepo = new Mock<ICategoryRepository>();
            var Id = 101;
            Category category = new Category { Id = 101, Name = "Sports", CreatedBy = "Mukesh", Description = "All about sports", CreationDate = new DateTime() };

            mockRepo.Setup(repo => repo.GetCategoryById(Id)).Returns(category);
            var service = new CategoryService.Service.CategoryService(mockRepo.Object);

            var actual = service.GetCategoryById(Id);

            Assert.IsAssignableFrom<Category>(actual);
            Assert.Equal("Sports", actual.Name);
        }

        [Fact, TestPriority(4)]
        public void DeleteCategoryShouldReturnTrue()
        {
            var mockRepo = new Mock<ICategoryRepository>();
            var Id = 102;

            mockRepo.Setup(repo => repo.DeleteCategory(Id)).Returns(true);
            var service = new CategoryService.Service.CategoryService(mockRepo.Object);

            var actual = service.DeleteCategory(Id);
            Assert.True(actual);
        }

        [Fact, TestPriority(5)]
        public void UpdateCategoryShouldReturnTrue()
        {
            int Id = 101;
            Category category = new Category { Id = 101, Name = "Sports", CreatedBy = "Mukesh", Description = "Olympic Games", CreationDate = DateTime.Now };

            var mockRepo = new Mock<ICategoryRepository>();

            mockRepo.Setup(repo => repo.GetCategoryById(Id)).Returns(category);
            mockRepo.Setup(repo => repo.UpdateCategory(Id,category)).Returns(true);
            var service = new CategoryService.Service.CategoryService(mockRepo.Object);

            var actual = service.UpdateCategory(Id, category);
            Assert.True(actual);
        }

        private List<Category> GetCategories()
        {
            List<Category> categories = new List<Category> {
                new Category{Id=101, Name="Sports", CreatedBy="Mukesh", Description="All about sports", CreationDate=new DateTime() },
                 new Category{Id=102, Name="Politics", CreatedBy="Mukesh", Description="INDIAN politics", CreationDate=new DateTime() }
            } ;

            return categories;
        }

        #endregion Positive tests

        #region Negative tests

        [Fact, TestPriority(6)]
        public void CreateCategoryShouldThrowException()
        {
            var mockRepo = new Mock<ICategoryRepository>();
            Category category = new Category { Id = 101, Name = "Sports", CreatedBy = "Mukesh", Description = "All about sports", CreationDate = new DateTime() };
            List<Category> categories = new List<Category>();
            mockRepo.Setup(repo => repo.GetAllCategoriesByUserId("Mukesh")).Returns(this.GetCategories());
            var service = new CategoryService.Service.CategoryService(mockRepo.Object);

            var actual = Assert.Throws<CategoryNotCreatedException>(()=> service.CreateCategory(category));
            Assert.Equal("This category already exists",actual.Message);
        }

        
        [Fact, TestPriority(7)]
        public void GetCategoryByUserShouldReturnEmptyList()
        {
            var mockRepo = new Mock<ICategoryRepository>();
            var userId = "Nitin";
            mockRepo.Setup(repo => repo.GetAllCategoriesByUserId(userId)).Returns(new List<Category>());
            var service = new CategoryService.Service.CategoryService(mockRepo.Object);

            var actual = service.GetAllCategoriesByUserId(userId);

            Assert.IsAssignableFrom<List<Category>>(actual);
            Assert.Empty(actual);
        }

        
        [Fact, TestPriority(8)]
        public void GetCategoryByIdShouldThrowException()
        {
            var mockRepo = new Mock<ICategoryRepository>();
            var Id = 105;
            Category category = null;
            mockRepo.Setup(repo => repo.GetCategoryById(Id)).Returns(category);
            var service = new CategoryService.Service.CategoryService(mockRepo.Object);

            var actual =Assert.Throws<CategoryNotFoundException>(()=>  service.GetCategoryById(Id));

            Assert.Equal("This category id not found", actual.Message);
        }

       
        [Fact, TestPriority(9)]
        public void DeleteCategoryShouldThrowException()
        {
            var mockRepo = new Mock<ICategoryRepository>();
            var Id = 105;

            mockRepo.Setup(repo => repo.DeleteCategory(Id)).Returns(false);
            var service = new CategoryService.Service.CategoryService(mockRepo.Object);


            var actual = Assert.Throws<CategoryNotFoundException>(()=> service.DeleteCategory(Id));

            Assert.Equal("This category id not found", actual.Message);
        }
        
       [Fact, TestPriority(10)]
       public void UpdateCategoryShouldThrowException()
       {
           int Id = 105;
           Category category = new Category { Id = 105, Name = "Sports", CreatedBy = "Mukesh", Description = "Olympic Games", CreationDate = new DateTime() };

           var mockRepo = new Mock<ICategoryRepository>();

           mockRepo.Setup(repo => repo.UpdateCategory(Id, category)).Returns(false);
           var service = new CategoryService.Service.CategoryService(mockRepo.Object);


           var actual = Assert.Throws<CategoryNotFoundException>(() => service.UpdateCategory(Id, category));
            Assert.Equal("This category id not found", actual.Message);
        }

        #endregion Negative tests
    }
}
