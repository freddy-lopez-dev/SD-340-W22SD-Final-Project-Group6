using Microsoft.EntityFrameworkCore;
using SD_340_W22SD_Final_Project_Group6.Data;
using SD_340_W22SD_Final_Project_Group6.Models;

namespace SD_340_W22SD_Final_Project_Group6.DAL
{
    public class CommentRepository : IRepository<Comment>
    {
        private ApplicationDbContext _db;
        public CommentRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public void Create(Comment entity)
        {
            _db.Comments.Add(entity);
        }

        public void Delete(Comment entity)
        {
            _db.Comments.Remove(entity);
        }

        public Comment? GetById(int id)
        {
            return _db.Comments.FirstOrDefault(c => c.Id == id);
        }

        public Comment Get(Func<Comment, bool> predicate)
        {
            return _db.Comments.FirstOrDefault(predicate);
        }

        public ICollection<Comment> GetAll()
        {
            return _db.Comments.ToList();
        }

        public ICollection<Comment> GetList(Func<Comment, bool> predicate)
        {
            return _db.Comments.Where(predicate).ToList();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public Comment Update(Comment entity)
        {
            return _db.Comments.Update(entity).Entity;
        }
    }
}