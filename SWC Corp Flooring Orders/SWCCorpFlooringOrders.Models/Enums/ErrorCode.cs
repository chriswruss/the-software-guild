using SWCCorpFlooringOrders.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCCorpFlooringOrders.Models.Enums {
    // ErrorCodes are used in all response objects, and some other places
    public enum ErrorCode {
        CorruptFile,
        NotAFutureDate,
        NotAValidDate,
        OrderNumberDoesNotExist,
        OrderDateLength,
        OrderDateNotAllNumbers,
        OrderNumberNotInt,
        OrderNumberNotPositive,
        EmptyName,
        NotAValidAbbreviation,
        NameIsInvalid,
        CouldNotFindFile,
        DontSellToState,
        FileIsEmpty,
        ProductNotFound,
        FoundBadState,
        AreaTooSmall,
        InvalidState = 101,
        OrderCancelled,
        AreaNotDecimal,
        AreaNegative,
        NoError,
        DidntConfirmOrder,
        NotAnInt
    }
}
