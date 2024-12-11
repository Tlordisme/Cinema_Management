﻿using CM.ApplicationService.Common;
using CM.ApplicationService.Seat.Abstracts;
using CM.Domain.Seat;
using CM.Dtos.Seat;
using CM.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.ApplicationService.Seat.Implements
{
    public class SeatPriceService : ServiceBase, ISeatPriceService
    {
        public SeatPriceService(CMDbContext dbContext, ILogger<ServiceBase> logger)
            : base(logger, dbContext)
        {
        }

        public void AddSeatPrice(AddSeatPriceDto seatPriceDto)
        {
            var existingPrice = _dbContext.SeatPrices
                .FirstOrDefault(sp => sp.RoomID == seatPriceDto.RoomID && sp.SeatType == seatPriceDto.SeatType &&
                                      (seatPriceDto.StartDate >= sp.StartDate && seatPriceDto.StartDate <= sp.EndDate ||
                                       seatPriceDto.EndDate >= sp.StartDate && seatPriceDto.EndDate <= sp.EndDate));

            if (existingPrice != null)
            {
                throw new Exception("Giá đã tồn tại cho ghế này trong thời gian áp dụng.");
            }

            var seatPrice = new CMSeatPrice
            {
                SeatType = seatPriceDto.SeatType,
                RoomID = seatPriceDto.RoomID,
                StartDate = seatPriceDto.StartDate,
                EndDate = seatPriceDto.EndDate,
                Price = seatPriceDto.Price
            };

            _dbContext.SeatPrices.Add(seatPrice);
            _dbContext.SaveChanges();
        }

        public void DeleteSeatPrice(int seatPriceId)
        {
            var seatPrice = _dbContext.SeatPrices.FirstOrDefault(sp => sp.Id == seatPriceId);
            if (seatPrice == null)
            {
                throw new Exception("Giá ghế không tồn tại.");
            }

            _dbContext.SeatPrices.Remove(seatPrice);
            _dbContext.SaveChanges();
        }

        public SeatPriceDto GetSeatPrice(int seatPriceId)
        {
            var seatPrice = _dbContext.SeatPrices.FirstOrDefault(sp => sp.Id == seatPriceId);
            if (seatPrice == null)
            {
                throw new Exception("Giá ghế không tồn tại.");
            }

            return new SeatPriceDto
            {
                Id = seatPrice.Id,
                SeatType = seatPrice.SeatType,
                RoomID = seatPrice.RoomID,
                StartDate = seatPrice.StartDate,
                EndDate = seatPrice.EndDate,
                Price = seatPrice.Price
            };
        }

        public List<SeatPriceDto> GetSeatPricesByRoomId(string roomId)
        {
            return _dbContext.SeatPrices
                .Where(sp => sp.RoomID == roomId)
                .Select(sp => new SeatPriceDto
                {
                    Id = sp.Id,
                    SeatType = sp.SeatType,
                    RoomID = sp.RoomID,
                    StartDate = sp.StartDate,
                    EndDate = sp.EndDate,
                    Price = sp.Price
                })
                .ToList();
        }

        public void UpdateSeatPrice(UpdateSeatPriceDto seatPriceDto)
        {
            var seatPrice = _dbContext.SeatPrices.FirstOrDefault(sp => sp.Id == seatPriceDto.Id);
            if (seatPrice == null)
            {
                throw new Exception("Giá ghế không tồn tại.");
            }

            seatPrice.SeatType = seatPriceDto.SeatType;
            seatPrice.RoomID = seatPriceDto.RoomID;
            seatPrice.StartDate = seatPriceDto.StartDate;
            seatPrice.EndDate = seatPriceDto.EndDate;
            seatPrice.Price = seatPriceDto.Price;

            _dbContext.SaveChanges();
        }
    }
}