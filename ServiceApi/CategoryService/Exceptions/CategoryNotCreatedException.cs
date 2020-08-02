using System;

namespace CategoryService.Exceptions
{
    public class CategoryNotCreatedException:ApplicationException
    {
        public CategoryNotCreatedException() { }
        public CategoryNotCreatedException(string message) : base(message) { }
    }
}
