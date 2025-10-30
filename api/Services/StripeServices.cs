using ClientPortalApi.DTOs;
using Stripe;
using Stripe.Checkout;

namespace ClientPortalApi.Services;

public interface IStripeService
{
	Task<string> CreateCustomerAsync(string email, string name);
	Task<StripeProduct> CreateProductAsync(string name, string description, int priceCents, string currency);
	Task<Session> GetSessionAsync(StripeProduct product);
}

public class StripeService : IStripeService
{
	public StripeService(IConfiguration configuration)
	{
		StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
	}
	public async Task<string> CreateCustomerAsync(string email, string name)
	{
		var optionsCustomer = new CustomerCreateOptions
		{
			Email = email,
			Name = name
		};

		var serviceCustomer = new CustomerService();
		var customer = await serviceCustomer.CreateAsync(optionsCustomer);

		return customer.Id;
	}

	public async Task<StripeProduct> CreateProductAsync(string name, string description, int price, string currency)
	{
		var client = new StripeClient();
		var optioinsProduct = new ProductCreateOptions
		{
			Name = name,
			Description = description,
		};
		var serviceProduct = new ProductService();
		var product = await serviceProduct.CreateAsync(optioinsProduct);
		var optionsPrice = new PriceCreateOptions
		{
			Product = product.Id,
			UnitAmount = price,
			Currency = currency,
		};
		var servicePrice = new PriceService();
		var priceResult = servicePrice.CreateAsync(optionsPrice);

		return new StripeProduct { ProductId = product.Id, PriceId = priceResult.Result.Id };
	}

	public async Task<Session> GetSessionAsync(StripeProduct product)
	{
		var options = new SessionCreateOptions
		{
			SuccessUrl = "http://localhost:8081/",
			LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
			{
				new SessionLineItemOptions
				{
					Price = product.PriceId,
					Quantity = 1
				},
			},
			Mode = "payment",
		};
		var service = new Stripe.Checkout.SessionService();
		Session session = service.Create(options);

		return session;
	}
}