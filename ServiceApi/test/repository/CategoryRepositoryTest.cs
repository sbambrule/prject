using System;
using System.Collections.Generic;
using CategoryService.Models;
using CategoryService.Repository;
using Test;
using Test.InfraSetup;
using Xunit;

namespace Repository.Test
{
    [TestCaseOrderer("Test.PriorityOrderer", "Test")]
    public class CategoryRepositoryTest:IClassFixture<CategoryDbFixture>
    {
        private ICategoryRepository repository;

        public CategoryRepositoryTest(CategoryDbFixture _fixture)
        {
            repository = new CategoryRepository(_fixture.context);
        }
        [Fact, TestPriority(1)]
        public void CreateCategoryShouldReturnCategory()
        {
            Category category = new Category { Name = "Entertainment", CreatedBy = "Sanjeev", Description = "All about entertainment", CreationDate = new DateTime() };

            var actual = repository.CreateCategory(category);
            Assert.NotNull(actual);
            Assert.IsAssignableFrom<Category>(actual);
            Assert.Equal(103, actual.Id);
        }

        [Fact, TestPriority(2)]
        public void GetCategoryByUserShouldReturnListOfcategory()
        {
            var actual = repository.GetAllCategoriesByUserId("Mukesh");
            Assert.IsAssignableFrom<List<Category>>(actual);
            Assert.Contains(actual, c => c.Name == "Sports");
        }

        [Fact, TestPriority(3)]
        public void GetCategoryByIdShouldReturnCategory()
        {
            var actual = repository.GetCategoryById(101);

            Assert.IsAssignableFrom<Category>(actual);
            Assert.Equal("Sports", actual.Name);
        }

        [Fact, TestPriority(4)]
        public void DeleteCategoryShouldReturnTrue()
        {
            var actual = repository.DeleteCategory(103);
            Assert.True(actual);
        }

        [Fact,TestPriority(5)]
        public void UpdateCategoryShouldReturnTrue()
        {
            Category category = new Category { Id = 101, Name = "Sports", CreatedBy = "Mukesh", Description = "Olympic Games", CreationDate =DateTime.Now };

            var actual = repository.UpdateCategory(101, category);
            Assert.True(actual);
        }
    }
}
