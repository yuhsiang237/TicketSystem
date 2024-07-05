# TicketSystem
TicketSystem，簡易訂票系統實作。  
該服務提供三支API，並且實作簡易訂票系統，並以C# lock鎖避免race condition(競爭條件)，確保大量訂票不會遇到資料重複存取問題。  
並且於測試案例中採用了80000人同時非同步購買同一張票的情況，結果為正常訂票。
(https://github.com/yuhsiang237/TicketSystem/blob/master/TicketSystemTest/Services/TicketServiceTest.cs)

<b>關鍵字:</b> Race condition、系統架構分層(邏輯層/資料層)、單元測試

```
POST
​/Ticket​/OrderTicket
購買票
req:
{
  "passengerName": "string",
  "ticketNumber": "string"
}
rsp:
{
  "ticketNumber": A-1,
  "dateTime": "2024-07-05T19:45:38.759933+08:00",
  "isOK": true
}
```
```
POST
​/Ticket​/GetUnPurchasedTicket
取得未購買的票
req:無
rsp:
[
  {
    "ticketNumber": "T-1",
  },
  {
    "ticketNumber": "T-2",
  },
  {
    "ticketNumber": "T-3",
  }...
]
```
```
POST
​/Ticket​/GetAllTicket
取得所有的票
req:無
rsp:
[
  {
    "ticketNumber": "T-1",
    "isPurchase": false
  },
  {
    "ticketNumber": "T-2",
    "isPurchase": false
  },
  {
    "ticketNumber": "T-3",
    "isPurchase": false
  }...
]
```
