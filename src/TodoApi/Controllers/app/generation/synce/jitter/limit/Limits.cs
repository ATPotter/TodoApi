using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using Swashbuckle.SwaggerGen.Application;
using Swashbuckle.SwaggerGen.Generator;

namespace TodoApi.Controllers.app.generation.synce.jitter
{
    /// <summary>
    /// /api/app/generation/synce/jitter/limit...
    /// </summary>
    public partial class appController
    {

        /// <summary>Returns the limts values for the CFP2 interface, based on G8262.1</summary>
        /// <returns>Information about the CFP2 interface limits</returns>
        /// 
        /// <remarks>
        /// This only returns information about the <b>CFP2</b> interface
        /// <para>
        /// 
        /// Note that the response is returned as an array of frequency bands, and
        /// for each band there is a corresponding limit value.
        ///</para>
        ///
        /// <para>
        /// 
        /// If there is a "denominator" field, it indicates that the actual frequency
        /// should be divided by this "denominator" to find the actual limit.
        /// </para>
        /// <para>
        /// 
        /// The limits are defined in <a href="https://www.itu.int/rec/T-REC-G.8262-201501-I/en">G8262.1</a>
        /// </para>
        /// 
        /// </remarks>
        [HttpGet("/api/app/generation/synce/jitter/limit/cfp2")]
        public int GetLimitCfp2()
        {
            return 27;
        }

        /// <summary>Returns the limts values for the CFP4 interface, based on G8262.1</summary>
        /// <returns>Information about the CFP4 interface limits</returns>
        /// 
        /// <remarks>
        /// This only returns information about the <b>CFP4</b> interface
        /// <para>
        /// 
        /// Note that the response is returned as an array of frequency bands, and
        /// for each band there is a corresponding limit value.
        ///</para>
        ///
        /// <para>
        /// 
        /// If there is a "denominator" field, it indicates that the actual frequency
        /// should be divided by this "denominator" to find the actual limit.
        /// </para>
        /// <para>
        /// 
        /// The limits are defined in <a href="https://www.itu.int/rec/T-REC-G.8262-201501-I/en">G8262.1</a>
        /// </para>
        /// 
        /// </remarks>
        [HttpGet("api/app/generation/synce/jitter/limit/cfp4")]
        public int GetLimitCfp4()
        {
            return 48;
        }

        /// <summary>Returns the limts values for the CXP interface, based on G8262.1</summary>
        /// <returns>Information about the CXP interface limits</returns>
        /// 
        /// <remarks>
        /// This only returns information about the <b>CXP</b> interface
        /// <para>
        /// 
        /// Note that the response is returned as an array of frequency bands, and
        /// for each band there is a corresponding limit value.
        ///</para>
        ///
        /// <para>
        /// 
        /// If there is a "denominator" field, it indicates that the actual frequency
        /// should be divided by this "denominator" to find the actual limit.
        /// </para>
        /// <para>
        /// 
        /// The limits are defined in <a href="https://www.itu.int/rec/T-REC-G.8262-201501-I/en">G8262.1</a>
        /// </para>
        /// 
        /// </remarks>
        [HttpGet("api/app/generation/synce/jitter/limit/cxp")]
        public int GetLimitCxp()
        {
            return 96;
        }
    }
}
