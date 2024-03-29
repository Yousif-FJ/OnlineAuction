﻿namespace AuctionBackend.Api.RemoteSchema.V1.Auction
{
    public record BidRemote(double Value, string Username, DateTime DateTime);
    public record AddBidRequest(double Value, int AuctionId);
}
