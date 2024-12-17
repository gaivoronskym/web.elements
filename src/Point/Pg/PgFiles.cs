using System.Net;
using Point.Exceptions;
using Point.Rq;
using Point.Rs;
using Yaapii.Atoms.IO;

namespace Point.Pg;

public sealed class PgFiles : IPage
{
    private readonly FileInfo file;

    public PgFiles(string path)
        : this(new FileInfo(path))
    {
    }

    public PgFiles(FileInfo file)
    {
        this.file = file;
    }

    public Task<IResponse> Act(IRequest req)
    {
        if (!file.Exists)
        {
            throw new HttpException(
                HttpStatusCode.NotFound,
                $"file {file.FullName} not found"
            );
        }

        return new IResponse.Smart(
            new RsWithBody(
                new InputOf(file).Stream()
            )
        ).AsTask();
    }
}