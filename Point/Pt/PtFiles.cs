﻿using Point.Exceptions;
using Point.Rq.Interfaces;
using Point.Rs;
using System.Net;
using Yaapii.Atoms.IO;

namespace Point.Pt
{
    public sealed class PtFiles : IPoint
    {
        private readonly FileInfo _file;

        public PtFiles(string path)
            : this(new FileInfo(path))
        {

        }

        public PtFiles(FileInfo file)
        {
            _file = file;
        }

        public Task<IResponse> Act(IRequest req)
        {
            if (!_file.Exists)
            {
                throw new HttpException(
                    HttpStatusCode.NotFound,
                    $"file {_file.FullName} not found"
                );
            }

            return Task.FromResult<IResponse>(
                        new RsWithBody(
                    new InputOf(_file).Stream()
                )
            );
        }
    }
}
