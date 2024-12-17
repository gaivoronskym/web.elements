using System.Buffers;
using System.IO.Pipelines;
using Point.Http;
using Yaapii.Atoms.Scalar;

namespace Point.Rq;

public sealed class RqLive : RqWrap
{
    public RqLive(Stream input)
        : base(
            new ScalarOf<IRequest>(() => AsyncHelper.RunSync(() => ParseAsync(input)))
        )
    {
    }

    private static async Task<IRequest> ParseAsync(Stream input)
    {
        const int bufferSize = 65536 * 3;
        var pipe = PipeReader.Create(
            input,
            new StreamPipeReaderOptions(pool: MemoryPool<byte>.Shared, leaveOpen: true, bufferSize: bufferSize)
        );
        
        var pipeResult = await pipe.ReadAsync();
        var token = new HttpToken(pipe, pipeResult.Buffer);
        IList<string> head = new List<string>();
        while (!token.NextIs("\r\n"))
        {
            var line = token.AsString('\r');
            if (line.StartsWith(' ') && head.Count > 0)
            {
                head[^1] += line;
            }
            else
            {
                head.Add(line);
            }
            
            token = token
                .SkipTo('\r')
                .SkipNext(2);
        }
        
        return new RequestOf(head, token.Stream());
    }
}