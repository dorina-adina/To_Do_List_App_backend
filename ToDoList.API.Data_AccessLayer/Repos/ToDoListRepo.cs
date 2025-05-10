using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using ToDoListInfo.API.DBLayer.DbContexts;
using ToDoListInfo.API.DBLayer.Entities;

namespace ToDoListInfo.API.Data_AccessLayer.Repos
{
    public class ToDoListRepo : IToDoListRepo
    {
        private readonly ToDoListInfoContext _context;

        public ToDoListRepo(ToDoListInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<ToDoList>> GetLists()
        {
            return _context.Set<ToDoList>();
        }
        public async Task<bool> ListExistsAsync(int listaId)
        {
            var result = await _context.ToDoList.FindAsync(listaId);
            if (result == null)
            {
                return false;
            }
            return true;
        }
        public async Task<ToDoList> GetListAsync(int listaId)
        {
            var result = await _context.ToDoList.FindAsync(listaId);
            return result;
        }
        public async Task<IEnumerable<ToDoList>> GetListCreatedByAsync(int idOwner)
        {
            var result = await _context.ToDoList.Where(l => l.IdOwner == idOwner).ToListAsync();
            return result;
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<ToDoList> AddList(ToDoList toDoList)
        {
                     
             _context.ToDoList.Add(toDoList);
             await _context.SaveChangesAsync();
             return toDoList;

        }

        public async Task<ToDoList> UpdateList(int id, ToDoList list)
        {
            var listToUpdate = await _context.ToDoList.FindAsync(id);
            if(listToUpdate != null)
            {
                listToUpdate.Id = id;
                listToUpdate.Task = list.Task;
                listToUpdate.CreatedBy = list.CreatedBy;
                listToUpdate.IdOwner = list.IdOwner;
                listToUpdate.DueDate = list.DueDate;
            }
            await _context.SaveChangesAsync();
            return list;
        }

        public void DeleteList(int id)
        {
           var list = _context.ToDoList.Find(id);
           _context.ToDoList.Remove(list);
           _context.SaveChanges();
        }


        //public async Task<Upload> AddFileAsync(string fileName, string filePath, int idOwner, string emailOwner, string text)
        //{
        //    var fileUpload = new Upload
        //    {
        //        Name = fileName,
        //        Path = filePath,
        //        CreatedDate = DateTime.Now,
        //        IdOwner = idOwner,
        //        EmailOwner = emailOwner,
        //        InfoPath = text
        //    };

        //    _context.Upload.Add(fileUpload);
        //    await _context.SaveChangesAsync();

        //    return fileUpload;
        //}

        public async Task<Upload> AddFileAsync(string fileName, string filePath, int idOwner, string emailOwner, string text)
        {
            var fileUpload = new Upload
            {
                Name = fileName,
                Path = filePath,
                CreatedDate = DateTime.Now,
                IdOwner = idOwner,
                EmailOwner = emailOwner,
                InfoPath = text
            };

            _context.Upload.Add(fileUpload);
            await _context.SaveChangesAsync();

            return fileUpload;
        }
    }
}