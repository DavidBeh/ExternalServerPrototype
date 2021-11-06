using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExternalServerPrototype
{
    
    public class ExternalServerS : ExternalServerService.ExternalServerServiceBase
    {
        private readonly ILogger<ExternalServerS> _logger;

        public ExternalServerS(ILogger<ExternalServerS> logger)
        {
            _logger = logger;
        }

        public override Task<RepeatReply> RepeatWords(RepeatRequest request, ServerCallContext context)
        {
            
            return Task.FromResult(new RepeatReply()
                {
                    RepeatedWords = string.Concat(Enumerable.Repeat(request.Words, request.Repeats))
                }
            );
        }
    }
}