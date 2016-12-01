using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Controllers.app.generation.synce.jitter
{
    public partial class appController
    {
        [HttpGet("app/generation/synce/jitter")]
        public string Get()
        {
            return "Jitter Generation app all info";
        }
    }
}
