import domtoimage from 'dom-to-image';
import jsPDF from 'jspdf';
import { toast } from 'react-hot-toast';
import { ExtendedWindow } from '../../type';

interface ExportButtonProps {
    elementId: string;
    fileName?: string;
}

declare const window: ExtendedWindow;

export default function ExportButton({ elementId, fileName = 'diagram' }: ExportButtonProps) {
  const handleExport = async () => {
    const element = document.getElementById(elementId);
    if (!element) return;

    try {
      const button = document.getElementById('export-button');
      if (button) button.textContent = 'Exporting...';

      const flowInstance = window.flowInstance;
      if (flowInstance) {
        flowInstance.fitView({ padding: 0.2 });
      }

      // 使用更高的缩放比例提高清晰度
      const scale = 4;
      const width = element.clientWidth * scale;
      const height = element.clientHeight * scale;

      // 过滤掉控制元素
      const filter = (node: Node): boolean => {
        if (!(node instanceof Element)) return true;
        const excludeClasses = [
          'react-flow__minimap',
          'react-flow__controls',
          'react-flow__attribution',
          'react-flow__panel',
        ];
        return !excludeClasses.some(className => node.classList?.contains(className));
      };

      // 导出设置
      const blob = await domtoimage.toBlob(element, {
        filter,
        bgcolor: '#ffffff', // 确保使用白色背景
        height: height,
        width: width,
        style: {
          transform: `scale(${scale})`,
          transformOrigin: 'top left',
          width: `${element.clientWidth}px`,
          height: `${element.clientHeight}px`,
        },
        quality: 1,
      });

      const reader = new FileReader();
      reader.readAsDataURL(blob);
      reader.onloadend = () => {
        const base64data = reader.result as string;

        const pdf = new jsPDF({
          orientation: width > height ? 'landscape' : 'portrait',
          unit: 'px',
          format: [width, height],
        });

        pdf.addImage(
          base64data,
          'PNG',
          0,
          0,
          width,
          height
        );

        pdf.save(`${fileName}.pdf`);
      };
    } catch (error: unknown) {
      console.error('Export failed:', error);
      toast.error('导出失败');
    } finally {
      const button = document.getElementById('export-button');
      if (button) button.textContent = '导出 ER 关系图';
    }
  };

  return (
    <div className="relative group">
      <button
        id="export-button"
        onClick={() => handleExport}
        className="px-4 py-2 bg-green-600 hover:bg-green-700 text-white rounded-lg shadow-sm hover:shadow-md transition-all duration-200 flex items-center gap-2"
        aria-label="导出 ER 图为 PDF"
      >
        <svg
          className="w-5 h-5"
          fill="none"
          stroke="currentColor"
          viewBox="0 0 24 24"
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            strokeWidth={2}
            d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4"
          />
        </svg>
                导出 ER 关系图
      </button>
      <div
        className="absolute bottom-full left-1/2 -translate-x-1/2 mb-2 px-3 py-2 bg-gray-900 text-white text-sm rounded-lg opacity-0 invisible group-hover:opacity-100 group-hover:visible transition-all duration-200 whitespace-nowrap">
                将当前 ER 图导出为 PDF 文件，保留表间关系连线
      </div>
    </div>
  );
}
