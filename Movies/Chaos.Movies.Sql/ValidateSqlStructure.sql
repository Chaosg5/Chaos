use CMDB;

-- Title Unique Indexes (should not be any rows with null values)

select *
from INFORMATION_SCHEMA.TABLES
left join (
SELECT 
    IndexName = I.name, 
    TableName =
        QUOTENAME(SCHEMA_NAME(T.[schema_id])) + 
        N'.' + QUOTENAME(T.name), 
    IsPrimaryKey = I.is_primary_key
FROM sys.indexes AS I
INNER JOIN sys.tables AS T
    ON I.[object_id] = T.[object_id]
WHERE
    I.type_desc <> N'HEAP'
	and( I.name like '%UX%'
	or I.name like '%UK%')
) as x on x.IndexName like '%' + TABLE_NAME
where TABLE_NAME like '%Titles'
	or TABLE_NAME like '%ExternalLookup'

-- Invalid key or index names (should not be any rows at all)

SELECT
    IndexName = I.name, 
    TableName =
        QUOTENAME(SCHEMA_NAME(T.[schema_id])) + 
        N'.' + QUOTENAME(T.name), 
    IsPrimaryKey = I.is_primary_key
FROM sys.indexes AS I
INNER JOIN sys.tables AS T
    ON I.[object_id] = T.[object_id]
WHERE
    I.type_desc <> N'HEAP'
	and (I.name like '%[_]_'
	or I.name like '%1'
	)

-- Rating constraints (should not be null)

select *
from INFORMATION_SCHEMA.CHECK_CONSTRAINTS as s
right join INFORMATION_SCHEMA.COLUMNS as c on s.CONSTRAINT_NAME like '%' + c.TABLE_NAME + '%'
where COLUMN_NAME like '%Rating'