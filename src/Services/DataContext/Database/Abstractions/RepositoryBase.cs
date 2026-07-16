using Domain.Abstractions;
using Domain.Attributes;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Database.Abstractions;

public class RepositoryBase<TDocument> : IRepositoryBase<TDocument> where TDocument : class, IDocument
{
    private readonly IMongoCollection<TDocument> _collection;

    public RepositoryBase(IMongoDatabase database)
    {
        var collectionName = GetCollectionName(typeof(TDocument));
        _collection = database.GetCollection<TDocument>(collectionName);
    }

    private protected string GetCollectionName(Type documentType)
    {
        return ((BsonCollectionAttribute)documentType
            .GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                .FirstOrDefault())?.CollectionName
                ?? documentType.Name;
    }

    public IQueryable<TDocument> AsQueryable(Expression<Func<TDocument, bool>>? filterExpression = null)
    {
        var query = _collection.AsQueryable();
        return filterExpression != null ? query.Where(filterExpression) : query;
    }

    public void DeleteById(string id)
    {
        _collection.DeleteOne(x => x.Id == id);
    }

    public async Task DeleteByIdAsync(string id)
    {
        await _collection.DeleteOneAsync(x => x.Id == id);
    }

    public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
    {
        _collection.DeleteMany(filterExpression);
    }

    public async Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        await _collection.DeleteManyAsync(filterExpression);
    }

    public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        _collection.DeleteOne(filterExpression);
    }

    public async Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        await _collection.DeleteOneAsync(filterExpression);
    }

    public IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression)
    {
        return _collection.Find(filterExpression).ToEnumerable();
    }

    public IEnumerable<TProjected> FilterBy<TProjected>(
        Expression<Func<TDocument, bool>> filterExpression,
        Expression<Func<TDocument, TProjected>> projectionExpression)
    {
        return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
    }

    public TDocument FindById(string id)
    {
        return _collection.Find(x => x.Id == id).FirstOrDefault();
    }

    public async Task<TDocument> FindByIdAsync(string id)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        return _collection.Find(filterExpression).FirstOrDefault();
    }

    public async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return await _collection.Find(filterExpression).FirstOrDefaultAsync();
    }

    public void InsertMany(ICollection<TDocument> documents)
    {
        _collection.InsertMany(documents);
    }

    public async Task InsertManyAsync(ICollection<TDocument> documents)
    {
        await _collection.InsertManyAsync(documents);
    }

    public void InsertOne(TDocument document)
    {
        _collection.InsertOne(document);
    }

    public async Task InsertOneAsync(TDocument document)
    {
        await _collection.InsertOneAsync(document);
    }

    public void ReplaceOne(TDocument document)
    {
        _collection.ReplaceOne(x => x.Id == document.Id, document);
    }

    public async Task ReplaceOneAsync(TDocument document)
    {
        await _collection.ReplaceOneAsync(x => x.Id == document.Id, document);
    }

    public async Task<List<TResult>> ToListAsync<TResult>(IQueryable<TResult> query, CancellationToken cancellationToken)
    {
        return await Task.Run(() => query.ToList(), cancellationToken);
    }
}
