import { type ClassValue, clsx } from "clsx";
import { twMerge } from "tailwind-merge";

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

interface ValidationResult {
  isValid: boolean;
  error?: string;
  line?: number;
  column?: number;
  position?: number;
}

export function validateJson(jsonString: string): ValidationResult {
  if (!jsonString.trim()) {
    return {
      isValid: false,
      error: '输入不能为空'
    };
  }

  try {
    // 基本的JSON解析验证
    JSON.parse(jsonString);

    // 额外的格式检查
    const lines = jsonString.split('\n');
    let openBraces = 0;
    let openBrackets = 0;
    let inString = false;
    let escapeNext = false;

    for (let lineNum = 0; lineNum < lines.length; lineNum++) {
      const line = lines[lineNum];
      
      for (let colNum = 0; colNum < line.length; colNum++) {
        const char = line[colNum];
        
        if (escapeNext) {
          escapeNext = false;
          continue;
        }

        if (char === '\\' && !escapeNext) {
          escapeNext = true;
          continue;
        }

        if (char === '"' && !escapeNext) {
          inString = !inString;
          continue;
        }

        if (!inString) {
          switch (char) {
            case '{':
              openBraces++;
              break;
            case '}':
              openBraces--;
              if (openBraces < 0) {
                return {
                  isValid: false,
                  error: '多余的右花括号 }',
                  line: lineNum + 1,
                  column: colNum + 1
                };
              }
              break;
            case '[':
              openBrackets++;
              break;
            case ']':
              openBrackets--;
              if (openBrackets < 0) {
                return {
                  isValid: false,
                  error: '多余的右方括号 ]',
                  line: lineNum + 1,
                  column: colNum + 1
                };
              }
              break;
          }
        }
      }
    }

    if (openBraces > 0) {
      return {
        isValid: false,
        error: `缺少 ${openBraces} 个右花括号 }`
      };
    }

    if (openBraces < 0) {
      return {
        isValid: false,
        error: `多余 ${Math.abs(openBraces)} 个右花括号 }`
      };
    }

    if (openBrackets > 0) {
      return {
        isValid: false,
        error: `缺少 ${openBrackets} 个右方括号 ]`
      };
    }

    if (openBrackets < 0) {
      return {
        isValid: false,
        error: `多余 ${Math.abs(openBrackets)} 个右方括号 ]`
      };
    }

    // 检查是否有尾随逗号
    if (/,\s*[}\]]/.test(jsonString)) {
      return {
        isValid: false,
        error: '存在尾随逗号'
      };
    }

    // 检查属性名是否使用双引号
    if (/{\s*\w+\s*:/.test(jsonString)) {
      return {
        isValid: false,
        error: '属性名必须使用双引号'
      };
    }

    return {
      isValid: true
    };
  } catch (e) {
    if (e instanceof SyntaxError) {
      // 尝试提取行号和列号信息
      const match = e.message.match(/position\s+(\d+)/);
      const position = match ? parseInt(match[1]) : undefined;
      
      // 计算行号和列号
      if (position !== undefined) {
        const beforeError = jsonString.substring(0, position);
        const lines = beforeError.split('\n');
        const line = lines.length;
        const column = lines[lines.length - 1].length + 1;
        
        return {
          isValid: false,
          error: '语法错误: ' + e.message,
          line,
          column,
          position
        };
      }
    }
    
    return {
      isValid: false,
      error: '无效的 JSON 格式'
    };
  }
}