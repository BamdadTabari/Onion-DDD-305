using _305.Application.IRepository;
using _305.Application.IUOW;
using _305.Infrastructure.Persistence;
using _305.Infrastructure.Repository;

namespace _305.Infrastructure.UnitOfWork;
public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UnitOfWork>? _logger;

    private IBlogCategoryRepository? _blogCategoryRepository;
    private IBlogRepository? _blogRepository;
    private ITokenBlacklistRepository? _tokenBlacklistRepository;

    private IDbContextTransaction? _transaction;
    private bool _disposed;

    public UnitOfWork(ApplicationDbContext context, ILogger<UnitOfWork>? logger = null)
    {
        _context = context;
        _logger = logger;
    }

    // Lazy Initialization
    public IBlogCategoryRepository BlogCategoryRepository =>
        _blogCategoryRepository ??= new BlogCategoryRepository(_context);

    public IBlogRepository BlogRepository =>
        _blogRepository ??= new BlogRepository(_context);

    public ITokenBlacklistRepository TokenBlacklistRepository =>
        _tokenBlacklistRepository ??= new TokenBlacklistRepository(_context);

    /// <summary>
    /// شروع تراکنش به صورت دستی
    /// </summary>
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }
    }

    /// <summary>
    /// ذخیره‌سازی تغییرات و تأیید تراکنش
    /// </summary>
    public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);

            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
                await _transaction.DisposeAsync();
                _transaction = null;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "خطا در هنگام Commit تراکنش");

            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellationToken);
                await _transaction.DisposeAsync();
                _transaction = null;
            }

            throw;
        }
    }

    /// <summary>
    /// بازگرداندن تراکنش در صورت نیاز
    /// </summary>
    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    /// <summary>
    /// آزادسازی منابع با الگوی Dispose استاندارد
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}