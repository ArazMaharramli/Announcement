using System.Collections.Generic;
using Domain.Common;

namespace Application.Common.Models.ConfigModels;

public class StaticPageModel
{
    public Meta SEO { get; set; }
    public List<string> Headers { get; set; }
}
