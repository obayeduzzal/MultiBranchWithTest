using MultiBranchWithTest.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiBranchWithTest.App.Service
{
    public interface IPersonService
    {
        public IEnumerable<Person> GetAllItems();
        Person Add(Person newItem);
        Person GetById(int id);
        void Remove(int id);
    }
}
