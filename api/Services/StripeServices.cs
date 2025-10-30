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
	IConfiguration _configuration;
	public StripeService(IConfiguration configuration)
	{
		_configuration = configuration;
	}
	public async Task<string> CreateCustomerAsync(string email, string name)
	{
		StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
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
		StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
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
		StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

		var options = new SessionCreateOptions
		{
			SuccessUrl = Environment.GetEnvironmentVariable("ASPNETCORE_REACTAPPURL"),
			LineItems = new List<SessionLineItemOptions>
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