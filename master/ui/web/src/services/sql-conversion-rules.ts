import { ConversionRule } from './types';

// SQL Server 到 MySQL 的转换规则
export const sqlServerToMySqlRules: ConversionRule[] = [
  // 已有规则保持不变...
];

// MySQL 到 SQL Server 的转换规则
export const mySqlToSqlServerRules: ConversionRule[] = [
  // 已有规则保持不变...
];

// Oracle 到 MySQL 的转换规则
export const oracleToMySqlRules: ConversionRule[] = [
  { pattern: /--/g, replacement: '#' },
  { pattern: /ROWNUM\s*<=\s*(\d+)/gi, replacement: 'LIMIT $1' },
  { pattern: /SYSDATE/gi, replacement: 'NOW()' },
  { pattern: /NVL\((.*?),\s*(.*?)\)/gi, replacement: 'IFNULL($1, $2)' },
  { pattern: /\"([^\"]+)\"/g, replacement: '`$1`' },
  { pattern: /NUMBER(\(\d+,\s*\d+\))?/gi, replacement: 'DECIMAL$1' },
  { pattern: /VARCHAR2/gi, replacement: 'VARCHAR' },
  { pattern: /NVARCHAR2/gi, replacement: 'VARCHAR' },
  { pattern: /CLOB/gi, replacement: 'LONGTEXT' },
  { pattern: /BLOB/gi, replacement: 'LONGBLOB' },
  { pattern: /TO_DATE\((.*?),\s*'([^']+)'\)/gi, replacement: 'STR_TO_DATE($1, $2)' },
  { pattern: /TO_CHAR\((.*?),\s*'([^']+)'\)/gi, replacement: 'DATE_FORMAT($1, $2)' },
  { pattern: /SYSTIMESTAMP/gi, replacement: 'NOW(6)' },
  { pattern: /EMPTY_CLOB\(\)/gi, replacement: "''" },
  { pattern: /EMPTY_BLOB\(\)/gi, replacement: "''" },
];

// MySQL 到 Oracle 的转换规则
export const mySqlToOracleRules: ConversionRule[] = [
  { pattern: /#/g, replacement: '--' },
  { pattern: /LIMIT\s+(\d+)/gi, replacement: 'ROWNUM <= $1' },
  { pattern: /NOW\(\)/gi, replacement: 'SYSDATE' },
  { pattern: /IFNULL\((.*?),\s*(.*?)\)/gi, replacement: 'NVL($1, $2)' },
  { pattern: /`([^`]+)`/g, replacement: '"$1"' },
  { pattern: /DECIMAL(\(\d+,\s*\d+\))?/gi, replacement: 'NUMBER$1' },
  { pattern: /LONGTEXT/gi, replacement: 'CLOB' },
  { pattern: /LONGBLOB/gi, replacement: 'BLOB' },
  { pattern: /STR_TO_DATE\((.*?),\s*([^)]+)\)/gi, replacement: 'TO_DATE($1, $2)' },
  { pattern: /DATE_FORMAT\((.*?),\s*([^)]+)\)/gi, replacement: 'TO_CHAR($1, $2)' },
];

// PostgreSQL 到 MySQL 的转换规则
export const postgresqlToMySqlRules: ConversionRule[] = [
  { pattern: /--/g, replacement: '#' },
  { pattern: /NOW\(\)/gi, replacement: 'NOW()' }, // 相同的函数名
  { pattern: /COALESCE\((.*?)\)/gi, replacement: 'IFNULL($1)' },
  { pattern: /\"([^\"]+)\"/g, replacement: '`$1`' },
  { pattern: /SERIAL/gi, replacement: 'AUTO_INCREMENT' },
  { pattern: /BIGSERIAL/gi, replacement: 'BIGINT AUTO_INCREMENT' },
  { pattern: /TEXT/gi, replacement: 'LONGTEXT' },
  { pattern: /BYTEA/gi, replacement: 'LONGBLOB' },
  { pattern: /CURRENT_TIMESTAMP/gi, replacement: 'NOW()' },
  { pattern: /INTERVAL '(\d+) (\w+)'/gi, replacement: "INTERVAL '$1' $2" },
];

// MySQL 到 PostgreSQL 的转换规则
export const mySqlToPostgresqlRules: ConversionRule[] = [
  { pattern: /#/g, replacement: '--' },
  { pattern: /`([^`]+)`/g, replacement: '"$1"' },
  { pattern: /IFNULL\((.*?),\s*(.*?)\)/gi, replacement: 'COALESCE($1, $2)' },
  { pattern: /AUTO_INCREMENT/gi, replacement: 'SERIAL' },
  { pattern: /LONGTEXT/gi, replacement: 'TEXT' },
  { pattern: /LONGBLOB/gi, replacement: 'BYTEA' },
  { pattern: /TINYINT\(1\)/gi, replacement: 'BOOLEAN' },
  { pattern: /INT UNSIGNED/gi, replacement: 'INTEGER' },
  { pattern: /DATETIME/gi, replacement: 'TIMESTAMP' },
];

// Oracle 到 PostgreSQL 的转换规则
export const oracleToPostgresqlRules: ConversionRule[] = [
  { pattern: /SYSDATE/gi, replacement: 'CURRENT_TIMESTAMP' },
  { pattern: /NVL\((.*?),\s*(.*?)\)/gi, replacement: 'COALESCE($1, $2)' },
  { pattern: /NUMBER(\(\d+,\s*\d+\))?/gi, replacement: 'NUMERIC$1' },
  { pattern: /VARCHAR2/gi, replacement: 'VARCHAR' },
  { pattern: /NVARCHAR2/gi, replacement: 'VARCHAR' },
  { pattern: /CLOB/gi, replacement: 'TEXT' },
  { pattern: /BLOB/gi, replacement: 'BYTEA' },
  { pattern: /ROWNUM\s*<=\s*(\d+)/gi, replacement: 'LIMIT $1' },
  { pattern: /TO_DATE\((.*?),\s*'([^']+)'\)/gi, replacement: "TO_TIMESTAMP($1, '$2')" },
];

// PostgreSQL 到 Oracle 的转换规则
export const postgresqlToOracleRules: ConversionRule[] = [
  { pattern: /CURRENT_TIMESTAMP/gi, replacement: 'SYSDATE' },
  { pattern: /COALESCE\((.*?),\s*(.*?)\)/gi, replacement: 'NVL($1, $2)' },
  { pattern: /NUMERIC(\(\d+,\s*\d+\))?/gi, replacement: 'NUMBER$1' },
  { pattern: /TEXT/gi, replacement: 'CLOB' },
  { pattern: /BYTEA/gi, replacement: 'BLOB' },
  { pattern: /LIMIT\s+(\d+)/gi, replacement: 'ROWNUM <= $1' },
  { pattern: /BOOLEAN/gi, replacement: 'NUMBER(1)' },
  { pattern: /TRUE/gi, replacement: '1' },
  { pattern: /FALSE/gi, replacement: '0' },
];