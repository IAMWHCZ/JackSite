// 声明全局字符串接口扩展
declare global {
  interface String {
    /**
     * 将字符串首字母大写
     */
    capitalize(): string;
    
    /**
     * 将字符串转换为驼峰命名
     */
    toCamelCase(): string;
    
    /**
     * 将字符串转换为短横线命名
     */
    toKebabCase(): string;
    
    /**
     * 截断字符串并添加省略号
     */
    truncate(length: number, suffix?: string): string;
    
    /**
     * 检查字符串是否为有效邮箱
     */
    isValidEmail(): boolean;
    
    /**
     * 移除字符串中的HTML标签
     */
    stripHtml(): string;
    
    /**
     * 反转字符串
     */
    reverse(): string;

    /**
     * 检查字符串是否为有效邮箱格式
     */
    isEmail(): boolean;
  }
}

// 实现扩展方法
String.prototype.capitalize = function (): string {
  return this.charAt(0).toUpperCase() + this.slice(1).toLowerCase();
};

String.prototype.toCamelCase = function (): string {
  return this.replace(/[-_\s]+(.)?/g, (_, char) => (char ? char.toUpperCase() : ''));
};

String.prototype.toKebabCase = function (): string {
  return this.replace(/([a-z])([A-Z])/g, '$1-$2')
    .replace(/[\s_]+/g, '-')
    .toLowerCase();
};

String.prototype.truncate = function (length: number, suffix: string = '...'): string {
  if (this.length <= length) return this.toString();
  return this.slice(0, length) + suffix;
};

String.prototype.isValidEmail = function (): boolean {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(this.toString());
};

String.prototype.stripHtml = function (): string {
  return this.replace(/<[^>]*>/g, '');
};

String.prototype.reverse = function (): string {
  return this.split('').reverse().join('');
};

String.prototype.isEmail = function (): boolean {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; 
  return emailRegex.test(this.toString());
};

// 导出空对象以使此文件成为模块
export {};