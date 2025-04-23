import { useState, useCallback } from 'react';
import { toast } from 'react-hot-toast';
import { Copy, Trash2, Wand2, FileText } from 'lucide-react';
import { Button } from '@/components/ui/button';

// 未格式化的示例 JSON 字符串
const EXAMPLE_JSON = '{"name":"示例项目","version":"1.0.0","description":"这是一个示例 JSON","main":"index.js","scripts":{"start":"node index.js","test":"jest","build":"webpack"},"dependencies":{"react":"^18.2.0","react-dom":"^18.2.0","typescript":"^5.0.0"},"author":{"name":"开发者","email":"developer@example.com"},"repository":{"type":"git","url":"https://github.com/example/project"},"license":"MIT","keywords":["example","demo","sample"],"config":{"port":3000,"debug":true,"api":{"endpoint":"https://api.example.com","timeout":5000}}}';

export default function JsonFormatPage() {
    const [input, setInput] = useState('');
    const [output, setOutput] = useState('');
    const [error, setError] = useState<string | null>(null);

    const formatJson = useCallback(() => {
        if (!input.trim()) {
            setOutput('');
            setError(null);
            return;
        }

        try {
            const parsed = JSON.parse(input);
            const formatted = JSON.stringify(parsed, null, 2);
            setOutput(formatted);
            setError(null);
            toast.success('格式化成功');
        } catch (e) {
            console.error(e);
            setError('无效的 JSON 格式');
            toast.error('无效的 JSON 格式');
        }
    }, [input]);

    const handleCopy = useCallback(() => {
        if (!output) return;
        navigator.clipboard.writeText(output)
            .then(() => toast.success('已复制到剪贴板'))
            .catch(() => toast.error('复制失败'));
    }, [output]);

    const handleClear = useCallback(() => {
        setInput('');
        setOutput('');
        setError(null);
        toast.success('已清空');
    }, []);

    const loadExample = useCallback(() => {
        setInput(EXAMPLE_JSON);
        setError(null);
        toast.success('已加载示例');
    }, []);

    return (
        <div className="container mx-auto p-4 md:p-6 min-h-screen">
            <div className="space-y-6">
                <div className="flex flex-col space-y-2">
                    <h1 className="text-3xl font-bold tracking-tight">JSON 格式化工具</h1>
                    <p className="text-muted-foreground">
                        粘贴 JSON 文本，一键格式化并优化缩进
                    </p>
                </div>
                
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                    {/* 输入区域 */}
                    <div className="space-y-3">
                        <div className="flex items-center justify-between">
                            <label className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70">
                                输入 JSON
                            </label>
                            <div className="flex items-center gap-2">
                                <Button
                                    variant="ghost"
                                    size="sm"
                                    onClick={loadExample}
                                >
                                    <FileText className="h-4 w-4 mr-2" />
                                    加载示例
                                </Button>
                                {error && (
                                    <span className="text-sm text-destructive">
                                        {error}
                                    </span>
                                )}
                            </div>
                        </div>
                        <div className="relative">
                            <textarea
                                className={`w-full h-[70vh] p-4 font-mono text-sm rounded-lg 
                                    border focus:outline-none focus:ring-2 
                                    bg-card text-card-foreground
                                    border-input focus:ring-ring/20
                                    placeholder:text-muted-foreground
                                    ${error ? 'border-destructive focus:ring-destructive/20' : ''}`}
                                value={input}
                                onChange={(e) => setInput(e.target.value)}
                                placeholder="在此粘贴需要格式化的 JSON，或点击‘加载示例’按钮查看示例..."
                                spellCheck={false}
                            />
                        </div>
                    </div>

                    {/* 输出区域 */}
                    <div className="space-y-3">
                        <div className="flex items-center justify-between">
                            <label className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70">
                                格式化结果
                            </label>
                            {output && (
                                <Button
                                    variant="ghost"
                                    size="sm"
                                    onClick={handleCopy}
                                >
                                    <Copy className="h-4 w-4 mr-2" />
                                    复制
                                </Button>
                            )}
                        </div>
                        <textarea
                            readOnly
                            className="w-full h-[70vh] p-4 font-mono text-sm rounded-lg 
                                border focus:outline-none
                                bg-muted/50 text-foreground
                                border-input"
                            value={output}
                            placeholder="格式化后的 JSON 将显示在这里..."
                            spellCheck={false}
                        />
                    </div>
                </div>

                {/* 操作按钮 */}
                <div className="flex gap-3">
                    <Button
                        onClick={formatJson}
                        className="gap-2"
                    >
                        <Wand2 className="h-4 w-4" />
                        格式化
                    </Button>
                    <Button
                        variant="outline"
                        onClick={handleClear}
                        className="gap-2"
                    >
                        <Trash2 className="h-4 w-4" />
                        清空
                    </Button>
                </div>
            </div>
        </div>
    );
}