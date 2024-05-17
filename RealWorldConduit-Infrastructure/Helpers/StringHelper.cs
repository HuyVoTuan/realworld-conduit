using NanoidDotNet;
using Slugify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealWorldConduit_Infrastructure.Helpers
{
    public static class StringHelper
    {
        public static String GenerateSlug(String memberName)
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var uniqueId = Nanoid.Generate(alphabet, size: 10);

            var config = new SlugHelperConfiguration();
            config.StringReplacements = new()
            {
                ["&"] = "-",
                [","] = "-",
                [" "] = "-",
            };
            config.ForceLowerCase = true;

            var helper = new SlugHelper(config);
            var result = helper.GenerateSlug($"{memberName} {uniqueId}");

            return result;
        }

        public static string GenerateJobId(string jobId) => $"{jobId}.jobDetail";
        public static string GenerateJobTriggerId(string jobId) => $"{jobId}.jobTrigger";
    }
}
