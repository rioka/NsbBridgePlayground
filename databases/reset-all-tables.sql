-- USE [NsbBridgePlayground.Sender]
-- USE [NsbBridgePlayground.Notifier]
-- USE [NsbBridgePlayground.OrderProcessor]
-- USE [NsbBridgePlayground.Shipping]

declare cur_delete cursor for
select schema_NAME(schema_id), name
from sys.tables
where type = 'u';

declare
  @stmt   nvarchar(1000),
  @schema nvarchar(100),
  @table  nvarchar(100),
  @dryRun bit = 1

OPEN cur_delete;
FETCH NEXT FROM cur_delete
  INTO @schema, @table;

--SET @dryRun bit = 0 ;

WHILE @@FETCH_STATUS = 0
BEGIN
  SET @stmt = 'DELETE FROM ['+ @schema + '].[' + @table + ']';
  PRINT @stmt;
  IF @dryRun = 0
  BEGIN
    EXECUTE sp_executesql @stmt;
  END

  FETCH NEXT FROM cur_delete INTO @schema, @table;
END

CLOSE cur_delete;
DEALLOCATE cur_delete; 