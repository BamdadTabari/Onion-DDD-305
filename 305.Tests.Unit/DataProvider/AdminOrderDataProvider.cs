using Core.EntityFramework.Models;
using Core.Pagination;
using GoldAPI.Application.AdminOrderFeatures.Command;
using GoldAPI.Application.AdminOrderFeatures.Query;
using GoldAPI.Application.AdminOrderFeatures.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoldAPI.Test.DataProvider;
public class AdminOrderDataProvider
{
	public static Order Row(string name = "name", long id = 1, string slug = "slug")
	=> new Order()
	{
		id = id,
		email = "info@304.com",
		mobile = "09309309393",
		name = name,
		created_at = new DateTime(2025, 1, 1, 12, 0, 0),
		updated_at = new DateTime(2025, 1, 1, 12, 0, 0),
		slug = slug,
		amount = 100000,
		authority = "a;jd[aohrfu[oqhwerdua",
		card_number = "6337998554467389",
		date_paid = new DateTime(2025, 1, 1, 12, 0, 0),
		order_status = Core.Enums.OrderStatusEnum.Success,
		ref_id = "ref",
		response_message = "response",
		status = 100,
		user_id = 1,
	};

	public static DeleteAdminOrderCommand Delete(long id = 1)
		=> new DeleteAdminOrderCommand()
		{
			id = id,
		};

	public static GetAdminOrderBySlugQuery GetBySlug(string slug = "slug")
	=> new GetAdminOrderBySlugQuery()
	{
		slug = slug,
	};

	public static AdminOrderResponse GetOne(string slug = "slug", string name = "name")
		=> new AdminOrderResponse()
		{
			id = 1,
			email = "info@304.com",
			mobile = "09309309393",
			name = name,
			created_at = new DateTime(2025, 1, 1, 12, 0, 0),
			updated_at = new DateTime(2025, 1, 1, 12, 0, 0),
			slug = slug,
			amount = 100000,
			authority = "a;jd[aohrfu[oqhwerdua",
			card_number = "6337998554467389",
			date_paid = new DateTime(2025, 1, 1, 12, 0, 0),
			order_status = Core.Enums.OrderStatusEnum.Success,
			ref_id = "ref",
			response_message = "response",
			status = 100,
			user_id = 1,
		};

	public static GetPaginatedAdminOrderQuery GetByQueryFilter(string searchTerm = "")
	=> new GetPaginatedAdminOrderQuery()
	{
		Page = 1,
		PageSize = 10,
		SearchTerm = searchTerm,
	};

	public static PaginatedList<Order> GetPaginatedList()
	=> new PaginatedList<Order>(new List<Order>
		{
			Row(name: "Tech", id: 1, slug: "tech"),
			Row(name: "Health", id: 2, slug: "health")
		}
	, count: 2, page: 1, pageSize: 10);
}
