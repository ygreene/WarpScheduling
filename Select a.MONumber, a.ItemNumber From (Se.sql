Select a.MONumber, a.ItemNumber From (Select h.MONumber, h.MOCreatedDate, i.ItemNumber, l.ItemKey, f.COHeaderKey, f.COLineKey, f.ReleaseQty, f.IssuedQty,f.ReceiptIssuedQty, f.RequiredQty
From dbo.FS_MOHeader h inner join dbo.FS_MOLine l on h.MOHeaderKey=l.MOHeaderKey
Inner Join dbo.FS_Item i on l.ItemKey =i.ItemKey
Inner Join dbo.t_STI_FinMO f on f.FinishMONumber=h.MONumber
WHere h.MONumber Like '3-%' and l.MOLineStatus=3 and l.ItemOrderedQuantity=0) a 
Inner Join dbo.FS_COHeader c on a.COHeaderKey=c.COHeaderKey 
Inner Join dbo.FS_COLine cl on a.COLineKey=cl.COLineKey



Select h.BillToCustomerID,h.ShipToCustomerID, h.ShipToDeliveryLocationID, l.COLineNumber ,i.ItemNumber, l.COLineStatus, l.ItemOrderedQuantity ,sl.ShipToDeliveryLocationName, sl.ShipToDeliveryLocationCity From 
dbo.FS_COHeader h inner join dbo.FS_COLine l on h.COHeaderKey=l.COHeaderKey 
inner join dbo.FS_Item i on i.ItemKey = l.ItemKey
Inner Join dbo.FS_ShipToDeliveryLocation sl on sl.ShipToDeliveryLocationKey =h.ShipToDeliveryLocationKey
Where l.COLineStatus=4 and i.ItemNumber Like '%-%-F3[5-7]'
Order by sl.ShipToDeliveryLocationCity

Select a.BillToCustomerID,Right(a.ItemNumber,3)as finish,Sum(a.ItemOrderedQuantity) From (Select h.BillToCustomerID,h.ShipToCustomerID, h.ShipToDeliveryLocationID, l.COLineNumber ,i.ItemNumber, l.COLineStatus, l.ItemOrderedQuantity ,sl.ShipToDeliveryLocationName, sl.ShipToDeliveryLocationCity From 
dbo.FS_COHeader h inner join dbo.FS_COLine l on h.COHeaderKey=l.COHeaderKey 
inner join dbo.FS_Item i on i.ItemKey = l.ItemKey
Inner Join dbo.FS_ShipToDeliveryLocation sl on sl.ShipToDeliveryLocationKey =h.ShipToDeliveryLocationKey
Where l.COLineStatus=4 and i.ItemNumber Like '%-%-F3[5-7]') a
Group by a.BillToCustomerID,Right(a.ItemNumber,3)

SELECT [TransactionDate]
      ,[TransactionFunctionCode]
      ,[MONumber]
       ,[ReceivingType]
      ,[ReceiptQuantity]
       ,[ItemOrderedQuantity]
      ,[ItemNumber]
       FROM [dbo].[FS_HistoryMOReceipt]
  Where ItemNumber Like 'WS%'
  and ReceivingType='R' and TransactionDate>'2020-10-13'
  Order by ItemNumber, MONumber