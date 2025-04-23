export type SqlType = 'mysql' | 'sqlserver' | 'oracle' | 'postgresql';

export interface ConversionRule {
  pattern: RegExp;
  replacement: string | ((match: string, ...args: string[]) => string);
}