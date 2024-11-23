﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CM.Domain.Seat;
using CM.Dtos.Seat;

namespace CM.ApplicationService.Seat.Abstracts
{
    public interface ISeatService
    {
        List<SeatDto> GetSeatsByRoomId(string roomId);
        void AddSeat(AddSeatDto seatDto);
        void UpdateSeat(UpdateSeatDto seatDto);
        void DeleteSeat(int seatId);
        bool IsDoubleSeatValid(int seatX, int seatY, int? doubleSeatId);
    }
}
