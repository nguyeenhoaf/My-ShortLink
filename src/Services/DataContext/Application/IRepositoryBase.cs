using System.Linq.Expressions;

namespace Database.Abstractions;

public interface IRepositoryBase<TDocument>
{
    IQueryable<TDocument> AsQueryable(Expression<Func<TDocument, bool>>? filterExpression = null);
    IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression);
    IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProjected>> projectionExpression);
    TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression);
    Task<List<TResult>> ToListAsync<TResult>(IQueryable<TResult> query, CancellationToken cancellationToken);
    Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);
    TDocument FindById(string id);
    Task<TDocument> FindByIdAsync(string id);
    void InsertOne(TDocument document);
    Task InsertOneAsync(TDocument document);
    void InsertMany(ICollection<TDocument> documents);
    Task InsertManyAsync(ICollection<TDocument> documents);
    void ReplaceOne(TDocument document);
    Task ReplaceOneAsync(TDocument document);
    void DeleteOne(Expression<Func<TDocument, bool>> filterExpression);
    Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);
    void DeleteById(string id);
    Task DeleteByIdAsync(string id);
    void DeleteMany(Expression<Func<TDocument, bool>> filterExpression);
    Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
}
