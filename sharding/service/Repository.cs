using System;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using sharding.Models;

namespace sharding.service;

public class Repository
{
    private readonly PostgresContext postgresContext;
    private readonly Pgshard2Context postgresContext2;
    private readonly Pgshard3Context postgresContext3;

    public Repository(PostgresContext postgresContext, Pgshard2Context postgresContext2, Pgshard3Context pgshardContext3)
    {
        this.postgresContext = postgresContext;
        this.postgresContext2 = postgresContext2;
        this.postgresContext3 = pgshardContext3;
    }

    public async Task<string> write(string url)
    {
        try
        {
            var ConsistentHashing = new ConsistentHashing();
            var url_id = Guid.NewGuid().ToString().Substring(0, 5);
            var obj = new UrlShortner() { Url = url, UrlId = url_id };

            var node = ConsistentHashing.GetNode(url_id);

            DbContext _ctx;
            switch (node)
            {
                case 0:
                    _ctx = postgresContext;
                    break;

                case 1:
                    _ctx = postgresContext2;
                    break;

                case 2:
                    _ctx = postgresContext3;
                    break;

                default:
                    _ctx = postgresContext;
                    break;
            }

            await _ctx.Set<UrlShortner>().AddAsync(obj);
            await _ctx.SaveChangesAsync();
            return url_id;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public async Task<UrlShortner> getUrl(string urlId)
    {
        var node = new ConsistentHashing().GetNode(urlId);
        DbSet<UrlShortner> _dbsetForUrlShortner;
        switch (node)
        {
            case 0:
                _dbsetForUrlShortner =  postgresContext.UrlShortners;
                break;

            case 1:
                _dbsetForUrlShortner =  postgresContext2.UrlShortners;
                break;

            case 2:
                _dbsetForUrlShortner =  postgresContext3.UrlShortners;
                break;

            default:
                _dbsetForUrlShortner = postgresContext.UrlShortners;
                break;
        }

        var result = await (from e in _dbsetForUrlShortner.AsNoTracking()
                                    where e.UrlId == urlId
                                    select e).FirstOrDefaultAsync()?? new UrlShortner();

        return result;
    }
}
