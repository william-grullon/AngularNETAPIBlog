using System.Text;

namespace AngularNETAPIBlog.API.Repositories.Implementation
{
    internal static class MarkdownFrontMatter
    {
        public static (Dictionary<string, string> FrontMatter, string Body) Parse(string markdown)
        {
            var frontMatter = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            using var reader = new StringReader(markdown);
            var firstLine = reader.ReadLine();

            if (!string.Equals(firstLine?.Trim(), "---", StringComparison.Ordinal))
            {
                return (frontMatter, markdown);
            }

            var bodyBuilder = new StringBuilder();
            var inBody = false;

            while (true)
            {
                var line = reader.ReadLine();
                if (line is null)
                {
                    break;
                }

                if (!inBody && line.Trim() == "---")
                {
                    inBody = true;
                    continue;
                }

                if (!inBody)
                {
                    var separatorIndex = line.IndexOf(':');
                    if (separatorIndex <= 0)
                    {
                        continue;
                    }

                    var key = line[..separatorIndex].Trim();
                    var value = line[(separatorIndex + 1)..].Trim();
                    frontMatter[key] = value;
                    continue;
                }

                bodyBuilder.AppendLine(line);
            }

            return (frontMatter, bodyBuilder.ToString().TrimStart('\r', '\n').TrimEnd('\r', '\n'));
        }

        public static string Build(IDictionary<string, string> frontMatter, string body)
        {
            var builder = new StringBuilder();
            builder.AppendLine("---");

            foreach (var pair in frontMatter)
            {
                builder.AppendLine($"{pair.Key}: {pair.Value}");
            }

            builder.AppendLine("---");

            if (!string.IsNullOrWhiteSpace(body))
            {
                builder.AppendLine(body.TrimEnd());
            }

            return builder.ToString();
        }
    }
}
