using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Application.CQRS.Roles.Queries.GetAllClaimTitles
{
	public class GetAllClaimTitlesQuery : IRequest<List<string>>
	{
		public class Handler : IRequestHandler<GetAllClaimTitlesQuery, List<string>>
		{
			public async Task<List<string>> Handle(GetAllClaimTitlesQuery request, CancellationToken cancellationToken)
			{
				var list = new List<string>();

				list.Add("Role");

				return list;
			}

		}
	}
}

