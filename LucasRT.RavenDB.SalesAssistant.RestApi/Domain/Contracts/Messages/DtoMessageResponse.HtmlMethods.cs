using System.Text;
using System.Text.Encodings.Web;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Contracts.Messages
{
    public partial class DtoMessageResponse
    {
        /// <summary>
        /// Converte uma lista de DtoMessageResponse em um HTML completo (doctype+head+body).
        /// </summary>
        public static string ToHtml(IEnumerable<DtoMessageResponse> items, HtmlRenderOptions options = null)
        {
            options ??= HtmlRenderOptions.Default;
            var enc = HtmlEncoder.Default;
            var list = items?.ToList() ?? [];

            var sb = new StringBuilder();
            sb.AppendLine("<!doctype html>");
            sb.AppendLine("<html lang=\"pt-br\">");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset=\"utf-8\">");
            sb.AppendLine("<meta name=\"viewport\" content=\"width=device-width,initial-scale=1\">");
            sb.Append("<title>").Append(enc.Encode(options.Title ?? "Mensagens")).AppendLine("</title>");

            if (options.IncludeStyles)
            {
                sb.AppendLine("<style>");
                sb.AppendLine(@"
:root { --fg:#111827; --muted:#6b7280; --border:#e5e7eb; --bg:#ffffff; }
*{box-sizing:border-box}
body{font-family:system-ui,-apple-system,Segoe UI,Roboto,Arial,sans-serif;background:var(--bg);color:var(--fg);margin:24px;line-height:1.55}
.container{max-width:1000px;margin:0 auto}
.header{margin-bottom:20px}
.header h1{margin:0 0 4px 0;font-size:24px}
.header .meta{color:var(--muted);font-size:14px}
.grid{display:grid;grid-template-columns:1fr;gap:16px}
@media(min-width:800px){.grid{grid-template-columns:1fr 1fr}}
.card{border:1px solid var(--border);border-radius:14px;padding:16px;box-shadow:0 1px 2px rgba(0,0,0,.05);background:#fff}
.kv{display:grid;grid-template-columns:160px 1fr;gap:8px 16px;margin:8px 0}
.kv .label{font-weight:600}
.section{margin-top:10px;padding-top:10px;border-top:1px dashed var(--border)}
.section h3{margin:0 0 8px 0;font-size:16px}
.body p{margin:6px 0}
.badge{display:inline-block;padding:.15rem .5rem;border:1px solid var(--border);border-radius:999px;font-size:12px;color:var(--muted)}
            ");
                sb.AppendLine("</style>");
            }

            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<div class=\"container\">");
            sb.Append("<div class=\"header\"><h1>")
              .Append(enc.Encode(options.Title ?? "Mensagens"))
              .Append("</h1><div class=\"meta\">")
              .Append(enc.Encode($"{list.Count} item(ns)"))
              .AppendLine("</div></div>");

            sb.AppendLine("<div class=\"grid\">");
            foreach (var item in list)
            {
                sb.Append(RenderItemCard(item, enc, options));
            }
            sb.AppendLine("</div>"); // .grid
            sb.AppendLine("</div>"); // .container
            sb.AppendLine("</body></html>");

            return sb.ToString();
        }

        /// <summary>
        /// Converte UM item apenas em um HTML completo. Útil para endpoints de detalhe.
        /// </summary>
        public static string ToHtml(DtoMessageResponse item, HtmlRenderOptions options = null)
            => ToHtml([item], options);

        // ------------------------------
        //  Helpers privados
        // ------------------------------

        private static string RenderItemCard(DtoMessageResponse item, HtmlEncoder enc, HtmlRenderOptions options)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<div class=\"card\">");

            // Cabeçalho simples do card
            if (!string.IsNullOrWhiteSpace(item.Id) || !string.IsNullOrWhiteSpace(item.LeadId))
            {
                sb.Append("<div style=\"display:flex;gap:8px;align-items:center;justify-content:space-between\">");
                sb.Append("<div class=\"badge\">")
                  .Append(enc.Encode($"LeadOrigin: {item.LeadOrigin}"))
                  .Append("</div>");
                sb.Append("<div class=\"badge\">")
                  .Append(enc.Encode($"Culture: {item.CultureName}"))
                  .Append("</div>");
                sb.Append("</div>");
            }

            // Key-Values principais
            sb.AppendLine("<div class=\"kv\">");
            AppendKv(sb, "Id", item.Id, enc);
            AppendKv(sb, "LeadId", item.LeadId, enc);
            AppendKv(sb, "Lead Name", item.LeadName, enc);
            AppendKv(sb, "LeadOrigin", item.LeadOrigin.ToString(), enc);
            AppendKv(sb, "CultureName", item.CultureName.ToString(), enc);
            sb.AppendLine("</div>");

            // Seções Professional / Personal
            if (item.Professional != null)
            {
                sb.Append(RenderMessageSection("Professional", item.Professional, enc));
            }
            if (item.Personal != null)
            {
                sb.Append(RenderMessageSection("Personal", item.Personal, enc));
            }

            sb.AppendLine("</div>"); // .card
            return sb.ToString();
        }

        private static void AppendKv(StringBuilder sb, string label, string value, HtmlEncoder enc)
        {
            if (value == null) value = string.Empty;
            sb.Append("<div class=\"label\">").Append(enc.Encode(label)).Append("</div>")
              .Append("<div>").Append(enc.Encode(value)).Append("</div>");
        }

        private static string RenderMessageSection(string title, DtoMessage msg, HtmlEncoder enc)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<div class=\"section\">");
            sb.Append("<h3>").Append(enc.Encode(title)).AppendLine("</h3>");

            sb.AppendLine("<div class=\"kv\">");
            AppendKv(sb, "Recipient", msg.Recipient ?? string.Empty, enc);
            AppendKv(sb, "Subject", msg.Subject ?? string.Empty, enc);
            sb.AppendLine("</div>");

            // Corpo com parágrafos
            sb.AppendLine("<div class=\"body\">");
            sb.Append(EncodeParagraphs(msg.Body ?? string.Empty, enc));
            sb.AppendLine("</div>");

            sb.AppendLine("</div>");
            return sb.ToString();
        }

        /// <summary>
        /// Converte texto com \n\n em parágrafos e \n simples em <br>, escapando conteúdo.
        /// </summary>
        private static string EncodeParagraphs(string text, HtmlEncoder enc)
        {
            if (string.IsNullOrEmpty(text))
                return "<p></p>";

            var blocks = SplitParagraphs(text);
            var sb = new StringBuilder();
            foreach (var block in blocks)
            {
                var encoded = enc.Encode(block)
                                 .Replace("\r\n", "<br>")
                                 .Replace("\n", "<br>")
                                 .Replace("\r", "<br>");
                sb.Append("<p>").Append(encoded).Append("</p>");
            }
            return sb.ToString();
        }

        private static IEnumerable<string> SplitParagraphs(string s)
        {
            // quebra dupla conta como novo parágrafo; preserva blocos vazios intencionais
            return s.Split(["\r\n\r\n", "\n\n", "\r\r"], System.StringSplitOptions.None);
        }

        // ------------------------------
        //  Opções de renderização
        // ------------------------------
        public sealed class HtmlRenderOptions
        {
            public string Title { get; init; } = "Mensagens";
            public bool IncludeStyles { get; init; } = true;

            public static HtmlRenderOptions Default { get; } = new HtmlRenderOptions();
        }

    }
}
