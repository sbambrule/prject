using System;
using System.Collections.Generic;
using CategoryService.Models;
using CategoryService.Repository;
using CategoryService.Exceptions;
using MongoDB.Driver;
using System.Linq;

namespace CategoryService.Service
{
    public class CategoryService:ICategoryService
    {
        //define a private variable to represent repository
        ICategoryRepository categoryRepository = null;

        //Use constructor Injection to inject all required dependencies.
        public CategoryService(ICategoryRepository _repository)
        {
            this.categoryRepository = _repository;
        }

        //This method should be used to save a new category.
        public Category CreateCategory(Category category)
        {
            var findUser = categoryRepository.GetAllCategoriesByUserId(category.CreatedBy);
            if (findUser != null && findUser.Count(x => x.Name == category.Name) > 0)
            {
                throw new CategoryNotCreatedException("This category already exists");
            }
            var result = categoryRepository.CreateCategory(category);
            return result;
        }
        //This method should be used to delete an existing category.
        public bool DeleteCategory(int categoryId)
        {
            var result = categoryRepository.DeleteCategory(categoryId);
            if (!result)
            {
                throw new CategoryNotFoundException("This category id not found");
            }
            return result;
        }
        // This method should be used to get all category by userId
        public List<Category> GetAllCategoriesByUserId(string userId)
        {
            return categoryRepository.GetAllCategoriesByUserId(userId);
        }
        //This method should be used to get a category by categoryId.
        public Category GetCategoryById(int categoryId)
        {
            var result = categoryRepository.GetCategoryById(categoryId);
            if (result == null)
            {
                throw new CategoryNotFoundException("This category id not found");
            }
            return result;
        }
        //This method should be used to update an existing category.
        public bool UpdateCategory(int categoryId, Category category)
        {
            var result = categoryRepository.UpdateCategory(categoryId, category);
            if (!result)
            {
                throw new CategoryNotFoundException("This category id not found");
            }
            return result;
        }
    }
}
