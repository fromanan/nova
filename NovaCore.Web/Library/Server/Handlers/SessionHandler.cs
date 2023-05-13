using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Threading.Tasks;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Handlers;

public class SessionHandler<TSession> : IHttpRequestHandler
{
    private const string _SESSION_ID = "SESSID";
        
    private readonly Func<TSession> _sessionFactory;
        
    private readonly TimeSpan _expiration;

    private readonly Random _randomGenerator = new();
        
    private readonly ConcurrentDictionary<string, SessionHolder> _sessions = new();

    public SessionHandler(Func<TSession> sessionFactory, TimeSpan expiration)
    {
        _sessionFactory = sessionFactory;
        _expiration = expiration;
    }

    public Task Handle(IHttpContext context, Func<Task> next)
    {
        if (!context.Cookies.TryGetByName(_SESSION_ID, out string sessId))
        {
            sessId = _randomGenerator.Next().ToString(CultureInfo.InvariantCulture);
            context.Cookies.Upsert(_SESSION_ID, sessId);
        }

        SessionHolder sessionHolder = _sessions.GetOrAdd(sessId, CreateSession);

        if (DateTime.Now - sessionHolder.LastAccessTime > _expiration)
        {
            sessionHolder = CreateSession(sessId);
            _sessions.AddOrUpdate(sessId, sessionHolder, (sessionId, oldSession) => sessionHolder);
        }

        context.State.Session = sessionHolder.Session;

        return next();
    }

    private SessionHolder CreateSession(string sessionId)
    {
        return new SessionHolder(_sessionFactory());
    }

    #region Embedded Types

    private struct SessionHolder
    {
        private readonly TSession _session;

        public TSession Session
        {
            get
            {
                LastAccessTime = DateTime.Now;
                return _session;
            }
        }

        public DateTime LastAccessTime { get; private set; } = DateTime.Now;

        public SessionHolder(TSession session)
        {
            _session = session;
        }
    }

    #endregion
}