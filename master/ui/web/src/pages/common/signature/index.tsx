import { useCallback, useRef, useState, useEffect } from 'react';
import SignatureCanvas from 'react-signature-canvas';
import { Button } from '@/components/ui/button';
import { Slider } from '@/components/ui/slider';
import { Input } from '@/components/ui/input';
import { toast } from 'react-hot-toast';
import { Download, Eraser, Undo, Wand2, Shuffle } from 'lucide-react';

// 手写字体库
const SIGNATURE_FONTS = [
    { name: '草书', value: 'cursive' },
    { name: '楷书', value: '"KaiTi", "楷体", serif' },
    { name: '行书', value: '"STXingkai", "华文行楷", cursive' },
    { name: '娃娃体', value: '"Zpix", "Cubic11", monospace' },
    { name: '仿宋', value: '"FangSong", "仿宋", serif' },
    { name: '隶书', value: '"LiSu", "隶书", serif' },
    { name: '魏碑', value: '"WeiBei", "魏碑", serif' },
    { name: '书宋', value: '"ShiShangZhongSong", "时尚中宋", serif' },
    { name: '手札体', value: '"ShouZhaTi", "手札体", cursive' },
    { name: '钢笔体', value: '"GangBiTi", "钢笔体", cursive' },
    { name: '毛笔体', value: '"MaoBiTi", "毛笔体", cursive' },
    { name: '黑体', value: '"SimHei", "黑体", sans-serif' },
    { name: '圆体', value: '"YouYuan", "幼圆", sans-serif' },
    { name: '标楷体', value: '"BiauKai", "标楷体", serif' },
    { name: '宋体', value: '"SimSun", "宋体", serif' },
    { name: '新魏体', value: '"XinWei", "新魏", serif' },
    { name: '华文新魏', value: '"STXinwei", "华文新魏", serif' },
    { name: '华文琥珀', value: '"STHupo", "华文琥珀", serif' },
    { name: '华文隶书', value: '"STLiti", "华文隶书", serif' },
    { name: '方正舒体', value: '"FZShuTi", "方正舒体", cursive' },
    { name: '方正姚体', value: '"FZYaoti", "方正姚体", cursive' }
];

// 添加字体预加载
const GOOGLE_FONTS_URL = 'https://fonts.googleapis.com/css2?family=Ma+Shan+Zheng&family=Zhi+Mang+Xing&family=Liu+Jian+Mao+Cao&family=Long+Cang&family=ZCOOL+KuaiLe&family=ZCOOL+XiaoWei&family=ZCOOL+QingKe+HuangYou&family=Noto+Sans+SC&display=swap';

// 添加字体样式的接口定义
interface FontStyle {
    fontWeight: number;
    fontStretch: string;
    fontStyle: string;
    transform: string;
    fontVariationSettings: string;
    skewX: string; // 添加 skewX 属性
}

export default function SignaturePage() {
    const signatureRef = useRef<SignatureCanvas>(null);
    const [penColor, setPenColor] = useState('#000000');
    const [penWidth, setPenWidth] = useState([2]);
    const [name, setName] = useState('');
    const [selectedFont, setSelectedFont] = useState(SIGNATURE_FONTS[0].value);
    const [rotation, setRotation] = useState(0);

    // 添加一个判断是否应该禁用控件的函数
    const shouldDisableControls = useCallback(() => {
        return !name.trim() && (!signatureRef.current || signatureRef.current.isEmpty());
    }, [name]);

    // 在组件中添加状态跟踪
    const [isControlsDisabled, setIsControlsDisabled] = useState(true);

    // 预加载字体
    useEffect(() => {
        const link = document.createElement('link');
        link.href = GOOGLE_FONTS_URL;
        link.rel = 'stylesheet';
        document.head.appendChild(link);
        return () => {
            document.head.removeChild(link);
        };
    }, []);

    // 在useEffect中监听变化
    useEffect(() => {
        setIsControlsDisabled(shouldDisableControls());
    }, [name, shouldDisableControls]);

    const handleClear = useCallback(() => {
        signatureRef.current?.clear();
    }, []);

    const handleUndo = useCallback(() => {
        const data = signatureRef.current?.toData();
        if (data && data.length > 0) {
            signatureRef.current?.fromData(data.slice(0, -1));
        }
    }, []);

    const handleDownload = useCallback(() => {
        if (signatureRef.current?.isEmpty() && !name.trim()) {
            toast.error('请先签名');
            return;
        }

        const canvas = signatureRef.current?.getCanvas();
        if (!canvas) return;

        // 创建一个新的canvas来处理背景
        const exportCanvas = document.createElement('canvas');
        exportCanvas.width = canvas.width;
        exportCanvas.height = canvas.height;
        
        const ctx = exportCanvas.getContext('2d');
        if (!ctx) return;

        // 填充白色背景
        ctx.fillStyle = 'white';
        ctx.fillRect(0, 0, canvas.width, canvas.height);
        
        // 将原始签名绘制到新canvas上
        ctx.drawImage(canvas, 0, 0);

        // 使用新canvas导出图片
        const dataUrl = exportCanvas.toDataURL('image/png');
        const link = document.createElement('a');
        const filename = name.trim() || 'signature';
        link.download = `${filename}-${new Date().getTime()}.png`;
        link.href = dataUrl;
        link.click();
        toast.success('签名已下载');
    }, [name]);

    // 随机生成签名样式
    const generateRandomStyle = useCallback(() => {
        // 随机选择字体
        const randomFont = SIGNATURE_FONTS[Math.floor(Math.random() * SIGNATURE_FONTS.length)].value;
        setSelectedFont(randomFont);

        // 随机旋转角度 (-15到15度)
        const randomRotation = Math.random() * 30 - 15;
        setRotation(randomRotation);

        // 固定为黑色
        const color = '#000000';
        setPenColor(color);

        // 设置较细的笔画宽度 (1-2之间)
        const width = 1 + Math.random();  // 生成1到2之间的随机数
        setPenWidth([width]);

        generateSignature(randomFont, randomRotation, color, width);
    }, [name]);

    // 使用名字的特征来生成一致的随机字体效果
    const generateFontStyle = (name: string): FontStyle => {
        // 使用名字生成一个稳定的哈希值
        const hashCode = name.split('')
            .reduce((acc, char) => acc + char.charCodeAt(0), 0);

        // 基于哈希值生成字体特征
        const fontWeight = 400 + (hashCode % 400); // 400-800
        const fontStretch = 85 + (hashCode % 30); // 85%-115%
        const fontStyle = hashCode % 2 === 0 ? 'normal' : 'italic';
        const skewX = (-5 + (hashCode % 10)).toString(); // -5 to 5 degrees

        return {
            fontWeight,
            fontStretch: `${fontStretch}%`,
            fontStyle,
            transform: `skewX(${skewX}deg)`,
            fontVariationSettings: `'wght' ${fontWeight}, 'wdth' ${fontStretch}`,
            skewX: `${skewX}deg` // 添加 skewX 属性
        };
    };

    const generateSignature = useCallback((
        font = selectedFont,
        angle = rotation,
        color = penColor,
        width = penWidth[0],
        skipNameCheck = false  // 新增参数
    ) => {
        if (!skipNameCheck && !name.trim()) {
            toast.error('请输入姓名');
            return;
        }

        const canvas = signatureRef.current?.getCanvas();
        if (!canvas) return;

        const ctx = canvas.getContext('2d');
        if (!ctx) return;

        // 清除画布
        signatureRef.current?.clear();

        // 获取基于名字的字体样式
        const fontStyle = generateFontStyle(name);

        // 计算合适的字体大小
        const fontSize = Math.min(canvas.height / 2, canvas.width / (name.length * 1.2));

        // 保存当前上下文状态
        ctx.save();

        // 移动到画布中心并旋转
        ctx.translate(canvas.width / 2, canvas.height / 2);
        ctx.rotate(angle * Math.PI / 180);

        // 设置字体样式和线条宽度
        ctx.font = `${fontStyle.fontStyle} ${fontStyle.fontWeight} ${fontSize}px ${font}`;
        ctx.fillStyle = color;
        ctx.lineWidth = width; // 设置线条宽度
        ctx.textBaseline = 'middle';
        ctx.textAlign = 'center';

        // 应用倾斜变换
        const skewAngle = parseFloat(fontStyle.skewX);
        ctx.transform(1, 0, Math.tan(skewAngle * Math.PI / 180), 1, 0, 0);

        // 绘制文字（使用 strokeText 来应用线条宽度）
        ctx.strokeText(name, 0, 0);
        ctx.fillText(name, 0, 0);
        // 恢复上下文状态
        ctx.restore();
    }, [name, rotation, penColor, penWidth, selectedFont]);

    // 在SignatureCanvas的onChange事件中更新状态
    const handleCanvasChange = useCallback(() => {
        setIsControlsDisabled(shouldDisableControls());
    }, [shouldDisableControls]);

    return (
        <div className="container mx-auto p-4 md:p-6 min-h-screen">
            <div className="space-y-6">
                <div className="flex flex-col space-y-2">
                    <h1 className="text-3xl font-bold tracking-tight">电子签名工具</h1>
                    <p className="text-muted-foreground">
                        在下方空白处绘制您的签名，或输入姓名自动生成签名
                    </p>
                </div>
                {/* 工具栏 */}
                <div className="flex flex-wrap gap-4 items-center">
                    {/* 姓名输入和生成按钮 */}
                    <div className="flex items-center gap-2">
                        <Input
                            type="text"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                            placeholder="输入姓名"
                            className="w-32"
                        />
                        <select
                            value={selectedFont}
                            onChange={(e) => {
                                setSelectedFont(e.target.value);
                                generateSignature(e.target.value, rotation, penColor, penWidth[0]);
                            }}
                            className="h-9 rounded-md border border-input bg-transparent px-3 py-1 text-sm shadow-sm transition-colors hover:bg-accent hover:text-accent-foreground"
                        >
                            {SIGNATURE_FONTS.map(font => (
                                <option key={font.value} value={font.value}>
                                    {font.name}
                                </option>
                            ))}
                        </select>
                        <Button
                            variant="outline"
                            size="sm"
                            onClick={() => generateSignature(selectedFont, rotation, penColor, penWidth[0])}
                        >
                            <Wand2 className="h-4 w-4 mr-2" />
                            生成
                        </Button>
                        <Button
                            variant="outline"
                            size="sm"
                            onClick={generateRandomStyle}
                        >
                            <Shuffle className="h-4 w-4 mr-2" />
                            随机样式
                        </Button>
                    </div>

                    {/* 颜色选择器 */}
                    <div className="flex items-center gap-2">
                        <input
                            type="color"
                            value={penColor}
                            onChange={(e) => {
                                setPenColor(e.target.value);
                                if (!signatureRef.current?.isEmpty()) {
                                    // 如果画布不为空，直接应用新的颜色
                                    signatureRef.current?.fromData(signatureRef.current.toData());
                                } else if (name.trim()) {
                                    // 如果有姓名，则生成签名
                                    generateSignature(selectedFont, rotation, e.target.value, penWidth[0], true);
                                }
                            }}
                            className="w-8 h-8 rounded-full overflow-hidden cursor-pointer border border-input hover:border-ring transition-colors [&::-webkit-color-swatch-wrapper]:p-0 [&::-webkit-color-swatch]:border-none"
                            title="选择颜色"
                        />
                        <span className="text-sm">笔画颜色</span>
                    </div>

                    {/* 原有的笔画粗细控制 */}
                    <div className="flex items-center gap-4 min-w-[200px]">
                        <span className="text-sm">笔画粗细:</span>
                        <Slider
                            value={penWidth}
                            onValueChange={(value) => {
                                setPenWidth(value);
                                if (!signatureRef.current?.isEmpty()) {
                                    // 如果画布不为空，直接应用新的笔画宽度
                                    signatureRef.current?.fromData(signatureRef.current.toData());
                                } else if (name.trim()) {
                                    // 如果有姓名，则生成签名
                                    generateSignature(selectedFont, rotation, penColor, value[0], true);
                                }
                            }}
                            min={1}
                            max={10}
                            step={0.5}
                            disabled={isControlsDisabled}
                            className={isControlsDisabled ? 'opacity-50 cursor-not-allowed' : ''}
                        />
                    </div>

                    {/* 旋转角度控制 */}
                    <div className="flex items-center gap-4 min-w-[200px]">
                        <span className="text-sm">旋转角度:</span>
                        <Slider
                            value={[rotation]}
                            onValueChange={(value) => {
                                setRotation(value[0]);
                                if (name.trim()) {
                                    // 只有在有姓名时才生成签名
                                    generateSignature(selectedFont, value[0], penColor, penWidth[0], true);
                                }
                            }}
                            min={-30}
                            max={30}
                            step={1}
                            disabled={isControlsDisabled}
                            className={isControlsDisabled ? 'opacity-50 cursor-not-allowed' : ''}
                        />
                    </div>

                    {/* 原有的操作按钮 */}
                    <div className="flex gap-2">
                        <Button
                            variant="outline"
                            size="sm"
                            onClick={handleUndo}
                        >
                            <Undo className="h-4 w-4 mr-2" />
                            撤销
                        </Button>
                        <Button
                            variant="outline"
                            size="sm"
                            onClick={handleClear}
                        >
                            <Eraser className="h-4 w-4 mr-2" />
                            清除
                        </Button>
                        <Button
                            size="sm"
                            onClick={handleDownload}
                        >
                            <Download className="h-4 w-4 mr-2" />
                            下载
                        </Button>
                    </div>
                </div>

                {/* 签名板 */}
                <div className="border rounded-lg bg-white">
                    <SignatureCanvas
                        ref={signatureRef}
                        penColor={penColor}
                        dotSize={penWidth[0]}
                        minWidth={penWidth[0]}
                        maxWidth={penWidth[0]}
                        canvasProps={{
                            className: 'w-full h-[400px]',
                            style: {
                                touchAction: 'none',
                                backgroundColor: 'white', // 添加白色背景
                            }
                        }}
                        backgroundColor="rgb(255, 255, 255)" // 添加这一行
                        onEnd={handleCanvasChange}
                    />
                </div>

                <p className="text-sm text-muted-foreground text-center">
                    提示：可以使用鼠标或触控笔在上方区域绘制签名，或输入姓名自动生成签名
                </p>
            </div>
        </div>
    );
}

