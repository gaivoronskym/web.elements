using Point.Exceptions;
using Point.Rq.Interfaces;
using Point.Rs;
using System.Net;
using Yaapii.Atoms.IO;

namespace Point.Pt
{
    public sealed class PtFiles : IPoint
    {
        private readonly FileInfo file;

        public PtFiles(string path)
            : this(new FileInfo(path))
        {

        }

        public PtFiles(FileInfo file)
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

            return Task.FromResult<IResponse>(
                        new RsWithBody(
                    new InputOf(file).Stream()
                )
            );
        }
    }
}
