using System;
using System.Collections.Generic;
using System.Linq;
using CategoryService.Models;
using MongoDB.Driver;

namespace CategoryService.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        //define a private variable to represent CategoryContext
        CategoryContext categoryContext = null;
        public CategoryRepository(CategoryContext _context)
        {
            this.categoryContext = _context;
        }

        //This method should be used to save a new category.
        public Category CreateCategory(Category category)
        {
            category.CreationDate = DateTime.Now;
            var result = this.categoryContext.Categories.Find(x => true).SortByDescending(d => d.Id).Limit(1).FirstOrDefaultAsync();


            if (result.Result == null || result.Result.Id == 0)
            {
                category.Id = 100;
            }
            else
            {
                category.Id = result.Result.Id + 1;
            }
            this.categoryContext.Categories.InsertOne(category);

            return category;
        }

        //This method should be used to delete an existing category.
        public bool DeleteCategory(int categoryId)
        {
            var r = this.categoryContext.Categories.DeleteMany(c => c.Id == categoryId);
            return r.DeletedCount > 0;
        }

        //This method should be used to get all category by userId
        public List<Category> GetAllCategoriesByUserId(string userId)
        {
            return this.categoryContext.Categories.Find(user => user.CreatedBy == userId).ToList();
        }

        //This method should be used to get a category by categoryId
        public Category GetCategoryById(int categoryId)
        {
            return this.categoryContext.Categories.Find<Category>(x => x.Id == categoryId).SingleOrDefault();
        }

        // This method should be used to update an existing category.
        public bool UpdateCategory(int categoryId, Category category)
        {
            var cat = this.categoryContext.Categories.Find(x => x.Id == categoryId).FirstOrDefault();
            if (cat == null)
            {
                return false;
            }
            cat.Description = category.Description;
            cat.Name = category.Name;

            return this.categoryContext.Categories.ReplaceOne<Category>(u => u.Id == categoryId, cat).IsAcknowledged;
        }
    }
}
