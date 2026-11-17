using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Innolux.Application.Dto
{
  public class GetRoutesOutPut
  {
    public string Id { get; set; }
    public string ParentId { get; set; }

    public string ComponentMap { get; set; }


    public string Name { get; set; }


    public RoutMetaOutPut Meta { get; set; }
  }

  public class RoutMetaOutPut
  {
    public string Title { get; set; }
    public bool NoCache { get; set; } = true;
    public string Icon { get; set; }

    public List<string> Roles { get; set; }
    public RoutMetaOutPut()
    {
      Roles = new List<string>();
    }

  }
}
