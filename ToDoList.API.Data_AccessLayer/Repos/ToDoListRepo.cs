using Microsoft.EntityFrameworkCore;
using ToDoListInfo.API.DBLayer.DbContexts;
using ToDoListInfo.API.DBLayer.Entities;


namespace ToDoListInfo.API.Data_AccessLayer.Repos
{
    public class ToDoListRepo : IToDoListRepo
    {
        private readonly ToDoListInfoContext _context;

        // initializare, DI
        public ToDoListRepo(ToDoListInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // se obtin toate task-urile
        public async Task<IEnumerable<ToDoList>> GetLists()
        {
            return _context.Set<ToDoList>();
        }

        // se verifica daca exista un task cu un id dat
        public async Task<bool> ListExistsAsync(int listaId)
        {
            var result = await _context.ToDoList.FindAsync(listaId);
            if (result == null)
            {
                return false;
            }
            return true;
        }

        // se obtine task-ul cu id-ul dat
        public async Task<ToDoList> GetListAsync(int listaId)
        {
            var result = await _context.ToDoList.FindAsync(listaId);
            return result;
        }

        // se obtin task-ruile create de un anumit user
        public async Task<IEnumerable<ToDoList>> GetListCreatedByAsync(int idOwner)
        {
            var result = await _context.ToDoList.Where(l => l.IdOwner == idOwner).ToListAsync();
            return result;
        }


        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        // adaugarea unui task
        public async Task<ToDoList> AddList(ToDoList toDoList)
        {
                     
             _context.ToDoList.Add(toDoList);
             await _context.SaveChangesAsync();
             return toDoList;

        }

        // modificarea unui task
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

        // stergerea unui task dupa id-ul sau
        public void DeleteList(int id)
        {
           var list = _context.ToDoList.Find(id);
           _context.ToDoList.Remove(list);
           _context.SaveChanges();
        }

        // adaugarea unui fisier
        public async Task<Upload> AddFileAsync(string fileName, string filePath, int idOwner, string emailOwner, string text, int idTask)
        {
            var fileUpload = new Upload
            {
                Name = fileName,
                Path = filePath,
                CreatedDate = DateTime.Now,
                IdOwner = idOwner,
                EmailOwner = emailOwner,
                Text = text,
                IdTask = idTask
            };

            _context.Upload.Add(fileUpload);
            await _context.SaveChangesAsync();

            return fileUpload;
        }

        // se obtin toate fisierele
        public async Task<IEnumerable<Upload>> GetFiles()
        {
            return _context.Set<Upload>();
        }

        // se obtin toate fisierele create de un anumit user 
        public async Task<IEnumerable<Upload>> GetFilesCreatedByAsync(int idOwner)
        {
            var result = await _context.Upload.Where(l => l.IdOwner == idOwner).ToListAsync();
            return result;
        }

        // se obtine un fisier care are un anumit id
        public async Task<Upload> GetFileById(int id)
        {
            var result = await _context.Upload.FindAsync(id);
            return result;
        }
    }
}