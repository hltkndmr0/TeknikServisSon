namespace TeknikServis.Domain.Requests.IsEmri;

public record AddIsNotuRequest(
    Guid IsEmriId,
    string NotMetni
);