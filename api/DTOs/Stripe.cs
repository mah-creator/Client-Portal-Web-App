using System;
using System.Collections.Generic;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
namespace ClientPortalApi.DTOs;

public class StripeProduct
{
	public string ProductId { get; set; } = null!;
	public string PriceId { get; set; } = null!;
}

public delegate void StripePaymentCompletedHandler(string paymentIntent, string sessionId);

public class StripeCompletedSessionStatus
{
	public event StripePaymentCompletedHandler OnPaymentCompleted;

	public void CompletePayment(string paymentIntent, string sessionId, string status, string eventType)
	{
		if (eventType == "checkout.session.completed" && status != null && status.Equals("complete", StringComparison.OrdinalIgnoreCase))
			OnPaymentCompleted?.Invoke(paymentIntent, sessionId);
	}
}


public partial class CompletedStripeSessionEvent
{
	[JsonPropertyName("id")]
	public string Id { get; set; }

	[JsonPropertyName("object")]
	public string Object { get; set; }

	[JsonPropertyName("api_version")]
	public string ApiVersion { get; set; }

	[JsonPropertyName("created")]
	public long Created { get; set; }

	[JsonPropertyName("data")]
	public Data Data { get; set; }

	[JsonPropertyName("livemode")]
	public bool Livemode { get; set; }

	[JsonPropertyName("pending_webhooks")]
	public long PendingWebhooks { get; set; }

	[JsonPropertyName("request")]
	public Request Request { get; set; }

	[JsonPropertyName("type")]
	public string Type { get; set; }
}

public partial class Data
{
	[JsonPropertyName("object")]
	public Object Object { get; set; }
}

public partial class Object
{
	[JsonPropertyName("id")]
	public string Id { get; set; }

	[JsonPropertyName("object")]
	public string ObjectObject { get; set; }

	[JsonPropertyName("amount")]
	public long Amount { get; set; }

	[JsonPropertyName("amount_captured")]
	public long AmountCaptured { get; set; }

	[JsonPropertyName("amount_refunded")]
	public long AmountRefunded { get; set; }

	[JsonPropertyName("application")]
	public object Application { get; set; }

	[JsonPropertyName("application_fee")]
	public object ApplicationFee { get; set; }

	[JsonPropertyName("application_fee_amount")]
	public object ApplicationFeeAmount { get; set; }

	[JsonPropertyName("balance_transaction")]
	public object BalanceTransaction { get; set; }

	[JsonPropertyName("billing_details")]
	public BillingDetails BillingDetails { get; set; }

	[JsonPropertyName("calculated_statement_descriptor")]
	public string CalculatedStatementDescriptor { get; set; }

	[JsonPropertyName("captured")]
	public bool Captured { get; set; }

	[JsonPropertyName("created")]
	public long Created { get; set; }

	[JsonPropertyName("currency")]
	public string Currency { get; set; }

	[JsonPropertyName("customer")]
	public object Customer { get; set; }

	[JsonPropertyName("description")]
	public object Description { get; set; }

	[JsonPropertyName("destination")]
	public object Destination { get; set; }

	[JsonPropertyName("dispute")]
	public object Dispute { get; set; }

	[JsonPropertyName("disputed")]
	public bool Disputed { get; set; }

	[JsonPropertyName("failure_balance_transaction")]
	public object FailureBalanceTransaction { get; set; }

	[JsonPropertyName("failure_code")]
	public object FailureCode { get; set; }

	[JsonPropertyName("failure_message")]
	public object FailureMessage { get; set; }

	[JsonPropertyName("fraud_details")]
	public FraudDetails FraudDetails { get; set; }

	[JsonPropertyName("livemode")]
	public bool Livemode { get; set; }

	[JsonPropertyName("metadata")]
	public FraudDetails Metadata { get; set; }

	[JsonPropertyName("on_behalf_of")]
	public object OnBehalfOf { get; set; }

	[JsonPropertyName("order")]
	public object Order { get; set; }

	[JsonPropertyName("outcome")]
	public Outcome Outcome { get; set; }

	[JsonPropertyName("paid")]
	public bool Paid { get; set; }

	[JsonPropertyName("payment_intent")]
	public string PaymentIntent { get; set; }

	[JsonPropertyName("payment_method")]
	public string PaymentMethod { get; set; }

	[JsonPropertyName("payment_method_details")]
	public PaymentMethodDetails PaymentMethodDetails { get; set; }

	[JsonPropertyName("presentment_details")]
	public PresentmentDetails PresentmentDetails { get; set; }

	[JsonPropertyName("radar_options")]
	public FraudDetails RadarOptions { get; set; }

	[JsonPropertyName("receipt_email")]
	public object ReceiptEmail { get; set; }

	[JsonPropertyName("receipt_number")]
	public object ReceiptNumber { get; set; }

	[JsonPropertyName("receipt_url")]
	public Uri ReceiptUrl { get; set; }

	[JsonPropertyName("refunded")]
	public bool Refunded { get; set; }

	[JsonPropertyName("review")]
	public object Review { get; set; }

	[JsonPropertyName("shipping")]
	public object Shipping { get; set; }

	[JsonPropertyName("source")]
	public object Source { get; set; }

	[JsonPropertyName("source_transfer")]
	public object SourceTransfer { get; set; }

	[JsonPropertyName("statement_descriptor")]
	public object StatementDescriptor { get; set; }

	[JsonPropertyName("statement_descriptor_suffix")]
	public object StatementDescriptorSuffix { get; set; }

	[JsonPropertyName("status")]
	public string Status { get; set; }

	[JsonPropertyName("transfer_data")]
	public object TransferData { get; set; }

	[JsonPropertyName("transfer_group")]
	public object TransferGroup { get; set; }
}

public partial class BillingDetails
{
	[JsonPropertyName("address")]
	public Address Address { get; set; }

	[JsonPropertyName("email")]
	public string Email { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("phone")]
	public object Phone { get; set; }

	[JsonPropertyName("tax_id")]
	public object TaxId { get; set; }
}

public partial class Address
{
	[JsonPropertyName("city")]
	public object City { get; set; }

	[JsonPropertyName("country")]
	public string Country { get; set; }

	[JsonPropertyName("line1")]
	public object Line1 { get; set; }

	[JsonPropertyName("line2")]
	public object Line2 { get; set; }

	[JsonPropertyName("postal_code")]
	public object PostalCode { get; set; }

	[JsonPropertyName("state")]
	public object State { get; set; }
}

public partial class FraudDetails
{
}

public partial class Outcome
{
	[JsonPropertyName("advice_code")]
	public object AdviceCode { get; set; }

	[JsonPropertyName("network_advice_code")]
	public object NetworkAdviceCode { get; set; }

	[JsonPropertyName("network_decline_code")]
	public object NetworkDeclineCode { get; set; }

	[JsonPropertyName("network_status")]
	public string NetworkStatus { get; set; }

	[JsonPropertyName("reason")]
	public object Reason { get; set; }

	[JsonPropertyName("risk_level")]
	public string RiskLevel { get; set; }

	[JsonPropertyName("risk_score")]
	public long RiskScore { get; set; }

	[JsonPropertyName("seller_message")]
	public string SellerMessage { get; set; }

	[JsonPropertyName("type")]
	public string Type { get; set; }
}

public partial class PaymentMethodDetails
{
	[JsonPropertyName("card")]
	public Card Card { get; set; }

	[JsonPropertyName("type")]
	public string Type { get; set; }
}

public partial class Card
{
	[JsonPropertyName("amount_authorized")]
	public long AmountAuthorized { get; set; }

	[JsonPropertyName("authorization_code")]
	[JsonConverter(typeof(ParseStringConverter))]
	public long AuthorizationCode { get; set; }

	[JsonPropertyName("brand")]
	public string Brand { get; set; }

	[JsonPropertyName("checks")]
	public Checks Checks { get; set; }

	[JsonPropertyName("country")]
	public string Country { get; set; }

	[JsonPropertyName("exp_month")]
	public long ExpMonth { get; set; }

	[JsonPropertyName("exp_year")]
	public long ExpYear { get; set; }

	[JsonPropertyName("extended_authorization")]
	public ExtendedAuthorization ExtendedAuthorization { get; set; }

	[JsonPropertyName("fingerprint")]
	public string Fingerprint { get; set; }

	[JsonPropertyName("funding")]
	public string Funding { get; set; }

	[JsonPropertyName("incremental_authorization")]
	public ExtendedAuthorization IncrementalAuthorization { get; set; }

	[JsonPropertyName("installments")]
	public object Installments { get; set; }

	[JsonPropertyName("last4")]
	[JsonConverter(typeof(ParseStringConverter))]
	public long Last4 { get; set; }

	[JsonPropertyName("mandate")]
	public object Mandate { get; set; }

	[JsonPropertyName("multicapture")]
	public ExtendedAuthorization Multicapture { get; set; }

	[JsonPropertyName("network")]
	public string Network { get; set; }

	[JsonPropertyName("network_token")]
	public NetworkToken NetworkToken { get; set; }

	[JsonPropertyName("network_transaction_id")]
	public string NetworkTransactionId { get; set; }

	[JsonPropertyName("overcapture")]
	public Overcapture Overcapture { get; set; }

	[JsonPropertyName("regulated_status")]
	public string RegulatedStatus { get; set; }

	[JsonPropertyName("three_d_secure")]
	public object ThreeDSecure { get; set; }

	[JsonPropertyName("wallet")]
	public object Wallet { get; set; }
}

public partial class Checks
{
	[JsonPropertyName("address_line1_check")]
	public object AddressLine1Check { get; set; }

	[JsonPropertyName("address_postal_code_check")]
	public object AddressPostalCodeCheck { get; set; }

	[JsonPropertyName("cvc_check")]
	public string CvcCheck { get; set; }
}

public partial class ExtendedAuthorization
{
	[JsonPropertyName("status")]
	public string Status { get; set; }
}

public partial class NetworkToken
{
	[JsonPropertyName("used")]
	public bool Used { get; set; }
}

public partial class Overcapture
{
	[JsonPropertyName("maximum_amount_capturable")]
	public long MaximumAmountCapturable { get; set; }

	[JsonPropertyName("status")]
	public string Status { get; set; }
}

public partial class PresentmentDetails
{
	[JsonPropertyName("presentment_amount")]
	public long PresentmentAmount { get; set; }

	[JsonPropertyName("presentment_currency")]
	public string PresentmentCurrency { get; set; }
}

public partial class Request
{
	[JsonPropertyName("id")]
	public object Id { get; set; }

	[JsonPropertyName("idempotency_key")]
	public Guid IdempotencyKey { get; set; }
}

public partial class Welcome
{
	public static Welcome FromJson(string json) => JsonSerializer.Deserialize<Welcome>(json, Converter.Settings);
}

public static class Serialize
{
	public static string ToJson(this Welcome self) => JsonSerializer.Serialize(self, Converter.Settings);
}

internal static class Converter
{
	public static readonly JsonSerializerOptions Settings = new(JsonSerializerDefaults.General)
	{
		Converters =
		{
			new DateOnlyConverter(),
			new TimeOnlyConverter(),
			IsoDateTimeOffsetConverter.Singleton
		},
	};
}

internal class ParseStringConverter : JsonConverter<long>
{
	public override bool CanConvert(Type t) => t == typeof(long);

	public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		long l;
		if (Int64.TryParse(value, out l))
		{
			return l;
		}
		throw new Exception("Cannot unmarshal type long");
	}

	public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
	{
		JsonSerializer.Serialize(writer, value.ToString(), options);
		return;
	}

	public static readonly ParseStringConverter Singleton = new ParseStringConverter();
}

public class DateOnlyConverter : JsonConverter<DateOnly>
{
	private readonly string serializationFormat;
	public DateOnlyConverter() : this(null) { }

	public DateOnlyConverter(string? serializationFormat)
	{
		this.serializationFormat = serializationFormat ?? "yyyy-MM-dd";
	}

	public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return DateOnly.Parse(value!);
	}

	public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
			=> writer.WriteStringValue(value.ToString(serializationFormat));
}

public class TimeOnlyConverter : JsonConverter<TimeOnly>
{
	private readonly string serializationFormat;

	public TimeOnlyConverter() : this(null) { }

	public TimeOnlyConverter(string? serializationFormat)
	{
		this.serializationFormat = serializationFormat ?? "HH:mm:ss.fff";
	}

	public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return TimeOnly.Parse(value!);
	}

	public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
			=> writer.WriteStringValue(value.ToString(serializationFormat));
}

internal class IsoDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
	public override bool CanConvert(Type t) => t == typeof(DateTimeOffset);

	private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

	private DateTimeStyles _dateTimeStyles = DateTimeStyles.RoundtripKind;
	private string? _dateTimeFormat;
	private CultureInfo? _culture;

	public DateTimeStyles DateTimeStyles
	{
		get => _dateTimeStyles;
		set => _dateTimeStyles = value;
	}

	public string? DateTimeFormat
	{
		get => _dateTimeFormat ?? string.Empty;
		set => _dateTimeFormat = (string.IsNullOrEmpty(value)) ? null : value;
	}

	public CultureInfo Culture
	{
		get => _culture ?? CultureInfo.CurrentCulture;
		set => _culture = value;
	}

	public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
	{
		string text;


		if ((_dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
				|| (_dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
		{
			value = value.ToUniversalTime();
		}

		text = value.ToString(_dateTimeFormat ?? DefaultDateTimeFormat, Culture);

		writer.WriteStringValue(text);
	}

	public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		string? dateText = reader.GetString();

		if (string.IsNullOrEmpty(dateText) == false)
		{
			if (!string.IsNullOrEmpty(_dateTimeFormat))
			{
				return DateTimeOffset.ParseExact(dateText, _dateTimeFormat, Culture, _dateTimeStyles);
			}
			else
			{
				return DateTimeOffset.Parse(dateText, Culture, _dateTimeStyles);
			}
		}
		else
		{
			return default(DateTimeOffset);
		}
	}


	public static readonly IsoDateTimeOffsetConverter Singleton = new IsoDateTimeOffsetConverter();
}