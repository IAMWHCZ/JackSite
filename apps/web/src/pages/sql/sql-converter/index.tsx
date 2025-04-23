import { useState, useCallback } from 'react';
import { Button } from '@/components/ui/button';
import { Textarea } from '@/components/ui/textarea';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import toast from 'react-hot-toast';
import { SqlConverter, type SqlType } from '@/services/sql-converter';

const EXAMPLE_SQL = `SELECT TOP 10 
    [OrderId],
    [CustomerName],
    CONVERT(VARCHAR(20), OrderDate, 120) AS OrderDate,
    ISNULL([Description], 'No Description') AS Description,
    DATEADD(day, 7, OrderDate) AS DueDate,
    DATEDIFF(day, OrderDate, GETDATE()) AS DaysElapsed
FROM [Sales].[Orders] WITH(NOLOCK)
WHERE OrderDate >= DATEADD(month, -3, GETDATE())
ORDER BY OrderDate DESC`;

const SqlConverterPage = () => {
  const [sourceSQL, setSourceSQL] = useState(EXAMPLE_SQL);
  const [targetSQL, setTargetSQL] = useState('');
  const [sourceType, setSourceType] = useState<SqlType>('sqlserver');
  const [targetType, setTargetType] = useState<SqlType>('mysql');

  const convertSQL = useCallback(() => {
    if (!sourceSQL.trim()) {
      toast.error('请输入需要转换的SQL');
      return;
    }

    try {
      const converted = SqlConverter.convert(sourceSQL, sourceType, targetType);
      setTargetSQL(converted);
      toast.success('转换完成');
    } catch (error) {
      console.error('SQL转换错误:', error);
      toast.error('SQL转换失败，请检查语法');
    }
  }, [sourceSQL, sourceType, targetType]);

  const handleCopy = useCallback(() => {
    if (targetSQL) {
      void (async () => {
        try {
          await navigator.clipboard.writeText(targetSQL);
          toast.success('已复制到剪贴板');
        } catch {
          toast.error('复制失败');
        }
      })();
    }
  }, [targetSQL]);

  return (
    <div className="container mx-auto p-4 space-y-4">
      <div className="flex items-center space-x-4">
        <Select value={sourceType} onValueChange={(e) => setSourceType(e as SqlType)}>
          <SelectTrigger className="w-[180px]">
            <SelectValue placeholder="选择源数据库类型" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="sqlserver">SQL Server</SelectItem>
            <SelectItem value="mysql">MySQL</SelectItem>
            <SelectItem value="oracle">Oracle</SelectItem>
            <SelectItem value="postgresql">PostgreSQL</SelectItem>
          </SelectContent>
        </Select>

        <span>转换到</span>

        <Select value={targetType} onValueChange={(e) => setTargetType(e as SqlType)}>
          <SelectTrigger className="w-[180px]">
            <SelectValue placeholder="选择目标数据库类型" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="mysql">MySQL</SelectItem>
            <SelectItem value="sqlserver">SQL Server</SelectItem>
            <SelectItem value="oracle">Oracle</SelectItem>
            <SelectItem value="postgresql">PostgreSQL</SelectItem>
          </SelectContent>
        </Select>
      </div>

      <div className="grid grid-cols-2 gap-4">
        <div className="space-y-2">
          <label className="text-sm font-medium">源SQL:</label>
          <Textarea
            value={sourceSQL}
            onChange={(e) => setSourceSQL(e.target.value)}
            placeholder="请输入需要转换的SQL语句"
            className="h-[400px] font-mono"
          />
        </div>

        <div className="space-y-2">
          <label className="text-sm font-medium">转换结果:</label>
          <Textarea
            value={targetSQL}
            readOnly
            className="h-[400px] font-mono bg-muted"
          />
        </div>
      </div>

      <div className="flex justify-center space-x-4">
        <Button onClick={convertSQL}>转换</Button>
        <Button
          variant="outline"
          onClick={handleCopy}
        >
          复制结果
        </Button>
      </div>
    </div>
  );
};

export default SqlConverterPage;