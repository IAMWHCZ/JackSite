import { SqlType, ConversionRule } from './types';
import {
  sqlServerToMySqlRules,
  mySqlToSqlServerRules,
  oracleToMySqlRules,
  mySqlToOracleRules,
  postgresqlToMySqlRules,
  mySqlToPostgresqlRules,
  oracleToPostgresqlRules,
  postgresqlToOracleRules,
} from './sql-conversion-rules';

class SqlConverter {
  private static conversionMap = new Map<string, ConversionRule[]>([
    ['sqlserver-mysql', sqlServerToMySqlRules],
    ['mysql-sqlserver', mySqlToSqlServerRules],
    ['oracle-mysql', oracleToMySqlRules],
    ['mysql-oracle', mySqlToOracleRules],
    ['postgresql-mysql', postgresqlToMySqlRules],
    ['mysql-postgresql', mySqlToPostgresqlRules],
    ['oracle-postgresql', oracleToPostgresqlRules],
    ['postgresql-oracle', postgresqlToOracleRules],
  ]);

  private static applyRules(sql: string, rules: ConversionRule[]): string {
    return rules.reduce((result, rule) => {
      return result.replace(rule.pattern, rule.replacement as string);
    }, sql);
  }

  private static getConversionPath(sourceType: SqlType, targetType: SqlType): string[] {
    if (sourceType === targetType) {
      return [];
    }

    // 如果有直接转换规则
    const directPath = `${sourceType}-${targetType}`;
    if (this.conversionMap.has(directPath)) {
      return [directPath];
    }

    // 通过 MySQL 作为中间转换
    if (sourceType !== 'mysql' && targetType !== 'mysql') {
      return [`${sourceType}-mysql`, `mysql-${targetType}`];
    }

    throw new Error(`Unsupported conversion: ${sourceType} to ${targetType}`);
  }

  static convert(sql: string, sourceType: SqlType, targetType: SqlType): string {
    if (sourceType === targetType) {
      return sql;
    }

    try {
      const conversionPath = this.getConversionPath(sourceType, targetType);
      return conversionPath.reduce((currentSql, conversion) => {
        const rules = this.conversionMap.get(conversion);
        if (!rules) {
          throw new Error(`No conversion rules found for ${conversion}`);
        }
        return this.applyRules(currentSql, rules);
      }, sql);
    } catch (error) {
      console.error('SQL conversion error:', error);
      throw new Error(`Failed to convert from ${sourceType} to ${targetType}: ${error!.message}`);
    }
  }
}

export { SqlConverter };
