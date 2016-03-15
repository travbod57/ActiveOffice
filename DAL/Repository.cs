using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        T GetById(long Id);
        IEnumerable<T> All();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly IDbSet<T> _dbset;

        public Repository(DbContext context)
        {
            _context = context;
            _dbset = context.Set<T>();
        }

        public virtual void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public virtual void Delete(T entity)
        {
            var entry = _context.Entry(entity);
            entry.State = EntityState.Deleted;
        }

        public virtual void Update(T entity)
        {
            var entry = _context.Entry(entity);
            _dbset.Attach(entity);
            entry.State = EntityState.Modified;
        }

        public virtual T GetById(long id)
        {
            return _dbset.Find(id);
        }

        public virtual IEnumerable<T> All()
        {
            return _dbset;
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbset.Where(predicate);
        }
    }

    //public class Repository<T> where T : DBEntity
    //{
    //    private readonly DbContext context;
    //    private IDbSet<T> entities;
    //    string errorMessage = string.Empty;

    //    public Repository(DbContext context)
    //    {
    //        this.context = context;
    //    }

    //    public T GetById(object id)
    //    {
    //        return this.Entities.Find(id);
    //    }

    //    public void Insert(T entity)
    //    {
    //        try
    //        {
    //            if (entity == null)
    //            {
    //                throw new ArgumentNullException("entity");
    //            }
    //            this.Entities.Add(entity);
    //            this.context.SaveChanges();
    //        }
    //        catch (DbEntityValidationException dbEx)
    //        {

    //            foreach (var validationErrors in dbEx.EntityValidationErrors)
    //            {
    //                foreach (var validationError in validationErrors.ValidationErrors)
    //                {
    //                    errorMessage += string.Format("Property: {0} Error: {1}",
    //                    validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
    //                }
    //            }
    //            throw new Exception(errorMessage, dbEx);
    //        }
    //    }

    //    public void Update(T entity)
    //    {
    //        try
    //        {
    //            if (entity == null)
    //            {
    //                throw new ArgumentNullException("entity");
    //            }
    //            this.context.SaveChanges();
    //        }
    //        catch (DbEntityValidationException dbEx)
    //        {
    //            foreach (var validationErrors in dbEx.EntityValidationErrors)
    //            {
    //                foreach (var validationError in validationErrors.ValidationErrors)
    //                {
    //                    errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}",
    //                    validationError.PropertyName, validationError.ErrorMessage);
    //                }
    //            }

    //            throw new Exception(errorMessage, dbEx);
    //        }
    //    }

    //    public void Delete(T entity)
    //    {
    //        try
    //        {
    //            if (entity == null)
    //            {
    //                throw new ArgumentNullException("entity");
    //            }

    //            this.Entities.Remove(entity);
    //            this.context.SaveChanges();
    //        }
    //        catch (DbEntityValidationException dbEx)
    //        {

    //            foreach (var validationErrors in dbEx.EntityValidationErrors)
    //            {
    //                foreach (var validationError in validationErrors.ValidationErrors)
    //                {
    //                    errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}",
    //                    validationError.PropertyName, validationError.ErrorMessage);
    //                }
    //            }
    //            throw new Exception(errorMessage, dbEx);
    //        }
    //    }

    //    public virtual IQueryable<T> Table
    //    {
    //        get
    //        {
    //            return this.Entities;
    //        }
    //    }

    //    private IDbSet<T> Entities
    //    {
    //        get
    //        {
    //            if (entities == null)
    //            {
    //                entities = context.Set<t>();
    //            }
    //            return entities;
    //        }
    //    }
    //}
}
