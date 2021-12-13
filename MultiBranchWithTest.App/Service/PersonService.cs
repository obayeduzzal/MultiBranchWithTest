using MultiBranchWithTest.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiBranchWithTest.App.Service
{
    public class PersonService : IPersonService
    {
        private readonly List<Person> _personList;

        public PersonService()
        {
            _personList = new List<Person>
            {
                new Person{Id = 1,UserEmail = "a1@b.c",UserName = "Name1",UserPassword = "Pass1",CreatedOn = DateTime.Now,IsDeleted = false},
                new Person{Id = 2,UserEmail = "a2@b.c",UserName = "Name2",UserPassword = "Pass2",CreatedOn = DateTime.Now,IsDeleted = false},
                new Person{Id = 3,UserEmail = "a3@b.c",UserName = "Name3",UserPassword = "Pass3",CreatedOn = DateTime.Now,IsDeleted = false},
                new Person{Id = 4,UserEmail = "a4@b.c",UserName = "Name4",UserPassword = "Pass4",CreatedOn = DateTime.Now,IsDeleted = false},
                new Person{Id = 5,UserEmail = "a5@b.c",UserName = "Name5",UserPassword = "Pass5",CreatedOn = DateTime.Now,IsDeleted = false},
            };
        }

        public IEnumerable<Person> GetAllItems()
        {
            return _personList;
        }
        public Person Add(Person newItem)
        {
            newItem.Id = _personList.Max(i => i.Id) + 1;
            _personList.Add(newItem);
            return newItem;
        }
        public Person GetById(int id)
        {
            return _personList.FirstOrDefault(a => a.Id == id);
        }
        public void Remove(int id)
        {
            var existing = _personList.First(a => a.Id == id);
            _personList.Remove(existing);
        }
    }
}
