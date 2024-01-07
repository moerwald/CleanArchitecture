﻿using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Dapper;

namespace Bookify.Application.Bookings.Apartments.SearchApartments;

internal sealed class SearchAppartmentsQueryHandler(
    ISqlConnectionFactory connectionFactory)
    : IQueryHandler<SearchApartmentsQuery, IReadOnlyList<ApartmentResponse>>
{

    private static readonly int[] ActiveBookingStatuses =
    {
        (int)BookingStatus.Reserved,
        (int)BookingStatus.Confirmend,
        (int)BookingStatus.Completed,
    };

    private readonly ISqlConnectionFactory _connectionFactory = connectionFactory;

    public async Task<Result<IReadOnlyList<ApartmentResponse>>> Handle(SearchApartmentsQuery request, CancellationToken cancellationToken)
    {
        if (request.StartDate > request.EndDate)
        {
            return new List<ApartmentResponse>();
        }

        using var connection = _connectionFactory.CreateConnection();
        const string sql = """
            SELECT
                a.id AS ID,
                a.name AS Name,
                a.price_amount AS Price,
                a.price_currency AS Currency,
                a.address_country AS Country,
                a.address_state as State,
                a.address_zip_code as ZipCode,
                a.address_city as City,
                a.address_street as Street,
            FROM apartments AS a
            WHERE NOT EXISTS
            (
                SELECT 1
                FROM bookings AS b
                WHERE
                    b.apartment_id = a.id AND
                    b.duration_start <= @EndDate AND
                    b.duration_end <= @StartDate AND
                    b.status = ANY(@ActiveBookingStatuses)
            )
            """;

        var apartments = await connection.QueryAsync<ApartmentResponse, AddressResponse, ApartmentResponse>(
            sql,
            (apartment, address) =>
            {
                apartment.Address = address;
                return apartment;
            },
            new
            {
                request.StartDate,
                request.EndDate,
                ActiveBookingStatuses
            },
            splitOn: "Country");


        return apartments.ToList();


    }
}
